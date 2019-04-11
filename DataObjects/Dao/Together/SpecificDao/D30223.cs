using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/9
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D30223:DataGate {
        public DataTable d_30223(string as_ymd, string as_to_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_to_ymd", as_to_ymd
            };

            string sql =
@"
SELECT PLS2_YMD,   
         PLS2_EFFECTIVE_YMD,   
         PLS2_KIND_ID2 as PLS2_KIND_ID2,   
         PLS2_FUT as  PLS2_FUT,   
         PLS2_OPT as PLS2_OPT,   
         PLS2_SID,   
         PLS2_LEVEL,   
         PLS2_NATURE,   
         PLS2_LEGAL,   
         PLS2_999,   
         PLS2_PREV_LEVEL,
         PLS2_PREV_NATURE,
         PLS2_PREV_LEGAL,
         PLS2_PREV_999,
         PLS2_LEVEL_ADJ,   
         PLS2_W_TIME,   
         PLS2_W_USER_ID,  
         NVL(APDK_NAME,TFXMS_SNAME) AS APDK_NAME,
         PLS2_KIND_GRP2 as APDK_KIND_GRP2
   from   CI.PLS2,
         --現貨名稱
         ci.TFXMS,
        (select APDK_KIND_ID2,nvl(max(case when APDK_PROD_TYPE = 'F' then APDK_NAME else '' end),max(case when APDK_PROD_TYPE = 'O' then APDK_NAME else '' end)) AS APDK_NAME 
           from ci.APDK GROUP BY APDK_KIND_ID2) P ,
        (select MAX(PLS2_YMD) as MAX_YMD,PLS2_KIND_ID2 as MAX_KIND_ID2 from ci.PLS2 
       WHERE PLS2_YMD >= :as_ymd
            and PLS2_YMD <= :as_to_ymd
          group by PLS2_KIND_ID2) 
   WHERE PLS2_YMD =  MAX_YMD
      and PLS2_KIND_ID2 = MAX_KIND_ID2
      and PLS2_KIND_ID2 = APDK_KIND_ID2
      and PLS2_SID = TFXMS_SID(+)
   ORDER BY apdk_kind_grp2, pls2_kind_id2, pls2_effective_ymd 
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
