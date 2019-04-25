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
using Common;
using BaseGround.Shared;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;

/// <summary>
/// Lukas, 2019/3/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    /// <summary>
    /// 42033 股票類(STF)期貨價格及現貨資料下載
    /// </summary>
    public partial class W42033 : FormParent {

        private D42033 dao42033;

        public W42033(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {

            try {
                base.Open();
                txtEDate.DateTimeValue = DateTime.Now;
                txtSDate.DateTimeValue = txtEDate.DateTimeValue;
#if DEBUG
                txtSDate.Text = "2016/01/04";
                txtEDate.Text = "2016/01/04";
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

        protected override ResultStatus Export() {

            try {
                lblProcessing.Visible = true;
                dao42033 = new D42033();
                string rptId, file, rptName, kindId = "";
                decimal  sheetKindCnt, kindTotCnt; //商品總個數
                int rowStart=0, colStart, f, colNum, sheetCnt;
                rptId = "42033";
                rptName = "股票類(STF)期貨價格及現貨資料";
                lblProcessing.Text = rptId + "－" + rptName + " 轉檔中...";

                //讀取資料(當日保證金適用比例)
                DataTable dt42033 = dao42033.d_42033(txtSDate.Text.Replace("/", ""), txtEDate.Text.Replace("/", ""));
                if (dt42033.Rows.Count == 0) {
                    MessageDisplay.Info(PbFunc.f_ocf_date(1).SubStr(0, 6) + "," + rptId + '－' + rptName + ",讀取「股票類(STF)期貨價格及現貨資料」無任何資料!");
                    lblProcessing.Visible = false;
                    return ResultStatus.Fail;
                }
                DataView dv = dt42033.AsDataView();
                dv.Sort="KIND_ID, data_date";
                DataTable dtSorted = dv.ToTable();

                //複製檔案
                file = PbFunc.wf_copy_file(rptId, rptId);
                if (file == "") return ResultStatus.Fail;

                //開啟檔案
                Workbook workbook = new Workbook();
                workbook.LoadDocument(file);


                //切換sheet
                //1個Sheet最多可放63個商品
                Worksheet ws42033 = workbook.Worksheets[0];
                ws42033.Cells[0, 0].Value = txtSDate.Text + "至" + txtEDate.Text + ws42033.Cells[0, 0].Value;

                colStart = -3;
                //兩個運算欄位
                //CP_KIND_SEQ_NO: if(isnull( kind_id[-1]) or  kind_id[0] <> kind_id[-1],1,0)
                //CP_TOT_KIND_CNT: sum(cp_kind_seq_no)
                kindTotCnt = dtSorted.AsEnumerable().Select(q => q.Field<string>("KIND_ID")).Distinct().Count().AsDecimal();  //全部總商品數
                //複製Sheet
                for (f = 1; f <= Math.Ceiling(kindTotCnt / 63) - 1; f++) {
                    workbook.Worksheets.Add();
                    // 新增worksheet兩種方式:
                    // workbook.Worksheets.Add().Name="";
                    // workbook.Worksheets.Insert(0,"");
                    Worksheet wsNew = workbook.Worksheets[f];
                    wsNew.CopyFrom(ws42033);
                }
                for (f = 0; f < Math.Ceiling(kindTotCnt / 63); f++) {
                    workbook.Worksheets[f].Name = "42033_" + (f+1).AsString();
                }

                //複製column
                sheetKindCnt = 1;  //每個sheet商品數
                sheetCnt = 0;
                ws42033 = workbook.Worksheets[sheetCnt];
                for (f = 2; f <= kindTotCnt; f++) {
                    sheetKindCnt = sheetKindCnt + 1;
                    //可擺放的最大商品數 = 63 = truncate((256 - 1日期欄) / 4column)
                    if (sheetKindCnt > 63) {
                        sheetCnt = sheetCnt + 1;
                        ws42033 = workbook.Worksheets[sheetCnt];
                        sheetKindCnt = 1;
                        continue;
                    }

                    ws42033.Cells[0, (sheetKindCnt.AsInt() * 4) - 3].CopyFrom(ws42033.Range["B:E"]);
                }


                //填值
                sheetCnt = 0;
                ws42033 = workbook.Worksheets[sheetCnt];
                sheetKindCnt = 0;
                foreach (DataRow dr in dtSorted.Rows) {
                    if (dr["kind_id"].AsString() != kindId) {
                        kindId = dr["kind_id"].AsString();
                        sheetKindCnt = sheetKindCnt + 1;
                        //每4個column為一組, 若超過256限制則結束
                        colStart = colStart + 4;
                        if (colStart > 252) {
                            sheetCnt = sheetCnt + 1;
                            ws42033 = workbook.Worksheets[sheetCnt];
                            sheetKindCnt = 0;
                            colStart = 1;
                        }
                        //Head
                        //1.股票期貨英文代碼
                        //2.股票期貨中文簡稱
                        //3.股票期貨標的證券代號
                        //4.上市/上櫃類別
                        rowStart = 4;
                        for (colNum = 2; colNum <= 5; colNum++) {
                            ws42033.Cells[rowStart, colStart].Value = dr["KIND_ID"].AsString();
                            ws42033.Cells[rowStart, colStart+1].Value = dr["APDK_NAME"].AsString();
                            ws42033.Cells[rowStart, colStart+2].Value = dr["APDK_STOCK_ID"].AsString();
                            ws42033.Cells[rowStart, colStart+3].Value = dr["PID_NAME"].AsString();
                        }
                        rowStart = 5;
                    }
                    //Detial
                    //1.期貨結算價
                    //2.期貨開盤參考價
                    //3.標的指數收盤價
                    //4.標的指數開盤參考價
                    //第1個商品才輸日期
                    rowStart = rowStart + 1;
                    if (sheetKindCnt == 1) {
                        ws42033.Cells[rowStart, 0].Value = dr["DATA_DATE"].AsDateTime().ToString("yyyy/MM/dd");
                    }
                    ws42033.Cells[rowStart, colStart].SetValue(dr["F_SETTLE_PRICE"]);
                    ws42033.Cells[rowStart, colStart + 1].SetValue(dr["F_OPEN_REF"]);
                    ws42033.Cells[rowStart, colStart + 2].SetValue(dr["T_PRICE"]);
                    ws42033.Cells[rowStart, colStart + 3].SetValue(dr["T_OPEN_REF"]);
                }

                //存檔
                workbook.SaveDocument(file);
                lblProcessing.Text = "轉檔成功";
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }
    }
}