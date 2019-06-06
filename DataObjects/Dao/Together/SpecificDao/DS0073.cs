using BusinessObjects;
using BusinessObjects.Enums;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
   public class DS0073 {
      private Db db;

      public DS0073() {
         db = GlobalDaoSetting.DB;
      }

      public DataTable GetMarginData() {
         string sql = @"select  * from cfo.SPAN_MARGIN";

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

      public DataTable GetPeriodData(string module, string user_Id) {
         object[] parms = {
                "@module", module,
                "@userId",user_Id
            };

         string sql = @"select SPAN_PERIOD_MODULE,
	                                 SPAN_PERIOD_START_DATE,
	                                 SPAN_PERIOD_END_DATE,
	                                 SPAN_PERIOD_USER_ID,
	                                 SPAN_PERIOD_W_TIME
                                 from cfo.SPAN_PERIOD
                                where span_period_module = :module
                                and span_period_user_id like :userId";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      public DataTable GetMarginUserData() {
         string sql = @"select distinct
                                    trim(span_margin_spn) as span_margin_spn,
                                    trim(span_margin_pos) as span_margin_pos,
                                    trim(span_margin_spn_path) as span_margin_spn_path
                                    from cfo.SPAN_MARGIN";

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }

      public ResultData updatePeriodData(DataTable inputData) {

         string sql = @"SELECT SPAN_PERIOD_MODULE,
	SPAN_PERIOD_START_DATE,
	SPAN_PERIOD_END_DATE,
	SPAN_PERIOD_USER_ID,
	SPAN_PERIOD_W_TIME
FROM CFO.SPAN_PERIOD";

         return db.UpdateOracleDB(inputData, sql);

      }

      public ResultData updateMarginData(DataTable inputData) {

         string sql = @"SELECT   
                                        SPAN_MARGIN_SPN_PATH  ,
                                        SPAN_MARGIN_SPN,
                                        SPAN_MARGIN_POS,
                                        SPAN_MARGIN_RATIO,
                                        SPAN_MARGIN_USER_ID,
                                        SPAN_MARGIN_W_TIME 
                                   FROM CFO.SPAN_MARGIN";

         return db.UpdateOracleDB(inputData, sql);

      }

      public ResultStatus UpdateAllDB(DataTable periodData, DataTable marginData) {
         List<string> sqlList = new List<string>();
         List<DataTable> dtList = new List<DataTable>();


         string periodsql = @"SELECT SPAN_PERIOD_MODULE,
	                        SPAN_PERIOD_START_DATE,
	                        SPAN_PERIOD_END_DATE,
	                        SPAN_PERIOD_USER_ID,
	                        SPAN_PERIOD_W_TIME
                        FROM CFO.SPAN_PERIOD";


         string marginsql = @"SELECT   
                                        SPAN_MARGIN_SPN_PATH  ,
                                        SPAN_MARGIN_SPN,
                                        SPAN_MARGIN_POS,
                                        SPAN_MARGIN_RATIO,
                                        SPAN_MARGIN_USER_ID,
                                        SPAN_MARGIN_W_TIME 
                                   FROM CFO.SPAN_MARGIN";

         //sql
         sqlList.Add(periodsql);
         sqlList.Add(marginsql);

         //Update Data
         dtList.Add(periodData);
         dtList.Add(marginData);

         return db.UpdateMultiTable(dtList, sqlList).Status;
      }


   }
}
