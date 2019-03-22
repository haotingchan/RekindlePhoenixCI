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
using BusinessObjects.Enums;
using BaseGround.Shared;
using Common;
using PhoenixCI.Shared;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;

/// <summary>
/// Lukas, 2019/3/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    /// <summary>
    /// 41020 個股契約價值查詢
    /// </summary>
    public partial class W41020 : BaseGround.FormParent {

        private D41020 dao41020;

        public W41020(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            try {
                base.Open();
                txtEDate.EditValue = PbFunc.f_ocf_date(0);
                txtSDate.EditValue = txtEDate.Text;
#if DEBUG
                //txtSDate.Text = "2014/01/01";
                //txtEDate.Text = "2017/12/31";
                //txtProd.Text = "";
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
                dao41020 = new D41020();
                string prodType, saveFilePath;
                prodType = rdgProdType.EditValue.AsString();

                //1. 點選儲存檔案之目錄
                saveFilePath = ReportExportFunc.wf_GetFileSaveName("41020" + "_" + DateTime.Now.ToString("yyyy.MM.dd-hh.mm.ss") + ".xls");
                if (string.IsNullOrEmpty(saveFilePath)) {
                    return ResultStatus.Fail;
                }
                DataTable dt41020 = dao41020.d_41020(prodType, txtSDate.DateTimeValue,txtEDate.DateTimeValue,txtProd.Text.Trim());
                if (dt41020.Rows.Count <= 0) {
                    MessageDisplay.Error("轉出筆數為０!");
                    return ResultStatus.Fail;
                }
                //2. 設定表頭
                dt41020.Columns["PCP_DATE"].ColumnName = "日期";
                dt41020.Columns["PCP_PROD_TYPE"].ColumnName = "商品別";
                dt41020.Columns["PDK_KIND_ID"].ColumnName = "契約代碼";
                dt41020.Columns["PDK_C_LAST_SETTLE_PRICE"].ColumnName = "最後結算價(P)";
                dt41020.Columns["PDK_STOCK_QNTY"].ColumnName = "約定標的證券股數(Q)";

                dt41020.Columns["PDK_STOCK_CASH2"].ColumnName = "現金股利(D)";
                dt41020.Columns["PDK_STOCK_PRICE3"].ColumnName = "現金增資認購價(X)";
                dt41020.Columns["PDK_STOCK_DATE3"].ColumnName = "現金增資繳款截止日";
                dt41020.Columns["PDK_STOCK_QNTY3"].ColumnName = "現金增資可認購股數(N)";
                dt41020.Columns["PDK_STOCK_CASH3"].ColumnName = "繳款已截止之現金增資相當價格(G)";

                dt41020.Columns["SFD_LAST_PRICE"].ColumnName = "計算現增價值之現貨價格(S)";
                dt41020.Columns["SDI_DISINVEST_RATE"].ColumnName = "減資換發後股數比例(R)";
                dt41020.Columns["SDI_COMB_RATE"].ColumnName = "合併換股比例";
                dt41020.Columns["PCP_LAST_SETTLE_PRICE"].ColumnName = "最後結算價(U1)=(P) *(Q)";
                dt41020.Columns["PCP_ADJ_LAST_SETTLE_PRICE"].ColumnName = "減資後調整最後結算價(U2)=(P/R)*1Q";

                dt41020.Columns["PCP_INCREASE_PRICE"].ColumnName = "現金增資相當價格(H1)=Max(S-X,0)*N";
                dt41020.Columns["H"].ColumnName = "H=H1，元以下捨去";
                dt41020.Columns["V"].ColumnName = "V=R>0則U2,否則U1值,元以下捨去";
                dt41020.Columns["PCP_PRICE"].ColumnName = "約定標的價值=V+D+[H]+G 註:[H]:繳款截止日>=到期日 時要加上H";
                dt41020.Columns["PDK_STOCK_ID"].ColumnName = "標的證券代碼";

                dt41020.Columns["pdk_trade_pause"].ColumnName = "暫停交易日";
                dt41020.Columns["CVAR_VAR_CODE_4"].ColumnName = "恢復交易日";
                dt41020.Columns["PDK_STATUS_CODE"].ColumnName = "狀態代碼";

                Workbook wb = new Workbook();
                wb.Worksheets[0].Import(dt41020, true, 0, 0);
                wb.Worksheets[0].Name = "41020" + "_" + DateTime.Now.ToString("yyyy.MM.dd-hh.mm.ss");
                //存檔
                wb.SaveDocument(saveFilePath);
            }
            catch (Exception ex) {
                MessageDisplay.Error("輸出錯誤");
                throw ex;
            }

            return ResultStatus.Success;
        }
    }
}