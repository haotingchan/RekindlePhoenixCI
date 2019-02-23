using OnePiece;
using System;
using System.Data;

/// <summary>
/// john,20190128
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   /// <summary>
   /// 造市者交易量轉檔作業
   /// </summary>
   public class D70020
   {

      private Db db;

      public D70020()
      {

         db = GlobalDaoSetting.DB;

      }
      /// <summary>
      /// return RAMM1_BRK_TYPE/KIND_ID/BO/BQ/SO/SQ/IBQ/ISQ/MARKET_CODE
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_source"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable ListAll(string as_symd, string as_eymd, string as_source, string as_market_code)
      {

         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_source",as_source,
                ":as_market_code",as_market_code
            };
         string sql = @"SELECT RAMM1_BRK_TYPE,
                                 RAMM1_KIND_ID as KIND_ID,  
                                 SUM(RAMM1_BO_QNTY) as BO,   
                                 SUM(RAMM1_BQ_QNTY) as BQ,   
                                 SUM(RAMM1_SO_QNTY) as SO,   
                                 SUM(RAMM1_SQ_QNTY) as SQ,  
                                 SUM(RAMM1_BQ_QNTY_INVALID) as IBQ, 
                                 SUM(RAMM1_SQ_QNTY_INVALID) as ISQ,
                                 case when RAMM1_MARKET_CODE = '1' then '盤後' else '一般' end as MARKET_CODE
                            FROM ci.RAMM1  
                           WHERE RAMM1_YMD >= :as_symd    
                             AND RAMM1_YMD <= :as_eymd
                             AND RAMM1_SOURCE = :as_source    
                             AND RAMM1_SUM_TYPE = 'D'
                             AND RAMM1_MARKET_CODE LIKE :as_market_code
                        GROUP BY RAMM1_BRK_TYPE,RAMM1_KIND_ID,RAMM1_MARKET_CODE
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }//ListAll
      /// <summary>
      /// return AM8_YMD/AM8_PROD_TYPE/AM8_FCM_NO/AM8_PARAM_KEY/qnty_8/qnty_2/MARKET_CODE
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable ListAM8(string as_symd, string as_eymd,string as_market_code)
      {

         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_market_code",as_market_code
            };
         string sql = @"SELECT AM8_YMD,   
                              AM8_PROD_TYPE,   
                              AM8_FCM_NO,   
                              AM8_PARAM_KEY,   
                              sum(AM8_QNTY_8) as qnty_8,   
                              sum(AM8_QNTY_2) as qnty_2,  
                              case when AM8_MARKET_CODE = '1' then '盤後' else '一般' end as MARKET_CODE
                         FROM CI.AM8  
                        WHERE CI.AM8.AM8_YMD >= :as_symd
                          and CI.AM8.AM8_YMD <= :as_eymd 
                          and AM8_MARKET_CODE LIKE :as_market_code
                     group by AM8_YMD,   
                              AM8_PROD_TYPE,   
                              AM8_FCM_NO,   
                              AM8_PARAM_KEY,
                              AM8_MARKET_CODE
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }//ListAM8
      /// <summary>
      /// return RAMM1_BRK_TYPE/KIND_ID/BO/BQ/SO/SQ/IBQ/ISQ/MARKET_CODE
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_source"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable ListDel(string as_symd, string as_eymd, string as_source, string as_market_code)
      {

         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_source",as_source,
                ":as_market_code",as_market_code
            };
         string sql = @"SELECT RAMM1_BRK_TYPE,
                              RAMM1_KIND_ID as KIND_ID,  
                              SUM(RAMM1_BO_QNTY) as BO,   
                              SUM(RAMM1_BQ_QNTY) as BQ,   
                              SUM(RAMM1_SO_QNTY) as SO,   
                              SUM(RAMM1_SQ_QNTY) as SQ,  
                              SUM(RAMM1_BQ_QNTY_INVALID) as IBQ, 
                              SUM(RAMM1_SQ_QNTY_INVALID) as ISQ,
                              case when RAMM1_MARKET_CODE = '1' then '盤後' else '一般' end as MARKET_CODE
                         FROM ci.RAMM1  
                        WHERE RAMM1_YMD >= :as_symd    
                          AND RAMM1_YMD <= :as_eymd
                          AND RAMM1_SOURCE = :as_source    
                          AND RAMM1_SUM_TYPE = 'D'
                          AND RAMM1_MARKET_CODE LIKE :as_market_code
                     GROUP BY RAMM1_BRK_TYPE,RAMM1_KIND_ID,RAMM1_MARKET_CODE
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }//ListDel
      /// <summary>
      /// return RAMM1_BRK_TYPE/KIND_ID/BO/BQ/SO/SQ/IBQ/ISQ/MARKET_CODE
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_source"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable ListM(string as_symd, string as_eymd, string as_source, string as_market_code)
      {

         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_source",as_source,
                ":as_market_code",as_market_code
            };
         string sql = @"SELECT RAMM1_BRK_TYPE,
                                 RAMM1_KIND_ID as KIND_ID,  
                                 SUM(RAMM1_BO_QNTY) as BO,   
                                 SUM(RAMM1_BQ_QNTY) as BQ,   
                                 SUM(RAMM1_SO_QNTY) as SO,   
                                 SUM(RAMM1_SQ_QNTY) as SQ,  
                                 SUM(RAMM1_BQ_QNTY_INVALID) as IBQ, 
                                 SUM(RAMM1_SQ_QNTY_INVALID) as ISQ,
                                 case when RAMM1_MARKET_CODE = '1' then '盤後' else '一般' end as MARKET_CODE
                            FROM ci.RAMM1  
                           WHERE RAMM1_YMD >= :as_symd    
                             AND RAMM1_YMD <= :as_eymd
                             AND RAMM1_SOURCE = :as_source    
                             AND RAMM1_SUM_TYPE = 'D'
                             AND RAMM1_MARKET_CODE LIKE :as_market_code
                        GROUP BY RAMM1_BRK_TYPE,RAMM1_KIND_ID,RAMM1_MARKET_CODE
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }//ListM
      /// <summary>
      /// return RAMM1_BRK_TYPE/KIND_ID/BO/BQ/SO/SQ/IBQ/ISQ/MARKET_CODE
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_source"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable ListO(string as_symd, string as_eymd, string as_source, string as_market_code)
      {

         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_source",as_source,
                ":as_market_code",as_market_code
            };
         string sql = @"SELECT RAMM1_BRK_TYPE,
                                 RAMM1_KIND_ID as KIND_ID,  
                                 SUM(RAMM1_BO_QNTY) as BO,   
                                 SUM(RAMM1_BQ_QNTY) as BQ,   
                                 SUM(RAMM1_SO_QNTY) as SO,   
                                 SUM(RAMM1_SQ_QNTY) as SQ,  
                                 case when RAMM1_MARKET_CODE = '1' then '盤後' else '一般' end as MARKET_CODE
                            FROM ci.RAMM1  
                           WHERE RAMM1_YMD >= :as_symd    
                             AND RAMM1_YMD <= :as_eymd
                             AND RAMM1_SOURCE = :as_source    
                             AND RAMM1_SUM_TYPE = 'D'
                             AND RAMM1_MARKET_CODE LIKE :as_market_code
                        GROUP BY RAMM1_BRK_TYPE,RAMM1_KIND_ID,RAMM1_MARKET_CODE
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }//ListO

   }
}
