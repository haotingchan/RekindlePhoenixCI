using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/04/03
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 40130 保證金計算流程表
   /// </summary>
   public partial class W40130 : FormParent {

      protected enum SheetNo {
         Opt = 0,
         OptDetail = 1,
         Fut = 2,
         FutDetail = 3
      }

      private D40130 dao40130;

      public W40130(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao40130 = new D40130();
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            //1. 設定初始年月yyyy/MM/dd
            txtDate.DateTimeValue = DateTime.Now;


#if DEBUG
            //Winni test
            //txtDate.DateTimeValue = DateTime.ParseExact("2018/10/11" , "yyyy/MM/dd" , null);
            //this.Text += "(開啟測試模式),ocfDate=2018/10/11";
#endif


            //2. 設定dropdownlist(商品)
            DataTable dtKindId = dao40130.GetDataList(txtDate.DateTimeValue , "%"); //第一行全部+mgt2_kind_id_out/mgt2_kind_id/apdk_name/cpr_price_risk_rate/cp_display
            dwKindId.SetDataTable(dtKindId , "MGT2_KIND_ID","CP_DISPLAY", TextEditStyles.DisableTextEditor);
            dwKindId.ItemIndex = 0;

            txtDate.Focus();

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

         #region 確認統計資料未轉入完畢
         string lsRtn = PbFunc.f_get_jsw(_ProgramID , "E" , txtDate.Text);
         DialogResult liRtn;

         if (lsRtn != "Y") {
            liRtn = MessageDisplay.Choose(String.Format("{0} 統計資料未轉入完畢,是否要繼續?" , txtDate.Text));
            if (liRtn == DialogResult.No) {
               labMsg.Visible = false;
               Cursor.Current = Cursors.Arrow;
               return ResultStatus.Fail;
            }
         }//if (lsRtn != "Y")
         #endregion

         try {
            //1. 判斷為單一商品還是全部
            DataTable dt = dao40130.GetDataList(txtDate.DateTimeValue , "%");
            DataTable dtSelect = new DataTable();
            string kindId = dwKindId.EditValue.AsString();
            if (kindId != "%") {
               dtSelect = dt.Filter("mgt2_kind_id = '" + kindId + "'");
            } else { //kindId = "%"
               dtSelect = dt.Filter("mgt2_kind_id <> '%'");
            }

            //1.1 準備開檔
            string originalFilePath = "";
            string destinationFilePath = "";
            Workbook workbook = new Workbook();

            //2. 填資料
            foreach (DataRow dr in dtSelect.Rows) {
               string fileKind = dr["mgt2_kind_id"].AsString();

               //2.1 開啟&複製檔案(因可能有多個excel所以在迴圈裡兜路徑)
               originalFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH ,
                     string.Format("{0}.{1}" , _ProgramID , FileType.XLS.ToString().ToLower()));
               destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                  string.Format("{0}({1})_{2}.{3}" , _ProgramID , fileKind , DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") , FileType.XLS.ToString().ToLower()));
               
               File.Copy(originalFilePath , destinationFilePath , true);
               workbook.LoadDocument(destinationFilePath);
               kindId = dr["mgt2_kind_id"].AsString() + "%";
               string tmpKindId = kindId.SubStr(2 , 1);
               if (tmpKindId == "F") { //Sheet:期貨data
                  wf_40131(workbook , kindId);
               } else { //Sheet:選擇權data
                  wf_40132(workbook , kindId);
               }

               //3. save
               workbook.SaveDocument(destinationFilePath);
            }//foreach (DataRow dr in dt.Rows)

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

      #region wf_40131
      private void wf_40131(Workbook workbook , string kindId) {
         string rptName = "期貨資料";
         string rptId = "40131";
         ShowMsg(string.Format("{0}-{1} 轉檔中..." , rptId , rptName));

         try {
            Worksheet wsDetail;
            Worksheet ws;

            //1. 切換Sheet5
            if (kindId == "CPF%") { //下拉選單無此選項，應該都不會進來這裡
               wsDetail = workbook.Worksheets["CPF_Detail"];
            } else {
               wsDetail = workbook.Worksheets[(int)SheetNo.FutDetail];
            }

            //1.1 讀取基本資料
            DateTime startDate = txtDate.DateTimeValue;
            DataTable dt = dao40130.GetDataList(startDate , kindId);
            if (dt.Rows.Count > 0) {
               wsDetail.Cells[0 , 0].Value = dt.Rows[1]["mgt2_kind_id_out"].AsString();
               wsDetail.Cells[0 , 1].Value = dt.Rows[1]["apdk_name"].AsString();
               wsDetail.Cells[0 , 2].Value = dt.Rows[1]["cpr_price_risk_rate"].AsString();
            }

            //1.2 內容
            DataTable dtContent = dao40130.GetAi5Data(startDate.AddDays(-665) , startDate , kindId);
            if (dtContent.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1},{2}-{3},無任何資料!" , txtDate.Text , kindId , rptId , rptName));
               return;
            }

            int rowNum = 419;
            int pos = 0;
            foreach (DataRow drCon in dtContent.Rows) {
               DateTime ai5Date = dtContent.Rows[pos]["ai5_date"].AsDateTime();
               decimal settlePrice = dtContent.Rows[pos]["ai5_settle_price"].AsDecimal();
               decimal openRef = dtContent.Rows[pos]["ai5_open_ref"].AsDecimal();

               //最後只放結算價P(t-1)
               if (pos == 180) {
                  wsDetail.Cells[rowNum , 4].Value = settlePrice;
                  break;
               }
               rowNum--;
               wsDetail.Cells[rowNum , 2].Value = ai5Date;
               wsDetail.Cells[rowNum , 3].Value = settlePrice;
               wsDetail.Cells[rowNum , 4].Value = openRef;
               pos++;
            }//foreach (DataRow drCon in dtContent.Rows)
            //workbook.Dispose();

            //2. 切換Sheet
            if (kindId == "CPF%") { //下拉選單無此選項，應該都不會進來這裡
               ws = workbook.Worksheets["CPF"];
            } else {
               ws = workbook.Worksheets[(int)SheetNo.Fut];
            }

            //2.1 讀取資料
            DataTable dtMg1 = dao40130.GetMg1Data(startDate , kindId);
            if (dtMg1.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1},{2}-{3},無「保證金狀況表」資料!" , txtDate.Text , kindId , rptId , rptName));
               return;
            }

            //2.2 附表二 (分成7種類型報表)
            DataRow drMg1 = dtMg1.Rows[0];
            decimal price = drMg1["mg1_price"].AsDecimal();
            decimal mg1Xxx = drMg1["mg1_xxx"].AsDecimal();
            decimal risk = drMg1["mg1_risk"].AsDecimal();
            decimal cm = drMg1["mg1_cm"].AsDecimal();

            switch (kindId) {
               case "CPF%":
                  ws.Cells[11 , 3].Value = "";
                  Range range1 = ws.Range["C12:D12"];
                  range1.Merge();

                  Range range2 = ws.Range["C13:D13"];
                  range2.Merge();

                  ws.Range["A1"].Select();

                  ws.Cells[12 , 2].Value = "8219178"; //'=100000000*30/365'
                  ws.Cells[12 , 5].Formula = "=C13*E13";

                  break;
               case "GBF%":
                  ws.Cells[12 , 2].Value = price;
                  ws.Cells[12 , 3].Value = mg1Xxx * 100;
                  ws.Cells[12 , 5].Formula = "=C13*D13*E13/100";
                  break;
               default:
                  ws.Cells[12 , 2].Value = price;
                  ws.Cells[12 , 3].Value = mg1Xxx;
                  break;
            }

            ws.Cells[12 , 4].Value = risk;
            ws.Cells[12 , 6].Value = cm;

            //2.3 附表三
            decimal mMulti = drMg1["mg1_m_multi"].AsDecimal();
            decimal mg1Mm = drMg1["mg1_mm"].AsDecimal();
            decimal iMulti = drMg1["mg1_i_multi"].AsDecimal();
            decimal im = drMg1["mg1_im"].AsDecimal();

            ws.Cells[17 , 3].Value = mMulti;
            ws.Cells[17 , 5].Value = mg1Mm;
            ws.Cells[17 , 6].Value = iMulti;
            ws.Cells[17 , 8].Value = im;

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }
      #endregion

      #region wf_40132
      private void wf_40132(Workbook workbook , string kindId) {
         string rptName = "選擇權資料";
         string rptId = "40132";
         ShowMsg(string.Format("{0}-{1} 轉檔中..." , rptId , rptName));

         try {
            Worksheet wsDetail;
            Worksheet ws;

            //1. 切換Sheet
            wsDetail = workbook.Worksheets[(int)SheetNo.OptDetail];

            //1.1 讀取基本資料(同40131)
            DateTime startDate = txtDate.DateTimeValue;
            DataTable dt = dao40130.GetDataList(startDate , kindId);
            if (dt.Rows.Count > 0) {
               wsDetail.Cells[0 , 0].Value = dt.Rows[1]["mgt2_kind_id_out"].AsString();
               wsDetail.Cells[0 , 1].Value = dt.Rows[1]["apdk_name"].AsString();
               wsDetail.Cells[0 , 2].Value = dt.Rows[1]["cpr_price_risk_rate"].AsString();
            }

            //1.2 內容
            DataTable dtContent = dao40130.GetIdxfData(startDate.AddDays(-665) , startDate , kindId);
            if (dtContent.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1},{2}-{3},無任何資料!" , txtDate.Text , kindId , rptId , rptName));
               return;
            }

            int rowNum = 419;
            int pos = 0;
            foreach (DataRow drCon in dtContent.Rows) {
               decimal idx = dtContent.Rows[pos]["idxf_idx"].AsDecimal();
               DateTime idxfDate = dtContent.Rows[pos]["idxf_date"].AsDateTime();
               //最後只放結算價P(t-1)
               if (pos == 180) {
                  wsDetail.Cells[rowNum , 4].Value = idx;
                  break;
               }
               rowNum--;
               wsDetail.Cells[rowNum , 2].Value = idxfDate;
               wsDetail.Cells[rowNum , 3].Value = idx;

               //第1筆不放結算價P(t-1)
               if (pos != 0) {
                  wsDetail.Cells[rowNum + 1 , 4].Value = idx;
               }
               pos++;
            }//foreach (DataRow drCon in dtContent.Rows)
             //workbook.Dispose();

            //2. 切換Sheet
            ws = workbook.Worksheets[(int)SheetNo.Opt];

            //2.1 讀取資料
            DataTable dtMg1 = dao40130.GetMg1Data(startDate , kindId);
            if (dtMg1.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1},{2}-{3},無「保證金狀況表」資料!" , txtDate.Text , kindId , rptId , rptName));
               return;
            }

            //2.2 內容    
            DataRow drMg1 = dtMg1.Rows[0];
            decimal price = drMg1["mg1_price"].AsDecimal();
            decimal mg1Xxx = drMg1["mg1_xxx"].AsDecimal();
            decimal risk = drMg1["mg1_risk"].AsDecimal();
            decimal cm = drMg1["mg1_cm"].AsDecimal();
            decimal mMulti = drMg1["mg1_m_multi"].AsDecimal();
            decimal mm = drMg1["mg1_mm"].AsDecimal();
            decimal iMulti = drMg1["mg1_i_multi"].AsDecimal();
            decimal im = drMg1["mg1_im"].AsDecimal();

            ws.Cells[13 , 3].Value = price;
            ws.Cells[13 , 4].Value = mg1Xxx;
            ws.Cells[13 , 5].Value = risk;
            ws.Cells[13 , 7].Value = cm;
            ws.Cells[14 , 7].Value = cm;
            ws.Cells[19 , 3].Value = mMulti;
            ws.Cells[19 , 5].Value = mm;
            ws.Cells[20 , 5].Value = mm;
            ws.Cells[19 , 6].Value = iMulti;
            ws.Cells[19 , 8].Value = im;
            ws.Cells[20 , 8].Value = im;

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }
      #endregion
   }
}