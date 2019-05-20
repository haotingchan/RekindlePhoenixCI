using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.TableDao
{
    public class AP:DataGate
    {
        public String GetMaxVersion()
        {

            object[] parms = {
            };

            string sql =
        @"
                SELECT MAX(AP_VERSION) FROM ci.AP
            ";

            return db.ExecuteScalar(sql, CommandType.Text, parms);
        }
    }
}
