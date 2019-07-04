using System.Data;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BusinessObjects;
using Common;
using System;
using BaseGround.Shared;
using PhoenixCI.BusinessLogic.Prefix3;
using System.IO;
using System.Threading;
/// <summary>
/// 20190422,john,30290 造市者限制設定
/// </summary>
namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 造市者限制設定
   /// </summary>
   public partial class W30290 : FormParent
   {

      private B30290 b30290;
      /// <summary>
      /// 已存在相同生效日期資料對話視窗選取(是/否)紀錄
      /// </summary>
      private DialogResult retrieveChoose;
      /// <summary>
      /// 紀錄log string
      /// </summary>
      string logtxt;

      public W30290(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         GridHelper.SetCommonGrid(gvMain);
         PrintableComponent = gcMain;
         retrieveChoose = DialogResult.None;
      }

      protected override ResultStatus Retrieve()
      {
         base.Retrieve(gcMain);
         if (!emDate.IsDate(emDate.Text, "日期輸入錯誤!"))
            return ResultStatus.Fail;

         //存檔和刪除都是GridView的操作，應該要等讀取後才出現這些按鈕
         _ToolBtnSave.Enabled = true;
         _ToolBtnDel.Enabled = true;
         //messagebox(gs_t_warning,"下方視窗無資料無法進行轉檔，請先執行「讀取／預覽」!",StopSign!)
         _ToolBtnExport.Enabled = true;

         string isYMD = YMDlookUpEdit.EditValue.AsString();
         gcMain.DataSource = b30290.List30290GridData(isYMD).Clone();

         int cnt = b30290.DataCount(isYMD);
         if (cnt > 0) {
            retrieveChoose = MessageDisplay.Choose("已存在相同生效日期資料，按「是」讀取已存檔資料，按「否」為重新產至資料");
            if (retrieveChoose == DialogResult.Yes) {
               gcMain.DataSource = b30290.List30290GridData(isYMD);
               return ResultStatus.Success;
            }
         }
         //已存在相同生效日期資料，按「否」重新產至資料
         RowsCopy(isYMD);

         return ResultStatus.Success;
      }

      /// <summary>
      /// lds_insert.RowsCopy(1,lds_insert.rowcount(), primary!, dw_1, 1, primary!)
      /// </summary>
      /// <param name="isYMD"></param>
      private void RowsCopy(string isYMD)
      {
         DataTable insertData = b30290.ListInsertGridData(emDate.Text, isYMD);
         if (insertData.Rows.Count <= 0) {
            _ToolBtnSave.Enabled = false;
            _ToolBtnDel.Enabled = false;
            _ToolBtnExport.Enabled = false;
            MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
         }
         DataTable data = b30290.List30290GridData(isYMD).Clone();//dw_1.reset()
         //新增與insertData對應的行數
         foreach (DataRow item in insertData.Rows) {
            data.Rows.InsertAt(data.NewRow(), 0);
         }
         //InsertData寫入List30290Data
         for (int k = 0; k < insertData.Rows.Count; k++) {
            for (int j = 0; j < insertData.Columns.Count; j++) {
               data.Rows[k][j] = insertData.Rows[k][j];
            }
         }
         gcMain.DataSource = data;
      }

      protected override ResultStatus Save(PokeBall poke)
      {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         DataTable dt = (DataTable)gcMain.DataSource;
         DataTable dtChange = dt.GetChanges();

         //存檔前檢查
         try {
            //無法經由資料列存取已刪除的資料列資訊。
            if (dtChange != null) {
               //if (dt.Rows.Count <= 0) {
               //   MessageDisplay.Warning("下方視窗無資料無法進行存檔，請先執行「讀取／預覽」!");
               //   ShowMsg("轉檔有誤!");
               //   return ResultStatus.Fail;
               //}

               //重新產置資料儲存確認
               string isYMD = YMDlookUpEdit.EditValue.ToString();
               int dataCount = b30290.DataCount(isYMD);

               if (dataCount > 0) {
                  DialogResult ChooseResult = MessageDisplay.Choose("已存在相同生效日期資料，請問是否繼續儲存?");
                  if (ChooseResult == DialogResult.Yes)
                     if (retrieveChoose == DialogResult.No)//一開始讀取資料按「否」重新產至資料
                        if (!b30290.DeleteData(isYMD))//刪除已有的資料
                           return ResultStatus.Fail;
               }

               //變更儲存日期以及USER_ID
               foreach (DataRow dr in dt.Rows) {
                  if (dr.RowState != DataRowState.Deleted) {
                     dr["PLP13_W_TIME"] = DateTime.Now;
                     dr["PLP13_W_USER_ID"] = GlobalInfo.USER_ID;
                  }
               }
            }
            if (dtChange != null) {
               try {
                  dtChange = dt.GetChanges();
                  this.Cursor = Cursors.WaitCursor;
                  ShowMsg("存檔中...");
                  //儲存PLP13
                  ResultData myResultData = b30290.UpdateData(dtChange);
                  //初始訊息選擇狀態
                  retrieveChoose = DialogResult.None;
               }
               catch (Exception ex) {
                  WriteLog(ex);
               }
               //Write LOGF
               WriteLog("變更資料 " + logtxt, "Info", "I", false);
               return ResultStatus.Success;
            }
            else {
               MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
         }
         catch (Exception ex) {
            WriteLog(ex);
         }
         finally {
            Export();//存檔後轉出Excel
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

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();

         b30290 = new B30290(_ProgramID);
         //生效日期下拉選單
         YMDlookUpEdit.SetDataTable(b30290.GetEffectiveYMD(emDate.Text).Clone(), "YMD", "YMD", DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor, "");

         emDate.Text = b30290.LastQuarter(GlobalInfo.OCF_DATE);
         //計算日期Leave事件綁定
         this.emDate.Leave += new System.EventHandler(this.emDate_Leave);
         //生效日期下拉選單事件綁定
         this.YMDlookUpEdit.Properties.EditValueChanged += new System.EventHandler(this.YMDlookUpEdit_Properties_EditValueChanged);
         return ResultStatus.Success;
      }

      /// <summary>
      /// 轉檔前檢查日期格式及其他狀態
      /// </summary>
      /// <returns></returns>
      private bool StartExport()
      {
         if (!emDate.IsDate(emDate.Text, "日期輸入錯誤")) {
            //is_chk = "Y";
            return false;
         }

         DataTable dt = (DataTable)gcMain.DataSource;
         if (dt.Rows.Count <= 0) {
            MessageDisplay.Warning("下方視窗無資料無法進行存檔，請先執行「讀取／預覽」!");
            ShowMsg("轉檔有誤!");
            return false;
         }

         if (retrieveChoose == DialogResult.No) {
            MessageDisplay.Warning("已重新產置資料，請先執行「儲存」!");
            ShowMsg("轉檔有誤!");
            return false;
         }

         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      /// <summary>
      /// 轉檔後清除文字訊息
      /// </summary>
      private void EndExport()
      {
         stMsgTxt.Text = "";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      /// <summary>
      /// 在label上show出文字訊息
      /// </summary>
      /// <param name="msg"></param>
      private void ShowMsg(string msg)
      {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }
         //複製template
         string saveFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
         logtxt = saveFilePath;
         //Write LOGF
         WriteLog("轉出檔案:" + logtxt, "Info", "E", false);
         try {
            //OutputShowMessage只會儲存ok的狀態,如沒有任何一個ok代表全部function都沒有資料
            MessageDisplay message = new MessageDisplay();
            //Sheet : rpt_future
            string isYMD = YMDlookUpEdit.EditValue.AsString();
            message.OutputShowMessage = b30290.WfExport(saveFilePath, isYMD, emDate.Text, GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH);
            if (string.IsNullOrEmpty(message.OutputShowMessage)) {
               ShowMsg("轉檔有誤!");
               if (File.Exists(saveFilePath))
                  File.Delete(saveFilePath);
               return ResultStatus.Fail;
            }
         }
         catch (Exception ex) {
            if (File.Exists(saveFilePath))
               File.Delete(saveFilePath);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
         }

         return ResultStatus.Success;
      }

      protected override ResultStatus COMPLETE()
      {
         MessageDisplay.Info(MessageDisplay.MSG_OK);
         return ResultStatus.Success;
      }

      private void emDate_Leave(object sender, EventArgs e)
      {
         //每次輸入完畢重新搜尋下拉選單資料
         YMDlookUpEdit.SetDataTable(b30290.GetEffectiveYMD(emDate.Text), "YMD", "YMD", DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor, "");
      }

      private void YMDlookUpEdit_Properties_EditValueChanged(object sender, EventArgs e)
      {
         //選取下拉選單重新刷新頁面
         gcMain.DataSource = new DataTable();
         _ToolBtnSave.Enabled = false;
         _ToolBtnDel.Enabled = false;
         _ToolBtnExport.Enabled = false;
      }
   }
}