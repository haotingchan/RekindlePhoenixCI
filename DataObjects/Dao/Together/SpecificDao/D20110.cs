using BusinessObjects;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/1/29
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D20110: DataGate {

        public DataTable d_20110(DateTime adt_date) {

            object[] parms = {
                ":adt_date", adt_date
            };

            string sql =
@"
SELECT A.*
FROM
    (SELECT AMIF_DATE,       
             AMIF_KIND_ID,   
             decode(trim(AMIF_SETTLE_DATE),'000000','指數',trim(AMIF_SETTLE_DATE)) as AMIF_SETTLE_DATE,     
             AMIF_OPEN_PRICE,   
             AMIF_HIGH_PRICE,   
             AMIF_LOW_PRICE,   
             AMIF_CLOSE_PRICE, 
             AMIF_UP_DOWN_VAL,
             AMIF_SETTLE_PRICE, 
             AMIF_M_QNTY_TAL,    
             AMIF_OPEN_INTEREST,   
             AMIF_SUM_AMT,       
             AMIF_PROD_TYPE,   
             AMIF_PROD_SUBTYPE,   
             AMIF_DATA_SOURCE,
             decode(trim(AMIF_KIND_ID),'GDF', AMIF_EXCHANGE_RATE, '') as AMIF_EXCHANGE_RATE,
             AMIF_M_TIME,  
             AMIF_PARAM_KEY,   
             AMIF_STRIKE_PRICE, 
             AMIF_PC_CODE,    
             AMIF_PROD_ID,       
             (select min(RPT.RPT_SEQ_NO)
                from ci.RPT
               where RPT.RPT_TXD_ID = '20110'  
                 and RPT.RPT_VALUE = AMIF_KIND_ID) as RPT_SEQ_NO,
             ' ' as OP_TYPE ,
             AMIF_CLOSE_PRICE as AMIF_CLOSE_PRICE_Y,
             ' ' as AMIF_CLOSE_PRICE_Y_FLAG,AMIF_YEAR,
             AMIF_CLOSE_PRICE as AMIF_CLOSE_PRICE_ORIG,
             '                              ' AS AMIFU_ERR_TEXT,
             '' AS CP_ERR,   
             AMIF_OPEN_PRICE as R_OPEN_PRICE,
             AMIF_HIGH_PRICE as R_HIGH_PRICE,
             AMIF_LOW_PRICE as R_LOW_PRICE,
             AMIF_CLOSE_PRICE as R_CLOSE_PRICE ,
             AMIF_M_QNTY_TAL as R_M_QNTY_TAL,    
             AMIF_OPEN_INTEREST as R_OPEN_INTEREST,   
             AMIF_UP_DOWN_VAL as R_UP_DOWN_VAL,
             AMIF_MTH_SEQ_NO ,
             AMIF_OSW_GRP        
        FROM ci.AMIF
       WHERE AMIF_DATE = :adt_date AND
    	     AMIF_DATA_SOURCE = 'U') A
ORDER BY RPT_SEQ_NO, AMIF_KIND_ID, AMIF_SETTLE_DATE
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 因為這張table可能會塞進grid，所以欄位名稱在撈取時改成和grid一樣
        /// </summary>
        /// <param name="as_date"></param>
        /// <returns></returns>
        public DataTable d_20110_amifu(DateTime as_date) {

            object[] parms = {
                ":as_date", as_date
            };
            
            string sql =
@"
SELECT A.*
FROM
    (SELECT U.AMIFU_DATE as AMIF_DATE,   
           U.AMIFU_KIND_ID as AMIF_KIND_ID,
           decode(trim(U.AMIFU_SETTLE_DATE),'000000','指數',trim(U.AMIFU_SETTLE_DATE)) as AMIF_SETTLE_DATE, 
           U.AMIFU_OPEN_PRICE as AMIF_OPEN_PRICE,   
           U.AMIFU_HIGH_PRICE as AMIF_HIGH_PRICE,   
           U.AMIFU_LOW_PRICE as AMIF_LOW_PRICE,   
           U.AMIFU_CLOSE_PRICE as AMIF_CLOSE_PRICE,   
           U.AMIFU_UP_DOWN_VAL as AMIF_UP_DOWN_VAL,   
           U.AMIFU_SETTLE_PRICE as AMIF_SETTLE_PRICE,   
           U.AMIFU_M_QNTY_TAL as AMIF_M_QNTY_TAL,   
           U.AMIFU_OPEN_INTEREST as AMIF_OPEN_INTEREST,   
           U.AMIFU_SUM_AMT as AMIF_SUM_AMT,   
           U.AMIFU_PROD_TYPE as AMIF_PROD_TYPE,   
           U.AMIFU_PROD_SUBTYPE as AMIF_PROD_SUBTYPE,   
           U.AMIFU_DATA_SOURCE as AMIF_DATA_SOURCE,
           decode(trim(U.AMIFU_KIND_ID),'GDF', U.AMIFU_EXCHANGE_RATE, '') as AMIF_EXCHANGE_RATE,
           U.AMIFU_INS_TIME as AMIF_INS_TIME,  
           U.AMIFU_KIND_ID as AMIF_PARAM_KEY,   
           0 as AMIF_STRIKE_PRICE, 
           ' ' as AMIF_PC_CODE,    
           TRIM(U.AMIFU_KIND_ID)||substr(U.AMIFU_SETTLE_DATE,5,2) as AMIF_PROD_ID,       
           rpt_seq as RPT_SEQ_NO,
           'I' as OP_TYPE,
           U.AMIFU_CLOSE_PRICE as AMIF_CLOSE_PRICE_Y,
           ' ' as AMIF_CLOSE_PRICE_Y_FLAG,
           to_char(U.AMIFU_DATE,'yyyy') as AMIF_YEAR,
           0 as AMIF_CLOSE_PRICE_ORIG,
           U.AMIFU_ERR_TEXT,
           NVL(R_OPEN_PRICE,U.AMIFU_OPEN_PRICE) as R_OPEN_PRICE,
           NVL(R_HIGH_PRICE,U.AMIFU_HIGH_PRICE) AS R_HIGH_PRICE,   
           NVL(R_LOW_PRICE,U.AMIFU_LOW_PRICE) AS R_LOW_PRICE,   
           NVL(R_CLOSE_PRICE,U.AMIFU_CLOSE_PRICE) AS R_CLOSE_PRICE  ,
           AMIFU_M_QNTY_TAL as R_M_QNTY_TAL,    
           AMIFU_OPEN_INTEREST as R_OPEN_INTEREST  ,   
           000000.0000 as R_UP_DOWN_VAL,
           0 as MTH_SEQ_NO,
           TRIM(rpt_grp) AS RPT_OSW_GRP
    FROM CI.AMIFU U,
        (select R.AMIFU_KIND_ID AS R_KIND_ID,
                R.AMIFU_OPEN_PRICE as R_OPEN_PRICE,
                R.AMIFU_HIGH_PRICE AS R_HIGH_PRICE,   
                R.AMIFU_LOW_PRICE AS R_LOW_PRICE,   
                R.AMIFU_CLOSE_PRICE AS R_CLOSE_PRICE  
            FROM ci.AMIFU R
            WHERE R.AMIFU_DATE = :as_date
            AND R.AMIFU_DATA_SOURCE = 'R') R,   
            (select RPT_VALUE as rpt_kind_id, RPT_VALUE_4 as rpt_grp, min(RPT.RPT_SEQ_NO) as rpt_seq
            from ci.RPT
            where RPT.RPT_TXD_ID = '20110'
            group by RPT_VALUE,RPT_VALUE_4  ) RP
    WHERE U.AMIFU_DATE = :as_date    
      and U.AMIFU_DATA_SOURCE = 'U'
      and U.AMIFU_KIND_ID = R_KIND_ID(+)
      and U.AMIFU_KIND_ID = rpt_kind_id(+)) A
ORDER BY RPT_SEQ_NO, AMIF_KIND_ID, AMIF_SETTLE_DATE
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_20110_i5f(DateTime as_date) {

            object[] parms = {
                ":as_date", as_date
            };

            string sql =
@"
SELECT A.*
FROM
    (SELECT U.AMIFU_DATE,   
         U.AMIFU_KIND_ID,   
         U.AMIFU_SETTLE_DATE,   
         U.AMIFU_OPEN_PRICE,   
         U.AMIFU_HIGH_PRICE,   
         U.AMIFU_LOW_PRICE,   
         U.AMIFU_CLOSE_PRICE,   
         U.AMIFU_UP_DOWN_VAL,   
         U.AMIFU_SETTLE_PRICE,   
         U.AMIFU_M_QNTY_TAL,   
         U.AMIFU_OPEN_INTEREST,   
         U.AMIFU_SUM_AMT,   
         U.AMIFU_PROD_TYPE,   
         U.AMIFU_PROD_SUBTYPE,   
         U.AMIFU_DATA_SOURCE,   
         U.AMIFU_EXCHANGE_RATE,   
         U.AMIFU_INS_TIME  ,  
         U.AMIFU_KIND_ID as AMIF_PARAM_KEY,   
         0 as AMIF_STRIKE_PRICE, 
         ' ' as AMIF_PC_CODE,    
         TRIM(U.AMIFU_KIND_ID)||substr(U.AMIFU_SETTLE_DATE,5,2) as AMIF_PROD_ID,       
         rpt_seq as RPT_SEQ_NO,
         'I' as OP_TYPE ,
         U.AMIFU_CLOSE_PRICE as AMIF_CLOSE_PRICE_Y,
         ' ' as AMIF_CLOSE_PRICE_Y_FLAG,
         to_char(U.AMIFU_DATE,'yyyy') as AMIF_YEAR,
         0 as AMIF_CLOSE_PRICE_ORIG,
         U.AMIFU_ERR_TEXT,
         NVL(R_OPEN_PRICE,U.AMIFU_OPEN_PRICE) as R_OPEN_PRICE,
         NVL(R_HIGH_PRICE,U.AMIFU_HIGH_PRICE) AS R_HIGH_PRICE,   
         NVL(R_LOW_PRICE,U.AMIFU_LOW_PRICE) AS R_LOW_PRICE,   
         NVL(R_CLOSE_PRICE,U.AMIFU_CLOSE_PRICE) AS R_CLOSE_PRICE  ,
         AMIFU_M_QNTY_TAL as R_M_QNTY_TAL,    
         AMIFU_OPEN_INTEREST as R_OPEN_INTEREST  ,   
         000000.0000 as R_UP_DOWN_VAL,
         0 as MTH_SEQ_NO,
         TRIM(rpt_grp) AS RPT_OSW_GRP
    FROM CI.AMIFU U,
        (select R.AMIFU_KIND_ID AS R_KIND_ID,
                R.AMIFU_OPEN_PRICE as R_OPEN_PRICE,
                R.AMIFU_HIGH_PRICE AS R_HIGH_PRICE,   
                R.AMIFU_LOW_PRICE AS R_LOW_PRICE,   
                R.AMIFU_CLOSE_PRICE AS R_CLOSE_PRICE  
           FROM ci.AMIFU R
          WHERE R.AMIFU_DATE = :as_date
            AND R.AMIFU_DATA_SOURCE = 'R') R,   
         (select RPT_VALUE as rpt_kind_id, RPT_VALUE_4 as rpt_grp, min(RPT.RPT_SEQ_NO) as rpt_seq
            from ci.RPT
           where RPT.RPT_TXD_ID = '20110'
           group by RPT_VALUE,RPT_VALUE_4  ) RP
   WHERE U.AMIFU_DATE = :as_date    
     and U.AMIFU_DATA_SOURCE = 'U'
     and U.AMIFU_KIND_ID = R_KIND_ID(+)
     and U.AMIFU_KIND_ID = rpt_kind_id(+)) A
ORDER BY RPT_SEQ_NO, AMIFU_KIND_ID, AMIFU_SETTLE_DATE
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_20110_y(DateTime adt_date) {

            object[] parms = {
                ":adt_date", adt_date
            };

            string sql =
@"
select AMIF_KIND_ID,
       decode(trim(AMIF_SETTLE_DATE),'000000','指數',trim(AMIF_SETTLE_DATE)) as AMIF_SETTLE_DATE,
       AMIF_CLOSE_PRICE,
       AMIF_DATE       
from ci.AMIF AMIF	 
where AMIF_DATE = :adt_date
  and AMIF_DATA_SOURCE = 'U'
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public string nextDay(DateTime ldt_date) {

            object[] parms = {
                ":ldt_date", ldt_date
            };

            string sql =
@"
Select TO_DATE(min(AI2_YMD),'yyyymmdd') as id_last_date
from ci.AI2
where AI2_YMD > to_char(:ldt_date,'yyyymmdd')
  and AI2_SUM_TYPE = 'D'
  and AI2_SUM_SUBTYPE = '1'
";
            DataTable dtResult = db.GetDataTable(sql, parms);
            string id_last_date = dtResult.Rows[0]["ID_LAST_DATE"].AsString();
            return id_last_date;

        }

        public DataTable W20110_filename() {

            string sql =
@"
SELECT MAX(CASE WHEN COD_ID = 'MARKET' THEN TRIM(COD_DESC) ELSE '' END) as ls_file,
       MAX(CASE WHEN COD_ID = 'OI' THEN TRIM(COD_DESC) ELSE '' END) as ls_file_oi
FROM CI.COD 
WHERE COD_TXN_ID = '20110' 
  AND COD_COL_ID = 'JPX'
";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// Lukas, 2019/1/30
        /// for W20110
        /// </summary>
        /// <returns></returns>
        public int RowCount() {

            string sql =
@"
SELECT count(*) as ll_found
FROM CI.RPT   
WHERE RPT_TXD_ID = '20110'
 AND  RPT_VALUE_2 = '000000'
";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult.Rows[0]["LL_FOUND"].AsInt();
        }

        /// <summary>
        /// for W20110
        /// </summary>
        /// <param name="ad_date"></param>
        /// <returns></returns>
        public string MinSettleDate(DateTime ad_date) {

            string ls_settle_date;
            object[] parms =
            {
                ":ad_date", ad_date
            };

            string sql =
@"
select min(AMIF_SETTLE_DATE) as ls_settle_date
from ci.AMIF
where AMIF_DATE = :ad_date
  and AMIF_KIND_ID = 'TXF' 
  and AMIF_DATA_SOURCE = 'T'
  and AMIF_MTH_SEQ_NO = 1
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            ls_settle_date = dtResult.Rows[0]["LS_SETTLE_DATE"].AsString();
            return ls_settle_date;
        }

        /// <summary>
        /// for W20110
        /// </summary>
        /// <param name="ldt_bef_date"></param>
        /// <param name="ldt_date"></param>
        /// <returns></returns>
        public DateTime MaxAMIF_DATE(DateTime ldt_bef_date, DateTime ldt_date) {

            DateTime rtnDate;
            object[] parms =
            {
                ":ldt_bef_date", ldt_bef_date,
                ":ldt_date", ldt_date,
            };

            string sql =
@"
SELECT max(AMIF_DATE) as ldt_date
  FROM ci.AMIF
WHERE AMIF_DATE >= :ldt_bef_date
  and AMIF_DATE < :ldt_date
  and AMIF_DATA_SOURCE = 'U'
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            rtnDate = dtResult.Rows[0]["LDT_DATE"].AsDateTime();
            return rtnDate;
        }

        /// <summary>
        /// for W20110
        /// </summary>
        /// <param name="ls_date"></param>
        /// <param name="ls_m"></param>
        /// <returns></returns>
        public decimal GetSettlePrice(string ls_date, string ls_m) {

            decimal ld_value = 0;
            object[] parms =
            {
                ":ls_date", ls_date,
                ":ls_m", ls_m
            };

            string sql =
@"
select STWD_SETTLE_PRICE
from ci.STWD
where STWD_YMD = :ls_date
  and STWD_SETTLE_DATE = :ls_m
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count == 0) {
                return ld_value;
            }
            else {
                ld_value = dtResult.Rows[0]["STWD_SETTLE_PRICE"].AsDecimal();
                return ld_value;
            }

        }

        /// <summary>
        /// Lukas, for W20110
        /// dddw_pdk_kind_id_f
        /// </summary>
        /// <returns>PDK_KIND_ID</returns>
        public DataTable ListAllF_20110() {

            string sql = @"
SELECT APDK_KIND_ID as PDK_KIND_ID 
FROM ci.APDK  
WHERE APDK_PROD_TYPE = 'F'    
GROUP BY APDK_KIND_ID
ORDER BY PDK_KIND_ID
";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public ResultData UpdateAMIF(DataTable inputData) {

            string tableName = "CI.AMIF";
            string keysColumnList = "AMIF_DATE, AMIF_PROD_ID";
            string insertColumnList = @"AMIF_DATE, 
                                        AMIF_KIND_ID, 
                                        AMIF_SETTLE_DATE, 
                                        AMIF_OPEN_PRICE, 
                                        AMIF_HIGH_PRICE, 
                                        AMIF_LOW_PRICE,   
                                        AMIF_CLOSE_PRICE, 
                                        AMIF_UP_DOWN_VAL,
                                        AMIF_SETTLE_PRICE, 
                                        AMIF_M_QNTY_TAL,    
                                        AMIF_OPEN_INTEREST,   
                                        AMIF_SUM_AMT,       
                                        AMIF_PROD_TYPE,   
                                        AMIF_PROD_SUBTYPE,   
                                        AMIF_DATA_SOURCE,
                                        AMIF_EXCHANGE_RATE,
                                        AMIF_M_TIME,  
                                        AMIF_PARAM_KEY,   
                                        AMIF_STRIKE_PRICE, 
                                        AMIF_PC_CODE,    
                                        AMIF_PROD_ID,       
                                        AMIF_YEAR, 
                                        AMIF_MTH_SEQ_NO,
                                        AMIF_OSW_GRP";
            string updateColumnList = insertColumnList;
            try {
                //update to DB
                return SaveForChanged(inputData, tableName, insertColumnList, updateColumnList, keysColumnList);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public ResultData UpdateData(DataTable inputData) {

            string sql = @"
SELECT AMIF_DATE, 
       AMIF_KIND_ID, 
       AMIF_SETTLE_DATE, 
       AMIF_OPEN_PRICE, 
       AMIF_HIGH_PRICE, 
       AMIF_LOW_PRICE,   
       AMIF_CLOSE_PRICE, 
       AMIF_UP_DOWN_VAL,
       AMIF_SETTLE_PRICE, 
       AMIF_M_QNTY_TAL,    
       AMIF_OPEN_INTEREST,   
       AMIF_SUM_AMT,       
       AMIF_PROD_TYPE,   
       AMIF_PROD_SUBTYPE,   
       AMIF_DATA_SOURCE,
       AMIF_EXCHANGE_RATE,
       AMIF_M_TIME,  
       AMIF_PARAM_KEY,   
       AMIF_STRIKE_PRICE, 
       AMIF_PC_CODE,    
       AMIF_PROD_ID,       
       AMIF_YEAR, 
       AMIF_MTH_SEQ_NO,
       AMIF_OSW_GRP
FROM CI.AMIF
";
            return db.UpdateOracleDB(inputData, sql);
        }

        #region SP

        public ResultData sp_U_gen_H_TDT(DateTime ldt_date, string ls_prod_type) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date),
            new DbParameterEx(":ls_prod_type",ls_prod_type)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "CI.sp_U_gen_H_TDT";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);
            
            return reResult;
        }

        public ResultData sp_U_stt_H_AI2_Day(DateTime ldt_date, string ls_prod_type) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date),
            new DbParameterEx(":ls_prod_type",ls_prod_type)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "CI.sp_U_stt_H_AI2_Day";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);

            return reResult;
        }

        public ResultData sp_U_stt_H_AI2_Month(DateTime ldt_date, string ls_prod_type) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date),
            new DbParameterEx(":ls_prod_type",ls_prod_type)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "CI.sp_U_stt_H_AI2_Month";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);

            return reResult;
        }

        public ResultData sp_H_stt_AI3(DateTime ldt_date) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "CI.sp_H_stt_AI3";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);

            return reResult;
        }

        public ResultData sp_H_gen_AI6(DateTime ldt_date) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "CI.sp_H_gen_AI6";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);

            return reResult;
        }

        public ResultData sp_H_upd_AA3(DateTime ldt_date) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date.ToString("yyyyMMdd"))
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "CI.sp_H_upd_AA3";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);

            return reResult;
        }

        public ResultData sp_H_gen_H_AI8(DateTime ldt_date) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date.ToString("yyyyMMdd"))
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "CI.sp_H_gen_H_AI8";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);

            return reResult;
        }

        #endregion
    }
}
