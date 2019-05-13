using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.TableDao;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
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

      RepositoryItemLookUpEdit lupKind;
      RepositoryItemLookUpEdit lupForeign;
      RepositoryItemLookUpEdit lupCurrency;
      RepositoryItemLookUpEdit lupAmt;

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

            //0.check (沒有資料時,則自動新增一筆)
            if (dt.Rows.Count <= 0) {
               InsertRow();
            }

            //1.1 設定欄位caption      
            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dt;
            GridHelper.SetCommonGrid(gvMain);

            string[] showColCaption = {"交易所＋商品代號", "交易所", "商品","路透代號","商品類別","國內/外","幣別",
                                       "","金額類型","契約乘數","異動人員" ,"異動時間", ""};

            //1.1 設定欄位caption       
            foreach (DataColumn dc in dt.Columns) {
               gvMain.SetColumnCaption(dc.ColumnName , showColCaption[dt.Columns.IndexOf(dc)]);
               gvMain.Columns[dc.ColumnName].AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
               gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
               gvMain.Columns[dc.ColumnName].OptionsColumn.AllowMerge = DefaultBoolean.False;

               //設定欄位header顏色
               if (dc.ColumnName == "MGT8_F_ID") {
                  gvMain.Columns[dc.ColumnName].AppearanceHeader.BackColor = Color.Yellow;
               } else {
                  gvMain.Columns[dc.ColumnName].AppearanceHeader.BackColor = Color.FromArgb(128 , 255 , 255);
               }
            }

            //1.2 設定隱藏欄位
            gvMain.Columns["MGT8_STRUTURE"].Visible = false;
            gvMain.Columns["MGT8_W_USER_ID"].Visible = false;
            gvMain.Columns["MGT8_W_TIME"].Visible = false;
            gvMain.Columns["IS_NEWROW"].Visible = false;

            //1.2 設定欄位format格式
            RepositoryItemTextEdit exchange = new RepositoryItemTextEdit(); //交易所
            gcMain.RepositoryItems.Add(exchange);
            gvMain.Columns["MGT8_F_EXCHANGE"].ColumnEdit = exchange;
            exchange.MaxLength = 12;

            RepositoryItemTextEdit fName = new RepositoryItemTextEdit(); //商品
            gcMain.RepositoryItems.Add(fName);
            gvMain.Columns["MGT8_F_NAME"].ColumnEdit = fName;
            fName.MaxLength = 29;

            RepositoryItemTextEdit rtId = new RepositoryItemTextEdit(); //路透代號
            gcMain.RepositoryItems.Add(rtId);
            gvMain.Columns["MGT8_RT_ID"].ColumnEdit = rtId;
            rtId.MaxLength = 10;

            RepositoryItemTextEdit mgt8Xxx = new RepositoryItemTextEdit(); //契約乘數
            gcMain.RepositoryItems.Add(mgt8Xxx);
            gvMain.Columns["MGT8_XXX"].ColumnEdit = mgt8Xxx;
            mgt8Xxx.DisplayFormat.FormatType = FormatType.Numeric;
            mgt8Xxx.DisplayFormat.FormatString = "######0.####";
            mgt8Xxx.Mask.EditMask = "######0.0000";
            mgt8Xxx.MaxLength = 12;
            mgt8Xxx.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;

            //1.3 設定dropdownlist       
            gvMain.Columns["MGT8_KIND_TYPE"].ColumnEdit = lupKind;
            gvMain.Columns["MGT8_FOREIGN"].ColumnEdit = lupForeign;
            gvMain.Columns["MGT8_CURRENCY_TYPE"].ColumnEdit = lupCurrency;
            gvMain.Columns["MGT8_AMT_TYPE"].ColumnEdit = lupAmt;

            //1.4 設定欄位順序
            gvMain.Columns["MGT8_F_ID"].VisibleIndex = 0;
            gvMain.Columns["MGT8_F_EXCHANGE"].VisibleIndex = 1;
            gvMain.Columns["MGT8_F_NAME"].VisibleIndex = 2;
            gvMain.Columns["MGT8_RT_ID"].VisibleIndex = 3;
            gvMain.Columns["MGT8_KIND_TYPE"].VisibleIndex = 4;

            gvMain.Columns["MGT8_FOREIGN"].VisibleIndex = 5;
            gvMain.Columns["MGT8_CURRENCY_TYPE"].VisibleIndex = 6;
            gvMain.Columns["MGT8_AMT_TYPE"].VisibleIndex = 7;
            gvMain.Columns["MGT8_XXX"].VisibleIndex = 8;

            gvMain.BestFitColumns();
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

            dtChange = dtCurrent.GetChanges();
            ResultData result = new MGT8().UpdateData(dtChange);
            if (result.Status == ResultStatus.Fail) {
               return ResultStatus.Fail;
            }
            AfterSaveForPrint(gcMain , dtForAdd , dtForDeleted , dtForModified);

         } catch (Exception ex) {
            throw ex;
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