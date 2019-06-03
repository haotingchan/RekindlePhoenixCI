﻿using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/01/31
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30591 小型臺股期貨交易概況表
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30591 : FormParent {

      private D30591 dao30591;

      public W30591(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao30591 = new D30591();
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            //設定初始年月yyyy/MM/dd     
            txtStartYMD.DateTimeValue = DateTime.ParseExact(GlobalInfo.OCF_DATE.ToString("yyyy/MM/01") , "yyyy/MM/dd" , null);
            txtStartYMD.EnterMoveNextControl = true;
            txtStartYMD.Focus();

            txtEndYMD.DateTimeValue = GlobalInfo.OCF_DATE;
            txtEndYMD.EnterMoveNextControl = true;
            txtEndYMD.Focus();

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

      protected override ResultStatus Export() {

         try {

            //0. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            string tempMarketCode;
            //RadioButton (rbMarket0 = 一般 / rbMarket1 = 盤後 / rbMarketAll = 全部)
            if (gbMarket.EditValue.ToString() == "rbMarket0") {
               tempMarketCode = "一般";
            } else if (gbMarket.EditValue.ToString() == "rbMarket1") {
               tempMarketCode = "盤後";
            } else {
               tempMarketCode = "全部";
            }

            //1.複製檔案 & 開啟檔案 (因檔案需因MarketCode更動，所以另外寫)
            string originalFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH , _ProgramID + "." + FileType.XLS.ToString().ToLower());

            string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                _ProgramID + "_" + tempMarketCode + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss.") + FileType.XLS.ToString().ToLower());

            File.Copy(originalFilePath , destinationFilePath , true);

            Workbook workbook = new Workbook();
            workbook.LoadDocument(destinationFilePath);
            Worksheet worksheet = workbook.Worksheets[0];

            //2.填資料
            bool result = false;
            result = wf_Export(workbook , worksheet);  //function 30591

            if (!result) {
               workbook = null;
               File.Delete(destinationFilePath);
               return ResultStatus.Fail;
            }

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

      //function wf_30591
      private bool wf_Export(Workbook workbook , Worksheet worksheet) {

         try {
            /*************************************
            ls_year = 年
            li_ole_row_tol = 總列數
            li_row_start = 開始列
            *************************************/
            string ls_kind_id2, ls_pc_code, ls_expiry_type, ls_prod_type, ls_param_key, ls_market_code;
            int li_col, ii_ole_row = 1;

            #region 讀取資料(每日)
            ls_prod_type = "F";
            ls_param_key = "MXF";
            if (gbContract.EditValue.ToString() == "rbContractAll" || gbContract.EditValue.ToString() == "rbContractAllTot") {
               ls_kind_id2 = "%";
            } else {
               if (gbContract.EditValue.ToString() == "rbContractTxw") {
                  ls_kind_id2 = "MXW%";
               } else {
                  ls_kind_id2 = "MXF%";
               }
            }

            ls_pc_code = "%";

            if (gbContract.EditValue.ToString() == "rbContractAllTot") {
               ls_expiry_type = "9";
            } else {
               ls_expiry_type = "%";
            }
            #endregion

            #region 交易時段
            if (gbMarket.EditValue.ToString() == "rbMarket0") {
               ls_market_code = "0";
            } else if (gbMarket.EditValue.ToString() == "rbMarket1") {
               ls_market_code = "1";
            } else {
               ls_market_code = "%";
            }

            string StartYMD = txtStartYMD.Text.Replace("/" , "");
            string EndYMD = txtEndYMD.Text.Replace("/" , "");

            DataTable dt = new DataTable();
            dt = dao30591.GetData(ls_expiry_type , ls_pc_code , ls_param_key , ls_kind_id2 ,
                                  StartYMD , EndYMD , ls_prod_type , ls_market_code);
            if (dt.Rows.Count <= 0) {

               labMsg.Text = String.Format("{0}:yyyymm" , StartYMD) + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!";
               MessageDisplay.Info(string.Format("{0:yyyymm},{1}–市場總成交量雙邊(A)無任何資料!" , StartYMD , this.Text));
               return false;
            }
            #endregion

            #region 表頭      
            ii_ole_row = 1;
            li_col = 3;

            /*************************************
            chkGroup.Items[0] = MonQnty
            chkGroup.Items[1] = OI
            chkGroup.Items[2] = MonCnt
            chkGroup.Items[3] = Amt
            chkGroup.Items[4] = Acc
            chkGroup.Items[5] = Id
            *************************************/
            if (chkGroup.Items[0].CheckState.ToString() == "Checked") {
               li_col += 1;
               worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "總交易量 [(買+賣)/2]";
            }

            if (chkGroup.Items[1].CheckState.ToString() == "Checked") {
               li_col += 1;
               worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "未平倉量[(買+賣)/2]";
            }

            if (chkGroup.Items[2].CheckState.ToString() == "Checked") {
               li_col += 1;
               worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "成交筆數(買+賣)";
            }

            if (chkGroup.Items[3].CheckState.ToString() == "Checked") {
               li_col += 1;
               worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "成交金額 [(買+賣)/2]";
            }

            if (chkGroup.Items[4].CheckState.ToString() == "Checked") {
               li_col += 1;
               worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "交易戶數(買+賣)";
            }

            if (chkGroup.Items[5].CheckState.ToString() == "Checked") {
               li_col += 1;
               worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "交易人數(ID數)(買+賣)";
            }
            #endregion

            #region 內容
            for (int i = 0 ; i < dt.Rows.Count ; i++) {
               ii_ole_row += 1;
               worksheet.Cells[ii_ole_row - 1 , 0].Value = dt.Rows[i]["APDK_YMD"].AsString();
               worksheet.Cells[ii_ole_row - 1 , 1].Value = dt.Rows[i]["APDK_PARAM_KEY"].AsString();
               worksheet.Cells[ii_ole_row - 1 , 2].Value = dt.Rows[i]["APDK_EXPIRY_TYPE"].AsString();

               li_col = 3;

               if (chkGroup.Items[0].CheckState.ToString() == "Checked") {
                  li_col += 1;
                  if (dt.Rows[i]["AI2_M_QNTY"] != DBNull.Value)
                     worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dt.Rows[i]["AI2_M_QNTY"].AsDecimal();
               }

               if (chkGroup.Items[1].CheckState.ToString() == "Checked") {
                  li_col += 1;
                  if (dt.Rows[i]["AI2_OI"] != DBNull.Value)
                     worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dt.Rows[i]["AI2_OI"].AsDecimal();
               }

               if (chkGroup.Items[2].CheckState.ToString() == "Checked") {
                  li_col += 1;
                  if (dt.Rows[i]["AM10_CNT"] != DBNull.Value)
                     worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dt.Rows[i]["AM10_CNT"].AsDecimal();
               }

               if (chkGroup.Items[3].CheckState.ToString() == "Checked") {
                  li_col += 1;
                  if (dt.Rows[i]["AA2_AMT"] != DBNull.Value)
                     worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dt.Rows[i]["AA2_AMT"].AsDecimal();
               }

               if (chkGroup.Items[4].CheckState.ToString() == "Checked") {
                  li_col += 1;
                  if (dt.Rows[i]["AM9_ACC_CNT"] != DBNull.Value)
                     worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dt.Rows[i]["AM9_ACC_CNT"].AsDecimal();
               }

               if (chkGroup.Items[5].CheckState.ToString() == "Checked") {
                  li_col += 1;
                  if (dt.Rows[i]["AB4_ID_CNT"] != DBNull.Value)
                     worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dt.Rows[i]["AB4_ID_CNT"].AsDecimal();
               }

            }
            #endregion

            return true;
         } catch (Exception ex) {
            WriteLog(ex, "error");
            labMsg.Visible = false;
            return false;
         }
      }

      /// <summary>
      /// set checkbox list focus background color
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void chkGroup_DrawItem(object sender , ListBoxDrawItemEventArgs e) {
         e.AllowDrawSkinBackground = false;
      }
   }
}