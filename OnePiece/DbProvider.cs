using Oracle.ManagedDataAccess.Client;
using Sybase.Data.AseClient;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace OnePiece
{
    class DbProviderFactoriesEx
    {
        public static DbProviderFactory GetFactory(string providerInvariantName)
        {
            DbProviderFactory resultDbProviderFactory = null;

            switch (providerInvariantName)
            {
                case "System.Data.Oledb":
                    resultDbProviderFactory = OleDbFactory.Instance;
                    break;

                case "Sybase.Data.AseClient":
                    resultDbProviderFactory = AseClientFactory.Instance;
                    break;

                case "System.Data.SqlClient":
                    resultDbProviderFactory = SqlClientFactory.Instance;
                    break;

                case "System.Data.Odbc":
                    resultDbProviderFactory = OdbcFactory.Instance;
                    break;

                case "System.Data.OracleClient":
                    resultDbProviderFactory = DbProviderFactories.GetFactory(providerInvariantName);
                    break;

                case "Oracle.ManagedDataAccess.Client":
                    resultDbProviderFactory = OracleClientFactory.Instance;
                    break;

                case "System.Data.SqlServerCe.3.5":
                    resultDbProviderFactory = DbProviderFactories.GetFactory(providerInvariantName);
                    break;

                case "System.Data.SqlServerCe.4.0":
                    resultDbProviderFactory = DbProviderFactories.GetFactory(providerInvariantName);
                    break;

                case "MongoDB":
                    //resultDbProviderFactory = new MongoDbFactory();
                    break;

                default:

                    break;
            }

            return resultDbProviderFactory;
        }
    }
}
