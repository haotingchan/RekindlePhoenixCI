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
using Common;
using BusinessObjects.Enums;
using BaseGround.Report;
using BusinessObjects;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using BaseGround.Shared;
using DataObjects.Dao.Together.SpecificDao;
/// <summary>
/// Lukas, 2019/1/4
/// </summary>
namespace PhoenixCI.FormUI.Prefix5 {
    public partial class W51040 : BaseGround.FormParent {

        private D51040 dao51040;
        private ReportHelper _ReportHelper;
        private MMWK daoMMWK;
        private RepositoryItemLookUpEdit _RepLookUpEdit;

        public W51040(string programID, string programName) : base(programID, programName) {
            InitializeComponent();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtMonth.DateTimeValue = GlobalInfo.OCF_DATE;
            dao51040 = new D51040();
        }

        public override ResultStatus BeforeOpen() {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open() {
            base.Open();

            #region 處理下拉選單
            _RepLookUpEdit = new RepositoryItemLookUpEdit();
            DataTable dtProd_Type = new DataTable();
            dtProd_Type.Columns.Add("ID");
            dtProd_Type.Columns.Add("DESC");
            DataRow ptRow1 = dtProd_Type.NewRow();
            ptRow1["ID"] = "F";
            ptRow1["DESC"] = "期貨";
            dtProd_Type.Rows.Add(ptRow1);
            DataRow ptRow2 = dtProd_Type.NewRow();
            ptRow2["ID"] = "O";
            ptRow2["DESC"] = "選擇權";
            dtProd_Type.Rows.Add(ptRow2);
            _RepLookUpEdit.DataSource = dtProd_Type;
            _RepLookUpEdit.ValueMember = "ID";
            _RepLookUpEdit.DisplayMember = "DESC";
            _RepLookUpEdit.ShowHeader = false;
            _RepLookUpEdit.ShowFooter = false;
            _RepLookUpEdit.NullText = "";
            LookUpColumnInfoCollection singleCol2 = _RepLookUpEdit.Columns;
            singleCol2.Add(new LookUpColumnInfo("DESC"));
            gcMain.RepositoryItems.Add(_RepLookUpEdit);
            MMWK_PROD_TYPE.ColumnEdit = _RepLookUpEdit;
            //調整下拉選單長度
            _RepLookUpEdit.DropDownRows = dtProd_Type.Rows.Count;
            _RepLookUpEdit.BestFit();
            #endregion

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen() {
            base.AfterOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm() {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = true;
            _ToolBtnSave.Enabled = true;
            _ToolBtnDel.Enabled = true;
            _ToolBtnRetrieve.Enabled = true;
            _ToolBtnImport.Enabled = true;
            _ToolBtnPrintAll.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve() {
            //測試資料: 2016/10
            base.Retrieve(gcMain);
            daoMMWK = new MMWK();
            string as_ym = txtMonth.Text.Replace("/", "");
            DataTable returnTable = daoMMWK.ListAllByDate(as_ym);
            if (returnTable.Rows.Count == 0) {
                MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            returnTable.Columns.Add("Is_NewRow", typeof(string));
            gcMain.DataSource = returnTable;
            gcMain.Visible = true;
            gcMain.Focus();


            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield() {
            base.CheckShield(gcMain);
            if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall) {
            base.Save(gcMain);

            DataTable dt = (DataTable)gcMain.DataSource;

            DataTable dtChange = dt.GetChanges();
            if (dtChange == null) {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ResultStatus.Fail;
            }

            if (dtChange.Rows.Count == 0) {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return ResultStatus.Fail;
            }
            else {
                ResultStatus status = base.Save_Override(dt, "MMWK");
                if (status == ResultStatus.Fail) {
                    return ResultStatus.Fail;
                }
            }
            return ResultStatus.Success; 
        }

        protected override ResultStatus Run(PokeBall args) {
            base.Run(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Import() {
            base.Import(gcMain);

            //1.讀檔並寫入DataTable
            try {
                DataTable dtReadTxt = new DataTable();
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "*.txt (*.txt)|*.txt";
                open.Title = "請點選儲存檔案之目錄";
                open.FileName = "51040.txt";
                DialogResult openResult = open.ShowDialog();
                if (openResult == DialogResult.OK) {
                    using (TextReader tr = File.OpenText(open.FileName)) {
                        string line;
                        while ((line = tr.ReadLine()) != null) {
                            string[] items = line.Split('\t');
                            if (dtReadTxt.Columns.Count == 0) {
                                // Create the data columns for the data table based on the number of items
                                // on the first line of the file
                                for (int i = 0; i < items.Length; i++)
                                    dtReadTxt.Columns.Add(new DataColumn("Column" + i, typeof(string)));
                            }
                            dtReadTxt.Rows.Add(items);
                        }
                    }
                }
                else {
                    return ResultStatus.Fail;
                }

                //2.確認資料日期&畫面日期(讀取資料)
                string datadate = dtReadTxt.Rows[0][1].AsString();
                if (datadate != txtMonth.Text.Replace("/", "")) {
                    DialogResult result = MessageBox.Show("資料年月(" + datadate + ")與畫面年月不同,是否將畫面改為資料年月?", "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No) {
                        return ResultStatus.Fail;
                    }
                    else {
                        txtMonth.Text = datadate.SubStr(0, 4) + "/" + datadate.SubStr(4, 2);
                        Retrieve();
                    }
                }

                //3.刪除舊有資料
                if (gvMain.DataRowCount > 0) {
                    DialogResult result = MessageBox.Show("資料年月(" + datadate + ")資料已存在,是否刪除?", "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No) {
                        return ResultStatus.Fail;
                    }
                    else {
                        dao51040.DeleteByDate(datadate);
                    }
                }

                //4.轉入資料(補上mmwk_w_user_id和mmwk_w_time欄位的資料)
                //  即PB的wf_importfile()→wf_importfile_extra()
                dtReadTxt.Columns[0].ColumnName = "MMWK_PROD_TYPE";
                dtReadTxt.Columns[1].ColumnName = "MMWK_YM";
                dtReadTxt.Columns[2].ColumnName = "MMWK_KIND_ID";
                dtReadTxt.Columns[3].ColumnName = "MMWK_WEIGHT";
                dtReadTxt.Columns.Add("MMWK_W_USER_ID", typeof(string));
                dtReadTxt.Columns.Add("MMWK_W_TIME", typeof(DateTime));
                dtReadTxt.Columns.Add("Is_NewRow", typeof(string));

                for (int cnt = 0; cnt < dtReadTxt.Rows.Count; cnt++) {
                    DataRow drReadTxt = dtReadTxt.Rows[cnt];
                    drReadTxt["MMWK_W_USER_ID"] = GlobalInfo.USER_ID;
                    drReadTxt["MMWK_W_TIME"] = DateTime.Now.ToString("yyyy/M/d tt h:mm:ss");
                }
                PokeBall pb = new PokeBall();
                gcMain.DataSource = dtReadTxt;
                gvMain.Columns["MMWK_W_USER_ID"].Visible = false;
                gvMain.Columns["MMWK_W_TIME"].Visible = false;
                gvMain.Columns["Is_NewRow"].Visible = false;
                Save(pb);
                //Retrieve();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            return ResultStatus.Success;
        }

        protected override ResultStatus Export(ReportHelper reportHelper) {
            reportHelper = _ReportHelper;
            base.Export(reportHelper);

            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper) {
            _ReportHelper = reportHelper;
            CommonReportPortraitA4 report = new CommonReportPortraitA4();

            report.printableComponentContainerMain.PrintableComponent = gcMain;

            _ReportHelper.Create(report);
            //_ReportHelper.LeftMemo = "設定權限給(" + cbxUserId.Text.Trim() + ")";

            base.Print(_ReportHelper);
            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow() {
            int focusIndex = gvMain.GetFocusedDataSourceRowIndex();

            gvMain.CloseEditor();//必須先做close edit, like dt.AcceptChanges();

            //新增一行並做初始值設定
            DataTable dt = (DataTable)gcMain.DataSource;
            DataRow drNew = dt.NewRow();

            drNew["Is_NewRow"] = 1;
            drNew["MMWK_W_USER_ID"] = GlobalInfo.USER_ID;
            drNew["MMWK_W_TIME"] = DateTime.Now.ToString("yyyy/M/d tt h:mm:ss");

            dt.Rows.InsertAt(drNew, focusIndex);
            gcMain.DataSource = dt;//重新設定給grid,雖然會更新但是速度太快,畫面不會閃爍
            gvMain.FocusedRowHandle = focusIndex;//原本的focusRowHandle會記住之前的位置,其實只是往上一行
            gvMain.FocusedColumn = gvMain.Columns[0];

            return ResultStatus.Success;
        }

        #region GridControl事件

        private void gvMain_ShowingEditor(object sender, CancelEventArgs e) {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                 gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();

            if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
                e.Cancel = false;
                gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
                object a = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]);
            }
            //已寫入資料庫的只有權重欄位可編輯
            else if (gv.FocusedColumn.FieldName == "MMWK_WEIGHT") {
                e.Cancel = false;
            }
            else {
                e.Cancel = true;
            }
        }

        //ken,把原本grid init new row需要的初始值設定,搬移到InsertRow
        private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e) {
            //GridView gv = sender as GridView;
            //gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
            //gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["MMWK_W_USER_ID"], GlobalInfo.USER_ID);
            //gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["MMWK_W_TIME"], DateTime.Now.ToString("yyyy/M/d tt h:mm:ss"));
        }

        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e) {
            //要用RowHandle不要用FocusedRowHandle
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                               gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();
            e.Column.OptionsColumn.AllowFocus = true;

            if (e.Column.FieldName != "MMWK_WEIGHT") {
                e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.Silver;
            }
        }

        private void gvMain_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) {

            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
            gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();

            if (e.FocusedColumn != null) {
                if (Is_NewRow == "1") {
                    e.FocusedColumn.OptionsColumn.AllowFocus = true;
                }
                else if (e.FocusedColumn.FieldName != "MMWK_WEIGHT") {
                    e.FocusedColumn.OptionsColumn.AllowFocus = false;
                }
            }
        }
        #endregion

        protected override ResultStatus DeleteRow() {
            base.DeleteRow(gvMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose() {
            return base.BeforeClose();
        }

        protected override ResultStatus COMPLETE() {
            MessageDisplay.Info(MessageDisplay.MSG_OK);
            Retrieve();
            return ResultStatus.Success;
        }
    }
}