using BaseGround;
using BusinessObjects.Enums;
using Common;

namespace PhoenixCI.FormUI.Prefix3 {
   public partial class W35040 : FormParent {

      public W35040(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtAftStartYM.DateTimeValue = GlobalInfo.OCF_DATE;
         ddlScCode.SelectedIndex = 0;

         labMsg.Hide();
      }

      protected override ResultStatus AfterOpen() {
         //MessageDisplay.Info("執行完成");
         //this.Close();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();
         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

   }
}