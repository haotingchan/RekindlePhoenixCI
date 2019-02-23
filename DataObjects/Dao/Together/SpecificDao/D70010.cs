using OnePiece;
using System;
using System.Data;

/// <summary>
/// john,20190128
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   /// <summary>
   /// 交易量資料轉檔作業
   /// </summary>
   public class D70010
   {
      //test is_sum_type=D,is_kind_id2=%,is_param_key=TXO,is_prod_type=O
      private Db db;

      public D70010()
      {

         db = GlobalDaoSetting.DB;

      }
      /// <summary>
      /// d_70010
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable ListAll(string as_symd, string as_eymd, string as_sum_type, string as_prod_type)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type
            };
         string sql = @"SELECT AM0_BRK_NO4,   
                              AM0_BRK_TYPE,  
                              AM0_YMD, 
                              AM0_PARAM_KEY,
                              sum(AM0_M_QNTY) as qnty
                           FROM CI.AM0  
                        WHERE AM0_YMD >= :as_symd    
                           AND AM0_YMD <= :as_eymd   
                           AND AM0_SUM_TYPE  = :as_sum_type   
                           AND AM0_PROD_TYPE = :as_prod_type 
                     GROUP BY AM0_BRK_NO4,   
                              AM0_BRK_TYPE , 
                              AM0_YMD  ,  
                              AM0_PARAM_KEY
                     union all
                        SELECT AM0_BRK_NO4,   
                              AM0_BRK_TYPE,  
                              '99999999', 
                              AM0_PARAM_KEY,
                              sum(AM0_M_QNTY) as qnty
                           FROM CI.AM0  
                        WHERE AM0_YMD >= :as_symd     
                           AND AM0_YMD <= :as_eymd   
                           AND AM0_SUM_TYPE  = :as_sum_type   
                           AND AM0_PROD_TYPE = :as_prod_type 
                     GROUP BY AM0_BRK_NO4,   
                              AM0_BRK_TYPE ,
                              AM0_PARAM_KEY
                     ORDER BY am0_brk_no4,am0_brk_type,am0_ymd,am0_param_key;
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// return AM0_BRK_NO4/AM0_BRK_TYPE/sum(AM0.AM0_M_QNTY) as qnty/cp_rate
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable List70010brk(string as_symd, string as_eymd, string as_sum_type, string as_prod_type)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type
            };
         string sql = @"SELECT AM0_BRK_NO4,   
                              AM0_BRK_TYPE,
                              sum(AM0.AM0_M_QNTY) as qnty
                           FROM CI.AM0  
                        WHERE ( AM0_YMD >= :as_symd ) AND  
                              ( AM0_YMD <= :as_eymd ) AND  
                              ( AM0_SUM_TYPE  = :as_sum_type ) AND  
                              ( AM0_PROD_TYPE = :as_prod_type )   
                     GROUP BY AM0_BRK_NO4,   
                              AM0_BRK_TYPE
                     ORDER BY qnty Desc,am0_brk_no4,am0_brk_type
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         if (dtResult.Rows.Count <= 0) return dtResult;
         dtResult.Columns.Add("cp_rate", typeof(decimal));
         int cp_sum_qnty = Convert.ToInt32(dtResult.Compute("sum(qnty)", ""));

         foreach (DataRow dr in dtResult.Rows) {
            dr["cp_rate"] = Math.Round(Convert.ToDouble(dr["qnty"].ToString()) / cp_sum_qnty * 100, 2);
         }
         dtResult.AcceptChanges();
         return dtResult;
      }
      /// <summary>
      /// return AM0_BRK_NO4/AM0_BRK_TYPE/qnty/cp_rate
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable List70010brkByMarketCode(
         string as_symd, string as_eymd, string as_sum_type, string as_prod_type, string as_market_code)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type,
                ":as_market_code",as_market_code
            };
         string sql = @"
                        SELECT CI.AM0.AM0_BRK_NO4,   
                        CI.AM0.AM0_BRK_TYPE,
                        sum(case :as_market_code when  '1' then AM0_AH_M_QNTY 
                              when '0' then AM0_M_QNTY - nvl(AM0_AH_M_QNTY,0) 
                              else AM0_M_QNTY end ) as qnty
                        FROM CI.AM0  
                        WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                              ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                              ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                              ( CI.AM0.AM0_PROD_TYPE = :as_prod_type )   
                        AND (:as_market_code <> '1'  or NOT AM0_AH_M_QNTY IS NULL)
                        GROUP BY CI.AM0.AM0_BRK_NO4,   
                        CI.AM0.AM0_BRK_TYPE 
                        ORDER BY qnty Desc,am0_brk_no4,am0_brk_type
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         if (dtResult.Rows.Count <= 0) return dtResult;
         dtResult.Columns.Add("cp_rate", typeof(decimal));
         int cp_sum_qnty = Convert.ToInt32(dtResult.Compute("sum(qnty)", ""));

         foreach (DataRow dr in dtResult.Rows) {
            dr["cp_rate"] = Math.Round(Convert.ToDouble(dr["qnty"].ToString()) / cp_sum_qnty * 100, 2);
         }
         dtResult.AcceptChanges();
         return dtResult;
      }
      /// <summary>
      /// return B.AM0_BRK_NO4/B.AM0_BRK_TYPE/NVL(qnty,0) AS QNTY/cp_rate
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable List70010brkYear(string as_symd, string as_eymd, string as_sum_type, string as_prod_type)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type
            };
         string sql = @"
                        SELECT B.AM0_BRK_NO4,B.AM0_BRK_TYPE,NVL(qnty,0) AS QNTY
                        FROM
                        ( SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE
                            FROM CI.AM0  
                           WHERE ( AM0_YMD >= :as_symd ) AND  
                                 ( AM0_YMD <= :as_eymd ) AND  
                                 ( AM0_SUM_TYPE  = :as_sum_type ) AND  
                                 ( AM0_PROD_TYPE = :as_prod_type )   
                        GROUP BY CI.AM0.AM0_BRK_NO4,   
                                 CI.AM0.AM0_BRK_TYPE) B,
                        ( SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,
                                 sum(CI.AM0.AM0_M_QNTY) as qnty
                            FROM CI.AM0  
                           WHERE ( AM0_YMD >= :as_eymd ) AND  
                                 ( AM0_YMD <= :as_eymd ) AND  
                                 ( AM0_SUM_TYPE  = :as_sum_type ) AND  
                                 ( AM0_PROD_TYPE = :as_prod_type )   
                        GROUP BY CI.AM0.AM0_BRK_NO4,   
                                 CI.AM0.AM0_BRK_TYPE) M
                        WHERE B.AM0_BRK_NO4 = M.AM0_BRK_NO4(+)
                          AND B.AM0_BRK_TYPE = M.AM0_BRK_TYPE(+)
                        ORDER BY qnty Desc ,am0_brk_no4,am0_brk_type 
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         if (dtResult.Rows.Count <= 0) return dtResult;
         dtResult.Columns.Add("cp_rate", typeof(decimal));
         int cp_sum_qnty = Convert.ToInt32(dtResult.Compute("sum(qnty)", ""));

         foreach (DataRow dr in dtResult.Rows) {
            dr["cp_rate"] = Math.Round(Convert.ToDouble(dr["qnty"].ToString()) / cp_sum_qnty * 100, 2);
         }
         dtResult.AcceptChanges();
         return dtResult;
      }
      /// <summary>
      /// return AM0_BRK_NO4/AM0_BRK_TYPE/NVL(qnty,0) AS QNTY/cp_rate
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable List70010brkYearByMarketCode(
         string as_symd, string as_eymd, string as_sum_type, string as_prod_type, string as_market_code)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type,
                ":as_market_code",as_market_code
            };
         string sql = @"
                        SELECT B.AM0_BRK_NO4,B.AM0_BRK_TYPE,NVL(qnty,0) AS QNTY
                        FROM
                        ( SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE
                            FROM CI.AM0  
                           WHERE ( AM0_YMD >= :as_symd ) AND  
                                 ( AM0_YMD <= :as_eymd ) AND  
                                 ( AM0_SUM_TYPE  = :as_sum_type ) AND  
                                 ( AM0_PROD_TYPE = :as_prod_type )   
                             AND (:as_market_code <> '1'  or NOT AM0_AH_M_QNTY IS NULL)
                        GROUP BY CI.AM0.AM0_BRK_NO4,   
                                 CI.AM0.AM0_BRK_TYPE) B,
                        ( SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,
                                 sum(case :as_market_code when  '1' then AM0_AH_M_QNTY 
                                                                             when '0' then AM0_M_QNTY - nvl(AM0_AH_M_QNTY,0) 
                                                                             else AM0_M_QNTY end ) as qnty
                            FROM CI.AM0  
                           WHERE ( AM0_YMD >= :as_eymd ) AND  
                                 ( AM0_YMD <= :as_eymd ) AND  
                                 ( AM0_SUM_TYPE  = :as_sum_type ) AND  
                                 ( AM0_PROD_TYPE = :as_prod_type )   
                             AND (:as_market_code <> '1'  or NOT AM0_AH_M_QNTY IS NULL)
                        GROUP BY CI.AM0.AM0_BRK_NO4,   
                                 CI.AM0.AM0_BRK_TYPE) M
                        WHERE B.AM0_BRK_NO4 = M.AM0_BRK_NO4(+)
                          AND B.AM0_BRK_TYPE = M.AM0_BRK_TYPE(+)
                        ORDER BY qnty Desc, am0_brk_no4,am0_brk_type
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         if (dtResult.Rows.Count <= 0) return dtResult;
         dtResult.Columns.Add("cp_rate", typeof(decimal));
         int cp_sum_qnty = Convert.ToInt32(dtResult.Compute("sum(qnty)", ""));

         foreach (DataRow dr in dtResult.Rows) {
            dr["cp_rate"] = Math.Round(Convert.ToDouble(dr["qnty"].ToString()) / cp_sum_qnty * 100, 2);
         }
         dtResult.AcceptChanges();
         return dtResult;
      }
      /// <summary>
      /// return AM0_BRK_NO4/AM0_BRK_TYPE/AM0_YMD/AM0_PARAM_KEY/sum(CI.AM0.AM0_M_QNTY) as qnty/cp_rate
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable List70010End(string as_symd, string as_eymd, string as_sum_type, string as_prod_type)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type
            };
         string sql = @"
                        SELECT CI.AM0.AM0_BRK_NO4,   
                              CI.AM0.AM0_BRK_TYPE,  
                              CI.AM0.AM0_YMD, 
                              CI.AM0.AM0_PARAM_KEY,
                              sum(CI.AM0.AM0_M_QNTY) as qnty
                           FROM CI.AM0  
                        WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                              ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                              ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                              ( CI.AM0.AM0_PROD_TYPE = :as_prod_type )   
                     GROUP BY CI.AM0.AM0_BRK_NO4,   
                              CI.AM0.AM0_BRK_TYPE , 
                              CI.AM0.AM0_YMD ,  
                              CI.AM0.AM0_PARAM_KEY
                     ORDER BY am0_brk_no4,am0_brk_type,am0_ymd,am0_param_key
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         if (dtResult.Rows.Count <= 0) return dtResult;
         dtResult.Columns.Add("cp_rate", typeof(decimal));
         int cp_sum_qnty = Convert.ToInt32(dtResult.Compute("sum(qnty)", ""));

         foreach (DataRow dr in dtResult.Rows) {
            dr["cp_rate"] = Math.Round(Convert.ToDouble(dr["qnty"].ToString()) / cp_sum_qnty * 100, 2);
         }
         dtResult.AcceptChanges();
         return dtResult;
      }
      /// <summary>
      /// ex: F000999,期交所,20181001,GTO,0
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable ListOpendata(string as_symd, string as_eymd, string as_sum_type, string as_prod_type)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type
            };
         string sql = @"
                        SELECT trim(T_BRK_NO4)||case when T_BRK_TYPE = '9' then '999' else '000' end ||','||
                              trim(ABRK_NAME)||','||
                              T_YMD||','||
                              trim(T_PARAM_KEY)||','||
                              to_char(NVL(qnty,0))
                         FROM ci.ABRK,
                      (SELECT AM0_BRK_NO4 as T_BRK_NO4,   
                              AM0_BRK_TYPE as T_BRK_TYPE,
                              AI2_YMD as T_YMD,
                              AI2_PARAM_KEY as T_PARAM_KEY
                         FROM
                             (SELECT AM0_BRK_NO4,   
                                     AM0_BRK_TYPE
                                FROM CI.AM0  
                               WHERE AM0_YMD >= :as_symd    
                                 AND AM0_YMD <= :as_eymd   
                                 AND AM0_SUM_TYPE  = :as_sum_type   
                                 AND AM0_PROD_TYPE = :as_prod_type
                               group by AM0_BRK_NO4,   
                                     AM0_BRK_TYPE ),
                             (SELECT AI2_PARAM_KEY
                                FROM CI.AI2
                               WHERE AI2_YMD >= :as_symd    
                                 AND AI2_YMD <= :as_eymd   
                                 AND AI2_SUM_TYPE  = :as_sum_type   
                                 AND AI2_SUM_SUBTYPE = '3'
                                 AND AI2_PROD_TYPE = :as_prod_type 
                                 AND AI2_PROD_SUBTYPE <> 'S'
                               GROUP BY  AI2_PARAM_KEY
                               UNION ALL
                               SELECT case when AI2_PROD_TYPE = 'F' then 'STF' else 'STC' end
                                FROM CI.AI2
                               WHERE AI2_YMD >= :as_symd    
                                 AND AI2_YMD <= :as_eymd   
                                 AND AI2_SUM_TYPE  = :as_sum_type   
                                 AND AI2_SUM_SUBTYPE = '3'
                                 AND AI2_PROD_TYPE = :as_prod_type 
                                 AND AI2_PROD_SUBTYPE = 'S'
                               GROUP BY AI2_PROD_TYPE
                               UNION ALL
                              SELECT '小計' FROM DUAL),
                             (SELECT AI2_YMD
                                FROM CI.AI2
                               WHERE AI2_YMD >= :as_symd    
                                 AND AI2_YMD <= :as_eymd   
                                 AND AI2_SUM_TYPE  = :as_sum_type   
                                 AND AI2_SUM_SUBTYPE = '3'
                                 AND AI2_PROD_TYPE = :as_prod_type
                               GROUP BY AI2_YMD
                               UNION ALL
                              SELECT '99999999' FROM DUAL)),                
                      (SELECT AM0_BRK_NO4,   
                              AM0_BRK_TYPE,  
                              AM0_YMD, 
                              AM0_PARAM_KEY,
                              sum(AM0_M_QNTY) as qnty
                         FROM CI.AM0  
                        WHERE AM0_YMD >= :as_symd    
                          AND AM0_YMD <= :as_eymd   
                          AND AM0_SUM_TYPE  = :as_sum_type   
                          AND AM0_PROD_TYPE = :as_prod_type 
                          and AM0_PROD_SUBTYPE <> 'S'
                     GROUP BY AM0_BRK_NO4,   
                              AM0_BRK_TYPE , 
                              AM0_YMD  ,  
                              AM0_PARAM_KEY
                     union all
                       SELECT AM0_BRK_NO4,   
                              AM0_BRK_TYPE,  
                              AM0_YMD, 
                              case when AM0_PROD_TYPE = 'F' then 'STF' else 'STC' end,
                              sum(AM0_M_QNTY) as qnty
                         FROM CI.AM0  
                        WHERE AM0_YMD >= :as_symd     
                          AND AM0_YMD <= :as_eymd   
                          AND AM0_SUM_TYPE  = :as_sum_type   
                          AND AM0_PROD_TYPE = :as_prod_type 
                          and AM0_PROD_SUBTYPE = 'S'
                     GROUP BY AM0_BRK_NO4,   
                              AM0_BRK_TYPE , 
                              AM0_YMD  ,  
                              AM0_PROD_TYPE       
                     union all  
                       SELECT AM0_BRK_NO4,   
                              AM0_BRK_TYPE,  
                              AM0_YMD, 
                              '小計',
                              sum(AM0_M_QNTY) as qnty
                         FROM CI.AM0  
                        WHERE AM0_YMD >= :as_symd    
                          AND AM0_YMD <= :as_eymd   
                          AND AM0_SUM_TYPE  = :as_sum_type   
                          AND AM0_PROD_TYPE = :as_prod_type 
                     GROUP BY AM0_BRK_NO4,   
                              AM0_BRK_TYPE , 
                              AM0_YMD                  
                     union all
                       SELECT AM0_BRK_NO4,   
                              AM0_BRK_TYPE,  
                              '99999999', 
                              AM0_PARAM_KEY,
                              sum(AM0_M_QNTY) as qnty
                         FROM CI.AM0  
                        WHERE AM0_YMD >= :as_symd     
                          AND AM0_YMD <= :as_eymd   
                          AND AM0_SUM_TYPE  = :as_sum_type   
                          AND AM0_PROD_TYPE = :as_prod_type 
                          and AM0_PROD_SUBTYPE <> 'S'
                     GROUP BY AM0_BRK_NO4,   
                              AM0_BRK_TYPE ,
                              AM0_PARAM_KEY
                     union all
                       SELECT AM0_BRK_NO4,   
                              AM0_BRK_TYPE,  
                              '99999999', 
                              case when AM0_PROD_TYPE = 'F' then 'STF' else 'STC' end,
                              sum(AM0_M_QNTY) as qnty
                         FROM CI.AM0  
                        WHERE AM0_YMD >= :as_symd     
                          AND AM0_YMD <= :as_eymd   
                          AND AM0_SUM_TYPE  = :as_sum_type   
                          AND AM0_PROD_TYPE = :as_prod_type 
                          and AM0_PROD_SUBTYPE = 'S'  
                     GROUP BY AM0_BRK_NO4,   
                              AM0_BRK_TYPE ,
                              AM0_PROD_TYPE
                     union all
                       SELECT AM0_BRK_NO4,   
                              AM0_BRK_TYPE,  
                              '99999999', 
                              '小計',
                              sum(AM0_M_QNTY) as qnty
                         FROM CI.AM0  
                        WHERE AM0_YMD >= :as_symd     
                          AND AM0_YMD <= :as_eymd   
                          AND AM0_SUM_TYPE  = :as_sum_type   
                          AND AM0_PROD_TYPE = :as_prod_type 
                     GROUP BY AM0_BRK_NO4,   
                              AM0_BRK_TYPE)
                     where ABRK_NO = trim(T_BRK_NO4)||case when T_BRK_TYPE = '9' then '999' else '000' end
                       AND T_BRK_NO4 = AM0_BRK_NO4(+)
                       AND T_BRK_TYPE = AM0_BRK_TYPE(+)
                       AND T_YMD = AM0_YMD(+)
                       AND T_PARAM_KEY = AM0_PARAM_KEY(+)
                     ORDER BY T_BRK_NO4,T_BRK_TYPE,T_YMD,T_PARAM_KEY
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// ex: F000999,期交所,20181001,GTO,0
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable ListOpendataYear(string as_symd, string as_eymd, string as_sum_type, string as_prod_type)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type
            };
         string sql = @"
                        SELECT trim(T_BRK_NO4)||case when T_BRK_TYPE = '9' then '999' else '000' end ||','||
                                 trim(ABRK_NAME)||','||
                                 T_YMD||','||
                                 trim(T_PARAM_KEY)||','||
                                 to_char(NVL(qnty,0))
                            FROM ci.ABRK,
                         (SELECT AM0_BRK_NO4 as T_BRK_NO4,   
                                 AM0_BRK_TYPE as T_BRK_TYPE,
                                 AI2_YMD as T_YMD,
                                 AI2_PARAM_KEY as T_PARAM_KEY
                            FROM
                                (SELECT AM0_BRK_NO4,   
                                        AM0_BRK_TYPE
                                   FROM CI.AM0  
                                  WHERE AM0_YMD >= :as_symd    
                                    AND AM0_YMD <= :as_eymd   
                                    AND AM0_SUM_TYPE  = :as_sum_type   
                                    AND AM0_PROD_TYPE = :as_prod_type
                                  group by AM0_BRK_NO4,   
                                        AM0_BRK_TYPE ),
                                (SELECT AI2_PARAM_KEY
                                   FROM CI.AI2
                                  WHERE AI2_YMD >= trim(:as_symd)||'01  '
                                    AND AI2_YMD <= trim(:as_eymd)||'12  '  
                                    AND AI2_SUM_TYPE  = 'M'  
                                    AND AI2_SUM_SUBTYPE = '3'
                                    AND AI2_PROD_TYPE = :as_prod_type 
                                    AND AI2_PROD_SUBTYPE <> 'S'
                                  GROUP BY AI2_PARAM_KEY
                                  UNION ALL
                                 SELECT case when AI2_PROD_TYPE = 'F' then 'STF' else 'STC' end
                                   FROM CI.AI2
                                  WHERE AI2_YMD >= trim(:as_symd)||'01  '
                                    AND AI2_YMD <= trim(:as_eymd)||'12  '  
                                    AND AI2_SUM_TYPE  = 'M'  
                                    AND AI2_SUM_SUBTYPE = '3'
                                    AND AI2_PROD_TYPE = :as_prod_type 
                                    AND AI2_PROD_SUBTYPE = 'S'
                                  GROUP BY AI2_PROD_TYPE
                                  UNION ALL
                                 SELECT '小計' FROM DUAL),
                                (SELECT substr(AI2_YMD,1,4)||'    '  as AI2_YMD
                                   FROM CI.AI2
                                  WHERE AI2_YMD >= trim(:as_symd)||'01  '
                                    AND AI2_YMD <= trim(:as_eymd)||'12  '  
                                    AND AI2_SUM_TYPE  = 'M'   
                                    AND AI2_SUM_SUBTYPE = '3'
                                    AND AI2_PROD_TYPE = :as_prod_type 
                                    AND AI2_PROD_SUBTYPE = 'S'
                                  GROUP BY substr(AI2_YMD,1,4))),                
                         (SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,  
                                 AM0_YMD, 
                                 AM0_PARAM_KEY,
                                 sum(AM0_M_QNTY) as qnty
                            FROM CI.AM0  
                           WHERE AM0_YMD >= :as_symd    
                             AND AM0_YMD <= :as_eymd   
                             AND AM0_SUM_TYPE  = :as_sum_type   
                             AND AM0_PROD_TYPE = :as_prod_type 
                             and AM0_PROD_SUBTYPE <> 'S'
                        GROUP BY AM0_BRK_NO4,   
                                 AM0_BRK_TYPE , 
                                 AM0_YMD  ,  
                                 AM0_PARAM_KEY
                        union all
                          SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,  
                                 AM0_YMD, 
                                 case when AM0_PROD_TYPE = 'F' then 'STF' else 'STC' end,
                                 sum(AM0_M_QNTY) as qnty
                            FROM CI.AM0  
                           WHERE AM0_YMD >= :as_symd     
                             AND AM0_YMD <= :as_eymd   
                             AND AM0_SUM_TYPE  = :as_sum_type   
                             AND AM0_PROD_TYPE = :as_prod_type 
                             and AM0_PROD_SUBTYPE = 'S'
                        GROUP BY AM0_BRK_NO4,   
                                 AM0_BRK_TYPE , 
                                 AM0_YMD  ,  
                                 AM0_PROD_TYPE       
                        union all  
                          SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,  
                                 AM0_YMD, 
                                 '小計',
                                 sum(AM0_M_QNTY) as qnty
                            FROM CI.AM0  
                           WHERE AM0_YMD >= :as_symd    
                             AND AM0_YMD <= :as_eymd   
                             AND AM0_SUM_TYPE  = :as_sum_type   
                             AND AM0_PROD_TYPE = :as_prod_type 
                        GROUP BY AM0_BRK_NO4,   
                                 AM0_BRK_TYPE , 
                                 AM0_YMD )
                        where ABRK_NO = trim(T_BRK_NO4)||case when T_BRK_TYPE = '9' then '999' else '000' end
                          AND T_BRK_NO4 = AM0_BRK_NO4(+)
                          AND T_BRK_TYPE = AM0_BRK_TYPE(+)
                          AND T_YMD = AM0_YMD(+)
                          AND T_PARAM_KEY = AM0_PARAM_KEY(+)
                        ORDER BY T_BRK_NO4,T_BRK_TYPE,T_YMD,T_PARAM_KEY
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// ex: F000999,期交所,99999999,小計,27887
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable ListOpendataYearHaveTotal(string as_symd, string as_eymd, string as_sum_type, string as_prod_type)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type
            };
         string sql = @"
                        SELECT trim(T_BRK_NO4)||case when T_BRK_TYPE = '9' then '999' else '000' end ||','||
                                 trim(ABRK_NAME)||','||
                                 T_YMD||','||
                                 trim(T_PARAM_KEY)||','||
                                 to_char(NVL(qnty,0))
                            FROM ci.ABRK,
                         (SELECT AM0_BRK_NO4 as T_BRK_NO4,   
                                 AM0_BRK_TYPE as T_BRK_TYPE,
                                 AI2_YMD as T_YMD,
                                 AI2_PARAM_KEY as T_PARAM_KEY
                            FROM
                                (SELECT AM0_BRK_NO4,   
                                        AM0_BRK_TYPE
                                   FROM CI.AM0  
                                  WHERE AM0_YMD >= :as_symd    
                                    AND AM0_YMD <= :as_eymd   
                                    AND AM0_SUM_TYPE  = :as_sum_type   
                                    AND AM0_PROD_TYPE = :as_prod_type
                                  group by AM0_BRK_NO4,   
                                        AM0_BRK_TYPE ),
                                (SELECT AI2_PARAM_KEY
                                   FROM CI.AI2
                                  WHERE AI2_YMD >= trim(:as_symd)||'01  '
                                    AND AI2_YMD <= trim(:as_eymd)||'12  '  
                                    AND AI2_SUM_TYPE  = 'M'  
                                    AND AI2_SUM_SUBTYPE = '3'
                                    AND AI2_PROD_TYPE = :as_prod_type 
                                    AND AI2_PROD_SUBTYPE <> 'S'
                                  GROUP BY AI2_PARAM_KEY
                                  UNION ALL
                                 SELECT case when AI2_PROD_TYPE = 'F' then 'STF' else 'STC' end
                                   FROM CI.AI2
                                  WHERE AI2_YMD >= trim(:as_symd)||'01  '
                                    AND AI2_YMD <= trim(:as_eymd)||'12  '  
                                    AND AI2_SUM_TYPE  = 'M'  
                                    AND AI2_SUM_SUBTYPE = '3'
                                    AND AI2_PROD_TYPE = :as_prod_type 
                                    AND AI2_PROD_SUBTYPE = 'S'
                                  GROUP BY AI2_PROD_TYPE
                                  UNION ALL
                                 SELECT '小計' FROM DUAL),
                                (SELECT substr(AI2_YMD,1,4)||'    '  as AI2_YMD
                                   FROM CI.AI2
                                  WHERE AI2_YMD >= trim(:as_symd)||'01  '
                                    AND AI2_YMD <= trim(:as_eymd)||'12  '  
                                    AND AI2_SUM_TYPE  = 'M'   
                                    AND AI2_SUM_SUBTYPE = '3'
                                    AND AI2_PROD_TYPE = :as_prod_type 
                                  GROUP BY substr(AI2_YMD,1,4) 
                                  UNION ALL
                                 SELECT '99999999' FROM DUAL)),                
                         (SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,  
                                 AM0_YMD, 
                                 AM0_PARAM_KEY,
                                 sum(AM0_M_QNTY) as qnty
                            FROM CI.AM0  
                           WHERE AM0_YMD >= :as_symd    
                             AND AM0_YMD <= :as_eymd   
                             AND AM0_SUM_TYPE  = :as_sum_type   
                             AND AM0_PROD_TYPE = :as_prod_type 
                             and AM0_PROD_SUBTYPE <> 'S'
                        GROUP BY AM0_BRK_NO4,   
                                 AM0_BRK_TYPE , 
                                 AM0_YMD  ,  
                                 AM0_PARAM_KEY
                        union all
                          SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,  
                                 AM0_YMD, 
                                 case when AM0_PROD_TYPE = 'F' then 'STF' else 'STC' end,
                                 sum(AM0_M_QNTY) as qnty
                            FROM CI.AM0  
                           WHERE AM0_YMD >= :as_symd     
                             AND AM0_YMD <= :as_eymd   
                             AND AM0_SUM_TYPE  = :as_sum_type   
                             AND AM0_PROD_TYPE = :as_prod_type 
                             and AM0_PROD_SUBTYPE = 'S'
                        GROUP BY AM0_BRK_NO4,   
                                 AM0_BRK_TYPE , 
                                 AM0_YMD  ,  
                                 AM0_PROD_TYPE       
                        union all  
                          SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,  
                                 AM0_YMD, 
                                 '小計',
                                 sum(AM0_M_QNTY) as qnty
                            FROM CI.AM0  
                           WHERE AM0_YMD >= :as_symd    
                             AND AM0_YMD <= :as_eymd   
                             AND AM0_SUM_TYPE  = :as_sum_type   
                             AND AM0_PROD_TYPE = :as_prod_type 
                        GROUP BY AM0_BRK_NO4,   
                                 AM0_BRK_TYPE , 
                                 AM0_YMD                  
                        union all
                          SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,  
                                 '99999999', 
                                 AM0_PARAM_KEY,
                                 sum(AM0_M_QNTY) as qnty
                            FROM CI.AM0  
                           WHERE AM0_YMD >= :as_symd     
                             AND AM0_YMD <= :as_eymd   
                             AND AM0_SUM_TYPE  = :as_sum_type   
                             AND AM0_PROD_TYPE = :as_prod_type 
                             and AM0_PROD_SUBTYPE <> 'S'
                        GROUP BY AM0_BRK_NO4,   
                                 AM0_BRK_TYPE ,
                                 AM0_PARAM_KEY
                        union all
                          SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,  
                                 '99999999', 
                                 case when AM0_PROD_TYPE = 'F' then 'STF' else 'STC' end,
                                 sum(AM0_M_QNTY) as qnty
                            FROM CI.AM0  
                           WHERE AM0_YMD >= :as_symd     
                             AND AM0_YMD <= :as_eymd   
                             AND AM0_SUM_TYPE  = :as_sum_type   
                             AND AM0_PROD_TYPE = :as_prod_type 
                             and AM0_PROD_SUBTYPE = 'S'  
                        GROUP BY AM0_BRK_NO4,   
                                 AM0_BRK_TYPE ,
                                 AM0_PROD_TYPE
                        union all
                          SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,  
                                 '99999999', 
                                 '小計',
                                 sum(AM0_M_QNTY) as qnty
                            FROM CI.AM0  
                           WHERE AM0_YMD >= :as_symd     
                             AND AM0_YMD <= :as_eymd   
                             AND AM0_SUM_TYPE  = :as_sum_type   
                             AND AM0_PROD_TYPE = :as_prod_type 
                        GROUP BY AM0_BRK_NO4,   
                                 AM0_BRK_TYPE)
                        where ABRK_NO = trim(T_BRK_NO4)||case when T_BRK_TYPE = '9' then '999' else '000' end
                          AND T_BRK_NO4 = AM0_BRK_NO4(+)
                          AND T_BRK_TYPE = AM0_BRK_TYPE(+)
                          AND T_YMD = AM0_YMD(+)
                          AND T_PARAM_KEY = AM0_PARAM_KEY(+)
                        ORDER BY T_BRK_NO4,T_BRK_TYPE,T_YMD,T_PARAM_KEY
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// rpt_seq_no/rpt_value/0 AS QNTY/rpt_value_2/rpt_value_3
      /// </summary>
      /// <param name="as_txd_id"></param>
      /// <returns></returns>
      public DataTable ListParamKey(string as_txd_id)
      {
         object[] parms = {
                ":as_txd_id",as_txd_id
            };
         string sql = @"
                        SELECT rpt_seq_no AS cp_sort,rpt_value AS am0_param_key, 0 AS QNTY ,rpt_value_2 ,rpt_value_3
                        FROM ci.rpt
                        WHERE(trim(CI.RPT.RPT_TXD_ID) =  :as_txd_id )
                        ORDER BY cp_sort
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// AM0_PARAM_KEY/cp_sort
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable ListParamKey2(string as_symd, string as_eymd, string as_sum_type, string as_prod_type)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type
            };
         string sql = @"
                        SELECT CI.AM0.AM0_PARAM_KEY
                         FROM CI.AM0  
                        WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                              ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                              ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                              ( CI.AM0.AM0_PROD_TYPE = :as_prod_type )   
                         GROUP BY CI.AM0.AM0_PARAM_KEY
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         dtResult.Columns.Add("cp_sort", typeof(int));
         dtResult.Columns["cp_sort"].Expression = "iif( Trim(am0_param_key) = 'STO' , 9,0)";
         DataView dv = dtResult.AsDataView();
         dv.Sort = "cp_sort,am0_param_key";
         dtResult = dv.ToTable();
         dtResult.AcceptChanges();
         return dtResult;
      }
      /// <summary>
      /// return AM0_BRK_NO4/AM0_BRK_TYPE/AM0_YMD/AM0_PARAM_KEY/qnty/cp_rate
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable ListRowdata
         (string as_symd, string as_eymd, string as_sum_type, string as_prod_type, string as_market_code)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type,
                ":as_market_code",as_market_code
            };
         string sql = @"
                        SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,  
                                 AM0_YMD, 
                                 AM0_PARAM_KEY,
                                 sum(case :as_market_code when  '1' then AM0_AH_M_QNTY 
                                                                             when '0' then AM0_M_QNTY - nvl(AM0_AH_M_QNTY,0) 
                                                                             else AM0_M_QNTY end ) as qnty
                            FROM CI.AM0  
                           WHERE AM0_YMD >= :as_symd    
                             AND AM0_YMD <= :as_eymd   
                             AND AM0_SUM_TYPE  = :as_sum_type   
                             AND AM0_PROD_TYPE = :as_prod_type 
                             AND (:as_market_code <> '1'  or NOT AM0_AH_M_QNTY IS NULL)
                        GROUP BY AM0_BRK_NO4,   
                                 AM0_BRK_TYPE , 
                                 AM0_YMD  ,  
                                 AM0_PARAM_KEY
                        union all
                          SELECT AM0_BRK_NO4,   
                                 AM0_BRK_TYPE,  
                                 '99999999', 
                                 AM0_PARAM_KEY,
                                 sum(case :as_market_code when  '1' then AM0_AH_M_QNTY 
                                                                             when '0' then AM0_M_QNTY - nvl(AM0_AH_M_QNTY,0) 
                                                                             else AM0_M_QNTY end ) as qnty
                            FROM CI.AM0  
                           WHERE AM0_YMD >= :as_symd     
                             AND AM0_YMD <= :as_eymd   
                             AND AM0_SUM_TYPE  = :as_sum_type   
                             AND AM0_PROD_TYPE = :as_prod_type 
                             AND (:as_market_code <> '1'  or NOT AM0_AH_M_QNTY IS NULL)
                        GROUP BY AM0_BRK_NO4,   
                                 AM0_BRK_TYPE ,
                                 AM0_PARAM_KEY
                        ORDER BY am0_brk_no4,am0_brk_type,am0_ymd,am0_param_key
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
      /// <summary>
      /// return 期貨商代號,期貨商名稱,日期,短天期選擇權交易口數,市占率
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable ListRowdataOpendata(string as_symd, string as_eymd, string as_market_code)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_market_code",as_market_code
            };
         string sql = @"
                        select '期貨商代號,期貨商名稱,日期,短天期選擇權交易口數,市占率' as col_data from dual 
                         union all
                         select TRIM(AM0_BRK_NO)||','||TRIM(ABRK_NAME)||','||trim(AM0_YMD)||','||to_char(M_QNTY)||','||trim(to_char(case when TOT_M_QNTY = 0 then 0 else round(M_QNTY / TOT_M_QNTY * 100,2) end ,'990.99')) 
                           from ci.ABRK,
                               (select AM0_YMD,TRIM(AM0_BRK_NO4)||case when AM0_BRK_TYPE = '9' then '999' else '000' end as AM0_BRK_NO,
                                 sum(case :as_market_code when  '1' then AM0_AH_M_QNTY 
                                                                             when '0' then AM0_M_QNTY - nvl(AM0_AH_M_QNTY,0) 
                                                                             else AM0_M_QNTY end ) as M_QNTY
                                  from ci.am0
                                where am0_ymd >= :as_symd
                                   and am0_ymd <= :as_eymd
                                   and am0_sum_type = 'D'
                                   and am0_prod_Type = 'O'
                                   and am0_kind_id2 = 'TXW'
                                 group by AM0_YMD,AM0_BRK_NO4,AM0_BRK_TYPE),
                               (select sum(case :as_market_code when  '1' then AM0_AH_M_QNTY 
                                                                             when '0' then AM0_M_QNTY - nvl(AM0_AH_M_QNTY,0) 
                                                                             else AM0_M_QNTY end) as TOT_M_QNTY 
                                  from ci.am0
                                where am0_ymd >= :as_symd
                                   and am0_ymd <= :as_eymd
                                   and am0_sum_type = 'D'
                                   and am0_prod_Type = 'O'
                                   and am0_kind_id2 = 'TXW')
                          where AM0_BRK_NO = ABRK_NO
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// return AM0_YMD(yyyyMMdd)
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable ListTmp(string as_symd, string as_eymd, string as_sum_type, string as_prod_type)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type
            };
         string sql = @"
                        SELECT CI.AM0.AM0_BRK_NO4,   
                              CI.AM0.AM0_BRK_TYPE,  
                              CI.AM0.AM0_YMD, 
                              APDK_PARAM_KEY,
                              sum(CI.AM0.AM0_M_QNTY) as qnty
                         FROM CI.AM0  ,ci.APDK 
                        WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                              ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                              ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                              ( CI.AM0.AM0_PROD_TYPE = :as_prod_type )  AND
                              AM0_KIND_ID = APDK_KIND_ID  
                     GROUP BY CI.AM0.AM0_BRK_NO4,   
                              CI.AM0.AM0_BRK_TYPE , 
                              CI.AM0.AM0_YMD  ,  
                              APDK_PARAM_KEY
                     union
                       SELECT CI.AM0.AM0_BRK_NO4,   
                              CI.AM0.AM0_BRK_TYPE,  
                              '99999999', 
                              APDK_PARAM_KEY,
                              sum(CI.AM0.AM0_M_QNTY) as qnty
                         FROM CI.AM0 ,ci.APDK 
                        WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                              ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                              ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                              ( CI.AM0.AM0_PROD_TYPE = :as_prod_type )   AND
                              AM0_KIND_ID = APDK_KIND_ID
                     GROUP BY CI.AM0.AM0_BRK_NO4,   
                              CI.AM0.AM0_BRK_TYPE ,
                              APDK_PARAM_KEY
                     ORDER BY am0_brk_no4,am0_brk_type,am0_ymd
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         if (dtResult.Rows.Count <= 0) return dtResult;
         //int cp_sum_qnty = Convert.ToInt32(dtResult.Compute("sum(qnty)", ""));
         return dtResult;
      }

      /// <summary>
      /// return AM0_YMD(yyyyMMdd)
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable ListYMD(string as_symd,
                                  string as_eymd,
                                  string as_sum_type,
                                  string as_prod_type)
      {

         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type
            };

         string sql = @"
                   SELECT CI.AM0.AM0_YMD
                      FROM CI.AM0  
                     WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                           ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                           ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                           ( CI.AM0.AM0_PROD_TYPE = :as_prod_type )   
                  GROUP BY CI.AM0.AM0_YMD
                  ORDER BY AM0_YMD
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
      /// <summary>
      /// return AM0_YMD(yyyyMMdd)
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable ListYmdByMarketCode(string as_symd,
                                  string as_eymd,
                                  string as_sum_type,
                                  string as_prod_type,
                                  string as_market_code)
      {

         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type,
                ":as_market_code",as_market_code
            };

         string sql = @"
                    SELECT CI.AM0.AM0_YMD
                     FROM CI.AM0  
                     WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                        ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                        ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                        ( CI.AM0.AM0_PROD_TYPE = :as_prod_type )   
                     AND (:as_market_code <> '1'  or NOT AM0_AH_M_QNTY IS NULL)
                     GROUP BY CI.AM0.AM0_YMD
                     ORDER BY AM0_YMD
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
      /// <summary>
      /// return AM0_YMD(yyyyMMdd)
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <returns></returns>
      public DataTable ListYmdEnd(string as_symd,
                                  string as_eymd,
                                  string as_sum_type,
                                  string as_prod_type)
      {

         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type
            };

         string sql = @"
                    SELECT CI.AM0.AM0_YMD,'        ' as YMD_END
                         FROM CI.AM0  
                        WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                              ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                              ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                              ( CI.AM0.AM0_PROD_TYPE = :as_prod_type )   
                     GROUP BY CI.AM0.AM0_YMD
                     ORDER BY AM0_YMD
";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
