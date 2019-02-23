using System;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;
using BaseGround;
using BaseGround.Report;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using Common;
using DevExpress.XtraGrid.Views.Grid;
using BusinessObjects;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using System.Drawing;

namespace PhoenixCI.FormUI.Prefix5
{
    public partial class W51010 : FormParent
    {
        private string disableCol = "DTS_DATE";

        private ReportHelper _ReportHelper;
        private D51010 dao51010;
        private COD daoCOD;
        private RepositoryItemLookUpEdit _RepLookUpEdit;
        private RepositoryItemLookUpEdit _RepLookUpEdit2;

        public W51010(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            dao51010 = new D51010();
            GridHelper.SetCommonGrid(gvMain);
            PrintableComponent = gcMain;
            TXTStartDate.DateTimeValue = Convert.ToDateTime(GlobalInfo.OCF_DATE.Year + "/01/01");
            TXTEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
            this.Text = _ProgramID + "─" + _ProgramName;

            //日期類別 是否交易 兩個欄位要換成LookUpEdit
            _RepLookUpEdit = new RepositoryItemLookUpEdit();
            daoCOD = new COD();
            _RepLookUpEdit.DataSource = daoCOD.ListByCol2("51010", "DTS_DATE_TYPE");
            _RepLookUpEdit.ValueMember = "COD_ID";
            _RepLookUpEdit.DisplayMember = "COD_DESC";
            _RepLookUpEdit.ShowHeader = false;
            _RepLookUpEdit.ShowFooter = false;
            _RepLookUpEdit.NullText = "";
            _RepLookUpEdit.TextEditStyle = TextEditStyles.DisableTextEditor;
            //讓下拉選單只剩單一欄位
            LookUpColumnInfoCollection singleCol = _RepLookUpEdit.Columns;
            singleCol.Add(new LookUpColumnInfo("CP_DISPLAY"));
            gcMain.RepositoryItems.Add(_RepLookUpEdit);
            DTS_DATE_TYPE.ColumnEdit = _RepLookUpEdit;
            _RepLookUpEdit.BestFitMode = BestFitMode.BestFitResizePopup;

            _RepLookUpEdit2 = new RepositoryItemLookUpEdit();
            _RepLookUpEdit2.DataSource = daoCOD.ListByCol2("51010", "DTS_WORK");
            _RepLookUpEdit2.ValueMember = "COD_ID";
            _RepLookUpEdit2.DisplayMember = "COD_DESC";
            _RepLookUpEdit2.ShowHeader = false;
            _RepLookUpEdit2.ShowFooter = false;
            _RepLookUpEdit2.NullText = "";
            //_RepLookUpEdit2.SearchMode = SearchMode.AutoFilter;
            _RepLookUpEdit2.TextEditStyle = TextEditStyles.DisableTextEditor;
            //讓下拉選單只剩單一欄位
            LookUpColumnInfoCollection singleCol2 = _RepLookUpEdit2.Columns;
            singleCol2.Add(new LookUpColumnInfo("CP_DISPLAY"));
            gcMain.RepositoryItems.Add(_RepLookUpEdit2);
            DTS_WORK.ColumnEdit = _RepLookUpEdit2;
            _RepLookUpEdit2.BestFitMode = BestFitMode.BestFitResizePopup;
        }

        protected override ResultStatus Retrieve()
        {
            base.Retrieve(gcMain);
            DataTable returnTable = new DataTable();
            returnTable = dao51010.GetData(TXTStartDate.Text.Replace("/", "-"), TXTEndDate.Text.Replace("/", "-"));
            if (returnTable.Rows.Count == 0)
            {
                MessageBox.Show("無任何資料", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            returnTable.Columns.Add("Is_NewRow", typeof(string));
            gcMain.DataSource = returnTable;

            //gcMain.Focus();

            return ResultStatus.Success;
        }

        protected override ResultStatus InsertRow()
        {
            base.InsertRow(gvMain);
            //gvMain.Focus();
            gvMain.FocusedColumn = gvMain.Columns[0];

            return ResultStatus.Success;
        }

        private void gvMain_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                 gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();

            if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1")
            {            
                e.Cancel = false;
                gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
            }
            else if (gv.FocusedColumn.FieldName == disableCol)
            {
                e.Cancel = true;
            }
        }

        private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
                 gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();

            if (e.Column.FieldName == disableCol)
            {
                e.Appearance.BackColor = Is_NewRow=="1" ? Color.White : Color.Silver;
            }
        }

        private void gvMain_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            GridView gv = sender as GridView;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);        
        }

        protected override ResultStatus Save(PokeBall poke)
        {
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            DataTable dt = (DataTable)gcMain.DataSource;

            DataTable dtChange = dt.GetChanges();

            if (dtChange.Rows.Count == 0)
            {
                MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                ResultStatus status = base.Save_Override(dt, "DTS");
                if (status == ResultStatus.Fail)
                {
                    return ResultStatus.Fail;
                }
            }
            _IsPreventFlowPrint = true;
            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield()
        {
            base.CheckShield(gcMain);
            if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }

            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper)
        {
            _ReportHelper = reportHelper;
            CommonReportPortraitA4 report = new CommonReportPortraitA4();
            report.printableComponentContainerMain.PrintableComponent = gcMain;
            _ReportHelper.Create(report);

            base.Print(_ReportHelper);
            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow()
        {
            base.DeleteRow(gvMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus ActivatedForm()
        {
            base.ActivatedForm();

            _ToolBtnInsert.Enabled = true;
            _ToolBtnSave.Enabled = true;
            _ToolBtnDel.Enabled = true;
            _ToolBtnRetrieve.Enabled = true;
            _ToolBtnPrintAll.Enabled = true;

            return ResultStatus.Success;
        }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            base.Open();

            //直接讀取資料
            //Retrieve();

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen()
        {
            base.AfterOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose()
        {
            return base.BeforeClose();
        }

        protected override ResultStatus COMPLETE()
        {
            MessageDisplay.Info(MessageDisplay.MSG_OK);
            Retrieve();
            return ResultStatus.Success;
        }
    }
}