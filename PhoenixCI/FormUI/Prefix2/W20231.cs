using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
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
using System.Threading;

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
         Pls4FutLookUpEdit.SetColumnLookUp(futList, "ValueMember", "DisplayMember", textEditStyles, null);
         PLS4_FUT.ColumnEdit = Pls4FutLookUpEdit;
         ////選擇權
         List<LookupItem> optList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = " ", DisplayMember = " "},
                                        new LookupItem() { ValueMember = "O", DisplayMember = "○" }};
         Pls4OptLookUpEdit.SetColumnLookUp(optList, "ValueMember", "DisplayMember", textEditStyles, null);
         PLS4_OPT.ColumnEdit = Pls4OptLookUpEdit;
         ////商品類別
         List<LookupItem> codeList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "I", DisplayMember = "新增"},
                                        new LookupItem() { ValueMember = "M", DisplayMember = "小型"},
                                        new LookupItem() { ValueMember = "N", DisplayMember = " " }};
         Pls4StatusCodeLookUpEdit.SetColumnLookUp(codeList, "ValueMember", "DisplayMember", textEditStyles, null);
         PLS4_STATUS_CODE.ColumnEdit = Pls4StatusCodeLookUpEdit;
         ////上市/上櫃
         Pls4PidLookUpEdit.SetColumnLookUp(new COD().ListByCol("TFXM", "TFXM_PID"), "COD_ID", "COD_DESC", textEditStyles, null);
         PLS4_PID.ColumnEdit = Pls4PidLookUpEdit;
         gcMain.Visible = false;

      }

      private bool WfChkDate()
      {
         //確認：比對日期
         if (!emProdDate.IsDate(emProdDate.Text, "「比對期貨/選擇權商品基準日期」非正確日期格式")) {
            return false;
         }

         string lsYMD;
         //確認：計算日期
         lsYMD = emDate.Text.Replace("/", "");
         DialogResult ChooseResult = MessageDisplay.Choose($"請確認「計算日期 :{emDate.Text}」是否正確?");
         if (ChooseResult == DialogResult.No) {
            return false;
         }

         Retrieve();
         DataTable dt = (DataTable)gcMain.DataSource;

         if (dt == null) {
            return false;
         }

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
         gvMain.FocusedColumn = gvMain.Columns[1];
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
         else if (gv.FocusedColumn.FieldName == "PLS4_SID") {
            e.Cancel = true;
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
         if (e.Column.FieldName == "PLS4_SID" || e.Column.FieldName == "PLS4_KIND_ID2")
            e.Appearance.BackColor = isNewRow == "1" ? Color.White : Color.FromArgb(224, 224, 224);
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
         //存檔前檢查
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
#if DEBUG
         emDate.Text = "2019/03/29";
#endif

         return ResultStatus.Success;
      }

      protected override ResultStatus Import()
      {
         Stream openFile = PbFunc.wf_getfileopenname("20231.txt", "*.txt (*.txt)|*.txt");
         if (openFile == null) {
            return ResultStatus.Fail;
         }

         DataTable dtReadTxt = dao20231.List20231(emDate.Text.Replace("/", "")).Clone();
         DataTable dt = new B20231().TxtWriteToDataTable(openFile, dtReadTxt);
         if (dt.Rows.Count > 0) {
            DateTime dateTime = dt.Rows[0][0].AsDateTime("yyyyMMdd");
            if (dateTime != DateTime.MinValue) {
               emProdDate.Text = dateTime.ToString("yyyy/MM/dd");
            }
         }

         ShowMsg("開始轉檔...");

         bool IsDone = WfChkDate();
         DataTable chkData = dao20231.List20231(emDate.Text.Replace("/", ""));
         if (chkData.Rows.Count <= 0) {
            MessageDisplay.Info("轉入筆數為０!");
            return ResultStatus.Fail;
         }
         //確認
         if (!IsDone) {
            return ResultStatus.Fail;
         }
         if (IsDone) {
            //TODO PB確認階段以後的邏輯有問題 需要等期交所確認真正需求
            return ResultStatus.Success;
         }
         //轉入資料
         ResultData myResultData = dao20231.UpdatePLS4(dt);
         //期貨/選擇權
         gcMain.BeginUpdate();
         string lsymd = emDate.Text.Replace("/", "");
         string lspdkymd = emProdDate.Text.Replace("/", "");
         DataTable dtHPDK = dao20231.ListHpdkData(lspdkymd);
         foreach (DataRow dr in chkData.Rows) {
            dr["PLS4_YMD"] = lsymd;
            dr["PLS4_PDK_YMD"] = lspdkymd;
            dr["PLS4_W_USER_ID"] = GlobalInfo.USER_ID;
            dr["PLS4_W_TIME"] = DateTime.Now;
            int lirtn = dtHPDK.Rows.IndexOf(dtHPDK.Select($"PLS4_KIND_ID2='{dr["PLS4_KIND_ID2"].AsString()}'")[0]);
            if (lirtn > -1) {
               dr["PLS4_FUT"] = dtHPDK.Rows[lirtn]["PLS4_FUT"];
               dr["PLS4_OPT"] = dtHPDK.Rows[lirtn]["PLS4_OPT"];
            }
            else {
               dr["PLS4_FUT"] = "";
               dr["PLS4_OPT"] = "";
            }
         }
         gcMain.DataSource = chkData;
         gcMain.EndUpdate();

         PLS4_SID.AppearanceCell.BackColor = Color.White;
         PLS4_SID.OptionsColumn.AllowEdit = true;
         PLS4_KIND_ID2.AppearanceCell.BackColor = Color.White;

         //存檔 這段也怪怪的 PB 根本就沒有寫入任何路徑在is_save_file這個變數
         string filepath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, "");
         gvMain.ExportToXlsx(filepath);
         //Write LOGF
         WriteLog(_ProgramID, "E", "轉出檔案:" + filepath);
         EndExport();
         return ResultStatus.Success;
      }

      protected void ShowMsg(string msg)
      {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected void EndExport()
      {
         stMsgTxt.Text = "";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      private void gvMain_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
      {

         e.Appearance.DrawBackground(e.Cache, e.Bounds);
         if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Header) {
            e.Appearance.BackColor = Color.FromArgb(192, 220, 192);
            e.Appearance.DrawString(e.Cache, "流水號", e.Bounds);
            e.Appearance.ForeColor = Color.Black;
            //e.Appearance.BorderColor = Color.Black;

            e.Handled = true;
         }
         if (e.Info.IsRowIndicator && e.RowHandle >= 0) {
            e.Appearance.BackColor = Color.FromArgb(192, 220, 192);
            e.Info.DisplayText = (e.RowHandle + 1).ToString();
            e.Appearance.ForeColor = Color.Black;
            //e.Appearance.BorderColor = Color.Black;
         }

      }

      private void btnCopy_Click(object sender, EventArgs e)
      {
         bool IsDone = WfChkDate();
         //確認
         if (!IsDone) {
            return;
         }
         gcMain.BeginUpdate();
         string lsymd = emDate.Text.Replace("/", "");
         string lspdkymd = emProdDate.Text.Replace("/", "");
         DataTable insertData = dao20231.ListHpdkData(lspdkymd);
         DataTable data = dao20231.List20231(lsymd).Clone();//dw_1.reset()
                                                            //InsertData寫入List20231
         data.Columns.Add("Is_NewRow", typeof(string));

         if (insertData.Rows.Count > 0) {
            for (int k = 0; k < insertData.Rows.Count; k++) {
               data.Rows.Add(data.NewRow());
               for (int j = 0; j < insertData.Columns.Count; j++) {
                  data.Rows[k][j] = insertData.Rows[k][j];
                  data.Rows[k]["PLS4_YMD"] = lsymd;
                  data.Rows[k]["PLS4_PDK_YMD"] = lspdkymd;
                  data.Rows[k]["Is_NewRow"] = 1;//複製的資料全部視為新增的row
               }
            }
         }
         gcMain.DataSource = data;
         gcMain.EndUpdate();
      }

      private void btnAdd_Click(object sender, EventArgs e)
      {
         FormMain frmMain = (FormMain)this.MdiParent;
         frmMain.OpenForm($"{_ProgramID}_2", _ProgramName);
      }

      private void gvMain_RowClick(object sender, RowClickEventArgs e)
      {
         if (e.RowHandle < 0 || gvMain.Columns.Count == 0) {
            return;
         }
         gvMain.FocusedColumn = gvMain.Columns["PLS4_KIND_ID2"];
         string Is_NewRow = gvMain.GetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["Is_NewRow"]) == null ? "0" :
             gvMain.GetRowCellValue(gvMain.FocusedRowHandle, gvMain.Columns["Is_NewRow"]).ToString();
         if (Is_NewRow == "1") {
            gvMain.FocusedColumn = gvMain.Columns["PLS4_SID"];
         }
         gvMain.FocusedRowHandle = e.RowHandle;
         gvMain.ShowEditor();
      }

      private void gvMain_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
      {
         GridView gv = sender as GridView;
         string isNewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
              gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();
         switch (e.Column.FieldName) {
            case "PLS4_SID":
               if (isNewRow == "1") {
                  e.RepositoryItem = SIDdefTextEdit;
               }
               else {
                  e.RepositoryItem = SIDTextEdit;
               }
               break;
            case "PLS4_KIND_ID2":
               if (isNewRow == "1") {
                  e.RepositoryItem = kindIDdefTextEdit;
               }
               else {
                  e.RepositoryItem = KindIDTextEdit;
               }
               break;
         }
      }
   }
}