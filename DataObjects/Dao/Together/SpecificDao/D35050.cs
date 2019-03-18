using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D35050 {
        private Db db;

        public D35050() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetTseOtcData(DateTime ymd) {

            object[] parms = {
                ":as_date", ymd
            };

            string sql = @"select TSE3_SID,TRIM(TFXMS_SNAME) AS TFXMS_SNAME,
       case MAX(case when TSE3_IDSTK_INDEX = '0000' then '0000' else TSE3_IDSTK_INDEX end)
            when '13'   then '上市-電子'   
            when '17'   then '上市-金融'
            when 'A07'  then '上市-非金電'
            when '4000' then '上櫃' end as IDX,
       TSE3_SHARES,TFXM1_SFD_FPR as TSE3_CLOSE_PRICE,
       sum(case when TSE3_IDSTK_INDEX = '0000' then TSE3_INDEX_WEIGHT else 0 end) AS IDX_TXO,
       sum(case when TSE3_IDSTK_INDEX = '13' then TSE3_INDEX_WEIGHT else 0 end) AS IDX_TEO,
       sum(case when TSE3_IDSTK_INDEX = '17' then TSE3_INDEX_WEIGHT else 0 end) AS IDX_TFO,
       sum(case when TSE3_IDSTK_INDEX = 'A07' then TSE3_INDEX_WEIGHT else 0 end) AS IDX_XIO,
       sum(case when TSE3_IDSTK_INDEX = '4000' then TSE3_INDEX_WEIGHT else 0 end) AS IDX_GTO
from ci.TSE3 TSE3,ci.TFXM1, 
     (SELECT TFXMS_SID,SUBSTR(MIN(TFXM2_PID||TFXMS_SNAME),2,30) AS TFXMS_SNAME
      FROM
     (SELECT '1' AS TFXM2_PID,TFXMS_SID,TFXMS_SNAME FROM ci.TFXMS
      UNION
      SELECT '2' AS TFXM2_PID,TFXMS_SID,TFXMS_SNAME FROM ci.TFXMS2)
      GROUP BY TFXMS_SID)
where TSE3_DATE = :as_date
  and TSE3_SID = TFXMS_SID(+)
  and TSE3_IDSTK_INDEX in ('0000','13','17','A07','4000')
  and TSE3_DATE = TFXM1_DATE(+)
  and TSE3_SID = TFXM1_SID(+)
GROUP BY TSE3_SID,TFXMS_SNAME,TSE3_SHARES,TFXM1_SFD_FPR
ORDER BY TSE3_SID ";

            return db.GetDataTable(sql, parms);
        }

        public DataTable GetTW50Data(DateTime ymd) {

            object[] parms = {
                ":as_date", ymd
            };

            string sql = @"select TSE3_SID,TFXMS_SNAME,
       TSE3_SHARES,TSE3_TW50_FACTOR,TFXM1_SFD_FPR as TSE3_CLOSE_PRICE,
       TSE3_INDEX_WEIGHT
from ci.TSE3 TSE3,ci.TFXMS TFXMS,ci.TFXM1
where TSE3_DATE = :as_date
  and TSE3_SID = TFXMS_SID(+)
  and TSE3_IDSTK_INDEX = 'TW50'
  and TSE3_DATE = TFXM1_DATE(+)
  and TSE3_SID = TFXM1_SID(+)
  ORDER BY TSE3_SID ";

            return db.GetDataTable(sql, parms);
        }
    }
}
