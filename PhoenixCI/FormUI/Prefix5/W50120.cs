using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
/// <summary>
/// Lukas, 2018/12/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
   /// <summary>
   /// 50120 造市者異動狀態維護
   /// 有寫到的功能：Open, Retrieve, Save, Print, InsertRow
   /// </summary>
   public partial class W50120 : FormParent {

      private ReportHelper _ReportHelper;
      private D50120 dao50120;
      private ABRK daoABRK;
      private APDK daoAPDK;
      protected DataTable dtInsertUse;
      private COD daoCOD;
      private RepositoryItemLookUpEdit _RepLookUpEdit;
      private RepositoryItemLookUpEdit _RepLookUpEdit2;
      private RepositoryItemLookUpEdit _RepLookUpEdit3;

      public W50120(string programID , string programName) : base(programID , programName) {

         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         GridHelper.SetCommonGrid(gvMain);

         txtMonth.DateTimeValue = GlobalInfo.OCF_DATE;
         txtMonth.Focus();
         txtMonth.ImeMode = ImeMode.Disable;
         dao50120 = new D50120();
         daoABRK = new ABRK();
         daoCOD = new COD();
         daoAPDK = new APDK();
         dtInsertUse = daoAPDK.ListAll2();
         dtInsertUse.Columns.Add("TEMP_PROD_TYPE");
         _IsPreventFlowPrint = false;

      }

      protected override ResultStatus Open() {
         base.Open();

         //[造市者代號]下拉選單
         _RepLookUpEdit = new RepositoryItemLookUpEdit();
         DataTable dtABRK = daoABRK.ListByNo();
         Extension.SetColumnLookUp(_RepLookUpEdit , dtABRK , "ABRK_NO" , "CP_DISPLAY" , TextEditStyles.DisableTextEditor , "");
         gcMain.RepositoryItems.Add(_RepLookUpEdit);

         //[狀態]下拉選單
         _RepLookUpEdit2 = new RepositoryItemLookUpEdit();
         //DataTable dtCOD = daoCOD.ListByTxn("50120");
         DataTable dtCODW = new CODW().ListLookUpEdit2("50120" , "50120_MPDF_STATUS"); //用有組合欄位的
         Extension.SetColumnLookUp(_RepLookUpEdit2 , dtCODW , "CODW_ID" , "CP_DISPLAY" , TextEditStyles.DisableTextEditor , "");
         gcMain.RepositoryItems.Add(_RepLookUpEdit2);

         //[契約]下拉選單
         _RepLookUpEdit3 = new RepositoryItemLookUpEdit();
         DataTable dtActId = daoAPDK.ListAll2();
         DataTable dtMerge = dao50120.GetMergeData(txtMonth.Text.Replace("/" , ""));
         Extension.SetColumnLookUp(_RepLookUpEdit3 , dtMerge , "MPDF_KIND_ID" , "CP_DISPLAY" , TextEditStyles.Standard , "");
         gcMain.RepositoryItems.Add(_RepLookUpEdit3);

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();

         //先確認有沒有資料(這邊不直接下Retrieve是為了不跳錯誤訊息)
         DataTable dtCheck = dao50120.GetMergeData(txtMonth.Text.Replace("/" , ""));

         //沒有新增資料時,則自動新增一筆內容
         if (dtCheck.Rows.Count <= 0) {
            InsertRow();
         } else {
            dtCheck.Columns.Add("Is_NewRow" , typeof(string));
            gcMain.DataSource = dtCheck;
            gcMain.Focus();
         }
         Retrieve();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = true;
         _ToolBtnSave.Enabled = true;
         _ToolBtnDel.Enabled = true;
         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {

         DataTable returnTable = dao50120.GetMergeData(txtMonth.Text.Replace("/" , "")); //MPDF的資料
         if (returnTable.Rows.Count <= 0) {
            MessageDisplay.Info(GlobalInfo.MsgNoData , MessageDisplay.MSG_OK);
            InsertRow();
         }

         returnTable.Columns.Add("Is_NewRow" , typeof(string));

         gcMain.RepositoryItems.Add(_RepLookUpEdit);
         gcMain.RepositoryItems.Add(_RepLookUpEdit2);
         gcMain.RepositoryItems.Add(_RepLookUpEdit3);
         MPDF_FCM_NO.ColumnEdit = _RepLookUpEdit;
         MPDF_STATUS.ColumnEdit = _RepLookUpEdit2;
         MPDF_KIND_ID.ColumnEdit = _RepLookUpEdit3;

         gcMain.DataSource = returnTable;
         gcMain.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus CheckShield() {
         base.CheckShield(gcMain);
         if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }
         //if (cbxUserId.SelectedItem == null) {
         //    MessageDisplay.Warning("使用者代號不可為空白!");
         //    return ResultStatus.Fail;
         //}

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {
         base.Save(gcMain);
         _IsPreventFlowPrint = true;//儲存完不要自動列印
         DataTable dt = (DataTable)gcMain.DataSource;

         DataTable dtChange = dt.GetChanges();

         if (dtChange == null) {
            MessageBox.Show("沒有變更資料,不需要存檔!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            return ResultStatus.Fail;
         }
         if (dtChange.Rows.Count == 0) {
            MessageBox.Show("沒有變更資料,不需要存檔!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            return ResultStatus.Fail;
         }
         //PB存的日期格式月份沒有補0，在維護上處理比較麻煩，故讀取資料時轉成字串，存檔時再轉回datetime
         DataTable dtCloned = dt.Clone();
         dtCloned.Columns["MPDF_EFF_DATE"].DataType = typeof(DateTime);
         foreach (DataRow row in dt.Rows) {
            dtCloned.ImportRow(row);
         }

         try {
            //update to DB
            ResultData myResultData = dao50120.UpdateMPDF(dtCloned);
            if (myResultData.Status == ResultStatus.Fail) {
               MessageDisplay.Error("更新資料庫MPDF錯誤! ");
               return ResultStatus.Fail;
            }
         } catch (Exception ex) {
            MessageBox.Show(ex.Message);
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         _ReportHelper = reportHelper;
         CommonReportLandscapeA4 report = new CommonReportLandscapeA4();
         report.printableComponentContainerMain.PrintableComponent = gcMain;
         _ReportHelper.Create(report);
         //_ReportHelper.LeftMemo = "設定權限給(" + cbxUserId.Text.Trim() + ")";

         base.Print(_ReportHelper);
         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         base.InsertRow(gvMain);
         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      #region GridControl事件

      /// <summary>
      /// 年月欄自動填上查詢年月(gvMain_InitNewRow事件)
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_InitNewRow(object sender , InitNewRowEventArgs e) {
         GridView gv = sender as GridView;
         gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"] , 1);
         gv.SetRowCellValue(e.RowHandle , gv.Columns["MPDF_YM"] , txtMonth.Text.Replace("/" , ""));
      }


      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
              gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false;
            gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"] , 1);
         }
         //既有資料除了生效日期之外不能編輯
         else if (gv.FocusedColumn.Name == "MPDF_EFF_DATE") {
            e.Cancel = false;
         } else {
            e.Cancel = true;
         }
      }

      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         //要用RowHandle不要用FocusedRowHandle
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
                            gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]).ToString();
         e.Column.OptionsColumn.AllowFocus = true;

         if (e.Column.FieldName != "MPDF_EFF_DATE") {
            if (Is_NewRow == "1") {
               e.Appearance.BackColor = Color.White;
               //if (e.Column.FieldName != "MPDF_KIND_ID") {
               //   _RepLookUpEdit3 = new RepositoryItemLookUpEdit();
               //   _RepLookUpEdit3.SetColumnLookUp(dtInsertUse , "APDK_KIND_ID" , "CP_DISPLAY" , TextEditStyles.Standard , "");
               //   gcMain.RepositoryItems.Add(_RepLookUpEdit3);
               //   MPDF_KIND_ID.ColumnEdit = _RepLookUpEdit3;
               //}
            } else {
               e.Appearance.BackColor = Color.FromArgb(224 , 224 , 224);
               //if (e.Column.FieldName != "MPDF_KIND_ID") {
               //   DataTable dtMer = dao50120.GetMergeData(txtMonth.Text.Replace("/" , ""));
               //   _RepLookUpEdit3 = new RepositoryItemLookUpEdit();
               //   _RepLookUpEdit3.SetColumnLookUp(dtMer , "APDK_KIND_ID" , "CP_DISPLAY" , TextEditStyles.Standard , "");
               //   gcMain.RepositoryItems.Add(_RepLookUpEdit3);
               //   MPDF_KIND_ID.ColumnEdit = _RepLookUpEdit3;
               //}
            }
         }
      }

      private void gvMain_FocusedColumnChanged(object sender , DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) {

         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
         gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]).ToString();

         if (e.FocusedColumn != null) {
            if (Is_NewRow == "1") {
               e.FocusedColumn.OptionsColumn.AllowFocus = true;
            } else if (e.FocusedColumn.FieldName != "MPDF_EFF_DATE") {
               e.FocusedColumn.OptionsColumn.AllowFocus = false;
            }
         }
      }

      private void gvMain_CustomRowCellEdit(object sender , CustomRowCellEditEventArgs e) {
         GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" : gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();
            if (Is_NewRow == "1") {

                if (e.Column.FieldName == "MPDF_KIND_ID" && e.RowHandle == gvMain.FocusedRowHandle) {
                    RepositoryItemLookUpEdit _RepLookUpEdit4 = new RepositoryItemLookUpEdit();
                    _RepLookUpEdit4.SetColumnLookUp(dtInsertUse, "MPDF_KIND_ID", "CP_DISPLAY", TextEditStyles.Standard, "-");
                    e.RepositoryItem = _RepLookUpEdit4;
                }
            }
      }
      #endregion

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus COMPLETE() {
         MessageDisplay.Info(MessageDisplay.MSG_OK);
         Retrieve();
         return ResultStatus.Success;
      }


   }
}