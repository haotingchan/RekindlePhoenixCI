using OnePiece;
using System.Data;

/// <summary>
/// ken,2019/1/7
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    /// <summary>
    /// 交易經手費收費明細表
    /// </summary>
    public class D55050 {

        private Db db;

        public D55050() {

            db = GlobalDaoSetting.DB;

        }

        /// <summary>
        /// List FEETRD+FEETDCC data 依照期貨商別
        /// </summary>
        /// <param name="startDate">起始月份,可空白,format=yyyymm</param>
        /// <param name="endDate">結束月份,可空白,format=yyyymm</param>
        /// <param name="startFcmNo">期貨商起始編號,可空白</param>
        /// <param name="endFcmNo">期貨商結束編號,可空白</param>
        /// <returns></returns>
        public DataTable ListAll(string startDate,
                                    string endDate,
                                    string startFcmNo = "",
                                    string endFcmNo = "") {

            object[] parms = {
                ":as_sym", startDate,
                ":as_eym", endDate
            };
                        
            string filter = "";
            if (startFcmNo.Length > 0)
                filter += string.Format(" and FEETRD_FCM_NO >= '{0}'",startFcmNo);
            if (endFcmNo.Length > 0)
                filter += string.Format(" and FEETRD_FCM_NO <= '{0}'", endFcmNo);


            string sql = string.Format(@"
SELECT FEETRD_FCM_NO,
    (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK WHERE ABRK_NO = FEETRD_FCM_NO ) as BRK_ABBR_NAME,  
    FEETRD_AR ,
    FEETRD_DISC_AMT,
    FEETRD_REC_AMT,
    FEETDCC_AR,
    FEETDCC_DISC_AMT,
    (FEETDCC_AR - FEETDCC_DISC_AMT) as FEETDCC_REC_AMT  
FROM 
    (SELECT FEETRD_FCM_NO,    
        SUM(FEETRD_AR) AS FEETRD_AR,   
        SUM(FEETRD_MK_DISC_AMT+FEETRD_OTH_DISC_AMT) AS FEETRD_DISC_AMT, 
        SUM(FEETRD_REC_AMT - FEETRD_OTH_DISC_AMT) AS FEETRD_REC_AMT  
    FROM CI.FEETRD 
    WHERE FEETRD_YM >= :as_sym    
    AND FEETRD_YM <= :as_eym
    AND FEETRD_KIND_ID = FEETRD_PARAM_KEY
    {0}
    GROUP BY FEETRD_FCM_NO) TRD,  

    (SELECT FEETDCC_FCM_NO,   
        SUM(FEETDCC_ORG_AR) AS FEETDCC_AR,   
        SUM(FEETDCC_DISC_AMT) AS FEETDCC_DISC_AMT
    FROM CI.FEETDCC 
    WHERE FEETDCC_YM >= :as_sym    
    AND FEETDCC_YM <= :as_eym
    {0}
    GROUP BY FEETDCC_FCM_NO) TDCC
    
WHERE FEETRD_FCM_NO = FEETDCC_FCM_NO(+)
order by feetrd_fcm_no", filter);

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }


        /// <summary>
        /// List FEETRD+FEETDCC data 依照商品別
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
            if (startFcmNo.Length > 0)
                filter += string.Format(" and FEETRD_FCM_NO >= '{0}'", startFcmNo);
            if (endFcmNo.Length > 0)
                filter += string.Format(" and FEETRD_FCM_NO <= '{0}'", endFcmNo);


            string sql = string.Format(@"
SELECT FEETRD_FCM_NO,
   (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK WHERE ABRK_NO = FEETRD_FCM_NO ) as BRK_ABBR_NAME,  
   FEETRD_KIND_ID,
   FEETRD_AR ,
   FEETRD_DISC_AMT,
   FEETRD_REC_AMT,
   FEETDCC_AR,
   FEETDCC_DISC_AMT,
   (FEETDCC_AR - FEETDCC_DISC_AMT) as FEETDCC_REC_AMT  
FROM 
   (SELECT FEETRD_FCM_NO,    
      FEETRD_KIND_ID,
      SUM(FEETRD_AR) AS FEETRD_AR,   
      SUM(FEETRD_MK_DISC_AMT+FEETRD_OTH_DISC_AMT) AS FEETRD_DISC_AMT, 
      SUM(FEETRD_REC_AMT - FEETRD_OTH_DISC_AMT) AS FEETRD_REC_AMT  
   FROM CI.FEETRD 
   WHERE FEETRD_YM >= :as_sym    
   AND FEETRD_YM <= :as_eym
   AND FEETRD_KIND_ID = FEETRD_PARAM_KEY
   {0}
   GROUP BY FEETRD_FCM_NO,FEETRD_KIND_ID) TRD,  

   (SELECT FEETDCC_FCM_NO,
      FEETDCC_KIND_ID,   
      SUM(FEETDCC_ORG_AR) AS FEETDCC_AR,   
      SUM(FEETDCC_DISC_AMT) AS FEETDCC_DISC_AMT
   FROM CI.FEETDCC 
   WHERE FEETDCC_YM >= :as_sym    
   AND FEETDCC_YM <= :as_eym
   {0}
   GROUP BY FEETDCC_FCM_NO,FEETDCC_KIND_ID) TDCC

WHERE FEETRD_FCM_NO = FEETDCC_FCM_NO(+)
AND FEETRD_KIND_ID = FEETDCC_KIND_ID(+)
order by feetrd_fcm_no , feetrd_kind_id", filter);

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
