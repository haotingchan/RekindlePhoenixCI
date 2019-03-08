using OnePiece;
using System;
using System.Data;

/// <summary>
/// ken 2018/12/27
/// </summary>
namespace DataObjects.Dao.Together {
    /// <summary>
    /// AI5券商資訊?
    /// </summary>
    public class AI5 {
        private Db db;

        public AI5() {
            db = GlobalDaoSetting.DB;
        }



      /// <summary>
      /// Get 1 AI5_name by AI5_no (取得AI5最大日期)
      /// f_get_ai5_date(datetime(date(em_edate.text)))
      /// </summary>
      /// <param name="AI5_NO">7位數</param>
      /// <returns>AI5_name</returns>
      public string GetNameByNo(DateTime ldt_date) {
            object[] parms =
            {
                ":ldt_date", ldt_date
            };

            string sql = @"
SELECT MAX(AI5_DATE) as ldt_ai5_date
FROM ci.AI5
WHERE AI5_DATE <= :ldt_date
AND AI5_DATE >=  fut.DATE_DIFF_OCF_DAYS(:ldt_date,-30)
";

            string res = db.ExecuteScalar(sql, CommandType.Text, parms);
            return res;
        }

    }
}
