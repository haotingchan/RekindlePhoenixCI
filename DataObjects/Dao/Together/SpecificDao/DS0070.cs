using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class DS0070
    {
        private Db db;

        public DS0070()
        {
            db = GlobalDaoSetting.DB;
        }

        /// <summary>
        /// Get SPAN_PERIOD data, return SPAN_PERIOD_MODULE/SPAN_PERIOD_START_DATE/SPAN_PERIOD_END_DATE/SPAN_PERIOD_USER_ID/SPAN_PERIOD_W_TIME/OP_TYPE
        /// </summary>
        /// <param name="module">SPAN_PERIOD_MODULE</param>
        /// <param name="userId">SPAN_PERIOD_USER_ID</param>
        /// <returns></returns>
        public DataTable GetPeriodByUserId(string module, string userId)
        {
            object[] parms = {
                ":module",module,
                ":userId",userId
            };

            string sql = @"
SELECT SPAN_PERIOD_MODULE,
	SPAN_PERIOD_START_DATE,
	SPAN_PERIOD_END_DATE,
	SPAN_PERIOD_USER_ID,
	SPAN_PERIOD_W_TIME,
	'' as OP_TYPE
FROM CFO.SPAN_PERIOD
WHERE SPAN_PERIOD_MODULE = :module
AND SPAN_PERIOD_USER_ID like :userId
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 類別 return prod_group
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable dddw_zparm_comb_prod(string as_ymd) {
            object[] parms = {
                ":as_ymd",as_ymd
            };

            string sql = @"
select prod_group from (
    select '2' as sort, trim(ZPARM_PROD_GROUP) as prod_group ,
        trim(ZPARM_PROD_GROUP) as prod_group_value
    from ci.HZPARM
    where ZPARM_DATE = TO_DATE( (select Max(OCF_YMD)
                                from ci.AOCF
                                where OCF_YMD >=TO_CHAR(TO_DATE(:as_ymd,'YYYYMMDD') - 60,'YYYYMMDD')
                                and OCF_YMD <= :as_ymd
                                ),'YYYYMMDD') 
    and ZPARM_PROD_GROUP <> 'EQT'
    group by ZPARM_PROD_GROUP
    union all
    select '2','EQT-STF','EQT-STF' from dual
    union all
    select '2','EQT-ETF','EQT-ETF' from dual
    union all
    select '1','全部','ALL' from dual
) a
order by sort,prod_group
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 商品組合 (先選類別,在連動到此下拉選單) return comb_prod/comb_prod_value/prod_group
        /// </summary>
        /// <param name="as_ymd"></param>
        /// <param name="group_org"></param>
        /// <param name="param_key"></param>
        /// <returns></returns>
        public DataTable dddw_zparm_comb_prod_by_group(string as_ymd, string group_org, string param_key) {
            object[] parms = {
                ":as_ymd",as_ymd,
                ":group_org",group_org,
                ":param_key",param_key
            };

            string sql = @"
select trim(ZPARM_COMB_PROD) as comb_prod, 
    trim(ZPARM_COMB_PROD) as comb_prod_value,
    trim(ZPARM_PROD_GROUP) as prod_group
from ci.HZPARM,ci.APDK
where ZPARM_DATE = TO_DATE( (select Max(OCF_YMD) from ci.AOCF
                            where OCF_YMD >=TO_CHAR(TO_DATE(:as_ymd,'YYYYMMDD') - 60,'YYYYMMDD')
                            and OCF_YMD <= :as_ymd
                            ),'YYYYMMDD') 
and ZPARM_PROD_ID = APDK_KIND_ID(+)
and ZPARM_PROD_GROUP LIKE :group_org
and NVL(APDK_PARAM_KEY,' ') LIKE :param_key
group by ZPARM_COMB_PROD,ZPARM_PROD_GROUP
union all
select '全部','ALL', '全部' from dual
order by comb_prod desc
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        ///Get SPAN_ACCT_MODULE return SPAN_ACCT_MODULE/SPAN_ACCT_FCM_NO/SPAN_ACCT_ACC_NO/SPAN_ACCT_USER_ID/SPAN_ACCT_W_TIME    
        /// </summary>
        /// <param name="module">SPAN_ACCT_MODULE</param>
        /// <returns></returns>
        public DataTable GetExAccountData(string module) {
            object[] parms = {
                ":module",module
            };

            string sql = @"
                                 SELECT 
                                    SPAN_ACCT_MODULE,     
                                    trim(SPAN_ACCT_FCM_NO) as SPAN_ACCT_FCM_NO,
                                    trim(SPAN_ACCT_ACC_NO) as SPAN_ACCT_ACC_NO,
                                    SPAN_ACCT_USER_ID,     
                                    SPAN_ACCT_W_TIME,
                                    '0' AS EXACCOUNT_IS_NEWROW
                                    FROM CFO.SPAN_ACCT
                                    WHERE SPAN_ACCT_MODULE=:module";
            DataTable result = db.GetDataTable(sql, parms);
            return result;
        }

        /// <summary>
        /// Get SPAN_PARAM_MODULE return SPAN_PARAM_MODULE/SPAN_PARAM_CLASS/SPAN_PARAM_CC/SPAN_PARAM_TYPE/SPAN_PARAM_VALUE/SPAN_PARAM_EXPIRY/SPAN_PARAM_VOL_TYPE/SPAN_PARAM_VOL_VALUE/SPAN_PARAM_USER_ID/SPAN_PARAM_W_TIME
        /// </summary>
        /// <param name="module">SPAN_PARAM_MODULE</param>
        /// <param name="userId">SPAN_PARAM_USER_ID</param>
        /// <returns></returns>
        public DataTable GetParamData(string module, string userId) {
            object[] parms = {
                ":module",module,
                ":userId",userId
            };

            string sql = @" 
                                SELECT 
                                    trim(SPAN_PARAM_MODULE) as SPAN_PARAM_MODULE,     
                                    trim(SPAN_PARAM_CLASS) as SPAN_PARAM_CLASS,     
                                    trim(SPAN_PARAM_CC) AS SPAN_PARAM_CC,     
                                    SPAN_PARAM_TYPE,     
                                    SPAN_PARAM_VALUE,     
                                    SPAN_PARAM_EXPIRY,     
                                    SPAN_PARAM_VOL_TYPE,     
                                    SPAN_PARAM_VOL_VALUE,     
                                    SPAN_PARAM_USER_ID,    
                                    SPAN_PARAM_W_TIME,
                                    '0' AS IS_NEWROW
                                    FROM CFO.SPAN_PARAM
                                    WHERE SPAN_PARAM_MODULE=:module
                                    AND SPAN_PARAM_USER_ID=:userId";

            DataTable result = db.GetDataTable(sql, parms);
            return result;
        }

        public DataTable GetREQDataByUser(string module, string userId) {
            object[] parms = {
                ":module",module,
                ":userId",userId
            };

            string sql = @"
                                SELECT 
                                    CFO.SPAN_REQ.SPAN_REQ_MODULE,   
                                    CFO.SPAN_REQ.SPAN_REQ_TYPE,   
                                    CFO.SPAN_REQ.SPAN_REQ_VALUE,   
                                    CFO.SPAN_REQ.SPAN_REQ_USER_ID,   
                                    CFO.SPAN_REQ.SPAN_REQ_W_TIME 
                                    FROM CFO.SPAN_REQ   
	                                WHERE SPAN_REQ_MODULE = :module
	                                AND SPAN_REQ_USER_ID =:userId";

            DataTable result = db.GetDataTable(sql, parms);
            return result;
        }
    }
}
