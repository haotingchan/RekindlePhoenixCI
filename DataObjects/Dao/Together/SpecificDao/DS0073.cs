using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
    public class DS0073 {
        private Db db;

        public DS0073() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetMarginData() {
            string sql = @"select  * from cfo.SPAN_MARGIN";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public DataTable GetPeriodData(string module, string user_Id) {
            object[] parms = {
                "@module", module,
                "@userId",user_Id
            };

            string sql = @"select  * from cfo.SPAN_PERIOD
                                where span_period_module = :module
                                and span_period_user_id = :userId";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable GetMarginUserData() {
            string sql = @"select distinct
                                    trim(span_margin_spn) as span_margin_spn,
                                    trim(span_margin_pos) as span_margin_pos,
                                    trim(span_margin_spn_path) as span_margin_spn_path
                                    from cfo.SPAN_MARGIN";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }
    }
}
