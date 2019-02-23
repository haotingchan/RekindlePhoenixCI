using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using System.Threading;
using BaseGround.Shared;
using Common;
using PhoenixCI.BusinessLogic.Prefix3;
/// <summary>
/// john,20190218,證期局七組月報
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 證期局七組月報
   /// </summary>
   public partial class W30310 : FormParent
   {
      private B30310 b30310;
      public W30310(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
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
         em_month.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
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

      protected void ShowMsg(string msg)
      {
         st_msg_txt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export()
      {
         if (!ExportBefore()) {
            return ResultStatus.Fail;
         }
         try {
            bool isComply=false;//判斷是否執行成功
            string ls_file = PbFunc.wf_copy_file(_ProgramID, "30310");
            b30310 = new B30310(ls_file, em_month.Text);

            ShowMsg("30310－我國臺股期貨契約價量資料 轉檔中...");
            isComply = b30310.wf_30310_1("TXF", "30311_1");
            if(!isComply) return ResultStatus.Fail;
            isComply = b30310.wf_30310_2("EXF", "30311_2(EXF)");
            if (!isComply) return ResultStatus.Fail;
            isComply = b30310.wf_30310_2("FXF", "30311_3(FXF)");
            if (!isComply) return ResultStatus.Fail;
            ShowMsg("30313－當年每月日均量統計表 轉檔中...");
            isComply = b30310.wf_30310_4();
            if (!isComply) return ResultStatus.Fail;
            ExportAfter();
         }
         catch (Exception ex) {
            ExportAfter();
            PbFunc.messageBox(GlobalInfo.gs_t_err, ex.Message, MessageBoxIcon.Stop);
            return ResultStatus.Fail;
         }
         return ResultStatus.Success;
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