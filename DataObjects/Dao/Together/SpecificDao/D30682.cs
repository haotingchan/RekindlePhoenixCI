using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/3/19
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 臺股期貨歷史及瞬時波動率查詢
   /// </summary>
   public class D30682 {

      private Db db;

      public D30682() {

         db = GlobalDaoSetting.DB;

      }

      /// <summary>
      /// get CI.VOLS data return VOLS_MARKET_CODE/VOLS_YMD/VOLS_EXCLUDE_CNT/VOLS_VALUE 4 feilds (d30682_1)
      /// return I.AI2_YMD/AI2_M_QNTY/AI2_OI/AI2_DAY_COUNT/AM9_ACC_CNT/AM10_CNT 6 feilds
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <param name="dataType">H:歷史 I:瞬時</param>
      /// <returns></returns>
      public DataTable ListStatisticsData(string startDate , string endDate , string dataType , string header = "N") {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate,
                ":dataType", dataType
            };

         string sql = @"
SELECT 
    VOLS_MARKET_CODE,
    VOLS_YMD,   
    VOLS_EXCLUDE_CNT,   
    VOLS_VALUE  
FROM CI.VOLS   
WHERE VOLS_YMD >= :startDate
AND VOLS_YMD <= :endDate
AND VOLS_DATA_TYPE = :dataType
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         if (header == "Y") {
            string[] title = new string[] { "交易時段:0一般/1夜盤" , "日期" , "極端筆數" , "平均值" };
            int w = 0;
            foreach (DataColumn col in dtResult.Columns) {
               col.Caption = title[w++];
            }
         }

         return dtResult;
      }

      /// <summary>
      /// get CI.VOLD data return VOLD_MARKET_CODE/VOLD_YMD/VOLD_DATA_TIME/VOLD_VALUE/VOLD_EXCLUDE_FLAG 5 feilds (d30682_2)
      /// </summary>
      /// <param name="startDate">yyyyMMdd</param>
      /// <param name="endDate">yyyyMMdd</param>
      /// <param name="dataType">H:歷史 I:瞬時</param>
      /// <returns></returns>
      public DataTable ListDetailData(string startDate , string endDate , string dataType , string header = "N") {

         object[] parms = {
                ":startDate", startDate,
                ":endDate", endDate,
                ":dataType", dataType
            };

         string sql = @"
SELECT 
    VOLD_MARKET_CODE,
    VOLD_YMD,    
    VOLD_DATA_TIME,   
    VOLD_VALUE,   
    VOLD_EXCLUDE_FLAG  
FROM CI.VOLD   
WHERE VOLD_YMD >= :startDate
AND VOLD_YMD <= :endDate
AND VOLD_DATA_TYPE = :dataType
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         if (header == "Y") {
            string[] title = new string[] { "交易時段:0一般/1夜盤" , "日期" , "時間" , "值" , "Y: 極端值/N:非極端值" };
            int w = 0;
            foreach (DataColumn col in dtResult.Columns) {
               col.Caption = title[w++];
            }
         }

         return dtResult;
      }
   }
}
