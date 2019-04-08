using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/1
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D30080: DataGate {

        /// <summary>
        /// 主要資料 AI2
        /// </summary>
        /// <param name="as_prod_type">F/O</param>
        /// <param name="as_symd">yyyyMMdd</param>
        /// <param name="as_eymd">yyyyMMdd</param>
        /// <param name="as_param_key">%</param>
        /// <param name="as_kind_id">%</param>
        /// <param name="as_underlying_market">%</param>
        /// <returns></returns>
        public DataTable d_30080(string as_prod_type,string as_symd, string as_eymd,  
                                 string as_param_key, string as_kind_id, string as_underlying_market) {

            object[] parms = {
                ":as_prod_type",as_prod_type,
                ":as_symd", as_symd,
                ":as_eymd", as_eymd,
                ":as_param_key",as_param_key,
                ":as_kind_id",as_kind_id,
                ":as_underlying_market",as_underlying_market
            };

            string sql =
@"
select AI2_YMD,substr(AI2_KIND_ID,1,2)||:as_prod_type as AI2_KIND_ID,
       sum(AI2_M_QNTY) as AI2_M_QNTY,
       sum(AI2_OI) as AI2_OI
  from ci.AI2
 where AI2_SUM_TYPE = 'D' 
   and AI2_YMD >= :as_symd
   and AI2_YMD <= :as_eymd
   and AI2_SUM_SUBTYPE = '4'
   and AI2_PROD_TYPE = :as_prod_type
   and AI2_PROD_SUBTYPE = 'S'
   AND AI2_PARAM_KEY LIKE :as_param_key
   AND AI2_KIND_ID LIKE :as_kind_id
   and AI2_UNDERLYING_MARKET LIKE :as_underlying_market
group by AI2_YMD,substr(AI2_KIND_ID,1,2)
order by ai2_ymd, ai2_kind_id 
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 抓日期範圍
        /// </summary>
        /// <param name="as_prod_type">F/O</param>
        /// <param name="as_symd">yyyyMMdd</param>
        /// <param name="as_eymd">yyyyMMdd</param>
        /// <param name="as_param_key">%</param>
        /// <param name="as_kind_id">%</param>
        /// <param name="as_underlying_market">%</param>
        /// <returns></returns>
        public DataTable d_30080_date(string as_prod_type, string as_symd, string as_eymd,
                         string as_param_key, string as_kind_id, string as_underlying_market) {

            object[] parms = {
                ":as_prod_type",as_prod_type,
                ":as_symd", as_symd,
                ":as_eymd", as_eymd,
                ":as_param_key",as_param_key,
                ":as_kind_id",as_kind_id,
                ":as_underlying_market",as_underlying_market
            };

            string sql =
@"
select AI2_YMD
  from ci.AI2
 where AI2_SUM_TYPE = 'D' 
   and AI2_YMD >= :as_symd
   and AI2_YMD <= :as_eymd
   and AI2_SUM_SUBTYPE = '4'
   and AI2_PROD_TYPE = :as_prod_type
   and AI2_PROD_SUBTYPE = 'S'
   and AI2_PARAM_KEY like :as_param_key
   and AI2_KIND_ID like :as_kind_id
   and AI2_UNDERLYING_MARKET like :as_underlying_market
group by AI2_YMD
order by ai2_ymd
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 商品清單
        /// </summary>
        /// <param name="as_prod_type">F/O</param>
        /// <param name="as_symd">yyyyMMdd</param>
        /// <param name="as_eymd">yyyyMMdd</param>
        /// <param name="as_param_key">%</param>
        /// <param name="as_kind_id">%</param>
        /// <param name="as_underlying_market">%</param>
        /// <returns></returns>
        public DataTable d_30080_sort(string as_prod_type, string as_symd, string as_eymd,
                 string as_param_key, string as_kind_id, string as_underlying_market) {

            object[] parms = {
                ":as_prod_type",as_prod_type,
                ":as_symd", as_symd,
                ":as_eymd", as_eymd,
                ":as_param_key",as_param_key,
                ":as_kind_id",as_kind_id,
                ":as_underlying_market",as_underlying_market
            };

            string sql =
@"
select A.AI2_KIND_ID, A.AI2_M_QNTY, A.AI2_OI, rownum as cp_seq_no, A.PDK_NAME
from
(select substr(AI2_KIND_ID,1,2)||:as_prod_type as AI2_KIND_ID,
       sum(AI2_M_QNTY) AS AI2_M_QNTY,
       sum(AI2_OI) AS AI2_OI,
       MIN(APDK_NAME) as PDK_NAME
  from ci.AI2,ci.apdk
 where AI2_SUM_TYPE = 'D' 
   and AI2_YMD >= :as_symd
   and AI2_YMD <= :as_eymd
   and AI2_SUM_SUBTYPE = '4'
   and AI2_PROD_TYPE = :as_prod_type
   and AI2_PROD_SUBTYPE = 'S'
   and AI2_PARAM_KEY like :as_param_key
   and AI2_KIND_ID like :as_kind_id
   and AI2_UNDERLYING_MARKET like :as_underlying_market
   and AI2_KIND_ID = APDK_KIND_ID
group by substr(AI2_KIND_ID,1,2))A
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
