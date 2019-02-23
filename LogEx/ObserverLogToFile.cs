using System.IO;

namespace Log
{
    /// <summary>
    /// ¼gLog¨ìÀÉ®×¸Ì
    /// </summary>
    public class ObserverLogToFile : ILog
    {
        private string _fileName;

        private object syncObj = new object();

        public ObserverLogToFile(string fileName)
        {
            _fileName = fileName;
        }

        public void Log(object sender, LogEventArgs e)
        {
            string message = "[" + e.Date.ToString() + "] " + e.SeverityString + ": " + e.Message;
            
            FileStream fileStream;

            lock (syncObj)
            {
                try
                {
                    fileStream = new FileStream(_fileName, FileMode.Append);
                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory((new FileInfo(_fileName)).DirectoryName);
                    fileStream = new FileStream(_fileName, FileMode.Append);
                }

                var writer = new StreamWriter(fileStream);

                try
                {
                    writer.WriteLine(message);
                }
                catch { /* do nothing */}
                finally
                {
                    try
                    {
                        writer.Close();
                    }
                    catch { /* do nothing */}
                }
            }
        }
    }
}
