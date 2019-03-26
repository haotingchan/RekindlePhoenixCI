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
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
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

      RepositoryItemLookUpEdit lupOswGrp;
      RepositoryItemLookUpEdit lupDataType;

      protected class LookupItem {
         public string ValueMember { get; set; }
         public string DisplayMember { get; set; }
      }

      public W49070(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open() {
         base.Open();
         try {
            lupOswGrp = new RepositoryItemLookUpEdit();
            lupDataType = new RepositoryItemLookUpEdit();

            COD cod = new COD();

            //收盤群組
            DataTable dtOswGrp = cod.ListByCol("49070" , "SPT1_OSW_GRP" , " " , "  ");
            Extension.SetColumnLookUp(lupOswGrp , dtOswGrp , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor , "");
            gcMain.RepositoryItems.Add(lupOswGrp);

            //商品狀態
            DataTable dtDataType = cod.ListByCol("49070" , "SPT1_DATA_TYPE" , " " , " ");
            Extension.SetColumnLookUp(lupDataType , dtDataType , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor , "");
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
            DataTable dt = new D49070().ListData();

            //0.check (沒有資料時,則自動新增一筆)
            if (dt.Rows.Count <= 0) {
               InsertRow();
            }

            //1. 設定gvExport
            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dt;
            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);

            //1.1 設定欄位caption       
            gvMain.SetColumnCaption("SPT1_KIND_ID1" , "商品");
            gvMain.SetColumnCaption("SPT1_KIND_ID2" , "商品");
            gvMain.SetColumnCaption("SPT1_KIND_ID1_OUT" , "對外商品");
            gvMain.SetColumnCaption("SPT1_KIND_ID2_OUT" , "對外商品");
            gvMain.SetColumnCaption("SPT1_SEQ_NO" , "順序");

            gvMain.SetColumnCaption("SPT1_COM_ID" , "適用商品組合");
            gvMain.SetColumnCaption("SPT1_ABBR_NAME" , "簡稱");
            gvMain.SetColumnCaption("SPT1_ADJUST_RATE" , "判斷調整標準");
            gvMain.SetColumnCaption("SPT1_OSW_GRP" , "收盤群組");
            gvMain.SetColumnCaption("SPT1_DATA_TYPE" , "商品狀態");

            gvMain.SetColumnCaption("SPT1_MAX_SPNS_RATE" , "跨商品MAX折抵比率");
            gvMain.SetColumnCaption("IS_NEWROW" , "Is_NewRow");

            //1.2 設定隱藏欄位
            gvMain.Columns["SPT1_NAME"].Visible = false;
            gvMain.Columns["SPT1_W_TIME"].Visible = false;
            gvMain.Columns["SPT1_W_USER_ID"].Visible = false;
            gvMain.Columns["IS_NEWROW"].Visible = false;

            //1.3 設定dropdownlist       
            gvMain.Columns["SPT1_OSW_GRP"].ColumnEdit = lupOswGrp;
            gvMain.Columns["SPT1_DATA_TYPE"].ColumnEdit = lupDataType;

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
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }
            if (dtChange.Rows.Count == 0) {
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }

            //隱藏欄位賦值
            foreach (DataRow dr in dtCurrent.Rows) {
               if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified) {
                  dr["SPT1_W_TIME"] = DateTime.Now;
                  dr["SPT1_W_USER_ID"] = GlobalInfo.USER_ID;

                  if (string.IsNullOrEmpty(dr["SPT1_OSW_GRP"].AsString())) {
                     dr["SPT1_OSW_GRP"] = "  ";
                  }

                  if (string.IsNullOrEmpty(dr["SPT1_DATA_TYPE"].AsString())) {
                     dr["SPT1_DATA_TYPE"] = " ";
                  }

                  if (string.IsNullOrEmpty(dr["SPT1_NAME"].AsString())) {
                     dr["SPT1_NAME"] = " ";
                  }

                  string kindId2 = dr["SPT1_KIND_ID2"].AsString();
                  decimal maxSpnsRate = dr["SPT1_MAX_SPNS_RATE"].AsDecimal();

                  if (kindId2 != "-" && string.IsNullOrEmpty(maxSpnsRate.AsString())) {
                     MessageDisplay.Info("請輸入跨商品MAX折抵比率");
                     return ResultStatus.Fail;
                  }
               }
            }

            dtChange = dtCurrent.GetChanges();
            ResultData result = new SPT1().UpdateData(dtChange);
            if (result.Status == ResultStatus.Fail) {
               return ResultStatus.Fail;
            }
            PrintOrExportChangedByKen(gcMain , dtForAdd , dtForDeleted , dtForModified);

         } catch (Exception ex) {
            throw ex;
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