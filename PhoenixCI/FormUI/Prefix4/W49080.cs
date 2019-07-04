using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/4/16
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

   /// <summary>
   /// 49080 ETF契約基本資料維護
   /// </summary>
   public partial class W49080 : FormParent {

      RepositoryItemLookUpEdit lupTfxmPid;
      private D49080 dao49080;

      public W49080(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao49080 = new D49080();
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            lupTfxmPid = new RepositoryItemLookUpEdit();

            //商品
            DataTable dtTfxmPid = new COD().ListByCol2("TFXM" , "TFXM_PID");
            Extension.SetColumnLookUp(lupTfxmPid , dtTfxmPid , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor , "");
            gcMain.RepositoryItems.Add(lupTfxmPid);

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
            DataTable dt = dao49080.GetDataList();

            //0.check (沒有資料時,則自動新增一筆)
            if (dt.Rows.Count <= 0) {
               InsertRow();
            }

            //1. 設定gvMain
            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dt;
            GridHelper.SetCommonGrid(gvMain);

            string[] showColCaption = {$"上櫃{Environment.NewLine}/上市", $"ETF{Environment.NewLine}代號","名稱",
                                       $"漲幅{Environment.NewLine}限制",$"跌幅{Environment.NewLine}限制",
                                       $"期貨{Environment.NewLine}乘數", $"選擇權{Environment.NewLine}乘數",
                                       "異動時間", $"異動/確認{Environment.NewLine}人員", "TFXMSE_SP_W_TIME","" };

            //1.1 設定欄位caption       
            foreach (DataColumn dc in dt.Columns) {
               gvMain.SetColumnCaption(dc.ColumnName , showColCaption[dt.Columns.IndexOf(dc)]);
               gvMain.Columns[dc.ColumnName].AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
               gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
               gvMain.Columns[dc.ColumnName].OptionsColumn.AllowMerge = DefaultBoolean.False;

               //設定欄位header顏色
               if (dc.ColumnName == "TFXMSE_PID") {
                  gvMain.Columns[dc.ColumnName].AppearanceHeader.BackColor = GridHelper.PK;
                  gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
               } else {
                  if (dc.ColumnName == "TFXMSE_W_TIME") {
                     gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                     gvMain.Columns[dc.ColumnName].DisplayFormat.FormatType = FormatType.DateTime;
                     gvMain.Columns[dc.ColumnName].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
                  }
                  gvMain.Columns[dc.ColumnName].AppearanceHeader.BackColor = GridHelper.NORMAL;
               }
            }

            ////1.1 設定欄位caption       
            //gvMain.SetColumnCaption("TFXMSE_PID" , "上櫃/上市");
            //gvMain.SetColumnCaption("TFXMSE_SID" , "ETF代號");
            //gvMain.SetColumnCaption("TFXMSE_SNAME" , "名稱");
            //gvMain.SetColumnCaption("TFXMSE_RAISE_LIMIT" , "漲幅限制");
            //gvMain.SetColumnCaption("TFXMSE_FALL_LIMIT" , "跌幅限制");

            //gvMain.SetColumnCaption("TFXMSE_FUT_XXX" , "期貨乘數");
            //gvMain.SetColumnCaption("TFXMSE_OPT_XXX" , "選擇權乘數");
            //gvMain.SetColumnCaption("TFXMSE_W_TIME" , "異動時間");
            //gvMain.SetColumnCaption("TFXMSE_W_USER_ID" , "異動/確認人員");
            //gvMain.SetColumnCaption("TFXMSE_SP_W_TIME" , "TFXMSE_SP_W_TIME");

            //gvMain.SetColumnCaption("IS_NEWROW" , "Is_NewRow");

            //1.2 設定欄位format格式
            RepositoryItemTextEdit raiseLimit = new RepositoryItemTextEdit();
            gcMain.RepositoryItems.Add(raiseLimit);
            gvMain.Columns["TFXMSE_RAISE_LIMIT"].ColumnEdit = raiseLimit;
            raiseLimit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            raiseLimit.Mask.EditMask = "####0.00";

            RepositoryItemTextEdit fallLimit = new RepositoryItemTextEdit();
            gcMain.RepositoryItems.Add(fallLimit);
            gvMain.Columns["TFXMSE_FALL_LIMIT"].ColumnEdit = fallLimit;
            fallLimit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            fallLimit.Mask.EditMask = "####0.00";

            RepositoryItemTextEdit fut = new RepositoryItemTextEdit();
            gcMain.RepositoryItems.Add(fut);
            gvMain.Columns["TFXMSE_FUT_XXX"].ColumnEdit = fut;
            fut.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            fut.Mask.EditMask = "##########0.0000";

            RepositoryItemTextEdit opt = new RepositoryItemTextEdit();
            gcMain.RepositoryItems.Add(opt);
            gvMain.Columns["TFXMSE_OPT_XXX"].ColumnEdit = opt;
            opt.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            opt.Mask.EditMask = "##########0.0000";


            //1.3 設定隱藏欄位
            gvMain.Columns["TFXMSE_SP_W_TIME"].Visible = false;
            gvMain.Columns["IS_NEWROW"].Visible = false;

            //1.4 設定dropdownlist       
            gvMain.Columns["TFXMSE_PID"].ColumnEdit = lupTfxmPid;

            gvMain.BestFitColumns();
            gcMain.Focus();

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus BeforeClose() {

         DataTable dtCurrent = (DataTable)gcMain.DataSource;
         DataTable dtChange = dtCurrent.GetChanges();

         if (dtChange == null) {
            return ResultStatus.Success;
         }

         DialogResult myDialogResult = ConfirmToExitWithoutSave(dtChange);

         if (myDialogResult == DialogResult.Yes) { return ResultStatus.Success; } else if (myDialogResult == DialogResult.No) { return ResultStatus.FailButNext; } else if (myDialogResult == DialogResult.Cancel) { return ResultStatus.Fail; }

         return ResultStatus.Success;
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
               //if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified) {
               //   dr["TFXMSE_W_TIME"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fffffff");
               //   dr["TFXMSE_W_USER_ID"] = GlobalInfo.USER_ID;
               //}

               {
                  if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified) {
                     dr["TFXMSE_W_TIME"] = DateTime.Now;
                     dr["TFXMSE_W_USER_ID"] = GlobalInfo.USER_ID;

                     //if (dr.RowState == DataRowState.Added) {
                     //   foreach (DataRow drAdd in dtForAdd.Rows) {
                     //      drAdd["TFXMSE_W_TIME"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                     //      drAdd["TFXMSE_W_USER_ID"] = GlobalInfo.USER_ID;
                     //   }
                     //   gvMain.SetRowCellValue(gvMain.FocusedRowHandle , "TFXMSE_W_TIME" , DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                     //   gvMain.SetRowCellValue(gvMain.FocusedRowHandle , "TFXMSE_W_USER_ID" , GlobalInfo.USER_ID);
                     //   gvMain.CloseEditor();
                     //   gvMain.UpdateCurrentRow();
                     //}

                     //if (dr.RowState == DataRowState.Modified) {
                     //   foreach (DataRow drMod in dtForModified.Rows) {
                     //      drMod["TFXMSE_W_TIME"] = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                     //      drMod["TFXMSE_W_USER_ID"] = GlobalInfo.USER_ID;
                     //   }
                     //   gvMain.SetRowCellValue(gvMain.FocusedRowHandle , "TFXMSE_W_TIME" , DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                     //   gvMain.SetRowCellValue(gvMain.FocusedRowHandle , "TFXMSE_W_USER_ID" , GlobalInfo.USER_ID);
                     //   gvMain.CloseEditor();
                     //   gvMain.UpdateCurrentRow();
                     //}

                     //if (dr.RowState == DataRowState.Deleted) {
                     //   dr.Delete();
                     //}
                  }
               }
            }

            //DataTable dtCloned = dtCurrent.Clone();
            //dtCloned.Columns["TFXMSE_W_TIME"].DataType = typeof(DateTime);
            //foreach (DataRow row in dtCurrent.Rows) {
            //    if (row.RowState == DataRowState.Deleted) continue;
            //    dtCloned.ImportRow(row);
            //}

            ResultData result = new TFXMSE().UpdateData(dtCurrent);
            if (result.Status == ResultStatus.Fail) {
               MessageDisplay.Error("儲存失敗" , GlobalInfo.ErrorText);
               return ResultStatus.FailButNext;
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

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["IS_NEWROW"] , 1);

         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);
         return ResultStatus.Success;
      }

      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["IS_NEWROW"]) == null ? "0" :
                   gv.GetRowCellValue(e.RowHandle , gv.Columns["IS_NEWROW"]).ToString();

         switch (e.Column.FieldName) {
            case ("TFXMSE_W_TIME"):
            case ("TFXMSE_W_USER_ID"):
               e.Column.OptionsColumn.AllowFocus = false;
               e.Appearance.BackColor = Color.Transparent;
               break;
            case ("TFXMSE_PID"):
            case ("TFXMSE_SID"):
            case ("TFXMSE_SNAME"):
               e.Appearance.BackColor = Is_NewRow == "1" ? Color.Pink : Color.White;
               //e.Appearance.BackColor = Color.Pink;
               break;
            default:
               e.Appearance.BackColor = Color.White;
               break;
         }//switch (e.Column.FieldName) 
      }
   }
}