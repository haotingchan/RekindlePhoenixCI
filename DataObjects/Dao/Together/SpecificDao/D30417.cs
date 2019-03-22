using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/3/19
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 股票期貨每週交概況統計表
   /// </summary>
   public class D30417 {

      private Db db;

      public D30417() {

         db = GlobalDaoSetting.DB;

      }

      /// <summary>
      /// get CI.AI2/CI.AM9/CI.AM10 data (d30417)
      /// return AI2_YMD/AI2_M_QNTY/AI2_OI/AI2_DAY_COUNT/AM9_ACC_CNT/AM10_CNT 6 feilds
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <returns></returns>
      public DataTable ListData(string startDate , string endDate) {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate
            };

         string sql = @"
select AI2_YMD,AI2_M_QNTY,AI2_OI,AI2_DAY_COUNT,
       AM9_ACC_CNT,AM10_CNT 
  from     
     (SELECT AI2_YMD,sum(AI2_M_QNTY) as AI2_M_QNTY,sum(AI2_OI) as AI2_OI,MAX(AI2_DAY_COUNT) AS AI2_DAY_COUNT 
      FROM CI.AI2
     WHERE AI2_YMD >= :startDate
       AND AI2_YMD <= :endDate
       AND AI2_SUM_TYPE = 'D'
       AND AI2_SUM_SUBTYPE = '2'
       AND AI2_PROD_TYPE = 'F'
       AND AI2_PROD_SUBTYPE = 'S'
     GROUP BY AI2_YMD) I,
     (select AM9_YMD AS AM9_YMD ,sum(AM9_ACC_CNT) as AM9_ACC_CNT
        from ci.AM9
       where AM9_YMD >= :startDate
         and AM9_YMD <= :endDate
         and AM9_PROD_TYPE = 'F'
         and AM9_PROD_SUBTYPE = 'S'
         and AM9_PARAM_KEY = '999'
      group by AM9_YMD ) M,
     (select AM10_YMD ,sum(AM10_CNT) as AM10_CNT
        from ci.AM10
       where AM10_YMD >= :startDate||'01'
         and AM10_YMD <= :endDate||'31'
         and AM10_PROD_TYPE = 'F'
         and AM10_PROD_SUBTYPE = 'S'
      group by AM10_YMD) C
 where AI2_YMD = AM9_YMD(+)
   AND AI2_YMD = AM10_YMD(+)
order by ai2_ymd 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get CI.AI2 data return AI2_YMD/AI2_KIND_ID/AI2_M_QNTY/AI2_OI/AI2_DAY_COUNT 5 feilds (d30418)
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <returns></returns>
      public DataTable ListData2(string startDate , string endDate) {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate
            };

         string sql = @"
SELECT AI2_YMD,substr(AI2_KIND_ID,1,2) AS AI2_KIND_ID,
       sum(AI2_M_QNTY) as AI2_M_QNTY,sum(AI2_OI) as AI2_OI,MAX(AI2_DAY_COUNT) AS AI2_DAY_COUNT 
      FROM CI.AI2
     WHERE AI2_YMD >= :startDate
       AND AI2_YMD <= :endDate
       AND AI2_SUM_TYPE = 'D'
       AND AI2_SUM_SUBTYPE = '4'
       AND AI2_PROD_TYPE = 'F'
       AND AI2_PROD_SUBTYPE = 'S'
     GROUP BY AI2_YMD,substr(AI2_KIND_ID,1,2)
order by ai2_kind_id , ai2_ymd 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }   
   }
}
