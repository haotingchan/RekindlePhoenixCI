using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/25
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D20231:DataGate {

        public DataTable d_20231(string as_ymd) {

            object[] parms = {
                ":as_ymd",as_ymd
            };

            string sql =
@"
SELECT   ROW_NUMBER() over (ORDER BY apdk_kind_grp2, pls4_kind_id2) as compute_1,
         PLS4_SID,   
         PLS4_KIND_ID2,   
         PLS4_YMD,   
         PLS4_FUT,   
         PLS4_OPT,   
         PLS4_PDK_YMD,   
         PLS4_W_TIME,   
         PLS4_W_USER_ID,   
         ' ' as OP_TYPE,   
         PLS4_STATUS_CODE,   
         PLS4_PID  ,
         APDK_KIND_GRP2 as APDK_KIND_GRP2
    FROM CI.PLS4 ,
       (select APDK_KIND_ID2,APDK_KIND_GRP2 from ci.APDK group by APDK_KIND_ID2,APDK_KIND_GRP2) A
   WHERE PLS4_YMD = :as_ymd    
    and PLS4_KIND_ID2 = APDK_KIND_ID2(+)
   ORDER BY apdk_kind_grp2, pls4_kind_id2
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
