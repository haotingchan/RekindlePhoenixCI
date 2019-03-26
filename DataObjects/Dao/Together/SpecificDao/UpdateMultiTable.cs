using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataObjects.Dao.Together.SpecificDao {
   public class UpdateMultiTable : DataGate {

      public virtual ResultStatus UpdateDB(List<DataTable> ds, List<string> sqlList) {
         var connection = db.CreateConnection();
         OracleConnection oracleConn = (OracleConnection)connection;

         OracleTransaction tran = oracleConn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

         try {
            foreach (DataTable inputDt in ds) {
               db = new OnePiece.Db(oracleConn);
               db.UpdateOracleDB(inputDt, sqlList[ds.IndexOf(inputDt)]);
               tran.Commit();
            }
         } catch (Exception ex) {
            tran.Rollback();
            throw ex;
         }
         return ResultStatus.Success;
      }
   }
}
