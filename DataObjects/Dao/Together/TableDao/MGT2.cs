using BusinessObjects;
using System;
using System.Data;
/// <summary>
/// Winni, 2019/4/3
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
   public class MGT2 : DataGate {

      /// <summary>
      /// save data
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {
            try {
                string sql = @"
SELECT  
	MGT2_KIND_ID,
	MGT2_KIND_ID_OUT,
	MGT2_SEQ_NO,
	MGT2_PROD_TYPE,
	MGT2_PROD_SUBTYPE,

	MGT2_ABBR_NAME,
	MGT2_NAME,
	MGT2_GROUP_KIND_ID,
	MGT2_STOCK_ID,
	MGT2_END_YMD,

	MGT2_DATA_TYPE,
	MGT2_ADJUST_RATE,
	MGT2_CP_KIND,
	MGT2_ABROAD,
	MGT2_W_TIME,

	MGT2_W_USER_ID,
   MGT2_ADJUST_RATE_MAXV,
   MGT2_ADJUST_RATE_EWMA
FROM CI.MGT2
ORDER BY MGT2_SEQ_NO , MGT2_KIND_ID
";

                return db.UpdateOracleDB(inputData, sql);
            }
            catch(Exception ex) {
                throw ex;
            }
      }

      public string GetldValue(string prod_type,string kind_id) {

         object[] parms = {
                ":prod_type", prod_type,
                ":kind_id", kind_id
            };

         string sql = @"SELECT MGT2_ADJUST_RATE as LD_VALUE
		                   FROM CI.MGT2
		                   WHERE MGT2_PROD_TYPE = :prod_type AND MGT2_KIND_ID = :kind_id";

         return db.ExecuteScalar(sql, CommandType.Text, parms);
      }
   }
}
