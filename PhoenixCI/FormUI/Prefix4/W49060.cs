using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BaseGround;
using BaseGround.Shared;
using DevExpress.XtraGrid.Views.Grid;
using BusinessObjects.Enums;
using BaseGround.Report;
using BusinessObjects;
using DataObjects.Dao.Together.TableDao;
using Common;
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;

/// <summary>
/// Winni, 2019/3/19
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

   /// <summary>
   /// 49060 國外保證金資料輸入
   /// </summary>
   public partial class W49060 : FormParent {

      private ReportHelper _ReportHelper;
      protected DataTable dtForDeleted;
      DataTable retDt = new DataTable(); //存retrrieve後的datatable

      #region 一般交易查詢條件縮寫
      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string StartDate {
         get {
            return txtStartDate.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string EndDate {
         get {
            return txtEndDate.DateTimeValue.ToString("yyyyMMdd");
         }
      }
      #endregion

      public W49060(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         GridHelper.SetCommonGrid(gvMain);
         dtForDeleted = new DataTable();
      }

      protected override ResultStatus Open() {
         base.Open();
         txtStartDate.Text = GlobalInfo.OCF_DATE.AsString("yyyy/01/01");
         txtEndDate.EditValue = GlobalInfo.OCF_DATE.AsString("yyyy/MM/dd");

#if DEBUG
         //winni test
         txtStartDate.DateTimeValue = DateTime.ParseExact("2019/01/01" , "yyyy/MM/dd" , null);
         txtEndDate.DateTimeValue = DateTime.ParseExact("2019/03/20" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),Date=2019/01/01~2019/03/20";
#endif

         RepositoryItemLookUpEdit _RepLookUpEdit = new RepositoryItemLookUpEdit();
         DataTable dtTradekindId = new MGT8().ListDataByMGT8(); //交易所+商品的dropdownlist
         Extension.SetColumnLookUp(_RepLookUpEdit , dtTradekindId , "MGT8_F_ID" , "CP_DISPLAY" , TextEditStyles.DisableTextEditor , "");
         gcMain.RepositoryItems.Add(_RepLookUpEdit);
         MG8_F_ID.ColumnEdit = _RepLookUpEdit;
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

         retDt = new MG8().ListData(StartDate , EndDate);

         if (retDt.Rows.Count == 0) {
            MessageDisplay.Info("無任何資料");
         }

         gcMain.Visible = true;
         retDt.Columns.Add("Is_NewRow" , typeof(string));
         gcMain.DataSource = retDt;
         gcMain.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {

         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();
         ResultStatus resultStatus = ResultStatus.Fail;

         DataTable dt = (DataTable)gcMain.DataSource; //抓取目前gcMain
         DataTable dtKindId = new MGT8().ListDataByMGT8(); //取得dropdownlist資料
         DataTable dtChange = dt.GetChanges();
         DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
         DataTable dtForModified = dt.GetChanges(DataRowState.Modified);

         if (dtChange != null) {
            if (dtChange.Rows.Count == 0) {
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }
         //Update to DB
         else {
               //隱藏欄位賦值
               foreach (DataRow dr in dt.Rows) {
                  if (dr.RowState == DataRowState.Added) {
                     dr["MG8_W_TIME"] = DateTime.Now;
                     dr["MG8_W_USER_ID"] = GlobalInfo.USER_ID;
                  }
                  if (dr.RowState == DataRowState.Modified) {
                     dr["AM7T_W_TIME"] = DateTime.Now;
                     dr["AM7T_W_USER_ID"] = GlobalInfo.USER_ID;

                     string effectYmd = dr["mg8_effect_ymd"].AsString();
                     string fId = dr["mg8_f_id"].AsString();
                     int found = retDt.Rows.IndexOf(retDt.Rows.Find(string.Format("mg8_effect_ymd='{0}' and mg8_f_id='{1}'" , effectYmd , fId)));

                     ////寫異動LOG: 紀錄異動前後的值
                     if (found < 0) continue;
                     for (int w = 2 ; 2 <= 5 ; w++) {
                        if (dr[w] != retDt.Rows[found][w]) {
                           string befChange = dr[w].AsString();
                           string aftChange = retDt.Rows[found][w].AsString();
                           WriteLog(string.Format("變更後:{0},原始:{1}" , aftChange , befChange) , "Info" , "U");
                        }
                     }
                  }
               }
               //dt.Columns.Remove("OP_TYPE");
               //dt.Columns.Remove("Is_NewRow");
               ResultData result = new MG8().UpdateData(dt);//base.Save_Override(dt, "MG8");
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
         return ResultStatus.Success;
      }


      protected override ResultStatus Print(ReportHelper reportHelper) {
         _ReportHelper = reportHelper;
         CommonReportPortraitA4 report = new CommonReportPortraitA4();
         report.printableComponentContainerMain.PrintableComponent = gcMain;
         _ReportHelper.Create(report);

         base.Print(_ReportHelper);
         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         gvMain.AddNewRow();
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MG8_EFFECT_YMD"] , DateTime.Now.ToString("yyyyMMdd"));
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MG8_ISSUE_YMD"] , DateTime.Now.ToString("yyyyMMdd"));
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MG8_CM"] , 0);
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MG8_MM"] , 0);
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MG8_IM"] , 0);
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["Is_NewRow"] , 1);

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

      #region GridControl事件

      /// <summary>
      /// 決定哪些欄位無法編輯的事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
              gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false;
            gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"] , 1);
         }
         //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
         else if (gv.FocusedColumn.Name == "MG8_EFFECT_YMD" || gv.FocusedColumn.Name == "MG8_F_ID") {
            e.Cancel = true;
         } else {
            e.Cancel = false;
         }

      }

      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         //要用RowHandle不要用FocusedRowHandle
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
                            gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]).ToString();

         //描述每個欄位,在is_newRow時候要顯示的顏色
         //當該欄位不可編輯時,設定為灰色 Color.FromArgb(192,192,192)
         //當該欄位不可編輯時,AllowFocus為false(PB的wf_set_order方法)
         switch (e.Column.FieldName) {
            case ("MG8_EFFECT_YMD"):
            case ("MG8_F_ID"):
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