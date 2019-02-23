using OnePiece;
using System;
using System.Data;

/// <summary>
/// john,20190128
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   /// <summary>
   /// 小型臺股期貨期貨商交易量表
   /// </summary>
   public class D70050
   {
      //test is_sum_type=D,is_kind_id2=%,is_param_key=TXO,is_prod_type=O
      private Db db;

      public D70050()
      {
         db = GlobalDaoSetting.DB;
      }
      /// <summary>
      /// return AM0_BRK_NO4/AM0_BRK_TYPE/AM0_YMD/AM0_PARAM_KEY/qnty
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <param name="as_kind_id2"></param>
      /// <param name="as_param_key"></param>
      /// <returns></returns>
      public DataTable ListAll(
         string as_symd, string as_eymd, string as_sum_type, string as_prod_type, string as_kind_id2,string as_param_key)
      {

         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type,
                ":as_kind_id2",as_kind_id2,
                ":as_param_key",as_param_key
            };
         string sql = @"SELECT CI.AM0.AM0_BRK_NO4,   
                                 CI.AM0.AM0_BRK_TYPE,  
                                 CI.AM0.AM0_YMD, 
                                 CI.AM0.AM0_KIND_ID2 AS AM0_PARAM_KEY,
                                 sum(CI.AM0.AM0_M_QNTY) as qnty
                            FROM CI.AM0  
                           WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                                 ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                                 ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                                 ( CI.AM0.AM0_PROD_TYPE = :as_prod_type ) and
                                 ( CI.AM0.AM0_PARAM_KEY = :as_param_key)  and
                                 ( CI.AM0.AM0_KIND_ID2 LIKE :as_kind_id2)
                        GROUP BY CI.AM0.AM0_BRK_NO4,   
                                 CI.AM0.AM0_BRK_TYPE , 
                                 CI.AM0.AM0_YMD  ,  
                                 CI.AM0.AM0_KIND_ID2
                        union
                          SELECT CI.AM0.AM0_BRK_NO4,   
                                 CI.AM0.AM0_BRK_TYPE,  
                                 '99999999', 
                                 CI.AM0.AM0_KIND_ID2 AS AM0_PARAM_KEY,
                                 sum(CI.AM0.AM0_M_QNTY) as qnty
                            FROM CI.AM0  
                           WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                                 ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                                 ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                                 ( CI.AM0.AM0_PROD_TYPE = :as_prod_type ) and
                                 ( CI.AM0.AM0_PARAM_KEY = :as_param_key) and
                                 ( CI.AM0.AM0_KIND_ID2 LIKE :as_kind_id2)
                        GROUP BY CI.AM0.AM0_BRK_NO4,   
                                 CI.AM0.AM0_BRK_TYPE ,
                                 CI.AM0.AM0_KIND_ID2
                        ORDER BY am0_brk_no4,am0_brk_type,am0_ymd,am0_param_key
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// return am0_brk_no4,am0_brk_type,qnty,cp_rate
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <param name="as_kind_id2"></param>
      /// <param name="as_param_key"></param>
      /// <returns></returns>
      public DataTable List70050brk(
         string as_symd, string as_eymd, string as_sum_type, string as_prod_type, string as_kind_id2, string as_param_key)
      {
         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type,
                ":as_kind_id2",as_kind_id2,
                ":as_param_key",as_param_key
            };
         string sql = @"SELECT CI.AM0.AM0_BRK_NO4,   
                              CI.AM0.AM0_BRK_TYPE,
                              sum(CI.AM0.AM0_M_QNTY) as qnty
                         FROM CI.AM0  
                        WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                              ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                              ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                              ( CI.AM0.AM0_PROD_TYPE = :as_prod_type ) and 
                              ( CI.AM0.AM0_PARAM_KEY = :as_param_key)  and
                              ( CI.AM0.AM0_KIND_ID2 LIKE :as_kind_id2) 
                     GROUP BY CI.AM0.AM0_BRK_NO4,   
                              CI.AM0.AM0_BRK_TYPE 
                     ORDER BY qnty Desc,am0_brk_no4,am0_brk_type
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         if (dtResult.Rows.Count <= 0) return dtResult;
         dtResult.Columns.Add("cp_rate", typeof(decimal));
         decimal cp_sum_qnty = Convert.ToInt32(dtResult.Compute("sum(qnty)", ""));

         foreach (DataRow dr in dtResult.Rows) {
            dr["cp_rate"] = Math.Round(Convert.ToDecimal(dr["qnty"].ToString()) / cp_sum_qnty * 100, 2);
         }
         dtResult.AcceptChanges();
         return dtResult;
      }

      /// <summary>
      /// return AM0_BRK_NO4/AM0_BRK_TYPE/AM0_YMD/AM0_PARAM_KEY/qnty/cp_sum_qnty
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <param name="as_param_key"></param>
      /// <param name="as_kind_id2"></param>
      /// <returns></returns>
      public DataTable d_70050(string as_symd,
                                  string as_eymd,
                                  string as_sum_type,
                                  string as_prod_type,
                                  string as_param_key,
                                  string as_kind_id2)
      {

         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type,
                ":as_param_key",as_param_key,
                ":as_kind_id2",as_kind_id2
            };

         string sql = @"
select a.*,b.cp_sum_qnty
from(
    SELECT CI.AM0.AM0_BRK_NO4,   
             CI.AM0.AM0_BRK_TYPE,  
             CI.AM0.AM0_YMD, 
             CI.AM0.AM0_KIND_ID2 AS AM0_PARAM_KEY,
             sum(CI.AM0.AM0_M_QNTY) as qnty
        FROM CI.AM0  
       WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
             ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
             ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
             ( CI.AM0.AM0_PROD_TYPE = :as_prod_type ) and
             ( CI.AM0.AM0_PARAM_KEY = :as_param_key)  and
             ( CI.AM0.AM0_KIND_ID2 LIKE :as_kind_id2)
    GROUP BY CI.AM0.AM0_BRK_NO4,   
             CI.AM0.AM0_BRK_TYPE , 
             CI.AM0.AM0_YMD  ,  
             CI.AM0.AM0_KIND_ID2
    union
      SELECT CI.AM0.AM0_BRK_NO4,   
             CI.AM0.AM0_BRK_TYPE,  
             '99999999', 
             CI.AM0.AM0_KIND_ID2 AS AM0_PARAM_KEY,
             sum(CI.AM0.AM0_M_QNTY) as qnty
        FROM CI.AM0  
       WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
             ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
             ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
             ( CI.AM0.AM0_PROD_TYPE = :as_prod_type ) and
             ( CI.AM0.AM0_PARAM_KEY = :as_param_key) and
             ( CI.AM0.AM0_KIND_ID2 LIKE :as_kind_id2)
    GROUP BY CI.AM0.AM0_BRK_NO4,   
             CI.AM0.AM0_BRK_TYPE ,
             CI.AM0.AM0_KIND_ID2
) a,
(
    select am0_brk_no4 ,am0_brk_type,am0_ymd,sum(qnty) as cp_sum_qnty from (
        SELECT CI.AM0.AM0_BRK_NO4,   
                 CI.AM0.AM0_BRK_TYPE,  
                 CI.AM0.AM0_YMD, 
                 CI.AM0.AM0_KIND_ID2 AS AM0_PARAM_KEY,
                 sum(CI.AM0.AM0_M_QNTY) as qnty
            FROM CI.AM0  
           WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                 ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                 ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                 ( CI.AM0.AM0_PROD_TYPE = :as_prod_type ) and
                 ( CI.AM0.AM0_PARAM_KEY = :as_param_key)  and
                 ( CI.AM0.AM0_KIND_ID2 LIKE :as_kind_id2)
        GROUP BY CI.AM0.AM0_BRK_NO4,   
                 CI.AM0.AM0_BRK_TYPE , 
                 CI.AM0.AM0_YMD  ,  
                 CI.AM0.AM0_KIND_ID2
        union
          SELECT CI.AM0.AM0_BRK_NO4,   
                 CI.AM0.AM0_BRK_TYPE,  
                 '99999999', 
                 CI.AM0.AM0_KIND_ID2 AS AM0_PARAM_KEY,
                 sum(CI.AM0.AM0_M_QNTY) as qnty
            FROM CI.AM0  
           WHERE ( CI.AM0.AM0_YMD >= :as_symd ) AND  
                 ( CI.AM0.AM0_YMD <= :as_eymd ) AND  
                 ( CI.AM0.AM0_SUM_TYPE  = :as_sum_type ) AND  
                 ( CI.AM0.AM0_PROD_TYPE = :as_prod_type ) and
                 ( CI.AM0.AM0_PARAM_KEY = :as_param_key) and
                 ( CI.AM0.AM0_KIND_ID2 LIKE :as_kind_id2)
        GROUP BY CI.AM0.AM0_BRK_NO4,   
                 CI.AM0.AM0_BRK_TYPE ,
                 CI.AM0.AM0_KIND_ID2
      ) group by am0_brk_no4 ,am0_brk_type,am0_ymd             
) b
where a.am0_brk_no4=b.am0_brk_no4
and a.am0_brk_type=b.am0_brk_type
and a.am0_ymd=b.am0_ymd
";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// return am0_brk_no4,am0_brk_type,qnty,cp_rate
      /// </summary>
      /// <param name="as_symd"></param>
      /// <param name="as_eymd"></param>
      /// <param name="as_sum_type"></param>
      /// <param name="as_prod_type"></param>
      /// <param name="as_kind_id2"></param>
      /// <param name="as_param_key"></param>
      /// <returns></returns>
      public DataTable d_70050_brk(string as_symd,
                                  string as_eymd,
                                  string as_sum_type,
                                  string as_prod_type,
                                  string as_param_key,
                                  string as_kind_id2)
      {

         object[] parms = {
                ":as_symd",as_symd,
                ":as_eymd",as_eymd,
                ":as_sum_type",as_sum_type,
                ":as_prod_type",as_prod_type,
                ":as_param_key",as_param_key,
                ":as_kind_id2",as_kind_id2
            };

         string sql = @"
                        select am0_brk_no4,am0_brk_type,qnty,round(qnty*100/total,2) as cp_rate
                        from (
                            select am0_brk_no4,   
                                am0_brk_type,
                                sum(am0_m_qnty) as qnty,
                                b.total
                            from ci.am0,
                            (
                                select sum(a.qnty) as total 
                                from (
                                    select am0_brk_no4,   
                                        am0_brk_type,
                                        sum(am0_m_qnty) as qnty
                                    from ci.am0
                                    where am0_ymd >= :as_symd  
                                    and am0_ymd <= :as_eymd  
                                    and am0_sum_type  = :as_sum_type  
                                    and am0_prod_type = :as_prod_type 
                                    and am0_param_key = :as_param_key
                                    and am0_kind_id2 like :as_kind_id2
                                    group by am0_brk_no4,am0_brk_type 
                                    order by qnty desc, am0_brk_no4 , am0_brk_type
                                ) a 
                            ) b
                            where am0_ymd >= :as_symd  
                            and am0_ymd <= :as_eymd  
                            and am0_sum_type  = :as_sum_type  
                            and am0_prod_type = :as_prod_type 
                            and am0_param_key = :as_param_key
                            and am0_kind_id2 like :as_kind_id2
                            group by am0_brk_no4,am0_brk_type,b.total
                            order by qnty desc, am0_brk_no4 , am0_brk_type
)";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

   }
}
