using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/20
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D43010: DataGate {

        /// <summary>
        /// 現行收取保證金金額
        /// </summary>
        /// <param name="as_date">yyyy/MM/dd</param>
        /// <param name="as_osw_grp">1%/ 5%/ %%</param>
        /// <returns></returns>
        public DataTable d_43010a(DateTime as_date, string as_osw_grp) {

            object[] parms = {
                ":as_date",as_date,
                ":as_osw_grp",as_osw_grp
            };

            string sql =
@"
SELECT   ROW_NUMBER() over (order by mg1_seq_no, mg1_kind_id, mg1_type, mg1_date) as NO,
         MG1_KIND_ID,
         APDK_NAME,
         APDK_STOCK_ID,
         PID_NAME,
         MG1_CUR_CM,
         MG1_CUR_MM,
         MG1_CUR_IM,
         MG1_CM_RATE,
         MG1_MM_RATE,
         MG1_IM_RATE
    FROM CI.MG1,   
         CI.APDK  ,
         --上市/上櫃中文名稱
         (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM')
   WHERE ( MG1_KIND_ID = CI.APDK.APDK_KIND_ID ) and  
         ( MG1_PROD_TYPE = CI.APDK.APDK_PROD_TYPE ) and  
         ( ( MG1_DATE = :as_date ) AND  
         ( MG1_PROD_SUBTYPE = 'S' ) AND  
         ( MG1_PROD_TYPE = 'F' ) )    
     and APDK_UNDERLYING_MARKET = COD_ID
     and MG1_OSW_GRP LIKE :as_osw_grp
   ORDER BY mg1_seq_no, mg1_kind_id, mg1_type, mg1_date
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 本日結算保證金計算
        /// </summary>
        /// <param name="as_date">yyyy/MM/dd</param>
        /// <param name="as_osw_grp">1%/ 5%/ %%</param>
        /// <returns></returns>
        public DataTable d_43010b(DateTime as_date, string as_osw_grp) {

            object[] parms = {
                ":as_date",as_date,
                ":as_osw_grp",as_osw_grp
            };

            string sql =
@"
SELECT   
         MG1_PRICE,
         MG1_XXX,
         MG1_RISK,
         MG1_CP_RISK,
         MG1_MIN_RISK,
         MG1_CP_CM
    FROM CI.MG1,   
         CI.APDK  ,
         --上市/上櫃中文名稱
         (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM')
   WHERE ( MG1_KIND_ID = CI.APDK.APDK_KIND_ID ) and  
         ( MG1_PROD_TYPE = CI.APDK.APDK_PROD_TYPE ) and  
         ( ( MG1_DATE = :as_date ) AND  
         ( MG1_PROD_SUBTYPE = 'S' ) AND  
         ( MG1_PROD_TYPE = 'F' ) )    
     and APDK_UNDERLYING_MARKET = COD_ID
     and MG1_OSW_GRP LIKE :as_osw_grp
   ORDER BY mg1_seq_no, mg1_kind_id, mg1_type, mg1_date
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 本日結算保證金變動幅度
        /// </summary>
        /// <param name="as_date">yyyy/MM/dd</param>
        /// <param name="as_osw_grp">1%/ 5%/ %%</param>
        /// <returns></returns>
        public DataTable d_43010c(DateTime as_date, string as_osw_grp) {

            object[] parms = {
                ":as_date",as_date,
                ":as_osw_grp",as_osw_grp
            };

            string sql =
@"
SELECT   
         MG1_CHANGE_RANGE,
         MG1_CHANGE_FLAG 
    FROM CI.MG1,   
         CI.APDK  ,
         --上市/上櫃中文名稱
         (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM')
   WHERE ( MG1_KIND_ID = CI.APDK.APDK_KIND_ID ) and  
         ( MG1_PROD_TYPE = CI.APDK.APDK_PROD_TYPE ) and  
         ( ( MG1_DATE = :as_date ) AND  
         ( MG1_PROD_SUBTYPE = 'S' ) AND  
         ( MG1_PROD_TYPE = 'F' ) )    
     and APDK_UNDERLYING_MARKET = COD_ID
     and MG1_OSW_GRP LIKE :as_osw_grp
   ORDER BY mg1_seq_no, mg1_kind_id, mg1_type, mg1_date
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
