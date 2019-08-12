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
/// Lukas, 2019/1/24
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {

   /// <summary>
   /// 20430 各商品年度預估日均量維護(交易部)
   /// </summary>
   public partial class W20430 : FormParent {

      private ReportHelper _ReportHelper;
      private AM7T daoAM7T;
      private LOGV daoLOGV;
      private APDK_PARAM daoAPDK_PARAM;
      protected DataTable dtForDeleted;

      public W20430(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         GridHelper.SetCommonGrid(gvMain);
         daoAM7T = new AM7T();
         daoAPDK_PARAM = new APDK_PARAM();
         dtForDeleted = new DataTable();
      }

      protected override ResultStatus Open() {
         base.Open();
         txtDate.EditValue = GlobalInfo.OCF_DATE.Year;

         RepositoryItemLookUpEdit _RepLookUpEdit = new RepositoryItemLookUpEdit();
         DataTable lookUpDt = daoAPDK_PARAM.ListAll2(_ProgramID);
         Extension.SetColumnLookUp(_RepLookUpEdit , lookUpDt , "PARAM_KEY" , "CP_DISPLAY" , TextEditStyles.DisableTextEditor , "");
         gcMain.RepositoryItems.Add(_RepLookUpEdit);
         AM7T_PARAM_KEY.ColumnEdit = _RepLookUpEdit;
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();

         //先確認有沒有資料(這邊不直接下Retrieve是為了不跳錯誤訊息)
         DataTable dtCheck = daoAM7T.ListAllByDate(txtDate.Text);
         dtForDeleted = dtCheck.Clone();
         //沒有新增資料時,則自動新增內容
         if (dtCheck.Rows.Count == 0) {
            dtCheck.Columns.Add("Is_NewRow" , typeof(string));
            gcMain.DataSource = dtCheck;
            InsertRow();
         } else {
            dtCheck.Columns.Add("Is_NewRow" , typeof(string));
            gcMain.DataSource = dtCheck;
            gcMain.Focus();
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

         DataTable returnTable = daoAM7T.ListAllByDate(txtDate.Text);

         if (returnTable.Rows.Count == 0) {
            MessageBox.Show("無任何資料" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
            //需要讓grid清空資料所以不return fail
            //return ResultStatus.Fail;
         }

         returnTable.Columns.Add("Is_NewRow" , typeof(string));
         //dtForDeleted = returnTable.Clone();
         gcMain.DataSource = returnTable;
         gcMain.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus CheckShield() {
         base.CheckShield(gcMain);
         if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {
         base.Save(gcMain);
         daoLOGV = new LOGV();
         DataTable dt = (DataTable)gcMain.DataSource;
         DataTable dtProdType = daoAPDK_PARAM.ListAll2();
         DataTable dtChange = dt.GetChanges();
         DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
         DataTable dtForModified = dt.GetChanges(DataRowState.Modified);

         if (dtChange == null) {
            MessageBox.Show("沒有變更資料,不需要存檔!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            return ResultStatus.Fail;
         }
         if (dtChange.Rows.Count == 0) {
            MessageBox.Show("沒有變更資料,不需要存檔!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            return ResultStatus.Fail;
         }
         //Update to DB
         else {
            string val1, val2, val3, val4;
            string prodType, subProdType;
            //隱藏欄位賦值
            foreach (DataRow dr in dt.Rows) {
               if (dr.RowState == DataRowState.Added) {
                  dr["AM7T_W_TIME"] = DateTime.Now;
                  dr["AM7T_W_USER_ID"] = GlobalInfo.USER_ID;
                  //從下拉選單的table去取得對應的商品類別和副類別
                  string selectStr = "PARAM_KEY=" + "'" + dr["AM7T_PARAM_KEY"] + "'";
                  prodType = dtProdType.Select(selectStr)[0]["PARAM_PROD_TYPE"].ToString();
                  subProdType = dtProdType.Select(selectStr)[0]["PARAM_PROD_SUBTYPE"].ToString();
                  dr["AM7T_PROD_TYPE"] = prodType;
                  dr["AM7T_PROD_SUBTYPE"] = subProdType;
               }
               if (dr.RowState == DataRowState.Modified) {
                  dr["AM7T_W_TIME"] = DateTime.Now;
                  dr["AM7T_W_USER_ID"] = GlobalInfo.USER_ID;
                  //寫LOGV
                  val1 = dr["AM7T_Y"].AsString();
                  val2 = dr["AM7T_PARAM_KEY"].AsString();
                  val3 = dr["ORG_DAY_COUNT"].AsString() + "->" + dr["AM7T_DAY_COUNT"].AsString();
                  val4 = dr["ORG_AVG_QNTY"].AsString() + "->" + dr["AM7T_AVG_QNTY"].AsString();
                  daoLOGV.Insert(_ProgramID , GlobalInfo.USER_ID , "U" , val1 , val2 , val3 , val4 , "");
               }
            }
            ResultData myResultData = daoAM7T.UpdateAM7T(dt);
            if (myResultData.Status == ResultStatus.Fail) {
               MessageDisplay.Error("更新資料庫AM7T錯誤! ");
               return ResultStatus.Fail;
            }
            //列印(新增/刪除/修改)
            PrintOrExportChangedByKen(gcMain , dtForAdd , dtForDeleted , dtForModified);
            dtForDeleted.Clear();
            _IsPreventFlowPrint = true;
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
         DataTable dtCurrent = (DataTable)gcMain.DataSource;
         if (dtCurrent.Rows.Count == 0) {
            DialogResult result = MessageBox.Show("請問要新增「" + txtDate.Text + "」所有商品資料?" , "請選擇" , MessageBoxButtons.YesNo , MessageBoxIcon.Question);
            if (result == DialogResult.Yes) {
               DataTable dtInsertAll = daoAPDK_PARAM.ListAllForInsert();
               foreach (DataRow dr in dtInsertAll.Rows) {
                  gvMain.AddNewRow();
                  gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["AM7T_Y"] , txtDate.Text);
                  gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["AM7T_PROD_TYPE"] , dr["PARAM_PROD_TYPE"]);
                  gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["AM7T_PROD_SUBTYPE"] , dr["PARAM_PROD_SUBTYPE"]);
                  gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["AM7T_PARAM_KEY"] , dr["PARAM_KEY"]);
                  gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["Is_NewRow"] , 1);
               }
            } else {
               gvMain.AddNewRow();
               gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["AM7T_Y"] , txtDate.Text);
               gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["Is_NewRow"] , 1);
            }
         } else {
            gvMain.AddNewRow();
            gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["AM7T_Y"] , txtDate.Text);
            gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["AM7T_PARAM_KEY"] , "%");
            gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["Is_NewRow"] , 1);
         }
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
         else if (gv.FocusedColumn.Name == "AM7T_Y" || gv.FocusedColumn.Name == "AM7T_PARAM_KEY") {
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
            case ("AM7T_Y"):
            case ("AM7T_PARAM_KEY"):
               //e.Column.OptionsColumn.AllowFocus = Is_NewRow == "1" ? true : false;
               e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(224 , 224 , 224);
               break;
            default:
               e.Appearance.BackColor = Color.White;
               break;
         }//switch (e.Column.FieldName) {

      }

      #endregion
   }
}