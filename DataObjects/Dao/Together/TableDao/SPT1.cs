using BusinessObjects;
using System;
using System.Data;
/// <summary>
/// Winni, 2019/5/20 (解決並行違規)
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class SPT1 : DataGate {

      /// <summary>
      /// save CI.SPT1 data
      /// 處理並行違規的方式
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateSPT1(DataTable inputData) {

         string tableName = "CI.SPT1";
         string keysColumnList = @"
   SPT1_KIND_ID1, 
   SPT1_KIND_ID2";
         string insertColumnList = @"
   SPT1_KIND_ID1, 
   SPT1_KIND_ID2, 
   SPT1_ABBR_NAME, 
   SPT1_NAME, 
   SPT1_SEQ_NO, 

   SPT1_W_TIME, 
   SPT1_W_USER_ID, 
   SPT1_KIND_ID1_OUT, 
   SPT1_KIND_ID2_OUT, 
   SPT1_COM_ID, 

   SPT1_OSW_GRP, 
   SPT1_ADJUST_RATE, 
   SPT1_DATA_TYPE, 
   SPT1_MAX_SPNS_RATE";
         string updateColumnList = insertColumnList;
         try {
            //update to DB
            return SaveForChanged(inputData , tableName , insertColumnList , updateColumnList , keysColumnList);
         } catch (Exception ex) {
            throw new Exception("儲存錯誤");
         }
      }
   }
}
