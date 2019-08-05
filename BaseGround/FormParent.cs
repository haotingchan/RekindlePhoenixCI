using ActionService.DbDirect;
using ActionServiceW.DbDirect.Prefix;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using Common.Config;
using Common.Helper;
using DataObjects;
using DataObjects.Dao;
using DataObjects.Dao.Together;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraVerticalGrid;
using Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace BaseGround {
   public partial class FormParent : DevExpress.XtraBars.Ribbon.RibbonForm {
      #region 變數區

      private string _DefaultFileNamePath;
      private string _ReportID;
      private string _ReportTitle;
      private string _ReportClass;
      private object _PrintableComponent;
      private DataTable _DataSource;

      protected string _ProgramID;
      protected string _ProgramName;

      protected BarButtonItem _ToolBtnInsert;
      protected BarButtonItem _ToolBtnSave;
      protected BarButtonItem _ToolBtnDel;
      protected BarButtonItem _ToolBtnRetrieve;
      protected BarButtonItem _ToolBtnRun;
      protected BarButtonItem _ToolBtnImport;
      protected BarButtonItem _ToolBtnExport;
      protected BarButtonItem _ToolBtnPrintAll;

      /// <summary>
      /// 防止接下來的Print事件觸發
      /// </summary>
      protected bool _IsPreventFlowPrint = true;

      /// <summary>
      /// 防止接下來的Export事件觸發
      /// </summary>
      protected bool _IsPreventFlowExport = true;

      /// <summary>
      /// 執行功能是否要非同步
      /// </summary>
      protected bool _IsProcessRunAsync = false;

      /// <summary>
      /// DB Owner
      /// </summary>
      protected string _DB_TYPE;

      protected ServiceCommon serviceCommon = new ServiceCommon();
      protected ServicePrefix1 servicePrefix1 = new ServicePrefix1();

      /// <summary>
      /// 將顯示元件(像是Grid)設定用來列印或匯出用
      /// </summary>
      public object PrintableComponent {
         get {
            return _PrintableComponent;
         }

         set {
            _PrintableComponent = value;
         }
      }

      /// <summary>
      /// 用來檢查資料是否修改過
      /// </summary>
      public DataTable DataSource {
         get {
            return _DataSource;
         }

         set {
            _DataSource = value;
         }
      }

      /// <summary>
      /// 用測試帳號登入,可看到一些隱藏UI,之後要調整
      /// </summary>
      public bool FlagAdmin {
         get {
            if (GlobalInfo.USER_ID == GlobalDaoSetting.GetConnectionInfo.ConnectionName)
               return true;
            else
               return false;
         }
      }

      private UserProgInfo _UserProgInfo;

      /// <summary>
      /// 紀錄著目前登錄的使用者和目前使用程式的代碼等資訊
      /// </summary>
      public UserProgInfo UserProgInfo {
         get {
            if (_UserProgInfo == null) {
               _UserProgInfo = new UserProgInfo();
               _UserProgInfo.UserID = GlobalInfo.USER_ID;
               _UserProgInfo.TxnID = _ProgramID;
            }

            return _UserProgInfo;
         }
      }

      #endregion

      public FormParent() {
         InitializeComponent();

         PaintFormBorder();
      }

      public FormParent(string program_id , string program_name) {
         InitializeComponent();

         PaintFormBorder();

         _ProgramID = program_id;
         _ProgramName = program_name;
         _ReportClass = "R" + _ProgramID;

         _DefaultFileNamePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , "CI_" + _ProgramID + "─" + _ProgramName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
         _ReportID = _ProgramID;
         _ReportTitle = _ProgramID + "─" + _ProgramName + GlobalInfo.REPORT_TITLE_MEMO;
      }

      private void FormParent_Load(object sender , EventArgs e) {
         Open();
      }

      private void FormParent_Shown(object sender , EventArgs e) {
         AfterOpen();

         FormParent_Activated(sender , e);
      }

      private void FormParent_Activated(object sender , EventArgs e) {
         // 當PB要直接呼叫.NET打開某隻程式時，會先觸發到Activated再觸發Load，所以要判斷 正常的點擊程式開啟是先觸發Load再觸發Activated
         if (_ToolBtnInsert != null) {
            SetAllToolBtnDisable();

            ActivatedForm();
         }
      }

      private void FormParent_FormClosing(object sender , FormClosingEventArgs e) {
         ResultStatus myResultStatus = ResultStatus.Fail;

         myResultStatus = BeforeClose();

         if (myResultStatus != ResultStatus.Success) {
            e.Cancel = true;
         }
      }

      public ResultStatus ProcessSaveFlow() {
         ResultStatus myResultStatus = ResultStatus.Fail;
         //serviceCommon = new ServiceCommon();
         try {
            myResultStatus = Save(new PokeBall());//直接跑各程式save func
                                                  //myResultStatus = CheckShield();

            //if (myResultStatus != ResultStatus.Success) return myResultStatus;

            //myResultStatus = serviceCommon.MultiActionTransaction(Save).Status;

            //if (myResultStatus != ResultStatus.Success) return myResultStatus;

            if (!_IsPreventFlowPrint) {
               ReportHelper reportHelperForPrint = PrintOrExportSetting();

               myResultStatus = Print(reportHelperForPrint);

               if (myResultStatus != ResultStatus.Success)
                  return myResultStatus;
            }

            if (!_IsPreventFlowExport) {
               ReportHelper reportHelperForExport = PrintOrExportSetting();

               myResultStatus = Export(reportHelperForExport);

               if (myResultStatus != ResultStatus.Success)
                  return myResultStatus;
            }

            if (myResultStatus == ResultStatus.Success) {
               COMPLETE();

               WriteLog("Save" , "Operation" , "I");//儲存成功log
            } else if (myResultStatus == ResultStatus.Fail) {
               //MessageDisplay.Error("儲存失敗");
               WriteLog("Save" , "Operation" , "I");
               Retrieve();
            }
            return myResultStatus;
         } catch (Exception ex) {
            WriteLog(ex);//各支程式之save error 由這裡紀錄log
            Retrieve();
            return ResultStatus.Fail;
         }
      }

      public void ProcessInsert() {
         InsertRow();
      }

      public void ProcessDelete() {
         DeleteRow();
      }

      public void ProcessRetrieve() {
         try {
            Retrieve();
         } catch (Exception ex) {
            WriteLog(ex);
         }
      }

      public void ProcessRun() {
         PokeBall args = new PokeBall();

         // 同步
         if (!_IsProcessRunAsync) {
            try {
               if (RunBefore(args) == ResultStatus.Success) {
                  if (Run(args) == ResultStatus.Success) {
                     RunAfter(args);
                  }
               }
            } catch (Exception ex) {
               WriteLog(ex);
            }
         }
         // 非同步
         else {
            if (RunBefore(args) == ResultStatus.Success) {
               AsyncHelper.DoWorkAsync(() => {
                  return Run(args);
               } ,
               (result) => {
                  if (result == ResultStatus.Success) {
                     this.BeginInvoke(new MethodInvoker(() => {
                        RunAfter(args);
                     }));
                  }
               } ,
               (exception) => {
                  MessageBox.Show(exception.Message);
               });
            }
         }
      }

      public void ProcessImport() {
         try {
            ResultStatus myResultStatus = Import();
            if (myResultStatus == ResultStatus.Success)
               MessageDisplay.Info(MessageDisplay.MSG_IMPORT);

            WriteLog("Import" , "Operation" , "I");//處理完成log
         } catch (Exception ex) {
            WriteLog(ex);//各匯入檔案程式error log
         }
      }

      public void ProcessExport() {
         string startTime = DateTime.Now.ToString("HH:mm:ss");

         try {
            if (CheckShield() == ResultStatus.Success) {
               WriteLog("Export" , "Operation" , "E");
               ResultStatus result = Export();

               if (result == ResultStatus.Success)
                  ExportAfter(startTime);
            } else {
               return;
            }
         } catch (Exception ex) {
            WriteLog(ex);
         }
      }

      public virtual void ProcessPrintAll(ReportHelper reportHelper) {
         reportHelper = PrintOrExportSetting();
         reportHelper.IsPrintedFromPrintButton = true;
         Print(reportHelper);
         MessageDisplay.Info(MessageDisplay.MSG_PRINT);

         //Export(reportHelper);會印空的, 要由各程式來做export pdf
      }

      public virtual ResultStatus BeforeOpen() {
         if (DesignMode)
            return ResultStatus.Success;

         return ResultStatus.Success;
      }

      protected virtual ResultStatus Open() {
         if (DesignMode)
            return ResultStatus.Success;

         if (MdiParent != null) {
            _ToolBtnInsert = ((FormMain)MdiParent).toolStripButtonInsert;
            _ToolBtnDel = ((FormMain)MdiParent).toolStripButtonDelete;
            _ToolBtnSave = ((FormMain)MdiParent).toolStripButtonSave;
            _ToolBtnRetrieve = ((FormMain)MdiParent).toolStripButtonRetrieve;
            _ToolBtnRun = ((FormMain)MdiParent).toolStripButtonRun;
            _ToolBtnImport = ((FormMain)MdiParent).toolStripButtonImport;
            _ToolBtnExport = ((FormMain)MdiParent).toolStripButtonExport;
            _ToolBtnPrintAll = ((FormMain)MdiParent).toolStripButtonPrintAll;
         }

         SingletonLogger.Instance.Info(GlobalInfo.USER_ID , _ProgramID , "OPEN" , " ");

         return ResultStatus.Success;
      }

      protected virtual ResultStatus AfterOpen() {
         if (DesignMode)
            return ResultStatus.Success;

         return ResultStatus.Success;
      }

      protected virtual ResultStatus ActivatedForm() {
         return ResultStatus.Success;
      }

      protected virtual ResultStatus CheckShield() {
         return ResultStatus.Success;
      }

      protected virtual ResultStatus CheckShield(Control control) {
         GridHelper.AcceptText(control);

         return ResultStatus.Success;
      }

      protected virtual ResultStatus Save(PokeBall args) {
         return ResultStatus.Success;
      }

      protected virtual ResultStatus Save(Control control) {
         GridHelper.AcceptText(control);

         return ResultStatus.Success;
      }

      protected virtual ResultStatus Save_Override(DataTable dt , string tableName , DBName dBName = DBName.CI) {
         DataGate DG = new DataGate();
         MessageDisplay.Info("Save_Override has been remove");
         return ResultStatus.Fail;
      }

      protected virtual ResultStatus Retrieve() {
         return ResultStatus.Success;
      }

      protected virtual ResultStatus Retrieve(Control control) {
         GridHelper.AcceptText(control);

         return ResultStatus.Success;
      }

      protected virtual ResultStatus RunBefore(PokeBall args) {
         GridView gv = (GridView)args.GridControlMain.MainView;
         string txfServer = gv.GetRowCellValue(1 , "TXF_SERVER").AsString();
         if (txfServer != GlobalDaoSetting.GetConnectionInfo.ConnectionName) {
            MessageDisplay.Warning("作業Server(" + txfServer + ") 不等於連線Server(" + GlobalDaoSetting.GetConnectionInfo.ConnectionName + ")");
            return ResultStatus.Fail;
         }

        if (args.OcfType == "D")
        {
            string inputDate = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
            string nowDate = DateTime.Now.ToString("yyyy/MM/dd");

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
        }
        else if (args.OcfType == "M") {
            string inputDate = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
            string nowDate = DateTime.Now.ToString("yyyy/MM");

            if (inputDate != nowDate)
            {
                if (MessageDisplay.Choose("月份(" + inputDate + ") 不等於本月(" + nowDate + ")，是否要繼續?") == DialogResult.No)
                {
                    return ResultStatus.Fail;
                }
            }
        }

         GridHelper.AcceptText(args.GridControlMain);

         return ResultStatus.Success;
      }

      protected virtual ResultStatus Run(PokeBall args) {
         return ResultStatus.Success;
      }

      protected virtual ResultStatus Run(GridControl gc) {
         GridHelper.AcceptText(gc);

         return ResultStatus.Success;
      }

      protected virtual ResultStatus RunAsync(PokeBall args) {
         this.Invoke(new MethodInvoker(() => {
            FormWait formWait = new FormWait();
            Point pointWait = new Point();

            int totalLeft = this.Parent.Left + this.Left;
            int totalTop = this.Parent.Top + this.Top;

            pointWait.X = totalLeft + (this.Width / 2) - (formWait.Width / 2);
            pointWait.Y = totalTop + (this.Height / 2) - (formWait.Height / 2);

            SplashScreenManager.ShowForm(this , typeof(FormWait) , true , true , SplashFormStartPosition.Manual , pointWait , ParentFormState.Locked);
         }));

         GridView gv = (GridView)args.GridControlMain.MainView;


        DataTable dtLOGSPForRuned = servicePrefix1.ListLogspForRunned(args.OcfDate, _ProgramID);
        DataView dvLOGSPForRuned = new DataView(dtLOGSPForRuned);



         servicePrefix1.SetTXF1(" " , _ProgramID);

         for (int i = 0 ; i < gv.RowCount ; i++) {
            string TXF_SERVER = gv.GetRowCellValue(i , "TXF_SERVER").AsString();
            string TXF_DB = gv.GetRowCellValue(i , "TXF_DB").AsString();
            string TXF_TXN_ID = gv.GetRowCellValue(i , "TXF_TXN_ID").AsString();
            int TXF_SEQ_NO = gv.GetRowCellValue(i , "TXF_SEQ_NO").AsInt();
            string TXF_TYPE = gv.GetRowCellValue(i , "TXF_TYPE").AsString();
            string TXF_TID = gv.GetRowCellValue(i , "TXF_TID").AsString();
            string TXF_TID_NAME = gv.GetRowCellValue(i , "TXF_TID_NAME").AsString();
            string TXF_DEFAULT = gv.GetRowCellValue(i , "TXF_DEFAULT").AsString();
            string TXF_REDO = gv.GetRowCellValue(i , "TXF_REDO").AsString();
            string TXF_ARG = gv.GetRowCellValue(i , "TXF_ARG").AsString();
            string TXF_PERIOD = gv.GetRowCellValue(i , "TXF_PERIOD").AsString();
            string TXF_SERVICE = gv.GetRowCellValue(i , "TXF_SERVICE").AsString();
            string TXF_FOLDER = gv.GetRowCellValue(i , "TXF_FOLDER").AsString();
            string TXF_AP_NAME = gv.GetRowCellValue(i, "TXF_AP_NAME").AsString();
            args.TXF_TID = TXF_TID;
            args.TXF_TID_NAME = TXF_TID_NAME;

            if (TXF_DEFAULT == "1") {
               DateTime LOGSP_DATE = args.OcfDate;
               string LOGSP_TXN_ID = _ProgramID;
               int LOGSP_SEQ_NO = TXF_SEQ_NO;
               string LOGSP_TID = TXF_TID;
               string LOGSP_TID_NAME = TXF_TID_NAME;
               DateTime LOGSP_BEGIN_TIME = new DateTime();
               DateTime LOGSP_END_TIME = new DateTime();
               string LOGSP_MSG = "";

                if (args.OcfType == "D")
                {
                    //判斷是否可重覆執行
                    if (TXF_REDO == "N")
                    {
                        dvLOGSPForRuned.RowFilter = "LOGSP_TID='" + TXF_TID + "' AND NOT ISNULL(LOGSP_BEGIN_TIME)";
                        if (dvLOGSPForRuned.Count != 0)
                        {
                            if (MessageDisplay.Choose(TXF_TID + " ★★★曾經執行過且不可重覆執行，是否強迫繼續執行 ?") == DialogResult.No)
                            {
                                return ResultStatus.Fail;
                            }
                        }
                    }
                }

               #region 開始執行
               LOGSP_BEGIN_TIME = DateTime.Now;

               string nextYmd = PbFunc.f_ocf_date(2 , _DB_TYPE);
               if (!string.IsNullOrEmpty(TXF_PERIOD)) {
                  switch (TXF_PERIOD) {
                     case "M"://月底執行
                        if (args.OcfDate.ToString("yyyyMM") == PbFunc.Left(nextYmd , 6)) {
                           LOGSP_MSG = "完成! (今日非月底，不需執行)";
                           this.Invoke(new MethodInvoker(() => { gv.SetRowCellValue(i , "TXF_DEFAULT" , 0); }));
                        }
                        break;
                     case "W"://週最後一天執行
                        if (Convert.ToInt32(args.OcfDate.DayOfWeek) < Convert.ToInt32(nextYmd.AsDateTime("yyyyMMdd").DayOfWeek)) {
                           LOGSP_MSG = "完成! (今日非本週最後1天，不需執行)";
                           this.Invoke(new MethodInvoker(() => { gv.SetRowCellValue(i , "TXF_DEFAULT" , 0); }));
                        }
                        break;
                     case "Y"://年底執行
                        if (args.OcfDate.ToString("yyyy") == PbFunc.Left(nextYmd , 4)) {
                           LOGSP_MSG = "完成! (今日非本年度最後1日，不需執行)";
                           this.Invoke(new MethodInvoker(() => { gv.SetRowCellValue(i , "TXF_DEFAULT" , 0); }));
                        }
                        break;

                  }
                  LOGSP_END_TIME = DateTime.Now;
                    if (args.OcfType == "D")
                    {
                        servicePrefix1.SaveLogsp(LOGSP_DATE, LOGSP_TXN_ID, LOGSP_SEQ_NO, LOGSP_TID, LOGSP_TID_NAME, LOGSP_BEGIN_TIME, LOGSP_END_TIME, LOGSP_MSG);
                    }
                    continue;
               }


               //開始前執行特別的Function
               string rtnText = RunBeforeEveryItem(args);
               if (!string.IsNullOrEmpty(rtnText)) {
                        if (PbFunc.Left(rtnText, 4) == "不需執行")
                        {
                            LOGSP_MSG = "完成! (" + rtnText + ")";
                            gv.SetRowCellValue(i, "ERR_MSG", LOGSP_MSG);
                            gv.SetRowCellValue(i, "TXF_DEFAULT", 0);
                            LOGSP_END_TIME = DateTime.Now;
                            if (args.OcfType == "D")
                            {
                                servicePrefix1.SaveLogsp(LOGSP_DATE, LOGSP_TXN_ID, LOGSP_SEQ_NO, LOGSP_TID, LOGSP_TID_NAME, LOGSP_BEGIN_TIME, LOGSP_END_TIME, LOGSP_MSG);
                            }
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
               servicePrefix1.SetTXF1(TXF_TID , _ProgramID);
                if (args.OcfType == "D")
                {
                    servicePrefix1.SaveLogs(LOGSP_DATE, TXF_TID, DateTime.Now, GlobalInfo.USER_ID, "開始執行");
                }
               ResultData resultData = new ResultData();
               string fileName = "";
               switch (TXF_TYPE) {
                  case "I":
                     fileName = $@"{GlobalInfo.DEFAULT_BATCH_ErrSP_DIRECTORY_PATH}\{TXF_SERVER}_{TXF_TXN_ID}_{TXF_SEQ_NO}_infor";
                     resultData = serviceCommon.ExecuteInfoWorkFlow(TXF_TID , UserProgInfo, TXF_FOLDER, TXF_SERVICE, TXF_AP_NAME,fileName);
                     break;

                  case "S":
                     List<DbParameterEx> listParams = null;

                     // 如果這個SP有參數的話
                     if (TXF_ARG == "Y") {
                        DataTable dtTXFPARM = serviceCommon.ListTXFPARM(TXF_SERVER , TXF_DB , TXF_TXN_ID , TXF_TID);

                        if (dtTXFPARM.Rows.Count > 0) {
                           listParams = new List<DbParameterEx>();
                        }

                        foreach (DataRow row in dtTXFPARM.Rows) {
                           string TXFPARM_ARG = row["TXFPARM_ARG"].AsString();
                           string TXFPARM_ARG_TYPE = row["TXFPARM_ARG_TYPE"].AsString();
                           string TXFPARM_DEFAULT = row["TXFPARM_DEFAULT"].AsString();

                           DbParameterEx paramEx;

                           switch (TXFPARM_ARG) {
                              case "":
                                 paramEx = new DbParameterEx("" , TXFPARM_DEFAULT);
                                 listParams.Add(paramEx);
                                 break;

                              case "em_ymd":
                                 paramEx = new DbParameterEx("" , args.OcfDate.ToString("yyyyMMdd"));
                                 listParams.Add(paramEx);
                                 break;

                              case "em_ym":
                                 paramEx = new DbParameterEx("" , args.OcfDate.ToString("yyyyMM"));
                                 listParams.Add(paramEx);
                                 break;

                              case "em_date":
                                 paramEx = new DbParameterEx();
                                 paramEx.DbType = DbTypeEx.Date;
                                 paramEx.Name = "";
                                 paramEx.Value = args.OcfDate;
                                 listParams.Add(paramEx);
                                 break;

                              default:
                                 break;
                           }
                        }
                     }

                     ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(TXF_DB);

                     try {
                        resultData = serviceCommon.ExecuteStoredProcedure(connectionInfo , string.Format("{0}.{1}" , TXF_DB , TXF_TID) , listParams , true);
                     } catch (Exception ex) {
                        resultData.Status = ResultStatus.Fail;
                        MessageDisplay.Error(ex.Message);
                     }

                     break;

                  case "W":
                     this.Invoke(new MethodInvoker(() => { resultData.Status = ExecuteForm(args); }));

                     break;

                  default:
                     break;
               }

               LOGSP_END_TIME = DateTime.Now;

               if (resultData.Status == ResultStatus.Success) {
                  LOGSP_MSG = "執行正常完成!";
               } else {
                  LOGSP_MSG = "作業執行失敗!";
                    if (args.OcfType == "D")
                    {
                        servicePrefix1.SaveLogsp(LOGSP_DATE, LOGSP_TXN_ID, LOGSP_SEQ_NO, LOGSP_TID, LOGSP_TID_NAME, LOGSP_BEGIN_TIME, LOGSP_END_TIME, LOGSP_MSG);
                    }
                  MessageDisplay.Error("序號" + LOGSP_SEQ_NO + "的" + LOGSP_TID + "," + LOGSP_MSG);
                  
                  this.Invoke(new MethodInvoker(() => {
                     SplashScreenManager.CloseForm();
                     gv.SetRowCellValue(i , "ERR_MSG" , LOGSP_MSG);
                  }));

                  return ResultStatus.Fail;
               }


               this.Invoke(new MethodInvoker(() => { gv.SetRowCellValue(i , "ERR_MSG" , LOGSP_MSG); }));
                if (args.OcfType == "D")
                {
                    servicePrefix1.SaveLogsp(LOGSP_DATE, LOGSP_TXN_ID, LOGSP_SEQ_NO, LOGSP_TID, LOGSP_TID_NAME, LOGSP_BEGIN_TIME, LOGSP_END_TIME, LOGSP_MSG);
                    servicePrefix1.SaveLogs(LOGSP_DATE, TXF_TID, DateTime.Now, GlobalInfo.USER_ID, "執行完畢");
                }
               #endregion 開始執行

               #region 執行特別的程式


               this.Invoke(new MethodInvoker(() => { RunAfterEveryItem(args); }));

               #endregion 執行特別的程式

               //流程時間控制
               DataTable dtJRF = servicePrefix1.ListJrf(_ProgramID , args.TXF_TID);
               if (dtJRF.Rows.Count > 0) {
                  string JRF_DO_TXN_ID = dtJRF.Rows[0]["JRF_DO_TXN_ID"].AsString();
                  string JRF_DO_JOB_TYPE = dtJRF.Rows[0]["JRF_DO_TXN_ID"].AsString();
                  string JRF_DO_SEQ_NO = dtJRF.Rows[0]["JRF_DO_TXN_ID"].AsString();
                  string JRF_SW_CODE = dtJRF.Rows[0]["JRF_DO_TXN_ID"].AsString();
                  servicePrefix1.UpdateJsw(JRF_DO_TXN_ID , JRF_DO_JOB_TYPE , JRF_DO_SEQ_NO , JRF_SW_CODE , args.OcfDate , DateTime.Now , GlobalInfo.USER_ID);
               }
            } else {
               // 沒勾選項目的話清空狀態
               this.Invoke(new MethodInvoker(() => { gv.SetRowCellValue(i , "ERR_MSG" , ""); }));
            }
            if (i == gv.RowCount -1)
            {
                    servicePrefix1.setOCF();
            }
         }


         //全部結束
         servicePrefix1.SetTXF1(" " , _ProgramID);

         this.Invoke(new MethodInvoker(() => { SplashScreenManager.CloseForm(); }));

         return ResultStatus.Success;
      }

      protected virtual ResultStatus ExecuteForm(PokeBall args) {
         var dllIndividual = Assembly.LoadFile(Application.ExecutablePath);
         string typeFormat = "{0}.FormUI.Prefix{1}.W{2}";
         string txnId = args.TXF_TID.Substring(2 , 5);
         Type myType = dllIndividual.GetType(string.Format(typeFormat , Path.GetFileNameWithoutExtension(Application.ExecutablePath) , txnId.Substring(0 , 1) , txnId));

         if (myType == null) {
            MessageDisplay.Error("無此程式");
         }

         object myObj = Activator.CreateInstance(myType , txnId , args.TXF_TID_NAME);

         FormParent formInstance = (FormParent)myObj;


         if (formInstance.BeforeOpen() == ResultStatus.Success) {
            formInstance.MdiParent = this.MdiParent;
            formInstance.StartPosition = FormStartPosition.Manual;
            formInstance.WindowState = FormWindowState.Maximized;
            formInstance.Show();
         }
         return ResultStatus.Success;
      }

      protected virtual ResultStatus RunAfterEveryItem(PokeBall args) {
         if (args.TXF_TID == "wf_CI_CIOPF") {
            DataTable dtTxemail = servicePrefix1.ListTxemail(_ProgramID , 1);

            if (dtTxemail.Rows.Count != 0) {
               string TXEMAIL_SENDER = dtTxemail.Rows[0]["TXEMAIL_SENDER"].AsString();
               string TXEMAIL_RECIPIENTS = dtTxemail.Rows[0]["TXEMAIL_RECIPIENTS"].AsString();
               string TXEMAIL_CC = dtTxemail.Rows[0]["TXEMAIL_CC"].AsString();
               string TXEMAIL_TITLE = dtTxemail.Rows[0]["TXEMAIL_TITLE"].AsString();
               string TXEMAIL_TEXT = dtTxemail.Rows[0]["TXEMAIL_TEXT"].AsString();

               string attachmentFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , _ProgramID + ".csv");

               this.Invoke(new MethodInvoker(() => {
                  // 先把結果產生出csv檔，再用成附件寄出去
                  Retrieve();
                  args.GridControlSecond.MainView.OptionsPrint.ShowPrintExportProgress = false;
                  args.GridControlSecond.ExportToCsv(attachmentFilePath);
               }));
               MailHelper.SendEmail(TXEMAIL_SENDER , TXEMAIL_RECIPIENTS , TXEMAIL_CC , TXEMAIL_TITLE , TXEMAIL_TEXT , attachmentFilePath);
            }
         }
         return ResultStatus.Success;
      }

      protected virtual string RunBeforeEveryItem(PokeBall args) {
         DataTable dt = servicePrefix1.CheckTXF2(_ProgramID , args.TXF_TID);
         if (dt.Rows.Count > 0) {
            string runTid = dt.Rows[0]["TXF1_TID"].AsString();
            do {
               SingletonLogger.Instance.Info(GlobalInfo.USER_ID , _ProgramID , $"{args.TXF_TID} wait：{runTid}" , "S");
            } while (args.TXF_TID == runTid);
         }
         SingletonLogger.Instance.Info(GlobalInfo.USER_ID , _ProgramID , $"{args.TXF_TID} nowait：{args.TXF_TID}" , "S");

         return "";
      }

      protected virtual ResultStatus RunAfter(PokeBall args) {
         MessageDisplay.Normal("執行完畢");

         return ResultStatus.Success;
      }

      protected virtual ResultStatus Import() {
         return ResultStatus.Success;
      }

      protected virtual ResultStatus Import(Control control) {
         GridHelper.AcceptText(control);

         return ResultStatus.Success;
      }

      protected virtual ResultStatus Export() {
         return ResultStatus.Success;
      }

      protected virtual ResultStatus Export(Control control) {
         GridHelper.AcceptText(control);

         return ResultStatus.Success;
      }

      protected virtual ResultStatus Export(ReportHelper reportHelper) {
         //ShowFormWait("轉出中...");
         reportHelper.Export(reportHelper.FileType , reportHelper.FilePath);
         //CloseFormWait();

         return ResultStatus.Success;
      }

      protected virtual ResultStatus ExportAfter(string startTime) {
         string finishTime = DateTime.Now.ToString("HH:mm:ss");

         MessageDisplay.Info("轉檔完成!" , "處理結果" + " " + startTime + " ~ " + finishTime);

         return ResultStatus.Success;
      }

      public virtual ReportHelper PrintOrExportSetting() {
         ReportHelper reportHelper = new ReportHelper(PrintableComponent , _ReportID , _ReportTitle);
         reportHelper.FilePath = _DefaultFileNamePath;
         reportHelper.FileType = FileType.PDF;

         return reportHelper;
      }

      protected virtual ResultStatus Print(ReportHelper reportHelper) {
         //ShowFormWait("列印中...");
         reportHelper.Print();
         //CloseFormWait();

         return ResultStatus.Success;
      }

      protected virtual ResultStatus COMPLETE() {
         MessageDisplay.Info(MessageDisplay.MSG_OK);

         Retrieve();
         //this.Close();

         return ResultStatus.Success;
      }

      protected virtual ResultStatus InsertRow() {
         return ResultStatus.Success;
      }

      protected virtual ResultStatus InsertRow(object gridObject) {
         if (gridObject != null) {
            if (gridObject is GridView) {
               GridView gv = (GridView)gridObject;

               gv.CloseEditor();
               gv.AddNewRow();
               //gv.UpdateCurrentRow();
            }
         }

         return ResultStatus.Success;
      }

      protected virtual ResultStatus DeleteRow() {
         return ResultStatus.Success;
      }

      protected virtual ResultStatus DeleteRow(object gridObject) {
         if (gridObject != null) {
            if (gridObject is GridView) {
               GridView gv = (GridView)gridObject;
               gv.UpdateCurrentRow();
               gv.CloseEditor();
               if (!ConfirmToDelete(gv.FocusedRowHandle + 1)) { return ResultStatus.Fail; }

               gv.DeleteSelectedRows();
            }
         }

         return ResultStatus.Success;
      }

      protected virtual ResultStatus BeforeClose() {
         if (DataSource == null) {
            return ResultStatus.Success;
         }

         DialogResult myDialogResult = ConfirmToExitWithoutSave(DataSource);

         if (myDialogResult == DialogResult.Yes) { return ProcessSaveFlow(); } else if (myDialogResult == DialogResult.No) { return ResultStatus.Success; } else if (myDialogResult == DialogResult.Cancel) { return ResultStatus.Fail; }

         return ResultStatus.Success;
      }

      protected bool ConfirmToDelete(int rowNum) {
         if (MessageDisplay.Choose("確定刪除第" + rowNum + "筆資料?") == DialogResult.Yes) {
            return true;
         } else {
            return false;
         }
      }

      protected DialogResult ConfirmToExitWithoutSave(DataTable dt) {
         DialogResult myDialogResult = DialogResult.No;

         if (dt.GetChanges() != null && dt.GetChanges().Rows.Count != 0) {
            foreach (DataRow row in dt.GetChanges().Rows) {
               if (row.RowState == DataRowState.Deleted) {
                  myDialogResult = MessageDisplay.Choose("未完成存檔，請問是否要離開作業?");
                  return myDialogResult;
               }

               foreach (object item in row.ItemArray) {
                  if ((item is string && string.IsNullOrEmpty(item.AsString())) || item is DBNull) {
                     // 代表都沒有修改資料
                  } else {
                     myDialogResult = MessageDisplay.Choose("未完成存檔，請問是否要離開作業?");
                     return myDialogResult;
                  }
               }
            }
         }

         return myDialogResult;
      }

      protected bool IsDataModify(Object grid) {
         DataTable dt = null;
         if (grid is GridControl) {
            GridControl gridControl = ((GridControl)grid);
            gridControl.MainView.CloseEditor();
            gridControl.MainView.UpdateCurrentRow();
            dt = (DataTable)gridControl.DataSource;
         } else if (grid is VGridControl) {
            VGridControl gridControl = ((VGridControl)grid);
            gridControl.CloseEditor();
            gridControl.UpdateFocusedRecord();
            dt = (DataTable)gridControl.DataSource;
         }

         if (dt == null || dt.GetChanges() == null || dt.GetChanges().Rows.Count == 0) {
            MessageDisplay.Error(MessageDisplay.MSG_NO_DATA_FOR_MODIFY);
            return false;
         }

         return true;
      }

      /// <summary>
      /// 將新增、刪除、變更的紀錄分別都列印或匯出出來 改用PrintOrExportChangedByKen
      /// </summary>
      protected void PrintOrExportChanged(GridControl gridControl , ResultData resultData) {
         GridControl gridControlPrint = GridHelper.CloneGrid(gridControl);

         ReportHelper reportHelper = new ReportHelper(gridControl , _ProgramID , _ReportTitle);

         if (resultData.ChangedDataViewForAdded.Count != 0) {
            gridControlPrint.DataSource = resultData.ChangedDataViewForAdded;
            reportHelper.PrintableComponent = gridControlPrint;
            reportHelper.ReportTitle = _ReportTitle + "─" + "新增";

            reportHelper.Print();
            reportHelper.Export(FileType.PDF , reportHelper.FilePath);
         }

         if (resultData.ChangedDataViewForDeleted.Rows.Count != 0) {
            DataTable dtTemp = resultData.ChangedDataViewForDeleted.Clone();

            int rowIndex = 0;
            foreach (DataRow dr in resultData.ChangedDataViewForDeleted.Rows) {
               for (int colIndex = 0 ; colIndex < resultData.ChangedDataViewForDeleted.Columns.Count ; colIndex++) {
                  dtTemp.Rows[rowIndex][colIndex] = dr[colIndex , DataRowVersion.Original];
               }
               rowIndex++;
            }

            gridControlPrint.DataSource = dtTemp.AsDataView();
            reportHelper.PrintableComponent = gridControlPrint;
            reportHelper.ReportTitle = _ReportTitle + "─" + "刪除";

            reportHelper.Print();
            reportHelper.Export(FileType.PDF , reportHelper.FilePath);
         }

         if (resultData.ChangedDataViewForModified.Count != 0) {
            gridControlPrint.DataSource = resultData.ChangedDataViewForModified;
            reportHelper.PrintableComponent = gridControlPrint;
            reportHelper.ReportTitle = _ReportTitle + "─" + "變更";

            reportHelper.Print();
            reportHelper.Export(FileType.PDF , reportHelper.FilePath);
         }
      }

      /// <summary>
      /// 將新增、刪除、變更的紀錄分別都列印或匯出出來
      /// </summary>
      /// <param name="gridControl"></param>
      /// <param name="ChangedForAdded"></param>
      /// <param name="ChangedForDeleted"></param>
      /// <param name="ChangedForModified"></param>
      protected void PrintOrExportChangedByKen(GridControl gridControl , DataTable ChangedForAdded ,
          DataTable ChangedForDeleted , DataTable ChangedForModified , bool IsHandlePersonVisible = true , bool IsManagerVisible = true) {
         GridControl gridControlPrint = GridHelper.CloneGrid(gridControl);

         ReportHelper reportHelper = new ReportHelper(gridControl , _ProgramID , _ReportTitle);
         reportHelper.IsHandlePersonVisible = IsHandlePersonVisible;
         reportHelper.IsManagerVisible = IsManagerVisible;

         if (ChangedForAdded != null)
            if (ChangedForAdded.Rows.Count != 0) {
               gridControlPrint.DataSource = ChangedForAdded;
               reportHelper.PrintableComponent = gridControlPrint;
               reportHelper.ReportTitle = _ReportTitle + "─" + "新增";

               reportHelper.Print();
               reportHelper.Export(FileType.PDF , reportHelper.FilePath);
            }

         if (ChangedForDeleted != null)
            if (ChangedForDeleted.Rows.Count != 0) {
               DataTable dtTemp = ChangedForDeleted.Clone();

               int rowIndex = 0;
               foreach (DataRow dr in ChangedForDeleted.Rows) {
                  DataRow drNewDelete = dtTemp.NewRow();
                  for (int colIndex = 0 ; colIndex < ChangedForDeleted.Columns.Count ; colIndex++) {
                     drNewDelete[colIndex] = dr[colIndex , DataRowVersion.Original];
                  }
                  dtTemp.Rows.Add(drNewDelete);
                  rowIndex++;
               }

               gridControlPrint.DataSource = dtTemp.AsDataView();
               reportHelper.PrintableComponent = gridControlPrint;
               reportHelper.ReportTitle = _ReportTitle + "─" + "刪除";

               reportHelper.Print();
               reportHelper.Export(FileType.PDF , reportHelper.FilePath);
            }

         if (ChangedForModified != null)
            if (ChangedForModified.Rows.Count != 0) {
               gridControlPrint.DataSource = ChangedForModified;
               reportHelper.PrintableComponent = gridControlPrint;
               reportHelper.ReportTitle = _ReportTitle + "─" + "變更";

               reportHelper.Print();
               reportHelper.Export(FileType.PDF , reportHelper.FilePath);
            }

         MessageDisplay.Info(MessageDisplay.MSG_PRINT , GlobalInfo.ResultText);
      }

      /// <summary>
      /// 即將廢除, 改用PBFunc wf_copy_file
      /// </summary>
      /// <param name="fileName"></param>
      /// <param name="fileType"></param>
      /// <returns></returns>
      protected string CopyExcelTemplateFile(string fileName , FileType fileType) {
         string originalFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH , fileName + "." + fileType.ToString().ToLower());

         string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
             fileName + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH.mm.ss") + "." + fileType.ToString().ToLower());

         File.Copy(originalFilePath , destinationFilePath , true);

         return destinationFilePath;
      }

      private void SetAllToolBtnDisable() {
         _ToolBtnInsert.Enabled = false;
         _ToolBtnDel.Enabled = false;
         _ToolBtnSave.Enabled = false;
         _ToolBtnRetrieve.Enabled = false;
         _ToolBtnRun.Enabled = false;
         _ToolBtnImport.Enabled = false;
         _ToolBtnExport.Enabled = false;
         _ToolBtnPrintAll.Enabled = false;
      }

      private void FormParent_KeyDown(object sender , KeyEventArgs e) {
         if (e.KeyCode == Keys.Escape) {
            this.Close();
         }
      }

      public void PaintFormBorder() {
         SkinElement element = SkinManager.GetSkinElement(SkinProductId.Ribbon , DevExpress.LookAndFeel.UserLookAndFeel.Default , "FormCaptionNoRibbon");
         Image image = element.Image.GetImages().Images[1];
         int counter = element.Image.ImageCount;
         Bitmap bmp = new Bitmap(image.Width , image.Height * 2);

         using (Graphics graphics = Graphics.FromImage(bmp)) {
            int y = 0;
            while (counter-- > 0) {
               graphics.DrawImage(image , new Rectangle(0 , y , image.Width , image.Height));
               graphics.DrawRectangle(Pens.DarkSlateGray , 0 , y , image.Width - 1 , image.Height);
               y += image.Height;
            }
         }
         element.SetActualImage(bmp , true);
         LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
      }

      private void FormParent_SizeChanged(object sender , EventArgs e) {
         if (MdiParent != null) {
            if (this.WindowState == FormWindowState.Maximized) {
               ((FormMain)MdiParent).standaloneBarDockControlMdi.Visible = true;
            } else {
               ((FormMain)MdiParent).standaloneBarDockControlMdi.Visible = false;
            }
         }
      }

      /// <summary>
      /// 發生錯誤時,寫log to db (logType=Error) 順便截圖和顯示錯誤訊息
      /// </summary>
      /// <param name="ex">最後儲存的長度為100字元</param>
      /// <param name="extraMsg">額外的資訊,顯示在最前面</param>
      /// <param name="showMsg">true=顯示錯誤訊息,false=不顯示</param>
      public void WriteLog(Exception ex , string extraMsg = "" , bool showMsg = true) {
         //1.一旦發生非預期的Error(有產生Exception),截取目前畫面並儲存在local端的log/yyyyMM
         SaveErrorScreenshot();

         //2.write log to db
         string logType = "Error";
         WriteLog(extraMsg + Environment.NewLine + ex.ToString() , logType , "Z" , false);//ken,如果要發送訊息,在本身的函數內處理,比較詳盡

         //3.show message to UI
         if (showMsg) {
            string tmp = "";
            MethodBase site = ex.TargetSite;
            tmp = "發生錯誤";
            tmp += (extraMsg == "" ? "" : "(" + extraMsg + ")");
            tmp += (site == null ? "" : Environment.NewLine + "Error Function=" + site.Name);
            if (FlagAdmin)
               tmp += Environment.NewLine + ex.ToString().SubStr(0 , 1000);//Msg太多字數不好,要看詳細直接去檔案看
            MessageDisplay.Error(tmp);
         }//if (showMsg) {
      }

      /// <summary>
      /// 寫log to db (當logType=Error或資料庫連線失敗,會額外將錯誤訊息寫到檔案)
      /// </summary>
      /// <param name="msg">最後儲存的長度為100字元</param>
      /// <param name="logType">基本logType可定義為 Info/Operation/Error</param>
      /// <param name="operationType">
      /// logType=Info,此參數才有效(I = change data, E = export, R = query, P = print, X = execute)
      /// </param>
      /// <param name="showMsg">true=顯示錯誤訊息,false=不顯示</param>
      public void WriteLog(string msg , string logType = "Info" , string operationType = " " , bool showMsg = false , bool isNeedWriteFile = false) {
         //bool isNeedWriteFile = false;
         string dbErrorMsg = "";

         //1.write log to db
         //ken,先把WriteLog集中,之後可根據不同的logType,存放不同的TABLE或檔案
         //基本logType可定義為 info/operation/error
         //logf_job_type value: I = change data, E = export, R = query, P = print, X = execute
         try {
            switch (logType) {
               case ("Info"):
                  operationType = "A";
                  break;

               case ("Error"):
                  operationType = "Z";
                  isNeedWriteFile = true;
                  break;
            }
            //LOGF_KEY_DATA長度要取前100字元,但是logf.LOGF_KEY_DATA型態為VARCHAR2 (100 Byte),如果有中文會算2byte...
            string msgC = "[C#]" + msg;
            new LOGF().Insert(GlobalInfo.USER_ID , _ProgramID , msgC.SubStr(0 , 50) , operationType);
         } catch (Exception ex2) {
            // write log to db failed , ready write file to local
            isNeedWriteFile = true;
            dbErrorMsg = ex2.ToString();
            MessageDisplay.Error("資料庫連線發生錯誤,先將錯誤訊息寫到檔案");
         }//try {
          //2.write file to local
         if (isNeedWriteFile) {
            try {
               string filename = "log_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
               string filepath = Path.Combine(Application.StartupPath , "Log" , DateTime.Today.ToString("yyyyMM"));
               Directory.CreateDirectory(filepath);
               filepath = Path.Combine(filepath , filename);
               using (StreamWriter sw = File.AppendText(filepath)) {
                  sw.WriteLine("");
                  sw.WriteLine("");
                  sw.WriteLine("==============================");
                  sw.WriteLine("datetime=" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                  sw.WriteLine("userId=" + GlobalInfo.USER_ID);
                  sw.WriteLine("txnId=" + _ProgramID);
                  sw.WriteLine("logType=" + logType);
                  sw.WriteLine("operationType=" + operationType);
                  sw.Write("msg=" + msg);
                  sw.WriteLine("");
                  if (dbErrorMsg != "")
                     sw.Write("dbErrorMsg=" + dbErrorMsg);
               }//using (StreamWriter sw = File.AppendText(filepath)) {
            } catch (Exception fileEx) {
               MessageDisplay.Error("將log寫入檔案發生錯誤,請聯絡管理員" + Environment.NewLine + "msg=" + fileEx.Message);
               return;
            }
         }//if (isNeedWriteFile) {
          //3.show message to UI (ken,這裡主要處理一般的訊息,Error的錯誤訊息由另外傳入Exception參數的那個來發比較詳盡)
         if (showMsg) {
            switch (logType) {
               case ("Operation"):
                  MessageDisplay.Normal(msg);
                  break;

               case ("Info"):
                  MessageDisplay.Warning(msg);
                  break;

               case ("Error"):
                  MessageDisplay.Error(msg);
                  break;
            }
         }//if (showMsg) {
      }

      /// <summary>
      /// 顯示loding訊息
      /// </summary>
      /// <param name="setCaption">訊息標題</param>
      /// <param name="setContent">訊息內容</param>
      public void ShowFormWait(string setCaption = "處理中" , string setContent = "") {
         //Open Wait Form
         SplashScreenManager.ShowForm(this , typeof(FormWait) , true , true , false);

         //The Wait Form is opened in a separate thread. To change its Description, use the SetWaitFormDescription method.
         SplashScreenManager.Default.SetWaitFormCaption(setCaption);
         SplashScreenManager.Default.SetWaitFormDescription(setContent);

         this.Refresh();
         Thread.Sleep(25);
      }

      /// <summary>
      /// 關閉loding訊息
      /// </summary>
      public void CloseFormWait() {
         //Close Wait Form
         SplashScreenManager.CloseForm(false);
      }

      /// <summary>
      /// 抓取畫面存檔成png
      /// </summary>
      public void SaveErrorScreenshot() {
         try {
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + _ProgramID + ".png";
            string filepath = Path.Combine(Application.StartupPath , "Log" , DateTime.Today.ToString("yyyyMM"));
            Directory.CreateDirectory(filepath);
            filepath = Path.Combine(filepath , filename);

            //ken,只抓此應用程式的顯示範圍,但沒有強制focus此應用程式,所以如果user自己把畫面切走,會抓錯

            //ken,想單純抓該應用程式大小的畫面,實際很難抓,所以抓整個畫面
            ImageFormat format = ImageFormat.Png;
            Rectangle bounds = Screen.PrimaryScreen.Bounds;

            using (Bitmap bitmap = new Bitmap(bounds.Width , bounds.Height)) {
               using (Graphics g = Graphics.FromImage(bitmap)) {
                  g.CopyFromScreen(new Point(0 , 0) , Point.Empty , bounds.Size);
               }

               bitmap.Save(filepath , format);
            }
         } catch {
            //if error,no response
         }
      }
   }
}