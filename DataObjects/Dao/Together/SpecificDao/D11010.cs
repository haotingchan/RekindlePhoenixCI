using OnePiece;
using System;
using System.Data;

/// <summary>
/// john,20190128
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   /// <summary>
   /// 
   /// </summary>
   public class D11010
   {

      private Db db;

      public D11010()
      {

         db = GlobalDaoSetting.DB;

      }
      /// <summary>
      /// AM0
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable ListAM0(string as_symd, string as_eymd, string as_sum_type, string as_prod_type)
      {

         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type
            };
         string sql = @"SELECT CI.AM0.AM0_BRK_NO4,   
                              CI.AM0.AM0_BRK_TYPE,  
                              CI.AM0.AM0_YMD, 
                              decode(CI.AM0.AM0_PARAM_KEY,'STO    ','STC    ',CI.AM0.AM0_PARAM_KEY) AS AM0_PARAM_KEY,
                              sum(CI.AM0.AM0_M_QNTY) as qnty
                         FROM CI.AM0  
                        WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                              ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                              ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                              ( CI.AM0.AM0_PROD_TYPE = :as_prod_type )   
                     GROUP BY CI.AM0.AM0_BRK_NO4,   
                              CI.AM0.AM0_BRK_TYPE , 
                              CI.AM0.AM0_YMD  ,  
                              decode(CI.AM0.AM0_PARAM_KEY,'STO    ','STC    ',CI.AM0.AM0_PARAM_KEY) 
                     union
                       SELECT CI.AM0.AM0_BRK_NO4,   
                              CI.AM0.AM0_BRK_TYPE,  
                              '99999999', 
                              decode(CI.AM0.AM0_PARAM_KEY,'STO    ','STC    ',CI.AM0.AM0_PARAM_KEY) ,
                              sum(CI.AM0.AM0_M_QNTY) as qnty
                         FROM CI.AM0  
                        WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                              ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                              ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                              ( CI.AM0.AM0_PROD_TYPE = :as_prod_type )   
                     GROUP BY CI.AM0.AM0_BRK_NO4,   
                              CI.AM0.AM0_BRK_TYPE ,
                              decode(CI.AM0.AM0_PARAM_KEY,'STO    ','STC    ',CI.AM0.AM0_PARAM_KEY) 
                     ORDER BY am0_ymd,am0_brk_no4,am0_brk_type,am0_param_key
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
   }
}
