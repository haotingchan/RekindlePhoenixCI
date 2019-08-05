using BaseGround;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using Common.Helper;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

      protected override ResultStatus Open() {
         base.Open();

         //隱藏一些開發用的資訊和測試按鈕
         if (!FlagAdmin) {
            chkTest.Visible = false;
         } else {
            chkTest.Visible = true;
         }
         return ResultStatus.Success;
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

         DataTable dt = new DataTable(); //Merge用
         DataTable dt_1 = new DataTable(); //上櫃
         DataTable dt_2 = new DataTable(); //上市

         try {

            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //上櫃 11 
            foreach (CheckedListBoxItem item in chkGroup1.Items) {
               if (item.CheckState == CheckState.Checked) {
                  //DevExpress.XtraEditors.TextEdit txt = (DevExpress.XtraEditors.TextEdit)this.Controls["txt" + item.Value];
                  DevExpress.XtraEditors.TextEdit txt = (DevExpress.XtraEditors.TextEdit)this.Controls.Find("txt" + item.Value , true)[0];
                  //BaseGround.Widget.TextDateEdit txtDate = (BaseGround.Widget.TextDateEdit)this.Controls["txtDate" + item.Value];
                  BaseGround.Widget.TextDateEdit txtDate = (BaseGround.Widget.TextDateEdit)this.Controls.Find("txtDate" + item.Value , true)[0];

                  #region 11 / 22 / 33
                  if (string.IsNullOrEmpty(txt.Text) || !File.Exists(txt.Text)) {
                     MessageDisplay.Info("請輸入正確資料來源路徑!" , GlobalInfo.ResultText);
                     txt.BackColor = Color.Red;
                     return ResultStatus.Fail;
                  }

                  DataTable dtTxt = wf_CopyToTxt(txt.Text , "2");

                  dt_1 = wf_20232_2(txt.Text , txtDate.DateTimeValue.ToString("yyyyMM") , dtTxt);
                  if (dt_1 == null) {
                     txt.BackColor = Color.Red;
                     continue;
                  } else if (dt_1.Rows.Count <= 0) {
                     txt.BackColor = Color.Red;
                  } else {
                     item.CheckState = CheckState.Unchecked;
                     txt.BackColor = Color.FromArgb(128 , 255 , 255);
                  }
                  #endregion

                  dt.Merge(dt_1);
               }//if (item.CheckState == CheckState.Checked)
            }//foreach (CheckedListBoxItem item in chkGroup1.Items)

            //上市 1
            foreach (CheckedListBoxItem item in chkGroup2.Items) {
               if (item.CheckState == CheckState.Checked) {

                  //DevExpress.XtraEditors.TextEdit txt = (DevExpress.XtraEditors.TextEdit)this.Controls["txt" + item.Value];
                  DevExpress.XtraEditors.TextEdit txt = (DevExpress.XtraEditors.TextEdit)this.Controls.Find("txt" + item.Value , true)[0];
                  //BaseGround.Widget.TextDateEdit txtDate = (BaseGround.Widget.TextDateEdit)this.Controls["txtDate" + item.Value];
                  BaseGround.Widget.TextDateEdit txtDate = (BaseGround.Widget.TextDateEdit)this.Controls.Find("txtDate" + item.Value , true)[0];

                  #region 1 / 2 / 3
                  if (string.IsNullOrEmpty(txt.Text) || !File.Exists(txt.Text)) {
                     MessageDisplay.Info("請輸入正確資料來源路徑!" , GlobalInfo.ResultText);
                     txt.BackColor = Color.Red;
                     return ResultStatus.Fail;
                  }

                  DataTable dtTxt = wf_CopyToTxt(txt.Text , "1");

                  dt_2 = wf_20232_1(txt.Text , txtDate.DateTimeValue.ToString("yyyyMM") , dtTxt);
                  if (dt_2 == null) {
                     txt.BackColor = Color.Red;
                     continue;
                  } else if (dt_2.Rows.Count <= 0) {
                     txt.BackColor = Color.Red;
                  } else {
                     item.CheckState = CheckState.Unchecked;
                     txt.BackColor = Color.FromArgb(128 , 255 , 255);
                  }
                  #endregion

                  dt.Merge(dt_2);
               }
            }

            if (dt.Rows.Count <= 0) {
               return ResultStatus.Fail;
            }

            gvMain.Columns.Clear();
            gvMain.OptionsBehavior.AutoPopulateColumns = true;
            gcMain.DataSource = dt;
            gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);

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
      /// wf_20232_1 (上市)
      /// </summary>
      /// <param name="filename"></param>
      /// <param name="txtDate">yyyyMM</param>
      /// <returns></returns>
      private DataTable wf_20232_1(string filename , string txtDate , DataTable dt) {

         DataTable dtSource = new DataTable();

         try {

            //1. 檢查excel的年月是否跟外面txt設定的年月相同
            Workbook workbook = new Workbook();
            workbook.LoadDocument(filename);
            Worksheet worksheet = workbook.Worksheets[0];

            string tmpDate = worksheet.Cells[0 , 1].Value.AsDateTime().ToString("yyyyMM");
            if (tmpDate != txtDate) {
               MessageDisplay.Error(string.Format("轉檔檔案之年月= {0} ,與輸入條件= {1} 不符" , tmpDate , txtDate) , GlobalInfo.ErrorText);
               return null;
            }

            //1.1 刪除資料
            int delRes = dao20232.DeletePls3(tmpDate , "1");

            //1.2 判斷重複
            DataTable dtDup = dao20232.GetDataList(tmpDate , "2"); //上櫃的資料
            DataTable dtTemp = dtDup.Clone();

            string ls_sid = "", ls_val = "", ls_kind = " ";
            decimal ld_val = 0, valNum = 0;
            foreach (DataRow dr in dt.Rows) {
               if (ls_sid == txtEnd.Text) break;
               ls_sid = dr["col1"].AsString();
               if (string.IsNullOrEmpty(ls_sid)) {
                  ls_sid = "";
               }

               ls_val = dr["col2"].AsString();
               if (string.IsNullOrEmpty(ls_val)) {
                  ls_val = "";
               }
               bool res = decimal.TryParse(ls_val , out valNum);
               if (res) {
                  ld_val = valNum;
               }

               if (ls_sid == "" || ls_val == "" || res == false) {
                  int sidNum = 0;
                  bool num = int.TryParse(ls_sid.SubStr(0 , 2) , out sidNum);
                  if (num) {
                     ls_kind = ls_sid.SubStr(0 , 2);
                  }
                  continue;
               }

               ls_sid = ls_sid.SubStr(0 , 6);

               //尋找重覆
               int found = -1;
               DataColumn[] keys = new DataColumn[1];
               keys[0] = dtDup.Columns[1];
               dtDup.PrimaryKey = keys;

               found = dtDup.Rows.IndexOf(dtDup.Select("pls3_sid='" + ls_sid + "'").FirstOrDefault());
               DataRow drNew = dtTemp.NewRow();
               drNew["pls3_pid"] = "1";
               drNew["pls3_ym"] = txtDate;
               drNew["pls3_sid"] = ls_sid;
               drNew["pls3_kind"] = ls_kind;

               dtTemp.Rows.Add(drNew);

               //成交值
               if (found >= 0) {
                  ld_val += dtDup.Rows[found]["pls3_amt"].AsDecimal();
                  dtDup.Rows[found]["pls3_amt"] = ld_val;
               }
               int count = dtTemp.Rows.Count;
               dtTemp.Rows[count - 1]["pls3_amt"] = ld_val;

               //成交量
               ls_val = dr["col3"].AsString();
               decimal valDec;
               bool num2 = decimal.TryParse(ls_val , out valDec);
               if (num2) {
                  ld_val = valDec;
               }

               if (found >= 0) {
                  ld_val += dtDup.Rows[found]["pls3_qnty"].AsDecimal();
                  dtDup.Rows[found]["pls3_qnty"] = ld_val;
               }
               dtTemp.Rows[count - 1]["pls3_qnty"] = ld_val;

               //成交筆數
               ls_val = dr["col4"].AsString();
               decimal valDec2;
               bool num3 = decimal.TryParse(ls_val , out valDec2);
               if (num3) {
                  ld_val = valDec2;
               }

               if (found >= 0) {
                  ld_val += dtDup.Rows[found]["pls3_cnt"].AsDecimal();
                  dtDup.Rows[found]["pls3_cnt"] = ld_val;
               }
               dtTemp.Rows[count - 1]["pls3_cnt"] = ld_val;

               if (found >= 0) {
                  dtTemp.Rows[count - 1]["pls3_pid"] = "1";
                  dtDup.Rows.RemoveAt(found);
               }
            }

            ////2.把excel轉成dataTable
            //DataColumn[] columns = { new DataColumn("pls3_ym" , typeof(string)) ,
            //                        new DataColumn("pls3_sid" , typeof(string)) ,
            //                        new DataColumn("pls3_kind" , typeof(string)) ,
            //                        new DataColumn("pls3_amt" , typeof(decimal)) ,
            //                        new DataColumn("pls3_qnty" , typeof(decimal)) ,
            //                        new DataColumn("pls3_cnt" , typeof(decimal)) ,
            //                        new DataColumn("pls3_pid" , typeof(string)) };
            //dtSource.Columns.AddRange(columns);

            //string kind = " ";
            //int pos = 0;
            //int space = 0;
            //for (int k = 6 ; k < 9999 ; k++) {
            //   pos++;
            //   ShowMsg(string.Format("訊息：{0} 轉TXT，處理筆數：{1}" , filename , pos));

            //   string sid = worksheet.Cells[k , 0].Value.AsString();

            //   if (string.IsNullOrEmpty(sid)) {
            //      space++;
            //      continue;
            //   }

            //   if (space > 10 || sid == txtEnd.Text)
            //      break;

            //   DataRow drNew = dtSource.NewRow();

            //   if (!string.IsNullOrEmpty(worksheet.Cells[k , 1].Value.AsString()))
            //      drNew["pls3_amt"] = worksheet.Cells[k , 1].Value.AsDecimal();
            //   else
            //      drNew["pls3_amt"] = 0;
            //   if (!string.IsNullOrEmpty(worksheet.Cells[k , 2].Value.AsString()))
            //      drNew["pls3_qnty"] = worksheet.Cells[k , 2].Value.AsDecimal();
            //   else
            //      drNew["pls3_qnty"] = 0;
            //   if (!string.IsNullOrEmpty(worksheet.Cells[k , 3].Value.AsString()))
            //      drNew["pls3_cnt"] = worksheet.Cells[k , 3].Value.AsDecimal();
            //   else
            //      drNew["pls3_cnt"] = 0;

            //   if (string.IsNullOrEmpty(sid) || drNew["pls3_amt"].AsDecimal() == 0) {
            //      if (sid.SubStr(0 , 2).AsInt() > 0)
            //         kind = sid.SubStr(0 , 2);
            //      continue;
            //   }

            //   drNew["pls3_ym"] = txtDate;
            //   drNew["pls3_sid"] = sid.SubStr(0 , 6).Trim();
            //   drNew["pls3_kind"] = kind;
            //   drNew["pls3_pid"] = "1";

            //   dtSource.Rows.Add(drNew);
            //}

            //3.將dataTable to db table 
            ////好幾個步驟,包含create temp table/insert temp/delete pls3/insert pls3 use group by/drop temp table
            //int rowCount = dao20232.ImportDataToPls3(dtTemp , txtDate , "1");

            //servername登入可決定是否產此txt檔
            if (chkTest.Checked) {
               string testFilenameTxt = string.Format("20232_1({0})_{1}.txt" , txtDate , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss"));
               testFilenameTxt = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , testFilenameTxt);
               Common.Helper.ExportHelper.ToText(dtSource , testFilenameTxt);
            }

            int updateDtDup = dao20232.ImportDataToPls3(dtDup , txtDate , "2"); //先update dtDup(若有重複資料會在這邊被刪除)
            int updateDtTemp = dao20232.ImportDataToPls3(dtTemp , txtDate , "1"); //新的上市資料update

            return dtTemp;

         } catch (Exception ex) {
            WriteLog(ex);
         }
         return null;
      }

      /// <summary>
      /// wf_20232_2 (上櫃)
      /// </summary>
      /// <param name="filename"></param>
      /// <param name="txtDate">yyyyMM</param>
      /// <returns></returns>
      private DataTable wf_20232_2(string filename , string txtDate , DataTable dt) {

         DataTable dtSource = new DataTable();

         try {

            //1. 檢查excel的年月是否跟外面txt設定的年月相同
            //worksheet.Cells[1,0] = 107年10月 Oct., 18
            //取得月之前的值，[民、國、年、月] replace "" , substring(0,3).AsInt() + 1911 + substring(3,2)
            Workbook workbook = new Workbook();
            workbook.LoadDocument(filename);
            Worksheet worksheet = workbook.Worksheets[0];

            string tmp = worksheet.Cells[1 , 0].Value.AsString().Replace("民" , "").Replace("國" , "").Replace("年" , "").Replace("月" , "");
            string tmpDate = (tmp.Substring(0 , 3).AsInt() + 1911).AsString() + tmp.Substring(3 , 2).Trim().PadLeft(3 , '0').SubStr(1 , 2);
            if (tmpDate != txtDate) {
               MessageDisplay.Error(string.Format("轉檔檔案之年月= {0} ,與輸入條件= {1} 不符" , tmpDate , txtDate) , GlobalInfo.ErrorText);
               return null;
            }

            //1.1 刪除資料
            int delRes = dao20232.DeletePls3(tmpDate , "2");

            //1.2 判斷重複
            DataTable dtDup = dao20232.GetDataList(tmpDate , "1"); //上市的資料先撈出來
            DataTable dtTemp = dtDup.Clone();

            string ls_sid = "", ls_val = "", ls_kind = "";
            decimal ld_val = 0, valNum = 0;
            foreach (DataRow dr in dt.Rows) {
               if (ls_sid == txtEnd.Text) break;
               ls_sid = dr["col1"].AsString();
               if (string.IsNullOrEmpty(ls_sid)) {
                  ls_sid = "";
               }

               ls_val = dr["col3"].AsString();
               if (string.IsNullOrEmpty(ls_val)) {
                  ls_val = "";
               }
               bool res = decimal.TryParse(ls_val , out valNum);
               if (res) {
                  ld_val = valNum;
               }

               if (ls_sid == "" || ls_val == "" || res == false) {
                  //decimal sidNum = 0;
                  //bool num = decimal.TryParse(ls_sid.SubStr(0 , 2) , out sidNum);
                  //if (num) {
                  //   ls_kind = ls_sid.SubStr(0 , 2);
                  //}
                  continue;
               }

               ls_sid = ls_sid.SubStr(0 , 6).PadRight(6 , ' ');

               //尋找重覆
               int found = -1;
               DataColumn[] keys = new DataColumn[1];
               keys[0] = dtDup.Columns[1];
               dtDup.PrimaryKey = keys;

               found = dtDup.Rows.IndexOf(dtDup.Select("pls3_sid='" + ls_sid + "'").FirstOrDefault());
               DataRow drNew = dtTemp.NewRow();
               drNew["pls3_pid"] = "2";
               drNew["pls3_ym"] = txtDate;
               drNew["pls3_sid"] = ls_sid;
               drNew["pls3_kind"] = " ";

               dtTemp.Rows.Add(drNew);

               //成交值
               if (found >= 0) {
                  ld_val += dtDup.Rows[found]["pls3_amt"].AsDecimal();
               }
               int count = dtTemp.Rows.Count;
               dtTemp.Rows[count - 1]["pls3_amt"] = ld_val;

               //成交量
               ls_val = dr["col4"].AsString();
               decimal valDec;
               bool num = decimal.TryParse(ls_val , out valDec);
               if (num) {
                  ld_val = valDec;
               }

               if (found >= 0) {
                  ld_val += dtDup.Rows[found]["pls3_qnty"].AsDecimal();
               }
               dtTemp.Rows[count - 1]["pls3_qnty"] = ld_val;

               //成交筆數
               ls_val = dr["col5"].AsString();
               decimal valDec2;
               bool num2 = decimal.TryParse(ls_val , out valDec2);
               if (num2) {
                  ld_val = valDec2;
               }

               if (found >= 0) {
                  ld_val += dtDup.Rows[found]["pls3_cnt"].AsDecimal();
               }
               dtTemp.Rows[count - 1]["pls3_cnt"] = ld_val;

               if (found >= 0) {
                  dtTemp.Rows[count - 1]["pls3_pid"] = "1";
                  dtDup.Rows.RemoveAt(found);
               }
            }

            ////2.把excel轉成dataTable          
            //DataColumn[] columns = { new DataColumn("pls3_ym" , typeof(string)) ,
            //                        new DataColumn("pls3_sid" , typeof(string)) ,
            //                        new DataColumn("pls3_kind" , typeof(string)) ,
            //                        new DataColumn("pls3_amt" , typeof(decimal)) ,
            //                        new DataColumn("pls3_qnty" , typeof(decimal)) ,
            //                        new DataColumn("pls3_cnt" , typeof(decimal)) ,
            //                        new DataColumn("pls3_pid" , typeof(string)) };
            //dtSource.Columns.AddRange(columns);

            //int pos = 0;
            //for (int k = 8 ; k < 9999 ; k++) {
            //   pos++;
            //   ShowMsg(string.Format("訊息：{0} 轉TXT，處理筆數：{1}" , filename , pos));

            //   string sid = worksheet.Cells[k , 0].Value.AsString();
            //   if (string.IsNullOrEmpty(sid))
            //      break;

            //   DataRow drNew = dtSource.NewRow();

            //   if (!string.IsNullOrEmpty(worksheet.Cells[k , 2].Value.AsString()))
            //      drNew["pls3_amt"] = worksheet.Cells[k , 2].Value.AsDecimal();
            //   if (!string.IsNullOrEmpty(worksheet.Cells[k , 3].Value.AsString()))
            //      drNew["pls3_qnty"] = worksheet.Cells[k , 3].Value.AsDecimal();
            //   if (!string.IsNullOrEmpty(worksheet.Cells[k , 4].Value.AsString()))
            //      drNew["pls3_cnt"] = worksheet.Cells[k , 4].Value.AsDecimal();

            //   drNew["pls3_ym"] = txtDate;
            //   drNew["pls3_sid"] = sid;
            //   drNew["pls3_kind"] = " ";
            //   drNew["pls3_pid"] = "2";

            //   dtSource.Rows.Add(drNew);
            //}

            //3.將dataTable to db table 
            ////好幾個步驟,包含create temp table/insert temp/delete pls3/insert pls3 use group by/drop temp table
            //int rowCount = dao20232.ImportDataToPls3(dtTemp , txtDate , "2");

            if (chkTest.Checked) {
               string testFilenameTxt = string.Format("20232_2({0})_{1}.txt" , txtDate , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss"));
               testFilenameTxt = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , testFilenameTxt);
               Common.Helper.ExportHelper.ToText(dtSource , testFilenameTxt);
            }

            int updateDtDup = dao20232.ImportDataToPls3(dtDup , txtDate , "1"); //先update dtDup(若有重複資料會在這邊被刪除)
            int updateDtTemp = dao20232.ImportDataToPls3(dtTemp , txtDate , "2"); //新的上櫃資料update

            return dtTemp;

         } catch (Exception ex) {
            WriteLog(ex);
         }
         return null;
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

            xtraOpenFileDialog1.Filter = "Excel | *.xls | Excelx | *.xlsx | All files (*.*) | (*.*) "; // file types, that will be allowed to upload
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

      /// <summary>
      /// wf_copyto_txt()
      /// </summary>
      /// <returns></returns>
      private DataTable wf_CopyToTxt(string pathName , string pid) {

         /*****************************************
         將Excel SaveAs成CSV
         再將CSV開啟，每個cell讀取出來存TXT
         (因為直接存CSV或TXT，import會發生中間截斷)
         *****************************************/
         try {
            int li_pos = -1;
            do {
               li_pos = pathName.IndexOf('.' , li_pos + 1);
            } while (pathName.IndexOf('.' , li_pos + 1) >= 0);

            string ls_new_pathname = pathName.Substring(0 , li_pos + 1) + "txt";
            string ls_csv_pathname = pathName.Substring(0 , li_pos + 1) + "csv";

            //TXT
            li_pos = -1;
            do {
               li_pos = ls_new_pathname.IndexOf('\\' , li_pos + 1);
            } while (ls_new_pathname.IndexOf('\\' , li_pos + 1) >= 0);

            ls_new_pathname = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH + "\\20232_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss_") + ls_new_pathname.SubStr(li_pos + 1 , 30);

            //CSV
            li_pos = -1;
            do {
               li_pos = ls_csv_pathname.IndexOf('\\' , li_pos + 1);
            } while (ls_csv_pathname.IndexOf('\\' , li_pos + 1) >= 0);

            ls_csv_pathname = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH + "\\20232_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss_") + ls_csv_pathname.SubStr(li_pos + 1 , 30);


            //開啟excel並儲存成csv
            Workbook workbook = new Workbook();
            workbook.LoadDocument(pathName);
            Worksheet worksheet = workbook.Worksheets[0];
            workbook.SaveDocument(ls_csv_pathname , DocumentFormat.Csv); 

            DataTable dtTxt = new DataTable();  //d_20232_txt         
            DataColumn[] columns = { new DataColumn("col1" , typeof(string)),
                                    new DataColumn("col2" , typeof(string)),
                                    new DataColumn("col3" , typeof(string)),
                                    new DataColumn("col4" , typeof(string)),
                                    new DataColumn("col5" , typeof(string)),
                                    new DataColumn("col6" , typeof(string)),
                                    new DataColumn("col7" , typeof(string)),
                                    new DataColumn("col8" , typeof(string)),
                                    new DataColumn("col9" , typeof(string)),
                                    new DataColumn("col10" , typeof(string)),
                                    new DataColumn("col11" , typeof(string)),
                                    new DataColumn("col12" , typeof(string)) };
            dtTxt.Columns.AddRange(columns);

            int li_null_row = 0;
            labMsg.Visible = true;

            string ls_sid, ls_val, ls_oth_col1, ls_oth_col2;
            int ll_row = 0;
            for (int w = 0 ; w < 999999 ; w++) {
               ls_sid = worksheet[w , 0].Value.AsString();
               if (string.IsNullOrEmpty(ls_sid)) {
                  ls_sid = "";
               }
               if (pid == "1") {
                  ls_val = worksheet[w , 1].Value.AsString();

                  ls_oth_col1 = worksheet[w , 2].Value.AsString();
                  if (string.IsNullOrEmpty(ls_oth_col1)) {
                     ls_oth_col1 = "";
                  }

                  ls_oth_col2 = worksheet[w , 3].Value.AsString();
                  if (string.IsNullOrEmpty(ls_oth_col2)) {
                     ls_oth_col2 = "";
                  }
               } else {
                  ls_val = worksheet[w , 2].Value.AsString();

                  ls_oth_col1 = worksheet[w , 3].Value.AsString();
                  if (string.IsNullOrEmpty(ls_oth_col1)) {
                     ls_oth_col1 = "";
                  }

                  ls_oth_col2 = worksheet[w , 4].Value.AsString();
                  if (string.IsNullOrEmpty(ls_oth_col2)) {
                     ls_oth_col2 = "";
                  }
               }

               if (string.IsNullOrEmpty(ls_val)) {
                  ls_val = "";
               }

               //符合終止字串
               if (ls_sid == txtEnd.Text || li_null_row > 10) break;

               //代表整列空白
               if (string.IsNullOrEmpty(ls_sid) && string.IsNullOrEmpty(ls_val) && string.IsNullOrEmpty(ls_oth_col1) && string.IsNullOrEmpty(ls_oth_col2)) {
                  li_null_row++;
                  continue;
               }

               li_null_row = 0;
               DataRow drNew = dtTxt.NewRow();
               ll_row++;
               ShowMsg(string.Format("訊息：{0} 轉TXT，處理筆數：{1}" , pathName , ll_row));

               for (int x = 0 ; x < 12 ; x++) {
                  drNew[x] = worksheet[w , x].Value;
               }
               dtTxt.Rows.Add(drNew);
            }

            ExportHelper.ToText(dtTxt , ls_new_pathname);

            return dtTxt;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return null;
      }

      protected override ResultStatus DeleteRow() {
         string ls_rtn = GlobalInfo.OCF_DATE.ToString("yyyyMMdd");
         DialogResult result = MessageDisplay.Choose($"請問確定要刪除{GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd")}資料嗎?" , MessageBoxDefaultButton.Button2);
         if (result == DialogResult.Yes) {
            int res = dao20232.DeleteStw(ls_rtn);
            if (res <= 0) {
               MessageDisplay.Error($"刪除 {GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd")} 資料失敗" , GlobalInfo.ErrorText);
               WriteLog($"sqlca.sqlcode= {res}");
               return ResultStatus.Fail;
            } else {
               MessageDisplay.Info("刪除完成!" , GlobalInfo.ResultText);
            }
         }
         return ResultStatus.Success;
      }

      #region Click Event
      private void btn11_Click(object sender , EventArgs e) {
         txt11.BackColor = Color.FromArgb(245 , 245 , 247);
         ChooseImportFile(txtDate11.DateTimeValue , txt11 , chkGroup1.Items[0]);
      }

      private void btn22_Click(object sender , EventArgs e) {
         txt22.BackColor = Color.FromArgb(245 , 245 , 247);
         ChooseImportFile(txtDate22.DateTimeValue , txt22 , chkGroup1.Items[1]);
      }

      private void btn33_Click(object sender , EventArgs e) {
         txt33.BackColor = Color.FromArgb(245 , 245 , 247);
         ChooseImportFile(txtDate33.DateTimeValue , txt33 , chkGroup1.Items[2]);
      }

      private void btn1_Click(object sender , EventArgs e) {
         txt1.BackColor = Color.FromArgb(245 , 245 , 247);
         ChooseImportFile(txtDate1.DateTimeValue , txt1 , chkGroup2.Items[0]);
      }

      private void btn2_Click(object sender , EventArgs e) {
         txt2.BackColor = Color.FromArgb(245 , 245 , 247);
         ChooseImportFile(txtDate2.DateTimeValue , txt2 , chkGroup2.Items[1]);
      }

      private void btn3_Click(object sender , EventArgs e) {
         txt3.BackColor = Color.FromArgb(245 , 245 , 247);
         ChooseImportFile(txtDate3.DateTimeValue , txt3 , chkGroup2.Items[2]);
      }
      #endregion
   }
}