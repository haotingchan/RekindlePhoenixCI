using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
    public class DS0020
    {
        private Db db;

        public DS0020() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetSP4Data(string ym_date) {
            object[] parms = {
                "@ym_date", ym_date
            };

            string sql = @"select  *
                                from ci.sp4
                                where sp4_date=to_date(:ym_date, 'YYYY/MM/DD')
                                order by sp4_date";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable GetSP5Data(string ym_date) {
            object[] parms = {
                "@ym_date", ym_date
            };

            string sql = @"select  *
                                from ci.sp5
                                where sp5_date=to_date(:ym_date, 'YYYY/MM/DD')
                                order by sp5_date";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
