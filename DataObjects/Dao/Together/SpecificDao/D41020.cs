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
    public class D41020: DataGate {

        /// <summary>
        /// table: CI.PCP
        /// </summary>
        /// <param name="as_prod_type">F or O</param>
        /// <param name="as_sdate">yyyy/MM/dd</param>
        /// <param name="as_edate">yyyy/MM/dd</param>
        /// <param name="as_kind_id">商品</param>
        /// <returns></returns>
        public DataTable d_41020(string as_prod_type, DateTime as_sdate, DateTime as_edate, string as_kind_id) {

            object[] parms = {
                ":as_prod_type", as_prod_type,
                ":as_sdate", as_sdate,
                ":as_edate", as_edate,
                ":as_kind_id", as_kind_id
            };

            string sql =
    @"
    SELECT 
         to_char(CI.PCP.PCP_DATE, 'yyyy/mm/dd hh24:mi:ss') as PCP_DATE,   
         CI.PCP.PCP_PROD_TYPE,   
         CI.PCP.PDK_KIND_ID,     
         CI.PCP.PDK_STOCK_ID,      
         CI.PCP.PDK_STOCK_QNTY,  
         CI.PCP.PDK_C_LAST_SETTLE_PRICE,  
         CI.PCP.PDK_STOCK_CASH2,   
         CI.PCP.PDK_STOCK_DATE3,  
         CI.PCP.PDK_STOCK_PRICE3,   
         CI.PCP.PDK_STOCK_QNTY3,    
         CI.PCP.SFD_LAST_PRICE,  
         CI.PCP.SDI_DISINVEST_RATE, 
         CI.PCP.SDI_COMB_RATE,    
         CI.PCP.PDK_STOCK_CASH3,   
         CI.PCP.PCP_LAST_SETTLE_PRICE,   
         CI.PCP.PCP_ADJ_LAST_SETTLE_PRICE,
         TRUNC(case when SDI_DISINVEST_RATE > 0  then PCP_ADJ_LAST_SETTLE_PRICE else PCP_ADJ_LAST_SETTLE_PRICE end) V,
         CI.PCP.PCP_INCREASE_PRICE,  
         TRUNC(PCP_INCREASE_PRICE) H,    
         CI.PCP.PCP_PRICE,    
         CI.PCP.PDK_TRADE_PAUSE,   
         CI.PCP.CVAR_VAR_CODE_4,   
         CI.PCP.PDK_STATUS_CODE 
    FROM CI.PCP  
   WHERE ( CI.PCP.PCP_PROD_TYPE = :as_prod_type ) AND  
         ( CI.PCP.PCP_DATE >= :as_sdate ) AND  
         ( CI.PCP.PCP_DATE <= :as_edate ) AND  
         ( CI.PCP.PDK_KIND_ID like :as_kind_id )
   ORDER BY pcp_date, pcp_prod_type, pdk_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
