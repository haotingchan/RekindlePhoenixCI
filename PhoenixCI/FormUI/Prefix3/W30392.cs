using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/04/08
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30392 國外指數類期貨契約價量資料
   /// </summary>
   public partial class W30392 : FormParent {

      private D30392 dao30392;
      private int flag;

      public W30392(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao30392 = new D30392();

      }

      protected override ResultStatus Open() {
         base.Open();

         txtMonth.DateTimeValue = GlobalInfo.OCF_DATE;

#if DEBUG
         txtMonth.EditValue = DateTime.ParseExact("2018/10/11" , "yyyy/MM/dd" , null).ToString("yyyy/MM");
         this.Text += "(開啟測試模式)";
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

      protected override ResultStatus Export() {
         try {

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "轉檔中...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //2. 設定日期
            DateTime ldt_sdate, ldt_edate;
            flag = 0;

            //2.1 copy template xls to target path
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            //3. 填資料
            //3.1 I5F
            //前月倒數2天交易日
            ldt_sdate = PbFunc.f_get_last_day("AI3" , "I5F" , txtMonth.Text , 2);

            //抓當月最後交易日
            ldt_edate = PbFunc.f_get_end_day("AI3" , "I5F" , txtMonth.Text);

            int row = 1;
            wf_30392_1(workbook , "I5F" , "30392_2(I5F)" , ldt_sdate , ldt_edate , row);

            row = 3;
            wf_30392_1abc(workbook , "I5F" , "data_30392_2abc" , row);

            //3.2 TJF
            ldt_sdate = PbFunc.f_get_last_day("AI3" , "TJF" , txtMonth.Text , 2);
            ldt_edate = PbFunc.f_get_end_day("AI3" , "TJF" , txtMonth.Text);

            row = 1;
            wf_30392_1(workbook , "TJF" , "30392_1(TJF)" , ldt_sdate , ldt_edate , row);

            row = 3;
            wf_30392_1abc(workbook , "TJF" , "data_30392_1abc" , row);

            //3.3 UDF
            ldt_sdate = PbFunc.f_get_last_day("AI3" , "UDF" , txtMonth.Text , 2);
            ldt_edate = PbFunc.f_get_end_day("AI3" , "UDF" , txtMonth.Text);

            string sdate = ldt_sdate.ToString("yyyyMM");
            string edate = ldt_edate.ToString("yyyyMM");

            if (sdate == edate) {
               ldt_sdate = DateTime.ParseExact(ldt_sdate.ToString("yyyy/MM/01") , "yyyy/MM/dd" , null).AddDays(-1);
            }

            row = 1;
            wf_30392_1(workbook , "UDF" , "30392_3(UDF)" , ldt_sdate , ldt_edate , row);

            row = 3;
            wf_30392_1abc(workbook , "UDF" , "data_30392_3abc" , row);

            //3.4 SPF
            ldt_sdate = PbFunc.f_get_last_day("AI3" , "SPF" , txtMonth.Text , 2);
            ldt_edate = PbFunc.f_get_end_day("AI3" , "SPF" , txtMonth.Text);

            sdate = ldt_sdate.ToString("yyyyMM");
            edate = ldt_edate.ToString("yyyyMM");

            if (sdate == edate) {
               ldt_sdate = DateTime.ParseExact(ldt_sdate.ToString("yyyy/MM/01") , "yyyy/MM/dd" , null).AddDays(-1);
            }

            row = 1;
            wf_30392_1(workbook , "SPF" , "30392_4(SPF)" , ldt_sdate , ldt_edate , row);

            row = 3;
            wf_30392_1abc(workbook , "SPF" , "data_30392_4abc" , row);

            //3.5 一定要放到最後，因為ldt_sdate會變成當月1日
            row = 4;
            wf_30392_1_aprf(workbook , "30392_1d" , row);

            if (flag == 0) {
               File.Delete(excelDestinationPath);
               MessageDisplay.Info("查無資料!");
               return ResultStatus.Fail;
            }

            //4. save
            workbook.SaveDocument(excelDestinationPath);

            if (FlagAdmin)
               System.Diagnostics.Process.Start(excelDestinationPath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            MessageDisplay.Info("查無資料!");
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
      /// wf_30392_1
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="kindId"></param>
      /// <param name="sheetName"></param>
      /// <param name="row"></param>
      protected void wf_30392_1(Workbook workbook , string kindId , string sheetName , DateTime ldt_sdate , DateTime ldt_edate , int row) {

         string rptName = string.Format("「{0}」期貨契約價量資料" , kindId);
         ShowMsg(string.Format("{0}－{1} 轉檔中..." , _ProgramID , rptName));

         try {
            //1. 處理日期
            //DateTime ldt_sdate = PbFunc.f_get_last_day("AI3" , kindId , txtMonth.Text , 2); //前月倒數2天交易日     
            //DateTime ldt_edate = PbFunc.f_get_end_day("AI3" , kindId , txtMonth.Text);//抓當月最後交易日

            DataTable dtAi3 = dao30392.d_ai3(kindId , ldt_sdate , ldt_edate);
            if (dtAi3.Rows.Count <= 0) {
               //MessageDisplay.Info(string.Format("{0}~{1},{2}-{3},無任何資料!" , ldt_sdate.ToString("yyyy/MM/dd") , ldt_edate.ToString("yyyy/MM/dd") , _ProgramID , rptName));
               return;
            }

            //2. 切換sheet
            Worksheet ws1 = workbook.Worksheets[sheetName];

            //3. 內容
            //無前月資料
            string ai3Date = dtAi3.Rows[0]["ai3_date"].AsDateTime().ToString("yyyy/MM");
            if (ai3Date == txtMonth.Text) {
               row += 2;
            }

            DateTime ldtDate = DateTime.MinValue;
            foreach (DataRow dr in dtAi3.Rows) {
               DateTime ai3date = dr["ai3_date"].AsDateTime();
               decimal closePrice = dr["ai3_close_price"].AsDecimal();
               decimal val = dr["ai3_last_close_price"].AsDecimal();
               decimal mQnty = dr["ai3_m_qnty"].AsDecimal();
               decimal oi = dr["ai3_oi"].AsDecimal();
               decimal index = dr["ai3_index"].AsDecimal();
               //decimal tfxmmdPx = 0;
               //if (dr["tfxmmd_px"] != DBNull.Value) {
               //   tfxmmdPx = dr["tfxmmd_px"].AsDecimal();
               //}

               if (ldtDate != ai3date) {
                  ldtDate = ai3date;
                  row++;
                  ws1.Cells[row , 0].Value = ldtDate.ToString("MM/dd");
               }

               ws1.Cells[row , 1].Value = closePrice;

               if (!val.Equals(null) && val != 0) {
                  ws1.Cells[row , 2].Value = closePrice - val;
               }

               ws1.Cells[row , 3].Value = mQnty;
               ws1.Cells[row , 4].Value = oi;
               ws1.Cells[row , 5].Value = index;

               ws1.Cells[row , 8].SetValue(dr["tfxmmd_px"]);//.Value = tfxmmdPx;

            }//foreach (DataRow dr in dtAi3.Rows)

            //4. 刪除空白列
            int rowTotal = 35;
            if (rowTotal > dtAi3.Rows.Count + 2) {
               Range ra = ws1.Range[(row + 2).AsString() + ":" + rowTotal.AsString()];
               ra.Delete(DeleteMode.EntireRow);
            }

            ws1.Range["A1"].Select();
            ws1.ScrollToRow(0);

            //5. 表尾
            DataTable dtAi2Ym = dao30392.d_ai2_ym(kindId , ldt_sdate.ToString("yyyyMM") , ldt_edate.ToString("yyyyMM"));
            if (dtAi2Ym.Rows.Count <= 0) {
               return;
            }

            DataRow drAi2 = dtAi2Ym.Rows[0];

            //5.1 上月
            row += 5;
            decimal dayCnt = drAi2["last_m_day_cnt"].AsDecimal();
            decimal lastMQnty = drAi2["last_m_qnty"].AsDecimal();
            decimal lastMOi = drAi2["last_m_oi"].AsDecimal();
            if (dayCnt > 0) {
               ws1.Cells[row , 5].Value = Math.Round((lastMQnty / dayCnt) , 0 , MidpointRounding.AwayFromZero);
               ws1.Cells[row , 7].Value = Math.Round((lastMOi / dayCnt) , 0 , MidpointRounding.AwayFromZero);
            }

            //5.2 今年迄今
            row += 2;
            dayCnt = drAi2["y_day_cnt"].AsDecimal();
            decimal yQnty = drAi2["y_qnty"].AsDecimal();
            decimal yOi = drAi2["y_oi"].AsDecimal();
            if (dayCnt > 0) {
               ws1.Cells[row , 5].Value = Math.Round((yQnty / dayCnt) , 0 , MidpointRounding.AwayFromZero);
               ws1.Cells[row , 7].Value = Math.Round((yOi / dayCnt) , 0 , MidpointRounding.AwayFromZero);
            }

            flag++;

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }

      /// <summary>
      /// wf_30392_aprf
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="kindId"></param>
      /// <param name="sheetName"></param>
      /// <param name="row"></param>
      protected void wf_30392_1_aprf(Workbook workbook , string sheetName , int row) {

         string rptName = "「東證期貨」放寬漲跌幅統計表";
         ShowMsg(string.Format("{0}－{1} 轉檔中..." , _ProgramID , rptName));

         try {

            //1. 處理日期
            DateTime ldt_sdate = PbFunc.f_get_last_day("AI3" , "SPF" , txtMonth.Text , 2); //前月倒數2天交易日     
            DateTime ldt_edate = PbFunc.f_get_end_day("AI3" , "SPF" , txtMonth.Text);//抓當月最後交易日

            ldt_sdate = DateTime.ParseExact(ldt_edate.ToString("yyyy/MM/01") , "yyyy/MM/dd" , null);

            DataTable dtAprf = dao30392.d_30392_aprf(ldt_sdate , ldt_edate);
            if (dtAprf.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}~{1},{2}-{3},無任何資料!" , ldt_sdate.ToString("yyyy/MM/dd") , ldt_edate.ToString("yyyy/MM/dd") , _ProgramID , rptName));
               return;
            }

            //2. 切換sheet
            Worksheet ws2 = workbook.Worksheets[sheetName];

            //3. 內容
            ws2.Import(dtAprf , false , row , 0);

            //4. 刪除空白列
            int rowTotal = 35;
            if (rowTotal > dtAprf.Rows.Count + 4) {
               Range ra = ws2.Range[(dtAprf.Rows.Count + 5).AsString() + ":" + rowTotal.AsString()];
               ra.Delete(DeleteMode.EntireRow);
            }

            ws2.Range["A1"].Select();
            ws2.ScrollToRow(0);

            flag++;

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }

      /// <summary>
      /// wf_30392_1abc
      /// </summary>
      /// <param name="workbook"></param>
      /// <param name="kindId"></param>
      /// <param name="sheetName"></param>
      /// <param name="row"></param>
      protected void wf_30392_1abc(Workbook workbook , string kindId , string sheetName , int row) {

         string rptName = string.Format("「{0}」期貨契約價量資料(買賣方比重)" , kindId);
         ShowMsg(string.Format("{0}－{1} 轉檔中..." , _ProgramID , rptName));

         try {
            //1. 切換sheet
            Worksheet ws3 = workbook.Worksheets[sheetName];

            int rowTotal = 16;
            string sDate = txtMonth.DateTimeValue.ToString("yyyy") + "01";
            string eDate = txtMonth.DateTimeValue.ToString("yyyyMM");

            DataTable dtAm2 = dao30392.d_am2(kindId , sDate , eDate);
            if (dtAm2.Rows.Count <= 0) {
               //刪除空白列         
               //ws3.Rows.Remove(4 , 12);
               if (rowTotal > row + 1) {
                  Range ra = ws3.Range["5:16"];
                  ra.Delete(DeleteMode.EntireRow);
               }
               return;
            }

            //3. 內容
            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
            ws3.Cells[16 , 0].Value = taiwanCalendar.GetYear(txtMonth.DateTimeValue).ToString().SubStr(0 , 3) + "小計";

            DateTime ymd = DateTime.MinValue;
            foreach (DataRow dr in dtAm2.Rows) {
               DateTime am2Ymd = dr["am2_ymd"].AsDateTime("yyyyMM");
               string temp = string.Format("{0}/{1}" , taiwanCalendar.GetYear(am2Ymd) , am2Ymd.Month.ToString().PadLeft(2 , '0'));

               if (ymd != am2Ymd) {
                  row++;
                  ymd = am2Ymd;
                  ws3.Cells[row , 0].Value = temp;
               }

               #region 依條件判斷欄位
               string idfgType = dr["am2_idfg_type"].AsString();
               string bsCode = dr["am2_bs_code"].AsString();
               int col = 0;
               switch (idfgType) {
                  case "1":
                     if (bsCode == "B")
                        col = 2;
                     else
                        col = 3;
                     break;
                  case "2":
                     if (bsCode == "B")
                        col = 4;
                     else
                        col = 5;
                     break;
                  case "3":
                     if (bsCode == "B")
                        col = 6;
                     else
                        col = 7;
                     break;
                  case "5":
                     if (bsCode == "B")
                        col = 8;
                     else
                        col = 9;
                     break;
                  case "6":
                     if (bsCode == "B")
                        col = 10;
                     else
                        col = 11;
                     break;
                  case "8":
                     if (bsCode == "B")
                        col = 12;
                     else
                        col = 13;
                     break;
                  case "7":
                     if (bsCode == "B")
                        col = 14;
                     else
                        col = 15;
                     break;
               }
               decimal mQnty = dr["am2_m_qnty"].AsDecimal();
               ws3.Cells[row , col - 1].Value = mQnty;
               #endregion

            }//foreach (DataRow dr in dtAm2.Rows)

            //4. 刪除空白列
            if (rowTotal > row + 1) {
               Range ra = ws3.Range[(row + 2).AsString() + ":" + rowTotal.AsString()];
               ra.Delete(DeleteMode.EntireRow);
            }

            ws3.Range["A1"].Select();
            ws3.ScrollToRow(0);

            flag++;

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }

   }
}