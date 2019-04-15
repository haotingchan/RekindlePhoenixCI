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
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using BusinessObjects;
using System.Threading;

/// <summary>
/// Lukas, 2019/3/28
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
    /// <summary>
    /// 30202 股價指數暨黃金類商品部位限制數計算
    /// </summary>
    public partial class W30202 : FormParent {

        private D30202 dao30202;

        public W30202(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {

            try {
                base.Open();
                DateTime date = DateTime.Now;
                //本次
                date = PbFunc.relativedate(date, (date.Day * -1));
                txtCurEymd.DateTimeValue = date;
                txtDate.DateTimeValue = date;
                txtCurEMonth.DateTimeValue = date;
                date = PbFunc.relativedate(date, (date.Day * -1));
                date = PbFunc.relativedate(date, (date.Day * -1));
                txtCurSMonth.DateTimeValue = date;
                //前次
                date = PbFunc.relativedate(date, (date.Day * -1));
                txtPrevEymd.DateTimeValue = date;
                txtEMonth.DateTimeValue = date;
                date = PbFunc.relativedate(date, (date.Day * -1));
                date = PbFunc.relativedate(date, (date.Day * -1));
                txtSMonth.DateTimeValue = date;
#if DEBUG
                txtDate.Text = "2019/03/31";
                txtSMonth.Text = "2018/10";
                txtEMonth.Text = "2018/12";
                txtCurSMonth.Text = "2019/01";
                txtCurEMonth.Text = "2019/03";
#endif
                txtDate.Focus();
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
            string showMsg = "";
            try {
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");
                dao30202 = new D30202();
                //判斷是否有檔案,決定是否要寫入DB.
                showMsg = "讀取既有計算資料錯誤";
                string cpYmd = txtDate.DateTimeValue.ToString("yyyyMMdd");
                DataTable dtPL1 = dao30202.d_30202_pl1(cpYmd);
                if (dtPL1.Rows.Count > 0) {
                    DialogResult result = MessageDisplay.Choose("已有計算資料,是否要更新資料庫資料?");
                    if (result == DialogResult.No) {
                        cbxDB.Checked = false;
                    }
                }
                txtPrevEymd.DateTimeValue = PbFunc.f_get_end_day("DATE", "", txtEMonth.Text);
                txtCurEymd.DateTimeValue = PbFunc.f_get_end_day("DATE", "", txtCurEMonth.Text);

                string rptId = "30202", rptName = "股價指數暨黃金類商品部位限制數檢視表", file,
                       curSMonth = txtCurSMonth.Text.Replace("/", ""),
                       curEMonth = txtCurEMonth.Text.Replace("/", ""),
                       sMonth = txtSMonth.Text.Replace("/", ""),
                       eMonth = txtEMonth.Text.Replace("/", "");
                decimal natureSdt = txtMultiNature.Text.AsDecimal() / 100;
                decimal legalSdt = txtMultiLegal.Text.AsDecimal() / 100;

                //讀取資料
                showMsg = "讀取資料錯誤";
                DataTable dt30202 = dao30202.d_30202(cpYmd, sMonth, eMonth, curSMonth, curEMonth, natureSdt, legalSdt);
                if (dt30202.Rows.Count == 0) {
                    MessageDisplay.Info(eMonth + "," + rptId + '－' + rptName + ",無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }

                ShowMsg(rptId + '－' + rptName + " 轉檔中...");
                //複製檔案
                showMsg = "複製檔案錯誤";
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return ResultStatus.Fail;

                //開啟檔案
                showMsg = "開啟檔案錯誤";
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);

                //切換Sheet
                showMsg = "切換Sheet錯誤";
                Worksheet ws30202 = workbook.Worksheets[0];

                //寫入資料
                showMsg = "寫入資料錯誤";
                int rowIndex = 2;
                lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";

                #region wf_30202
                string str, natureType, legalType, col;
                decimal value1, changeRange, curNature, curLegal, cpNature, cpLegal, nature, legal;
                if (!cbxDB.Checked) ws30202.Cells[0, 0].Value = ws30202.Cells[0, 0].Value.AsString() + "(試算)";
                //(一)
                ws30202.Cells[3, 1].Value = "(" + txtSMonth.Text + "/01" + "～" + txtPrevEymd.Text + ")";
                //(二)
                ws30202.Cells[3, 3].Value = "(" + txtCurSMonth.Text + "/01" + "～" + txtCurEymd.Text + ")";
                //(六)
                str = txtCurEMonth.Text;
                DateTime date = (str + "/01").AsDateTime("yyyy/MM/dd");
                do {
                    date = PbFunc.relativedate(date, date.Day * -1);
                    str = str + "、" + date.ToString("yyyy/MM～") + txtCurEMonth.Text;
                } while ((date.ToString("yyyy/MM") + "/01").AsDateTime("yyyy/MM/dd") > txtSMonth.DateTimeValue);//只比年月
                ws30202.Cells[3, 12].Value = str;

                foreach (DataRow dr in dt30202.Rows) {
                    rowIndex = dr["RPT_SEQ_NO"].AsInt() - 1;
                    decimal pAvgQnty = dr["P_AVG_QNTY"].AsDecimal();
                    decimal pAvgOi = dr["P_AVG_OI"].AsDecimal();
                    //前次檢視之數值
                    ws30202.Cells[rowIndex, 1].Value = pAvgQnty;
                    ws30202.Cells[rowIndex, 2].Value = pAvgOi;
                    //本次檢視之數值
                    ws30202.Cells[rowIndex, 3].Value = dr["C_AVG_QNTY"].AsDecimal();
                    ws30202.Cells[rowIndex, 4].Value = dr["C_AVG_OI"].AsDecimal();
                    //相較前次數值增減幅度
                    if (pAvgQnty > pAvgOi) value1 = pAvgQnty; else value1 = pAvgOi;
                    if (value1 == 0) changeRange = -1; else changeRange = Math.Round(dr["C_MAX_VALUE"].AsDecimal() / value1 - 1, 4, MidpointRounding.AwayFromZero);
                    ws30202.Cells[rowIndex, 5].Value = changeRange;
                    dr["CHANGE_RANGE"] = changeRange;
                    //現行部位限制數
                    curNature = dr["PL2_NATURE"] == DBNull.Value ? -1 : dr["PL2_NATURE"].AsDecimal();//當該欄位的值為DBNull時等於-1 (權宜做法)
                    curLegal = dr["PL2_LEGAL"] == DBNull.Value ? -1 : dr["PL2_LEGAL"].AsDecimal();//當該欄位的值為DBNull時等於-1 (權宜做法)
                    if (dr["PL2_NATURE"] != DBNull.Value) ws30202.Cells[rowIndex, 6].Value = curNature;
                    if (dr["PL2_LEGAL"] != DBNull.Value) ws30202.Cells[rowIndex, 7].Value = curLegal;

                    //按交易規則檢視後之部位限制數
                    //自然人
                    value1 = dr["PLT1_T1_MULTIPLE"] == DBNull.Value ? -1 : dr["PLT1_T1_MULTIPLE"].AsDecimal();//當該欄位的值為DBNull時等於-1 (權宜做法)
                    if (dr["PLT1_T1_MIN_NATURE"] == DBNull.Value) {
                        cpNature = dr["C_MAX_VALUE"].AsDecimal() * natureSdt;
                        if (value1 > 0) cpNature = Math.Truncate(cpNature / value1) * value1;
                    }
                    else {
                        cpNature = dr["PLT1_T1_MIN_NATURE"].AsDecimal();
                    }
                    ws30202.Cells[rowIndex, 9].Value = cpNature;
                    dr["CP_NATURE"] = cpNature;
                    //法人
                    value1 = dr["PLT1_T2_MULTIPLE"] == DBNull.Value ? -1 : dr["PLT1_T2_MULTIPLE"].AsDecimal();//當該欄位的值為DBNull時等於-1 (權宜做法)
                    if (dr["PLT1_T2_MIN_LEGAL"] == DBNull.Value) {
                        cpLegal = dr["C_MAX_VALUE"].AsDecimal() * legalSdt;
                        if (value1 > 0) cpLegal = Math.Truncate(cpLegal / value1) * value1;
                    }
                    else {
                        cpLegal = dr["PLT1_T2_MIN_LEGAL"].AsDecimal();
                    }
                    ws30202.Cells[rowIndex, 10].Value = cpLegal;
                    dr["CP_LEGAL"] = cpLegal;
                    dr["CP_999"] = cpLegal * 3;

                    //近1~6月日均交易量與未沖銷量
                    /****************************
                    1. 檢視部位限制級距時，若該期間之每日平均交易量或未沖銷量與前次調整時相較，其增減未逾百分之二‧五時，雖達調整級距標準，仍不調整。
                    2.針對須降低部位限制數之商品再增加以最近1、2、4、5、6個月區間資料檢視，並取其數額大者為基準數，惟計算後不得超過前次之部位限制數。
                    ****************************/
                    //(isnull(ld_cur_nature) and isnull(ld_cur_legal))這個條件拿掉，因為不會成立
                    if (Math.Abs(changeRange) <= 0.025m ||
                            (curNature == -1 && curLegal == -1) ||
                                (curNature == cpNature && curLegal == cpLegal)) {
                        str = "不適用";
                        if (curNature != -1 && curLegal != -1) {
                            nature = curNature;
                            legal = curLegal;
                        }
                        else {
                            nature = cpNature;
                            legal = cpLegal;
                        }
                        natureType = "不變";
                        legalType = "不變";
                    }
                    else {
                        if (cpNature < curNature || cpLegal < curLegal) col = "max";//最大者
                        else col = "min";//最小者

                        str = "近" + dr[col + "_MONTH_SEQ_NO"].AsString() + "個月";
                        if (dr[col + "_TYPE"].AsString() == "OI") str = str + "未沖銷量"; else str = str + "交易量";
                        if (col == "max") str = str + "最大者"; else str = str + "最小者";
                        str = str + "(" + dr[col + "_VALUE"].AsDecimal().ToString("#,##0") + ")";
                        //自然人
                        value1 = dr["PLT1_R1_MULTIPLE"] == DBNull.Value ? -1 : dr["PLT1_R1_MULTIPLE"].AsDecimal();//當該欄位的值為DBNull時等於-1 (權宜做法)
                        if (dr["PLT1_R1_MIN_NATURE"] == DBNull.Value) {
                            nature = dr[col + "_VALUE"].AsDecimal() * natureSdt;
                            if (value1 > 0) {
                                nature = Math.Truncate(nature / value1) * value1;
                            }
                        }
                        else {
                            nature = dr["PLT1_R1_MIN_NATURE"].AsDecimal();
                        }
                        natureType = "不變";
                        if (nature < curNature) natureType = "降低"; else if (nature > curNature) natureType = "調高";
                        //法人
                        value1 = dr["PLT1_R2_MULTIPLE"] == DBNull.Value ? -1 : dr["PLT1_R2_MULTIPLE"].AsDecimal();//當該欄位的值為DBNull時等於-1 (權宜做法)
                        if (dr["PLT1_R2_MIN_LEGAL"] == DBNull.Value) {
                            legal = dr[col + "_VALUE"].AsDecimal() * legalSdt;
                            if (value1 > 0) {
                                legal = Math.Truncate(legal / value1) * value1;
                            }
                        }
                        else {
                            legal = dr["PLT1_R2_MIN_LEGAL"].AsDecimal();
                        }
                        legalType = "不變";
                        if (legal < curLegal) legalType = "降低"; else if (legal > curLegal) legalType = "調高";
                    }
                    ws30202.Cells[rowIndex, 12].Value = str;
                    ws30202.Cells[rowIndex, 14].Value = nature;
                    ws30202.Cells[rowIndex, 15].Value = legal;
                    ws30202.Cells[rowIndex, 16].Value = natureType;
                    ws30202.Cells[rowIndex, 17].Value = legalType;
                    dr["NATURE"] = nature;
                    dr["LEGAL"] = legal;
                    dr["P999"] = legal * 3;
                    switch (natureType) {
                        case "降低":
                            str = "-";
                            break;
                        case "調高":
                            str = "+";
                            break;
                        default:
                            str = " ";
                            break;
                    }
                    dr["NATURE_ADJ"] = str;
                    switch (legalType) {
                        case "降低":
                            str = "-";
                            break;
                        case "調高":
                            str = "+";
                            break;
                        default:
                            str = " ";
                            break;
                    }
                    dr["LEGAL_ADJ"] = str;
                    dr["P999_ADJ"] = str;
                    dr["YMD"] = cpYmd;
                    //針對須調降之商品再增加檢視標準後之部位限制數
                }//foreach (DataRow dr in dt30202.Rows)

                //表尾
                rowIndex = dao30202.row_index();
                if (rowIndex > 0) ws30202.Cells[rowIndex - 1, 0].Value = ws30202.Cells[rowIndex - 1, 0].Value.ToString() +
                                                                         "自然人乘以" + txtMultiNature.Text +
                                                                         "%，法人乘以" + txtMultiLegal.Text + "%)";

                //存檔
                ws30202.ScrollToRow(0);
                workbook.SaveDocument(file);
                ShowMsg("轉檔成功");
                #endregion

                if (!cbxDB.Checked) {
                    return ResultStatus.Success;
                }
                #region wf_30202_write
                bool dbCommit = false;
                //刪除PL0的資料
                showMsg = "PL0刪除失敗";
                dbCommit = dao30202.DeletePL0ByDate(cpYmd);
                if (!dbCommit) {
                    MessageDisplay.Error(showMsg);
                    return ResultStatus.Fail;
                }
                //新增PL0的資料
                showMsg = "PL0新增失敗";
                dbCommit = dao30202.InsertPL0(cpYmd, sMonth, eMonth, curSMonth, curEMonth, GlobalInfo.USER_ID);
                if (!dbCommit) {
                    MessageDisplay.Error(showMsg);
                    return ResultStatus.Fail;
                }
                //刪除PL2的資料
                showMsg = "PL2刪除失敗";
                dbCommit = dao30202.DeletePL2ByDate(cpYmd);
                if (!dbCommit) {
                    MessageDisplay.Error(showMsg);
                    return ResultStatus.Fail;
                }
                //刪除PL1的資料
                showMsg = "PL1刪除失敗";
                dbCommit = dao30202.DeletePL1ByDate(cpYmd);
                if (!dbCommit) {
                    MessageDisplay.Error(showMsg);
                    return ResultStatus.Fail;
                }

                //寫入
                dtPL1.Clear();
                dtPL1.AcceptChanges();
                for (int f = 0; f < dt30202.Rows.Count; f++) {
                    dtPL1.Rows.Add();
                    for (int g = 0; g < 24; g++) {
                        dtPL1.Rows[dtPL1.Rows.Count - 1][g] = dt30202.Rows[f][g];
                        //現行自然人,現行法人PL
                        if (g == 9 || g == 11) {
                            if (dt30202.Rows[f][g] == DBNull.Value) dtPL1.Rows[dtPL1.Rows.Count - 1][g] = 0;
                        }
                    }
                    dtPL1.Rows[dtPL1.Rows.Count - 1][24] = DateTime.Now;
                    dtPL1.Rows[dtPL1.Rows.Count - 1][25] = GlobalInfo.USER_ID;
                }
                try {
                    ResultData myResultData = dao30202.updatePL1(dtPL1);
                }
                catch (Exception ex) {
                    MessageDisplay.Error("計算結果新增至資料庫錯誤! ");
                    WriteLog(ex);
                    return ResultStatus.Fail;
                }
                #endregion
            }
            catch (Exception ex) {
                MessageDisplay.Error(showMsg);
                throw ex;
            }
            finally {
                this.Cursor = Cursors.Arrow;
                this.Refresh();
                Thread.Sleep(5);
            }
            return ResultStatus.Success;
        }
    }
}