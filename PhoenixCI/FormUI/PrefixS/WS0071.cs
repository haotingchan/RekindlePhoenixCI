using System;
using System.Data;
using System.Windows.Forms;
using BaseGround;
using DataObjects.Dao.Together;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using DevExpress.XtraEditors.Repository;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;
using BusinessObjects;
using static DataObjects.Dao.DataGate;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using System.Linq;

namespace PhoenixCI.FormUI.PrefixS {
   public partial class WS0071 : FormParent {
      protected DS0071 daoS0071;
      protected COD daoCod;
      protected string fmYmd;
      protected string toYmd;
      protected DateTime maxYmd;
      protected DateTime startDateOldValue;
      protected DateTime endDateOldValue;
      protected DataTable periodTable;

      public WS0071(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         daoS0071 = new DS0071();
         daoCod = new COD();
         GridHelper.SetCommonGrid(gvMain);

         #region Set Date Period
         //設定初始年月yyyy/MM/dd
         DataTable dtSPN = daoS0071.GetPeriodByUserId("PL", GlobalInfo.USER_ID);
         if (dtSPN.Rows.Count <= 0) {
            fmYmd = DateTime.Now.AddDays(-60).ToString("yyyyMMdd");
            toYmd = DateTime.Now.ToString("yyyyMMdd");
            maxYmd = new AOCF().GetMaxDate(fmYmd, toYmd);

            txtEndDate.DateTimeValue = DateTime.ParseExact(maxYmd.ToString("yyyy/MM/dd"), "yyyy/MM/dd", null);
            txtStartDate.DateTimeValue = DateTime.ParseExact(maxYmd.ToString("yyyy/MM/dd"), "yyyy/MM/dd", null);
            startDateOldValue = txtStartDate.DateTimeValue;
            endDateOldValue = txtEndDate.DateTimeValue;
         } else {
            txtEndDate.DateTimeValue = DateTime.ParseExact(dtSPN.Rows[0]["SPAN_PERIOD_END_DATE"].AsString(), "yyyyMMdd", null);
            txtStartDate.DateTimeValue = DateTime.ParseExact(dtSPN.Rows[0]["SPAN_PERIOD_START_DATE"].AsString(), "yyyyMMdd", null);
            startDateOldValue = txtStartDate.DateTimeValue;
            endDateOldValue = txtEndDate.DateTimeValue;
         }
         #endregion

         #region Set Drop Down Lsit
         //設定方式
         RepositoryItemLookUpEdit cbxParamType = new RepositoryItemLookUpEdit();
         DataTable cbxParamTypeSource = daoCod.ListByCol2("S0070", "SPAN_PARAM_TYPE");
         cbxParamType.SetColumnLookUp(cbxParamTypeSource, "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor, "");
         gcMain.RepositoryItems.Add(cbxParamType);
         SPAN_PARAM_TYPE.ColumnEdit = cbxParamType;

         //設定值 要顯示user自己輸入的值
         RepositoryItemLookUpEdit cbxParamValue = new RepositoryItemLookUpEdit();
         DataTable cbxParamValueSource = daoCod.ListByCol2("S0070", "SPAN_PARAM_VALUE");
         DataTable dtParamValueData = daoS0071.GetParamData("PL", GlobalInfo.USER_ID);//DB現有資料
         DataTable dtTempParamValue = cbxParamValueSource.Clone();
         for (int i = 0; i < dtParamValueData.Rows.Count; i++) {
            //參數檔案
            dtTempParamValue.Rows.Add();
            dtTempParamValue.Rows[i].SetField("COD_ID", dtParamValueData.Rows[i]["span_param_value"]);
            dtTempParamValue.Rows[i].SetField("COD_DESC", dtParamValueData.Rows[i]["span_param_value"]);
            dtTempParamValue.Rows[i].SetField("CP_DISPLAY", dtParamValueData.Rows[i]["span_param_value"]);

            //CODID = 4 時 顯示 "最大漲跌停"
            if (dtTempParamValue.Rows[i]["COD_ID"].AsString() == "4") {
               dtTempParamValue.Rows[i].SetField("COD_DESC", "最大漲跌停");
            }
         }
         DataView dtDistinc = new DataView(dtTempParamValue);
         dtTempParamValue = dtDistinc.ToTable(true);
         dtTempParamValue.PrimaryKey = new DataColumn[] { dtTempParamValue.Columns["COD_ID"] };
         cbxParamValueSource.PrimaryKey = new DataColumn[] { cbxParamValueSource.Columns["COD_ID"] };
         cbxParamValueSource.Merge(dtTempParamValue, false);
         cbxParamValue.SetColumnLookUp(cbxParamValueSource, "COD_ID", "COD_DESC", TextEditStyles.Standard, "");
         cbxParamValue.ProcessNewValue += new ProcessNewValueEventHandler(cbxParamValue_ProcessNewValue);
         gcMain.RepositoryItems.Add(cbxParamValue);
         SPAN_PARAM_VALUE.ColumnEdit = cbxParamValue;

         //波動度設定
         RepositoryItemLookUpEdit cbxParamVolType = new RepositoryItemLookUpEdit();
         DataTable cbxParamVolTypeSource = daoCod.ListByCol2("S0070", "SPAN_PARAM_VOL_TYPE");
         cbxParamVolType.SetColumnLookUp(cbxParamVolTypeSource, "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor, "");
         gcMain.RepositoryItems.Add(cbxParamVolType);
         SPAN_PARAM_VOL_TYPE.ColumnEdit = cbxParamVolType;

         //商品類別
         DataTable dtProdType = daoS0071.dddw_zparm_comb_prod(txtEndDate.DateTimeValue.ToString("yyyyMMdd"));
         RepositoryItemLookUpEdit cbxProdType = new RepositoryItemLookUpEdit();
         cbxProdType.SetColumnLookUp(dtProdType, "PROD_GROUP_VALUE", "PROD_GROUP", TextEditStyles.DisableTextEditor, "");
         gcMain.RepositoryItems.Add(cbxProdType);
         SPAN_PARAM_CLASS.ColumnEdit = cbxProdType;

         //商品組合
         DataTable dtProd = daoS0071.dddw_zparm_comb_prod_by_group(txtEndDate.DateTimeValue.ToString("yyyyMMdd"), "%%", "%%");
         RepositoryItemLookUpEdit cbxProd = new RepositoryItemLookUpEdit();
         cbxProd.SetColumnLookUp(dtProd, "COMB_PROD_VALUE", "COMB_PROD", TextEditStyles.DisableTextEditor, "");
         gcMain.RepositoryItems.Add(cbxProd);
         SPAN_PARAM_CC.ColumnEdit = cbxProd;

         //製作連動下拉選單(觸發事件)
         gvMain.ShownEditor += GridView_ShownEditor;
         cbxProdType.EditValueChanged += cbxProdType_EditValueChanged;
         #endregion
      }

      protected override ResultStatus Retrieve() {
         base.Retrieve(gcMain);

         #region Set Date Period
         //save 後替換新值
         DataTable dtSPN = daoS0071.GetPeriodByUserId("PL", GlobalInfo.USER_ID);
         if (dtSPN.Rows.Count <= 0) {
            fmYmd = DateTime.Now.AddDays(-60).ToString("yyyyMMdd");
            toYmd = DateTime.Now.ToString("yyyyMMdd");
            maxYmd = new AOCF().GetMaxDate(fmYmd, toYmd);

            txtStartDate.DateTimeValue = DateTime.ParseExact(maxYmd.ToString("yyyy/MM/dd"), "yyyy/MM/dd", null);
            txtEndDate.DateTimeValue = DateTime.ParseExact(maxYmd.ToString("yyyy/MM/dd"), "yyyy/MM/dd", null);
            startDateOldValue = txtStartDate.DateTimeValue;
            endDateOldValue = txtEndDate.DateTimeValue;
         } else {
            txtStartDate.DateTimeValue = DateTime.ParseExact(dtSPN.Rows[0]["SPAN_PERIOD_START_DATE"].AsString(), "yyyyMMdd", null);
            txtEndDate.DateTimeValue = DateTime.ParseExact(dtSPN.Rows[0]["SPAN_PERIOD_END_DATE"].AsString(), "yyyyMMdd", null);
            startDateOldValue = txtStartDate.DateTimeValue;
            endDateOldValue = txtEndDate.DateTimeValue;
         }
         #endregion

         DataTable returnTable = daoS0071.GetParamData("PL", GlobalInfo.USER_ID);
         if (returnTable.Rows.Count == 0) {
            MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
         gcMain.DataSource = returnTable;

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();
         Retrieve();
         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall poke) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();
         ResultStatus resultStatus = ResultStatus.Fail;

         try {
            DataTable dt = (DataTable)gcMain.DataSource;

            if (!checkChanged()) {
               MessageDisplay.Info("沒有變更資料, 不需要存檔!");
               return ResultStatus.FailButNext;
            }

            foreach (DataRow r in dt.Rows) {
               if (r.RowState == DataRowState.Deleted) continue;

               r.SetField("span_param_w_time", DateTime.Now);
               if (r["span_param_type"].AsString() == "2") {
                  switch (r["span_param_value"].AsString()) {
                     case "1":
                     case "2":
                     case "3":
                     case "4": {//最大漲跌停
                        break;
                     }
                     default: {
                        MessageDisplay.Info("設定方式選漲跌停價格, 則設定值請選擇1, 2, 3或最大漲跌停");
                        return ResultStatus.FailButNext;
                     }
                  }
               }
            }

            if (!checkComplete(dt)) return ResultStatus.FailButNext;

            if (savePeriod() != ResultStatus.Success) return ResultStatus.Fail;

            resultStatus = daoS0071.UpdateAllDB(periodTable, dt);
            if (resultStatus == ResultStatus.Fail) {
               MessageDisplay.Info("儲存錯誤!");
               return ResultStatus.Fail;
            }
         } catch (Exception ex) {
            throw ex;
         }
         return resultStatus;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnSave.Enabled = true;
         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnRun.Enabled = true;
         _ToolBtnInsert.Enabled = true;
         _ToolBtnDel.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus RunBefore(PokeBall args) {
         ResultStatus resultStatus = ResultStatus.Fail;

         if (checkChanged()) {
            MessageDisplay.Info("資料有變更, 請先存檔!");
            resultStatus = ResultStatus.FailButNext;
         } else {
            Run(args);
         }
         return resultStatus;
      }

      protected override ResultStatus Run(PokeBall args) {
         if (!checkChanged()) {
            PbFunc.f_bat_span("S0071", "PL", GlobalInfo.USER_ID);
         }
         return base.Run(args);
      }

      protected override ResultStatus InsertRow() {
         base.InsertRow(gvMain);
         //gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      /// <summary>
      /// 組日期區間dataTable
      /// </summary>
      /// <returns></returns>
      private ResultStatus savePeriod() {
         periodTable = daoS0071.GetPeriodByUserId("PL", GlobalInfo.USER_ID);

         if (periodTable.Rows.Count == 0) {
            DataRow dr = periodTable.NewRow();

            dr.SetField("span_period_module", "PL");
            dr.SetField("span_period_user_id", GlobalInfo.USER_ID);

            periodTable.Rows.Add(dr);
         }

         periodTable.Rows[0].SetField("span_period_start_date", txtStartDate.DateTimeValue.ToString("yyyyMMdd"));
         periodTable.Rows[0].SetField("span_period_end_date", txtEndDate.DateTimeValue.ToString("yyyyMMdd"));
         periodTable.Rows[0].SetField("span_period_w_time", DateTime.Now);

         return checkPeriod() ? ResultStatus.Success : ResultStatus.FailButNext;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);
         return ResultStatus.Success;
      }

      private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e) {
         GridView gv = sender as GridView;
         gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["SPAN_PARAM_MODULE"], "PL");
         gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"], 1);
         gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["SPAN_PARAM_USER_ID"], GlobalInfo.USER_ID);
         gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["SPAN_PARAM_EXPIRY"], 0);
      }

      private void btnClear_Click(object sender, EventArgs e) {
         if (MessageDisplay.Choose("確定刪除所有資料?").AsBool()) {
            while (gvMain.DataRowCount != 0)
               gvMain.DeleteRow(0);
         }
      }

      private void GridView_ShownEditor(object sender, EventArgs e) {
         ColumnView view = (ColumnView)sender;
         if (view.FocusedColumn.FieldName == "SPAN_PARAM_CC") {
            string prodType = gvMain.GetFocusedRowCellValue(SPAN_PARAM_CLASS).ToString();
            LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
            DataTable dtProd = new DataTable();
            RepositoryItemLookUpEdit cbxProd = new RepositoryItemLookUpEdit();
            //修改商品組合下拉清單(重綁data source)
            switch (prodType) {
               case "EQT-STF":
               case "EQT-ETF": {
                  string[] prodEQT = prodType.Split('-');
                  dtProd = daoS0071.dddw_zparm_comb_prod_by_group(txtEndDate.DateTimeValue.ToString("yyyyMMdd"), "%" + prodEQT[0] + "%", "%" + prodEQT[1] + "%");
                  break;
               }
               default: {
                  dtProd = daoS0071.dddw_zparm_comb_prod_by_group(txtEndDate.DateTimeValue.ToString("yyyyMMdd"), "%" + prodType + "%", "%%");
                  break;
               }
            }
            cbxProd.SetColumnLookUp(dtProd, "COMB_PROD_VALUE", "COMB_PROD");
            edit.Properties.DataSource = cbxProd.DataSource;
            edit.ShowPopup();
         }
      }

      private void cbxProdType_EditValueChanged(object sender, EventArgs e) {
         gvMain.PostEditor();
         gvMain.SetFocusedRowCellValue("SPAN_PARAM_CC", null);
      }

      private void cbxParamValue_ProcessNewValue(object sender, ProcessNewValueEventArgs e) {
         LookUpEdit lookUpEdit = sender as LookUpEdit;
         DataTable table = lookUpEdit.Properties.DataSource as DataTable;
         string value = e.DisplayValue.ToString();
         table.Rows.Add(new object[] { value, value, table.Rows.Count, value }).ToString();
         table.AcceptChanges();
         e.DisplayValue = value;
         e.Handled = true;
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
            if (dtChange.Rows.Count > 0) {
               return true;
            }
         }

         if (txtStartDate.DateTimeValue != startDateOldValue
                 || txtEndDate.DateTimeValue != endDateOldValue) {
            return true;
         }

         return false;
      }
   }
}