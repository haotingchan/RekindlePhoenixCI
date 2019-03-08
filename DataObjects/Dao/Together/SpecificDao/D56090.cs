using BusinessObjects;
using BusinessObjects.Enums;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D56090 {
        private Db db;

        public D56090() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetData() {
            string sql = @"
                        SELECT * FROM CI.FEETDCC
                        order by FEETDCC_YM";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public int DeleteByYM(string feetdcc_ym) {
            object[] parms = {
                "@feetdcc_ym", feetdcc_ym
            };

            try {
                string sql = @"DELETE FROM CI.FEETDCC
                           WHERE FEETDCC_YM = :feetdcc_ym";
                return db.ExecuteSQL(sql, parms);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public ResultData updateData(DataTable inputData) {

            string sql = @"SELECT 
feetdcc_ym,
feetdcc_fcm_no,
feetdcc_kind_id, 
feetdcc_disc_qnty, 
feetdcc_disc_rate, 
feetdcc_org_ar, 
feetdcc_disc_amt, 
feetdcc_w_user_id, 
feetdcc_w_time, 
feetdcc_acc_no, 
feetdcc_session
from ci.feetdcc";

            return db.UpdateOracleDB(inputData, sql);

        }
    }
}
