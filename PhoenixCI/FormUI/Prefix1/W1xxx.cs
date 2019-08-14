using ActionService.Extensions;
using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using Common.Config;
using Common.Helper;
using DataObjects;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
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

        //protected override ResultStatus Run(PokeBall args)
        //{
        //    this.BeginInvoke(new MethodInvoker(() => {
        //        args.GridControlMain = gcMain;
        //        args.GridControlSecond = gcLogsp;
        //        args.OcfDate = txtOcfDate.DateTimeValue;
        //        args.OcfType = OCF_TYPE;
        //    }));

        //    ResultStatus result = base.RunAsync(args);

        //    return result;
        //}

        /// <summary>
        /// 1系列功能使用
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected override ResultStatus Run(PokeBall args)
        {
            DateTime OcfDate = txtOcfDate.DateTimeValue;
            this.Invoke(new MethodInvoker(() => {
                FormWait formWait = new FormWait();

                SplashScreenManager.ShowForm(this, typeof(FormWait), true, true);

                //SplashScreenManager.ShowForm(this , typeof(FormWait) , true , true , SplashFormStartPosition.Manual , pointWait , ParentFormState.Locked);
            }));

            GridView gv = gvMain;


            DataTable dtLOGSPForRuned = servicePrefix1.ListLogspForRunned(OcfDate, _ProgramID, OCF_TYPE);
            DataView dvLOGSPForRuned = new DataView(dtLOGSPForRuned);



            servicePrefix1.SetTXF1(" ", _ProgramID);

            for (int i = 0; i < gv.RowCount; i++)
            {
                string TXF_SERVER = gv.GetRowCellValue(i, "TXF_SERVER").AsString();
                string TXF_DB = gv.GetRowCellValue(i, "TXF_DB").AsString();
                string TXF_TXN_ID = gv.GetRowCellValue(i, "TXF_TXN_ID").AsString();
                int TXF_SEQ_NO = gv.GetRowCellValue(i, "TXF_SEQ_NO").AsInt();
                string TXF_TYPE = gv.GetRowCellValue(i, "TXF_TYPE").AsString();
                string TXF_TID = gv.GetRowCellValue(i, "TXF_TID").AsString();
                string TXF_TID_NAME = gv.GetRowCellValue(i, "TXF_TID_NAME").AsString();
                string TXF_DESC = gv.GetRowCellValue(i, "TXF_DESC").AsString();
                string TXF_DEFAULT = gv.GetRowCellValue(i, "TXF_DEFAULT").AsString();
                string TXF_REDO = gv.GetRowCellValue(i, "TXF_REDO").AsString();
                string TXF_ARG = gv.GetRowCellValue(i, "TXF_ARG").AsString();
                string TXF_PERIOD = gv.GetRowCellValue(i, "TXF_PERIOD").AsString();
                string TXF_SERVICE = gv.GetRowCellValue(i, "TXF_SERVICE").AsString();
                string TXF_FOLDER = gv.GetRowCellValue(i, "TXF_FOLDER").AsString();
                string TXF_AP_NAME = gv.GetRowCellValue(i, "TXF_AP_NAME").AsString();
                args.TXF_TID = TXF_TID;
                args.TXF_TID_NAME = TXF_TID_NAME;

                if (TXF_DEFAULT == "1")
                {
                    DateTime LOGSP_DATE = OcfDate;
                    string LOGSP_TXN_ID = _ProgramID;
                    int LOGSP_SEQ_NO = TXF_SEQ_NO;
                    string LOGSP_TID = TXF_DESC;
                    string LOGSP_TID_NAME = TXF_TID_NAME;
                    DateTime LOGSP_BEGIN_TIME = new DateTime();
                    DateTime LOGSP_END_TIME = new DateTime();
                    string LOGSP_MSG = "";


                    //判斷是否可重覆執行
                    if (TXF_REDO == "N")
                    {
                        dvLOGSPForRuned.RowFilter = "LOGSP_TID='" + LOGSP_TID + "' AND NOT ISNULL(LOGSP_BEGIN_TIME)";
                        if (dvLOGSPForRuned.Count != 0)
                        {
                            if (MessageDisplay.Choose(TXF_TID + " ★★★曾經執行過且不可重覆執行，是否強迫繼續執行 ?") == DialogResult.No)
                            {
                                return ResultStatus.Fail;
                            }
                        }
                    }


                    #region 開始執行
                    LOGSP_BEGIN_TIME = DateTime.Now;

                    string nextYmd = PbFunc.f_ocf_date(2, _DB_TYPE);
                    if (!string.IsNullOrEmpty(TXF_PERIOD))
                    {
                        switch (TXF_PERIOD)
                        {
                            case "M"://月底執行
                                if (OcfDate.ToString("yyyyMM") == PbFunc.Left(nextYmd, 6))
                                {
                                    LOGSP_MSG = "完成! (今日非月底，不需執行)";
                                    this.Invoke(new MethodInvoker(() => { gv.SetRowCellValue(i, "TXF_DEFAULT", 0); }));
                                }
                                break;
                            case "W"://週最後一天執行
                                if (Convert.ToInt32(OcfDate.DayOfWeek) < Convert.ToInt32(nextYmd.AsDateTime("yyyyMMdd").DayOfWeek))
                                {
                                    LOGSP_MSG = "完成! (今日非本週最後1天，不需執行)";
                                    this.Invoke(new MethodInvoker(() => { gv.SetRowCellValue(i, "TXF_DEFAULT", 0); }));
                                }
                                break;
                            case "Y"://年底執行
                                if (OcfDate.ToString("yyyy") == PbFunc.Left(nextYmd, 4))
                                {
                                    LOGSP_MSG = "完成! (今日非本年度最後1日，不需執行)";
                                    this.Invoke(new MethodInvoker(() => { gv.SetRowCellValue(i, "TXF_DEFAULT", 0); }));
                                }
                                break;

                        }
                        LOGSP_END_TIME = DateTime.Now;

                        servicePrefix1.SaveLogsp(LOGSP_DATE, LOGSP_TXN_ID, LOGSP_SEQ_NO, LOGSP_TID, LOGSP_TID_NAME, LOGSP_BEGIN_TIME, LOGSP_END_TIME, LOGSP_MSG, OCF_TYPE);
                        continue;
                    }


                    //開始前執行特別的Function
                    string rtnText = RunBeforeEveryItem(args);
                    if (!string.IsNullOrEmpty(rtnText))
                    {
                        if (PbFunc.Left(rtnText, 4) == "不需執行")
                        {
                            LOGSP_MSG = "完成! (" + rtnText + ")";
                            gv.SetRowCellValue(i, "ERR_MSG", LOGSP_MSG);
                            gv.SetRowCellValue(i, "TXF_DEFAULT", 0);
                            LOGSP_END_TIME = DateTime.Now;

                            servicePrefix1.SaveLogsp(LOGSP_DATE, LOGSP_TXN_ID, LOGSP_SEQ_NO, LOGSP_TID, LOGSP_TID_NAME, LOGSP_BEGIN_TIME, LOGSP_END_TIME, LOGSP_MSG, OCF_TYPE);

                        }
                        else
                        {
                            if (MessageDisplay.Choose($"{rtnText}是否強迫繼續執行?", MessageBoxDefaultButton.Button2).AsInt() == 2)
                            {
                                gv.SetRowCellValue(i, "ERR_MSG", rtnText);
                                continue;
                            }
                        }
                    }

                    //記錄正在執行
                    servicePrefix1.SetTXF1(TXF_TID, _ProgramID);

                    servicePrefix1.SaveLogs(LOGSP_DATE, TXF_TID, DateTime.Now, GlobalInfo.USER_ID, "開始執行");

                    ResultData resultData = new ResultData();
                    string fileName = "";
                    switch (TXF_TYPE)
                    {
                        //Informatica
                        case "I":
                            fileName = $@"{GlobalInfo.DEFAULT_BATCH_ErrSP_DIRECTORY_PATH}\{TXF_SERVER}_{TXF_TXN_ID}_{TXF_SEQ_NO}_infor";
                            resultData = serviceCommon.ExecuteInfoWorkFlow(TXF_TID, UserProgInfo, TXF_FOLDER, TXF_SERVICE, TXF_AP_NAME, fileName);
                            break;
                        //SP
                        case "S":
                            List<DbParameterEx> listParams = null;

                            // 如果這個SP有參數的話
                            if (TXF_ARG == "Y")
                            {
                                DataTable dtTXFPARM = serviceCommon.ListTXFPARM(TXF_SERVER, TXF_DB, TXF_TXN_ID, TXF_TID);

                                if (dtTXFPARM.Rows.Count > 0)
                                {
                                    listParams = new List<DbParameterEx>();
                                }

                                foreach (DataRow row in dtTXFPARM.Rows)
                                {
                                    string TXFPARM_ARG = row["TXFPARM_ARG"].AsString();
                                    string TXFPARM_ARG_TYPE = row["TXFPARM_ARG_TYPE"].AsString();
                                    string TXFPARM_DEFAULT = row["TXFPARM_DEFAULT"].AsString();

                                    DbParameterEx paramEx;

                                    switch (TXFPARM_ARG)
                                    {
                                        case "":
                                            paramEx = new DbParameterEx("", TXFPARM_DEFAULT);
                                            listParams.Add(paramEx);
                                            break;

                                        case "em_ymd":
                                            paramEx = new DbParameterEx("", OcfDate.ToString("yyyyMMdd"));
                                            listParams.Add(paramEx);
                                            break;

                                        case "em_ym":
                                            paramEx = new DbParameterEx("", OcfDate.ToString("yyyyMM"));
                                            listParams.Add(paramEx);
                                            break;

                                        case "em_date":
                                            paramEx = new DbParameterEx();
                                            paramEx.DbType = DbTypeEx.Date;
                                            paramEx.Name = "";
                                            paramEx.Value = OcfDate;
                                            listParams.Add(paramEx);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }

                            ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(TXF_DB);

                            try
                            {
                                resultData = serviceCommon.ExecuteStoredProcedure(connectionInfo, string.Format("{0}.{1}", TXF_DB, TXF_TID), listParams, true);
                            }
                            catch (Exception ex)
                            {
                                resultData.Status = ResultStatus.Fail;
                                string msg =
                                fileName = $@"{GlobalInfo.DEFAULT_BATCH_ErrSP_DIRECTORY_PATH}\{TXF_SERVER}_{TXF_TXN_ID}_{TXF_SEQ_NO}.err";
                                System.IO.File.WriteAllText(fileName, ex.Message);
                                resultData.returnString = $"請通知「{TXF_AP_NAME}」 作業執行失敗!\n{ex.Message}";
                            }

                            break;
                        //視窗功能
                        case "W":
                            this.Invoke(new MethodInvoker(() => { resultData = ExecuteForm(args); }));

                            break;

                        default:
                            break;
                    }

                    LOGSP_END_TIME = DateTime.Now;

                    if (resultData.Status == ResultStatus.Success)
                    {
                        LOGSP_MSG = "執行正常完成!";
                    }
                    else
                    {
                        LOGSP_MSG = "作業執行失敗!";

                        servicePrefix1.SaveLogsp(LOGSP_DATE, LOGSP_TXN_ID, LOGSP_SEQ_NO, LOGSP_TID, LOGSP_TID_NAME, LOGSP_BEGIN_TIME, LOGSP_END_TIME, LOGSP_MSG, OCF_TYPE);

                        //MessageDisplay.Error("序號" + LOGSP_SEQ_NO + "的" + LOGSP_TID + "," + LOGSP_MSG);
                        MessageDisplay.Error(resultData.returnString);

                        this.Invoke(new MethodInvoker(() => {
                            SplashScreenManager.CloseForm();
                            gv.SetRowCellValue(i, "ERR_MSG", LOGSP_MSG);
                        }));

                        return ResultStatus.Fail;
                    }

                    this.Invoke(new MethodInvoker(() => { gv.SetRowCellValue(i, "ERR_MSG", LOGSP_MSG); }));

                    servicePrefix1.SaveLogsp(LOGSP_DATE, LOGSP_TXN_ID, LOGSP_SEQ_NO, LOGSP_TID, LOGSP_TID_NAME, LOGSP_BEGIN_TIME, LOGSP_END_TIME, LOGSP_MSG, OCF_TYPE);
                    servicePrefix1.SaveLogs(LOGSP_DATE, TXF_TID, DateTime.Now, GlobalInfo.USER_ID, "執行完畢");

                    #endregion 開始執行

                    #region 執行特別的程式


                    this.Invoke(new MethodInvoker(() => { RunAfterEveryItem(args); }));

                    #endregion 執行特別的程式

                    //流程時間控制
                    DataTable dtJRF = servicePrefix1.ListJrf(_ProgramID, TXF_TID);
                    if (dtJRF.Rows.Count > 0)
                    {
                        string JRF_DO_TXN_ID = dtJRF.Rows[0]["JRF_DO_TXN_ID"].AsString();
                        string JRF_DO_JOB_TYPE = dtJRF.Rows[0]["JRF_DO_TXN_ID"].AsString();
                        string JRF_DO_SEQ_NO = dtJRF.Rows[0]["JRF_DO_TXN_ID"].AsString();
                        string JRF_SW_CODE = dtJRF.Rows[0]["JRF_DO_TXN_ID"].AsString();
                        servicePrefix1.UpdateJsw(JRF_DO_TXN_ID, JRF_DO_JOB_TYPE, JRF_DO_SEQ_NO, JRF_SW_CODE, OcfDate, DateTime.Now, GlobalInfo.USER_ID);
                    }
                }
                else
                {
                    // 沒勾選項目的話清空狀態
                    this.Invoke(new MethodInvoker(() => { gv.SetRowCellValue(i, "ERR_MSG", ""); }));
                }
                if (i == gv.RowCount - 1 && OCF_TYPE == "D")
                {
                    servicePrefix1.setCIOCF();
                }
            }


            //全部結束
            servicePrefix1.SetTXF1(" ", _ProgramID);

            this.Invoke(new MethodInvoker(() => { SplashScreenManager.CloseForm(); }));

            return ResultStatus.Success;
        }

        protected virtual ResultData ExecuteForm(PokeBall args)
        {
            ResultData resultData = new ResultData();
            var dllIndividual = Assembly.LoadFile(Application.ExecutablePath);
            string typeFormat = "{0}.FormUI.Prefix{1}.W{2}";
            string txnId = args.TXF_TID.Substring(2, 5);
            Type myType = dllIndividual.GetType(string.Format(typeFormat, Path.GetFileNameWithoutExtension(Application.ExecutablePath), txnId.Substring(0, 1), txnId));

            if (myType == null)
            {
                MessageDisplay.Error("無此程式");
            }

            object myObj = Activator.CreateInstance(myType, txnId, args.TXF_TID_NAME);

            FormParent formInstance = (FormParent)myObj;


            if (formInstance.BeforeOpen() == ResultStatus.Success)
            {
                formInstance.MdiParent = this.MdiParent;
                formInstance.StartPosition = FormStartPosition.Manual;
                formInstance.WindowState = FormWindowState.Maximized;
                formInstance.Show();
            }

            ExecuteFormBefore(formInstance,args);
            resultData.Status = formInstance.ProcessExport();
            formInstance.Close();

            return resultData;
        }

        protected virtual void ExecuteFormBefore(FormParent formInstance, PokeBall args)
        {

        }

        protected virtual ResultStatus RunAfterEveryItem(PokeBall args)
        {
            if (args.TXF_TID == "wf_CI_CIOPF")
            {
                DataTable dtTxemail = servicePrefix1.ListTxemail(_ProgramID, 1);

                if (dtTxemail.Rows.Count != 0)
                {
                    string TXEMAIL_SENDER = dtTxemail.Rows[0]["TXEMAIL_SENDER"].AsString();
                    string TXEMAIL_RECIPIENTS = dtTxemail.Rows[0]["TXEMAIL_RECIPIENTS"].AsString();
                    string TXEMAIL_CC = dtTxemail.Rows[0]["TXEMAIL_CC"].AsString();
                    string TXEMAIL_TITLE = dtTxemail.Rows[0]["TXEMAIL_TITLE"].AsString();
                    string TXEMAIL_TEXT = dtTxemail.Rows[0]["TXEMAIL_TEXT"].AsString();

                    string attachmentFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, _ProgramID + ".csv");

                    this.Invoke(new MethodInvoker(() => {
                        // 先把結果產生出csv檔，再用成附件寄出去
                        Retrieve();
                        
                        gvSpLog.OptionsPrint.ShowPrintExportProgress = false;
                        gvSpLog.ExportToCsv(attachmentFilePath);
                    }));
                    MailHelper.SendEmail(TXEMAIL_SENDER, TXEMAIL_RECIPIENTS, TXEMAIL_CC, TXEMAIL_TITLE, TXEMAIL_TEXT, attachmentFilePath);
                }
            }
            return ResultStatus.Success;
        }

        protected virtual string RunBeforeEveryItem(PokeBall args)
        {
            DataTable dt = servicePrefix1.CheckTXF2(_ProgramID, args.TXF_TID);
            if (dt.Rows.Count > 0)
            {
                string runTid = dt.Rows[0]["TXF1_TID"].AsString();
                do
                {
                    SingletonLogger.Instance.Info(GlobalInfo.USER_ID, _ProgramID, $"{args.TXF_TID} wait：{runTid}", "S");
                } while (args.TXF_TID == runTid);
            }
            SingletonLogger.Instance.Info(GlobalInfo.USER_ID, _ProgramID, $"{args.TXF_TID} nowait：{args.TXF_TID}", "S");

            return "";
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