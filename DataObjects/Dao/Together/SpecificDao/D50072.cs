using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {

    public class D50072 {

        private Db db;

        public D50072() {

            db = GlobalDaoSetting.DB;

        }

        public DataTable ListData(string as_symd, string as_eymd) {

            object[] parms = {
                "@as_symd", as_symd,
                "@as_eymd", as_eymd
            };

            string sql = @"select MC_DATE, FUT_ID, ACCTNO, PARAM_KEY, VALID_CNT, VALID_TIME, VALID_RESULT, QTY, NQTY, PROD_TYPE,
                        DENSE_RANK() OVER (PARTITION BY MC_DATE, PARAM_KEY ORDER BY MC_DATE, VALID_RESULT desc, QTY-NQTY desc) as DRANK
                        from ci.R_MARKET_RWM
                        where MC_DATE between @as_symd and @as_eymd
                        order by MC_DATE, FUT_ID, ACCTNO, PARAM_KEY";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable ListData_etf(string as_sym, string as_eym) {

            object[] parms = {
                "@as_sym", as_sym,
                "@as_eym", as_eym
            };

            string sql = @"select * from reward.R_WEIGHT_MONTHLY 
                           where mc_month >= @as_sym
                           and mc_month <= @as_eym
                           order by fut_id,
                                    prod_type,
                                    param_key";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable ListData_mtx(string as_symd, string as_eymd) {

            object[] parms = {
                "@as_symd", as_symd,
                "@as_eymd", as_eymd
            };

            string sql = @"SELECT REWARD.RWD_AUCTION_ACCUM.YYMMDD,   
                           REWARD.RWD_AUCTION_ACCUM.FCM_NO,   
                           REWARD.RWD_AUCTION_ACCUM.ACC_NO,   
                           REWARD.RWD_AUCTION_ACCUM.AUCTION_RATE,   
                           REWARD.RWD_AUCTION_ACCUM.BUY_KEEP_TIME,   
                           REWARD.RWD_AUCTION_ACCUM.SELL_KEEP_TIME,   
                           REWARD.RWD_AUCTION_ACCUM.MATCH_RATE  
                           FROM REWARD.RWD_AUCTION_ACCUM   
	                       WHERE YYMMDD BETWEEN @as_symd AND @as_eymd
                           order by yymmdd, fcm_no, acc_no";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable ListData_txf(string as_sym, string as_eym) {

            object[] parms = {
                "@as_sym", as_sym,
                "@as_eym", as_eym
            };

            string sql = @"select MC_MONTH, FUT_ID, ACCTNO, PROD_TYPE, PARAM_KEY, WEIGHT, TO_CHAR(W_TIME,'YYYY/fmMM/fmDD HH24:mm:ss:ff') as W_TIME 
                           from reward.R_WEIGHT_MONTHLY_TXF
                           where mc_month >= @as_sym
                           and mc_month <= @as_eym
                           order by fut_id, param_key";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
