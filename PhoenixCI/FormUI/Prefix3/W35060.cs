﻿using BaseGround;
using BusinessObjects.Enums;
using Common;

namespace PhoenixCI.FormUI.Prefix3 {
   public partial class W35060 : FormParent {

      public W35060(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

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