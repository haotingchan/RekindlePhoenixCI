using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using PhoenixCI.Shared;
using DataObjects.Dao.Together.SpecificDao;
using System.Threading;
using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using DevExpress.XtraReports.UI;
using System.IO;
using PhoenixCI.BusinessLogic.Prefix7;
/// <summary>
/// john,20190218,三十天期商業本票利率期貨契約價量資料
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 三十天期商業本票利率期貨契約價量資料
   /// </summary>
   public partial class W30340 : FormParent
   {
      private D70030 dao70030;
      string is_save_file;
      public W30340(string programID, string programName) : base(programID, programName)
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
         em_month.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         em_month.Text= GlobalInfo.OCF_DATE.ToString("yyyy/MM"); 
         em_month.Focus();
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
         is_save_file = ReportExportFunc.wf_GetFileSaveName($"dgbas({em_month.Text.Replace("/", "").SubStr(0, 6)}).xls");
         if (string.IsNullOrEmpty(is_save_file)) {
            return false;
         }
         /*******************
         Messagebox
         *******************/
         st_msg_txt.Visible = true;
         st_msg_txt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      protected void ExportAfter()
      {
         st_msg_txt.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         st_msg_txt.Visible = false;
      }

      protected override ResultStatus Export()
      {
         if (!ExportBefore()) {
            return ResultStatus.Fail;
         }
         try {
            string ls_ym= em_month.Text.Replace("/", "").SubStr(0, 6);
            DataTable ids_1 = dao70030.ListAll(ls_ym);
            /*******************
            轉統計資料RAM1
            *******************/
            DataTable reResult = dao70030.sp_H_stt_RAM1(ls_ym);
            if (reResult.Rows.Count >= 0) {
               saveExcel(ids_1, is_save_file, DocumentFormat.Xls);
               ExportAfter();
               return ResultStatus.Success;
            }
            else {
               PbFunc.messageBox(GlobalInfo.gs_t_err, "執行SP(sp_H_stt_RAM1)錯誤!", MessageBoxIcon.Stop);
            }
            
         }
         catch (Exception ex) {
            PbFunc.messageBox(GlobalInfo.gs_t_err, ex.Message, MessageBoxIcon.Stop);
            return ResultStatus.Fail;
         }
         return ResultStatus.Success;
      }

      private void saveExcel(DataTable dt,string filePath,DocumentFormat documentFormat, bool addHeader=true, int firstRowIndex=0, int firstColumnIndex=0)
      {
         Workbook wb = new Workbook();
         wb.Worksheets[0].Import(dt, addHeader, firstRowIndex, firstColumnIndex);
         wb.Worksheets[0].Name = sheetName(filePath);
         wb.SaveDocument(filePath, documentFormat);
      }

      private string sheetName(string is_save_file)
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