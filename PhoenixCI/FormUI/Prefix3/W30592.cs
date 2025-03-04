﻿using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/02/11
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30592 匯率類期貨交易概況表
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30592 : FormParent {

      protected D30592 dao30592;
      private int flag;

      public W30592(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao30592 = new D30592();
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            txtStartYMD.DateTimeValue = GlobalInfo.OCF_DATE.AddDays(-GlobalInfo.OCF_DATE.Day + 1); //取得當月第1天
            txtEndYMD.DateTimeValue = GlobalInfo.OCF_DATE;

            DataTable dtProd = new CODW().ListLookUpEdit("30592" , "KIND_ID");
            Extension.SetDataTable(ddlProd , dtProd , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , "");
            ddlProd.ItemIndex = 0;

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
            if (string.Compare(txtStartYMD.Text , txtEndYMD.Text) > 0) {
               MessageDisplay.Error(CheckDate.Datedif , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            #endregion

            //0. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            ///*************************************
            //   chkGroup.Items[0] = MonQnty
            //   chkGroup.Items[1] = OI
            //   chkGroup.Items[2] = MonCnt
            //   chkGroup.Items[3] = Amt
            //   chkGroup.Items[4] = Acc
            //   chkGroup.Items[5] = Id
            //   chkGroup.Items[6] = Rmb
            //*************************************/

            //1. 判斷是否至少勾選一個選項
            if (chkGroup.CheckedItemsCount < 1) {
               MessageDisplay.Warning("請勾選至少一個選項!" , GlobalInfo.WarningText);
               return ResultStatus.Fail;
            } else {
               string tempMarketCode = "";
               //RadioButton (rbMarket0 = 一般 / rbMarket1 = 盤後 / rbMarketAll = 全部)
               if (gbMarket.EditValue.AsString() == "rbMarket0") {
                  tempMarketCode = "一般";
               } else if (gbMarket.EditValue.AsString() == "rbMarket1") {
                  tempMarketCode = "盤後";
               } else {
                  tempMarketCode = "全部";
               }

               flag = 0;
               //2. 複製檔案 & 開啟檔案 (因檔案需因MarketCode更動，所以另外寫)
               string originalFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH , _ProgramID + "." + FileType.XLSX.ToString().ToLower());

               string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                   _ProgramID + "_" + tempMarketCode + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH.mm.ss") + "." + FileType.XLSX.ToString().ToLower());

               File.Copy(originalFilePath , destinationFilePath , true);

               Workbook workbook = new Workbook();
               workbook.LoadDocument(destinationFilePath);

               if (chkGroup.CheckedItemsCount == 1) {
                  foreach (CheckedListBoxItem item in chkGroup.Items) {
                     if (item.Value.AsString() == "chkRmb") {
                        File.Delete(destinationFilePath);
                     }
                  }
               }

               foreach (CheckedListBoxItem item in chkGroup.Items) {
                  if (item.CheckState == CheckState.Unchecked) continue;
                  switch (item.Value) {
                     case "chkRmb":
                        wf_30592_RMB();
                        break;
                     default:
                        //3. 填資料
                        bool result = false;
                        result = wf_30592(workbook , destinationFilePath);  //function 30592

                        if (!result) {
                           try {
                              workbook = null;
                              File.Delete(destinationFilePath);
                           } catch (Exception ex) {
                              WriteLog(ex);
                           }
                           return ResultStatus.Fail;
                        } else {
                           flag++;
                        }
                        break;
                  }
               }//foreach (CheckedListBoxItem item in chkGroup.Items)

               if (flag <= 0) {
                  MessageDisplay.Info(MessageDisplay.MSG_NO_DATA , GlobalInfo.ResultText);
               }

               //3.存檔改寫在Function內
               labMsg.Visible = false;
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

      #region wf_30592
      private bool wf_30592(Workbook workbook , string destinationFilePath) {

         try {
            string rptName = "臺股期貨交易概況表";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , _ProgramID , rptName));

            Worksheet worksheet = workbook.Worksheets[0]; //切換sheet

            #region 交易時段
            string ls_market_code = "";
            if (gbMarket.EditValue.AsString() == "rbMarket0") {
               ls_market_code = "0";
            } else if (gbMarket.EditValue.ToString() == "rbMarket1") {
               ls_market_code = "1";
            } else {
               ls_market_code = "%";
            }

            string StartYMD = txtStartYMD.DateTimeValue.ToString("yyyyMMdd");
            string EndYMD = txtEndYMD.DateTimeValue.ToString("yyyyMMdd");

            DataTable dt = dao30592.GetData(StartYMD , EndYMD , ls_market_code);
            if (dt.Rows.Count <= 0) {
               ShowMsg(string.Format("{0},{1}－{2},無任何資料!" , txtStartYMD.DateTimeValue.ToString("yyyyMM") , _ProgramID , rptName));
               MessageDisplay.Info(string.Format("{0},{1}－{2},(市場總成交量雙邊(A)無任何資料!" , txtStartYMD.DateTimeValue.ToString("yyyyMM") , _ProgramID , rptName) , GlobalInfo.ResultText);
               workbook = null;
               File.Delete(destinationFilePath);
               return false;
            }

            DataTable dtFilter = new DataTable();
            //全部 or 單一商品
            string prod = ddlProd.Text.SubStr(0 , 1);
            if (prod != "%") {
               dtFilter = dt.Filter("apdk_param_key ='" + ddlProd.Text + "'"); //單一商品
            } else {
               dtFilter = dt.Copy();
            }
            //要再判斷一次
            if (dtFilter.Rows.Count <= 0) {
               ShowMsg(string.Format("{0},{1}－{2},無任何資料!" , txtStartYMD.DateTimeValue.ToString("yyyyMM") , _ProgramID , rptName));
               MessageDisplay.Info(string.Format("{0},{1}－{2},(市場總成交量雙邊(A)無任何資料!" , txtStartYMD.DateTimeValue.ToString("yyyyMM") , _ProgramID , rptName) , GlobalInfo.ResultText);
               workbook = null;
               File.Delete(destinationFilePath);
               return false;
            }
            #endregion

            #region 表頭      
            int ii_ole_row = 1;
            int li_col = 2;

            foreach (CheckedListBoxItem item in chkGroup.Items) {
               if (item.CheckState == CheckState.Checked) {
                  switch (item.Value) {
                     case "chkMonQnty":
                        li_col += 1;
                        worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "總交易量 [(買+賣)/2]";
                        break;
                     case "chkOI":
                        li_col += 1;
                        worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "未平倉量[(買+賣)/2]";
                        break;
                     case "chkMonCnt":
                        li_col += 1;
                        worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "成交筆數(買+賣)";
                        break;
                     case "chkAmt":
                        li_col += 1;
                        worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "成交金額(台幣) [(買+賣)/2]";
                        li_col += 1;
                        worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "成交金額(原始幣別) [(買+賣)/2]";
                        break;
                     case "chkAcc":
                        li_col += 1;
                        worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "交易戶數(買+賣)";
                        break;
                     case "chkId":
                        li_col += 1;
                        worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "交易人數(ID數)(買+賣)";
                        break;
                  }
               }
            }//foreach (CheckedListBoxItem item in chkGroup.Items)
            #endregion

            #region 內容
            foreach (DataRow dr in dtFilter.Rows) {
               ii_ole_row++;
               worksheet.Cells[ii_ole_row - 1 , 0].Value = dr["APDK_YMD"].AsString();
               worksheet.Cells[ii_ole_row - 1 , 1].Value = dr["APDK_PARAM_KEY"].AsString();

               li_col = 2;

               foreach (CheckedListBoxItem item in chkGroup.Items) {
                  if (item.CheckState == CheckState.Checked) {
                     switch (item.Value) {
                        case "chkMonQnty":
                           li_col++;
                           if (dr["AI2_M_QNTY"] != DBNull.Value)
                              worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dr["AI2_M_QNTY"].AsDecimal();
                           break;
                        case "chkOI":
                           li_col++;
                           if (dr["AI2_OI"] != DBNull.Value)
                              worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dr["AI2_OI"].AsDecimal();
                           break;
                        case "chkMonCnt":
                           li_col++;
                           if (dr["AM10_CNT"] != DBNull.Value)
                              worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dr["AM10_CNT"].AsDecimal();
                           break;
                        case "chkAmt":
                           li_col++;
                           if (dr["AA2_AMT"] != DBNull.Value)
                              worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dr["AA2_AMT"].AsDecimal();
                           li_col++;
                           if (dr["AA2_AMT_ORG_CURRENCY"] != DBNull.Value)
                              worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dr["AA2_AMT_ORG_CURRENCY"].AsDecimal();
                           break;
                        case "chkAcc":
                           li_col++;
                           if (dr["AM9_ACC_CNT"] != DBNull.Value)
                              worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dr["AM9_ACC_CNT"].AsDecimal();
                           break;
                        case "chkId":
                           li_col++;
                           if (dr["AB4_ID_CNT"] != DBNull.Value)
                              worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dr["AB4_ID_CNT"].AsDecimal();
                           break;
                     }
                  }
               }//foreach (CheckedListBoxItem item in chkGroup.Items)
            }//foreach (DataRow dr in dtFilter.Rows)
            #endregion

            worksheet.Range["A1"].Select();
            worksheet.ScrollToRow(0);
            workbook.SaveDocument(destinationFilePath);
            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            labMsg.Visible = false;
            return false;
         }
      }
      #endregion

      #region wf_30592_RMB
      private void wf_30592_RMB() {
         try {
            string rptName = "各交易所RMB期貨交易量";
            string rptId = "30592_rmb";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , rptId , rptName));

            //1.複製檔案 & 開啟檔案
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID + "_RMB");
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);
            Worksheet worksheet = workbook.Worksheets[0]; //切換sheet

            //2.讀取資料(每日)
            string StartYMD = txtStartYMD.DateTimeValue.ToString("yyyyMMdd");
            string EndYMD = txtEndYMD.DateTimeValue.ToString("yyyyMMdd");

            DataTable dt = dao30592.GetRmbData(StartYMD , EndYMD);
            if (dt.Rows.Count <= 0) {
               ShowMsg(string.Format("{0},{1}－{2},無任何資料!" , txtStartYMD.Text , rptId , rptName));
               MessageDisplay.Info(string.Format("{0}～{1},{2}－{3}, RHF&RTF無成交量資料!" , txtStartYMD.Text , txtEndYMD.Text , rptId , rptName) , GlobalInfo.ResultText);
               workbook = null;
               File.Delete(excelDestinationPath);
               return;
            }

            int ii_ole_row = 4;
            for (int i = 0 ; i < dt.Rows.Count ; i++) {
               ii_ole_row += 1;
               for (int j = 0 ; j < 6 ; j++) {
                  if (j == 0) {
                     DateTime d = dt.Rows[i][j].AsDateTime();
                     worksheet.Cells[ii_ole_row - 1 , j].Value = string.Format("{0}/{1}/{2}" ,
                                                                                 d.Year ,
                                                                                 d.Month ,
                                                                                 d.Day);
                  } else {
                     if (String.IsNullOrEmpty(dt.Rows[i][j].AsString()))
                        worksheet.Cells[ii_ole_row - 1 , j].Value = "-";
                     else
                        worksheet.Cells[ii_ole_row - 1 , j].Value = dt.Rows[i][j].AsDecimal();
                  }
               }

               //RHF,RTF沒交易
               if (string.IsNullOrEmpty(dt.Rows[i]["rhf_vol"].AsString()))
                  worksheet.Cells[ii_ole_row - 1 , 6].Value = "-";
            }

            //3.刪除空白列
            if (dt.Rows.Count < 100) {
               worksheet.Rows.Remove(ii_ole_row + 1 , 120 - (dt.Rows.Count + 20)); //原dt的row數再留20行
            }
            worksheet.Range["A1"].Select();
            worksheet.ScrollToRow(0);

            //4.存檔
            workbook.SaveDocument(excelDestinationPath);
            flag++;
         } catch (Exception ex) {
            WriteLog(ex);
            labMsg.Visible = false;
            return;
         }
      }
      #endregion

      private void ddlProd_EditValueChanged(object sender , EventArgs e) {
         if (ddlProd.Text.AsString() == "RHF" || ddlProd.Text.AsString() == "RTF" || ddlProd.Text.AsString() == "%(全部)") {
            chkGroup.Items[6].Enabled = true;
         } else {
            chkGroup.Items[6].Enabled = false;
            chkGroup.Items[6].CheckState = CheckState.Unchecked;
         }
      }

   }
}