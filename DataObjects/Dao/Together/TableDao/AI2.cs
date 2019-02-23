using OnePiece;
using System;
using System.Data;

/// <summary>
/// ken 2018/12/27
/// </summary>
namespace DataObjects.Dao.Together
{
   /// <summary>
   /// AI5券商資訊?
   /// </summary>
   public class AI2
   {
      private Db db;

      public AI2()
      {
         db = GlobalDaoSetting.DB;
      }


      /// <summary>
      /// Get max(AI2_YMD) to string [yyyyMMdd]
      /// </summary>
      /// <param name="kindId"></param>
      /// <param name="endDate"></param>
      /// <param name="sumType"></param>
      /// <returns>yyyyMMdd</returns>
      public string GetMaxDate(string kindId, DateTime endDate, string sumType = "D")
      {
         object[] parms =
         {
                ":as_kind_id", kindId,
                ":ls_date", endDate.ToString("yyyyMMdd"),
                ":sum_type", sumType
            };

         //AI2_YMD format=yyyyMMdd
         string sql = @"
select max(AI2_YMD) as maxdate
from ci.AI2
where trim(AI2_KIND_ID) LIKE :as_kind_id
and AI2_YMD < :ls_date
and AI2_SUM_TYPE = :sum_type
";

         string res = db.ExecuteScalar(sql, CommandType.Text, parms);
         return res;
      }


      /// <summary>
      /// 上個月的倒數n天交易日 to string [yyyyMMdd]
      /// </summary>
      /// <param name="kindId"></param>
      /// <param name="thisMonthDate"></param>
      /// <param name="sumType"></param>
      /// <param name="interval"></param>
      /// <returns>yyyyMMdd</returns>
      public string GetLastMonthLastTradeDate(string kindId, DateTime thisMonthDate, string sumType = "D", int interval = 1)
      {
         object[] parms =
         {
                ":as_kind_id", kindId,
                ":thisMonthDate", thisMonthDate.ToString("yyyyMMdd"),
                ":sum_type", sumType
            };

         //AI2_YMD format=yyyyMMdd
         string sql = string.Format(@"
select AI2_YMD from (
    select AI2_YMD from (
        select AI2_YMD
        from AI2
        where trim(AI2_KIND_ID) = trim(:as_kind_id)
        and AI2_SUM_TYPE = :sum_type
        and AI2_YMD < :thisMonthDate
        --ken,原PB一直往前抓交易日,程式碼與函數定義的[上個月的倒數n天交易日]有差
        --and AI2_YMD >= to_char(to_date(:thisMonthDate,'yyyymmdd')-32,'yyyymmdd')
        group by AI2_YMD
        order by AI2_YMD desc
    ) a
    where rownum <= {0}
    order by AI2_YMD
) b
where rownum=1", interval);

         string res = db.ExecuteScalar(sql, CommandType.Text, parms);
         return res;
      }


      /// <summary>
      /// 以AI2_YMD為主,以每一周為一筆,輸出欄位yyyyIW/startDate/endDate
      /// </summary>
      /// <param name="startDate">include</param>
      /// <param name="endDate">include</param>
      /// <param name="sumType">include</param>
      /// <param name="prodType">include</param>
      /// <returns>yyyyIW/startDate/endDate</returns>
      public DataTable ListWeek(DateTime startDate, DateTime endDate, string sumType = "D", string prodType = "F")
      {
         object[] parms =
         {
                ":as_symd", startDate.ToString("yyyyMMdd"),
                ":as_eymd", endDate.ToString("yyyyMMdd"),
                ":sumType", sumType,
                ":prodType", prodType
            };

         //AI2_YMD format=yyyyMMdd
         //ken,oracle的date可以用to_char(d,'IW')找到該天為當年的哪一周,加上yyyy可以以周為單位做區分
         string sql = @"
SELECT substr(AI2_YMD,1,4)||to_char(to_date(AI2_YMD,'yyyymmdd'),'IW') as week,
min(to_date(AI2_YMD,'yyyymmdd')) as startDate,
max(to_date(AI2_YMD,'yyyymmdd')) as endDate
FROM ci.AI2
WHERE length(trim(AI2_YMD))=8
and AI2_SUM_TYPE = 'D'
and AI2_PROD_TYPE = 'F'
and AI2_YMD >= :as_symd  
and AI2_YMD <= :as_eymd
group by substr(AI2_YMD,1,4)||to_char(to_date(AI2_YMD,'yyyymmdd'),'IW')
order by week";

         return db.GetDataTable(sql, parms);

      }

      /// <summary>
      /// AI2_YMD
      /// </summary>
      /// <param name="startDate"></param>
      /// <param name="endDate"></param>
      /// <returns>AI2_YMD</returns>
      public DataTable ListAI2Date(DateTime startDate, DateTime endDate, string sumType = "D", string prodType = "F")
      {
         object[] parms =
         {
                ":as_symd", startDate.ToString("yyyyMMdd"),
                ":as_eymd", endDate.ToString("yyyyMMdd"),
                ":sumType", sumType,
                ":prodType", prodType
            };

         //AI2_YMD format=yyyyMMdd
         string sql = @"
SELECT AI2_YMD
FROM ci.AI2 
WHERE AI2_SUM_TYPE  = :sumType
and AI2_PROD_TYPE = :prodType
and AI2_YMD >= :as_symd  
and AI2_YMD <= :as_eymd 
group by AI2_YMD
order by AI2_YMD
";

         return db.GetDataTable(sql, parms);

      }
      /// <summary>
      /// d_ai2_ym
      /// </summary>
      /// <param name="as_kind_id">ex: TXF</param>
      /// <param name="as_ym">起始日期</param>
      /// <param name="as_last_ym">終止日期</param>
      /// <returns></returns>
      public DataTable ListAI2ym(string as_kind_id, string as_ym, string as_last_ym)
      {
         object[] parms =
         {
                ":as_kind_id", as_kind_id,
                ":as_ym", as_ym,
                ":as_last_ym", as_last_ym
            };

         string sql = @"
                     select AI2_PARAM_KEY, 
                            sum(case when TRIM(AI2_YMD) >= :as_last_ym||'01' and TRIM(AI2_YMD) <= :as_last_ym||'31' then AI2_M_QNTY else 0 end) as LAST_M_QNTY,
                            sum(case when TRIM(AI2_YMD) >= :as_last_ym||'01' and TRIM(AI2_YMD) <= :as_last_ym||'31' then AI2_OI else 0 end) as LAST_M_OI,
                            sum(case when TRIM(AI2_YMD) >= :as_last_ym||'01' and TRIM(AI2_YMD) <= :as_last_ym||'31' then 1 else 0 end) as LAST_M_DAY_CNT,
                            sum(case when TRIM(AI2_YMD) >= :as_ym||'01' and TRIM(AI2_YMD) <= :as_ym||'31' then AI2_M_QNTY else 0 end) as CUR_M_QNTY,
                            sum(case when TRIM(AI2_YMD) >= :as_ym||'01' and TRIM(AI2_YMD) <= :as_ym||'31' then AI2_OI else 0 end) as CUR_M_OI,
                            sum(case when TRIM(AI2_YMD) >= :as_ym||'01' and TRIM(AI2_YMD) <= :as_ym||'31' then 1 else 0 end) as CUR_M_DAY_CNT,
                            sum(case when TRIM(AI2_YMD) >= substr(:as_ym,1,4)||'0101'  and TRIM(AI2_YMD) <= :as_ym||'31' then AI2_M_QNTY else 0 end) as Y_QNTY,
                            sum(case when TRIM(AI2_YMD) >= substr(:as_ym,1,4)||'0101'  and TRIM(AI2_YMD) <= :as_ym||'31' then AI2_OI else 0 end) as Y_OI,
                            sum(case when TRIM(AI2_YMD) >= substr(:as_ym,1,4)||'0101'  and TRIM(AI2_YMD) <= :as_ym||'31' then 1 else 0 end) AS Y_DAY_CNT 
                       from CI.AI2 
                      where TRIM(AI2_YMD) >= case when substr(:as_last_ym,1,4) < substr(:as_ym,1,4) then :as_last_ym||'01' else substr(:as_ym,1,4)||'0101'  end 
                        and TRIM(AI2_YMD) <= :as_ym||'31'
                        and TRIM(AI2_SUM_TYPE) = 'D'
                        and TRIM(AI2_SUM_SUBTYPE) = '3'
                        and TRIM(AI2_PROD_TYPE) = 'F'
                        and TRIM(AI2_PARAM_KEY) = :as_kind_id
                      group by AI2_PARAM_KEY
";

         return db.GetDataTable(sql, parms);

      }
   }
}
