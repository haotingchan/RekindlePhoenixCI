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
/// Winni, 2019/02/20
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30610 現、期貨市場振幅、波動度、成交量彙集
   /// </summary>
   public partial class W30610 : FormParent {

      protected enum SheetNo {
         mon = 0,
         day = 1
      }

      private D30610 dao30610;

      public W30610(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao30610 = new D30610();
      }

      protected override ResultStatus Open() {
         base.Open();
         try {
            //起始日期初始化           
            txtStartMonth.DateTimeValue = new DateTime(GlobalInfo.OCF_DATE.Year , 1 , 1);
            txtEndMonth.DateTimeValue = GlobalInfo.OCF_DATE;
            txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE.AddDays(-GlobalInfo.OCF_DATE.Day + 1); //取得當月第1天
            txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;

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

      protected void ShowMsg(string msg) {
         labMsg.Visible = true;
         labMsg.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export() {
         try {
            #region 輸入&日期檢核
            if (gbStatistics.EditValue.AsString() == "rbMon") {
               if (string.Compare(txtStartMonth.Text , txtEndMonth.Text) > 0) {
                  MessageDisplay.Error("月份起始年月不可小於迄止年月!" , GlobalInfo.ErrorText);
                  return ResultStatus.Fail;
               }
            } else {
               if (string.Compare(txtStartDate.Text , txtEndDate.Text) > 0) {
                  MessageDisplay.Error(CheckDate.Datedif , GlobalInfo.ErrorText);
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

            //1.複製檔案 & 開啟檔案 
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            //2.填資料    
            bool res = false;
            if (gbStatistics.EditValue.AsString() == "rbMon") {
               res = wf_30611(workbook);
            } else {
               res = wf_30612(workbook);
            }

            if (!res) {
               workbook = null;
               File.Delete(excelDestinationPath);
               return ResultStatus.Fail;
            }

            //存檔
            workbook.SaveDocument(excelDestinationPath);
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

      #region wf_30611
      private bool wf_30611(Workbook workbook) {
         try {
            string rptName = "每月現、期貨市場振幅、波動度、成交量彙集";
            string rptId = "30611";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            Worksheet worksheet = workbook.Worksheets[(int)SheetNo.mon]; //切換sheet

            int ii_ole_row = 4;
            int li_ole_row_tol = 184;

            //每月
            DateTime endDay = PbFunc.f_get_end_day("AI2" , "TXF" , txtEndMonth.Text);
            DataTable dtContent = dao30610.GetMonData(txtStartMonth.DateTimeValue , endDay); ; //月明細表
            if (dtContent.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},{2}-{3},無任何資料!" , txtStartMonth.Text , txtEndMonth.Text , rptId , rptName) , "處理結果");
               return false;
            }

            int li_ole_end_row = 0;
            for (int w = 1 ; w <= dtContent.Rows.Count ; w++) {
               if (w == dtContent.Rows.Count) {
                  li_ole_end_row = ii_ole_row;
                  ii_ole_row = li_ole_row_tol;
               } else {
                  ii_ole_row++;
               }

               worksheet.Cells[ii_ole_row - 1 , 0].Value = dtContent.Rows[w - 1]["AMIF_YM"].AsString().SubStr(0 , 4).AsInt() - 1911 +
                                                                               dtContent.Rows[w - 1]["AMIF_YM"].AsString().SubStr(4 , 2);
               worksheet.Cells[ii_ole_row - 1 , 1].Value = dtContent.Rows[w - 1]["AMIF_TOT_CNT"].AsInt();
               worksheet.Cells[ii_ole_row - 1 , 2].Value = dtContent.Rows[w - 1]["TFXM_AVG_UP_DOWN"].AsDecimal();
               worksheet.Cells[ii_ole_row - 1 , 4].Value = dtContent.Rows[w - 1]["TFXM_CNT"].AsInt();
               worksheet.Cells[ii_ole_row - 1 , 5].Value = dtContent.Rows[w - 1]["RETURN_P2"].AsDecimal() * 100;
               worksheet.Cells[ii_ole_row - 1 , 6].Value = dtContent.Rows[w - 1]["TFXM_AVG_CLOSE_PRICE"].AsDecimal();
               worksheet.Cells[ii_ole_row - 1 , 7].Value = dtContent.Rows[w - 1]["TFXM_M_QNTY_TAL"].AsDecimal();
               worksheet.Cells[ii_ole_row - 1 , 8].Value = dtContent.Rows[w - 1]["AMIF_AVG_UP_DOWN"].AsDecimal();
               worksheet.Cells[ii_ole_row - 1 , 10].Value = dtContent.Rows[w - 1]["AMIF_CNT"].AsInt();
               worksheet.Cells[ii_ole_row - 1 , 11].Value = dtContent.Rows[w - 1]["RETURN_P1"].AsDecimal() * 100;
               worksheet.Cells[ii_ole_row - 1 , 12].Value = dtContent.Rows[w - 1]["AMIF_AVG_CLOSE_PRICE"].AsDecimal();
               worksheet.Cells[ii_ole_row - 1 , 13].Value = dtContent.Rows[w - 1]["AI2_AVG_QTY_TXF"].AsDecimal();
               worksheet.Cells[ii_ole_row - 1 , 14].Value = dtContent.Rows[w - 1]["AI2_AVG_QTY_TXO"].AsDecimal();
               worksheet.Cells[ii_ole_row - 1 , 15].Value = dtContent.Rows[w - 1]["AI2_AVG_TOT_QTY"].AsDecimal();
            }

            //刪除空白列
            //if (li_ole_end_row < li_ole_row_tol) {
            //   worksheet.Rows.Remove(ii_ole_row , li_ole_row_tol - ii_ole_row);
            //}
            if (li_ole_row_tol > dtContent.Rows.Count + 4) {
               Range ra = worksheet.Range[(dtContent.Rows.Count + 4).AsString() + ":183"];
               ra.Delete(DeleteMode.EntireRow);
            }

            worksheet.Range["A1"].Select();
            worksheet.ScrollToRow(0);
            return true;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return false;
      }
      #endregion

      #region wf_30612
      private bool wf_30612(Workbook workbook) {
         try {
            string rptName = "每日現、期貨市場振幅、波動度、成交量彙集";
            string rptId = "30612";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            Worksheet worksheet = workbook.Worksheets[(int)SheetNo.day]; //切換sheet

            //每日
            DataTable dtContent = dao30610.GetDayData(txtStartDate.DateTimeValue , txtEndDate.DateTimeValue); ; //日明細表
            if (dtContent.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},{2}-{3},無任何資料!" , txtStartDate.Text , txtEndDate.Text , rptId , rptName) , "處理結果");
               return false;
            }

            int ii_ole_row = 1;
            for (int w = 0 ; w < dtContent.Rows.Count ; w++) {
               ii_ole_row += 1;
               worksheet.Cells[ii_ole_row - 1 , 0].Value = dtContent.Rows[w]["AMIF_YM"].AsString();
               worksheet.Cells[ii_ole_row - 1 , 1].Value = dtContent.Rows[w]["AMIF_YM"].AsString().SubStr(0 , 4).AsInt() - 1911 +
                                                                            dtContent.Rows[w]["AMIF_YM"].AsString().SubStr(4 , 2);
               worksheet.Cells[ii_ole_row - 1 , 2].Value = dtContent.Rows[w]["AMIF_YM"].AsString().SubStr(0 , 4).AsInt() - 1911 +
                                                                            dtContent.Rows[w]["AMIF_YM"].AsString().SubStr(4 , 4);
               worksheet.Cells[ii_ole_row - 1 , 3].Value = dtContent.Rows[w]["TFXM_UP_DOWN"].AsDecimal();

               if (dtContent.Rows[w]["TFXM_UP_DOWN"].AsDecimal() < 100) {
                  worksheet.Cells[ii_ole_row - 1 , 4].Value = "Y";
                  worksheet.Cells[ii_ole_row - 1 , 13].Value = worksheet.Cells[ii_ole_row - 1 , 1].Value;
               }

               worksheet.Cells[ii_ole_row - 1 , 5].Value = dtContent.Rows[w]["TFXM_CLOSE_PRICE"].AsDecimal();
               worksheet.Cells[ii_ole_row - 1 , 6].Value = dtContent.Rows[w]["TFXM_M_QNTY_TAL"].AsDecimal();
               worksheet.Cells[ii_ole_row - 1 , 7].Value = dtContent.Rows[w]["AMIF_UP_DOWN"].AsDecimal();

               if (dtContent.Rows[w]["AMIF_UP_DOWN"].AsDecimal() < 100) {
                  worksheet.Cells[ii_ole_row - 1 , 8].Value = "Y";
                  worksheet.Cells[ii_ole_row - 1 , 14].Value = worksheet.Cells[ii_ole_row - 1 , 1].Value;
               }

               worksheet.Cells[ii_ole_row - 1 , 9].Value = dtContent.Rows[w]["AMIF_CLOSE_PRICE"].AsDecimal();
               worksheet.Cells[ii_ole_row - 1 , 10].Value = dtContent.Rows[w]["AI2_QTY_TXF"].AsDecimal();
               worksheet.Cells[ii_ole_row - 1 , 11].Value = dtContent.Rows[w]["AI2_QTY_TXO"].AsDecimal();
               worksheet.Cells[ii_ole_row - 1 , 12].Value = dtContent.Rows[w]["AI2_TOT_QTY"].AsDecimal();
            }

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return false;
      }
      #endregion
   }
}