using System;
using System.Data;
/// <summary>
/// Winni, 2019/3/25
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D49070 : DataGate {
      /// <summary>
      /// get CI.SPT1 data return 14 feild (d49070)
      /// </summary>
      /// <returns></returns>
      public DataTable ListData() {

         string sql = @"
SELECT 
    SPT1_KIND_ID1, 
    SPT1_KIND_ID2, 
    SPT1_ABBR_NAME, 
    SPT1_NAME, 
    SPT1_SEQ_NO, 

    SPT1_W_TIME, 
    SPT1_W_USER_ID, 
    SPT1_KIND_ID1_OUT, 
    SPT1_KIND_ID2_OUT, 
    SPT1_COM_ID, 

    SPT1_OSW_GRP, 
    SPT1_ADJUST_RATE, 
    SPT1_DATA_TYPE, 
    SPT1_MAX_SPNS_RATE,
    ' ' as IS_NEWROW
FROM CI.SPT1
ORDER BY SPT1_SEQ_NO             
";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }
   }
}
