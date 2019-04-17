﻿using BaseGround;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/04/17
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {
   /// <summary>
   /// 20232 部位限制交易量轉入
   /// </summary>
   public partial class W20232 : FormParent {
      private D20232 dao20232;
      DateTime emDate;

      public W20232(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao20232 = new D20232();
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = true;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = true;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = false;//列印

         emDate = DateTime.Now;
#if DEBUG
         //Winni test
         emDate = DateTime.ParseExact("2019/01" , "yyyy/MM" , null);
         this.Text += "(開啟測試模式),ocfDate=2019/01";
#endif

         txtDate33.DateTimeValue = txtDate3.DateTimeValue = emDate.AddMonths(-1);
         txtDate22.DateTimeValue = txtDate2.DateTimeValue = emDate.AddMonths(-2);
         txtDate11.DateTimeValue = txtDate1.DateTimeValue = emDate.AddMonths(-3);

         txtEnd.Text = "受益證券";


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
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Import() {

         #region 日期檢核
         //if (gbItem.EditValue.AsString() == "rbSdateToEdate") {
         //   if (!txtStartDate.IsDate(txtStartDate.Text , "日期輸入錯誤!") ||
         //      !txtEndDate.IsDate(txtEndDate.Text , "日期輸入錯誤!")) {
         //      txtEndDate.Focus();
         //      MessageDisplay.Error("日期輸入錯誤!");
         //      return ResultStatus.Fail;
         //   }
         //} else {
         //   if (!txtDate.IsDate(txtDate.Text , "日期輸入錯誤!")) {
         //      txtEndDate.Focus();
         //      MessageDisplay.Error("日期輸入錯誤!");
         //      return ResultStatus.Fail;
         //   }
         //}
         #endregion

         try {
            //上櫃 11 
            foreach (CheckedListBoxItem item in chkGroup1.Items) {
               if (item.CheckState == CheckState.Checked) {
                  if (string.IsNullOrEmpty(txt11.Text) || !File.Exists(txt11.Text)) {
                     MessageDisplay.Info("請輸入正確資料來源路徑!");
                     txt11.BackColor = Color.Red;
                     return ResultStatus.Fail;
                  }

                  bool result = wf_20232_2(txt11.Text , txtDate11.DateTimeValue.ToString("yyyyMM"));
                  if (!result) {
                     txt11.BackColor = Color.Red;
                  } else {
                     item.CheckState = CheckState.Unchecked;
                     txt11.BackColor = Color.FromArgb(128 , 255 , 255);
                  }

               }//if (item.CheckState == CheckState.Checked)
            }//foreach (CheckedListBoxItem item in chkGroup1.Items)

            //上市 1
            foreach (CheckedListBoxItem item in chkGroup2.Items) {
               if (item.CheckState == CheckState.Checked) {
                  if (string.IsNullOrEmpty(txt1.Text) || !File.Exists(txt1.Text)) {
                     MessageDisplay.Info("請輸入正確資料來源路徑!");
                     txt11.BackColor = Color.Red;
                     return ResultStatus.Fail;
                  }

                  bool result = wf_20232_1(txt1.Text , txtDate1.DateTimeValue.ToString("yyyyMM"));
                  if (!result) {
                     txt1.BackColor = Color.Red;
                  } else {
                     item.CheckState = CheckState.Unchecked;
                     txt1.BackColor = Color.FromArgb(128 , 255 , 255);
                  }

               }
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

      /// <summary>
      /// wf_20232_1
      /// </summary>
      /// <param name="filename"></param>
      /// <param name="txtDate">yyyyMM</param>
      /// <returns></returns>
      private bool wf_20232_1(string filename , string txtDate) {
         try {

            //1. 檢查excel的年月是否跟外面txt設定的年月相同
            Workbook workbook = new Workbook();
            workbook.LoadDocument(filename);
            Worksheet worksheet = workbook.Worksheets[0];

            //string tmp = worksheet.Cells[0 , 1].Value.AsString().Replace("民" , "").Replace("國" , "").Replace("年" , "").Replace("月" , "");
            //string tmpDate = (tmp.Substring(0 , 3).AsInt() - 1911).AsString() + tmp.Substring(3 , 2);
            string tmpDate = "201810";
            if (tmpDate != txtDate) {
               MessageDisplay.Error(string.Format("轉檔檔案之年月= {0} ,與輸入條件= {1} 不符" , tmpDate , txtDate));
            }

            //2.把excel轉成dataTable
            DataTable dtSource = new DataTable();
            DataColumn[] columns = { new DataColumn("pls3_ym" , typeof(string)) ,
                                    new DataColumn("pls3_sid" , typeof(string)) ,
                                    new DataColumn("pls3_kind" , typeof(string)) ,
                                    new DataColumn("pls3_amt" , typeof(decimal)) ,
                                    new DataColumn("pls3_qnty" , typeof(decimal)) ,
                                    new DataColumn("pls3_cnt" , typeof(decimal)) ,
                                    new DataColumn("pls3_pid" , typeof(string)) };
            dtSource.Columns.AddRange(columns);

            int pos = 0;
            for (int k = 8 ; k < 9999 ; k++) {
               string sid = worksheet.Cells[k , 0].Value.AsString();

               if (string.IsNullOrEmpty(sid)) {
                  pos++;
                  continue;
               }

               if (pos > 10 || sid == txtEnd.Text)
                  break;

               DataRow drNew = dtSource.NewRow();

               if (!string.IsNullOrEmpty(worksheet.Cells[k , 1].Value.AsString()))
                  drNew["pls3_amt"] = worksheet.Cells[k , 1].Value.AsDecimal();
               else
                  drNew["pls3_amt"] = 0;
               if (!string.IsNullOrEmpty(worksheet.Cells[k , 2].Value.AsString()))
                  drNew["pls3_qnty"] = worksheet.Cells[k , 2].Value.AsDecimal();
               else
                  drNew["pls3_qnty"] = 0;
               if (!string.IsNullOrEmpty(worksheet.Cells[k , 3].Value.AsString()))
                  drNew["pls3_cnt"] = worksheet.Cells[k , 3].Value.AsDecimal();
               else
                  drNew["pls3_cnt"] = 0;

               if (drNew["pls3_amt"].AsDecimal() == 0 && drNew["pls3_qnty"].AsDecimal() == 0 && drNew["pls3_cnt"].AsDecimal() == 0)
                  continue;

               drNew["pls3_ym"] = tmpDate;
               drNew["pls3_sid"] = sid.Substring(0 , 6).Trim();
               drNew["pls3_kind"] = "";
               drNew["pls3_pid"] = "2";

               dtSource.Rows.Add(drNew);
            }

            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dtSource;
            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);

            return true;

            //3.將dataTable to db table 
            //好幾個步驟,包含create temp table/insert temp/delete pls3/insert pls3 use group by/drop temp table
            int rowCount = dao20232.ImportDataToPls3(dtSource , "");


            return true;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return false;
      }


      /// <summary>
      /// wf_20232_2
      /// </summary>
      /// <param name="filename"></param>
      /// <param name="txtDate">yyyyMM</param>
      /// <returns></returns>
      private bool wf_20232_2(string filename , string txtDate) {
         try {

            //1. 檢查excel的年月是否跟外面txt設定的年月相同
            //worksheet.Cells[1,0] = 107年10月 Oct., 18
            //取得月之前的值，[民、國、年、月] replace "" , substring(0,3).AsInt() + 1911 + substring(3,2)
            Workbook workbook = new Workbook();
            workbook.LoadDocument(filename);
            Worksheet worksheet = workbook.Worksheets[0];

            //string tmp = worksheet.Cells[1 , 0].Value.AsString().Replace("民" , "").Replace("國" , "").Replace("年" , "").Replace("月" , "");
            //string tmpDate = (tmp.Substring(0 , 3).AsInt() - 1911).AsString() + tmp.Substring(3 , 2);
            string tmpDate = "201810";
            if (tmpDate != txtDate) {
               MessageDisplay.Error(string.Format("轉檔檔案之年月= {0} ,與輸入條件= {1} 不符" , tmpDate , txtDate));
            }

            //2.把excel轉成dataTable
            DataTable dtSource = new DataTable();
            DataColumn[] columns = { new DataColumn("pls3_ym" , typeof(string)) ,
                                    new DataColumn("pls3_sid" , typeof(string)) ,
                                    new DataColumn("pls3_kind" , typeof(string)) ,
                                    new DataColumn("pls3_amt" , typeof(decimal)) ,
                                    new DataColumn("pls3_qnty" , typeof(decimal)) ,
                                    new DataColumn("pls3_cnt" , typeof(decimal)) ,
                                    new DataColumn("pls3_pid" , typeof(string)) };
            dtSource.Columns.AddRange(columns);

            for (int k = 8 ; k < 9999 ; k++) {
               string sid = worksheet.Cells[k , 0].Value.AsString();
               if (string.IsNullOrEmpty(sid))
                  break;

               DataRow drNew = dtSource.NewRow();
               drNew["pls3_ym"] = tmpDate;
               drNew["pls3_sid"] = sid;
               drNew["pls3_kind"] = "";

               if (!string.IsNullOrEmpty(worksheet.Cells[k , 2].Value.AsString()))
                  drNew["pls3_amt"] = worksheet.Cells[k , 2].Value.AsDecimal();
               if (!string.IsNullOrEmpty(worksheet.Cells[k , 3].Value.AsString()))
                  drNew["pls3_qnty"] = worksheet.Cells[k , 3].Value.AsDecimal();
               if (!string.IsNullOrEmpty(worksheet.Cells[k , 4].Value.AsString()))
                  drNew["pls3_cnt"] = worksheet.Cells[k , 4].Value.AsDecimal();

               drNew["pls3_pid"] = "2";

               dtSource.Rows.Add(drNew);
            }

            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dtSource;
            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);

            return true;

            //3.將dataTable to db table 
            //好幾個步驟,包含create temp table/insert temp/delete pls3/insert pls3 use group by/drop temp table
            int rowCount = dao20232.ImportDataToPls3(dtSource , "");


            return true;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return false;
      }



      /// <summary>
      /// ChooseImportFile
      /// </summary>
      /// <param name="fileDate"></param>
      /// <param name="txt"></param>
      /// <param name="chkItem"></param>
      private void ChooseImportFile(DateTime fileDate ,
                                    DevExpress.XtraEditors.TextEdit txt ,
                                    CheckedListBoxItem chkItem) {
         try {

            xtraOpenFileDialog1.Filter = "Excel | *.xls | Excelx | *.xlsx"; // file types, that will be allowed to upload
            xtraOpenFileDialog1.Multiselect = false; // allow/deny user to upload more than one file at a time

            //上櫃資料__請輸入交易日期 , 範本檔名稱=成交彙總表10710
            //上市資料__請輸入交易日期 , 範本檔名稱=09201810
            string filename = string.Format("09{0}.xls" , fileDate.ToString("yyyyMM"));
            xtraOpenFileDialog1.FileName = filename;//default filename

            xtraOpenFileDialog1.InitialDirectory = Application.StartupPath;//default path
            if (!File.Exists(Path.Combine(Application.StartupPath , filename))) {
               xtraOpenFileDialog1.InitialDirectory = "C:\\";
            }


            if (xtraOpenFileDialog1.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
               txt.Text = xtraOpenFileDialog1.FileName; // get name of file
               chkItem.CheckState = CheckState.Checked;
            } else {
               txt.Text = "";
               chkItem.CheckState = CheckState.Unchecked;
            }
         } catch (Exception ex) {
            WriteLog(ex);
         }
      }


      private void btn11_Click(object sender , EventArgs e) {
         ChooseImportFile(txtDate11.DateTimeValue , txt11 , chkGroup1.Items[0]);
      }

      private void btn22_Click(object sender , EventArgs e) {
         ChooseImportFile(txtDate22.DateTimeValue , txt22 , chkGroup1.Items[1]);
      }

      private void btn33_Click(object sender , EventArgs e) {
         ChooseImportFile(txtDate33.DateTimeValue , txt33 , chkGroup1.Items[2]);
      }

      private void btn1_Click(object sender , EventArgs e) {
         ChooseImportFile(txtDate1.DateTimeValue , txt1 , chkGroup2.Items[0]);
      }

      private void btn2_Click(object sender , EventArgs e) {
         ChooseImportFile(txtDate2.DateTimeValue , txt2 , chkGroup2.Items[1]);
      }

      private void btn3_Click(object sender , EventArgs e) {
         ChooseImportFile(txtDate3.DateTimeValue , txt3 , chkGroup2.Items[2]);
      }
   }
}