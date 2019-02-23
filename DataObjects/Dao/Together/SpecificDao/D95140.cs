using OnePiece;
using System.Data;

/// <summary>
/// winni,2019/1/16
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// Eurex商品OI轉入帳號檢核錯誤(訊息代碼9514)統計表 
   /// </summary>
   public class D95140 {

      private Db db;

      public D95140() {

         db = GlobalDaoSetting.DB;

      }

      /// <summary>
      /// List AE4+ABRK data (月計總表) (欄位直接輸出到excel)
      /// </summary>
      /// <param name="startMonth">起始月份,format=yyyyMM</param>
      /// <param name="endMonth">結束月份,format=yyyyMM</param>
      /// <returns></returns>
      public DataTable ListMonth(string startMonth ,
                                  string endMonth) {

         object[] parms = {
                ":as_fm_ymd", startMonth,
                ":as_to_ymd", endMonth
            };

         string sql = @"
select substr(substr(AE4_YMD,1,6),1,4) || '/' || substr(substr(AE4_YMD,1,6),5,2)  AS  AE4_YMD,
   AE4_BRK_NO,
   ABRK_ABBR_NAME,
   sum(AE4_9514_CNT) as AE4_9514_CNT,
   sum(AE4_TOT_CNT) as AE4_TOT_CNT,
   case when sum(AE4_TOT_CNT) > 0 then round(sum(AE4_9514_CNT) /sum(AE4_TOT_CNT),6) else 0 END as AE4_RATE
from ci.AE4,ci.ABRK
where AE4_YMD >= trim(:as_fm_ymd) || '01'
and AE4_YMD <= trim(:as_to_ymd) || '31'
and AE4_BRK_NO = ABRK_NO
group by substr(AE4_YMD,1,6),AE4_BRK_NO,ABRK_ABBR_NAME
order by ae4_ymd , ae4_brk_no ";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// List AE4+ABRK data 依照期貨商 (欄位直接輸出到excel)
      /// </summary>
      /// <param name="startDate">起始日,format=yyyyMMdd</param>
      /// <param name="endDate">結束日,format=yyyyMMdd</param>
      /// <returns></returns>
      public DataTable ListDateFcm(string startDate ,
                                  string endDate) {

         object[] parms = {
                ":as_fm_ymd", startDate,
                ":as_to_ymd", endDate
            };

         string sql = @"
select substr(AE4_YMD,1,4)  || '/' || substr(AE4_YMD,5,2) || '/' || substr(AE4_YMD,7,2) as AE4_YMD,
   AE4_BRK_NO,
   ABRK_ABBR_NAME,
   AE4_9514_CNT,
   AE4_TOT_CNT
   --case when AE4_TOT_CNT > 0 then round(AE4_9514_CNT / AE4_TOT_CNT,6) else 0 END as AE4_RATE
from ci.AE4,ci.ABRK
where AE4_YMD >= :as_fm_ymd
and AE4_YMD <= :as_to_ymd
and AE4_BRK_NO = ABRK_NO
order by ae4_ymd , ae4_brk_no ";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// List EUS data 依照交易人帳號 (欄位直接輸出到excel)
      /// </summary>
      /// <param name="startDate">起始日,format=yyyyMMdd</param>
      /// <param name="endDate">結束日,format=yyyyMMdd</param>
      /// <returns></returns>
      public DataTable ListDateAcc(string startDate ,
                                  string endDate) {

         object[] parms = {
                ":as_fm_ymd", startDate,
                ":as_to_ymd", endDate
            };

         string sql = @"
SELECT substr(EUS_TRADE_DATE,1,4)  || '/' || substr(EUS_TRADE_DATE,5,2) || '/' || substr(EUS_TRADE_DATE,7,2) as EUS_TRADE_DATE,
   EUS_EU1_PARTYID,
   EUS_EUREX_ACC_NO,
   EUS_EU2_PARTYID,
   EUS_ACC_NO,
   EUS_ACC_CODE
FROM ci.EUS
where EUS_TRADE_DATE >=:as_fm_ymd
and EUS_TRADE_DATE <=:as_to_ymd
and EUS_STATUSCODE ='9514'
order by eus_trade_date , eus_eu1_partyid , eus_eurex_acc_no , eus_eu2_partyid , eus_acc_no ";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}
