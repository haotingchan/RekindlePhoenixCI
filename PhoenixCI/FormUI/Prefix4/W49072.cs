using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using System;
using System.Data;

/// <summary>
/// Winni, 2019/4/10
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

   /// <summary>
   /// 49071 Delta契約價值乘數設定
   /// </summary>
   public partial class W49072 : FormParent {

      RepositoryItemLookUpEdit lupSpnt1;
      private D49072 dao49072;

      public W49072(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            lupSpnt1 = new RepositoryItemLookUpEdit();
            dao49072 = new D49072();

            //商品
            CODW codw = new CODW();
            //DataTable dtSpnt1 = cod.ListByTxn("SPNT1");
            DataTable dtSpnt1 = codw.ListLookUpEdit("49072", "49072_SPNT1_TYPE");
            Extension.SetColumnLookUp(lupSpnt1 , dtSpnt1 , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , "");
            gcMain.RepositoryItems.Add(lupSpnt1);

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
            DataTable dt = dao49072.ListData();

            //0.check (沒有資料時,則自動新增一筆)
            if (dt.Rows.Count <= 0) {
               InsertRow();
            }

            //1. 設定gvExport
            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dt;
            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);

            //1.1 設定欄位caption       
            gvMain.SetColumnCaption("SPNT1_TYPE" , "商品");
            gvMain.SetColumnCaption("SPNT1_DAYS" , "天期");
            gvMain.SetColumnCaption("SPNT1_VAL" , "值");
            gvMain.SetColumnCaption("SPNT1_W_TIME" , "SPNT1_W_TIME");
            gvMain.SetColumnCaption("SPNT1_W_USER_ID" , "SPNT1_W_USER_ID");

            gvMain.SetColumnCaption("IS_NEWROW" , "Is_NewRow");

            //設定欄位header顏色
            gvMain.Columns["SPNT1_TYPE"].AppearanceHeader.BackColor = GridHelper.PK;
            gvMain.Columns["SPNT1_DAYS"].AppearanceHeader.BackColor = GridHelper.PK;
            gvMain.Columns["SPNT1_VAL"].AppearanceHeader.BackColor = GridHelper.PK;

            //1.2 設定欄位format格式
            RepositoryItemTextEdit type = new RepositoryItemTextEdit();
            gcMain.RepositoryItems.Add(type);
            gvMain.Columns["SPNT1_VAL"].ColumnEdit = type;
            type.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            type.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            type.DisplayFormat.FormatString = "###0.########";
            type.Mask.EditMask = "####0.########";

            //1.3 設定隱藏欄位
            gvMain.Columns["SPNT1_W_TIME"].Visible = false;
            gvMain.Columns["SPNT1_W_USER_ID"].Visible = false;
            gvMain.Columns["IS_NEWROW"].Visible = false;

            //1.4 設定dropdownlist       
            gvMain.Columns["SPNT1_TYPE"].ColumnEdit = lupSpnt1;

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
               MessageDisplay.Warning("沒有變更資料,不需要存檔!" , GlobalInfo.WarningText);
               return ResultStatus.Fail;
            }
            if (dtChange.Rows.Count == 0) {
               MessageDisplay.Warning("沒有變更資料,不需要存檔!" , GlobalInfo.WarningText);
               return ResultStatus.Fail;
            }

            //隱藏欄位賦值
            foreach (DataRow dr in dtCurrent.Rows) {
               if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified) {
                  dr["SPNT1_W_TIME"] = DateTime.Now;
                  dr["SPNT1_W_USER_ID"] = GlobalInfo.USER_ID;
               }
            }

            dtChange = dtCurrent.GetChanges();
            ResultData result = new SPNT1().UpdateData(dtChange);
            if (result.Status == ResultStatus.Fail) {
               return ResultStatus.Fail;
            }

         } catch (Exception ex) {
            WriteLog(ex);
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

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["SPNT1_TYPE"] , " ");
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["SPNT1_DAYS"] , 0);
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["SPNT1_VAL"] , 0);
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["IS_NEWROW"] , 1);

         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);
         return ResultStatus.Success;
      }
   }
}