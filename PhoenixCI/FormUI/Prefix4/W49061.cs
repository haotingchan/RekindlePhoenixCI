using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
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
/// Winni, 2019/3/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

   /// <summary>
   /// 49061 國外保證金基本資料設定
   /// </summary>
   public partial class W49061 : FormParent {

      //protected DataTable dtForDeleted;
      RepositoryItemLookUpEdit lupKind;
      RepositoryItemLookUpEdit lupForeign;
      RepositoryItemLookUpEdit lupCurrency;
      RepositoryItemLookUpEdit lupAmt;

      protected class LookupItem {
         public string ValueMember { get; set; }
         public string DisplayMember { get; set; }
      }

      public W49061(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         GridHelper.SetCommonGrid(gvMain);
      }

      protected override ResultStatus Open() {
         base.Open();
         try {
            lupKind = new RepositoryItemLookUpEdit();
            lupForeign = new RepositoryItemLookUpEdit();
            lupCurrency = new RepositoryItemLookUpEdit();
            lupAmt = new RepositoryItemLookUpEdit();

            COD cod = new COD();

            //商品類別
            DataTable dtKind = cod.ListByCol("MGT8" , "MGT8_KIND_TYPE" , " " , "  ");
            Extension.SetColumnLookUp(lupKind , dtKind , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor , "");
            gcMain.RepositoryItems.Add(lupKind);

            //國內外
            //此處國內/外下拉清單 於CI.MGT8參數為(國內 : " "  國外: "Y")
            //避免取空值有問題於SQL中判斷" " -> "D"(切記存檔時需將'D'存回' '不然會影響其他table)
            DataTable dtForeign = cod.ListByCol("49061" , "MGT8_FOREIGN" , "國內" , " ");
            Extension.SetColumnLookUp(lupForeign , dtForeign , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor , "");
            gcMain.RepositoryItems.Add(lupForeign);

            //幣別
            DataTable dtCurrency = cod.ListByCol2("EXRT" , "EXRT_CURRENCY_TYPE");
            Extension.SetColumnLookUp(lupCurrency , dtCurrency , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor , "");
            gcMain.RepositoryItems.Add(lupCurrency);

            //金額類型
            DataTable dtAmt = cod.ListByCol2("49061" , "MGT8_AMT_TYPE");
            Extension.SetColumnLookUp(lupAmt , dtAmt , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor , "");
            gcMain.RepositoryItems.Add(lupAmt);


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

            DataTable dt = new MGT8().ListData();
            //dtForDeleted = dt.Clone();

            //0.check (沒有資料時,則自動新增一筆)
            if (dt.Rows.Count <= 0) {
               InsertRow();
            }

            //1. 設定gvExport
            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dt;
            gvMain.BestFitColumns();

            //1.1 設定欄位caption       
            gvMain.SetColumnCaption("MGT8_F_ID" , "交易所＋商品代號");
            gvMain.SetColumnCaption("MGT8_F_EXCHANGE" , "交易所");
            gvMain.SetColumnCaption("MGT8_F_NAME" , "商品");
            gvMain.SetColumnCaption("MGT8_RT_ID" , "路透代號");
            gvMain.SetColumnCaption("MGT8_KIND_TYPE" , "商品類別");

            gvMain.SetColumnCaption("MGT8_FOREIGN" , "國內/外");
            gvMain.SetColumnCaption("MGT8_CURRENCY_TYPE" , "幣別");
            gvMain.SetColumnCaption("MGT8_AMT_TYPE" , "金額類型");
            gvMain.SetColumnCaption("MGT8_XXX" , "契約乘數");
            gvMain.SetColumnCaption("MGT8_W_USER_ID" , "異動人員");

            gvMain.SetColumnCaption("MGT8_W_TIME" , "異動時間");
            gvMain.SetColumnCaption("IS_NEWROW" , "Is_NewRow");

            //1.2 設定隱藏欄位
            gvMain.Columns["MGT8_STRUTURE"].Visible = false;
            gvMain.Columns["MGT8_W_USER_ID"].Visible = false;
            gvMain.Columns["MGT8_W_TIME"].Visible = false;
            gvMain.Columns["IS_NEWROW"].Visible = false;

            #region 1.3 設定dropdownlist       
            gvMain.Columns["MGT8_KIND_TYPE"].ColumnEdit = lupKind;
            gvMain.Columns["MGT8_FOREIGN"].ColumnEdit = lupForeign;
            gvMain.Columns["MGT8_CURRENCY_TYPE"].ColumnEdit = lupCurrency;
            gvMain.Columns["MGT8_AMT_TYPE"].ColumnEdit = lupAmt;
            #endregion

            gcMain.Focus();

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus Save(PokeBall poke) {
         ResultStatus resultStatus = ResultStatus.Fail;

         try {
            DataTable dtCurrent = (DataTable)gcMain.DataSource;
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            DataTable dtChange = dtCurrent.GetChanges();
            DataTable dtForAdd = dtCurrent.GetChanges(DataRowState.Added);
            DataTable dtForModified = dtCurrent.GetChanges(DataRowState.Modified);
            DataTable dtForDeleted = dtCurrent.GetChanges(DataRowState.Deleted);

            ResultData resultData = new ResultData();
            resultData.ChangedDataViewForAdded = dtForAdd == null ? new DataView() : dtForAdd.DefaultView;
            resultData.ChangedDataViewForModified = dtForModified == null ? new DataView() : dtForModified.DefaultView;
            resultData.ChangedDataViewForDeleted = dtForDeleted;

            if (dtChange == null) {
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }
            if (dtChange.Rows.Count == 0) {
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }
            //Update to DB
            else {

               //隱藏欄位賦值
               foreach (DataRow dr in dtCurrent.Rows) {
                  if (dr.RowState == DataRowState.Added) {
                     dr["MGT8_W_TIME"] = DateTime.Now;
                     dr["MGT8_W_USER_ID"] = GlobalInfo.USER_ID;

                     if (string.IsNullOrEmpty(dr["MGT8_RT_ID"].AsString())) {
                        continue;
                     } else if (string.IsNullOrEmpty(dr["MGT8_KIND_TYPE"].AsString())) {
                        dr["MGT8_KIND_TYPE"] = " ";
                     }

                     //else {
                     //   MessageDisplay.Info("新增資料欄位不可為空!");
                     //   return ResultStatus.FailButNext;
                     //}

                     if (dr["MGT8_FOREIGN"].AsString() == "D") {
                        dr["MGT8_FOREIGN"] = " ";
                     }
                  }
                  if (dr.RowState == DataRowState.Modified) {
                     dr["MGT8_W_TIME"] = DateTime.Now;
                     dr["MGT8_W_USER_ID"] = GlobalInfo.USER_ID;
                     if (dr["MGT8_FOREIGN"].AsString() == "D") {
                        dr["MGT8_FOREIGN"] = " ";
                     }
                  }
               }

               //dtCurrent.AcceptChanges();
               //dtCurrent.Columns.Remove("IS_NEWROW");
               dtChange = dtCurrent.GetChanges();
               ResultData result = new MGT8().UpdateData(dtChange);//base.Save_Override(dt, "MGT8");
               if (result.Status == ResultStatus.Fail) {
                  return ResultStatus.Fail;
               }
               PrintOrExportChangedByKen(gcMain , resultData);
            }
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

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MGT8_KIND_TYPE"] , " ");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MGT8_FOREIGN"] , " ");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MGT8_CURRENCY_TYPE"] , "1");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MGT8_AMT_TYPE"] , "A");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MGT8_STRUTURE"] , " ");
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
         else if (gv.FocusedColumn.Name == "MGT8_KIND_TYPE") {
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
            case ("MGT8_F_ID"):
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