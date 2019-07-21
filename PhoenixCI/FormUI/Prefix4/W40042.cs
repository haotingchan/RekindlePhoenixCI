using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix4;
using System.IO;
using Common;
using DevExpress.XtraGrid.Views.Grid;
using System.Data;
using System.Drawing;
using DevExpress.XtraEditors.Repository;
using DataObjects.Dao.Together;
/// <summary>
/// john,20190422,收盤前保證金試算表
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// john,20190422,收盤前保證金試算表 
   /// </summary>
   public partial class W40042 : FormParent {
      private B40042 b40042;
      /// <summary>
      /// 空元件(替代非TextEditor)
      /// </summary>
      private RepositoryItemButtonEdit emptyEditor;
      /// <summary>
      /// 時段選單
      /// </summary>
      private RepositoryItemLookUpEdit OSW_GRP_LookUpEdit;

      public W40042(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Retrieve() {
         try {
            gcMain.DataSource = b40042.GetDataList();
         } catch (Exception ex) {
            WriteLog(ex);
         }

         return ResultStatus.Success;
      }

      protected override ResultStatus Open() {
         base.Open();

         OSW_GRP_LookUpEdit = new RepositoryItemLookUpEdit();
         OSW_GRP_LookUpEdit.SetColumnLookUp(new OCFG().ListAll() , "OSW_GRP" , "OSW_GRP_NAME" , DevExpress.XtraEditors.Controls.TextEditStyles.Standard , null);
         OSW_GRP.ColumnEdit = OSW_GRP_LookUpEdit;
         CreateEmptyItem();
         b40042 = new B40042();
         Retrieve();
         //抓取交易日期
         string emdate = gvMain.GetRowCellValue(0 , gvMain.Columns["DT_DATE"]).AsDateTime().ToString("yyyy/MM/dd");
         //交易日期
         DateLabel.Text = emdate;
         //狀態顯示
         StatusLabel.Text = b40042.Status(emdate);
#if DEBUG
         //DateLabel.Text = "2018/10/12";
         TestBtn.Visible = true;
#endif
         if (!FlagAdmin) {
            TestBtn.Visible = false;
         } else {
            TestBtn.Visible = true;
         }

         return ResultStatus.Success;
      }

      /// <summary>
      /// 創建RepositoryItem空元件 取代CheckBox欄位時使用
      /// </summary>
      private void CreateEmptyItem() {
         emptyEditor = new RepositoryItemButtonEdit();
         emptyEditor.Buttons.Clear();
         emptyEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
         emptyEditor.Appearance.BackColor = Color.FromArgb(192 , 220 , 192);
         emptyEditor.ReadOnly = true;
         gcMain.RepositoryItems.Add(emptyEditor);
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;
         return ResultStatus.Success;
      }

      /// <summary>
      /// 轉檔前檢查
      /// </summary>
      /// <returns></returns>
      private bool StartExport() {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);

         DataTable dt = (DataTable)gcMain.DataSource;
         MessageDisplay message = new MessageDisplay();
#if DEBUG
         message.OutputShowMessage = b40042.ExportBeforeCheck(dt, GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH);
#else
         message.OutputShowMessage = b40042.ExportBeforeCheck(dt);
#endif

         if (string.IsNullOrEmpty(message.OutputShowMessage)) {
            return false;
         }

         return true;
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

      /// <summary>
      /// show出訊息在label
      /// </summary>
      /// <param name="msg"></param>
      protected void ShowMsg(string msg) {
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      private bool OutputChooseMessage(string str) {
         DialogResult ChooseResult = MessageDisplay.Choose(str);
         if (ChooseResult == DialogResult.No) {
            EndExport();
            return false;
         }
         return true;
      }

      protected override ResultStatus Export() {

         string saveFilePath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);
         try {
            if (!StartExport()) {
               return ResultStatus.Fail;
            }
            b40042 = new B40042(saveFilePath , DateLabel.Text , GlobalInfo.USER_ID);
            MessageDisplay message = new MessageDisplay();

            ShowMsg($"{_ProgramID}_mg1－股票期貨保證金狀況表－標的證券為受益憑證 轉檔中...");
            message.OutputShowMessage = b40042.Wf40042();
            ShowMsg($"{_ProgramID}_40011_1－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b40042.Wf40011Fut();
            ShowMsg($"{_ProgramID}_40011_2－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b40042.Wf40011Opt();
            ShowMsg($"{_ProgramID}_40012_1－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b40042.Wf40012Fut();
            ShowMsg($"{_ProgramID}_40012_2－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b40042.Wf40012Opt();
            ShowMsg($"{_ProgramID}_40013_1－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b40042.Wf40013Fut();
         } catch (Exception ex) {
            if (File.Exists(saveFilePath))
               File.Delete(saveFilePath);
            WriteLog(ex);
            return ResultStatus.Fail;
         } finally {
            EndExport();
         }

         return ResultStatus.Success;
      }

      /// <summary>
      /// 觸發時判斷該cell可否編輯
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_ShowingEditor(object sender , System.ComponentModel.CancelEventArgs e) {
         GridView view = (GridView)sender;
         switch (view.FocusedColumn.FieldName) {
            case "AI5_SETTLE_PRICE":
            case "F_CHOOSE":
               var fkindid = view.GetRowCellValue(view.FocusedRowHandle , view.Columns["F_KIND_ID"]);
               if (!string.IsNullOrEmpty(fkindid.AsString())) {
                  e.Cancel = false;
               } else {
                  e.Cancel = true;
               }
               break;
            case "TFXM1_SFD_FPR":
               var flag = view.GetRowCellValue(view.FocusedRowHandle , view.Columns["SFD_UPD_FLAG"]).AsString();
               if (flag == "Y") {
                  e.Cancel = false;
               } else {
                  e.Cancel = true;
               }
               break;
            case "O_CHOOSE":
            case "O_KIND_ID":
            case "O_PDK_NAME":
               var okindid = view.GetRowCellValue(view.FocusedRowHandle , view.Columns["O_KIND_ID"]);
               if (!string.IsNullOrEmpty(okindid.AsString())) {
                  e.Cancel = false;
               } else {
                  e.Cancel = true;
               }
               break;
         }
      }

      /// <summary>
      /// 根據條件更換RepositoryItem
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_CustomRowCellEdit(object sender , CustomRowCellEditEventArgs e) {
         GridView view = (GridView)sender;
         int index = e.RowHandle;
         var FutKindid = view.GetRowCellValue(index , view.Columns["F_KIND_ID"]);
         var OptKindid = view.GetRowCellValue(index , view.Columns["O_KIND_ID"]);
         switch (e.Column.FieldName) {
            case "AI5_SETTLE_PRICE":
               if (!string.IsNullOrEmpty(FutKindid.AsString())) {
                  e.RepositoryItem = repositorySettlePriceTextEdit;
               } else {
                  e.RepositoryItem = null;
               }
               break;
            case "TFXM1_SFD_FPR":
               string flag = view.GetRowCellValue(index , view.Columns["SFD_UPD_FLAG"]).AsString();
               if (flag == "Y") {
                  e.RepositoryItem = repositorySfdPriceTextEdit;
               } else {
                  e.RepositoryItem = null;
               }
               break;
            case "F_CHOOSE":
               if (!string.IsNullOrEmpty(FutKindid.AsString())) {
                  e.RepositoryItem = repositoryFutCheckEdit1;
               } else {
                  e.RepositoryItem = emptyEditor;
               }
               break;
            case "O_CHOOSE":
               if (!string.IsNullOrEmpty(OptKindid.AsString())) {
                  e.RepositoryItem = repositoryOptCheckEdit1;
               } else {
                  e.RepositoryItem = emptyEditor;
               }
               break;
         }
      }

      /// <summary>
      /// //可編輯時背景為白 不可編輯為綠
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         GridView view = (GridView)sender;
         int index = e.RowHandle;
         Color colorMint = Color.FromArgb(192 , 220 , 192);//綠背景色

         switch (e.Column.FieldName) {
            case "AI5_SETTLE_PRICE"://近月份期貨價格
               var fkindid = view.GetRowCellValue(index , view.Columns["F_KIND_ID"]);//契約代碼
               if (!string.IsNullOrEmpty(fkindid.AsString())) {
                  e.Appearance.BackColor = Color.White;
               } else {
                  e.Appearance.BackColor = colorMint;
               }
               break;
            case "TFXM1_SFD_FPR"://標的現貨收盤價格
               var flag = view.GetRowCellValue(index , view.Columns["SFD_UPD_FLAG"]).AsString();
               if (flag == "Y") {
                  e.Appearance.BackColor = Color.White;
               } else {
                  e.Appearance.BackColor = colorMint;
               }
               break;
         }
      }

      /// <summary>
      /// 全選 or 全取消 選取事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void radioGroup1_EditValueChanged(object sender , EventArgs e) {
         int rowCount = gvMain.DataRowCount;

         if (rowCount <= 0)
            return;
         //全選 or 全取消
         if (radioGroup1.EditValue.Equals("rb_sel_all")) {
            for (int k = 0 ; k < rowCount ; k++) {
               if (!string.IsNullOrEmpty(gvMain.GetRowCellValue(k , gvMain.Columns["F_KIND_ID"]).AsString())) {
                  gvMain.SetRowCellValue(k , gvMain.Columns["F_CHOOSE"] , "Y");
               }
               if (!string.IsNullOrEmpty(gvMain.GetRowCellValue(k , gvMain.Columns["O_KIND_ID"]).AsString())) {
                  gvMain.SetRowCellValue(k , gvMain.Columns["O_CHOOSE"] , "Y");
               }
            }
         } else {
            for (int k = 0 ; k < rowCount ; k++) {
               gvMain.SetRowCellValue(k , gvMain.Columns["F_CHOOSE"] , "N");
               gvMain.SetRowCellValue(k , gvMain.Columns["O_CHOOSE"] , "N");
            }
         }

      }

      /// <summary>
      /// 測試按鈕(在Debug模式下)
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void TestBtn_Click(object sender , EventArgs e) {
         DataTable dt = b40042.GetDataList();
         int rowCnt = dt.Rows.Count;
         if (rowCnt <= 0)
            return;

         //近月份期貨價格和標的證券收盤價格 不可為0 要測試這兩個欄位不等於0 所以塞入不為0的值
         for (int k = 0 ; k < rowCnt ; k++) {
            gvMain.SetRowCellValue(k , gvMain.Columns["AI5_SETTLE_PRICE"] , dt.Rows[k]["TFXMSPF_IPR"].AsDecimal() - 0.2m);
            gvMain.SetRowCellValue(k , gvMain.Columns["TFXM1_SFD_FPR"] , dt.Rows[k]["TFXMSPF_IPR"].AsDecimal() - 0.1m);
         }
      }
   }
}