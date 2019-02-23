using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D50060 {
      private Db db;

      public D50060() {
         db = GlobalDaoSetting.DB;
      }


      public DataTable ListData(string as_brk_no ,
                                  string as_acc_no ,
                                  string as_time1 ,
                                  string as_time2 ,
                                  string as_kind_id ,
                                  string as_settle_date ,
                                  string as_pc_code ,
                                  Decimal ad_strike_price1 ,
                                  Decimal ad_strike_price2 ,
                                  string TableName,
                                  string isPrint = "N") {
         object[] parms = {
               ":as_brk_no",as_brk_no,
               ":as_acc_no",as_acc_no,
               ":as_time1",as_time1,
               ":as_time2",as_time2,
               ":as_kind_id",as_kind_id,
               ":as_settle_date",as_settle_date,
               ":as_pc_code",as_pc_code,
               ":ad_strike_price1",ad_strike_price1,
               ":ad_strike_price2",ad_strike_price2
            };

         //ken,強制只能是Ammd/AmmdAH
         if (TableName != "ammd" && TableName != "ammdAH") {
            throw new Exception("D50060.ListData.TableName有錯");
         }

         string sql = @"select ammd_date,
                  ammd_brk_no,ammd_acc_no,
                  (SELECT NVL(ABRK_NAME,'') FROM ci.ABRK
                     WHERE ABRK_NO = AMMD_BRK_NO ) as BRK_ABBR_NAME,  
                  AMMD_PROD_TYPE,
                  ammd_kind_id,
                  ammd_settle_date, 
                  ammd_pc_code,
                  ammd_strike_price,
                  ammd_buy_price,
                  ammd_sell_price,
                  ammd_b_qnty,
                  ammd_s_qnty,
                  ammd_w_time
                  from ci." + TableName +
                  @" where ammd_data_type = 'Q'
                  and ammd_date  between to_date(substr(:as_time1,1,10),'yyyy/MM/dd') 
                                    and  to_date(substr(:as_time2,1,10),'yyyy/MM/dd')
                  and ammd_brk_type = '9'
                  and ammd_brk_no like :as_brk_no
                  and ammd_acc_no like :as_acc_no
                  and ammd_cp_time_flag = 'Y'
                  and ammd_w_time between to_date(:as_time1,'yyyy/MM/dd hh24:mi:ss')
                                     and  to_date(:as_time2,'yyyy/MM/dd hh24:mi:ss')
                  and rtrim(ammd_kind_id) like :as_kind_id
                  and ammd_settle_date like :as_settle_date
                  and ammd_pc_code like :as_pc_code
                  and ammd_strike_price between :ad_strike_price1  and :ad_strike_price2
                  order by ammd_brk_no , ammd_acc_no , ammd_prod_type , 
                  ammd_kind_id , ammd_settle_date , ammd_pc_code , ammd_strike_price , ammd_w_time";


         DataTable dtResult = db.GetDataTable(sql , parms);

         if (isPrint == "Y") {
            string[] title = new string[]{ "日期","造市者代號","帳號","造市者名稱","F期貨/O選擇權","商品","契約月份",
                                           "買賣權","履約價格","買價價格","賣價價格","委買數量","委賣數量","委託時間"
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