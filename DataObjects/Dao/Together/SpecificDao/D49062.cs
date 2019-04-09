using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/4/8
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 49062 SPAN參數名稱設定
   /// </summary>
   public class D49062 : DataGate {

      /// <summary>
      /// get ci.mgt8 (dddw_mgt8_f_id_src) 交易所 + 商品
      /// return mgt8_f_id/mgt8_f_name/mgt8_f_exchange/cp_display 4 fields 
      /// </summary>
      /// <returns></returns>
      public DataTable GetMgt8FIdList() {

         string sql = @"
select
    mgt8_f_id,   
    mgt8_f_name,   
    mgt8_f_exchange,
    '【' || trim(mgt8_f_id) || '】 ' ||   trim(mgt8_f_exchange) || ' ' || trim(mgt8_f_name) as cp_display
from ci.mgt8  
union all 
select 
    '%',
    ' ',
    '全部',
    ' 【%】 全部'
from dual 
order by mgt8_f_id
";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      /// <summary>
      /// get ci.mgt8 (dddw_mgt8_kind_type_src) 契約種類
      /// return kind_type/kind_name/cod_seq_no 3 fields 
      /// </summary>
      /// <returns></returns>
      public DataTable GetMgt8KindFIdList() {

         string sql = @"
select 
	trim(cod_id) as kind_type,   
	trim(cod_desc) as kind_name,   
	cod_seq_no 
from ci.cod  
where cod_txn_id = 'MGT8'   
and cod_col_id = 'MGT8_KIND_TYPE'    
union all
select 
	'% ',
	'全部',
	0
from dual
order by cod_seq_no
";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      /// <summary>
      /// ci.sp_h_txn_49062_stat (d_49062)
      /// </summary>
      /// <param name="startDate"></param>
      /// <param name="endDate"></param>
      /// <param name="fId"></param>
      /// <param name="kindType"></param>
      /// <param name="seqNo"></param>
      /// <returns></returns>
      public DataTable ExecuteStoredProcedure(string startDate , string endDate , string fId , string kindType , int seqNo) {
         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("startDate",startDate),
            new DbParameterEx("endDate",endDate),
            new DbParameterEx("fId",fId),
            new DbParameterEx("kindType",kindType),
            new DbParameterEx("seqNo",seqNo)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_TXN_49062_STAT";

         return db.ExecuteStoredProcedureEx(sql , parms , true);
      }

      /// <summary>
      /// ci.sp_h_txn_49062_detl (d_49062_daily)
      /// </summary>
      /// <returns></returns>
      public DataTable ExecuteStoredProcedure2(string startDate , string endDate , string fId , string kindType , int seqNo) {
         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("startDate",startDate),
            new DbParameterEx("endDate",endDate),
            new DbParameterEx("fId",fId),
            new DbParameterEx("kindType",kindType),
            new DbParameterEx("seqNo",seqNo)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_TXN_49062_DETL";

         return db.ExecuteStoredProcedureEx(sql , parms , true);
      }
   }
}
