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

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DbConnection GetConnection(string conn, string ls_db) {
            try {
                db = new Db(conn, "Oracle.ManagedDataAccess.Client", ls_db);
                db.dbConnection = db.CreateConnection();
            }
            catch (Exception ex) {
                db.dbConnection = null;
                //MessageDisplay.Error("「" + ini_key + "資料庫」無法連接，無法執行交易");
            }
            return db.dbConnection;
        }
    }
}