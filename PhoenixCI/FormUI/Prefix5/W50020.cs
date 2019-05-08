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
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using System.IO;
using BusinessObjects;
using DataObjects.Dao.Together;
using DevExpress.XtraLayout.Utils;
using Common.Helper;

namespace PhoenixCI.FormUI.Prefix5
{
   public partial class W50020 : FormParent
   {
      public W50020(string programID, string programName) : base(programID, programName)
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
         /*******************
         Input Condition
         *******************/
         //GlobalInfo.OCF_DATE = serviceCommon.GetOCF().OCF_DATE;
         emEndDate.EditValue = GlobalInfo.OCF_DATE;
         emStartDate.EditValue = (emEndDate.Text.Substring(0, 5) + "01").AsDateTime();
         emStartYM.EditValue = GlobalInfo.OCF_DATE;
         emEndYM.EditValue = GlobalInfo.OCF_DATE;
         /* 造市者代號 */
         ////////dw_sbrkno.settransobject(sqlca);
         ////////dw_sbrkno.insertrow(0);
         ////////dw_ebrkno.settransobject(sqlca);
         ////////dw_ebrkno.insertrow(0);
         /////////* 商品群組 */
         ////////dw_prod_ct.settransobject(sqlca);
         ////////dw_prod_ct.insertrow(0);
         /////////* 造市商品 */
         ////////dw_prod_kd.settransobject(sqlca);
         ////////dw_prod_kd.insertrow(0);
         /////////* 個股商品 */
         ////////dw_prod_kd_sto.settransobject(sqlca);
         ////////dw_prod_kd_sto.insertrow(0);

         //is_table_name = 'AMM0';
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve()
      {
         base.Retrieve();

         return ResultStatus.Success;
      }

      protected override ResultStatus CheckShield()
      {
         base.CheckShield();

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall)
      {
         base.Save(pokeBall);

         return ResultStatus.Success;
      }

      protected override ResultStatus Run(PokeBall args)
      {
         base.Run(args);

         return ResultStatus.Success;
      }

      protected override ResultStatus Import()
      {
         base.Import();

         return ResultStatus.Success;
      }

      protected override ResultStatus Export()
      {
         base.Export();
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow()
      {
         base.InsertRow();

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow()
      {
         base.DeleteRow();

         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         return base.BeforeClose();
      }

   }
}