using System;
using System.Diagnostics;

namespace Log
{
    /// <summary>
    /// ¼gLog¨ìWindows event log.
    /// </summary>
    public class ObserverLogToEventlog : ILog
    {
        public void Log(object sender, LogEventArgs e)
        {
            string message = "[" + e.Date.ToString() + "] " + e.SeverityString + ": " + e.Message;

            var eventLog = new EventLog();
            eventLog.Source = "³î«}^.<Y";

            // Map severity level to an Windows EventLog entry type
            var type = EventLogEntryType.Error;
            if (e.Severity < LogSeverity.Warning)   type = EventLogEntryType.Information;
            if (e.Severity < LogSeverity.Error)     type = EventLogEntryType.Warning;

            try 
            { 
                eventLog.WriteEntry(message, type); 
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
