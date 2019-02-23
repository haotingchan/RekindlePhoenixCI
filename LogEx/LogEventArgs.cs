using System;

namespace Log
{
    /// <summary>
    /// Log事件用的參數
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        #region 屬性

        /// <summary>
        /// Gets and sets the log severity.
        /// </summary>        
        public LogSeverity Severity { get; private set; }

        /// <summary>
        /// Gets and sets the log message.
        /// </summary>        
        public string Message { get; private set; }

        /// <summary>
        /// Gets and sets the optional inner exception.
        /// </summary>        
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets and sets the log date and time.
        /// </summary>        
        public DateTime Date { get; private set; }

        /// <summary>
        /// Friendly string that represents the severity.
        /// </summary>
        public String SeverityString
        {
            get { return Severity.ToString("G"); }
        }

        public string LOGF_USER_ID { get; set; }

        public string LOGF_TXN_ID { get; set; }

        public string LOGF_KEY_DATA { get; set; }

        public string LOGF_JOB_TYPE { get; set; }

        #endregion

        public LogEventArgs(LogSeverity severity, string message, Exception exception, DateTime date)
        {
            Severity = severity;
            Message = message;
            Exception = exception;
            Date = date;
        }

        public LogEventArgs(LogSeverity severity, string logf_user_id, string logf_txn_id, string logf_key_data, string logf_job_type)
        {
            LOGF_USER_ID = logf_user_id;
            LOGF_TXN_ID = logf_txn_id;
            LOGF_KEY_DATA = logf_key_data;
            LOGF_JOB_TYPE = logf_job_type;

            Severity = severity;
            Message = logf_key_data;
            Date = DateTime.Now;
        }

        /// <summary>
        /// LogEventArgs as a string representation.
        /// </summary>
        /// <returns>String representation of the LogEventArgs.</returns>
        public override String ToString()
        {
            return "" + Date
                + " - " + SeverityString
                + " - " + Message
                + " - " + Exception.ToString();
        }
    }
}
