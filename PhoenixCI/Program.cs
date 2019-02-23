using ActionService;
using ActionService.DbDirect;
using BaseGround;
using BusinessObjects;
using BusinessObjects.Enums;
using CI;
using Common;
using Common.Config;
using DataObjects;
using Log;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CI {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            // 將App.config檔裡面DevExpress的設定讀進來
            DevExpress.XtraEditors.WindowsFormsSettings.LoadApplicationSettings();

            #region Log設定

            SingletonLogger.Instance.Severity = (LogSeverity)Enum.Parse(typeof(LogSeverity), SettingDragons.Instance.Setting.Log.LogSeverity, true);

            ILog log = new ObserverLogToDatabase();
            SingletonLogger.Instance.Attach(log);

            log = new ObserverLogToFile("Debug.txt");
            SingletonLogger.Instance.Attach(log);

            log = new ObserverLogToConsole();
            SingletonLogger.Instance.Attach(log);

            #endregion Log設定

            #region GlobalSetting

            SystemStatus.SystemType = SystemType.CI;

            ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
            GlobalDaoSetting.Set(connectionInfo);
            ServiceCommon serviceCommon = new ServiceCommon();

            BO_OCF boOCF = serviceCommon.GetOCF();
            //user info
            GlobalInfo.USER_ID = "I0001";
            GlobalInfo.USER_NAME = "菲魯特";
            GlobalInfo.USER_DPT_ID = "J";
            GlobalInfo.USER_DPT_NAME = "資訊規劃部";
            GlobalInfo.OCF_DATE = boOCF.OCF_DATE;
            GlobalInfo.OCF_NEXT_DATE = boOCF.OCF_NEXT_DATE;
            GlobalInfo.OCF_PREV_DATE = boOCF.OCF_PREV_DATE;

            string reportDirectoryPath = "";
            reportDirectoryPath = Path.Combine(Application.StartupPath, "Report", DateTime.Now.ToString("yyyyMMdd"));

            string excelTemplateDirectoryPath = "";
            excelTemplateDirectoryPath = Path.Combine(Application.StartupPath, "Excel_Template");

            string batchErrSPDirectoryPath = Path.Combine(Application.StartupPath, "ErrSP");

            Directory.CreateDirectory(reportDirectoryPath);
            GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH = reportDirectoryPath;

            Directory.CreateDirectory(excelTemplateDirectoryPath);
            GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH = excelTemplateDirectoryPath;

            Directory.CreateDirectory(batchErrSPDirectoryPath);
            GlobalInfo.DEFAULT_BATCH_ErrSP_DIRECTORY_PATH = batchErrSPDirectoryPath;

            #endregion GlobalSetting

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += new ThreadExceptionEventHandler(UIThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.Run(new FormMain());
            //Application.Run(new FormLogin());
        }

        private static void UIThreadException(object sender, ThreadExceptionEventArgs t) {
            MessageBox.Show(t.Exception.ToString(), "UIThreadException");
            Application.Exit();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            MessageBox.Show(e.ExceptionObject.ToString(), "CurrentDomain_UnhandledException");
            Application.Exit();
        }
    }
}