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
      /// <param name="SumType">日期型態 D/M/Y</param>
      /// <param name="ProdType">商品別</param>
      /// <param name="MarketCode">交易時段</param>
      /// <param name="isEnglish">是否轉換英文版</param>
      public string F70010ByMarketCodeExport(string rgDateSelected, string saveFilePath, string symd, string eymd, string SumType, string ProdType, string MarketCode,bool isEnglish)
      {
         if (rgDateSelected.Equals("rb_week")) {
            if (!isEnglish) {
               return b700xxFunc.F70010WeekByMarketCode(saveFilePath, symd, eymd, SumType, ProdType, MarketCode);
            }
            else {
               return b700xxFunc.F70010WeekEngByMarketCode(saveFilePath, symd, eymd, SumType, ProdType, MarketCode);
            }
         }//if (rgDate.EditValue.Equals("rb_week"))
         else {
            if (!isEnglish) {
               return b700xxFunc.F70010YmdByMarketCode(saveFilePath, symd, eymd, SumType, ProdType, MarketCode);
            }
            else {
               return b700xxFunc.F70010YmdEngByMarketCode(saveFilePath, symd, eymd, SumType, ProdType, MarketCode);
            }
         }
      }
   }
}
