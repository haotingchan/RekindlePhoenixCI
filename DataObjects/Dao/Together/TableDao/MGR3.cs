using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/11
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    public class MGR3: DataGate {

        /// <summary>
        /// 確認當日保證金適用比例資料是否已轉入
        /// </summary>
        /// <param name="ls_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public int mgr3Count(string ls_ymd) {

            object[] parms = {
                ":ls_ymd",ls_ymd
            };

            string sql =
@"
SELECT COUNT(*) as MGR3COUNT 
  FROM CI.MGR3
 WHERE MGR3_YMD = :LS_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count > 0)
                return dtResult.Rows[0]["MGR3COUNT"].AsInt();
            else
                return 0;
        }
    }
}
