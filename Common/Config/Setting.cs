using System.Collections.Generic;

namespace Common.Config
{
    public class Setting
    {
        public Setting()
        {
        }

        /// <summary>
        /// 系統名稱
        /// </summary>
        public string System { get; set; }

        /// <summary>
        /// 資料庫名稱
        /// </summary>
        public Database Database { get; set; }

        /// <summary>
        ///  資料庫連線
        /// </summary>
        public List<Connection> ConnectionStrings { get; set; }

        public Log Log { get; set; }
    }
}