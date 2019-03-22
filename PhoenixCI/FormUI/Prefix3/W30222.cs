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
using DataObjects.Dao.Together.SpecificDao;
using Common;
using DevExpress.XtraEditors.Repository;
using BaseGround.Report;
using DevExpress.XtraGrid.Views.Grid;
using BusinessObjects;

/// <summary>
/// Lukas, 2019/3/18
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

    /// <summary>
    /// 30222 個股部位限制確認
    /// </summary>
    public partial class W30222 : FormParent {

        private D30222 dao30222;
        private D30203 dao30203;
        private RepositoryItemLookUpEdit statusLookUpEdit;
        private RepositoryItemLookUpEdit levelLookUpEdit;
        private Dictionary<string, string> dictAdj;
        private Dictionary<string, string> dictLevel;
        private ReportHelper _ReportHelper;

        public W30222(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            GridHelper.SetCommonGrid(gvMain);
            dao30222 = new D30222();
            dao30203 = new D30203();
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
            _ToolBtnSave.Enabled = true;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
            _ToolBtnDel.Enabled = true;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

            _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
            _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

            _ToolBtnImport.Enabled = false;//匯入
            _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
            _ToolBtnPrintAll.Enabled = true;//列印

            return ResultStatus.Success;
        }

        protected override ResultStatus Open() {
            try {
                base.Open();
                txtDate.EditValue = PbFunc.f_ocf_date(0);
                txtEffDate.DateTimeValue = DateTime.MinValue;
                txtEffDateLower.DateTimeValue = DateTime.MinValue;
#if DEBUG
                txtDate.EditValue = "2018/10/17";
#endif

                //「調整情形」欄位的下拉選單
                dictAdj = new Dictionary<string, string>() { { " ", "不變" }, { "+", "提高" }, { "-", "降低" }, { "*", "新增" } };
                DataTable dtStatus = setColItem(dictAdj);
                statusLookUpEdit = new RepositoryItemLookUpEdit();
                statusLookUpEdit.SetColumnLookUp(dtStatus, "ID", "Desc");
                PLS1_LEVEL_ADJ.ColumnEdit = statusLookUpEdit;

                //「調整後部位限制級距」欄位的下拉選單
                dictLevel = new Dictionary<string, string>() { { "1", "1" }, { "2", "2" }, { "3", "3" } };
                DataTable dtLevel = setColItem(dictLevel);
                levelLookUpEdit = new RepositoryItemLookUpEdit();
                levelLookUpEdit.SetColumnLookUp(dtLevel, "ID", "Desc");
                PLS1_CP_LEVEL.ColumnEdit = levelLookUpEdit;

                //BandedColumnCaption換行
                PLS1_SID.Caption = "標的證卷" + Environment.NewLine + "代號";
                PLS1_STKOUT.Caption = "在外" + Environment.NewLine + "流通股數";
                PLS1_CUR_LEVEL.Caption = "上季部位" + Environment.NewLine + "限制級距";
                PLS1_CP_LEVEL.Caption = "調整後部位" + Environment.NewLine + "限制級距";

                //自動調整欄寬
                gvMain.BestFitColumns();
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

        protected override ResultStatus Retrieve() {

            try {
                //1. 讀取資料
                string ymd;
                int found;
                ymd = txtDate.Text.Replace("/", "");

                DataTable dt30222 = dao30222.d_30222(ymd);
                if (dt30222.Rows.Count == 0) {
                    MessageDisplay.Info("PL1無任何資料!");
                }
                else {
                    dt30222.Columns.Add("Is_NewRow", typeof(string));
                    gcMain.DataSource = dt30222;
                    gcMain.Focus();
                }

                //2. 確認公告日期
                DataTable dtPostDate = dao30222.PostDate(ymd);
                if (dtPostDate.Rows.Count == 0) {
                    MessageDisplay.Info("公告日期無任何資料!");
                    return ResultStatus.Fail;
                }
                if (dtPostDate.Rows[0]["RAISE_YMD"].AsDateTime("yyyyMMdd") != default(DateTime)) {
                    txtEffDate.DateTimeValue = dtPostDate.Rows[0]["RAISE_YMD"].AsDateTime("yyyyMMdd");
                    txtEffDateLower.DateTimeValue = dtPostDate.Rows[0]["LOWER_YMD"].AsDateTime("yyyyMMdd");
                    lblEff.Text = "（已確認）";
                }
                else {
                    lblEff.Text = "";
                }

                //3. 選擇是否重新產製資料
                DataTable dt30222PLS2 = dao30222.d_30222_pls2(ymd);
                if (dt30222PLS2.Rows.Count == 0) {
                    MessageDisplay.Info("PL2無任何資料!");
                    return ResultStatus.Fail;
                }
                if (dtPostDate.Rows[0]["LI_COUNT"].AsInt() <= 0) return ResultStatus.Fail;
                DialogResult result = MessageDisplay.Choose("已確認資料，按「是」讀取已存檔資料，按「否」為重新產製資料");
                if (result == DialogResult.No) return ResultStatus.Fail;

                gvMain.CloseEditor();
                DataTable dtGridView = (DataTable)gcMain.DataSource;
                foreach (DataRow dr in dt30222PLS2.Rows) {
                    //此時gridview的資料還沒被動過，原本要在gridview中查找(datawindow.find)的資料直接在datasource查找即可
                    DataRow[] find = dtGridView.Select("PLS1_KIND_ID2='" + dr["PLS2_KIND_ID2"].ToString() + "'");
                    if (find.Length > 0) {
                        found = dtGridView.Rows.IndexOf(find[0]);
                    }
                    else {
                        found = -1;
                    }
                    if (found == -1) {
                        dtGridView.Rows.Add();
                        found = dtGridView.Rows.Count - 1;
                    }

                    if (dr["PLS2_EFFECTIVE_YMD"].AsString() == dtPostDate.Rows[0]["LOWER_YMD"].AsString()) {
                        dtGridView.Rows[found]["PLS1_LEVEL_ADJ"] = "-";
                    }
                    //for 	j = 2 to 16
                    dtGridView.Rows[found]["PLS1_YMD"] = dr["PLS2_YMD"].AsString();
                    if (dr["PLS2_KIND_ID2"] != DBNull.Value) dtGridView.Rows[found]["PLS1_KIND_ID2"] = dr["PLS2_KIND_ID2"].ToString();
                    dtGridView.Rows[found]["PLS1_FUT"] = dr["PLS2_FUT"];
                    dtGridView.Rows[found]["PLS1_OPT"] = dr["PLS2_OPT"];
                    if (dr["PLS2_SID"] != DBNull.Value) dtGridView.Rows[found]["PLS1_SID"] = dr["PLS2_SID"].ToString();

                    if (dr["PLS2_LEVEL_ADJ"] != DBNull.Value) dtGridView.Rows[found]["PLS1_LEVEL_ADJ"] = dr["PLS2_LEVEL_ADJ"].ToString();
                    dtGridView.Rows[found]["PLS1_CP_LEVEL"] = dr["PLS2_LEVEL"].AsString();
                    if (dr["PLS2_NATURE"] != DBNull.Value) dtGridView.Rows[found]["PLS1_CP_NATURE"] = dr["PLS2_NATURE"].AsInt();
                    if (dr["PLS2_LEGAL"] != DBNull.Value) dtGridView.Rows[found]["PLS1_CP_LEGAL"] = dr["PLS2_LEGAL"].AsInt();
                    if (dr["PLS2_999"] != DBNull.Value) dtGridView.Rows[found]["PLS1_CP_999"] = dr["PLS2_999"].AsInt();

                    dtGridView.Rows[found]["PLS1_CUR_LEVEL"] = dr["PLS2_PREV_LEVEL"].AsString();
                    if (dr["PLS2_PREV_NATURE"] != DBNull.Value) dtGridView.Rows[found]["PLS1_CUR_NATURE"] = dr["PLS2_PREV_NATURE"].AsInt();
                    if (dr["PLS2_PREV_LEGAL"] != DBNull.Value) dtGridView.Rows[found]["PLS1_CUR_LEGAL"] = dr["PLS2_PREV_LEGAL"].AsInt();
                    if (dr["PLS2_PREV_999"] != DBNull.Value) dtGridView.Rows[found]["PLS1_CUR_999"] = dr["PLS2_PREV_999"].AsInt();
                    if (dr["PLS2_KIND_GRP2"] != DBNull.Value) dtGridView.Rows[found]["KIND_GRP2"] = dr["PLS2_KIND_GRP2"].ToString();

                    dtGridView.Rows[found]["PLS1_W_TIME"] = DateTime.Now;
                    dtGridView.Rows[found]["PLS1_W_USER_ID"] = GlobalInfo.USER_ID;

                    //計算欄位COMPUTE_1: if( pls1_kind_id2 <> kind_grp2 ,'小型',' ')
                    if (dtGridView.Rows[found]["PLS1_KIND_ID2"].AsString() != dtGridView.Rows[found]["KIND_GRP2"].AsString()) {
                        dtGridView.Rows[found]["COMPUTE_1"] = "小型";
                    }
                    else {
                        dtGridView.Rows[found]["COMPUTE_1"] = " ";
                    }
                }
                gcMain.DataSource = dtGridView;
            }
            catch (Exception ex) {
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall) {

            //PB不管資料有無異動都會存檔
            string showMsg = "";

            try {
                #region ue_save_before
                gvMain.CloseEditor();

                //0. 確認是否填入正確公告日期
                if (txtEffDate.DateTimeValue==DateTime.MinValue) {
                    MessageDisplay.Error("提高－公告日期非正確日期!");
                    return ResultStatus.Fail;
                }
                if (txtEffDateLower.DateTimeValue == DateTime.MinValue) {
                    MessageDisplay.Error("降低－公告日期非正確日期!");
                    return ResultStatus.Fail;
                }

                //1. 寫LOG到ci.PLLOG
                showMsg = "異動紀錄(PLLOG)更新資料庫錯誤! ";
                DataTable dtPLLOG = dao30203.d_30203_pllog();
                DataTable dtGridView = (DataTable)gcMain.DataSource;
                foreach (DataRow dr in dtGridView.Rows) {
                    if (dr["PLS1_CP_LEVEL"].AsString() == dr["PLS1_LEVEL_ORG"].AsString()) continue;
                    DataRow drNew = dtPLLOG.NewRow();
                    drNew["PLLOG_YMD"] = dr["PLS1_YMD"];
                    drNew["PLLOG_KIND_ID"] = dr["PLS1_KIND_ID2"];
                    drNew["PLLOG_DATA_TYPE"] = " ";
                    drNew["PLLOG_ORG_VALUE"] = dr["PLS1_LEVEL_ORG"];
                    drNew["PLLOG_UPD_VALUE"] = dr["PLS1_CP_LEVEL"];
                    drNew["PLLOG_W_TIME"] = DateTime.Now;
                    drNew["PLLOG_W_USER_ID"] = GlobalInfo.USER_ID;
                    dtPLLOG.Rows.Add(drNew);
                }

                //2. 寫入DB
                ResultData myResultData = dao30203.updatePLLOG(dtPLLOG);
                #endregion

                int i, j;
                string ymd, ls_eff_ymd, ls_eff_ymd_lower;
                bool delResult = false;
                //3. 判斷是否有已確認之資料
                ymd = txtDate.Text.Replace("/", "");
                i = dao30222.checkData(ymd);
                if (i > 0) {
                    DialogResult result = MessageDisplay.Choose("已確認,是否刪除舊有資料?");
                    if (result == DialogResult.No) return ResultStatus.Fail;
                    //3.1 刪除PLS2
                    showMsg = "PLS2刪除失敗";
                    delResult = dao30222.DeletePLS2ByDate(ymd);
                    if (!delResult) {
                        MessageDisplay.Error(showMsg);
                        return ResultStatus.Fail;
                    }
                }
                //4. 新增PLS2
                showMsg = "確認資料(PLS2)更新資料庫錯誤! ";
                ls_eff_ymd = txtEffDate.Text.Replace("/", "");
                ls_eff_ymd_lower = txtEffDateLower.Text.Replace("/", "");
                DataTable dtPLS2 = dao30222.d_30222_pls2(ymd);
                dtPLS2.Clear();
                foreach (DataRow dr in dtGridView.Rows) {
                    DataRow drNew = dtPLS2.NewRow();
                    if (dr["PLS1_LEVEL_ADJ"].ToString() == "-") {
                        drNew["PLS2_EFFECTIVE_YMD"] = ls_eff_ymd_lower;
                    }
                    else {
                        drNew["PLS2_EFFECTIVE_YMD"] = ls_eff_ymd;
                    }
                    drNew["PLS2_YMD"] = dr["PLS1_YMD"];
                    drNew["PLS2_KIND_ID2"] = dr["PLS1_KIND_ID2"];
                    drNew["PLS2_FUT"] = dr["PLS1_FUT"];
                    drNew["PLS2_OPT"] = dr["PLS1_OPT"];
                    drNew["PLS2_SID"] = dr["PLS1_SID"];

                    drNew["PLS2_LEVEL_ADJ"] = dr["PLS1_LEVEL_ADJ"];
                    drNew["PLS2_LEVEL"] = dr["PLS1_CP_LEVEL"];
                    drNew["PLS2_NATURE"] = dr["PLS1_CP_NATURE"];
                    drNew["PLS2_LEGAL"] = dr["PLS1_CP_LEGAL"];
                    drNew["PLS2_999"] = dr["PLS1_CP_999"];

                    drNew["PLS2_PREV_LEVEL"] = dr["PLS1_CUR_LEVEL"];
                    drNew["PLS2_PREV_NATURE"] = dr["PLS1_CUR_NATURE"];
                    drNew["PLS2_PREV_LEGAL"] = dr["PLS1_CUR_LEGAL"];
                    drNew["PLS2_PREV_999"] = dr["PLS1_CUR_999"];
                    drNew["PLS2_KIND_GRP2"] = dr["KIND_GRP2"];

                    drNew["PLS2_W_TIME"] = DateTime.Now;
                    drNew["PLS2_W_USER_ID"] = GlobalInfo.USER_ID;
                    dtPLS2.Rows.Add(drNew);
                }
                //5. 寫入DB
                myResultData = dao30222.updatePLS2(dtPLS2);
            }
            catch (Exception ex) {
                MessageDisplay.Error(showMsg);
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow() {
            try {
                base.DeleteRow(gvMain);
            }
            catch (Exception ex) {
                MessageDisplay.Error("刪除資料列錯誤");
                throw ex;
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            try {
                _ReportHelper = reportHelper;
                CommonReportLandscapeA4 report = new CommonReportLandscapeA4();
                report.printableComponentContainerMain.PrintableComponent = gcMain;
                _ReportHelper.Create(report);

                base.Print(_ReportHelper);
            }
            catch (Exception ex) {
                throw ex;
            }
            return ResultStatus.Success;
        }

        #region GridView Events

        /// <summary>
        /// 決定哪些欄位無法編輯的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
            try {
                GridView gv = sender as GridView;
                int pls1Qnty, pls1Stkout;
                if (gv.GetRowCellValue(gv.FocusedRowHandle, "PLS1_QNTY") != DBNull.Value) {
                    pls1Qnty = gv.GetRowCellValue(gv.FocusedRowHandle, "PLS1_QNTY").AsInt();
                }
                else {
                    pls1Qnty = -1;
                }
                if (gv.GetRowCellValue(gv.FocusedRowHandle, "PLS1_STKOUT") != DBNull.Value) {
                    pls1Stkout = gv.GetRowCellValue(gv.FocusedRowHandle, "PLS1_STKOUT").AsInt();
                }
                else {
                    pls1Stkout = -1;
                }
                string opType = gv.GetRowCellValue(gv.FocusedRowHandle, "OP_TYPE").AsString();

                //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
                switch (gv.FocusedColumn.Name) {
                    case "PLS1_CP_NATURE":
                    case "PLS1_CP_LEGAL":
                    case "PLS1_CP_999":
                    case "PLS1_LEVEL_ADJ":
                    case "PLS1_CUR_LEVEL":
                        e.Cancel = false;
                        break;
                    case "PLS1_CP_LEVEL":
                        if (opType == "P" ||
                            pls1Qnty == 0 && pls1Stkout == 0) {
                            e.Cancel = true;
                        }
                        else {
                            e.Cancel = false;
                        }
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
            catch (Exception ex) {
                WriteLog(ex, "", false);
            }
        }

        /// <summary>
        /// 動態改變資料列底色的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            try {
                //要用RowHandle不要用FocusedRowHandle
                GridView gv = sender as GridView;

                string pls1LevelOrg = gv.GetRowCellValue(e.RowHandle, gv.Columns["PLS1_LEVEL_ORG"]).AsString();
                //if( PLS1_KIND_ID2 <> KIND_GRP2) 則整列變色
                string pls1KindId = gv.GetRowCellValue(e.RowHandle, "PLS1_KIND_ID2").AsString();
                string grp2 = gv.GetRowCellValue(e.RowHandle, "KIND_GRP2").AsString();
                int pls1Qnty, pls1Stkout;
                if (gv.GetRowCellValue(e.RowHandle, "PLS1_QNTY") != DBNull.Value) {
                    pls1Qnty = gv.GetRowCellValue(e.RowHandle, "PLS1_QNTY").AsInt();
                }
                else {
                    pls1Qnty = -1;
                }
                if (gv.GetRowCellValue(e.RowHandle, "PLS1_STKOUT") != DBNull.Value) {
                    pls1Stkout = gv.GetRowCellValue(e.RowHandle, "PLS1_STKOUT").AsInt();
                }
                else {
                    pls1Stkout = -1;
                }
                string opType = gv.GetRowCellValue(e.RowHandle, "OP_TYPE").AsString();
                //描述每個欄位,在is_newRow時候要顯示的顏色
                //當該欄位不可編輯時,設定為Mint Color.FromArgb(192,220,192)
                switch (e.Column.FieldName) {
                    case ("PLS1_CP_LEVEL"):
                        if (pls1KindId != grp2) {
                            e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                        }
                        if (pls1LevelOrg!=null && 
                            pls1LevelOrg != gv.GetRowCellValue(e.RowHandle, gv.Columns["PLS1_CP_LEVEL"]).AsString()) {
                            e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                        }
                        else if (opType == "P" ||
                                 pls1Qnty == 0 && pls1Stkout == 0) {
                            e.Appearance.BackColor = Color.FromArgb(192, 220, 192);
                        }
                        else {
                            e.Appearance.BackColor = Color.White;
                        }
                        break;
                    case ("PLS1_CP_NATURE"):
                    case ("PLS1_CP_LEGAL"):
                    case ("PLS1_CP_999"):
                    case ("PLS1_LEVEL_ADJ"):
                        if (pls1LevelOrg != null && 
                            pls1LevelOrg != gv.GetRowCellValue(e.RowHandle, gv.Columns["PLS1_CP_LEVEL"]).AsString()) {
                            e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                        }
                        else {
                            e.Appearance.BackColor = Color.FromArgb(192, 220, 192);
                        }
                        if (pls1KindId != grp2) {
                            e.Appearance.BackColor = Color.FromArgb(255, 168, 255);
                        }
                        break;
                    default:
                        e.Appearance.BackColor = pls1KindId != grp2 ? Color.FromArgb(255, 168, 255) : Color.FromArgb(192, 220, 192);
                        break;
                }//switch (e.Column.FieldName) {
            }
            catch (Exception ex) {
                WriteLog(ex, "", false);
            }
        }

        /// <summary>
        /// 調整後部位限制級距(PLS1_CP_LEVEL)、上季部位限制級距(PLS1_CUR_LEVEL)
        /// 欄位異動時觸發的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) {
            try {
                GridView gv = sender as GridView;
                int data;
                switch (e.Column.FieldName) {
                    case "PLS1_CP_LEVEL":
                        data = gv.GetRowCellValue(e.RowHandle, "PLS1_CP_LEVEL").AsInt();
                        DataTable dtPLST1 = dao30222.SetPLST1LevelData(data);
                        if (dtPLST1.Rows.Count == 0) {
                            MessageDisplay.Error("PLST1無任何資料!");
                            return;
                        }
                        gv.SetRowCellValue(e.RowHandle, "PLS1_CP_NATURE", dtPLST1.Rows[0]["PLST1_NATURE"]);
                        gv.SetRowCellValue(e.RowHandle, "PLS1_CP_LEGAL", dtPLST1.Rows[0]["PLST1_LEGAL"]);
                        gv.SetRowCellValue(e.RowHandle, "PLS1_CP_999", dtPLST1.Rows[0]["PLST1_999"]);
                        if (gv.GetRowCellValue(e.RowHandle, "PLS1_CUR_LEVEL").AsInt() > data) {
                            gv.SetRowCellValue(e.RowHandle, "PLS1_LEVEL_ADJ", "+");
                        }
                        else if (gv.GetRowCellValue(e.RowHandle, "PLS1_CUR_LEVEL").AsInt() < data) {
                            gv.SetRowCellValue(e.RowHandle, "PLS1_LEVEL_ADJ", "-");
                        }
                        else {
                            gv.SetRowCellValue(e.RowHandle, "PLS1_LEVEL_ADJ", " ");
                        }
                        break;
                    case "PLS1_CUR_LEVEL":
                        data = gv.GetRowCellValue(e.RowHandle, "PLS1_CUR_LEVEL").AsInt();
                        if (data.AsString() == "") {
                            gv.SetRowCellValue(e.RowHandle, "PLS1_LEVEL_ADJ", "*");
                        }
                        else if (data > gv.GetRowCellValue(e.RowHandle, "PLS1_CP_LEVEL").AsInt()) {
                            gv.SetRowCellValue(e.RowHandle, "PLS1_LEVEL_ADJ", "+");
                        }
                        else if (data < gv.GetRowCellValue(e.RowHandle, "PLS1_CP_LEVEL").AsInt()) {
                            gv.SetRowCellValue(e.RowHandle, "PLS1_LEVEL_ADJ", "-");
                        }
                        else {
                            gv.SetRowCellValue(e.RowHandle, "PLS1_LEVEL_ADJ", " ");
                        }
                        break;
                }
            }
            catch (Exception ex) {
                WriteLog(ex, "", false);
            }
        }

        #endregion

        /// <summary>
        /// 點擊加入前次公告資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click(object sender, EventArgs e) {
            try {
                if (gvMain.RowCount == 0) {
                    MessageDisplay.Error("請先執行「讀取／預覽」!");
                    return;
                }
                string ymd = txtDate.Text.Replace("/", "");
                gvMain.CloseEditor();
                DataTable dtGridView = (DataTable)gcMain.DataSource;
                DataView dv;
                DataTable dtSorted;
                DataTable dtPrev = dao30222.d_30222_prev(ymd);
                if (dtPrev.Rows.Count == 0) {
                    dv = dtGridView.AsDataView();
                    dv.Sort = "PLS1_KIND_ID2";
                    dtSorted = dv.ToTable();
                    gcMain.DataSource = dtSorted;
                    return;
                }
                for (int f = 0; f < dtPrev.Rows.Count; f++) {
                    DataRow dr = dtPrev.Rows[f];
                    DataRow drNew = dtGridView.NewRow();
                    drNew["PLS1_YMD"] = ymd;

                    drNew["PLS1_KIND_ID2"] = dr["PLS2_KIND_ID2"];
                    drNew["PLS1_FUT"] = dr["PLS2_FUT"];
                    drNew["PLS1_OPT"] = dr["PLS2_OPT"];
                    drNew["PLS1_SID"] = dr["PLS2_SID"];
                    drNew["PLS1_LEVEL_ADJ"] = dr["PLS2_LEVEL_ADJ"];

                    drNew["PLS1_CP_LEVEL"] = dr["PLS2_LEVEL"];
                    drNew["PLS1_CP_NATURE"] = dr["PLS2_NATURE"];
                    drNew["PLS1_CP_LEGAL"] = dr["PLS2_LEGAL"];
                    drNew["PLS1_CP_999"] = dr["PLS2_999"];
                    drNew["PLS1_CUR_LEVEL"] = dr["PLS2_PREV_LEVEL"];

                    drNew["PLS1_CUR_NATURE"] = dr["PLS2_PREV_NATURE"];
                    drNew["PLS1_CUR_LEGAL"] = dr["PLS2_PREV_LEGAL"];
                    drNew["PLS1_CUR_999"] = dr["PLS2_PREV_999"];
                    drNew["KIND_GRP2"] = dr["PLS2_KIND_GRP2"];
                    drNew["PLS1_W_TIME"] = dr["PLS2_W_TIME"];

                    drNew["PLS1_W_USER_ID"] = dr["PLS2_W_USER_ID"];
                    if (dr["PLS1_QNTY"] != DBNull.Value) drNew["PLS1_QNTY"] = dr["PLS1_QNTY"];
                    if (dr["PLS1_STKOUT"] != DBNull.Value) drNew["PLS1_STKOUT"] = dr["PLS1_STKOUT"];
                    drNew["PLS1_LEVEL_ORG"] = dr["PLS1_LEVEL_ORG"];
                    drNew["PLS1_LEVEL_ADJ_ORG"] = dr["PLS1_LEVEL_ADJ_ORG"];

                    //計算欄位COMPUTE_1: if( pls1_kind_id2 <> kind_grp2 ,'小型',' ')
                    if (dr["PLS2_KIND_ID2"].AsString() != dr["PLS2_KIND_GRP2"].AsString()) {
                        drNew["COMPUTE_1"] = "小型";
                    }
                    else {
                        drNew["COMPUTE_1"] = " ";
                    }

                    dtGridView.Rows.Add(drNew);
                }
                dv = dtGridView.AsDataView();
                dv.Sort = "PLS1_KIND_ID2";
                dtSorted = dv.ToTable();
                gcMain.DataSource = dtSorted;
            }
            catch (Exception ex) {
                MessageDisplay.Error("加入前次公告資料錯誤");
                WriteLog(ex, "", false);
            }
        }
    }
}