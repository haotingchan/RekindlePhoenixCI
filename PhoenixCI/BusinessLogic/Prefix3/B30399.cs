using DevExpress.Spreadsheet;
using System;
/// <summary>
/// 20190319,john,非金電期貨契約價量資料
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 非金電期貨契約價量資料
   /// 與B30398共用
   /// </summary>
   public class B30399
   {
      private B30398 b30398;
      private readonly Workbook _workbook;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30399(Workbook workbook, string datetime)
      {
         _workbook = workbook;
         b30398 = new B30398(_workbook, datetime);
      }
      /// <summary>
      /// wf_30331
      /// </summary>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public string Wf30331(string IsKindID= "XIF", string SheetName= "30399", int RowIndex=1, int RowTotal=33)
      {
         try {
            /*add some infor 原本template標題就已經設定 這段看不出意義在哪 所以不翻
            iole_1.application.activecell(1, 1).value = "非金電期貨"
            iole_1.application.activecell(2, 2).value = "非金電價格"
            iole_1.application.activecell(2, 4).value = "非金電期貨總成交量"
            iole_1.application.activecell(2, 5).value = "非金電期貨總未平倉量"
            end add*/
            b30398.Wf30331(IsKindID, SheetName);
         }
         catch (Exception ex) {
            throw ex;
         }
         return Common.MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30333()
      /// </summary>
      /// <param name="IsKindID">商品代號</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public string Wf30333(string IsKindID= "XIF", string SheetName= "data_30399abc", int RowIndex = 3, int RowTotal = 12)
      {
         try {
            return b30398.Wf30333(IsKindID, SheetName);
         }
         catch (Exception ex) {
            throw ex;
         }
      }

      
   }
}
