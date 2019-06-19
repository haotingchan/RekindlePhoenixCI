using System;
using System.Data;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using DevExpress.Spreadsheet;
using BaseGround.Shared;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet.Charts;

namespace PhoenixCI.FormUI.Prefix3 {
   public partial class W30710 : FormParent {
      private AI3 daoAI3;

      public W30710(string programID, string programName) : base(programID, programName) {
         daoAI3 = new AI3();
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

         ExportShow.Hide();
      }

      protected override ResultStatus Export() {
         ExportShow.Text = "轉檔中...";
         ExportShow.Show();
         try {
            Workbook workbook = new Workbook();
            DataTable dt = new DataTable();

            string kindId = "TXF";
            DateTime sdate = PbFunc.f_get_last_day("AI3", kindId, txtDate.DateTimeValue.ToString("yyyy/MM"), 1);
            DateTime edate = PbFunc.f_get_end_day("AI3", kindId, txtDate.DateTimeValue.ToString("yyyy/MM"));

            dt = daoAI3.ListAI3(kindId, sdate, edate);

            //查無資料時不產檔
            if (dt.Rows.Count <= 0) {
               ExportShow.Hide();
               MessageDisplay.Info(sdate + "~" + edate + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!");
               return ResultStatus.Fail;
            }

            //複製Template
            string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
            workbook.LoadDocument(destinationFilePath);

            //切換sheet
            Worksheet worksheet = workbook.Worksheets["30711"];
            DateTime ldt_ymd = new DateTime(1900, 1, 1);
            int row_tol = 33;
            //寫入資料
            if (dt.Rows.Count > 0) {

               for (int i = 0; i < dt.Rows.Count; i++) {
                  if (ldt_ymd != Convert.ToDateTime(dt.Rows[i]["ai3_date"])) {
                     ldt_ymd = Convert.ToDateTime(dt.Rows[i]["ai3_date"]);
                     worksheet.Cells[i + 1, 0].Value = ldt_ymd;
                  }
                  worksheet.Cells[i + 1, 1].Value = float.Parse(dt.Rows[i]["ai3_index"].ToString());
                  worksheet.Cells[i + 1, 2].Value = float.Parse(dt.Rows[i]["ai3_close_price"].ToString());
               }
            }

            //刪除空白列
            if (row_tol > dt.Rows.Count) {
               Range ra = worksheet.Range[(dt.Rows.Count + 2).ToString() + ":" + row_tol.ToString()];
               ra.Delete(DeleteMode.EntireRow);
            }

            //圖表重選範圍
            ChartObject chartObjs = workbook.ChartSheets[0].Chart;
            chartObjs.Series[0].SeriesName.SetValue("近月份期貨契約指數");
            ChartData closePrice = new ChartData();
            closePrice.RangeValue = worksheet.Range["C2:C" + (dt.Rows.Count + 1).ToString()];
            chartObjs.Series[0].Values = closePrice;
            ChartData index = new ChartData();
            index.RangeValue = worksheet.Range["B2:B" + (dt.Rows.Count + 1).ToString()];
            chartObjs.Series[1].Values = index;

            workbook.SaveDocument(destinationFilePath);
         } catch (Exception ex) {
            ExportShow.Text = "轉檔失敗";
            throw ex;
         }
         ExportShow.Text = "轉檔成功!";
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();
         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }
   }
}