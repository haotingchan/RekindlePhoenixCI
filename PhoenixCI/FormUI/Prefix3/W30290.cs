using System.Data;
using System.Windows.Forms;
using BaseGround;
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Repository;
using BaseGround.Report;
using BusinessObjects.Enums;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraGrid.Views.Grid;
using BusinessObjects;
using System.ComponentModel;
using Common;
using System.Drawing;
using System;
using BaseGround.Shared;
using System.Collections.Generic;
using System.Data.OracleClient;
using DataObjects.Dao.Together.TableDao;
using PhoenixCI.BusinessLogic.Prefix3;
using System.IO;
using System.Threading;

namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 20190422,john,30290 造市者限制設定
   /// </summary>
   public partial class W30290 : FormParent
   {

      private B30290 b30290;
      //private D30290 dao30290;
      private DialogResult retrieveChoose;
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

         //存檔和刪除都是GridView的操作，應該要等讀取後才出現這些按鈕
         _ToolBtnSave.Enabled = true;
         _ToolBtnDel.Enabled = true;
         //messagebox(gs_t_warning,"下方視窗無資料無法進行轉檔，請先執行「讀取／預覽」!",StopSign!)
         _ToolBtnExport.Enabled = true;

         string isYMD = YMDlookUpEdit.EditValue.AsString();

         int cnt = b30290.DataCount(isYMD);
         if (cnt > 0) {
            retrieveChoose = MessageDisplay.Choose("已存在相同生效日期資料，按「是」讀取已存檔資料，按「否」為重新產至資料");
            if (retrieveChoose == DialogResult.Yes) {
               gcMain.DataSource = b30290.List30290GridData(isYMD);
               return ResultStatus.Success;
            }
         }
         //if (retrieveChoose == DialogResult.No)
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

         ////存檔前檢查
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
                     if (retrieveChoose == DialogResult.No)
                        if (!b30290.DeleteData(isYMD))
                           return ResultStatus.Fail;
               }

               //儲存PLP13
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
                  ResultData myResultData = b30290.UpdateData(dtChange);
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
            Export();
         }

         retrieveChoose = DialogResult.None;
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

      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();

         b30290 = new B30290(_ProgramID);
         YMDlookUpEdit.SetDataTable(b30290.GetEffectiveYMD(emDate.Text).Clone(), "YMD", "YMD", DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor, "");

         emDate.Text = b30290.LastQuarter(GlobalInfo.OCF_DATE);
         this.emDate.Leave += new System.EventHandler(this.emDate_Leave);
         this.YMDlookUpEdit.Properties.EditValueChanged += new System.EventHandler(this.YMDlookUpEdit_Properties_EditValueChanged);
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         return base.BeforeClose();
      }

      private bool StartExport()
      {
         if (!emDate.IsDate(emDate.Text, "日期輸入錯誤")) {
            //is_chk = "Y";
            return false;
         }

         //DataTable dt = (DataTable)gcMain.DataSource;
         //if (dt.Rows.Count <= 0) {
         //   MessageDisplay.Warning("下方視窗無資料無法進行存檔，請先執行「讀取／預覽」!");
         //   ShowMsg("轉檔有誤!");
         //   return false;
         //}

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

      protected void EndExport()
      {
         stMsgTxt.Text = "";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      protected void ShowMsg(string msg)
      {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      private string OutputShowMessage {
         set {
            if (value != MessageDisplay.MSG_OK)
               MessageDisplay.Info(value);
         }
      }

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }
         string saveFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
         string msg;
         logtxt = saveFilePath;
         //Write LOGF
         WriteLog("轉出檔案:" + logtxt, "Info", "E", false);
         try {
            //Sheet : rpt_future
            string isYMD = YMDlookUpEdit.EditValue.AsString();
            msg = b30290.WfExport(saveFilePath, isYMD,emDate.Text, GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH);
            OutputShowMessage = msg;
         }
         catch (Exception ex) {
            File.Delete(saveFilePath);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
         }

         if (msg != MessageDisplay.MSG_OK) {
            ShowMsg("轉檔有誤!");
            File.Delete(saveFilePath);
            return ResultStatus.Fail;
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
         YMDlookUpEdit.SetDataTable(b30290.GetEffectiveYMD(emDate.Text), "YMD", "YMD", DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor, "");
      }

      private void YMDlookUpEdit_Properties_EditValueChanged(object sender, EventArgs e)
      {
         gcMain.DataSource = new DataTable();
         _ToolBtnSave.Enabled = false;
         _ToolBtnDel.Enabled = false;
         _ToolBtnExport.Enabled = false;
      }
   }
}