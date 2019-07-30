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
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using System.Globalization;
using DevExpress.Spreadsheet.Charts;

/// <summary>
/// Lukas, 2019/2/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30020 期貨交易累計開戶及交易戶數統計表
    /// </summary>
    public partial class W30020 : FormParent {

        private D30020 dao30020;

        public W30020(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
         txtEDate.ImeMode = ImeMode.Disable;
         txtSDate.ImeMode = ImeMode.Disable;
        }

        protected override ResultStatus Open() {
            base.Open();
            txtEDate.DateTimeValue = GlobalInfo.OCF_DATE;
            txtSDate.EditValue = txtEDate.Text.SubStr(0, 8) + "01";
            txtSDate.Focus();
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

        protected override ResultStatus Export() {

            dao30020 = new D30020();

            string rptId, file;
            rptId = "30020";
            /******************
            複製檔案
            ******************/
            file = PbFunc.wf_copy_file(rptId, rptId);
            if (file == "") {
                return ResultStatus.Fail;
            }
            /******************
            開啟檔案
            ******************/
            Workbook workbook = new Workbook();
            workbook.LoadDocument(file);

            #region 30021
            string rptName;
            string accType;
            DateTime date, maxDate;
            int i, j, rowNum, colNum, accRow, accRowTol;
            long found;

            rptName = "期貨交易累計開戶及交易戶數統計表";
            rptId = "30021";
            
            /******************
            讀取資料
            ******************/
            DataTable dt30021 = dao30020.d_30021(txtSDate.DateTimeValue, txtEDate.DateTimeValue);
            if (dt30021.Rows.Count == 0) {
                MessageDisplay.Info(txtSDate.Text + "～" + txtEDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                return ResultStatus.Fail;
            }
            //ACC_TYPE
            //不撈資料，只要schema
            DataTable dt30021_acc_type = dao30020.d_30021_acc_type(null, null);

            //切換sheet
            Worksheet ws30021 = workbook.Worksheets[0];

            /******************
            身份碼總列數
            隱藏於A2
            ******************/
            accRowTol = ws30021.Cells[1, 0].Value.AsInt();
            date = "1900/1/1".AsDateTime();
            maxDate = dt30021.Rows[0]["AB1_DATE"].AsDateTime();
            rowNum = 1;
            accRow = 2;
            colNum = 2;
            //ACC_TYPE
            for (i = accRow; i < accRowTol; i++) {
                dt30021_acc_type.Rows.Add();
                dt30021_acc_type.Rows[dt30021_acc_type.Rows.Count - 1]["AB1_ACC_TYPE"] = ws30021.Cells[i, 0].Value;
            }

            for (j = 0; j < dt30021.Rows.Count; j++) {
                rowNum = rowNum + 1;
                /* 換日期時,Row:重頭開始& Col:加1 */
                if (date != dt30021.Rows[j]["AB1_DATE"].AsDateTime()) {
                    rowNum = 1;
                    accRow = 2;
                    colNum = colNum + 1;
                    date = dt30021.Rows[j]["AB1_DATE"].AsDateTime();
                    /* 日期 */
                    ws30021.Cells[rowNum, colNum].Value = date;
                    /* 累計開戶數日期 */
                    if (j == 0) {
                        ws30021.Cells[rowNum, 2].Value = dt30021.Rows[j]["AB1_DATE"].AsDateTime();
                    }
                }
                accType = dt30021.Rows[j]["AB1_ACC_TYPE"].AsString();
                //DataTable的Select預設不分大小寫，這邊要將它開啟
                dt30021_acc_type.CaseSensitive = true;
                if (dt30021_acc_type.Select("AB1_ACC_TYPE = '" + accType + "'").Length > 0) {
                    rowNum = dt30021_acc_type.Rows.IndexOf(dt30021_acc_type.Select("AB1_ACC_TYPE = '" + accType + "'")[0]);
                }
                else {
                    rowNum = -1;
                }
                if (rowNum >= 0) {
                    /* 每日 */
                    rowNum = rowNum + 2;
                    ws30021.Cells[rowNum, colNum].SetValue(dt30021.Rows[j]["AB1_COUNT"]);
                    /* 累計開戶數 */
                    if (date == maxDate) {
                        ws30021.Cells[rowNum, 2].SetValue(dt30021.Rows[j]["AB1_ACCU_COUNT"]);
                    }
                }
            }
            #endregion

            #region 30022

            rptName = "期貨交易累計開戶及交易戶數統計表";
            rptId = "30022";

            //讀取資料
            /* 往前追溯100天 */
            DateTime relDate;
            relDate = PbFunc.relativedate(txtEDate.DateTimeValue, -100);
            DataTable dt30022 = dao30020.d_30022(relDate, txtEDate.DateTimeValue);
            if (dt30022.Rows.Count == 0) {
                MessageDisplay.Info(relDate.ToString("yyyy/MM/dd") + "～" + txtEDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
                return ResultStatus.Fail;
            }
            //切換sheet
            Worksheet ws30022 = workbook.Worksheets[1];

            rowNum = 0;
            for (j = 0; j < dt30022.Rows.Count; j++) {
                rowNum = rowNum + 1;
                ws30022.Cells[rowNum, 0].Value = dt30022.Rows[j]["AB1_DATE"].AsDateTime();
                ws30022.Cells[rowNum, 1].SetValue(dt30022.Rows[j]["AB1_COUNT"]);
                ws30022.Cells[rowNum, 2].SetValue(dt30022.Rows[j]["AB1_ACCU_COUNT"]);
                ws30022.Cells[rowNum, 3].SetValue(dt30022.Rows[j]["AB1_TRADE_COUNT"]);
            }

            #endregion

            //圖表重設
            ChartObject chartObjs = ws30021.Charts[0];
            string rcnt = (dt30022.Rows.Count + 1).AsString();
            ChartData chartDataArgs = new ChartData();
            ChartData chartData1 = new ChartData();
            ChartData chartData2 = new ChartData();
            Range range1 = ws30022.Range["='30022'!$D$2:$D$" + rcnt];
            Range range2 = ws30022.Range["='30022'!$C$2:$C$" + rcnt];
            Range args = ws30022.Range["='30022'!$A$2:$A$" + rcnt];
            chartData1.RangeValue = range1;
            chartData2.RangeValue = range2;
            chartDataArgs.RangeValue = args;
            chartObjs.Series[0].Arguments = chartDataArgs;
            chartObjs.Series[0].Values = chartData1;
            chartObjs.Series[1].Arguments = chartDataArgs;
            chartObjs.Series[1].Values = chartData2;

            workbook.SaveDocument(file);
            return ResultStatus.Success;
        }
    }
}