using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// ken,2019/3/12
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    /// <summary>
    /// 保證金調整影響分析說明
    /// </summary>
    public class D40120 {

        private Db db;

        public D40120() {

            db = GlobalDaoSetting.DB;

        }

        /// <summary>
        /// get mg1/mg2/mgt2 data, return 9 fields
        /// </summary>
        /// <param name="ad_date"></param>
        /// <returns></returns>
        public DataTable ListData(DateTime ad_date) {

            object[] parms = {
                ":ad_date", ad_date
            };


            string sql = @"
SELECT MG2_DATE,   
         MG2_KIND_ID,   
         MG2_VALUE_DATE,   
         MG1_CHANGE_RANGE,   
         MGT2_KIND_ID_OUT,   

         MGT2_ABBR_NAME,   
         MGT2_PROD_TYPE,   
         MGT2_SEQ_NO,   
         MG1_CHANGE_COND  
    FROM CI.MG1,   
         CI.MG2,   
         CI.MGT2 
   WHERE MG2_KIND_ID = MG1_KIND_ID   
     AND MG2_DATE = MG1_DATE 
     AND MG2_KIND_ID = MGT2_KIND_ID
     AND MG1_TYPE in ( '-','A' ) 
     AND MG2_VALUE_DATE = :ad_date     
order by mg2_date , mgt2_seq_no , mg2_kind_id
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// get mgt2 data, return mgt2_group_kind_id/mgt2_prod_type/mgt2_kind_id/mgt2_kind_id_out/mgt2_abbr_name/mgt2_name
        /// </summary>
        /// <param name="as_group">if null, use '%'</param>
        /// <returns></returns>
        public DataTable ListMgt2ByKindId(string as_group) {

            object[] parms = {
                ":as_group", as_group
            };


            string sql = @"
select 
    mgt2_group_kind_id,
    mgt2_prod_type,
    mgt2_kind_id,
    mgt2_kind_id_out,
    mgt2_abbr_name,
    mgt2_name
from ci.mgt2
where trim(nvl(mgt2_group_kind_id,'%')) = :as_group
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

    }
}
