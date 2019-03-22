using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/1/22
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    /// <summary>
    /// for d_50073_export
    /// </summary>
    public class D50073: DataGate {

        public DataTable ListAll () {

            string sql =
                    @"
SELECT   
        COD_DESC AS RWD_REF_OMNI_ACTIVITY_ID,   
        case RWD_REF_OMNI_PROD_TYPE when 'F' then '期貨' when 'O' then '選擇權' else '' end as RWD_REF_OMNI_PROD_TYPE,   
        RWD_REF_OMNI_FCM_NO,   
        RWD_REF_OMNI_ACC_NO,   
        case RWD_REF_OMNI_MARKET_CLOSE when '1' then '一般交易時段' when '2' then '盤後交易時段' else '' end as RWD_REF_OMNI_MARKET_CLOSE
FROM REWARD.RWD_REF_OMNI ,ci.COD
where COD_TXN_ID = '50073'    
  AND COD_COL_ID = 'ACTIVITY_ID'
  and RWD_REF_OMNI_ACTIVITY_ID = TRIM(COD_ID)
ORDER BY COD_ID,RWD_REF_OMNI_PROD_TYPE, RWD_REF_OMNI_FCM_NO,RWD_REF_OMNI_ACC_NO,RWD_REF_OMNI_MARKET_CLOSE 
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public ResultData update(DataTable inputData) {

            string tableName = "REWARD.RWD_REF_OMNI";
            string keysColumnList = "RWD_REF_OMNI_ACTIVITY_ID, RWD_REF_OMNI_PROD_TYPE, RWD_REF_OMNI_FCM_NO, RWD_REF_OMNI_ACC_NO, RWD_REF_OMNI_MARKET_CLOSE";
            string insertColumnList = "RWD_REF_OMNI_ACTIVITY_ID, RWD_REF_OMNI_PROD_TYPE, RWD_REF_OMNI_FCM_NO, RWD_REF_OMNI_ACC_NO, RWD_REF_OMNI_MARKET_CLOSE";
            string updateColumnList = insertColumnList;
            try {
                //update to DB
                return SaveForAll(inputData, tableName, insertColumnList, updateColumnList, keysColumnList);
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}
