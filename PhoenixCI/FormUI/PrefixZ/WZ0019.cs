using ActionService;
using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using Common.Helper;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using PhoenixCI.Widget;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace PhoenixCI.FormUI.PrefixZ
{
    public partial class WZ0019 : FormParent
    {
      private DZ0019 daoZ0019;
      private UPF daoUPF;

      public WZ0019(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            GridHelper.SetCommonGrid(gvMain);
            PrintableComponent = gcMain;
            this.Text = _ProgramID + "─" + _ProgramName;

         daoZ0019 = new DZ0019();
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

            DropDownList.LookUpItemDptIdAndName(ddlDept);

            gcMain.DataSource = daoUPF.ListDataByDept("");
            gvMain.TrimAllCell();

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

            _ToolBtnSave.Enabled = true;
            _ToolBtnRetrieve.Enabled = true;
            _ToolBtnPrintAll.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve()
        {
            base.Retrieve(gcMain);

            string dptID = ddlDept.EditValue.AsString();
            gcMain.DataSource = daoZ0019.ListUTPByDept(dptID);

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

            string fileName = "使用者權限查詢_{0}.txt";
            fileName = string.Format(fileName, DateTime.Now.ToString("yyyyMMdd"));

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt";
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
                    ExportHelper.ToText(dtMain, stream, exportOptions);
                    MessageDisplay.Info("存檔完成!");
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