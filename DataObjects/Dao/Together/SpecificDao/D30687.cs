﻿using OnePiece;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DataObjects.Dao.Together.SpecificDao
{
   //John, 20190227
   public class D30687
   {
      private Db db;

      public D30687()
      {
         db = GlobalDaoSetting.DB;
      }

      public DataTable ListRuNewData(string startdate, string enddate, string as_prod_id, string as_market_code, string as_data_type)
      {
         object[] parms = {
               ":startdate",startdate,
               ":enddate",enddate,
               ":as_prod_id",as_prod_id,
               ":as_market_code",as_market_code,
               ":as_data_type",as_data_type
            };

         string sql = @"
SELECT FTPRICELOGS_MARKET_CODE,FTPRICELOGS_YMD,FTPRICELOGS_KIND_ID1,FTPRICELOGS_SEQ_NO1,FTPRICELOGS_KIND_ID2,FTPRICELOGS_SEQ_NO2,SUM(FTPRICELOGS_CNT),
       --MAX(RU_SEQ_NO) as RU_SEQ_NO,
       SUM(CASE WHEN RU_SEQ_NO = 1  THEN FTPRICELOGS_CNT ELSE 0 END) as R01,
       SUM(CASE WHEN RU_SEQ_NO = 2  THEN FTPRICELOGS_CNT ELSE 0 END) as R02,
       SUM(CASE WHEN RU_SEQ_NO = 3  THEN FTPRICELOGS_CNT ELSE 0 END) as R03,
       SUM(CASE WHEN RU_SEQ_NO = 4  THEN FTPRICELOGS_CNT ELSE 0 END) as R04,
       SUM(CASE WHEN RU_SEQ_NO = 5  THEN FTPRICELOGS_CNT ELSE 0 END) as R05,
       SUM(CASE WHEN RU_SEQ_NO = 6  THEN FTPRICELOGS_CNT ELSE 0 END) as R06,
       SUM(CASE WHEN RU_SEQ_NO = 7  THEN FTPRICELOGS_CNT ELSE 0 END) as R07,
       SUM(CASE WHEN RU_SEQ_NO = 8  THEN FTPRICELOGS_CNT ELSE 0 END) as R08,
       SUM(CASE WHEN RU_SEQ_NO = 9  THEN FTPRICELOGS_CNT ELSE 0 END) as R09,
       SUM(CASE WHEN RU_SEQ_NO = 10 THEN FTPRICELOGS_CNT ELSE 0 END) as R10,
       SUM(CASE WHEN RU_SEQ_NO = 11 THEN FTPRICELOGS_CNT ELSE 0 END) as R11,
       SUM(CASE WHEN RU_SEQ_NO = 12 THEN FTPRICELOGS_CNT ELSE 0 END) as R12,
       SUM(CASE WHEN RU_SEQ_NO = 13 THEN FTPRICELOGS_CNT ELSE 0 END) as R13,
       SUM(CASE WHEN RU_SEQ_NO = 14 THEN FTPRICELOGS_CNT ELSE 0 END) as R14,
       SUM(CASE WHEN RU_SEQ_NO = 15 THEN FTPRICELOGS_CNT ELSE 0 END) as R15,
       SUM(CASE WHEN RU_SEQ_NO = 16 THEN FTPRICELOGS_CNT ELSE 0 END) as R16,
       SUM(CASE WHEN RU_SEQ_NO = 17 THEN FTPRICELOGS_CNT ELSE 0 END) as R17,
       SUM(CASE WHEN RU_SEQ_NO = 18 THEN FTPRICELOGS_CNT ELSE 0 END) as R18,
       SUM(CASE WHEN RU_SEQ_NO = 19 THEN FTPRICELOGS_CNT ELSE 0 END) as R19,
       SUM(CASE WHEN RU_SEQ_NO = 20 THEN FTPRICELOGS_CNT ELSE 0 END) as R20,
       SUM(CASE WHEN RU_SEQ_NO = 21 THEN FTPRICELOGS_CNT ELSE 0 END) as R21,
       SUM(CASE WHEN RU_SEQ_NO = 22 THEN FTPRICELOGS_CNT ELSE 0 END) as R22,
       SUM(CASE WHEN RU_SEQ_NO = 23 THEN FTPRICELOGS_CNT ELSE 0 END) as R23,
       SUM(CASE WHEN RU_SEQ_NO = 24 THEN FTPRICELOGS_CNT ELSE 0 END) as R24,
       SUM(CASE WHEN RU_SEQ_NO = 25 THEN FTPRICELOGS_CNT ELSE 0 END) as R25
  from ci.FTPRICELOGS,
       --RU個數
      (SELECT FTPRICELOGS_RU as RU_VAL,
              ROW_NUMBER( ) OVER (ORDER BY FTPRICELOGS_RU NULLS LAST) as RU_SEQ_NO
         FROM ci.FTPRICELOGS
        WHERE FTPRICELOGS_YMD >= :startdate
          AND FTPRICELOGS_YMD <= :enddate
          AND FTPRICELOGS_PROD LIKE :as_prod_id 
          AND FTPRICELOGS_MARKET_CODE LIKE :as_market_code
          AND FTPRICELOGS_DATA_TYPE LIKE :as_data_type
        GROUP BY FTPRICELOGS_RU) R
 WHERE FTPRICELOGS_YMD >= :startdate
   AND FTPRICELOGS_YMD <= :enddate
   AND FTPRICELOGS_PROD LIKE :as_prod_id 
 AND FTPRICELOGS_MARKET_CODE LIKE :as_market_code
 AND FTPRICELOGS_DATA_TYPE LIKE :as_data_type
   --AND FTPRICELOGS_SEQ_NO1 IN (1,2)
   --AND (FTPRICELOGS_SEQ_NO2 = 2 OR FTPRICELOGS_SEQ_NO2 IS NULL)
   AND FTPRICELOGS_RU = RU_VAL
 GROUP BY FTPRICELOGS_MARKET_CODE,FTPRICELOGS_YMD,FTPRICELOGS_KIND_ID1,FTPRICELOGS_SEQ_NO1,FTPRICELOGS_KIND_ID2,FTPRICELOGS_SEQ_NO2
 UNION ALL 
SELECT '','',NULL,NULL,NULL,NULL,NULL,
       --MAX(RU_SEQ_NO) as RU_SEQ_NO,
       MAX(CASE WHEN RU_SEQ_NO = 1  THEN RU_VAL ELSE 0 END) as R01,
       MAX(CASE WHEN RU_SEQ_NO = 2  THEN RU_VAL ELSE 0 END) as R02,
       MAX(CASE WHEN RU_SEQ_NO = 3  THEN RU_VAL ELSE 0 END) as R03,
       MAX(CASE WHEN RU_SEQ_NO = 4  THEN RU_VAL ELSE 0 END) as R04,
       MAX(CASE WHEN RU_SEQ_NO = 5  THEN RU_VAL ELSE 0 END) as R05,
       MAX(CASE WHEN RU_SEQ_NO = 6  THEN RU_VAL ELSE 0 END) as R06,
       MAX(CASE WHEN RU_SEQ_NO = 7  THEN RU_VAL ELSE 0 END) as R07,
       MAX(CASE WHEN RU_SEQ_NO = 8  THEN RU_VAL ELSE 0 END) as R08,
       MAX(CASE WHEN RU_SEQ_NO = 9  THEN RU_VAL ELSE 0 END) as R09,
       MAX(CASE WHEN RU_SEQ_NO = 10 THEN RU_VAL ELSE 0 END) as R10,
       MAX(CASE WHEN RU_SEQ_NO = 11 THEN RU_VAL ELSE 0 END) as R11,
       MAX(CASE WHEN RU_SEQ_NO = 12 THEN RU_VAL ELSE 0 END) as R12,
       MAX(CASE WHEN RU_SEQ_NO = 13 THEN RU_VAL ELSE 0 END) as R13,
       MAX(CASE WHEN RU_SEQ_NO = 14 THEN RU_VAL ELSE 0 END) as R14,
       MAX(CASE WHEN RU_SEQ_NO = 15 THEN RU_VAL ELSE 0 END) as R15,
       MAX(CASE WHEN RU_SEQ_NO = 16 THEN RU_VAL ELSE 0 END) as R16,
       MAX(CASE WHEN RU_SEQ_NO = 17 THEN RU_VAL ELSE 0 END) as R17,
       MAX(CASE WHEN RU_SEQ_NO = 18 THEN RU_VAL ELSE 0 END) as R18,
       MAX(CASE WHEN RU_SEQ_NO = 19 THEN RU_VAL ELSE 0 END) as R19,
       MAX(CASE WHEN RU_SEQ_NO = 20 THEN RU_VAL ELSE 0 END) as R20,
       MAX(CASE WHEN RU_SEQ_NO = 21 THEN RU_VAL ELSE 0 END) as R21,
       MAX(CASE WHEN RU_SEQ_NO = 22 THEN RU_VAL ELSE 0 END) as R22,
       MAX(CASE WHEN RU_SEQ_NO = 23 THEN RU_VAL ELSE 0 END) as R23,
       MAX(CASE WHEN RU_SEQ_NO = 24 THEN RU_VAL ELSE 0 END) as R24,
       MAX(CASE WHEN RU_SEQ_NO = 25 THEN RU_VAL ELSE 0 END) as R25
  from 
       --RU個數
      (SELECT FTPRICELOGS_RU as RU_VAL,
              ROW_NUMBER( ) OVER (ORDER BY FTPRICELOGS_RU NULLS LAST) as RU_SEQ_NO
         FROM ci.FTPRICELOGS
        WHERE FTPRICELOGS_YMD >= :startdate
          AND FTPRICELOGS_YMD <= :enddate
          AND FTPRICELOGS_PROD LIKE :as_prod_id 
          AND FTPRICELOGS_MARKET_CODE LIKE :as_market_code
          AND FTPRICELOGS_DATA_TYPE LIKE :as_data_type
        GROUP BY FTPRICELOGS_RU) R
 ORDER BY FTPRICELOGS_MARKET_CODE,FTPRICELOGS_YMD,FTPRICELOGS_KIND_ID1,FTPRICELOGS_KIND_ID2,FTPRICELOGS_SEQ_NO1,FTPRICELOGS_SEQ_NO2
";

         DataTable dtResult = db.GetDataTable(sql, parms);
         //PB sort排序
         dtResult = dtResult.AsEnumerable().OrderBy(x => {
            if (string.IsNullOrEmpty(x.Field<string>("FTPRICELOGS_MARKET_CODE")))
               return 1;
            else
               return 2;
         }).ThenBy(x => x.Field<string>("FTPRICELOGS_MARKET_CODE"))
         .ThenBy(x => x.Field<string>("FTPRICELOGS_YMD"))
         .ThenBy(x => x.Field<string>("FTPRICELOGS_KIND_ID1"))
         .ThenBy(x=> {
            if (string.IsNullOrEmpty(x.Field<string>("FTPRICELOGS_KIND_ID2")))
               return 1;
            else
               return 2;
         })
         .ThenBy(x => x.Field<decimal?>("FTPRICELOGS_SEQ_NO1"))
         .ThenBy(x => x.Field<string>("FTPRICELOGS_KIND_ID2"))
         .ThenBy(x => x.Field<decimal?>("FTPRICELOGS_SEQ_NO2"))
         .CopyToDataTable();
         return dtResult;
      }

   }
}