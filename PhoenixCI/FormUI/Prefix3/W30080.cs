﻿using System;
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
using System.IO;
using System.Threading;

/// <summary>
/// Lukas, 2019/4/1
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
    /// <summary>
    /// 30080 各檔股票期貨、選擇權交易量、未平倉統計
    /// </summary>
    public partial class W30080 : FormParent {

        private D30080 dao30080;

        /// <summary>
        /// 資料別
        /// </summary>
        protected string DataType {
            get {
                return rgpData.EditValue.AsString();
            }
        }

        public W30080(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {

            try {
                base.Open();
                txtEDate.DateTimeValue = GlobalInfo.OCF_DATE;
                txtSDate.EditValue = txtEDate.Text.SubStr(0, 8) + "01";
#if DEBUG
                txtSDate.Text = "2018/10/01";
                txtEDate.Text = "2018/10/31";
#endif

                txtSDate.Focus();
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
                dao30080 = new D30080();
                string rptId, file, rptName, symd, eymd,
                       underlyingMarket = "", paramKey = "", prodType, kindID;
                rptId = "30080";
                rptName = "各檔股票期貨、選擇權交易量、未平倉統計";
                ShowMsg(rptId + "－" + rptName + " 轉檔中...");

                //輸入條件
                symd = txtSDate.DateTimeValue.ToString("yyyyMMdd");
                eymd = txtEDate.DateTimeValue.ToString("yyyyMMdd");
                file = "(" + "日期-" + symd + "-" + eymd;

                //市場別
                switch (rgpMarket.SelectedIndex) {
                    case 0:
                        underlyingMarket = "%";
                        paramKey = "%";
                        break;
                    case 1:
                        underlyingMarket = "1%";
                        paramKey = "ST%";
                        file = file + rgpMarket.EditValue;
                        break;
                    case 2:
                        underlyingMarket = "2%";
                        paramKey = "ST%";
                        file = file + rgpMarket.EditValue;
                        break;
                    case 3:
                        underlyingMarket = "%";
                        paramKey = "ET%";
                        file = file + rgpMarket.EditValue;
                        break;
                }

                //類別
                if (rgpType.EditValue.AsString() == "F") {
                    prodType = "F";
                    file = file + "_期貨";
                }
                else {
                    prodType = "O";
                    file = file + "_選擇權";
                }

                //資料
                file += (DataType == "M_QNTY" ? "交易量" : "未平倉量");

                //統計前
                if (txtRank.Text != "999") {
                    file = file + "_統計前" + txtRank.Text + "大";
                }

                //股票代號
                if (txtKindID.Text.Trim() == "%") {
                    kindID = "%";
                }
                else {
                    kindID = txtKindID.Text.Trim() + "%";
                    if (txtKindID.Text.Trim().IndexOf("%") < 0) {
                        file = file + "_股票商品代號-" + txtKindID.Text.Trim();
                    }
                }
                file = file + ")";

                //RPT
                DataTable dtDate = dao30080.d_30080_date(prodType, symd, eymd, paramKey, kindID, underlyingMarket);
                if (dtDate.Rows.Count == 0) {
                    MessageDisplay.Info(rptId + '－' + "日期無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }

                DataTable dtReSorted = dao30080.d_30080_sort(prodType, symd, eymd, paramKey, kindID, underlyingMarket, DataType, txtRank.Text);
                if (dtReSorted.Rows.Count == 0) {
                    MessageDisplay.Info(rptId + '－' + "資料值無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }

                //DataView dv = dtSort.AsDataView();
                //dv.Sort = "AI2_" + DataType + " DESC";
                ////dv.RowFilter = "rownum <=" + txtRank.Text;
                //DataTable dtReSorted = dv.ToTable();

                //讀取資料 (每日)
                DataTable dt30080 = dao30080.d_30080(prodType, symd, eymd, paramKey, kindID, underlyingMarket);
                if (dt30080.Rows.Count == 0) {
                    MessageDisplay.Info(rptId + '－' + "資料值無任何資料!");
                    lblProcessing.Text = rptId + '－' + "資料值無任何資料!";
                    return ResultStatus.Fail;
                }

                int f = 0, found;
                string ymd = "", tab = ",", content = "交易日", seq = "排序", kindName = "";
                //避免重複寫入
                file = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH,
                                    rptId + file + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv");
                if (File.Exists(file)) {
                    File.Delete(file);
                }
                File.Create(file).Close();
                foreach (DataRow dr in dtReSorted.Rows) {
                    f++;
                    kindID = dr["AI2_KIND_ID"].ToString();
                    seq = seq + tab + f.ToString();
                    content = content + tab + kindID;
                    kindName = kindName + tab + dr["PDK_NAME"].ToString();
                }
                FileWrite(file, seq);
                FileWrite(file, content);
                FileWrite(file, kindName);
                
                foreach (DataRow drDate in dtDate.Rows) {
                    ymd = drDate["AI2_YMD"].ToString();
                    DataView dv30080 = dt30080.AsDataView();
                    dv30080.RowFilter = "AI2_YMD='" + ymd + "'";
                    DataTable dtFiltered = dv30080.ToTable();

                    content = ymd;
                    foreach (DataRow drReSorted in dtReSorted.Rows) {
                        kindID = drReSorted["AI2_KIND_ID"].ToString();

                        DataRow[] find = dtFiltered.Select("AI2_KIND_ID='" + kindID + "'");
                        if (find.Length != 0) {
                            found = dtFiltered.Rows.IndexOf(find[0]);
                        }
                        else {
                            found = -1;
                        }
                        if (found < 0) {
                            content = content + tab + "0";
                        }
                        else {
                            if (dtFiltered.Rows[found]["AI2_" + DataType] != DBNull.Value) {
                                content = content + tab + dtFiltered.Rows[found]["AI2_" + DataType].AsDecimal();
                            }
                            else {
                                content = content + tab;
                            }
                        }
                    }
                    FileWrite(file, content);
                    ShowMsg("轉檔成功");
                }
            }
            catch (Exception ex) {
                ShowMsg("轉檔失敗");
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
        /// 重複寫入文字並換行
        /// </summary>
        /// <param name="openData">檔案路徑</param>
        /// <param name="textToAdd">文字內容</param>
        private void FileWrite(string openData, string textToAdd) {
            using (FileStream fs = new FileStream(openData, FileMode.Append)) {
                using (StreamWriter writer = new StreamWriter(fs, Encoding.GetEncoding(950))) {
                    writer.WriteLine(textToAdd);
                }
            }
        }
    }
}