using ActionService;
using BaseGround;
using BaseGround.Report;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DevExpress.XtraReports.UI;
using Log;
using PhoenixCI.Widget;
using System;
using System.Collections.Generic;
using System.Data;

namespace PhoenixCI.FormUI.PrefixZ
{
    public partial class WZ0011 : FormParent
    {
        private ReportHelper _ReportHelper;

      private UPF daoUPF;

      public WZ0011(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;

         daoUPF = new UPF();
      }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            base.Open();

            DropDownList.ComboBoxUserIdAndName(cbxUserId);

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

            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper)
        {
            reportHelper.Create(GenerateReport());
            _ReportHelper = reportHelper;

            base.Print(reportHelper);

            return ResultStatus.Success;
        }

        protected override ResultStatus Export(ReportHelper reportHelper)
        {
            reportHelper = _ReportHelper;

            base.Export(reportHelper);

            return ResultStatus.Success;
        }

        protected override ResultStatus COMPLETE()
        {
            MessageDisplay.Info("使用者起始設定完成!");

            this.Close();

            return ResultStatus.Success;
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (cbxUserId.SelectedValue == null)
            {
                MessageDisplay.Warning("請選擇使用者代號!");
            }
            else
            {
                bool result = daoUPF.UpdatePasswordByUserId(cbxUserId.SelectedValue.ToString(), txtPassword.Text);

                if (result)
                {
                    base.ProcessSaveFlow();
                    SingletonLogger.Instance.Info(GlobalInfo.USER_ID, _ProgramID, "使用者起始設定", "I");
                }
                else
                {
                    MessageDisplay.Error("密碼變更失敗!");
                }
            }
        }

        private void cbxUserId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxUserId.SelectedItem != null)
            {
                char[] userId = cbxUserId.SelectedValue.ToString().Trim().ToCharArray();
                Array.Reverse(userId);
                txtPassword.Text = DateTime.Now.ToString("MMdd") + "m" + new string(userId);
            }
        }

        private XtraReport GenerateReport()
        {
            RZ0011 report = new RZ0011();

            DataTable dtMain = daoUPF.ListDataByUserId(cbxUserId.SelectedValue.AsString());

            report.DataSource = dtMain;
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
            reportColumns.Add("DPT_ID_NAME", "部門");
            reportColumns.Add("UPF_PASSWORD", "密碼");
            reportColumns.Add("UPF_W_TIME", "設定時間");

            XRTableCell headerCell = new XRTableCell();

            XRTableCell contentCell = new XRTableCell();

            foreach (DataColumn column in dtMain.Columns)
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
                    else if (column.ColumnName == "UPF_PASSWORD")
                    {
                        contentCell.Text = "**********";
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