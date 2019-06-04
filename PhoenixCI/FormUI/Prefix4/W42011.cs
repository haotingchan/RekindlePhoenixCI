using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Shared;
using DataObjects.Dao.Together.SpecificDao;
using Common;
using System.Threading;
using DataObjects.Dao.Together.TableDao;
using DevExpress.Spreadsheet;
using System.IO;

/// <summary>
/// Lukas, 2019/4/11
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    /// <summary>
    /// 42011 股票期貨風險價格係數檢核表
    /// </summary>
    public partial class W42011 : FormParent {

        private D42011 dao42011;
        private MGR3 daoMGR3;

        public W42011(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao42011 = new D42011();
        }

        protected override ResultStatus Open() {
            base.Open();
            //日期
            txtSDate.DateTimeValue = DateTime.Now;
            txtSDate.Focus();
            txtRate1Ref.Visible = false;
            txtRate2Ref.Visible = false;
            txtRate3Ref.Visible = false;
            txtRate4Ref.Visible = false;
            lblCmRateRef.Visible = false;

#if DEBUG
            txtSDate.EditValue = "2018/12/20";
#endif

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            try {
                if (!FlagAdmin) {
                    cbxRate.Visible = false;
                }
                decimal ld_cm_rate1, ld_cm_rate2, ld_cm_rate3;
                //最高%
                ld_cm_rate1 = dao42011.GetCmRate();
                lblCmRate.Text = (ld_cm_rate1 * 100).ToString("#.####");

                //級距1
                DataTable dtRate = dao42011.Get3CmRate();
                ld_cm_rate1 = dtRate.Rows[0]["LD_CM_RATE1"].AsDecimal();
                ld_cm_rate1 = ld_cm_rate1 - 0.01m;
                txtRate1.Text = (ld_cm_rate1 * 100).ToString("#.####");
                //級距2
                ld_cm_rate2 = dtRate.Rows[0]["LD_CM_RATE2"].AsDecimal();
                ld_cm_rate2 = ld_cm_rate2 - 0.01m;
                txtRate2.Text = (ld_cm_rate2 * 100).ToString("#.####");
                //級距3
                ld_cm_rate3 = dtRate.Rows[0]["LD_CM_RATE3"].AsDecimal();
                ld_cm_rate3 = ld_cm_rate3 - 0.01m;
                txtRate3.Text = (ld_cm_rate3 * 100).ToString("#.####");
                //表一達標準判斷
                ld_cm_rate1 = dao42011.GetRange();
                txtRange.Text = (ld_cm_rate1 * 100).ToString("#.####");

            }
            catch (Exception ex) {
                throw ex;
            }
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

        protected void ShowMsg(string msg) {
            lblProcessing.Text = msg;
            this.Refresh();
            Thread.Sleep(5);
        }

        protected override ResultStatus Export() {
            try {
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");
                string rptId = "42011", file, rptName = "股票期貨風險價格係數機動評估指標";

                #region ue_export_before
                //判斷資料已轉入
                daoMGR3 = new MGR3();
                int rtn;
                string ymd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
                rtn = daoMGR3.mgr3Count(ymd);
                if (rtn == 0) {
                    DialogResult result = MessageDisplay.Choose(" 當日保證金適用比例資料未轉入完畢,是否要繼續?");
                    if (result == DialogResult.No) {
                        lblProcessing.Visible = false;
                        return ResultStatus.Fail;
                    }
                }
                //130批次作業做完
                string rtnStr;
                rtnStr = PbFunc.f_chk_130_wf(_ProgramID, txtSDate.DateTimeValue, "5");
                if (rtnStr != "") {
                    DialogResult result = MessageDisplay.Choose(txtSDate.Text + "-" + rtnStr + "，是否要繼續?");
                    if (result == DialogResult.No) {
                        lblProcessing.Visible = false;
                        return ResultStatus.Fail;
                    }
                }

                //是否勾選任一報表
                if (!cbx1.Checked && !cbx2.Checked && !cbx3.Checked) {
                    MessageDisplay.Error("未勾選任何報表!");
                    return ResultStatus.Fail;
                }
                #endregion

                //取前一交易日
                DateTime lastDate = dao42011.GetLastDate(txtSDate.DateTimeValue);
                if (lastDate == DateTime.MinValue) {
                    MessageDisplay.Warning(txtSDate.Text + ",讀取前一交易日失敗!");
                    return ResultStatus.Fail;
                }

                //讀取資料(保證金適用比例級距)
                DataTable dt42011 = dao42011.d_42011_detl(txtSDate.DateTimeValue, lastDate, txtRange.Text.AsDecimal() / 100,
                                                          txtRate2Ref.Text.AsDecimal() / 100, txtRate3Ref.Text.AsDecimal() / 100, txtRate4Ref.Text.AsDecimal() / 100,
                                                          txtRate1.Text.AsDecimal() / 100, txtRate2.Text.AsDecimal() / 100, txtRate3.Text.AsDecimal() / 100, txtRate4.Text.AsDecimal() / 100);
                if (dt42011.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }
                dt42011.Sort("APDK_KIND_GRP2, APDK_KIND_LEVEL, MGR3_KIND_ID");

                //複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return ResultStatus.Fail;

                //開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //切換Sheet
                Worksheet ws = workbook.Worksheets[0];
                ws.Cells[0, 15].Value = txtSDate.DateTimeValue.Year + "年" + txtSDate.DateTimeValue.Month + "月" + txtSDate.DateTimeValue.Day + "日";

                //表1
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                int minusRow;
                minusRow = wf_42011_1(ws, dt42011);
                minusRow = wf_42011_2(minusRow, ws, dt42011);
                minusRow = wf_42011_3(minusRow, ws, dt42011);

                //存檔
                ws.ScrollToRow(0);
                workbook.SaveDocument(file);
                ShowMsg("轉檔成功");
            }
            catch (Exception ex) {
                ShowMsg("轉檔錯誤");
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            finally {
                this.Cursor = Cursors.Arrow;
                this.Refresh();
                Thread.Sleep(5);
            }
            return ResultStatus.Success;
        }

        protected int wf_42011_1(Worksheet ws, DataTable dt) {
            int rowIndex = 2 - 1, startRow, totalRow, f, minusRow = 0;
            string rptName;
            Range range;

            //表1
            startRow = 15 - 1;
            totalRow = 300;
            rowIndex = startRow;
            if (!cbx1.Checked) {
                //刪明細
                range = ws.Range[(startRow - 4 + 1) + ":" + (startRow + totalRow + 2 + 1)];
                range.Delete(DeleteMode.EntireRow);
                minusRow = minusRow + (totalRow + 5) + 2;  //5表首,2表尾
                //刪表頭
                startRow = 3 - 1;
                range = ws.Range[(startRow + 1) + ":" + (startRow + 1)];
                range.Delete(DeleteMode.EntireRow);
                minusRow = minusRow + 1;
                //改編號
                f = 1;
                ws.Cells[startRow, 1].Value = f.AsString();
                ws.Cells[startRow + 1, 1].Value = f.AsString();
                ws.Cells[startRow + 2, 1].Value = f.AsString();
                ws.Cells[startRow + 3, 1].Value = f.AsString();
                ws.Cells[startRow + 4, 1].Value = f.AsString();
                return minusRow;
            }

            if (txtRange.Text != "10") {
                //表首
                //使用RichTextString才能保留原本設定的格式
                RichTextString richText = new RichTextString();
                richText = ws.Cells[2, 2].GetRichText();
                f = richText.Text.IndexOf("10%") + 1;
                if (f > 0) richText.Characters(f - 1, 3).Text = txtRange.Text + "%";
                ws.Cells[2, 2].SetRichText(richText);
                //表頭
                richText = ws.Cells[rowIndex - 4, 2].GetRichText();
                f = richText.Text.IndexOf("10%") + 1;
                if (f > 0) richText.Characters(f - 1, 3).Text = txtRange.Text + "%";
                ws.Cells[rowIndex - 4, 2].SetRichText(richText);
            }

            if (cbxRate.Checked) {
                DataView dv = dt.AsDataView();
                dv.RowFilter = "rpt1_flag='Y'";
                dt = dv.ToTable();
            }
            dt = dt.Sort("T_30_RATE DESC, APDK_KIND_GRP2, APDK_KIND_LEVEL DESC, MGR3_KIND_ID");
            f = 0;
            foreach (DataRow dr in dt.Rows) {
                rowIndex++;
                f++;
                ws.Cells[rowIndex, 1].Value = f.AsString();
                ws.Cells[rowIndex, 2].Value = dr["MGR3_KIND_ID"].AsString();
                ws.Cells[rowIndex, 3].Value = dr["APDK_NAME"].AsString();
                ws.Cells[rowIndex, 4].Value = dr["MGR3_SID"].AsString();
                ws.Cells[rowIndex, 5].Value = dr["PID_NAME"].AsString();
                ws.Cells[rowIndex, 6].SetValue(dr["T_30_RATE"]);
                ws.Cells[rowIndex, 7].SetValue(dr["MGR2_DAY_RATE"]);
                ws.Cells[rowIndex, 8].SetValue(dr["MGR2_DAY_RATE_AVG_1Y"]);
                if (dr["MGR3_CUR_LEVEL"].AsString() == "Z") {
                    ws.Cells[rowIndex, 9].Value = "從其高(" + (dr["MGR3_CUR_CM"].AsDecimal() * 100).AsString() + "%)";
                }
                else {
                    ws.Cells[rowIndex, 9].Value = dr["MGR3_CUR_LEVEL"].AsString();
                }
                if (dr["DAY_CNT"].AsInt() == 0) {
                    RichTextString richText = new RichTextString();
                    richText.AddTextRun("-", new RichTextRunFont("Times New Roman", 12));
                    ws.Cells[rowIndex, 10].SetRichText(richText);
                }
                else {
                    RichTextString richText = new RichTextString();
                    richText.AddTextRun(dr["DAY_CNT"].AsInt().ToString(), new RichTextRunFont("Times New Roman", 12));
                    ws.Cells[rowIndex, 10].SetRichText(richText);
                }

                ws.Cells[rowIndex, 11].SetValue(dr["TFXM1_PRICE"]);
                ws.Cells[rowIndex, 12].SetValue(dr["AI5_PRICE"]);
                ws.Cells[rowIndex, 13].SetValue(dr["TS_UPDOWN"]);
                ws.Cells[rowIndex, 14].SetValue(dr["TI_UPDOWN"]);
                ws.Cells[rowIndex, 15].SetValue(dr["YS_UPDOWN"]);
                ws.Cells[rowIndex, 16].SetValue(dr["YI_UPDOWN"]);
                ws.Cells[rowIndex, 17].SetValue(dr["AI2_OI"]);
                ws.Cells[rowIndex, 18].SetValue(dr["AI2_M_QNTY"]);
            }

            //刪除空白列
            //刪"本日無"
            if (dt.Rows.Count > 0) {
                range = ws.Range[(startRow - 3 + 1) + ":" + (startRow - 3 + 1)];
                range.Delete(DeleteMode.EntireRow);
                startRow = startRow - 1;
                minusRow = minusRow + 1;
            }
            //刪多餘空白列
            if (dt.Rows.Count < totalRow) {
                //沒資料連表頭都刪
                if (dt.Rows.Count == 0) {
                    startRow = startRow - 3;
                    totalRow = totalRow + 4;
                    //表頭3列+表尾註1列
                }
                range = ws.Range[(startRow + dt.Rows.Count + 1 + 1) + ":" + (startRow + totalRow + 1)];
                range.Delete(DeleteMode.EntireRow);
                minusRow = minusRow + (totalRow - dt.Rows.Count);
            }
            return minusRow;
        }

        protected int wf_42011_2(int minusRow, Worksheet ws, DataTable dt) {
            int rowIndex, startRow, totalRow, f, headRow;
            string rptName;
            Range range;

            startRow = 322 - 1;
            totalRow = 300;
            startRow = startRow - minusRow;
            rowIndex = startRow;
            headRow = 4 - 1;

            if (!cbx1.Checked) headRow = headRow - 1;
            if (!cbx2.Checked) {
                //刪明細
                startRow = 321 - minusRow;
                range = ws.Range[(startRow - 4 + 1) + ":" + (startRow + totalRow + 2 + 1)];
                range.Delete(DeleteMode.EntireRow);
                minusRow = minusRow + (totalRow + 5) + 2;  //5表首,2表尾
                //刪表頭
                range = ws.Range[(headRow + 1) + ":" + (headRow + 1)];
                range.Delete(DeleteMode.EntireRow);
                minusRow = minusRow + 1;
                //改編號
                if (cbx1.Checked) {
                    f = 2;
                }
                else {
                    f = 1;
                }
                ws.Cells[headRow, 1].Value = f.AsString();
                ws.Cells[headRow + 1, 1].Value = (f + 1).AsString();
                ws.Cells[headRow + 2, 1].Value = (f + 2).AsString();
                ws.Cells[headRow + 3, 1].Value = (f + 3).AsString();
                return minusRow;
            }

            if (!cbx1.Checked) ws.Cells[rowIndex - 4, 1].Value = "1";
            if (txtRange.Text != "8.5" || txtRate2.Text != "10.5" || txtRate3.Text != "13.5" || txtRate4.Text != "1.0" || lblCmRate.Text != "15") {
                //表首
                RichTextString richText = new RichTextString();
                richText = ws.Cells[headRow, 2].GetRichText();
                if (txtRange.Text != "8.5") {
                    f = richText.Text.IndexOf("8.5 %") + 1;
                    if (f > 0) richText.Characters(f - 1, 5).Text = txtRate1.Text + "%";
                }
                if (txtRate2.Text != "10.5") {
                    f = richText.Text.IndexOf("10.5 %") + 1;
                    if (f > 0) richText.Characters(f - 1, 6).Text = txtRate2.Text + "%";
                }
                if (txtRate3.Text != "13.5") {
                    f = richText.Text.IndexOf("13.5 %") + 1;
                    if (f > 0) richText.Characters(f - 1, 6).Text = txtRate3.Text + "%";
                }
                if (txtRate4.Text != "1.0") {
                    f = richText.Text.IndexOf("1.0 %") + 1;
                    if (f > 0) richText.Characters(f - 1, 5).Text = txtRate4.Text + "%";
                }
                if (lblCmRate.Text != "12") {
                    f = richText.Text.IndexOf("12 %") + 1;
                    if (f > 0) richText.Characters(f - 1, 4).Text = lblCmRate.Text + "%";
                }
                ws.Cells[headRow, 2].SetRichText(richText);
                //表頭
                richText = ws.Cells[rowIndex - 4, 2].GetRichText();
                if (txtRange.Text != "8.5") {
                    f = richText.Text.IndexOf("8.5 %") + 1;
                    if (f > 0) richText.Characters(f - 1, 5).Text = txtRate1.Text + "%";
                }
                if (txtRate2.Text != "10.5") {
                    f = richText.Text.IndexOf("10.5 %") + 1;
                    if (f > 0) richText.Characters(f - 1, 6).Text = txtRate2.Text + "%";
                }
                if (txtRate3.Text != "13.5") {
                    f = richText.Text.IndexOf("13.5 %") + 1;
                    if (f > 0) richText.Characters(f - 1, 6).Text = txtRate3.Text + "%";
                }
                if (txtRate4.Text != "1.0") {
                    f = richText.Text.IndexOf("1.0 %") + 1;
                    if (f > 0) richText.Characters(f - 1, 5).Text = txtRate4.Text + "%";
                }
                if (lblCmRate.Text != "12") {
                    f = richText.Text.IndexOf("12 %") + 1;
                    if (f > 0) richText.Characters(f - 1, 4).Text = lblCmRate.Text + "%";
                }
                ws.Cells[rowIndex - 4, 2].SetRichText(richText);
            }
            if (cbxRate.Checked) {
                DataView dv = dt.AsDataView();
                dv.RowFilter = "rpt3_flag='Y'";
                dt = dv.ToTable();
            }
            dt = dt.Sort("MGR2_DAY_RATE DESC, APDK_KIND_GRP2, APDK_KIND_LEVEL DESC, MGR3_KIND_ID");

            f = 0;
            foreach (DataRow dr in dt.Rows) {
                rowIndex++;
                f++;
                ws.Cells[rowIndex, 1].Value = f.AsString();
                ws.Cells[rowIndex, 2].Value = dr["MGR3_KIND_ID"].AsString();
                ws.Cells[rowIndex, 3].Value = dr["APDK_NAME"].AsString();
                ws.Cells[rowIndex, 4].Value = dr["MGR3_SID"].AsString();
                ws.Cells[rowIndex, 5].Value = dr["PID_NAME"].AsString();
                ws.Cells[rowIndex, 6].SetValue(dr["MGR2_DAY_RATE"]);
                ws.Cells[rowIndex, 7].SetValue(dr["T_30_RATE"]);
                ws.Cells[rowIndex, 8].SetValue(dr["MGR2_DAY_RATE_AVG_1Y"]);
                if (dr["MGR3_CUR_LEVEL"].AsString() == "Z") {
                    ws.Cells[rowIndex, 9].Value = "從其高(" + (dr["MGR3_CUR_CM"].AsDecimal() * 100).AsString() + "%)";
                }
                else {
                    ws.Cells[rowIndex, 9].Value = dr["MGR3_CUR_LEVEL"].AsString();
                }
                if (dr["DAY_CNT_3"].AsInt() == 0) {
                    RichTextString richText = new RichTextString();
                    richText.AddTextRun("-", new RichTextRunFont("Times New Roman", 12));
                    ws.Cells[rowIndex, 10].SetRichText(richText);
                }
                else {
                    RichTextString richText = new RichTextString();
                    richText.AddTextRun(dr["DAY_CNT_3"].AsInt().ToString(), new RichTextRunFont("Times New Roman", 12));
                    ws.Cells[rowIndex, 10].SetRichText(richText);
                }

                ws.Cells[rowIndex, 11].SetValue(dr["TFXM1_PRICE"]);
                ws.Cells[rowIndex, 12].SetValue(dr["AI5_PRICE"]);
                ws.Cells[rowIndex, 13].SetValue(dr["TS_UPDOWN"]);
                ws.Cells[rowIndex, 14].SetValue(dr["TI_UPDOWN"]);
                ws.Cells[rowIndex, 15].SetValue(dr["YS_UPDOWN"]);
                ws.Cells[rowIndex, 16].SetValue(dr["YI_UPDOWN"]);
                ws.Cells[rowIndex, 17].SetValue(dr["AI2_OI"]);
                ws.Cells[rowIndex, 18].SetValue(dr["AI2_M_QNTY"]);
            }

            //刪除空白列
            //刪"本日無"
            if (dt.Rows.Count > 0) {
                range = ws.Range[(startRow - 3 + 1) + ":" + (startRow - 3 + 1)];
                range.Delete(DeleteMode.EntireRow);
                startRow = startRow - 1;
                minusRow = minusRow + 1;
            }
            //刪多餘空白列
            if (dt.Rows.Count < totalRow) {
                //沒資料連表頭都刪
                if (dt.Rows.Count == 0) {
                    startRow = startRow - 3;
                    totalRow = totalRow + 4;
                    //表頭3列+表尾註1列
                }
                range = ws.Range[(startRow + dt.Rows.Count + 1 + 1) + ":" + (startRow + totalRow + 1)];
                range.Delete(DeleteMode.EntireRow);
                minusRow = minusRow + (totalRow - dt.Rows.Count);
            }

            return minusRow;
        }

        protected int wf_42011_3(int minusRow, Worksheet ws, DataTable dt) {
            int rowIndex, startRow, totalRow, f, headRow;
            string rptName;
            Range range;

            //3.現貨或期貨連續二日漲跌幅度≧12%之股票期貨(以現貨連續二日漲跌幅度之絕對值由大至小排序)
            startRow = 629 - 1;
            totalRow = 300;
            startRow = startRow - minusRow;
            rowIndex = startRow;
            headRow = 5 - 1;
            f = dt.Rows.Count;

            if (!cbx1.Checked) {
                headRow--;
                f--;
            }
            if (!cbx2.Checked) {
                headRow--;
                f--;
            }
            if (!cbx3.Checked) {
                //刪明細
                range = ws.Range[(startRow - 4 + 1) + ":" + (startRow + totalRow + 2 + 1)];
                range.Delete(DeleteMode.EntireRow);
                minusRow = minusRow + (totalRow + 5);
                //刪表頭
                f = 3;
                if (!cbx1.Checked) f--;
                if (!cbx2.Checked) f--;
                range = ws.Range[(headRow + 1) + ":" + (headRow + 1)];
                range.Delete(DeleteMode.EntireRow);
                minusRow = minusRow + 1;
                //改編號
                ws.Cells[headRow, 1].Value = f.AsString();
                ws.Cells[headRow + 1, 1].Value = (f + 1).AsString();
                ws.Cells[headRow + 2, 1].Value = (f + 2).AsString();
                return minusRow;
            }

            if (!cbx1.Checked) ws.Cells[rowIndex - 4, 1].Value = ws.Cells[rowIndex - 4, 1].Value.AsInt() - 1;
            if (!cbx2.Checked) ws.Cells[rowIndex - 4, 1].Value = ws.Cells[rowIndex - 4, 1].Value.AsInt() - 1;

            if (txtUpDown.Text != "12") {
                //表首
                //使用RichTextString才能保留原本設定的格式
                RichTextString richText = new RichTextString();
                richText = ws.Cells[headRow, 2].GetRichText();
                f = richText.Text.IndexOf("12%") + 1;
                if (f > 0) {
                    richText.Characters(f - 1, 3).Text = txtUpDown.Text + "%";
                    ws.Cells[headRow, 2].SetRichText(richText);
                }
                //表頭
                richText = ws.Cells[rowIndex - 4, 2].GetRichText();
                f = richText.Text.IndexOf("12%") + 1;
                if (f > 0) {
                    richText.Characters(f - 1, 3).Text = txtUpDown.Text + "%";
                    ws.Cells[rowIndex - 4, 2].SetRichText(richText);
                }
            }
            //DataView dv = dt.AsDataView();
            //dv.RowFilter = "ABS(YS_UPDOWN * 100) >= " + txtUpDown.Text + " or ABS(YI_UPDOWN*100) >= " + txtUpDown.Text;
            //dv.Sort = "ABS(YS_UPDOWN) DESC, APDK_KIND_GRP2, APDK_KIND_LEVEL DESC, MGR3_KIND_ID";
            //dt = dv.ToTable();
            //dt = dt.AsEnumerable().Where(x => Math.Round(Math.Abs(x.Field<decimal>("YS_UPDOWN") * 100),16) >= txtUpDown.AsDecimal() ||
            //                        Math.Round(Math.Abs(x.Field<decimal>("YI_UPDOWN") * 100), 16) >= txtUpDown.AsDecimal())
            //                      .OrderByDescending(x => Math.Round(Math.Abs(x.Field<decimal>("YS_UPDOWN")),16))
            //                      .ThenBy(x => x.Field<string>("APDK_KIND_GRP2"))
            //                      .ThenByDescending(x => x.Field<int>("APDK_KIND_LEVEL"))
            //                      .ThenBy(x => x.Field<string>("MGR3_KIND_ID")).CopyToDataTable();
            dt = dt.AsEnumerable().Where(x => Math.Round(Math.Abs(x.Field<decimal>("YS_UPDOWN") * 100), 15,MidpointRounding.AwayFromZero) >= txtUpDown.AsDecimal() ||
                                 Math.Round(Math.Abs(x.Field<decimal>("YI_UPDOWN") * 100), 15, MidpointRounding.AwayFromZero) >= txtUpDown.AsDecimal())
                                 .OrderByDescending(x => Math.Abs(x.Field<decimal>("YS_UPDOWN")))
                                 .ThenBy(x => x.Field<string>("APDK_KIND_GRP2"))
                                 .ThenByDescending(x => x.Field<Int16>("APDK_KIND_LEVEL"))
                                 .ThenBy(x => x.Field<string>("MGR3_KIND_ID"))
                                .CopyToDataTable();

            f = 0;
            foreach (DataRow dr in dt.Rows) {
                rowIndex++;
                f++;
                ws.Cells[rowIndex, 1].Value = f.AsString();
                ws.Cells[rowIndex, 2].Value = dr["MGR3_KIND_ID"].AsString();
                ws.Cells[rowIndex, 3].Value = dr["APDK_NAME"].AsString();
                ws.Cells[rowIndex, 4].Value = dr["MGR3_SID"].AsString();
                ws.Cells[rowIndex, 5].Value = dr["PID_NAME"].AsString();
                ws.Cells[rowIndex, 6].SetValue(dr["TFXM1_PRICE"]);
                ws.Cells[rowIndex, 7].SetValue(dr["AI5_PRICE"]);
                ws.Cells[rowIndex, 8].SetValue(dr["TS_UPDOWN"]);
                ws.Cells[rowIndex, 9].SetValue(dr["TI_UPDOWN"]);
                ws.Cells[rowIndex, 10].SetValue(dr["YS_UPDOWN"]);
                ws.Cells[rowIndex, 11].SetValue(dr["YI_UPDOWN"]);
                ws.Cells[rowIndex, 12].SetValue(dr["T_30_RATE"]);
                ws.Cells[rowIndex, 13].SetValue(dr["MGR2_DAY_RATE"]);
                if (dr["MGR3_CUR_LEVEL"].AsString() == "Z") {
                    RichTextString richText = new RichTextString();
                    richText.AddTextRun("從其高", new RichTextRunFont("標楷體", 12));
                    richText.AddTextRun("(" + (dr["MGR3_CUR_CM"].AsDecimal() * 100).AsString() + "%)", new RichTextRunFont("Times New Roman", 12));
                    ws.Cells[rowIndex, 14].SetRichText(richText);
                }
                else {
                    RichTextString richText = new RichTextString();
                    richText.AddTextRun(dr["MGR3_CUR_LEVEL"].AsString(), new RichTextRunFont("Times New Roman", 12));
                    ws.Cells[rowIndex, 14].SetRichText(richText);
                }

                ws.Cells[rowIndex, 15].SetValue(dr["AI2_OI"]);
                ws.Cells[rowIndex, 16].SetValue(dr["AI2_M_QNTY"]);
            }

            //刪除空白列
            //刪"本日無"
            if (dt.Rows.Count > 0) {
                range = ws.Range[(startRow - 3 + 1) + ":" + (startRow - 3 + 1)];
                range.Delete(DeleteMode.EntireRow);
                startRow = startRow - 1;
                minusRow = minusRow + 1;
            }
            //刪多餘空白列
            if (dt.Rows.Count < totalRow) {
                //沒資料連表頭都刪
                if (dt.Rows.Count == 0) {
                    startRow = startRow - 3;
                    totalRow = totalRow + 3;
                }
                range = ws.Range[(startRow + dt.Rows.Count + 1 + 1) + ":" + (startRow + totalRow + 1)];
                range.Delete(DeleteMode.EntireRow);
                minusRow = minusRow + (totalRow - dt.Rows.Count);
            }
            return minusRow;
        }
    }
}