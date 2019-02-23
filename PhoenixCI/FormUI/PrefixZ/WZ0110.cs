using ActionService;
using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Repository;
using PhoenixCI.Widget;
using System;
using System.Data;

namespace PhoenixCI.FormUI.PrefixZ
{
   public partial class WZ0110 : FormParent
   {
      private ReportHelper _ReportHelper;
      private RepositoryItemCheckEdit _RepCheck;
      private DZ0110 daoDZ0110;
      private UTP daoUTP;
      private LOGUTP daoLOGUTP;

      public WZ0110(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         daoDZ0110 = new DZ0110();
         daoUTP = new UTP();
         daoLOGUTP = new LOGUTP();
         GridHelper.SetCommonGrid(gvMain);

         PrintableComponent = gcMain;
         this.Text = _ProgramID + "─" + _ProgramName;

         _RepCheck = new RepositoryItemCheckEdit();
         _RepCheck.AllowGrayed = false;
         _RepCheck.ValueChecked = "Y";
         _RepCheck.ValueUnchecked = " ";
         _RepCheck.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;

         gcMain.RepositoryItems.Add(_RepCheck);
         UTP_FLAG.ColumnEdit = _RepCheck;
         TXN_DEFAULT.ColumnEdit = _RepCheck;
      }

      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();

         DropDownList.ComboBoxUserIdAndName(cbxUserId);
         GridHelper.AddModifyCheckMark(gcMain, _RepCheck, MODIFY_MARK);

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

         _ToolBtnSave.Enabled = true;
         _ToolBtnRetrieve.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve()
      {
         base.Retrieve(gcMain);

         gcMain.DataSource = daoDZ0110.ListUTPByUser(cbxUserId.SelectedValue.AsString());
         gcMain.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus CheckShield()
      {
         base.CheckShield(gcMain);
         if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }
         if (cbxUserId.SelectedItem == null) {
            MessageDisplay.Warning("使用者代號不可為空白!");
            return ResultStatus.Fail;
         }

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall)
      {
         base.Save(gcMain);

         DataTable dt = (DataTable)gcMain.DataSource;

         DataTable dtChange = dt.GetChanges();

         foreach (DataRow row in dtChange.Rows) {
            string flag = row["UTP_FLAG"].AsString();
            string userId = row["UTP_USER_ID"] == null ? "" : row["UTP_USER_ID"].AsString();
            string txnId = row["TXN_ID"].AsString();
            string opType = "";
            if (string.IsNullOrEmpty(userId) && flag == "Y") {
               //INSERT
               userId = cbxUserId.SelectedValue.AsString();
               opType = "I";
               bool result = daoUTP.InsertUTPByTXN(userId, GlobalInfo.USER_ID, txnId);
               bool logResult = daoLOGUTP.InsertByUTPAndUPF(txnId, GlobalInfo.USER_DPT_ID, GlobalInfo.USER_ID, GlobalInfo.USER_NAME, opType, userId);
            }
            else if (!string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(flag)) {
               //DELETE
               opType = "D";
               bool logResult = daoLOGUTP.InsertByUTPAndUPF(txnId, GlobalInfo.USER_DPT_ID, GlobalInfo.USER_ID, GlobalInfo.USER_NAME, opType, userId);
               bool result = daoUTP.DeleteUTPByUserIdAndTxnId(userId, txnId);
            }
         }

         gcMain.DataSource = dt.GetChanges();
         return ResultStatus.Success;
      }

      protected override ResultStatus Run(PokeBall args)
      {
         base.Run(gcMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus Import()
      {
         base.Import(gcMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus Export(ReportHelper reportHelper)
      {
         reportHelper = _ReportHelper;
         base.Export(reportHelper);

         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         _ReportHelper = reportHelper;
         CommonReportPortraitA4 report = new CommonReportPortraitA4();
         _ReportHelper.Create(report);
         _ReportHelper.LeftMemo = "設定權限給(" + cbxUserId.Text.Trim() + ")";

         base.Print(_ReportHelper);
         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow()
      {
         base.InsertRow(gvMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow()
      {
         base.DeleteRow(gvMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         return base.BeforeClose();
      }

      protected override ResultStatus COMPLETE()
      {
         MessageDisplay.Info(MessageDisplay.MSG_OK);
         Retrieve();
         return ResultStatus.Success;
      }

      private void cbxUserId_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (cbxUserId.SelectedItem != null) {
            Retrieve();
         }
         else {
            gcMain.DataSource = null;
         }
      }

      private void gridViewMain_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
      {
         DataRow row = (DataRow)gvMain.GetFocusedDataRow();

         string txnDefault = row["TXN_DEFAULT"].AsString();
         if (txnDefault == "Y") {
            e.Cancel = true;
         }
      }
   }
}