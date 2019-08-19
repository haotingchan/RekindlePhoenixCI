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
select 
   to_date(mgd2_ymd,'YYYYMMDD')as mgd2_ymd,
   mgd2_kind_id,
   to_date(mgd2_issue_begin_ymd,'YYYYMMDD') as mgd2_issue_begin_ymd,
   mgd2_adj_rate,
   mgt2_kind_id_out,   
   mgt2_abbr_name,   
   mgt2_prod_type,   
   mgt2_seq_no,   
   mg1_change_cond
from 
   ci.mgd2,   
   ci.mgt2,
   ci.mg1_3m
where mgd2_issue_begin_ymd = to_char(:ad_date,'YYYYMMDD')
and mgd2_prod_type = mgt2_prod_type
and mgd2_ab_type in ('-','A')
and mgd2_kind_id = mgt2_kind_id
and mgd2_ymd = mg1_ymd
and nvl(mgd2_adj_rsn,'S') = mg1_model_type
and mgd2_kind_id = mg1_kind_id
and mgd2_ab_type = mg1_ab_type
order by mgd2_ymd , mgt2_seq_no , mgd2_kind_id 
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

      /// <summary>
      /// get mg1/mg2/mgt2 data, return 9 fields
      /// </summary>
      /// <param name="ad_date"></param>
      /// <returns></returns>
      public DataTable ListData2(DateTime ad_date) {

         object[] parms = {
                ":ad_date", ad_date
            };

         string sql = @"
SELECT MGD2_KIND_ID,
APDK_NAME,
MGD2_CM,
MGD2_CUR_CM,
MGD2_IM,
MGD2_CUR_IM,
MGD2_ADJ_RATE,
--MGD2_IM_RATE無條件進位到小數點第三位
ceil(((MGD2_IM-MGD2_CUR_IM)/NULLIF(MGD2_CUR_IM, 0))* 1000)/1000  AS MGD2_IM_RATE
FROM ci.MGD2,ci.APDK
WHERE MGD2_ISSUE_BEGIN_YMD = to_char(:ad_date,'YYYYMMDD')
AND MGD2_KIND_ID = APDK_KIND_ID
";

         DataTable dtResult = db.GetDataTable(sql , parms);

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
