using OnePiece;
using System;
using System.Data;

/// <summary>
/// ken 2018/12/27
/// </summary>
namespace DataObjects.Dao.Together
{
   /// <summary>
   /// AI3券商資訊?
   /// </summary>
   public class AI3
   {
      private Db db;

      public AI3()
      {
         db = GlobalDaoSetting.DB;
      }



      /// <summary>
      /// Get Max(AI3_DATE) to string [yyyyMMdd]
      /// </summary>
      /// <param name="kindId"></param>
      /// <param name="endDate"></param>
      /// <returns>yyyyMMdd</returns>
      public string GetMaxDate(string kindId, DateTime endDate)
      {
         object[] parms =
         {
                ":as_kind_id", kindId,
                ":ldt_edate", endDate.ToString("yyyyMMdd")
            };

         //AI3_YMD format=yyyyMMdd
         string sql = @"
select to_char(max(AI3_DATE),'yyyymmdd') as maxdate
from ci.AI3
where trim(AI3_KIND_ID) LIKE :as_kind_id
and AI3_DATE < to_date(:ldt_edate,'yyyymmdd')
";

         string res = db.ExecuteScalar(sql, CommandType.Text, parms);
         return res;
      }




      /// <summary>
      /// 上個月的倒數n天交易日 to string [yyyyMMdd]
      /// </summary>
      /// <param name="kindId"></param>
      /// <param name="thisMonthDate"></param>
      /// <param name="interval">unit:day</param>
      /// <returns>yyyyMMdd</returns>
      public string GetLastMonthLastTradeDate(string kindId, DateTime thisMonthDate, int interval = 1)
      {
         object[] parms =
         {
                ":as_kind_id", kindId,
                ":thisMonthDate", thisMonthDate.ToString("yyyyMMdd")
            };

         //AI3_YMD format=yyyyMMdd
         string sql = string.Format(@"
select to_char(ai3_date,'yyyymmdd') from(
    select ai3_date from (
        select ai3_date
        from ci.AI3
        where trim(AI3_KIND_ID) = trim(:as_kind_id)
        and AI3_DATE < to_date(:thisMonthDate,'yyyymmdd')
        --ken,原PB一直往前抓交易日,程式碼與函數定義的[上個月的倒數n天交易日]有差
        --and AI3_DATE >= to_date(:thisMonthDate,'yyyymmdd')-32
        group by ai3_date
        order by ai3_date desc
    ) a
    where rownum <= {0}
    order by ai3_date
) b
where rownum=1", interval);

         string res = db.ExecuteScalar(sql, CommandType.Text, parms);
         return res;
      }

      /// <summary>
      /// d_ai3_1
      /// </summary>
      /// <param name="as_kind_id">ex:MSF</param>
      /// <param name="as_sdate">起始DateTime</param>
      /// <param name="as_edate">終止DateTime</param>
      /// <returns></returns>
      public DataTable ListAI3(string as_kind_id, DateTime as_sdate, DateTime as_edate)
      {
         object[] parms =
         {
            "as_kind_id",as_kind_id,
            ":as_sdate", as_sdate,
            ":as_edate", as_edate
            };

         string sql = @"SELECT AI3_DATE,   
                                 AI3_CLOSE_PRICE,   
                                 AI3_M_QNTY,   
                                 AI3_OI,   
                                 AI3_INDEX,   
                                 AI3_AMOUNT  ,   
                                 AI3_LAST_CLOSE_PRICE ,
                                 TFXMMD_PX
                            FROM ci.AI3  ,
                                (select TFXMMD_YMD,TFXMMD_PX
                                   from ci.TFXMMD
                                  where TFXMMD_YMD >= TO_CHAR(:as_sdate,'YYYYMMDD')
                                    and TFXMMD_YMD <= TO_CHAR(:as_edate,'YYYYMMDD')
                                    and TFXMMD_MTH_SEQ_NO = 1
                                    and TFXMMD_KIND_ID = :as_kind_id  )
                           WHERE AI3_KIND_ID = :as_kind_id  
                             AND AI3_DATE >= :as_sdate   
                             AND AI3_DATE <= :as_edate     
                             AND AI3_DATE = TO_DATE(TFXMMD_YMD(+),'YYYYMMDD')
                           ORDER BY AI3_DATE";

         DataTable dt = db.GetDataTable(sql, parms);
         return dt;
      }

   }
}
