using OnePiece;
using System.Data;
/// <summary>
/// Lukas, 2019/1/3
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    /// <summary>
    /// 結算服務費收費明細表
    /// </summary>
    public class D56010 {

        private Db db;

        public D56010() {

            db = GlobalDaoSetting.DB;
        }

        /// <summary>
        /// D56011
        /// List ABRK+FEETDCC+APDK+FEETRD data 依照期貨商別
        /// </summary>
        /// <param name="startDate">起始月份,可空白,format=yyyymm</param>
        /// <param name="endDate">結束月份,可空白,format=yyyymm</param>
        /// <param name="prodType">商品條件</param>
        /// <param name="prodId">商品條件-ID</param>
        /// <param name="startFcmNo">期貨商起始編號,可空白</param>
        /// <param name="endFcmNo">期貨商結束編號,可空白</param>
        /// <returns></returns>
        public DataTable D56011(string startDate,
                                    string endDate,
                                    string prodType = "全部",
                                    string startFcmNo = "",
                                    string endFcmNo = "") {

            object[] parms = {
                ":as_sym", startDate,
                ":as_eym", endDate
            };

            string filter = "";
            if (startFcmNo != "")
                filter += string.Format(" and  FEETDCC_FCM_NO >= '{0}'", startFcmNo.ToString());
            if (endFcmNo != "")
                filter += string.Format(" and  FEETDCC_FCM_NO <= '{0}'", endFcmNo.ToString());

            switch (prodType) {
                case "全部":
                    //
                    break;
                case "期貨":
                    filter += " and APDK_PROD_TYPE = 'F'";
                    break;
                case "選擇權":
                    filter += " and APDK_PROD_TYPE = 'O'";
                    break;
                default:
                    filter += " and FEETDCC_KIND_ID like '" + prodType + "%'";
                    break;
            }

            string sql = string.Format(@"
SELECT FEETDCC_FCM_NO,FEETDCC_ACC_NO,FEETDCC_KIND_ID,FEETRD_M_QNTY,FEETDCC_ORG_AR,FEETDCC_DISC_AMT,
              ABRK_ABBR_NAME AS brk_abbr_name,APDK_PROD_TYPE
   FROM ci.ABRK,
        (SELECT FEETDCC_FCM_NO,FEETDCC_ACC_NO,   
                decode(FEETDCC_KIND_ID,'ETF    ','STF    ','ETO    ','STO    ',FEETDCC_KIND_ID) as FEETDCC_KIND_ID,
                SUM(FEETDCC_ORG_AR) AS FEETDCC_ORG_AR,   
                SUM(FEETDCC_DISC_AMT) AS FEETDCC_DISC_AMT,
                MAX(case when FEETDCC_KIND_ID in ('ETF','STF') then 'F' 
                        when FEETDCC_KIND_ID in ('ETO','STO') then 'O'
                        else APDK_PROD_TYPE end) AS APDK_PROD_TYPE    
           FROM CI.FEETDCC,ci.APDK
          WHERE FEETDCC_YM >= :as_sym    
            AND FEETDCC_YM <= :as_eym
            AND FEETDCC_KIND_ID = APDK_KIND_ID(+)
          group by FEETDCC_FCM_NO,FEETDCC_ACC_NO,   
                decode(FEETDCC_KIND_ID,'ETF    ','STF    ','ETO    ','STO    ',FEETDCC_KIND_ID)) TD,         
        (select FEETRD_FCM_NO,FEETRD_ACC_NO,
                decode(FEETRD_KIND_ID,'ETF    ','STF    ','ETO    ','STO    ',FEETRD_KIND_ID) as FEETRD_KIND_ID,
                NVL(SUM(FEETRD_M_QNTY),0) as FEETRD_M_QNTY 
          from ci.FEETRD
          where FEETRD_YM >= :as_sym    
            AND FEETRD_YM <= :as_eym
            --AND FEETRD_SESSION = :as_session 
          group by FEETRD_FCM_NO,FEETRD_ACC_NO,decode(FEETRD_KIND_ID,'ETF    ','STF    ','ETO    ','STO    ',FEETRD_KIND_ID)) TR
  WHERE FEETDCC_FCM_NO = FEETRD_FCM_NO(+) 
    AND FEETDCC_ACC_NO = FEETRD_ACC_NO(+)
    AND FEETDCC_KIND_ID = FEETRD_KIND_ID(+)
    AND FEETDCC_FCM_NO = ABRK_NO
{0}
  Order By  feetdcc_fcm_no, feetdcc_acc_no, feetdcc_kind_id", filter);

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }


        /// <summary>
        /// D56012
        /// List FEETDCC+APDK+FEETRD data 依照商品別
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable D56012(string startDate,
                                    string endDate,
                                    string startFcmNo = "",
                                    string endFcmNo = "") {

            object[] parms = {
                ":as_sym", startDate,
                ":as_eym", endDate
            };

            string filter = "";
            if (startFcmNo != "")
                filter += string.Format(" and  FEETDCC_FCM_NO >= '{0}'", startFcmNo);
            if (endFcmNo != "")
                filter += string.Format(" and  FEETDCC_FCM_NO <= '{0}'", endFcmNo);


            string sql = string.Format(@"
SELECT FEETDCC_KIND_ID,   
        (select NVL(SUM(FEETRD_M_QNTY),0) as FEETRD_M_QNTY from ci.FEETRD
          where FEETRD_YM >= :as_sym
            and FEETRD_YM <= :as_eym
            and FEETRD_KIND_ID = FEETDCC_KIND_ID) as FEETRD_M_QNTY,
         SUM(FEETDCC_ORG_AR) AS FEETDCC_ORG_AR,   
         SUM(FEETDCC_DISC_AMT) AS FEETDCC_DISC_AMT   
    FROM CI.FEETDCC,CI.APDK
   WHERE FEETDCC_YM >= :as_sym    
     AND FEETDCC_YM <= :as_eym
     AND FEETDCC_KIND_ID = APDK_KIND_ID(+)
{0}
   GROUP BY FEETDCC_KIND_ID
   Order By feetdcc_kind_id", filter);

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable dw_prod_cond() {

            string sql = @"
select a.PARAM_PROD_TYPE,
a.PARAM_KEY,
a.PARAM_KEY as cp_display
from (
SELECT PARAM_PROD_TYPE,PARAM_KEY 
FROM CI.APDK_PARAM  

    UNION
        SELECT ' ','全部' FROM DUAL
    UNION
        SELECT ' ','期貨' FROM DUAL
    UNION
        SELECT ' ','選擇權' FROM DUAL
) a
order by param_prod_type , PARAM_KEY";

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }
    }
}
