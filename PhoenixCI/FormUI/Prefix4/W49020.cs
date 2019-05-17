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
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

/// <summary>
/// Winni, 2019/05/16
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

   /// <summary>
   /// 49020 保證金報表契約名稱設定
   /// </summary>
   public partial class W49020 : FormParent {

      #region 全域變數
      private RepositoryItemLookUpEdit lupProdType; //商品別
      private RepositoryItemLookUpEdit lupProdSubtypeCod; //契約類別
      private RepositoryItemLookUpEdit lupDataType; //商品狀態
      private RepositoryItemLookUpEdit lupCpKind; //風險價格係數計算方式
      private RepositoryItemLookUpEdit lupAbroad; //國內/國外類別

      private COD cod;
      private D49020 dao49020;
      #endregion

      public W49020(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         cod = new COD();
         dao49020 = new D49020();
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            #region 下拉選單設定
            //商品別
            List<LookupItem> prodTypeList = new List<LookupItem>(){
                                            new LookupItem() { ValueMember = "F", DisplayMember = "F：期貨"},
                                            new LookupItem() { ValueMember = "O", DisplayMember = "O：選擇權"}};
            lupProdType = new RepositoryItemLookUpEdit();
            lupProdType.SetColumnLookUp(prodTypeList , "ValueMember" , "DisplayMember" , TextEditStyles.DisableTextEditor , null);
            gcMain.RepositoryItems.Add(lupProdType);

            //契約類別
            lupProdSubtypeCod = new RepositoryItemLookUpEdit();
            DataTable dtProdSubtypeCod = cod.ListByCol2("49020" , "PDK_SUBTYPE");
            Extension.SetColumnLookUp(lupProdSubtypeCod , dtProdSubtypeCod , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor , "");
            gcMain.RepositoryItems.Add(lupProdSubtypeCod);


            //商品狀態
            List<LookupItem> dataTypeList = new List<LookupItem>(){
                                            new LookupItem() { ValueMember = "E", DisplayMember = "下市"},
                                            new LookupItem() { ValueMember = "N", DisplayMember = "不計算"}};
            lupDataType = new RepositoryItemLookUpEdit();
            lupDataType.SetColumnLookUp(dataTypeList , "ValueMember" , "DisplayMember" , TextEditStyles.DisableTextEditor , null);
            gcMain.RepositoryItems.Add(lupDataType);

            //風險價格係數計算方式
            lupCpKind = new RepositoryItemLookUpEdit();
            DataTable dtCpKind = dao49020.GetCpKind("MGT2" , "MGT2_CP_KIND");
            Extension.SetColumnLookUp(lupCpKind , dtCpKind , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor , "");
            gcMain.RepositoryItems.Add(lupCpKind);

            //國內/國外類別
            //此處國內/外下拉清單 於CI.MGT2參數為(國內 : " "  國外: "Y") CI.COD參數為(國內 : ""  國外: "Y")
            lupAbroad = new RepositoryItemLookUpEdit();
            DataTable dtAbroad = cod.ListByCol2("MGT2" , "MGT2_ABROAD");
            foreach (DataRow dr in dtAbroad.Rows) {
               if (dr["cod_id"] == DBNull.Value) {
                  dr["cod_id"] = " ";
               }
            }
            Extension.SetColumnLookUp(lupAbroad , dtAbroad , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor , "");
            gcMain.RepositoryItems.Add(lupAbroad);
            #endregion

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
            DataTable dt = new D49020().ListData();

            //0.check (沒有資料時,則自動新增一筆)
            if (dt.Rows.Count <= 0) {
               InsertRow();
            }

            //1. 設定gvMain
            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dt;

            string[] showColCaption = {"商品", $"對外{Environment.NewLine}商品", $"順{Environment.NewLine}序","商品別",
                                       $"契約{Environment.NewLine}類別","簡稱","全稱","群組",$"標的{Environment.NewLine}現貨",
                                       $"下市日期{Environment.NewLine}yyyymmdd","商品狀態", $"判斷{Environment.NewLine}調整標準",
                                       $"風險價格係數{Environment.NewLine}計算方式",$"國內/國外{Environment.NewLine}類別",
                                       "MGT2_W_TIME" ,"MGT2_W_USER_ID",$"最大振幅MaxVol{Environment.NewLine}調整標準",
                                       $"EWMA{Environment.NewLine}調整標準","Is_NewRow"};

            //1.1 設定欄位caption       
            foreach (DataColumn dc in dt.Columns) {
               gvMain.SetColumnCaption(dc.ColumnName , showColCaption[dt.Columns.IndexOf(dc)]);
               gvMain.Columns[dc.ColumnName].AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
               gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
               //設定合併欄位(一樣的值不顯示)
               gvMain.OptionsView.AllowCellMerge = true;
               gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
               gvMain.Columns[dc.ColumnName].OptionsColumn.AllowMerge = DefaultBoolean.False;
               gvMain.Columns[dc.ColumnName].AppearanceCell.Font = new Font("微軟正黑體",10f);

               //設定column style
               gvMain.Columns[dc.ColumnName].AppearanceHeader.BackColor = (dc.ColumnName.AsString() == "MGT2_KIND_ID" ? Color.Yellow : Color.FromArgb(128 , 255 , 255));
            }

            //1.2 設定隱藏欄位
            gvMain.Columns["MGT2_W_TIME"].Visible = false;
            gvMain.Columns["MGT2_W_USER_ID"].Visible = false;
            gvMain.Columns["IS_NEWROW"].Visible = false;

            //1.3 設定dropdownlist       
            gvMain.Columns["MGT2_PROD_TYPE"].ColumnEdit = lupProdType;
            gvMain.Columns["MGT2_PROD_TYPE"].ShowButtonMode = ShowButtonModeEnum.ShowAlways;

            gvMain.Columns["MGT2_PROD_SUBTYPE"].ColumnEdit = lupProdSubtypeCod;
            gvMain.Columns["MGT2_PROD_SUBTYPE"].ShowButtonMode = ShowButtonModeEnum.ShowAlways;

            gvMain.Columns["MGT2_DATA_TYPE"].ColumnEdit = lupDataType;
            gvMain.Columns["MGT2_DATA_TYPE"].ShowButtonMode = ShowButtonModeEnum.ShowAlways;

            gvMain.Columns["MGT2_CP_KIND"].ColumnEdit = lupCpKind;
            gvMain.Columns["MGT2_CP_KIND"].ShowButtonMode = ShowButtonModeEnum.ShowAlways;

            gvMain.Columns["MGT2_ABROAD"].ColumnEdit = lupAbroad;
            gvMain.Columns["MGT2_ABROAD"].ShowButtonMode = ShowButtonModeEnum.ShowAlways;

            gvMain.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
            gvMain.AppearancePrint.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;
            gvMain.ColumnPanelRowHeight = 40;

            gvMain.AppearancePrint.Row.Font = new Font("Microsoft YaHei" , 10);
            gvMain.OptionsPrint.AllowMultilineHeaders = true;
            gvMain.AppearancePrint.GroupRow.Font = new Font("Microsoft YaHei" , 10);

            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);
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

            foreach (DataRow dr in dtCurrent.Rows) {
               if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified) {
                  dr["mgt2_w_time"] = DateTime.Now;
                  dr["mgt2_w_user_id"] = GlobalInfo.USER_ID;

                  //有設下市日                 
                  if (!Convert.IsDBNull(dr["mgt2_end_ymd"])) {
                     string mgt2EndDate = dr["mgt2_end_ymd"].AsString();
                     if (mgt2EndDate != "") {
                        //要輸入8位數值
                        int dateNumber;
                        if (int.TryParse(mgt2EndDate , out dateNumber) == true) {
                           //檢核有效日
                           string strDate = DateTime.ParseExact(mgt2EndDate , "yyyyMMdd" , null).ToString("yyyy/MM/dd");
                           DateTime isEndDate;
                           if (DateTime.TryParse(strDate , out isEndDate) == false) {
                              MessageDisplay.Error(string.Format("下市日期格式不符yyyymmdd,({0})非有效日期" , mgt2EndDate));
                              return ResultStatus.FailButNext;
                           }
                        } else {
                           MessageDisplay.Error(string.Format("下市日期格式不符yyyymmdd,({0})非有效日期" , mgt2EndDate));
                           return ResultStatus.FailButNext;
                        }//if (int.TryParse(mgt2EndDate , out int tmp) == true)
                     }
                  }//if (!Convert.IsDBNull(dr["mgt2_end_ymd"]))
               }
            }// foreach (DataRow dr in dtCurrent.Rows)

            ResultData result = new MGT2().UpdateData(dtCurrent);
            if (result.Status == ResultStatus.Fail) {
               return ResultStatus.Fail;
            }
            //PrintOrExportChangedByKen(gcMain , dtForAdd , dtForDeleted , dtForModified);

         } catch (Exception ex) {
            throw ex;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            ReportHelper _ReportHelper = new ReportHelper(gcMain , _ProgramID , this.Text);
            CommonReportLandscapeA3 reportLandscape = new CommonReportLandscapeA3();//設定為橫向列印
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

         int focusIndex = gvMain.GetFocusedDataSourceRowIndex();
         gvMain.CloseEditor();//必須先做close edit, like dt.AcceptChanges();

         //新增一行並做初始值設定
         DataTable dt = (DataTable)gcMain.DataSource;
         DataRow drNew = dt.NewRow();

         drNew["Is_NewRow"] = 1;

         dt.Rows.InsertAt(drNew , focusIndex);
         gcMain.DataSource = dt;
         gvMain.FocusedRowHandle = focusIndex;//原本的focusRowHandle會記住之前的位置,其實只是往上一行
         gvMain.FocusedColumn = gvMain.Columns[0];
         SetOrder();

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);
         SetOrder();
         return ResultStatus.Success;
      }

      /// <summary>
      /// 重新排序 wf_set_order()
      /// </summary>
      protected void SetOrder() {
         DataTable dtCurrent = (DataTable)gcMain.DataSource;
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         int pos = 0;
         foreach (DataRow dr in dtCurrent.Rows) {
            pos++;
            int seq = 0;
            if (dr.RowState == DataRowState.Deleted) {
               pos--;
               continue;
            }

            seq = dr["mgt2_seq_no"].AsInt();
            if (seq != pos) {
               gvMain.SetRowCellValue(pos - 1 , gvMain.Columns["MGT2_SEQ_NO"] , pos);
            }

         }
         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];
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
         else if (gv.FocusedColumn.Name == "MGT2_KIND_ID") {
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
            case ("MGT2_KIND_ID"):
               e.Column.AppearanceHeader.BackColor = Color.Yellow;
               e.Column.OptionsColumn.AllowFocus = Is_NewRow == "1" ? true : false;
               e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(192 , 192 , 192);
               break;
            default:
               e.Column.AppearanceHeader.BackColor = Color.FromArgb(128 , 255 , 255);
               e.Appearance.BackColor = Color.White;
               break;
         }//switch (e.Column.FieldName) {
      }

      private void gvMain_CustomDrawRowFooterCell(object sender , FooterCellCustomDrawEventArgs e) {
         //Change the background color
         e.Appearance.BackColor = Color.Azure;
         //Paint using the new background color
         e.Painter.DrawObject(e.Info);
         //Prevent default painting
         e.Handled = true;
      }


      private void gvMain_CustomDrawColumnHeader(object sender , ColumnHeaderCustomDrawEventArgs e) {
         e.Appearance.Font = new Font("微軟正黑體" , 10);
      }
      #endregion

   }
}