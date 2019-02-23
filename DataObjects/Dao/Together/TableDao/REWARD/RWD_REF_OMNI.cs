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
                    REWARD.RWD_REF_OMNI.RWD_REF_OMNI_ACTIVITY_ID,   
                    REWARD.RWD_REF_OMNI.RWD_REF_OMNI_PROD_TYPE,   
                    REWARD.RWD_REF_OMNI.RWD_REF_OMNI_FCM_NO,   
                    REWARD.RWD_REF_OMNI.RWD_REF_OMNI_ACC_NO,   
                    REWARD.RWD_REF_OMNI.RWD_REF_OMNI_MARKET_CLOSE,  
		            ' ' as OP_TYPE
                    FROM REWARD.RWD_REF_OMNI
                    Order By rwd_ref_omni_activity_id Asc, 
	                rwd_ref_omni_prod_type Asc, 
	                rwd_ref_omni_fcm_no Asc, 
	                rwd_ref_omni_acc_no Asc, 
	                rwd_ref_omni_market_close Asc
                ";

            #endregion sql

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }
    }
}
