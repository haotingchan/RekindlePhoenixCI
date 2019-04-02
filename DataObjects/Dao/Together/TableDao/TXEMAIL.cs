using System.Data;
using OnePiece;

namespace DataObjects.Dao.Together {
   public class TXEMAIL {
      private Db db;

      public TXEMAIL() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// get email config, return txemail_sender, txemail_recipients, txemail_cc, txemail_title
      /// </summary>
      /// <param name="txemail_txn_id"></param>
      /// <param name="txemail_seq_no"></param>
      /// <returns></returns>
      public DataTable ListData(string txemail_txn_id, int txemail_seq_no = 1) {
         object[] parms =
         {
                "@txemail_txn_id", txemail_txn_id,
                "@txemail_seq_no", txemail_seq_no
            };

         #region sql

         string sql = @"
select  txemail_sender,
   txemail_recipients,
   txemail_cc,
   txemail_title
from    ci.txemail
where   txemail_txn_id = @txemail_txn_id  
and     txemail_seq_no = @txemail_seq_no
";

         #endregion sql

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }




   }
}