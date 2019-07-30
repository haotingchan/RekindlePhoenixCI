using ActionService.DbDirect;
using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraReports.UI;
using PhoenixCI.Widget;
using System;
using System.Collections.Generic;
using System.Data;

namespace PhoenixCI.FormUI.PrefixZ
{
    public partial class WZ0010 : FormParent
    {
        private UPF daoUPF;
        private UTP daoUTP;
        private DataTable dtForAdd;

        public WZ0010(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            GridHelper.SetCommonGrid(gvMain,true,new GridColumn[] { UPF_USER_ID});
            PrintableComponent = gcMain;
            this.Text = _ProgramID + "─" + _ProgramName;

            daoUPF = new UPF();
            daoUTP = new UTP();
            serviceCommon = new ServiceCommon();
        }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            base.Open();

            DropDownList.LookUpItemDptIdAndName(ddlDept);

            RepositoryItemLookUpEdit repLookUp = new RepositoryItemLookUpEdit();
            DropDownList.RepositoryItemDptIdAndName(repLookUp);

            GridHelper.AddModifyMark(gcMain, MODIFY_MARK);
            GridHelper.AddOpType(gcMain, new GridColumn[] { UPF_USER_ID });

            gcMain.DataSource = daoUPF.ListDataByDept("");
            UPF_DPT_ID.ColumnEdit = repLookUp;

            //if (GlobalInfo.USER_ID.ToUpper() != GlobalDaoSetting.GetConnectionInfo.ConnectionName)
            //{
            //    btnPrint.Visible = false;
            //}

            gvMain.BestFitColumns();

            return ResultStatus.Success;
        }

        protected override ResultStatus AfterOpen()
        {
            base.AfterOpen();

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

        protected override ResultStatus Retrieve()
        {
            base.Retrieve(gcMain);

            string dptID = ddlDept.EditValue.AsString();
            gcMain.DataSource = daoUPF.ListDataByDept(dptID);

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield()
        {
            base.CheckShield(gcMain);
            if (!IsDataModify(gcMain)) { return ResultStatus.Fail; }

            DataTable dt = (DataTable)gcMain.DataSource;

            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Unchanged)
                {
                    string UPF_USER_ID = row["UPF_USER_ID"].AsString().Trim();

                    if (!string.IsNullOrEmpty(row["MODIFY_MARK"].ToString().Trim()))
                    {
                        row["UPF_W_USER_ID"] = GlobalInfo.USER_ID;
                        row["UPF_W_TIME"] = DateTime.Now;
                        row["UPF_CHANGE_FLAG"] = "Y";

                        char[] userId = UPF_USER_ID.ToCharArray();
                        Array.Reverse(userId);

                        row["UPF_PASSWORD"] = DateTime.Now.ToString("MMdd") + 'm' + new string(userId);
                    }

                    string[] allowNullColumnList = { "UPF_LOCK_TIME" };
                    if (!GridHelper.CheckRequired(gcMain, allowNullColumnList))
                    {
                        return ResultStatus.Fail;
                    }
                }
            }

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall)
        {
            try
            {
                ResultStatus myCheckResult = CheckShield();
                if (myCheckResult != ResultStatus.Success) return myCheckResult;
                if (myCheckResult == ResultStatus.Success)
                {
                    base.Save(gcMain);

                    DataTable dt = (DataTable)gcMain.DataSource;
                    dtForAdd = dt.GetChanges(DataRowState.Added);
                    DataTable dtForModified = dt.GetChanges(DataRowState.Modified);
                    DataTable dtForDeleted = dt.GetChanges(DataRowState.Deleted);


                    ResultData myResultData = daoUPF.Update(dt);

                    DataTable dtTemp = new DataTable();
                    //若刪除user ,一併刪除相關權限
                    if (dtForDeleted != null)
                    {
                        dtTemp = dtForDeleted.Clone();

                        int rowIndex = 0;
                        foreach (DataRow dr in dtForDeleted.Rows)
                        {
                            DataRow drNewDelete = dtTemp.NewRow();
                            for (int colIndex = 0; colIndex < dtForDeleted.Columns.Count; colIndex++)
                            {
                                drNewDelete[colIndex] = dr[colIndex, DataRowVersion.Original];
                            }
                            dtTemp.Rows.Add(drNewDelete);
                            rowIndex++;
                        }

                        foreach (DataRow row in dtTemp.Rows)
                        {
                            string userId = row["UPF_USER_ID"].AsString();
                            bool result = daoUTP.DeleteUTPByUserId(userId);
                        }
                    }

                    //列印
                    PrintOrExport(gcMain, dtForAdd, dtTemp, dtForModified);

                    _IsPreventFlowPrint = true;
                    _IsPreventFlowExport = true;

                }
            }
            catch (Exception ex)
            {
                MessageDisplay.Error(ex.Message);
                throw;
            }
            return ResultStatus.Success;
        }

        protected void PrintOrExport(GridControl gridControl, DataTable ChangedForAdded,
          DataTable ChangedForDeleted, DataTable ChangedForModified)
        {
            GridControl gridControlPrint = GridHelper.CloneGrid(gridControl);

            string originReportTitle = this.Text;

            ReportHelper reportHelper;

            if (ChangedForAdded != null)
                if (ChangedForAdded.Rows.Count != 0)
                {
                    reportHelper = PrintOrExportSetting();
                    reportHelper.IsHandlePersonVisible = true;
                    reportHelper.IsManagerVisible = true;
                    gridControlPrint.DataSource = ChangedForAdded;
                    reportHelper.ReportTitle = originReportTitle + "─" + "新增";

                    reportHelper.Create(GenerateReport(gridControlPrint));

                    Print(reportHelper);
                    Export(reportHelper);
                }

            PrintableComponent = gridControlPrint;
            if (ChangedForDeleted != null)
                if (ChangedForDeleted.Rows.Count != 0)
                {
                    reportHelper = PrintOrExportSetting();
                    reportHelper.IsHandlePersonVisible = true;
                    reportHelper.IsManagerVisible = true;
                    gridControlPrint.DataSource = ChangedForDeleted;
                    reportHelper.ReportTitle = originReportTitle + "─" + "刪除";

                    reportHelper.Print();
                    reportHelper.Export(FileType.PDF, reportHelper.FilePath);
                }

            if (ChangedForModified != null)
                if (ChangedForModified.Rows.Count != 0)
                {
                    reportHelper = PrintOrExportSetting();
                    reportHelper.IsHandlePersonVisible = true;
                    reportHelper.IsManagerVisible = true;
                    gridControlPrint.DataSource = ChangedForModified;
                    reportHelper.ReportTitle = originReportTitle + "─" + "變更";

                    Print(reportHelper);
                    Export(reportHelper);
                }
        }

        protected override ResultStatus Run(PokeBall args)
        {
            base.Run(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Import()
        {
            base.Import(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Export()
        {
            base.Export(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper)
        {
            base.Print(reportHelper);

            return ResultStatus.Success;
        }

        public override void ProcessPrintAll(ReportHelper reportHelper)
        {
            CommonReportPortraitA4 report = new CommonReportPortraitA4();
            report.IsHandlePersonVisible = false;
            report.IsManagerVisible = false;
            reportHelper.Create(report);

            base.Print(reportHelper);
            MessageDisplay.Info(MessageDisplay.MSG_PRINT);
        }

        protected override ResultStatus InsertRow()
        {
            base.InsertRow(gvMain);

            gvMain.GetFocusedDataRow()["OP_TYPE"] = "I";
            gvMain.GetFocusedDataRow()["MODIFY_MARK"] = "※";
            gvMain.GetFocusedDataRow()["UPF_DPT_ID"] = ddlDept.EditValue == null ? "" : ddlDept.EditValue.AsString();
            gvMain.Focus();

            gvMain.FocusedColumn = gvMain.Columns[0];

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow()
        {
            base.DeleteRow(gvMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose()
        {
            return base.BeforeClose();
        }

        private DataTable FormatDataTableForPrintAndExport(DataTable inputDT)
        {
            DataTable result = inputDT.Copy();

            foreach (DataRow row in result.Rows)
            {
                row["UPF_DPT_ID"] = UPF_DPT_ID.ColumnEdit.GetDisplayText(row["UPF_DPT_ID"].AsString());
            }

            return result;
        }

        private XtraReport GenerateReport(GridControl gridControl)
        {
            RZ0011 report = new RZ0011();

            DataTable dt = (DataTable)gridControl.DataSource;
            dt = FormatDataTableForPrintAndExport(dt);

            report.DataSource = dt;
            report.Detail.Borders = DevExpress.XtraPrinting.BorderSide.All;

            XRTable table = new XRTable();

            XRTableRow headerRow = new XRTableRow();
            headerRow.BackColor = System.Drawing.SystemColors.Control;
            table.Controls.Add(headerRow);
            report.Detail.Controls.Add(table);

            XRTableRow contentRow = new XRTableRow();
            table.Controls.Add(contentRow);

            Dictionary<string, string> reportColumns = new Dictionary<string, string>();
            reportColumns.Add("UPF_USER_ID", "使用者代號");
            reportColumns.Add("UPF_USER_NAME", "使用者名稱");
            reportColumns.Add("UPF_EMPLOYEE_ID", "員工編號");
            reportColumns.Add("UPF_DPT_ID", "部門");
            reportColumns.Add("UPF_W_TIME", "設定時間");

            XRTableCell headerCell = new XRTableCell();

            XRTableCell contentCell = new XRTableCell();

            foreach (DataColumn column in dt.Columns)
            {
                if (reportColumns.ContainsKey(column.ColumnName))
                {
                    headerCell = new XRTableCell();

                    headerCell.Text = reportColumns[column.ColumnName];
                    headerCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    headerCell.Padding = 1;
                    headerRow.Controls.Add(headerCell);

                    contentCell = new XRTableCell();
                    contentCell.Padding = 1;

                    if (column.ColumnName == "UPF_W_TIME")
                    {
                        headerCell.WidthF = 150F;
                        contentCell.WidthF = 150F;
                        contentCell.DataBindings.Add("Text", null, column.ColumnName, "{0:yyyy/MM/dd HH:mm:ss}");
                    }
                    else
                    {
                        contentCell.DataBindings.Add("Text", null, column.ColumnName);
                    }

                    contentRow.Controls.Add(contentCell);
                }
            }
            report.lblUserIdTxt.DataBindings.Add("Text", null, "UPF_USER_ID");
            report.lblUserNameTxt.DataBindings.Add("Text", null, "UPF_USER_NAME");
            report.lblUserPwdTxt.DataBindings.Add("Text", null, "UPF_PASSWORD");
            report.lblWdateTxt.DataBindings.Add("Text", null, "UPF_W_TIME", "{0:yyyy/MM/dd}");

            table.WidthF = ((XRSubreport)report.FindControl("xrSubreportMain", true)).WidthF;

            return report;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (((DataTable)gcMain.DataSource).Rows.Count > 0)
            {
                GridControl gridControlPrint = GridHelper.CloneGrid(gcMain);

                string originReportTitle = this.Text;

                ReportHelper reportHelper;

                DataTable dt = ((DataView)gvMain.DataSource).ToTable().Clone();
                dt.ImportRow(((DataRowView)gvMain.GetFocusedRow()).Row);
                reportHelper = PrintOrExportSetting();
                reportHelper.IsHandlePersonVisible = true;
                reportHelper.IsManagerVisible = true;
                gridControlPrint.DataSource = dt;
                reportHelper.ReportTitle = originReportTitle;

                reportHelper.Create(GenerateReport(gridControlPrint));

                Print(reportHelper);
                Export(reportHelper);
            }
            else
            {
                MessageDisplay.Info("無補印資料");
            }

        }
    }
}