using ActionService.Extensions;
using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects;
using DevExpress.XtraEditors.Repository;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PhoenixCI.FormUI.Prefix1
{
    public partial class W1xxx : FormParent
    {
        private string OCF_TYPE;
        public W1xxx() {
            InitializeComponent();
        }
        
        public W1xxx(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            GridHelper.SetCommonGrid(gvMain,true);
            GridHelper.SetCommonGrid(gvSpLog);

            PrintableComponent = gcMain;
            this.Text = _ProgramID + "─" + _ProgramName;

            RepositoryItemCheckEdit repCheck = new RepositoryItemCheckEdit();
            repCheck.AllowGrayed = false;
            repCheck.ValueChecked = "1";
            repCheck.ValueUnchecked = "0";
            repCheck.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;

            gcMain.RepositoryItems.Add(repCheck);
            gcol_gcMain_TXF_DEFAULT.ColumnEdit = repCheck;
            _IsProcessRunAsync = true;
        }

        public override ResultStatus BeforeOpen()
        {
            base.BeforeOpen();

            return ResultStatus.Success;
        }

        protected override ResultStatus Open()
        {
            if (DesignMode) return ResultStatus.Success;
            base.Open();

            //txtPrevOcfDate.DateTimeValue = GlobalInfo.OCF_PREV_DATE;
            
            txtOcfDate.DateTimeValue = PbFunc.f_ocf_date(0, _DB_TYPE).AsDateTime(); //GlobalInfo.OCF_DATE;
            OCF_TYPE = txtOcfDate.DateType == BaseGround.Widget.TextDateEdit.DateTypeItem.Month ? "M" : "D";

            gcMain.DataSource = servicePrefix1.ListTxfByTxn(_ProgramID).Trim();

            gcLogsp.DataSource = servicePrefix1.ListLogsp(txtOcfDate.DateTimeValue, _ProgramID,OCF_TYPE).Trim();
            gvSpLog.AppearancePrint.HeaderPanel.Font = new Font("標楷體", 10);
            gvSpLog.AppearancePrint.Row.Font = new Font("標楷體", 10);
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

            _ToolBtnRun.Enabled = true;

            _ToolBtnInsert.Enabled = false;
            _ToolBtnSave.Enabled = false;
            _ToolBtnDel.Enabled = false;
            _ToolBtnRetrieve.Enabled = true;
            _ToolBtnPrintAll.Enabled = true;

            return ResultStatus.Success;
        }

        protected override ResultStatus Retrieve()
        {
            base.Retrieve(gcMain);

            DataTable dtLogSP = servicePrefix1.ListLogsp(txtOcfDate.DateTimeValue, _ProgramID,OCF_TYPE).Trim();
            gcLogsp.DataSource = dtLogSP;
            if (dtLogSP.Rows.Count == 0)
            {
                MessageDisplay.Info(GlobalInfo.MsgNoData);
                xtraTabControl.SelectedTabPage = xtraTabPageMain;
            }
            else
            {
                xtraTabControl.SelectedTabPage = xtraTabPageQuery;
            }


            return ResultStatus.Success;
        }

        protected override ResultStatus CheckShield()
        {
            base.CheckShield(gcMain);

            return ResultStatus.Success;
        }

        protected override ResultStatus RunBefore(PokeBall args)
        {

            string txfServer = gvMain.GetRowCellValue(1, "TXF_SERVER").AsString();
            if (txfServer != GlobalDaoSetting.GetConnectionInfo.ConnectionName)
            {
                MessageDisplay.Warning("作業Server(" + txfServer + ") 不等於連線Server(" + GlobalDaoSetting.GetConnectionInfo.ConnectionName + ")");
                return ResultStatus.Fail;
            }

            string inputDate = txtOcfDate.Text;
            string nowDate = DateTime.Now.ToString(txtOcfDate.Properties.EditFormat.FormatString);

            if (OCF_TYPE == "D")
            {
                if (inputDate != nowDate)
                {
                    if (MessageDisplay.Choose("交易日期(" + inputDate + ") 不等於今日(" + nowDate + ")，是否要繼續?") == DialogResult.No)
                    {
                        return ResultStatus.Fail;
                    }
                }
                if (servicePrefix1.HasLogspDone(Convert.ToDateTime(inputDate), _ProgramID))
                {
                    if (MessageDisplay.Choose(_ProgramID + " 作業 " + inputDate + "「曾經」執行過，\n是否要繼續？\n\n★★★建議先執行 [預覽] 確認執行狀態") == DialogResult.No)
                    {
                        return ResultStatus.Fail;
                    }
                }

                if (!servicePrefix1.setOCF(txtOcfDate.DateTimeValue, _DB_TYPE, GlobalInfo.USER_ID))
                {
                    return ResultStatus.Fail;
                }
            }
            else if (OCF_TYPE == "M")
            {
                if (inputDate != nowDate)
                {
                    if (MessageDisplay.Choose("月份(" + inputDate + ") 不等於本月(" + nowDate + ")，是否要繼續?") == DialogResult.No)
                    {
                        return ResultStatus.Fail;
                    }
                }
            }


            GridHelper.AcceptText(gcMain);

            return base.RunBefore(args);
        }

        protected override ResultStatus Run(PokeBall args)
        {
            this.BeginInvoke(new MethodInvoker(() => {
                args.GridControlMain = gcMain;
                args.GridControlSecond = gcLogsp;
                args.OcfDate = txtOcfDate.DateTimeValue;
                args.OcfType = OCF_TYPE;
            }));

            ResultStatus result = base.RunAsync(args);

            return result;
        }

        protected override string RunBeforeEveryItem(PokeBall args)
        {
            base.RunBeforeEveryItem(args);

            return "";
        }

        protected override ResultStatus RunAfterEveryItem(PokeBall args)
        {
            base.RunAfterEveryItem(args);

            return ResultStatus.Success;
        }

        protected override ResultStatus RunAfter(PokeBall args)
        {
            base.RunAfter(args);

            Retrieve();

            return ResultStatus.Success;
        }

        protected override ResultStatus Print(ReportHelper reportHelper)
        {
            Retrieve();
            CommonReportLandscapeA4 report = new CommonReportLandscapeA4();
            reportHelper.Create(report);
            
            base.Print(reportHelper);

            return ResultStatus.Success;
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

        protected override ResultStatus BeforeClose()
        {
            return base.BeforeClose();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvMain.RowCount; i++)
            {
                gvMain.SetRowCellValue(i, "TXF_DEFAULT", "1");
            }
        }

        private void btnUnselectedAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvMain.RowCount; i++)
            {
                gvMain.SetRowCellValue(i, "TXF_DEFAULT", "0");
            }
        }

        private void xtraTabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if(xtraTabControl.SelectedTabPage == xtraTabPageMain)
            {
                PrintableComponent = gcMain;
            }
            else if(xtraTabControl.SelectedTabPage == xtraTabPageQuery)
            {
                Retrieve();
                PrintableComponent = gcLogsp;
            }
        }
    }
}