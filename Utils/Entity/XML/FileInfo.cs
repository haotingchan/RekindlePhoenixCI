namespace Utils.Entity.XML
{
    public class FileInfo
    {
        public FileInfo()
        {
        }

        public FileInfo(string name)
        {
            this.Name = name;
            this.Ver = "0";
        }

        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版號
        /// </summary>
        public string Ver { get; set; }
    }
}