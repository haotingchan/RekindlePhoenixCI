using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using BaseGround.Shared;
using BusinessObjects;
using Common;
using DataObjects.Dao.Together;
using DevExpress.XtraEditors;
using static DataObjects.Dao.DataGate;

namespace PhoenixCI.FormUI.PrefixS {
   public partial class WS0073 : FormParent {
      protected DS0073 daoS0073;
      protected COD daoCod;
      protected string fmYmd;
      protected string toYmd;
      protected DateTime maxYmd;
      protected DateTime startDateOldValue;
      protected DateTime endDateOldValue;
      protected DataTable periodTable;

      public WS0073(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         daoS0073 = new DS0073();
         daoCod = new COD();
         GridHelper.SetCommonGrid(gvMain);
         this.Text = _ProgramID + "─" + _ProgramName;

         #region Set DropDownList
         //參數設定 參數路徑檔案 部位檔案 三個欄位要換成LookUpEdit
         //參數檔案
         DataTable dtParam = daoCod.ListByCol2("S0073", "SPAN_MARGIN_SPN");
         DataTable dtParamData = daoS0073.GetMarginUserData();//DB現有資料
         DataTable dtTempParam = dtParam.Clone();
         //部位檔案
         DataTable dtMarginPos = daoCod.ListByCol2("S0073", "SPAN_MARGIN_POS");
         DataTable dtTempMarginPos = dtMarginPos.Clone();
         //參數檔案路徑設定
         DataTable dtSpnPath = new DataTable();
         dtSpnPath.Columns.Add("COD_ID");
         dtSpnPath.Columns.Add("COD_DESC");
         dtSpnPath.Columns.Add("CP_DISPLAY");
         DataTable dtTempSpnPath = dtSpnPath.Clone();
         //參數設定下拉選單
         string[] spnParms = { "D:\\SPAN_TEST\\SPN\\", "D:\\SPAN_TEST\\UNZIP\\" };
         for (int i = 0; i < spnParms.Count(); i++) {
            dtSpnPath.Rows.Add();
            dtSpnPath.Rows[i].SetField("COD_ID", spnParms[i]);
            dtSpnPath.Rows[i].SetField("COD_DESC", spnParms[i]);
            dtSpnPath.Rows[i].SetField("CP_DISPLAY", spnParms[i]);
         }

         for (int i = 0; i < dtParamData.Rows.Count; i++) {
            //參數檔案
            dtTempParam.Rows.Add();
            dtTempParam.Rows[i].SetField("COD_ID", dtParamData.Rows[i]["span_margin_spn"]);
            dtTempParam.Rows[i].SetField("COD_DESC", dtParamData.Rows[i]["span_margin_spn"]);
            dtTempParam.Rows[i].SetField("CP_DISPLAY", "(" + dtParamData.Rows[i]["span_margin_spn"] + ")" + dtParamData.Rows[i]["span_margin_spn"]);

            //部位檔案
            dtTempMarginPos.Rows.Add();
            dtTempMarginPos.Rows[i].SetField("COD_ID", dtParamData.Rows[i]["span_margin_pos"]);
            dtTempMarginPos.Rows[i].SetField("COD_DESC", dtParamData.Rows[i]["span_margin_pos"]);
            dtTempMarginPos.Rows[i].SetField("CP_DISPLAY", "(" + dtParamData.Rows[i]["span_margin_pos"] + ")" + dtParamData.Rows[i]["span_margin_pos"]);

            //參數檔案路徑設定
            dtTempSpnPath.Rows.Add();
            dtTempSpnPath.Rows[i].SetField("COD_ID", dtParamData.Rows[i]["span_margin_spn_path"]);
            dtTempSpnPath.Rows[i].SetField("COD_DESC", dtParamData.Rows[i]["span_margin_spn_path"]);
            dtTempSpnPath.Rows[i].SetField("CP_DISPLAY", dtParamData.Rows[i]["span_margin_spn_path"]);
         }
         //參數檔案
         dtTempParam.PrimaryKey = new DataColumn[] { dtTempParam.Columns["COD_ID"] };
         dtParam.PrimaryKey = new DataColumn[] { dtParam.Columns["COD_ID"] };
         dtParam.Merge(dtTempParam, false);
         RepositoryItemLookUpEdit cbxParam = new RepositoryItemLookUpEdit();
         cbxParam.SetColumnLookUp(dtParam, "COD_ID");
         cbxParam.ProcessNewValue += new ProcessNewValueEventHandler(cbxParam_ProcessNewValue);
         gcMain.RepositoryItems.Add(cbxParam);
         SPAN_MARGIN_SPN.ColumnEdit = cbxParam;

         //部位檔案
         dtTempMarginPos.PrimaryKey = new DataColumn[] { dtTempMarginPos.Columns["COD_ID"] };
         dtMarginPos.PrimaryKey = new DataColumn[] { dtMarginPos.Columns["COD_ID"] };
         dtMarginPos.Merge(dtTempMarginPos, false);
         RepositoryItemLookUpEdit cbxMarginPos = new RepositoryItemLookUpEdit();
         cbxMarginPos.SetColumnLookUp(dtMarginPos, "COD_ID");
         gcMain.RepositoryItems.Add(cbxMarginPos);
         SPAN_MARGIN_POS.ColumnEdit = cbxMarginPos;

         //參數檔案路徑設定
         dtTempSpnPath.PrimaryKey = new DataColumn[] { dtTempSpnPath.Columns["COD_ID"] };
         dtSpnPath.PrimaryKey = new DataColumn[] { dtSpnPath.Columns["COD_ID"] };
         dtSpnPath.Merge(dtTempSpnPath, false);
         RepositoryItemLookUpEdit cbxSpnPath = new RepositoryItemLookUpEdit();
         cbxSpnPath.SetColumnLookUp(dtSpnPath, "COD_ID", "COD_DESC");
         gcMain.RepositoryItems.Add(cbxSpnPath);
         SPAN_MARGIN_SPN_PATH.ColumnEdit = cbxSpnPath;
         #endregion
      }

      protected override ResultStatus Save(PokeBall pokeBall) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         DataTable dt = (DataTable)gcMain.DataSource;
         ResultStatus resultStatus = ResultStatus.Fail;

         try {
            if (!checkChanged()) {
               MessageDisplay.Info("沒有變更資料,不需要存檔!");
               return ResultStatus.FailButNext;
            }

            dt.Rows[0].SetField("span_margin_w_time", DateTime.Now);
            if (!checkComplete(dt)) return ResultStatus.FailButNext;

            resultStatus = savePeriod();
            if (resultStatus != ResultStatus.Success) {
               MessageDisplay.Error("儲存錯誤!");
               return ResultStatus.Fail;
            }
            resultStatus = daoS0073.updateMarginData(dt).Status;//base.Save_Override(dt, "SPAN_MARGIN", DBName.CFO);
            if (resultStatus != ResultStatus.Success) {
               MessageDisplay.Error("儲存錯誤!");
               return ResultStatus.Fail;
            }
         } catch (Exception ex) {
            throw ex;
         }
         return resultStatus;
      }

      protected override ResultStatus Retrieve() {
         base.Retrieve(gcMain);
         DataTable marginTable = new DataTable();

         periodTable = daoS0073.GetPeriodData("MARGIN", "%");
         if (periodTable.Rows.Count <= 0) {
            fmYmd = DateTime.Now.AddDays(-60).ToString("yyyyMMdd");
            toYmd = DateTime.Now.ToString("yyyyMMdd");
            maxYmd = new AOCF().GetMaxDate(fmYmd, toYmd);

            txtStartDate.DateTimeValue = DateTime.ParseExact(maxYmd.ToString("yyyy/MM/dd"), "yyyy/MM/dd", null);
            txtEndDate.DateTimeValue = DateTime.ParseExact(maxYmd.ToString("yyyy/MM/dd"), "yyyy/MM/dd", null);
            startDateOldValue = txtStartDate.DateTimeValue;
            endDateOldValue = txtEndDate.DateTimeValue;
         } else {
            txtStartDate.DateTimeValue = DateTime.ParseExact(periodTable.Rows[0]["SPAN_PERIOD_START_DATE"].AsString(), "yyyyMMdd", null);
            txtEndDate.DateTimeValue = DateTime.ParseExact(periodTable.Rows[0]["SPAN_PERIOD_END_DATE"].AsString(), "yyyyMMdd", null);
            startDateOldValue = txtStartDate.DateTimeValue;
            endDateOldValue = txtEndDate.DateTimeValue;
         }

         marginTable = daoS0073.GetMarginData();

         if (marginTable.Rows.Count == 0) {
            marginTable.Rows.Add();
            marginTable.Rows[0].SetField("span_margin_ratio", 1.35);
            marginTable.Rows[0].SetField("span_margin_user_id", GlobalInfo.USER_ID);
            marginTable.Rows[0].SetField("span_margin_w_time", DateTime.Now);
         }
         gcMain.DataSource = marginTable;

         return ResultStatus.Success;
      }

      protected override ResultStatus RunBefore(PokeBall args) {
         ResultStatus resultStatus = ResultStatus.Fail;

         if (checkChanged()) {
            MessageDisplay.Info("資料有變更, 請先存檔!");
            resultStatus = ResultStatus.FailButNext;
         } else {
            if (base.RunBefore(args) == ResultStatus.Success) Run(args);
         }
         return resultStatus;
      }

      protected override ResultStatus Run(PokeBall args) {
         if (!checkChanged()) {
            PbFunc.f_bat_span("S0073", "MARGIN", GlobalInfo.USER_ID);
         }
         return base.Run(args);
      }

      private ResultStatus savePeriod() {
         periodTable = daoS0073.GetPeriodData("MARGIN", GlobalInfo.USER_ID);

         if (periodTable.Rows.Count == 0) {
            DataRow dr = periodTable.NewRow();

            dr.SetField("span_period_module", "MARGIN");
            dr.SetField("span_period_user_id", GlobalInfo.USER_ID);

            periodTable.Rows.Add(dr);
         }

         periodTable.Rows[0].SetField("span_period_start_date", txtStartDate.DateTimeValue.ToString("yyyyMMdd"));
         periodTable.Rows[0].SetField("span_period_end_date", txtEndDate.DateTimeValue.ToString("yyyyMMdd"));
         periodTable.Rows[0].SetField("span_period_w_time", DateTime.Now);

         if (checkPeriod()) {
            return daoS0073.updatePeriodData(periodTable).Status;//base.Save_Override(periodTable, "SPAN_PERIOD", DBName.CFO);
         } else {
            return ResultStatus.FailButNext;
         }
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();
         Retrieve();
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnSave.Enabled = true;
         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnRun.Enabled = true;

         return ResultStatus.Success;
      }

      private void cbxParam_ProcessNewValue(object sender, ProcessNewValueEventArgs e) {
         LookUpEdit lookUpEdit = sender as LookUpEdit;
         DataTable table = lookUpEdit.Properties.DataSource as DataTable;
         string value = e.DisplayValue.ToString();
         table.Rows.Add(new object[] { value, value, table.Rows.Count, value }).ToString();
         table.AcceptChanges();
         e.DisplayValue = value;
         e.Handled = true;
      }

      private bool checkPeriod() {

         if (txtEndDate.DateTimeValue.Subtract(txtStartDate.DateTimeValue).Days > 31) {
            MessageDisplay.Info("日期區間不可超過31天!");
            txtEndDate.Select();
            return false;
         } else if (txtStartDate.DateTimeValue > txtEndDate.DateTimeValue) {
            MessageDisplay.Info("起始值不可大於迄止值!");
            txtStartDate.Select();
            return false;
         }
         return true;
      }

      private bool checkChanged() {

         DataTable dt = (DataTable)gcMain.DataSource;
         DataTable dtChange = dt.GetChanges();

         if (dtChange != null) {
            if (dtChange.Rows.Count != 0) {
               return true;
            }
         }

         if (txtStartDate.DateTimeValue != startDateOldValue
                 || txtEndDate.DateTimeValue != endDateOldValue) {
            return true;
         }

         return false;
      }

      private bool checkComplete(DataTable dtSource) {

         foreach (DataColumn column in dtSource.Columns) {
            if (dtSource.Rows.OfType<DataRow>().Where(r => r.RowState != DataRowState.Deleted).Any(r => string.IsNullOrEmpty(r[column].ToString()))) {
               MessageDisplay.Error("尚未填寫完成");
               return false;
            }
         }
         return true;
      }
   }
}