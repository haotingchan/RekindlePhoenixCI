using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;
using System.Threading;
using DataObjects.Dao.Together;
using Common;
using DevExpress.Spreadsheet;
using System.IO;
using DataObjects.Dao.Together.SpecificDao;
using Common.Helper;

/// <summary>
/// Lukas, 2019/4/9
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30053 每日商品交易行情
   /// </summary>
   public partial class W30053 : FormParent {

      private OCFG daoOCFG;
      private D30053 dao30053;
      private string date;

      public W30053(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         dao30053 = new D30053();
      }

      protected override ResultStatus Open() {
         base.Open();
         //日期
         txtSDate.DateTimeValue = GlobalInfo.OCF_DATE;
#if DEBUG
         txtSDate.Text = "2018/10/15";
#endif
         //盤別下拉選單
         List<LookupItem> ddlb_grp = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "1", DisplayMember = "16:15收盤"},
                                        new LookupItem() { ValueMember = "2", DisplayMember = "全部收盤" }};
         Extension.SetDataTable(ddlType , ddlb_grp , "ValueMember" , "DisplayMember" , TextEditStyles.DisableTextEditor , "");

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         try {
            //決定盤別下拉選單
            daoOCFG = new OCFG();
            if (daoOCFG.f_get_txn_osw_grp(_ProgramID) == "5") {
               ddlType.EditValue = "1";
            } else {
               ddlType.EditValue = "2";
            }

         } catch (Exception ex) {
            throw ex;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = false;//列印

         return ResultStatus.Success;
      }

      protected void ShowMsg(string msg) {
         lblProcessing.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export() {
         try {
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);
            lblProcessing.Visible = true;
            ShowMsg("開始轉檔...");
            string rptId, file, rptName = "";
            date = txtSDate.DateTimeValue.Year + "年" + txtSDate.DateTimeValue.Month + "月" + txtSDate.DateTimeValue.Day + "日";

            #region ue_export_before
            //判斷盤別
            int rtnInt, seq;
            string rtnStr, grp;
            if (ddlType.Text == "16:15收盤") {
               grp = "1";
            } else {
               grp = "2";
            }

            //判斷統計資料轉檔已完成
            for (int f = 1 ; f <= 2 ; f++) {
               if (grp == "1") {
                  if (f == 1) {
                     seq = 13;
                  } else {
                     seq = 23;
                  }
               } else {
                  seq = 17;
                  f = 2;
               }
               //check JSW
               rtnStr = PbFunc.f_get_jsw_seq(_ProgramID , "E" , seq , txtSDate.DateTimeValue , "0");
               if (rtnStr != "") {
                  DialogResult result = MessageDisplay.Choose(" 統計資料未轉入完畢,是否要繼續?");
                  if (result == DialogResult.No) {
                     lblProcessing.Visible = false;
                     return ResultStatus.Fail;
                  }
               }
            }
            #endregion

            rptId = "30053";

            //複製檔案
            file = wfCopy30053(rptId , grp);
            if (file == "") return ResultStatus.Fail;

            //開啟檔案
            Workbook workbook = new Workbook();
            workbook.LoadDocument(file);

            //切換Sheet
            Worksheet ws = workbook.Worksheets["期貨"];

            #region 11張報表

            int rowIndex = 1;
            //1.期貨
            if (!wf30053f(grp , rowIndex , ws)) return showEmailMsg(cbxNews.Checked);

            //2.選擇權
            rowIndex = rowIndex + 3;
            ws = workbook.Worksheets["選擇權"];
            if (!wf30053o(rowIndex , ws)) return showEmailMsg(cbxNews.Checked);

            //3.股票選擇權
            rowIndex = rowIndex + 3;
            ws = workbook.Worksheets["股票選擇權"];
            if (!wf30053stc(rowIndex , ws)) return showEmailMsg(cbxNews.Checked);

            //4.股票期貨(For工商時報)
            //5.股票期貨(For工商時報)50
            rowIndex = rowIndex + 3;
            ws = workbook.Worksheets["股票期貨(For工商時報)"];
            if (!wf30053stfNear(ws)) return showEmailMsg(cbxNews.Checked);
            ws = workbook.Worksheets["股票期貨(For工商時報) 50大"];
            if (!wfCtee50(ws)) return showEmailMsg(cbxNews.Checked);

            //6.股票期貨Top10檔(For聯合晚報)
            ws = workbook.Worksheets["股票期貨Top10檔(For經濟日報)"];
            if (!wf30053stfTop10(ws)) return showEmailMsg(cbxNews.Checked);

            //7.ETF期貨Top10檔
            ws = workbook.Worksheets["ETF期貨Top10檔"];
            if (!wf30053etfTop10(ws)) return showEmailMsg(cbxNews.Checked);

            //8.股票選擇權TOP10檔(For聯合晚報)
            ws = workbook.Worksheets["股票選擇權TOP10檔(For聯合晚報)"];
            if (!wf30053stcTop10(ws)) return showEmailMsg(cbxNews.Checked);

            //9.ETF選擇權前20大行情表
            ws = workbook.Worksheets["ETF選擇權TOP20檔"];
            if (!wf30053etcTop20(ws)) return showEmailMsg(cbxNews.Checked);

            //10.美元兌人民幣選擇權前20大行情表
            ws = workbook.Worksheets["美元兌人民幣選擇權(RHO)TOP20檔"];
            if (!wf30053rhoTop20(ws)) return showEmailMsg(cbxNews.Checked);

            //11.小型美元兌人民幣選擇權前20大行情表
            ws = workbook.Worksheets["小型美元兌人民幣選擇權(RTO)TOP20檔"];
            if (!wf30053rtoTop20(ws)) return showEmailMsg(cbxNews.Checked);

            #endregion

            //存檔
            workbook.SaveDocument(file);
            ShowMsg("轉檔完成");

            //email
            rptId = "30053_" + grp;
            if (cbxNews.Checked) {
               DataTable dtTxemail = new TXEMAIL().ListData(rptId , 1);

               if (dtTxemail.Rows.Count != 0) {
                  string TXEMAIL_SENDER = dtTxemail.Rows[0]["TXEMAIL_SENDER"].AsString();
                  string TXEMAIL_RECIPIENTS = dtTxemail.Rows[0]["TXEMAIL_RECIPIENTS"].AsString();
                  string TXEMAIL_CC = dtTxemail.Rows[0]["TXEMAIL_CC"].AsString();
                  string TXEMAIL_TITLE = dtTxemail.Rows[0]["TXEMAIL_TITLE"].AsString();
                  string TXEMAIL_TEXT = dtTxemail.Rows[0]["TXEMAIL_TEXT"].AsString();

                  TXEMAIL_TITLE = txtSDate.DateTimeValue.ToString("yyyyMMdd") + TXEMAIL_TITLE;
                  MailHelper.SendEmail(TXEMAIL_SENDER , TXEMAIL_RECIPIENTS , TXEMAIL_CC , TXEMAIL_TITLE , TXEMAIL_TEXT , file);
               }
            }
         } catch (Exception ex) {
            MessageDisplay.Error("輸出錯誤");
            throw ex;
         } finally {
            this.Cursor = Cursors.Arrow;
            this.Refresh();
            Thread.Sleep(5);
         }
         return ResultStatus.Success;
      }


      /// <summary>
      /// 這支功能PB覆寫公用的wf_copyfile
      /// </summary>
      /// <param name="fileName"></param>
      /// <param name="grp"></param>
      /// <returns></returns>
      private string wfCopy30053(string fileName , string grp) {

         string template = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH , fileName + ".xls");
         if (!File.Exists(template)) {
            MessageDisplay.Error("無此檔案「" + template + "」!");
            return "";
         }
         string lsFilename;
         lsFilename = txtSDate.Text.Replace("/" , "") + "股票期貨暨其他商品行情表";
         if (grp == "1") {
            lsFilename = lsFilename + "(16時15分收盤)" + ".xls";
         } else {
            lsFilename = lsFilename + "(全部收盤)" + ".xls";
         }
         bool lbChk;
         string file = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , lsFilename);
         lbChk = File.Exists(file);
         if (lbChk) {
            File.Move(file , file + "_bak_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".xls");
         }
         try {
            File.Copy(template , file , false);
         } catch {
            MessageDisplay.Error("複製「" + template + "」到「" + file + "」檔案錯誤!");
            return "";
         }
         lsFilename = file;
         return lsFilename;
      }

      /// <summary>
      /// 期貨商品行情表
      /// </summary>
      /// <param name="rowIndex"></param>
      /// <param name="ws30011"></param>
      protected bool wf30053f(string grp , int rowIndex , Worksheet ws) {
         string rptName = "期貨商品行情表", rptId = "30053", etfFileName;
         int col, delRow, row;
         Range delRange;

         ShowMsg(rptId + '－' + rptName + " 轉檔中...");

         //讀取資料
         DataTable dt30053f = dao30053.d_30053_f(txtSDate.DateTimeValue);
         if (dt30053f.Rows.Count == 0) {
            MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
            lblProcessing.Visible = false;
            return false;
         }

         //把table存成txt(先拿掉)
         //etfFileName = "d:\temp\a.txt";
         //ExportOptions txtref = new ExportOptions();
         //txtref.HasHeader = false;
         //txtref.Encoding = Encoding.GetEncoding(950);//ASCII
         //Common.Helper.ExportHelper.ToText(dt30053f, etfFileName, txtref);

         //填資料
         ws.Cells[0 , 7].Value = date;
         delRow = 0;
         foreach (DataRow dr in dt30053f.Rows) {
            rowIndex = dr["RPT_SEQ_NO"].AsInt() + dr["SEQ_NO"].AsInt() - 1 - 1;
            //刪除列:無短天期
            if (dr["AMIF_KIND_ID"] == DBNull.Value) {
               row = dr["RPT_DEL_ROW"].AsInt();
               if (row > 0) {
                  delRow = delRow + row;
                  delRange = ws.Range[(rowIndex + 1).ToString() + ":" + (rowIndex + row).ToString()];
                  delRange.Delete(DeleteMode.EntireRow);
                  //ws.Rows.Hide(ii_ole_row, ii_ole_row + li_row - 1);
                  continue;
               }
            }
            for (col = 1 ; col < 10 ; col++) {
               ws.Cells[rowIndex - delRow , col].SetValue(dr[col]);
            }
         }
         //刪除空白列
         rowIndex = dao30053.get30053fRow();
         //當盤別為1的時候，刪除印度50的資料(尚未收盤)
         if (grp == "1") {
            delRange = ws.Range[(rowIndex - delRow).ToString() + ":" + ((rowIndex + 1) - delRow).ToString()];
            delRange.Delete(DeleteMode.EntireRow);
            //ws.Rows.Hide(ii_ole_row - li_del_row, (ii_ole_row + 1) - li_del_row);
         }
         ws.ScrollToRow(0);
         return true;
      }

      /// <summary>
      /// 2表(選擇權)
      /// </summary>
      /// <param name="grp"></param>
      /// <param name="rowIndex"></param>
      /// <param name="ws30053o"></param>
      protected bool wf30053o(int rowIndex , Worksheet ws) {
         string rptName = "2表", rptId = "30053";
         int col, delRow, row;
         Range delRange;

         ShowMsg(rptId + '－' + rptName + " 轉檔中...");
         //讀取資料
         DataTable dt30053o = dao30053.d_30053_o(txtSDate.DateTimeValue);
         if (dt30053o.Rows.Count == 0) {
            MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
            lblProcessing.Visible = false;
            return false;
         }

         //填資料
         ws.Cells[0 , 9].Value = date;
         delRow = 0;
         foreach (DataRow dr in dt30053o.Rows) {
            rowIndex = dr["RPT_SEQ_NO"].AsInt() + dr["SEQ_NO"].AsInt() - 1 - 1;
            //刪除列:無短天期
            if (dr["AMIF_KIND_ID"] == DBNull.Value) {
               row = dr["RPT_DEL_ROW"].AsInt();
               if (row > 0) {
                  delRow = dr["RPT_DEL_ROW"].AsInt();
                  string tmp = ws.Cells[rowIndex + 1 , 0].Value.AsString();
                  delRange = ws.Range[(rowIndex + 1).ToString() + ":" + (rowIndex + row).ToString()];
                  delRange.Delete(DeleteMode.EntireRow);
                  ws.Cells[rowIndex + row + 1 , 0].Value = tmp;
                  continue;
               }
            }
            for (col = 1 ; col < 12 ; col++) {
               ws.Cells[rowIndex - delRow , col].SetValue(dr[col]);
            }
         }
         ws.ScrollToRow(0);
         return true;
      }

      /// <summary>
      /// 3表(股票選擇權)
      /// </summary>
      /// <param name="rowIndex"></param>
      /// <param name="ws30053stc"></param>
      protected bool wf30053stc(int rowIndex , Worksheet ws) {
         string rptName = "3表", rptId = "30053";
         int totalRow = 0;
         Range range;

         ShowMsg(rptId + '－' + rptName + " 轉檔中...");

         //讀取資料
         DataTable dt30053stc;
         int cnt = dao30053.checkSTCdata();
         if (cnt > 0) {
            dt30053stc = dao30053.d_30053_c(txtSDate.Text , "O");
            if (dt30053stc.Rows.Count == 0) {
               MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
               lblProcessing.Visible = false;
               return false;
            }
         } else {
            dt30053stc = dao30053.d_30053_c_sto(txtSDate.Text , "O");
            if (dt30053stc.Rows.Count == 0) {
               MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
               lblProcessing.Visible = false;
               return false;
            }
         }

         //填資料
         ws.Cells[0 , 9].Value = date;
         int row, col;
         DataView dv = dt30053stc.AsDataView();
         dv.Sort = "C1 Desc , C2 Asc , C4 Desc , C3 Asc ";
         DataTable dtReSorted = dv.ToTable();
         for (row = 0 ; row < dtReSorted.Rows.Count ; row++) {
            for (col = 0 ; col < 12 ; col++) {
               ws.Cells[2 + row , col].SetValue(dtReSorted.Rows[row][col]);
            }
         }

         //合併cell
         int p, start = 3 - 1, end = 3 - 1;
         string prod_name = "";
         ws.Cells[32 , 0].Value = "  ";
         prod_name = ws.Cells[2 , 0].Value.AsString();
         for (p = 3 ; p < 33 ; p++) {
            end = p - 1;
            if (prod_name != ws.Cells[p , 0].Value.AsString()) {
               range = ws.Range["A" + (start + 1).ToString() + ":A" + (end + 1).ToString()];
               range.Merge();
               start = p;
            }
            prod_name = ws.Cells[start , 0].Value.AsString();
         }

         //刪除空白列
         if (totalRow > rowIndex) {
            range = ws.Range[(rowIndex + 1).ToString() + ":" + totalRow.ToString()];
            range.Delete(DeleteMode.EntireRow);
         }
         ws.ScrollToRow(0);
         return true;
      }

      /// <summary>
      /// 股票期貨(For工商時報)
      /// </summary>
      /// <param name="ws30053stfNear"></param>
      protected bool wf30053stfNear(Worksheet ws) {
         string rptName = "股票期貨(For工商時報)", rptId = "30053";

         ShowMsg(rptId + '－' + rptName + " 轉檔中...");

         //讀取資料
         DataTable dt30053stfNear = dao30053.d_30053_c_stf_near40(txtSDate.Text , "F" , "40");
         if (dt30053stfNear.Rows.Count == 0) {
            MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
            lblProcessing.Visible = false;
            return false;
         }
         //填資料
         ws.Cells[0 , 9].Value = date;
         ws.Import(dt30053stfNear , false , 2 , 0);
         ws.ScrollToRow(0);
         return true;
      }

      /// <summary>
      /// 股票期貨(For工商時報) 50大
      /// </summary>
      /// <param name="ws30053ctee50"></param>
      protected bool wfCtee50(Worksheet ws) {
         string rptName = "股票期貨(For工商時報) 50大", rptId = "30053";

         ShowMsg(rptId + '－' + rptName + " 轉檔中...");

         //讀取資料
         DataTable dt30053ctee50 = dao30053.d_30053_c_stf_near40(txtSDate.Text , "F" , "100");
         if (dt30053ctee50.Rows.Count == 0) {
            MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
            lblProcessing.Visible = false;
            return false;
         }
         //填資料
         ws.Cells[0 , 9].Value = date;
         ws.Import(dt30053ctee50 , false , 2 , 0);
         ws.ScrollToRow(0);
         return true;
      }

      /// <summary>
      /// 股票期貨Top10檔(For經濟日報)
      /// </summary>
      /// <param name="ws"></param>
      protected bool wf30053stfTop10(Worksheet ws) {
         string rptName = "股票期貨Top10檔_經濟", rptId = "30053";

         ShowMsg(rptId + '－' + rptName + " 轉檔中...");

         //讀取資料
         DataTable dt30053StfTop40 = dao30053.d_30053_c_stf_top40(txtSDate.Text);
         if (dt30053StfTop40.Rows.Count == 0) {
            MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
            lblProcessing.Visible = false;
            return false;
         }
         //填資料
         ws.Cells[0 , 4].Value = date;
         ws.Import(dt30053StfTop40 , false , 2 , 0);
         ws.ScrollToRow(0);
         return true;
      }

      /// <summary>
      /// ETF期貨前10大行情表
      /// </summary>
      /// <param name="ws"></param>
      protected bool wf30053etfTop10(Worksheet ws) {
         string rptName = "ETF期貨前10大行情表", rptId = "30053";

         ShowMsg(rptId + '－' + rptName + " 轉檔中...");

         //讀取資料
         DataTable dt30053EtfTop10 = dao30053.d_30053_c_etf_top10(txtSDate.Text);
         if (dt30053EtfTop10.Rows.Count == 0) {
            MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
            lblProcessing.Visible = false;
            return false;
         }
         //填資料
         //ws.Cells[0, 5].Value = date;
         ws.Import(dt30053EtfTop10 , false , 2 , 0);
         ws.ScrollToRow(0);
         return true;
      }

      /// <summary>
      /// 股票期貨Top10檔_聯晚
      /// </summary>
      /// <param name="ws"></param>
      protected bool wf30053stcTop10(Worksheet ws) {
         string rptName = "股票期貨Top10檔_聯晚", rptId = "30053";

         ShowMsg(rptId + '－' + rptName + " 轉檔中...");

         //讀取資料
         DataTable dt30053StcTop10 = dao30053.d_30053_c_stc_top10(txtSDate.Text);
         if (dt30053StcTop10.Rows.Count == 0) {
            MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
            lblProcessing.Visible = false;
            return false;
         }
         //填資料
         ws.Cells[0 , 5].Value = date;
         ws.Import(dt30053StcTop10 , false , 2 , 0);
         ws.ScrollToRow(0);
         return true;
      }

      /// <summary>
      /// ETF選擇權前20大行情表
      /// </summary>
      /// <param name="ws"></param>
      protected bool wf30053etcTop20(Worksheet ws) {
         string rptName = "ETF選擇權前20大行情表", rptId = "30053";

         ShowMsg(rptId + '－' + rptName + " 轉檔中...");

         //讀取資料
         DataTable dt30053EtcTop20 = dao30053.d_30053_c_etc_top20(txtSDate.Text);
         if (dt30053EtcTop20.Rows.Count == 0) {
            MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
            lblProcessing.Visible = false;
            return false;
         }
         //填資料
         ws.Cells[0 , 5].Value = date;
         ws.Import(dt30053EtcTop20 , false , 2 , 0);
         ws.ScrollToRow(0);
         return true;
      }

      /// <summary>
      /// 美元兌人民幣選擇權(RHO)20大行情表
      /// </summary>
      /// <param name="ws"></param>
      protected bool wf30053rhoTop20(Worksheet ws) {
         string rptName = "美元兌人民幣選擇權(RHO)20大行情表", rptId = "30053";

         ShowMsg(rptId + '－' + rptName + " 轉檔中...");

         //讀取資料
         DataTable dt30053RhoTop20 = dao30053.d_30053_c_top20(txtSDate.Text , "RHO");
         if (dt30053RhoTop20.Rows.Count == 0) {
            MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
            lblProcessing.Visible = false;
            return false;
         }
         //填資料
         ws.Cells[0 , 5].Value = date;
         ws.Import(dt30053RhoTop20 , false , 2 , 0);
         ws.ScrollToRow(0);
         return true;
      }

      /// <summary>
      /// 小型美元兌人民幣選擇權(RTO)20大行情表
      /// </summary>
      /// <param name="ws"></param>
      protected bool wf30053rtoTop20(Worksheet ws) {
         string rptName = "小型美元兌人民幣選擇權(RTO)20大行情表", rptId = "30053";

         ShowMsg(rptId + '－' + rptName + " 轉檔中...");

         //讀取資料
         DataTable dt30053RtoTop20 = dao30053.d_30053_c_top20(txtSDate.Text , "RTO");
         if (dt30053RtoTop20.Rows.Count == 0) {
            MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
            lblProcessing.Visible = false;
            return false;
         }
         //填資料
         ws.Cells[0 , 5].Value = date;
         ws.Import(dt30053RtoTop20 , false , 2 , 0);
         ws.ScrollToRow(0);
         return true;
      }

      /// <summary>
      /// 只要發生錯誤,而且有勾選email news,則show message box
      /// </summary>
      /// <param name="sendEmail"></param>
      protected ResultStatus showEmailMsg(bool sendEmail) {
         if (sendEmail)
            MessageDisplay.Warning("產出檔案有異常資訊，請通知系統負責人！");

         return ResultStatus.Fail;
      }
   }
}