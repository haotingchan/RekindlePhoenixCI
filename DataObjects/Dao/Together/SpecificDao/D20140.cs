using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Lukas, 2019/1/15
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D20140: DataGate {
        /// <summary>
        /// d_20140
        /// </summary>
        /// <param name="adt_sdate"></param>
        /// <param name="adt_edate"></param>
        /// <returns></returns>
        public DataTable ListAllByDate(DateTime adt_sdate, DateTime adt_edate) {

            object[] parms = {
                ":adt_sdate",adt_sdate,
                ":adt_edate",adt_edate
            };

            string sql =
                @"
SELECT KPR.KPR_DATE,   
         KPR.KPR_PROD_TYPE,   
         ' ' as OP_TYPE,   
         KPR.KPR_RATE  
FROM ci.KPR  
WHERE KPR.KPR_DATE >= :adt_sdate   
  and KPR.KPR_DATE <= :adt_edate
Order By kpr_date
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
