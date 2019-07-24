using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

/// <summary>
/// Winni, 2019/3/29 修改
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
   /// <summary>
   /// 51050 造市商品單邊回應詢價價格限制設定
   /// </summary>
   public partial class W51050 : FormParent {

      RepositoryItemLookUpEdit lupMarketCode;

      public W51050(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open() {
         base.Open();
         try {
            lupMarketCode = new RepositoryItemLookUpEdit();

            //商品狀態
            //List<LookupItem> dtMarketCode = new List<LookupItem>(){
            //                                new LookupItem() { ValueMember = "0", DisplayMember = "一般"},
            //                                new LookupItem() { ValueMember = "1", DisplayMember = "夜盤"}};
            DataTable dtMarketCode = new CODW().ListLookUpEdit("51050" , "51050_MARKET_CODE");
            lupMarketCode.SetColumnLookUp(dtMarketCode , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , null);
            gcMain.RepositoryItems.Add(lupMarketCode);

            Retrieve();
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
            DataTable dt = new D51050().GetMmfoData();

            //1.check (沒有資料時,則自動新增一筆)
            if (dt.Rows.Count <= 0) {
               InsertRow();
            }

            //2. 設定gvMain
            gcMain.DataSource = dt;
            GridHelper.SetCommonGrid(gvMain);

            //3 設定dropdownlist       
            gvMain.Columns["MMFO_MARKET_CODE"].ColumnEdit = lupMarketCode;

            gvMain.Columns["MMFO_MARKET_CODE"].Width = 50;
            gvMain.Columns["MMFO_PARAM_KEY"].Width = 50;
            gvMain.Columns["MMFO_MIN_PRICE"].Width = 100;

            //gvMain.BestFitColumns();
            gcMain.Focus();
            return ResultStatus.Success;

         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus Save(PokeBall poke) {
         try {
            DataTable dtCurrent = (DataTable)gcMain.DataSource;
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            DataTable dtChange = dtCurrent.GetChanges();
            DataTable dtForAdd = dtCurrent.GetChanges(DataRowState.Added);
            DataTable dtForModified = dtCurrent.GetChanges(DataRowState.Modified);
            DataTable dtForDeleted = dtCurrent.GetChanges(DataRowState.Deleted);

            if (dtChange == null) {
               MessageDisplay.Warning("沒有變更資料,不需要存檔!" , GlobalInfo.WarningText);
               return ResultStatus.Fail;
            }
            if (dtChange.Rows.Count == 0) {
               MessageDisplay.Warning("沒有變更資料,不需要存檔!" , GlobalInfo.WarningText);
               return ResultStatus.Fail;
            }

            //隱藏欄位賦值
            foreach (DataRow dr in dtCurrent.Rows) {
               if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified) {
                  dr["MMFO_W_TIME"] = DateTime.Now;
                  dr["MMFO_W_USER_ID"] = GlobalInfo.USER_ID;
               }
            }

            dtChange = dtCurrent.GetChanges();
            ResultData result = new MMFO().UpdateData(dtChange);
            if (result.Status == ResultStatus.Fail) {
               MessageDisplay.Error("儲存錯誤" , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            PrintOrExportChangedByKen(gcMain , dtForAdd , dtForDeleted , dtForModified);

         } catch (Exception ex) {
            MessageDisplay.Error("儲存錯誤" , GlobalInfo.ErrorText);
            WriteLog(ex , "" , false);
            return ResultStatus.FailButNext;
         }
         return ResultStatus.Success;

      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            ReportHelper _ReportHelper = new ReportHelper(gcMain , _ProgramID , this.Text);
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

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MMFO_MARKET_CODE"] , "");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MMFO_PARAM_KEY"] , "");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MMFO_MIN_PRICE"] , "");
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
      //設定只有新增列可以編輯，原有資料不能編輯
      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"]) == null ? "0" :
              gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false; //新增行可編輯
            //gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"] , 1);
            //object a = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]); //?
         }
         //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
         else if (gv.FocusedColumn.Name == "MMFO_MIN_PRICE") {
            e.Cancel = false;
         } else {
            e.Cancel = true;
         }
      }

      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["IS_NEWROW"]) == null ? "0" :
                            gv.GetRowCellValue(e.RowHandle , gv.Columns["IS_NEWROW"]).ToString();

         //描述每個欄位,在is_newRow時候要顯示的顏色
         //當該欄位不可編輯時,設定為灰色 Color.FromArgb(192,192,192)
         //當該欄位不可編輯時,AllowFocus為false(PB的wf_set_order方法)
         switch (e.Column.FieldName) {
            case ("MMFO_MARKET_CODE"):
            case ("MMFO_PARAM_KEY"):
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