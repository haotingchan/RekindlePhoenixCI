using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;

//TODO: Filter的部分尚未轉為SQL

/// <summary>
/// Winni, 2019/01/23
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30633 期貨市場交易人結構日統計(交易量及未沖銷部位)
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30633 : FormParent {

      protected enum SheetNo {
         vol = 0,    //交易量結構
         oi = 1      //OI結構
      }
      string ls_param_key, ls_sum_subtype, ls_market_code;
      private D30633 dao30633;

      public W30633(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         dao30633 = new D30633();
         this.Text = _ProgramID + "─" + _ProgramName;

         txtPrevStartYM.Text = DateTime.Parse(GlobalInfo.OCF_DATE.ToString("yyyy/MM/01")).AddMonths(-1).ToString("yyyy/MM/01");
         txtPrevEndYM.Text = DateTime.Parse(txtPrevStartYM.Text).AddDays(6).ToString("yyyy/MM/dd");
         txtAftStartYM.Text = DateTime.Parse(txtPrevEndYM.Text).AddDays(1).ToString("yyyy/MM/dd");
         txtAftEndYM.Text = DateTime.Parse(txtAftStartYM.Text).AddDays(6).ToString("yyyy/MM/dd");

         //商品 dropdownList
         DataTable dtParamKey = new APDK_PARAM().ListAll2();//前面[% – 全部]+PARAM_KEY/PARAM_NAME/PARAM_PROD_TYPE/cp_display
         dw_paramKey.SetDataTable(dtParamKey , "PARAM_KEY");

      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {

         base.Export();
         lblProcessing.Visible = true;
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

         string tempMarketCode;
         //RadioButton (gb_market_0 = 一般 / gb_market_1 = 盤後 / gb_market_All = 全部)
         if (gb_market.EditValue.ToString() == "gb_market_0") {
            ls_market_code = "0%";
            tempMarketCode = "一般";
         } else if (gb_market.EditValue.ToString() == "gb_market_1") {
            ls_market_code = "1%";
            tempMarketCode = "盤後";
         } else {
            ls_market_code = "%";
            tempMarketCode = "全部";
         }

         // Param_Key & SUM_SUBTYPE
         ls_param_key = dw_paramKey.EditValue.AsString().Trim();
         if (ls_param_key == "%") {
            ls_sum_subtype = "0";   //全部
         } else {
            ls_sum_subtype = "3";   //各契約
            ls_param_key += "%";
         }

         //1.複製檔案 & 開啟檔案 (兩張報表都輸出到同一份excel,所以提出來)
         string originalFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH , _ProgramID + "." + FileType.XLS.ToString().ToLower());

         string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
             _ProgramID + "_" + tempMarketCode + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH.mm.ss") + "." + FileType.XLS.ToString().ToLower());

         File.Copy(originalFilePath , destinationFilePath , true);

         Workbook workbook = new Workbook();
         workbook.LoadDocument(destinationFilePath);

         //2.填資料
         bool result1 = false, result2 = false;
         result1 = wf_Export(workbook , SheetNo.vol);  //function 30633_1
         result2 = wf_Export(workbook , SheetNo.oi);   //function 30633_2

         if (!result1 && !result2) {
            try {
               workbook = null;
               System.IO.File.Delete(destinationFilePath);
            } catch (Exception) {
               //
            }
            return ResultStatus.Fail;
         }

         //3.存檔
         workbook.SaveDocument(destinationFilePath);
         lblProcessing.Visible = false;

         return ResultStatus.Success;
      }

      private bool wf_Export(Workbook workbook , SheetNo sheetNo) {

         try {
            string prevStartYM = txtPrevStartYM.Text.Replace("/" , "");
            string prevEndYM = txtPrevEndYM.Text.Replace("/" , "");
            string AftStartYM = txtAftStartYM.Text.Replace("/" , "");
            string AftEndYM = txtAftEndYM.Text.Replace("/" , "");

            //讀取資料
            DataTable dt = new DataTable();
            dt = dao30633.GetData(ls_market_code , prevStartYM , prevEndYM , ls_sum_subtype ,
                                            ls_param_key , AftStartYM , AftEndYM);
            if (dt.Rows.Count == 0) {
               MessageDisplay.Info(string.Format("{0}-{1}~{2}-{3},{4}–無任何資料!" , txtPrevStartYM.Text , txtPrevEndYM.Text ,
                                                                           txtAftStartYM.Text , txtAftEndYM.Text , this.Text));
               return false;
            }

            //切換Sheet
            Worksheet worksheet = workbook.Worksheets[(int)sheetNo];

            if (dw_paramKey.EditValue.AsString().Trim() != "%") {
               worksheet.Cells[1 , 0].Value = "商品：" + dw_paramKey.Text;
            }
            if (txtAftStartYM.Text == txtAftEndYM.Text) {
               worksheet.Cells[2 , 2].Value = PbFunc.f_conv_date(txtAftStartYM.DateTimeValue , 6);
            } else {
               worksheet.Cells[2 , 2].Value = PbFunc.f_conv_date(txtAftStartYM.DateTimeValue , 6) + "～" + PbFunc.f_conv_date(txtAftEndYM.DateTimeValue , 6);
            }
            if (txtPrevStartYM.Text == txtPrevEndYM.Text) {
               worksheet.Cells[2 , 5].Value = PbFunc.f_conv_date(txtPrevStartYM.DateTimeValue , 6);
            } else {
               worksheet.Cells[2 , 5].Value = PbFunc.f_conv_date(txtPrevStartYM.DateTimeValue , 6) + "～" + PbFunc.f_conv_date(txtPrevEndYM.DateTimeValue , 6);
            }

            //只有成交量(Sheet1)需要執行這段
            if (sheetNo == 0) {
               if (gb_market.EditValue.ToString() == "gb_market_0") {
                  worksheet.Cells[1 , 0].Value = "一般交易時段";
               } else if (gb_market.EditValue.ToString() == "gb_market_1") {
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
            return true;
         } catch (Exception ex) { //失敗寫LOG
            PbFunc.f_write_logf(_ProgramID , "error" , ex.Message);
            return false;
         }


      }

   }
}