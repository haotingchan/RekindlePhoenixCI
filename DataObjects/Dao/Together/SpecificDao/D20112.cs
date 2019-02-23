using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/1/29
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D20112: DataGate {

        public DataTable ListAllByDate(string as_year) {

            object[] parms = {
                ":as_year", as_year
            };

            string sql =
@"
SELECT A.*
FROM
    (SELECT INTWSE1_YMD,
                    INTWSE1_TRADE_VOLUMN,
                    INTWSE1_TRADE_AMT,
                    INTWSE1_TRADE_CNT,
                    INTWSE1_INDEX,
                    INTWSE1_UP_DOWN,
                     '上市' as TYPE_NAME 
    FROM ci.INTWSE1
    WHERE INTWSE1_YMD>=:as_year||'0101'
      AND INTWSE1_YMD <=:as_year||'1231'
    UNION ALL
    SELECT INOTC1_YMD,
                    INOTC1_TRADE_VOLUMN,
                    INOTC1_TRADE_AMT,
                    INOTC1_TRADE_CNT,
                    INOTC1_INDEX,
                    INOTC1_UP_DOWN ,
                    '上櫃' 
    FROM ci.INOTC1
    WHERE INOTC1_YMD>=:as_year||'0101'
      AND INOTC1_YMD <=:as_year||'1231') A
ORDER BY TYPE_NAME, INTWSE1_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
