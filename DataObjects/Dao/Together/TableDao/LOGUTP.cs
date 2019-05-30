using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class LOGUTP
    {
        private Db db;

        public LOGUTP()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListDataByUser(DateTime LOGUTP_START_DATE, DateTime LOGUTP_END_DATE, string LOGUTP_USER_ID)
        {
            object[] parms =
           {
                "@LOGUTP_START_DATE",LOGUTP_START_DATE,
                "@LOGUTP_END_DATE",LOGUTP_END_DATE,
                "@LOGUTP_USER_ID",LOGUTP_USER_ID+"%"
            };

            string sql = @"
                                        SELECT   CI.LOGUTP.*,
                                                       DPT_NAME,
                                                       TXN_NAME
                                          FROM   CI.LOGUTP,
                                                       CI.TXN,
                                                       CI.DPT
                                        WHERE    LOGUTP_DPT = DPT_ID AND
                                                        LOGUTP_TXN_ID = TXN_ID AND
                                                        LOGUTP_DATE >= @LOGUTP_START_DATE AND
                                                        LOGUTP_DATE <= @LOGUTP_END_DATE AND
                                                        LOGUTP_USER_ID like @LOGUTP_USER_ID
                                        ORDER BY LOGUTP_W_TIME,LOGUTP_USER_ID,LOGUTP_TXN_ID
                                  ";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public bool InsertByUTPAndUPF(string TXN_ID, string LOGUTP_W_DPT, string LOGUTP_W_USER_ID, string LOGUTP_W_USER_NAME, string LOGUTP_TYPE)
        {
            object[] parms =
            {
                "@LOGUTP_DATE",DateTime.Now.Date,
                "@LOGUTP_W_TIME",DateTime.Now,
                "@LOGUTP_W_DPT",LOGUTP_W_DPT,
                "@LOGUTP_W_USER_ID",LOGUTP_W_USER_ID,
                "@LOGUTP_W_USER_NAME",LOGUTP_W_USER_NAME,
                "@LOGUTP_TYPE",LOGUTP_TYPE,
                "@TXN_ID",TXN_ID
            };

            #region sql

            string sql =
                @"
                      INSERT INTO ci.LOGUTP
                      SELECT @LOGUTP_DATE,@LOGUTP_W_TIME,@LOGUTP_W_DPT,@LOGUTP_W_USER_ID,
                                   @LOGUTP_W_USER_NAME,UPF_DPT_ID,UTP_USER_ID,ci.UPF.UPF_USER_NAME,UTP_TXN_ID,
                                   @LOGUTP_TYPE
                      FROM ci.UTP , ci.UPF
                      WHERE UTP_TXN_ID = @TXN_ID
                      AND UTP_USER_ID = UPF_USER_ID
                ";

            #endregion sql

            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult >= 0)
            {
                return true;
            }
            else
            {
                throw new Exception("LOGUTP更新失敗");
            }
        }

        public bool InsertByUTPAndUPF(string TXN_ID, string LOGUTP_W_DPT, string LOGUTP_W_USER_ID, string LOGUTP_W_USER_NAME, string LOGUTP_TYPE, string UTP_USER_ID)
        {
            object[] parms =
            {
                "@LOGUTP_DATE",DateTime.Now.Date,
                "@LOGUTP_W_TIME",DateTime.Now,
                "@LOGUTP_W_DPT",LOGUTP_W_DPT,
                "@LOGUTP_W_USER_ID",LOGUTP_W_USER_ID,
                "@LOGUTP_W_USER_NAME",LOGUTP_W_USER_NAME,
                "@LOGUTP_TYPE",LOGUTP_TYPE,
                "@TXN_ID",TXN_ID,
                "@UTP_USER_ID",UTP_USER_ID
            };

            #region sql

            string sql =
                @"
                      INSERT INTO ci.LOGUTP
                      SELECT @LOGUTP_DATE,@LOGUTP_W_TIME,@LOGUTP_W_DPT,@LOGUTP_W_USER_ID,
                                   @LOGUTP_W_USER_NAME,UPF_DPT_ID,UTP_USER_ID,ci.UPF.UPF_USER_NAME,UTP_TXN_ID,
                                   @LOGUTP_TYPE
                      FROM ci.UTP , ci.UPF
                      WHERE UTP_TXN_ID = @TXN_ID
                      AND UTP_USER_ID = UPF_USER_ID
                      AND UTP_USER_ID = @UTP_USER_ID
                ";

            #endregion sql

            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult >= 0)
            {
                return true;
            }
            else
            {
                throw new Exception("LOGUTP更新失敗");
            }
        }
    }
}