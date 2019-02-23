using OnePiece;
using System.Data;

/// <summary>
/// Winni 2019/01/24
/// </summary>
namespace DataObjects.Dao.Together {
   public class D30640 {
      private Db db;

      public D30640() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// Get data by CI.RPT CI.AI2 (已經固定一些過濾條件)  (AM2_PARAM_KEY/AM2_IDFG_TYPE/AM2_OI/RPT_SEQ_NO)
      /// </summary>
      /// <param name="as_market_code">全部/一般/盤後</param>
      /// <param name="as_prev_sym">前期起始日</param>
      /// <returns></returns>
      public DataTable GetData(string as_sym , string as_eym) {
         object[] parms =
         {
                ":as_sym", as_sym,
                ":as_eym", as_eym
            };

         string sql = @"
SELECT AM2_PARAM_KEY,AM2_IDFG_TYPE,AM2_OI,RPT_SEQ_NO                    
FROM                                                                    
	(SELECT AM2_PARAM_KEY,AM2_IDFG_TYPE,SUM(AM2_OI) as AM2_OI               
	FROM ci.AM2                                                           
	WHERE AM2_YMD >= :as_sym                                      
	and AM2_YMD <=   :as_eym                                           
	AND AM2_SUM_SUBTYPE = '3'                                            
	--AND AM2_PROD_SUBTYPE <> 'S'                                          
	AND AM2_IDFG_TYPE IN ('1','2','3','4','5','6','7','8')               
	GROUP BY AM2_PARAM_KEY,AM2_IDFG_TYPE                                   
/*
20150720
      union all                                                               
      SELECT CASE WHEN AM2_PROD_TYPE = 'F' THEN 'STF     ' ELSE 'STC    ' END, 
             AM2_IDFG_TYPE,SUM(AM2_OI) AS AM2_OI                        
      FROM CI.AM2                                                           
      WHERE AM2_YMD >= :as_sym                                             
      and AM2_YMD <=  :as_eym                                                
      AND AM2_SUM_SUBTYPE = '2'                                            
      AND AM2_PROD_SUBTYPE = 'S'                                           
      AND AM2_IDFG_TYPE IN ('1','2','3','4','5','6','7','8')               
      GROUP BY AM2_PROD_TYPE,AM2_PROD_SUBTYPE,AM2_IDFG_TYPE
*/
 ),
    (SELECT RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXD_ID = '30640')                                                          
	 WHERE TRIM(AM2_PARAM_KEY)= TRIM(RPT_VALUE(+) )   
ORDER BY AM2_PARAM_KEY , AM2_IDFG_TYPE 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
