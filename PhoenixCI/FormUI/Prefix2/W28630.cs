﻿using BaseGround;
using BusinessObjects.Enums;
using Common;
/// <summary>
/// john,20190531,每日結算會員保證金轉入(廢除)
/// </summary>
namespace PhoenixCI.FormUI.Prefix2
{
   /// <summary>
   /// 每日結算會員保證金轉入 
   /// </summary>
   public partial class W28630 : FormParent
   {

      public W28630(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         emDate.DateTimeValue = GlobalInfo.OCF_DATE;
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

         _ToolBtnExport.Enabled = false;//功能廢除
         return ResultStatus.Success;
      }

      protected override ResultStatus Export()
      {
         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         return ResultStatus.Success;
      }

   }
}