using System;

namespace Log
{
    /// <summary>
    /// 所有的Log紀錄都透過這個Class
    /// </summary>
    public sealed class SingletonLogger
    {
        #region 變數屬性事件

        /// <summary>
        /// The one and only Singleton Logger instance. 
        /// </summary>
        private static readonly SingletonLogger instance = new SingletonLogger();

        /// <summary>
        /// Delegate event handler that hooks up requests.
        /// </summary>
        public delegate void LogEventHandler(object sender, LogEventArgs e);

        /// <summary>
        /// The Log event.
        /// </summary>
        public event LogEventHandler Log;

        private LogSeverity _severity;

        /// <summary>
        /// Gets the instance of the singleton logger object.
        /// </summary>
        public static SingletonLogger Instance
        {
            get { return instance; }
        }

        // These booleans are used strictly to improve performance.
        private bool _isDebug;
        private bool _isInfo;
        private bool _isWarning;
        private bool _isError;
        private bool _isFatal;

        /// <summary>
        /// Gets and sets the severity level of logging activity.
        /// </summary>
        public LogSeverity Severity
        {
            get { return _severity; }
            set
            {
                _severity = value;

                // Set booleans to help improve performance
                int severity = (int)_severity;

                _isDebug = ((int)LogSeverity.Debug) >= severity ? true : false;
                _isInfo = ((int)LogSeverity.Info) >= severity ? true : false;
                _isWarning = ((int)LogSeverity.Warning) >= severity ? true : false;
                _isError = ((int)LogSeverity.Error) >= severity ? true : false;
                _isFatal = ((int)LogSeverity.Fatal) >= severity ? true : false;
            }
        }

        #endregion

        /// <summary>
        /// Private constructor. Initializes default severity to "Error".
        /// 這是Private的，因為是singleton
        /// </summary>
        private SingletonLogger()
        {
            // Default severity is Error level
            Severity = LogSeverity.Error;
        }

        #region 寫入Log的各種函式

        /// <summary>
        /// Log a message when severity level is "Debug" or higher.
        /// </summary>
        /// <param name="message">Log message</param>
        public void Debug(string message)
        {
            if (_isDebug)
                Debug(message, null);
        }

        /// <summary>
        /// Log a message when severity level is "Debug" or higher.
        /// </summary>
        /// <param name="message">Log message.</param>
        /// <param name="exception">Inner exception.</param>
        public void Debug(string message, Exception exception)
        {
            if (_isDebug)
                OnLog(new LogEventArgs(LogSeverity.Debug, message, exception, DateTime.Now));
        }

        /// <summary>
        /// Log a message when severity level is "Info" or higher.
        /// </summary>
        public void Info(string logf_user_id, string logf_txn_id, string logf_key_data, string logf_job_type)
        {
            if (_isInfo)
                OnLog(new LogEventArgs(LogSeverity.Info, logf_user_id, logf_txn_id, logf_key_data, logf_job_type));
        }

        /// <summary>
        /// Log a message when severity level is "Warning" or higher.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Warning(string message)
        {
            if (_isWarning)
                Warning(message, null);
        }

        /// <summary>
        /// Log a message when severity level is "Warning" or higher.
        /// </summary>
        /// <param name="message">Log message.</param>
        /// <param name="exception">Inner exception.</param>
        public void Warning(string message, Exception exception)
        {
            if (_isWarning)
                OnLog(new LogEventArgs(LogSeverity.Warning, message, exception, DateTime.Now));
        }

        /// <summary>
        /// Log a message when severity level is "Error" or higher.
        /// </summary>
        /// <param name="message">Log message</param>
        public void Error(string message)
        {
            if (_isError)
                Error(message, null);
        }

        /// <summary>
        /// Log a message when severity level is "Error" or higher.
        /// </summary>
        /// <param name="message">Log message.</param>
        /// <param name="exception">Inner exception.</param>
        public void Error(string message, Exception exception)
        {
            if (_isError)
                OnLog(new LogEventArgs(LogSeverity.Error, message, exception, DateTime.Now));
        }

        /// <summary>
        /// Log a message when severity level is "Fatal"
        /// </summary>
        /// <param name="message">Log message</param>
        public void Fatal(string message)
        {
            if (_isFatal)
                Fatal(message, null);
        }

        /// <summary>
        /// Log a message when severity level is "Fatal"
        /// </summary>
        /// <param name="message">Log message.</param>
        /// <param name="exception">Inner exception.</param>
        public void Fatal(string message, Exception exception)
        {
            if (_isFatal)
                OnLog(new LogEventArgs(LogSeverity.Fatal, message, exception, DateTime.Now));
        }

        #endregion

        /// <summary>
        /// Invoke the Log event.
        /// </summary>
        /// <param name="e">Log event parameters.</param>
        public void OnLog(LogEventArgs e)
        {
            if (Log != null)
            {
                Log(this, e);
            }
        }

        /// <summary>
        /// Attach a listening observer logging device to logger.
        /// </summary>
        /// <param name="observer">Observer (listening device).</param>
        public void Attach(ILog observer)
        {
            Log += observer.Log;
        }

        /// <summary>
        /// Detach a listening observer logging device from logger.
        /// </summary>
        /// <param name="observer">Observer (listening device).</param>
        public void Detach(ILog observer)
        {
            Log -= observer.Log;
        }
    }
}
