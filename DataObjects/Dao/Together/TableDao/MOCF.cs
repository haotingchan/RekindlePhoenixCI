using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.TableDao {
   public class MOCF:DataGate {


      public string GetMaxOcfDate(string beginDate, string endDate) {

         object[] parms = {
                ":begin_ymd", beginDate,
                ":end_ymd", endDate
            };

         string sql = @"SELECT MAX(MOCF_YMD) as ls_mocf_ymd
FROM ci.MOCF 
WHERE MOCF_YMD >= :begin_ymd
AND MOCF_YMD < :end_ymd
AND MOCF_OPEN_CODE = 'Y'";

         return db.ExecuteScalar(sql, CommandType.Text, parms);
      }

      public string GetValidDatePrev(string validDate) {

         object[] parms = {
                ":validDate", validDate
            };

         string sql = @"SELECT MAX(MOCF_YMD) as validDatePrev  
FROM ci.MOCF 
WHERE MOCF_YMD < :validDate 
AND MOCF_OPEN_CODE = 'Y' ";

         return db.ExecuteScalar(sql, CommandType.Text, parms);
      }

   }
}
