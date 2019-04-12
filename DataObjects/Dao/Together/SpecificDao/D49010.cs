using System;
using System.Data;
/// <summary>
/// Winni, 2019/4/11
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D49010 : DataGate {

      /// <summary>
      /// get ci.cod data return cod_id/cod_desc/cod_seq_no 3 feild (dddw_mgt2_prod_subtype) ProdSubtype下拉選單
      /// 需以cod_seq_no order by 所以不用ci.cod的共用function
      /// </summary>
      /// <returns></returns>
      public DataTable GetDdlProdSubtype() {

         string sql = @"
select 
    trim(cod_id) as cod_id,   
    trim(cod_desc) as cod_desc,   
    cod_seq_no  
from ci.cod
where cod_txn_id = '49020' 
order by  decode(cod_seq_no,'',-1), cod_seq_no
";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      /// <summary>
      /// get ci.mgt2 data return mgt2_seq_no/mgt2_kind_id/mgt2_prod_subtype 3 feild (dddw_mgt2_kind_id_edit) KindId下拉選單
      /// </summary>
      /// <returns></returns>
      public DataTable GetDdlKindId() {

         string sql = @"
select 
    mgt2_seq_no,
    mgt2_kind_id,
    mgt2_prod_subtype
from ci.mgt2    
where (nvl(mgt2_data_type,' ') = ' ' or mgt2_kind_id in ('ETF','ETC'))
order by mgt2_seq_no
";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      /// <summary>
      /// get ci.hcpr data return 11 feild (d_49010)
      /// </summary>
      /// <returns></returns>
      public DataTable GetDataList() {

         string sql = @"
select
cpr_prod_subtype,
cpr_kind_id,
cpr_effective_date, 
cpr_price_risk_rate, 
cpr_approval_date, 

cpr_approval_number,
cpr_remark,
cpr_w_time, 
cpr_w_user_id, 
cpr_data_num,

' ' as is_newrow
from ci.hcpr   
order by cpr_kind_id
";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }
   }
}
