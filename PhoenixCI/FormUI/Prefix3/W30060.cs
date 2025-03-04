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
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using Common;
using System.Threading;
using System.IO;

/// <summary>
/// Lukas, 2019/3/6
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30060 各商品每日成交紀錄
    /// </summary>
    public partial class W30060 : FormParent {

        private D30060 dao30060;

        public W30060(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            dao30060 = new D30060();
        }

        protected override ResultStatus Open() {
            try {
                base.Open();
                txtEDate.DateTimeValue = GlobalInfo.OCF_DATE;
                txtSDate.EditValue = txtEDate.Text.SubStr(0, 8) + "01";
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
                if (!cbxDay.Checked && !cbxNight.Checked) {
                    MessageDisplay.Error("請選擇盤別");
                    ShowMsg("");
                    return ResultStatus.Fail;
                }
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);
                lblProcessing.Visible = true;
                ShowMsg("開始轉檔...");
                string file,rptId = "30060", rptName = "各商品每日成交紀錄"; ;
                string symd = txtSDate.Text.Replace("/", "");
                string eymd = txtEDate.Text.Replace("/", "");
                // 1. 複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") {
                    return ResultStatus.Fail;
                }
                ShowMsg(rptId + '－' + rptName + " 轉檔中...");

                // 2. 開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);
                Worksheet ws30060 = workbook.Worksheets[0];

                // 3. 匯出資料
                DataTable dt30060 = new DataTable();
                DataTable dtProd = new DataTable();
                if (cbxDay.Checked) {
                    // 切換Sheet
                    ws30060 = workbook.Worksheets[0];
                    // 讀取資料
                     dt30060 = dao30060.d_30060(symd, eymd,"%"); //不分日夜盤
                    if (dt30060.Rows.Count == 0) {
                        MessageDisplay.Info(GlobalInfo.OCF_DATE.ToString("yyyyMM") + "," + rptId + '－' + rptName + ",無任何資料!");
                        ShowMsg("");
                    }
                    wf_30060(ws30060, dt30060);
                    //週到期商品 2019/06/28 新增sheet3 不分日夜盤
                    dtProd = dao30060.d_30060_prod(symd, eymd, "%");
                    ws30060 = workbook.Worksheets[2];
                    wf_30060_prod(ws30060, dtProd);
                }
                if (cbxNight.Checked) {
                    // 切換Sheet
                    ws30060 = workbook.Worksheets[1];
                    // 讀取資料
                     dt30060 = dao30060.d_30060(symd, eymd, "1"); //夜盤
                    if (dt30060.Rows.Count == 0) {
                        MessageDisplay.Info(GlobalInfo.OCF_DATE.ToString("yyyyMM") + "," + rptId + '－' + rptName + ",無任何資料!");
                        ShowMsg("");
                    }
                    wf_30060(ws30060, dt30060);
                    //週到期商品 2019/06/28 新增sheet4 夜盤
                    dtProd = dao30060.d_30060_prod(symd, eymd, "1");
                    ws30060 = workbook.Worksheets[3];
                    wf_30060_prod(ws30060, dtProd);
                }



                if (dt30060.Rows.Count == 0 && dtProd.Rows.Count==0) {
                    workbook = null;
                    File.Delete(file);
                    return ResultStatus.Fail;
                }

                // 4. 存檔
                ws30060.ScrollToRow(0);
                workbook.SaveDocument(file);
                ShowMsg("轉檔成功");
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                ShowMsg("轉檔有錯誤");
                throw ex;
            }
            finally {
                this.Cursor = Cursors.Arrow;
                this.Refresh();
                Thread.Sleep(5);
            }
            return ResultStatus.Success;
        }

        private void wf_30060(Worksheet ws30060, DataTable dt) {
            int rowNum = 0, colNum;
            string ymd;

            ymd = "";
            foreach (DataRow dr in dt.Rows) {
                if (ymd != dr["AI2_YMD"].AsString()) {
                    ymd = dr["AI2_YMD"].AsString();
                    rowNum = rowNum + 1;
                    ws30060.Cells[rowNum, 0].Value = ymd.SubStr(0, 4) + "/" + ymd.SubStr(4, 2) + "/" + ymd.SubStr(6, 2);
                }
                // 交易量
                colNum = dr["M_COL_SEQ"].AsInt() - 1;
                ws30060.Cells[rowNum, colNum].SetValue(dr["AI2_M_QNTY"]);
                // OI
                colNum = dr["OI_COL_SEQ"].AsInt() - 1;
                ws30060.Cells[rowNum, colNum].SetValue(dr["AI2_OI"]);
            }

        }

        private void wf_30060_prod(Worksheet ws30060, DataTable dt) {
            int rowNum = 0, colNum;
            string ymd;

            ymd = "";
            foreach (DataRow dr in dt.Rows) {
                if (ymd != dr["AI2_YMD"].AsString()) {
                    ymd = dr["AI2_YMD"].AsString();
                    rowNum = rowNum + 1;
                    ws30060.Cells[rowNum, 0].Value = ymd.SubStr(0, 4) + "/" + ymd.SubStr(4, 2) + "/" + ymd.SubStr(6, 2);
                }
                // 交易量
                colNum = dr["M_COL_SEQ"].AsInt() - 1;
                ws30060.Cells[rowNum, colNum].SetValue(dr["AI2_M_QNTY"]);
                // OI
                colNum = dr["OI_COL_SEQ"].AsInt() - 1;
                ws30060.Cells[rowNum, colNum].SetValue(dr["AI2_OI"]);
            }

        }
    }
}