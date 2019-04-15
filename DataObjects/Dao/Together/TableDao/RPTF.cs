using BusinessObjects;
using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class RPTF
    {
        private Db db;

        public RPTF()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListData(string RPTF_TXN_ID, string RPTF_TXD_ID, string RPTF_KEY)
        {
            object[] parms = {
                "@RPTF_TXN_ID",RPTF_TXN_ID,
                "@RPTF_TXD_ID",RPTF_TXD_ID,
                "@RPTF_KEY",RPTF_KEY
            };

            string sql =
                @"
                              SELECT ' ' as OP_TYPE,
                                     CI.RPTF.RPTF_TXN_ID,
                                     CI.RPTF.RPTF_TXD_ID,
                                     CI.RPTF.RPTF_KEY,
                                     CI.RPTF.RPTF_SEQ_NO,
                                     CI.RPTF.RPTF_TEXT
                                FROM CI.RPTF
                               WHERE ( CI.RPTF.RPTF_TXN_ID = @RPTF_TXN_ID ) AND
                                     ( CI.RPTF.RPTF_TXD_ID = @RPTF_TXD_ID ) AND
                                     ( CI.RPTF.RPTF_KEY = @RPTF_KEY )
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

      /// <summary>
      /// save ci.rptf data
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
select 
    rptf_txn_id,
    rptf_txd_id,
    rptf_key,
    rptf_seq_no,
    rptf_text
from ci.rptf 
";
         return db.UpdateOracleDB(inputData , sql);
      }
   }
}