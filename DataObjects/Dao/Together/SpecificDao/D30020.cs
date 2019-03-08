using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/2/21
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D30020: DataGate {

        public DataTable d_30021(DateTime adt_sdate, DateTime adt_edate) {

            object[] parms = {
                ":adt_sdate", adt_sdate,
                ":adt_edate", adt_edate
            };

            string sql =
@"
SELECT   AB1_DATE,   
         AB1_ACC_TYPE,   
         AB1_TRADE_COUNT AS AB1_COUNT,   
         AB1_ACCU_COUNT
    FROM ci.AB1  
   WHERE AB1_DATE >= :adt_sdate  AND  
         AB1_DATE <= :adt_edate  AND 
        (AB1_TRADE_COUNT > 0  OR AB1_ACCU_COUNT > 0)
ORDER BY AB1_DATE Desc, AB1_ACC_TYPE
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_30021_acc_type(DateTime? adt_sdate, DateTime? adt_edate) {

            object[] parms = {
                ":adt_sdate", adt_sdate,
                ":adt_edate", adt_edate
            };

            string sql =
@"
SELECT AB1_ACC_TYPE 
FROM ci.AB1  
WHERE AB1_DATE >= :adt_sdate  AND  
      AB1_DATE <= :adt_edate 
GROUP BY AB1_ACC_TYPE
ORDER BY AB1_ACC_TYPE
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_30022(DateTime adt_sdate, DateTime adt_edate) {

            object[] parms = {
                ":adt_sdate", adt_sdate,
                ":adt_edate", adt_edate
            };

            string sql =
@"
SELECT A.AB1_DATE AS AB1_DATE,
       A.AB1_ACCU_COUNT AS AB1_ACCU_COUNT,
       B.AB1_COUNT AS AB1_COUNT,
       B.AB1_TRADE_COUNT AS AB1_TRADE_COUNT
FROM
 (SELECT AB1_DATE,   
         sum(AB1_ACCU_COUNT) as AB1_ACCU_COUNT
    FROM ci.AB1  
   WHERE AB1_DATE >= :adt_sdate  AND  
         AB1_DATE <= :adt_edate  and
         AB1_ACC_TYPE <> '9'   
   GROUP BY AB1_DATE ) A,
 (SELECT AB1_DATE,   
         sum(AB1_COUNT) as AB1_COUNT,   
         sum(AB1_ACCU_COUNT) as AB1_ACCU_COUNT,   
         sum(AB1_TRADE_COUNT)  as AB1_TRADE_COUNT
    FROM ci.AB1  
   WHERE AB1_DATE >= :adt_sdate  AND  
         AB1_DATE <= :adt_edate  
   GROUP BY AB1_DATE ) B
WHERE A.AB1_DATE = B.AB1_DATE
ORDER BY AB1_DATE
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
