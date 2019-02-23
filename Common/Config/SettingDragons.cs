using BusinessObjects;
using Common.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Common.Config
{
    public class SettingDragons<T> where T : class
    {
        protected SettingDragons()
        {
            ReadFile();
        }

        public T Setting { get; set; }
        public static string FilePath { get; set; }

        private static SettingDragons<T> _Instance = null;

        public static SettingDragons<T> Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SettingDragons<T>();
                }
                return SettingDragons<T>._Instance;
            }
            set { SettingDragons<T>._Instance = value; }
        }

        /// <summary>
        /// 讀設定檔
        /// </summary>
        private void ReadFile()
        {
            try
            {
                string path = FilePath;

                if (String.IsNullOrEmpty(FilePath))
                {
                    if (HostingEnvironment.MapPath("~/bin") != null)
                    {
                        //Web用
                        path = Path.Combine(HostingEnvironment.MapPath("~/bin"), "Setting.xml");
                    }
                    else
                    {
                        //WinForm用
                        path = Directory.GetCurrentDirectory() + @"\Setting.xml";
                    }
                }

                T set = SDerializer.XmlToObject<T>(path);
                this.Setting = set;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 取出連線資串相關資訊
        /// </summary>
        /// <param name="connectionName">連線名稱</param>
        public Connection GetConnectionString(string connectionName)
        {
            T setting = SettingDragons<T>.Instance.Setting;
            List<Connection> list = (List<Connection>)setting.GetType().GetProperty("ConnectionStrings").GetValue(setting, null);
            Connection conn = list.Find(x => x.Name == connectionName);
            return conn;
        }

        public ConnectionInfo GetConnectionInfo(DBInfo dbInfo)
        {
            ConnectionInfo connectionInfo = new ConnectionInfo();

            connectionInfo.ConnectionName = dbInfo.ConnectionName;
            connectionInfo.Database = dbInfo.InitialCatalog;

            Connection connection = GetConnectionString(dbInfo.ConnectionName);

            connectionInfo.ConnectionString = connection.ConnectionString;
            connectionInfo.ProviderName = connection.ProviderName;

            return connectionInfo;
        }

        public ConnectionInfo GetConnectionInfo(string dbSchema)
        {
            ConnectionInfo resultConnInfo;

            switch (dbSchema)
            {
                case "ci":
                    resultConnInfo = GetConnectionInfo(SettingDragons.Instance.Setting.Database.Ci);
                    break;
                case "fut":
                    resultConnInfo = GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiFut);
                    break;
                case "opt":
                    resultConnInfo = GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiOpt);
                    break;
                case "futAH":
                    resultConnInfo = GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiFutAH);
                    break;
                case "optAH":
                    resultConnInfo = GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiOptAH);
                    break;
                default:
                    resultConnInfo = null;
                    break;
            }

            return resultConnInfo;
        }
    }

    public class SettingDragons : SettingDragons<Setting>
    {
    }
}