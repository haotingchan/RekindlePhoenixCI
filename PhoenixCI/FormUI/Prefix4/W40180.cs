using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using Common.Helper;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
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

            txtDate_EditValueChanged(txtDate , null);

            //2. 設定dropdownlist(頻率)
            DataTable dtMsg = daoCOD.ListByTxn("40180");
            dwMsg.SetDataTable(dtMsg , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor);
            dwMsg.ItemIndex = 0;

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

            if (gbMsg.CheckedItemsCount > 0) {
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
            }

            if (txtDate.Text != txtStartDate.Text) {
               DialogResult res = MessageDisplay.Choose("生效日期 <> 開始公告日期,是否要修改日期?");
               if (res == DialogResult.Yes) {
                  return ResultStatus.Fail;
               }
            }
            #endregion

            is_adj_type.Clear();
            is_adj_type_rtn.Clear();
            foreach (CheckedListBoxItem item in gbType.Items) {
               if (item.CheckState != CheckState.Checked) continue;

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
               (gbMsg.Items[0].CheckState == CheckState.Checked || gbMsg.Items[1].CheckState == CheckState.Checked ||
                gbMoney.Items[0].CheckState == CheckState.Checked || gbMoney.Items[1].CheckState == CheckState.Checked ||
                gbSpan.Items[1].CheckState == CheckState.Checked || gbSpan.Items[4].CheckState == CheckState.Checked)) {
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

               if (item.CheckState != CheckState.Checked) continue;

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

               //foreach (CheckedListBoxItem item in gbMoney.Items) {

               //   if (item.CheckState != CheckState.Checked) continue;

               //   if (item.Value.AsString() == "chkFutMoney") {
               //      wf_40180_fut_30004(ls_osw_grp);
               //   } else if (item.Value.AsString() == "chkOptMoney") {
               //      //選擇權
               //      wf_40180_opt_30004(ls_osw_grp);
               //   }
               //}

               //chkFutMoney
               if (gbMoney.Items[0].CheckState.ToString() == "Checked") {
                  wf_40180_fut_30004(ls_osw_grp);
               }

               foreach (CheckedListBoxItem item in gbSpan.Items) {

                  if (item.CheckState != CheckState.Checked) continue;

                  if (item.Value.AsString() == "chkVsrS1010") {
                     wf_40180_s1010_vsr(ls_osw_grp); //2018/06/13
                  } else if (item.Value.AsString() == "chkPsrS1010") {
                     wf_40180_s1010_psr(ls_osw_grp); //(待 ci.hzparm , ci.mgd2 補資料 目前查不到資料)
                  } else if (item.Value.AsString() == "chkDpsrS1030") {
                     wf_40180_s1030_dpsr(ls_osw_grp); //2018/06/13
                  } else if (item.Value.AsString() == "chkScS1030") {
                     wf_40180_s1030_sc(ls_osw_grp); //(目前無符合'SS'的資料)
                  } else if (item.Value.AsString() == "chkS1020") {
                     wf_40180_s1020(ls_osw_grp);
                  }
               }

               //選擇權 chkOptMoney
               if (gbMoney.Items[1].CheckState.ToString() == "Checked") {
                  wf_40180_opt_30004(ls_osw_grp);
               }

            }

            if (gbMoney.Items[2].CheckState == CheckState.Checked) {
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
               return;
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

            //取得網路磁碟機路徑、帳密
            DataTable dtInfo = daoTXFP.GetPathAccPwd("file" , _ProgramID);
            string userId = dtInfo.Rows[0]["ls_user"].AsString();
            string pwd = dtInfo.Rows[0]["ls_pwd"].AsString();
            string targetPath = dtInfo.Rows[0]["is_path"].AsString();
            pwd = PbFunc.f_decode(pwd);

            //本機
            string txtPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            //dt saveas csv
            ExportOptions csvref = new ExportOptions();
            csvref.HasHeader = false;
            csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
            Common.Helper.ExportHelper.ToCsv(dt , txtPath , csvref);

            if (chkTxt.CheckState == CheckState.Checked) {
               //執行f_netdragon
               int li_rtn = Go("N" , userId , pwd , txtPath , targetPath + "\\" + fileName , "Y");
               if (li_rtn != 1) {
                  MessageDisplay.Error("全部保證金7122文字檔搬檔失敗，請確認網路磁碟機連線");
                  return "E";
               }
               return "Y";
            }

         } catch (Exception ex) {
            WriteLog(ex);
         }
         return "E";
      }
      #endregion

      #region f_netdragon 中的 method Go()
      public static int Go(string domain , string userName , string password , string pathSourceFile , string pathTargetFile , string isOutputReadyFile) {
         int num = 0;
         NetworkCredential credentials = new NetworkCredential(userName , password);
         string directoryName = Path.GetDirectoryName(pathTargetFile);
         try {
            using (new ConnectSharedFolder(directoryName , credentials)) {
               File.Copy(pathSourceFile , pathTargetFile , true);
               Thread.Sleep(100);
               if (isOutputReadyFile == "Y") {
                  File.Copy(pathSourceFile , pathTargetFile + ".ready" , true);
               }
               num = 1;
            }
         } catch (Exception exception1) {
            MessageBox.Show(exception1.Message);
            num = -1;
         }
         return num;
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

            DataTable dt = dao40180.GetA0001TextDate(ymd , "F" , oswGrp , is_adj_type , is_adj_type_rtn);

            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取「期貨調整保證金商品設定」無任何資料!" , txtDate.Text , rptId , rptName));
               return;
            }
               
            //主旨
            string fromTime = string.Format("{0} {1}", txtStartDate.Text, txtStartTime.Text);
            string toTime = string.Format("{0} {1}" , txtEndDate.Text , txtEndTime.Text);
            string ls_head = fromTime + "\t" + toTime + "\t" + dwMsg.EditValue.AsString() + "\t";

            string ls_txt = "";
            foreach (DataRow dr in dt.Rows) {
               int currencyType = dr["mgd2_currency_type"].AsString().AsInt();
               CurrencyType enumCurType = (CurrencyType)currencyType;
               string ls_currency_type = PbFunc.f_conv_currency_type(enumCurType);

               string txt1 = "", txt2 = "", txt3 = "", txt4 = "";

               string paramKey = dr["mgd2_param_key"].AsString();
               string abbrName = dr["mgt2_abbr_name"].AsString();
               decimal mgd2Cm = dr["mgd2_cm"].AsDecimal();
               decimal mgd2Mm = dr["mgd2_mm"].AsDecimal();
               decimal mgd2Im = dr["mgd2_im"].AsDecimal();
               if (paramKey == "STC" || paramKey == "STF") {
                  //(1)
                  txt1 = ls_head + "自" + PbFunc.f_conv_date(txtDate.DateTimeValue , 3) + "一般交易時段結束後" + Environment.NewLine;

                  //(2)
                  txt2 = abbrName + "結算保證金適用比例調整為" + string.Format("{0:0.00%}" , mgd2Cm) + ",";
                  if (txt2.Length > 80) {
                     MessageDisplay.Warning(string.Format("文字超過80字元（{0}),按確定後繼續..." , txt2));
                     txt2 = txt2.SubStr(0 , 80);
                  }
                  txt2 = ls_head + txt2 + Environment.NewLine;

                  //(3)
                  txt3 = ls_head + "維持保證金適用比例調整為" + string.Format("{0:0.00%}" , mgd2Mm) + "," + Environment.NewLine;

                  //(4)
                  txt4 = ls_head + "原始保證金適用比例調整為" + string.Format("{0:0.00%}" , mgd2Im) + Environment.NewLine;

               } else {
                  //(1)
                  txt1 = ls_head + "自" + PbFunc.f_conv_date(txtDate.DateTimeValue , 3) + "一般交易時段結束後" + Environment.NewLine;

                  //(2)
                  txt2 = abbrName + "結算保證金調整為" + string.Format("{0:N0}" , mgd2Cm) + ls_currency_type + ",";
                  if (txt2.Length > 80) {
                     MessageDisplay.Warning(string.Format("文字超過80字元（{0}),按確定後繼續..." , txt2));
                     txt2 = txt2.SubStr(0 , 80);
                  }
                  txt2 = ls_head + txt2 + Environment.NewLine;

                  //(3)
                  txt3 = ls_head + "維持保證金調整為" + string.Format("{0:N0}" , mgd2Mm) + ls_currency_type + "," + Environment.NewLine;

                  //(4)
                  txt4 = ls_head + "原始保證金調整為" + string.Format("{0:N0}" , mgd2Im) + ls_currency_type + Environment.NewLine;

               }
               ls_txt += txt1 + txt2 + txt3 + txt4;
            }

            //開啟文字檔
            //Fut
            string fileName = _ProgramID + "_fut_A0001_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(ls_txt , filePath , System.Text.Encoding.GetEncoding(950));
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

            DataTable dt = dao40180.GetA0001TextDate(ymd , "O" , oswGrp , is_adj_type , is_adj_type_rtn);

            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取「選擇權調整保證金商品設定」無任何資料!" , txtDate.Text , rptId , rptName));
               return;
            }

            //主旨           
            string fromTime = string.Format("{0} {1}" , txtStartDate.Text , txtStartTime.Text);
            string toTime = string.Format("{0} {1}" , txtEndDate.Text , txtEndTime.Text);
            string ls_head = fromTime + "\t" + toTime + "\t" + dwMsg.EditValue.AsString() + "\t";

            int pos = -1;
            string ls_txt = "";
            foreach (DataRow dr in dt.Rows) {
               pos++;
               if (pos % 2 != 0) continue;
               int currencyType = dr["mgd2_currency_type"].AsString().AsInt();
               CurrencyType enumCurType = (CurrencyType)currencyType;
               string ls_currency_type = PbFunc.f_conv_currency_type(enumCurType);

               string txt1 = "", txt2 = "", txt3 = "", txt4 = "";

               string paramKey = dr["mgd2_param_key"].AsString();
               string abbrName = dr["mgt2_abbr_name"].AsString();
               decimal mgd2Cm = dr["mgd2_cm"].AsDecimal();
               decimal mgd2Mm = dr["mgd2_mm"].AsDecimal();
               decimal mgd2Im = dr["mgd2_im"].AsDecimal();
               if (paramKey == "STC" || paramKey == "STF") {
                  //(1)
                  txt1 = ls_head + "自" + PbFunc.f_conv_date(txtDate.DateTimeValue , 3) + "一般交易時段結束後" + Environment.NewLine;

                  //(2)
                  txt2 = abbrName + "的A/B值之結算保證金適用比例調整為" + string.Format("{0:0.000%}" , mgd2Cm) + "/" +
                                       string.Format("{0:0.000%}" , dt.Rows[pos + 1]["mgd2_cm"].AsDecimal()) + ",";

                  if (txt2.Length > 80) {
                     MessageDisplay.Warning(string.Format("文字超過80字元（{0}),按確定後繼續..." , txt2));
                     txt2 = txt2.SubStr(0 , 80);
                  }
                  txt2 = ls_head + txt2 + Environment.NewLine;

                  //(3)
                  txt3 = ls_head + "維持保證金適用比例調整為" + string.Format("{0:0.000%}" , mgd2Mm) + "/" +
                                 string.Format("{0:0.000%}" , dt.Rows[pos + 1]["mgd2_mm"].AsDecimal()) + "," + Environment.NewLine;

                  //(4)
                  txt4 = ls_head + "原始保證金適用比例調整為" + string.Format("{0:0.000%}" , mgd2Im) + "/" +
                                 string.Format("{0:0.000%}" , dt.Rows[pos + 1]["mgd2_im"].AsDecimal()) + "," + Environment.NewLine;
               } else {
                  //(1)
                  txt1 = ls_head + "自" + PbFunc.f_conv_date(txtDate.DateTimeValue , 3) + "一般交易時段結束後" + Environment.NewLine;

                  //(2)
                  txt2 = abbrName + "的A/B值之結算保證金調整為" + string.Format("{0:N0}" , mgd2Cm) + "/" +
                       string.Format("{0:N0}" , dt.Rows[pos + 1]["mgd2_cm"].AsDecimal()) + ls_currency_type + ",";

                  if (txt2.Length > 80) {
                     MessageDisplay.Warning(string.Format("文字超過80字元（{0}),按確定後繼續..." , txt2));
                     txt2 = txt2.SubStr(0 , 80);
                  }
                  txt2 = ls_head + txt2 + Environment.NewLine;

                  //(3)
                  txt3 = ls_head + "維持保證金調整為" + string.Format("{0:N0}" , mgd2Mm) + "/" +
                      string.Format("{0:N0}" , dt.Rows[pos + 1]["mgd2_mm"].AsDecimal()) + ls_currency_type + "," + Environment.NewLine;

                  //(4)
                  txt4 = ls_head + "原始保證金調整為" + string.Format("{0:N0}" , mgd2Im) + "/" +
                      string.Format("{0:N0}" , dt.Rows[pos + 1]["mgd2_im"].AsDecimal()) + ls_currency_type + "," + Environment.NewLine;
               }
               ls_txt += txt1 + txt2 + txt3 + txt4;
            }

            //開啟文字檔
            //Opt
            string fileName = _ProgramID + "_opt_A0001_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(ls_txt , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!");
               return;
            }

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }
      #endregion

      #region wf_40180_fut_30004
      protected void wf_40180_fut_30004(string ls_osw_grp) {
         try {
            //需求單10300299：新增報價部位保證金數值=結算保證金數值
            string rptName = "期貨30004保證金文字檔";
            string rptId = "40180_30004";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            string ls_file_grp = wf_file_grp(ls_osw_grp);

            foreach (CheckedListBoxItem item in gbType.Items) {

               if (item.CheckState != CheckState.Checked) continue;

               if (item.Value.AsString() == "chkType4") {
                  is_adj_type.Add("'4'");
               }
            }

            //讀取資料
            string ymd = txtDate.DateTimeValue.ToString("yyyyMMdd");
            string oswGrp = ls_osw_grp + "%";

            DataTable dt = dao40180.Get30004TextDate(ymd , "F" , oswGrp , is_adj_type , is_adj_type_rtn);

            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2}(_{3}),讀取「期貨調整保證金商品設定」無任何資料!" , txtDate.Text , rptId , rptName , ls_file_grp));
               return;
            }

            //主旨           
            string ls_txt = "";
            foreach (DataRow dr in dt.Rows) {
               //(1)
               string kindId = dr["mgd2_kind_id"].AsString();
               string prodType = dr["mgd2_prod_type"].AsString();
               decimal mgd2Cm = dr["mgd2_cm"].AsDecimal();
               decimal mgd2Im = dr["mgd2_im"].AsDecimal();
               ls_txt += kindId + "\t" + prodType + "\t" + mgd2Cm + "\t" + mgd2Im + "\t" + mgd2Cm + Environment.NewLine;
            }

            //開啟文字檔
            //Fut
            string fileName = _ProgramID + "_fut_30004_" + ls_file_grp + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(ls_txt , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!");
               return;
            }

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }
      #endregion

      #region wf_40180_opt_30004
      protected void wf_40180_opt_30004(string ls_osw_grp) {
         try {
            string rptName = "選擇權30004保證金文字檔";
            string rptId = "40180_30004";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            string ls_file_grp = wf_file_grp(ls_osw_grp);

            if (gbType.CheckedItems.AsString() == "chkType4")
               is_adj_type.Add("'4'");

            //讀取資料
            string ymd = txtDate.DateTimeValue.ToString("yyyyMMdd");
            string oswGrp = ls_osw_grp + "%";

            DataTable dt = dao40180.Get30004TextDate(ymd , "O" , oswGrp , is_adj_type , is_adj_type_rtn);

            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2}(_{3}),讀取「選擇權調整保證金商品設定」無任何資料!" , txtDate.Text , rptId , rptName , ls_file_grp));
               return;
            }

            //主旨           
            string ls_txt = "";
            foreach (DataRow dr in dt.Rows) {
               //(1)
               string kindId = dr["mgd2_kind_id"].AsString();
               string prodType = dr["mgd2_prod_type"].AsString();
               decimal mgd2Cm = dr["mgd2_cm"].AsDecimal();
               decimal cpCm = dr["cp_cm"].AsDecimal();
               ls_txt += kindId + "\t" + prodType + "\t" + mgd2Cm + "\t" + cpCm + "\t" + mgd2Cm + "\t" + mgd2Cm + "\t" + mgd2Cm + Environment.NewLine;
            }

            //開啟文字檔
            //Opt
            string fileName = _ProgramID + "_opt_30004_" + ls_file_grp + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(ls_txt , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!");
               return;
            }

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }
      #endregion

      #region wf_40180_s1010_vsr
      protected void wf_40180_s1010_vsr(string ls_osw_grp) {
         try {
            string rptName = "SPAN VSR文字檔";
            string rptId = "40180_span";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            string ls_file_grp = wf_file_grp(ls_osw_grp);

            //讀取資料
            string oswGrp = ls_osw_grp + "%";
            DataTable dt = dao40180.GetVsrTxtData(txtDate.DateTimeValue , "SV" , oswGrp);

            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2}(_{3}),讀取「期貨調整保證金商品設定」無任何資料!" , txtDate.Text , rptId , rptName , ls_file_grp));
               return;
            }

            #region 40180_fut_S1010(VSR)_
            //主旨   
            string ls_txt = "";
            foreach (DataRow dr in dt.Rows) {
               //(1)
               string kindId1Out = dr["spt1_kind_id1_out"].AsString();
               decimal sp1Rate = dr["sp1_rate"].AsDecimal();
               ls_txt += kindId1Out + "\t" + sp1Rate + Environment.NewLine;
            }

            //fut_S1010(VSR)文字檔
            string fileName = _ProgramID + "_fut_S1010(VSR)_" + ls_file_grp + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(ls_txt , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!");
               return;
            }
            #endregion

            #region 40180_fut_S1110(VSR)_
            //主旨   
            string ls_txt2 = "";
            foreach (DataRow dr in dt.Rows) {
               //(1)
               string kindId1Out = dr["spt1_kind_id1_out"].AsString();
               decimal sp1Rate = dr["sp1_rate"].AsDecimal();
               string sp2ValueDate = dr["sp2_value_date"].AsDateTime().ToString("yyyyMMdd");
               string sp2OswGrp = dr["sp2_osw_grp"].AsString();

               string sp2_osw_grp = wf_conv_osw_grp(ls_osw_grp);

               ls_txt2 += kindId1Out + "\t" + sp1Rate + "\t" + sp2ValueDate + "\t" + sp2_osw_grp + Environment.NewLine;
            }

            //fut_S1110(VSR)文字檔
            string fileName2 = _ProgramID + "_fut_S1110(VSR)_" + ls_file_grp + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath2 = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName2);

            bool IsSuccess2 = ToText(ls_txt2 , filePath2 , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess2) {
               MessageDisplay.Error("文字檔「" + filePath2 + "」Open檔案錯誤!");
               return;
            }
            #endregion

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }
      #endregion

      #region wf_40180_s1010_psr 
      protected void wf_40180_s1010_psr(string ls_osw_grp) {
         try {
            string rptName = "Span PSR(結算保證金)文字檔";
            string rptId = "40180_30004";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            string ls_file_grp = wf_file_grp(ls_osw_grp);

            //讀取資料
            string ymd = txtDate.DateTimeValue.ToString("yyyyMMdd");
            string oswGrp = ls_osw_grp + "%";
            DataTable dt = dao40180.Get30004TextDate(ymd , "F" , oswGrp , is_adj_type , is_adj_type_rtn);

            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2}(_{3}),讀取「期貨&選擇權調整保證金商品設定」無任何資料!" , txtDate.Text , rptId , rptName , ls_file_grp));
               return;
            }

            //商品群組
            //資料日期
            DateTime ldt_date = DateTime.ParseExact(dt.Rows[0]["mgd2_ymd"].AsString() , "yyyyMMdd" , null);
            DataTable dtZ = dao40180.GetHzparmData(ldt_date);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取「期貨&選擇權調整保證金商品設定」無任何資料!" , txtDate.Text , rptId , rptName));
               return;
            }

            #region 40180_fut_S1010(PSR)_
            //主旨
            string ls_txt = "";
            int pos = -1;
            foreach (DataRow dr in dt.Rows) {
               pos++;
               //組合商品
               //找組合碼comb_prod
               int found = 0;
               if (dtZ.Select("zparm_prod_id='" + dr["mgd2_kind_id"] + "'").Length != 0) {
                  found = dtZ.Rows.IndexOf(dtZ.Select("zparm_prod_id='" + dr["mgd2_kind_id"] + "'")[0]);
               }

               string kindId = dr["mgd2_kind_id"].AsString();
               decimal mgd2Cm = dr["mgd2_cm"].AsDecimal();
               if (found < 0) {
                  ls_txt += kindId + "\t" + mgd2Cm + Environment.NewLine;
                  continue;
               }

               string ls_comb = dtZ.Rows[found]["zparm_comb_prod"].AsString();
               dr["comb_prod"] = ls_comb;

               //先前組合碼已列示
               if (pos > 0) {

                  DataTable dtTmp = dt.Clone();
                  for (int w = 0 ; w < pos ; w++) {
                     dtTmp.ImportRow(dt.Rows[w]);
                  }

                  if (dtTmp.Select("comb_prod='" + ls_comb + "'").Length != 0) {
                     found = dtTmp.Rows.IndexOf(dtTmp.Select("comb_prod='" + ls_comb + "'")[0]);
                     if (found >= 0)
                        continue;
                  }
               }

               ls_txt += kindId + "\t" + mgd2Cm + Environment.NewLine;

               //找組合碼的其它商品
               DataTable dtZfilter = dtZ.Filter("zparm_prod_id<>'" + kindId + "' and zparm_comb_prod='" + ls_comb + "'");
               foreach (DataRow drZ in dtZfilter.Rows) {
                  string prodId = drZ["zparm_prod_id"].AsString();
                  ls_txt += prodId + "\t";
                  if (prodId == "MXF") {
                     if (dt.Select("mgd2_kind_id='MXF'").Length != 0) {
                        found = dt.Rows.IndexOf(dt.Select("mgd2_kind_id='MXF'")[0]);
                        ls_txt += dt.Rows[found]["mgd2_cm"] + Environment.NewLine;
                     }
                  } else {
                     ls_txt += dr["mgd2_cm"] + Environment.NewLine;
                  }
               }
            }//foreach (DataRow dr in dt.Rows)

            //fut_S1010(PSR)文字檔
            string fileName = _ProgramID + "_fut_S1010(PSR)_" + ls_file_grp + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(ls_txt , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!");
               return;
            }
            #endregion

            #region 40180_fut_S1110(PSR)_
            string ls_txt2 = "";
            pos = -1;
            foreach (DataRow dr in dt.Rows) {
               pos++;

               //組合商品
               //找組合碼comb_prod
               int found2 = 0;
               string kindId = dr["mgd2_kind_id"].AsString();
               decimal mgd2Cm = dr["mgd2_cm"].AsDecimal();
               string issueBeginYmd = dr["mgd2_issue_begin_ymd"].AsString();
               string mgd2OswGrp = dr["mgd2_osw_grp"].AsString();
               if (dtZ.Select("zparm_prod_id = '" + kindId + "'").Length != 0) {
                  found2 = dtZ.Rows.IndexOf(dtZ.Select("zparm_prod_id = '" + kindId + "'")[0]);
               }

               string oswGrp2 = wf_conv_osw_grp(mgd2OswGrp);

               if (found2 < 0) {
                  ls_txt2 += kindId + "\t" + mgd2Cm + "\t" + issueBeginYmd + "\t" + oswGrp2 + Environment.NewLine;
                  continue;
               }

               string ls_comb = dtZ.Rows[found2]["zparm_comb_prod"].AsString();
               dr["comb_prod"] = ls_comb;

               //先前組合碼已列示
               if (pos > 0) {

                  DataTable dtTmp2 = dt.Clone();
                  for (int w = 0 ; w < pos ; w++) {
                     dtTmp2.ImportRow(dt.Rows[w]);
                  }

                  if (dtTmp2.Select("comb_prod='" + ls_comb + "'").Length != 0) {
                     found2 = dtTmp2.Rows.IndexOf(dtTmp2.Select("comb_prod='" + ls_comb + "'")[0]);
                     if (found2 >= 0)
                        continue;
                  }
               }

               ls_txt2 += kindId + "\t" + mgd2Cm + "\t" + issueBeginYmd + "\t" + oswGrp2 + Environment.NewLine;

               //找組合碼的其它商品
               DataTable dtZfilter2 = dtZ.Filter("zparm_prod_id<>'" + kindId + "' and zparm_comb_prod='" + ls_comb + "'");
               //dtZfilter2.DefaultView.Sort = "zparm_prod_id desc"; //為了要跟PB的排序一樣
               //DataTable dtZfilter3 = dtZfilter2.DefaultView.ToTable();

               foreach (DataRow drZ2 in dtZfilter2.Rows) {
                  string zparmProdId = drZ2["zparm_prod_id"].AsString();
                  ls_txt2 += zparmProdId + "\t";

                  if (zparmProdId == "MXF") {
                     if (dt.Select("mgd2_kind_id='MXF'").Length != 0) {
                        found2 = dt.Rows.IndexOf(dt.Select("mgd2_kind_id='MXF'")[0]);
                     }
                     decimal mgd2Cm2 = dt.Rows[found2]["mgd2_cm"].AsDecimal();
                     ls_txt2 += mgd2Cm2 + "\t";
                  } else {
                     ls_txt2 += mgd2Cm + "\t";
                  }

                  ls_txt2 += issueBeginYmd + "\t" + oswGrp2 + Environment.NewLine;

               }//foreach (DataRow drZ2 in dtZfilter2.Rows)
            }//foreach (DataRow dr in dt.Rows)

            //fut_S1110(PSR)文字檔
            string fileName2 = _ProgramID + "_fut_S1110(PSR)_" + ls_file_grp + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath2 = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName2);

            bool IsSuccess2 = ToText(ls_txt2 , filePath2 , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess2) {
               MessageDisplay.Error("文字檔「" + filePath2 + "」Open檔案錯誤!");
               return;
            }
            #endregion

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }
      #endregion

      #region wf_40180_s1020
      protected void wf_40180_s1020(string ls_osw_grp) {
         /********************************
         2009.09.11
         程式碼同 wf_40180_s1010_psr()
         差在：(1)產出檔名不同
              (2)群組中商品為選擇權時,不處理
         ********************************/

         try {
            string rptName = "Span S1020結算保證金文字檔";
            string rptId = "40180_30004";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            string ls_file_grp = wf_file_grp(ls_osw_grp);

            //讀取資料(只有期貨)
            string ymd = txtDate.DateTimeValue.ToString("yyyyMMdd");
            string oswGrp = ls_osw_grp + "%";
            DataTable dt = dao40180.Get30004TextDate(ymd , "F" , oswGrp , is_adj_type , is_adj_type_rtn);

            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2}(_{3}),讀取「期貨&選擇權調整保證金商品設定」無任何資料!" , txtDate.Text , rptId , rptName , ls_file_grp));
               return;
            }

            #region 40180_fut_S1020_
            //主旨
            string ls_txt = "";
            foreach (DataRow dr in dt.Rows) {
               string kindId = dr["mgd2_kind_id"].AsString();
               string paramKey = dr["mgd2_param_key"].AsString();
               decimal mgd2Cm = dr["mgd2_cm"].AsDecimal();
               if (kindId == "MXF" || paramKey == "STC" || paramKey == "STF") continue;

               ls_txt += kindId + "\t" + mgd2Cm + Environment.NewLine;

            }//foreach (DataRow dr in dt.Rows)

            if (ls_txt.Length != 0) {
               //_fut_S1020_文字檔
               string fileName = _ProgramID + "_fut_S1020_" + ls_file_grp + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
               string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

               bool IsSuccess = ToText(ls_txt , filePath , System.Text.Encoding.GetEncoding(950));
               if (!IsSuccess) {
                  MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!");
                  return;
               }
            }
            #endregion

            #region 40180_fut_S1120_
            //主旨
            string ls_txt2 = "";
            foreach (DataRow dr in dt.Rows) {
               string kindId = dr["mgd2_kind_id"].AsString();
               string paramKey = dr["mgd2_param_key"].AsString();
               decimal mgd2Cm = dr["mgd2_cm"].AsDecimal();
               string issueBeginYmd = dr["mgd2_issue_begin_ymd"].AsString();
               string mgd2OswGrp = dr["mgd2_osw_grp"].AsString();
               if (kindId == "MXF" || paramKey == "STC" || paramKey == "STF") continue;

               string oswGrp2 = wf_conv_osw_grp(mgd2OswGrp);

               ls_txt2 += kindId + "\t" + mgd2Cm + "\t" + issueBeginYmd + "\t" + oswGrp2 + Environment.NewLine;

            }//foreach (DataRow dr in dt.Rows)

            if (ls_txt2.Length != 0) {
               //fut_S1120文字檔
               string fileName2 = _ProgramID + "_fut_S1120_" + ls_file_grp + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
               string filePath2 = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName2);

               bool IsSuccess2 = ToText(ls_txt2 , filePath2 , System.Text.Encoding.GetEncoding(950));
               if (!IsSuccess2) {
                  MessageDisplay.Error("文字檔「" + filePath2 + "」Open檔案錯誤!");
                  return;
               }
            }
            #endregion

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }
      #endregion

      #region wf_40180_s1030_dpsr
      protected void wf_40180_s1030_dpsr(string ls_osw_grp) {
         try {
            string rptName = "SPAN Delta Per Spread Ratio文字檔";
            string rptId = "40180_span";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            string ls_file_grp = wf_file_grp(ls_osw_grp);
            string oswGrp = ls_osw_grp + "%";

            //讀取資料
            DataTable dt = dao40180.GetVsrTxtData(txtDate.DateTimeValue , "SD" , oswGrp);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2}(_{3}),讀取「期貨調整保證金商品設定」無任何資料!" , txtDate.Text , rptId , rptName , ls_file_grp));
               return;
            }

            #region 40180_fut_S1030(Delta Per Spread Ratio)_
            //主旨
            string ls_txt = "";

            foreach (DataRow dr in dt.Rows) {
               //(1)
               string kindId1Out = dr["spt1_kind_id1_out"].AsString();
               string kindId2Out = dr["spt1_kind_id2_out"].AsString();
               decimal sp1Rate = dr["sp1_rate"].AsDecimal();
               ls_txt += kindId1Out + "\t" + kindId2Out + "\t" + sp1Rate + Environment.NewLine;
            }

            //fut_S1030(Delta Per Spread Ratio)文字檔
            string fileName = _ProgramID + "_fut_S1030(Delta Per Spread Ratio)_" + ls_file_grp + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(ls_txt , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!");
               return;
            }
            #endregion

            #region 40180_fut_S1130(Delta Per Spread Ratio)_
            //主旨
            string ls_txt2 = "";

            foreach (DataRow dr in dt.Rows) {
               //(1)
               string kindId1Out = dr["spt1_kind_id1_out"].AsString();
               string kindId2Out = dr["spt1_kind_id2_out"].AsString();
               decimal sp1Rate = dr["sp1_rate"].AsDecimal();
               string sp2ValueDate = dr["sp2_value_date"].AsDateTime().ToString("yyyyMMdd");
               string sp2OswGrp = dr["sp2_osw_grp"].AsString();
               string og = wf_conv_osw_grp(sp2OswGrp);
               ls_txt2 += kindId1Out + "\t" + kindId2Out + "\t" + sp1Rate + "\t" + sp2ValueDate + "\t" + og + Environment.NewLine;
            }

            //fut_S1130(Delta Per Spread Ratio)文字檔
            string fileName2 = _ProgramID + "_fut_S1130(Delta Per Spread Ratio)_" + ls_file_grp + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath2 = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName2);

            bool IsSuccess2 = ToText(ls_txt2 , filePath2 , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath2 + "」Open檔案錯誤!");
               return;
            }
            #endregion

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }
      #endregion

      #region wf_40180_s1030_sc
      protected void wf_40180_s1030_sc(string ls_osw_grp) {
         try {
            string rptName = "SPAN Spread credit文字檔";
            string rptId = "40180_span";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            string ls_file_grp = wf_file_grp(ls_osw_grp);
            string oswGrp = ls_osw_grp + "%";

            //讀取資料
            DataTable dt = dao40180.GetVsrTxtData(txtDate.DateTimeValue , "SS" , oswGrp);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2}(_{3}),讀取「期貨調整保證金商品設定」無任何資料!" , txtDate.Text , rptId , rptName , ls_file_grp));
               return;
            }

            #region 40180_fut_S1030(Spread credit)_
            //主旨
            string ls_txt = "";

            foreach (DataRow dr in dt.Rows) {
               //(1)
               string kindId1Out = dr["spt1_kind_id1_out"].AsString();
               string kindId2Out = dr["spt1_kind_id2_out"].AsString();
               decimal sp1Rate = dr["sp1_rate"].AsDecimal();
               ls_txt += kindId1Out + "\t" + kindId2Out + "\t" + sp1Rate + Environment.NewLine;
            }

            //fut_S1030(Spread credit)文字檔
            string fileName = _ProgramID + "_fut_S1030(Spread credit)_" + ls_file_grp + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

            bool IsSuccess = ToText(ls_txt , filePath , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!");
               return;
            }
            #endregion

            #region 40180_fut_S1130(Spread credit)_
            //主旨
            string ls_txt2 = "";

            foreach (DataRow dr in dt.Rows) {
               //(1)
               string kindId1Out = dr["spt1_kind_id1_out"].AsString();
               string kindId2Out = dr["spt1_kind_id2_out"].AsString();
               decimal sp1Rate = dr["sp1_rate"].AsDecimal();
               string sp2ValueDate = dr["sp2_value_date"].AsDateTime().ToString("yyyyMMdd");
               string sp2OswGrp = dr["sp2_osw_grp"].AsString();
               string og = wf_conv_osw_grp(sp2OswGrp);
               ls_txt2 += kindId1Out + "\t" + kindId2Out + "\t" + sp1Rate + "\t" + sp2ValueDate + "\t" + og + Environment.NewLine;
            }

            //fut_S1130(Spread credit)文字檔
            string fileName2 = _ProgramID + "_fut_S1130(Spread credit)_" + ls_file_grp + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
            string filePath2 = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName2);

            bool IsSuccess2 = ToText(ls_txt2 , filePath2 , System.Text.Encoding.GetEncoding(950));
            if (!IsSuccess) {
               MessageDisplay.Error("文字檔「" + filePath2 + "」Open檔案錯誤!");
               return;
            }
            #endregion

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }
      #endregion

      #region wf_file_grp
      protected string wf_file_grp(string ls_osw_grp) {

         string ls_file_grp = "";

         if (ls_osw_grp.SubStr(0 , 1) != "%") {
            switch (ls_osw_grp) {
               case "5":
                  ls_file_grp = "g2_";
                  break;
               case "7":
                  ls_file_grp = "g3_";
                  break;
               default:
                  ls_file_grp = "g" + ls_osw_grp + "_";
                  break;
            }
         }
         return ls_file_grp;
      }
      #endregion

      #region wf_conv_osw_grp
      protected string wf_conv_osw_grp(string ls_osw_grp) {
         string is_osw_grp = "";

         switch (ls_osw_grp) {
            case "5":
               is_osw_grp = "2";
               break;
            case "7":
               is_osw_grp = "3";
               break;
            default:
               is_osw_grp = ls_osw_grp;
               break;
         }

         return is_osw_grp;
      }
      #endregion

      /// <summary>
      /// write string to txt
      /// </summary>
      /// <param name="source"></param>
      /// <param name="filePath"></param>
      /// <param name="encoding">System.Text.Encoding.GetEncoding(950)</param>
      /// <returns></returns>
      protected bool ToText(string source , string filePath , System.Text.Encoding encoding) {
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

      private void txtDate_EditValueChanged(object sender , EventArgs e) {
         DevExpress.XtraEditors.TextEdit textEditor = sender as DevExpress.XtraEditors.TextEdit;
         txtStartDate.DateTimeValue = textEditor.EditValue.AsDateTime();

         string startDate = textEditor.EditValue.AsDateTime().ToString("yyyyMMdd");
         string endDate = textEditor.EditValue.AsDateTime().AddDays(30).ToString("yyyyMMdd");
         txtEndDate.DateTimeValue = new MOCF().GetSpecOcfDay(startDate , endDate , 2);
      }

      /// <summary>
      /// set checkbox list focus background color
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gbType_DrawItem(object sender , ListBoxDrawItemEventArgs e) {
         e.AllowDrawSkinBackground = false;
      }

      private void gbMsg_DrawItem(object sender , ListBoxDrawItemEventArgs e) {
         e.AllowDrawSkinBackground = false;
      }

      private void gbMoney_DrawItem(object sender , ListBoxDrawItemEventArgs e) {
         e.AllowDrawSkinBackground = false;
      }

      private void gbSpan_DrawItem(object sender , ListBoxDrawItemEventArgs e) {
         e.AllowDrawSkinBackground = false;
      }

   }
}