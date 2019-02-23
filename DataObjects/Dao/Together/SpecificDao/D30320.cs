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
   }
}
