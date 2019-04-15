using System;
using System.Data;
/// <summary>
/// Winni, 2019/4/15
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D49074 : DataGate {

      /// <summary>
      /// get ci.rptf data return rptf_txn_id/rptf_txd_id/rptf_key/rptf_seq_no/rptf_text/is_newrow 6 feild (d_49074)
      /// </summary>
      /// <param name="key"></param>
      /// <returns></returns>
      public DataTable GetDataList(string key) {

         object[] parms =
            {
               ":key",key
            };

         string sql = @"
select 
    rptf_txn_id,
    rptf_txd_id,
    rptf_key,
    rptf_seq_no,
    rptf_text,
    ' ' as is_newrow 
from ci.rptf 
where rptf_txd_id = '49074'
and rptf_txd_id = '49074'
and rptf_key like :key
order by rptf_seq_no
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}
