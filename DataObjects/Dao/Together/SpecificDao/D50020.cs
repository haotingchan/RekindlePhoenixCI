using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
   public class D50020:DataGate
   {
      /// <summary>
      /// d_50020
      /// </summary>
      /// <param name="d500Xx"></param>
      /// <returns></returns>
      public DataTable List50020(D500xx d500Xx)
      {
         object[] parms = {
                ":as_sum_type",d500Xx.SumType,
                ":as_sum_subtype",d500Xx.SumSubType,
                ":as_data_type",d500Xx.DataType,
                ":as_sort_type",d500Xx.SortType,
                ":Sdate",d500Xx.Sdate,
                ":Edate",d500Xx.Edate,
                ":Sbrkno",d500Xx.Sbrkno,
                ":Ebrkno",d500Xx.Ebrkno,
                ":ProdCategory",d500Xx.ProdCategory,
                ":ProdKindIdSto",d500Xx.ProdKindIdSto,
                ":ProdKindId",d500Xx.ProdKindId
            };
         string iswhere = d500Xx.ConditionWhereSyntax();
         string sql = string.Format(
@"SELECT AMM0_YMD,   
         AMM0_BRK_NO, 
         BRK_ABBR_NAME,  
         AMM0_PROD_ID,
         AMM0_CNT,   
         CP_RATE_VALID_CNT,
         AMM0_MARKET_R_CNT
    FROM
(SELECT AMM0_YMD,   
         AMM0_BRK_NO, 
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then AMM0_KIND_ID2
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         AMM0_CNT,   
         round(decode(AMM0_MARKET_R_CNT,0,0,(AMM0_CNT /AMM0_MARKET_R_CNT)*100),16) as CP_RATE_VALID_CNT,
         AMM0_MARKET_R_CNT,
         AMM0_DATA_TYPE
    FROM ci.AMM0  
   WHERE AMM0_SUM_TYPE = :as_sum_type  AND  
         AMM0_SUM_SUBTYPE = :as_sum_subtype   AND  
         (AMM0_DATA_TYPE = :as_data_type  or AMM0_DATA_TYPE = 'r')
         {0}
   ORDER BY AMM0_YMD,decode(:as_sort_type,'F',AMM0_BRK_NO,AMM0_PROD_ID ),decode(:as_sort_type,'F',AMM0_PROD_ID,AMM0_BRK_NO)
)", iswhere
);
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable List50020

      /// <summary>
      /// d_50020_accu
      /// </summary>
      /// <param name="d500Xx"></param>
      /// <returns></returns>
      public DataTable ListACCU(D500xx d500Xx)
      {
         object[] parms = {
            ":as_symd",d500Xx.Sdate,
            ":as_eymd",d500Xx.Edate,
            ":as_sum_type",d500Xx.SumType,
            ":as_sum_subtype",d500Xx.SumSubType,
            ":as_data_type",d500Xx.DataType,
            ":as_sort_type",d500Xx.SortType,
                ":Sdate",d500Xx.Sdate,
                ":Edate",d500Xx.Edate,
                ":Sbrkno",d500Xx.Sbrkno,
                ":Ebrkno",d500Xx.Ebrkno,
                ":ProdCategory",d500Xx.ProdCategory,
                ":ProdKindIdSto",d500Xx.ProdKindIdSto,
                ":ProdKindId",d500Xx.ProdKindId
            };
         string iswhere = d500Xx.ConditionWhereSyntax();
         string sql = string.Format(@"
 SELECT AMM0_YMD,   
         AMM0_BRK_NO, 
         BRK_ABBR_NAME,  
         AMM0_PROD_ID,
         AMM0_CNT,   
         round(decode(AMM0_MARKET_R_CNT,0,0,(AMM0_CNT /AMM0_MARKET_R_CNT)*100),16) as CP_RATE_VALID_CNT,
         AMM0_MARKET_R_CNT
    FROM
(SELECT min(AMM0_YMD) ||'-'|| max(AMM0_YMD) as AMM0_YMD,   
         AMM0_BRK_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then AMM0_KIND_ID2
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         SUM(AMM0_CNT) AS AMM0_CNT ,
            NVL((select sum(R.AMM0_CNT)
            from ci.AMM0 R
           where R.AMM0_SUM_TYPE = A.AMM0_SUM_TYPE    
             and R.AMM0_SUM_SUBTYPE = A.AMM0_SUM_SUBTYPE
             and R.AMM0_DATA_TYPE = A.AMM0_DATA_TYPE
             and R.AMM0_PROD_TYPE = A.AMM0_PROD_TYPE
             and R.AMM0_PROD_SUBTYPE = A.AMM0_PROD_SUBTYPE
             and R.AMM0_PARAM_KEY = A.AMM0_PARAM_KEY
             and R.AMM0_KIND_ID2 = A.AMM0_KIND_ID2
             and R.AMM0_KIND_ID = A.AMM0_KIND_ID
             and R.AMM0_PROD_ID = A.AMM0_PROD_ID
             and R.AMM0_DATA_TYPE = A.AMM0_DATA_TYPE 
             and R.AMM0_YMD >= :as_symd 
             and R.AMM0_YMD <= :as_eymd),0) as AMM0_MARKET_R_CNT,
             AMM0_DATA_TYPE  
    FROM ci.AMM0 A
   WHERE AMM0_SUM_TYPE = :as_sum_type  AND  
         AMM0_SUM_SUBTYPE = :as_sum_subtype   AND  
         (AMM0_DATA_TYPE = :as_data_type  or AMM0_DATA_TYPE = 'r')  AND 
         AMM0_YMD >= :as_symd AND
         AMM0_YMD <= :as_eymd 
         {0}
 GROUP BY A.AMM0_BRK_NO,
          A.AMM0_SUM_TYPE,
          A.AMM0_SUM_SUBTYPE,
          A.AMM0_DATA_TYPE,
          A.AMM0_PROD_TYPE,
          A.AMM0_PROD_SUBTYPE,
          AMM0_PARAM_KEY,
          A.AMM0_KIND_ID2,
          A.AMM0_KIND_ID,
          A.AMM0_PROD_ID
 ORDER BY decode(:as_sort_type,'F',AMM0_BRK_NO,AMM0_PROD_ID ),decode(:as_sort_type,'F',AMM0_PROD_ID,AMM0_BRK_NO)
)", iswhere
);
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListACCU

      /// <summary>
      /// d_50020_accu_ah
      /// </summary>
      /// <param name="d500Xx"></param>
      /// <returns></returns>
      public DataTable ListACCUAH(D500xx d500Xx)
      {
         object[] parms = {
            ":as_symd",d500Xx.Sdate,
            ":as_eymd",d500Xx.Edate,
            ":as_sum_type",d500Xx.SumType,
            ":as_sum_subtype",d500Xx.SumSubType,
            ":as_data_type",d500Xx.DataType,
            ":as_sort_type",d500Xx.SortType,
                ":Sdate",d500Xx.Sdate,
                ":Edate",d500Xx.Edate,
                ":Sbrkno",d500Xx.Sbrkno,
                ":Ebrkno",d500Xx.Ebrkno,
                ":ProdCategory",d500Xx.ProdCategory,
                ":ProdKindIdSto",d500Xx.ProdKindIdSto,
                ":ProdKindId",d500Xx.ProdKindId
            };
         string iswhere = d500Xx.ConditionWhereSyntax();
         string sql = string.Format(@"
SELECT AMM0_YMD,   
         AMM0_BRK_NO, 
         BRK_ABBR_NAME,  
         AMM0_PROD_ID,
         AMM0_CNT,   
         round(decode(AMM0_MARKET_R_CNT,0,0,(AMM0_CNT /AMM0_MARKET_R_CNT)*100),16) as CP_RATE_VALID_CNT,
         AMM0_MARKET_R_CNT
    FROM
(SELECT min(AMM0_YMD) ||'-'|| max(AMM0_YMD) as AMM0_YMD,   
         AMM0_BRK_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then AMM0_KIND_ID2
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         SUM(AMM0_CNT) AS AMM0_CNT ,
            NVL((select sum(R.AMM0_CNT)
            from ci.AMM0AH R
           where R.AMM0_SUM_TYPE = A.AMM0_SUM_TYPE    
             and R.AMM0_SUM_SUBTYPE = A.AMM0_SUM_SUBTYPE
             and R.AMM0_DATA_TYPE = A.AMM0_DATA_TYPE
             and R.AMM0_PROD_TYPE = A.AMM0_PROD_TYPE
             and R.AMM0_PROD_SUBTYPE = A.AMM0_PROD_SUBTYPE
             and R.AMM0_PARAM_KEY = A.AMM0_PARAM_KEY
             and R.AMM0_KIND_ID2 = A.AMM0_KIND_ID2
             and R.AMM0_KIND_ID = A.AMM0_KIND_ID
             and R.AMM0_PROD_ID = A.AMM0_PROD_ID
             and R.AMM0_DATA_TYPE = A.AMM0_DATA_TYPE 
             and R.AMM0_YMD >= :as_symd 
             and R.AMM0_YMD <= :as_eymd),0) as AMM0_MARKET_R_CNT,
             AMM0_DATA_TYPE  
    FROM ci.AMM0AH A
   WHERE AMM0_SUM_TYPE = :as_sum_type  AND  
         AMM0_SUM_SUBTYPE = :as_sum_subtype   AND  
         (AMM0_DATA_TYPE = :as_data_type  or AMM0_DATA_TYPE = 'r')  AND 
         AMM0_YMD >= :as_symd AND
         AMM0_YMD <= :as_eymd 
         {0}
 GROUP BY A.AMM0_BRK_NO,
          A.AMM0_SUM_TYPE,
          A.AMM0_SUM_SUBTYPE,
          A.AMM0_DATA_TYPE,
          A.AMM0_PROD_TYPE,
          A.AMM0_PROD_SUBTYPE,
          AMM0_PARAM_KEY,
          A.AMM0_KIND_ID2,
          A.AMM0_KIND_ID,
          A.AMM0_PROD_ID
 ORDER BY decode(:as_sort_type,'F',AMM0_BRK_NO,AMM0_PROD_ID ),decode(:as_sort_type,'F',AMM0_PROD_ID,AMM0_BRK_NO)
)
", iswhere);
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListACCUAH

      /// <summary>
      /// d_50020_ah
      /// </summary>
      /// <param name="d500Xx"></param>
      /// <returns></returns>
      public DataTable ListAH(D500xx d500Xx)
      {
         object[] parms = {
                ":as_sum_type",d500Xx.SumType,
                ":as_sum_subtype",d500Xx.SumSubType,
                ":as_data_type",d500Xx.DataType,
                ":as_sort_type",d500Xx.SortType,
                ":Sdate",d500Xx.Sdate,
                ":Edate",d500Xx.Edate,
                ":Sbrkno",d500Xx.Sbrkno,
                ":Ebrkno",d500Xx.Ebrkno,
                ":ProdCategory",d500Xx.ProdCategory,
                ":ProdKindIdSto",d500Xx.ProdKindIdSto,
                ":ProdKindId",d500Xx.ProdKindId
            };
         string iswhere = d500Xx.ConditionWhereSyntax();
         string sql = string.Format(@"
 SELECT AMM0_YMD,   
         AMM0_BRK_NO, 
         BRK_ABBR_NAME,  
         AMM0_PROD_ID,
         AMM0_CNT,   
         CP_RATE_VALID_CNT,
         AMM0_MARKET_R_CNT
    FROM
(SELECT AMM0_YMD,   
         AMM0_BRK_NO, 
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then AMM0_KIND_ID2
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         AMM0_CNT,   
         round(decode(AMM0_MARKET_R_CNT,0,0,(AMM0_CNT /AMM0_MARKET_R_CNT)*100),16) as CP_RATE_VALID_CNT,
         AMM0_MARKET_R_CNT,
         AMM0_DATA_TYPE 
    FROM ci.AMM0AH   
   WHERE AMM0_SUM_TYPE = :as_sum_type  AND  
         AMM0_SUM_SUBTYPE = :as_sum_subtype   AND  
         (AMM0_DATA_TYPE = :as_data_type  or AMM0_DATA_TYPE = 'r')
   {0}
  ORDER BY AMM0_YMD,decode(:as_sort_type,'F',AMM0_BRK_NO,AMM0_PROD_ID ),decode(:as_sort_type,'F',AMM0_PROD_ID,AMM0_BRK_NO)
)
", iswhere);
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListAH

      /// <summary>
      /// d_50020_d
      /// </summary>
      /// <param name="as_fm_date">起始日</param>
      /// <param name="as_to_date">終止日</param>
      /// <returns></returns>
      public DataTable List50020d(DateTime as_fm_date, DateTime as_to_date)
      {
         object[] parms = {
                ":as_fm_date",as_fm_date,
                ":as_to_date",as_to_date
            };
         string sql = @"SELECT to_char(AMMD_DATE,'yyyy/mm/dd') as DATA_DATE,AMMD_BRK_NO AS FCM,AMMD_PROD_ID AS  PROD ,TO_CHAR(AMMD_W_TIME,'YYYY/MM/DD HH24:MI:SSxff') as  SEND_TIME
                         FROM ci.AMMD
                        WHERE AMMD_DATE >= :as_fm_date
                          and AMMD_DATE <= :as_to_date
                          AND AMMD_DATA_TYPE = 'R'";
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable List50020d

      /// <summary>
      /// d_50020_d_ah
      /// </summary>
      /// <param name="as_fm_date">起始日</param>
      /// <param name="as_to_date">終止日</param>
      /// <returns></returns>
      public DataTable List50020dAH(DateTime as_fm_date, DateTime as_to_date)
      {
         object[] parms = {
                ":as_fm_date",as_fm_date,
                ":as_to_date",as_to_date
            };
         string sql = @"SELECT to_char(AMMD_DATE,'yyyy/mm/dd') as DATA_DATE,AMMD_BRK_NO AS FCM,AMMD_PROD_ID AS  PROD ,TO_CHAR(AMMD_W_TIME,'YYYY/MM/DD HH24:MI:SSxff') as  SEND_TIME
                         FROM ci.AMMDAH 
                        WHERE AMMD_DATE >= :as_fm_date
                          and AMMD_DATE <= :as_to_date
                          AND AMMD_DATA_TYPE = 'R'";
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable List50020dAH

   }
}
