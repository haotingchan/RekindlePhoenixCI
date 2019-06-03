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
   public class D51030:DataGate
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

      public ResultData UpdateMMF(DataTable inputData)
      {
         //string sql = @"
         //               SELECT 
         //                MMF_PARAM_KEY 
         //               ,MMF_RESP_RATIO 
         //               ,MMF_QNTY_LOW 
         //               ,MMF_QUOTE_VALID_RATE 
         //               ,MMF_AVG_TIME 
         //               ,MMF_W_USER_ID 
         //               ,MMF_W_TIME 
         //               ,MMF_RFC_MIN_CNT 
         //               ,MMF_MARKET_CODE 
         //               ,MMF_END_TIME 
         //               ,MMF_PROD_TYPE 
         //               ,MMF_CP_KIND 
         //               ,MMF_RESP_TIME 
         //               ,MMF_QUOTE_DURATION
         //            FROM CI.MMF";
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
            return SaveForAll(inputData, tableName, insertColumnList, updateColumnList, keysColumnList);
         }
         catch (Exception ex) {
            throw ex;
         }
      }
   }
}
