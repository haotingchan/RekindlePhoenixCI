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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataObjects.Dao.Together;
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
      private string _saveFilePath;

      public W40011(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         OCFG daoOCFG = new OCFG();
         oswGrpLookItem.SetDataTable(daoOCFG.ListAll(), "OSW_GRP", "OSW_GRP_NAME", DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor, null);
         oswGrpLookItem.EditValue = daoOCFG.f_gen_osw_grp();
      }

      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
#if DEBUG
         emDate.Text = "2019/05/22";
#else
         emDate.DateTimeValue = DateTime.Now;
#endif
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         emDate.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;
         return ResultStatus.Success;
      }

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

      private bool OutputChooseMessage(string str)
      {
         DialogResult ChooseResult = MessageDisplay.Choose(str);
         if (ChooseResult == DialogResult.No) {
            EndExport();
            return false;
         }
         return true;
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
            File.Delete(_saveFilePath);
            return ResultStatus.Fail;
         }
         try {
            //Sheet : rpt_future
            ShowMsg($"{_ProgramID}_1－保證金狀況表 轉檔中...");
            OutputShowMessage = b40011.WfFutureSheet();
            //Sheet : rpt_option
            ShowMsg($"{_ProgramID}_2－保證金狀況表 轉檔中...");
            OutputShowMessage = b40011.WfOptionSheet();
            //Sheet : fut_3index
            ShowMsg("40011_stat－保證金狀況表 轉檔中...");
            OutputShowMessage = b40011.WfStat("F", "fut_3index");
            //Sheet : opt_3index
            ShowMsg("40011_stat－保證金狀況表 轉檔中...");
            OutputShowMessage = b40011.WfStat("O", "opt_3index");
         }
         catch (Exception ex) {
            File.Delete(_saveFilePath);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
         }

         return ResultStatus.Success;
      }

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
         string oswGrp = oswGrpLookItem.EditValue.AsString() + "%";

         string filepath = "";
         string[] pathList = null;
         try {
            DataTable dtMGR2 = b40010.MGR2DataClone();
            DataTable dt = b40010.ProductList(oswGrp);
            int prodCount = dt.Rows.Count;
            pathList = new string[prodCount];
            if (prodCount <= 0)
               MessageDisplay.Info(emDate.Text + MessageDisplay.MSG_NO_DATA);

            this.Cursor = Cursors.WaitCursor;
            ShowMsg($"WEMA 計算中...");

            DataRow dataRow = null;

            int k = 0;
            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 };
            Parallel.ForEach(dt.AsEnumerable(), options, (dr, state) => {
               string kindID = dr["MG1_KIND_ID"].AsString();

               filepath = CopyWemaTemplateFile(kindID);
               pathList[k++] = filepath;
               dataRow = b40010.ComputeEWMA(filepath, kindID);

               if (dataRow == null)
                  return;

               dtMGR2.ImportRow(dataRow); ;
            });

            if (dtMGR2.Rows.Count > 0) {
               //Save
               b40010.MGR2Save(dtMGR2);
               MessageDisplay.Info(MessageDisplay.MSG_IMPORT);
            }
            else {
               MessageDisplay.Info($"{emDate.Text},40011_stat－保證金狀況表,無任何資料!");
            }

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