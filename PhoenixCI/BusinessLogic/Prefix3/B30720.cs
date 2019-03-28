using Common;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;
using DevExpress.Spreadsheet;
using BaseGround.Shared;
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
      private readonly string lsFile;
      private readonly string _emMonthText;
      private readonly string _sleYearText;
      private readonly string _rgMarketVal;
      private readonly string _rgDateVal;

      public B30720(string lsFile, string emMonth, string sleYear, string MarketEditValue, string DateEditValue)
      {
         this.lsFile = lsFile;
         _emMonthText = emMonth;
         _sleYearText = sleYear;
         _rgMarketVal = MarketEditValue;
         _rgDateVal = DateEditValue;
      }
      /// <summary>
      /// wf_30720()
      /// </summary>
      /// <returns></returns>
      public string WF30720()
      {
         try {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets["30720"];
            //交易時段
            string lsMarketCode = string.Empty;
            switch (_rgMarketVal) {
               case "rb_market0":
                  lsMarketCode = "0%";
                  worksheet.Cells["A2"].Value = "一般交易時段";
                  break;
               case "rb_market1":
                  lsMarketCode = "1%";
                  worksheet.Cells["A2"].Value = "盤後交易時段";
                  break;
               default:
                  lsMarketCode = "%";
                  break;
            }
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
               DataTable dataSource = dt.AsEnumerable().Where(x => x.Field<int>("RPT_SEQ_NO") == rowIndex).CopyToDataTable();
               worksheet.Import(dataSource, false, rowIndex-1, 1);
            }
            
            //存檔
            worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            workbook.SaveDocument(lsFile);
            return MessageDisplay.MSG_OK;
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("WF30720:" + ex.Message);
#else
            throw ex;
#endif
         }
      }

   }
}