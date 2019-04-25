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
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using BaseGround.Shared;
using Common;
using DevExpress.XtraEditors.Repository;
using BaseGround.Report;

/// <summary>
/// Lukas, 2019/3/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
    /// <summary>
    /// 41010 個股契約查詢
    /// </summary>
    public partial class W41010 : FormParent {

        private D41010 dao41010;
        private RepositoryItemLookUpEdit statusLookUpEdit;
        private Dictionary<string, string> dictStatus;

        public W41010(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
        }

        protected override ResultStatus Open() {
            try {
                base.Open();
                txtEDate.DateTimeValue = GlobalInfo.OCF_DATE;
                txtSDate.EditValue = txtEDate.Text;
#if DEBUG
                //txtSDate.Text = "2014/01/01";
                //txtEDate.Text = "2017/12/31";
                //txtProd.Text = "";
#endif
                txtSDate.Focus();

                //狀態欄位下拉選單 dddw_pdk_status_code
                dictStatus = new Dictionary<string, string>() { { "1", "暫停交易後契約調整" },
                                                                { "E", "下市" },
                                                                { "P", "暫停交易" },
                                                                { "N", "正常交易" },
                                                                { "U", "未上市" },
                                                                { " ", "合併下市" } };
                DataTable dtStatus = setColItem(dictStatus);
                statusLookUpEdit = new RepositoryItemLookUpEdit();
                statusLookUpEdit.SetColumnLookUp(dtStatus, "ID", "Desc");
                PDK_STATUS_CODE.ColumnEdit = statusLookUpEdit;
                PDK_STATUS_CODE_E.ColumnEdit = statusLookUpEdit;


            }
            catch (Exception ex) {
                throw ex;
            }
            return ResultStatus.Success;
        }

        /// <summary>
        /// 自訂下拉式選項
        /// </summary>
        /// <param name="dic">陣列</param>
        /// <returns></returns>
        private DataTable setColItem(Dictionary<string, string> dic) {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Desc");
            foreach (var str in dic) {
                DataRow rows = dt.NewRow();
                rows["ID"] = str.Key;
                rows["Desc"] = str.Value;
                dt.Rows.Add(rows);
            }
            return dt;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
            _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
            _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

            _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
            _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

            _ToolBtnImport.Enabled = false;//匯入
            _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
            _ToolBtnPrintAll.Enabled = true;//列印

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve() {

            try {
                dao41010 = new D41010();
                string prodType, kindId;
                prodType = rdgProdType.EditValue.AsString();
                kindId = txtProd.Text.Trim();
                if (kindId.IndexOf("%") == -1) {
                    kindId = kindId + "%";
                }
                if (kindId == "") kindId = "%";

                //讀取資料
                DataTable dt41010 = dao41010.d_41010(prodType, txtSDate.DateTimeValue, txtEDate.DateTimeValue, kindId);
                gcMain.DataSource = dt41010;
                DataTable dt41010e = dao41010.d_41010e(prodType, txtSDate.DateTimeValue, txtEDate.DateTimeValue, kindId);
                gcMainE.DataSource = dt41010e;
                //自動調整欄寬
                gvMain.BestFitColumns();
                gvMainE.BestFitColumns();
            }
            catch (Exception ex) {
                MessageDisplay.Error("讀取錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            try {
                ReportHelper _ReportHelper = new ReportHelper(gcMain, _ProgramID, this.Text);
                CommonReportPortraitA4 reportPortraitA4 = new CommonReportPortraitA4();
                reportPortraitA4.printableComponentContainerMain.PrintableComponent = gcMain;
                reportPortraitA4.IsHandlePersonVisible = false;
                reportPortraitA4.IsManagerVisible = false;
                _ReportHelper.Create(reportPortraitA4);

                _ReportHelper.Print();

                _ReportHelper = new ReportHelper(gcMainE, _ProgramID, this.Text);
                reportPortraitA4.printableComponentContainerMain.PrintableComponent = gcMainE;
                reportPortraitA4.IsHandlePersonVisible = false;
                reportPortraitA4.IsManagerVisible = false;
                _ReportHelper.Create(reportPortraitA4);

                _ReportHelper.Print();

                return ResultStatus.Success;
            }
            catch (Exception ex) {
                WriteLog(ex);
            }
            return ResultStatus.Fail;
        }
    }
}