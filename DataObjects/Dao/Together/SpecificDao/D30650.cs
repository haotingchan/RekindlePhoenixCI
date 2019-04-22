using OnePiece;
using System.Data;

/// <summary>
/// Winni 2019/01/24
/// </summary>
namespace DataObjects.Dao.Together {
   public class D30650 {
      private Db db;

      public D30650() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// Get CI.ABRK CI.AM10 data (d_30650)
      /// return abrk_no/abrk_abbr_name/abrk_no4/am10_ym/am10_qnty/am10_dt_qnty/am10_rate/cp_tot_qnty/cp_tot_dt_qnty/cp_rate 10 field
      /// </summary>
      /// <param name="as_symd">yyyyMM01 起始年月</param>
      /// <param name="as_eymd">yyyyMMdd 結束年月</param>
      /// <returns></returns>
      public DataTable GetData(string as_symd , string as_eymd) {
         object[] parms =
         {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

         string sql = @"
select 
    abrk_no,
    abrk_abbr_name,
    abrk_no4,
    am10_ym,
    am10_qnty,
    am10_dt_qnty,
    case when am10_qnty = 0 then 0 else round(am10_dt_qnty/am10_qnty,4)*100 end as am10_rate,
    sum(am10_qnty) over(partition by am10_ym ) as cp_tot_qnty ,
    sum(am10_dt_qnty) over(partition by am10_ym ) as cp_tot_dt_qnty,
    (case when (sum(am10_qnty) over(partition by am10_ym)) = 0 then 0 else round((sum(am10_dt_qnty) over(partition by am10_ym)) / (sum(am10_qnty) over(partition by am10_ym)),4)*100 end) as cp_rate
from 
    (select substr(abrk_no,1,4) as abrk_no4,substr(abrk_no,1,4)||nvl(max(case when substr(abrk_no,5,3) = '000' then '000' else null end),'999') as abrk_no7
    from ci.abrk 
    group by substr(abrk_no,1,4)),
    ci.abrk,
    (select substr(am10_ymd,1,6) as am10_ym,
    substr(am10_brk_no,1,4) as am10_brk_no4,
    sum(am10_qnty) as am10_qnty,
    sum(am10_dt_qnty) as am10_dt_qnty
      from ci.am10
    where am10_ymd >= :as_symd
    and am10_ymd <= :as_eymd
    and substr(am10_brk_no,5,3) <> '999'
     group by substr(am10_ymd,1,6),substr(am10_brk_no,1,4)) m
where abrk_no4 = am10_brk_no4
and abrk_no7 = abrk_no
order by am10_ym , abrk_no
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// Get CI.ABRK CI.AM10 data (d_30650_brk)
      /// return ABRK_NO/ABRK_ABBR_NAME/ABRK_NO4/AM10_QNTY/AM10_DT_QNTY/AM10_RATE 6 field
      /// </summary>
      /// <param name="as_symd">起始年月</param>
      /// <param name="as_eymd">結束年月</param>
      /// <returns></returns>
      public DataTable GetTmpData(string as_symd , string as_eymd) {
         object[] parms =
         {
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };

         string sql = @"
select 
    nvl(abrk_no,'null') as abrk_no,
    nvl(abrk_abbr_name,'null') as abrk_abbr_name,
    nvl(abrk_no4,'null') as abrk_no4,
    nvl(am10_qnty,0) as am10_qnty,
    nvl(am10_dt_qnty,0) as am10_dt_qnty,
    case when am10_qnty = 0 then 0 else round(am10_dt_qnty/am10_qnty,4)*100 end as am10_rate,             
    sum(am10_qnty) as cp_tot_qnty,
    sum(am10_dt_qnty) as cp_tot_dt_qnty,
   (case when sum(am10_qnty) = 0 then 0 else round((sum(am10_dt_qnty))/( sum(am10_qnty)),4)*100 end) as cp_rate
from 
    (select substr(abrk_no,1,4) as abrk_no4,
            substr(abrk_no,1,4)||nvl(max(case when substr(abrk_no,5,3) = '000' then '000' else null end),'999') as abrk_no7
    from ci.abrk 
    group by substr(abrk_no,1,4)),
    ci.abrk,
    (select substr(am10_brk_no,1,4) as am10_brk_no4,
            sum(am10_qnty) as am10_qnty,sum(am10_dt_qnty) as am10_dt_qnty
    from ci.am10
    where am10_ymd >= :as_symd
    and am10_ymd <= :as_eymd
    and substr(am10_brk_no,5,3) <> '999'
    group by substr(am10_brk_no,1,4)) m
where abrk_no4 = am10_brk_no4
and abrk_no7 = abrk_no
group by  grouping sets( (),(abrk_no,
    abrk_abbr_name,
    abrk_no4,
    am10_qnty,
    am10_dt_qnty))
order by nvl(am10_rate,0) desc,abrk_no asc
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
