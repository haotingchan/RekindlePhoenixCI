using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
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
/// Winni, 2019/3/19
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

   /// <summary>
   /// 49040 保證金維持與原始乘數設定
   /// </summary>
   public partial class W49040 : FormParent {

      protected DataTable dtForDeleted;
      private RepositoryItemLookUpEdit lupType;

      public W49040(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         GridHelper.SetCommonGrid(gvMain);
         dtForDeleted = new DataTable();
      }

      protected override ResultStatus Open() {
         base.Open();

         RepositoryItemLookUpEdit _RepLookUpEdit = new RepositoryItemLookUpEdit();
         lupType = new RepositoryItemLookUpEdit();

         //dropdownlist
         DataTable dtDropType = new DataTable();
         dtDropType.Columns.Add("PARAM_KEY" , typeof(string));
         dtDropType.Columns.Add("CP_DISPLAY" , typeof(string));
         DataRow row1 = dtDropType.NewRow();
         row1["PARAM_KEY"] = "-";
         row1["CP_DISPLAY"] = "無";
         dtDropType.Rows.Add(row1);
         DataRow row2 = dtDropType.NewRow();
         row2["PARAM_KEY"] = "A";
         row2["CP_DISPLAY"] = "A值";
         dtDropType.Rows.Add(row2);
         DataRow row3 = dtDropType.NewRow();
         row3["PARAM_KEY"] = "B";
         row3["CP_DISPLAY"] = "B值";
         dtDropType.Rows.Add(row3);

         Extension.SetColumnLookUp(lupType , dtDropType , "PARAM_KEY" , "CP_DISPLAY" , TextEditStyles.DisableTextEditor , "");
         gcMain.RepositoryItems.Add(lupType);

         Retrieve();

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();

         //先確認有沒有資料(這邊不直接下Retrieve是為了不跳錯誤訊息)
         DataTable dtCheck = new MGT4().ListDataByMGT4();
         dtForDeleted = dtCheck.Clone();

         //0.check (沒有資料時,則自動新增一筆)
         if (dtCheck.Rows.Count <= 0) {
            InsertRow();
         }

         return ResultStatus.Success;
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
            //1. 讀取資料
            DataTable dt = new MGT4().ListDataByMGT4();

            //好像讀取此Table都一定會有資料,先寫著
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(MessageDisplay.MSG_NO_DATA , GlobalInfo.ResultText);
            }

            //2. 設定gvMain
            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dt;
            // GridHelper.SetCommonGrid(gvMain);

            string[] showColCaption = {"商品", $"類{Environment.NewLine}別", $"現行{Environment.NewLine}維持比率",
                                       $"現行{Environment.NewLine}原始比率",$"現行{Environment.NewLine}進位數",
                                       $"本日{Environment.NewLine}維持比率",$"本日{Environment.NewLine}原始比率",
                                       $"本日{Environment.NewLine}進位數","MGT4_W_TIME","MGT4_W_USER_ID",
                                       $"本日{Environment.NewLine}結算進位數"," "};

            //2.1 設定欄位caption       
            foreach (DataColumn dc in dt.Columns) {
               gvMain.SetColumnCaption(dc.ColumnName , showColCaption[dt.Columns.IndexOf(dc)]);
               gvMain.Columns[dc.ColumnName].AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
               gvMain.Columns[dc.ColumnName].AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
               gvMain.Columns[dc.ColumnName].OptionsColumn.AllowMerge = DefaultBoolean.False;

               //設定欄位header顏色
               if (dc.ColumnName == "MGT4_KIND_ID") {
                  gvMain.Columns[dc.ColumnName].AppearanceHeader.BackColor = GridHelper.PK;
               } else {
                  gvMain.Columns[dc.ColumnName].AppearanceHeader.BackColor = GridHelper.NORMAL;
                  if (dc.ColumnName == "MGT4_M_MULTI" || dc.ColumnName == "MGT4_I_MULTI" ||
                      dc.ColumnName == "MGT4_DIGITAL" || dc.ColumnName == "MGT4_M_DIGITAL") {
                     gvMain.Columns[dc.ColumnName].AppearanceHeader.ForeColor = Color.Navy;
                  }
               }
            }

            //1.2 設定隱藏欄位
            gvMain.Columns["MGT4_W_TIME"].Visible = false;
            gvMain.Columns["MGT4_W_USER_ID"].Visible = false;
            gvMain.Columns["IS_NEWROW"].Visible = false;

            //1.3 設定dropdownlist       
            gvMain.Columns["MGT4_TYPE"].ColumnEdit = lupType;

            gcMain.DataSource = dt;
            gvMain.BestFitColumns();
            gvMain.Columns["MGT4_TYPE"].Width = 40;
            GridHelper.SetCommonGrid(gvMain);
            gcMain.Focus();

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;

      }

      protected override ResultStatus InsertRow() {

         gvMain.AddNewRow();
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["IS_NEWROW"] , 1);

         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         GridView gv = gvMain as GridView;
         DataRowView deleteRowView = (DataRowView)gv.GetFocusedRow();
         dtForDeleted.ImportRow(deleteRowView.Row);
         base.DeleteRow(gvMain);
         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {
         try {
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();
            ResultStatus resultStatus = ResultStatus.Fail;

            DataTable dt = (DataTable)gcMain.DataSource;
            DataTable dtChange = dt.GetChanges();
            DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
            DataTable dtForModified = dt.GetChanges(DataRowState.Modified);

            if (dtChange != null) {
               if (dtChange.Rows.Count == 0) {
                  MessageDisplay.Warning("沒有變更資料,不需要存檔!" , GlobalInfo.WarningText);
                  return ResultStatus.Fail;
               } else {
                  foreach (DataRow dr in dt.Rows) {
                     if (dr.RowState == DataRowState.Added) {

                        foreach (DataRow drAdd in dtForAdd.Rows) {
                           for (int w = 0 ; w < dtForAdd.Rows.Count ; w++) {
                              if (string.IsNullOrEmpty(drAdd[w].AsString())) {
                                 MessageDisplay.Warning("新增資料欄位不可為空!" , GlobalInfo.WarningText);
                                 return ResultStatus.Fail;
                              }
                           }
                        }

                        dr["MGT4_W_TIME"] = DateTime.Now;
                        dr["MGT4_W_USER_ID"] = GlobalInfo.USER_ID;
                     }

                     if (dr.RowState == DataRowState.Modified) {
                        dr["MGT4_W_TIME"] = DateTime.Now;
                        dr["MGT4_W_USER_ID"] = GlobalInfo.USER_ID;
                     }
                  }

                  dt.Columns.Remove("IS_NEWROW");
                  ResultData result = new MGT4().UpdateData(dt);//base.Save_Override(dt, "MGT4");
                  if (result.Status == ResultStatus.Fail) {
                     return ResultStatus.Fail;
                  }
               }

               if (resultStatus == ResultStatus.Success) {

                  PrintableComponent = gcMain;
               }
            }

            //不要自動列印
            _IsPreventFlowPrint = true;
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

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
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
            //gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"] , 1);
         }

         //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
         else if (gv.FocusedColumn.FieldName == "MGT4_KIND_ID" || gv.FocusedColumn.FieldName == "MGT4_TYPE") {
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
            case ("MGT4_KIND_ID"):
            case ("MGT4_TYPE"):
               //e.Column.OptionsColumn.AllowFocus = Is_NewRow == "1" ? true : false;
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