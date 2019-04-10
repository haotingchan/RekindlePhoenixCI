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
    public class D30204: DataGate {

        /// <summary>
        /// 全部資料 Table: PL2
        /// </summary>
        /// <param name="as_ymd"></param>
        /// <param name="as_to_ymd"></param>
        /// <returns></returns>
        public DataTable d_30204(string as_ymd, string as_to_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_to_ymd", as_to_ymd
            };

            string sql =
@"
SELECT PL2_YMD,   
         PL2_EFFECTIVE_YMD,   
         PL2_KIND_ID,   
         PL2_NATURE,   
         PL2_LEGAL,   
         PL2_999,   
         PL2_PREV_NATURE,
         PL2_PREV_LEGAL,
         PL2_PREV_999
    FROM ci.PL2,
        (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204c') C,
        (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204e') E ,
        (select APDK_KIND_ID2,nvl(max(case when APDK_PROD_TYPE = 'F' then APDK_NAME else '' end),max(case when APDK_PROD_TYPE = 'O' then APDK_NAME else '' end)) AS APDK_NAME 
           from ci.APDK GROUP BY APDK_KIND_ID2) P
where PL2_YMD >= :as_ymd 
  and PL2_YMD <= :as_to_ymd
  and trim(PL2_KIND_ID) = trim(C.RPT_VALUE)
  and trim(PL2_KIND_ID) = trim(E.RPT_VALUE)
  and PL2_KIND_ID = APDK_KIND_ID2
order by pl2_effective_ymd
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 提高部位限制數之契約 Table: PL2
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <param name="as_to_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30204_up(string as_ymd, string as_to_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_to_ymd", as_to_ymd
            };

            string sql =
@"
SELECT   ROW_NUMBER() over (order by PL2_EFFECTIVE_YMD) as NO,
         APDK_NAME,    
         PL2_KIND_ID,   
         PL2_NATURE,   
         PL2_LEGAL,   
         PL2_999
    FROM ci.PL2,
        (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204c') C,
        (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204e') E ,
        (select APDK_KIND_ID2,nvl(max(case when APDK_PROD_TYPE = 'F' then APDK_NAME else '' end),max(case when APDK_PROD_TYPE = 'O' then APDK_NAME else '' end)) AS APDK_NAME 
           from ci.APDK GROUP BY APDK_KIND_ID2) P
where  PL2_YMD >= :as_ymd 
  and PL2_YMD <= :as_to_ymd
  and trim(PL2_KIND_ID) = trim(C.RPT_VALUE)
  and trim(PL2_KIND_ID) = trim(E.RPT_VALUE)
  and PL2_KIND_ID = APDK_KIND_ID2
  and (pl2_nature_adj<>'-' and pl2_nature_adj<>' ') or (pl2_legal_adj<>'-' and pl2_legal_adj<>' ') or (pl2_999_adj<>'-' and pl2_999_adj<>' ')
  order by PL2_EFFECTIVE_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 降低部位限制數之契約 Table: PL2
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <param name="as_to_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30204_down(string as_ymd, string as_to_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_to_ymd", as_to_ymd
            };

            string sql =
@"
SELECT   ROW_NUMBER() over (order by PL2_EFFECTIVE_YMD) as NO,
         APDK_NAME,    
         PL2_KIND_ID,   
         PL2_NATURE,   
         PL2_LEGAL,   
         PL2_999
    FROM ci.PL2,
        (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204c') C,
        (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204e') E ,
        (select APDK_KIND_ID2,nvl(max(case when APDK_PROD_TYPE = 'F' then APDK_NAME else '' end),max(case when APDK_PROD_TYPE = 'O' then APDK_NAME else '' end)) AS APDK_NAME 
           from ci.APDK GROUP BY APDK_KIND_ID2) P
where  PL2_YMD >= :as_ymd 
  and PL2_YMD <= :as_to_ymd
  and trim(PL2_KIND_ID) = trim(C.RPT_VALUE)
  and trim(PL2_KIND_ID) = trim(E.RPT_VALUE)
  and PL2_KIND_ID = APDK_KIND_ID2
  and pl2_nature_adj='-' or pl2_legal_adj='-' or pl2_999_adj='-'
  order by PL2_EFFECTIVE_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 全部資料 Table: PL2B
        /// </summary>
        /// <param name="as_ymd"></param>
        /// <param name="as_to_ymd"></param>
        /// <returns></returns>
        public DataTable d_30204_gbf(string as_ymd, string as_to_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_to_ymd", as_to_ymd
            };

            string sql =
@"
SELECT   PL2B_YMD,   
         PL2B_EFFECTIVE_YMD,   
         PL2B_KIND_ID,
         PL2B_NATURE_LEGAL_MTH,
         PL2B_NATURE_LEGAL_TOT,

         PL2B_999_MTH,
         PL2B_999_NEARBY_MTH,
         PL2B_999_TOT,
         PL2B_PREV_NATURE_LEGAL_MTH,
         PL2B_PREV_NATURE_LEGAL_TOT,

         PL2B_PREV_999_MTH,
         PL2B_PREV_999_NEARBY_MTH,
         PL2B_PREV_999_TOT
    FROM ci.PL2B,
        (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204c') C,
        (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204e') E ,
        (select APDK_KIND_ID2,nvl(max(case when APDK_PROD_TYPE = 'F' then APDK_NAME else '' end),max(case when APDK_PROD_TYPE = 'O' then APDK_NAME else '' end)) AS APDK_NAME 
           from ci.APDK GROUP BY APDK_KIND_ID2) P
where PL2B_YMD >= :as_ymd 
  and PL2B_YMD <= :as_to_ymd
  and trim(PL2B_KIND_ID) = trim(C.RPT_VALUE)
  and trim(PL2B_KIND_ID) = trim(E.RPT_VALUE)
  and PL2B_KIND_ID = APDK_KIND_ID2
order by pl2b_effective_ymd
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 提高部位限制數之契約 Table: PL2B
        /// </summary>
        /// <param name="as_ymd"></param>
        /// <param name="as_to_ymd"></param>
        /// <returns></returns>
        public DataTable d_30204_gbf_up(string as_ymd, string as_to_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_to_ymd", as_to_ymd
            };

            string sql =
@"
SELECT   ROW_NUMBER() over (order by PL2B_EFFECTIVE_YMD) as NO,
         APDK_NAME,   
         PL2B_KIND_ID,
         '單一月份'||ltrim(to_char(PL2B_NATURE_LEGAL_MTH, '9,999,999,990'))||
         '，各月份合計'||ltrim(to_char(PL2B_NATURE_LEGAL_TOT, '9,999,999,990')) as total_1,
         '單一月份'||ltrim(to_char(PL2B_NATURE_LEGAL_MTH, '9,999,999,990'))||
         '，各月份合計'||ltrim(to_char(PL2B_NATURE_LEGAL_TOT, '9,999,999,990')) as total_2,
         '單一月份'||ltrim(to_char(PL2B_999_MTH, '9,999,999,990'))||
         '(最近到期月份'||ltrim(to_char(PL2B_999_NEARBY_MTH, '9,999,999,990'))||
         ')，各月份合計'||ltrim(to_char(PL2B_999_TOT, '9,999,999,990')) as total_3
    FROM ci.PL2B,
        (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204c') C,
        (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204e') E ,
        (select APDK_KIND_ID2,nvl(max(case when APDK_PROD_TYPE = 'F' then APDK_NAME else '' end),max(case when APDK_PROD_TYPE = 'O' then APDK_NAME else '' end)) AS APDK_NAME 
           from ci.APDK GROUP BY APDK_KIND_ID2) P
where PL2B_YMD >= :as_ymd 
  and PL2B_YMD <= :as_to_ymd
  and trim(PL2B_KIND_ID) = trim(C.RPT_VALUE)
  and trim(PL2B_KIND_ID) = trim(E.RPT_VALUE)
  and PL2B_KIND_ID = APDK_KIND_ID2
  and pl2b_adj<>'-' and pl2b_adj<>' '
order by pl2b_effective_ymd
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 降低部位限制數之契約 Table: PL2B
        /// </summary>
        /// <param name="as_ymd"></param>
        /// <param name="as_to_ymd"></param>
        /// <returns></returns>
        public DataTable d_30204_gbf_down(string as_ymd, string as_to_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_to_ymd", as_to_ymd
            };

            string sql =
@"
SELECT   ROW_NUMBER() over (order by PL2B_EFFECTIVE_YMD) as NO,
         APDK_NAME,   
         PL2B_KIND_ID,
         '單一月份'||ltrim(to_char(PL2B_NATURE_LEGAL_MTH, '9,999,999,990'))||
         '，各月份合計'||ltrim(to_char(PL2B_NATURE_LEGAL_TOT, '9,999,999,990')) as total_1,
         '單一月份'||ltrim(to_char(PL2B_NATURE_LEGAL_MTH, '9,999,999,990'))||
         '，各月份合計'||ltrim(to_char(PL2B_NATURE_LEGAL_TOT, '9,999,999,990')) as total_2,
         '單一月份'||ltrim(to_char(PL2B_999_MTH, '9,999,999,990'))||
         '(最近到期月份'||ltrim(to_char(PL2B_999_NEARBY_MTH, '9,999,999,990'))||
         ')，各月份合計'||ltrim(to_char(PL2B_999_TOT, '9,999,999,990')) as total_3
    FROM ci.PL2B,
        (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204c') C,
        (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30204' and RPT_TXD_ID = '30204e') E ,
        (select APDK_KIND_ID2,nvl(max(case when APDK_PROD_TYPE = 'F' then APDK_NAME else '' end),max(case when APDK_PROD_TYPE = 'O' then APDK_NAME else '' end)) AS APDK_NAME 
           from ci.APDK GROUP BY APDK_KIND_ID2) P
where PL2B_YMD >= :as_ymd 
  and PL2B_YMD <= :as_to_ymd
  and trim(PL2B_KIND_ID) = trim(C.RPT_VALUE)
  and trim(PL2B_KIND_ID) = trim(E.RPT_VALUE)
  and PL2B_KIND_ID = APDK_KIND_ID2
  and pl2b_adj='-'
order by pl2b_effective_ymd
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
