﻿using System;
using System.Data;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using BaseGround.Shared;
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using System.IO;

namespace PhoenixCI.FormUI.Prefix4 {
   public partial class W40041 : FormParent {
      private D40041 dao40041;

      public W40041(string programID, string programName) : base(programID, programName) {
         dao40041 = new D40041();

         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = DateTime.Now;

         oswGrpLookItem.SetDataTable(new OCFG().ListAll(), "OSW_GRP", "OSW_GRP_NAME", DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor, null);
         oswGrpLookItem.EditValue = "1";

         prodLookItem.SetDataTable(new COD().ListByCol2("40041", "Prod_ID"), "COD_ID", "COD_DESC", DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor, null);
         prodLookItem.EditValue = "Y";

         ExportShow.Hide();
      }

      protected override ResultStatus Retrieve() {
         string diffDays = dao40041.DiffOcfDays(txtDate.DateTimeValue);
         string changeFlag = prodLookItem.EditValue.AsString();
         string oswGrp = oswGrpLookItem.EditValue.AsString() + "%";
         string[] colCaption = { "勾選", "契約名稱", "標的代碼", "上次調整公告日", "資料起日", "資料迄日", "資料筆數", "", "", "","" };

         DataTable dt = new DataTable();
         dt = dao40041.ListData(changeFlag, txtDate.DateTimeValue, oswGrp);
         gcMain.DataSource = dt;

         if (dt == null) {
            MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
            _ToolBtnExport.Enabled = false;
            return ResultStatus.Fail;
         }

         if (dt.Rows.Count == 0) {
            MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
            _ToolBtnExport.Enabled = false;
            return ResultStatus.Fail;
         }

         foreach (DataColumn dc in dt.Columns) {
            gvMain.SetColumnCaption(dc.ColumnName, colCaption[dt.Columns.IndexOf(dc)]);
            gvMain.Columns[dc.ColumnName].OptionsColumn.AllowEdit = false;
         }

         //第一欄為 checkbox
         RepositoryItemCheckEdit itemCheckEditForChangeFlag = new RepositoryItemCheckEdit();
         itemCheckEditForChangeFlag.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
         itemCheckEditForChangeFlag.ValueChecked = "Y";
         itemCheckEditForChangeFlag.ValueUnchecked = "N";
         gcMain.RepositoryItems.Add(itemCheckEditForChangeFlag);
         gvMain.Columns[0].ColumnEdit = itemCheckEditForChangeFlag;
         //勾選 資料起日  可編輯
         gvMain.Columns[0].OptionsColumn.AllowEdit = true;
         gvMain.Columns[4].OptionsColumn.AllowEdit = true;
         gvMain.Columns[7].Visible = false;
         gvMain.Columns[8].Visible = false;
         gvMain.Columns[9].Visible = false;
         gvMain.Columns[10].Visible = false;

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         ExportShow.Text = "轉檔中...";
         ExportShow.Show();

         try {
            DataTable exportDt = (DataTable)gcMain.DataSource;
            exportDt = exportDt.Filter("RUN_FLAG ='Y'");

            Workbook workbook = new Workbook();

            foreach (DataRow exportDr in exportDt.Rows) {
               string kindId = exportDr["MG1_KIND_ID"].AsString();
               string subType = exportDr["MG1_PROD_SUBTYPE"].AsString();
               string prodType = exportDr["MG1_PROD_TYPE"].AsString();
               string newFileName = _ProgramID + "(" + kindId + ")_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".xlsx";
               string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID, newFileName);
               int sheetIndex = prodType == "O" ? 2 : 0;

               destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, destinationFilePath);

               workbook.LoadDocument(destinationFilePath);

               #region Write Data

               Worksheet worksheet = workbook.Worksheets[sheetIndex];

               DataTable importData = dao40041.GetExportData(exportDr["MG1_KIND_ID"].AsString(),
                  exportDr["DATA_SDATE"].AsDateTime(), txtDate.DateTimeValue);

               if (importData == null) {
                  MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
                  return ResultStatus.Fail;
               }

               if (importData.Rows.Count == 0) {
                  MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
                  return ResultStatus.Fail;
               }

               DataRow firstRow = importData.Select("ROWNUM =1")[0];

               worksheet.Cells[2, 2].Value = prodType == "O" ? kindId : kindId + "結算價";
               worksheet.Cells[0, 1].Value = firstRow["MG6_DATE"].AsDateTime();

               for (int i = 2; i <= importData.Columns.Count - 1; i++) {
                  worksheet.Cells[0, i].Value = firstRow[i].AsDecimal();
               }
               worksheet.Import(importData, false, 3, 0);

               Range emptyRa = worksheet.Range[(importData.Rows.Count + 4).ToString() + ":1003"];
               emptyRa.Delete(DeleteMode.EntireRow);

               #endregion

               #region Gen Figure
               sheetIndex = prodType == "O" ? 3 : 1;

               worksheet = workbook.Worksheets[sheetIndex];

               if (subType != "S") {
                  Range ra = worksheet.Range["4:5"];
                  ra.Delete(DeleteMode.EntireRow);
               } else {
                  worksheet.Cells[4, 1].Value = exportDr["APDK_NAME"].AsString();
                  worksheet.Cells[4, 2].Value = exportDr["APDK_STOCK_ID"].AsString();
                  worksheet.Cells[4, 3].Value = exportDr["PID_NAME"].AsString();

                  Range ra = worksheet.Range["2:3"];
                  ra.Delete(DeleteMode.EntireRow);
               }

               //表頭日期
               worksheet.Cells[3, 8].Value = importData.Rows[importData.Rows.Count - 1]["MG6_DATE"].AsDateTime();

               int count = 2;
               for (int f = 7; f >= 6; f--) {
                  DataRow dataRow = importData.Rows[importData.Rows.Count - count];
                  count--;

                  worksheet.Cells[f, 1].Value = dataRow[3].AsDecimal();
                  worksheet.Cells[f, 3].Value = dataRow[2].AsDecimal();
                  worksheet.Cells[f, 5].Value = dataRow[4].AsDecimal();
                  worksheet.Cells[f, 6].Value = dataRow[5].AsDecimal();
                  worksheet.Cells[f, 7].Value = dataRow[6].AsDecimal();
                  worksheet.Cells[f, 8].Value = dataRow[7].AsDecimal();
                  worksheet.Cells[f, 9].Value = dataRow[8].AsDecimal();
               }

               #endregion

               workbook.SaveDocument(destinationFilePath);
            }
         } catch (Exception ex) {
            ExportShow.Text = "轉檔失敗";
            throw ex;
         }
         ExportShow.Text = "轉檔成功!";
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();
         _ToolBtnRetrieve.Enabled = true;

         return ResultStatus.Success;
      }

      private void radioGroup1_Properties_EditValueChanged(object sender, EventArgs e) {
         RadioGroup radioGroup = sender as RadioGroup;

         switch (radioGroup.EditValue.AsString()) {
            case "ALL": {
               for (int i = 0; i < gvMain.DataRowCount; i++) {
                  gvMain.SetRowCellValue(i, "RUN_FLAG", "Y");
               }
               break;
            }
            case "Cancel": {
               for (int i = 0; i < gvMain.DataRowCount; i++) {
                  gvMain.SetRowCellValue(i, "RUN_FLAG", "N");
               }
               break;
            }
            case "Index": {
               for (int i = 0; i < gvMain.DataRowCount; i++) {
                  if (gvMain.GetRowCellValue(i, "MG1_PROD_SUBTYPE").AsString() == "I") {
                     gvMain.SetRowCellValue(i, "RUN_FLAG", "Y");
                  } else {
                     gvMain.SetRowCellValue(i, "RUN_FLAG", " ");
                  }
               }
               break;
            }
            case "ETF": {
               for (int i = 0; i < gvMain.DataRowCount; i++) {
                  if (gvMain.GetRowCellValue(i, "MG1_PROD_SUBTYPE").AsString() == "E") {
                     gvMain.SetRowCellValue(i, "RUN_FLAG", "Y");
                  } else {
                     gvMain.SetRowCellValue(i, "RUN_FLAG", " ");
                  }
               }
               break;
            }
         }
      }
   }
}