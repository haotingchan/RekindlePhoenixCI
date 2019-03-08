using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
/// <summary>
/// john,20190220,D30320
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30320 : DataGate
   {
      /// <summary>
      /// 取得前月倒數1天交易日
      /// </summary>
      /// <param name="ls_date">輸入日期</param>
      /// <returns></returns>
      public DateTime GetLastTradeDate(string ls_date)
      {
         object[] parms = {
            ":ls_date",ls_date
            };

         string sql =
             @"select nvl(max(AI2_YMD),'19000101')
                 from ci.AI2
                where AI2_PROD_TYPE = 'F'
                  and AI2_SUM_TYPE = 'D'
                  and AI2_YMD < :ls_date ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         if (!DateTime.TryParseExact(dtResult.Rows[0][0].AsString(), "yyyyMMdd", null, DateTimeStyles.AllowWhiteSpaces, out DateTime lastTradeDate)) {
            throw new Exception("時間格式錯誤!");
         }
         return lastTradeDate;
      }

      /// <summary>
      /// return AI2_YMD/AI2_PARAM_KEY/AI2_DAY_COUNT/AI2_M_QNTY/AI2_OI
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <returns></returns>
      public DataTable Get30321Data(string as_symd, string as_eymd)
      {
         object[] parms = {
            ":as_symd",as_symd,
            ":as_eymd",as_eymd
            };

         string sql =
             @"SELECT AI2_YMD,   
                        AI2_PARAM_KEY,   
                        AI2_DAY_COUNT,   
                        AI2_M_QNTY as AI2_M_QNTY,
                        AI2_OI as AI2_OI ,  
                        (select nvl(min(RPT.RPT_SEQ_NO),0)
                           from ci.RPT
                          where RPT.RPT_TXD_ID = '30321'  
                            and RPT.RPT_VALUE = A.AI2_PARAM_KEY) as RPT_SEQ_NO
                   FROM ci.AI2 A
                  WHERE AI2_SUM_TYPE = 'D'  AND  
                        AI2_YMD >= :as_symd  AND  
                        AI2_YMD <= :as_eymd  AND
                        AI2_PROD_TYPE = 'F' AND
                        AI2_SUM_SUBTYPE = '3'
                  ORDER BY AI2_YMD,RPT_SEQ_NO,AI2_PARAM_KEY";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// return AI3_DATE/AI3_KIND_ID/AI3_CLOSE_PRICE/RPT_SEQ_NO
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <returns></returns>
      public DataTable Get30322Data(DateTime as_sdate, DateTime as_edate)
      {
         object[] parms = {
            ":as_sdate",as_sdate,
            ":as_edate",as_edate
            };

         string sql =
             @"SELECT AI3_DATE,   
                        AI3_KIND_ID,
                        AI3_CLOSE_PRICE,  
                        (select nvl(min(RPT.RPT_SEQ_NO),0)
                           from ci.RPT
                          where RPT.RPT_TXD_ID = '30321'  
                            and RPT.RPT_VALUE =  AI3.AI3_KIND_ID) as RPT_SEQ_NO
                   FROM ci.AI3  AI3
                  WHERE AI3_KIND_ID in (SELECT APDK_KIND_ID FROM ci.APDK WHERE APDK_PROD_TYPE = 'F' and APDK_PROD_SUBTYPE = 'I') 
                    AND AI3_DATE >= :as_sdate  
                    AND AI3_DATE <= :as_edate 
                  ORDER BY AI3_DATE";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
