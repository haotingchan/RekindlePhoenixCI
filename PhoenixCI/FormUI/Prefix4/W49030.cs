using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;

/// <summary>
/// Winni, 2019/5/20
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

   /// <summary>
   /// 49030 保證金調整條件設定
   /// </summary>
   public partial class W49030 : FormParent {

      private ReportHelper _ReportHelper;
      protected DataTable dtForDeleted;

      public W49030(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         GridHelper.SetCommonGrid(gvMain);
         dtForDeleted = new DataTable();

         MessageDisplay.Info("執行完成!");
      }

      protected override ResultStatus Open() {
         base.Open();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = true;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = true;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = true;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = true;//列印

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {

         return ResultStatus.Success;
      }
     
      protected override ResultStatus Save(PokeBall pokeBall) {

         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();
         ResultStatus resultStatus = ResultStatus.Fail;

         DataTable dt = (DataTable)gcMain.DataSource;
         DataTable dtChange = dt.GetChanges();
         DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
         DataTable dtForModified = dt.GetChanges(DataRowState.Modified);

         if (dtChange != null) {
            if (dtChange.Rows.Count == 0) {
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            } else {
               foreach (DataRow dr in dt.Rows) {
                  if (dr.RowState == DataRowState.Added) {

                     foreach (DataRow drAdd in dtForAdd.Rows) {
                        for (int w = 0 ; w < dtForAdd.Rows.Count ; w++) {
                           if (string.IsNullOrEmpty(drAdd[w].AsString())) {
                              MessageDisplay.Info("新增資料欄位不可為空!");
                              return ResultStatus.Fail;
                           }
                        }
                     }

                     dr["MGT4_W_TIME"] = DateTime.Now;
                     dr["MGT4_W_USER_ID"] = GlobalInfo.USER_ID;
                  }
                  if (dr.RowState == DataRowState.Modified) {
                     dr["MGT4_W_TIME"] = DateTime.Now;
                     dr["MGT4_W_USER_ID"] = GlobalInfo.USER_ID;
                  }
               }

               dt.Columns.Remove("OP_TYPE");
               dt.Columns.Remove("Is_NewRow");
               ResultData result = new MGT4().UpdateData(dt);//base.Save_Override(dt, "MGT4");
               if (result.Status == ResultStatus.Fail) {
                  return ResultStatus.Fail;
               }

            }

            if (resultStatus == ResultStatus.Success) {

               PrintableComponent = gcMain;
            }
         }

         //不要自動列印
         _IsPreventFlowPrint = true;
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         _ReportHelper = reportHelper;
         CommonReportPortraitA4 report = new CommonReportPortraitA4();
         report.printableComponentContainerMain.PrintableComponent = gcMain;
         _ReportHelper.Create(report);

         base.Print(_ReportHelper);
         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {

         gvMain.AddNewRow();
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["Is_NewRow"] , 1);

         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         GridView gv = gvMain as GridView;
         DataRowView deleteRowView = (DataRowView)gv.GetFocusedRow();
         dtForDeleted.ImportRow(deleteRowView.Row);
         base.DeleteRow(gvMain);
         return ResultStatus.Success;
      }

      #region GridControl事件

      /// <summary>
      /// 決定哪些欄位無法編輯的事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
              gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false;
            gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"] , 1);
         }
         //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
         else if (gv.FocusedColumn.Name == "AM7T_Y" || gv.FocusedColumn.Name == "AM7T_PARAM_KEY") {
            e.Cancel = true;
         } else {
            e.Cancel = false;
         }

      }

      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         //要用RowHandle不要用FocusedRowHandle
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
                            gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]).ToString();

         //描述每個欄位,在is_newRow時候要顯示的顏色
         //當該欄位不可編輯時,設定為灰色 Color.FromArgb(192,192,192)
         //當該欄位不可編輯時,AllowFocus為false(PB的wf_set_order方法)
         switch (e.Column.FieldName) {
            case ("MGT4_KIND_ID"):
            case ("MGT4_TYPE"):
               e.Column.OptionsColumn.AllowFocus = Is_NewRow == "1" ? true : false;
               e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(192 , 192 , 192);
               break;
            case ("MGT4_M_MULTI"):
            case ("MGT4_I_MULTI"):
            case ("MGT4_DIGITAL"):
            case ("MGT4_M_DIGITAL"):
               e.Column.AppearanceHeader.ForeColor = Color.Blue;
               break;
            default:
               e.Appearance.BackColor = Color.White;
               break;
         }//switch (e.Column.FieldName) {

      }

      #endregion
   }
}