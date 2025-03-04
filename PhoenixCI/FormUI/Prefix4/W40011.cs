﻿using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Shared;
using System.Threading;
using PhoenixCI.BusinessLogic.Prefix4;
using System.IO;
using Common;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DataObjects.Dao.Together;
using DataObjects;
/// <summary>
/// john,20190410,保證金狀況表 (Group1) 
/// </summary>
namespace PhoenixCI.FormUI.Prefix4
{
   /// <summary>
   /// 保證金狀況表 (Group1) 
   /// </summary>
   public partial class W40011 : FormParent
   {
      private B40011 b40011;
      /// <summary>
      /// 存檔路徑
      /// </summary>
      private string _saveFilePath;

      public W40011(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         //預設交易時段
         OCFG daoOCFG = new OCFG();
         oswGrpLookItem.SetDataTable(daoOCFG.ListAll(), "OSW_GRP", "OSW_GRP_NAME", DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor, null);
         oswGrpLookItem.EditValue = daoOCFG.f_gen_osw_grp();
      }

      protected override ResultStatus Open()
      {
         base.Open();
#if DEBUG
         emDate.Text = "2019/05/22";
#else
         emDate.DateTimeValue = DateTime.Now;
#endif
         emDate.Focus();

            if (GlobalInfo.USER_ID.ToUpper() == GlobalDaoSetting.GetConnectionInfo.ConnectionName)
            {
                chkOnlyExport.Visible = true;
            }
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;
         return ResultStatus.Success;
      }

      /// <summary>
      /// 轉檔前檢查日期格式
      /// </summary>
      /// <returns></returns>
      private bool StartExport()
      {
         if (!emDate.IsDate(emDate.Text, "日期輸入錯誤")) {
            //is_chk = "Y";
            return false;
         }

         _saveFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
         b40011 = new B40011(_ProgramID, _saveFilePath, emDate.Text);

         stMsgTxt.Visible = true;

         //判斷FMIF資料已轉入
         string chkFMIF = b40011.CheckFMIF();
         if (chkFMIF != MessageDisplay.MSG_OK) {
            if (!OutputChooseMessage(chkFMIF))
               return false;
         }

         //130批次作業做完
         string strRtn = b40011.Check130Wf();
         if (strRtn != MessageDisplay.MSG_OK) {
            if (!OutputChooseMessage(strRtn))
               return false;
         }

         stMsgTxt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      /// <summary>
      /// 轉檔後清除文字訊息
      /// </summary>
      protected void EndExport()
      {
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
      protected void ShowMsg(string msg)
      {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      private bool OutputChooseMessage(string str)
      {
         DialogResult ChooseResult = MessageDisplay.Choose(str);
         if (ChooseResult == DialogResult.No) {
            EndExport();
            return false;
         }
         return true;
      }

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            File.Delete(_saveFilePath);
            return ResultStatus.Fail;
         }
         try {
            MessageDisplay message = new MessageDisplay();
            //Sheet : rpt_future
            ShowMsg($"{_ProgramID}_1－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b40011.WfFutureSheet();
            //Sheet : rpt_option
            ShowMsg($"{_ProgramID}_2－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b40011.WfOptionSheet();
            //Sheet : fut_3index
            ShowMsg("40011_stat－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b40011.WfStat("F", "fut_3index");
            //Sheet : opt_3index
            ShowMsg("40011_stat－保證金狀況表 轉檔中...");
            message.OutputShowMessage = b40011.WfStat("O", "opt_3index");
         }
         catch (Exception ex) {
            if (File.Exists(_saveFilePath))
               File.Delete(_saveFilePath);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
         }

         return ResultStatus.Success;
      }

      /// <summary>
      /// 複製40010template並改變檔名
      /// </summary>
      /// <param name="kindID"></param>
      /// <returns></returns>
      private static string CopyWemaTemplateFile(string kindID)
      {
         string filepath;
         string tmpDate1 = DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss");
         string targetFileName = $"40010_{ kindID}_{tmpDate1}.xlsm";
         string newExcelFileName = PbFunc.wf_copy_file("40010", "40010", targetFileName);
         filepath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, newExcelFileName);
         return filepath;
      }

      private void EWMAbtn_Click(object sender, EventArgs e)
      {
         if (!emDate.IsDate(emDate.Text, "日期輸入錯誤")) {
            //is_chk = "Y";
            return;
         }

         B40010 b40010 = new B40010(emDate.Text);
         //選取時段
         string oswGrp = oswGrpLookItem.EditValue.AsString();

         string filepath = "";
         string[] pathList = null;
         string NoDataMessage = $"計算{emDate.Text}_{oswGrpLookItem.Text}-EWMA,{MessageDisplay.MSG_NO_DATA}";

         try {
            //dtMGR2暫存所有要update的資料
            DataTable dtMGR2 = b40010.MGR2DataClone();
            //商品清單
            DataTable dt = b40010.ProductList(oswGrp + "%");
            int prodCount = dt.Rows.Count;
            pathList = new string[prodCount];

            if (prodCount <= 0) {
               MessageDisplay.Info(NoDataMessage);
               return;
            }

            this.Cursor = Cursors.WaitCursor;
            ShowMsg($"EWMA 計算中...");

            //DataRow dataRow = null;

            //讀取每個商品的EWMA
            int k = 0;
            foreach (DataRow dr in dt.Rows) {
               string kindID = dr["MG1_KIND_ID"].AsString();
               ShowMsg($"EWMA_{kindID} 計算中...");
               //複製template
               filepath = CopyWemaTemplateFile(kindID);
               //記錄所產商品的檔案路徑
               pathList[k++] = filepath;
               //產出經Excel計算後的資料
               b40010.ComputeEWMA(filepath, kindID, chkOnlyExport.Checked);
               Thread.Sleep(0);
            }
            if (chkOnlyExport.Checked == false)
            {
                //呼叫SP
                ShowMsg($"EWMA 寫入資料庫...");
                b40010.ExecuteSP(oswGrp);
            }

            MessageDisplay.Info(MessageDisplay.MSG_IMPORT);

         }
         catch (Exception ex) {
            foreach (string path in pathList) {
               if (File.Exists(path))
                  File.Delete(path);
            }
            WriteLog(ex);
         }
         finally {
            EndExport();
         }
      }


   }
}