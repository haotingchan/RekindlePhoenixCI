using OnePiece;
using System.Data;

/// <summary>
/// ken 2018/12/20
/// </summary>
namespace DataObjects.Dao.Together {
   /// <summary>
   /// 就是lookup檔,每個功能有自己的參照資訊,之後會亂
   /// </summary>
   public class COD {
      private Db db;

      public COD() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// CI.COD
      /// </summary>
      /// <param name="COD_TXN_ID"></param>
      /// <returns>cod_id/cod_desc/cp_display</returns>
      public DataTable ListByTxn(string COD_TXN_ID) {
         object[] parms =
         {
                ":COD_TXN_ID", COD_TXN_ID
            };

         string sql = @"
SELECT trim(COD_ID) as cod_id, 
    trim(COD_DESC) as cod_desc,
    trim(cod_id) || ' : ' || trim(cod_desc) as cp_display
FROM CI.COD  
WHERE COD_TXN_ID = :COD_TXN_ID";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// CI.COD 
      /// </summary>
      /// <param name="COD_TXN_ID"></param>
      /// <param name="COD_COL_ID"></param>
      /// <param name="FirstRowText">自行定義新增的第一個item的Text</param>
      /// <param name="FirstRowValue">自行定義新增的第一個item的Value</param>
      /// <returns>第一行空白+COD_ID/COD_DESC/COD_SEQ_NO</returns>
      public DataTable ListByCol(string COD_TXN_ID , string COD_COL_ID , string FirstRowText = " " , string FirstRowValue = " ") {
         object[] parms =
         {
                ":COD_TXN_ID", COD_TXN_ID,
                ":COD_COL_ID", COD_COL_ID
            };

         //ken,簡易防止sql injection(基本上這兩個值不應該從UI傳進來)
         string tmp = FirstRowText.Length > 20 ? FirstRowText.Substring(0 , 20) : FirstRowText;
         string firstRowText = tmp.Replace("'" , "").Replace("--" , "").Replace(";" , "");
         string tmp2 = FirstRowValue.Length > 20 ? FirstRowValue.Substring(0 , 20) : FirstRowValue;
         string firstRowValue = tmp2.Replace("'" , "").Replace("--" , "").Replace(";" , "");

         string sql = string.Format(@"
SELECT TRIM(COD_ID) AS COD_ID,
    TRIM(COD_DESC) AS COD_DESC,
    COD_SEQ_NO
FROM CI.COD
WHERE COD_TXN_ID = :COD_TXN_ID
AND COD_COL_ID = :COD_COL_ID
UNION ALL
SELECT '{0}','{1}',0 FROM DUAL
order by cod_seq_no" , FirstRowValue , FirstRowText);

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// CI.COD 
      /// </summary>
      /// <param name="COD_TXN_ID"></param>
      /// <param name="COD_COL_ID"></param>
      /// <returns>COD_ID/COD_DESC/COD_SEQ_NO/cp_display</returns>
      public DataTable ListByCol2(string COD_TXN_ID , string COD_COL_ID) {

         object[] parms =
{
                ":COD_TXN_ID", COD_TXN_ID,
                ":COD_COL_ID", COD_COL_ID
            };

         string sql = @"
select a.COD_ID as COD_ID,
a.COD_DESC as COD_DESC,
a.COD_SEQ_NO,
'('||COD_ID||')'||COD_DESC as cp_display
from (
    SELECT trim(COD_ID) as COD_ID,   
        COD_DESC,
        COD_SEQ_NO
    FROM CI.COD  
    WHERE COD_TXN_ID = :COD_TXN_ID
    AND COD_COL_ID = :COD_COL_ID
) a   
order by cod_seq_no";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// dddw_mgt8_kind_type
      /// 商品類別(目前 for 49061,若有相似的可帶入參數或改寫)
      /// </summary>
      /// <param name="COD_TXN_ID"></param>
      /// <param name="COD_COL_ID"></param>
      /// <returns>cod_id/cod_desc/cp_display</returns>
      public DataTable ListKindByCol2(string COD_TXN_ID = "MGT8" , string COD_COL_ID = "MGT8_KIND_TYPE") {
         object[] parms =
         {
                ":COD_TXN_ID", COD_TXN_ID,
                ":COD_COL_ID", COD_COL_ID
            };

         string sql = @"
SELECT 
  TRIM(COD_ID)AS KIND_TYPE,   
  TRIM(COD_DESC) AS KIND_NAME,   
  COD_SEQ_NO 
FROM CI.COD  
WHERE COD_TXN_ID = :COD_TXN_ID   
AND COD_COL_ID = :COD_COL_ID 
ORDER BY COD_SEQ_NO
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// dddw_mgt8_currency_type
      /// 幣別(目前 for 49061,若有相似的可帶入參數或改寫)
      /// </summary>
      /// <param name="COD_TXN_ID"></param>
      /// <param name="COD_COL_ID"></param>
      /// <returns>cod_id/cod_desc/cp_display</returns>
      public DataTable ListCurrencyByCol2(string COD_TXN_ID = "EXRT" , string COD_COL_ID = "EXRT_CURRENCY_TYPE") {
         object[] parms =
         {
                ":COD_TXN_ID", COD_TXN_ID,
                ":COD_COL_ID", COD_COL_ID
            };

         string sql = @"
SELECT 
  TRIM(COD_ID)AS CURRENCY_TYPE,   
  TRIM(COD_DESC) AS CURRENCY_NAME,   
  COD_SEQ_NO  
FROM CI.COD  
WHERE COD_TXN_ID = 'EXRT'   
AND COD_COL_ID = 'EXRT_CURRENCY_TYPE'
ORDER BY COD_SEQ_NO
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
