using Common;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;
using DevExpress.Spreadsheet;
using BaseGround.Shared;
using System.IO;
/// <summary>
/// john,20190328,交易量資料轉檔作業
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 交易量資料轉檔作業
   /// </summary>
   public class B30720
   {
      private readonly string _lsFile;
      private readonly string _emMonthText;
      private readonly string _sleYearText;
      private readonly string _rgMarketVal;
      private readonly string _rgDateVal;

      public B30720(string lsFile, string emMonth, string sleYear, string DateEditValue ,string MarketEditValue)
      {
         _lsFile = lsFile;
         _emMonthText = emMonth;
         _sleYearText = sleYear;
         _rgMarketVal = MarketEditValue;
         _rgDateVal = DateEditValue;
      }
      /// <summary>
      /// 更換檔名
      /// </summary>
      /// <param name="filePath">檔案路徑</param>
      /// <param name="newName">要新增的字元</param>
      private static string Rename(string filePath, string newName)
      {
         FileInfo file = new FileInfo(filePath);
         string newPath = Path.Combine(file.Directory.FullName, Path.GetFileName(filePath).Replace("30720", "30720" + newName));
         if (!File.Exists(newPath)) {
            file.MoveTo(newPath);
            File.Delete(filePath);
         }
            
         return newPath;
      }
      /// <summary>
      /// wf_30720()
      /// </summary>
      /// <returns></returns>
      public string WF30720()
      {
         string newFilePath = _lsFile;
         try {
            //交易時段
            string lsMarketCode = string.Empty;
            string marketTitle = string.Empty;
            switch (_rgMarketVal) {
               case "rb_market0":
                  lsMarketCode = "0%";
                  marketTitle = "一般交易時段";
                  newFilePath = Rename(_lsFile, "_一般");
                  break;
               case "rb_market1":
                  lsMarketCode = "1%";
                  marketTitle = "盤後交易時段";
                  newFilePath = Rename(_lsFile, "_盤後");
                  break;
               default:
                  lsMarketCode = "%";
                  newFilePath = Rename(_lsFile, "_全部");
                  break;
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(newFilePath);
            Worksheet worksheet = workbook.Worksheets["30720"];
            worksheet.Cells["A2"].Value = marketTitle;
            //資料來源
            D30720 dao30720 = new D30720();
            DataTable dt = null;
            string lsYMD = string.Empty;
            DateTime dateMonth = _emMonthText.AsDateTime("yyyy/MM");
            if (_rgDateVal.Equals("rb_month")) {
               //月
               lsYMD = _emMonthText.Replace("/", "");
               dt = dao30720.GetData("M", lsYMD, lsYMD, lsYMD, lsMarketCode);
               worksheet.Cells["E1"].Value = $"本國期貨市場{dateMonth.Year - 1911}年{dateMonth.Month}月份" + worksheet.Cells["E1"].Value;
               worksheet.Cells["E2"].Value = PbFunc.f_get_month_eng_name(dateMonth.Month, "1") + dateMonth.Year + worksheet.Cells["E2"].Value;
            }
            else {
               //年
               lsYMD = _sleYearText;
               dt = dao30720.GetData("Y", lsYMD, lsYMD + "01", lsYMD + "12", lsMarketCode);
               worksheet.Cells["E1"].Value = $"本國期貨市場{dateMonth.Year - 1911}年" + worksheet.Cells["E1"].Value;
               worksheet.Cells["E2"].Value = _sleYearText + worksheet.Cells["E2"].Value;
            }

            if (dt.Rows.Count <= 0) {
               return $"{lsYMD},30720－月份交易量彙總表,無任何資料!";
            }
            foreach (DataRow dr in dt.Rows) {
               int rowIndex = dr["RPT_SEQ_NO"].AsInt();
               for (int k = 2; k < 22; k++) {
                  worksheet.Rows[rowIndex-1][k].SetValue(dr[k]);
               }
            }

            //存檔
            worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            workbook.SaveDocument(newFilePath);
            return MessageDisplay.MSG_OK;
         }
         catch (Exception ex) {
            File.Delete(newFilePath);
#if DEBUG
            throw new Exception("WF30720:" + ex.Message);
#else
            throw ex;
#endif
         }
      }

   }
}