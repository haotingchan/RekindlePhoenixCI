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
/// john,20190408,調整保證金資料記錄
/// </summary>
namespace PhoenixCI.FormUI.Prefix4
{
   /// <summary>
   /// 調整保證金資料記錄
   /// </summary>
   public partial class W40060 : FormParent
   {
      private B40060 b40060;

      public W40060(string programID, string programName) : base(programID, programName)
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
         emDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
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

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }
         string saveFilePath = PbFunc.wf_copy_file(_ProgramID, "40060");
         try {
            //資料來源
            DataTable dt = new D40060().GetData(emDate.Text.AsDateTime(), emYear.Text, $"{oswGrpLookItem.EditValue.AsString()}%");
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(emDate.Text + ",讀取「Span歷次調整紀錄」無任何資料!");
               return ResultStatus.Success;
            }
            b40060 = new B40060(saveFilePath, dt, emCount.Text.AsInt());
            bool isChk = false;//判斷是否執行成功

            /* Sheet:VSR */
            ShowMsg("40061－VSR 轉檔中...");
            isChk = b40060.Wf40061();
            /* Sheet:Spread Credit */
            ShowMsg("40062－Spread Credit 轉檔中...");
            isChk = b40060.Wf40062();
            /* Sheet:Delta Per Spread Ratio */
            ShowMsg("40063－Delta Per Spread Ratio 轉檔中...");
            isChk = b40060.Wf40063();
            if (!isChk) return ResultStatus.Fail;//Exception
         }
         catch (Exception ex) {
            File.Delete(saveFilePath);
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