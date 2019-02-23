using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/1/24
/// </summary>
namespace DataObjects.Dao.Together.TableDao {

    public class LOGV: DataGate {

        public bool Insert(string is_txn_id, string gs_user_id, string ls_type, string ls_val1, string ls_val2, string ls_val3, string ls_val4, string ls_val5) {

            object[] parms = {
               ":is_txn_id",is_txn_id,
               ":gs_user_id",gs_user_id,
               ":ls_type",ls_type,
                ":ls_val1",ls_val1,
                ":ls_val2",ls_val2,
                ":ls_val3",ls_val3,
                ":ls_val4",ls_val4,
                ":ls_val5",ls_val5,
            };
            string sql = @"insert into ci.LOGV
                          values
                         (:is_txn_id, sysdate,:gs_user_id,:ls_type,:ls_val1,:ls_val2,:ls_val3,:ls_val4,:ls_val5)";
            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0) {
                return true;
            }
            else {
                throw new Exception("LOGV更新失敗");
            }
        }
    }
}
