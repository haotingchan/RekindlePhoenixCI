using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/27
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D30100:DataGate {

        public DataTable d_30100(DateTime as_sdate, DateTime as_edate, 
                                        string as_ks_fcm_no, string as_in_fcm_no, string as_market_code) {
            object[] parms = {
                ":as_sdate", as_sdate,
                ":as_edate", as_edate,
                ":as_ks_fcm_no", as_ks_fcm_no,
                ":as_in_fcm_no", as_in_fcm_no,
                ":as_market_code", as_market_code
            };

            string sql =
@"
SELECT   KSW_DATE,   
         TO_CHAR(KSW_W_TIME,'YYYY/MM/DD HH24:MI:SS.XFF') AS KSW_W_TIME,   
         KSW_FCM_NO,   
         KSW_SOURCE_FCM_NO,   
         KSW_SESSION_ID, 

         case when KSW_TYPE = '1' then '期貨商' else '線路' end as KSW_TYPE,   
         case when KSW_CMD = '1' then '啟動' else '恢復' end as KSW_CMD,   
         case when KSW_FILE_SYSTEM = 'F' then '期貨' else '選擇權' end ||
         case KSW_TRADE_TYPE when  'N' then '一般'   
                             when 'S' then '單式鉅額'
                             when 'C' then '組合鉅額' end as SYS_TYPE,
         KSW_KILL_COUNT, 
         KSW_GW_SYSTEM,   

         KSW_GW_FCM_NO,   
         KSW_GW_SESSION_ID,   
         KSW_STATUS,
         case when KSW_MARKET_CODE = '0'    then '一般'  else '盤後' end as MARKET_CODE
    FROM CI.KSW  
   WHERE KSW_DATE >= :as_sdate
     AND KSW_DATE <= :as_edate  
     AND KSW_GW_FCM_NO like :as_in_fcm_no 
     AND KSW_FCM_NO like :as_ks_fcm_no
     AND KSW_MARKET_CODE LIKE :as_market_code
   ORDER BY ksw_date, ksw_market_code, ksw_w_time, ksw_fcm_no, ksw_source_fcm_no, ksw_session_id, ksw_seq_no Desc, KSW_GW_SYSTEM, SYS_TYPE
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
