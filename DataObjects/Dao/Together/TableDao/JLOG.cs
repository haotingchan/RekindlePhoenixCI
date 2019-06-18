using OnePiece;
using System;
using System.Data;

/// <summary>
/// ken 2018/12/28
/// </summary>
namespace DataObjects.Dao.Together {
    /// <summary>
    /// job log
    /// </summary>
    public class JLOG {
        private Db db;

        public JLOG() {
            db = GlobalDaoSetting.DB;
        }



        /// <summary>
        /// Get Job Count
        /// </summary>
        /// <param name="startDate">include</param>
        /// <param name="oswGroup">char(1)</param>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public int GetJobCount(DateTime startDate,string oswGroup,string jobId = "AI0130C") {
            object[] parms =
            {
                ":adt_date", startDate,
                ":ls_osw_grp", oswGroup,
                ":jobId",jobId
            };

            string sql = @"
select count(*) as li_rtn
from (
    select JLOG_WORKFLOW
    from CI.JLOG
    where JLOG_WORKFLOW in ('wf_FB_AI0130C', 'wf_OB_AI0130C')
    and TRIM(JLOG_ID) = :jobId
    and JLOG_DATE >= :adt_date
    and TRIM(JLOG_OSW_GRP) = :ls_osw_grp
    group by JLOG_WORKFLOW
)
";

            string res = db.ExecuteScalar(sql, CommandType.Text, parms);
            int.TryParse(res, out int iResult);

            return iResult;
        }

      /// <summary>
      /// 因應 where JLOG_WORKFLOW的條件不同
      /// </summary>
      /// <param name="startDate">include</param>
      /// <param name="oswGroup">char(1)</param>
      /// <param name="jobId"></param>
      /// <returns>JLOG_WORKFLOW</returns>
      public DataTable ListJobWorkflow(DateTime startDate, string oswGroup, string jobId = "AI0130C")
      {
         object[] parms =
         {
                ":adt_date", startDate,
                ":ls_osw_grp", oswGroup,
                ":jobId",jobId
            };

         string sql = @"
                  select JLOG_WORKFLOW
                  from (
                    select JLOG_WORKFLOW
                    from CI.JLOG
                    where JLOG_ID = :jobId
                    and JLOG_DATE >= :adt_date
                    and JLOG_OSW_GRP = :ls_osw_grp
                    group by JLOG_WORKFLOW
                  )
                  ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

   }
}
