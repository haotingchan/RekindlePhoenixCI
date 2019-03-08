using BusinessObjects;
using BusinessObjects.Enums;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class D51010
    {
        private Db db;

        public D51010()
        {

            db = GlobalDaoSetting.DB;
        }

        public DataTable GetData(string adt_sdate , string adt_edate) {
            object[] parms = {
                "@adt_sdate", adt_sdate,
                "@adt_edate", adt_edate
            };

            string sql = @"select  
                                dts_date,
                                dts_date_type,                              
                                dts_work
                                from ci.dts
                                where dts_date>=to_date(:adt_sdate, 'YYYY-MM-DD')
                                and dts_date<=to_date(:adt_edate, 'YYYY-MM-DD')
                                order by dts_date";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public ResultData UpdateData(DataTable inputData) {
            string sql = @"select  
                                dts_date,
                                dts_date_type,                              
                                dts_work
                                from ci.dts";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
