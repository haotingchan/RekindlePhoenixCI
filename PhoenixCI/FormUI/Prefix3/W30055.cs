using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using Common.Helper;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;

/// <summary>
/// ken 2019/4/2
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 特製行情表(經濟工商)
   /// </summary>
   public partial class W30055 : FormParent {
      protected DataTable dtMsg;
      protected D30055 dao30055;
      protected AMIF amif;
      protected AI2 ai2;

      public W30055(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         GridHelper.SetCommonGrid(gvMsg);
         gcMsg.Visible = false;
         gvMsg.OptionsBehavior.Editable = false;
         gvMsg.OptionsBehavior.AutoPopulateColumns = true;
         gvMsg.OptionsView.ColumnAutoWidth = true;

         dtMsg = new DataTable("ProcessMessage");
         dtMsg.Columns.Add(new DataColumn("SheetName"));
         dtMsg.Columns.Add(new DataColumn("SheetSubTitle"));
         dtMsg.Columns.Add(new DataColumn("SubMsg"));
         dtMsg.Columns.Add(new DataColumn("Msg"));

         dao30055 = new D30055();
         amif = new AMIF();
         ai2 = new AI2();
      }

      protected override ResultStatus Open() {
         base.Open();
         txtSDate.DateTimeValue = GlobalInfo.OCF_DATE;

         //盤別下拉選單
         //List<LookupItem> lstType = new List<LookupItem>(){
         //                               new LookupItem() { ValueMember = "1", DisplayMember = "16:15收盤"},
         //                               new LookupItem() { ValueMember = "2", DisplayMember = "全部收盤" }};
         DataTable lstType = new CODW().ListLookUpEdit("GRP" , "GRP_NO");
         Extension.SetDataTable(ddlType , lstType , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , "");
         ddlType.ItemIndex = 1;

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();

         //比對現在時間,如果小於晚上6: 15(時段7),則回傳"5", 否則回傳"%"
         if (PbFunc.f_get_txn_osw_grp() == "5") {
            ddlType.ItemIndex = 0;
         } else {
            ddlType.ItemIndex = 1;
         }

#if DEBUG
         txtSDate.DateTimeValue = DateTime.ParseExact("2018/10/11" , "yyyy/MM/dd" , null);
         ddlType.ItemIndex = 0;
         this.Text += "(開啟測試模式),ocfDate=2018/10/11";
#endif

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

      protected override ResultStatus Export() {

         string flowStepDesc = "1.開始轉出資料";
         try {
            //1.開始轉出資料
            panFilter.Enabled = false;
            gcMsg.DataSource = null;
            gcMsg.Refresh();
            gcMsg.Visible = true;
            labMsg.Visible = true;
            labMsg.Text = "訊息：資料轉出中........";
            this.Refresh();

            //1.1 ready some value
            DateTime tradeDate = txtSDate.DateTimeValue;//當日
            DateTime lastTradeDate = ai2.GetLastDate(tradeDate , "D" , "TXF%" , "%");//找到前一日期
            bool haveTradeTxw = amif.haveTradeTxw(tradeDate);//判斷當日有無TXW
            decimal closePrice = amif.GetClosePrice(tradeDate);//現貨收盤指數
            if (closePrice == 0) {
               closePrice = new AMIFU().GetClosePrice(tradeDate);
            }
            if (closePrice == 0) {
               MessageDisplay.Error("讀取現貨收盤指數無資料,請確認20110作業中有今天資料!" , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }

            //1.2 copy template xls to target path
            string tempOutputDate = (ddlType.Text == "全部收盤" ? "全部收盤" : "16時15分收盤");//ken,檔名不能有冒號,所以無法直接用下拉選單text
            string targetFileName = string.Format("{0}新版行情表({1}).xlsx" , tradeDate.ToString("yyyyMMdd") , tempOutputDate);
            string reportId = "30055_" + (ddlType.ItemIndex + 1).ToString();//後面還會用到
            string excelDestinationPath = wf_copy_file(reportId , targetFileName);
            if (excelDestinationPath == "") return ResultStatus.Fail;//當copy file發生錯誤,直接離開(這裡檔案名稱會重複,所以容易造成檔案開始時無法move造成失敗)

            //1.3 open excel
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);
            Worksheet ws = workbook.Worksheets[0];


            //2.1 今日台指期收盤指數
            flowStepDesc = "2.1 今日台指期收盤指數";
            if (!wf_30055_a(ws , tradeDate , closePrice)) return showEmailMsg(cbxNews.Checked);

            //2.2 主要指數期貨商品行情表
            flowStepDesc = "2.2 主要指數期貨商品行情表";
            if (!wf_30055_b(ws , tradeDate , lastTradeDate)) return showEmailMsg(cbxNews.Checked);

            //2.3 台指選擇權(近月及一週到期)主要序列行情表
            flowStepDesc = "2.3 台指選擇權(近月及一週到期)主要序列行情表";
            if (!wf_30055_tx(ws , tradeDate , haveTradeTxw , closePrice)) return showEmailMsg(cbxNews.Checked);


            //2.4 主要指數期貨大額交易人未平倉部位一覽表 (三大法人=外商/投信/自營商)
            //2.4 台指選擇權十大交易人未平倉部位一覽表 (三大法人=外商/投信/自營商)
            flowStepDesc = "2.4 主要指數期貨大額交易人未平倉部位一覽表 (三大法人)";
            if (!wf_30055_three_keep(ws , tradeDate , lastTradeDate)) return showEmailMsg(cbxNews.Checked);


            //2.5 主要指數期貨大額交易人未平倉部位一覽表 (大額交易人=十大交易人(近月)+十大交易人(所有月份))
            //2.5 台指選擇權十大交易人未平倉部位一覽表 (大額交易人=十大交易人(近月)+十大交易人(所有月份))
            flowStepDesc = "2.5 主要指數期貨大額交易人未平倉部位一覽表 (大額交易人)";
            if (!wf_30055_big_keep(ws , tradeDate , lastTradeDate)) return showEmailMsg(cbxNews.Checked);



            //2.6 主要股票(不含ETF)期貨行情表（依未平倉量） = STF
            //2.6 主要ETF期貨行情表（依未平倉量）= ETF
            flowStepDesc = "2.6 主要股票 期貨/ETF 商品行情表（依未平倉量）";
            if (!wf_30055_stf(ws , tradeDate , lastTradeDate , "STF")) return showEmailMsg(cbxNews.Checked);
            if (!wf_30055_stf(ws , tradeDate , lastTradeDate , "ETF")) return showEmailMsg(cbxNews.Checked);

            //2.7 主要ETF選擇權(近月價平)序列行情表
            flowStepDesc = "2.7 主要ETF選擇權(近月價平)序列行情表";
            if (!wf_30055_etc(ws , tradeDate , "ETC")) return showEmailMsg(cbxNews.Checked);


            //2.8 匯率期貨行情表
            flowStepDesc = "2.8 匯率期貨行情表";
            if (!wf_30055_prod_subtype(ws , tradeDate , "E")) return showEmailMsg(cbxNews.Checked);

            //2.9 人民幣匯率選擇權主要序列行情表(依成交量) RHF,RTF
            //ken,template是隱藏的grid,嗯
            flowStepDesc = "2.9 人民幣匯率選擇權主要序列行情表(依成交量) RHF,RTF";
            if (!wf_30055_rho(ws , tradeDate)) return showEmailMsg(cbxNews.Checked);

            //2.10 商品期貨行情表 GDF,TGF,BRF
            flowStepDesc = "2.10 商品期貨行情表 (美元黃金期貨/臺幣黃金期貨/布蘭特原油期貨)GDF,TGF,BRF";
            if (!wf_30055_prod_subtype(ws , tradeDate , "C")) return showEmailMsg(cbxNews.Checked);

            //2.11 刪除列
            flowStepDesc = "2.11 刪除列";
            wf_del_row(ws , haveTradeTxw);

            //2.12 股票期貨週
            flowStepDesc = "2.12 股票期貨週";
            ws = workbook.Worksheets[1];//切換到第二個sheet
            if (!wf_30055_weekly(ws , tradeDate)) return showEmailMsg(cbxNews.Checked);

            //2.13 先存檔
            flowStepDesc = "2.13 Save file";
            ws = workbook.Worksheets[0];
            ws.Range["A1"].Select();
            ws.ScrollToRow(0);
            workbook.SaveDocument(excelDestinationPath);

            //2.14 email news
            flowStepDesc = "2.14 email news";
            if (cbxNews.Checked) {

               DataTable dtTxemail = new TXEMAIL().ListData(reportId , 1);

               if (dtTxemail.Rows.Count != 0) {
                  string TXEMAIL_SENDER = dtTxemail.Rows[0]["TXEMAIL_SENDER"].AsString();
                  string TXEMAIL_RECIPIENTS = dtTxemail.Rows[0]["TXEMAIL_RECIPIENTS"].AsString();
                  string TXEMAIL_CC = dtTxemail.Rows[0]["TXEMAIL_CC"].AsString();
                  string TXEMAIL_TITLE = dtTxemail.Rows[0]["TXEMAIL_TITLE"].AsString();
                  string TXEMAIL_TEXT = dtTxemail.Rows[0]["TXEMAIL_TEXT"].AsString();

                  TXEMAIL_TITLE = txtSDate.DateTimeValue.ToString("yyyyMMdd") + TXEMAIL_TITLE;
                  MailHelper.SendEmail(TXEMAIL_SENDER , TXEMAIL_RECIPIENTS , TXEMAIL_CC , TXEMAIL_TITLE , TXEMAIL_TEXT , excelDestinationPath);
               }


               //return txemail_sender, txemail_recipients, txemail_cc, txemail_title
               //     DataTable dtEmail = new TXEMAIL().ListData(reportId, 1);
               //string sender = dtEmail.Rows[0]["txemail_sender"].AsString();
               //string recipient = dtEmail.Rows[0]["txemail_recipients"].AsString();
               //string cc = dtEmail.Rows[0]["txemail_cc"].AsString();
               //string title = tradeDate.ToString("yyyyMMdd") + dtEmail.Rows[0]["txemail_title"].AsString();

               //TODO:write f_send_email
               //PbFunc.f_send_email(reportId, "01", sender, recipient, cc, title, " ", excelDestinationPath);
               //is_chk = f_send_email(reportId, "01", ls_sender, ls_recipient, ls_cc, ls_title, " ", gs_savereport_path + ls_file)

            }

            #region //3.產生TJF檔案

            //3.1複製檔案
            targetFileName = string.Format("{0}_TJF.xlsx" , tradeDate.ToString("yyyy.MM.dd"));
            reportId = "30055_TJF";
            excelDestinationPath = wf_copy_file(reportId , targetFileName);
            workbook.LoadDocument(excelDestinationPath);
            ws = workbook.Worksheets[0];

            //3.2
            if (!wf_30055_tjf(ws , tradeDate , lastTradeDate)) return showEmailMsg(cbxNews.Checked);

            //3.3 儲存及關閉檔案
            workbook.SaveDocument(excelDestinationPath);

            //3.4 email
            if (cbxTJF.Checked) {
               string txnId = "30055";
               DataTable dtTxemail = new TXEMAIL().ListData(txnId , 1);

               if (dtTxemail.Rows.Count != 0) {
                  string TXEMAIL_SENDER = dtTxemail.Rows[0]["TXEMAIL_SENDER"].AsString();
                  string TXEMAIL_RECIPIENTS = dtTxemail.Rows[0]["TXEMAIL_RECIPIENTS"].AsString();
                  string TXEMAIL_CC = dtTxemail.Rows[0]["TXEMAIL_CC"].AsString();
                  string TXEMAIL_TITLE = dtTxemail.Rows[0]["TXEMAIL_TITLE"].AsString();
                  string TXEMAIL_TEXT = dtTxemail.Rows[0]["TXEMAIL_TEXT"].AsString();

                  TXEMAIL_TITLE = txtSDate.DateTimeValue.ToString("yyyyMMdd") + TXEMAIL_TITLE;
                  MailHelper.SendEmail(TXEMAIL_SENDER , TXEMAIL_RECIPIENTS , TXEMAIL_CC , TXEMAIL_TITLE , TXEMAIL_TEXT , excelDestinationPath);
               }


               //return txemail_sender, txemail_recipients, txemail_cc, txemail_title

               //DataTable dtEmail = new TXEMAIL().ListData(txnId, 1);
               //string sender = dtEmail.Rows[0]["txemail_sender"].AsString();
               //string recipient = dtEmail.Rows[0]["txemail_recipients"].AsString();
               //string cc = dtEmail.Rows[0]["txemail_cc"].AsString();
               //string title = tradeDate.ToString("yyyyMMdd") + dtEmail.Rows[0]["txemail_title"].AsString();

               //TODO:write f_send_email
               //PbFunc.f_send_email(txnId, "01", sender, recipient, cc, title, " ", excelDestinationPath);
               //is_chk = f_send_email(txnId, "01", ls_sender, ls_recipient, ls_cc, ls_title, " ", gs_savereport_path + ls_file)

            }
            #endregion


            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex , flowStepDesc);
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
         }
         return ResultStatus.Fail;
      }



      /// <summary>
      /// 2.1今日台指期收盤指數
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="tradeDate"></param>
      /// <param name="closePrice"></param>
      /// <returns></returns>
      protected bool wf_30055_a(Worksheet ws , DateTime tradeDate , decimal closePrice = 0) {
         string sheetName = "Sheet1";
         string sheetSubTitle = "今日台指期收盤指數";
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //2.1.1先抓amif現貨收盤指數,沒有抓到的話再去抓amifu的收盤指數,又沒有的話就回傳錯誤
         //2.1.2期貨近月價格
         DataTable dtAmif = amif.ListAll(tradeDate);//get some field data, return amif_close_price/amif_up_down_val
         if (dtAmif.Rows.Count <= 0) {
            showMsg(sheetName , sheetSubTitle , "無資料,請確認今日轉檔作業已完成!");
            return false;
         }
         decimal ld_txf_close_price = dtAmif.Rows[0]["amif_close_price"].AsDecimal();
         decimal ld_txf_up_down_val = dtAmif.Rows[0]["amif_up_down_val"].AsDecimal();

         if (ld_txf_close_price == 0) {
            showMsg(sheetName , sheetSubTitle , "無資料,請確認今日轉檔作業已完成!");
            return false;
         }

         //2.1.3全市總成交量
         string ll_m_qnty = ai2.GetTotalQnty(tradeDate);
         if (ll_m_qnty == "") {
            showMsg(sheetName , sheetSubTitle , "今日轉行情統計檔(AI2)未完成!");
            return false;
         }

         ws.Cells[0 , 0].Value = tradeDate;
         ws.Cells[1 , 1].Value = ld_txf_close_price;
         ws.Cells[1 , 3].Value = ld_txf_up_down_val;
         ws.Cells[1 , 4].Value = (ld_txf_close_price - closePrice >= 0 ? "正價差" : "逆價差");
         ws.Cells[1 , 5].Value = Math.Abs(ld_txf_close_price - closePrice);
         ws.Cells[2 , 1].Value = ll_m_qnty.AsDecimal();

         showMsg(sheetName , sheetSubTitle , "完成");
         return true;
      }

      /// <summary>
      /// 2.2主要指數期貨商品行情表
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="tradeDate"></param>
      /// <param name="lastTradeDate"></param>
      protected bool wf_30055_b(Worksheet ws , DateTime tradeDate , DateTime lastTradeDate) {
         string sheetName = "Sheet1";
         string sheetSubTitle = "主要指數期貨商品行情表";
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //讀取資料
         DataTable dtFuture = dao30055.wf_30055_b(tradeDate.ToString("yyyyMMdd") , lastTradeDate.ToString("yyyyMMdd"));
         if (dtFuture.Rows.Count == 0) {
            showMsg(sheetName , sheetSubTitle , "無資料");
            return false;
         }

         //根據RPT_SEQ_NO決定填寫的row index,然後一次填入8個欄位
         int rowIndex = 0;
         foreach (DataRow dr in dtFuture.Rows) {
            rowIndex = dr["RPT_SEQ_NO"].AsInt() - 1;
            if (rowIndex < 0) continue;
            for (int colIndex = 1 ; colIndex < 9 ; colIndex++) {
               if (dr[colIndex] != DBNull.Value)
                  ws.Cells[rowIndex , colIndex].Value = dr[colIndex].AsDecimal();
            }
         }

         showMsg(sheetName , sheetSubTitle , dtFuture.Rows.Count.ToString());
         return true;
      }

      /// <summary>
      /// 2.3台指選擇權(近月及一週到期)主要序列行情表
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="tradeDate"></param>
      /// <param name="haveTradeTxw"></param>
      /// <param name="closePrice"></param>
      /// <returns></returns>
      protected bool wf_30055_tx(Worksheet ws , DateTime tradeDate , bool haveTradeTxw , decimal closePrice) {
         string sheetName = "Sheet1";
         string sheetSubTitle = "台指選擇權(近月及一週到期)主要序列行情表";
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //有無TXW,會當成參數條件
         int txo_row_cnt, txw_row_cnt;
         if (haveTradeTxw) {
            txo_row_cnt = 4;
            txw_row_cnt = 2;
         } else {
            txo_row_cnt = 6;
            txw_row_cnt = 0;
         }
         decimal StockStrikePrice = Math.Truncate(closePrice / 100) * 100;//取現貨收盤價,百元以下無條件捨去當價平

         DataTable dtOption = dao30055.wf_30055_tx(tradeDate , StockStrikePrice , txo_row_cnt , txw_row_cnt);
         if (dtOption.Rows.Count == 0) {
            showMsg(sheetName , sheetSubTitle , string.Format("價平:{0},無資料" , StockStrikePrice.AsString()));
            return false;
         }

         //write data (跟據每個商品的每個價位,分C和P兩邊寫入)
         string kindId = "";
         decimal strikePrice = 0;
         int rowBegin = 0;
         int rowPos = 0;
         int colBegin = 0;
         foreach (DataRow dr in dtOption.Rows) {
            //init(跟據每個商品)
            if (dr["AMIF_KIND_ID"].AsString() != kindId) {
               kindId = dr["AMIF_KIND_ID"].AsString();
               rowBegin = dr["RPT_SEQ_NO"].AsInt() - 1;
               strikePrice = 0;
               rowPos = 0;
            }
            //跟據每個價位
            if (strikePrice != dr["AMIF_STRIKE_PRICE"].AsDecimal()) {
               strikePrice = dr["AMIF_STRIKE_PRICE"].AsDecimal();
               rowPos = rowPos + 1;
               ws.Cells[rowBegin + rowPos , 5].Value = strikePrice;
            }

            colBegin = (dr["AMIF_PC_CODE"].AsString() == "C" ? 0 : 6);//call=A欄開始,put=G欄開始

            if (dr["AMIF_OPEN_PRICE"] != DBNull.Value) ws.Cells[rowBegin + rowPos , colBegin + 0].Value = dr["AMIF_OPEN_PRICE"].AsDecimal();
            if (dr["AMIF_HIGH_PRICE"] != DBNull.Value) ws.Cells[rowBegin + rowPos , colBegin + 1].Value = dr["AMIF_HIGH_PRICE"].AsDecimal();
            if (dr["AMIF_LOW_PRICE"] != DBNull.Value) ws.Cells[rowBegin + rowPos , colBegin + 2].Value = dr["AMIF_LOW_PRICE"].AsDecimal();
            if (dr["AMIF_SETTLE_PRICE"] != DBNull.Value) ws.Cells[rowBegin + rowPos , colBegin + 3].Value = dr["AMIF_SETTLE_PRICE"].AsDecimal();
            if (dr["AMIF_M_QNTY_TAL"] != DBNull.Value) ws.Cells[rowBegin + rowPos , colBegin + 4].Value = dr["AMIF_M_QNTY_TAL"].AsDecimal();

         }//foreach (DataRow dr in dtOption.Rows) {

         showMsg(sheetName , sheetSubTitle , dtOption.Rows.Count.ToString());
         return true;
      }


      /// <summary>
      /// 2.4 主要指數期貨大額交易人未平倉部位一覽表 (三大法人=外商/投信/自營商)
      /// 2.4 台指選擇權十大交易人未平倉部位一覽表 (三大法人=外商/投信/自營商)
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="tradeDate"></param>
      /// <param name="lastTradeDate"></param>
      protected bool wf_30055_three_keep(Worksheet ws , DateTime tradeDate , DateTime lastTradeDate) {
         string sheetName = "Sheet1";
         string sheetSubTitle = "主要指數期貨大額交易人未平倉部位一覽表 (三大法人)";
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         DataTable dtThree = dao30055.wf_30055_three_keep(tradeDate , lastTradeDate);
         if (dtThree.Rows.Count == 0) {
            showMsg(sheetName , sheetSubTitle , "無資料");
            return false;
         }

         //ken,這邊很特別,一次把兩張表格的資料都撈出,第一張是期貨,第二章是選擇權,然後一直在算位置填入,有夠亂,只要一調整,又會整個亂掉
         int futureCount = 0;
         int optionCount = 0;
         foreach (DataRow dr in dtThree.Rows) {
            int rpt_seq_no = dr["rpt_seq_no"].AsInt() - 1;
            int cp_seq_no = dr["cp_seq_no"].AsInt();
            string pcCode = dr["pc_code"].AsString();
            //string btinoivl3f_prodid = dr["btinoivl3f_prodid"].AsString();
            int rowIndex = rpt_seq_no + cp_seq_no - 1;

            //pcCode 空值=期貨,C=call,P=put
            if (pcCode == "") {
               futureCount++;
            } else {
               optionCount++;
            }

            int colBegin = 0;
            for (int k = 3 ; k < 7 ; k++) {
               if (pcCode == "C")
                  colBegin = 1;
               else if (pcCode == "P")
                  colBegin = 5;
               else
                  colBegin = 2;

               if (pcCode == "")//pcCode 空值=期貨,C=call,P=put
                  ws.Cells[rowIndex , colBegin + k - 2].SetValue(dr[k]);
               else
                  ws.Cells[rowIndex , colBegin + k - 3].SetValue(dr[k]);

            }//for (int k = 3;k < 7;k++) {

         }//foreach(DataRow dr in dtThree.Rows) {

         //2.4 主要指數期貨大額交易人未平倉部位一覽表 (三大法人=外商/投信/自營商)
         //2.4 台指選擇權十大交易人未平倉部位一覽表 (三大法人=外商/投信/自營商)
         showMsg(sheetName , "主要指數期貨大額交易人未平倉部位一覽表 (三大法人)" , futureCount.ToString());
         showMsg(sheetName , "台指選擇權十大交易人未平倉部位一覽表 (三大法人)" , optionCount.ToString());
         return true;
      }

      /// <summary>
      /// 2.5 主要指數期貨大額交易人未平倉部位一覽表 (大額交易人)
      /// 2.5 台指選擇權十大交易人未平倉部位一覽表 (大額交易人)
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="tradeDate"></param>
      /// <param name="lastTradeDate"></param>
      protected bool wf_30055_big_keep(Worksheet ws , DateTime tradeDate , DateTime lastTradeDate) {
         string sheetName = "Sheet1";
         string sheetSubTitle = "主要指數期貨大額交易人未平倉部位一覽表 (大額交易人)";
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         DataTable dtBig = dao30055.wf_30055_big_keep(tradeDate , lastTradeDate);
         if (dtBig.Rows.Count == 0) {
            showMsg(sheetName , sheetSubTitle , "無資料");
            return false;
         }

         //ken,這邊很特別,一次把兩張表格的資料都撈出,第一張是期貨,第二章是選擇權,然後一直在算位置填入,有夠亂,只要一調整,又會整個亂掉
         int futureCount = 0;
         int optionCount = 0;
         foreach (DataRow dr in dtBig.Rows) {
            // "大額交易人"的Month Type如果超過 6位數, 不可以寫入excel裡面(期貨的資料量太少,不適合寫入excel) 
            if (dr["toi1_prod_type"].AsString() == "F" && dr["month_type"].AsString().Length > 6) continue;
            int rpt_seq_no = dr["rpt_seq_no"].AsInt() - 1;
            int cp_seq_no = dr["cp_seq_no"].AsInt();
            string pcCode = dr["toi1_pc_code"].AsString();
            //string toi1_prod_type = dr["toi1_prod_type"].AsString();
            int rowIndex = rpt_seq_no + cp_seq_no - 1;

            //pcCode 空值=期貨,C=call,P=put
            if (pcCode == "") {
               futureCount++;
            } else {
               rowIndex++;//ken,修正範本檔與RPT設定不同步造成的問題
               optionCount++;
            }

            int colBegin = 0;
            for (int k = 3 ; k < 7 ; k++) {
               if (pcCode == "C")
                  colBegin = 1;
               else if (pcCode == "P")
                  colBegin = 5;
               else
                  colBegin = 2;

               if (pcCode == "")//pcCode 空值=期貨,C=call,P=put
                  ws.Cells[rowIndex , colBegin + k - 2].SetValue(dr[k]);
               else
                  ws.Cells[rowIndex , colBegin + k - 3].SetValue(dr[k]);
            }//for (int k = 3;k < 7;k++) {

         }//foreach(DataRow dr in dtBig.Rows) {

         //2.5 主要指數期貨大額交易人未平倉部位一覽表 (大額交易人)
         //2.5 台指選擇權十大交易人未平倉部位一覽表 (大額交易人)
         showMsg(sheetName , "主要指數期貨大額交易人未平倉部位一覽表 (大額交易人)" , futureCount.ToString());
         showMsg(sheetName , "台指選擇權十大交易人未平倉部位一覽表 (大額交易人)" , optionCount.ToString());
         showMsg(sheetName , sheetSubTitle , dtBig.Rows.Count.ToString());
         return true;
      }

      /// <summary>
      /// 2.6主要股票(不含ETF)期貨行情表（依未平倉量）
      /// 2.6主要ETF期貨行情表（依未平倉量）
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="tradeDate"></param>
      /// <param name="lastTradeDate"></param>
      /// <param name="paramKey">"STF"=期貨, "ETF"=ETF期貨</param>
      protected bool wf_30055_stf(Worksheet ws , DateTime tradeDate , DateTime lastTradeDate , string paramKey = "STF") {
         string sheetName = "Sheet1";
         string sheetSubTitle = (paramKey == "STF" ? "主要股票(不含ETF)期貨行情表（依未平倉量）" : "主要ETF期貨行情表（依未平倉量）");
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //2.6.1 讀取資料
         DataTable dtKind = dao30055.wf_30055_stf(tradeDate.ToString("yyyyMMdd") , lastTradeDate.ToString("yyyyMMdd") , paramKey);
         if (dtKind.Rows.Count == 0) {
            showMsg(sheetName , sheetSubTitle , "無資料");
            return false;
         }

         //2.6.2 get rowBegin
         string txdId = string.Format("30055{0}" , paramKey.ToLower());
         string rptSeqNo = new RPT().GetSeqNo("30055" , txdId);//參考RPT的RPT_SEQ_NO
         if (string.IsNullOrEmpty(rptSeqNo)) {
            showMsg(sheetName , sheetSubTitle , string.Format("未設定{0}起始位置!" , txdId));
            return false;
         }
         int rowBegin = int.Parse(rptSeqNo);

         //2.6.3 write data
         foreach (DataRow dr in dtKind.Rows) {
            dr[0] = RemoveString(dr[0].AsString() , (paramKey == "STF" ? "期貨" : "ETF期貨"));//apdk_name去掉"期貨/ETF期貨"這個字眼之後的字
         }
         dtKind.Columns.Remove("seq_no");//這裡的seq_no只是colBegin而且是流水號,不需要特別使用

         int colBegin = 0;
         //ken,注意ETF輸出時的col前兩格為合併儲存格,所以要特別設計一個無所謂的欄位給第二個col(不會輸出)
         if (paramKey == "STF") {
            dtKind.Columns.Remove("kind_id2");
         }

         ws.Import(dtKind , false , rowBegin , colBegin);


         showMsg(sheetName , sheetSubTitle , dtKind.Rows.Count.ToString());
         return true;
      }

      /// <summary>
      /// 2.7主要ETF選擇權(近月價平)序列行情表
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="tradeDate"></param>
      /// <param name="lastTradeDate"></param>
      /// <param name="paramKey">"ETC"=期貨, "ETF"=ETF期貨</param>
      protected bool wf_30055_etc(Worksheet ws , DateTime tradeDate , string paramKey = "ETC") {
         string sheetName = "Sheet1";
         string sheetSubTitle = "主要ETF選擇權(近月價平)序列行情表";
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //2.7.1 讀取資料
         DataTable dtKind = dao30055.wf_30055_etc(tradeDate);
         if (dtKind.Rows.Count == 0) {
            showMsg(sheetName , sheetSubTitle , "無資料");
            return false;
         }

         //2.7.2 get rowBegin
         string txdId = string.Format("30055{0}" , paramKey.ToLower());
         string rptSeqNo = new RPT().GetSeqNo("30055" , txdId);
         if (string.IsNullOrEmpty(rptSeqNo)) {
            showMsg(sheetName , sheetSubTitle , string.Format("未設定{0}起始位置!" , txdId));
            return false;
         }
         int rowBegin = int.Parse(rptSeqNo);

         //2.7.3 write data
         //special 就是只寫8筆沒錯,目前先直接寫在sql
         //每筆12欄位,pdk_name在第六個位置
         foreach (DataRow dr in dtKind.Rows) {
            dr["pdk_name"] = RemoveString(dr["pdk_name"].AsString() , "ETF選擇權");//pdk_name去掉"ETF選擇權"這個字眼之後的字
         }
         dtKind.Columns.Remove("m_qnty");
         ws.Import(dtKind , false , rowBegin , 0);

         showMsg(sheetName , sheetSubTitle , dtKind.Rows.Count.ToString());
         return true;
      }

      /// <summary>
      /// 2.8 匯率期貨行情表
      /// 2.10商品期貨行情表
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="tradeDate"></param>
      /// <param name="as_type">E=匯率期貨行情表,C=商品期貨行情表</param>
      protected bool wf_30055_prod_subtype(Worksheet ws , DateTime tradeDate , string as_type = "E") {
         string sheetName = "Sheet1";
         string sheetSubTitle = (as_type == "E" ? "匯率期貨行情表" : "商品期貨行情表");
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //2.8.1 get 匯率類/商品類 前一交易日
         DateTime tempDate = tradeDate.AddDays(-30);
         DateTime lastDate = new AI2().GetLastDate(tradeDate , "D" , "%" , as_type);
         if (lastDate == DateTime.MinValue) {
            lastDate = tradeDate;
         }

         //2.8.2 讀取資料
         DataTable dtFuture = dao30055.wf_30055_prod_subtype(tradeDate.ToString("yyyyMMdd") , lastDate.ToString("yyyyMMdd") , as_type);
         if (dtFuture.Rows.Count <= 0) {
            showMsg(sheetName , sheetSubTitle , "無資料");
            return false;
         }

         //2.8.3 write data
         //順序都連續並且固定,所以可以用import
         int rowBegin = dtFuture.Rows[0]["seq_no"].AsInt() - 1;//rowIndex是參考RPT的rpt_level_1=seq_no
         dtFuture.Columns.Remove("apdk_name");
         dtFuture.Columns.Remove("amif_settle_date");
         dtFuture.Columns.Remove("kind_id2");
         dtFuture.Columns.Remove("seq_no");
         ws.Import(dtFuture , false , rowBegin , 2);

         showMsg(sheetName , sheetSubTitle , dtFuture.Rows.Count.ToString());
         return true;
      }

      /// <summary>
      /// 2.9 人民幣匯率選擇權主要序列行情表(依成交量)
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="tradeDate"></param>
      protected bool wf_30055_rho(Worksheet ws , DateTime tradeDate) {
         string sheetName = "Sheet1";
         string sheetSubTitle = "人民幣匯率選擇權主要序列行情表(依成交量)";
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //2.6.1 讀取資料
         DataTable dtKind = dao30055.wf_30055_rho(tradeDate);
         if (dtKind.Rows.Count == 0) {
            showMsg(sheetName , sheetSubTitle , "無資料");
            return false;
         }

         //2.6.2 get rowBegin
         string txdId = "30055rho";
         string rptSeqNo = new RPT().GetSeqNo("30055" , txdId);
         if (string.IsNullOrEmpty(rptSeqNo)) {
            showMsg(sheetName , sheetSubTitle , string.Format("未設定{0}起始位置!" , txdId));
            return false;
         }

         //2.6.3 write data
         //special 就是只寫三筆沒錯,目前先直接寫在sql
         int rowBegin = int.Parse(rptSeqNo);
         for (int k = 0 ; k < 3 ; k++) {
            //把pdk_name改個名
            string pdk_name = dtKind.Rows[k]["pdk_name"].AsString();
            dtKind.Rows[k]["pdk_name"] = RemoveString(pdk_name , "選擇權");
         }//foreach (DataRow dr in dtKind.Rows) {

         ws.Import(dtKind , false , rowBegin , 0);

         showMsg(sheetName , sheetSubTitle , dtKind.Rows.Count.ToString());
         return true;
      }

      /// <summary>
      /// 2.11刪除列
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="haveTradeTxw">判斷當日有無TXW</param>
      protected void wf_del_row(Worksheet ws , bool haveTradeTxw) {

         DataTable dtRemove = dao30055.wf_del_row();
         if (dtRemove.Rows.Count <= 0) return;

         DataRow dr = dtRemove.Rows[0];
         int mxw_seq_no = dr["mxw_seq_no"].AsInt();
         int txo_seq_no_end = dr["txo_seq_no_end"].AsInt();
         int txo_seq_no_start = dr["txo_seq_no_start"].AsInt();
         int txw_seq_no_end = dr["txw_seq_no_end"].AsInt();
         int txw_seq_no_start = dr["txw_seq_no_start"].AsInt();
         int toi_seq_no = dr["toi_seq_no"].AsInt();


         if (haveTradeTxw) {
            ws.Rows.Remove(txo_seq_no_start - 1 , txo_seq_no_end - txo_seq_no_start + 1);
         } else {
            //從後面往前刪
            //台指選擇權大額交易人未平倉部位一覽表
            ws.Rows.Remove(toi_seq_no - 1 , 1); //(只刪除一行 name=小臺指期近月)

            //台指選擇權(一週)主要序列行情表
            ws.Rows.Remove(txw_seq_no_start - 1 , txw_seq_no_end - txw_seq_no_start + 1); //(只刪除4行 name=一週到期option)

            //主要指數期貨商品行情表
            ws.Rows.Remove(mxw_seq_no - 1 , 1); //(只刪除一行 name=一周)
         }//if (haveTradeTxw) {


      }

      /// <summary>
      /// 2.12 股票期貨週
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="tradeDate"></param>
      protected bool wf_30055_weekly(Worksheet ws , DateTime tradeDate) {
         string sheetName = "Sheet2";
         string sheetSubTitle = "股票期貨週";
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //TODO:邏輯有問題
         //PS: DayNumber = 一星期中的第几天(用1到7之间的整数表示，星期天为1，星期一为2，...)
         //daynum = DayNumber(date(ldt_edate.text))
         //ldt_sdate = RelativeDate(date(ldt_edate.text), (daynum - 2) * (-1))
         //然後去找AI2 AI2_YMD日期區間between ldt_sdate and ldt_edate
         int dayNumber = (int)tradeDate.DayOfWeek;
         DateTime ldt_sdate = tradeDate.AddDays(1 - dayNumber);

         //2.12.1 讀取資料
         DataTable dtWeek = dao30055.wf_30055_weekly(ldt_sdate , tradeDate);//return AI2_KIND_ID, APDK_NAME, AI2_M_QNTY
         if (dtWeek.Rows.Count == 0) {
            showMsg(sheetName , sheetSubTitle , "無資料");
            return false;
         }

         //2.12.2 write data
         //只輸出10筆資料,直接設定在sql
         ws.Cells[0 , 0].Value = string.Format("資料期間：{0}～{1}" , ldt_sdate.ToString("yyyy/MM/dd") , tradeDate.ToString("yyyy/MM/dd"));
         ws.Import(dtWeek , false , 3 , 0);

         showMsg(sheetName , sheetSubTitle , dtWeek.Rows.Count.ToString());
         return true;
      }


      /// <summary>
      /// 3.2 東證期貨商品行情表 (和2.2主要指數期貨商品行情表 非常像)
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="tradeDate"></param>
      /// <param name="lastTradeDate"></param>
      /// <returns></returns>
      protected bool wf_30055_tjf(Worksheet ws , DateTime tradeDate , DateTime lastTradeDate) {
         string sheetName = "Sheet1";
         string sheetSubTitle = "東證期貨商品行情表";
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //讀取資料
         DataTable dtFuture = dao30055.wf_30055_b(tradeDate.ToString("yyyyMMdd") , lastTradeDate.ToString("yyyyMMdd"));
         dtFuture = dtFuture.Filter("amif_kind_id = 'TJF' and isnull(amif_m_qnty_tal,0) <> 0 ");
         if (dtFuture.Rows.Count <= 0) {
            showMsg(sheetName , sheetSubTitle , "無資料");
            return false;
         }

         ws.Cells[0 , 0].Value = tradeDate.ToString("yyyy/MM/dd");

         //根據RPT_SEQ_NO決定填寫的row index,然後一次填入8個欄位
         int rowIndex = 0;
         foreach (DataRow dr in dtFuture.Rows) {
            rowIndex = dr["RPT_SEQ_NO"].AsInt() - 1;
            if (rowIndex < 0) continue;
            for (int colIndex = 1 ; colIndex < 9 ; colIndex++) {
               if (dr[colIndex] != DBNull.Value)
                  ws.Cells[rowIndex , colIndex].Value = dr[colIndex].AsDecimal();
            }
         }//foreach (DataRow dr in dtFuture.Rows) {

         showMsg(sheetName , sheetSubTitle , dtFuture.Rows.Count.ToString());
         return true;
      }


      #region 顯示匯出訊息的相關函數

      /// <summary>
      /// 顯示匯出的進度
      /// </summary>
      /// <param name="sheetName"></param>
      /// <param name="sheetSubTitle"></param>
      /// <param name="msg"></param>
      protected void showMsg(string sheetName , string sheetSubTitle , string msg) {
         showMsg(sheetName , sheetSubTitle , "" , msg);
      }

      /// <summary>
      /// 顯示匯出的進度
      /// </summary>
      /// <param name="sheetName"></param>
      /// <param name="sheetSubTitle"></param>
      /// <param name="subMsg"></param>
      /// <param name="msg"></param>
      protected void showMsg(string sheetName , string sheetSubTitle , string subMsg , string msg) {

         DataRow drTemp = dtMsg.NewRow();
         drTemp["SheetName"] = sheetName;
         drTemp["SheetSubTitle"] = sheetSubTitle;
         drTemp["SubMsg"] = subMsg;
         drTemp["Msg"] = msg;
         dtMsg.Rows.Add(drTemp);

         gcMsg.DataSource = dtMsg;
         gcMsg.RefreshDataSource();
         gcMsg.Refresh();

         Thread.Sleep(100);
      }

      private void gvMsg_RowCountChanged(object sender , EventArgs e) {
         gvMsg.MoveLast();
      }

      #endregion

      /// <summary>
      /// 將範本檔copy到Report Path,成功的話回傳完整路徑 (與PbFunc.f_copy_file有差異)
      /// </summary>
      /// <param name="reportId"></param>
      /// <param name="targetFileName"></param>
      /// <returns></returns>
      protected string wf_copy_file(string reportId , string targetFileName) {
         string excelFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH , reportId + ".xlsx");

         //1.檢查範本檔是否存在
         if (!File.Exists(excelFilePath)) {
            throw new Exception("無此檔案「" + excelFilePath + "」!");
         }

         string targetFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , targetFileName.Replace(":" , "時"));
         string targetFilePathBak = targetFilePath.Replace(".xlsx" , "_bak_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".xlsx");

         //2.假如檔案存在,改原檔名加上_bak_yyyy.MM.dd-HH.mm.ss
         if (File.Exists(targetFilePath)) {
            try {
               File.Move(targetFilePath , targetFilePathBak);
            } catch (Exception ex) {
               WriteLog(ex , "" , true);
               return "";
            }
         }

         //3.copy template excel to target report path
         try {
            File.Copy(excelFilePath , targetFilePath);
         } catch (Exception ex) {
            WriteLog(ex , "" , true);
            //throw new Exception(string.Format("複製「{0}」到「{1}」檔案錯誤!", excelFilePath, targetFilePath));
            return "";
         }

         return targetFilePath;
      }

      /// <summary>
      /// 移除契約名稱不必要顯示的字眼(例如期貨/選擇權)
      /// </summary>
      /// <param name="source"></param>
      /// <param name="removeString"></param>
      /// <returns></returns>
      protected string RemoveString(string source , string removeString) {
         int pos = source.IndexOf(removeString);
         if (pos >= 0)
            return source.Remove(pos);
         else
            return source;
      }

      /// <summary>
      /// 只要發生錯誤,而且有勾選email news,則show message box
      /// </summary>
      /// <param name="sendEmail"></param>
      protected ResultStatus showEmailMsg(bool sendEmail) {
         if (sendEmail)
            MessageDisplay.Warning("產出檔案有異常資訊，請通知系統負責人！" , GlobalInfo.WarningText);

         return ResultStatus.Fail;
      }


   }
}