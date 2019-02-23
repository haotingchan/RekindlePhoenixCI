
namespace Log
{
    /// <summary>
    /// 寫Log到輸出的Console視窗(在Visual Studio的=>檢視=>輸出)
    /// </summary>
    public class ObserverLogToConsole : ILog
    {
        public void Log(object sender, LogEventArgs e)
        {
            string message = "[" + e.Date.ToString() + "] " + e.SeverityString + ": " + e.Message;

            System.Diagnostics.Debugger.Log(0, null,  message + "\r\n\r\n");
        }
    }
}
