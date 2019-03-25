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
using Common;
using BusinessObjects.Enums;
using BusinessObjects;
using BaseGround.Report;
using DevExpress.XtraEditors.Repository;
using BaseGround.Shared;
using DevExpress.XtraGrid.Views.Grid;
using DataObjects.Dao.Together.TableDao;

namespace PhoenixCI.FormUI.Prefix4
{
   public partial class W49010 : FormParent
   {
      public W49010(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         GridHelper.SetCommonGrid(gvMain);
      }

      protected override ResultStatus Open()
      {
         base.Open();

         RepositoryItemLookUpEdit _RepLookUpEdit = new RepositoryItemLookUpEdit();

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();

         //先確認有沒有資料(這邊不直接下Retrieve是為了不跳錯誤訊息)
         DataTable dtCheck = new MGT4().ListDataByMGT4();

         //沒有新增資料時,則自動新增內容
         if (dtCheck.Rows.Count == 0) {
            dtCheck.Columns.Add("Is_NewRow", typeof(string));
            gcMain.DataSource = dtCheck;
            InsertRow();
         }
         else {
            dtCheck.Columns.Add("Is_NewRow", typeof(string));
            gcMain.DataSource = dtCheck;
            gcMain.Focus();
         }

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
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

      protected override ResultStatus Retrieve()
      {

         DataTable dt = new MGT4().ListDataByMGT4();

         //好像讀取此Table都一定會有資料,先寫著
         if (dt.Rows.Count <= 0) {
            MessageDisplay.Info("無任何資料");
         }

         dt.Columns.Add("Is_NewRow", typeof(string));
         gcMain.DataSource = dt;
         gcMain.Focus();

         return ResultStatus.Success;
      }

      //不確定是要check什麼
      protected override ResultStatus CheckShield()
      {
         base.CheckShield(gcMain);
         if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall)
      {

         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();
         ResultStatus resultStatus = ResultStatus.Fail;

         DataTable dt = (DataTable)gcMain.DataSource;
         DataTable dtChange = dt.GetChanges();
         DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
         DataTable dtForModified = dt.GetChanges(DataRowState.Modified);

         if (dtChange != null) {
            if (dtChange.Rows.Count == 0) {
               MessageDisplay.Choose("沒有變更資料,不需要存檔!");
               return ResultStatus.Fail;
            }
            else {
               foreach (DataRow dr in dt.Rows) {
                  if (dr.RowState == DataRowState.Added) {

                     foreach (DataRow drAdd in dtForAdd.Rows) {
                        for (int w = 0; w < dtForAdd.Rows.Count; w++) {
                           if (string.IsNullOrEmpty(drAdd[w].AsString())) {
                              MessageDisplay.Info("新增資料欄位不可為空!");
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

               dt.Columns.Remove("OP_TYPE");
               dt.Columns.Remove("Is_NewRow");
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
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper ReportHelper)
      {
         try {
            ReportHelper reportHelper = new ReportHelper(gcMain, _ProgramID, _ProgramID + _ProgramName);
            reportHelper.Print();

         }
         catch (Exception ex) {
            throw ex;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow()
      {
         int focusIndex = gvMain.GetFocusedDataSourceRowIndex();
         gvMain.CloseEditor();//必須先做close edit, like dt.AcceptChanges();

         //新增一行並做初始值設定
         DataTable dt = (DataTable)gcMain.DataSource;
         DataRow drNew = dt.NewRow();

         drNew["Is_NewRow"] = 1;
         drNew["MMF_W_USER_ID"] = GlobalInfo.USER_ID;
         drNew["MMF_W_TIME"] = DateTime.Now;

         dt.Rows.InsertAt(drNew, focusIndex);
         gcMain.DataSource = dt;//重新設定給grid,雖然會更新但是速度太快,畫面不會閃爍
         gvMain.FocusedRowHandle = focusIndex;//原本的focusRowHandle會記住之前的位置,其實只是往上一行
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow()
      {
         GridView gv = gvMain as GridView;
         DataRowView deleteRowView = (DataRowView)gv.GetFocusedRow();
         base.DeleteRow(gvMain);
         return ResultStatus.Success;
      }

   }
}