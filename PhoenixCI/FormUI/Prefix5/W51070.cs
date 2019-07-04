using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Utils;
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
/// Winni, 2019/05/14
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
   /// <summary>
   /// 51070 報價價差放寬維護
   /// </summary>
   public partial class W51070 : FormParent {

      //private string state;
      RepositoryItemLookUpEdit lupPriceFluc;
      ReportHelper _ReportHelper;
      private D51070 dao51070;

      public W51070(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao51070 = new D51070();
      }

      protected override ResultStatus Open() {
         base.Open();
         labTradeDate.Text = "交易日期：" + PbFunc.f_ocf_date(0 , "fut");

         //設定價差數值型態下拉選單
         List<LookupItem> priceFluc = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "F", DisplayMember = " "},
                                        new LookupItem() { ValueMember = "P", DisplayMember = "百分比" }};

         //價差數值型態下拉選單
         lupPriceFluc = new RepositoryItemLookUpEdit();
         lupPriceFluc.SetColumnLookUp(priceFluc , "ValueMember" , "DisplayMember" , TextEditStyles.DisableTextEditor , "");
         gcMain.RepositoryItems.Add(lupPriceFluc);

         wf_retrieve();
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = true;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = true;//列印

         return ResultStatus.Success;
      }

      //RadioButton 判斷
      private int wf_retrieve() {
         try {

            string ls_dw_name, ls_date, systemType;
            if (gbMarket.EditValue.AsString() == "rbMarket1") {
               ls_dw_name = "";
               if (gbType.EditValue.AsString() == "rbTypeFut") {
                  ls_dw_name += "_fut";
                  ls_date = PbFunc.f_ocf_date(0 , "futAH");
                  systemType = "(期貨夜盤)";
               } else {
                  ls_dw_name += "_opt";
                  ls_date = PbFunc.f_ocf_date(0 , "optAH");
                  systemType = "(選擇權夜盤)";
               }
               ls_dw_name += "_AH";
            } else {
               ls_dw_name = "";
               if (gbType.EditValue.AsString() == "rbTypeFut") {
                  ls_dw_name += "_fut";
                  ls_date = PbFunc.f_ocf_date(0 , "fut");
                  systemType = "(期貨一般)";
               } else {
                  ls_dw_name += "_opt";
                  ls_date = PbFunc.f_ocf_date(0 , "opt");
                  systemType = "(選擇權一般)";
               }
            }

            //若資料有變動 未儲存前retrieve則跳出msg
            //if      dw_1.modifiedCount() > 0 then
            //     if      messagebox(gs_t_question , "資料有異動，是否放棄異動？" , Question!, YesNo!, 2) <> 1 then
            //                return -1
            //     end if
            //end if

            DataTable dt = dao51070.ListData(ls_dw_name);
            labTradeDate.Text = "交易日期：" + (ls_date == "0001/01/01" ? " " : ls_date);

            //1. 設定gvMain
            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dt;

            string[] showColCaption = {"商品", "最大值", "最小值","價差",$"月份{Environment.NewLine}順序",
                                       $"報價價差{Environment.NewLine}放寬倍數",$"價差{Environment.NewLine}max",
                                       " " ,$"最低{Environment.NewLine}報價口數", $"價差{Environment.NewLine}數值型態" };

            //1.1 設定欄位caption       
            foreach (DataColumn dc in dt.Columns) {
               gvMain.SetColumnCaption(dc.ColumnName , showColCaption[dt.Columns.IndexOf(dc)]);
               gvMain.Columns[dc.ColumnName].AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
               gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
               //設定合併欄位(一樣的值不顯示)
               gvMain.OptionsView.AllowCellMerge = true;
               gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
               gvMain.Columns[dc.ColumnName].OptionsColumn.AllowMerge = (dc.ColumnName == "SLT_KIND_ID") ? DefaultBoolean.True : DefaultBoolean.False;
            }

            //1.2 設定dropdownlist       
            gvMain.Columns["SLT_PRICE_FLUC"].ColumnEdit = lupPriceFluc;
            lupPriceFluc.ReadOnly = true;

            //1.3 設定欄位順序                
            gvMain.Columns["SLT_KIND_ID"].VisibleIndex = 0;
            gvMain.Columns["SLT_MIN"].VisibleIndex = 1;
            gvMain.Columns["SLT_MAX"].VisibleIndex = 2;
            gvMain.Columns["SLT_SPREAD"].VisibleIndex = 3;

            gvMain.Columns["SLT_SPREAD_LONG"].VisibleIndex = 4;
            gvMain.Columns["SLT_SPREAD_MULTI"].VisibleIndex = 5;
            gvMain.Columns["SLT_SPREAD_MAX"].VisibleIndex = 6;
            gvMain.Columns["SLT_VALID_QNTY"].VisibleIndex = 7;
            gvMain.Columns["SLT_PRICE_FLUC"].VisibleIndex = 8;
            gvMain.Columns["OP_TYPE"].VisibleIndex = 9;

            //1.4 設定cell style
            gvMain.Columns["OP_TYPE"].AppearanceCell.ForeColor = Color.Red;
            gvMain.Columns["SLT_KIND_ID"].VisibleIndex = 1;
            gvMain.Columns["SLT_PRICE_FLUC"].AppearanceCell.ForeColor = Color.Red;

            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);
            gvMain.Columns["OP_TYPE"].Width = 25;
            gcMain.Focus();

            return 1;

         } catch (Exception ex) {
            WriteLog(ex);
            return -1;
         }
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

            dtChange = dtCurrent.GetChanges();
            string ls_dw_name, ls_date, systemType;
            if (gbMarket.EditValue.AsString() == "rbMarket1") {
               ls_dw_name = "";
               if (gbType.EditValue.AsString() == "rbTypeFut") {
                  ls_dw_name += "_fut";
                  ls_date = PbFunc.f_ocf_date(0 , "futAH");
                  systemType = "(期貨夜盤)";
               } else {
                  ls_dw_name += "_opt";
                  ls_date = PbFunc.f_ocf_date(0 , "optAH");
                  systemType = "(選擇權夜盤)";
               }
               ls_dw_name += "_AH";
            } else {
               ls_dw_name = "";
               if (gbType.EditValue.AsString() == "rbTypeFut") {
                  ls_dw_name += "_fut";
                  ls_date = PbFunc.f_ocf_date(0 , "fut");
                  systemType = "(期貨一般)";
               } else {
                  ls_dw_name += "_opt";
                  ls_date = PbFunc.f_ocf_date(0 , "opt");
                  systemType = "(選擇權一般)";
               }
            }

            ResultData result = dao51070.UpdateData(dtChange , ls_dw_name);
            if (result.Status == ResultStatus.Fail) {
               return ResultStatus.Fail;
            }
            Print(_ReportHelper);
            //AfterSaveForPrint(gcMain , dtChange , systemType);

         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Success;
      }

      protected void AfterSaveForPrint(GridControl gridControl , DataTable dtChange , string systemType , bool IsHandlePersonVisible = true , bool IsManagerVisible = true) {
         GridControl gridControlPrint = GridHelper.CloneGrid(gridControl);

         string _ReportTitle = _ProgramID + "─" + _ProgramName + GlobalInfo.REPORT_TITLE_MEMO;
         ReportHelper reportHelper = new ReportHelper(gridControl , _ProgramID , _ReportTitle);
         CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4(); //橫向A4
         reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;

         reportLandscape.IsHandlePersonVisible = IsHandlePersonVisible;
         reportLandscape.IsManagerVisible = IsManagerVisible;
         reportHelper.Create(reportLandscape);

         if (dtChange != null)
            if (dtChange.Rows.Count != 0) {
               gridControlPrint.DataSource = dtChange;
               reportHelper.PrintableComponent = gridControlPrint;
               reportHelper.ReportTitle = _ReportTitle + systemType;

               reportHelper.Print();
               reportHelper.Export(FileType.PDF , reportHelper.FilePath);
            }
      }

      protected override ResultStatus Print(ReportHelper _ReportHelper) {
         try {
            _ReportHelper = new ReportHelper(gcMain , _ProgramID , this.Text);
            CommonReportPortraitA4 reportLandscape = new CommonReportPortraitA4(); //直A4

            reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            _ReportHelper.Create(reportLandscape);

            string leftMemo = labTradeDate.Text;
            _ReportHelper.LeftMemo = leftMemo;

            _ReportHelper.Print();
            _ReportHelper.Export(FileType.PDF , _ReportHelper.FilePath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      private void gbMarket_EditValueChanged(object sender , EventArgs e) {
         wf_retrieve();
      }

      private void gbType_EditValueChanged(object sender , EventArgs e) {
         wf_retrieve();
      }

      private void gvMain_CellValueChanged(object sender , CellValueChangedEventArgs e) {
         GridView gv = sender as GridView;
         if (gv == null) return;
         if (e.Column.FieldName == "SLT_SPREAD" || e.Column.FieldName == "SLT_SPREAD_MULTI"
            || e.Column.FieldName == "SLT_SPREAD_MAX" || e.Column.FieldName == "SLT_VALID_QNTY") {

            //if (e.Value == dt.Rows[e.RowHandle][e.Column.FieldName]) {
            //   gv.SetRowCellValue(e.RowHandle , "OP_TYPE" , " ");
            //} else {
            gv.SetRowCellValue(e.RowHandle , "OP_TYPE" , "※");
            //}         
         }
      }

      /// <summary>
      /// 決定哪些欄位無法編輯的事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;

         if (gv.FocusedColumn.FieldName == "SLT_SPREAD" || gv.FocusedColumn.FieldName == "SLT_SPREAD_MULTI"
            || gv.FocusedColumn.FieldName == "SLT_SPREAD_MAX" || gv.FocusedColumn.FieldName == "SLT_VALID_QNTY") {
            e.Cancel = false;
         } else {
            e.Cancel = true;
         }
      }

      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         GridView gv = sender as GridView;
         //當該欄位不可編輯時,設定為淡灰色 Color.Azure
         //當該欄位不可編輯時,AllowFocus為false(PB的wf_set_order方法)
         switch (e.Column.FieldName) {
            case ("SLT_SPREAD"):
            case ("SLT_SPREAD_MULTI"):
            case ("SLT_SPREAD_MAX"):
            case ("SLT_VALID_QNTY"):
               e.Appearance.BackColor = Color.White;
               break;
            default:
               e.Appearance.BackColor = Color.Azure;
               break;
         }//switch (e.Column.FieldName) {
      }

   }
}