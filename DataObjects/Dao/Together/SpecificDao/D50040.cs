using OnePiece;
using System;
using System.Data;

/// <summary>
/// ken,2019/1/2
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 造市者各造市商品報價價差報表
   /// </summary>
   public class D50040 {
      private Db db;

      public D50040() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// List AMMD or AMMDAH data + ABRK
      /// </summary>
      /// <param name="as_date"></param>
      /// <param name="as_edate"></param>
      /// <param name="as_sort_type"></param>
      /// <param name="as_value"></param>
      /// <param name="as_fcm_no"></param>
      /// <param name="as_kind_id2"></param>
      /// <param name="dbName"></param>
      /// <param name="isPrint">如果為Y,則title都更換為中文</param>
      /// <returns></returns>
      public DataTable ListAll(DateTime as_date ,
                                  DateTime as_edate ,
                                  string as_sort_type ,
                                  int as_value ,
                                  string as_fcm_no ,
                                  string as_kind_id2 ,
                                  string dbName = "AMM1" ,
                                  string isPrint = "N"
                                  ) {
         object[] parms = {
               ":as_date",as_date,
               ":as_edate",as_edate,
               ":as_sort_type",as_sort_type,
               ":as_value",as_value,
               ":as_fcm_no",as_fcm_no,
               ":as_kind_id2",as_kind_id2
            };

         //ken,強制只能是Amm1/Amm1AH
         if (dbName != "AMM1" && dbName != "AMM1AH") {
            throw new Exception("D50040.ListData.TableName有錯");
         }

         //多做一層select為了執行原PB的兩個計算域(computer)
         //cp_key1 ---> if( sort_type ='F' ,amm1_fcm_no+ " , "+amm1_acc_no , amm1_prod_type+amm1_kind_id2 )
         //cp_key2 ---> if( sort_type ='F'  ,  amm1_prod_type+amm1_kind_id2,amm1_fcm_no+ " , "+amm1_acc_no )
         string sort = "";
         if (as_sort_type == "F") {
            sort = @"order by amm1_date, amm1_fcm_no || ',' || amm1_acc_no, amm1_prod_type || amm1_kind_id2";
         } else {
            sort = @"order by amm1_date, amm1_prod_type||amm1_kind_id2, amm1_fcm_no||','||amm1_acc_no";
         }


         string sql = string.Format(@"
select to_char(AMM1_DATE,'yyyy/MM/dd') as AMM1_DATE,
    AMM1_FCM_NO,
    AMM1_ACC_NO,
    ABRK_ABBR_NAME,
    AMM1_PROD_TYPE,
    AMM1_KIND_ID2,
    O_OUT5,O_OUT4,O_OUT3,O_OUT2,O_OUT1,
    O_0,
    O_IN1,O_IN2,O_IN3,O_IN4,O_IN5,
    F_0,
   :as_sort_type as SORT_TYPE
from (
    select AMM1_DATE,AMM1_FCM_NO,AMM1_ACC_NO,AMM1_PROD_TYPE,AMM1_KIND_ID2,
        sum(case AMM1_P_SEQ_NO when 5 then (case to_number(:as_value) when 1 then AMM1_MAX_SPREAD when 2 then AMM1_MIN_SPREAD else AMM1_AVG_SPREAD end) else NULL end) as o_out5,
        sum(case AMM1_P_SEQ_NO when 4 then (case to_number(:as_value) when 1 then AMM1_MAX_SPREAD when 2 then AMM1_MIN_SPREAD else AMM1_AVG_SPREAD end) else NULL end) as o_out4,
        sum(case AMM1_P_SEQ_NO when 3 then (case to_number(:as_value) when 1 then AMM1_MAX_SPREAD when 2 then AMM1_MIN_SPREAD else AMM1_AVG_SPREAD end) else NULL end) as o_out3,
        sum(case AMM1_P_SEQ_NO when 2 then (case to_number(:as_value) when 1 then AMM1_MAX_SPREAD when 2 then AMM1_MIN_SPREAD else AMM1_AVG_SPREAD end) else NULL end) as o_out2,
        sum(case AMM1_P_SEQ_NO when 1 then (case to_number(:as_value) when 1 then AMM1_MAX_SPREAD when 2 then AMM1_MIN_SPREAD else AMM1_AVG_SPREAD end) else NULL end) as o_out1,
        sum(case when AMM1_P_SEQ_NO = 0 AND AMM1_PROD_TYPE = 'O' then (case to_number(:as_value) when 1 then AMM1_MAX_SPREAD when 2 then AMM1_MIN_SPREAD else AMM1_AVG_SPREAD end) else NULL end) as o_0,
        sum(case AMM1_P_SEQ_NO when -1 then (case to_number(:as_value) when 1 then AMM1_MAX_SPREAD when 2 then AMM1_MIN_SPREAD else AMM1_AVG_SPREAD end) else NULL end) as o_in1,
        sum(case AMM1_P_SEQ_NO when -2 then (case to_number(:as_value) when 1 then AMM1_MAX_SPREAD when 2 then AMM1_MIN_SPREAD else AMM1_AVG_SPREAD end) else NULL end) as o_in2,
        sum(case AMM1_P_SEQ_NO when -3 then (case to_number(:as_value) when 1 then AMM1_MAX_SPREAD when 2 then AMM1_MIN_SPREAD else AMM1_AVG_SPREAD end) else NULL end) as o_in3,
        sum(case AMM1_P_SEQ_NO when -4 then (case to_number(:as_value) when 1 then AMM1_MAX_SPREAD when 2 then AMM1_MIN_SPREAD else AMM1_AVG_SPREAD end) else NULL end) as o_in4,
        sum(case AMM1_P_SEQ_NO when -5 then (case to_number(:as_value) when 1 then AMM1_MAX_SPREAD when 2 then AMM1_MIN_SPREAD else AMM1_AVG_SPREAD end) else NULL end) as o_in5,
        sum(case when AMM1_PROD_TYPE = 'F' then (case to_number(:as_value) when 1 then AMM1_MAX_SPREAD when 2 then AMM1_MIN_SPREAD else AMM1_AVG_SPREAD end) else NULL end) as f_0
    from ci.{0}
    where AMM1_DATE >= :as_date
    and AMM1_DATE <= :as_edate
    group by AMM1_DATE,AMM1_FCM_NO,AMM1_ACC_NO,AMM1_PROD_TYPE,AMM1_KIND_ID2
) A,
ci.ABRK
where AMM1_FCM_NO = ABRK_NO(+)
and AMM1_FCM_NO like :as_fcm_no
and AMM1_KIND_ID2 like :as_kind_id2
{1}" , dbName , sort);

         DataTable dtResult = db.GetDataTable(sql , parms);

         if (isPrint == "Y") {
            string[] title = new string[]{ "日期","造市者代號","造市者帳號","造市者名稱","期貨F/選擇權O","商品",
                                                "價外第5檔","價外第4檔","價外第3檔","價外第2檔","價外第1檔","價平",
                                                "價內第1檔","價內第2檔","價內第3檔","價內第4檔","價內第5檔","期貨",
                                                "條件:造市者F/商品P" };
            int k = 0;
            foreach (DataColumn col in dtResult.Columns) {
               col.Caption = title[k++];
            }
         }

         return dtResult;
      }


   }
}