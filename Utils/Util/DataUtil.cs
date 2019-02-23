using OnePiece;
using System.Collections.Generic;
using System.Data;
using Utils.Entity.XML;

namespace Utils.Util
{
    public class DataUtil
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        private string Conn { get; set; }

        /// <summary>
        /// 資料庫名稱
        /// </summary>
        private string DB { get; set; }

        public Db db { get; set; }

        public DataUtil()
        {
        }

        public DataUtil(string conn, string database)
        {
            Conn = conn;
            DB = database;
        }

        public DataUtil(string conn,string providerName,string database) { 
            db = new Db(conn,providerName,database);
        }

        private static Dictionary<string, DataUtil> dict = new Dictionary<string, DataUtil>();

        /// <summary>
        /// 是否有資料
        /// </summary>
        /// <param name="sql">查詢SQL</param>
        /// <returns>bool</returns>
        public bool getHasData(string sql)
        {
            bool hasData = false;
            DataTable table = new DataTable();
            table = db.GetDataTable(sql);
            if (table.Rows.Count > 0) {
                hasData = true;
            }
            return hasData;
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="sql">查詢SQL</param>
        /// <returns>DataTable</returns>
        public DataTable getData(string sql)
        {
            DataTable table = new DataTable();
            table = db.GetDataTable(sql);
            return table;
        }

        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="sql">查詢SQL</param>
        /// <param name="parm">參數</param>
        /// <returns></returns>
        public int updateData(string sql, object[] parm)
        {
            int result = 0;
            result = db.ExecuteSQL(sql, parm);
            return result;
        }


        

        /// <summary>
        ///
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static DataUtil getInstance(string database)
        {
            if (dict.Count == 0)
            {
                getConnectionSetting();
            }
            DataUtil util = dict[database];
            return util;
        }
        
        /// <summary>
        /// 取得XML設定檔資料庫設定
        /// </summary>
        private static void getConnectionSetting()
        {
            Database db = SettingDragons<Setting>.Instance.Setting.Database;
            var properties = db.GetType().GetProperties();
            DBInfo info = null;
            Connection connection = null;
            foreach (var p in properties)
            {
                info = (DBInfo)p.GetValue(db, null);
                if (info != null)
                {
                    connection = SettingDragons<Setting>.Instance.GetConnectionString(info.ConnectionName);
                    dict.Add(p.Name, new DataUtil(connection.ConnectionString,connection.ProviderName, info.InitialCatalog));
                }
            }
        }
    }
}