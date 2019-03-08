using BusinessObjects;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Winni, 2019/2/12
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class STW : DataGate {

      /// <summary>
      /// for W28110 retrieve (讀取選取的日期資料)
      /// </summary>
      /// <param name="dateYmd">yyyyMMdd</param>
      /// <returns></returns>
      public DataTable GetDataByDate(string dateYmd) {

         object[] parms =
         {
                ":dateYmd", dateYmd
            };

         string sql = @"
SELECT STW_YMD,   
         STW_COM,   
         STW_SETTLE_M,   
         STW_SETTLE_Y,   
         STW_OPEN_1,  
         STW_HIGH,     
         STW_LOW,    
         STW_CLSE_1,    
         STW_SETTLE,   
         STW_VOLUMN,   
         STW_OINT, 
         --多  
         STW_DEL,   
         STW_RECTYP,   
         STW_OPEN_I1,   
         STW_OPEN_2,   
         STW_OPEN_I2, 
         STW_HIGH_I,  
         STW_LOW_I,   
         STW_CLSE_I1,   
         STW_CLSE_2,   
         STW_CLSE_I2  
FROM ci.STW   
WHERE STW_YMD = :dateYmd
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// for W28110 delete (刪除被選取的日期資料)
      /// </summary>
      /// <param name="dateYmd">yyyyMMdd</param>
      /// <returns></returns>
      public bool DeleteByDate(string dateYmd) {

         object[] parms = {
                "@dateYmd", dateYmd
            };

         string sql = @"DELETE FROM CI.STW WHERE STW_YMD = :dateYmd";
         int executeResult = db.ExecuteSQL(sql , parms);

         if (executeResult >= 0) {
            return true;
         } else {
            throw new Exception("刪除失敗");
         }
      }

      /// <summary>
      /// GetSettleYM return yyyyMM (取得最小履約年月yyyyMM)
      /// </summary>
      /// <param name="dateYmd">yyyyMMdd</param>
      /// <returns></returns>
      public string GetSettleYM(string dateYmd) {

         object[] parms =
         {
               ":dateYmd", dateYmd
         };

         string sql = @"
select min(a) as ls_min_month
from  (select  trim(stw_settle_y || stw_settle_m)  a   
         from ci.STW
         where STW_YMD = :dateYmd )
where a is not null or a <> '' 
";

         return db.ExecuteScalar(sql , CommandType.Text , parms);
      }

      /// <summary>
      /// 當日若出現一筆結算價為0,有可能是最後結算日,將履約年月最小的一筆資料OI清為0
      /// </summary>
      /// <param name="dateYmd">yyyyMMdd</param>
      /// <param name="year">yyyy</param>
      /// <param name="month">MM</param>
      /// <returns></returns>
      public int updateOI(string dateYmd , string year , string month) {

         object[] parms =
         {
                ":dateYmd", dateYmd,
                ":year", year,
                ":month", month
            };

         string sql = @"
UPDATE CI.STW
SET STW_OINT = '0'
WHERE STW_YMD = :dateYmd
AND STW_COM = 'TW'
AND STW_SETTLE_Y = :year
AND STW_SETTLE_M = :month
";
         return db.ExecuteSQL(sql , parms);
      }

      /// <summary>
      /// Update CI.STW
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
SELECT STW_YMD,   
         STW_COM,   
         STW_SETTLE_M,   
         STW_SETTLE_Y,   
         STW_OPEN_1,  
         STW_HIGH,     
         STW_LOW,    
         STW_CLSE_1,    
         STW_SETTLE,   
         STW_VOLUMN,   
         STW_OINT, 
         --多  
         STW_DEL,   
         STW_RECTYP,   
         STW_OPEN_I1,   
         STW_OPEN_2,   
         STW_OPEN_I2, 
         STW_HIGH_I,  
         STW_LOW_I,   
         STW_CLSE_I1,   
         STW_CLSE_2,   
         STW_CLSE_I2 　
FROM CI.STW
";
         return db.UpdateOracleDB(inputData , sql);
      }

   }
}
