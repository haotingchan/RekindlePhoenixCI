using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting.Native;
using PhoenixCI.Widget;
using System;
using System.Data;
using System.Drawing;

/// <summary>
/// Winni, 2019/04/11
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 49010 最小風險價格係數維護
   /// </summary>
   public partial class W49010 : FormParent {
      private CellMerger _Helper;
      RepositoryItemLookUpEdit lupProdSubtype;
      RepositoryItemLookUpEdit lupKindId;
      private D49010 dao49010;

      public W49010(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            lupProdSubtype = new RepositoryItemLookUpEdit();
            lupKindId = new RepositoryItemLookUpEdit();
            dao49010 = new D49010();

            //契約類別
            DataTable dtProdSubtype = dao49010.GetDdlProdSubtype();
            Extension.SetColumnLookUp(lupProdSubtype , dtProdSubtype , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor);
            gcMain.RepositoryItems.Add(lupProdSubtype);

            //契約代號
            DataTable dtProdKindId = dao49010.GetDdlKindId();
            Extension.SetColumnLookUp(lupKindId , dtProdKindId , "MGT2_SEQ_NO" , "MGT2_KIND_ID" , TextEditStyles.DisableTextEditor);
            gcMain.RepositoryItems.Add(lupKindId);

            DataTable dtAll = dao49010.GetDataList();
            DataTable dt = dtAll.Clone();

            //1. 設定gvExport
            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dt;
            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);

            //1.1 設定欄位caption       
            gvMain.SetColumnCaption("CPR_PROD_SUBTYPE" , "契約類別");
            gvMain.SetColumnCaption("CPR_KIND_ID" , "契約代號");
            gvMain.SetColumnCaption("CPR_EFFECTIVE_DATE" , "系統生效日");
            gvMain.SetColumnCaption("CPR_PRICE_RISK_RATE" , "最小風險價格係數(輸入方式：如3.5 %，則輸入0.035)");
            gvMain.SetColumnCaption("CPR_APPROVAL_DATE" , "核定日期");

            gvMain.SetColumnCaption("CPR_APPROVAL_NUMBER" , "核定文號及日期");
            gvMain.SetColumnCaption("CPR_REMARK" , "備註");
            gvMain.SetColumnCaption("CPR_W_TIME" , "CPR_W_TIME");
            gvMain.SetColumnCaption("CPR_W_USER_ID" , "CPR_W_USER_ID");
            gvMain.SetColumnCaption("CPR_DATA_NUM" , "CPR_DATA_NUM");

            gvMain.SetColumnCaption("IS_NEWROW" , "Is_NewRow");

            


            //1.3 設定隱藏欄位
            gvMain.Columns["CPR_DATA_NUM"].Visible = false;
            gvMain.Columns["IS_NEWROW"].Visible = false;

            //1.4 設定dropdownlist       
            gvMain.Columns["CPR_PROD_SUBTYPE"].ColumnEdit = lupProdSubtype;
            gvMain.Columns["CPR_KIND_ID"].ColumnEdit = lupKindId;

            //製作連動下拉選單(觸發事件)
            gvMain.ShownEditor += gvMain_ShownEditor;
            lupProdSubtype.EditValueChanged += lupProdSubtype_EditValueChanged;

            gcMain.Focus();
            InsertRow();

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();

         ////先確認有沒有資料(這邊不直接下Retrieve是為了不跳錯誤訊息)
         //DataTable dtAll = dao49010.GetDataList();
         //DataTable dt = dtAll.Clone();

         ////不跑retrieve，直接新增row
         //InsertRow();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = true;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = true;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = false;//列印

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {

         //gvMain.CloseEditor();
         //gvMain.UpdateCurrentRow();
         //ResultStatus resultStatus = ResultStatus.Fail;

         //DataTable dt = (DataTable)gcMain.DataSource;
         //DataTable dtChange = dt.GetChanges();
         //DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
         //DataTable dtForModified = dt.GetChanges(DataRowState.Modified);

         //if (dtChange != null) {
         //   if (dtChange.Rows.Count == 0) {
         //      MessageDisplay.Choose("沒有變更資料,不需要存檔!");
         //      return ResultStatus.Fail;
         //   } else {
         //      foreach (DataRow dr in dt.Rows) {
         //         if (dr.RowState == DataRowState.Added) {

         //            foreach (DataRow drAdd in dtForAdd.Rows) {
         //               for (int w = 0 ; w < dtForAdd.Rows.Count ; w++) {
         //                  if (string.IsNullOrEmpty(drAdd[w].AsString())) {
         //                     MessageDisplay.Info("新增資料欄位不可為空!");
         //                     return ResultStatus.Fail;
         //                  }
         //               }
         //            }

         //            dr["MGT4_W_TIME"] = DateTime.Now;
         //            dr["MGT4_W_USER_ID"] = GlobalInfo.USER_ID;
         //         }
         //         if (dr.RowState == DataRowState.Modified) {
         //            dr["MGT4_W_TIME"] = DateTime.Now;
         //            dr["MGT4_W_USER_ID"] = GlobalInfo.USER_ID;
         //         }
         //      }

         //      dt.Columns.Remove("OP_TYPE");
         //      dt.Columns.Remove("Is_NewRow");
         //      ResultData result = new MGT4().UpdateData(dt);//base.Save_Override(dt, "MGT4");
         //      if (result.Status == ResultStatus.Fail) {
         //         return ResultStatus.Fail;
         //      }

         //   }

         //   if (resultStatus == ResultStatus.Success) {

         //      PrintableComponent = gcMain;
         //   }
         //}

         ////不要自動列印
         //_IsPreventFlowPrint = true;
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper ReportHelper) {
         try {
            ReportHelper reportHelper = new ReportHelper(gcMain , _ProgramID , _ProgramID + _ProgramName);
            reportHelper.Print();

         } catch (Exception ex) {
            throw ex;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         DataTable dt = (DataTable)gcMain.DataSource;
         gvMain.AddNewRow();
         gvMain.OptionsView.RowAutoHeight = true; //整個grid設定要開，不然設定column會無效

         RepositoryItemTextDateEdit effectiveDate = new RepositoryItemTextDateEdit();
         RepositoryItemTextDateEdit approvalDate = new RepositoryItemTextDateEdit();
         RepositoryItemMemoEdit can = new RepositoryItemMemoEdit();
         RepositoryItemMemoEdit remark = new RepositoryItemMemoEdit();

         gcMain.RepositoryItems.Add(effectiveDate);
         gcMain.RepositoryItems.Add(approvalDate);
         gcMain.RepositoryItems.Add(can);
         gcMain.RepositoryItems.Add(remark);

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_PROD_SUBTYPE"] , "");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_KIND_ID"] , " ");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_EFFECTIVE_DATE"] , DateTime.MinValue);
         gvMain.Columns["CPR_EFFECTIVE_DATE"].ColumnEdit = effectiveDate;

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_APPROVAL_DATE"] , DateTime.MinValue);
         gvMain.Columns["CPR_APPROVAL_DATE"].ColumnEdit = approvalDate;

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_APPROVAL_NUMBER"] , " ");
         gvMain.Columns["CPR_APPROVAL_NUMBER"].ColumnEdit = can;
         gvMain.Columns["CPR_APPROVAL_NUMBER"].Width = 100;
         can.AutoHeight = true;
         can.MaxLength = 100;

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_REMARK"] , " ");
         gvMain.Columns["CPR_REMARK"].ColumnEdit = remark;
         gvMain.Columns["CPR_REMARK"].Width = 100;
         remark.AutoHeight = true;
         remark.MaxLength = 100;

         //gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_W_TIME"] , DateTime.MinValue);
         //gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_W_USER_ID"] , GlobalInfo.USER_ID);

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_DATA_NUM"] , 0);
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["IS_NEWROW"] , 1);

         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      private void gvMain_ShownEditor(object sender , EventArgs e) {
         ColumnView view = (ColumnView)sender;
         if (view.FocusedColumn.FieldName == "CPR_KIND_ID") {
            string prodType = gvMain.GetFocusedRowCellValue("CPR_PROD_SUBTYPE").ToString();

            LookUpEdit edit = (LookUpEdit)view.ActiveEditor;
            DataTable dtFilter = new DataTable();
            DataTable dtKindId = dao49010.GetDdlKindId();
            RepositoryItemLookUpEdit cbxKindId = new RepositoryItemLookUpEdit();

            //修改商品組合下拉清單(重綁data source)
            if (!string.IsNullOrEmpty(prodType)) {
               dtFilter = dtKindId.Filter(string.Format("mgt2_prod_subtype = '{0}'" , prodType));
            } else {
               //
            }

            cbxKindId.SetColumnLookUp(dtFilter , "MGT2_SEQ_NO" , "MGT2_KIND_ID" , TextEditStyles.DisableTextEditor , "  ");
            edit.Properties.DataSource = cbxKindId.DataSource;
            edit.ShowPopup();

         }

      }

      private void lupProdSubtype_EditValueChanged(object sender , EventArgs e) {
         gvMain.PostEditor();
         gvMain.SetFocusedRowCellValue("CPR_KIND_ID" , null);
      }


   }
}