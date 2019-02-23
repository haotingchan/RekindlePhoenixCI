using System.Data.Common;
using Utils.Util;
namespace Utils.Entity.Table
{
    public class PBCODE
    {
        public PBCODE()
        {
        }

        public string PBCODE_VERSION { get; set; }

        public string PBCODE_FILE_NAME { get; set; }

        public byte[] PBCODE_BIN_CODE { get; set; }

        public string PBCODE_TYPE { get; set; }

        public static PBCODE Create(DbDataReader reader)
        {
            return reader.DataToObject<PBCODE>();
        }
    }
}