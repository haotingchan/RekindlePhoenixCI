using System;
using System.Data;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using PhoenixCI.Shared;
using DataObjects.Dao.Together.SpecificDao;
using System.Threading;
using BaseGround.Shared;
using Common;
using DevExpress.Spreadsheet;
using System.IO;
/// <summary>
/// john,20190211,每月主計處資料轉檔作業
/// </summary>
namespace PhoenixCI.FormUI.Prefix7
{
   /// <summary>
   /// 每月主計處資料轉檔作業
   /// </summary>
   public partial class W70030 : FormParent
   {
      private D70030 dao70030;
      string saveFilePath;
      public W70030(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         dao70030 = new D70030();
      }
      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         emMonth.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         emMonth.Text= GlobalInfo.OCF_DATE.ToString("yyyy/MM"); 
         emMonth.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();
         _ToolBtnExport.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      private bool ExportBefore()
      {
         /*******************
         點選儲存檔案之目錄
         *******************/
         //檔名	= 報表型態(起-迄).xls
         saveFilePath = ReportExportFunc.wf_GetFileSaveName($"dgbas({emMonth.Text.Replace("/", "").SubStr(0, 6)}).xls");
         if (string.IsNullOrEmpty(saveFilePath)) {
            return false;
         }
         /*******************
         Messagebox
         *******************/
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      protected void ExportAfter()
      {
         stMsgTxt.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      protected override ResultStatus Export()
      {
         if (!ExportBefore()) {
            return ResultStatus.Fail;
         }
         try {
            string lsYM= emMonth.Text.Replace("/", "").SubStr(0, 6);
            DataTable dt = dao70030.ListAll(lsYM);
            /*******************
            轉統計資料RAM1
            *******************/
            DataTable reResult = dao70030.sp_H_stt_RAM1(lsYM);
            if (reResult.Rows.Count >= 0) {
               SaveExcel(dt, saveFilePath, DocumentFormat.Xls);
               ExportAfter();
               return ResultStatus.Success;
            }
            else {
               PbFunc.messageBox(GlobalInfo.ErrorText, "執行SP(sp_H_stt_RAM1)錯誤!", MessageBoxIcon.Stop);
            }
            
         }
         catch (Exception ex) {
            PbFunc.messageBox(GlobalInfo.ErrorText, ex.Message, MessageBoxIcon.Stop);
            return ResultStatus.Fail;
         }
         return ResultStatus.Success;
      }

      private void SaveExcel(DataTable dt,string filePath,DocumentFormat documentFormat, bool addHeader=true, int firstRowIndex=0, int firstColumnIndex=0)
      {
         Workbook wb = new Workbook();
         wb.Worksheets[0].Import(dt, addHeader, firstRowIndex, firstColumnIndex);
         wb.Worksheets[0].Name = SheetName(filePath);
         wb.SaveDocument(filePath, documentFormat);
      }

      private string SheetName(string is_save_file)
      {
         string filename = Path.GetFileNameWithoutExtension(is_save_file);
         int nameLen = filename.Length > 31 ? 31 : filename.Length;//sheetName不能超過31字
         return filename.Substring(0, nameLen);
      }

      protected override ResultStatus Export(ReportHelper reportHelper)
      {
         base.Export(reportHelper);

         return ResultStatus.Success;
      }

      protected override ResultStatus CheckShield()
      {
         return ResultStatus.Success;
      }

      protected override ResultStatus COMPLETE()
      {
         return ResultStatus.Success;
      }
   }
}