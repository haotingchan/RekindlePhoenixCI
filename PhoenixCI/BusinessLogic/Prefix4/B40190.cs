using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Linq;
/// <summary>
/// 20190408,john,每月保證金狀況表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 每月保證金狀況表
   /// </summary>
   public class B40190
   {
      private readonly string _lsFile;
      private readonly string _emDateText;
      private readonly D40190 dao40190;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      public B40190(string FilePath, string emDate)
      {
         _lsFile = FilePath;
         _emDateText = emDate;
         dao40190 = new D40190();
      }
      /// <summary>
      /// wf_40191()
      /// </summary>
      /// <returns></returns>
      public string Wf40191()
      {
         try {
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[0];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Rows[0][0].Value = emdate;

            //讀取資料(保證金適用比例級距)
            DataTable dtMgrt1F = dao40190.Get42010Mgrt1FData();
            if (dtMgrt1F.Rows.Count <= 0) {
               return $"{_emDateText},40190_1－期貨保證金,讀取「保證金適用比例級距」無任何資料!";
            }

            worksheet.Import(dtMgrt1F, false, 106, 1);
            DataTable dtMgrt1O = dao40190.Get42010Mgrt1OData();
            if (dtMgrt1O.Rows.Count <= 0) {
               return $"{_emDateText},40190_1－期貨保證金,讀取「保證金適用比例級距」無任何資料!";
            }
            worksheet.Import(dtMgrt1O, false, 208, 1);

            //讀取資料 (期貨)
            DataTable dtFUT = dao40190.Get49191FUT(emdate);
            if (dtFUT.Rows.Count <= 0) {
               return $"{_emDateText},40190_1－期貨保證金,無任何資料!";
            }

            worksheet.Import(dtFUT.AsEnumerable().Take(1).CopyToDataTable(), false, 5, 2);
            ////跳過第7行
            worksheet.Import(dtFUT.AsEnumerable().Skip(1).CopyToDataTable(), false, 7, 2);

            //ETF類股票期貨
            DataTable dtETF = dao40190.Get49191ETF(emdate);
            worksheet.Import(dtETF, false, 52, 1);
            //刪除空白列
            int ETFcount = dtETF.Rows.Count;
            worksheet.Rows.Hide(52 + ETFcount, 102 - 1);

            //註2
            //備註2：目前除銘異期貨(IVF)、三陽期貨(FPF)保證金適用級距為級距2、XX期貨(XXX)結算保證金適用比例XX%外，所有其餘股票期貨保證金皆適用級距1。

            //註2
            //備註2：目前除XX選擇權(XXX)、XX選擇權(XXX)保證金適用級距為級距2、XX選擇權(XXX)結算保證金適用比例XX%外，所有其餘目前所有股票選擇權保證金皆適用級距1。						

            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40191:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }
      /// <summary>
      /// wf_40192()
      /// </summary>
      /// <returns></returns>
      public string Wf40192()
      {
         try {
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[0];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");

            //讀取資料
            DataTable dtFUT = dao40190.Get49192FUT(emdate);
            if (dtFUT.Rows.Count <= 0) {
               return $"{_emDateText},40192－選擇權保證金,無任何資料!";
            }
            worksheet.Import(dtFUT, false, 114, 2);

            //ETF類股票選擇權
            DataTable dtETF = dao40190.Get49192ETF(emdate);
            worksheet.Import(dtETF, false, 162, 1);

            //刪除空白列
            int ETFcount = dtETF.Rows.Count;
            if (40> ETFcount) {
               worksheet.Rows.Hide(162 + ETFcount, 202 - 1);
            }
            

            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40192:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }
      /// <summary>
      /// wf_40193()
      /// </summary>
      /// <returns></returns>
      public string Wf40193()
      {
         try {
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[0];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            
            //讀取資料
            DataTable dt = dao40190.Get40193Data(new DateTime(emdate.Year, emdate.Month, 01), emdate);
            if (dt.Rows.Count <= 0) {
               return $"{new DateTime(emdate.Year, emdate.Month, 01)},40193－調整狀況,無任何資料!";
            }
            worksheet.Import(dt, false, dao40190.Get40193SeqNo-1, 1);
            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40193:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }


   }
}
