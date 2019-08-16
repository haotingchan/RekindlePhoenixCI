using System;
using System.Data;

/// <summary>
/// ken,2019/3/26
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 延長交易時間商品13:45後交易量比重
   /// </summary>
   public class D30770 : DataGate {

      /// <summary>
      /// get data, return 10fields, AM11_YMD/AM11_KIND_ID/SEQ_NO/M_QNTY/TOT_QNTY/DAY_CNT/APDK_NAME/cp_grp_m_qnty/cp_grp_tot_qnty/cp_max_seq_no
      /// </summary>
      /// <param name="as_prod_type"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_osw_grp"></param>
      /// <returns></returns>
      public DataTable d_30770(string as_prod_type , string as_sum_type ,
                                string as_symd , string as_eymd , string as_osw_grp = "%") {

         object[] parms = {
                ":as_prod_type", as_prod_type,
                ":as_sum_type", as_sum_type,
                ":as_symd", as_symd,
                ":as_eymd", as_eymd,
                ":as_osw_grp", as_osw_grp
            };

         string sql = @"
SELECT T.AM11_YMD as AM11_YMD,
    T.AM11_KIND_ID2 as AM11_KIND_ID,
    SEQ_NO,
    M_QNTY,
    TOT_QNTY,

    DAY_CNT,
    APDK_NAME,
    Sum( M_QNTY ) Over( partition by T.AM11_YMD ) as cp_grp_m_qnty,
    Sum( TOT_QNTY ) Over( partition by T.AM11_YMD ) as cp_grp_tot_qnty,
    Max( SEQ_NO ) Over( partition by T.AM11_YMD ) as cp_max_seq_no 
FROM 
    (SELECT APDK_PROD_TYPE,APDK_KIND_ID2,MIN(APDK_NAME) AS APDK_NAME from ci.APDK group by APDK_PROD_TYPE,APDK_KIND_ID2),
    --依交易量大小排序
    (SELECT AM11_PROD_TYPE as SEQ_PROD_TYPE,AM11_KIND_ID2 as SEQ_KIND_ID2,
            ROWNUM as SEQ_NO
        FROM 
            (SELECT AM11_PROD_TYPE,AM11_KIND_ID2
                FROM ci.AM11
                WHERE AM11_SUM_TYPE = :as_sum_type
                AND AM11_YMD >= :as_symd
                AND AM11_YMD <= :as_eymd 
                AND AM11_PROD_TYPE LIKE :as_prod_type
                GROUP BY AM11_PROD_TYPE,AM11_KIND_ID2
                order by AM11_PROD_TYPE,AM11_KIND_ID2)) P,
    --1345-1615交易量  
    (SELECT AM11_YMD,AM11_PROD_TYPE,AM11_KIND_ID2,sum(AM11_M_QNTY) / 2 as M_QNTY
        FROM ci.AM11
        WHERE AM11_SUM_TYPE = :as_sum_type
        AND AM11_OSW_GRP like :as_osw_grp
        AND AM11_OSW_GRP <> '1'
        AND AM11_YMD >= :as_symd
        AND AM11_YMD <= :as_eymd
        AND AM11_PROD_TYPE LIKE :as_prod_type
        GROUP BY AM11_YMD,AM11_PROD_TYPE,AM11_KIND_ID2) G5,
    --全部交易量  
    (SELECT AM11_YMD,AM11_PROD_TYPE,AM11_KIND_ID2,sum(AM11_M_QNTY) / 2  as TOT_QNTY,MAX(AM11_DAY_COUNT) as DAY_CNT
        FROM ci.AM11
        WHERE  AM11_SUM_TYPE = :as_sum_type
        AND AM11_YMD >= :as_symd
        AND AM11_YMD <= :as_eymd
        AND AM11_PROD_TYPE LIKE :as_prod_type
        GROUP BY AM11_YMD,AM11_PROD_TYPE,AM11_KIND_ID2) T
WHERE SEQ_KIND_ID2 = T.AM11_KIND_ID2
AND SEQ_PROD_TYPE = APDK_PROD_TYPE
AND SEQ_KIND_ID2 = APDK_KIND_ID2
AND SEQ_PROD_TYPE = T.AM11_PROD_TYPE
AND SEQ_KIND_ID2 = T.AM11_KIND_ID2
AND T.AM11_PROD_TYPE = G5.AM11_PROD_TYPE
AND T.AM11_KIND_ID2 = G5.AM11_KIND_ID2
AND T.AM11_YMD = G5.AM11_YMD
order by am11_ymd , am11_kind_id , seq_no
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      public string GetOcfgTimePeriod() {
         string sql = @"
select 
    to_char(min(ocfg_last_close_time),'hh24:mi'),
    to_char(max(ocfg_close_time),'hh24:mi') ,
    ( to_char(min(ocfg_last_close_time),'hh24:mi') || ' - ' || to_char(max(ocfg_close_time),'hh24:mi') ) as cp_display
from ci.ocfg
where ocfg_market_code = 0 and ocfg_osw_grp>1
";
         string res = "";
         DataTable dtResult = db.GetDataTable(sql , null);

         try {
            if (dtResult.Rows.Count > 0)
               res = dtResult.Rows[0]["cp_display"].ToString();
         } catch (Exception ex) {

            throw ex;
         }

         return res;

      }

   }
}
