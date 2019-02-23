using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/1/23
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    public class HPDK: DataGate {

        /// <summary>
        /// for d_20233
        /// </summary>
        /// <returns></returns>
        public DataTable ListAll(string as_pdk_ymd) {

            object[] parms = {
                ":as_pdk_ymd", as_pdk_ymd
            };

            string sql =
@"
SELECT A.*
FROM
    (SELECT PDK_KIND_ID2 as PLS4_KIND_ID2,
           '        ' as PLS4_YMD,
           max(PDK_FUT) as PLS4_FUT,
           max(PDK_OPT) as PLS4_OPT,     
           to_char(PDK_DATE,'yyyymmdd') as PLS4_PDK_YMD,
           sysdate as   PLS4_W_TIME,
           '          ' as PLS4_W_USER_ID,
           'I' as OP_TYPE   
    from
    (select PDK_DATE,substr(PDK_KIND_ID,1,2) as PDK_KIND_ID2,'F' as PDK_FUT,' ' as PDK_OPT from ci.HPDK
    where PDK_DATE =to_date(:as_pdk_ymd,'yyyymmdd')
      and PDK_PROD_TYPE = 'F'
      and PDK_SUBTYPE = 'S'
      and PDK_STATUS_CODE in ('N','P','1','2')
    union all
    select PDK_DATE,substr(PDK_KIND_ID,1,2),' ','O' from ci.HPDK
    where PDK_DATE =to_date(:as_pdk_ymd,'yyyymmdd')
      and PDK_PROD_TYPE = 'O'
      and PDK_SUBTYPE = 'S'
      and PDK_STATUS_CODE in ('N','P','1','2'))
    group by PDK_DATE,PDK_KIND_ID2) A
order by pls4_kind_id2
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
