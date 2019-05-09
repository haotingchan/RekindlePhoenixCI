using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/05/08
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 40180 交易系統使用文字檔
   /// </summary>
   public partial class W40180 : FormParent {

      private D40180 dao40180;
      private COD daoCOD;
      private TXFP daoTXFP;
      List<string> is_adj_type;
      List<string> is_adj_type_rtn;
      string startYmd, startYmd2;

      public W40180(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao40180 = new D40180();
         daoCOD = new COD();
         daoTXFP = new TXFP();
         is_adj_type = new List<string>();
         is_adj_type_rtn = new List<string>();
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            //1. 設定初始日期時間
            txtDate.DateTimeValue = GlobalInfo.OCF_DATE;
            txtStartDate.EditValue = DateTime.ParseExact("2009/05/12" , "yyyy/MM/dd" , null);
            txtEndDate.EditValue = DateTime.ParseExact("2009/05/14" , "yyyy/MM/dd" , null);
            txtStartTime.EditValue = DateTime.ParseExact("08:45:00" , "HH:mm:ss" , null);
            txtEndTime.EditValue = DateTime.ParseExact("16:30:00" , "HH:mm:ss" , null);

            //2. 設定dropdownlist(頻率)
            DataTable dtMsg = daoCOD.ListByTxn("40180");
            dwMsg.SetDataTable(dtMsg , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor);

            if (FlagAdmin) {
               chkTxt.Visible = true;
            }

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected void ExportAfter() {
         labMsg.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         labMsg.Visible = false;
      }

      protected void ShowMsg(string msg) {
         labMsg.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export() {
         try {

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "轉檔中...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            #region 日期txtEdit檢核
            int w = 0;
            foreach (CheckedListBoxItem item in gbMsg.Items) {
               if (item.Value.AsString() == "chkFutMsg" || item.Value.AsString() == "chkOptMsg") {
                  if (w >= 1) continue;
                  w++;
                  DialogResult res = MessageDisplay.Choose("請確認公告開始及結束公告時間!");
                  if (res == DialogResult.No) {
                     return ResultStatus.Fail;
                  }
               }
            }

            if (txtDate.Text != txtStartDate.Text) {
               DialogResult res = MessageDisplay.Choose("生效日期 <> 開始公告日期,是否要修改日期?");
               if (res == DialogResult.Yes) {
                  return ResultStatus.Fail;
               }
            }
            #endregion

            foreach (CheckedListBoxItem item in gbType.Items) {
               string type = item.Value.AsString();
               switch (type) {
                  case "chkType0":
                     is_adj_type.Add("'0'");
                     break;
                  case "chkType1b":
                     is_adj_type.Add("'1'");
                     break;
                  case "chkType2b":
                     is_adj_type.Add("'2'");
                     break;
                  case "chkType3":
                     is_adj_type.Add("'3'");
                     break;
                  case "chkType1e":
                     is_adj_type_rtn.Add("'1'");
                     break;
                  case "chkType2e":
                     is_adj_type_rtn.Add("'2'");
                     break;
               }
            }

            if (gbType.CheckedItemsCount == 0 &&
               (gbMsg.CheckedItems.AsString() == "chkFutMsg" || gbMsg.CheckedItems.AsString() == "chkOptMsg" ||
                gbMoney.CheckedItems.AsString() == "chkFutMoney" || gbMoney.CheckedItems.AsString() == "chkOptMoney" ||
                gbSpan.CheckedItems.AsString() == "chkPsrS1010" || gbSpan.CheckedItems.AsString() == "chkS1020")) {
               MessageDisplay.Error("請勾選選項");
               return ResultStatus.Fail;
            }

            if (is_adj_type.Count == 0) {
               is_adj_type.Add("'N'");
            }

            if (is_adj_type_rtn.Count == 0) {
               is_adj_type_rtn.Add("'N'");
            }

            //轉文字檔
            string ls_osw_grp = "%";
            foreach (CheckedListBoxItem item in gbMsg.Items) {
               if (item.Value.AsString() == "chkFutMsg") {
                  wf_40180_fut_a0001(ls_osw_grp);
               } else if (item.Value.AsString() == "chkOptMsg") {
                  wf_40180_opt_a0001(ls_osw_grp);
               }
            }

            for (int n = 1 ; n <= 3 ; n++) {
               switch (n) {
                  case 1:
                     ls_osw_grp = "1";
                     break;
                  case 2:
                     ls_osw_grp = "5";
                     break;
                  case 3:
                     ls_osw_grp = "7";
                     break;
               }

               foreach (CheckedListBoxItem item in gbMoney.Items) {
                  if (item.Value.AsString() == "chkFutMoney") {
                     //wf_40180_fut_30004(ls_osw_grp);
                  } else if (item.Value.AsString() == "chkOptMoney") {
                     //選擇權
                     //wf_40180_opt_30004(ls_osw_grp)
                  }
               }

               foreach (CheckedListBoxItem item in gbSpan.Items) {
                  if (item.Value.AsString() == "chkVsrS1010") {
                     //wf_40180_s1010_vsr(ls_osw_grp)
                  } else if (item.Value.AsString() == "chkPsrS1010") {
                     //wf_40180_s1010_psr(ls_osw_grp)
                  } else if (item.Value.AsString() == "chkDpsrS1030") {
                     //wf_40180_s1030_dpsr(ls_osw_grp)
                  } else if (item.Value.AsString() == "chkScS1030") {
                     //wf_40180_s1030_sc(ls_osw_grp)
                  } else if (item.Value.AsString() == "chkS1020") {
                     //wf_40180_s1020(ls_osw_grp)
                  }
               }
            }

            if (gbMoney.CheckedItems.AsString() == "chkAllMoney") {
               wf_40180_all_7122();
            }

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
            this.Cursor = Cursors.Arrow;
         }
         return ResultStatus.Fail;
      }

      #region wf_40180_all_7122
      private void wf_40180_all_7122() {
         try {
            string rptName = "全部保證金7122文字檔";
            string rptId = "40180_7122";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            string ymd = txtDate.DateTimeValue.ToString("yyyyMMdd");
            DataTable dtAllTxt = dao40180.GetAllTxtData(ymd);

            if (dtAllTxt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取「保證金調整表」無任何資料!" , txtDate.Text , rptId , rptName));
            }

            //儲存文字檔
            string issueEndDate = "";

            /***************************************************
            生效起迄：20181011.txt-20181015
            20181011.txt (預約生效日=20181011, 恢復日=空日)　建立10/11 (1.5倍)
            20181015.txt (預約生效日=20181015, 恢復日=空日)　建立10/15 (1倍)
            *****************************************************/
            foreach (DataRow drAll in dtAllTxt.Rows) {
               string mgd2IssueEndDate = drAll["mgd2_issue_end_ymd"].AsString();
               if (issueEndDate != mgd2IssueEndDate) {
                  issueEndDate = mgd2IssueEndDate;
                  if (!string.IsNullOrEmpty(issueEndDate)) {
                     DataTable dtTmp = dao40180.GetAllTxtTmp(ymd , issueEndDate);

                     string fileName = "times_7122_margin_" + issueEndDate + ".txt";

                     string res = wf_copy_7122_file(fileName , dtTmp);

                     if (res != "Y") return;
                  }
               }

               if (!string.IsNullOrEmpty(issueEndDate)) {
                  drAll["mgd2_issue_end_ymd"] = "";
               }
            }

            string file = "times_7122_margin_" + ymd + ".txt";
            string res2 = wf_copy_7122_file(file , dtAllTxt);
            if (res2 != "Y") return;

         } catch (Exception ex) {
            WriteLog(ex);
            return;
         }
      }
      #endregion

      #region wf_copy_7122_file
      protected string wf_copy_7122_file(string fileName , DataTable dt) {

         try {

            string rptId = "40180_7122";

            //取得網路磁碟機路徑、帳密
            DataTable dtInfo = daoTXFP.GetPathAccPwd("file" , _ProgramID);
            string pwd = dtInfo.Rows[0]["ls_pwd"].AsString();
            pwd = PbFunc.f_decode(pwd);

            //本機
            string txtPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            //dt saveas csv
            ExportOptions csvref = new ExportOptions();
            csvref.HasHeader = false;
            csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
            Common.Helper.ExportHelper.ToCsv(dt , txtPath , csvref);

            if (chkTxt.CheckState == CheckState.Checked) {
               //執行bat f_netdragon
               //string li_rtn = f_netdragon("N" , ls_user , ls_pwd , ls_txt , is_path + "\"+ as_filename,'Y');
               //if    li_rtn <> 1  then
               //   messagebox(gs_t_err , "全部保證金7122文字檔搬檔失敗，請確認網路磁碟機連線" , StopSign!)
               return "E";
               //end   if
            } else {
               return "Y";
            }

         } catch (Exception ex) {
            WriteLog(ex);
         }
         return "E";
      }
      #endregion

      #region wf_40180_fut_a0001
      protected void wf_40180_fut_a0001(string ls_osw_grp) {
         try {

            string rptName = "期貨A0001公告文字檔";
            string rptId = "40180_a0001";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            //讀取資料
            string ymd = txtDate.DateTimeValue.ToString("yyyyMMdd");
            string oswGrp = ls_osw_grp + "%";
            string adjType = string.Join("," , is_adj_type.ToArray());
            string adjTypeRtn = string.Join("," , is_adj_type_rtn.ToArray());
            DataTable dt = dao40180.GetA0001TextDate(ymd , "F" , oswGrp , adjType , adjTypeRtn);

            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取「期貨調整保證金商品設定」無任何資料!" , txtDate.Text , rptId , rptName));
               return;
            }

            //開啟文字檔
            //Fut
            string fileName = _ProgramID + "_fut_A0001_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(dt , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!");
               return;
            }

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }
      #endregion

      #region wf_40180_opt_a0001
      protected void wf_40180_opt_a0001(string ls_osw_grp) {
         try {

            string rptName = "選擇權A0001公告文字檔";
            string rptId = "40180_a0001";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            //讀取資料
            string ymd = txtDate.DateTimeValue.ToString("yyyyMMdd");
            string oswGrp = ls_osw_grp + "%";
            string adjType = string.Join("," , is_adj_type.ToArray());
            string adjTypeRtn = string.Join("," , is_adj_type_rtn.ToArray());
            DataTable dt = dao40180.GetA0001TextDate(ymd , "O" , oswGrp , adjType , adjTypeRtn);

            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取「期貨調整保證金商品設定」無任何資料!" , txtDate.Text , rptId , rptName));
               return;
            }

            //開啟文字檔
            //Fut
            string fileName = _ProgramID + "_fut_A0001_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(dt , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!");
               return;
            }
         } catch (Exception ex) {
            WriteLog(ex);
         }
      }
      #endregion

      /// <summary>
      /// write string to txt
      /// </summary>
      /// <param name="source"></param>
      /// <param name="filePath"></param>
      /// <param name="encoding">System.Text.Encoding.GetEncoding(950)</param>
      /// <returns></returns>
      protected bool ToText(DataTable source , string filePath , System.Text.Encoding encoding) {
         try {
            FileStream fs = new FileStream(filePath , FileMode.Create);
            StreamWriter str = new StreamWriter(fs , encoding);
            str.Write(source);

            str.Flush();
            str.Close();
            return true;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return false;
      }

   }
}