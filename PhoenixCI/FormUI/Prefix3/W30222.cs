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
                txtDate.EditValue = "2018/12/28";
#endif

                //「調整情形」欄位的下拉選單
                dictAdj = new Dictionary<string, string>() { { " ", "不變" }, { "+", "調高" }, { "-", "降低" } };
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
                dao30222 = new D30222();
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
                DataTable dt30222PLS2 = dao30222.d_30203_pls2(ymd);
                if (dt30222PLS2.Rows.Count == 0) {
                    MessageDisplay.Info("PL2無任何資料!");
                    return ResultStatus.Fail;
                }
                if (dtPostDate.Rows[0]["LI_COUNT"].AsInt() <= 0) return ResultStatus.Fail;
                DialogResult result = MessageDisplay.Choose("已確認資料，按「是」讀取已存檔資料，按「否」為重新產製資料");
                if (result == DialogResult.No) return ResultStatus.Fail;

                foreach (DataRow dr in dt30222PLS2.Rows) {
                    //此時gridview的資料還沒被動過，原本要在gridview中查找(datawindow.find)的資料直接在datasource查找即可
                    DataRow[] find = dt30222.Select("PLS1_KIND_ID2='" + dr["PLS2_KIND_ID2"].ToString() + "'");
                    if (find.Length > 0) {
                        found = dt30222.Rows.IndexOf(find[0]);
                    }
                    else {
                        found = -1;
                    }
                    if (found == -1) {
                        InsertRow();
                        found = gvMain.RowCount;
                    }

                    if (dr["PLS2_EFFECTIVE_YMD"].AsString() == dtPostDate.Rows[0]["LOWER_YMD"].AsString()) {
                        gvMain.SetRowCellValue(found, "PLS1_LEVEL_ADJ", "-");
                    }
                    //for 	j = 2 to 16
                    gvMain.SetRowCellValue(found, "PLS1_YMD", dr["PLS2_YMD"].AsString());
                    if (dr["PLS2_KIND_ID2"] != DBNull.Value) gvMain.SetRowCellValue(found, "PLS1_KIND_ID2", dr["PLS2_KIND_ID2"].ToString());
                    gvMain.SetRowCellValue(found, "PLS1_FUT", dr["PLS2_FUT"].AsString());
                    gvMain.SetRowCellValue(found, "PLS1_OPT", dr["PLS2_OPT"].AsString());
                    if (dr["PLS2_SID"] != DBNull.Value) gvMain.SetRowCellValue(found, "PLS1_SID", dr["PLS2_SID"].ToString());

                    if (dr["PLS2_LEVEL_ADJ"] != DBNull.Value) gvMain.SetRowCellValue(found, "PLS1_LEVEL_ADJ", dr["PLS2_LEVEL_ADJ"].ToString());
                    gvMain.SetRowCellValue(found, "PLS1_CP_LEVEL", dr["PLS2_LEVEL"].AsString());
                    if (dr["PLS2_NATURE"] != DBNull.Value) gvMain.SetRowCellValue(found, "PLS1_CP_NATURE", dr["PLS2_NATURE"].AsInt());
                    if (dr["PLS2_LEGAL"] != DBNull.Value) gvMain.SetRowCellValue(found, "PLS1_CP_LEGAL", dr["PLS2_LEGAL"].AsInt());
                    if (dr["PLS2_999"] != DBNull.Value) gvMain.SetRowCellValue(found, "PLS1_CP_999", dr["PLS2_999"].AsInt());

                    gvMain.SetRowCellValue(found, "PLS1_CUR_LEVEL", dr["PLS2_PREV_LEVEL"].AsString());
                    if (dr["PLS2_PREV_NATURE"] != DBNull.Value) gvMain.SetRowCellValue(found, "PLS1_CUR_NATURE", dr["PLS2_PREV_NATURE"].AsInt());
                    if (dr["PLS2_PREV_LEGAL"] != DBNull.Value) gvMain.SetRowCellValue(found, "PLS1_CUR_LEGAL", dr["PLS2_PREV_LEGAL"].AsInt());
                    if (dr["PLS2_PREV_999"] != DBNull.Value) gvMain.SetRowCellValue(found, "PLS1_CUR_999", dr["PLS2_PREV_999"].AsInt());
                    if (dr["PLS2_KIND_GRP2"] != DBNull.Value) gvMain.SetRowCellValue(found, "KIND_GRP2", dr["PLS2_KIND_GRP2"].ToString());

                    gvMain.SetRowCellValue(found, "PLS1_W_TIME", DateTime.Now);
                    gvMain.SetRowCellValue(found, "PLS1_W_USER_ID", GlobalInfo.USER_ID);
                }
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
                //0. 先結束編輯
                gvMain.CloseEditor();

                //1. 寫LOG到ci.PLLOG
                dao30203 = new D30203();
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

                // 寫入DB
                ResultData myResultData = dao30203.updatePLLOG(dtPLLOG);
                #endregion


            }
            catch (Exception ex) {
                MessageDisplay.Error(showMsg);
                throw ex;
            }
            return ResultStatus.Success;
        }

        #region GridView Events

        /// <summary>
        /// 新增資料列時，本次擬調整狀態預設為"調高"。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e) {
            try {
                GridView gv = sender as GridView;
                gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);

                //直接設定值給dataTable(have UI)
                gv.SetRowCellValue(e.RowHandle, gv.Columns["PL1_NATURE_ADJ"], "+");
                gv.SetRowCellValue(e.RowHandle, gv.Columns["PL1_LEGAL_ADJ"], "+");

                //直接設定值給dataTable(no UI)
            }
            catch (Exception ex) {
                WriteLog(ex, "", false);
            }
        }

        /// <summary>
        /// 決定哪些欄位無法編輯的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
            try {
                GridView gv = sender as GridView;
                string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                     gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();
                int pls1Qnty = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["PLS1_QNTY"]).AsInt();
                int pls1Stkout = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["PLS1_STKOUT"]).AsInt();

                if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
                    e.Cancel = false;
                    //if (gv.FocusedColumn.Name == "PL1_CUR_NATURE" ||
                    //    gv.FocusedColumn.Name == "PL1_CUR_LEGAL" ||
                    //    gv.FocusedColumn.Name == "COMPUTE_1") {
                    //    e.Cancel = true;
                    //}
                }
                //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
                else if (gv.FocusedColumn.Name == "PLS1_CP_NATURE" ||
                        gv.FocusedColumn.Name == "PLS1_CP_LEGAL" ||
                        gv.FocusedColumn.Name == "PLS1_CP_999" ||
                        gv.FocusedColumn.Name == "PLS1_LEVEL_ADJ") {
                    e.Cancel = false;
                }
                // if(  op_type ='P'  or ( pls1_qnty =0 and  pls1_stkout =0),0,1)
                else if (gv.FocusedColumn.Name == "PLS1_CP_LEVEL") {
                    if (pls1Qnty == 0 && pls1Stkout == 0) {
                        e.Cancel = true;
                    }
                    e.Cancel = false;
                }
                else {
                    e.Cancel = true;
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
                string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                                   gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();

                string pls1LevelOrg = gv.GetRowCellValue(e.RowHandle, gv.Columns["PLS1_LEVEL_ORG"]).AsString();


                //描述每個欄位,在is_newRow時候要顯示的顏色
                //當該欄位不可編輯時,設定為Mint Color.FromArgb(192,220,192)
                switch (e.Column.FieldName) {
                    case ("PLS1_CP_LEVEL"):
                    case ("PLS1_CP_NATURE"):
                    case ("PLS1_CP_LEGAL"):
                    case ("PLS1_CP_999"):
                    case ("PLS1_LEVEL_ADJ"):
                        if (pls1LevelOrg != gv.GetRowCellValue(e.RowHandle, gv.Columns["PLS1_CP_LEVEL"]).AsString()) {
                            e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(255, 168, 255);
                        }
                        else {
                            e.Appearance.BackColor = e.Column.FieldName == "PLS1_CP_LEVEL" ? Color.White : Color.FromArgb(192, 220, 192);
                        }
                        break;
                    default:
                        e.Appearance.BackColor = Color.FromArgb(192, 220, 192);
                        break;
                }//switch (e.Column.FieldName) {
            }
            catch (Exception ex) {
                WriteLog(ex, "", false);
            }
        }
        #endregion
    }
}