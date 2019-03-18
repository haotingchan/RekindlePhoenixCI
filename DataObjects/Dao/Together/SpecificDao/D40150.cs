using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/3/13
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// SPAN參數一覽表
   /// </summary>
   public class D40150 {

      private Db db;

      public D40150() {

         db = GlobalDaoSetting.DB;

      }

      /// <summary>
      /// get CI.SP1/CI.SPT1 data return 12 fields (D40150)
      /// </summary>
      /// <param name="isDate">yyyy/MM/dd</param>
      /// <returns></returns>
      public DataTable GetDataList(DateTime isDate) {

         object[] parms = {
                ":isDate", isDate
            };


         string sql = @"
SELECT 
	SD.SP1_DATE,   
	SD.SP1_TYPE,   
	SD.SP1_KIND_ID1,   
	SD.SP1_KIND_ID2,   
	SPT1_KIND_ID1_OUT,   
	SPT1_KIND_ID2_OUT,   
	SPT1_COM_ID,  
	SD.SP1_SEQ_NO,   
	SD.SP1_RATE AS SD_SP1_RATE,
	SS.SP1_RATE AS SS_SP1_RATE,
	SD.SP1_CUR_RATE AS SD_SP1_CUR_RATE,
	SS.SP1_CUR_RATE AS SS_SP1_CUR_RATE
FROM 
	CI.SP1 SD,
	CI.SP1 SS,
	CI.SPT1  
WHERE SD.SP1_KIND_ID1 = SPT1_KIND_ID1  
AND SD.SP1_KIND_ID2 = SPT1_KIND_ID2  
AND SD.SP1_DATE = :isDate  
AND SD.SP1_TYPE= 'SD' 
AND SS.SP1_TYPE= 'SS' 
AND SD.SP1_DATE = SS.SP1_DATE  
AND SD.SP1_KIND_ID1 = SS.SP1_KIND_ID1 
AND SD.SP1_KIND_ID2 = SS.SP1_KIND_ID2 
ORDER BY SP1_TYPE , SP1_SEQ_NO 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get CI.SP2 return SP2_DATE/SP2_TYPE/SP2_KIND_ID1/SP2_KIND_ID2/SP2_VALUE_DATE/SP2_W_TIME/SP2_W_USER_ID (D40150_sp2) 
      /// </summary>
      /// <param name="isDate">yyyy/MM/dd</param>
      /// <returns></returns>
      public DataTable ListSp2ByDate(DateTime isDate) {

         object[] parms = {
                ":isDate", isDate
            };


         string sql = @"
SELECT
	SP2_DATE, 
	SP2_TYPE, 
	SP2_KIND_ID1, 
	SP2_KIND_ID2, 
	SP2_VALUE_DATE, 
	SP2_W_TIME, 
	SP2_W_USER_ID
FROM CI.SP2
WHERE SP2_DATE =:isDate
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get CI.SP1,CI.SPT1,CI.RPT (D40152) 
      /// return SP1_DATE/SP1_TYPE/SP1_KIND_ID1/SPT1_ABBR_NAME/SPT1_COM_ID/SP1_SEQ_NO/SD_SP1_RATE/SD_SP1_CUR_RATE/RPT_SEQ_NO 
      /// </summary>
      /// <param name="isDate">yyyy/MM/dd</param>
      /// <returns></returns>
      public DataTable ListByDate(DateTime isDate) {

         object[] parms = {
                ":isDate", isDate
            };


         string sql = @"
SELECT 
	SD.SP1_DATE,   
	SD.SP1_TYPE,   
	SD.SP1_KIND_ID1,   
	SPT1_ABBR_NAME,     
	SPT1_COM_ID,  
	SD.SP1_SEQ_NO,   
	SD.SP1_RATE AS SD_SP1_RATE ,
	SD.SP1_CUR_RATE AS SD_SP1_CUR_RATE ,
	RPT_SEQ_NO
FROM 
CI.SP1 SD, 
CI.SPT1,
CI.RPT 
WHERE SD.SP1_KIND_ID1 = SPT1_KIND_ID1  
AND SD.SP1_KIND_ID2 = SPT1_KIND_ID2  
AND SD.SP1_DATE = :isDate  
AND SD.SP1_TYPE= 'SV' 
AND RPT_TXD_ID = '40152' 
AND SD.SP1_KIND_ID1 = RPT_VALUE
ORDER BY SP1_TYPE , SP1_SEQ_NO
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
