﻿using System;
using System.Data;
using BaseGround;
using Common;
using BusinessObjects.Enums;
using DataObjects.Dao.Together.SpecificDao;
using System.IO;
using BaseGround.Shared;
using DevExpress.Spreadsheet;
using BaseGround.Report;
using DevExpress.XtraPrinting;

/// <summary>
/// david 2019/03/18
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   public partial class W30501 : FormParent {
      private D30501 dao30501;
      private ReportHelper _ReportHelper;
      private string FooterMemo = "";

      public W30501(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         dao30501 = new D30501();

         ExportShow.Hide();

         this.Text = _ProgramID + "─" + _ProgramName;

         gcMain.Hide();
         gcExport.Hide();
         txtSDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEDate.DateTimeValue = GlobalInfo.OCF_DATE;
      }

      protected override ResultStatus Export() {
         ExportShow.Text = "轉檔中...";
         ExportShow.Show();
         FooterMemo = "";

         string destinationFilePath = PbFunc.wf_GetFileSaveName(_ProgramID + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss"));
         string txtFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH, _ProgramID + ".txt");
         DataTable dtSource = (DataTable)gcExport.DataSource;

         try {
            gvExport.Columns["PROD_ID"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            gcExport.ExportToXlsx(destinationFilePath);

            Workbook workbook = new Workbook();
            workbook.LoadDocument(destinationFilePath);
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.ScrollTo(0, 0);
            worksheet.Rows.Insert(0);
            worksheet.Cells[0, 6].Value = labTime.Text;

            using (TextReader tr = new StreamReader(txtFilePath, System.Text.Encoding.Default)) {
               string line = "";
               int startRow = dtSource.Rows.Count + 3;
               while ((line = tr.ReadLine()) != null) {
                  worksheet.Cells[startRow, 0].Value = line;
                  FooterMemo += line + Environment.NewLine;
                  startRow++;
               }
            }

            workbook.SaveDocument(destinationFilePath);
         } catch (Exception ex) {
            ExportShow.Text = "轉檔失敗";
            WriteLog(ex);
         }
         ExportShow.Text = "轉檔成功!";
         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {
         gcMain.Show();
         base.Retrieve(gcMain);
         DataTable returnTable = new DataTable();
         string symd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
         string eymd = txtEDate.DateTimeValue.ToString("yyyyMMdd");
         FooterMemo = "";

         returnTable = dao30501.ListData(symd, eymd);
         if (returnTable.Rows.Count == 0) {
            _ToolBtnExport.Enabled = false;
            _ToolBtnPrintAll.Enabled = false;
            MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
            return ResultStatus.Fail;
         }
         labTime.Text = "統計期間 : " + txtSDate.DateTimeValue.ToString("yyyy/MM/dd") + "~" + txtEDate.DateTimeValue.ToString("yyyy/MM/dd");
         labTime.Show();

         gcMain.DataSource = returnTable;
         gcExport.DataSource = returnTable;

         string txtFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH, _ProgramID + ".txt");
         using (TextReader tr = new StreamReader(txtFilePath, System.Text.Encoding.Default)) {
            string line = "";
            while ((line = tr.ReadLine()) != null) {
               FooterMemo += line + Environment.NewLine;
            }
         }

         GridHelper.SetCommonGrid(gvMain);
         gvMain.SelectRow(0);
         gcMain.Focus();
         _ToolBtnExport.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);

         CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
         reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
         reportLandscape.IsHandlePersonVisible = false;
         reportLandscape.IsManagerVisible = false;
         _ReportHelper.LeftMemo = labTime.Text;
         _ReportHelper.FooterMemo = FooterMemo;
         _ReportHelper.Create(reportLandscape);

         _ReportHelper.Print();//如果有夜盤會特別標註
         _ReportHelper.Export(FileType.PDF, _ReportHelper.FilePath);

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();
         _ToolBtnRetrieve.Enabled = true;

         return ResultStatus.Success;
      }
   }
}