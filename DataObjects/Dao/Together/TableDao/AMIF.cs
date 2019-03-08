using BusinessObjects;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/1/30
/// </summary>
namespace DataObjects.Dao.Together.TableDao {

    public class AMIF: DataGate {

        /// <summary>
        /// for W20110
        /// </summary>
        /// <param name="ad_date"></param>
        /// <returns></returns>
        public string MinSettleDate(DateTime ad_date) {

            string ls_settle_date;
            object[] parms =
            {
                ":ad_date", ad_date
            };

            string sql =
@"
select min(AMIF_SETTLE_DATE) as ls_settle_date
from ci.AMIF
where AMIF_DATE = :ad_date
  and AMIF_KIND_ID = 'TXF' 
  and AMIF_DATA_SOURCE = 'T'
  and AMIF_MTH_SEQ_NO = 1
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            ls_settle_date = dtResult.Rows[0]["LS_SETTLE_DATE"].AsString();
            return ls_settle_date;
        }

        /// <summary>
        /// for W20110
        /// </summary>
        /// <param name="ldt_bef_date"></param>
        /// <param name="ldt_date"></param>
        /// <returns></returns>
        public DateTime MaxAMIF_DATE(DateTime ldt_bef_date, DateTime ldt_date) {

            DateTime rtnDate;
            object[] parms =
            {
                ":ldt_bef_date", ldt_bef_date,
                ":ldt_date", ldt_date,
            };

            string sql =
@"
SELECT max(AMIF_DATE) as ldt_date
  FROM ci.AMIF
WHERE AMIF_DATE >= :ldt_bef_date
  and AMIF_DATE < :ldt_date
  and AMIF_DATA_SOURCE = 'U'
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            rtnDate = dtResult.Rows[0]["LDT_DATE"].AsDateTime();
            return rtnDate;
        }

      //for 28110
      public DataTable d_28110_amif2(string as_ymd) {

         object[] parms =
         {
                ":as_ymd", as_ymd
            };

         string sql = @"
SELECT AMIF_YEAR,   
         AMIF_DATE,   
         AMIF_PROD_ID,   
         AMIF_HIGH_PRICE,   
         AMIF_LOW_PRICE,   
         AMIF_OPEN_PRICE,   
         AMIF_CLOSE_PRICE,   
         AMIF_KIND_ID,   
         AMIF_SETTLE_DATE  
 from ci.amif
where amif_date = :as_date
  and amif_prod_type = 'M'
  and amif_kind_id = 'STW'
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// Update CI.AMIF
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
SELECT AMIF_YEAR,   
         AMIF_DATE,   
         AMIF_PROD_ID,   
         AMIF_HIGH_PRICE,   
         AMIF_LOW_PRICE,   
         AMIF_OPEN_PRICE,   
         AMIF_CLOSE_PRICE, 
         AMIF_SETTLE_PRICE,
         AMIF_OPEN_INTEREST,
         AMIF_UP_DOWN_VAL,
         AMIF_M_QNTY_TAL,  
         AMIF_KIND_ID,   
         AMIF_SETTLE_DATE
FROM CI.AMIF
";
         return db.UpdateOracleDB(inputData , sql);
      }
   }
}
