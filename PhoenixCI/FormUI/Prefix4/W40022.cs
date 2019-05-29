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
/// Winni, 2019/05/03
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 40022 延長交易商品SPAN參數狀況表
   /// </summary>
   public partial class W40022 : FormParent {

      private D40021 dao40021; //dw共用

      public W40022(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao40021 = new D40021();
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         txtDate.DateTimeValue = DateTime.Now;

#if DEBUG
         txtDate.DateTimeValue = DateTime.ParseExact("2018/10/11" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式)";
#endif

         if (FlagAdmin) {
            groupAdmin.Visible = true;
            chkTxt.Visible = true;
            //chk_40023_data.Visible = true; //好像沒用到
         }

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
            string ls_rtn = PbFunc.f_chk_130_wf(_ProgramID , txtDate.DateTimeValue , "5");
            if (!string.IsNullOrEmpty(ls_rtn.Trim())) {
               DialogResult liRtn = MessageDisplay.Choose(string.Format("{0}-{1}，是否要繼續?" , txtDate.Text , ls_rtn));
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

            if (ls_logf == "Y") {
               //wf_logt("40022");
            }

            if (txtDate.DateTimeValue < DateTime.ParseExact("2010/10/01" , "yyyy/MM/dd" , null)) {
               MessageDisplay.Warning("自99年10月1日起，各期貨契約之報酬率改以「當日結算價」及「當日開盤參考價」計算，故產出資料值不會異動至資料庫!");
               return ResultStatus.Fail;
            }

            //2.填資料
            //Sheet:標的現貨收盤價&開盤參考價
            bool result1 = false, result2 = false;
            result1 = wf_40022_9(workbook);
            if (ls_logf == "Y") {
               //wf_logt("40022_9")
            }

            //Sheet:Span參數日狀況表(一)(二)(三)
            result2 = wf_40022_7(workbook);
            if (ls_logf == "Y") {
               //wf_logt("40022_7")
            }

            if (!result1 && !result2) {
               try {
                  workbook = null;
                  System.IO.File.Delete(excelDestinationPath);
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

      #region wf_40022_9
      private bool wf_40022_9(Workbook workbook) {

         try {
            string rptId = "40022_9";
            ShowMsg("轉檔中...");

            DataTable dtContent = dao40021.GetData(txtDate.DateTimeValue , "5%" , rptId);
            if (dtContent.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},無任何資料!" , txtDate.Text , rptId , _ProgramName));
               return false;
            }

            Worksheet ws = workbook.Worksheets[0];
            ws.Range["A1"].Select();

            string chineseDate = txtDate.Text.SubStr(0 , 4) + "年" + txtDate.Text.SubStr(5 , 2) + "月" + txtDate.Text.SubStr(8 , 2) + "日";
            string dateMsg = string.Format("資料日期：{0}" , chineseDate);
            ws.Cells[0 , 6].Value = dateMsg;

            foreach (DataRow dr in dtContent.Rows) {

               int li_ole_col = dr["rpt_level_2"].AsInt() - 1;
               int li_ole_row = dr["rpt_level_3"].AsInt() - 1;
               int li_col = dr["rpt_value_2"].AsInt() - 1; //DataColumn.Index要從0開始算
               string ls_kind_id = dr["rpt_value"].AsString();
               string ls_kind_id2 = dr["rpt_value_3"].AsString();
               string ls_type = dr["rpt_value_4"].AsString();
               ws.Cells[li_ole_row , li_ole_col].Value = dr[li_col].AsDecimal();
            }

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

      #region wf_40022_7
      private bool wf_40022_7(Workbook workbook) {

         string rptName = "作業事項";
         string rptId = "40022_7";
         labMsg.Text = rptId + "－" + rptName + " 轉檔中...";

         try {

            //風險價格係數
            DataTable dtRisk = dao40021.GetRiskData(txtDate.DateTimeValue , "5%");
            if (dtRisk.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1}－{2},讀取「變動幅度」無任何資料!" , txtDate.Text , rptId , rptName));
               return false;
            }

            Worksheet ws = workbook.Worksheets[0];

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

            DataTable dtTemp1 = dao40021.GetRowColNum1("40022" , "40022_5e");
            int ii_ole_row = dtTemp1.Rows[0]["ii_ole_row"].AsInt() - 1;
            int li_row = dtTemp1.Rows[0]["li_row"].AsInt() - 1;
            int li_col = dtTemp1.Rows[0]["li_col"].AsInt() - 1;
            ws.Cells[ii_ole_row , li_col].Value = ls_str;
            ws.Cells[li_row , li_col].Value = ls_str2;

            ws.Range["A1"].Select();
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
                     ls_str2 += ";";
                  }
                  ls_str2 += id1Out + "vs" + id2Out;
               }
            }

            DataTable dtTemp3 = dao40021.GetRowColNum3("40022" , "40022_6e");
            int ii_ole_row3 = dtTemp3.Rows[0]["ii_ole_row"].AsInt() - 2;

            if (ls_str2 == " ") {
               Range ra = ws.Rows[ii_ole_row3];
               ra.Delete(DeleteMode.EntireRow);
               ws.Range["A1"].Select();
            } else {
               Range ra = ws.Rows[ii_ole_row3 + 1];
               ra.Delete(DeleteMode.EntireRow);
               ws.Range["A1"].Select();
            }
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
                     ls_str2 += ";";
                  }
                  ls_str2 += id1Out + "vs" + id2Out;
               }
            }

            DataTable dtTemp2 = dao40021.GetRowColNum2("40022" , "40022_6e");
            int ii_ole_row2 = dtTemp2.Rows[0]["ii_ole_row"].AsInt() - 1;
            int li_col2 = dtTemp2.Rows[0]["li_col"].AsInt() - 1;

            if (string.IsNullOrEmpty(ls_str2)) {
               Range ra = ws.Rows[ii_ole_row2];
               ra.Delete(DeleteMode.EntireRow);
               ws.Range["A1"].Select();
            } else {
               Range ra = ws.Rows[ii_ole_row2 - 1];
               ra.Delete(DeleteMode.EntireRow);
               ws.Range["A1"].Select();
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