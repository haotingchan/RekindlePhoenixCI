using Common;
using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together {
   public class RPT {
      private Db db;

      public RPT() {
         db = GlobalDaoSetting.DB;
      }

      public DataTable ListData(string RPT_TXD_ID) {
         object[] parms =
         {
                "@RPT_TXD_ID", RPT_TXD_ID
            };

         #region sql

         string sql =
             @"
                    SELECT  *
                    FROM    CI.RPT
                    WHERE   RPT_TXD_ID like @RPT_TXD_ID
                ";

         #endregion sql

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// Lukas, 2019/1/30
      /// from PB: ci_fun.d_rpt
      /// </summary>
      /// <param name="as_txd_id"></param>
      /// <returns></returns>
      public DataTable ListAllByTXD_ID(string as_txd_id) {

         object[] parms =
         {
                ":as_txd_id", as_txd_id
            };

         string sql =
@"
SELECT RPT_TXN_ID,   
       RPT_TXD_ID,   
       RPT_SEQ_NO,   
       RPT_VALUE,   
       RPT_LEVEL_1,   
       RPT_LEVEL_2,   
       RPT_LEVEL_3,   
       RPT_LEVEL_4,   
       RPT_TYPE,   
       RPT_LEVEL_CNT,   
       RPT_VALUE_2,   
       RPT_VALUE_3,   
       RPT_VALUE_4,   
       RPT_VALUE_5  
FROM CI.RPT   
WHERE RPT_TXD_ID = :as_txd_id
ORDER BY RPT_SEQ_NO
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// Winni, 2019/3/15
      /// get Excel的Column預留數 (目前 for 30410 ，但有將參數拉出來)
      /// </summary>
      /// <param name="txnId"></param>
      /// <param name="txdId"></param>
      /// <returns></returns>
      public string DataByRptId(string txnId , string txdId) {

         object[] parms =
         {
                ":txnId", txnId,
                ":txdId", txdId
            };

         string sql =
@"
select to_number(rpt_value) as rowTotal
from ci.rpt
where rpt_txn_id = :txnId
and rpt_txd_id = :txdId
";

            string rowTotal = db.ExecuteScalar(sql , CommandType.Text , parms);
         if (string.IsNullOrEmpty(rowTotal))
            throw new Exception("回傳數據為null或空值");

         return rowTotal;
      }
   }
}