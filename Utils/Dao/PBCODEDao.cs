using System.Data;
using Utils.Util;
using Utils.Entity.Table;
using System.Collections.Generic;

namespace Utils.Dao
{
    internal class PBCODEDao
    {
        private static PBCODEDao dao = new PBCODEDao();

        private DataUtil util;

        public static PBCODEDao getInstance(DataUtil util)
        {
            dao.util = util;
            return dao;
        }

        private const string sql = "SELECT * FROM PBCODE WHERE PBCODE_TYPE = '{0}'";

        private const string sql2 = "SELECT PBCODE_VERSION,PBCODE_FILE_NAME FROM PBCODE WHERE PBCODE_TYPE = '{0}'";

        private const string sql3 = "SELECT PBCODE_BIN_CODE FROM PBCODE WHERE PBCODE_FILE_NAME = '{0}'";
        /// <summary>
        /// 取得最新版本
        /// </summary>
        /// <returns></returns>
        public DataTable getVersion(string type)
        {
            return util.getData(string.Format(sql, type));
        }

        public IEnumerable<PBCODE> getVersionList(string type)
        {
            return util.db.Read<PBCODE>(string.Format(sql, type), x => new PBCODE
            {
                PBCODE_VERSION = x.GetString(0).Trim(),
                PBCODE_FILE_NAME = x.GetString(1).Trim(),
                PBCODE_BIN_CODE = (byte[])x["PBCODE_BIN_CODE"],
                PBCODE_TYPE = x.GetString(3).Trim()
            });
        }

        public DataTable getFileVersion(string type)
        {
            return util.getData(string.Format(sql2, type));
        }

        public DataTable getFileVersionByFile(string fileName) {
            return util.getData(string.Format(sql3, fileName));
        }
    }
}