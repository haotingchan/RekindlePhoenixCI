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
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;

/// <summary>
/// david 2019/03/18
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
   public partial class W51020 : FormParent {
      //不可編輯欄位
      private string disableCol = "MMFT_MARKET_CODE";
      private string disableCol2 = "MMFT_ID";

      private COD daoCOD;
      private D51020 dao51020;
      private RepositoryItemLookUpEdit _RepLookUpEdit;
      private RepositoryItemLookUpEdit _RepLookUpEdit2;

      public W51020(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         dao51020 = new D51020();
         daoCOD = new COD();
         GridHelper.SetCommonGrid(gvMain);

         this.Text = _ProgramID + "─" + _ProgramName;

         #region Set Drop Down Lsit
         //交易時段 價平月份 兩個欄位要換成LookUpEdit
         _RepLookUpEdit = new RepositoryItemLookUpEdit();
         //DataTable cbxCPKindSource = daoCOD.ListByCol2("MMFT" , "MMFT_CP_KIND");
         DataTable dtCPKindSource = new CODW().ListLookUpEdit("51020" , "CP_KIND");
         foreach (DataRow dr in dtCPKindSource.Rows) {
            if (dr["CODW_ID"].AsString() == "N") {
               dr["CODW_ID"] = " ";
            }
         }
         //cbxCPKindSource.Rows.Add("" , "");
         _RepLookUpEdit.SetColumnLookUp(dtCPKindSource , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , "");
         gcMain.RepositoryItems.Add(_RepLookUpEdit);
         MMFT_CP_KIND.ColumnEdit = _RepLookUpEdit;

         _RepLookUpEdit2 = new RepositoryItemLookUpEdit();
         //DataTable cbxMarketCodeSource = daoCOD.ListByCol2("MMFT" , "MMFT_MARKET_CODE");
         DataTable dtMarketCodeSource = new CODW().ListLookUpEdit("MMF" , "MMF_MARKET_CODE");
         _RepLookUpEdit2.SetColumnLookUp(dtMarketCodeSource , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , "");
         gcMain.RepositoryItems.Add(_RepLookUpEdit2);
         MMFT_MARKET_CODE.ColumnEdit = _RepLookUpEdit2;
         #endregion

         Retrieve();
      }

      protected override ResultStatus Retrieve() {
         base.Retrieve(gcMain);
         DataTable returnTable = new DataTable();
         returnTable = dao51020.ListAll();
         if (returnTable.Rows.Count == 0) {
            MessageDisplay.Info("無任何資料");
         }
         gcMain.DataSource = returnTable;

         gvMain.BestFitColumns();
         gcMain.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         base.InsertRow(gvMain);
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall poke) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         try {
            DataTable dt = (DataTable)gcMain.DataSource;

            DataTable dtChange = dt.GetChanges();
            DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
            DataTable dtForModified = dt.GetChanges(DataRowState.Modified);
            DataTable dtForDeleted = dt.GetChanges(DataRowState.Deleted);

            if (dtChange == null) {
               MessageDisplay.Info("沒有變更資料, 不需要存檔!");
               return ResultStatus.FailButNext;
            }
            ResultStatus status = dao51020.updateData(dt).Status;

            if (status == ResultStatus.Fail) {
               MessageDisplay.Error("儲存失敗");
               return ResultStatus.Fail;
            }

            //列印新增 刪除 修改 的資料
            AfterSaveForPrint(gcMain , dtForAdd , dtForDeleted , dtForModified);
            //PrintOrExportChangedByKen(gcMain, dtForAdd, dtForDeleted, dtForModified);
         } catch (Exception ex) {
            MessageDisplay.Error("儲存錯誤");
            WriteLog(ex , "" , false);
            return ResultStatus.Fail;
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
            string footerMemo = "";
            string txtFilePath = System.IO.Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH , _ProgramID + ".txt");
            using (System.IO.TextReader tr = new System.IO.StreamReader(txtFilePath , System.Text.Encoding.Default)) {
               string line = "";
               while ((line = tr.ReadLine()) != null) {
                  footerMemo += line + Environment.NewLine;
               }
            }

            gvMain.BestFitColumns(); //避免列印時字被吃掉

            ReportHelper _ReportHelper = new ReportHelper(gcMain , _ProgramID , this.Text);
            CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
            reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            _ReportHelper.FooterMemo = footerMemo;
            _ReportHelper.Create(reportLandscape);

            _ReportHelper.Print();//如果有夜盤會特別標註
            _ReportHelper.Export(FileType.PDF , _ReportHelper.FilePath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus DeleteRow() {
         return base.DeleteRow(gvMain);
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

      /// <summary>
      /// 判斷是否為新增列, 來確認是否可編輯
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
         } else if (gv.FocusedColumn.FieldName == disableCol ||
              gv.FocusedColumn.FieldName == disableCol2) {
            e.Cancel = true;
         }
         gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["MMFT_END_S"] , 0);
         gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["MMFT_END_E"] , 0);
      }

      /// <summary>
      /// 判斷是新增列來改變顏色
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["IS_NEWROW"]) == null ? "0" :
              gv.GetRowCellValue(e.RowHandle , gv.Columns["IS_NEWROW"]).ToString();

         if (e.Column.FieldName == disableCol ||
             e.Column.FieldName == disableCol2) {
            e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.Silver;
         }
      }

      /// <summary>
      /// 新增列時初始設定
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_InitNewRow(object sender , InitNewRowEventArgs e) {
         GridView gv = sender as GridView;
         //新增列, 將IS_NEWROW =1
         gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"] , 1);
      }
   }
}