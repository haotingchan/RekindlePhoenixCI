using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix4;
using DataObjects.Dao.Together;
using System.IO;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using System.Data;
/// <summary>
/// john,20190416,保證金調整檢核表
/// </summary>
namespace PhoenixCI.FormUI.Prefix4
{
   /// <summary>
   /// 保證金調整檢核表
   /// </summary>
   public partial class W40040 : FormParent
   {
      private B40040 b40040;
      private string _saveFilePath;

      public W40040(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         OCFG daoOCFG = new OCFG();
         oswGrpLookItem.SetDataTable(daoOCFG.ListAll(), "OSW_GRP", "OSW_GRP_NAME", DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor, null);
         oswGrpLookItem.EditValue = daoOCFG.f_gen_osw_grp();
      }

      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         emDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         emDate.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;
         return ResultStatus.Success;
      }

      private bool StartExport()
      {
         if (!emDate.IsDate(emDate.Text, CheckDate.Start)) {
            //is_chk = "Y";
            return false;
         }

         
         string oswGrp = oswGrpLookItem.EditValue.AsString();
         

         string chkAi2 ="";
         if (oswGrp=="1"|| oswGrp=="%") {
            chkAi2 = PbFunc.f_chk_ai2(emDate.Text.Replace("/", ""), oswGrp, "N", oswGrpLookItem.SelectedText, 2);
         }
         else if (oswGrp == "5" || oswGrp == "%") {
            chkAi2 = PbFunc.f_chk_ai2(emDate.Text.Replace("/", ""), oswGrp, "N", oswGrpLookItem.SelectedText, 1);
         }

         if (chkAi2 != "") {
            //is_chk = "E";
            return false;
         }

         stMsgTxt.Visible = true;

         
         _saveFilePath = PbFunc.wf_copy_file(_ProgramID, "40040");
         b40040 = new B40040(_saveFilePath, emDate.Text, oswGrp);

         stMsgTxt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      protected void EndExport()
      {
         stMsgTxt.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      protected void ShowMsg(string msg)
      {
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      private string OutputShowMessage {
         set {
            if (value == MessageDisplay.MSG_NO_DATA) {
               value = MessageDisplay.MSG_NO_DATA;
            }else if (value != MessageDisplay.MSG_OK)
               MessageDisplay.Info(value);
         }
      }


      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }
         try {
            //轉檔
            ShowMsg($"{_ProgramID}－保證金調整檢核表 轉檔中...");
            OutputShowMessage = b40040.WfSheetOne();
            ShowMsg($"{_ProgramID}－保證金調整檢核表 轉檔中...");
            OutputShowMessage = b40040.WfSheetTwo();
            ShowMsg($"{_ProgramID}_SPAN－SPAN參數檔檢核結果 轉檔中...");
            OutputShowMessage = b40040.Wf40040SPAN();
         }
         catch (Exception ex) {
            File.Delete(_saveFilePath);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
         }

         return ResultStatus.Success;
      }

   }
}