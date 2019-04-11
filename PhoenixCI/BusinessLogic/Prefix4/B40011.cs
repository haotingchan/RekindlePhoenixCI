using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
/// <summary>
/// john,20190410,保證金狀況表 (Group1)
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 保證金狀況表 (Group1)
   /// </summary>
   public class B40011
   {
      private readonly string _lsFile;
      private readonly string _emDateText;
      private readonly D40011 dao40011;

      public B40011(string FilePath, string emDate)
      {
         _lsFile = FilePath;
         _emDateText = emDate;
         dao40011 = new D40011();
      }

      /// <summary>
      /// wf_40011_1()  rpt_future
      /// </summary>
      /// <returns></returns>
      public string Wf40011FutureSheet()
      {
         try {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets["rpt_future"];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["G1"].Value = "資料日期：" + emdate.ToLongDateString().ToString();
            const int SheetOne = 1;//第一張sheet

            //一、現行收取保證金金額：CDEFGH
            DataTable dtR1 = dao40011.GetFutR1Data(emdate);
            if (dtR1.Rows.Count <= 0)
            {
               return $"{_emDateText},40011_1－保證金狀況表,無任何資料!";
            }

            worksheet.Import(dtR1.AsEnumerable().Take(1).CopyToDataTable(), false, 2, 2);
            ////跳過第4行
            worksheet.Import(dtR1.AsEnumerable().Skip(1).CopyToDataTable(), false, 4, 2);

            //二、	本日結算保證金計算：CDEFGH
            DataTable dtR2 = dao40011.GetFutR2Data(emdate);
            //要Take的筆數
            int takeRow = dtR2.Rows.Count - 1;
            //dtR2最後一筆是 excel第49行 十年期公債期貨契約結算保證金
            worksheet.Import(dtR2.AsEnumerable().Take(takeRow).CopyToDataTable(), false, 35, 2);
            //excel第49行 十年期公債期貨契約結算保證金
            worksheet.Import(dtR2.AsEnumerable().Skip(takeRow).CopyToDataTable(), false, 48, 2);

            //四、	作業事項
            string itemOne= string.Empty;
            string itemTwo = string.Empty;
            dao40011.WorkItem(emdate,SheetOne).AsEnumerable().ToList().ForEach(dr => {
               itemOne += dr.Field<string>("N10P").AsString() + "　";
               itemTwo += dr.Field<string>("R10P").AsString() + "　";
               });
            worksheet.Cells["B68"].Value = itemOne;
            worksheet.Cells["B71"].Value = itemTwo;

            //save
            worksheet.ScrollTo(0, 0);
            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40011FutureSheet:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_40011_2()  rpt_option
      /// </summary>
      /// <returns></returns>
      public string Wf40011OptionSheet()
      {
         try
         {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets["rpt_option"];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["G5"].Value = "資料日期：" + emdate.ToLongDateString().ToString();
            const int SheetTwo = 2;//第二張sheet

            //一、現行收取保證金金額：CDEFGH
            DataTable dtR1 = dao40011.GetOptR1Data(emdate);
            if (dtR1.Rows.Count <= 0)
            {
               return $"{_emDateText},40011_2－保證金狀況表,無任何資料!";
            }

            worksheet.Import(dtR1, false, 8, 2);

            //二、	本日結算保證金計算：CDEFGH
            DataTable dtR2 = dao40011.GetOptR2Data(emdate);
            worksheet.Import(dtR2, false, 48, 3);

            //四、	作業事項
            string itemOne = string.Empty;
            string itemTwo = string.Empty;
            dao40011.WorkItem(emdate, SheetTwo).AsEnumerable().ToList().ForEach(dr => {
               itemOne += dr.Field<string>("N10P").AsString() + "　";
               itemTwo += dr.Field<string>("R10P").AsString() + "　";
            });
            worksheet.Cells["B101"].Value = itemOne;
            worksheet.Cells["B103"].Value = itemTwo;

            //save
            worksheet.ScrollTo(0, 0);
            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex)
         {
#if DEBUG
            throw new Exception($"Wf40011OptionSheet:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }

   }
}
