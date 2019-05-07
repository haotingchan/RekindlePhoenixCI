using OnePiece;
using System.Data;

/// <summary>
/// ken 2018/12/20
/// </summary>
namespace DataObjects.Dao.Together {
   /// <summary>
   /// ABRK券商資訊?
   /// </summary>
   public class ABRK {
      private Db db;

      public ABRK() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// CI.ABRK
      /// </summary>
      /// <returns>abrk_no/abrk_name/cp_display</returns>
      public DataTable ListAll() {

         string sql = @"
SELECT abrk_no,
abrk_name,
trim(Abrk_no)||'('||trim(abrk_name)||')' as cp_display
FROM CI.ABRK
order by abrk_no";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      /// <summary>
      /// CI.ABRK
      /// </summary>
      /// <param name="ABRK_SUB_NO">ABRK_NO後三碼</param>
      /// <returns>abrk_no/abrk_name/cp_display</returns>
      public DataTable ListByNo(string ABRK_SUB_NO = "999") {
         object[] parms =
         {
                ":ABRK_NO", ABRK_SUB_NO
            };

         string sql = @"
SELECT abrk_no,
abrk_name,
trim(Abrk_no)||'('||trim(abrk_name)||')' as cp_display
FROM CI.ABRK
WHERE SUBSTR(ABRK_NO,5,3) = :ABRK_NO
order by abrk_no";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// CI.ABRK
      /// </summary>
      /// <returns>第一行空白+ABRK_NO/ABRK_NAME/cp_display/cp_display2</returns>
      public DataTable ListAll2() {

         string sql = @"
select 
a.*,
(case when trim( abrk_no ) is null then '' else abrk_no||'－'||abrk_name end) as cp_display,
(case when trim( abrk_no ) is null then '' else trim(Abrk_no)||'('||trim(abrk_name)||')' end) as cp_display2
from (
SELECT '2' as s,
        CI.ABRK.ABRK_NO,   
        CI.ABRK.ABRK_NAME  
    FROM CI.ABRK   
union
  select '1',' ',' '
    from dual
) a    
order by s,abrk_no";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }


      /// <summary>
      /// ci.AMPD, ci.ABRK
      /// </summary>
      /// <returns>ampd_fcm_no/abrk_abrk_name/cp_display</returns>
      public DataTable ListFcmNo() {

         string sql = @"
select a.ampd_fcm_no, a.abrk_abrk_name,
(case when  ampd_fcm_no = ' ' then ' ' else ampd_fcm_no || ' － ' || abrk_abrk_name  end) as cp_display
from (
    SELECT ampd_fcm_no,
         ABRK_NAME as abrk_abrk_name
    FROM ci.AMPD, ci.ABRK
    WHERE AMPD_FCM_NO = ABRK_NO
    GROUP BY AMPD_FCM_NO, ABRK_NAME
    UNION
      SELECT ' ',' '
        FROM DUAL
) a
order by ampd_fcm_no";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      /// <summary>
      /// get 1 abrk_name by abrk_no
      /// </summary>
      /// <param name="ABRK_NO">7位數</param>
      /// <returns>abrk_name</returns>
      public string GetNameByNo(string ABRK_NO) {
         object[] parms =
         {
                ":ABRK_NO", ABRK_NO
            };

         string sql = @"
SELECT trim(abrk_name) as abrk_name
FROM CI.ABRK
WHERE ABRK_NO = :ABRK_NO
";

         string res = db.ExecuteScalar(sql , CommandType.Text , parms);
         return res;
      }

      /// <summary>
      /// ci.AMPD, ci.ABRK
      /// </summary>
      /// <returns>第一行空白+ abrk_abrk_name/cp_display</returns>
      public DataTable ListFcmAccNo() {
         string sql = @"
select a.*,(case when trim( AMPD_FCM_NO ) is null then '' else AMPD_FCM_NO || '(' || trim(ABRK_NAME) || ')' end) as cp_display
   from (
   select AMPD_FCM_NO||'--'||AMPD_ACC_NO AS AMPD_FCM_NO,ABRK_NAME
from
(select AMPD_FCM_NO,AMPD_ACC_NO 
   from ci.AMPD
  group by AMPD_FCM_NO,AMPD_ACC_NO),
ci.ABRK
where AMPD_FCM_NO = ABRK_NO
union all 
select ' ',' ' from dual
) a    
order by ampd_fcm_no";

         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

   }
}

