using System;
using System.Data;
/// <summary>
/// Winni, 2019/4/2
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D49020 : DataGate {
      /// <summary>
      /// get CI.MGT2 data return 17 feild (d49020)
      /// </summary>
      /// <returns></returns>
      public DataTable ListData() {

         string sql = @"
SELECT  
	MGT2_KIND_ID,
	MGT2_KIND_ID_OUT,
	MGT2_SEQ_NO,
	MGT2_PROD_TYPE,
	MGT2_PROD_SUBTYPE,

	MGT2_ABBR_NAME,
	MGT2_NAME,
	MGT2_GROUP_KIND_ID,
	MGT2_STOCK_ID,
	MGT2_END_YMD,

	MGT2_DATA_TYPE,
	MGT2_ADJUST_RATE,
	MGT2_CP_KIND,
	MGT2_ABROAD,
	MGT2_W_TIME,

	MGT2_W_USER_ID,
   MGT2_ADJUST_RATE_MAXV,
   MGT2_ADJUST_RATE_EWMA,   
	' ' AS IS_NEWROW
FROM CI.MGT2
ORDER BY MGT2_SEQ_NO , MGT2_KIND_ID
";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      /// <summary>
      /// get CI.COD data return COD_ID/COD_DESC/CP_DISPLAY 3 feild (dddw_prod_type)
      /// (商品別) dropdownlist
      /// </summary>
      /// <param name="COD_TXN_ID"></param>
      /// <param name="COD_COL_ID"></param>
      /// <returns></returns>
      public DataTable GetProdType(string COD_TXN_ID = "49020" , string COD_COL_ID = "MGT2_PROD_TYPE") {

         object[] parms =
         {
                ":COD_TXN_ID", COD_TXN_ID,
                ":COD_COL_ID", COD_COL_ID
            };

         string sql = @"
SELECT 
    TRIM(COD_ID) AS COD_ID, 
    TRIM(COD_DESC) AS COD_DESC,
    TRIM(COD_ID) || ' : ' || TRIM(COD_DESC) AS CP_DISPLAY
FROM CI.COD  
WHERE COD_TXN_ID = :COD_TXN_ID
AND COD_COL_ID = :COD_COL_ID          
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get CI.COD data return COD_ID/COD_DESC/COD_SEQ_NO 3 feild (dddw_mgt2_cp_kind)
      /// (風險價格係數計算方式) dropdownlist
      /// </summary>
      /// <param name="COD_TXN_ID"></param>
      /// <param name="COD_COL_ID"></param>
      /// <returns></returns>
      public DataTable GetCpKind(string COD_TXN_ID = "MGT2" , string COD_COL_ID = "MGT2_CP_KIND") {

         object[] parms =
         {
                ":COD_TXN_ID", COD_TXN_ID,
                ":COD_COL_ID", COD_COL_ID
            };

         string sql = @"
SELECT 
    TRIM(COD_ID) AS COD_ID,   
    TRIM(COD_DESC) AS COD_DESC,   
    COD_SEQ_NO  
FROM CI.COD
WHERE COD_TXN_ID = :COD_TXN_ID
AND COD_COL_ID = :COD_COL_ID
UNION ALL
SELECT ' ',' ',0 FROM DUAL
ORDER BY COD_SEQ_NO 
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}
