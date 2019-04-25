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
using System.Threading;
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;

/// <summary>
/// Lukas, 2019/4/9
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
    /// <summary>
    /// 30223 個股類部位限制公告表
    /// </summary>
    public partial class W30223 : FormParent {

        private D30223 dao30223;

        public W30223(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao30223 = new D30223();
        }

        protected override ResultStatus Open() {
            base.Open();
            //日期
            txtSDate.DateTimeValue = GlobalInfo.OCF_DATE;
            txtEDate.DateTimeValue = txtSDate.DateTimeValue;

#if DEBUG
            txtSDate.EditValue = "2018/12/28";
            txtEDate.EditValue = txtSDate.EditValue;
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
                string rptId = "30223", file, rptName = "公告表－個股",
                       sDate = txtSDate.DateTimeValue.ToString("yyyyMMdd"),
                       eDate = txtEDate.DateTimeValue.ToString("yyyyMMdd");

                //讀取資料
                DataTable dt30223 = dao30223.d_30223(sDate, eDate);
                if (dt30223.Rows.Count == 0) {
                    MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }

                //複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return ResultStatus.Fail;

                //開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //切換Sheet
                Worksheet ws = workbook.Worksheets["附件6-1中文公告"];

                #region 5 Sheets
                //wf_30223_1() 只是讀取資料，已搬到前面
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                wf_30223_ch2(dt30223, ws);

                ws = workbook.Worksheets["附件6-2英文公告"];
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                wf_30223_eng2(dt30223, ws);

                ws = workbook.Worksheets["附件6-4公告附件"];
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                wf_30223_ch(dt30223, ws);

                ws = workbook.Worksheets["附件6-5英文附件"];
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                wf_30223_eng(dt30223, ws);

                //原始資料
                ws = workbook.Worksheets["data"];
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                wf_30223_data(dt30223, ws);
                #endregion

                //存檔
                workbook.SaveDocument(file);
                ShowMsg("轉檔成功");
            }
            catch (Exception ex) {
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

        /// <summary>
        /// 公告表－個股
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ws"></param>
        protected void wf_30223_ch2(DataTable dt, Worksheet ws) {
            string str;
            int rowIndex, rowTotal, rowMinus, seq, found;
            Range range;

            //寫資料
            //調高
            DataView dv = dt.AsDataView();
            dv.RowFilter = "pls2_level_adj<>'-' and pls2_level_adj<>' '";
            DataTable dtFiltered = dv.ToTable();
            rowIndex = 4 - 1;
            rowTotal = 500 + rowIndex;
            seq = 0;

            foreach (DataRow dr in dtFiltered.Rows) {
                if (dr["PLS2_KIND_ID2"].AsString() != dr["APDK_KIND_GRP2"].AsString()) continue;
                seq = seq + 1;
                rowIndex = rowIndex + 1;
                ws.Cells[rowIndex, 0].Value = seq.AsString();
                str = dr["APDK_NAME"].AsString();
                if (dr["PLS2_FUT"].AsString() == "F" && dr["PLS2_OPT"].AsString() == "O") {
                    str = str + "及選擇權";
                }
                ws.Cells[rowIndex, 1].Value = str;
                ws.Cells[rowIndex, 2].Value = dr["PLS2_KIND_ID2"].AsString();
                ws.Cells[rowIndex, 3].Value = dr["PLS2_SID"].AsString();
                ws.Cells[rowIndex, 4].Value = dr["PLS2_LEVEL"].AsString();
                ws.Cells[rowIndex, 5].SetValue(dr["PLS2_NATURE"]);
                ws.Cells[rowIndex, 6].SetValue(dr["PLS2_LEGAL"]);
                ws.Cells[rowIndex, 7].SetValue(dr["PLS2_999"]);
                //找出小型商品
                DataRow[] find = dtFiltered.Select("apdk_kind_grp2 = '" + dr["PLS2_KIND_ID2"].AsString() + "' and pls2_kind_id2 <>  apdk_kind_grp2 ");
                str = "";
                if (find.Length > 0) {
                    foreach (DataRow drFind in find) {
                        str = str + drFind["PLS2_KIND_ID2"].AsString() + ",";
                    }
                }
                ws.Cells[rowIndex, 8].Value = str.SubStr(0, str.Length - 1);
            }//foreach (DataRow dr in dtFiltered.Rows)

            //刪除空白列
            rowMinus = rowTotal - rowIndex;
            if (rowTotal > rowIndex) {
                range = ws.Range[(rowIndex + 1 + 1).ToString() + ":" + (rowTotal + 1).ToString()];
                range.Delete(DeleteMode.EntireRow);
            }
            //合併欄位在刪除空白列之後會跑掉，手動加回來
            ws.MergeCells(ws.Range["A3:A4"]);
            ws.MergeCells(ws.Range["B3:B4"]);
            ws.MergeCells(ws.Range["C3:C4"]);
            ws.MergeCells(ws.Range["D3:D4"]);
            ws.MergeCells(ws.Range["E3:E4"]);

            //降低
            dv = dt.AsDataView();
            dv.RowFilter = "pls2_level_adj='-'";
            dtFiltered = dv.ToTable();
            rowIndex = 508 - rowMinus - 1;
            rowTotal = 500 + rowIndex;
            seq = 0;

            foreach (DataRow dr in dtFiltered.Rows) {
                if (dr["PLS2_KIND_ID2"].AsString() != dr["APDK_KIND_GRP2"].AsString()) continue;
                seq++;
                rowIndex++;
                ws.Cells[rowIndex, 0].Value = seq.AsString();
                str = dr["APDK_NAME"].AsString();
                if (dr["PLS2_FUT"].AsString() == "F" && dr["PLS2_OPT"].AsString() == "O") {
                    str = str + "及選擇權";
                }
                ws.Cells[rowIndex, 1].Value = str;
                ws.Cells[rowIndex, 2].Value = dr["PLS2_KIND_ID2"].AsString();
                ws.Cells[rowIndex, 3].Value = dr["PLS2_SID"].AsString();
                ws.Cells[rowIndex, 4].Value = dr["PLS2_LEVEL"].AsString();
                ws.Cells[rowIndex, 5].SetValue(dr["PLS2_NATURE"]);
                ws.Cells[rowIndex, 6].SetValue(dr["PLS2_LEGAL"]);
                ws.Cells[rowIndex, 7].SetValue(dr["PLS2_999"]);
                //找出小型商品
                DataRow[] find = dtFiltered.Select("apdk_kind_grp2 = '" + dr["PLS2_KIND_ID2"].AsString() + "' and pls2_kind_id2 <>  apdk_kind_grp2 ");
                str = "";
                if (find.Length > 0) {
                    foreach (DataRow drFind in find) {
                        str = str + drFind["PLS2_KIND_ID2"].AsString() + ",";
                    }
                }
                ws.Cells[rowIndex, 8].Value = str.SubStr(0, str.Length - 1);
            }//foreach (DataRow dr in dtFiltered.Rows)

            //刪除空白列
            if (dtFiltered.Rows.Count == 0) {
                //刪表頭
                rowIndex = rowIndex - 4;
            }
            if (rowTotal > rowIndex) {
                range = ws.Range[(rowIndex + 1 + 1).ToString() + ":" + (rowTotal + 1).ToString()];
                range.Delete(DeleteMode.EntireRow);
            }
            //合併欄位在刪除空白列之後會跑掉，手動加回來
            int mergeRowIndex = 508 - rowMinus - 1;
            ws.MergeCells(ws.Range["A" + mergeRowIndex + ":" + "A" + (mergeRowIndex + 1)]);
            ws.MergeCells(ws.Range["B" + mergeRowIndex + ":" + "B" + (mergeRowIndex + 1)]);
            ws.MergeCells(ws.Range["C" + mergeRowIndex + ":" + "C" + (mergeRowIndex + 1)]);
            ws.MergeCells(ws.Range["D" + mergeRowIndex + ":" + "D" + (mergeRowIndex + 1)]);
            ws.MergeCells(ws.Range["E" + mergeRowIndex + ":" + "E" + (mergeRowIndex + 1)]);

            ws.ScrollToRow(0);
        }

        /// <summary>
        /// 公告表－個股(英文版)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ws"></param>
        protected void wf_30223_eng2(DataTable dt, Worksheet ws) {
            string str, raiseYmd, lowerYmd;
            int rowIndex, rowTotal, rowMinus, seq;
            Range range;

            //寫資料
            //Raising
            raiseYmd = dt.Rows[0]["pls2_effective_ymd"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
            ws.Cells[1, 8].Value = ws.Cells[1, 8].Value.ToString() + raiseYmd;

            DataView dv = dt.AsDataView();
            dv.RowFilter = "pls2_level_adj<>'-' and pls2_level_adj<>' '";
            DataTable dtFiltered = dv.ToTable();
            rowIndex = 5 - 1;
            rowTotal = 500 + rowIndex;
            seq = 0;

            foreach (DataRow dr in dtFiltered.Rows) {
                if (dr["PLS2_KIND_ID2"].AsString() != dr["APDK_KIND_GRP2"].AsString()) continue;
                seq = seq + 1;
                rowIndex = rowIndex + 1;
                ws.Cells[rowIndex, 0].Value = seq.AsString();
                ws.Cells[rowIndex, 1].Value = dr["PLS2_KIND_ID2"].AsString();

                if (dr["PLS2_FUT"].AsString() == "F") ws.Cells[rowIndex, 2].Value = "○";
                if (dr["PLS2_OPT"].AsString() == "O") ws.Cells[rowIndex, 3].Value = "○";
                ws.Cells[rowIndex, 4].Value = dr["PLS2_SID"].AsString();
                ws.Cells[rowIndex, 5].Value = dr["PLS2_LEVEL"].AsString();
                ws.Cells[rowIndex, 6].SetValue(dr["PLS2_NATURE"]);
                ws.Cells[rowIndex, 7].SetValue(dr["PLS2_LEGAL"]);
                ws.Cells[rowIndex, 8].SetValue(dr["PLS2_999"]);

                //找出小型商品
                str = "";
                DataRow[] find = dtFiltered.Select("apdk_kind_grp2 = '" + dr["PLS2_KIND_ID2"].AsString() + "' and pls2_kind_id2 <>  apdk_kind_grp2 ");
                if (find.Length > 0) {
                    foreach (DataRow drFind in find) {
                        str = str + drFind["PLS2_KIND_ID2"].AsString() + ",";
                    }
                }
                ws.Cells[rowIndex, 9].Value = str.SubStr(0, str.Length - 1);

            }//foreach (DataRow dr in dtFiltered.Rows)

            //刪除空白列
            rowMinus = rowTotal - rowIndex;
            if (rowTotal > rowIndex) {
                range = ws.Range[(rowIndex + 1 + 1).ToString() + ":" + (rowTotal + 1).ToString()];
                range.Delete(DeleteMode.EntireRow);
            }

            //Lowing
            dv = dt.AsDataView();
            dv.RowFilter = "pls2_level_adj='-'";
            dtFiltered = dv.ToTable();
            rowIndex = 509 - rowMinus - 1;
            rowTotal = 500 + rowIndex;
            seq = 0;
            lowerYmd = dtFiltered.Rows[0]["PLS2_EFFECTIVE_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");

            foreach (DataRow dr in dtFiltered.Rows) {
                if (dr["PLS2_KIND_ID2"].AsString() != dr["APDK_KIND_GRP2"].AsString()) continue;
                seq = seq + 1;
                rowIndex = rowIndex + 1;

                ws.Cells[rowIndex, 0].Value = seq.AsString();
                ws.Cells[rowIndex, 1].Value = dr["PLS2_KIND_ID2"].AsString();
                if (dr["PLS2_FUT"].AsString() == "F") ws.Cells[rowIndex, 2].Value = "○";
                if (dr["PLS2_OPT"].AsString() == "O") ws.Cells[rowIndex, 3].Value = "○";
                ws.Cells[rowIndex, 4].Value = dr["PLS2_SID"].AsString();
                ws.Cells[rowIndex, 5].Value = dr["PLS2_LEVEL"].AsString();
                ws.Cells[rowIndex, 6].SetValue(dr["PLS2_NATURE"]);
                ws.Cells[rowIndex, 7].SetValue(dr["PLS2_LEGAL"]);
                ws.Cells[rowIndex, 8].SetValue(dr["PLS2_999"]);

                //找出小型商品
                str = "";
                DataRow[] find = dtFiltered.Select("apdk_kind_grp2 = '" + dr["PLS2_KIND_ID2"].AsString() + "' and pls2_kind_id2 <>  apdk_kind_grp2 ");
                if (find.Length > 0) {
                    foreach (DataRow drFind in find) {
                        str = str + drFind["PLS2_KIND_ID2"].AsString() + ",";
                    }
                }
                ws.Cells[rowIndex, 9].Value = str.SubStr(0, str.Length - 1);
            }//foreach (DataRow dr in dtFiltered.Rows)

            //刪除空白列
            if (dtFiltered.Rows.Count == 0) {
                //刪表頭
                rowIndex = rowIndex - 4;
            }
            if (rowTotal > rowIndex) {
                range = ws.Range[(rowIndex + 1 + 1).ToString() + ":" + (rowTotal + 1).ToString()];
                range.Delete(DeleteMode.EntireRow);
            }

            rowIndex = rowIndex + 2;
            ws.Cells[rowIndex, 0].Value = ws.Cells[rowIndex, 0].Value.ToString() + raiseYmd + ".";
            rowIndex = rowIndex + 1;
            ws.Cells[rowIndex, 0].Value = ws.Cells[rowIndex, 0].Value.ToString() + lowerYmd + ".";

            ws.ScrollToRow(0);
        }

        /// <summary>
        /// 公告表－個股(附件6-4公告附件)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ws"></param>
        protected void wf_30223_ch(DataTable dt, Worksheet ws) {
            string str;
            int rowIndex, rowTotal, seq;
            Range range;

            //寫資料
            rowIndex = 3 - 1;
            rowTotal = 1000 + rowIndex;
            seq = 0;

            foreach (DataRow dr in dt.Rows) {
                rowIndex++;
                seq++;
                ws.Cells[rowIndex, 0].Value = seq.AsString();
                str = dr["APDK_NAME"].AsString();
                if (dr["PLS2_FUT"].AsString() == "F" && dr["PLS2_OPT"].AsString() == "O") {
                    str = str + "及選擇權";
                }
                ws.Cells[rowIndex, 1].Value = str;
                ws.Cells[rowIndex, 2].Value = dr["PLS2_KIND_ID2"].AsString();
                ws.Cells[rowIndex, 3].Value = dr["PLS2_SID"].AsString();
                ws.Cells[rowIndex, 4].Value = dr["PLS2_LEVEL"].AsString();

                if (dr["PLS2_KIND_ID2"].AsString() != dr["APDK_KIND_GRP2"].AsString()) {
                    ws.Cells[rowIndex, 5].Value = "＊";
                    ws.Cells[rowIndex, 6].Value = "＊";
                    ws.Cells[rowIndex, 7].Value = "＊";
                }
                else {
                    ws.Cells[rowIndex, 5].SetValue(dr["PLS2_NATURE"]);
                    ws.Cells[rowIndex, 6].SetValue(dr["PLS2_LEGAL"]);
                    ws.Cells[rowIndex, 7].SetValue(dr["PLS2_999"]);
                }
            }//foreach (DataRow dr in dt.Rows)

            //刪除空白列
            if (rowTotal > rowIndex) {
                range = ws.Range[(rowIndex + 1 + 1).ToString() + ":" + (rowTotal + 1).ToString()];
                range.Delete(DeleteMode.EntireRow);
            }
            ws.ScrollToRow(0);
        }

        /// <summary>
        /// 公告表－個股(附件6-5英文附件)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ws"></param>
        protected void wf_30223_eng(DataTable dt, Worksheet ws) {
            int rowIndex, rowTotal, seq;
            Range range;

            //寫資料
            rowIndex = 3 - 1;
            rowTotal = 1000 + rowIndex;
            seq = 0;

            foreach (DataRow dr in dt.Rows) {
                rowIndex++;
                seq++;
                ws.Cells[rowIndex, 0].Value = seq.AsString();
                ws.Cells[rowIndex, 1].Value = dr["PLS2_KIND_ID2"].AsString();
                if (dr["PLS2_FUT"].AsString() == "F") ws.Cells[rowIndex, 2].Value = "○";
                if (dr["PLS2_OPT"].AsString() == "O") ws.Cells[rowIndex, 3].Value = "○";

                ws.Cells[rowIndex, 4].Value = dr["PLS2_SID"].AsString();
                ws.Cells[rowIndex, 5].Value = dr["PLS2_LEVEL"].AsString();

                if (dr["PLS2_KIND_ID2"].AsString() != dr["APDK_KIND_GRP2"].AsString()) {
                    ws.Cells[rowIndex, 3].Value = " ";
                    ws.Cells[rowIndex, 6].Value = "*";
                    ws.Cells[rowIndex, 7].Value = "*";
                    ws.Cells[rowIndex, 8].Value = "*";
                }
                else {
                    ws.Cells[rowIndex, 6].SetValue(dr["PLS2_NATURE"]);
                    ws.Cells[rowIndex, 7].SetValue(dr["PLS2_LEGAL"]);
                    ws.Cells[rowIndex, 8].SetValue(dr["PLS2_999"]);
                }
            }//foreach (DataRow dr in dt.Rows)

            //刪除空白列
            if (rowTotal > rowIndex) {
                range = ws.Range[(rowIndex + 1 + 1).ToString() + ":" + (rowTotal + 1).ToString()];
                range.Delete(DeleteMode.EntireRow);
            }
            ws.ScrollToRow(0);
        }

        /// <summary>
        /// 原始資料
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ws"></param>
        protected void wf_30223_data(DataTable dt, Worksheet ws) {
            int rowIndex = 1;

            //寫資料
            foreach (DataRow dr in dt.Rows) {
                rowIndex++;
                for (int f = 0; f < 15; f++) {
                    ws.Cells[rowIndex, f].SetValue(dr[f]);
                }
            }//foreach (DataRow dr in dt.Rows)

            ws.ScrollToRow(0);
        }
    }
}