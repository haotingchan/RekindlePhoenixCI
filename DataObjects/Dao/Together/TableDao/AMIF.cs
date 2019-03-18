using BusinessObjects;
using Common;
using System;
using System.Data;

/// <summary>
/// Lukas, 2019/1/30
/// ken,2019/3/12調整
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   /// <summary>
   /// AMIF
   /// </summary>
   public class AMIF : DataGate {

      /// <summary>
      /// for W20110, return yyyyMM
      /// </summary>
      /// <param name="ad_date"></param>
      /// <param name="amif_kind_id"></param>
      /// <param name="amif_data_source"></param>
      /// <param name="amif_mth_seq_no"></param>
      /// <returns></returns>
      public string MinSettleDate(DateTime amif_date , string amif_kind_id = "TXF" , string amif_data_source = "T" , int amif_mth_seq_no = 1) {
         object[] parms =
         {
                ":amif_date", amif_date,
                ":amif_kind_id", amif_kind_id,
                ":amif_data_source", amif_data_source,
                ":amif_mth_seq_no", amif_mth_seq_no
            };

         string sql = @"
select min(amif_settle_date) as ls_settle_date
from ci.amif
where amif_date = :ad_date
  and amif_kind_id = :amif_kind_id
  and amif_data_source = :amif_data_source
  and amif_mth_seq_no = :amif_mth_seq_no
";

         string res = db.ExecuteScalar(sql , CommandType.Text , parms);
         return res;
      }

      /// <summary>
      /// for W20110
      /// </summary>
      /// <param name="ldt_bef_date"></param>
      /// <param name="ldt_date"></param>
      /// <returns></returns>
      public DateTime MaxAMIF_DATE(DateTime ldt_bef_date , DateTime ldt_date , string amif_data_source = "U") {
         object[] parms =
         {
                ":ldt_bef_date", ldt_bef_date,
                ":ldt_date", ldt_date,
                ":amif_data_source", amif_data_source
            };

         string sql = @"
select max(amif_date) as ldt_date
  from ci.amif
where amif_date >= :ldt_bef_date
  and amif_date < :ldt_date
  and amif_data_source = 'U'
";

         DataTable dtResult = db.GetDataTable(sql , parms);
         DateTime rtnDate = dtResult.Rows[0]["LDT_DATE"].AsDateTime();
         return rtnDate;
      }

      /// <summary>
      /// for W28110
      /// </summary>
      /// <param name="amif_date"></param>
      /// <param name="amif_prod_type"></param>
      /// <param name="amif_kind_id"></param>
      /// <returns></returns>
      public DataTable d_28110_amif2(string amif_date , string amif_prod_type = "M" , string amif_kind_id = "STW") {
         object[] parms =
         {
                ":amif_date", amif_date,
                ":amif_prod_type", amif_prod_type,
                ":amif_kind_id", amif_kind_id
            };

         string sql = @"
SELECT AMIF_YEAR,   
         AMIF_DATE,   
         AMIF_PROD_ID,   
         AMIF_HIGH_PRICE,   
         AMIF_LOW_PRICE,   
         AMIF_OPEN_PRICE,   
         AMIF_CLOSE_PRICE,   
         AMIF_KIND_ID,   
         AMIF_SETTLE_DATE  
 from ci.amif
where amif_date = :amif_date
  and amif_prod_type = :amif_prod_type
  and amif_kind_id = :amif_kind_id
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// Update CI.AMIF
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
SELECT AMIF_YEAR,   
         AMIF_DATE,   
         AMIF_PROD_ID,   
         AMIF_HIGH_PRICE,   
         AMIF_LOW_PRICE,   
         AMIF_OPEN_PRICE,   
         AMIF_CLOSE_PRICE, 
         AMIF_SETTLE_PRICE,
         AMIF_OPEN_INTEREST,
         AMIF_UP_DOWN_VAL,
         AMIF_M_QNTY_TAL,  
         AMIF_KIND_ID,   
         AMIF_SETTLE_DATE
FROM CI.AMIF
";
         return db.UpdateOracleDB(inputData , sql);
      }

      /// <summary>
      /// if have data in amif,return Y, else return N
      /// </summary>
      /// <param name="idt_date"></param>
      /// <param name="amif_kind_id2"></param>
      /// <param name="amif_delivery_mth_seq_no"></param>
      /// <returns></returns>
      public bool haveTradeTxw(DateTime amif_date , string amif_kind_id2 = "TXW" , int amif_delivery_mth_seq_no = 1) {
         object[] parms = {
                ":amif_date", amif_date,
                ":amif_kind_id2", amif_kind_id2,
                ":amif_delivery_mth_seq_no", amif_delivery_mth_seq_no,
            };

         string sql = @"
select case when COUNT(*) > 0 then 'Y' else 'N' end as is_trade_txw
from ci.AMIF
where amif_date = :amif_date
and amif_kind_id2 = :amif_kind_id2
and amif_delivery_mth_seq_no = :amif_delivery_mth_seq_no
";
         string res = db.ExecuteScalar(sql , CommandType.Text , parms);

         return (res == "Y" ? true : false);
      }

      /// <summary>
      /// get close price
      /// </summary>
      /// <param name="amif_date"></param>
      /// <param name="amif_data_source"></param>
      /// <param name="amif_kind_id"></param>
      /// <returns></returns>
      public decimal GetClosePrice(DateTime amif_date , string amif_data_source = "U" , string amif_kind_id = "TXF") {
         object[] parms = {
                ":amif_date", amif_date,
                ":amif_data_source", amif_data_source,
                ":amif_kind_id", amif_kind_id
            };

         string sql = @"
select amif_close_price as id_close_price
from ci.amif
where amif_date = :amif_date
and amif_data_source = :amif_data_source
and amif_kind_id = :amif_kind_id
";
         DataTable dtResult = db.GetDataTable(sql , parms);
         if (dtResult.Rows.Count > 0)
            return dtResult.Rows[0]["id_close_price"].AsDecimal();
         else
            return 0;
      }

      /// <summary>
      /// get some field data, return amif_close_price/amif_up_down_val
      /// </summary>
      /// <param name="amif_date"></param>
      /// <param name="amif_kind_id"></param>
      /// <param name="amif_data_source"></param>
      /// <param name="amif_mth_seq_no"></param>
      /// <returns></returns>
      public DataTable ListAll(DateTime amif_date ,
                              string amif_kind_id = "TXF" ,
                              string amif_data_source = "T" ,
                              int amif_mth_seq_no = 1) {
         object[] parms = {
                ":amif_date", amif_date,
                ":amif_kind_id", amif_kind_id,
                ":amif_data_source", amif_data_source,
                ":amif_mth_seq_no", amif_mth_seq_no
            };

         string sql = @"
select amif_close_price,amif_up_down_val
from ci.amif  
where amif_date = :amif_date
and amif_kind_id = :amif_kind_id
and amif_data_source = :amif_data_source
and amif_mth_seq_no = :amif_mth_seq_no
";
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}
