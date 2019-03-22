using System;
using System.Data;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors;
using BusinessObjects;
using BaseGround.Report;
using PhoenixCI.Widget;

namespace PhoenixCI.FormUI.PrefixS {
    public partial class WS0012 : FormParent {
        private DS0012 daoS0012;
        private int countGroup = 3;//設定群組數

        public WS0012(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            GridHelper.SetCommonGrid(gvMain);

            this.Text = _ProgramID + "─" + _ProgramName;
            daoS0012 = new DS0012();
            txtCountDate.DateTimeValue = DateTime.Now;
            adjustmentRadioGroup.SelectedIndex = 0;

            for (int i = 1; i <= countGroup; i++) {

                Control[] formControls = this.Controls.Find("txtDate" + i, true);
                TextDateEdit txt = (TextDateEdit)formControls[0];
                txt.DateTimeValue = DateTime.Now;

                formControls = this.Controls.Find("radioGroup" + i, true);
                RadioGroup radioGroup = (RadioGroup)formControls[0];
                radioGroup.SelectedIndex = 0;

            }
        }

        protected override ResultStatus Retrieve() {
            base.Retrieve(gcMain);
            DataTable dt = new DataTable();

            for (int i = 1; i <= countGroup; i++) {
                string radioSelect = "";
                RadioGroup radios = new RadioGroup();
                Control[] formControls = this.Controls.Find("radioGroup" + i, true);
                RadioGroup radioGroup = (RadioGroup)formControls[0];

                //取得user所選的是第幾列
                radioSelect = radioGroup.Properties.Items[radioGroup.SelectedIndex].Value.AsString().Substring(1);
                DateTime searchDate = GetDateByUserSelect(radioSelect);
                if (dt.Rows.Count == 0) {
                    dt = daoS0012.GetSP1Data(searchDate.ToShortDateString(), i.ToString());
                }

                //不是群組1時進行資料合併
                if (i != 1) {
                    DataTable dtTmp = daoS0012.GetSP1Data(searchDate.ToShortDateString(), i.ToString());
                    dt.Merge(dtTmp);
                }
            }

            if (dt.Rows.Count == 0) {
                MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            gcMain.DataSource = dt;
            gvMain.ExpandAllGroups();

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall poke) {
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();
            ResultStatus resultStatus = ResultStatus.Fail;

            DataTable dt = (DataTable)gcMain.DataSource;
            DataTable dtChange = dt.GetChanges();

            if (dtChange != null) {
                if (dtChange.Rows.Count == 0) {
                    MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return ResultStatus.FailButNext;
                }
                else {
                    if (daoS0012.DeleteSP2S() >= 0) {
                        DataTable insertSP2SData = daoS0012.GetSP2SColumns();
                        for (int i = 0; i < dt.Rows.Count; i++) {
                            insertSP2SData.Rows.Add();
                            insertSP2SData.Rows[i]["SP2S_DATE"] = dt.Rows[i]["SP1_DATE"];
                            insertSP2SData.Rows[i]["SP2S_TYPE"] = dt.Rows[i]["SP1_TYPE"];
                            insertSP2SData.Rows[i]["SP2S_KIND_ID1"] = dt.Rows[i]["SP1_KIND_ID1"];
                            insertSP2SData.Rows[i]["SP2S_KIND_ID2"] = dt.Rows[i]["SP1_KIND_ID2"];
                            insertSP2SData.Rows[i]["SP2S_VALUE_DATE"] = txtCountDate.DateTimeValue;
                            insertSP2SData.Rows[i]["SP2S_OSW_GRP"] = dt.Rows[i]["SP1_OSW_GRP"];
                            insertSP2SData.Rows[i]["SP2S_SPAN_CODE"] = dt.Rows[i]["SP2_SPAN_CODE"];
                            insertSP2SData.Rows[i]["SP2S_ADJ_CODE"] = "Y";
                            insertSP2SData.Rows[i]["SP2S_W_TIME"] = DateTime.Now;
                            insertSP2SData.Rows[i]["SP2S_W_USER_ID"] = GlobalInfo.USER_ID;
                            insertSP2SData.Rows[i]["SP2S_USER_CM"] = dt.Rows[i]["SP1_USER_RATE"].AsDecimal() == 0 ? DBNull.Value : dt.Rows[i]["SP1_USER_RATE"];
                        }
                        resultStatus = daoS0012.updateData(insertSP2SData).Status;//base.Save_Override(insertSP2SData, "SP2S", DBName.CFO);
                        if (resultStatus == ResultStatus.Success) {

                            PrintableComponent = gcMain;
                        }
                    }
                }
            }
            return resultStatus;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnSave.Enabled = true;
            _ToolBtnRetrieve.Enabled = true;
            _ToolBtnPrintAll.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            try {
                ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
                _ReportHelper.Print();//如果有夜盤會特別標註

                return ResultStatus.Success;
            }
            catch (Exception ex) {
                WriteLog(ex);
            }
            return ResultStatus.Fail;
        }

        private void adjustmentRadioGroup_SelectedIndexChanged(object sender, EventArgs e) {
            RadioGroup radios = sender as RadioGroup;

            #region Set SPAN CODE 
            switch (radios.Properties.Items[radios.SelectedIndex].Value.ToString()) {
                case "Clear": {
                    for (int i = 0; i < gvMain.DataRowCount; i++) {
                        gvMain.SetRowCellValue(i, "SP2_SPAN_CODE", " ");
                    }
                    break;
                }
                case "AllSelect": {
                    for (int i = 0; i < gvMain.DataRowCount; i++) {
                        gvMain.SetRowCellValue(i, "SP2_SPAN_CODE", "Y");
                    }
                    break;
                }
                case "1": {
                    for (int i = 0; i < gvMain.DataRowCount; i++) {
                        if (gvMain.GetRowCellValue(i, "GROUP_TYPE").AsString() == "1") {
                            gvMain.SetRowCellValue(i, "SP2_SPAN_CODE", "Y");
                        }
                        else {
                            gvMain.SetRowCellValue(i, "SP2_SPAN_CODE", " ");
                        }
                    }
                    break;
                }
                case "2": {
                    for (int i = 0; i < gvMain.DataRowCount; i++) {
                        if (gvMain.GetRowCellValue(i, "GROUP_TYPE").AsString() == "2") {
                            gvMain.SetRowCellValue(i, "SP2_SPAN_CODE", "Y");
                        }
                        else {
                            gvMain.SetRowCellValue(i, "SP2_SPAN_CODE", " ");
                        }
                    }
                    break;
                }
                case "3": {
                    for (int i = 0; i < gvMain.DataRowCount; i++) {
                        if (gvMain.GetRowCellValue(i, "GROUP_TYPE").AsString() == "3") {
                            gvMain.SetRowCellValue(i, "SP2_SPAN_CODE", "Y");
                        }
                        else {
                            gvMain.SetRowCellValue(i, "SP2_SPAN_CODE", " ");
                        }
                    }
                    break;
                }
                case "ETF": {
                    for (int i = 0; i < gvMain.DataRowCount; i++) {
                        if (gvMain.GetRowCellValue(i, "APDK_PROD_SUBTYPE").AsString() == "S") {
                            gvMain.SetRowCellValue(i, "SP2_SPAN_CODE", "Y");
                        }
                        else {
                            gvMain.SetRowCellValue(i, "SP2_SPAN_CODE", " ");
                        }
                    }
                    break;
                }
                case "Index": {
                    for (int i = 0; i < gvMain.DataRowCount; i++) {
                        if (gvMain.GetRowCellValue(i, "APDK_PROD_SUBTYPE").AsString() == "I") {
                            gvMain.SetRowCellValue(i, "SP2_SPAN_CODE", "Y");
                        }
                        else {
                            gvMain.SetRowCellValue(i, "SP2_SPAN_CODE", " ");
                        }
                    }
                    break;
                }
            }
            #endregion
        }

        /// <summary>
        /// 根據user所選列來確定search Date
        /// </summary>
        /// <param name="userSelect"></param>
        /// <returns></returns>
        private DateTime GetDateByUserSelect(string userSelect) {

            DateTime txtDate = new DateTime();
            Control[] formControls = this.Controls.Find("txtDate" + userSelect, true);
            TextDateEdit txt = (TextDateEdit)formControls[0];
            txtDate = txt.DateTimeValue;

            return txtDate;
        }
    }
}