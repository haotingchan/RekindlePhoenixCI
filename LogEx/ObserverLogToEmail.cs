using System.Net.Mail;

namespace Log
{
    /// <summary>
    /// ±NLog¿…±H®ÏEmail
    /// </summary>
    public class ObserverLogToEmail : ILog
    {
        private string      _from;
        private string      _to;
        private string      _subject;
        private string      _body;
        private SmtpClient  _smtpClient;

        public ObserverLogToEmail(string from, string to, string subject, string body, SmtpClient smtpClient)
		{
            _from = from;
            _to = to;
            _subject = subject;
            _body = body;
            _smtpClient = smtpClient;
		}

        #region ILog Members

        /// <summary>
        /// Sends a log request via email.
        /// </summary>
        public void Log(object sender, LogEventArgs e)
        {
            string message = "[" + e.Date.ToString() + "] " + e.SeverityString + ": " + e.Message;

            _smtpClient.Send(_from, _to, _subject, _body);
        }

        #endregion
    }
}
