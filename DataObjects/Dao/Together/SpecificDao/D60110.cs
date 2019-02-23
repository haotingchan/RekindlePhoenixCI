using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class D60110
    {
        private Db db;

        public D60110()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListW_AM_And_APDK_PARAM(string yyyyMM)
        {
            object[] parms = {
                "@as_ym",yyyyMM
            };

            string sql =
                @"
                      SELECT W_AM_YM,   
                             W_AM_PARAM_KEY,   
                             W_AM_QNTY_M,   
                             W_AM_QNTY_Y_L,   
                             W_AM_QNTY_M_L,   
                             W_AM_QNTY_ACCU_Y,   
                             W_AM_QNTY_ACCU_Y_L,   
                             W_AM_QNTY_AVG_Y ,  
                             W_AM_OI, 
	                           W_AM_QNTY_Y,
                             W_AM_DAYS_Y,
                             PARAM_NAME,PARAM_PROD_TYPE
                        FROM ci.W_AM ,
                             ci.APDK_PARAM
                       WHERE W_AM_YM = @as_ym
                         AND W_AM_PARAM_KEY = PARAM_KEY (+) 
                    ORDER BY PARAM_PROD_TYPE ASC nulls first, W_AM_PARAM_KEY ASC
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}