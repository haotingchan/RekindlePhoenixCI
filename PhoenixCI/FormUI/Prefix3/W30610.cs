using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/02/20
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30610 現、期貨市場振幅、波動度、成交量彙集
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30610 : FormParent {

      protected enum SheetNo {
         mon = 0,
         day = 1
      }
      #region 一般條件查詢縮寫

      /// <summary>
      /// 月明細(起)：yyyy/MM/01
      /// </summary>
      public DateTime FirstMonth {
         get {
            return DateTime.ParseExact((txtStartMonth.Text + "/01") , "yyyy/MM/dd" , null);
         }
      }

      /// <summary>
      /// 月明細(訖)：yyyy/MM/dd
      /// </summary>
      public DateTime EndMonth {
         get {
            return PbFunc.f_get_end_day("AI2" , "TXF" , txtEndMonth.Text); ;
         }
      }

      /// <summary>
      /// 日明細(起)：yyyy/MM/01
      /// </summary>
      public DateTime FirstDate {
         get {
            return DateTime.ParseExact((txtStartDate.Text) , "yyyy/MM/dd" , null);
         }
      }

      /// <summary>
      /// 日明細(訖)：yyyy/MM/dd
      /// </summary>
      public DateTime EndDate {
         get {
            return GlobalInfo.OCF_DATE;
         }
      }

      #endregion

      int ii_ole_row;
      private D30610 dao30610;

      public W30610(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         dao30610 = new D30610();
         this.Text = _ProgramID + "─" + _ProgramName;

         //txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
         //txtStartDate.DateTimeValue = new DateTime(GlobalInfo.OCF_DATE.Year , GlobalInfo.OCF_DATE.Month , 1); //取OCF_DATE月份的第1天
         txtStartMonth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/01"); //取OCF_DATE年的1月份
         txtEndMonth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
         txtStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01");
         txtEndDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");

         //winni test
         //2018/01-2018/10 月明細
         //2018/10/01-2018/10/11 日明細
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {
         try {
            lblProcessing.Visible = true;

            //1.複製檔案 & 開啟檔案 
            string excelDestinationPath = CopyExcelTemplateFile(_ProgramID , FileType.XLS);
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            //2.填資料            
            if (gbStatistics.EditValue.AsString() == "rbMon") {
               ii_ole_row = 4;
               wf_Export(workbook , SheetNo.mon , FirstMonth , EndMonth);
            } else {
               ii_ole_row = 1;
               wf_Export(workbook , SheetNo.day , FirstDate , EndDate);
            }

            //存檔
            workbook.SaveDocument(excelDestinationPath);
            lblProcessing.Visible = false;
            return ResultStatus.Success;
         } catch (Exception ex) {
            PbFunc.f_write_logf(_ProgramID , "Error" , ex.Message);
            MessageDisplay.Error(ex.Message);
         }
         return ResultStatus.Fail;
      }

      private void wf_Export(Workbook workbook , SheetNo sheetNo , DateTime as_symd , DateTime as_eymd) {
         try {
            string ls_rpt_name, ls_rpt_id;
            int li_ole_row_tol, li_ole_end_row;
            long ll_found;
            decimal ld_value;

            //1.讀取資料
            DataTable dtContent = new DataTable();
            if ((int)sheetNo == 0) {
               dtContent = dao30610.GetMonData(FirstMonth , EndMonth); ; //月明細表
            } else {
               dtContent = dao30610.GetDayData(FirstDate , EndDate); //日明細表
            }

            if (dtContent.Rows.Count == 0) {
               MessageDisplay.Info(string.Format("{0}~{1},{2},無任何資料!" , FirstMonth , EndMonth , this.Text));
            }

            //2.切換Sheet填資料
            Worksheet worksheet = workbook.Worksheets[(int)sheetNo];
            worksheet.Range["A1"].Select();
            worksheet.ScrollToRow(0);
            li_ole_row_tol = ii_ole_row + 180;

            if ((int)sheetNo == 0) {
               for (int i = 0 ; i < dtContent.Rows.Count ; i++) {
                  if (i == dtContent.Rows.Count) {
                     li_ole_end_row = ii_ole_row;
                     ii_ole_row = li_ole_row_tol;
                  } else {
                     ii_ole_row += 1;
                  }

                  worksheet.Cells[ii_ole_row - 1 , 0].Value = dtContent.Rows[i]["AMIF_YM"].AsString().SubStr(0 , 4).AsInt() - 1911 +
                                                                               dtContent.Rows[i]["AMIF_YM"].AsString().SubStr(4 , 2);
                  worksheet.Cells[ii_ole_row - 1 , 1].Value = dtContent.Rows[i]["AMIF_TOT_CNT"].AsInt();
                  worksheet.Cells[ii_ole_row - 1 , 2].Value = dtContent.Rows[i]["TFXM_AVG_UP_DOWN"].AsDecimal();
                  worksheet.Cells[ii_ole_row - 1 , 4].Value = dtContent.Rows[i]["TFXM_CNT"].AsInt();
                  worksheet.Cells[ii_ole_row - 1 , 5].Value = dtContent.Rows[i]["RETURN_P2"].AsDecimal() * 100;
                  worksheet.Cells[ii_ole_row - 1 , 6].Value = dtContent.Rows[i]["TFXM_AVG_CLOSE_PRICE"].AsDecimal();
                  worksheet.Cells[ii_ole_row - 1 , 7].Value = dtContent.Rows[i]["TFXM_M_QNTY_TAL"].AsDecimal();
                  worksheet.Cells[ii_ole_row - 1 , 8].Value = dtContent.Rows[i]["AMIF_AVG_UP_DOWN"].AsDecimal();
                  worksheet.Cells[ii_ole_row - 1 , 10].Value = dtContent.Rows[i]["AMIF_CNT"].AsInt();
                  worksheet.Cells[ii_ole_row - 1 , 11].Value = dtContent.Rows[i]["RETURN_P1"].AsDecimal() * 100;
                  worksheet.Cells[ii_ole_row - 1 , 12].Value = dtContent.Rows[i]["AMIF_AVG_CLOSE_PRICE"].AsDecimal();
                  worksheet.Cells[ii_ole_row - 1 , 13].Value = dtContent.Rows[i]["AI2_AVG_QTY_TXF"].AsDecimal();
                  worksheet.Cells[ii_ole_row - 1 , 14].Value = dtContent.Rows[i]["AI2_AVG_QTY_TXO"].AsDecimal();
                  worksheet.Cells[ii_ole_row - 1 , 15].Value = dtContent.Rows[i]["AI2_AVG_TOT_QTY"].AsDecimal();
               }
            } else {
               for(int i = 0 ; i < dtContent.Rows.Count ; i++) {
                  ii_ole_row += 1;
                  worksheet.Cells[ii_ole_row - 1 , 0].Value = dtContent.Rows[i]["AMIF_YM"].AsString();
                  worksheet.Cells[ii_ole_row - 1 , 1].Value = dtContent.Rows[i]["AMIF_YM"].AsString().SubStr(0 , 4).AsInt() - 1911 +
                                                                               dtContent.Rows[i]["AMIF_YM"].AsString().SubStr(4 , 2);
                  worksheet.Cells[ii_ole_row - 1 , 2].Value = dtContent.Rows[i]["AMIF_YM"].AsString().SubStr(0 , 4).AsInt() - 1911 +
                                                                               dtContent.Rows[i]["AMIF_YM"].AsString().SubStr(4 , 4);
                  worksheet.Cells[ii_ole_row - 1 , 3].Value = dtContent.Rows[i]["TFXM_UP_DOWN"].AsDecimal();

                  if (dtContent.Rows[i]["TFXM_UP_DOWN"].AsDecimal() < 100) {
                     worksheet.Cells[ii_ole_row - 1 , 4].Value = "Y";
                     worksheet.Cells[ii_ole_row - 1 , 13].Value = worksheet.Cells[ii_ole_row - 1 , 1].Value;
                  }

                  worksheet.Cells[ii_ole_row - 1 , 5].Value = dtContent.Rows[i]["TFXM_CLOSE_PRICE"].AsDecimal();
                  worksheet.Cells[ii_ole_row - 1 , 6].Value = dtContent.Rows[i]["TFXM_M_QNTY_TAL"].AsDecimal();
                  worksheet.Cells[ii_ole_row - 1 , 7].Value = dtContent.Rows[i]["AMIF_UP_DOWN"].AsDecimal();

                  if (dtContent.Rows[i]["AMIF_UP_DOWN"].AsDecimal() < 100) {
                     worksheet.Cells[ii_ole_row - 1 , 8].Value = "Y";
                     worksheet.Cells[ii_ole_row - 1 , 14].Value = worksheet.Cells[ii_ole_row - 1 , 1].Value;
                  }

                  worksheet.Cells[ii_ole_row - 1 , 9].Value = dtContent.Rows[i]["AMIF_CLOSE_PRICE"].AsDecimal();
                  worksheet.Cells[ii_ole_row - 1 , 10].Value = dtContent.Rows[i]["AI2_QTY_TXF"].AsDecimal();
                  worksheet.Cells[ii_ole_row - 1 , 11].Value = dtContent.Rows[i]["AI2_QTY_TXO"].AsDecimal();
                  worksheet.Cells[ii_ole_row - 1 , 12].Value = dtContent.Rows[i]["AI2_TOT_QTY"].AsDecimal();
               }
            }

            //刪除空白列
            if (ii_ole_row < li_ole_row_tol) {
               worksheet.Rows.Remove(ii_ole_row , li_ole_row_tol - ii_ole_row);
            }

         } catch (Exception ex) {
            PbFunc.f_write_logf(_ProgramID , "Error" , ex.Message);
            MessageDisplay.Error(ex.Message);
         }

      }

   }
}