using System.Collections.Generic;

namespace Utils.Entity.XML
{
    public class VersionInfo
    {
        public VersionInfo()
        {
        }
        
        /// <summary>
        /// 更新日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 檔案版本清單
        /// </summary>
        public List<FileInfo> FileList { get; set; }
    }
}