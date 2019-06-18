using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

/// <summary>
/// Winni, 2019/3/25
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

   /// <summary>
   /// 49070 SPAN參數名稱設定
   /// </summary>
   public partial class W49070 : FormParent {

      private RepositoryItemLookUpEdit lupOswGrp;
      private RepositoryItemLookUpEdit lupDataType;
      private D49070 dao49070;
      private SPT1 daoSPT1;

      public W49070(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao49070 = new D49070();
         daoSPT1 = new SPT1();
      }

      protected override ResultStatus Open() {
         base.Open();
         try {
            lupOswGrp = new RepositoryItemLookUpEdit();
            lupDataType = new RepositoryItemLookUpEdit();

            //收盤群組
            List<LookupItem> dtOswGrp = new List<LookupItem>(){
                                            new LookupItem() { ValueMember = " ", DisplayMember = " "},
                                            new LookupItem() { ValueMember = "1", DisplayMember = "Group 1"},
                                            new LookupItem() { ValueMember = "5", DisplayMember = "Group 2"}};
            lupOswGrp.SetColumnLookUp(dtOswGrp , "ValueMember" , "DisplayMember" , TextEditStyles.DisableTextEditor , null);
            gcMain.RepositoryItems.Add(lupOswGrp);

            //商品狀態
            List<LookupItem> dtDataType = new List<LookupItem>(){
                                            new LookupItem() { ValueMember = " ", DisplayMember = " "},
                                            new LookupItem() { ValueMember = "E", DisplayMember = "下市"}};
            lupDataType.SetColumnLookUp(dtDataType , "ValueMember" , "DisplayMember" , TextEditStyles.DisableTextEditor , null);
            gcMain.RepositoryItems.Add(lupDataType);

            Retrieve();
            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
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
            DataTable dt = dao49070.ListData();

            //0.check (沒有資料時,則自動新增一筆)
            if (dt.Rows.Count <= 0) {
               InsertRow();
            }

            //1. 設定gvMain
            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dt;
            GridHelper.SetCommonGrid(gvMain);

            string[] showColCaption = {"商品", "商品", "簡稱","","順序","","",
                                       $"對外{Environment.NewLine}商品",$"對外{Environment.NewLine}商品",
                                       "適用商品組合" ,$"收盤{Environment.NewLine}群組", $"判斷{Environment.NewLine}調整標準",
                                       $"商品{Environment.NewLine}狀態", $"跨商品{Environment.NewLine}MAX折抵比率", "" };

            //1.1 設定欄位caption       
            foreach (DataColumn dc in dt.Columns) {
               gvMain.SetColumnCaption(dc.ColumnName , showColCaption[dt.Columns.IndexOf(dc)]);
               gvMain.Columns[dc.ColumnName].AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
               gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
               gvMain.Columns[dc.ColumnName].OptionsColumn.AllowMerge = DefaultBoolean.False;

               //設定欄位header顏色
               if (dc.ColumnName == "SPT1_KIND_ID1" || dc.ColumnName == "SPT1_KIND_ID2") {
                  gvMain.Columns[dc.ColumnName].AppearanceHeader.BackColor = Color.Yellow;
               } else {
                  gvMain.Columns[dc.ColumnName].AppearanceHeader.BackColor = Color.FromArgb(128 , 255 , 255);
               }
            }

            //1.2 設定隱藏欄位
            gvMain.Columns["SPT1_NAME"].Visible = false;
            gvMain.Columns["SPT1_W_TIME"].Visible = false;
            gvMain.Columns["SPT1_W_USER_ID"].Visible = false;
            gvMain.Columns["IS_NEWROW"].Visible = false;

            //1.3 設定dropdownlist       
            gvMain.Columns["SPT1_OSW_GRP"].ColumnEdit = lupOswGrp;
            gvMain.Columns["SPT1_DATA_TYPE"].ColumnEdit = lupDataType;

            //1.4 設定欄位順序
            gvMain.Columns["SPT1_KIND_ID1"].VisibleIndex = 0;
            gvMain.Columns["SPT1_KIND_ID2"].VisibleIndex = 1;
            gvMain.Columns["SPT1_KIND_ID1_OUT"].VisibleIndex = 2;
            gvMain.Columns["SPT1_KIND_ID2_OUT"].VisibleIndex = 3;
            gvMain.Columns["SPT1_SEQ_NO"].VisibleIndex = 4;

            gvMain.Columns["SPT1_COM_ID"].VisibleIndex = 5;
            gvMain.Columns["SPT1_ABBR_NAME"].VisibleIndex = 6;
            gvMain.Columns["SPT1_ADJUST_RATE"].VisibleIndex = 7;
            gvMain.Columns["SPT1_OSW_GRP"].VisibleIndex = 8;
            gvMain.Columns["SPT1_DATA_TYPE"].VisibleIndex = 9;

            gvMain.Columns["SPT1_MAX_SPNS_RATE"].VisibleIndex = 10;

            gvMain.BestFitColumns();
            gcMain.Focus();

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus Save(PokeBall poke) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         try {
            DataTable dtCurrent = (DataTable)gcMain.DataSource;

            DataTable dtChange = dtCurrent.GetChanges();
            DataTable dtForAdd = dtCurrent.GetChanges(DataRowState.Added);
            DataTable dtForModified = dtCurrent.GetChanges(DataRowState.Modified);
            DataTable dtForDeleted = dtCurrent.GetChanges(DataRowState.Deleted);

            if (dtChange == null) {
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }
            if (dtChange.Rows.Count == 0) {
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }

            //判斷商品組合有無填入跨商品MAX折抵比率
            foreach (DataRow dr in dtChange.Rows) {
               if (dr.RowState != DataRowState.Deleted) {
                  string kind1 = dr["SPT1_KIND_ID1"].AsString();
                  string kind2 = dr["SPT1_KIND_ID2"].AsString();
                  string maxSpnsRate = dr["SPT1_MAX_SPNS_RATE"].AsString();
                  if (string.IsNullOrEmpty(kind1) || string.IsNullOrEmpty(kind2)) {
                     MessageDisplay.Error("請輸入商品名稱");
                     return ResultStatus.FailButNext;
                  }

                  if (kind2 != "-" && string.IsNullOrEmpty(maxSpnsRate)) {
                     MessageDisplay.Warning("請輸入跨商品MAX折抵比率");
                     return ResultStatus.FailButNext;
                  }
               }
            }

            //隱藏欄位賦值
            foreach (DataRow dr in dtCurrent.Rows) {
               if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified) {
                  dr["SPT1_W_TIME"] = DateTime.Now;
                  dr["SPT1_W_USER_ID"] = GlobalInfo.USER_ID;
               }

               if (dr.RowState == DataRowState.Deleted) {
                  dr.Delete();
               }
            }
            //dtCurrent.AcceptChanges();
            dtChange = dtCurrent.GetChanges();
            ResultData result = daoSPT1.UpdateSPT1(dtChange); //使用處理並行違規的function
            if (result.Status == ResultStatus.Fail) {
               MessageDisplay.Error("儲存失敗");
               return ResultStatus.FailButNext;
            }
            AfterSaveForPrint(gcMain , dtForAdd , dtForDeleted , dtForModified);

         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Success;
      }

      /// <summary>
      /// 將新增、刪除、變更的紀錄分別都列印或匯出出來(橫式A4)
      /// </summary>
      /// <param name="gridControl"></param>
      /// <param name="ChangedForAdded"></param>
      /// <param name="ChangedForDeleted"></param>
      /// <param name="ChangedForModified"></param>
      protected void AfterSaveForPrint(GridControl gridControl , DataTable ChangedForAdded ,
          DataTable ChangedForDeleted , DataTable ChangedForModified , bool IsHandlePersonVisible = true , bool IsManagerVisible = true) {
         GridControl gridControlPrint = GridHelper.CloneGrid(gridControl);

         string _ReportTitle = _ProgramID + "─" + _ProgramName + GlobalInfo.REPORT_TITLE_MEMO;
         ReportHelper reportHelper = new ReportHelper(gridControl , _ProgramID , _ReportTitle);
         CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4(); //橫向A4
         reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;

         reportLandscape.IsHandlePersonVisible = IsHandlePersonVisible;
         reportLandscape.IsManagerVisible = IsManagerVisible;
         reportHelper.Create(reportLandscape);

         if (ChangedForAdded != null)
            if (ChangedForAdded.Rows.Count != 0) {
               gridControlPrint.DataSource = ChangedForAdded;
               reportHelper.PrintableComponent = gridControlPrint;
               reportHelper.ReportTitle = _ReportTitle + "─" + "新增";

               reportHelper.Print();
               reportHelper.Export(FileType.PDF , reportHelper.FilePath);
            }

         if (ChangedForDeleted != null)
            if (ChangedForDeleted.Rows.Count != 0) {
               DataTable dtTemp = ChangedForDeleted.Clone();

               int rowIndex = 0;
               foreach (DataRow dr in ChangedForDeleted.Rows) {
                  DataRow drNewDelete = dtTemp.NewRow();
                  for (int colIndex = 0 ; colIndex < ChangedForDeleted.Columns.Count ; colIndex++) {
                     drNewDelete[colIndex] = dr[colIndex , DataRowVersion.Original];
                  }
                  dtTemp.Rows.Add(drNewDelete);
                  rowIndex++;
               }

               gridControlPrint.DataSource = dtTemp.AsDataView();
               reportHelper.PrintableComponent = gridControlPrint;
               reportHelper.ReportTitle = _ReportTitle + "─" + "刪除";

               reportHelper.Print();
               reportHelper.Export(FileType.PDF , reportHelper.FilePath);
            }

         if (ChangedForModified != null)
            if (ChangedForModified.Rows.Count != 0) {
               gridControlPrint.DataSource = ChangedForModified;
               reportHelper.PrintableComponent = gridControlPrint;
               reportHelper.ReportTitle = _ReportTitle + "─" + "變更";

               reportHelper.Print();
               reportHelper.Export(FileType.PDF , reportHelper.FilePath);
            }
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            ReportHelper _ReportHelper = new ReportHelper(gcMain , _ProgramID , this.Text);
            CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4(); //橫向A4

            reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            _ReportHelper.Create(reportLandscape);

            _ReportHelper.Print();
            _ReportHelper.Export(FileType.PDF , _ReportHelper.FilePath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus InsertRow() {
         DataTable dt = (DataTable)gcMain.DataSource;
         gvMain.AddNewRow();

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["SPT1_NAME"] , " ");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["SPT1_DATA_TYPE"] , " ");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["SPT1_OSW_GRP"] , " ");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["IS_NEWROW"] , 1);

         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
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
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"]) == null ? "0" :
              gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false;
            gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"] , 1);
         }
         //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
         else if (gv.FocusedColumn.Name == "SPT1_KIND_ID1" || gv.FocusedColumn.Name == "SPT1_KIND_ID2") {
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
            case ("SPT1_KIND_ID1"):
            case ("SPT1_KIND_ID2"):
               e.Column.OptionsColumn.AllowFocus = Is_NewRow == "1" ? true : false;
               e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(192 , 192 , 192);
               break;
            default:
               e.Appearance.BackColor = Color.White;
               break;
         }//switch (e.Column.FieldName) {
      }
      #endregion
   }
}