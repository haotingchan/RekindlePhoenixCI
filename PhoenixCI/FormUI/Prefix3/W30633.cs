using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using System.Threading;

/// <summary>
/// Winni, 2019/04/16
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30633 期貨市場交易人結構日統計(交易量及未沖銷部位)
   /// </summary>
   public partial class W30633 : FormParent {

      protected enum SheetNo {
         vol = 0,    //交易量結構
         oi = 1      //OI結構
      }
      string ls_param_key, ls_sum_subtype, ls_market_code;
      private D30633 dao30633;
      protected const string RPT_NAME_VOL = "期貨市場交易人結構統計(交易量)";
      protected const string RPT_NAME_OI = "期貨市場交易人結構統計(未沖銷部位)";

      #region 一般交易查詢條件縮寫
      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string PrevStart {
         get {
            return txtPrevStartYM.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string PrevEnd {
         get {
            return txtPrevEndYM.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string AftStart {
         get {
            return txtAftStartYM.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string AftEnd {
         get {
            return txtAftEndYM.DateTimeValue.ToString("yyyyMMdd");
         }
      }
      #endregion

      public W30633(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao30633 = new D30633();
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            //1. 設定初始年月
            string maxDate = dao30633.getMaxDate(); //yyyyMMdd

            DateTime em_date = DateTime.ParseExact(maxDate , "yyyyMMdd" , null);

            txtAftEndYM.DateTimeValue = em_date;
            txtAftEndYM.EnterMoveNextControl = true;
            txtAftEndYM.Focus();

            txtAftStartYM.DateTimeValue = txtAftEndYM.DateTimeValue.AddDays(-6);
            txtAftStartYM.EnterMoveNextControl = true;
            txtAftStartYM.Focus();

            txtPrevEndYM.DateTimeValue = txtAftStartYM.DateTimeValue.AddDays(-1);
            txtPrevEndYM.EnterMoveNextControl = true;
            txtPrevEndYM.Focus();

            txtPrevStartYM.DateTimeValue = txtPrevEndYM.DateTimeValue.AddDays(-6);
            txtPrevStartYM.EnterMoveNextControl = true;
            txtPrevStartYM.Focus();

            //2. 設定dropdownlist
            //商品
            DataTable dtParamKey = new APDK_PARAM().ListAll2();//前面[% – 全部]+PARAM_KEY/PARAM_NAME/PARAM_PROD_TYPE/cp_display
            dwParamKey.SetDataTable(dtParamKey , "PARAM_KEY" , "CP_DISPLAY" , TextEditStyles.DisableTextEditor);
            dwParamKey.ItemIndex = 0;

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
         labMsg.Visible = true;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export() {
         base.Export();

         #region 日期檢核
         if (Int32.Parse(txtAftStartYM.Text.Replace("/" , "")) > Int32.Parse(txtAftEndYM.Text.Replace("/" , ""))) {
            MessageDisplay.Info(string.Format("後期起年月({0})不可大於迄年月({1})" , txtAftStartYM.Text.Replace("/" , "") ,
                                                                                    txtAftEndYM.Text.Replace("/" , "")));
            return ResultStatus.Fail;
         }
         if (Int32.Parse(txtPrevStartYM.Text.Replace("/" , "")) > Int32.Parse(txtPrevEndYM.Text.Replace("/" , ""))) {
            MessageDisplay.Info(string.Format("後期起年月({0})不可大於迄年月({1})" , txtPrevStartYM.Text.Replace("/" , "") ,
                                                                                    txtPrevEndYM.Text.Replace("/" , "")));
            return ResultStatus.Fail;
         }
         #endregion

         try {
            ShowMsg("開始轉檔...");

            string tempMarketCode;
            //RadioButton (gb_market_0 = 一般 / gb_market_1 = 盤後 / gb_market_All = 全部)
            if (gbMarket.EditValue.AsString() == "rb_market_0") {
               ls_market_code = "0%";
               tempMarketCode = "一般";
            } else if (gbMarket.EditValue.AsString() == "rb_market_1") {
               ls_market_code = "1%";
               tempMarketCode = "盤後";
            } else {
               ls_market_code = "%";
               tempMarketCode = "全部";
            }

            // Param_Key & SUM_SUBTYPE
            ls_param_key = dwParamKey.EditValue.AsString();
            if (ls_param_key == "%") {
               ls_sum_subtype = "0";   //全部
            } else {
               ls_sum_subtype = "3";   //各契約
               ls_param_key += "%";
            }

            //讀取資料
            DataTable dt = new DataTable();
            dt = dao30633.GetData(ls_market_code , PrevStart , PrevEnd , ls_sum_subtype , ls_param_key , AftStart , AftEnd);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}-{1}~{2}-{3},{4}–無任何資料!" , txtPrevStartYM.Text , txtPrevEndYM.Text ,
                                                                           txtAftStartYM.Text , txtAftEndYM.Text , _ProgramID));
               return ResultStatus.Fail;
            }

            //1.複製檔案 & 開啟檔案
            string originalFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH , _ProgramID + "." + FileType.XLS.ToString().ToLower());

            string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                _ProgramID + "_" + tempMarketCode + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss.") + FileType.XLS.ToString().ToLower());

            File.Copy(originalFilePath , destinationFilePath , true);

            Workbook workbook = new Workbook();
            workbook.LoadDocument(destinationFilePath);

            //2.填資料
            wf_Export(workbook , SheetNo.vol , dt , "30633" , RPT_NAME_VOL);  //function 30633_1
            wf_Export(workbook , SheetNo.oi , dt , "30634" , RPT_NAME_OI);   //function 30633_2

            //3.存檔
            workbook.SaveDocument(destinationFilePath);
            labMsg.Visible = false;

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

      private void wf_Export(Workbook workbook , SheetNo sheetNo , DataTable dt , string rptId , string rptName) {

         try {
            //切換Sheet
            Worksheet worksheet = workbook.Worksheets[(int)sheetNo];

            if (dwParamKey.EditValue.AsString() != "%") {
               worksheet.Cells[1 , 0].Value = "商品：" + dwParamKey.Text;
            }

            if (txtAftStartYM.Text == txtAftEndYM.Text) {
               worksheet.Cells[2 , 2].Value = PbFunc.f_conv_date(txtAftStartYM.DateTimeValue , 6);
            } else {
               worksheet.Cells[2 , 2].Value = PbFunc.f_conv_date(txtAftStartYM.DateTimeValue , 6) + " ～ " + PbFunc.f_conv_date(txtAftEndYM.DateTimeValue , 6);
            }

            int tmp = ((int)sheetNo == 0 ? 5 : 4);
            if (txtPrevStartYM.Text == txtPrevEndYM.Text) {
               worksheet.Cells[2 , tmp].Value = PbFunc.f_conv_date(txtPrevStartYM.DateTimeValue , 6);
            } else {
               worksheet.Cells[2 , tmp].Value = PbFunc.f_conv_date(txtPrevStartYM.DateTimeValue , 6) + " ～ " + PbFunc.f_conv_date(txtPrevEndYM.DateTimeValue , 6);
            }

            //只有成交量(Sheet1)需要執行這段
            if (sheetNo == 0) {
               if (gbMarket.EditValue.ToString() == "rb_market_0") {
                  worksheet.Cells[1 , 0].Value = "一般交易時段";
               } else if (gbMarket.EditValue.ToString() == "rb_market_1") {
                  worksheet.Cells[1 , 0].Value = "盤後交易時段";
               }

               for (int i = 0 ; i < dt.Rows.Count ; i++) {

                  int ii_ole_row = dt.Rows[i]["RPT_SEQ_NO"].AsInt();
                  worksheet.Cells[ii_ole_row - 1 , 2].Value = dt.Rows[i]["AM21_M_QNTY_AFT"].AsDecimal();
                  if (dt.Rows[i]["TRADE_DAYS_AFT"].AsDecimal() > 0) {
                     worksheet.Cells[ii_ole_row - 1 , 4].Value = dt.Rows[i]["AM21_M_QNTY_AFT"].AsDecimal() / dt.Rows[i]["TRADE_DAYS_AFT"].AsDecimal();
                  }
                  worksheet.Cells[ii_ole_row - 1 , 5].Value = dt.Rows[i]["AM21_M_QNTY_PREV"].AsDecimal();
                  if (dt.Rows[i]["TRADE_DAYS_PREV"].AsDecimal() > 0) {
                     worksheet.Cells[ii_ole_row - 1 , 7].Value = dt.Rows[i]["AM21_M_QNTY_PREV"].AsDecimal() / dt.Rows[i]["TRADE_DAYS_PREV"].AsDecimal();
                  }

               }
            } else {
               for (int i = 0 ; i < dt.Rows.Count ; i++) {
                  int ii_ole_row = dt.Rows[i]["RPT_SEQ_NO"].AsInt();
                  if (dt.Rows[i]["TRADE_DAYS_AFT"].AsDecimal() > 0) {
                     worksheet.Cells[ii_ole_row - 1 , 2].Value = dt.Rows[i]["AM21_OI_QNTY_AFT"].AsDecimal() / dt.Rows[i]["TRADE_DAYS_AFT"].AsDecimal();
                  }
                  if (dt.Rows[i]["TRADE_DAYS_PREV"].AsDecimal() > 0) {
                     worksheet.Cells[ii_ole_row - 1 , 4].Value = dt.Rows[i]["AM21_OI_QNTY_PREV"].AsDecimal() / dt.Rows[i]["TRADE_DAYS_PREV"].AsDecimal();
                  }
               }
            }

            worksheet.Range["A1"].Select();
            worksheet.ScrollToRow(0);

         } catch (Exception ex) {
            WriteLog(ex);
         }
         return;

      }

   }
}