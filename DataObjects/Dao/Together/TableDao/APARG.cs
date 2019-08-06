using OnePiece;
using System;

namespace DataObjects.Dao.Together
{
    public class APARG:DataGate
    {

        public bool Update(string txnId,string ym,string user)
        {
            object[] parms =
            {
                "@APARG_ARG1", ym,
                "@APARG_W_USER_ID",user,
                "@APARG_TXN_ID",txnId
            };

            #region sql

            string sql =
                @"
                    UPDATE ci.APARG SET 
                                                APARG_ARG1 = @APARG_ARG1,
                                                APARG_W_TIME = sysdate,
                                                APARG_W_USER_ID = @APARG_W_USER_ID
                    WHERE APARG_TXN_ID = @APARG_TXN_ID
                    
                ";

            #endregion sql

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