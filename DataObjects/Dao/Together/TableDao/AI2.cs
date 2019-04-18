using OnePiece;
using System;
using System.Data;

/// <summary>
/// ken 2018/12/27
/// </summary>
namespace DataObjects.Dao.Together {
   /// <summary>
   /// 今日轉行情統計檔
   /// </summary>
   public class AI2 {
      private Db db;

      public AI2() {
         db = GlobalDaoSetting.DB;
      }


      /// <summary>
      /// Get max(AI2_YMD) to string [yyyyMMdd]
      /// </summary>
      /// <param name="kindId"></param>
      /// <param name="endDate"></param>
      /// <param name="sumType"></param>
      /// <returns>yyyyMMdd</returns>
      public string GetMaxDate(string kindId , DateTime endDate , string sumType = "D") {
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

         string res = db.ExecuteScalar(sql , CommandType.Text , parms);
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
      public string GetLastMonthLastTradeDate(string kindId , DateTime thisMonthDate , string sumType = "D" , int interval = 1) {
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
where rownum=1" , interval);

         string res = db.ExecuteScalar(sql , CommandType.Text , parms);
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
      public DataTable ListWeek(DateTime startDate , DateTime endDate , string sumType = "D" , string prodType = "F") {
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

         return db.GetDataTable(sql , parms);

      }

      /// <summary>
      /// AI2_YMD
      /// </summary>
      /// <param name="startDate"></param>
      /// <param name="endDate"></param>
      /// <returns>AI2_YMD</returns>
      public DataTable ListAI2Date(DateTime startDate , DateTime endDate , string sumType = "D" , string prodType = "F") {
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

         return db.GetDataTable(sql , parms);

      }

      /// <summary>
      /// d_ai2_ym
      /// </summary>
      /// <param name="as_kind_id">ex: TXF</param>
      /// <param name="as_ym">起始日期</param>
      /// <param name="as_last_ym">終止日期</param>
      /// <returns></returns>
      public DataTable ListAI2ym(string as_kind_id , string as_ym , string as_last_ym) {
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

         return db.GetDataTable(sql , parms);

      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="symd"></param>
      /// <param name="eymd"></param>
      /// <param name="paramKey"></param>
      /// <returns></returns>
      public DataTable ListAI2ByYmd(string symd , string eymd , string paramKey) {
         object[] parms = {
                ":as_symd", symd,
                ":as_eymd", eymd,
                ":as_param_key", paramKey
            };

         string sql = @"SELECT 
        AI2_KIND_ID2 AS AI2_PARAM_KEY,
         SUM(AI2_M_QNTY) AS AI2_M_QNTY,
         SUM(case when TRIM(AI2_YMD) = :as_eymd then AI2_OI else 0 end) AS AI2_OI
        FROM ci.AI2  
        WHERE AI2_SUM_TYPE = 'M'  AND  
            TRIM(AI2_YMD) >= :as_symd  AND
            TRIM(AI2_YMD) <= :as_eymd  AND
            AI2_PROD_TYPE IN ('F','O') AND
            AI2_SUM_SUBTYPE = '4' AND
            TRIM(AI2_PARAM_KEY) = :as_param_key
        GROUP BY AI2_KIND_ID2,AI2_PARAM_KEY";

         return db.GetDataTable(sql , parms);
      }

      /// <summary>
      /// 抓取前一天,return DateTime (ken,多增加下限時間,加快查詢效率)
      /// </summary>
      /// <param name="ai2_ymd">datetime</param>
      /// <param name="ai2_sum_type"></param>
      /// <param name="ai2_param_key"></param>
      /// <returns></returns>
      public DateTime GetLastDate(DateTime ai2_ymd , string ai2_sum_type = "D" , string ai2_param_key = "TXF%", string ai2_prod_subtype = "%") {
         object[] parms = {
                ":ai2_ymd", ai2_ymd,
                ":ai2_sum_type", ai2_sum_type,
                ":ai2_param_key", ai2_param_key,
                ":ai2_prod_subtype", ai2_prod_subtype
            };

         string sql = @"
select max(ai2_ymd) as idt_last_date
from ci.ai2
where ai2_sum_type like :ai2_sum_type
and ai2_param_key like :ai2_param_key
and ai2_prod_subtype like :ai2_prod_subtype
and ai2_ymd < to_char(:ai2_ymd,'yyyymmdd')
and ai2_ymd >= to_char(:ai2_ymd-32,'yyyymmdd')
";

         string temp = db.ExecuteScalar(sql , CommandType.Text , parms);
         DateTime result = DateTime.MinValue;
         DateTime.TryParseExact(temp , "yyyyMMdd" , null , System.Globalization.DateTimeStyles.AllowWhiteSpaces , out result);
         return result;
      }


      /// <summary>
      /// 全市總成交量
      /// </summary>
      /// <param name="ai2_ymd"></param>
      /// <param name="ai2_sum_type"></param>
      /// <param name="ai2_sum_subtype"></param>
      /// <returns></returns>
      public string GetTotalQnty(DateTime ai2_ymd , string ai2_sum_type = "D" , string ai2_sum_subtype = "1") {
         object[] parms = {
                ":ai2_ymd", ai2_ymd,
                ":ai2_sum_type", ai2_sum_type,
                ":ai2_sum_subtype", ai2_sum_subtype
            };

         string sql = @"
select sum(ai2_m_qnty) as ll_m_qnty
from ci.ai2
where ai2_ymd = to_char(:ai2_ymd,'yyyymmdd')
and ai2_sum_type = :ai2_sum_type
and ai2_sum_subtype = :ai2_sum_subtype
and ai2_prod_type in ('F','O')";

         return db.ExecuteScalar(sql , CommandType.Text , parms);
      }

      /// <summary>
      /// 取得(當月)日期期間最後交易日 to string [yyyyMMdd]
      /// </summary>
      /// <param name="AI2_SUM_TYPE"></param>
      /// <param name="AI2_PROD_TYPE"></param>
      /// <param name="AI2_PROD_SUBTYPE"></param>
      /// <param name="startDate"></param>
      /// <param name="endDate"></param>
      /// <returns>yyyyMMdd</returns>
      public string GetLastTradeDate(string AI2_SUM_TYPE , string AI2_PROD_TYPE , string AI2_PROD_SUBTYPE , DateTime startDate , DateTime endDate) {
         object[] parms =
         {
                ":AI2_SUM_TYPE", AI2_SUM_TYPE,
                ":AI2_PROD_TYPE", AI2_PROD_TYPE,
                ":AI2_PROD_SUBTYPE", AI2_PROD_SUBTYPE,
                ":startDate", startDate.ToString("yyyyMMdd"),
                ":endDate", endDate.ToString("yyyyMMdd")
            };

         //AI2_YMD format=yyyyMMdd
         string sql = @"
select max(ai2_ymd) as MaxDate
from ci.ai2
where ai2_sum_type = :AI2_SUM_TYPE
and ai2_prod_type = :AI2_PROD_TYPE 
and ai2_prod_subtype = :AI2_PROD_SUBTYPE
and ai2_ymd >= :startDate
and ai2_ymd <= :endDate 
";
         string res = db.ExecuteScalar(sql , CommandType.Text , parms);
         return res;
      }

      /// <summary>
      /// 判斷盤後交易項目是否完成(有join APDK)
      /// </summary>
      /// <param name="ls_ymd"></param>
      /// <param name="ls_grp"></param>
      /// <returns></returns>
      public int GetJobStatus(string ls_ymd, string ls_grp) {
         object[] parms =
         {
                ":ls_ymd", ls_ymd,
                ":ls_grp", ls_grp
            };


         string sql = @"select count(distinct AI2_PROD_TYPE)
		  into :li_cnt
		  from ci.AI2,ci.APDK
		 where AI2_YMD = :ls_ymd
			and AI2_SUM_TYPE = 'D'
			and AI2_PROD_TYPE in ('F','O')
			and AI2_KIND_ID = APDK_KIND_ID
			and APDK_MARKET_CLOSE like :ls_grp||'%'";

         string res = db.ExecuteScalar(sql, CommandType.Text, parms);

         int tmp = 0;
         int.TryParse(res, out tmp);

         return tmp;
      }


      /// <summary>
      /// 40040 前一交易日
      /// </summary>
      /// <param name="AI2_SUM_TYPE"></param>
      /// <param name="AI2_PROD_TYPE"></param>
      /// <param name="AI2_PROD_SUBTYPE"></param>
      /// <param name="startDate"></param>
      /// <param name="endDate"></param>
      /// <returns>string yyyy/MM/dd</returns>
      public string GetLastSumTypeDate(string AI2_SUM_TYPE, string AI2_SUM_SUBTYPE, string AI2_PROD_SUBTYPE, DateTime ld_date_last)
      {
         object[] parms =
         {
                ":AI2_SUM_TYPE", AI2_SUM_TYPE,
                ":AI2_SUM_SUBTYPE", AI2_SUM_SUBTYPE,
                ":AI2_PROD_SUBTYPE", AI2_PROD_SUBTYPE,
                ":ld_date_last", ld_date_last
            };

         //AI2_YMD format=yyyy/MM/dd
         string sql = @"
                  select TO_DATE(max(AI2_YMD),'yyyymmdd') as MaxDate
                  from ci.AI2
                  where AI2_SUM_TYPE = :AI2_SUM_TYPE
                  and AI2_SUM_SUBTYPE = :AI2_SUM_SUBTYPE 
                  and AI2_PROD_SUBTYPE = :AI2_PROD_SUBTYPE
                  and AI2_YMD < to_char(:ld_date_last,'yyyymmdd')
                  and AI2_YMD >= to_char(:ld_date_last-32,'yyyymmdd') 
";
         string res = db.ExecuteScalar(sql, CommandType.Text, parms);
         return res;
      }
   }
}
