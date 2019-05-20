using OnePiece;
using System;
using System.Data;

/// <summary>
/// Winni 2019/02/20
/// </summary>
namespace DataObjects.Dao.Together {
   public class D30610 : DataGate {

      /// <summary>
      /// Get 月明細表 by CI.AI6, CI.AMIF, CI.INTWSE1 , CI.AI2 (14個欄位)
      /// d_30611
      /// </summary>
      /// <param name="adt_sdate">查詢起始月(yyyyMM01)</param>
      /// <param name="adt_edate">查詢結束月(yyyyMM31)</param>
      /// <returns></returns>
      public DataTable GetMonData(DateTime adt_sdate , DateTime adt_edate) {
         object[] parms =
         {
                ":adt_sdate", adt_sdate,
                ":adt_edate", adt_edate
            };

         string sql = @"
select AMIF_YM,AMIF_TOT_CNT,
       --現貨市場
       TFXM_AVG_UP_DOWN,TFXM_CNT,RETURN_P2,TFXM_AVG_CLOSE_PRICE,
       --平均現貨成交值(億元)
       TWSE_AMT as TFXM_M_QNTY_TAL, --TFXM_M_QNTY_TAL,
       --期貨市場
       AMIF_AVG_UP_DOWN,AMIF_CNT,RETURN_P1,AMIF_AVG_CLOSE_PRICE,
       --日均量
       AI2_AVG_QTY_TXF,AI2_AVG_QTY_TXO,AI2_AVG_TOT_QTY
  from
       --指數波動度(%)
      (SELECT TO_CHAR(AI6_DATE,'YYYYMM') as P_YM,
              round(STDDEV(AI6_LN_RETURN) * SQRT(256),3) as RETURN_P1,
              round(STDDEV(AI6_TFXM_LN_RETURN) * SQRT(256),3) as RETURN_P2
         FROM ci.AI6
        WHERE AI6_DATE >= :adt_sdate
          AND AI6_DATE <= :adt_edate
          and AI6_KIND_ID = 'TXF'
        GROUP BY TO_CHAR(AI6_DATE,'YYYYMM')) P,
       --現貨市場
      (SELECT TO_CHAR(AMIF_DATE,'YYYYMM') as TFXM_YM,
              ROUND(AVG(AMIF_HIGH_PRICE - AMIF_LOW_PRICE),2) as TFXM_AVG_UP_DOWN,
              SUM(case when AMIF_HIGH_PRICE - AMIF_LOW_PRICE < 100 then 1 else 0 end) as TFXM_CNT,
              ROUND(AVG(AMIF_CLOSE_PRICE),2) as TFXM_AVG_CLOSE_PRICE,
              ROUND(AVG(AMIF_SUM_AMT),2) as TFXM_M_QNTY_TAL
         FROM ci.AMIF
        WHERE AMIF_DATE >= :adt_sdate
          AND AMIF_DATE <= :adt_edate
          AND AMIF_PROD_ID = 'TXF00'
        GROUP BY TO_CHAR(AMIF_DATE,'YYYYMM')) TFXM,
       --現貨成交值
      (SELECT SUBSTR(INTWSE1_YMD,1,6) as TWSE_YM,
              ROUND(AVG(INTWSE1_TRADE_AMT)  / 100000000 ,2) as TWSE_AMT
         FROM ci.INTWSE1
        WHERE INTWSE1_YMD >= TO_CHAR(:adt_sdate,'YYYYMMDD')
          AND INTWSE1_YMD <= TO_CHAR(:adt_edate,'YYYYMMDD')
        GROUP BY SUBSTR(INTWSE1_YMD,1,6)) TWSE,
       --期貨市場
      (SELECT TO_CHAR(AMIF_DATE,'YYYYMM') as AMIF_YM,
              ROUND(AVG(AMIF_HIGH_PRICE - AMIF_LOW_PRICE),0) as AMIF_AVG_UP_DOWN,
              SUM(case when AMIF_HIGH_PRICE - AMIF_LOW_PRICE < 100 then 1 else 0 end) as AMIF_CNT,
              SUM(1) as AMIF_TOT_CNT,
              ROUND(AVG(AMIF_CLOSE_PRICE),0) as AMIF_AVG_CLOSE_PRICE
         FROM ci.AMIF
        WHERE AMIF_DATE >= :adt_sdate
          AND AMIF_DATE <= :adt_edate
          AND AMIF_KIND_ID = 'TXF'
          AND AMIF_MTH_SEQ_NO = 1
        GROUP BY TO_CHAR(AMIF_DATE,'YYYYMM')) AM,
       --商品別日均量(口)        
      (select substr(AI2_YMD,1,6) as AI2_YM,round(AVG(QTY_TXF),0) as AI2_AVG_QTY_TXF,round(AVG(QTY_TXO),0) as AI2_AVG_QTY_TXO
         from
             (SELECT AI2_YMD,ROUND(
                            SUM(CASE WHEN AI2_PARAM_KEY = 'TXF' THEN AI2_M_QNTY ELSE 0 END) + 
                           (SUM(CASE WHEN AI2_PARAM_KEY = 'MXF' THEN AI2_M_QNTY ELSE 0 END) / 4),0) as QTY_TXF,
                    SUM(CASE WHEN AI2_PARAM_KEY = 'TXO' THEN AI2_M_QNTY ELSE 0 END) as QTY_TXO       
                FROM ci.AI2
               WHERE AI2_YMD >= to_char(:adt_sdate,'yyyymmdd')
                 AND AI2_YMD <= to_char(:adt_edate,'yyyymmdd')
                 AND AI2_SUM_TYPE = 'D'
                 AND AI2_SUM_SUBTYPE = '3'
                 AND AI2_PARAM_KEY IN ('TXF','MXF','TXO')
               GROUP BY AI2_YMD)
        GROUP BY substr(AI2_YMD,1,6)) I,
       --全市場日均量(口)        
      (select substr(AI2_YMD,1,6) as AI2_TOT_YM,round(AVG(AI2_TOT_QTY),0) as AI2_AVG_TOT_QTY
         from
             (SELECT AI2_YMD,SUM(AI2_M_QNTY) AS AI2_TOT_QTY       
                FROM ci.AI2
               WHERE AI2_YMD >= to_char(:adt_sdate,'yyyymmdd')
                 AND AI2_YMD <= to_char(:adt_edate,'yyyymmdd')
                 AND AI2_SUM_TYPE = 'D'
                 AND AI2_SUM_SUBTYPE = '1'
                 AND AI2_PROD_TYPE IN ('F','O')
               GROUP BY AI2_YMD)
        GROUP BY substr(AI2_YMD,1,6)) J
where AMIF_YM = P_YM
  and AMIF_YM = TFXM_YM
  and AMIF_YM = TWSE_YM(+)
  and AMIF_YM = AI2_YM
  and AMIF_YM = AI2_TOT_YM
  order by amif_ym 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// Get 日明細表 by CI.AMIF , CI.AI2  (d_30612)
      /// (MIF_YM/TFXM_UP_DOWN/TFXM_CLOSE_PRICE/TFXM_M_QNTY_TAL/AMIF_UP_DOWN/AMIF_CLOSE_PRICE/AI2_QTY_TXF/AI2_QTY_TXO/AI2_TOT_QTY)
      /// </summary>
      /// <param name="as_sdate">查詢起始日(yyyyMMdd)</param>
      /// <param name="as_edate">查詢結束日(yyyyMMdd)</param>
      /// <returns></returns>
      public DataTable GetDayData(DateTime adt_sdate , DateTime adt_edate) {
         object[] parms =
         {
                ":adt_sdate", adt_sdate,
                ":adt_edate", adt_edate
            };

         string sql = @"
select AMIF_YM,TFXM_UP_DOWN,TFXM_CLOSE_PRICE,TFXM_M_QNTY_TAL,
       AMIF_UP_DOWN,AMIF_CLOSE_PRICE,
       AI2_QTY_TXF,AI2_QTY_TXO,AI2_TOT_QTY
from
      (SELECT TO_CHAR(AMIF_DATE,'YYYYMMDD') as TFXM_YM,
              (AMIF_HIGH_PRICE - AMIF_LOW_PRICE) as TFXM_UP_DOWN,
              (AMIF_CLOSE_PRICE) as TFXM_CLOSE_PRICE,
              (AMIF_M_QNTY_TAL) as TFXM_M_QNTY_TAL
         FROM CI.AMIF
        WHERE AMIF_DATE >= :adt_sdate
          AND AMIF_DATE <= :adt_edate
          AND AMIF_PROD_ID = 'TXF00') TFXM,
      (SELECT TO_CHAR(AMIF_DATE,'YYYYMMDD') as AMIF_YM,
              (AMIF_HIGH_PRICE - AMIF_LOW_PRICE) as AMIF_UP_DOWN,
              AMIF_CLOSE_PRICE as AMIF_CLOSE_PRICE
         FROM CI.AMIF
        WHERE AMIF_DATE >= :adt_sdate
          AND AMIF_DATE <= :adt_edate
          AND AMIF_KIND_ID = 'TXF'
          AND AMIF_MTH_SEQ_NO = 1) AM,        
      (SELECT AI2_YMD as AI2_YM,ROUND(SUM(CASE WHEN AI2_PARAM_KEY = 'TXF' THEN AI2_M_QNTY ELSE 0 END) + 
                    (SUM(CASE WHEN AI2_PARAM_KEY = 'MXF' THEN AI2_M_QNTY ELSE 0 END) / 4),0) as  AI2_QTY_TXF,
              SUM(CASE WHEN AI2_PARAM_KEY = 'TXO' THEN AI2_M_QNTY ELSE 0 END) as AI2_QTY_TXO       
         FROM CI.AI2
        WHERE AI2_YMD >= to_char(:adt_sdate,'yyyymmdd')
          AND AI2_YMD <= to_char(:adt_edate,'yyyymmdd')
          AND AI2_SUM_TYPE = 'D'
          AND AI2_SUM_SUBTYPE = '3'
          AND AI2_PARAM_KEY IN ('TXF','MXF','TXO')
        GROUP BY AI2_YMD) I,
      (SELECT AI2_YMD as AI2_TOT_YM,SUM(AI2_M_QNTY) AS AI2_TOT_QTY       
         FROM CI.AI2
        WHERE AI2_YMD >= to_char(:adt_sdate,'yyyymmdd')
          AND AI2_YMD <= to_char(:adt_edate,'yyyymmdd')
          AND AI2_SUM_TYPE = 'D'
          AND AI2_SUM_SUBTYPE = '1'
          AND AI2_PROD_TYPE IN ('F','O')
        GROUP BY AI2_YMD) J
where AMIF_YM = TFXM_YM
  and AMIF_YM = AI2_YM
  and AMIF_YM = AI2_TOT_YM
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
