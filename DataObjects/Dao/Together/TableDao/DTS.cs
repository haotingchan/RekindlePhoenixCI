using OnePiece;
using System;
using System.Data;

/// <summary>
/// ken 2018/12/27
/// </summary>
namespace DataObjects.Dao.Together {
    /// <summary>
    /// 例假日
    /// </summary>
    public class DTS {
        private Db db;

        public DTS() {
            db = GlobalDaoSetting.DB;
        }

        /// <summary>
        /// 該月份額外減少的交易日 = 輸出颱風天數-假日補上班天數
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>substractDay</returns>
        public int GetSubstractDay(DateTime startDate, DateTime endDate) {
            //DTS_DATE_TYPE: W=上班日,H = 假日
            //DTS_WORK: Y=有上班,N = 沒上班
            object[] parms =
           {
                ":ldt_fm_date", startDate,
                ":ldt_to_date", endDate,
            };

            //颱風天 = 上班日W(星期一～五) & 不交易N
            //假日補上班 = 假日H(星期六～日) & 交易Y
            string sql = @"
SELECT nvl(sum(case when DTS_DATE_TYPE = 'W' and DTS_WORK = 'N' then 1 else 0 end) -
           sum(case when DTS_DATE_TYPE = 'H' and DTS_WORK = 'Y' then 1 else 0 end), 0) as substractDay
FROM CI.DTS
where DTS_DATE >= :ldt_fm_date
and DTS_DATE <  :ldt_to_date
";
            string res = db.ExecuteScalar(sql, CommandType.Text, parms);
            int.TryParse(res, out int substractDay);

            return substractDay;
        }


        /// <summary>
        /// 將起始日往後30天內的工作日列表,主要用來計算T+2
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns>DTS_DATE/IsWorkDay</returns>
        public DataTable ListWorkDay(DateTime startDate) {
            //DTS_DATE_TYPE: W=上班日,H = 假日
            //DTS_WORK: Y=有上班,N = 沒上班
            object[] parms =
            {
                ":ldt_fm_date", startDate.ToString("yyyy/MM/dd")
            };

            //颱風天 = 上班日W(星期一～五) & 不交易N
            //假日補上班 = 假日H(星期六～日) & 交易Y
            string sql = @"
SELECT  DTS_DATE,
(case when DTS_DATE_TYPE = 'H' and DTS_WORK = 'Y' then 1 
      when DTS_DATE_TYPE = 'W' and DTS_WORK = 'Y' then 1 else 0 end) as IsWorkDay
from dts
where DTS_DATE >= to_date(:ldt_fm_date,'yyyy/mm/dd')
and DTS_DATE < to_date(:ldt_fm_date,'yyyy/mm/dd')+30
";

            return db.GetDataTable(sql, parms);

        }


        /// <summary>
        /// List all DTS data where date between startDate and endDate
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>OP_TYPE/dts_date/dts_date_type/dts_work</returns>
        public DataTable ListAll(DateTime startDate, DateTime endDate) {
            //DTS_DATE_TYPE: W=上班日,H = 假日
            //DTS_WORK: Y=有上班,N = 沒上班
            object[] parms =
           {
                ":adt_sdate", startDate,
                ":adt_edate", endDate,
            };

            //颱風天 = 上班日dts_date_type=W(星期一～五) & 不交易dts_work=N
            //假日補上班 = 假日dts_date_type=H(星期六～日) & 交易dts_work=Y
            string sql = @"
SELECT  ' ' as OP_TYPE,
dts_date,
dts_date_type,
dts_work
from ci.dts
where DTS_DATE >= :adt_sdate
and DTS_DATE <= :adt_edate
";
            return db.GetDataTable(sql, parms);

        }

    }
}
