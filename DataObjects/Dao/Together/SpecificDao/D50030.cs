using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
   public class D50030:DataGate
   {
      /// <summary>
      /// d_50030
      /// </summary>
      /// <param name="d500Xx"></param>
      /// <returns></returns>
      public DataTable List50030(D500xx d500Xx)
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
 SELECT LV2.*,  decode(AMMF_CP_KIND,'2',case when(CP_CHK2 = 0 and CP_CHK3 = 0) then 0 else 1 end,case when(CP_CHK1 = 0 and CP_CHK2 = 0 and CP_CHK3 = 0) then 0 else 1 end) as CP_CHK,
 case when (LV2.AMM0_YMD <> nvl(NEXT_YMD,0)) then AMM0_MARKET_M_QNTY else 0 end as CP_MARKET_M_QNTY
FROM
(SELECT LV1.*,
case when CP_RATE_VALID_CNT >=MMF_RESP_RATIO then 0 else 1 end as CP_CHK1,
case when (AMM0_KEEP_FLAG='Y'  or  MMF_AVG_TIME =0) then 0 else 1 end as CP_CHK2,
case when  AMMF_CP_KIND in ('A','C') then (case when MMK_QNTY >= MMF_QNTY_LOW then 0 else 1 end) else (case when CP_AVG_MMK_QNTY >=MMF_QNTY_LOW then 0 else 1 end) end as CP_CHK3,
 Lead (LV1.AMM0_YMD) over (order by AMM0_YMD, CP_GROUP1 , CP_GROUP2) as NEXT_YMD
FROM
(SELECT main.*,
trim(AMM0_PROD_ID )||decode(AMM0_BASIC_PROD,'Y','*','') as CP_PROD_ID,
 decode(AMM0_MARKET_M_QNTY ,0,0, round(CP_M_QNTY / AMM0_MARKET_M_QNTY,16)) * 100 as CP_RATE_M,
 decode (AMM0_MARKET_R_CNT ,0, 1 ,round(AMM0_VALID_CNT / AMM0_MARKET_R_CNT,4)) * 100 as CP_RATE_VALID_REAL,
 decode(AMM0_SUM_TYPE,'D', CEIL( NVL(TRUNC(AMM0_KEEP_TIME,0) / 60/ NULLIF(AMM0_DAY_COUNT,0),0)) , CEIL( NVL(AMM0_KEEP_TIME / 60/ NULLIF(AMM0_DAY_COUNT,0),0))) as CP_KEEP_TIME,
 NVL(TRUNC(MMK_QNTY /  NULLIF(AMM0_DAY_COUNT,0) ,1),0) as CP_AVG_MMK_QNTY,
 decode(AMM0_MARKET_R_CNT,0,100,AMM0_RQ_RATE) as CP_RATE_VALID_CNT,
 decode(:as_sort_type,'F' ,AMM0_BRK_NO||AMM0_ACC_NO,AMM0_PROD_TYPE||AMM0_PROD_ID ) as CP_GROUP1,
 decode( :as_sort_type ,'F',AMM0_PROD_TYPE||AMM0_PROD_ID, AMM0_BRK_NO||AMM0_ACC_NO ) as CP_GROUP2
FROM
(SELECT AMM0_YMD,   
         AMM0_BRK_NO,
         AMM0_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         AMM0_CNT,   
         AMM0_VALID_CNT,   
         AMM0_OM_QNTY,   
         AMM0_QM_QNTY,  
         AMM0_MARKET_R_CNT, 
         AMM0_MARKET_M_QNTY,
         NVL(AMMF_QUOTE_VALID_RATE,0) as MMF_QUOTE_VALID_RATE,
         NVL(AMMF_RESP_RATIO,99999) as MMF_RESP_RATIO,
         NVL(AMMF_QNTY_LOW,0) as MMF_QNTY_LOW,
         0 as TOT_R,
         0 as TOT_M,
         AMM0_KEEP_TIME,
         AMM0_KEEP_FLAG,
         AMM0_SUM_TYPE,
         AMM0_SUM_SUBTYPE, NVL(AMMF_AVG_TIME,0) as MMF_AVG_TIME,
         AMM0_O_SUBTRACT_QNTY,
         AMM0_Q_SUBTRACT_QNTY,
         nvl(AMM0_IQM_QNTY,0) as AMM0_IQM_QNTY,
         nvl(AMM0_IQM_SUBTRACT_QNTY,0) as AMM0_IQM_SUBTRACT_QNTY,
         AMM0_BASIC_PROD,
         AMM0_DAY_COUNT,
         LEAST(nvl(AMM0_MM_QNTY,0),nvl(AMM0_MAX_MM_QNTY,0)) as MMK_QNTY,
         AMM0_TRD_INVALID_QNTY,
         AMM0_BTRADE_M_QNTY,
         AMM0_RQ_RATE,
         AMMF_RFC_MIN_CNT as MMF_RFC_MIN_CNT,
         AMMF_CP_KIND,
         ((AMM0_OM_QNTY + AMM0_QM_QNTY + nvl(AMM0_IQM_QNTY,0))+decode(NULLIF(AMM0_BTRADE_M_QNTY,null),0,AMM0_BTRADE_M_QNTY)) as CP_M_QNTY
    FROM ci.AMM0,ci.AMMF
   WHERE AMM0_SUM_TYPE = :as_sum_type  AND  
         AMM0_SUM_SUBTYPE = :as_sum_subtype   AND  
         AMM0_DATA_TYPE = :as_data_type 
     and AMM0_PARAM_KEY = AMMF_PARAM_KEY (+)
     and substr(AMM0_YMD,1,6) = AMMF_YM (+)
     and '0' = AMMF_MARKET_CODE(+)
     {0}
) main
ORDER BY AMM0_YMD, CP_GROUP1 , CP_GROUP2
) LV1
) LV2
", iswhere);
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListD50030

      /// <summary>
      /// d_50030_accu
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
 SELECT ROWNUM as CP_ROW,main.*,
         (AMM0_OM_QNTY + AMM0_QM_QNTY +AMM0_IQM_QNTY) as CP_M_QNTY,
          decode( AMM0_MARKET_M_QNTY ,0,0, round((AMM0_OM_QNTY + AMM0_QM_QNTY +AMM0_IQM_QNTY) /AMM0_MARKET_M_QNTY,16)) * 100 as CP_RATE_M,
          CEIL(TRUNC(AMM0_KEEP_TIME,0) / 60) as CP_KEEP_TIME,
          decode( AMM0_MARKET_R_CNT,0,0, round(AMM0_VALID_CNT/AMM0_MARKET_R_CNT,16)) * 100 as CP_RATE_VALID_CNT
FROM
 (SELECT min(AMM0_YMD) ||'-'|| max(AMM0_YMD) as AMM0_YMD,   
         AMM0_BRK_NO,
         AMM0_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         sum(AMM0_CNT) as AMM0_CNT,   
         sum(AMM0_VALID_CNT) as AMM0_VALID_CNT,   
         sum(AMM0_OM_QNTY) as AMM0_OM_QNTY,   
         sum(AMM0_QM_QNTY) as AMM0_QM_QNTY,  
            (select nvl(sum(R.AMM0_CNT),0)
            from ci.AMM0 R
           where R.AMM0_SUM_TYPE = A.AMM0_SUM_TYPE    
             and R.AMM0_SUM_SUBTYPE = A.AMM0_SUM_SUBTYPE
             and R.AMM0_PROD_TYPE = A.AMM0_PROD_TYPE
             and R.AMM0_PROD_SUBTYPE = A.AMM0_PROD_SUBTYPE
             and R.AMM0_PARAM_KEY = A.AMM0_PARAM_KEY
             and R.AMM0_KIND_ID2 = A.AMM0_KIND_ID2
             and R.AMM0_KIND_ID = A.AMM0_KIND_ID
             and R.AMM0_PROD_ID = A.AMM0_PROD_ID
             and R.AMM0_DATA_TYPE = 'R'
             and R.AMM0_YMD >= :as_symd 
             and R.AMM0_YMD <= :as_eymd) as AMM0_MARKET_R_CNT,   
            (select nvl(sum(R.AMM0_MARKET_M_QNTY),0)
            from ci.AMM0 R
           where R.AMM0_SUM_TYPE = A.AMM0_SUM_TYPE    
             and R.AMM0_SUM_SUBTYPE = A.AMM0_SUM_SUBTYPE
             and R.AMM0_PROD_TYPE = A.AMM0_PROD_TYPE
             and R.AMM0_PROD_SUBTYPE = A.AMM0_PROD_SUBTYPE
             and R.AMM0_PARAM_KEY = A.AMM0_PARAM_KEY
             and R.AMM0_KIND_ID2 = A.AMM0_KIND_ID2
             and R.AMM0_KIND_ID = A.AMM0_KIND_ID
             and R.AMM0_PROD_ID = A.AMM0_PROD_ID
             and R.AMM0_DATA_TYPE = 'M'
             and R.AMM0_YMD >= :as_symd 
             and R.AMM0_YMD <= :as_eymd) as AMM0_MARKET_M_QNTY, 
         NVL(sum(AMM0_KEEP_TIME),0) as AMM0_KEEP_TIME,
         NVL(sum(AMM0_O_SUBTRACT_QNTY),0) as AMM0_O_SUBTRACT_QNTY,
         NVL(sum(AMM0_Q_SUBTRACT_QNTY),0) as AMM0_Q_SUBTRACT_QNTY,
         NVL(sum(AMM0_IQM_QNTY),0) as AMM0_IQM_QNTY,
         NVL(sum(AMM0_IQM_SUBTRACT_QNTY),0) as AMM0_IQM_SUBTRACT_QNTY,
         NVL(sum(AMM0_BTRADE_M_QNTY),0) as AMM0_BTRADE_M_QNTY,
         CAST('' AS NUMBER(8,0)) AS AMM0_TRD_INVALID_QNTY,
         COUNT(DISTINCT (AMM0_YMD)) AS AMM0_DAY_COUNT
    FROM ci.AMM0  A
   WHERE AMM0_SUM_TYPE = :as_sum_type  AND  
         AMM0_SUM_SUBTYPE = :as_sum_subtype   AND  
         AMM0_DATA_TYPE = :as_data_type 
         {0}
 GROUP BY A.AMM0_BRK_NO,
          A.AMM0_ACC_NO,
          A.AMM0_SUM_TYPE,
          A.AMM0_SUM_SUBTYPE,
          A.AMM0_DATA_TYPE,
          A.AMM0_PROD_TYPE,
          A.AMM0_PROD_SUBTYPE,
          AMM0_PARAM_KEY,
          A.AMM0_KIND_ID2,
          A.AMM0_KIND_ID,
          A.AMM0_PROD_ID) main
ORDER BY  
decode(:as_sort_type,'F',AMM0_BRK_NO,AMM0_BRK_NO||AMM0_ACC_NO ,AMM0_PROD_TYPE||AMM0_PROD_ID ),
decode(:as_sort_type ,'F',AMM0_PROD_TYPE||AMM0_PROD_ID, AMM0_BRK_NO||AMM0_ACC_NO )
", iswhere);
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListD50030Accu

      /// <summary>
      /// d_50030_accu_ah
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
SELECT ROWNUM as CP_ROW,main.*,
         (AMM0_OM_QNTY + AMM0_QM_QNTY +AMM0_IQM_QNTY) as CP_M_QNTY,
          decode( AMM0_MARKET_M_QNTY ,0,0, round((AMM0_OM_QNTY + AMM0_QM_QNTY +AMM0_IQM_QNTY) /AMM0_MARKET_M_QNTY,16)) * 100 as CP_RATE_M,
          CEIL(TRUNC(AMM0_KEEP_TIME,0) / 60) as CP_KEEP_TIME,
          decode( AMM0_MARKET_R_CNT,0,0, round(AMM0_VALID_CNT/AMM0_MARKET_R_CNT,16)) * 100 as CP_RATE_VALID_CNT
FROM
 (SELECT min(AMM0_YMD) ||'-'|| max(AMM0_YMD) as AMM0_YMD,   
         AMM0_BRK_NO,
         AMM0_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         sum(AMM0_CNT) as AMM0_CNT,   
         sum(AMM0_VALID_CNT) as AMM0_VALID_CNT,   
         sum(AMM0_OM_QNTY) as AMM0_OM_QNTY,   
         sum(AMM0_QM_QNTY) as AMM0_QM_QNTY,  
            (select nvl(sum(R.AMM0_CNT),0)
            from ci.AMM0AH R
           where R.AMM0_SUM_TYPE = A.AMM0_SUM_TYPE    
             and R.AMM0_SUM_SUBTYPE = A.AMM0_SUM_SUBTYPE
             and R.AMM0_PROD_TYPE = A.AMM0_PROD_TYPE
             and R.AMM0_PROD_SUBTYPE = A.AMM0_PROD_SUBTYPE
             and R.AMM0_PARAM_KEY = A.AMM0_PARAM_KEY
             and R.AMM0_KIND_ID2 = A.AMM0_KIND_ID2
             and R.AMM0_KIND_ID = A.AMM0_KIND_ID
             and R.AMM0_PROD_ID = A.AMM0_PROD_ID
             and R.AMM0_DATA_TYPE = 'R'
             and R.AMM0_YMD >= :as_symd 
             and R.AMM0_YMD <= :as_eymd) as AMM0_MARKET_R_CNT,   
            (select nvl(sum(R.AMM0_MARKET_M_QNTY),0)
            from ci.AMM0AH R
           where R.AMM0_SUM_TYPE = A.AMM0_SUM_TYPE    
             and R.AMM0_SUM_SUBTYPE = A.AMM0_SUM_SUBTYPE
             and R.AMM0_PROD_TYPE = A.AMM0_PROD_TYPE
             and R.AMM0_PROD_SUBTYPE = A.AMM0_PROD_SUBTYPE
             and R.AMM0_PARAM_KEY = A.AMM0_PARAM_KEY
             and R.AMM0_KIND_ID2 = A.AMM0_KIND_ID2
             and R.AMM0_KIND_ID = A.AMM0_KIND_ID
             and R.AMM0_PROD_ID = A.AMM0_PROD_ID
             and R.AMM0_DATA_TYPE = 'M'
             and R.AMM0_YMD >= :as_symd 
             and R.AMM0_YMD <= :as_eymd) as AMM0_MARKET_M_QNTY, 
         NVL(sum(AMM0_KEEP_TIME),0) as AMM0_KEEP_TIME,
         NVL(sum(AMM0_O_SUBTRACT_QNTY),0) as AMM0_O_SUBTRACT_QNTY,
         NVL(sum(AMM0_Q_SUBTRACT_QNTY),0) as AMM0_Q_SUBTRACT_QNTY,
         NVL(sum(AMM0_IQM_QNTY),0) as AMM0_IQM_QNTY,
         NVL(sum(AMM0_IQM_SUBTRACT_QNTY),0) as AMM0_IQM_SUBTRACT_QNTY,
         NVL(sum(AMM0_BTRADE_M_QNTY),0) as AMM0_BTRADE_M_QNTY,
         CAST('' AS NUMBER(8,0)) AS AMM0_TRD_INVALID_QNTY,
         COUNT(DISTINCT (AMM0_YMD)) AS AMM0_DAY_COUNT
    FROM ci.AMM0AH  A
   WHERE AMM0_SUM_TYPE = :as_sum_type  AND  
         AMM0_SUM_SUBTYPE = :as_sum_subtype   AND  
         AMM0_DATA_TYPE = :as_data_type 
         {0}
 GROUP BY A.AMM0_BRK_NO,
          A.AMM0_ACC_NO,
          A.AMM0_SUM_TYPE,
          A.AMM0_SUM_SUBTYPE,
          A.AMM0_DATA_TYPE,
          A.AMM0_PROD_TYPE,
          A.AMM0_PROD_SUBTYPE,
          AMM0_PARAM_KEY,
          A.AMM0_KIND_ID2,
          A.AMM0_KIND_ID,
          A.AMM0_PROD_ID) main
ORDER BY  
decode(:as_sort_type,'F',AMM0_BRK_NO,AMM0_BRK_NO||AMM0_ACC_NO ,AMM0_PROD_TYPE||AMM0_PROD_ID ),
decode(:as_sort_type ,'F',AMM0_PROD_TYPE||AMM0_PROD_ID, AMM0_BRK_NO||AMM0_ACC_NO )
", iswhere);
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListD50030AccuAh

      /// <summary>
      /// d_50030_ah
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
 SELECT LV2.*,  decode(AMMF_CP_KIND,'2',case when(CP_CHK2 = 0 and CP_CHK3 = 0) then 0 else 1 end,case when(CP_CHK1 = 0 and CP_CHK2 = 0 and CP_CHK3 = 0) then 0 else 1 end) as CP_CHK,
 case when (LV2.AMM0_YMD <> nvl(NEXT_YMD,0)) then AMM0_MARKET_M_QNTY else 0 end as CP_MARKET_M_QNTY
FROM
(SELECT LV1.*,
case when CP_RATE_VALID_CNT >=MMF_RESP_RATIO then 0 else 1 end as CP_CHK1,
case when (AMM0_KEEP_FLAG='Y'  or  MMF_AVG_TIME =0) then 0 else 1 end as CP_CHK2,
case when  AMMF_CP_KIND in ('A','C') then (case when MMK_QNTY >= MMF_QNTY_LOW then 0 else 1 end) else (case when CP_AVG_MMK_QNTY >=MMF_QNTY_LOW then 0 else 1 end) end as CP_CHK3,
 Lead (LV1.AMM0_YMD) over (order by AMM0_YMD, CP_GROUP1 , CP_GROUP2) as NEXT_YMD
FROM
(SELECT main.*,
trim(AMM0_PROD_ID )||decode(AMM0_BASIC_PROD,'Y','*','') as CP_PROD_ID,
 decode(AMM0_MARKET_M_QNTY ,0,0, round(CP_M_QNTY / AMM0_MARKET_M_QNTY,16)) * 100 as CP_RATE_M,
 decode (AMM0_MARKET_R_CNT ,0, 1 ,round(AMM0_VALID_CNT / AMM0_MARKET_R_CNT,4)) * 100 as CP_RATE_VALID_REAL,
 decode(AMM0_SUM_TYPE,'D', CEIL( NVL(TRUNC(AMM0_KEEP_TIME,0) / 60/ NULLIF(AMM0_DAY_COUNT,0),0)) , CEIL( NVL(AMM0_KEEP_TIME / 60/ NULLIF(AMM0_DAY_COUNT,0),0))) as CP_KEEP_TIME,
 NVL(TRUNC(MMK_QNTY /  NULLIF(AMM0_DAY_COUNT,0) ,1),0) as CP_AVG_MMK_QNTY,
 decode(AMM0_MARKET_R_CNT,0,100,AMM0_RQ_RATE) as CP_RATE_VALID_CNT,
 decode(:as_sort_type,'F' ,AMM0_BRK_NO||AMM0_ACC_NO,AMM0_PROD_TYPE||AMM0_PROD_ID ) as CP_GROUP1,
 decode( :as_sort_type ,'F',AMM0_PROD_TYPE||AMM0_PROD_ID, AMM0_BRK_NO||AMM0_ACC_NO ) as CP_GROUP2
FROM
(SELECT AMM0_YMD,   
         AMM0_BRK_NO,
         AMM0_ACC_NO,
        (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
          WHERE ABRK_NO = AMM0_BRK_NO ) as BRK_ABBR_NAME,  
         AMM0_PROD_TYPE,
         case AMM0_SUM_SUBTYPE 
              when '1' then AMM0_PROD_TYPE
              when '2' then AMM0_PROD_SUBTYPE
              when '3' then case AMM0_PARAM_KEY  when 'STF    ' then 'STF*   ' when 'STC    ' then 'STC*   ' else AMM0_PARAM_KEY end
              when 'S' then  AMM0_PARAM_KEY
              when '4' then case when length(trim(AMM0_KIND_ID2)) = 2 then trim(AMM0_KIND_ID2)||AMM0_PROD_TYPE else trim(AMM0_KIND_ID2) end
              when '5' then AMM0_KIND_ID
              else AMM0_PROD_ID end as AMM0_PROD_ID,
         AMM0_CNT,   
         AMM0_VALID_CNT,   
         AMM0_OM_QNTY,   
         AMM0_QM_QNTY,  
         AMM0_MARKET_R_CNT, 
         AMM0_MARKET_M_QNTY,
         NVL(AMMF_QUOTE_VALID_RATE,0) as MMF_QUOTE_VALID_RATE,
         NVL(AMMF_RESP_RATIO,99999) as MMF_RESP_RATIO,
         NVL(AMMF_QNTY_LOW,0) as MMF_QNTY_LOW,
         0 as TOT_R,
         0 as TOT_M,
         AMM0_KEEP_TIME,
         AMM0_KEEP_FLAG,
         AMM0_SUM_TYPE,
         AMM0_SUM_SUBTYPE, NVL(AMMF_AVG_TIME,0) as MMF_AVG_TIME,
         AMM0_O_SUBTRACT_QNTY,
         AMM0_Q_SUBTRACT_QNTY,
         nvl(AMM0_IQM_QNTY,0) as AMM0_IQM_QNTY,
         nvl(AMM0_IQM_SUBTRACT_QNTY,0) as AMM0_IQM_SUBTRACT_QNTY,
         AMM0_BASIC_PROD,
         AMM0_DAY_COUNT,
         LEAST(nvl(AMM0_MM_QNTY,0),nvl(AMM0_MAX_MM_QNTY,0)) as mmk_qnty,
         AMM0_TRD_INVALID_QNTY,
         AMM0_BTRADE_M_QNTY,
         AMM0_RQ_RATE,
         AMMF_RFC_MIN_CNT as MMF_RFC_MIN_CNT,
         AMMF_CP_KIND,
         ((AMM0_OM_QNTY + AMM0_QM_QNTY + nvl(AMM0_IQM_QNTY,0))+decode(NULLIF(AMM0_BTRADE_M_QNTY,null),0,AMM0_BTRADE_M_QNTY)) as CP_M_QNTY
    FROM ci.AMM0AH,ci.AMMF
   WHERE AMM0_SUM_TYPE = :as_sum_type  AND  
         AMM0_SUM_SUBTYPE = :as_sum_subtype   AND  
         AMM0_DATA_TYPE = :as_data_type 
     and AMM0_PARAM_KEY = AMMF_PARAM_KEY (+)
     and substr(AMM0_YMD,1,6) = AMMF_YM (+)
     and '1' = AMMF_MARKET_CODE(+)
     {0}
) main
ORDER BY AMM0_YMD, CP_GROUP1,CP_GROUP2
) LV1
) LV2
", iswhere);
         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }//public DataTable ListD50030Ah

   }
}
