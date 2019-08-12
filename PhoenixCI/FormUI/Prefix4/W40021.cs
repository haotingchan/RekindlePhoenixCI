using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/04/30
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 40021 SPAN參數狀況表
   /// </summary>
   public partial class W40021 : FormParent {

      private D40021 dao40021;

      public W40021(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao40021 = new D40021();
      }

      protected override ResultStatus Open() {
         base.Open();

         if (!FlagAdmin) {
            groupAdmin.Visible = false;
            chkTxt.Visible = false;
         } else {
            groupAdmin.Visible = true;
            chkTxt.Visible = true;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         txtDate.DateTimeValue = DateTime.Now;

#if DEBUG
         txtDate.DateTimeValue = DateTime.ParseExact("2018/10/11" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式)";
#endif

         return ResultStatus.Success;
      }

      protected void ShowMsg(string msg) {
         labMsg.Text = msg;
         labMsg.Visible = true;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export() {

         try {

            #region export before
            //130批次作業做完
            string ls_rtn = PbFunc.f_chk_130_wf(_ProgramID , txtDate.DateTimeValue , "1");
            if (!string.IsNullOrEmpty(ls_rtn.Trim())) {
               DialogResult liRtn = MessageDisplay.Choose(string.Format("{0}-{1}，是否要繼續?" , txtDate.Text , ls_rtn) , MessageBoxDefaultButton.Button2 , GlobalInfo.QuestionText);
               if (liRtn == DialogResult.No) {
                  labMsg.Visible = false;
                  Cursor.Current = Cursors.Arrow;
                  return ResultStatus.Fail;
               }
            }
            #endregion

            //0. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //1. copy template xls to target path
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            string ls_logf = "N"; //LOGF記錄每項時間

            if (txtDate.DateTimeValue < DateTime.ParseExact("2010/10/01" , "yyyy/MM/dd" , null)) {
               MessageDisplay.Info("自99年10月1日起，各期貨契約之報酬率改以「當日結算價」及「當日開盤參考價」計算，故產出資料值不會異動至資料庫!" , GlobalInfo.WarningText);
               return ResultStatus.Fail;
            }

            //2.填資料
            //Sheet:標的現貨收盤價&開盤參考價
            bool result1 = false, result2 = false;

            //資料儲存至Table
            result1 = wf_40021(workbook);
            //if (chkTxt.CheckState == CheckState.Checked) {
            //   result1 = wf_40021(workbook);
            //   if (ls_logf == "Y") {
            //      //wf_logt("40021");

            //      //is_log_time = is_log_time + " - " + string(now())
            //      //f_write_logf(is_txn_id , 'T' , txd_id + ',' + is_log_time)
            //      //is_log_time = string(now())
            //   }
            //}

            //Sheet:Span參數日狀況表(一)(二)(三)
            result2 = wf_40020_7(workbook);

            if (!result1 && !result2) {
               try {
                  workbook = null;
                  File.Delete(excelDestinationPath);
               } catch (Exception) {
                  //
               }
               return ResultStatus.Fail;
            }

            //存檔
            workbook.SaveDocument(excelDestinationPath);
            labMsg.Visible = false;

            if (FlagAdmin)
               System.Diagnostics.Process.Start(excelDestinationPath);

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

      #region wf_40021
      private bool wf_40021(Workbook workbook) {

         try {
            string rptId = "40020_9";
            ShowMsg("轉檔中...");

            DataTable dtContent = dao40021.GetData(txtDate.DateTimeValue , "1%" , rptId);
            if (dtContent.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!" , txtDate.Text , rptId , _ProgramName) , GlobalInfo.ResultText);
               return false;
            }

            int li_sheet = 0, li_sheet_l = 0;
            Worksheet worksheet = workbook.Worksheets[li_sheet];
            foreach (DataRow dr in dtContent.Rows) {
               li_sheet = dr["rpt_level_1"].AsInt() - 1;
               if (li_sheet != li_sheet_l) {
                  li_sheet_l = li_sheet;
                  worksheet = workbook.Worksheets[li_sheet_l];
                  worksheet.Range["A1"].Select();
               }

               int li_ole_col = dr["rpt_level_2"].AsInt() - 1;
               int li_ole_row = dr["rpt_level_3"].AsInt() - 1;
               int li_col = dr["rpt_value_2"].AsInt() - 1;//DataColumn.Index要從0開始算
               //string ls_kind_id = dr["rpt_value"].AsString();
               //string ls_kind_id2 = dr["rpt_value_3"].AsString();
               //string ls_type = dr["rpt_value_4"].AsString();
               worksheet.Cells[li_ole_row , li_ole_col].Value = dr[li_col].AsDecimal();
            }

            string chineseDate = txtDate.Text.SubStr(0 , 4) + "年" + txtDate.Text.SubStr(5 , 2) + "月" + txtDate.Text.SubStr(8 , 2) + "日";
            string dateMsg = string.Format("資料日期：{0}" , chineseDate);

            Worksheet ws1 = workbook.Worksheets[0];
            Worksheet ws2 = workbook.Worksheets[1];
            Worksheet ws3 = workbook.Worksheets[2];
            ws1.Cells[1 , 3].Value = dateMsg;
            ws2.Cells[0 , 5].Value = dateMsg;
            ws3.Cells[0 , 7].Value = dateMsg;

            //servername使用
            if (chkTxt.CheckState == CheckState.Checked) {
               //write txt
               string etfFileName = "SP1.txt";
               etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
               ExportOptions txtref = new ExportOptions();
               txtref.HasHeader = false;
               txtref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
               Common.Helper.ExportHelper.ToText(dtContent , etfFileName , txtref);
            }

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }

      }
      #endregion

      #region wf_40020_7
      private bool wf_40020_7(Workbook workbook) {

         string rptName = "作業事項";
         string rptId = "40020_7";
         labMsg.Text = rptId + "－" + rptName + " 轉檔中...";

         try {

            //風險價格係數
            DataTable dtRisk = dao40021.GetRiskData(txtDate.DateTimeValue , "1%");
            if (dtRisk.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取「變動幅度」無任何資料!" , txtDate.Text , rptId , rptName) , GlobalInfo.ResultText);
               return false;
            }

            #region Span參數日狀況表(一)
            DataTable dtSV = dtRisk.Filter("sp1_type='SV'");
            string ls_str = "";
            string ls_str2 = "";
            foreach (DataRow dr in dtSV.Rows) {
               string sp1Flag = dr["sp1_flag"].AsString();
               if (sp1Flag == "N") {
                  ls_str += "■";
                  ls_str2 += "□";
               } else {
                  ls_str += "□";
                  ls_str2 += "■";
               }

               string id1Out = dr["sp1_kind_id1_out"].AsString();
               ls_str += id1Out + "　";
               ls_str2 += id1Out + "　";
            }

            DataTable dtTemp1 = dao40021.GetRowColNum1();
            Worksheet ws1 = workbook.Worksheets[0];
            int ii_ole_row = dtTemp1.Rows[0]["ii_ole_row"].AsInt() - 1;
            int li_row = dtTemp1.Rows[0]["li_row"].AsInt() - 1;
            int li_col = dtTemp1.Rows[0]["li_col"].AsInt() - 1;
            ws1.Cells[ii_ole_row , li_col].Value = ls_str;
            ws1.Cells[li_row , li_col].Value = ls_str2;

            ws1.Range["A1"].Select();
            #endregion

            #region Span參數日狀況表(二)
            DataTable dtSD = dtRisk.Filter("sp1_type='SD'");
            ls_str2 = "";
            foreach (DataRow dr in dtSD.Rows) {
               string sp1Flag = dr["sp1_flag"].AsString();
               string id1Out = dr["sp1_kind_id1_out"].AsString().SubStr(0 , 2);
               string id2Out = dr["sp1_kind_id2_out"].AsString().SubStr(0 , 2);
               if (sp1Flag == "Y") {
                  if (!string.IsNullOrEmpty(ls_str2)) {
                     ls_str2 += "；";
                  }
                  ls_str2 += id1Out + "vs" + id2Out;
               }
            }

            if (string.IsNullOrEmpty(ls_str2))
               ls_str2 = "無";

            DataTable dtTemp2 = dao40021.GetRowColNum2();
            Worksheet ws2 = workbook.Worksheets[1];
            int ii_ole_row2 = dtTemp2.Rows[0]["ii_ole_row"].AsInt() - 1;
            int li_col2 = dtTemp2.Rows[0]["li_col"].AsInt() - 1;

            if (ls_str2 == "無") {
               //刪除"1,2"說明文字
               for (int w = 1 ; w <= 5 ; w++) {
                  ws2.Cells[ii_ole_row2 + w , 0].Value = "";
                  ws2.Cells[ii_ole_row2 + w , 1].Value = "";
               }
            } else {
               //刪除"無契約變動"說明文字
               Range ra = ws2.Rows[ii_ole_row2];
               ws2.DeleteCells(ra , DeleteMode.EntireRow);
               ws2.Range["A1"].Select();

               //寫調整契約
               ws2.Cells[ii_ole_row2 , li_col2].Value = ls_str2;
            }
            #endregion

            #region Span參數日狀況表(三)
            DataTable dtSS = dtRisk.Filter("sp1_type='SS'");
            ls_str2 = "";

            foreach (DataRow dr in dtSS.Rows) {
               string sp1Flag = dr["sp1_flag"].AsString();
               string id1Out = dr["sp1_kind_id1_out"].AsString().SubStr(0 , 2);
               string id2Out = dr["sp1_kind_id2_out"].AsString().SubStr(0 , 2);
               if (sp1Flag == "Y") {
                  if (!string.IsNullOrEmpty(ls_str2)) {
                     ls_str2 += "；";
                  }
                  ls_str2 += id1Out + "vs" + id2Out;
               }
            }

            if (string.IsNullOrEmpty(ls_str2))
               ls_str2 = "無";

            DataTable dtTemp3 = dao40021.GetRowColNum3();
            Worksheet ws3 = workbook.Worksheets[2];
            int ii_ole_row3 = dtTemp3.Rows[0]["ii_ole_row"].AsInt() - 1;
            int li_col3 = dtTemp3.Rows[0]["li_col"].AsInt() - 1;

            if (ls_str2 == "無") {
               //刪除"1,2"說明文字
               for (int w = 1 ; w <= 5 ; w++) {
                  ws3.Cells[ii_ole_row3 + w , 0].Value = "";
                  ws3.Cells[ii_ole_row3 + w , 1].Value = "";
               }
            } else {
               //刪除"無契約變動"說明文字
               //ws3.Rows.Remove(ii_ole_row3);
               Range ra = ws3.Rows[ii_ole_row3];
               ra.Delete();
               ws3.Range["A1"].Select();

               //寫調整契約
               ws3.Cells[ii_ole_row3 , li_col3].Value = ls_str2;
            }
            #endregion

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }
      }
      #endregion
   }
}