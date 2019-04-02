using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/2
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D30010:DataGate {

        /// <summary>
        /// 判斷20110作業已完成
        /// </summary>
        /// <param name="em_date">yyyy/MM/dd</param>
        /// <returns></returns>
        public int check20110(string em_date) {

            object[] parms = {
                ":em_date", em_date
            };

            string sql =
@"
select count(*) as li_rtn
 from ci.AMIF
 where AMIF_DATE = TO_DATE(:em_date,'YYYY/MM/DD')
  and AMIF_DATA_SOURCE = 'U'
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                return dtResult.Rows[0]["LI_RTN"].AsInt();
            }
        }

        /// <summary>
        /// 期貨市場動態報導－選擇權
        /// </summary>
        /// <param name="adt_date">yyyy/MM/dd</param>
        /// <returns></returns>
        public DataTable d_30012(DateTime adt_date) {

            object[] parms = {
                ":adt_date", adt_date
            };

            string sql =
@"
SELECT AMIF_KIND_ID2 AS AMIF_PARAM_KEY,
         AMIF_SETTLE_DATE,
         AMIF_PC_CODE,
         sum(AMIF_M_QNTY_TAL) as M_QNTY,
         sum(AMIF_OPEN_INTEREST) AS OPEN_INTEREST,   
         (select MIN(RPT_SEQ_NO)
            from ci.RPT
           where RPT_TXD_ID = '30012'  
             and RPT_VALUE = case when AMIF_EXPIRY_TYPE = 'W' then AMIF_KIND_ID2 else A.AMIF_KIND_ID end) as RPT_SEQ_NO,
         AMIF_KIND_ID,
         AMIF_EXPIRY_TYPE
    FROM ci.AMIF A
   WHERE AMIF_PROD_TYPE = 'O'  AND  
         AMIF_PROD_SUBTYPE <> 'S' AND
         AMIF_DATE = :adt_date  
group by AMIF_KIND_ID,AMIF_KIND_ID2,AMIF_EXPIRY_TYPE,
         AMIF_SETTLE_DATE,
         AMIF_PC_CODE
order by amif_param_key, 
         case when amif_expiry_type = 'W' then amif_param_key else amif_kind_id end, 
         amif_kind_id, amif_settle_date, amif_pc_code
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 取得選擇權預設列數
        /// </summary>
        /// <returns></returns>
        public int getTxwRow() {

            string sql =
@"
        select RPT_SEQ_NO as li_txw_row
          from ci.RPT
         where RPT_TXD_ID = '30012'
            and RPT_VALUE = 'TXW'
";
            DataTable dtResult = db.GetDataTable(sql, null);

            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                return dtResult.Rows[0]["LI_TXW_ROW"].AsInt();
            }
        }

        /// <summary>
        /// 期貨市場動態報導－期貨
        /// </summary>
        /// <param name="adt_date">yyyy/MM/dd</param>
        /// <returns></returns>
        public DataTable d_30011(DateTime adt_date) {

            object[] parms = {
                ":adt_date", adt_date
            };

            string sql =
@"
SELECT AMIF_DATE,       
         AMIF_KIND_ID,   
         AMIF_SETTLE_DATE,     
         AMIF_OPEN_PRICE,   
         AMIF_HIGH_PRICE,   
         AMIF_LOW_PRICE,   
         AMIF_CLOSE_PRICE,   
         AMIF_M_QNTY_TAL,   
         AMIF_SETTLE_PRICE,   
         AMIF_OPEN_INTEREST,  
         AMIF_PROD_TYPE,   
         AMIF_PROD_SUBTYPE,   
         AMIF_PARAM_KEY,   
         AMIF_STRIKE_PRICE, 
         AMIF_PC_CODE,    
         AMIF_PROD_ID,      
         AMIF_DATA_SOURCE, 
         AMIF_UP_DOWN_VAL,  
         RPT_SEQ_NO as RPT_SEQ_NO,
         ' ' as OP_TYPE ,
         AMIF_SETTLE_PRICE,RPT_VALUE_3,nvl(AMIF_EXPIRY_TYPE,' ') as AMIF_EXPIRY_TYPE,
         NVL(AMIF_KIND_ID2,AMIF_PARAM_KEY) AS AMIF_KIND_ID2
    FROM ci.AMIF,
        (select trim(RPT.RPT_VALUE) as RPT_VALUE,RPT.RPT_SEQ_NO as RPT_SEQ_NO,RPT_VALUE_3 from ci.RPT where RPT.RPT_TXD_ID = '30011')  
   WHERE AMIF_DATE = :adt_date
     and AMIF_PROD_TYPE in ('F','M')
     and AMIF_PROD_SUBTYPE <> 'S'
     and trim(NVL(AMIF_KIND_ID2,AMIF_PARAM_KEY)) = RPT_VALUE(+)
     and (AMIF_SETTLE_DATE = '000000' OR AMIF_MTH_SEQ_NO >0)
     and rpt_seq_no is not null
   ORDER BY rpt_seq_no, amif_settle_date, amif_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 取得期貨預設列數
        /// </summary>
        /// <returns></returns>
        public int getMxwRow() {

            string sql =
@"
        select RPT_SEQ_NO as li_mxw_row
          from ci.RPT
         where RPT_TXD_ID = '30011'
            and RPT_VALUE = 'MXW'
";
            DataTable dtResult = db.GetDataTable(sql, null);

            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                return dtResult.Rows[0]["LI_MXW_ROW"].AsInt();
            }
        }

        /// <summary>
        /// 取得30013起始列位置和總列數
        /// </summary>
        /// <returns></returns>
        public DataTable getRowIndexandCount() {

            string sql =
@"
select rpt_seq_no as ii_ole_row,
       rpt_level_1 as li_tot_rowcount
from ci.rpt
where rpt_txd_id = '30013'  
  and rpt_value = 'F' 
  and rpt_value_2 = '2000'
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// 期貨市場動態報導－股票選擇權
        /// </summary>
        /// <param name="adt_date">yyyy/MM/dd</param>
        /// <param name="as_pdk_param_key"></param>
        /// <param name="as_prod_type"></param>
        /// <param name="as_pdk_underlying_market"></param>
        /// <returns></returns>
        public DataTable d_30013(DateTime adt_date, string as_pdk_param_key, string as_prod_type, string as_pdk_underlying_market) {

            object[] parms = {
                ":adt_date", adt_date,
                ":as_pdk_param_key", as_pdk_param_key,
                ":as_prod_type", as_prod_type,
                ":as_pdk_underlying_market", as_pdk_underlying_market
            };

            string sql =
@"
SELECT A.AMIF_KIND_ID,
         A.M_QNTY,
         A.OPEN_INTEREST,
         P.PDK_NAME,
         AMIF_DATA_SOURCE,
         PDK_STOCK_ID
    FROM 
 (SELECT substr(AMIF_KIND_ID,1,2) as AMIF_KIND_ID,
         sum(AMIF_M_QNTY_TAL) as M_QNTY,
         sum(AMIF_OPEN_INTEREST) AS OPEN_INTEREST,
         AMIF_DATA_SOURCE
    FROM ci.AMIF  
   WHERE AMIF_PROD_TYPE = :as_prod_type  AND  
         AMIF_PROD_SUBTYPE = 'S'  AND
         AMIF_DATA_SOURCE IN ('T','P') and
         AMIF_DATE = :adt_date  
group by substr(AMIF_KIND_ID,1,2),AMIF_DATA_SOURCE) A,
 (select SUBSTR(APDK.APDK_KIND_ID,1,2) as KIND_ID,
         min(APDK_NAME) as PDK_NAME ,
         min(APDK_STOCK_ID) as PDK_STOCK_ID
    from ci.APDK 
   where APDK_PARAM_KEY = :as_pdk_param_key
     and APDK_PROD_TYPE = :as_prod_type
     and APDK_UNDERLYING_MARKET LIKE :as_pdk_underlying_market
  group by SUBSTR(APDK.APDK_KIND_ID,1,2) 
   ) P
WHERE P.KIND_ID = A.AMIF_KIND_ID
ORDER BY amif_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 期貨市場動態報導－交易量表
        /// </summary>
        /// <param name="adt_date">yyyy/MM/dd</param>
        /// <returns></returns>
        public DataTable d_30014(DateTime adt_date) {

            object[] parms = {
                ":adt_date", adt_date
            };

            string sql =
@"
SELECT AI1_PROD_TYPE,
         AI1_PROD_SUBTYPE,
         AI1_PARAM_KEY,   
         AI1_M_QNTY,   
         (AI1_M_QNTY - AI1_M_QNTY_Y) as M_INCREASE,   
         AI1_OI,   
         (AI1_OI - AI1_OI_Y) as OI_INCREASE,   
         AI1_AVG_MONTH,   
         AI1_AVG_YEAR,   
         AI1_HIGH_QNTY,   
         AI1_HIGH_DATE, 
         nvl(RPT_SEQ_NO,0) as RPT_SEQ_NO,
         AI1_DATE,
         AI1_KIND_ID2 
    FROM ci.AI1 A,
         (select RPT_SEQ_NO,RPT_VALUE,RPT_VALUE_2,RPT_VALUE_3,RPT_VALUE_4
            from ci.RPT
           where RPT.RPT_TXD_ID = '30014')
   WHERE AI1_DATE = :adt_date  
     AND AI1_MARKET_CODE IN (' ','%')
     AND AI1_PROD_TYPE = RPT_VALUE_3(+)
     AND AI1_PROD_SUBTYPE = RPT_VALUE_2(+)   
     AND AI1_PARAM_KEY = RPT_VALUE(+)      
     AND nvl(trim(AI1_KIND_ID2),' ') = nvl(trim(RPT_VALUE_4(+)),' ') 
   ORDER BY rpt_seq_no
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 期貨市場動態報導－開戶數
        /// </summary>
        /// <param name="adt_date">yyyy/MM/dd</param>
        /// <returns></returns>
        public DataTable d_30015(DateTime adt_date) {

            object[] parms = {
                ":adt_date", adt_date
            };

            string sql =
@"
SELECT  AB3_DATE ,
           AB3_COUNT ,
           AB3_INCREASE ,
           AB3_TRADE_COUNT ,
           AB3_COUNT1 ,
           AB3_COUNT2     
        FROM ci.AB3      
 WHERE ( AB3_DATE = :adt_date )
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 判斷前一交易日之資料是否存在
        /// </summary>
        /// <param name="ldt_date"></param>
        /// <param name="ld_date_fm"></param>
        /// <returns></returns>
        public DateTime checkPreviousData(DateTime ldt_date, DateTime ld_date_fm) {

            object[] parms = {
                ":ldt_date", ldt_date,
                ":ld_date_fm", ld_date_fm
            };

            string sql =
@"
select to_date(max(AI2_YMD),'yyyymmdd') as ldt_date
  from ci.AI2
 where AI2_SUM_TYPE = 'D'
   and AI2_SUM_SUBTYPE = '1'
   and AI2_PROD_TYPE = 'F'
   and AI2_YMD < to_char(:ldt_date,'yyyymmdd')
   and AI2_YMD >= to_char(:ld_date_fm,'yyyymmdd')
";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count == 0) {

                return DateTime.MinValue;
            }
            else {
                return dtResult.Rows[0]["LDT_DATE"].AsDateTime();
            }
        }

        /// <summary>
        /// 取得開戶數預設列數
        /// </summary>
        /// <returns></returns>
        public int get30015Row() {

            string sql =
@"
select RPT_SEQ_NO as ii_ole_row
  from ci.RPT
 where RPT_tXN_ID = '30010' and RPT_TXD_ID = '30015'
";
            DataTable dtResult = db.GetDataTable(sql, null);

            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                return dtResult.Rows[0]["II_OLE_ROW"].AsInt();
            }
        }

        /// <summary>
        /// 取得30015成交值 1
        /// </summary>
        /// <returns></returns>
        public decimal get30015Amt_1(string ls_date) {

            object[] parms = {
                ":ls_date", ls_date
            };

            string sql =
@"
SELECT nvl(SUM(AA2_AMT),0) as ld_amt
  FROM ci.AA2      
 WHERE AA2_YMD = :ls_date
   AND AA2_PROD_TYPE = 'F'
    AND AA2_PROD_SUBTYPE = 'I'
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                return dtResult.Rows[0]["LD_AMT"].AsDecimal();
            }
        }

        /// <summary>
        /// 取得30015成交值 2
        /// </summary>
        /// <returns></returns>
        public decimal get30015Amt_2(DateTime ldt_date) {

            object[] parms = {
                ":ldt_date", ldt_date
            };

            string sql =
@"
SELECT nvl(amif_m_qnty_tal,0) as ld_amt
  FROM ci.AMIF
 where AMIF_DATE = :ldt_date
   and AMIF_KIND_ID = 'TXF'
    and AMIF_SETTLE_DATE = '000000'
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                return dtResult.Rows[0]["LD_AMT"].AsDecimal();
            }
        }

        /// <summary>
        /// 讀取前一交易日 wf_30016
        /// </summary>
        /// <param name="ls_date"></param>
        /// <returns></returns>
        public DateTime checkPreviousDay(string ls_date) {

            object[] parms = {
                ":ls_date", ls_date
            };

            string sql =
@"
select nvl(max(AI2_YMD),'')  as ls_date from ci.AI2
 WHERE AI2_YMD < :ls_date
   AND AI2_SUM_TYPE = 'D'
   AND AI2_SUM_SUBTYPE = '1'
   AND AI2_PROD_TYPE = 'F'
";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count == 0) {

                return DateTime.MinValue;
            }
            else {
                return dtResult.Rows[0]["LS_DATE"].AsDateTime();
            }
        }

        /// <summary>
        /// Eurex/TAIFEX 合作商品概況
        /// </summary>
        /// <param name="adt_date">yyyy/MM/dd</param>
        /// <returns></returns>
        public DataTable d_30016(DateTime as_date) {

            object[] parms = {
                ":as_date", as_date
            };

            string sql =
@"
SELECT AE3_DATE,   
         AE3_PROD_TYPE,   
         AE3_PROD_SUBTYPE,   
         AE3_PARAM_KEY,   
         AE3_KIND_ID2,   
         AE3_PC_CODE,   
         AE3_BEFORE_DATE,   
         AE3_M_QNTY,   
         AE3_M_QNTY_Y,   
         AE3_OI,   
         AE3_OI_Y,   
         AE3_AVG_MONTH,   
         AE3_AVG_YEAR,   
         AE3_HIGH_QNTY,   
         AE3_HIGH_DATE ,
         RPT_SEQ_NO ,
         (AE3_M_QNTY - AE3_M_QNTY_Y) as M_INCREASE,   
         (AE3_OI - AE3_OI_Y) as OI_INCREASE
    FROM ci.AE3,
         (select RPT_SEQ_NO,RPT_VALUE,RPT_VALUE_3,RPT_VALUE_5
            from ci.RPT
           where RPT.RPT_TXD_ID = '30016')
   WHERE AE3_DATE = :as_date
     and AE3_PARAM_KEY = RPT_VALUE(+)      
     AND AE3_PC_CODE = RPT_VALUE_5(+)   
     AND AE3_PROD_TYPE = RPT_VALUE_3(+)
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
