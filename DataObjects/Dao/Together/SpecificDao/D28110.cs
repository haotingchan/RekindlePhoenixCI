using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D28110 : DataGate {

      /// <summary>
      /// 刪除被選取的日期資料
      /// </summary>
      /// <param name="as_date">yyyyMMdd</param>
      /// <returns></returns>
      public bool DeleteByDate(string as_date) {

         object[] parms = {
                "@as_date", as_date
            };

         string sql = @"DELETE FROM CI.STW WHERE STW_YMD = :as_date";
         int executeResult = db.ExecuteSQL(sql , parms);

         if (executeResult > 0) {
            return true;
         } else {
            throw new Exception("刪除失敗");
         }
      }

      public DataTable d_28110_amif(DateTime as_ymd) {

         object[] parms =
         {
                ":as_ymd", as_ymd
            };

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
         AMIF_SETTLE_DATE,
         y_settle_price,y_close_price  
 from ci.amif,     
    (select STWD_SETTLE_DATE as settle_date,
             STWD_SETTLE_PRICE as y_settle_price,
             STWD_CLOSE_PRICE as y_close_price
          from ci.STWD
         where STWD_YMD = (SELECT to_char(max(AMIF_DATE),'yyyymmdd')
                        FROM ci.AMIF
                       WHERE AMIF_DATE <  :as_date
                             and AMIF_DATE >= :as_date - INTERVAL '90' day
                           and AMIF_DATA_SOURCE = 'U')) Y
where amif_date = :as_date
  and ((amif_prod_type = 'M' and amif_kind_id = 'STW') )
  and amif_settle_date = nvl(trim(Y.settle_date(+)),'000000')
  order by amif_settle_date 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get data insert into CI.STWD 
      /// (STW_YMD/KIND_ID/SETTLE_YM/OPEN_PRICE/HIGH/LOW/CLOSE_PRICE/SETTLE_PRICE/M_QNTY/OI/ROWNUM - 1/SYSDATE/:gs_user_id/STW_RECTYP)
      /// </summary>
      /// <param name="gs_user_id"></param>
      /// <param name="li_div"></param>
      /// <param name="ls_ymd"></param>
      /// <returns></returns>
      public DataTable InsertData(string gs_user_id , int li_div , string ls_ymd) {

         object[] parms = {
                "@gs_user_id", gs_user_id,
                "@li_div", li_div,
                "@ls_ymd", ls_ymd
            };

         string sql = @"
INSERT INTO CI.STWD
SELECT STW_YMD,
KIND_ID,
SETTLE_YM,
OPEN_PRICE,
HIGH,
LOW,
CLOSE_PRICE,
SETTLE_PRICE,
M_QNTY,
OI,
ROWNUM - 1,
SYSDATE,
:gs_user_id,
STW_RECTYP
FROM
(SELECT STW_YMD,'STW' AS KIND_ID,
       CASE WHEN STW_SETTLE_M = '  ' THEN '000000' ELSE TO_CHAR(STW_SETTLE_Y||STW_SETTLE_M) END AS SETTLE_YM,
       TO_NUMBER(STW_OPEN_1) / CASE WHEN STW_SETTLE_M = '  ' THEN 1 ELSE :li_div END AS OPEN_PRICE, 
       TO_NUMBER(STW_HIGH)/ CASE WHEN STW_SETTLE_M = '  ' THEN 1 ELSE :li_div END AS HIGH,
       TO_NUMBER(STW_LOW) / CASE WHEN STW_SETTLE_M = '  ' THEN 1 ELSE :li_div END AS LOW,
       TO_NUMBER(STW_CLSE_1) / CASE WHEN STW_SETTLE_M = '  ' THEN 1 ELSE :li_div END AS CLOSE_PRICE,
       TO_NUMBER(NVL(TRIM(STW_SETTLE),'0')) / CASE WHEN STW_SETTLE_M = '  ' THEN 1 ELSE :li_div END AS SETTLE_PRICE,
       TO_NUMBER(NVL(TRIM(STW_VOLUMN),'0')) AS M_QNTY,TO_NUMBER(NVL(TRIM(STW_OINT),'0')) AS OI,
         STW_RECTYP
 FROM CI.STW
WHERE STW_YMD = :ls_ymd
ORDER BY STW_SETTLE_Y,STW_SETTLE_M)
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// 執行SP
      /// </summary>
      /// <param name="ldt_date">yyyy/MM/dd</param>
      /// <param name="ls_prod_type">M</param>
      /// <param name="RETURNPARAMETER">一開始傳null，成功回傳0</param>
      /// <returns></returns>
      public DataTable ExecuteStoredProcedure(DateTime ldt_date , string ls_prod_type , string spName) {

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("ldt_date",ldt_date),
            new DbParameterEx("ls_prod_type",ls_prod_type)
            //new DbParameterEx("RETURNPARAMETER",null)
         };

         string sql = ":spName";

         DataTable reResult = db.ExecuteStoredProcedureEx(sql , parms , true);

         return reResult;
      }

   }
}
