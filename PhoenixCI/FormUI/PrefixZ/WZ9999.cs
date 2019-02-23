using ActionService;
using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using Common.Helper;
using DataObjects.Dao.Together.SpecificDao;
using Log;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace PhoenixCI.FormUI.PrefixZ
{
    public partial class WZ9999 : FormParent
    {
        private string _Condition;
      private DZ9999 daoDZ9999;

      public WZ9999(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            GridHelper.SetCommonGrid(gvMain);

            PrintableComponent = gcMain;
            this.Text = _ProgramID + "─" + _ProgramName;
         daoDZ9999 = new DZ9999();
      }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            base.Open();
            DateTime time = DateTime.Now;
            time = new DateTime(time.Year, time.Month, 1);
            txtStartDate.DateTimeValue = time.AddMonths(-1);
            txtEndDate.DateTimeValue = time.AddDays(-1);
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

            _ToolBtnPrintAll.Enabled = true;
            _ToolBtnSave.Enabled = true;
            _ToolBtnRetrieve.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve()
        {
            base.Retrieve(gcMain);
            DateTime startDate = txtStartDate.DateTimeValue;
            DateTime endDate = txtEndDate.DateTimeValue;
            string startTime = " ";
            string endTime = " ";
            _Condition = "查詢條件：日期：{0}-{1}";
            _Condition = string.Format(_Condition, txtStartDate.Text, txtEndDate.Text);

            if (chkTime.Checked)
            {
                startTime = txtStartTime.Text;
                endTime = txtEndTime.Text;
                _Condition = _Condition + string.Format(",非合理時間 < {0} or > {1}", startTime, endTime);
            }
            if (chkAudit.Checked)
            {
                _Condition = _Condition + ",權限設定作業";
            }
            else
            {
                _Condition = _Condition + ",全部作業";
            }

            gcMain.DataSource = daoDZ9999.ListLOGF(startDate, endDate, startTime, endTime, chkAudit.EditValue.ToString());
            gvMain.TrimAllCell();

            string keyData = "";
            if (gvMain.RowCount == 0)
            {
                keyData = String.Format("{0}無資料", _Condition);
                SingletonLogger.Instance.Info(GlobalInfo.USER_ID, _ProgramID, keyData, " ");
            }
            else
            {
                keyData = string.Format("{0}共有{1}筆", _Condition, gvMain.RowCount);
                SingletonLogger.Instance.Info(GlobalInfo.USER_ID, _ProgramID, keyData, " ");
            }

            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield()
        {
            base.CheckShield(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus Save(PokeBall pokeBall)
        {
            base.Save(gcMain);
            _IsPreventFlowPrint = true;
            _IsPreventFlowExport = true;

            string fileName = "Z9999_{0}.csv";
            fileName = string.Format(fileName, DateTime.Now.ToString("yyyyMMdd"));

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV|*.csv";
            saveFileDialog.FileName = fileName;
            saveFileDialog.InitialDirectory = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH;

            Stream stream;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((stream = saveFileDialog.OpenFile()) != null)
                {
                    DataTable dtMain = (DataTable)gcMain.DataSource;
                    ExportOptions exportOptions = new ExportOptions();
                    exportOptions.HasHeader = true;
                    ExportHelper.ToCsv(dtMain, stream, exportOptions);
                    MessageDisplay.Info("存檔完成!");
                    SingletonLogger.Instance.Info(GlobalInfo.USER_ID, _ProgramID, "轉出檔案" + saveFileDialog.FileName, "E");
                }
            }

            return ResultStatus.Success;
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
            CommonReportLandscapeA4 report = new CommonReportLandscapeA4();
            report.IsHandlePersonVisible = false;
            report.IsManagerVisible = false;
            reportHelper.Create(report);
            reportHelper.AddHeaderBottomInfo(_Condition);

            base.Print(reportHelper);
            MessageDisplay.Info(MessageDisplay.MSG_PRINT);
        }

        protected override ResultStatus InsertRow()
        {
            base.InsertRow(gvMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus DeleteRow()
        {
            base.DeleteRow(gvMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus COMPLETE()
        {
            return ResultStatus.Success;
        }

        protected override ResultStatus BeforeClose()
        {
            return base.BeforeClose();
        }
    }
}