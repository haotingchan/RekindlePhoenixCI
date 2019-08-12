using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using PhoenixCI.BusinessLogic.Prefix2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// John, 2019/5/6
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {
   /// <summary>
   /// 20231 部位限制個股類標的轉入
   /// </summary>
   public partial class W20231 : FormParent {
      #region 全域變數
      private D20231 dao20231;
      /// <summary>
      /// 計算日期
      /// </summary>
      private string _cpYMD;
      /// <summary>
      /// 比對期貨/選擇權商品基準日期
      /// </summary>
      private string _IsPdkYMD;
      #endregion

      public W20231(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         GridHelper.SetCommonGrid(gvMain);
         PrintableComponent = gcMain;

         dao20231 = new D20231();
         //統一設定下拉選單TextEditStyles
         TextEditStyles textEditStyles = TextEditStyles.DisableTextEditor;
         //期貨
         //List<LookupItem> futList = new List<LookupItem>(){
         //                               new LookupItem() { ValueMember = " ", DisplayMember = " "},
         //                               new LookupItem() { ValueMember = "F", DisplayMember = "○" }};

         DataTable dtFut = new CODW().ListLookUpEdit("20231" , "PLS4_FUT");
         Pls4FutLookUpEdit.SetColumnLookUp(dtFut , "CODW_ID" , "CODW_DESC" , textEditStyles , null);
         PLS4_FUT.ColumnEdit = Pls4FutLookUpEdit;
         //選擇權
         //List<LookupItem> optList = new List<LookupItem>(){
         //                               new LookupItem() { ValueMember = " ", DisplayMember = " "},
         //                               new LookupItem() { ValueMember = "O", DisplayMember = "○" }};

         DataTable dtOpt = new CODW().ListLookUpEdit("20231" , "PLS4_OPT");
         Pls4OptLookUpEdit.SetColumnLookUp(dtOpt , "CODW_ID" , "CODW_DESC" , textEditStyles , null);
         PLS4_OPT.ColumnEdit = Pls4OptLookUpEdit;
         //商品類別
         //List<LookupItem> codeList = new List<LookupItem>(){
         //                               new LookupItem() { ValueMember = "I", DisplayMember = "新增"},
         //                               new LookupItem() { ValueMember = "M", DisplayMember = "小型"},
         //                               new LookupItem() { ValueMember = "N", DisplayMember = " " }};

         DataTable dtCode = new CODW().ListLookUpEdit("20231" , "PLS4_STATUS_CODE");
         Pls4StatusCodeLookUpEdit.SetColumnLookUp(dtCode , "CODW_ID" , "CODW_DESC" , textEditStyles , null);
         PLS4_STATUS_CODE.ColumnEdit = Pls4StatusCodeLookUpEdit;

         //上市/上櫃
         //Pls4PidLookUpEdit.SetColumnLookUp(new COD().ListByCol("TFXM", "TFXM_PID"), "COD_ID", "COD_DESC", textEditStyles, null);
         DataTable dtTFXM = new CODW().ListLookUpEdit("APDK" , "UNDERLYING_MARKET");
         Pls4PidLookUpEdit.SetColumnLookUp(dtTFXM , "CODW_ID" , "CODW_DESC" , textEditStyles , null);
         PLS4_PID.ColumnEdit = Pls4PidLookUpEdit;

         //預設隱藏DataGridView
         gcMain.Visible = false;

      }

      /// <summary>
      /// 確認日期 有資料時清除PLS4相關日期資料
      /// </summary>
      /// <returns></returns>
      private string WfChkDate() {

         string lsYMD;
         //確認：計算日期
         lsYMD = emDate.Text.Replace("/" , "");
         DialogResult ChooseResult = MessageDisplay.Choose($"請確認「計算日期 :{emDate.Text}」是否正確?");
         if (ChooseResult == DialogResult.No) {
            return "E";
         }

         //刪除舊有資料
         DataTable data = dao20231.List20231(lsYMD);
         if (data.Rows.Count > 0) {
            DialogResult ChooseResult1 = MessageDisplay.Choose($"「計算日期 :{emDate.Text}」資料已存在,是否刪除?");
            if (ChooseResult1 == DialogResult.No) {
               return "E";
            }

            DialogResult ChooseResult2 = MessageBox.Show($"「計算日期 :{emDate.Text}」資料確定刪除?" , "注意" , MessageBoxButtons.OKCancel , MessageBoxIcon.Information);
            if (ChooseResult2 == DialogResult.Cancel) {
               return "E";
            }

            //刪除相關日期條件已存在的資料
            dao20231.DeletePLS4(lsYMD);
         }

         //確認：比對日期
         if (!emProdDate.IsDate(emProdDate.Text , "「比對期貨/選擇權商品基準日期」非正確日期格式")) {
            return "E";
         }

         DialogResult ChooseResult3 = MessageBox.Show($"請確認「比對期貨/選擇權商品基準日期 :{emDate.Text}」是否正確?" , "注意" , MessageBoxButtons.OKCancel , MessageBoxIcon.Information);
         if (ChooseResult3 == DialogResult.Cancel) {
            return "E";
         }

         return "";
      }

      protected override ResultStatus Retrieve() {
         gcMain.DataSource = null;
         DataTable returnTable = new DataTable();
         _cpYMD = emDate.Text.Replace("/" , "");
         returnTable = dao20231.List20231(_cpYMD);

         if (returnTable.Rows.Count <= 0) {
            gcMain.Visible = false;
            MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
            return ResultStatus.FailButNext;
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

         returnTable.Columns.Add("Is_NewRow" , typeof(string));
         gcMain.DataSource = returnTable;

         gcMain.Focus();
         this.gvMain.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvMain_CustomDrawRowIndicator);
         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         int focusIndex = gvMain.GetFocusedDataSourceRowIndex();
         gvMain.CloseEditor();//必須先做close edit, like dt.AcceptChanges();

         if (gcMain.DataSource == null) {
            DataTable dtEmpty = dao20231.List20231(emDate.Text.Replace("/" , "")).Clone();
            dtEmpty.Columns.Add("Is_NewRow" , typeof(string));
            gcMain.DataSource = dtEmpty;
            focusIndex = 0;
            gcMain.Visible = true;
         }

         //新增一行並做初始值設定
         DataTable dt = (DataTable)gcMain.DataSource;
         DataRow drNew = dt.NewRow();

         drNew["Is_NewRow"] = 1;
         if (string.IsNullOrEmpty(_cpYMD)) {
            _cpYMD = emDate.Text.Replace("/" , "");
         }
         drNew["PLS4_YMD"] = _cpYMD;
         drNew["PLS4_KIND_ID2"] = "";
         drNew["PLS4_FUT"] = " ";
         drNew["PLS4_OPT"] = " ";
         drNew["PLS4_STATUS_CODE"] = "N";
         drNew["PLS4_PID"] = "";

         dt.Rows.InsertAt(drNew , focusIndex);
         gcMain.DataSource = dt;//重新設定給grid,雖然會更新但是速度太快,畫面不會閃爍
         gvMain.FocusedRowHandle = focusIndex;//原本的focusRowHandle會記住之前的位置,其實只是往上一行
         gvMain.FocusedColumn = gvMain.Columns[1];

         return ResultStatus.Success;
      }

      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
             gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]).ToString();

         //判斷哪些欄位可以編輯
         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false;
            gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"] , 1);
         } else if (gv.FocusedColumn.FieldName == "PLS4_SID") {
            e.Cancel = true;
         } else {
            e.Cancel = false;
         }
      }

      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         GridView gv = sender as GridView;
         string isNewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
              gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]).ToString();
         //新增時顏色轉為白底
         if (e.Column.FieldName == "PLS4_SID" || e.Column.FieldName == "PLS4_KIND_ID2")
            e.Appearance.BackColor = isNewRow == "1" ? Color.White : Color.FromArgb(224 , 224 , 224);
      }

      private void SetFocused(DataTable dt , DataRow dr , string colName) {
         gvMain.FocusedRowHandle = dt.Rows.IndexOf(dr);
         gvMain.FocusedColumn = gvMain.Columns[colName];
         gvMain.ShowEditor();
      }

      protected override ResultStatus Save(PokeBall poke) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         DataTable dt = (DataTable)gcMain.DataSource;
         DataTable dtChange = dt.GetChanges();
         DataTable dtAdd = dt.GetChanges(DataRowState.Added);
         DataTable dtDeleteChange = dt.GetChanges(DataRowState.Deleted);

         if (dtChange == null) {
            MessageDisplay.Warning("沒有變更資料,不需要存檔!" , GlobalInfo.WarningText);
            return ResultStatus.Fail;
         }
         if (dtChange.Rows.Count == 0) {
            MessageDisplay.Warning("沒有變更資料,不需要存檔!" , GlobalInfo.WarningText);
            return ResultStatus.Fail;
         }

         if (dtChange != null) {
            //更新變動日期與使用者ID
            foreach (DataRow dr in dt.Rows) {
               if (dr.RowState == DataRowState.Modified) {
                  dr["PLS4_W_USER_ID"] = GlobalInfo.USER_ID;
                  dr["PLS4_W_TIME"] = DateTime.Now;
                  dr["PLS4_PDK_YMD"] = _IsPdkYMD;
               }

               if (dr.RowState == DataRowState.Added) {
                  dr["PLS4_W_USER_ID"] = GlobalInfo.USER_ID;
                  dr["PLS4_W_TIME"] = DateTime.Now;
                  dr["PLS4_PDK_YMD"] = _IsPdkYMD;
                  if (string.IsNullOrEmpty(dr["PLS4_SID"].AsString()) || string.IsNullOrEmpty(dr["PLS4_KIND_ID2"].AsString()) ||
                        string.IsNullOrEmpty(dr["PLS4_PID"].AsString())) {
                     MessageDisplay.Info("資料尚未填寫完成!");
                     return ResultStatus.FailButNext;
                  }
               }

               if (dr.RowState == DataRowState.Deleted) continue;
            }

            try {
               // 寫入DB
               ResultData myResultData = dao20231.UpdatePLS4(dt);
               //產出txt檔案
               Common.Helper.ExportHelper.ToText(dt , Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , "20231.txt"));
            } catch (Exception ex) {
               WriteLog(ex);
               return ResultStatus.Fail;
            }
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper ReportHelper) {
         try {
            ReportHelper reportHelper = new ReportHelper(gcMain , _ProgramID , _ProgramID + _ProgramName);
            reportHelper.Print();//列印

         } catch (Exception ex) {
            throw ex;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnImport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Open() {
         base.Open();
         emDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
#if DEBUG
         emDate.Text = "2019/03/29";
#endif

         return ResultStatus.Success;
      }

      /// <summary>
      /// 這功能user沒什麼在使用 用pb測試也發現這功能怪怪的 所以邏輯就照pb翻
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Import() {
         Stream openFile = PbFunc.wf_getfileopenname("20231.txt" , "*.txt (*.txt)|*.txt");
         if (openFile == null) {
            return ResultStatus.Fail;
         }

         DataTable dtReadTxt = dao20231.List20231(emDate.Text.Replace("/" , "")).Clone();
         DataTable dt = new B20231().TxtWriteToDataTable(openFile , dtReadTxt);
         if (dt.Rows.Count > 0) {
            DateTime dateTime = dt.Rows[0][0].AsDateTime("yyyyMMdd");
            if (dateTime != DateTime.MinValue) {
               emProdDate.Text = dateTime.ToString("yyyy/MM/dd");
            }
         }

         ShowMsg("開始轉檔...");

         string IsDone = "";
         IsDone = WfChkDate();
         DataTable chkData = dao20231.List20231(emDate.Text.Replace("/" , ""));
         if (chkData.Rows.Count <= 0) {
            MessageDisplay.Info("轉入筆數為０!");
            return ResultStatus.Fail;
         }
         //確認
         if (!string.IsNullOrEmpty(IsDone)) {
            return ResultStatus.Fail;
         }
         //if (IsDone) {
         //   //TODO PB確認階段以後的邏輯有問題 需要等期交所確認真正需求
         //   return ResultStatus.Success;
         //}
         //轉入資料
         ResultData myResultData = dao20231.UpdatePLS4(dt);
         //期貨/選擇權
         gcMain.BeginUpdate();
         string lsymd = emDate.Text.Replace("/" , "");
         string lspdkymd = emProdDate.Text.Replace("/" , "");
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
            } else {
               dr["PLS4_FUT"] = "";
               dr["PLS4_OPT"] = "";
            }
         }
         gcMain.DataSource = chkData;
         gcMain.EndUpdate();

         //匯入之後股票代號和個股商品2碼為可編輯狀態
         PLS4_SID.AppearanceCell.BackColor = Color.White;
         PLS4_SID.OptionsColumn.AllowEdit = true;
         PLS4_KIND_ID2.AppearanceCell.BackColor = Color.White;

         //存檔 這段也怪怪的 PB 根本就沒有寫入任何路徑在is_save_file這個變數
         string filepath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , "");
         if (string.IsNullOrEmpty(filepath)) {
            return ResultStatus.Fail;
         }
         gvMain.ExportToXlsx(filepath);
         //Write LOGF
         WriteLog("轉出檔案:" + filepath , "E");
         EndExport();
         return ResultStatus.Success;
      }

      /// <summary>
      /// show出訊息在label
      /// </summary>
      /// <param name="msg"></param>
      protected void ShowMsg(string msg) {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      /// <summary>
      /// 轉檔後清除文字訊息
      /// </summary>
      protected void EndExport() {
         stMsgTxt.Text = "";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      private void gvMain_CustomDrawRowIndicator(object sender , RowIndicatorCustomDrawEventArgs e) {
         //編排流水號
         e.Appearance.DrawBackground(e.Cache , e.Bounds);
         if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Header) {
            e.Appearance.BackColor = Color.FromArgb(192 , 220 , 192);
            e.Appearance.DrawString(e.Cache , "流水號" , e.Bounds);
            e.Appearance.ForeColor = Color.Black;
            //e.Appearance.BorderColor = Color.Black;

            e.Handled = true;
         }
         if (e.Info.IsRowIndicator && e.RowHandle >= 0) {
            e.Appearance.BackColor = Color.FromArgb(192 , 220 , 192);
            e.Info.DisplayText = (e.RowHandle + 1).ToString();
            e.Appearance.ForeColor = Color.Black;
            //e.Appearance.BorderColor = Color.Black;
         }

      }

      private void btnCopy_Click(object sender , EventArgs e) {
         string IsDone = "";
         IsDone = WfChkDate();
         //確認
         if (!string.IsNullOrEmpty(IsDone)) {
            return;
         }
         gcMain.BeginUpdate();
         string lsymd = emDate.Text.Replace("/" , "");
         string lspdkymd = emProdDate.Text.Replace("/" , "");
         DataTable insertData = dao20231.ListHpdkData(lspdkymd);
         DataTable data = dao20231.List20231(lsymd).Clone();//dw_1.reset(); InsertData寫入List20231

         //判斷新增行列
         data.Columns.Add("Is_NewRow" , typeof(string));

         if (insertData.Rows.Count > 0) {
            for (int k = 0 ; k < insertData.Rows.Count ; k++) {
               data.Rows.Add(data.NewRow());
               for (int j = 0 ; j < insertData.Columns.Count ; j++) {
                  data.Rows[k][j] = insertData.Rows[k][j];
                  data.Rows[k]["PLS4_YMD"] = lsymd;
                  data.Rows[k]["PLS4_PDK_YMD"] = lspdkymd;
                  data.Rows[k]["Is_NewRow"] = 1;//複製的資料全部視為新增的row
               }
            }
         }
         gcMain.DataSource = data;
         gcMain.Visible = true;
         gcMain.EndUpdate();
      }

      /// <summary>
      /// 開啟新增個股契約視窗
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnAdd_Click(object sender , EventArgs e) {
         FormMain frmMain = (FormMain)this.MdiParent;
         frmMain.OpenForm($"{_ProgramID}_2" , _ProgramName);
      }

      private void gvMain_RowClick(object sender , RowClickEventArgs e) {
         if (e.RowHandle < 0 || gvMain.Columns.Count == 0) {
            return;
         }
         //點擊資料列 預設是focuse到個股商品2碼 如為可編輯狀態則focuse到股票代號
         gvMain.FocusedColumn = gvMain.Columns["PLS4_KIND_ID2"];//個股商品2碼
         string Is_NewRow = gvMain.GetRowCellValue(gvMain.FocusedRowHandle , gvMain.Columns["Is_NewRow"]) == null ? "0" :
             gvMain.GetRowCellValue(gvMain.FocusedRowHandle , gvMain.Columns["Is_NewRow"]).ToString();
         if (Is_NewRow == "1") {
            gvMain.FocusedColumn = gvMain.Columns["PLS4_SID"];//股票代號
         }
         gvMain.FocusedRowHandle = e.RowHandle;
         gvMain.ShowEditor();
      }

      private void gvMain_CustomRowCellEditForEditing(object sender , CustomRowCellEditEventArgs e) {
         GridView gv = sender as GridView;
         string isNewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
              gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]).ToString();
         //isNewRow=1是新增、新增代表可編輯 其餘皆不可編輯
         switch (e.Column.FieldName) {
            case "PLS4_SID"://股票代號
               if (isNewRow == "1") {
                  e.RepositoryItem = SIDdefTextEdit;//白底、可編輯
               } else {
                  e.RepositoryItem = SIDTextEdit;//灰底、不可編輯
               }
               break;
            case "PLS4_KIND_ID2"://個股商品2碼
               if (isNewRow == "1") {
                  e.RepositoryItem = kindIDdefTextEdit;//白底、可編輯
               } else {
                  e.RepositoryItem = KindIDTextEdit;//灰底、不可編輯
               }
               break;
         }
      }
   }
}