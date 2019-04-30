using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.TableDao
{
    public class TPRICES_OPT: DataGate
    {

        public DataTable ListAllByDate(string as_ymd)
        {

            object[] parms =
            {
                ":as_ymd", as_ymd
            };

            string sql = @"
                                    SELECT ci.TPRICES_OPT.*,DECODE(TPRICES_PC_CODE,'C','買權','賣權') AS TPRICES_PC_CODE_NAME FROM ci.TPRICES_OPT 
                                    WHERE TPRICES_TRADE_YMD = :as_ymd
                                    ORDER BY TPRICES_KIND_ID,TPRICES_SETTLE_MONTH,TPRICES_STRIKE_PRICE
            ";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
