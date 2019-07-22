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
      private static void Main(string[] args) {
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
#if DEBUG
         GlobalInfo.USER_ID = "I0001";
         GlobalInfo.USER_NAME = "菲魯特";
         GlobalInfo.USER_DPT_ID = "J";
         GlobalInfo.USER_DPT_NAME = "資訊規劃部";
#endif

         string reportDirectoryPath = "";

        if (GlobalDaoSetting.GetConnectionInfo.ConnectionName == "CIN2" || Application.StartupPath.IndexOf(@"C:\CI") < 0)
        {
            reportDirectoryPath = Path.Combine(Application.StartupPath, "Report", DateTime.Now.ToString("yyyyMMdd"));
        }
        else
        {
            reportDirectoryPath = Path.Combine(Application.StartupPath, "Excel_Report", DateTime.Now.ToString("yyyyMMdd"));
        }
         

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

         // 如果有傳參數進來
         if (args.Length != 0) {
            string userID = args[0];
            string userName = args[1];
            string txnID = args[2];
            string txnName = args[3];

            GlobalInfo.USER_ID = userID;
            GlobalInfo.USER_NAME = userName;
            Application.Run(new FormMain(txnID, txnName));
         } else {
#if DEBUG
            Application.Run(new FormMain());
#else
            Application.Run(new FormLogin());
#endif
         }
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