using OnePiece;
using System.Data;

/// <summary>
/// ken 2018/12/20
/// </summary>
namespace DataObjects.Dao.Together {
   /// <summary>
   /// APDK商品資訊?
   /// </summary>
   public class APDK {
      private Db db;

      public APDK() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// CI.APDK (已經固定一些過濾條件)
      /// </summary>
      /// <returns>前面[全部/期貨/選擇權]+APDK_PROD_TYPE/PDK_KIND_ID/cp_display</returns>
      public DataTable ListAll() {

         string sql = @"
select a.APDK_PROD_TYPE,
a.PDK_KIND_ID,
a.PDK_KIND_ID as cp_display
from (
    SELECT '4' as s,
        APDK_PROD_TYPE as APDK_PROD_TYPE,
        APDK_KIND_ID as PDK_KIND_ID 
    FROM ci.APDK  
    where APDK_QUOTE_CODE = 'Y'
    and APDK_PROD_TYPE in ('F','O')
    GROUP BY APDK_PROD_TYPE,APDK_KIND_ID

    UNION
        SELECT '1','ALL','全部' FROM DUAL
    UNION
        SELECT '2','FUT','期貨' FROM DUAL
    UNION
        SELECT '3','OPT','選擇權' FROM DUAL
) a
order by s ,apdk_prod_type , pdk_kind_id";

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

      /// <summary>
      /// d_pdk_kind_id_o
      /// dw:dddw_pdk_kind_id
      /// </summary>
      /// <returns>前面[全部/期貨/選擇權]+apdk_prod_type/pdk_kind_id/cp_display</returns>
      public DataTable dddw_pdk_kind_id() {

         string sql = @"
select 
    a.apdk_prod_type,
    a.pdk_kind_id,
    a.pdk_kind_id as cp_display
from (
    select '4' as s,
    apdk_prod_type as apdk_prod_type,
    apdk_kind_id as pdk_kind_id,
    apdk_market_code as market_code
from ci.apdk  
where apdk_prod_type in ('F','O')
group by apdk_prod_type,apdk_kind_id ,apdk_market_code 
union
    select '1','ALL','全部','-1' from dual
union
    select '2','FUT','期貨','-1' from dual
union
    select '3','OPT','選擇權','-1' from dual
) a
order by s ,apdk_prod_type , pdk_kind_id
";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }


      /// <summary>
      /// CI.APDK (已經固定一些過濾條件)
      /// </summary>
      /// <returns>前面空一行+APDK_PROD_TYPE/APDK_KIND_ID/APDK_NAME/cp_display</returns>
      public DataTable ListAll2() {

         string sql = @"
select a.APDK_PROD_TYPE,a.APDK_KIND_ID,a.APDK_NAME,
(case when trim(apdk_name) is null then trim(apdk_kind_id)
     else trim(apdk_kind_id)||'('||trim(apdk_name)||')' end) as cp_display
from (
    SELECT APDK_PROD_TYPE,
        case APDK_PROD_SUBTYPE when 'S' then substr(APDK_KIND_ID,1,2) else substr(APDK_KIND_ID,1,3) end as APDK_KIND_ID,
        max(trim(APDK_NAME)) as APDK_NAME
    FROM CI.APDK
    WHERE (APDK_END_DATE IS NULL or APDK_END_DATE = '')
    and APDK_PROD_TYPE in ('F','O')
    and APDK_QUOTE_CODE = 'Y'
    group by APDK_PROD_TYPE,case APDK_PROD_SUBTYPE when 'S' then substr(APDK_KIND_ID,1,2) else substr(APDK_KIND_ID,1,3) end
    union 
    SELECT ' ','-',' '
    FROM DUAL
) a
order by apdk_prod_type , apdk_kind_id";

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }


      /// <summary>
      /// CI.APDK (已經固定一些過濾條件)
      /// </summary>
      /// <returns>前面空一行+APDK_PROD_TYPE/PDK_KIND_ID/MARKET_CODE</returns>
      public DataTable ListAll3() {

         string sql = @"
SELECT APDK_PROD_TYPE,
APDK_KIND_ID as PDK_KIND_ID,
APDK_MARKET_CODE as MARKET_CODE
FROM ci.APDK  
where APDK_PROD_TYPE in ('F','O')
GROUP BY APDK_PROD_TYPE,APDK_KIND_ID ,APDK_MARKET_CODE 
UNION
SELECT ' ','',' '
FROM DUAL
order by apdk_prod_type , pdk_kind_id";

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

      /// <summary>
      /// CI.APDK (已經固定一些過濾條件)
      /// </summary>
      /// <param name="APDK_PROD_TYPE"></param>
      /// <returns>前面空一行+APDK_PROD_TYPE/PDK_KIND_ID/MARKET_CODE</returns>
      public DataTable ListKindByType(string APDK_PROD_TYPE = "O") {
         object[] parms =
         {
                ":APDK_PROD_TYPE", APDK_PROD_TYPE
            };

         string sql = @"
SELECT APDK_PROD_TYPE,
APDK_KIND_ID as PDK_KIND_ID,
MAX(APDK_MARKET_CODE) as MARKET_CODE
FROM ci.APDK  
where APDK_PROD_TYPE = :APDK_PROD_TYPE
GROUP BY APDK_PROD_TYPE,APDK_KIND_ID ,APDK_MARKET_CODE 
UNION
SELECT '','',' '
FROM DUAL
order by pdk_kind_id";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }


      /// <summary>
      /// CI.APDK (已經固定一些過濾條件)
      /// </summary>
      /// <param name="APDK_PROD_TYPE">預設 = F+O</param>
      /// <returns>前面空一行+APDK_KIND_ID_STO/MARKET_CODE</returns>
      public DataTable ListStockKindByType(string APDK_PROD_TYPE = "") {
         object[] parms =
         {
                ":APDK_PROD_TYPE", APDK_PROD_TYPE
            };

         //預設 = F+O
         string filter = (APDK_PROD_TYPE == "" ? "WHERE APDK_PROD_TYPE in ('F','O')" : "WHERE APDK_PROD_TYPE = :APDK_PROD_TYPE");

         string sql = string.Format(@"
SELECT APDK_KIND_ID_STO,
MAX(APDK_MARKET_CODE) AS MARKET_CODE
FROM ci.APDK  
{0}
GROUP BY APDK_KIND_ID_STO 
UNION
  SELECT '',''
    FROM DUAL
order by apdk_kind_id_sto", filter);

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

      /// <summary>
      /// CI.APDK (已經固定一些過濾條件)
      /// </summary>
      /// <returns>前面空一行+APDK_KIND_ID_STO/MARKET_CODE</returns>
      public DataTable ListKindId(string marketCode="") {

         string sql = string.Format(@"
SELECT APDK_KIND_ID ,MAX(APDK_MARKET_CODE) AS MARKET_CODE
    FROM ci.APDK  
 where APDK_PROD_TYPE in ('F','O') 
{0}
GROUP BY APDK_KIND_ID  
UNION
  SELECT ' ',' '
    FROM DUAL", marketCode);

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

      /// <summary>
      /// CI.APDK (已經固定一些過濾條件)
      /// </summary>
      /// <returns>前面空一行+APDK_KIND_ID_STO/MARKET_CODE</returns>
      public DataTable ListKind2(string marketCode ="") {

         string sql = string.Format(@"
SELECT APDK_KIND_ID2 AS APDK_KIND_ID_STO,
MAX(APDK_MARKET_CODE) AS MARKET_CODE
FROM ci.APDK  
WHERE APDK_PROD_TYPE in ('O','F')   
{0}
GROUP BY APDK_KIND_ID2 
UNION
  SELECT ' ',' '
    FROM DUAL
order by apdk_kind_id_sto", marketCode);

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

      /// <summary>
      /// CI.APDK (已經固定一些過濾條件)
      /// </summary>
      /// <returns>前面空一行+APDK_PROD_TYPE/APDK_PARAM_KEY/MARKET_CODE</returns>
      public DataTable ListParamKey(string marketCode = "") {

         string sql = string.Format(@"
SELECT APDK_PROD_TYPE,
APDK_PARAM_KEY,
MAX(APDK_MARKET_CODE) MARKET_CODE,
MAX(APDK_PROD_SUBTYPE) as APDK_PROD_SUBTYPE
FROM ci.APDK
where APDK_QUOTE_CODE = 'Y'
and APDK_PROD_TYPE in ('F','O')
{0}
group by APDK_PROD_TYPE,APDK_PARAM_KEY
UNION
SELECT ' ','',' ',' '
FROM DUAL
order by APDK_PROD_TYPE , APDK_PARAM_KEY", marketCode);

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

        /// <summary>
        /// CI.APDK (已經固定一些過濾條件)
        /// </summary>
        /// <returns></returns>
        public DataTable ListParamKeyAndProd()
        {

            string sql = @"
                                    SELECT * FROM(            
                                                SELECT APDK_PROD_TYPE,
                                                APDK_PARAM_KEY,
                                                '999' AS SEQ_NO
                                                FROM CI.APDK
                                                WHERE APDK_PROD_TYPE IN ('F','O')
                                                GROUP BY APDK_PROD_TYPE,APDK_PARAM_KEY
                                                UNION
                                                    SELECT '%','全部','0' FROM DUAL
                                                UNION
                                                    SELECT 'F','期貨','1' FROM DUAL
                                                UNION
                                                    SELECT 'O','選擇權','2' FROM DUAL
                                    )A                
                                    ORDER BY SEQ_NO,APDK_PROD_TYPE , APDK_PARAM_KEY
                                    ";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// CI.APDK 契約類別 B－ C－商品類 E－匯率類 I－指數類 R－利率類 S－股票類
        /// </summary>
        /// <returns>前面空一行+APDK_PROD_SUBTYPE/PROD_SUBTYPE_NAME/cp_display</returns>
        public DataTable ListProdSubType() {

         //B－ C－商品類 E－匯率類 I－指數類 R－利率類 S－股票類

         string sql = @"
select a.*,
(case when prod_subtype_name is null then apdk_prod_subtype||'－ ' else apdk_prod_subtype||'－'||prod_subtype_name end) as cp_display
from (
    SELECT APDK_PROD_SUBTYPE,PROD_SUBTYPE_NAME
    FROM CI.APDK,
        (SELECT trim(COD_ID) as PROD_SUBTYPE,trim(COD_DESC) as PROD_SUBTYPE_NAME FROM CI.COD
        where COD_TXN_ID = '49020'
        and COD_COL_ID = 'PDK_SUBTYPE')
    where trim(APDK_PROD_SUBTYPE) = PROD_SUBTYPE(+)
    GROUP BY APDK_PROD_SUBTYPE,PROD_SUBTYPE_NAME
    UNION
      SELECT '',' '
        FROM DUAL
) a    
order by apdk_prod_subtype
";

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }


      /// <summary>
      /// 契約基本資料
      /// </summary>
      /// <param name="kindId"></param>
      /// <returns>APDK_KIND_ID/APDK_PROD_TYPE/APDK_PROD_SUBTYPE/APDK_PARAM_KEY</returns>
      public DataTable ListAllByKindId(string kindId) {
         object[] parms =
         {
                ":as_kind_id", kindId
            };

         string sql = @"
select APDK_KIND_ID,
APDK_PROD_TYPE,
APDK_PROD_SUBTYPE,
APDK_PARAM_KEY
from ci.APDK
where APDK_KIND_ID = :as_kind_id
";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// w500xx下拉選單
      /// </summary>
      /// <param name="col">欄位名稱</param>
      /// <returns></returns>
      public DataTable dw_prod_500xx(string col, string emptycol, string marketCode = "") {
         string sql = string.Format(@"
SELECT {0},MAX(APDK_MARKET_CODE) MARKET_CODE  
    FROM ci.APDK  
 where APDK_QUOTE_CODE = 'Y'
   and APDK_PROD_TYPE in ('F','O')
   {2}
group by {0}
UNION
  SELECT {1},' '
    FROM DUAL
", col, emptycol, marketCode);
         DataTable dtResult = db.GetDataTable(sql, null);
         return dtResult;
      }

      /// <summary>
      /// Lukas, 2018/12/25
      /// </summary>
      /// <returns>apdk_kind_id</returns>
      public DataTable ListAll_55031() {

         string sql = @"
SELECT SUBSTR(APDK_KIND_ID,1,2) as apdk_kind_id
  FROM ci.APDK
  where APDK_PROD_TYPE = 'O'
   and APDK_PROD_SUBTYPE = 'S'
   and APDK_PARAM_KEY = 'STC'
   and APDK_END_DATE is null
 group by SUBSTR(APDK_KIND_ID,1,2)
union
SELECT 'STC' from dual";

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

      /// <summary>
      /// CI.APDK (已經固定一些過濾條件)
      /// return CPR_SELECT / P_KIND_ID2 / P_NAME
      /// </summary>
      /// <param name="prodType"></param>
      /// <param name="prodSubType"></param>
      /// <returns>CPR_SELECT / P_KIND_ID2 / P_NAME</returns>
      public DataTable ListAll4(string prodType = "O", string prodSubType = "S") {
         object[] parms ={
                ":prodType", prodType,
                ":prodSubType", prodSubType,
            };

         string sql = @"
select 'N' as cpr_select,
    apdk_kind_id2 as p_kind_id2,
    max(apdk_name) as p_name
from ci.apdk
where apdk_prod_type = :prodType
and apdk_prod_subtype = :prodSubType
and apdk_end_date is null
group by apdk_kind_id2
order by p_kind_id2";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

   }
}
