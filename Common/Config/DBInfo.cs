using BusinessObjects;

namespace Common.Config
{
    public class DBInfo
    {
        public DBInfo()
        {
        }

        /// <summary>
        /// 連線名稱
        /// </summary>
        public string ConnectionName { get; set; }

        /// <summary>
        /// 預設資料庫
        /// </summary>
        public string InitialCatalog { get; set; }

        public ConnectionInfo GetConnectionInfo()
        {
            return SettingDragons.Instance.GetConnectionInfo(this);
        }
    }
}