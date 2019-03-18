using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
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
/// Lukas, 2019/3/11
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30055 特製行情表(經濟工商)
   /// </summary>
   public partial class W30055 : FormParent {
      protected DataTable dtMsg;
      protected D30055 dao30055;
      protected AMIF amif;
      protected AI2 ai2;

      protected class LookupItem {
         public string ValueMember { get; set; }
         public string DisplayMember { get; set; }
      }

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
         txtSDate.EditValue = PbFunc.f_ocf_date(0);

#if DEBUG
         //ken test
         //ken,2018/10/15以前才有大盤資料,每個table日期不相同,要分開測
         txtSDate.DateTimeValue = DateTime.ParseExact("2019/01/03" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),ocfDate=2019/01/03";
#endif

         //盤別下拉選單
         List<LookupItem> lstType = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "1", DisplayMember = "16:15收盤"},
                                        new LookupItem() { ValueMember = "2", DisplayMember = "全部收盤" }};
         Extension.SetDataTable(ddlType , lstType , "ValueMember" , "DisplayMember" , TextEditStyles.DisableTextEditor , "");

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

            //1.1ready some value
            DateTime idt_date = txtSDate.DateTimeValue;//當日
            DateTime idt_last_date = ai2.GetLastDate(idt_date);//找到前一日期
            bool haveTradeTxw = amif.haveTradeTxw(idt_date);//判斷當日有無TXW
            decimal id_close_price = amif.GetClosePrice(idt_date);//現貨收盤指數
            if (id_close_price == 0) {
               id_close_price = new AMIFU().GetClosePrice(idt_date);
            }

            //1.1 copy template xls to target path
            string targetFileName = string.Format("{0}新版行情表({1}).xlsx" , idt_date.ToString("yyyyMMdd") , ddlType.Text);
            string excelDestinationPath = wf_copy_file("30055_" + (ddlType.ItemIndex + 1).ToString() , targetFileName);
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);
            Worksheet ws = workbook.Worksheets[0];

            //2.1今日台指期收盤指數
            flowStepDesc = "2.1今日台指期收盤指數";
            wf_30055_a(ws , idt_date , id_close_price);

            //2.2主要指數期貨商品行情表
            flowStepDesc = "2.2主要指數期貨商品行情表";
            wf_30055_b(ws , idt_date , idt_last_date);

            //2.3台指選擇權(近月及一週到期)主要序列行情表
            flowStepDesc = "2.3台指選擇權(近月及一週到期)主要序列行情表";
            wf_30055_tx(ws , idt_date , haveTradeTxw , id_close_price);

            //2.4大額交易人－指數期貨
            flowStepDesc = "2.4大額交易人－指數期貨";
            wf_30055_toi1(ws , idt_date , idt_last_date);

            //2.5三大法人－指數期貨
            flowStepDesc = "2.5三大法人－指數期貨";
            wf_30055_3(ws , idt_date , idt_last_date);

            ////2.6主要股票期貨商品行情表（依未平倉量）
            //flowStepDesc = "2.6主要股票期貨商品行情表（依未平倉量）";
            //wf_30055_stf();
            //wf_30055_etf();

            ////2.7主要ETF選擇權(近月價平)序列行情表
            //flowStepDesc = "2.7主要ETF選擇權(近月價平)序列行情表";
            //wf_30055_etc();

            ////2.8 RHF,RTF
            //flowStepDesc = "2.8 RHF,RTF";
            //wf_30055_prod_subtype_e()

            ///2.9 RHF,RTF
            //flowStepDesc = "2.9 RHF,RTF";
            //wf_30055_rho();

            ////2.10 GDF,TGF,BRF
            //flowStepDesc = "2.10 GDF,TGF,BRF";
            //wf_30055_prod_subtype_c()

            ////2.11刪除列
            //flowStepDesc = "2.11刪除列";
            //wf_del_row()

            ////2.12週
            //flowStepDesc = "2.12週";
            //wf_30055_weekly();

            //2.13 先存檔
            flowStepDesc = "2.13 Save file";
            workbook.SaveDocument(excelDestinationPath);

            //2.14 email news
            //flowStepDesc = "2.14 email news";
            //string ls_filename, ls_rtn, ls_log_filename
            //string ls_sender, ls_recipient, ls_cc, ls_title
            //if      cbx_email_news.checked  then
            //        if      is_chk <> 'Y'   then
            //                messagebox(gs_t_err, "產出檔案有異常，請通知系統負責人！", StopSign!)
            //                return
            //        end if

            //        select TXEMAIL_SENDER,TXEMAIL_RECIPIENTS,TXEMAIL_CC,TXEMAIL_TITLE
            //            into :ls_sender,:ls_recipient,:ls_cc,:ls_title
            //          from ci.txEMAIL
            //        WHERE TXEMAIL_TXN_ID = :ls_rpt_id
            //          AND TXEMAIL_SEQ_NO = 1
            //        ;
            //                ls_title = string(date(em_date.text), "YYYYMMDD") + trim(ls_title)

            //        is_chk = f_send_email(ls_rpt_id, "01", ls_sender, ls_recipient, ls_cc, ls_title, " ", gs_savereport_path + ls_file)
            //end if


            //TJF檔案
            //ls_rpt_id = "30055_TJF"

            ////複製檔案
            //ls_file = wf_copyfile(ls_rpt_id)
            //if      ls_file = "" then
            //        return
            //end if
            //is_log_txt = ls_file

            ////開啟檔案
            //iole_1.visible = false
            //iole_1.application.workbooks.open(gs_SaveReport_path + ls_file)
            //is_rpt_flag = 'Y'
            //wf_30055_TJF()

            ////儲存及關閉檔案
            //iole_1.application.ActiveWorkbook.Save
            //iole_1.application.activeworkbook.close()
            //if      is_chk = 'Y'    then
            //        st_msg_txt.text = ""
            //end if

            ////email
            //if      cbx_email_tjf.checked then
            //        if      is_rpt_flag <> 'Y'  then
            //                messagebox(gs_t_err, "產出檔案有異常資訊，請通知系統負責人！", StopSign!)
            //                return
            //        end if

            //        select TXEMAIL_SENDER,TXEMAIL_RECIPIENTS,TXEMAIL_CC,TXEMAIL_TITLE
            //            into :ls_sender,:ls_recipient,:ls_cc,:ls_title
            //            from ci.txEMAIL
            //        WHERE TXEMAIL_TXN_ID = '30055'
            //        AND TXEMAIL_SEQ_NO = 1
            //        ;
            //        is_chk = f_send_email("30055", "01", ls_sender, ls_recipient, ls_cc, ls_title, " ", gs_savereport_path + ls_file)
            //end if



            if (FlagAdmin)
               System.Diagnostics.Process.Start(excelDestinationPath);

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
      /// 將範本檔copy到Report Path,成功的話回傳完整路徑 (與PbFunc.f_copy_file有差異)
      /// </summary>
      /// <param name="excelFileName"></param>
      /// <param name="targetFileName"></param>
      /// <returns></returns>
      protected string wf_copy_file(string excelFileName , string targetFileName) {
         string excelFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH , excelFileName + ".xlsx");

         //1.檢查範本檔是否存在
         if (!File.Exists(excelFilePath)) {
            throw new Exception("無此檔案「" + excelFilePath + "」!");
         }

         string targetFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , targetFileName);
         string targetFilePathBak = targetFilePath.Replace(".xlsx" , "_bak_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".xlsx");

         //2.假如檔案存在,改原檔名加上_bak_yyyy.MM.dd-HH.mm.ss
         if (File.Exists(targetFilePath)) {
            File.Move(targetFilePath , targetFilePathBak);
         }

         //3.copy template excel to target report path
         try {
            File.Copy(excelFilePath , targetFilePath);

            //ken,如果要開檔案寫檔,要把該檔案先lock
            //using (FileStream lockFile = new FileStream(targetFilePath,FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.Delete)){}
         } catch {
            throw new Exception(string.Format("複製「{0}」到「{1}」檔案錯誤!" , excelFilePath , targetFilePath));
         }

         return targetFilePath;
      }

      /// <summary>
      /// 2.1今日台指期收盤指數
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="idt_date"></param>
      private void wf_30055_a(Worksheet ws , DateTime idt_date , decimal id_close_price = 0) {
         string sheetName = "Sheet1";
         string sheetSubTitle = "今日台指期收盤指數";
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //2.1.1先抓amif現貨收盤指數,沒有抓到的話再去抓amifu的收盤指數,又沒有的話就回傳錯誤
         if (id_close_price == 0) {
            showMsg(sheetName , sheetSubTitle , "讀取現貨收盤指數無資料,請確認20110作業中有今天資料!");
            return;
         }

         //2.1.2期貨近月價格
         DataTable dtAmif = amif.ListAll(idt_date);//get some field data, return amif_close_price/amif_up_down_val
         if (dtAmif.Rows.Count <= 0) {
            showMsg(sheetName , sheetSubTitle , "讀取期貨收盤指數無資料,請確認今日轉檔作業已完成!");
            return;
         }
         decimal ld_txf_close_price = dtAmif.Rows[0]["amif_close_price"].AsDecimal();
         decimal ld_txf_up_down_val = dtAmif.Rows[0]["amif_up_down_val"].AsDecimal();

         if (ld_txf_close_price == 0) {
            showMsg(sheetName , sheetSubTitle , "讀取期貨收盤指數無資料,請確認今日轉檔作業已完成!");
            return;
         }

         //2.1.3全市總成交量
         string ll_m_qnty = ai2.GetTotalQnty(idt_date);
         if (ll_m_qnty == "") {
            showMsg(sheetName , sheetSubTitle , "今日轉行情統計檔(AI2)未完成!");
            return;
         }

         ws.Cells[0 , 0].Value = idt_date;
         ws.Cells[1 , 1].Value = ld_txf_close_price;
         ws.Cells[1 , 3].Value = ld_txf_up_down_val;
         ws.Cells[1 , 4].Value = (ld_txf_close_price - id_close_price >= 0 ? "正價差" : "逆價差");
         ws.Cells[1 , 5].Value = Math.Abs(ld_txf_close_price - id_close_price);
         ws.Cells[2 , 1].Value = ll_m_qnty.AsDecimal();

         showMsg(sheetName , sheetSubTitle , "完成");
      }

      /// <summary>
      /// 2.2主要指數期貨商品行情表
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="idt_date"></param>
      /// <param name="idt_last_date"></param>
      private void wf_30055_b(Worksheet ws , DateTime idt_date , DateTime idt_last_date) {
         string sheetName = "Sheet1";
         string sheetSubTitle = "主要指數期貨商品行情表";
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //讀取資料
         DataTable dtFuture = dao30055.wf_30055_b(idt_date.ToString("yyyyMMdd") , idt_last_date.ToString("yyyyMMdd"));
         if (dtFuture.Rows.Count == 0) {
            showMsg(sheetName , sheetSubTitle , "無資料");
            return;
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
      }

      /// <summary>
      /// 2.3台指選擇權(近月及一週到期)主要序列行情表
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="idt_date"></param>
      private void wf_30055_tx(Worksheet ws , DateTime idt_date , bool haveTradeTxw , decimal id_close_price) {
         string sheetName = "Sheet1";
         string sheetSubTitle = "台指選擇權(近月及一週到期)主要序列行情表";
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //有無TXW,會當成參數條件
         int li_txo_row_cnt, li_txw_row_cnt;
         if (haveTradeTxw) {
            li_txo_row_cnt = 4;
            li_txw_row_cnt = 2;
         } else {
            li_txo_row_cnt = 6;
            li_txw_row_cnt = 0;
         }
         decimal StockStrikePrice = Math.Truncate(id_close_price / 100) * 100;//取現貨收盤價,百元以下無條件捨去當價平

         DataTable dtOption = dao30055.wf_30055_tx(idt_date , StockStrikePrice , li_txo_row_cnt , li_txw_row_cnt);
         if (dtOption.Rows.Count == 0) {
            showMsg(sheetName , sheetSubTitle , string.Format("價平:{0},無資料" , StockStrikePrice.AsString()));
            return;
         }

         //write data (跟據每個商品的每個價位,分C和P兩邊寫入)
         //ken,rowBegin撈出來怪怪的,目前excel 台指選擇權(近月及一週到期)主要序列行情表 row起始位置應該是49
         //ken,rowBegin撈出來怪怪的,目前excel 台指選擇權(一週)主要序列行情表           row起始位置應該是63
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
      }

      /// <summary>
      /// 2.4大額交易人－指數期貨
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="idt_date"></param>
      /// <param name="idt_last_date"></param>
      private void wf_30055_toi1(Worksheet ws , DateTime idt_date , DateTime idt_last_date) {
         string sheetName = "Sheet1";
         string sheetSubTitle = "大額交易人－指數期貨";//主要指數期貨大額交易人未平倉部位一覽表+台指選擇權十大交易人未平倉部位一覽表
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //ken,這邊很特別,一次把兩張表格的資料都撈出,第一張是期貨,第二章是選擇權,然後一直在算位置填入,有夠亂,只要一調整,又會整個亂掉
         DataTable dtBig = dao30055.wf_30055_toi1(idt_date , idt_last_date);
         if (dtBig.Rows.Count == 0) {
            showMsg(sheetName , sheetSubTitle , "無資料");
            return;
         }

         foreach (DataRow dr in dtBig.Rows) {
            int rpt_seq_no = dr["rpt_seq_no"].AsInt() - 1;
            int cp_seq_no = dr["cp_seq_no"].AsInt();
            string pcCode = dr["toi1_pc_code"].AsString();
            int rowIndex = rpt_seq_no + cp_seq_no;

            //pcCode 空值=期貨,C=call,P=put
            if (pcCode == "") rowIndex--;

            int colBegin = 0;
            for (int k = 4 ; k <= 7 ; k++) {
               if (pcCode == "C")
                  colBegin = -1;
               else if (pcCode == "P")
                  colBegin = 3;
               else
                  colBegin = 1;

               ws.Cells[rowIndex , colBegin + k - 2].Value = dr[k].AsDecimal();
            }//for (int k = 4;k <= 7;k++) {

         }//foreach(DataRow dr in dtBig.Rows) {

         showMsg(sheetName , sheetSubTitle , dtBig.Rows.Count.ToString());
      }

      /// <summary>
      /// 2.5三大法人－指數期貨
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="idt_date"></param>
      /// <param name="idt_last_date"></param>
      private void wf_30055_3(Worksheet ws , DateTime idt_date , DateTime idt_last_date) {
         string sheetName = "Sheet1";
         string sheetSubTitle = "三大法人－指數期貨";//主要指數期貨大額交易人未平倉部位一覽表+台指選擇權十大交易人未平倉部位一覽表
         labMsg.Text = string.Format("{0} 資料轉出中......" , sheetName);
         this.Refresh();

         //ken,這邊很特別,一次把兩張表格的資料都撈出,第一張是期貨,第二章是選擇權,然後一直在算位置填入,有夠亂,只要一調整,又會整個亂掉
         DataTable dtThree = dao30055.wf_30055_3(idt_date , idt_last_date);
         if (dtThree.Rows.Count == 0) {
            showMsg(sheetName , sheetSubTitle , "無資料");
            return;
         }

         foreach (DataRow dr in dtThree.Rows) {
            int rpt_seq_no = dr["rpt_seq_no"].AsInt() - 1;
            int cp_seq_no = dr["cp_seq_no"].AsInt();
            string pcCode = dr["pc_code"].AsString();
            string btinoivl3f_prodid = dr["btinoivl3f_prodid"].AsString();
            int rowIndex = rpt_seq_no + cp_seq_no;

            //pcCode 空值=期貨,C=call,P=put
            if (pcCode == "") rowIndex--;

            int colBegin = 0;
            for (int k = 4 ; k <= 7 ; k++) {
               if (pcCode == "C")
                  colBegin = -1;
               else if (pcCode == "P")
                  colBegin = 3;
               else
                  colBegin = 0;

               if (btinoivl3f_prodid == "TXO")
                  ws.Cells[rowIndex , colBegin + k - 2].Value = dr[k].AsDecimal();
               else
                  ws.Cells[rowIndex , colBegin + k - 1].Value = dr[k].AsDecimal();

            }//for (int k = 4;k <= 7;k++) {

         }//foreach(DataRow dr in dtThree.Rows) {

         showMsg(sheetName , sheetSubTitle , dtThree.Rows.Count.ToString());
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
   }
}