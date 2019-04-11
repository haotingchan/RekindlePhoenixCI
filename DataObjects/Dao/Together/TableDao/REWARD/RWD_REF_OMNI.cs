using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.TableDao.REWARD {
    public class RWD_REF_OMNI {
        private Db db;

        public RWD_REF_OMNI() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetData() {

            #region sql

            string sql =
                @"
                    SELECT   
                        RWD_REF_OMNI_ACTIVITY_ID,
                        RWD_REF_OMNI_FCM_NO,     
                        RWD_REF_OMNI_ACC_NO,     
                        RWD_REF_OMNI_NAME,       
		                  ' ' as OP_TYPE
                    FROM REWARD.RWD_REF_OMNI
                    ORDER BY RWD_REF_OMNI_ACTIVITY_ID ASC, 
	                          RWD_REF_OMNI_FCM_NO ASC, 
	                          RWD_REF_OMNI_ACC_NO ASC, 
	                          RWD_REF_OMNI_NAME ASC
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }
    }
}
