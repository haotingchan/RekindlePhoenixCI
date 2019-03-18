using OnePiece;
using System;
using System.Data;

/// <summary>
/// ken,2019/3/12
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    /// <summary>
    /// SPAN參數調整影響分析說明
    /// </summary>
    public class D40122 {

        private Db db;

        public D40122() {

            db = GlobalDaoSetting.DB;

        }

        /// <summary>
        /// get sp1/sp2/spt1 data, return 11 fields
        /// </summary>
        /// <param name="as_date"></param>
        /// <returns></returns>
        public DataTable ListData(DateTime as_date) {

            object[] parms = {
                ":as_date", as_date
            };


            string sql = @"
select
    sp2.sp2_date,
    sp2.sp2_type,
    sp2.sp2_kind_id1,
    sp2.sp2_kind_id2,
    sp2.sp2_value_date,

    sp1.sp1_change_range,
    spt1.spt1_kind_id1_out,
    spt1.spt1_kind_id2_out,
    spt1.spt1_abbr_name,
    spt1.spt1_seq_no,
    spt1.spt1_com_id
from sp1,sp2,spt1
where sp2.sp2_date=sp1.sp1_date
and sp2.sp2_type=sp1.sp1_type
and sp2.sp2_kind_id1=sp1.sp1_kind_id1
and sp2.sp2_kind_id2=sp1.sp1_kind_id2
and sp2.sp2_kind_id1=spt1.spt1_kind_id1
and sp2.sp2_kind_id2=spt1.spt1_kind_id2
and sp2.sp2_value_date = :as_date
order by sp2_value_date , sp2_type , spt1_seq_no
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }


        /// <summary>
        /// get mgt1_adjust_rate from mgt1
        /// </summary>
        /// <param name="as_prod_type">SV/SD/SS</param>
        /// <returns></returns>
        public string GetRate(string as_prod_type = "SV") {

            object[] parms = {
                ":as_prod_type", as_prod_type
            };


            string sql = @"
select mgt1_adjust_rate as ld_value 
from ci.mgt1 
where mgt1_prod_type = :as_prod_type
";

            string res = db.ExecuteScalar(sql, CommandType.Text, parms);

            return res;
        }
    }
}
