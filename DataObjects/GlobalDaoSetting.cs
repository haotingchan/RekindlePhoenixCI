using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public static class GlobalDaoSetting
    {
        private static ConnectionInfo _ConnectionInfo;
        private static string connectionString;
        private static string providerName;
        private static string database;
        private static Db db;

        public static string ConnectionString
        {
            get
            {
                return connectionString;
            }

            set
            {
                connectionString = value;
            }
        }

        public static string ProviderName
        {
            get
            {
                return providerName;
            }

            set
            {
                providerName = value;
            }
        }

        public static string Database
        {
            get
            {
                return database;
            }

            set
            {
                database = value;
            }
        }

        public static void Set(ConnectionInfo connectionInfo)
        {
            _ConnectionInfo = connectionInfo;
             ConnectionString = connectionInfo.ConnectionString;
            ProviderName = connectionInfo.ProviderName;
            Database = connectionInfo.Database;
            db = new Db(ConnectionString, ProviderName, Database);
        }

        public static ConnectionInfo GetConnectionInfo
        {
            get
            {
                return _ConnectionInfo;
            }
        }

        public static Db DB
        {
            get { return db; }
            set { db = value; }
        }
    }
}
