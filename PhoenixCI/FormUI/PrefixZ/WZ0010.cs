using ActionService;
using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Controls;
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

      public WZ0010(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            GridHelper.SetCommonGrid(gvMain);
            PrintableComponent = gcMain;
            this.Text = _ProgramID + "─" + _ProgramName;

         daoUPF = new UPF();
         daoUTP = new UTP();
      }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            base.Open();

            //DropDownList.CreateComboBoxDptIdAndName(cbxDpt);
            DataTable dtDept = new DPT().ListData();
            ddlDept.SetDataTable(dtDept, "DPT_ID", "DPT_ID_NAME", TextEditStyles.DisableTextEditor, "");
            ddlDept.EditValue = "";

            RepositoryItemLookUpEdit repLookUp = new RepositoryItemLookUpEdit();
            repLookUp.SetColumnLookUp(dtDept, "DPT_ID", "DPT_ID_NAME", TextEditStyles.DisableTextEditor, "");

            

            gcMain.DataSource = daoUPF.ListDataByDept("");
            colUPF_DPT_ID.ColumnEdit = repLookUp;

            //UPF_DPT_ID.ColumnEdit = repLookUp;

            GridHelper.AddModifyMark(gcMain, MODIFY_MARK);
            GridHelper.AddOpType(gcMain, new GridColumn[] { UPF_USER_ID });

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

            DataTable dtMain = (DataTable)gcMain.DataSource;

            int rowIndex = 0;

            foreach (DataRow row in dtMain.Rows)
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

                    rowIndex++;
                }
            }

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall)
        {
            base.Save(gcMain);

            DataTable dt = (DataTable)gcMain.DataSource;

            string tableName = "ci.UPF";
            string keysColumnList = "UPF_USER_ID";
            string insertColumnList = "UPF_USER_ID, UPF_USER_NAME, UPF_EMPLOYEE_ID, UPF_DPT_ID, UPF_PASSWORD, UPF_W_TIME, UPF_W_USER_ID, UPF_CHANGE_FLAG";
            string updateColumnList = "UPF_USER_NAME, UPF_EMPLOYEE_ID, UPF_DPT_ID, UPF_PASSWORD, UPF_W_TIME, UPF_W_USER_ID, UPF_CHANGE_FLAG";

            ResultData myResultData = serviceCommon.SaveForChanged(dt, tableName, insertColumnList, updateColumnList, keysColumnList, pokeBall);

            //DataTable dtDelete = myResultData.ChangedDataViewForDeleted.ToTable();

            //foreach (DataRow row in dtDelete.Rows)
            //{
            //    string userId = row["UPF_USER_ID"].AsString();
            //    bool result = daoUTP.DeleteUTPByUserId(userId);
            //}

            PrintOrExport(gcMain, myResultData);
            _IsPreventFlowPrint = true;
            _IsPreventFlowExport = true;

            return ResultStatus.Success;
        }

        protected void PrintOrExport(GridControl gridControl, ResultData resultData)
        {
            GridControl gridControlPrint = GridHelper.CloneGrid(gridControl);

            string originReportTitle = this.Text;

            ReportHelper reportHelper;

            if (resultData.ChangedDataViewForAdded.Count != 0)
            {
                reportHelper = PrintOrExportSetting();
                gridControlPrint.DataSource = resultData.ChangedDataViewForAdded;
                reportHelper.ReportTitle = originReportTitle + "─" + "新增";

                reportHelper.Create(GenerateReport(gridControlPrint));

                Print(reportHelper);
                Export(reportHelper);
            }

            PrintableComponent = gridControlPrint;

            //if (resultData.ChangedDataViewForDeleted.Count != 0)
            //{
            //    reportHelper = PrintOrExportSetting();
            //    gridControlPrint.DataSource = resultData.ChangedDataViewForDeleted;
            //    reportHelper.ReportTitle = originReportTitle + "─" + "刪除";

            //    Print(reportHelper);
            //    Export(reportHelper);
            //}

            if (resultData.ChangedDataViewForModified.Count != 0)
            {
                reportHelper = PrintOrExportSetting();
                gridControlPrint.DataSource = resultData.ChangedDataViewForModified;
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

            DataTable dt = ((DataView)gridControl.DataSource).ToTable();
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
    }
}