using OnePiece;
using System;
using System.Data;
using System.Data.Common;

namespace DataObjects.Dao.Together {
   public class TXFP {
      private Db db;

      public TXFP() {
         db = GlobalDaoSetting.DB;
      }

      public DataTable ListDataByKey(string ini_key) {
         object[] parms =
         {
                ":ini_key",ini_key
            };

         #region sql

         string sql =
             @"
                    select trim(TXFP_PARAM1) as ls_str1,
                                trim(TXFP_PARAM2) as ls_str2,
                                trim(TXFP_PARAM3) as ls_dbms,
                                trim(TXFP_PARAM4) as ls_srv,
                                trim(TXFP_PARAM5) as ls_db,
                                trim(TXFP_PARAM6) as ls_dbparm
                                from ci.TXFP 
                                where TXFP_TXN_ID = :ini_key 
                ";

         #endregion sql

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      public DbConnection GetConnection(string conn , string ls_db) {
         try {
            db = new Db(conn , "Oracle.ManagedDataAccess.Client" , ls_db);
            db.dbConnection = db.CreateConnection();
         } catch (Exception ex) {
            db.dbConnection = null;
            //MessageDisplay.Error("「" + ini_key + "資料庫」無法連接，無法執行交易");
         }
         return db.dbConnection;
      }

      /// <summary>
      /// for 40180 wf_copy_7122_file
      /// (取得網路磁碟機路徑、帳密)
      /// </summary>
      /// <param name="txn_id"></param>
      /// <param name="param"></param>
      /// <returns></returns>
      public DataTable GetPathAccPwd(string txn_id = "file" , string param = "40180") {
         object[] parms =
           {
                ":txn_id",txn_id,
                ":param",param
            };

         string sql = @"
select 
    txfp_param1 as ls_user , 
    txfp_param2 as ls_pwd, 
    txfp_param3 as is_path
from ci.txfp
where txfp_txn_id = :txn_id
and txfp_param4 = :param
";
         return db.GetDataTable(sql , parms);
      }

   }
}