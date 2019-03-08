using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D30750 {
        private Db db;

        public D30750() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetAI2Data(string symd, string eymd) {

            object[] parms = {
                ":as_symd", symd,
                ":as_eymd", eymd
            };

            string sql = @" select AI2_YMD,
        AI2_PARAM_KEY,
        AI2_M_QNTY,
        AI2_DAY_COUNT,
        A.RPT_SEQ_NO as RPT_SEQ_NO,
        B.RPT_SEQ_NO as RPT_SEQ_NO_2
   from
         (select AI2_YMD,
                 AI2_PARAM_KEY,
                 AI2_M_QNTY,
                 AI2_DAY_COUNT   
            from ci.AI2
           where AI2_SUM_TYPE = 'M'
             and AI2_SUM_SUBTYPE = '3'
             and TRIM(AI2_YMD) >= :as_symd
             and TRIM(AI2_YMD) <= :as_eymd
             and AI2_PROD_TYPE IN ('F','O')) D,
         (select RPT.RPT_VALUE,RPT.RPT_SEQ_NO
            from ci.RPT
           where RPT.RPT_TXD_ID = '30751')  A,
         (select RPT.RPT_VALUE,RPT.RPT_SEQ_NO
            from ci.RPT
           where RPT.RPT_TXD_ID = '30752') B
  where trim(AI2_PARAM_KEY) = trim(A.RPT_VALUE)
    and trim(AI2_PARAM_KEY) = trim(B.RPT_VALUE)
    order by AI2_YMD";

            return db.GetDataTable(sql, parms);
        }

        public int GetColTot() {

            string sql = @"select nvl(max(RPT.RPT_SEQ_NO),0) as colTot
                                    from ci.RPT
                                    where RPT.RPT_TXD_ID = '30752'";

            DataTable re = db.GetDataTable(sql, null);

            return int.Parse(re.Rows[0]["colTot"].ToString());
        }

        public DataTable GetDayCount(string symd, string eymd) {

            object[] parms = {
                ":as_symd", symd,
                ":as_eymd", eymd
            };

            string sql = @" select AI2_YMD,
        MAX(AI2_DAY_COUNT) as CP_DAY_COUNT
        from ci.AI2
        where TRIM(AI2_YMD) >=:as_symd
        and TRIM(AI2_YMD)<= :as_eymd
        and AI2_SUM_TYPE = 'M'
         and AI2_PROD_TYPE IN ('F','O')
         group by AI2_YMD
        order by AI2_YMD";

            return db.GetDataTable(sql, parms);
        }

        public DataTable GetAI2Sum(string symd, string eymd) {
            object[] parms = {
                ":as_symd", symd,
                ":as_eymd", eymd
            };

            string sql = @" select AI2_YMD,
        AI2_PARAM_KEY,
        AI2_M_QNTY,
        AI2_DAY_COUNT,
        A.RPT_SEQ_NO as RPT_SEQ_NO,
        B.RPT_SEQ_NO as RPT_SEQ_NO_2
   from
         (select substr(AI2_YMD,1,4)  as AI2_YMD,
                 AI2_PARAM_KEY,
                 SUM(AI2_M_QNTY) as AI2_M_QNTY,
                 SUM(AI2_DAY_COUNT) as AI2_DAY_COUNT
            from ci.AI2
           where AI2_SUM_TYPE = 'M'
             and AI2_SUM_SUBTYPE = '3'
             and TRIM(AI2_YMD) >= :as_symd
             and TRIM(AI2_YMD) <= :as_eymd
             and AI2_PROD_TYPE IN ('F','O') 
           group by substr(AI2_YMD,1,4), AI2_PARAM_KEY) D,
         (select RPT.RPT_VALUE,RPT.RPT_SEQ_NO
            from ci.RPT
           where RPT.RPT_TXD_ID = '30751')  A,
         (select RPT.RPT_VALUE,RPT.RPT_SEQ_NO
            from ci.RPT
           where RPT.RPT_TXD_ID = '30752') B
  where trim(AI2_PARAM_KEY) = trim(A.RPT_VALUE)
    and trim(AI2_PARAM_KEY) = trim(B.RPT_VALUE)
    order by AI2_YMD";

            return db.GetDataTable(sql, parms);
        }
    }
}
