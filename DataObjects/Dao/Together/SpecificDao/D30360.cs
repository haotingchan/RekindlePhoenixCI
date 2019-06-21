using System;
using System.Data;
using Common;
/// <summary>
/// john,20190304,D30360
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30360 : DataGate
   {
      /// <summary>
      /// 確認有STC
      /// </summary>
      /// <returns></returns>
      public int ApdkSTCcount()
      {
         string sql = @"select count(*) from ci.APDK where trim(APDK_PARAM_KEY) = 'STC'";
         DataTable dtResult = db.GetDataTable(sql, null);
         return dtResult.Rows[0][0].AsInt();
      }
      /// <summary>
      /// 抓當月最後交易日 return Max(AI2_YMD)
      /// </summary>
      /// <param name="IsDate">yyyyMM</param>
      /// <returns></returns>
      public string GetMaxLastDay30361(DateTime IsDate)
      {
         object[] parms = {
            ":ls_date",IsDate.ToString("yyyyMM")
            };

         string sql =
             @"select max(AI2_YMD)
                       from ci.AI2
                      where (AI2_PARAM_KEY LIKE 'STC%' OR AI2_PARAM_KEY LIKE 'STO%')
                        and AI2_SUM_TYPE = 'D'
                        and Trim(substr(AI2_YMD,1,6)) = :ls_date";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult.Rows[0][0].AsString();
      }
      /// <summary>
      /// 抓當月最後交易日 return Max(AI2_YMD)
      /// </summary>
      /// <param name="StartDate">yyyyMM01</param>
      /// <param name="EndDate">yyyyMMdd</param>
      /// <returns></returns>
      public string GetMaxLastDay30366(DateTime StartDate, DateTime EndDate)
      {
         object[] parms = {
            ":ldt_sdate",StartDate.ToString("yyyyMMdd"),
            ":ls_date",EndDate.ToString("yyyyMMdd")
            };

         string sql =
             @"select max(AI2_YMD)
               from ci.AI2
               where AI2_SUM_TYPE = 'D'
                  and AI2_PROD_TYPE = 'O' and AI2_PROD_SUBTYPE = 'S'
                  and AI2_YMD >= :ldt_sdate
                  and AI2_YMD <= :ls_date ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult.Rows[0][0].AsString();
      }
      /// <summary>
      /// return AI2_YMD/AI2_PARAM_KEY/AI2_PC_CODE/sum(AI2_M_QNTY)/sum(AI2_OI)/sum(AI2_MMK_QNTY)
      /// </summary>
      /// <param name="as_kind_id"></param>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <returns></returns>
      public DataTable Get30361Data(string as_kind_id, string as_symd, string as_eymd)
      {
         object[] parms = {
            ":as_kind_id",as_kind_id,
            ":as_symd",as_symd,
            ":as_eymd",as_eymd
            };

         string sql =
             @"SELECT AI2_YMD,   
                     APDK_PARAM_KEY AS AI2_PARAM_KEY,   
                     AI2_PC_CODE,   
                     sum(AI2_M_QNTY) as AI2_M_QNTY,   
                     sum(AI2_OI) as AI2_OI,   
                     sum(AI2_MMK_QNTY) as AI2_MMK_QNTY  
                FROM ci.AI2  ,ci.APDK
               WHERE AI2_SUM_TYPE = 'D'  AND  
                     AI2_YMD >= :as_symd  AND  
                     AI2_YMD <= :as_eymd  AND  
                     AI2_SUM_SUBTYPE = '5'  AND
                     AI2_KIND_ID = APDK_KIND_ID AND
                     APDK_PARAM_KEY like :as_kind_id||'%'   
            GROUP BY AI2_YMD,   
                     APDK_PARAM_KEY,   
                     AI2_PC_CODE   
            ORDER BY AI2_YMD";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// return KIND_ID_2/M_QNTY/PDK_NAME
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_param_key"></param>
      /// <returns></returns>
      public DataTable Get30362Data(DateTime as_ymd, string as_param_key)
      {
         object[] parms = {
            ":as_ymd",as_ymd.ToString("yyyyMM"),
            ":as_param_key",as_param_key
            };

         string sql =
             @"SELECT I.AI2_KIND_ID_2 AS KIND_ID_2,
                      I.AI2_M_QNTY AS M_QNTY ,
                      P.APDK_NAME  AS PDK_NAME
                 FROM
               (SELECT SUBSTR(AI2.AI2_KIND_ID,1,2) AS AI2_KIND_ID_2,
                        SUM(AI2.AI2_M_QNTY) AS AI2_M_QNTY 
                   FROM ci.AI2 AI2 ,ci.APDK 
                  WHERE AI2.AI2_SUM_TYPE = 'M'  AND  
                        AI2.AI2_YMD = :as_ymd AND
                        AI2.AI2_SUM_SUBTYPE = '4' AND
                        AI2.AI2_PROD_TYPE = 'O' AND
                        AI2.AI2_PROD_SUBTYPE = 'S' and
                        AI2.AI2_KIND_ID = APDK_KIND_ID AND
                        APDK_PARAM_KEY like :as_param_key||'%'  
               GROUP BY SUBSTR(AI2.AI2_KIND_ID,1,2)) I,
               (SELECT SUBSTR(APDK.APDK_KIND_ID,1,2) AS APDK_KIND_ID_2,
                        MIN(APDK.APDK_NAME) AS APDK_NAME 
                   FROM ci.APDK APDK
                  WHERE APDK_PROD_TYPE = 'O'
               GROUP BY SUBSTR(APDK.APDK_KIND_ID,1,2)) P
               WHERE I.AI2_KIND_ID_2 = P.APDK_KIND_ID_2";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// return AI2_YMD/KIND_ID_2/AI2_PC_CODE/AI2_M_QNTY
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_param_key"></param>
      /// <returns></returns>
      public DataTable Get30363Data(string as_symd,string as_eymd, string as_param_key)
      {
         object[] parms = {
            ":as_symd",as_symd,
            ":as_eymd",as_eymd,
            ":as_param_key",as_param_key
            };

         string sql =
             @"SELECT AI2_YMD,
                     SUBSTR(AI2_KIND_ID,1,2) AS AI2_KIND_ID_2,
                     AI2_PC_CODE,
                     SUM(AI2_M_QNTY) AS AI2_M_QNTY 
                FROM ci.AI2  ,ci.APDK
               WHERE AI2_SUM_TYPE = 'D'   AND  
                     AI2_YMD >= :as_symd  AND
                     AI2_YMD <= :as_eymd  AND
                     AI2_SUM_SUBTYPE = '5' AND
                     AI2.AI2_PROD_TYPE = 'O' AND
                     AI2.AI2_PROD_SUBTYPE = 'S' AND
                     AI2.AI2_KIND_ID = APDK_KIND_ID AND
                     APDK_PARAM_KEY like :as_param_key||'%' 
            GROUP BY AI2_YMD,
                     SUBSTR(AI2_KIND_ID,1,2),
                     AI2_PC_CODE
            ORDER BY ai2_ymd,ai2_kind_id_2,ai2_pc_code";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// return AI2_YMD/KIND_ID_2/AI2_PC_CODE/AI2_M_QNTY
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_param_key"></param>
      /// <returns></returns>
      public DataTable Get30363KindID2Data(string as_symd, string as_eymd, string as_param_key)
      {
         object[] parms = {
            ":as_symd",as_symd,
            ":as_eymd",as_eymd,
            ":as_param_key",as_param_key
            };

         string sql =
             @"SELECT I.AI2_KIND_ID_2 AS KIND_ID_2,
                      P.APDK_NAME  AS PDK_NAME
                 FROM
               (SELECT SUBSTR(AI2_KIND_ID,1,2) AS AI2_KIND_ID_2
                   FROM ci.AI2  ,ci.APDK
                  WHERE AI2_SUM_TYPE = 'D'   AND  
                        AI2_YMD >= :as_symd  AND
                        AI2_YMD <= :as_eymd  AND
                        AI2_SUM_SUBTYPE = '5' AND
                        AI2.AI2_PROD_TYPE = 'O' AND
                        AI2.AI2_PROD_SUBTYPE = 'S' AND
                        AI2.AI2_KIND_ID = APDK_KIND_ID AND
                        APDK_PARAM_KEY like :as_param_key||'%' 
               GROUP BY SUBSTR(AI2_KIND_ID,1,2)) I,
               (SELECT SUBSTR(APDK.APDK_KIND_ID,1,2) AS APDK_KIND_ID_2,
                        MIN(APDK.APDK_NAME) AS APDK_NAME 
                   FROM ci.APDK APDK
                  WHERE APDK_PROD_TYPE = 'O'
               GROUP BY SUBSTR(APDK.APDK_KIND_ID,1,2)) P
               WHERE I.AI2_KIND_ID_2 = P.APDK_KIND_ID_2
               ORDER BY kind_id_2";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// return AI2_YMD/AI2_PROD_SUBTYPE/AI2_PC_CODE/sum(AI2_M_QNTY)/sum(AI2_OI)/sum(AI2_MMK_QNTY)
      /// </summary>
      /// <param name="as_prod_type"></param>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <returns></returns>
      public DataTable Get30366Data(string as_prod_type, string as_symd, string as_eymd)
      {
         object[] parms = {
            ":as_prod_type",as_prod_type,
            ":as_symd",as_symd,
            ":as_eymd",as_eymd
            };

         string sql =
             @"SELECT AI2_YMD,   
                     AI2_PROD_SUBTYPE,   
                     AI2_PC_CODE,   
                     sum(AI2_M_QNTY) as AI2_M_QNTY,   
                     sum(AI2_OI) as AI2_OI,   
                     sum(AI2_MMK_QNTY) as AI2_MMK_QNTY  
                FROM ci.AI2  
               WHERE AI2_SUM_TYPE = 'D'  AND  
                     AI2_YMD >= :as_symd  AND  
                     AI2_YMD <= :as_eymd  AND  
                     AI2_SUM_SUBTYPE = '5'  AND
                     AI2_PROD_TYPE = :as_prod_type AND
                     AI2_PROD_SUBTYPE = 'S' AND
                     AI2_PARAM_KEY <> 'STO'
            GROUP BY AI2_YMD,   
                     AI2_PROD_SUBTYPE,   
                     AI2_PC_CODE   
            ORDER BY AI2_YMD";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// return KIND_ID_2/M_QNTY/AI2_PC_CODE/PDK_NAME
      /// </summary>
      /// <param name="as_ymd">yyyyMM</param>
      /// <returns></returns>
      public DataTable Get30367Data(DateTime as_ymd)
      {
         object[] parms = {
            ":as_ymd",as_ymd.ToString("yyyyMM")
            };

         string sql =
             @"SELECT I.AI2_KIND_ID_2 AS KIND_ID_2,
                      I.AI2_M_QNTY AS M_QNTY ,
                      P.APDK_NAME  AS PDK_NAME
                 FROM
                     (SELECT SUBSTR(AI2.AI2_KIND_ID,1,2) AS AI2_KIND_ID_2,
                              SUM(AI2.AI2_M_QNTY) AS AI2_M_QNTY 
                         FROM ci.AI2 AI2 
                        WHERE AI2.AI2_SUM_TYPE = 'M'  AND  
                              trim(AI2.AI2_YMD) = :as_ymd AND
                              AI2.AI2_SUM_SUBTYPE = '4' AND
                              AI2.AI2_PROD_TYPE = 'O' AND
                              AI2.AI2_PROD_SUBTYPE = 'S' and
                              AI2.AI2_PARAM_KEY <> 'STO'
                     GROUP BY SUBSTR(AI2.AI2_KIND_ID,1,2)) I,
                     (SELECT SUBSTR(APDK.APDK_KIND_ID,1,2) AS APDK_KIND_ID_2,
                              MIN(APDK.APDK_NAME) AS APDK_NAME 
                         FROM ci.APDK APDK
                       WHERE APDK_PROD_TYPE = 'O'
                           and APDK_PROD_SUBTYPE = 'S'
               GROUP BY SUBSTR(APDK.APDK_KIND_ID,1,2)) P
               WHERE I.AI2_KIND_ID_2 = P.APDK_KIND_ID_2
               ORDER BY I.AI2_KIND_ID_2";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// return AI2_YMD/AI2_PROD_SUBTYPE/AI2_PC_CODE/sum(AI2_M_QNTY)/sum(AI2_OI)/sum(AI2_MMK_QNTY)
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <returns></returns>
      public DataTable Get30368Data(string as_symd, string as_eymd)
      {
         object[] parms = {
            ":as_symd",as_symd,
            ":as_eymd",as_eymd
            };

         string sql =
             @"SELECT SEQ_NO,PDK_NAME,A.AI2_KIND_ID_2,A.AI2_YMD,A.AI2_PC_CODE,A.AI2_M_QNTY,K.DATA_KIND_ID_2 ,K.DATA_YMD
                 FROM
                   (SELECT DATA_YMD,DATA_KIND_ID_2,PDK_NAME,SEQ_NO
                        from
                    --交易天數
                   (SELECT AI2_YMD  as DATA_YMD
                        FROM ci.AI2  
                      WHERE AI2_SUM_TYPE = 'D'   AND  
                                  AI2_YMD >= :as_symd  AND
                                  AI2_YMD <=  :as_eymd  AND
                                  AI2_SUM_SUBTYPE = '5' AND
                                  AI2.AI2_PROD_TYPE = 'O' AND
                                  AI2.AI2_PROD_SUBTYPE = 'S' AND
                                  AI2.AI2_PARAM_KEY <> 'STO'
                       GROUP BY AI2_YMD),
                      --全部商品順序
                      (SELECT I.AI2_KIND_ID_2 AS DATA_KIND_ID_2,
                              P.APDK_NAME  AS PDK_NAME,
                              rownum as SEQ_NO
                         FROM
                             (SELECT SUBSTR(AI2_KIND_ID,1,2) AS AI2_KIND_ID_2
                                FROM ci.AI2  
                               WHERE AI2_SUM_TYPE = 'D'   AND  
                                     AI2_YMD >= :as_symd  AND
                                     AI2_YMD <=  :as_eymd  AND
                                     AI2_SUM_SUBTYPE = '5' AND
                                     AI2.AI2_PROD_TYPE = 'O' AND
                                     AI2.AI2_PROD_SUBTYPE = 'S' AND
                                     AI2.AI2_PARAM_KEY <> 'STO'
                               GROUP BY SUBSTR(AI2_KIND_ID,1,2)
                               ORDER BY SUBSTR(AI2_KIND_ID,1,2)) I,
                             (SELECT SUBSTR(APDK.APDK_KIND_ID,1,2) AS APDK_KIND_ID_2,
                                     MIN(APDK.APDK_NAME) AS APDK_NAME 
                                FROM ci.APDK APDK
                               WHERE APDK_PROD_TYPE = 'O'
                               GROUP BY SUBSTR(APDK.APDK_KIND_ID,1,2)
                               ORDER BY SUBSTR(APDK.APDK_KIND_ID,1,2)) P
                        WHERE I.AI2_KIND_ID_2 = P.APDK_KIND_ID_2) ) K,
                     --成交量
                      (SELECT AI2_YMD,
                              SUBSTR(AI2_KIND_ID,1,2) AS AI2_KIND_ID_2,
                              AI2_PC_CODE,
                              SUM(AI2_M_QNTY) AS AI2_M_QNTY 
                         FROM ci.AI2
                        WHERE AI2_SUM_TYPE = 'D'   AND  
                              AI2_YMD >= :as_symd  AND
                              AI2_YMD <= :as_eymd  AND
                              AI2_SUM_SUBTYPE = '5' AND
                              AI2.AI2_PROD_TYPE = 'O' AND
                              AI2.AI2_PROD_SUBTYPE = 'S' AND
                              AI2.AI2_PARAM_KEY <> 'STO' 
                        GROUP BY AI2_YMD,
                              SUBSTR(AI2_KIND_ID,1,2),
                              AI2_PC_CODE) A
                 where K.DATA_YMD  = A.AI2_YMD(+)
                     and K.DATA_KIND_ID_2  = A.AI2_KIND_ID_2(+)
                ORDER BY data_ymd,ai2_ymd,data_kind_id_2,ai2_kind_id_2,ai2_pc_code";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
   }
}
