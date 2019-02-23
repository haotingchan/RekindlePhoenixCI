namespace Utils.Entity.XML
{
    public class Connection
    {
        public Connection()
        {
        }

        /// <summary>
        /// 資料連線名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 資料連線字串
        /// </summary>
        public string ConnectionString { get; set; }

        public string ProviderName { get; set; }
    }
}