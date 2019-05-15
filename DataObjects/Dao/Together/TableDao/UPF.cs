using BusinessObjects;
using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together
{
    public class UPF:DataGate
    {
        private Db db;

        public UPF()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListData()
        {
            #region sql

            string sql =
                @"
                    SELECT  *
                    FROM    CI.UPF
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public DataTable ListDataForUserIDAndUserName()
        {
            #region sql

            string sql =
                @"
                   SELECT UPF_USER_ID, UPF_USER_NAME,  TRIM(UPF_USER_ID) || '：' || UPF_USER_NAME AS UPF_USER_ID_NAME
                   FROM CI.UPF
                   ORDER BY UPF_USER_ID ASC
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public DataTable ListDataByDept(string UPF_DPT_ID)
        {
            object[] parms =
            {
                "@UPF_DPT_ID",UPF_DPT_ID
            };

            #region sql

            string sql =
                @"
                    SELECT UPF_USER_ID,
                    UPF_USER_NAME,
                    UPF_EMPLOYEE_ID,
                    UPF_DPT_ID,
                    UPF_PASSWORD,
                    UPF_W_TIME,
                    UPF_W_USER_ID,
                    UPF_CHANGE_FLAG,
                    ' ' AS OP_TYPE

                    FROM ci.UPF
                    WHERE UPF_DPT_ID = @UPF_DPT_ID
                    ORDER BY UPF_USER_ID
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable ListDataByUserId(string UPF_USER_ID)
        {
            object[] parms =
            {
                "@UPF_USER_ID",UPF_USER_ID
            };

            #region sql

            string sql =
                @"
                      SELECT UPF_USER_ID,
                             UPF_USER_NAME,
                             UPF_EMPLOYEE_ID,
                             UPF_DPT_ID,
                             DPT_NAME,
                             DPT_ID || '：' || DPT_NAME AS DPT_ID_NAME,
                             UPF_PASSWORD,
                             UPF_W_TIME,
                             UPF_W_USER_ID,
                             UPF_CHANGE_FLAG,
                             ' ' AS OP_TYPE
                       FROM ci.UPF,ci.DPT
                       WHERE UPF_USER_ID = @UPF_USER_ID
                        AND UPF_DPT_ID = DPT_ID
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable ListDataByUserIdAndPassword(string UPF_USER_ID, string UPF_PASSWORD)
        {
            object[] parms =
            {
                "@UPF_USER_ID",UPF_USER_ID,
                "@UPF_PASSWORD",UPF_PASSWORD
            };

            #region sql

            string sql =
                @"
                      SELECT *
                      FROM ci.UPF
                      WHERE UPF_USER_ID = @UPF_USER_ID
                      AND TRIM(UPF_PASSWORD) = TRIM(@UPF_PASSWORD)
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public ResultData Update(DataTable inputData)
        {
            string tableName = "ci.UPF";
            string keysColumnList = "UPF_USER_ID";
            string insertColumnList = "UPF_USER_ID, UPF_USER_NAME, UPF_EMPLOYEE_ID, UPF_DPT_ID, UPF_PASSWORD, UPF_W_TIME, UPF_W_USER_ID, UPF_CHANGE_FLAG";
            string updateColumnList = "UPF_USER_NAME, UPF_EMPLOYEE_ID, UPF_DPT_ID, UPF_PASSWORD, UPF_W_TIME, UPF_W_USER_ID, UPF_CHANGE_FLAG";

            try
            {
                //update to DB
                return SaveForChanged(inputData, tableName, insertColumnList, updateColumnList, keysColumnList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdatePasswordByUserId(string UPF_USER_ID, string UPF_PASSWORD)
        {
            object[] parms = {
                "@UPF_PASSWORD",UPF_PASSWORD,
                "@UPF_W_TIME",DateTime.Now,
                "@UPF_USER_ID",UPF_USER_ID
            };

            string sql = @"
                                        UPDATE CI.UPF
                                        SET UPF_PASSWORD = @UPF_PASSWORD,
                                        UPF_W_TIME   = @UPF_W_TIME
                                        WHERE TRIM(UPF_USER_ID)  = @UPF_USER_ID
                                ";

            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdatePasswordByUserId(string UPF_USER_ID, string UPF_PASSWORD, string UPF_CHANGE_FLAG)
        {
            object[] parms = {
                "@UPF_PASSWORD",UPF_PASSWORD,
                "@UPF_CHANGE_FLAG",UPF_CHANGE_FLAG,
                "@UPF_W_TIME",DateTime.Now,
                "@UPF_USER_ID",UPF_USER_ID
            };

            string sql = @"
                                        UPDATE CI.UPF
                                        SET UPF_PASSWORD = @UPF_PASSWORD,
                                        UPF_CHANGE_FLAG = @UPF_CHANGE_FLAG,
                                        UPF_W_TIME   = @UPF_W_TIME
                                        WHERE TRIM(UPF_USER_ID)  = @UPF_USER_ID
                                ";

            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}