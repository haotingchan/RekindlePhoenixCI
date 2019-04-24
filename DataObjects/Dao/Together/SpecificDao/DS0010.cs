using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
   public class DS0010 {
      private Db db;

      public DS0010() {
         db = GlobalDaoSetting.DB;
      }

      //================================================
      //查詢條件2018/8/3
      //sheet1
      //d_s0010_cm==>整個dataTable output
      //d_s0010_sp2==>查詢三次,填入下面三個cell==>改直接用sql串好(ken改寫)

      //sheet2
      //d_s0010_fcm==>整個dataTable output
      //================================================

      /// <summary>
      /// Get cfo.MGS1, cfo.MGS2, ci.HEXRT data, return DATA_DATE/FCM/BEF_MARGIN/AFT_MARGIN (結算會員List)
      /// </summary>
      /// <param name="ym_date">查詢日期</param>
      /// <returns></returns>
      public DataTable d_s0010_cm(DateTime as_date) {
         object[] parms = {
                ":as_date", as_date
            };

         string sql = @"
SELECT --MGS1.DATA_DATE as DATA_DATE,   
    substr(MGS1.CM_NO,1,4) as FCM,  
    sum(MGS1.MAINT * HEXRT_EXCHANGE_RATE) as BEF_MARGIN,
    sum(MGS2.MAINT * HEXRT_EXCHANGE_RATE) as AFT_MARGIN
FROM cfo.MGS1,   
    cfo.MGS2,
    (select HEXRT_CURRENCY_TYPE,HEXRT_EXCHANGE_RATE 
    from ci.HEXRT 
    where HEXRT_DATE = :as_date and HEXRT_COUNT_CURRENCY = '1'
    union all
    --台幣
    select '1',1 from dual) H
WHERE MGS1.DATA_DATE =  :as_date
and MGS1.FCM_NO = MGS2.FCM_NO 
and MGS1.DATA_DATE = MGS2.DATA_DATE 
and MGS1.CURRENCY_TYPE = HEXRT_CURRENCY_TYPE
and MGS2.CURRENCY_TYPE = HEXRT_CURRENCY_TYPE
group by MGS1.DATA_DATE,substr(MGS1.CM_NO,1,4)
order by fcm
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// 查詢三次，將撈出的資料串在一起 (結算會員下的調整內容) --- ken改寫原sp2
      /// </summary>
      /// <param name="ym_date">查詢日期</param>
      /// <returns></returns>
      public DataTable d_s0010_sp2(string as_date) {
         object[] parms = {
                ":as_date", as_date
            };

         string sql = @"
select sp2_type,
LISTAGG(test, '、') WITHIN GROUP (ORDER BY sp1_seq_no) as test
from (
    select
        sp2s_type as sp2_type,
        sp2s_kind_id1 as sp2_kind_id1,
        sp2s_kind_id2 as sp2_kind_id2,
        sp1_change_range,
        sp1_seq_no,
        (case when sp2s_type='SD' then trim(sp2s_kind_id1)||'/'||trim(sp2s_kind_id2)||':'||to_char(nvl(sp1_change_range,0)*100,'99.9')||'%' 
                                  else trim(sp2s_kind_id1)||':'||to_char(nvl(sp1_change_range,0)*100,'99.9')||'%' end ) as test
    from cfo.sp2s,ci.sp1
    where sp2s_value_date = to_date(:as_date,'yyyy/mm/dd')
    and sp2s_date = sp1_date
    and sp2s_type = sp1_type
    and sp2s_kind_id1 = sp1_kind_id1
    and sp2s_kind_id2 = sp1_kind_id2
    AND SP2S_SPAN_CODE = 'Y'
    union all
    SELECT
        'PSR',
        mg2s_kind_id as mg2_kind_id,
        ' ',
        mg1_change_range,
        mg1_seq_no,
        trim(mg2s_kind_id)||':'||to_char(nvl(mg1_change_range,0)*100,'99.9')||'%' as test
    from cfo.mg2s,ci.mg1
    where mg2s_value_date = to_date(:as_date ,'yyyy/mm/dd')
    and mg2s_date = mg1_date
    and mg2s_kind_id = mg1_kind_id
    AND MG1_TYPE IN ('-','A')
    AND MG2S_SPAN_CODE = 'Y'
    order by sp1_seq_no
) a
group by sp2_type
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

//      public DataTable d_s0010_sp2_old(DateTime as_date) {
//         object[] parms = {
//                ":as_date", as_date
//            };

//         string sql = @"
//SELECT SP2S_TYPE as SP2_TYPE,SP2S_KIND_ID1 as SP2_KIND_ID1,SP2S_KIND_ID2 as SP2_KIND_ID2,SP1_CHANGE_RANGE,SP1_SEQ_NO
// FROM cfo.SP2S,ci.SP1
//WHERE --SP2S_VALUE_DATE = :ad_date
//  --AND 
//  SP2S_DATE = SP1_DATE
//  AND SP2S_TYPE = SP1_TYPE
//  AND SP2S_KIND_ID1 = SP1_KIND_ID1
//  AND SP2S_KIND_ID2 = SP1_KIND_ID2
//  AND SP2S_SPAN_CODE = 'Y'
//union all
//SELECT 'PSR',MG2S_KIND_ID as MG2_KIND_ID,' ',MG1_CHANGE_RANGE,MG1_SEQ_NO
// FROM cfo.MG2S,ci.MG1
//WHERE --MG2S_VALUE_DATE = :ad_date
//  --AND 
//  MG2S_DATE = MG1_DATE
//  AND MG2S_KIND_ID = MG1_KIND_ID
//  AND MG1_TYPE IN ('-','A')
//  AND MG2S_SPAN_CODE = 'Y'
//order by sp1_seq_no 
//";

//         DataTable dtResult = db.GetDataTable(sql , parms);

//         return dtResult;
//      }

      /// <summary>
      /// Get cfo.MGS1, cfo.MGS2, ci.HEXRT data, return DATA_DATE/FCM/BEF_MARGIN/AFT_MARGIN (結算會員List的全市場合計)
      /// </summary>
      /// <param name="ym_date">查詢日期</param>
      /// <returns></returns>
      public DataTable d_s0010_fcm(DateTime as_date) {
         object[] parms = {
                ":as_date", as_date
            };

         string sql = @"
SELECT --MGS1.DATA_DATE as DATA_DATE,   
    substr(MGS1.FCM_NO,1,4) as FCM,  
    sum(MGS1.MAINT * HEXRT_EXCHANGE_RATE) as BEF_MARGIN,
    sum(MGS2.MAINT * HEXRT_EXCHANGE_RATE) as AFT_MARGIN
FROM cfo.MGS1,   
    cfo.MGS2,
    (select HEXRT_CURRENCY_TYPE,HEXRT_EXCHANGE_RATE 
    from ci.HEXRT 
    where HEXRT_DATE = :as_date and HEXRT_COUNT_CURRENCY = '1'
    union all
    --台幣
    select '1',1 from dual) H
WHERE MGS1.DATA_DATE =  :as_date
and MGS1.FCM_NO = MGS2.FCM_NO 
and MGS1.DATA_DATE = MGS2.DATA_DATE 
and MGS1.CURRENCY_TYPE = HEXRT_CURRENCY_TYPE
and MGS2.CURRENCY_TYPE = HEXRT_CURRENCY_TYPE
group by MGS1.DATA_DATE,substr(MGS1.FCM_NO,1,4)
order by fcm
";
      
         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
