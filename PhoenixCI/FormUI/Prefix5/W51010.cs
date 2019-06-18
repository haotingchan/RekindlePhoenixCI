using System;
using System.Data;
using System.ComponentModel;
using BaseGround;
using BaseGround.Shared;
using BaseGround.Report;
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using Common;
using DevExpress.XtraGrid.Views.Grid;
using BusinessObjects;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace PhoenixCI.FormUI.Prefix5 {
   public partial class W51010 : FormParent {
      private string disableCol = "DTS_DATE";

      private D51010 dao51010;
      private RepositoryItemLookUpEdit _RepLookUpEdit;
      private RepositoryItemLookUpEdit _RepLookUpEdit2;

      public W51010(string programID, string programName) : base(programID, programName) {
         InitializeComponent();

         dao51010 = new D51010();
         GridHelper.SetCommonGrid(gvMain);
         PrintableComponent = gcMain;
         TXTStartDate.DateTimeValue = Convert.ToDateTime(GlobalInfo.OCF_DATE.Year + "/01/01");
         TXTEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
         this.Text = _ProgramID + "─" + _ProgramName;

         //日期類別 是否交易 兩個欄位要換成LookUpEdit
         List<LookupItem> workTime = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "W", DisplayMember = "W:上班日"},
                                        new LookupItem() { ValueMember = "H", DisplayMember = "H:假日"}};

         _RepLookUpEdit = new RepositoryItemLookUpEdit();
         _RepLookUpEdit.SetColumnLookUp(workTime, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         gcMain.RepositoryItems.Add(_RepLookUpEdit);
         DTS_DATE_TYPE.ColumnEdit = _RepLookUpEdit;

         List<LookupItem> isTransation = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "Y", DisplayMember = "Y"},
                                        new LookupItem() { ValueMember = "N", DisplayMember = "N"}};

         _RepLookUpEdit2 = new RepositoryItemLookUpEdit();
         _RepLookUpEdit2.SetColumnLookUp(isTransation, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         gcMain.RepositoryItems.Add(_RepLookUpEdit2);
         DTS_WORK.ColumnEdit = _RepLookUpEdit2;

         Retrieve();
      }

      protected override ResultStatus Retrieve() {
         base.Retrieve(gcMain);
         DataTable returnTable = new DataTable();

         returnTable = dao51010.GetData(TXTStartDate.DateTimeValue.ToString("yyyyMMdd"), TXTEndDate.DateTimeValue.ToString("yyyyMMdd"));
         if (returnTable.Rows.Count == 0) {
            MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
         }
         gcMain.DataSource = returnTable;

         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         base.InsertRow(gvMain);
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall poke) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         try {
            DataTable dt = (DataTable)gcMain.DataSource;

            if (!checkComplete(dt)) return ResultStatus.FailButNext;

            DataTable dtChange = dt.GetChanges();
            if (dtChange == null) {
               MessageDisplay.Info("沒有變更資料, 不需要存檔!");
               return ResultStatus.FailButNext;
            }

            if (!checkDuplicate(dt.GetChanges(DataRowState.Added))) return ResultStatus.FailButNext;

            ResultData result = dao51010.UpdateData(dt);
            if (result.Status == ResultStatus.Fail) {
               MessageDisplay.Error("儲存失敗");
               return ResultStatus.Fail;
            }

         } catch (Exception ex) {
            throw ex;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
            _ReportHelper.LeftMemo = "查詢日期 : " + TXTStartDate.DateTimeValue.ToShortDateString() + "~" + TXTEndDate.DateTimeValue.ToShortDateString();
            _ReportHelper.Print();//如果有夜盤會特別標註
            _ReportHelper.Export(FileType.PDF, _ReportHelper.FilePath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = true;
         _ToolBtnSave.Enabled = true;
         _ToolBtnDel.Enabled = true;
         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);
         return ResultStatus.Success;
      }

      private bool checkComplete(DataTable dtSource) {

         foreach (DataColumn column in dtSource.Columns) {
            if (dtSource.Rows.OfType<DataRow>().Where(r => r.RowState != DataRowState.Deleted).Any(r => r.IsNull(column))) {
               MessageDisplay.Error("尚未填寫完成");
               return false;
            }
         }
         return true;
      }

      private bool checkDuplicate(DataTable dtAdd) {

         foreach (DataRow dr in dtAdd.Rows) {
            string dtTmp = dao51010.GetDuplicate(Convert.ToDateTime(dr["dts_date"]).ToShortDateString());

            if (!string.IsNullOrEmpty(dtTmp)) {
               MessageDisplay.Error("不可設定重複日期 !");
               return false;
            }
         }
         return true;
      }

      private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"]) == null ? "0" :
              gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"], 1);
         } else if (gv.FocusedColumn.FieldName == disableCol) {
            e.Cancel = true;
         }
      }

      private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["IS_NEWROW"]) == null ? "0" :
              gv.GetRowCellValue(e.RowHandle, gv.Columns["IS_NEWROW"]).ToString();

         if (e.Column.FieldName == disableCol) {
            e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.Silver;
         }
      }

      private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e) {
         GridView gv = sender as GridView;
         gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"], 1);
      }
   }
}