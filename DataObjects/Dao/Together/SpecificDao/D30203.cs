using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/13
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {

    public class D30203 : DataGate {

        /// <summary>
        /// Table: PL1
        /// data for gcMain
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30203(string as_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd
            };

            string sql =
@"
SELECT PL1_YMD AS PL2_EFFECTIVE_YMD,
         PL1_YMD,
         PL1_KIND_ID,   
         PL1_NATURE,   
         PL1_LEGAL,   
         PL1_999,
         PL1_NATURE_ADJ ,
         PL1_LEGAL_ADJ ,
         PL1_999_ADJ ,
         PL1_CUR_NATURE,   
         PL1_CUR_LEGAL,    
         PL1_CUR_999,   
         PL1_CP_NATURE,   
         PL1_CP_LEGAL,   
         PL1_MAX_MONTH_CNT,   
         PL1_MAX_TYPE,   
         PL1_MAX_QNTY,   
         PL1_NATURE as PL1_NATURE_ORG,   
         PL1_LEGAL as PL1_LEGAL_ORG,   
         PL1_NATURE_ADJ as PL1_NATURE_ADJ_ORG,   
         PL1_LEGAL_ADJ as PL1_LEGAL_ADJ_ORG,
         RPT_SEQ_NO,
         ' ' AS OP_TYPE ,
         PL1_PREV_AVG_QNTY,PL1_PREV_AVG_OI,PL1_AVG_QNTY,PL1_AVG_OI,PL1_CHANGE_RANGE,PL1_CP_999,PL1_PROD_TYPE,PL1_PROD_SUBTYPE,PL1_UPD_TIME,PL1_UPD_USER_ID,
         case when PL1_NATURE_ADJ =' ' and  PL1_LEGAL_ADJ  = ' ' then '不適用' 
         else '近'|| to_char(pl1_max_month_cnt) || '個月'|| (case when pl1_max_type = 'OI' then '未平倉量' else '交易量' || '(' || to_char(pl1_max_qnty,'9,999,999') || ')'end) end as COMPUTE_1
    FROM ci.PL1,
         (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30202' and RPT_TXD_ID = '30202') R  
   WHERE PL1_YMD = :as_ymd    
     and trim(PL1_KIND_ID) = trim(RPT_VALUE(+))
   ORDER BY RPT_SEQ_NO
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// Table: PL1B
        /// data for gcGBF
        /// </summary>
        /// <returns></returns>
        public DataTable d_30203_gbf() {

            string sql =
@"
SELECT '        ' AS PL2_EFFECTIVE_YMD,
       '        ' AS PL1B_YMD,
       PL1B_PROD_TYPE,
       PL1B_PROD_SUBTYPE,
       PL1B_KIND_ID,   
       PL1B_NATURE_LEGAL_MTH,
       PL1B_NATURE_LEGAL_TOT,
       PL1B_999_MTH,
       PL1B_999_NEARBY_MTH,
       PL1B_999_TOT,
       PL1B_NATURE_LEGAL_MTH as PL1B_PREV_NATURE_LEGAL_MTH,
       PL1B_NATURE_LEGAL_TOT as PL1B_PREV_NATURE_LEGAL_TOT,
       PL1B_999_MTH as PL1B_PREV_999_MTH ,
       PL1B_999_NEARBY_MTH as PL1B_PREV_999_NEARBY_MTH,
       PL1B_999_TOT as PL1B_PREV_999_TOT,
       ' ' AS PL1B_ADJ
FROM ci.PL1B
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public DataTable PostDate(string ls_ymd) {

            object[] parms = {
                ":ls_ymd", ls_ymd
            };

            string sql =
@"
select max(case when PL2_NATURE_ADJ = '-' then PL2_EFFECTIVE_YMD else ' ' end) as LOWER_YMD,
           max(case when PL2_NATURE_ADJ <> '-' then PL2_EFFECTIVE_YMD else ' ' end) as RAISE_YMD,
         count(*) as LI_COUNT
  --into :ls_eff_ymd_lower,:ls_eff_ymd,:li_count
  from ci.PL2
 where PL2_YMD = :ls_ymd
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// Table: PL2
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_30203_pl2(string as_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd
            };

            string sql =
@"
SELECT
    PL2_EFFECTIVE_YMD,
    PL2_YMD,
    PL2_KIND_ID,
    PL2_NATURE,
    PL2_LEGAL,
    PL2_999,
    PL2_NATURE_ADJ,
    PL2_LEGAL_ADJ,
    PL2_999_ADJ,
    PL2_PREV_NATURE,
    PL2_PREV_LEGAL,
    PL2_PREV_999,
    PL2_W_TIME,
    PL2_W_USER_ID
FROM CI.PL2
WHERE PL2_YMD=:AS_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// Table: PL2B
        /// </summary>
        /// <param name="as_ymd"yyyyMMdd></param>
        /// <returns></returns>
        public DataTable d_30203_pl2b(string as_ymd) {

            object[] parms = {
                ":as_ymd", as_ymd
            };

            string sql =
@"
SELECT
    PL2B_EFFECTIVE_YMD, 
    PL2B_YMD, 
    PL2B_PROD_TYPE, 
    PL2B_PROD_SUBTYPE, 
    PL2B_KIND_ID, 
    PL2B_NATURE_LEGAL_MTH, 
    PL2B_NATURE_LEGAL_TOT, 
    PL2B_999_MTH, 
    PL2B_999_NEARBY_MTH, 
    PL2B_999_TOT, 
    PL2B_PREV_NATURE_LEGAL_MTH,
    PL2B_PREV_NATURE_LEGAL_TOT,
    PL2B_PREV_999_MTH, 
    PL2B_PREV_999_NEARBY_MTH, 
    PL2B_PREV_999_TOT, 
    PL2B_ADJ, 
    PL2B_W_TIME, 
    PL2B_W_USER_ID
FROM CI.PL2B
WHERE PL2B_YMD = :AS_YMD
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// Table: PLLOG
        /// </summary>
        /// <returns></returns>
        public DataTable d_30203_pllog() {

            string sql =
@"
SELECT
    PLLOG_YMD, 
    PLLOG_KIND_ID, 
    PLLOG_DATA_TYPE, 
    PLLOG_ORG_VALUE, 
    PLLOG_UPD_VALUE, 
    PLLOG_W_TIME, 
    PLLOG_W_USER_ID
FROM CI.PLLOG
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public DataTable ProdType(string ls_kind_id) {

            object[] parms = {
                ":ls_kind_id", ls_kind_id
            };

            string sql =
@"
select nvl(APDK_PROD_TYPE,' ') as prod_type,
       nvl(APDK_PROD_SUBTYPE,' ') as  prod_subtype
--into :ls_prod_type,:ls_prod_subtype
from ci.APDK
where APDK_KIND_ID = :ls_kind_id;
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 刪除PL2的資料
        /// </summary>
        /// <param name="ls_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public bool DeletePL2ByDate(string ls_ymd) {
            object[] parms =
            {
                ":ls_ymd", ls_ymd
            };

            #region sql

            string sql =
@"
delete ci.PL2
where PL2_YMD = :ls_ymd
";

            #endregion sql

            try {
                int executeResult = db.ExecuteSQL(sql, parms);

                if (executeResult >= 0) {
                    return true;
                }
                else {
                    return false;
                    //throw new Exception("PL2刪除失敗");
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// 刪除PL2的資料
        /// </summary>
        /// <param name="ls_ymd">yyyyMMdd</param>
        /// <returns></returns>
        public bool DeletePL2BByDate(string ls_ymd) {
            object[] parms =
            {
                ":ls_ymd", ls_ymd
            };

            #region sql

            string sql =
@"
delete ci.PL2B
where PL2B_YMD = :ls_ymd
";

            #endregion sql
            try {
                int executeResult = db.ExecuteSQL(sql, parms);

                if (executeResult >= 0) {
                    return true;
                }
                else {
                    return false;
                    //throw new Exception("PL2B刪除失敗");
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public ResultData updatePLLOG(DataTable inputData) {

            string tableName = "CI.PLLOG";
            string keysColumnList = "PLLOG_YMD, PLLOG_KIND_ID, PLLOG_DATA_TYPE, PLLOG_W_TIME";
            string insertColumnList = @"PLLOG_YMD, 
                                        PLLOG_KIND_ID, 
                                        PLLOG_DATA_TYPE,
                                        PLLOG_ORG_VALUE,
                                        PLLOG_UPD_VALUE,
                                        PLLOG_W_TIME,
                                        PLLOG_W_USER_ID";
            string updateColumnList = insertColumnList;
            try {
                //update to DB
                return SaveForAll(inputData, tableName, insertColumnList, updateColumnList, keysColumnList);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public ResultData updatePL2(DataTable inputData) {
            string sql = @"
SELECT 
    PL2_EFFECTIVE_YMD,
    PL2_YMD,    
    PL2_KIND_ID,      
    PL2_NATURE,       
    PL2_LEGAL,        
    PL2_999,          
    PL2_NATURE_ADJ,   
    PL2_LEGAL_ADJ,    
    PL2_999_ADJ,      
    PL2_PREV_NATURE,  
    PL2_PREV_LEGAL,   
    PL2_PREV_999,     
    PL2_W_TIME,       
    PL2_W_USER_ID    
FROM CI.PL2";

            return db.UpdateOracleDB(inputData, sql);
        }

        public ResultData updatePL2B(DataTable inputData) {
            string sql = @"
SELECT 
    PL2B_EFFECTIVE_YMD,        
    PL2B_YMD,                 
    PL2B_PROD_TYPE,            
    PL2B_PROD_SUBTYPE,         
    PL2B_KIND_ID,              
    PL2B_NATURE_LEGAL_MTH,     
    PL2B_NATURE_LEGAL_TOT,     
    PL2B_999_MTH,              
    PL2B_999_NEARBY_MTH,       
    PL2B_999_TOT,              
    PL2B_ADJ,                  
    PL2B_PREV_NATURE_LEGAL_MTH,
    PL2B_PREV_NATURE_LEGAL_TOT,
    PL2B_PREV_999_MTH,         
    PL2B_PREV_999_NEARBY_MTH,  
    PL2B_PREV_999_TOT,        
    PL2B_W_TIME,               
    PL2B_W_USER_ID            
FROM CI.PL2B";

            return db.UpdateOracleDB(inputData, sql);
        }

        public ResultData updatePL1(DataTable inputData) {
            string sql = @"
SELECT 
PL1_YMD,          
PL1_PROD_TYPE,    
PL1_PROD_SUBTYPE, 
PL1_KIND_ID,      
PL1_PREV_AVG_QNTY,

PL1_PREV_AVG_OI,  
PL1_AVG_QNTY,     
PL1_AVG_OI,       
PL1_CHANGE_RANGE, 
PL1_CUR_NATURE, 

PL1_CUR_LEGAL,    
PL1_CUR_999,      
PL1_CP_NATURE,    
PL1_CP_LEGAL,     
PL1_CP_999,    

PL1_MAX_MONTH_CNT,
PL1_MAX_TYPE,     
PL1_MAX_QNTY,     
PL1_NATURE,       
PL1_LEGAL,   

PL1_999,          
PL1_NATURE_ADJ,   
PL1_LEGAL_ADJ,    
PL1_999_ADJ,      
PL1_UPD_TIME,    

PL1_UPD_USER_ID  

FROM CI.PL1";

            return db.UpdateOracleDB(inputData, sql);
        }

    }
}
