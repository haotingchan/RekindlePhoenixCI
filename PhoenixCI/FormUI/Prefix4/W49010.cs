using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BaseGround.Widget;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/04/11
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 49010 最小風險價格係數維護
   /// </summary>
   public partial class W49010 : FormParent {

      RepositoryItemLookUpEdit lupProdSubtype;
      RepositoryItemLookUpEdit lupKindId;
      private D49010 dao49010;

      public W49010(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         gvMain.OptionsView.ShowColumnHeaders = false;
         //gvMain.OptionsPrint.PrintBandHeader = true;
         gvMain.OptionsPrint.PrintHeader = false;
         //this.gvMain.ShowingEditor += gvMain_ShowingEditor;
         this.gvMain.RowCellStyle += gvMain_RowCellStyle;
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
            Extension.SetColumnLookUp(lupKindId , dtProdKindId , "MGT2_KIND_ID" , "MGT2_KIND_ID" , TextEditStyles.DisableTextEditor);
            gcMain.RepositoryItems.Add(lupKindId);

            DataTable dtAll = dao49010.GetDataList();
            DataTable dt = dtAll.Clone();

            //1. 設定gvMain
            gcMain.DataSource = dt;
            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);

            gridBand9.Caption = "(輸入方式：" + Environment.NewLine + "如3.5%，" + Environment.NewLine + "則輸入0.035)";

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
         _ToolBtnPrintAll.Enabled = true;//列印

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {
         int printStep = 0;

         try {
            DataTable dtCurrent = (DataTable)gcMain.DataSource;
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            DataTable dtChange = dtCurrent.GetChanges();

            if (dtChange == null) {
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }
            if (dtChange.Rows.Count == 0) {
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }

            DialogResult liRtn;
            foreach (DataRow dr in dtCurrent.Rows) {
               if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified) {
                  dr["CPR_W_TIME"] = DateTime.Now;
                  dr["CPR_W_USER_ID"] = GlobalInfo.USER_ID;
                  dr["CPR_DATA_NUM"] = 0; //隱藏欄位賦值

                  if (dr["CPR_PRICE_RISK_RATE"] == DBNull.Value) {
                     string kind = dr["CPR_KIND_ID"].AsString();

                     liRtn = MessageDisplay.Choose(string.Format("{0}最小風險價格係數欄位為空白，請確認是否為已下市契約" , kind));
                     if (liRtn == DialogResult.No) {
                        return ResultStatus.Fail;
                     } else {
                        dr["CPR_PRICE_RISK_RATE"] = DBNull.Value;
                     }
                  }
               }
            } //foreach (DataRow dr in dtCurrent.Rows)
            dtChange = dtCurrent.GetChanges();

            gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_W_TIME"] , DateTime.MinValue);
            gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_W_USER_ID"] , GlobalInfo.USER_ID);
            gvMain.UpdateCurrentRow();

            printStep = 1; //跑儲存前確認單
            CheckPrint(gcMain , dtChange , printStep);
            liRtn = MessageDisplay.Choose("已列印確認單，點選確認進行儲存資料");
            if (liRtn == DialogResult.No) {
               return ResultStatus.Fail;
            } else {

               ResultData result = new HCPR().UpdateData(dtChange);
               if (result.Status == ResultStatus.Fail) {
                  return ResultStatus.Fail;
               }

               printStep = 2; //儲存後列印已確認單
               CheckPrint(gcMain , dtChange , printStep);
            }

         } catch (Exception ex) {
            throw ex;
         } finally {
            this.Refresh();
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         DataTable dt = (DataTable)gcMain.DataSource;
         gvMain.AddNewRow();
         gvMain.OptionsView.RowAutoHeight = true; //整個grid設定要開，不然設定column會無效

         RepositoryItemTextDateEdit wTime = new RepositoryItemTextDateEdit();
         RepositoryItemMemoEdit can = new RepositoryItemMemoEdit();
         RepositoryItemMemoEdit remark = new RepositoryItemMemoEdit();

         gcMain.RepositoryItems.Add(wTime);
         gcMain.RepositoryItems.Add(can);
         gcMain.RepositoryItems.Add(remark);

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_PROD_SUBTYPE"] , "");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_KIND_ID"] , " ");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_EFFECTIVE_DATE"] , DateTime.ParseExact("1900/01/01" , "yyyy/MM/dd" , null));
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_APPROVAL_DATE"] , DateTime.ParseExact("1900/01/01" , "yyyy/MM/dd" , null));

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
         gvMain.Columns["CPR_W_TIME"].ColumnEdit = wTime;
         //gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["CPR_W_USER_ID"] , GlobalInfo.USER_ID);

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["IS_NEWROW"] , 1);

         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected void CheckPrint(GridControl gridControl , DataTable dtChange , int printSetp ,
                                         bool IsHandlePersonVisible = true , bool IsManagerVisible = true) {
         try {
            GridControl gridControlPrint = GridHelper.CloneGrid(gridControl);

            ReportHelper reportHelper = new ReportHelper(gridControl , _ProgramID , this.Text);
            //reportHelper.IsHandlePersonVisible = IsHandlePersonVisible;
            //reportHelper.IsManagerVisible = IsManagerVisible;

            gridControlPrint.DataSource = dtChange;
            //reportHelper.PrintableComponent = gridControlPrint; // 加這行bands會不見
            if (printSetp == 1) {
               reportHelper.ReportTitle = this.Text + "─" + "(確認單)";
            } else {
               reportHelper.ReportTitle = this.Text + "─" + "(已確認)";
            }

            CommonReportLandscapeA4 report = new CommonReportLandscapeA4(); //設定為橫向列印
            report.printableComponentContainerMain.PrintableComponent = gcMain;
            reportHelper.Create(report);

            //base.Print(reportHelper);
            reportHelper.Print();
            reportHelper.Export(FileType.PDF , reportHelper.FilePath);

         } catch (Exception ex) {
            throw ex;
         }
      }

      #region 下拉選單連動
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
      #endregion

      /// <summary>
      /// 決定哪些欄位無法編輯的事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      //private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
      //   GridView gv = sender as GridView;
      //   if (gv.FocusedColumn.Name == "CPR_W_TIME" || gv.FocusedColumn.Name == "CPR_W_USER_ID") {
      //      e.Cancel = true;
      //   } else {
      //      e.Cancel = false;
      //   }
      //}

      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         GridView gv = sender as GridView;

         switch (e.Column.FieldName) {
            case ("CPR_W_TIME"):
            case ("CPR_W_USER_ID"):
               e.Column.OptionsColumn.AllowFocus = false;
               e.Appearance.BackColor = Color.Transparent;
               break;
            default:
               e.Appearance.BackColor = Color.White;
               break;
         }//switch (e.Column.FieldName) 
      }
   }
}