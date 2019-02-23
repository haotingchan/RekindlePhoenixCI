namespace Utils.Entity.XML
{
    public class Download
    {
        public Download()
        {
        }

        /// <summary>
        /// Download 設定檔檔名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Download 設定檔的檔案類型：XML、INI
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 讀取PBCODE資料庫
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// PBCODE.PBCODE_TYPE
        /// </summary>
        public string PBcodeType { get; set; }

        /// <summary>
        /// 應用程式啟動檔名
        /// </summary>
        public string StartFileName { get; set; }
    }
}