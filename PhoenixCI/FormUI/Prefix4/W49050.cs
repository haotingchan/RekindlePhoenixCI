using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;

/// <summary>
/// Winni, 2019/4/12
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

   /// <summary>
   /// 49050 重大事件前5交易日設定
   /// </summary>
   public partial class W49050 : FormParent {

      private D49050 dao49050;

      public W49050(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtEndYear.Text = GlobalInfo.OCF_DATE.ToString("yyyy");
         txtStartYear.Text = txtEndYear.Text;
         dao49050 = new D49050();
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            DataTable dt = dao49050.GetDataList();

            //1. 設定欄位
            RepositoryItemTextEdit memo = new RepositoryItemTextEdit(); //說明
            gcMain.RepositoryItems.Add(memo);
            gvMain.Columns["MGT3_MEMO"].ColumnEdit = memo;
            memo.MaxLength = 30;

            //2. 設定gvMain
            gcMain.Visible = true;
            gcMain.DataSource = dt;
            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);
            gvMain.Columns["MGT3_MEMO"].Width = 600;
            gvMain.Columns["MGT3_DATE_FM"].Width = 120;
            gvMain.Columns["MGT3_DATE_TO"].Width = 120;
            gcMain.Focus();

            if (dt.Rows.Count <= 0) {
               InsertRow();
            }

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
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
         try {

            #region 輸入&日期檢核
            if (string.Compare(txtStartYear.Text , txtEndYear.Text) > 0) {
               MessageDisplay.Error("起始年度不可大於迄止年度!" , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            #endregion

            DataTable dt = dao49050.GetDataList();

            //1. 設定欄位
            RepositoryItemTextEdit memo = new RepositoryItemTextEdit(); //說明
            gcMain.RepositoryItems.Add(memo);
            gvMain.Columns["MGT3_MEMO"].ColumnEdit = memo;
            memo.MaxLength = 30;

            //2. 設定gvMain
            gcMain.Visible = true;
            gcMain.DataSource = dt;
            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);
            gvMain.Columns["MGT3_MEMO"].Width = 600;
            gvMain.Columns["MGT3_DATE_FM"].Width = 120;
            gvMain.Columns["MGT3_DATE_TO"].Width = 120;
            gcMain.Focus();

            //3.check (沒有資料時,則自動新增一筆)，放在gcMain.DataSource = dt後面才能帶入column
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(MessageDisplay.MSG_NO_DATA , GlobalInfo.ResultText);
               InsertRow();
            }

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {
         try {

            DataTable dtCurrent = (DataTable)gcMain.DataSource;
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            DataTable dtChange = dtCurrent.GetChanges();
            DataTable dtForAdd = dtCurrent.GetChanges(DataRowState.Added);
            DataTable dtForModified = dtCurrent.GetChanges(DataRowState.Modified);
            DataTable dtForDeleted = dtCurrent.GetChanges(DataRowState.Deleted);

            if (dtCurrent != null) {
               foreach (DataRow drCheck in dtCurrent.Rows) {
                  for (int w = 0 ; w < dtCurrent.Rows.Count ; w++) {
                     int x = dtCurrent.Columns.Count;
                     for (int y = 0 ; y < x ; y++) {
                        if (string.IsNullOrEmpty(drCheck[y].AsString())) {
                           MessageDisplay.Error("資料尚未填寫完成" , GlobalInfo.ErrorText);
                           return ResultStatus.FailButNext;
                        }
                     }
                  }
               }
            }

            if (dtChange == null) {
               MessageDisplay.Warning("沒有變更資料,不需要存檔!" , GlobalInfo.WarningText);
               return ResultStatus.Fail;
            }
            if (dtChange.Rows.Count == 0) {
               MessageDisplay.Warning("沒有變更資料,不需要存檔!" , GlobalInfo.WarningText);
               return ResultStatus.Fail;
            }

            foreach (DataRow dr in dtCurrent.Rows) {
               if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified) {
                  dr["MGT3_W_TIME"] = DateTime.Now;
                  dr["MGT3_W_USER_ID"] = GlobalInfo.USER_ID;
               }
            }

            dtChange = dtCurrent.GetChanges();
            ResultData result = new MGT3().UpdateData(dtChange);
            if (result.Status == ResultStatus.Fail) {
               MessageDisplay.Error("儲存失敗" , GlobalInfo.ErrorText);
               return ResultStatus.FailButNext;
            }

         } catch (Exception ex) {
            WriteLog(ex);
         }
         _IsPreventFlowPrint = true; //不要自動列印
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         ReportHelper _ReportHelper = new ReportHelper(gcMain , _ProgramID , this.Text);
         CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
         reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
         reportLandscape.IsHandlePersonVisible = false;
         reportLandscape.IsManagerVisible = false;
         _ReportHelper.Create(reportLandscape);

         _ReportHelper.Print();
         _ReportHelper.Export(FileType.PDF , _ReportHelper.FilePath);
         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         DataTable dt = (DataTable)gcMain.DataSource;
         gvMain.AddNewRow();

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["IS_NEWROW"] , 1);

         //base.InsertRow(gvMain);
         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.VisibleColumns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);
         return ResultStatus.Success;
      }

      /// <summary>
      /// write string to txt
      /// </summary>
      /// <param name="source"></param>
      /// <param name="filePath"></param>
      /// <param name="encoding">System.Text.Encoding.GetEncoding(950)</param>
      /// <returns></returns>
      protected bool ToText(string source , string filePath , System.Text.Encoding encoding) {
         try {
            FileStream fs = new FileStream(filePath , FileMode.Create);
            StreamWriter str = new StreamWriter(fs , encoding);
            str.Write(source);

            str.Flush();
            str.Close();
            return true;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return false;
      }

      //#region GridControl事件

      /// <summary>
      /// 決定哪些欄位無法編輯的事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"]) == null ? "0" :
              gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false;
            gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"] , 1);
         }
         //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
         else if (gv.FocusedColumn.Name == "MGT3_DATE_FM" || gv.FocusedColumn.Name == "MGT3_DATE_TO") {
            e.Cancel = true;
         } else {
            e.Cancel = false;
         }
      }

      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         //要用RowHandle不要用FocusedRowHandle
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["IS_NEWROW"]) == null ? "0" :
                   gv.GetRowCellValue(e.RowHandle , gv.Columns["IS_NEWROW"]).ToString();

         //描述每個欄位,在is_newRow時候要顯示的顏色
         //當該欄位不可編輯時,設定為灰色 Color.FromArgb(192,192,192)
         //當該欄位不可編輯時,AllowFocus為false(PB的wf_set_order方法)
         switch (e.Column.FieldName) {
            case ("MGT3_DATE_FM"):
            case ("MGT3_DATE_TO"):
               e.Column.OptionsColumn.AllowFocus = Is_NewRow == "1" ? true : false;
               e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(192 , 192 , 192);
               break;
            default:
               e.Appearance.BackColor = Color.White;
               break;
         }//switch (e.Column.FieldName) 
      }

      private void gvMain_InitNewRow(object sender , InitNewRowEventArgs e) {
         GridView gv = sender as GridView;
         gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"] , 1);

      }
      //#endregion

   }
}