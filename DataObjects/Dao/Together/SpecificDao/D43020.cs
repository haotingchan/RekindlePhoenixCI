using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/21
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D43020: DataGate {

        public DataTable d_43020(DateTime as_date, string as_osw_grp) {

            object[] parms = {
                ":as_date",as_date,
                ":as_osw_grp",as_osw_grp
            };

            string sql =
@"

SELECT CI.MG1.MG1_DATE,   
         CI.MG1.MG1_PROD_TYPE,   
         CI.MG1.MG1_TYPE,   
         CI.MG1.MG1_CUR_CM,   
         CI.MG1.MG1_CUR_MM,   
         CI.MG1.MG1_CUR_IM,   
         CI.MG1.MG1_CP_CM,   
         CI.MG1.MG1_CM,   
         CI.MG1.MG1_MM,   
         CI.MG1.MG1_IM,   
         CI.MG1.MG1_RISK,   
         CI.MG1.MG1_CP_RISK,   
         CI.MG1.MG1_CHANGE_RANGE,   
         CI.MG1.MG1_PRICE,   
         CI.MG1.MG1_CURRENCY_TYPE,   
         CI.MG1.MG1_M_MULTI,   
         CI.MG1.MG1_I_MULTI,   
         CI.MG1.MG1_XXX,   
         CI.MG1.MG1_SEQ_NO,   
         CI.MG1.MG1_MIN_RISK,   
         CI.MG1.MG1_CHANGE_FLAG,   
         CI.MG1.MG1_CM_RATE,   
         CI.MG1.MG1_MM_RATE,   
         CI.MG1.MG1_IM_RATE,   
         CI.MG1.MG1_PARAM_KEY,   
         CI.MG1.MG1_PROD_SUBTYPE,   
         CI.MG1.MG1_KIND_ID,   
         CI.APDK.APDK_NAME,   
         CI.APDK.APDK_UNDERLYING_MARKET,   
         CI.APDK.APDK_STOCK_ID,   
         CI.MGT7.MGT7_AB_XXX  ,
         PID_NAME
    FROM CI.MG1,   
         CI.APDK,   
         CI.MGT7,
         --上市/上櫃中文名稱
         (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM') 
   WHERE ( CI.MG1.MG1_KIND_ID = CI.APDK.APDK_KIND_ID ) and  
         ( CI.MG1.MG1_PROD_TYPE = CI.APDK.APDK_PROD_TYPE ) and  
         ( CI.MG1.MG1_TYPE = CI.MGT7.MGT7_AB_TYPE ) and  
         ( ( CI.MG1.MG1_DATE = :as_date ) AND  
         ( CI.MG1.MG1_PROD_SUBTYPE = 'S' ) AND  
         ( CI.MG1.MG1_PROD_TYPE = 'O' ) )    
     and APDK_UNDERLYING_MARKET = COD_ID
     and MG1_OSW_GRP LIKE :as_osw_grp
   ORDER BY mg1_seq_no, mg1_kind_id, mg1_type
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_40011_stat(string AS_YMD) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("AS_YMD",AS_YMD)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

            string sql = "CI.SP_H_TXN_40011_STAT";

            return db.ExecuteStoredProcedureEx(sql, parms, true);
        }
    }
}
