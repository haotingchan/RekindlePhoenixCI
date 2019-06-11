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
using BaseGround.Widget;

namespace PhoenixCI.FormUI.PrefixS {
   public partial class WS0011 : FormParent {
      private DS0011 daoS0011;
      private int countGroup = 3;//設定群組數

      public WS0011(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         GridHelper.SetCommonGrid(gvMain);

         this.Text = _ProgramID + "─" + _ProgramName;
         daoS0011 = new DS0011();

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
               dt = daoS0011.GetMG1Data(searchDate.ToShortDateString(), i.ToString());
            }

            //不是群組1時進行資料合併
            if (i != 1) {
               DataTable dtTmp = daoS0011.GetMG1Data(searchDate.ToShortDateString(), i.ToString());
               dt.Merge(dtTmp);
            }
         }

         if (dt.Rows.Count == 0) {
            MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
         gcMain.DataSource = dt;
         gvMain.ExpandAllGroups();
         gvMain.BestFitColumns();

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall poke) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();
         ResultStatus resultStatus = ResultStatus.Fail;
         try {
            DataTable dt = (DataTable)gcMain.DataSource;
            DataTable dtChange = dt.GetChanges();

            if (dtChange == null) {
               MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
               return ResultStatus.FailButNext;
            }

            //先刪除舊資料後, 再新增資料
            if (daoS0011.DeleteMG2S() >= 0) {
               DataTable insertMG2SData = daoS0011.GetMG2SColumns();
               for (int i = 0; i < dt.Rows.Count; i++) {
                  insertMG2SData.Rows.Add();
                  insertMG2SData.Rows[i]["MG2S_DATE"] = dt.Rows[i]["MG1_DATE"];
                  insertMG2SData.Rows[i]["MG2S_KIND_ID"] = dt.Rows[i]["MG1_KIND_ID"];
                  insertMG2SData.Rows[i]["MG2S_VALUE_DATE"] = txtCountDate.DateTimeValue;
                  insertMG2SData.Rows[i]["MG2S_OSW_GRP"] = dt.Rows[i]["MG1_OSW_GRP"];
                  insertMG2SData.Rows[i]["MG2S_ADJ_CODE"] = "Y";
                  insertMG2SData.Rows[i]["MG2S_W_TIME"] = DateTime.Now;
                  insertMG2SData.Rows[i]["MG2S_W_USER_ID"] = GlobalInfo.USER_ID;
                  insertMG2SData.Rows[i]["MG2S_SPAN_CODE"] = dt.Rows[i]["MG2_SPAN_CODE"];
                  insertMG2SData.Rows[i]["MG2S_USER_CM"] = dt.Rows[i]["USER_CM"].AsDecimal() == 0 ? DBNull.Value : dt.Rows[i]["USER_CM"];
               }
               resultStatus = daoS0011.updateData(insertMG2SData).Status;
               if (resultStatus == ResultStatus.Success) {

                  //儲存備份 pdf
                  ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
                  CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
                  reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
                  reportLandscape.IsHandlePersonVisible = false;
                  reportLandscape.IsManagerVisible = false;
                  _ReportHelper.Create(reportLandscape);

                  _ReportHelper.Export(FileType.PDF, _ReportHelper.FilePath);
               }
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
         _ToolBtnPrintAll.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
            CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
            reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            _ReportHelper.Create(reportLandscape);

            _ReportHelper.Print();//如果有夜盤會特別標註
            _ReportHelper.Export(FileType.PDF, _ReportHelper.FilePath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      private void adjustmentRadioGroup_SelectedIndexChanged(object sender, EventArgs e) {
         RadioGroup radios = sender as RadioGroup;

         #region Set SPAN CODE / Radio Group 改變時改變cell 值
         switch (radios.Properties.Items[radios.SelectedIndex].Value.ToString()) {
            case "Clear": {
               for (int i = 0; i < gvMain.DataRowCount; i++) {
                  gvMain.SetRowCellValue(i, "MG2_SPAN_CODE", " ");
               }
               break;
            }
            case "AllSelect": {
               for (int i = 0; i < gvMain.DataRowCount; i++) {
                  gvMain.SetRowCellValue(i, "MG2_SPAN_CODE", "Y");
               }
               break;
            }
            case "1": {
               for (int i = 0; i < gvMain.DataRowCount; i++) {
                  if (gvMain.GetRowCellValue(i, "GROUP_TYPE").AsString() == "1") {
                     gvMain.SetRowCellValue(i, "MG2_SPAN_CODE", "Y");
                  } else {
                     gvMain.SetRowCellValue(i, "MG2_SPAN_CODE", " ");
                  }
               }
               break;
            }
            case "2": {
               for (int i = 0; i < gvMain.DataRowCount; i++) {
                  if (gvMain.GetRowCellValue(i, "GROUP_TYPE").AsString() == "2") {
                     gvMain.SetRowCellValue(i, "MG2_SPAN_CODE", "Y");
                  } else {
                     gvMain.SetRowCellValue(i, "MG2_SPAN_CODE", " ");
                  }
               }
               break;
            }
            case "3": {
               for (int i = 0; i < gvMain.DataRowCount; i++) {
                  if (gvMain.GetRowCellValue(i, "GROUP_TYPE").AsString() == "3") {
                     gvMain.SetRowCellValue(i, "MG2_SPAN_CODE", "Y");
                  } else {
                     gvMain.SetRowCellValue(i, "MG2_SPAN_CODE", " ");
                  }
               }
               break;
            }
            case "ETF": {
               for (int i = 0; i < gvMain.DataRowCount; i++) {
                  if (gvMain.GetRowCellValue(i, "MG1_PROD_SUBTYPE").AsString() == "S") {
                     gvMain.SetRowCellValue(i, "MG2_SPAN_CODE", "Y");
                  } else {
                     gvMain.SetRowCellValue(i, "MG2_SPAN_CODE", " ");
                  }
               }
               break;
            }
            case "Index": {
               for (int i = 0; i < gvMain.DataRowCount; i++) {
                  if (gvMain.GetRowCellValue(i, "MG1_PROD_SUBTYPE").AsString() == "I") {
                     gvMain.SetRowCellValue(i, "MG2_SPAN_CODE", "Y");
                  } else {
                     gvMain.SetRowCellValue(i, "MG2_SPAN_CODE", " ");
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