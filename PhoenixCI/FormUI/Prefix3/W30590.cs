﻿using BaseGround;
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
/// Winni, 2019/01/29
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30590 臺指選擇權交易概況表
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30590 : FormParent {

      private D30590 dao30590;

      public W30590(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao30590 = new D30590();
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
            result = wf_Export(workbook , worksheet);  //function 30590

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

      #region function wf_30590
      private bool wf_Export(Workbook workbook , Worksheet worksheet) {

         try {
            /*************************************
            ls_year = 年
            li_ole_row_tol = 總列數
            li_row_start = 開始列
            *************************************/
            string ls_kind_id2, ls_pc_code, ls_expiry_type, ls_prod_type, ls_param_key, ls_market_code;
            int li_col, ii_ole_row = 1;
            long ll_found;

            #region 讀取資料(每日)
            ls_prod_type = "O";
            ls_param_key = "TXO";
            if (gbContract.EditValue.ToString() == "rbContractAll" || gbContract.EditValue.ToString() == "rbContractAllTot") {
               ls_kind_id2 = "%";
            } else {
               if (gbContract.EditValue.ToString() == "rbContractTxw") {
                  ls_kind_id2 = "TXW%";
               } else {
                  ls_kind_id2 = "TXO%";
               }
            }

            if (gbProd.EditValue.ToString() == "rbProdAll") {
               ls_pc_code = "%";
            } else {
               ls_pc_code = "Y";
            }

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
            dt = dao30590.GetData(ls_expiry_type , ls_pc_code , ls_kind_id2 , StartYMD , EndYMD , ls_market_code);
            if (dt.Rows.Count <= 0) {

               labMsg.Text = String.Format("{0}:yyyymm" , StartYMD) + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!";
               MessageDisplay.Info(string.Format("{0:yyyymm},{1}–市場總成交量雙邊(A)無任何資料!" , StartYMD , this.Text));
               return false;
            }
            #endregion

            #region 表頭      
            ii_ole_row = 1;
            if (gbProd.EditValue.ToString() == "rbProdPC") {
               li_col = 4;
            } else {
               li_col = 3;
            }

            /*************************************
            chkGroup.Items[0] = MonQnty
            chkGroup.Items[1] = OI
            chkGroup.Items[2] = MonCnt
            chkGroup.Items[3] = Amt
            chkGroup.Items[4] = AmtStk
            chkGroup.Items[5] = Acc
            chkGroup.Items[6] = Id
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
               worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "名目契約價值[(買+賣)/2]";
            }

            if (chkGroup.Items[5].CheckState.ToString() == "Checked") {
               li_col += 1;
               worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = "交易戶數(買+賣)";
            }

            if (chkGroup.Items[6].CheckState.ToString() == "Checked") {
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

               if (gbProd.EditValue.ToString() == "rbProdPC") {
                  if (dt.Rows[i]["APDK_PC_CODE"] != DBNull.Value)
                     worksheet.Cells[ii_ole_row - 1 , 3].Value = dt.Rows[i]["APDK_PC_CODE"].AsString();
                  li_col = 4;
               } else {
                  li_col = 3;
               }

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
                  if (dt.Rows[i]["AA2_AMT_STK"] != DBNull.Value)
                     worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dt.Rows[i]["AA2_AMT_STK"].AsDecimal();
               }

               if (chkGroup.Items[5].CheckState.ToString() == "Checked") {
                  li_col += 1;
                  if (dt.Rows[i]["AM9_ACC_CNT"] != DBNull.Value)
                     worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dt.Rows[i]["AM9_ACC_CNT"].AsDecimal();
               }

               if (chkGroup.Items[6].CheckState.ToString() == "Checked") {
                  li_col += 1;
                  if (dt.Rows[i]["AB4_ID_CNT"] != DBNull.Value)
                     worksheet.Cells[ii_ole_row - 1 , li_col - 1].Value = dt.Rows[i]["AB4_ID_CNT"].AsDecimal();
               }

            }
            #endregion

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            labMsg.Visible = false;
            return false;
         }
      }
      #endregion

      /// <summary>
      /// 設定 checkbox list 影響序列是否可編輯
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void chkGroup_ItemCheck(object sender , DevExpress.XtraEditors.Controls.ItemCheckEventArgs e) {
         if (chkGroup.Items[3].CheckState.ToString() == "Checked" || chkGroup.Items[4].CheckState.ToString() == "Checked" ||
            chkGroup.Items[5].CheckState.ToString() == "Checked" || chkGroup.Items[6].CheckState.ToString() == "Checked") {
            //只能選期別
            //序列
            gbProd.EditValue = "rbProdAll";
            gbProd.Enabled = false;
         } else {
            //序列
            gbProd.Enabled = true;
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