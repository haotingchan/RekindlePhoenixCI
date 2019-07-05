using Common;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;
using DevExpress.Spreadsheet;
using System.IO;
/// <summary>
/// john,20190329,每月報局交易量報表(國內期貨暨選擇權)
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 每月報局交易量報表(國內期貨暨選擇權)
   /// </summary>
   public class B30780
   {
      /// <summary>
      /// 檔案輸出路徑
      /// </summary>
      private readonly string _lsFile;
      /// <summary>
      /// 交易日期 月份
      /// </summary>
      private readonly string _emMonthText;
      /// <summary>
      /// 交易時段
      /// </summary>
      private readonly string _lsMarketCode;
      /// <summary>
      /// 最大交易日
      /// </summary>
      private readonly DateTime _emEndDateText;
      private D30780 dao30780;

      public B30780(string lsFile, string emMonth,string MarketCode, DateTime emEndDate)
      {
         _lsFile = lsFile;
         _emMonthText = emMonth;
         _lsMarketCode = MarketCode;
         _emEndDateText = emEndDate;
         dao30780 = new D30780();
      }
      /// <summary>
      /// wf_30780_1()
      /// </summary>
      /// <returns></returns>
      public string WF30780one()
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets["附表1"];

            //資料來源
            string lastYM = _emMonthText.AsDateTime("yyyy/MM").AddDays(-1).ToString("yyyyMM");
            DataTable dt = dao30780.List30780_1(_emMonthText.Replace("/",""), lastYM, _emEndDateText, _lsMarketCode);
            if (dt.Rows.Count <= 0) {
               return $"{_emMonthText},30780_1－附表1_期貨暨選擇權最近2個月市場成交量變動比較表,無任何資料!";
            }
            
            worksheet.Cells["C1"].Value = lastYM;
            worksheet.Cells["D1"].Value = _emMonthText.Replace("/", "");
            worksheet.Cells["E1"].Value = _emEndDateText.ToString("yyyy/MM/dd");
            worksheet.Cells["F1"].Value = _lsMarketCode;
            foreach (DataRow dr in dt.Rows) {
               int rowIndex = dr["RPT_SEQ_NO"].AsInt();
               worksheet.Cells[$"C{rowIndex}"].SetValue(dr["Y_QNTY"]);
               worksheet.Cells[$"E{rowIndex}"].SetValue(dr["M_QNTY"]);
               worksheet.Cells[$"I{rowIndex}"].SetValue(dr["Y_AVG_QNTY"]);
               worksheet.Cells[$"J{rowIndex}"].SetValue(dr["M_AVG_QNTY"]);
               worksheet.Cells[$"M{rowIndex}"].SetValue(dr["MAX_YMD"]!=DBNull.Value?dr["MAX_YMD"].AsDateTime("yyyyMMdd"): dr["MAX_YMD"]);
               worksheet.Cells[$"N{rowIndex}"].SetValue(dr["MAX_M_QNTY"]);
               worksheet.Cells[$"O{rowIndex}"].SetValue(dr["AI1_HIGH_DATE"]);
               worksheet.Cells[$"P{rowIndex}"].SetValue(dr["AI1_HIGH_QNTY"]);
            }
            //選擇盤後交易時段，則將無盤後交易之商品刪除
            if (_lsMarketCode=="1") {
               DataTable newDT = dt.AsEnumerable()
                  .Where(dr => !dr.Field<string>("MARKET_CODE").Contains("1") && !dr.Field<string>("MARKET_CODE").Contains("%"))
                  .OrderByDescending(dr => dr.Field<int>("RPT_SEQ_NO")).CopyToDataTable();
               foreach (DataRow dr in newDT.Rows) {
                  int rowIndex = dr["RPT_SEQ_NO"].AsInt();
                  worksheet.Rows.Remove(rowIndex-1);
               }
            }
            worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            //worksheet.Columns.AutoFit(1,16);
            //worksheet.Rows.AutoFit(1, dt.Rows.Count);
            
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("WF30780one:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);//存檔
         }

         return MessageDisplay.MSG_OK;
      }
      /// <summary>
      /// wf_30780_2()
      /// </summary>
      /// <returns></returns>
      public string WF30780two()
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets["附表2"];

            DateTime emMth = _emMonthText.AsDateTime("yyyy/MM");
            //前6個月
            string Last6mYM = "";
            if (emMth.Month<6) {
               Last6mYM =$"{emMth.Year-1}{(emMth.Month-5+12).ToString("00")}";
            }
            else {
               Last6mYM = $"{emMth.Year - 1}{(emMth.Month - 5).ToString("00")}";
            }
            //讀取資料
            DataTable dt = dao30780.List30780_2(Last6mYM, _emMonthText.Replace("/", ""),  _lsMarketCode);
            if (dt.Rows.Count <= 0) {
               return $"{_emMonthText},30780_2－附表2_期貨暨選擇權最近6個月市場成交量彙總表,無任何資料!";
            }
            
            worksheet.Cells["C1"].Value = Last6mYM.AsDateTime("yyyyMM").Month;
            worksheet.Cells["D1"].Value = emMth.Month;
            worksheet.Cells["F1"].Value = _lsMarketCode;
            foreach (DataRow dr in dt.Rows) {
               if(dr["ROW_NUM"].Equals(DBNull.Value)|| dr["COL_NUM"].Equals(DBNull.Value)) {
                  continue;
               }
               int rowIndex = dr["ROW_NUM"].AsInt()-1;
               int colIndex = dr["COL_NUM"].AsInt()-1;
               worksheet.Rows[rowIndex][colIndex+2].SetValue(dr["M_QNTY"]);
               worksheet.Rows[rowIndex][colIndex+8].SetValue(dr["AVG_QNTY"]);
            }
            //選擇盤後交易時段，則將無盤後交易之商品刪除
            string lsParamKey = string.Empty;
            if (_lsMarketCode == "1") {
               DataTable newDT = dt.AsEnumerable()
                  .Where(dr => !dr.Field<string>("MARKET_CODE").Contains("1") && !dr.Field<string>("MARKET_CODE").Contains("%"))
                  .OrderByDescending(dr => dr.Field<int>("ROW_NUM")).CopyToDataTable();
               foreach (DataRow dr in newDT.Rows) {
                  if (lsParamKey == dr["DATA_PARAM_KEY"].AsString()) {
                     continue;
                  }
                  lsParamKey = dr["DATA_PARAM_KEY"].AsString();
                  int rowIndex = dr["ROW_NUM"].AsInt();
                  worksheet.Rows.Remove(rowIndex - 1);
               }
            }//if (_lsMarketCode == "1")

            worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("WF30780two:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);//存檔
         }

         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30780_4()
      /// </summary>
      /// <returns></returns>
      public string WF30780four()
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets["附表4_交易量前10名"];

            //讀取資料
            DataTable dt = dao30780.List30780_4(_emMonthText.Replace("/", ""), _lsMarketCode);
            if (dt.Rows.Count <= 0) {
               return $"{_emMonthText},30780_4－附表4_國內期貨市場主要期貨商月市占率概況表(依成交量排序),無任何資料!";
            }
            
            worksheet.Cells["A1"].Value = _emMonthText.AsDateTime("yyyy/MM").Month;
            worksheet.Cells["F1"].Value = _lsMarketCode;
            int rowIndex = 3;
            for (int k = 0; k < dt.Rows.Count; k++) {
               if (k>=10) {
                  break;
               }
               DataRow dr = dt.Rows[k];
               rowIndex = rowIndex + 1;
               worksheet.Cells[$"B{rowIndex}"].SetValue(dr["AM0_BRK_NAME"]);
               worksheet.Cells[$"C{rowIndex}"].SetValue(dr["M_QNTY"]);
               worksheet.Cells[$"D{rowIndex}"].SetValue(dr["M_RATE"]);
            }
            
            worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("WF30780four:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);//存檔
         }

         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30780_5()
      /// </summary>
      /// <returns></returns>
      public string WF30780five()
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets["附表5_成長衰退前3名"];

            DateTime emMthDate = _emMonthText.AsDateTime("yyyy/MM");
            string lastYM = emMthDate.AddDays(-1).ToString("yyyyMM");
            //讀取資料
            DataTable dt = dao30780.List30780_5(_emMonthText.Replace("/", ""), lastYM, _lsMarketCode);
            if (dt.Rows.Count <= 0) {
               return $"{_emMonthText},30780_5－附表5_國內期貨市場期貨商月成交量成長暨衰退概況表,無任何資料!";
            }
            
            worksheet.Cells["A1"].Value = emMthDate.Month;
            worksheet.Cells["F1"].Value = _lsMarketCode;

            //成長
            MarketTop3(dt, worksheet, 4,"desc");
            //衰退
            MarketTop3(dt, worksheet, 9,"asc");

            
            worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            
         }
         catch (Exception ex) {
            File.Delete(_lsFile);
#if DEBUG
            throw new Exception("WF30780five:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);//存檔
         }
         return MessageDisplay.MSG_OK;
      }
      /// <summary>
      /// 成長/衰退 前三名
      /// </summary>
      /// <param name="dataTable"></param>
      /// <param name="worksheet"></param>
      /// <param name="rowIndex">起始行</param>
      /// <param name="sort">asc/desc</param>
      /// <returns></returns>
      private static void MarketTop3(DataTable dataTable, Worksheet worksheet, int rowIndex,string sort)
      {
         DataView dv = dataTable.AsDataView();
         dv.Sort = string.Format("diff_rate {0},diff_qnty {0}", sort);
         DataTable dt = dv.ToTable();
         for (int k = 0; k < 3; k++) {
            DataRow dr = dt.Rows[k];
            rowIndex = rowIndex + 1;
            worksheet.Cells[$"B{rowIndex}"].SetValue(dr["AM0_BRK_NAME"]);
            worksheet.Cells[$"C{rowIndex}"].SetValue(dr["Y_QNTY"]);
            worksheet.Cells[$"D{rowIndex}"].SetValue(dr["M_QNTY"]);
            worksheet.Cells[$"E{rowIndex}"].SetValue(dr["DIFF_QNTY"]);
            worksheet.Cells[$"F{rowIndex}"].SetValue(dr["DIFF_RATE"].AsDecimal() * 100);
            if (dr["DIFF_RATE"].AsDecimal() >= 99999999) {
               worksheet.Cells[$"F{rowIndex}"].SetValue("∞");
            }
            if (dr["DIFF_RATE"].AsDecimal() <= -99999999) {
               worksheet.Cells[$"F{rowIndex}"].SetValue("-∞");
            }
         }
      }//MarketTop3

   }
}