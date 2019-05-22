using BusinessObjects;
using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class DS0072 {
        private Db db;

        public DS0072() {
            db = GlobalDaoSetting.DB;
        }

        /// <summary>
        /// Get SPAN_PERIOD data, return SPAN_PERIOD_MODULE/SPAN_PERIOD_START_DATE/SPAN_PERIOD_END_DATE/SPAN_PERIOD_USER_ID/SPAN_PERIOD_W_TIME/OP_TYPE
        /// </summary>
        /// <param name="module">SPAN_PERIOD_MODULE</param>
        /// <param name="userId">SPAN_PERIOD_USER_ID</param>
        /// <returns></returns>
        public DataTable d_s0070_1(string module, string userId) {
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
WHERE TRIM(SPAN_PERIOD_MODULE) = :module
AND TRIM(SPAN_PERIOD_USER_ID) like :userId
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// CFO.SPAN_ZISP return 10 fields
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public DataTable zisp(string user_id,string tableName) {
            object[] parms = {
                ":user_id",user_id
            };

            string sql = string.Format(@"
SELECT trim(SPAN_ZISP_PROD_ID) as SPAN_ZISP_PROD_ID,
    trim(SPAN_ZISP_COM_PROD1) as SPAN_ZISP_COM_PROD1,
    trim(SPAN_ZISP_COM_PROD2) as SPAN_ZISP_COM_PROD2,
    SPAN_ZISP_CREDIT,
    SPAN_ZISP_DPSR1,
    SPAN_ZISP_DPSR2,
    SPAN_ZISP_PRIORITY,
    SPAN_ZISP_USER_ID,
    SPAN_ZISP_W_TIME,
    '0' as IS_NEWROW
FROM {0}
WHERE SPAN_ZISP_USER_ID like :user_id
", tableName);
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// CI.HZISP return 10 fields
        /// </summary>
        /// <param name="as_fm_ymd"></param>
        /// <param name="as_to_ymd"></param>
        /// <returns></returns>
        public DataTable hzisp(string as_fm_ymd, string as_to_ymd) {
            object[] parms = {
                ":as_fm_ymd",as_fm_ymd,
                ":as_to_ymd",as_to_ymd
            };

            string sql = @"
SELECT  trim(ZISP_PROD_ID) as SPAN_ZISP_PROD_ID,
			  ZISP_PRIORITY as SPAN_ZISP_PRIORITY,
			  trim(ZISP_COM_PROD1) as SPAN_ZISP_COM_PROD1,
			  trim(ZISP_COM_PROD2) as SPAN_ZISP_COM_PROD2,
			  ZISP_CREDIT as SPAN_ZISP_CREDIT,
			  ZISP_DPSR1 as SPAN_ZISP_DPSR1,
			  ZISP_DPSR2 as SPAN_ZISP_DPSR2,
			  ZISP_USER_ID as SPAN_ZISP_USER_ID,
			  ZISP_W_TIME as SPAN_ZISP_W_TIME,
			'1' AS IS_NEWROW
  FROM CI.HZISP
  WHERE ZISP_DATE = (
									 SELECT MAX(ZISP_DATE) FROM CI.HZISP 
									 WHERE ZISP_DATE >= TO_DATE('20181101','YYYYMMDD')
									 AND ZISP_DATE <= TO_DATE('20181111','YYYYMMDD'))
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// List Content by module/userId return 7 fields
        /// </summary>
        /// <param name="module">INTERMONTH / PSR / SOM / VSR </param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public DataTable ListSpanContentByModule(string module, string user_id) {
            object[] parms = {
                ":module",module,
                ":user_id",user_id
            };

            string sql = @"
SELECT SPAN_CONTENT_MODULE,
	SPAN_CONTENT_CLASS,
	SPAN_CONTENT_CC,
	trim(SPAN_CONTENT_TYPE) as SPAN_CONTENT_TYPE,
	SPAN_CONTENT_VALUE,
	SPAN_CONTENT_USER_ID,
	SPAN_CONTENT_W_TIME,
    '0' as IS_NEWROW
FROM CFO.SPAN_CONTENT
WHERE SPAN_CONTENT_MODULE = upper(:module)
AND SPAN_CONTENT_USER_ID = :user_id
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
order by comb_prod
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public ResultData updatePeriodData(DataTable inputData) {

            string sql = @"SELECT SPAN_PERIOD_MODULE,
	SPAN_PERIOD_START_DATE,
	SPAN_PERIOD_END_DATE,
	SPAN_PERIOD_USER_ID,
	SPAN_PERIOD_W_TIME
FROM CFO.SPAN_PERIOD";

            return db.UpdateOracleDB(inputData, sql);

        }

        public ResultData udpateSpanContentData(DataTable inputData) {

            string sql= @"
SELECT SPAN_CONTENT_MODULE,
	SPAN_CONTENT_CLASS,
	SPAN_CONTENT_CC,
	SPAN_CONTENT_TYPE,
	SPAN_CONTENT_VALUE,
	SPAN_CONTENT_USER_ID,
	SPAN_CONTENT_W_TIME
FROM CFO.SPAN_CONTENT
";
            return db.UpdateOracleDB(inputData, sql);
        }

        public ResultData udpateZIPData(DataTable inputData) {

            string sql = @"
SELECT 
    SPAN_ZISP_PROD_ID,
    SPAN_ZISP_COM_PROD1,
    SPAN_ZISP_COM_PROD2,
    SPAN_ZISP_CREDIT,
    SPAN_ZISP_DPSR1,
    SPAN_ZISP_DPSR2,
    SPAN_ZISP_PRIORITY,
    SPAN_ZISP_USER_ID,
    SPAN_ZISP_W_TIME
FROM CFO.SPAN_ZISP
";
            return db.UpdateOracleDB(inputData, sql);
        }

    }
}