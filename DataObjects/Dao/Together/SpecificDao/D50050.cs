using OnePiece;
using System.Data;

/// <summary>
/// ken,2019/1/2
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 造市者每日價平上下5檔各序列報價
   /// </summary>
   public class D50050 : DataGate {

      /// <summary>
      /// List AMMD or AMMDAH data
      /// </summary>
      /// <param name="as_brk_no"></param>
      /// <param name="as_acc_no"></param>
      /// <param name="as_time1"></param>
      /// <param name="as_time2"></param>
      /// <param name="as_kind_id"></param>
      /// <param name="as_settle_date"></param>
      /// <param name="as_pc_code"></param>
      /// <param name="as_p_seq_no1"></param>
      /// <param name="as_p_seq_no2"></param>
      /// <param name="dbName">ammd or ammdAH</param>
      /// <returns></returns>
      public DataTable ListAll(string as_brk_no ,
                                  string as_acc_no ,
                                  string as_time1 ,
                                  string as_time2 ,
                                  string as_kind_id ,
                                  string as_settle_date ,
                                  string as_pc_code ,
                                  int as_p_seq_no1 ,
                                  int as_p_seq_no2 ,
                                  string dbName = "ammd" ,
                                  string isPrint = "N") {

         object[] parms = {
                ":as_brk_no", as_brk_no,
                ":as_acc_no", as_acc_no,
                ":as_time1", as_time1,
                ":as_time2", as_time2,
                ":as_kind_id", as_kind_id,
                ":as_settle_date", as_settle_date,
                ":as_pc_code", as_pc_code,
                ":as_p_seq_no1", as_p_seq_no1,
                ":as_p_seq_no2", as_p_seq_no2
            };

         //ken,簡易防止sql injection(基本上這個值不應該從UI傳進來)
         //dbName = dbName.Substring(0 , 10).Replace("'" , "").Replace("--" , "").Replace(";" , "");

         string sql = string.Format(@"
select ammd_date,
       ammd_brk_no, 
       ammd_acc_no,
       (select nvl(abrk_name,'') from ci.abrk
          where abrk_no = ammd_brk_no ) as brk_abbr_name,  
       ammd_prod_type,
       ammd_kind_id,
       ammd_settle_date,
       ammd_pc_code,
       ammd_p_seq_no,
       ammd_buy_price,
       ammd_sell_price,
       ammd_b_qnty,
       ammd_s_qnty,
       ammd_w_time
from ci.{0}
where ammd_data_type = 'Q'
and ammd_cp_time_flag = 'Y'
and ammd_brk_no like :as_brk_no
and ammd_acc_no like :as_acc_no
and ammd_date  between to_date(substr(:as_time1,1,10),'yyyy/mm/dd') and  to_date(substr(:as_time2,1,10),'yyyy/mm/dd')
and ammd_w_time between to_date(:as_time1,'yyyy/mm/dd hh24:mi:ss')
                   and  to_date(:as_time2,'yyyy/mm/dd hh24:mi:ss')
and rtrim(ammd_kind_id) like :as_kind_id
and ammd_settle_date like :as_settle_date
and ammd_pc_code like :as_pc_code
and ammd_p_seq_no between :as_p_seq_no1  and :as_p_seq_no2
order by ammd_brk_no , ammd_acc_no , ammd_prod_type , ammd_kind_id , ammd_settle_date ,
ammd_pc_code , ammd_p_seq_no , ammd_w_time" , dbName);

         DataTable dtResult = db.GetDataTable(sql , parms);

         if (isPrint == "Y") {
            string[] title = new string[]{ "日期","造市者代號","帳號","造市者名稱","F期貨/O選擇權","商品","契約月份",
                                           "買賣權","價平上下檔數","買價價格","賣價價格","委買數量","委賣數量","委託時間"
                                                 };
            int k = 0;
            foreach (DataColumn col in dtResult.Columns) {
               col.Caption = title[k++];
            }
         }

         return dtResult;
      }

   }
}
