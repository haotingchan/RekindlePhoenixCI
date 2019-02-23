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
using BaseGround.Shared;
using Common;
using BusinessObjects.Enums;
using BaseGround.Report;
using BusinessObjects;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using static DataObjects.Dao.DataGate;

//TODO : save & 判斷是否有更動卻關閉尚未製作
namespace PhoenixCI.FormUI.Prefix5 {
   // Winni, 2019/01/02
   public partial class W51070 : FormParent {

      private ReportHelper _ReportHelper;
      private D51070 dao51070;

      public W51070(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         GridHelper.SetCommonGrid(gvMain);
         PrintableComponent = gcMain;
         dao51070 = new D51070();
      }

      public override ResultStatus BeforeOpen() {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open() {
         base.Open();
         lblDate.Text = PbFunc.f_ocf_date(0 , "fut");
         wf_retrieve();
         return ResultStatus.Success;
      }

      //RadioButton 判斷
      private string wf_retrieve() {
         string ls_dw_name = GetDbName();
         lblDate.Text = GetDate();

         DataTable defaultTable = new DataTable();
         defaultTable = dao51070.ListData(ls_dw_name);
         if (defaultTable.Rows.Count == 0) {
            MessageBox.Show("無任何資料" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
         }
         gcMain.DataSource = defaultTable;
         //gvMain.OptionsBehavior.Editable = false;
         gcMain.Focus();

         /*
if		dw_1.modifiedCount() > 0 then
		if		messagebox(gs_t_question,"資料有異動，是否放棄異動？",Question!, YesNo!, 2) <> 1	then
				return -1
		end	if
end	if
          */

         return ls_dw_name;
      }

      /// <summary>
      /// Get db name from gb_market and gb_system ,return=futAh/optAh/fut/opt
      /// </summary>
      /// <returns></returns>
      private string GetDbName() {
         string res = "";

         if (gb_market.EditValue.ToString() == "rb_market_1") {
            if (gb_system.EditValue.ToString() == "rb_system_0") {
               res = "futAh";
            } else {
               res = "optAh";
            }
         } else {
            if (gb_system.EditValue.ToString() == "rb_system_0") {
               res = "fut";
            } else {
               res = "opt";
            }
         }
         return res;
      }

      /// <summary>
      /// Get f_ocf_date from gb_market and gb_system
      /// </summary>
      /// <returns></returns>
      private string GetDate() {
         string res = "";

         if (gb_market.EditValue.ToString() == "rb_market_1") {
            if (gb_system.EditValue.ToString() == "rb_system_0") {
               res = PbFunc.f_ocf_date(0 , "futAH");
            } else {
               res = PbFunc.f_ocf_date(0 , "optAH");
            }
         } else {
            if (gb_system.EditValue.ToString() == "rb_system_0") {
               res = PbFunc.f_ocf_date(0 , "fut");
            } else {
               res = PbFunc.f_ocf_date(0 , "opt");
            }
         }
         return res;

      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnSave.Enabled = true;
         //_ToolBtnPrintAll.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {
         base.Retrieve(gcMain);
         DataTable returnTable = new DataTable();
         //returnTable = dao51070.ListData("opt");
         if (returnTable.Rows.Count == 0) {
            MessageBox.Show("無任何資料" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
         }
         gcMain.DataSource = returnTable;
         //gvMain.OptionsBehavior.Editable = false;
         gcMain.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus CheckShield() {
         base.CheckShield(gcMain);
         //資料未異動儲存則出現警告
         if (!IsDataModify(gcMain)) {
            return ResultStatus.Fail;
         }
         //資料有異動儲存
         else {
            return ResultStatus.Success;
         }

         // if    dw_1.modifiedCount() > 0 then
         //      if    messagebox(gs_t_question , "資料有異動，是否放棄異動？" , Question!, YesNo!, 2) <> 1  then
         //            return -1
         //      end   if
         // end   if

      }

      protected override ResultStatus Save(PokeBall pokeBall) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         DataTable dt = (DataTable)gcMain.DataSource;

         DataTable dtChange = dt.GetChanges();

         if (dtChange == null) {
            MessageBox.Show("沒有變更資料,不需要存檔!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            return ResultStatus.Fail;
         }
         if (dtChange.Rows.Count == 0) {
            MessageBox.Show("沒有變更資料,不需要存檔!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            return ResultStatus.Fail;
         }

         //Get db name from gb_market and gb_system , transfer to DBName
         string tmp = GetDbName().ToUpper();
         DBName dBName = (DBName)Enum.Parse(typeof(DBName) , tmp , false);

         ResultStatus status = base.Save_Override(dt , "SLT" , dBName);
         if (status == ResultStatus.Fail) {
            return ResultStatus.Fail;
         }

         return ResultStatus.Success;
      }

      protected override ResultStatus Run(PokeBall args) {
         base.Run(gcMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus Import() {
         base.Import(gcMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus Export(ReportHelper reportHelper) {
         reportHelper = _ReportHelper;
         base.Export(reportHelper);

         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         _ReportHelper = reportHelper;
         CommonReportPortraitA4 report = new CommonReportPortraitA4();
         _ReportHelper.Create(report);
         //_ReportHelper.LeftMemo = "設定權限給(" + cbxUserId.Text.Trim() + ")";

         base.Print(_ReportHelper);
         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose() {
         return base.BeforeClose();
      }

      protected override ResultStatus COMPLETE() {
         MessageDisplay.Info(MessageDisplay.MSG_OK);
         //Retrieve();
         return ResultStatus.Success;
      }

      private void gb_market_Properties_EditValueChanged(object sender , EventArgs e) {
         wf_retrieve();
      }

      private void gb_system_Properties_EditValueChanged(object sender , EventArgs e) {
         wf_retrieve();
      }

      private void panelControl1_Paint(object sender , PaintEventArgs e) {

      }
   }
}
