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
    FROM CI.PL2,
        (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT WHERE RPT_TXN_ID = '30204' AND RPT_TXD_ID = '30204C') C,
        (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT WHERE RPT_TXN_ID = '30204' AND RPT_TXD_ID = '30204E') E ,
        (SELECT APDK_KIND_ID2,NVL(MAX(CASE WHEN APDK_PROD_TYPE = 'F' THEN APDK_NAME ELSE '' END),MAX(CASE WHEN APDK_PROD_TYPE = 'O' THEN APDK_NAME ELSE '' END)) AS APDK_NAME 
           FROM CI.APDK GROUP BY APDK_KIND_ID2) P
WHERE PL2_YMD >= :AS_YMD 
  AND PL2_YMD <= :AS_TO_YMD
  AND TRIM(PL2_KIND_ID) = TRIM(C.RPT_VALUE)
  AND TRIM(PL2_KIND_ID) = TRIM(E.RPT_VALUE)
  AND PL2_KIND_ID = APDK_KIND_ID2
ORDER BY PL2_EFFECTIVE_YMD
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
SELECT   ROW_NUMBER() OVER (ORDER BY PL2_EFFECTIVE_YMD) AS NO,
         APDK_NAME,    
         PL2_KIND_ID,   
         PL2_NATURE,   
         PL2_LEGAL,   
         PL2_999
    FROM CI.PL2,
        (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT WHERE RPT_TXN_ID = '30204' AND RPT_TXD_ID = '30204C') C,
        (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT WHERE RPT_TXN_ID = '30204' AND RPT_TXD_ID = '30204E') E ,
        (SELECT APDK_KIND_ID2,NVL(MAX(CASE WHEN APDK_PROD_TYPE = 'F' THEN APDK_NAME ELSE '' END),MAX(CASE WHEN APDK_PROD_TYPE = 'O' THEN APDK_NAME ELSE '' END)) AS APDK_NAME 
           FROM CI.APDK GROUP BY APDK_KIND_ID2) P
WHERE  PL2_YMD >= :AS_YMD 
  AND PL2_YMD <= :AS_TO_YMD
  AND TRIM(PL2_KIND_ID) = TRIM(C.RPT_VALUE)
  AND TRIM(PL2_KIND_ID) = TRIM(E.RPT_VALUE)
  AND PL2_KIND_ID = APDK_KIND_ID2
  AND ((PL2_NATURE_ADJ<>'-' AND PL2_NATURE_ADJ<>' ') OR (PL2_LEGAL_ADJ<>'-' AND PL2_LEGAL_ADJ<>' ') OR (PL2_999_ADJ<>'-' AND PL2_999_ADJ<>' '))
  ORDER BY PL2_EFFECTIVE_YMD
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
SELECT   ROW_NUMBER() OVER (ORDER BY PL2_EFFECTIVE_YMD) AS NO,
         APDK_NAME,    
         PL2_KIND_ID,   
         PL2_NATURE,   
         PL2_LEGAL,   
         PL2_999
    FROM CI.PL2,
        (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT WHERE RPT_TXN_ID = '30204' AND RPT_TXD_ID = '30204C') C,
        (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT WHERE RPT_TXN_ID = '30204' AND RPT_TXD_ID = '30204E') E ,
        (SELECT APDK_KIND_ID2,NVL(MAX(CASE WHEN APDK_PROD_TYPE = 'F' THEN APDK_NAME ELSE '' END),MAX(CASE WHEN APDK_PROD_TYPE = 'O' THEN APDK_NAME ELSE '' END)) AS APDK_NAME 
           FROM CI.APDK GROUP BY APDK_KIND_ID2) P
WHERE  PL2_YMD >= :AS_YMD 
  AND PL2_YMD <= :AS_TO_YMD
  AND TRIM(PL2_KIND_ID) = TRIM(C.RPT_VALUE)
  AND TRIM(PL2_KIND_ID) = TRIM(E.RPT_VALUE)
  AND PL2_KIND_ID = APDK_KIND_ID2
  AND (PL2_NATURE_ADJ='-' OR PL2_LEGAL_ADJ='-' OR PL2_999_ADJ='-')
  ORDER BY PL2_EFFECTIVE_YMD
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
    FROM CI.PL2B,
        (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT WHERE RPT_TXN_ID = '30204' AND RPT_TXD_ID = '30204C') C,
        (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT WHERE RPT_TXN_ID = '30204' AND RPT_TXD_ID = '30204E') E ,
        (SELECT APDK_KIND_ID2,NVL(MAX(CASE WHEN APDK_PROD_TYPE = 'F' THEN APDK_NAME ELSE '' END),MAX(CASE WHEN APDK_PROD_TYPE = 'O' THEN APDK_NAME ELSE '' END)) AS APDK_NAME 
           FROM CI.APDK GROUP BY APDK_KIND_ID2) P
WHERE PL2B_YMD >= :AS_YMD 
  AND PL2B_YMD <= :AS_TO_YMD
  AND TRIM(PL2B_KIND_ID) = TRIM(C.RPT_VALUE)
  AND TRIM(PL2B_KIND_ID) = TRIM(E.RPT_VALUE)
  AND PL2B_KIND_ID = APDK_KIND_ID2
ORDER BY PL2B_EFFECTIVE_YMD
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
SELECT   ROW_NUMBER() OVER (ORDER BY PL2B_EFFECTIVE_YMD) AS NO,
         APDK_NAME,   
         PL2B_KIND_ID,
         '單一月份'||LTRIM(TO_CHAR(PL2B_NATURE_LEGAL_MTH, '9,999,999,990'))||
         '，各月份合計'||LTRIM(TO_CHAR(PL2B_NATURE_LEGAL_TOT, '9,999,999,990')) AS TOTAL_1,
         '單一月份'||LTRIM(TO_CHAR(PL2B_NATURE_LEGAL_MTH, '9,999,999,990'))||
         '，各月份合計'||LTRIM(TO_CHAR(PL2B_NATURE_LEGAL_TOT, '9,999,999,990')) AS TOTAL_2,
         '單一月份'||LTRIM(TO_CHAR(PL2B_999_MTH, '9,999,999,990'))||
         '(最近到期月份'||LTRIM(TO_CHAR(PL2B_999_NEARBY_MTH, '9,999,999,990'))||
         ')，各月份合計'||LTRIM(TO_CHAR(PL2B_999_TOT, '9,999,999,990')) AS TOTAL_3
    FROM CI.PL2B,
        (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT WHERE RPT_TXN_ID = '30204' AND RPT_TXD_ID = '30204C') C,
        (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT WHERE RPT_TXN_ID = '30204' AND RPT_TXD_ID = '30204E') E ,
        (SELECT APDK_KIND_ID2,NVL(MAX(CASE WHEN APDK_PROD_TYPE = 'F' THEN APDK_NAME ELSE '' END),MAX(CASE WHEN APDK_PROD_TYPE = 'O' THEN APDK_NAME ELSE '' END)) AS APDK_NAME 
           FROM CI.APDK GROUP BY APDK_KIND_ID2) P
WHERE PL2B_YMD >= :AS_YMD 
  AND PL2B_YMD <= :AS_TO_YMD
  AND TRIM(PL2B_KIND_ID) = TRIM(C.RPT_VALUE)
  AND TRIM(PL2B_KIND_ID) = TRIM(E.RPT_VALUE)
  AND PL2B_KIND_ID = APDK_KIND_ID2
  AND (PL2B_ADJ<>'-' AND PL2B_ADJ<>' ')
ORDER BY PL2B_EFFECTIVE_YMD
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
SELECT   ROW_NUMBER() OVER (ORDER BY PL2B_EFFECTIVE_YMD) AS NO,
         APDK_NAME,   
         PL2B_KIND_ID,
         '單一月份'||LTRIM(TO_CHAR(PL2B_NATURE_LEGAL_MTH, '9,999,999,990'))||
         '，各月份合計'||LTRIM(TO_CHAR(PL2B_NATURE_LEGAL_TOT, '9,999,999,990')) AS TOTAL_1,
         '單一月份'||LTRIM(TO_CHAR(PL2B_NATURE_LEGAL_MTH, '9,999,999,990'))||
         '，各月份合計'||LTRIM(TO_CHAR(PL2B_NATURE_LEGAL_TOT, '9,999,999,990')) AS TOTAL_2,
         '單一月份'||LTRIM(TO_CHAR(PL2B_999_MTH, '9,999,999,990'))||
         '(最近到期月份'||LTRIM(TO_CHAR(PL2B_999_NEARBY_MTH, '9,999,999,990'))||
         ')，各月份合計'||LTRIM(TO_CHAR(PL2B_999_TOT, '9,999,999,990')) AS TOTAL_3
    FROM CI.PL2B,
        (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT WHERE RPT_TXN_ID = '30204' AND RPT_TXD_ID = '30204C') C,
        (SELECT RPT_VALUE,RPT_SEQ_NO FROM CI.RPT WHERE RPT_TXN_ID = '30204' AND RPT_TXD_ID = '30204E') E ,
        (SELECT APDK_KIND_ID2,NVL(MAX(CASE WHEN APDK_PROD_TYPE = 'F' THEN APDK_NAME ELSE '' END),MAX(CASE WHEN APDK_PROD_TYPE = 'O' THEN APDK_NAME ELSE '' END)) AS APDK_NAME 
           FROM CI.APDK GROUP BY APDK_KIND_ID2) P
WHERE PL2B_YMD >= :AS_YMD 
  AND PL2B_YMD <= :AS_TO_YMD
  AND TRIM(PL2B_KIND_ID) = TRIM(C.RPT_VALUE)
  AND TRIM(PL2B_KIND_ID) = TRIM(E.RPT_VALUE)
  AND PL2B_KIND_ID = APDK_KIND_ID2
  AND PL2B_ADJ='-'
ORDER BY PL2B_EFFECTIVE_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
