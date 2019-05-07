using OnePiece;
using System.Data;

/// <summary>
/// ken,2019/1/2
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    /// <summary>
    /// 交易經手費收費明細表
    /// </summary>
    public class D55020 {

        private Db db;

        public D55020() {

            db = GlobalDaoSetting.DB;

        }

        /// <summary>
        /// List ABRK+FEETRD data 依照期貨商別
        /// </summary>
        /// <param name="startDate">起始月份,可空白,format=yyyymm</param>
        /// <param name="endDate">結束月份,可空白,format=yyyymm</param>
        /// <param name="startFcmNo">期貨商起始編號,可空白</param>
        /// <param name="endFcmNo">期貨商結束編號,可空白</param>
        /// <param name="prodId">商品條件-ID</param>
        /// <returns></returns>
        public DataTable ListAll(string startDate,
                                    string endDate,
                                    string startFcmNo = "",
                                    string endFcmNo = "",
                                    string prodId= "全部") {

            object[] parms = {
                ":as_sym", startDate,
                ":as_eym", endDate
            };
                        
            string filter = "";
            if (startFcmNo.Trim().Length > 0)
                filter += string.Format(" and FEETRD_FCM_NO >= '{0}'",startFcmNo);
            if (endFcmNo.Trim().Length > 0)
                filter += string.Format(" and FEETRD_FCM_NO <= '{0}'", endFcmNo);

            switch (prodId) {
                case "全部":
                case "":
                case "ALL":
                    //
                    break;
                case "期貨":
                case "F":
                case "FUT":
                    filter += " and APDK_PROD_TYPE = 'F'";
                    break;
                case "選擇權":
                case "O":
                case "OPT":
                    filter += " and APDK_PROD_TYPE = 'O'";
                    break;
                default:
                    //ken,簡易防止sql injection(基本上這兩個值不應該從UI傳進來)
                    string tmp = prodId.Length > 20 ? prodId.Substring(0, 20) : prodId;
                    prodId = tmp.Replace("'", "").Replace("--", "").Replace(";", "");
                    //ken,原本PB寫APDK_PARAM_KEY,肯定是錯的,調整為trim(FEETRD_KIND_ID)
                    //filter += " and APDK_PARAM_KEY = '" + prodId + "'";
                    filter += " and trim(FEETRD_KIND_ID) = '" + prodId + "'";
                    break;
            }

            //ken,原本SQL有誤,導致商品條件=期貨 or 選擇權 絕對出不來,我修改SQL語法
            string sql = string.Format(@"
SELECT APDK_PROD_TYPE,
   FEETRD_FCM_NO,
   NVL(ABRK_NAME,'') as BRK_ABBR_NAME,
   FEETRD_KIND_ID as sort_kind_id,
   (CASE WHEN FEETRD_KIND_ID = 'TXW    ' THEN ' └─'||trim(FEETRD_KIND_ID) ELSE FEETRD_KIND_ID END) AS FEETRD_KIND_ID,   
   FEETRD_M_QNTY,
   FEETRD_AR,
   FEETRD_MK_DISC_AMT,
   FEETRD_OTH_DISC_AMT,
   FEETRD_REC_AMT,
   FEETRD_UPD_TIME
FROM ci.ABRK,
   (SELECT APDK.APDK_PROD_TYPE,
       FEETRD_FCM_NO,   
       decode(FEETRD_KIND_ID,'ETF    ','STF    ','ETC    ','STC    ',FEETRD_KIND_ID) AS FEETRD_KIND_ID,   
       sum(FEETRD_M_QNTY) as FEETRD_M_QNTY,   
       sum(FEETRD_AR) AS FEETRD_AR,   
       sum(FEETRD_MK_DISC_AMT) AS FEETRD_MK_DISC_AMT,   
       sum(FEETRD_OTH_DISC_AMT) AS FEETRD_OTH_DISC_AMT,   
       sum(FEETRD_REC_AMT - FEETRD_OTH_DISC_AMT) AS FEETRD_REC_AMT ,
       max(FEETRD_UPD_DATETIME) AS FEETRD_UPD_TIME 
   FROM CI.FEETRD,ci.ABRK,CI.APDK
   WHERE FEETRD.FEETRD_FCM_NO = ABRK.ABRK_NO(+)
   AND FEETRD.FEETRD_KIND_ID = APDK.APDK_KIND_ID(+)
   AND (FEETRD_PARAM_KEY = FEETRD_KIND_ID OR FEETRD_KIND_ID = 'TXW')
   AND FEETRD_YM >= :as_sym
   AND FEETRD_YM <= :as_eym
   {0}
   GROUP BY APDK_PROD_TYPE,FEETRD_FCM_NO,decode(FEETRD_KIND_ID,'ETF    ','STF    ','ETC    ','STC    ',FEETRD_KIND_ID)
   ) a
WHERE FEETRD_FCM_NO = ABRK_NO(+)
order by feetrd_fcm_no , sort_kind_id", filter);

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }


        /// <summary>
        /// List ABRK+FEETRD data 依照商品別
        /// </summary>
        /// <param name="startDate">起始月份,可空白,format=yyyymm</param>
        /// <param name="endDate">結束月份,可空白,format=yyyymm</param>
        /// <param name="startFcmNo">期貨商起始編號,可空白</param>
        /// <param name="endFcmNo">期貨商結束編號,可空白</param>
        /// <returns></returns>
        public DataTable ListAll2(string startDate,
                                    string endDate,
                                    string startFcmNo = "",
                                    string endFcmNo = "") {

            object[] parms = {
                ":as_sym", startDate,
                ":as_eym", endDate
            };

            string filter = "";
            if (startFcmNo.Trim().Length > 0)
                filter += string.Format(" and FEETRD_FCM_NO >= '{0}'", startFcmNo);
            if (endFcmNo.Trim().Length > 0)
                filter += string.Format(" and FEETRD_FCM_NO <= '{0}'", endFcmNo);


            string sql = string.Format(@"
select s,FEETRD_PARAM_KEY,FEETRD_KIND_ID,FEETRD_M_QNTY,FEETRD_AR,FEETRD_MK_DISC_AMT,FEETRD_OTH_DISC_AMT,FEETRD_REC_AMT,FEETRD_UPD_TIME
from (
    SELECT '0' as s,
       decode(FEETRD_KIND_ID,'ETF    ','STF    ','ETC    ','STC    ',FEETRD_KIND_ID) AS FEETRD_PARAM_KEY,
       decode(FEETRD_KIND_ID,'ETF    ','STF    ','ETC    ','STC    ',FEETRD_KIND_ID) AS FEETRD_KIND_ID,   
       SUM(FEETRD_M_QNTY) as FEETRD_M_QNTY,   
       SUM(FEETRD_AR) AS FEETRD_AR,   
       SUM(FEETRD_MK_DISC_AMT) AS FEETRD_MK_DISC_AMT,   
       SUM(FEETRD_OTH_DISC_AMT) AS FEETRD_OTH_DISC_AMT,   
       SUM(FEETRD_REC_AMT - FEETRD_OTH_DISC_AMT) AS FEETRD_REC_AMT  ,
       max(FEETRD_UPD_DATETIME) AS FEETRD_UPD_TIME 
    FROM CI.FEETRD
    WHERE FEETRD_PARAM_KEY = FEETRD_KIND_ID
    AND FEETRD_YM >= :as_sym
    AND FEETRD_YM <= :as_eym
    {0}
    GROUP BY decode(FEETRD_KIND_ID,'ETF    ','STF    ','ETC    ','STC    ',FEETRD_KIND_ID)

    UNION ALL

    SELECT '9',
       decode(FEETRD_KIND_ID,'ETF    ','STF    ','ETC    ','STC    ',FEETRD_KIND_ID)  AS FEETRD_PARAM_KEY,
       ' └─'||case when FEETRD_KIND_ID = 'STF' then 'STF(不含ETF)' 
                   when FEETRD_KIND_ID = 'STC' then 'STC(不含ETC)'  else FEETRD_KIND_ID end AS FEETRD_KIND_ID,   
       SUM(FEETRD_M_QNTY) as FEETRD_M_QNTY,   
       SUM(FEETRD_AR) AS FEETRD_AR,   
       SUM(FEETRD_MK_DISC_AMT) AS FEETRD_MK_DISC_AMT,   
       SUM(FEETRD_OTH_DISC_AMT) AS FEETRD_OTH_DISC_AMT,   
       SUM(FEETRD_REC_AMT - FEETRD_OTH_DISC_AMT) AS FEETRD_REC_AMT,
       max(FEETRD_UPD_DATETIME) AS FEETRD_UPD_TIME   
    FROM CI.FEETRD
    WHERE FEETRD_KIND_ID IN ('TXW','ETF','STF','ETC','STC')
    AND FEETRD_YM >= :as_sym
    AND FEETRD_YM <= :as_eym
    {0}
    GROUP BY decode(FEETRD_KIND_ID,'ETF    ','STF    ','ETC    ','STC    ',FEETRD_KIND_ID),FEETRD_KIND_ID
) a
order by feetrd_param_key , s , feetrd_kind_id", filter);

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
