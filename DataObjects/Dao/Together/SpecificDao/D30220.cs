using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/3/12
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D30220:DataGate {

        public DataTable d_30220(string as_cp_ymd, string as_q_sym, string as_q_eym, string as_stkout_ymd) {

            object[] parms = {
                ":as_cp_ymd", as_cp_ymd,
                ":as_q_sym", as_q_sym,
                ":as_q_eym", as_q_eym,
                ":as_stkout_ymd", as_stkout_ymd
            };

            string sql =
@"
select PLS4_YMD,APDK_KIND_ID,NVL(APDK_NAME,trim(TFXMS_SNAME)) as APDK_NAME,PLS4_KIND_ID2,PLS4_FUT,PLS4_OPT,PLS4_SID AS APDK_STOCK_ID,nvl(STKOUT_B,0) as STKOUT_B,
                    PLS3_YM1,PLS3_YM2,PLS3_YM3,
                    PLS3_TOT_QNTY1,PLS3_TOT_QNTY2,PLS3_TOT_QNTY3
               from ci.PLS4,ci.APDK,
                   (select PLS3_SID,
                           MAX(case when seq_no = 1   then PLS3_YM else '' end) as PLS3_YM1 ,
                           MAX(case when seq_no = 2   then PLS3_YM else '' end) as PLS3_YM2 ,
                           MAX(case when seq_no = 3   then PLS3_YM else '' end) as PLS3_YM3 ,
                           sum(case when seq_no = 1  then PLS3_QNTY else 0 end) as PLS3_TOT_QNTY1 ,
                           sum(case when seq_no = 2  then PLS3_QNTY else 0 end) as PLS3_TOT_QNTY2 ,
                           sum(case when seq_no = 3  then PLS3_QNTY else 0 end) as PLS3_TOT_QNTY3 
                      from ci.PLS3,
                          (select PLS3_YM AS DT_YM,
                                  ROW_NUMBER( ) OVER (PARTITION BY ' ' ORDER BY PLS3_YM NULLS LAST) as seq_no 
                             from ci.PLS3
                            where PLS3_YM >= :as_q_sym
                              and PLS3_YM <= :as_q_eym
                            group by PLS3_YM)
                     where PLS3_YM >= :as_q_sym
                       and PLS3_YM <= :as_q_eym
                       and PLS3_YM = DT_YM
                     group by PLS3_SID) Q,
                    --在外流流通股
                   (select STKOUT_ID,MAX(STKOUT_B) AS STKOUT_B
                     from CI.STKOUT
                    where STKOUT_DATE = :as_stkout_ymd
                    group by STKOUT_ID) S,
                    ci.TFXMS
              where PLS4_YMD = :as_cp_ymd
                and trim(PLS4_KIND_ID2)||case when PLS4_FUT = 'F' then 'F' else PLS4_OPT end = trim(APDK_KIND_ID(+))
                and PLS4_SID = PLS3_SID(+)
                and PLS4_SID = STKOUT_ID(+)
                and PLS4_SID = TFXMS_SID(+)
              order by pls4_kind_id2
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
