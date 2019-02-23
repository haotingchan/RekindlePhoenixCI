using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/01/24
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30650 專、兼營期貨商當沖交易量統計
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30650 : FormParent {

      int ii_ole_row;
      private D30650 dao30650;

      public W30650(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         dao30650 = new D30650();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartMonth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/01"); //起始月份皆設為當年1月
         txtEndMonth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");

         //winni test
         //201207-201210
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {

         base.Export();
         lblProcessing.Visible = true;

         //1.複製檔案 & 開啟檔案
         string excelDestinationPath = CopyExcelTemplateFile(_ProgramID , FileType.XLS);
         Workbook workbook = new Workbook();
         workbook.LoadDocument(excelDestinationPath);
         Worksheet worksheet = workbook.Worksheets[0];

         //2.填資料
         ii_ole_row = 3;
         bool result = false;
         result = wf_Export(workbook , worksheet , txtStartMonth.Text.Replace("/" , "") , txtEndMonth.Text.Replace("/" , ""));

         //存檔
         workbook.SaveDocument(excelDestinationPath);
         lblProcessing.Visible = false;
         return ResultStatus.Success;
      }

      private bool wf_Export(Workbook workbook , Worksheet worksheet , string as_symd , string as_eymd) {

         try {
            int li_ole_row_tol = 300, li_header = 7, li_col = 0;
            int ll_found;
            string ls_ymd = "";

            lblProcessing.Text = this.Text + " 轉檔中...";
            string startDate = txtStartMonth.DateTimeValue.ToString("yyyy/MM/01"); //取起始月第一天
            string endDate = Convert.ToDateTime(txtEndMonth.DateTimeValue.ToString("yyyy/MM/01")).AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd"); //取結束月最後一天

            DataTable dtContent = dao30650.GetData(startDate.Replace("/" , "") , endDate.Replace("/" , ""));
            if (dtContent.Rows.Count == 0) {
               MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , startDate + "-" + endDate , this.Text));
               return false;
            }

            DataTable dtTmp = dao30650.GetTmpData(startDate.Replace("/" , "") , endDate.Replace("/" , ""));
            if (dtTmp.Rows.Count == 0) {
               MessageDisplay.Info(string.Format("{0},{1},(合計),無任何資料!" , startDate + "-" + endDate , this.Text));
               return false;
            }
            worksheet.Cells[li_header - 2 , 0].Value = "查詢條件" + txtStartMonth.DateTimeValue.ToString("yyyyMM") + "–" +
                                                                   txtEndMonth.DateTimeValue.ToString("yyyyMM");
            worksheet.Cells[li_header - 1 , 0].Value = "期貨商" + (char)13 + (char)10 + "代號";
            worksheet.Cells[li_header - 1 , 1].Value = "期貨商名稱";

            //先填上個月份值
            for (int i = 0 ; i < dtContent.Rows.Count ; i++) {
               if (ls_ymd != dtContent.Rows[i]["AM10_YM"].AsString()) {
                  ls_ymd = dtContent.Rows[i]["AM10_YM"].AsString();
                  li_col += 3;
                  worksheet.Cells[li_header - 1 , li_col - 1].Value = ls_ymd.SubStr(0 , 4) + "/" + ls_ymd.SubStr(4 , 2) +
                                                                      (char)13 + (char)10 + "合計" + (char)13 + (char)10 + "交易量";
                  worksheet.Cells[li_header - 1 , li_col].Value = "當沖" + (char)13 + (char)10 + "交易量";
                  worksheet.Cells[li_header - 1 , li_col + 1].Value = "當沖" + (char)13 + (char)10 + "比率" + (char)13 + (char)10 + "%";
                  worksheet.Cells[(dtTmp.Rows.Count + 1 + li_header) - 1 , li_col - 1].Value = dtContent.Compute("Sum(am10_qnty)" , "AM10_YM = " + dtContent.Rows[i]["AM10_YM"].AsString()).AsDecimal();
                  worksheet.Cells[(dtTmp.Rows.Count + 1 + li_header) - 1 , li_col].Value = dtContent.Compute("Sum(am10_dt_qnty)" , "AM10_YM = " + dtContent.Rows[i]["AM10_YM"].AsString()).AsDecimal();
                  worksheet.Cells[(dtTmp.Rows.Count + 1 + li_header) - 1 , li_col + 1].Value = dtContent.Compute("(Sum(am10_dt_qnty)/Sum(am10_qnty))*100" , "AM10_YM = " + dtContent.Rows[i]["AM10_YM"].AsString()).AsDecimal();
               }
               dtTmp.PrimaryKey = new DataColumn[] { dtTmp.Columns["ABRK_NO"] };
               ll_found = dtTmp.Rows.IndexOf(dtTmp.Rows.Find(dtContent.Rows[i]["ABRK_NO"])).AsInt();
               worksheet.Cells[(ll_found + li_header) , li_col - 1].Value = dtContent.Rows[i]["am10_qnty"].AsDecimal();
               worksheet.Cells[(ll_found + li_header) , li_col].Value = dtContent.Rows[i]["am10_dt_qnty"].AsDecimal();
               worksheet.Cells[(ll_found + li_header) , li_col + 1].Value = dtContent.Rows[i]["am10_rate"].AsDecimal();
            }
            //填上各期貨商名稱&合計
            li_col += 3;
            worksheet.Cells[li_header - 1 , li_col - 1].Value = txtStartMonth.DateTimeValue.ToString("yyyyMM") + '-' +
                txtEndMonth.DateTimeValue.ToString("yyyyMM") + (char)13 + (char)10 + "合計" + (char)13 + (char)10 + "交易量";
            worksheet.Cells[li_header - 1 , li_col].Value = "當沖" + (char)13 + (char)10 + "交易量";
            worksheet.Cells[li_header - 1 , li_col + 1].Value = "當沖" + (char)13 + (char)10 + "比率" + (char)13 + (char)10 + "%";
            for (int i = 0 ; i < dtTmp.Rows.Count ; i++) {

               worksheet.Cells[i + li_header , 0].Value = dtTmp.Rows[i]["abrk_no"].AsString();
               worksheet.Cells[i + li_header , 1].Value = dtTmp.Rows[i]["abrk_abbr_name"].AsString();
               worksheet.Cells[i + li_header , li_col - 1].Value = dtTmp.Rows[i]["am10_qnty"].AsDecimal();
               worksheet.Cells[i + li_header , li_col].Value = dtTmp.Rows[i]["am10_dt_qnty"].AsDecimal();
               worksheet.Cells[i + li_header , li_col + 1].Value = dtTmp.Rows[i]["am10_rate"].AsDecimal();
            }
            worksheet.Cells[dtTmp.Rows.Count + li_header , 1].Value = "合計";
            worksheet.Cells[dtTmp.Rows.Count + li_header , li_col - 1].Value = dtTmp.Compute("Sum(am10_qnty)" , null).AsDecimal();
            worksheet.Cells[dtTmp.Rows.Count + li_header , li_col].Value = dtTmp.Compute("Sum(am10_dt_qnty)" , null).AsDecimal();
            worksheet.Cells[dtTmp.Rows.Count + li_header , li_col + 1].Value = dtTmp.Compute("(Sum(am10_dt_qnty)/Sum(am10_qnty))*100" , null).AsDecimal();
            return true;
         } catch (Exception ex) { //失敗寫LOG
            PbFunc.f_write_logf(_ProgramID , "error" , ex.Message);
            return false;
         }

      }


   }
}