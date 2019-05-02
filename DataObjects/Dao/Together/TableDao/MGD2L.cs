using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/30
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    public class MGD2L: DataGate {

        public ResultData UpdateMGD2L(DataTable inputData) {
            string sql = @"
SELECT 
    MGD2_YMD,            
    MGD2_PROD_TYPE,      
    MGD2_KIND_ID,        
    MGD2_STOCK_ID,       
    MGD2_ADJ_TYPE,

    MGD2_ADJ_RATE,       
    MGD2_ADJ_CODE,       
    MGD2_ISSUE_BEGIN_YMD,
    MGD2_ISSUE_END_YMD,  
    MGD2_IMPL_BEGIN_YMD,

    MGD2_IMPL_END_YMD,   
    MGD2_PUB_YMD,        
    MGD2_PROD_SUBTYPE,   
    MGD2_PARAM_KEY,      
    MGD2_AB_TYPE,

    MGD2_AMT_TYPE,       
    MGD2_CUR_CM,         
    MGD2_CUR_MM,         
    MGD2_CUR_IM,         
    MGD2_CUR_LEVEL,

    MGD2_CM,             
    MGD2_MM,             
    MGD2_IM,             
    MGD2_LEVEL,          
    MGD2_CURRENCY_TYPE,

    MGD2_SEQ_NO,         
    MGD2_OSW_GRP,        
    MGD2_W_TIME,         
    MGD2_W_USER_ID,      
    MGD2_ADJ_RSN,

    MGD2_L_TIME,         
    MGD2_L_USER_ID,      
    MGD2_L_TYPE                 
  
FROM CI.MGD2L";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
