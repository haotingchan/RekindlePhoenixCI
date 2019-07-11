using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class D30418 : DataGate
    {
        public DataTable ListTfxmData(string as_sym, string as_eym)
        {

            object[] parms = {
                "@as_sym", as_sym,
                "@as_eym", as_eym
            };

            string sql =
        @"
            SELECT 	HBFIU_YYMMDD , 
                            DECODE(TRIM(HBFIU_TYPE),'000T','自營商','00T1','自營商(自行買賣)',
                            '00T2','自營商(避險)','97XX','投信','8888','外資及陸資(不含自營商)','888T','外資自營商','9999','三大法人合計')  AS HBFIU_TYPE, 
		                    HBFIU_BUY_VALUE , 
		                    HBFIU_SELL_VALUE , 
		                    HBFIU_DIFF_VALUE
            FROM ci.M_HBFIU 
            WHERE HBFIU_YYMMDD >= @as_sym
            AND HBFIU_YYMMDD <= @as_eym
            ORDER BY HBFIU_YYMMDD,HBFIU_TYPE
            ";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
        /// <summary>
        /// 期貨市場三大法人
        /// </summary>
        /// <param name="startDate">起始日</param>
        /// <param name="endDate">結束日</param>
        /// <param name="isSum">是否分計</param>
        /// <returns></returns>
        public DataTable ListFutAndOptData(DateTime startDate, DateTime endDate, string isSeparately, string kindId)
        {

            object[] parms = {
                "@startDate", startDate,
                "@endDate", endDate,
                "@isSeparately",isSeparately
            };

            string filter = "";
            switch (kindId) {
                case "全部":
                    break;
                case "期貨":
                    filter = @"AND APDK_PROD_TYPE = 'F'";
                    break;
                case "選擇權":
                    filter = @"AND APDK_PROD_TYPE = 'O'";
                    break;
                default:
                    filter = $"AND PRODID = '{kindId}'";
                    break;

            }


            string sql = string.Format(@"
                                SELECT 
                                            OCFDATE,	
                                            PRODNAME,	
                                            DECODE(PC_CODE,'C','買權','P','賣權') PC_CODE,
                                            DECODE(ACC_TYPE,'A','自營商','B','投信','C','外資') ACC_TYPE,
                                            TRANB_QNTY,	
                                            TRANB_VALUE,
                                            TRANS_QNTY,	
                                            TRANS_VALUE,
                                            TRAN_QNTY,	
                                            TRAN_VALUE,	
                                            OIB_QNTY,	
                                            OIB_VALUE,	
                                            OIS_QNTY,	
                                            OIS_VALUE                                        
                                FROM (
                                            --期貨
                                            SELECT 	BTINOIVL3F_OCFDATE AS OCFDATE,	
                                                            BTINOIVL3F_PRODNAME AS PRODNAME,	
                                                            BTINOIVL3F_PRODID AS PRODID,
                                                            '' AS PC_CODE,
                                                            TRIM(BTINOIVL3F_ACC_TYPE	)AS ACC_TYPE,
                                                            BTINOIVL3F_TRANB_QNTY AS TRANB_QNTY,	
                                                            BTINOIVL3F_TRANB_VALUE AS TRANB_VALUE,
                                                            BTINOIVL3F_TRANS_QNTY AS TRANS_QNTY,	
                                                            BTINOIVL3F_TRANS_VALUE AS TRANS_VALUE,
                                                            BTINOIVL3F_TRAN_QNTY AS TRAN_QNTY,	
                                                            BTINOIVL3F_TRAN_VALUE AS TRAN_VALUE,	
                                                            BTINOIVL3F_OIB_QNTY AS OIB_QNTY,	
                                                            BTINOIVL3F_OIB_VALUE AS OIB_VALUE,	
                                                            BTINOIVL3F_OIS_QNTY AS OIS_QNTY,	
                                                            BTINOIVL3F_OIS_VALUE AS OIS_VALUE
                                            FROM ci.M_BTINOIVL3F 
                                            WHERE BTINOIVL3F_OCFDATE >= @startDate
                                            AND BTINOIVL3F_OCFDATE <= @endDate
                                            UNION ALL
                                            --選擇不分計
                                            SELECT BTINOIVL4_OCFDATE,
                                                           BTINOIVL4_PRODNAME, /* 商品名稱  */
                                                           BTINOIVL4_PRODID, /* 商品代號  */
                                                           '' AS BTINOIVL4_PC_CODE,
                                                           TRIM(BTINOIVL4_ACC_TYPE), /* 身份別  */
                                                           sum(case when BTINOIVL4_PC_CODE = 'C' then BTINOIVL4_TRANB_QNTY
                                                           when BTINOIVL4_PC_CODE = 'P' then BTINOIVL4_TRANS_QNTY else 0 end),/* 本日交易口數-多方 */
                                                           sum(case when BTINOIVL4_PC_CODE = 'C' then BTINOIVL4_TRANB_VALUE
                                                           when BTINOIVL4_PC_CODE = 'P' then BTINOIVL4_TRANS_VALUE else 0 end),/* 本日交易契約金額-多方 */
                                                           sum(case when BTINOIVL4_PC_CODE = 'C' then BTINOIVL4_TRANS_QNTY
                                                           when BTINOIVL4_PC_CODE = 'P' then BTINOIVL4_TRANB_QNTY else 0 end), /* 本日交易口數-空方 */
                                                           sum(case when BTINOIVL4_PC_CODE = 'C' then BTINOIVL4_TRANS_VALUE
                                                           when BTINOIVL4_PC_CODE = 'P' then BTINOIVL4_TRANB_VALUE else 0 end), /* 本日交易契約金額-空方*/
                                                           sum(case when BTINOIVL4_PC_CODE = 'C' then BTINOIVL4_TRANB_QNTY
                                                           when BTINOIVL4_PC_CODE = 'P' then BTINOIVL4_TRANS_QNTY else 0
                                                           end)-sum(case when BTINOIVL4_PC_CODE = 'C' then BTINOIVL4_TRANS_QNTY
                                                           when BTINOIVL4_PC_CODE = 'P' then BTINOIVL4_TRANB_QNTY else 0 end), /* 本日交易契約金額-口數 */
                                                           sum(case when BTINOIVL4_PC_CODE = 'C' then BTINOIVL4_TRANB_VALUE
                                                           when BTINOIVL4_PC_CODE = 'P' then BTINOIVL4_TRANS_VALUE else 0
                                                           end)-sum(case when BTINOIVL4_PC_CODE = 'C' then BTINOIVL4_TRANS_VALUE
                                                           when BTINOIVL4_PC_CODE = 'P' then BTINOIVL4_TRANB_VALUE else 0 end), /* 本日交易契約金額-多空淨額*/
                                                           sum(BTINOIVL4_OIB_QNTY), /* 未平倉餘額口數-多方 */
                                                           sum(BTINOIVL4_OIB_VALUE), /* 未平倉餘額契約金額-多方 */
                                                           sum(BTINOIVL4_OIS_QNTY), /* 未平倉餘額口數-多方 */
                                                           sum(BTINOIVL4_OIS_VALUE) /* 未平倉餘額契約金額-空方 */
                                            FROM ci.M_BTINOIVL4  
                                            WHERE  @isSeparately = 'N' 
                                            AND BTINOIVL4_OCFDATE >= @startDate
                                            AND BTINOIVL4_OCFDATE <= @endDate
                                            group by BTINOIVL4_OCFDATE,BTINOIVL4_PRODNAME,BTINOIVL4_PRODID,BTINOIVL4_ACC_TYPE
                                            UNION ALL 
                                            --選擇權分計
                                             SELECT BTINOIVL4_OCFDATE, 	 
                                                            BTINOIVL4_PRODNAME ,
                                                            BTINOIVL4_PRODID 	,
                                                            TRIM(BTINOIVL4_PC_CODE) 	,
                                                            TRIM(BTINOIVL4_ACC_TYPE) ,
                                                            BTINOIVL4_TRANB_QNTY, 
                                                            BTINOIVL4_TRANB_VALUE,
                                                            BTINOIVL4_TRANS_QNTY ,
                                                            BTINOIVL4_TRANS_VALUE,
                                                            BTINOIVL4_TRAN_QNTY ,
                                                            BTINOIVL4_TRAN_VALUE ,
                                                            BTINOIVL4_OIB_QNTY ,
                                                            BTINOIVL4_OIB_VALUE ,
                                                            BTINOIVL4_OIS_QNTY ,
                                                            BTINOIVL4_OIS_VALUE
                                            FROM ci.M_BTINOIVL4 
                                            WHERE  @isSeparately = 'Y'
                                            AND BTINOIVL4_OCFDATE >= @startDate
                                            AND BTINOIVL4_OCFDATE <= @endDate
                                )B,
                                (SELECT APDK_PARAM_KEY,APDK_PROD_TYPE FROM CI.APDK GROUP BY APDK_PARAM_KEY,APDK_PROD_TYPE)A
                                WHERE B.PRODID = A.APDK_PARAM_KEY
                                {0} 
                                ORDER BY A.APDK_PROD_TYPE,B.PRODID
                                ", filter);


            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
