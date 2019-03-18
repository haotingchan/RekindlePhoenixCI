using System.Data;

/// <summary>
/// David 2019/3/13
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    public class TSE1 : DataGate {

        public DataTable ListAllByYQ(string yq) {

            object[] parms = {
                "@as_yq", yq,
            };

            string sql = @"select TSE1_YQ, TSE1_SID, TSE1_SNAME, TSE1_W_TIME, TSE1_W_USER_ID 
                                   from CI.TSE1
                                   where TSE1_YQ=:as_yq";

            return db.GetDataTable(sql, parms);

        }

        public int DeleteByYQ(string yq) {

            object[] parms = {
                "@as_yq", yq,
            };

            string sql = @"delete CI.TSE1
                                   where TSE1_YQ=:as_yq";

            return db.ExecuteSQL(sql, parms);

        }

    }
}
