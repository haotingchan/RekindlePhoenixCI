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
using DevExpress.XtraEditors.Repository;
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using DevExpress.XtraGrid.Views.Grid;
using BaseGround.Shared;
using BaseGround.Report;
using BusinessObjects;
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Controls;
using System.IO;
using PhoenixCI.BusinessLogic.Prefix2;

/// <summary>
/// John, 2019/5/6
/// </summary>
namespace PhoenixCI.FormUI.Prefix2
{
   /// <summary>
   /// 20231 部位限制個股類標的轉入
   /// </summary>
   public partial class W20231 : FormParent
   {
      #region 全域變數
      private D20231 dao20231;
      /// <summary>
      /// 期貨
      /// </summary>
      private RepositoryItemLookUpEdit Pls4FutLookUpEdit;
      /// <summary>
      /// 選擇權
      /// </summary>
      private RepositoryItemLookUpEdit Pls4OptLookUpEdit;
      /// <summary>
      /// 商品狀態
      /// </summary>
      private RepositoryItemLookUpEdit Pls4StatusCodeLookUpEdit;
      /// <summary>
      /// 上市/上櫃
      /// </summary>
      private RepositoryItemLookUpEdit Pls4PidLookUpEdit;

      private string _cpYMD;

      private string _IsPdkYMD;
      #endregion

      public W20231(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         GridHelper.SetCommonGrid(gvMain);
         PrintableComponent = gcMain;

         dao20231 = new D20231();
         TextEditStyles textEditStyles = TextEditStyles.DisableTextEditor;
         //期貨
         List<LookupItem> futList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = " ", DisplayMember = " "},
                                        new LookupItem() { ValueMember = "F", DisplayMember = "○" }};
         Pls4FutLookUpEdit = new RepositoryItemLookUpEdit();
         Pls4FutLookUpEdit.SetColumnLookUp(futList, "ValueMember", "DisplayMember", textEditStyles, null);
         PLS4_FUT.ColumnEdit = Pls4FutLookUpEdit;
         //選擇權
         List<LookupItem> optList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = " ", DisplayMember = " "},
                                        new LookupItem() { ValueMember = "O", DisplayMember = "○" }};
         Pls4OptLookUpEdit = new RepositoryItemLookUpEdit();
         Pls4OptLookUpEdit.SetColumnLookUp(optList, "ValueMember", "DisplayMember", textEditStyles, null);
         PLS4_OPT.ColumnEdit = Pls4OptLookUpEdit;
         //商品類別
         List<LookupItem> codeList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "I", DisplayMember = "新增"},
                                        new LookupItem() { ValueMember = "M", DisplayMember = "小型"},
                                        new LookupItem() { ValueMember = "N", DisplayMember = " " }};
         Pls4StatusCodeLookUpEdit = new RepositoryItemLookUpEdit();
         Pls4StatusCodeLookUpEdit.SetColumnLookUp(codeList, "ValueMember", "DisplayMember", textEditStyles, null);
         PLS4_STATUS_CODE.ColumnEdit = Pls4StatusCodeLookUpEdit;
         //上市/上櫃
         Pls4PidLookUpEdit = new RepositoryItemLookUpEdit();
         Pls4PidLookUpEdit.SetColumnLookUp(new COD().ListByCol("TFXM", "TFXM_PID"), "COD_ID", "COD_DESC", textEditStyles, null);
         PLS4_PID.ColumnEdit = Pls4PidLookUpEdit;
         gcMain.Visible = false;

      }

      private bool WfChkDate()
      {
         string lsYMD;
         //確認：計算日期
         lsYMD = emDate.Text.Replace("/", "");
         DialogResult ChooseResult = MessageDisplay.Choose($"請確認「計算日期 :{emDate.Text}」是否正確?");
         if (ChooseResult == DialogResult.No) {
            return false;
         }
         Retrieve();
         DataTable dt = (DataTable)gcMain.DataSource;
         if (dt.Rows.Count > 0) {
            DialogResult ChooseResult1 = MessageDisplay.Choose($"「計算日期 :{emDate.Text}」資料已存在,是否刪除?");
            if (ChooseResult1 == DialogResult.No) {
               return false;
            }
            DialogResult ChooseResult2 = MessageBox.Show($"「計算日期 :{emDate.Text}」資料確定刪除?", "注意", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (ChooseResult2 == DialogResult.Cancel) {
               return false;
            }

            dao20231.DeletePLS4(lsYMD);

         }
         //確認：比對日期
         if (!emProdDate.IsDate(emProdDate.Text, "「比對期貨/選擇權商品基準日期」非正確日期格式")) {
            return false;
         }
         DialogResult ChooseResult3 = MessageBox.Show($"請確認「比對期貨/選擇權商品基準日期 :{emDate.Text}」是否正確?", "注意", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
         if (ChooseResult3 == DialogResult.Cancel) {
            return false;
         }
         return true;
      }

      protected override ResultStatus Retrieve()
      {
         DataTable returnTable = new DataTable();
         _cpYMD = emDate.Text.Replace("/", "");
         returnTable = dao20231.List20231(_cpYMD);

         if (returnTable.Rows.Count <= 0) {
            MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
            return ResultStatus.Success;
         }
         if (!string.IsNullOrEmpty(returnTable.Rows[0]["PLS4_PDK_YMD"].AsString())) {
            _IsPdkYMD = returnTable.Rows[0]["PLS4_PDK_YMD"].AsString();
            emProdDate.Text = _IsPdkYMD.AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
         }
         _ToolBtnInsert.Enabled = true;
         _ToolBtnSave.Enabled = true;
         _ToolBtnDel.Enabled = true;

         gcMain.Visible = true;

         base.Retrieve(gcMain);
         //流水號欄寬
         gvMain.IndicatorWidth = 60;

         returnTable.Columns.Add("Is_NewRow", typeof(string));
         gcMain.DataSource = returnTable;

         gcMain.Focus();
         this.gvMain.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvMain_CustomDrawRowIndicator);
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
         if (string.IsNullOrEmpty(_cpYMD)) {
            _cpYMD = emDate.Text.Replace("/", "");
         }
         drNew["PLS4_YMD"] = _cpYMD;
         drNew["PLS4_KIND_ID2"] = "";
         drNew["PLS4_FUT"] = "";
         drNew["PLS4_OPT"] = "";
         drNew["PLS4_PID"] = "";

         dt.Rows.InsertAt(drNew, focusIndex);
         gcMain.DataSource = dt;//重新設定給grid,雖然會更新但是速度太快,畫面不會閃爍
         gvMain.FocusedRowHandle = focusIndex;//原本的focusRowHandle會記住之前的位置,其實只是往上一行
         gvMain.FocusedColumn = gvMain.Columns[0];
         return ResultStatus.Success;
      }

      private void gvMain_ShowingEditor(object sender, CancelEventArgs e)
      {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
             gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
         }
         else {
            e.Cancel = false;
         }
      }

      private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e)
      {
         GridView gv = sender as GridView;
         string isNewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
              gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();

      }

      private void SetFocused(DataTable dt, DataRow dr, string colName)
      {
         gvMain.FocusedRowHandle = dt.Rows.IndexOf(dr);
         gvMain.FocusedColumn = gvMain.Columns[colName];
         gvMain.ShowEditor();
      }

      protected override ResultStatus Save(PokeBall poke)
      {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         DataTable dt = (DataTable)gcMain.DataSource;
         DataTable dtChange = dt.GetChanges();
         DataTable dtDeleteChange = dt.GetChanges(DataRowState.Deleted);
         DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
         DataTable dtForModified = dt.GetChanges(DataRowState.Modified);

         int getDeleteCount = dtDeleteChange != null ? dtDeleteChange.Rows.Count : 0;
         ////存檔前檢查
         if (getDeleteCount == 0 && dtChange != null)//無法經由資料列存取已刪除的資料列資訊。
         {
            // 寫入DB
            foreach (DataRow dr in dt.Rows) {
               if (dr.RowState == DataRowState.Modified) {
                  dr["PLS4_W_USER_ID"] = GlobalInfo.USER_ID;
                  dr["PLS4_W_TIME"] = DateTime.Now;
                  dr["PLS4_PDK_YMD"] = _IsPdkYMD;

               }
            }
         }
         if (dtChange != null) {
            try {
               ResultData myResultData = dao20231.UpdatePLS4(dt);
               Common.Helper.ExportHelper.ToText(dt, Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, "20231.txt"));
            }
            catch (Exception ex) {
               WriteLog(ex);
            }
            //PrintOrExportChangedByKen(gcMain, dtForAdd, dtDeleteChange, dtForModified);
            return ResultStatus.Success;
         }
         else {
            MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
         }
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

      protected override ResultStatus DeleteRow()
      {
         base.DeleteRow(gvMain);
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnImport.Enabled = true;

         return ResultStatus.Success;
      }

      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         emDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         //直接讀取資料
         //Retrieve();
         //Header上色
         //CustomDrawColumnHeader(gcMain,gvMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Import()
      {
         Stream openFile = PbFunc.wf_getfileopenname("20231.txt", "*.txt (*.txt)|*.txt");
         DataTable dtReadTxt = dao20231.List20231(emDate.Text.Replace("/", "")).Clone();
         DataTable dt = new B20231().TxtWriteToDataTable(openFile, dtReadTxt);
         if (dt.Rows.Count > 0) {
            DateTime dateTime = dt.Rows[0][0].AsDateTime("yyyyMMdd");
            if (dateTime != DateTime.MinValue) {
               emProdDate.Text = dateTime.ToString("yyyy/MM/dd");
            }
         }

         bool IsDone = WfChkDate();
         //確認
         if (!IsDone) {
            return ResultStatus.Fail;
         }
         if (IsDone) {
            //TODO PB確認階段以後的邏輯有問題 須等期交所確認真正需求
            return ResultStatus.Success;
         }
         //轉入資料
         ResultData myResultData = dao20231.UpdatePLS4(dt);
         return ResultStatus.Success;
      }

      private void gvMain_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
      {

         e.Appearance.DrawBackground(e.Cache, e.Bounds);
         if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Header) {
            e.Appearance.BackColor = Color.FromArgb(192, 220, 192);
            e.Appearance.DrawString(e.Cache, "流水號", e.Bounds);
            e.Appearance.ForeColor = Color.Black;

            e.Handled = true;
         }
         if (e.Info.IsRowIndicator && e.RowHandle >= 0) {
            e.Appearance.BackColor = Color.FromArgb(192, 220, 192);
            e.Info.DisplayText = (e.RowHandle + 1).ToString();
            e.Appearance.ForeColor = Color.Black;
         }

      }

      private void btnCopy_Click(object sender, EventArgs e)
      {

      }

      private void btnAdd_Click(object sender, EventArgs e)
      {

      }
   }
}