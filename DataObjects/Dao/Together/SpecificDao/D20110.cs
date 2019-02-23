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

        #region SP

        public ResultData sp_U_gen_H_TDT(DateTime ldt_date, string ls_prod_type) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date),
            new DbParameterEx(":ls_prod_type",ls_prod_type)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "sp_U_gen_H_TDT";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);
            
            return reResult;
        }

        public ResultData sp_U_stt_H_AI2_Day(DateTime ldt_date, string ls_prod_type) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date),
            new DbParameterEx(":ls_prod_type",ls_prod_type)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "sp_U_stt_H_AI2_Day";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);

            return reResult;
        }

        public ResultData sp_U_stt_H_AI2_Month(DateTime ldt_date, string ls_prod_type) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date),
            new DbParameterEx(":ls_prod_type",ls_prod_type)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "sp_U_stt_H_AI2_Month";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);

            return reResult;
        }

        public ResultData sp_H_stt_AI3(DateTime ldt_date) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "sp_H_stt_AI3";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);

            return reResult;
        }

        public ResultData sp_H_gen_AI6(DateTime ldt_date) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "sp_H_gen_AI6";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);

            return reResult;
        }

        public ResultData sp_H_upd_AA3(DateTime ldt_date) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date.ToString("yyyyMMdd"))
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "sp_H_upd_AA3";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);

            return reResult;
        }

        public ResultData sp_H_gen_H_AI8(DateTime ldt_date) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx(":ldt_date",ldt_date.ToString("yyyyMMdd"))
            //new DbParameterEx("RETURNPARAMETER",null)
         };

            string sp = "sp_H_gen_H_AI8";

            ResultData reResult = db.ExecuteStoredProcedure(sp, parms, true);

            return reResult;
        }

        #endregion
    }
}
