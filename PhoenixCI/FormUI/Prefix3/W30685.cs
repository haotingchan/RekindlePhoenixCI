using BaseGround;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.TableDao;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PhoenixCI.FormUI.Prefix3
{
    /// <summary>
    /// 30685 CBOE VIX指數查詢
    /// 有寫到的功能：Export
    /// </summary>
    public partial class W30685 : FormParent
    {

        private VPR daoVPR;
        private VOLS daoVOLS;
        private VOLD daoVOLD;
        private ResultStatus exportStatus = ResultStatus.Fail;

        #region 一般查詢縮寫
        /// <summary>
        /// 起日(yyyyMMdd)
        /// </summary>
        public string StartDate
        {
            get
            {
                return txtStartDate.Text.Replace("/", "");
            }
        }

        /// <summary>
        /// 訖日(yyyyMMdd)
        /// </summary>
        public string EndDate
        {
            get
            {
                return txtEndDate.Text.Replace("/", "");
            }
        }
        #endregion

        public W30685(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();
            daoVPR = new VPR();
            daoVOLS = new VOLS();
            daoVOLD = new VOLD();
            this.Text = _ProgramID + "─" + _ProgramName;
            txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
            txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;

#if DEBUG
         txtStartDate.DateTimeValue = DateTime.ParseExact("2017/12/18" , "yyyy/MM/dd" , null);
         txtEndDate.DateTimeValue = DateTime.ParseExact("2017/12/19" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),Date=2017/12/18~2017/12/19";
#endif

        }

        protected override ResultStatus ActivatedForm()
        {
            base.ActivatedForm();

            _ToolBtnExport.Enabled = true;

            return ResultStatus.Success;
        }

        protected void ShowMsg(string msg)
        {
            labMsg.Text = msg;
            labMsg.Visible = true;
            this.Refresh();
            Thread.Sleep(5);
        }

        protected override ResultStatus Export()
        {
            try
            {
                #region 輸入&日期檢核
                if (string.Compare(txtStartDate.Text, txtEndDate.Text) > 0)
                {
                    MessageDisplay.Error(CheckDate.Datedif, GlobalInfo.ErrorText);
                    return ResultStatus.Fail;
                }
                #endregion

                //ready
                panFilter.Enabled = false;
                labMsg.Visible = true;
                labMsg.Text = "開始轉檔...";
                this.Cursor = Cursors.WaitCursor;
                this.Refresh();
                Thread.Sleep(5);

                string rptName;
                DataTable dt;

                //撈資料
                if (gbType.EditValue.AsString() == "rbCBOE")
                {
                    rptName = "CBOE";
                    DataTable dtContent = daoVPR.ListByMarket(StartDate, EndDate, 'C', 'C');
                    if (dtContent.Rows.Count <= 0)
                    {
                        labMsg.Visible = false;
                        MessageDisplay.Info(string.Format("{0},{1},{2},無任何資料!", txtStartDate.Text + "-" + txtEndDate.Text, this.Text, rptName), GlobalInfo.ResultText);
                        return ResultStatus.Fail;
                    }

                    //處理資料型態
                    dt = dtContent.Clone(); //轉型別用的datatable
                    dt.Columns["VPR_DATA_TIME"].DataType = typeof(string); //將原DataType(datetime)轉為string
                    foreach (DataRow row in dtContent.Rows)
                    {
                        dt.ImportRow(row);
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["VPR_DATA_TIME"] = Convert.ToDateTime(dtContent.Rows[i]["VPR_DATA_TIME"]).ToString("yyyy/MM/dd HH:mm:ss:fff");
                    }
                }
                else
                {
                    if (gbReport.EditValue.AsString() == "rbStatistics")
                    {
                        rptName = "JVVIXVolAvg";
                        dt = daoVOLS.GetDataByDate(StartDate, EndDate, "J", "Y");
                    }
                    else
                    {
                        rptName = "JVVIXVolDetail";
                        dt = daoVOLD.GetDataByDate(StartDate, EndDate, "J", "Y");
                    }
                    if (dt.Rows.Count <= 0)
                    {
                        labMsg.Visible = false;
                        MessageDisplay.Info(string.Format("{0},{1},{2}無任何資料!", txtStartDate.Text + "-" + txtEndDate.Text, this.Text, rptName), GlobalInfo.ResultText);
                        return ResultStatus.Fail;
                    }
                }

                //存Csv
                string etfFileName = "30685_" + rptName + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
                etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, etfFileName);
                ExportOptions csvref = new ExportOptions();
                csvref.HasHeader = true;
                csvref.Encoding = System.Text.Encoding.GetEncoding(950); //ASCII
                Common.Helper.ExportHelper.ToCsv(dt, etfFileName, csvref);

                labMsg.Text = "轉檔成功!";
                exportStatus = ResultStatus.Success;
                return ResultStatus.Success;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                labMsg.Text = "轉檔失敗";
            }
            finally
            {
                panFilter.Enabled = true;
                labMsg.Text = "";
                labMsg.Visible = false;
                this.Cursor = Cursors.Arrow;
            }
            return ResultStatus.Fail;

        }

        protected override ResultStatus ExportAfter(string startTime)
        {
            if (exportStatus == ResultStatus.Success)
            {
                MessageDisplay.Info("轉檔完成!", GlobalInfo.ResultText);
                return ResultStatus.Success;
            }
            else
            {
                MessageDisplay.Error("轉檔失敗", GlobalInfo.ErrorText);
                return ResultStatus.Fail;
            }
        }

        private void gbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool IsEnable = (sender as DevExpress.XtraEditors.RadioGroup).SelectedIndex == 0 ? false : true;

            gbReport.Enabled = IsEnable;
            gbReport.Visible = IsEnable;
            label7.Visible = IsEnable;
        }
    }
}