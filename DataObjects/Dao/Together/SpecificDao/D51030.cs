using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
   public class D51030 : DataGate
   {
      public DataTable ListD50130()
      {
         string sql = @"
                     SELECT 
                       MMF_PARAM_KEY 
                      ,MMF_RESP_RATIO 
                      ,MMF_QNTY_LOW 
                      ,MMF_QUOTE_VALID_RATE 
                      ,' ' AS OP_TYPE 
                      ,MMF_AVG_TIME 
                      ,MMF_W_USER_ID 
                      ,MMF_W_TIME 
                      ,MMF_RFC_MIN_CNT 
                      ,MMF_MARKET_CODE 
                      ,MMF_END_TIME 
                      ,MMF_PROD_TYPE 
                      ,MMF_CP_KIND 
                      ,MMF_RESP_TIME 
                      ,MMF_QUOTE_DURATION
                   FROM CI.MMF
                   ORDER BY
                      MMF_MARKET_CODE,MMF_PROD_TYPE,SUBSTR(MMF_PARAM_KEY,3,1),MMF_PARAM_KEY
";
         DataTable dtResult = db.GetDataTable(sql, null);
         return dtResult;
      }//public DbDataAdapter ListD51030

      public void UpdateMMF(DataTable inputData)
      {
         /*string sql = @"
                        SELECT 
                         MMF_PARAM_KEY 
                        ,MMF_RESP_RATIO 
                        ,MMF_QNTY_LOW 
                        ,MMF_QUOTE_VALID_RATE 
                        ,MMF_AVG_TIME 
                        ,MMF_W_USER_ID 
                        ,MMF_W_TIME 
                        ,MMF_RFC_MIN_CNT 
                        ,MMF_MARKET_CODE 
                        ,MMF_END_TIME 
                        ,MMF_PROD_TYPE 
                        ,MMF_CP_KIND 
                        ,MMF_RESP_TIME 
                        ,MMF_QUOTE_DURATION
                     FROM CI.MMF";

         return db.UpdateOracleDB(inputData, sql);*/

         string tableName = "CI.MMF";
         string keysColumnList = "MMF_PARAM_KEY, MMF_MARKET_CODE";
         string insertColumnList = @"MMF_PARAM_KEY 
                        ,MMF_RESP_RATIO 
                        ,MMF_QNTY_LOW 
                        ,MMF_QUOTE_VALID_RATE 
                        ,MMF_AVG_TIME 
                        ,MMF_W_USER_ID 
                        ,MMF_W_TIME 
                        ,MMF_RFC_MIN_CNT 
                        ,MMF_MARKET_CODE 
                        ,MMF_END_TIME 
                        ,MMF_PROD_TYPE 
                        ,MMF_CP_KIND 
                        ,MMF_RESP_TIME 
                        ,MMF_QUOTE_DURATION";
         string updateColumnList = insertColumnList;
         try {
            //update to DB

            //1.在更新前先刪除指定的資料
            DeleteForChanged(inputData, tableName, keysColumnList);
            //2.DB刪除資料後變更DataTable去除Deleted標記
            inputData.AcceptChanges();
            //3.尋找所有的更改或新增的資料儲存
            SaveForAll(inputData, tableName, insertColumnList, updateColumnList, keysColumnList);
         }
         catch (Exception ex) {
            throw ex;
         }
      }

      /// <summary>
      /// update方法如果檢測到刪除欄位會出錯 在儲存前先刪除datatable刪除的資料 再配合SaveForAll更新整個datatable
      /// </summary>
      /// <param name="inputDT"></param>
      /// <param name="tableName"></param>
      /// <param name="updateOrDeleteKeysColumnList"></param>
      private void DeleteForChanged(DataTable inputDT, string tableName, string updateOrDeleteKeysColumnList)
      {

         int myReturnValue = 0;

         #region 參數轉換

         List<string> myUpdateOrDeleteKeysColumnList = new List<string>();

         foreach (string everyStr in updateOrDeleteKeysColumnList.Split(',')) {
            myUpdateOrDeleteKeysColumnList.Add(everyStr.Trim());
         }

         #endregion

         DbConnection conn = db.CreateConnection();

         try {
            DataTable dtDeleteChange = inputDT.GetChanges(DataRowState.Deleted);

            //沒有刪除就跳出function
            if (dtDeleteChange == null || dtDeleteChange.Rows.Count <= 0) {
               return;
            }

            foreach (DataRow deletedRow in dtDeleteChange.Rows) {

               string myDeletedKeyAndValueStr = "";

               #region params

               List<object> myParamsNameAndValue = new List<object>();

               foreach (string paramName in myUpdateOrDeleteKeysColumnList) {
                  myParamsNameAndValue.Add(paramName);
                  myParamsNameAndValue.Add(deletedRow[paramName, DataRowVersion.Original]);

                  myDeletedKeyAndValueStr += paramName + ":" + deletedRow[paramName, DataRowVersion.Original].ToString().Trim() + Environment.NewLine;
               }

               object[] parms = myParamsNameAndValue.ToArray();

               #endregion

               #region sql

               string sql = "DELETE FROM " + tableName + " WHERE ";

               foreach (string everyKey in myUpdateOrDeleteKeysColumnList) {
                  sql += " " + everyKey + "=" + "@" + everyKey + " AND";
               }

               sql = sql.TrimEnd("AND".ToArray());

               #endregion

               myReturnValue = db.ExecuteSQL(sql, parms);
            }//foreach (DataRow deletedRow in dtDeleteChange.Rows)


         }
         catch (Exception ex) {
            myReturnValue = -1;
            throw ex;
         }
      }


   }
}
