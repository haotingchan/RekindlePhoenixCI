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
using System.Diagnostics;
using BaseGround.Shared;
using System.IO;
using ServiceStack.Text;
using DataObjects.Dao.Together.TableDao;
using Common;
using BusinessObjects.Enums;
using DataObjects.Dao.Together.SpecificDao;

/// <summary>
/// Lukas, 2019/1/28
/// </summary>
namespace PhoenixCI.FormUI.Prefix2 {

    /// <summary>
    /// 20112 20110資料轉入
    /// </summary>
    public partial class W20112 : FormParent {

        private INTWSE1 daoINTWSE1;
        private INOTC1 daoINOTC1;
        private D20112 dao20112;

        public W20112(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            GridHelper.SetCommonGrid(gvMain);
            txtYear.EditValue = PbFunc.f_ocf_date(0).SubStr(0, 4);
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
            _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
            _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

            _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
            _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

            _ToolBtnImport.Enabled = false;//匯入
            _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
            _ToolBtnPrintAll.Enabled = false;//列印

            return ResultStatus.Success;
        }


        #region LinkLabel事件
        /// <summary>
        /// 下載上櫃資料的連結
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shl_1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            shl_1.LinkVisited = true;
            Process.Start("IExplore", "http://www.tpex.org.tw/web/stock/aftertrading/daily_trading_index/st41rpk.php");
        }

        /// <summary>
        /// 下載上市資料的連結
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shl_2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            shl_2.LinkVisited = true;
            Process.Start("IExplore", "http://www.twse.com.tw/zh/page/trading/exchange/FMTQIK.html");
        }
        #endregion

        #region Button事件

        /// <summary>
        /// 上櫃 wf_20112_2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPath1_Click(object sender, EventArgs e) {
            int i, j, li_row;
            string ls_start_ymd, ls_end_ymd, ls_ym;
            string ls_ymd, ls_val, ls_type;
            ls_ym = "";

            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "*.csv (*.csv)|*.csv";
            open.Title = "請點選儲存檔案之目錄";
            DialogResult openResult = open.ShowDialog();
            if (openResult == DialogResult.OK) {
                txtPath1.Text = open.FileName;
                DataTable dt = new DataTable();
                for (i = 1; i < 7; i++) {
                    DataColumn dc = new DataColumn("Col" + i, typeof(string));
                    dt.Columns.Add(dc);
                }
                DataTable csvDt = OpenCSV(txtPath1.Text, dt, Encoding.Default);
                daoINOTC1 = new INOTC1();
                DataTable targetDt = daoINOTC1.ListAll();
                ls_type = csvDt.Rows[2]["Col5"].AsString();
                if (ls_type == null || ls_type.SubStr(0, 2) != "櫃買") {
                    MessageBox.Show("轉入資料來源錯誤!", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPath1.BackColor = Color.Red;
                    return;
                }
                li_row = 0;
                ls_end_ymd = "";
                ls_start_ymd = "";
                for (i = 0; i < csvDt.Rows.Count; i++) {
                    DataRow csvDr = csvDt.Rows[i];
                    ls_ymd = csvDr["Col1"].AsString();
                    ls_ymd = (ls_ymd.SubStr(0, 3).AsInt() + 1911).AsString() + ls_ymd.SubStr(4, 2) + ls_ymd.SubStr(ls_ymd.Length - 2, 2);
                    int n;
                    if (ls_ymd == null || int.TryParse(ls_ymd, out n) == false) {
                        continue;
                    }
                    li_row = li_row + 1;
                    if (li_row == 1) {
                        ls_start_ymd = ls_ymd.SubStr(0, 6) + "01";
                    }
                    ls_end_ymd = ls_ymd;
                    DataRow newDr = targetDt.NewRow();
                    targetDt.Rows.Add(newDr);
                    targetDt.Rows[targetDt.Rows.Count - 1][0] = ls_ymd;
                    for (j = 1; j < 6; j++) {
                        ls_val = csvDt.Rows[i][j].AsString();
                        if (ls_val.IndexOf(",") > 0) {
                            ls_val.Replace(",", "");
                        }
                        if (j == 3) {
                            ls_val = (ls_val.AsDecimal() * 1000).AsString();
                        }
                        targetDt.Rows[targetDt.Rows.Count - 1][j] = ls_val.AsDecimal();
                    }
                    targetDt.Rows[targetDt.Rows.Count - 1][6] = GlobalInfo.USER_ID;
                    targetDt.Rows[targetDt.Rows.Count - 1][7] = DateTime.Now;
                }
                ls_ym = ls_end_ymd.SubStr(0, 6);

                //刪除資料
                daoINOTC1.DeleteByDate(ls_start_ymd, ls_end_ymd);
                lblRange1.Text = ls_start_ymd.Insert(4, "/").Insert(7, "/") + "~" + ls_end_ymd.Insert(4, "/").Insert(7, "/");

                //更新資料
                ResultStatus result = base.Save_Override(targetDt, "INOTC1");
                if (result == ResultStatus.Success) {
                    MessageBox.Show(lblRange1.Text + "資料轉入完成!", "處理結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPath1.BackColor = Color.LightGray;
                }
                else {
                    MessageBox.Show(ls_ym + " 轉入資料庫失敗!", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else {
                MessageBox.Show(ls_ym + " 轉入資料庫失敗!", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// 上市 wf_20112_1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPath2_Click(object sender, EventArgs e) {
            int i, j, li_row;
            string ls_start_ymd, ls_end_ymd, ls_ym;
            string ls_ymd, ls_val, ls_type;
            ls_ym = "";

            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "*.csv (*.csv)|*.csv";
            open.Title = "請點選儲存檔案之目錄";
            DialogResult openResult = open.ShowDialog();
            if (openResult == DialogResult.OK) {
                txtPath2.Text = open.FileName;
                DataTable dt = new DataTable();
                for (i = 1; i < 7; i++) {
                    DataColumn dc = new DataColumn("Col" + i, typeof(string));
                    dt.Columns.Add(dc);
                }
                DataTable csvDt = OpenCSV(txtPath2.Text, dt, Encoding.Default);
                daoINTWSE1 = new INTWSE1();
                DataTable targetDt = daoINTWSE1.ListAll();
                ls_type = csvDt.Rows[1]["Col5"].AsString();
                if (ls_type == null || ls_type.SubStr(0, 2) != "發行") {
                    MessageBox.Show("轉入資料來源錯誤!", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPath2.BackColor = Color.Red;
                    return;
                }
                li_row = 0;
                ls_end_ymd = "";
                ls_start_ymd = "";
                for (i = 0; i < csvDt.Rows.Count; i++) {
                    DataRow csvDr = csvDt.Rows[i];
                    ls_ymd = csvDr["Col1"].AsString();
                    ls_ymd = (ls_ymd.SubStr(0, 3).AsInt() + 1911).AsString() + ls_ymd.SubStr(4, 2) + ls_ymd.SubStr(ls_ymd.Length - 2, 2);
                    int n;
                    if (ls_ymd == null || int.TryParse(ls_ymd, out n) == false) {
                        continue;
                    }
                    li_row = li_row + 1;
                    if (li_row == 1) {
                        ls_start_ymd = ls_ymd.SubStr(0, 6) + "01";
                    }
                    ls_end_ymd = ls_ymd;
                    DataRow newDr = targetDt.NewRow();
                    targetDt.Rows.Add(newDr);
                    targetDt.Rows[targetDt.Rows.Count - 1][0] = ls_ymd;
                    for (j = 1; j < 6; j++) {
                        ls_val = csvDt.Rows[i][j].AsString();
                        if (ls_val.IndexOf(",") > 0) {
                            ls_val.Replace(",", "");
                        }
                        targetDt.Rows[targetDt.Rows.Count - 1][j] = ls_val.AsDecimal();
                    }
                    targetDt.Rows[targetDt.Rows.Count - 1][6] = GlobalInfo.USER_ID;
                    targetDt.Rows[targetDt.Rows.Count - 1][7] = DateTime.Now;
                }
                ls_ym = ls_end_ymd.SubStr(0, 6);

                //刪除資料
                daoINTWSE1.DeleteByDate(ls_start_ymd, ls_end_ymd);
                lblRange2.Text = ls_start_ymd.Insert(4, "/").Insert(7, "/") + "~" + ls_end_ymd.Insert(4, "/").Insert(7, "/");

                //更新資料
                ResultStatus result = base.Save_Override(targetDt, "INTWSE1");
                if (result == ResultStatus.Success) {
                    MessageBox.Show(lblRange1.Text + "資料轉入完成!", "處理結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPath2.BackColor = Color.LightGray;
                }
                else {
                    MessageBox.Show(ls_ym + " 轉入資料庫失敗!", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else {
                MessageBox.Show(ls_ym + " 轉入資料庫失敗!", "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e) {
            dao20112 = new D20112();
            string as_year = txtYear.Text;
            DataTable returnTable = dao20112.ListAllByDate(as_year);
            if (returnTable.Rows.Count == 0) {
                MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            gcMain.DataSource = returnTable;
            gcMain.Focus();
        }
        #endregion

        /// <summary>
        /// Lukas, 將CSV文件的數據讀取到DataTable中
        /// </summary>
        /// <param name="filePath">CSV文件路徑</param>
        /// <param name="dt">指定欄位的DataTable</param>
        /// <param name="encoding">CSV編碼格式</param>
        /// <returns></returns>
        private DataTable OpenCSV(string filePath, DataTable dt, Encoding encoding) {

            var csv = File.ReadAllText(filePath, encoding);
            foreach (var line in CsvReader.ParseLines(csv)) {
                string newLine = line;
                if (line.SubStr(line.Length - 1, 1) == ",") {
                    newLine = line.Remove(line.Length - 1, 1);
                }
                string[] strArray = CsvReader.ParseFields(newLine).ToArray();
                int columnCount = strArray.Length;
                DataRow dr = dt.NewRow();
                for (int j = 0; j < columnCount; j++) {
                    dr[j] = strArray[j].Replace("\"", "");
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}