using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/5/15
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D40074 : DataGate {

        public DataTable d_40074() {

            string sql =
@"
SELECT
    MGD2_PROD_TYPE as PROD_TYPE,      
    MGD2_KIND_ID as KIND_ID,      
    MGD2_STOCK_ID as STOCK_ID,              
    MGD2_ADJ_CODE as ADJ_CODE,         
    MGD2_PUB_YMD as PUB_YMD,       
    MGD2_PROD_SUBTYPE as PROD_SUBTYPE,  
    MGD2_PARAM_KEY as PARAM_KEY,       
    MGD2_AMT_TYPE as AMT_TYPE,             
    MGD2_CM as CM_A,             
    MGD2_MM as MM_A,             
    MGD2_IM as IM_A,        
    MGD2_CM as CM_B,             
    MGD2_MM as MM_B,             
    MGD2_IM as IM_B,        
    MGD2_LEVEL as M_LEVEL,          
    MGD2_CURRENCY_TYPE as CURRENCY_TYPE,  
    MGD2_SEQ_NO as SEQ_NO,        
    MGD2_OSW_GRP as OSW_GRP,        
    ' ' as OP_TYPE,
    COD_SEQ_NO AS PROD_SEQ_NO,
    RPT_VALUE_2 AS CND_PARAM_KEY,
    RPT_VALUE_3 AS ABROAD
FROM CI.MGD2,CI.COD,CI.RPT
WHERE MGD2_YMD = ''
  AND MGD2_ADJ_TYPE = ''
  AND COD_TXN_ID = '40071'
  AND COD_COL_ID = 'SUBTYPE'
  AND RPT_TXN_ID = '40071'
  AND RPT_TXD_ID = 'SUBTYPE'
  AND RPT_SEQ_NO = COD_SEQ_NO
ORDER BY SEQ_NO, KIND_ID
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public int getProd(string as_kind_id, string as_prod_subtype) {
            string sql = "";
            object[] parms = {
                ":as_prod_subtype", as_prod_subtype,
                ":as_kind_id", as_kind_id
            };

            if (as_prod_subtype == "I") {

                sql =
@"
SELECT COD_SEQ_NO as li_prod_seq
		FROM ci.COD,ci.RPT,ci.MGT2 
		WHERE COD_TXN_ID = '40071'
			 AND COD_COL_ID = 'SUBTYPE'
		  AND RPT_TXN_ID = '40071'
		  AND RPT_TXD_ID = 'SUBTYPE'
		  AND RPT_SEQ_NO = COD_SEQ_NO
		  AND RPT_VALUE = :as_prod_subtype
		  AND MGT2_PROD_SUBTYPE = RPT_VALUE
		  AND MGT2_ABROAD = RPT_VALUE_3
		  AND MGT2_KIND_ID = :as_kind_id
";
            }
            else if (as_prod_subtype == "S") {


                sql =
@"
SELECT COD_SEQ_NO as li_prod_seq
		FROM ci.COD,ci.RPT ,ci.APDK
		WHERE COD_TXN_ID = '40071'
			 AND COD_COL_ID = 'SUBTYPE'
		  AND RPT_TXN_ID = '40071'
		  AND RPT_TXD_ID = 'SUBTYPE'
		  AND RPT_SEQ_NO = COD_SEQ_NO
		  AND APDK_PROD_SUBTYPE = RPT_VALUE
		  AND SUBSTR(APDK_PARAM_KEY,1,2) = SUBSTR(RPT_VALUE_2,1,2)								  
		  AND APDK_KIND_ID = :as_kind_id
";
            }
            else {
                sql =
@"
SELECT COD_SEQ_NO as li_prod_seq
		FROM ci.COD,ci.RPT 
		WHERE COD_TXN_ID = '40071'
			 AND COD_COL_ID = 'SUBTYPE'
		  AND RPT_TXN_ID = '40071'
		  AND RPT_TXD_ID = 'SUBTYPE'
		  AND RPT_SEQ_NO = COD_SEQ_NO
		  AND RPT_VALUE = :as_prod_subtype
";
            }

            DataTable dtResult = db.GetDataTable(sql, parms);

            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                return dtResult.Rows[0]["LI_PROD_SEQ"].AsInt();
            }
        }

        public DataTable checkProd(string ls_kind_id) {

            object[] parms = {
                ":ls_kind_id", ls_kind_id
            };

            string sql =
@"
				SELECT count(*) as li_count,
                   MAX(trim(APDK_CURRENCY_TYPE)) as ls_currency_type,
                   MAX(trim(APDK_STOCK_ID)) as ls_stock_id_ck
				FROM ci.APDK 
				WHERE APDK_KIND_ID = :ls_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable dddw_pdk_kind_id_40074(string as_param_key) {

            object[] parms = {
                ":as_param_key", as_param_key
            };

            string sql =
@"
SELECT APDK_KIND_ID AS KIND_ID,
       APDK_NAME,
       APDK_PROD_TYPE AS PROD_TYPE,
       APDK_PARAM_KEY AS PARAM_KEY,
       MGT2_SEQ_NO AS SEQ_NO
  FROM ci.APDK,ci.MGT2
 WHERE APDK_PROD_SUBTYPE = 'S'
   AND APDK_PARAM_KEY LIKE :as_param_key
   AND APDK_PARAM_KEY = MGT2_KIND_ID
 ORDER BY apdk_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
