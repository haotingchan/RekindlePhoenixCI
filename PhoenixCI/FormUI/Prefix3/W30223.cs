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
            txtSDate.EditValue = PbFunc.f_ocf_date(0);
            txtEDate.EditValue = txtSDate.EditValue;

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
                lblProcessing.Visible = true;
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

                ws = workbook.Worksheets["附件6-2英文公告"];
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                //wf_30223_ch();

                //wf_30223_eng()

                ////原始資料
                //ii_ole_row = 2
                //wf_30223_data()
                #endregion

            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            return ResultStatus.Success;

        }

        /// <summary>
        /// 公告表－個股
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ws"></param>
        protected void wf_30223_ch2(DataTable dt, Worksheet ws) {
            string ls_str;
            int ii_ole_row, li_ole_row_tol, li_minus, li_seq, ll_found;
            Range range;

            //寫資料
            //調高
            DataView dv = dt.AsDataView();
            dv.RowFilter = "pls2_level_adj<>'-' and pls2_level_adj<>' '";
            DataTable dtFiltered = dv.ToTable();
            ii_ole_row = 4 - 1;
            li_ole_row_tol = 500 + ii_ole_row;
            li_seq = 0;

            foreach (DataRow dr in dtFiltered.Rows) {
                if (dr["PLS2_KIND_ID2"].AsString() != dr["APDK_KIND_GRP2"].AsString()) continue;
                li_seq = li_seq + 1;
                ii_ole_row = ii_ole_row + 1;
                ws.Cells[ii_ole_row, 0].Value = li_seq.AsString();
                ls_str = dr["APDK_NAME"].AsString();
                if (dr["PLS2_FUT"].AsString() == "F" && dr["PLS2_OPT"].AsString() == "O") {
                    ls_str = ls_str + "及選擇權";
                }
                ws.Cells[ii_ole_row, 1].Value = ls_str;
                ws.Cells[ii_ole_row, 2].Value = dr["PLS2_KIND_ID2"].AsString();
                ws.Cells[ii_ole_row, 3].Value = dr["PLS2_SID"].AsString();
                ws.Cells[ii_ole_row, 4].Value = dr["PLS2_LEVEL"].AsString();
                ws.Cells[ii_ole_row, 5].SetValue(dr["PLS2_NATURE"]);
                ws.Cells[ii_ole_row, 6].SetValue(dr["PLS2_LEGAL"]);
                ws.Cells[ii_ole_row, 7].SetValue(dr["PLS2_999"]);
                //找出小型商品
                DataRow[] find = dtFiltered.Select("apdk_kind_grp2 = '" + dr["PLS2_KIND_ID2"].AsString() + "' and pls2_kind_id2 <>  apdk_kind_grp2 ");
                if (find.Length > 0) {
                    foreach (DataRow drFind in find) {
                        ls_str = ls_str + dr["PLS2_KIND_ID2"].AsString() + ",";
                    }
                }
                ws.Cells[ii_ole_row, 8].Value = ls_str.SubStr(0, ls_str.Length - 1);
            }//foreach (DataRow dr in dtFiltered.Rows)

            //刪除空白列
            li_minus = li_ole_row_tol - ii_ole_row;
            if (li_ole_row_tol > ii_ole_row) {
                range = ws.Range[(ii_ole_row + 1).ToString() + ":" + li_ole_row_tol.ToString()];
                range.Delete(DeleteMode.EntireRow);
            }

            //降低
            dv = dt.AsDataView();
            dv.RowFilter = "pls2_level_adj='-'";
            dtFiltered = dv.ToTable();
            ii_ole_row = 508 - li_minus;
            li_ole_row_tol = 500 + ii_ole_row;
            li_seq = 0;

            foreach (DataRow dr in dtFiltered.Rows) {
                if (dr["PLS2_KIND_ID2"].AsString() != dr["APDK_KIND_GRP2"].AsString()) continue;
                li_seq = li_seq + 1;
                ii_ole_row = ii_ole_row + 1;
                ws.Cells[ii_ole_row, 0].Value = li_seq.AsString();
                ls_str = dr["APDK_NAME"].AsString();
                if (dr["PLS2_FUT"].AsString() == "F" && dr["PLS2_OPT"].AsString() == "O") {
                    ls_str = ls_str + "及選擇權";
                }
                ws.Cells[ii_ole_row, 1].Value = ls_str;
                ws.Cells[ii_ole_row, 2].Value = dr["PLS2_KIND_ID2"].AsString();
                ws.Cells[ii_ole_row, 3].Value = dr["PLS2_SID"].AsString();
                ws.Cells[ii_ole_row, 4].Value = dr["PLS2_LEVEL"].AsString();
                ws.Cells[ii_ole_row, 5].SetValue(dr["PLS2_NATURE"]);
                ws.Cells[ii_ole_row, 6].SetValue(dr["PLS2_LEGAL"]);
                ws.Cells[ii_ole_row, 7].SetValue(dr["PLS2_999"]);
                //找出小型商品
                DataRow[] find = dtFiltered.Select("apdk_kind_grp2 = '" + dr["PLS2_KIND_ID2"].AsString() + "' and pls2_kind_id2 <>  apdk_kind_grp2 ");
                if (find.Length > 0) {
                    foreach (DataRow drFind in find) {
                        ls_str = ls_str + dr["PLS2_KIND_ID2"].AsString() + ",";
                    }
                }
                ws.Cells[ii_ole_row, 8].Value = ls_str.SubStr(0, ls_str.Length - 1);
            }//foreach (DataRow dr in dtFiltered.Rows)

            //刪除空白列
            if (dtFiltered.Rows.Count == 0) {
                //刪表頭
                ii_ole_row = ii_ole_row - 4;
            }
            if (li_ole_row_tol > ii_ole_row) {
                range = ws.Range[(ii_ole_row + 1).ToString() + ":" + li_ole_row_tol.ToString()];
                range.Delete(DeleteMode.EntireRow);
            }
        }

        /// <summary>
        /// 公告表－個股(英文版)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ws"></param>
        protected void wf_30223_eng2(DataTable dt, Worksheet ws) {
            string ls_str, is_raise_ymd, is_lower_ymd;
            int ii_ole_row, li_ole_row_tol, li_minus, li_seq, ll_found;
            Range range;

            //寫資料
            //Raising
            is_raise_ymd = dt.Rows[0]["pls2_effective_ymd"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
            ws.Cells[1, 8].Value = ws.Cells[1, 8].Value.ToString() + is_raise_ymd;

            DataView dv = dt.AsDataView();
            dv.RowFilter = "pls2_level_adj<>'-' and pls2_level_adj<>' '";
            DataTable dtFiltered = dv.ToTable();
            ii_ole_row = 5;
            li_ole_row_tol = 500 + ii_ole_row;
            li_seq = 0;

            foreach (DataRow dr in dtFiltered.Rows) {
                if (dr["PLS2_KIND_ID2"].AsString() != dr["APDK_KIND_GRP2"].AsString()) continue;
                li_seq = li_seq + 1;
                ii_ole_row = ii_ole_row + 1;
                ws.Cells[ii_ole_row, 0].Value = li_seq.AsString();
                ws.Cells[ii_ole_row, 1].Value = dr["PLS2_KIND_ID2"].AsString();

                if (dr["PLS2_FUT"].AsString() == "F") ws.Cells[ii_ole_row, 2].Value = "○";
                if (dr["PLS2_OPT"].AsString() == "F") ws.Cells[ii_ole_row, 3].Value = "○";
                ws.Cells[ii_ole_row, 4].Value = dr["PLS2_SID"].AsString();
                ws.Cells[ii_ole_row, 5].Value = dr["PLS2_LEVEL"].AsString();
                ws.Cells[ii_ole_row, 6].SetValue(dr["PLS2_NATURE"]);
                ws.Cells[ii_ole_row, 7].SetValue(dr["PLS2_LEGAL"]);
                ws.Cells[ii_ole_row, 8].SetValue(dr["PLS2_999"]);

                //找出小型商品
                ls_str = "";
                DataRow[] find = dtFiltered.Select("apdk_kind_grp2 = '" + dr["PLS2_KIND_ID2"].AsString() + "' and pls2_kind_id2 <>  apdk_kind_grp2 ");
                if (find.Length > 0) {
                    foreach (DataRow drFind in find) {
                        ls_str = ls_str + dr["PLS2_KIND_ID2"].AsString() + ",";
                    }
                }
                ws.Cells[ii_ole_row, 9].Value = ls_str.SubStr(0, ls_str.Length - 1);

            }//foreach (DataRow dr in dtFiltered.Rows)

            //刪除空白列
            li_minus = li_ole_row_tol - ii_ole_row;
            if (li_ole_row_tol > ii_ole_row) {
                range = ws.Range[(ii_ole_row + 1).ToString() + ":" + li_ole_row_tol.ToString()];
                range.Delete(DeleteMode.EntireRow);
            }

            //Lowing
            dv = dt.AsDataView();
            dv.RowFilter = "pls2_level_adj='-'";
            dtFiltered = dv.ToTable();
            ii_ole_row = 509 - li_minus;
            li_ole_row_tol = 500 + ii_ole_row;
            li_seq = 0;
            is_lower_ymd = dtFiltered.Rows[0]["PLS2_EFFECTIVE_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");

            foreach (DataRow dr in dtFiltered.Rows) {
                if (dr["PLS2_KIND_ID2"].AsString() != dr["APDK_KIND_GRP2"].AsString()) continue;
                li_seq = li_seq + 1;
                ii_ole_row = ii_ole_row + 1;

                ws.Cells[ii_ole_row, 0].Value = li_seq.AsString();
                ws.Cells[ii_ole_row, 1].Value = dr["PLS2_KIND_ID2"].AsString();
                if (dr["PLS2_FUT"].AsString() == "F") ws.Cells[ii_ole_row, 2].Value = "○";
                if (dr["PLS2_OPT"].AsString() == "F") ws.Cells[ii_ole_row, 3].Value = "○";
                ws.Cells[ii_ole_row, 4].Value = dr["PLS2_SID"].AsString();
                ws.Cells[ii_ole_row, 5].Value = dr["PLS2_LEVEL"].AsString();
                ws.Cells[ii_ole_row, 6].SetValue(dr["PLS2_NATURE"]);
                ws.Cells[ii_ole_row, 7].SetValue(dr["PLS2_LEGAL"]);
                ws.Cells[ii_ole_row, 8].SetValue(dr["PLS2_999"]);

                //找出小型商品
                ls_str = "";
                DataRow[] find = dtFiltered.Select("apdk_kind_grp2 = '" + dr["PLS2_KIND_ID2"].AsString() + "' and pls2_kind_id2 <>  apdk_kind_grp2 ");
                if (find.Length > 0) {
                    foreach (DataRow drFind in find) {
                        ls_str = ls_str + dr["PLS2_KIND_ID2"].AsString() + ",";
                    }
                }
                ws.Cells[ii_ole_row, 9].Value = ls_str.SubStr(0, ls_str.Length - 1);
            }//foreach (DataRow dr in dtFiltered.Rows)

            //刪除空白列
            if (dtFiltered.Rows.Count == 0) {
                //刪表頭
                ii_ole_row = ii_ole_row - 4;
            }
            if (li_ole_row_tol > ii_ole_row) {
                range = ws.Range[(ii_ole_row + 1).ToString() + ":" + li_ole_row_tol.ToString()];
                range.Delete(DeleteMode.EntireRow);
            }

            ii_ole_row = ii_ole_row + 2;
            ws.Cells[ii_ole_row, 0].Value = ws.Cells[ii_ole_row, 0].Value.ToString() + is_raise_ymd + ".";
            ii_ole_row = ii_ole_row + 1;
            ws.Cells[ii_ole_row, 0].Value = ws.Cells[ii_ole_row, 0].Value.ToString() + is_lower_ymd + ".";
        }
    }
}