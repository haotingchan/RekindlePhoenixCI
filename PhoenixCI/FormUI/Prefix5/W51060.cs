using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using BaseGround;
using BaseGround.Report;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using DevExpress.XtraGrid.Views.Grid;
using BusinessObjects;
using System.Linq;

namespace PhoenixCI.FormUI.Prefix5 {
   public partial class W51060 : FormParent {
      private string allowCol = "MMIQ_INVALID_QNTY";
      private string disableCol = "MMIQ_YM";
      private D51060 dao51060;

      public W51060(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         GridHelper.SetCommonGrid(gvMain);
         dao51060 = new D51060();
         this.Text = _ProgramID + "─" + _ProgramName;

         txtYM.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
      }

      protected override ResultStatus Retrieve() {
         base.Retrieve(gcMain);
         DataTable returnTable = new DataTable();
         returnTable = dao51060.GetData(txtYM.DateTimeValue.ToString("yyyyMM"));
         if (returnTable.Rows.Count == 0) {
            MessageDisplay.Info("無任何資料");
         }
         //returnTable.Columns.Add("Is_NewRow", typeof(string));
         gcMain.DataSource = returnTable;

         gcMain.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall poke) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();
         ResultStatus resultStatus = ResultStatus.Fail;

         try {
            DataTable dt = (DataTable)gcMain.DataSource;

            if (!checkComplete(dt)) return ResultStatus.Fail;

            DataTable dtChange = dt.GetChanges();
            DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
            DataTable dtForModified = dt.GetChanges(DataRowState.Modified);

            ResultData resultData = new ResultData();
            resultData.ChangedDataViewForAdded = dtForAdd == null ? new DataView() : dtForAdd.DefaultView;
            resultData.ChangedDataViewForModified = dtForModified == null ? new DataView() : dtForModified.DefaultView;

            if (dtChange == null) {
               MessageDisplay.Info("沒有變更資料, 不需要存檔!");
               return ResultStatus.FailButNext;
            }
            string result = dao51060.ExecuteStoredProcedure(txtYM.DateTimeValue.ToString("yyyyMMdd"));
            if (result == "0") {
               foreach (DataRow r in dt.Rows) {
                  if (r.RowState != DataRowState.Deleted) {
                     r["MMIQ_W_TIME"] = DateTime.Now;
                     r["MMIQ_W_USER_ID"] = GlobalInfo.USER_ID;
                  }
                  if (Equals(0, r["MMIQ_INVALID_QNTY"])) {
                     r.Delete();
                  }
               }
               resultStatus = dao51060.updateData(dt).Status;//base.Save_Override(dt, "MMIQ");
               if (resultStatus == ResultStatus.Fail) {
                  MessageDisplay.Error("儲存失敗");
                  return ResultStatus.Fail;
               }
            }
            PrintOrExportChangedByKen(gcMain, dtForAdd, null, dtForModified);
         } catch (Exception ex) {
            throw ex;
         }
         return resultStatus;
      }

      protected override ResultStatus InsertRow() {
         base.InsertRow(gvMain);
         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
            _ReportHelper.LeftMemo = "查詢條件 : " + txtYM.DateTimeValue.ToShortDateString();

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

      /// <summary>
      /// 判斷是否是新增列, 若是新增列開啟編輯
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"]) == null ? "0" :
              gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"], 1);
         } else if (gv.FocusedColumn.FieldName != allowCol) {
            e.Cancel = true;
         }
      }

      private void gvMain_NewRowAllowEdit(object sender, InitNewRowEventArgs e) {
         GridView gv = sender as GridView;
         gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["MMIQ_YM"], txtYM.Text.Replace("/", ""));
         gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["MMIQ_W_TIME"], GlobalInfo.OCF_DATE);
         gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["MMIQ_W_USER_ID"], GlobalInfo.USER_ID);
         gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["IS_NEWROW"], 1);
      }

      /// <summary>
      /// 新增列改變顏色
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["IS_NEWROW"]) == null ? "0" :
              gv.GetRowCellValue(e.RowHandle, gv.Columns["IS_NEWROW"]).ToString();

         if (e.Column.FieldName != allowCol) {
            e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.Silver;
         }

         if (e.Column.FieldName == disableCol) {
            e.Appearance.BackColor = Color.Silver;
         }
      }
   }
}