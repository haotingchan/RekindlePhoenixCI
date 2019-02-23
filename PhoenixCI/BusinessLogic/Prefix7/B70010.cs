using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixCI.BusinessLogic.Prefix7
{
   public class B70010
   {
      private B700xxFunc b700xxFunc;

      public B70010()
      {
         b700xxFunc =new B700xxFunc();
      }
      /// <summary>
      /// 商品別 日期/週期/月份/年度 選擇
      /// </summary>
      /// <param name="rgDateSelected">日期/週期/月份/年度</param>
      /// <param name="saveFilePath">存檔位置</param>
      /// <param name="symd">起始日期</param>
      /// <param name="eymd">終止日期</param>
      /// <param name="sum_type">日期型態 D/M/Y</param>
      /// <param name="prod_type">商品別</param>
      /// <param name="market_code">交易時段</param>
      /// <param name="isEnglish">是否轉換英文版</param>
      public void f70010ByMarketCodeExport(string rgDateSelected, string saveFilePath, string symd, string eymd, string sum_type, string prod_type, string market_code,bool isEnglish)
      {
         if (rgDateSelected.Equals("rb_week")) {
            if (!isEnglish) {
               b700xxFunc.f_70010_week_by_market_code(saveFilePath, symd, eymd, sum_type, prod_type, market_code);
            }
            else {
               b700xxFunc.f_70010_week_eng_by_market_code(saveFilePath, symd, eymd, sum_type, prod_type, market_code);
            }
         }//if (rgDate.EditValue.Equals("rb_week"))
         else {
            if (!isEnglish) {
               b700xxFunc.f_70010_ymd_by_market_code(saveFilePath, symd, eymd, sum_type, prod_type, market_code);
            }
            else {
               b700xxFunc.f_70010_ymd_eng_by_market_code(saveFilePath, symd, eymd, sum_type, prod_type, market_code);
            }
         }
      }
   }
}
