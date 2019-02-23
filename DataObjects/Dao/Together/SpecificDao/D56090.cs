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

        public ResultStatus DeleteByYM(string feetdcc_ym) {
            object[] parms = {
                "@feetdcc_ym", feetdcc_ym
            };

            string sql = @"DELETE FROM CI.FEETDCC
                           WHERE FEETDCC_YM = :feetdcc_ym";
            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult >= 0) {
                return ResultStatus.Success;
            }
            else {
                throw new Exception("刪除失敗");
            }
        }
    }
}
