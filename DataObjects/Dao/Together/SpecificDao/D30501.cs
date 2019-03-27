using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D30501 {
        private Db db;

        public D30501() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListData(string symd, string eymd) {

            object[] parms = {
                ":adt_sdate", symd,
                ":adt_edate", eymd
            };

            string sql = @"SELECT TRIM(D.BST1_KIND_ID) AS PROD_ID,TRIM(APDK_NAME) as APDK_NAME,
         ROUND(CASE WHEN NVL(TOT_B_SEC,0) = 0 THEN 0 ELSE B_SEC/TOT_B_SEC END,2) AS B_WEIGHT_QNTY,
         ROUND(CASE WHEN NVL(TOT_S_SEC,0) = 0 THEN 0 ELSE S_SEC/TOT_S_SEC END,2) AS S_WEIGHT_QNTY,
         NVL(B_MAX_QNTY,0) AS B_MAX_QNTY, NVL(B.QNTY_B_SEC,0) AS B_MAX_SEC,
         NVL(S_MAX_QNTY,0) AS S_MAX_QNTY, NVL(S.QNTY_S_SEC,0) AS S_MAX_SEC
    FROM CI.APDK, 
 (SELECT BST1_KIND_ID,
         SUM(BST1_B_QNTY_WEIGHT) AS B_SEC,SUM(BST1_B_TOT_SEC) AS TOT_B_SEC,
         SUM(BST1_S_QNTY_WEIGHT) AS S_SEC,SUM(BST1_S_TOT_SEC) AS TOT_S_SEC,
         MAX(BST1_B_MAX_QNTY) AS B_MAX_QNTY, SUM(BST1_B_MAX_SEC) AS B_MAX_SEC,
         MAX(BST1_S_MAX_QNTY) AS S_MAX_QNTY, SUM(BST1_S_MAX_SEC) AS S_MAX_SEC
	  FROM CI.BST1
   WHERE BST1_YMD >=:ADT_SDATE 
     AND BST1_YMD <=:ADT_EDATE
   GROUP BY BST1_KIND_ID) D,
 (SELECT BST1_KIND_ID,BST1_B_MAX_QNTY,SUM(BST1_B_MAX_SEC) AS QNTY_B_SEC
	 FROM CI.BST1
   WHERE BST1_YMD >=:ADT_SDATE 
     AND BST1_YMD <=:ADT_EDATE 
   GROUP BY BST1_KIND_ID,BST1_B_MAX_QNTY) B,
 (SELECT BST1_KIND_ID,BST1_S_MAX_QNTY,SUM(BST1_S_MAX_SEC) AS QNTY_S_SEC
	 FROM CI.BST1
   WHERE BST1_YMD >=:ADT_SDATE 
     AND BST1_YMD <=:ADT_EDATE
   GROUP BY BST1_KIND_ID,BST1_S_MAX_QNTY) S 
   WHERE D.BST1_KIND_ID = APDK_KIND_ID(+)
     AND D.BST1_KIND_ID  = B.BST1_KIND_ID(+)
     AND D.B_MAX_QNTY = B.BST1_B_MAX_QNTY(+)
     AND D.BST1_KIND_ID  = S.BST1_KIND_ID(+)
     AND D.S_MAX_QNTY = S.BST1_S_MAX_QNTY(+)";

            return db.GetDataTable(sql, parms);
        }
    }
}
