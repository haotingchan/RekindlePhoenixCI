using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class DS0012
    {
        private Db db;

        public DS0012() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetSP1Data(string ym_date,string group_Id) {
            object[] parms = {
                "@ym_date", ym_date,
                "@group_id",group_Id
            };

            string sql = @"
                    SELECT * FROM (
                            SELECT (case when sp1_type='SV' then 'Span-VSR' when sp1_type='SD' then 'Span-Delta Per Spread Ratio' else 'Spsn-Spread Credit' end) as cp_type,
                                          (case when sp1_osw_grp = 1 then 1 when sp1_osw_grp = 5 then 2 else 3 end) as GROUP_TYPE,
                                          SP1.* 
                                          FROM (
                                                     SELECT 
                                                        SP1_DATE,   
                                                        SP1_TYPE,
                                                        SP1_KIND_ID1,  
                                                        SP1_KIND_ID2,    
                                                        round(SP1_CHANGE_RANGE * 100, 4)|| '%' as SP1_CHANGE_RANGE,   
                                                        ' ' as SP2_SPAN_CODE ,
                                                        ' 'as SP1_USER_RATE,
                                                        SP1_CHANGE_COND,
                                                        SP1_RATE,
                                                        SP1_CUR_RATE,
                                                        SP1_SEQ_NO,
                                                        NVL(APDK_PROD_TYPE,PARAM_PROD_TYPE) AS APDK_PROD_TYPE,
                                                        NVL(APDK_PROD_SUBTYPE,PARAM_PROD_SUBTYPE) AS APDK_PROD_SUBTYPE,
                                                        SP1_OSW_GRP,
                                                        CAST(null AS decimal(19,4)) as USER_RATE
                                                        FROM CI.SP1, ci.APDK, ci.APDK_PARAM
                                                        WHERE SP1_CHANGE_FLAG = 'Y'
                                                        and SP1_KIND_ID1 = APDK_KIND_ID(+)
                                                        and SP1_KIND_ID1 = PARAM_KEY(+)
                                                        ) SP1
                                                    ) P
                                                   WHERE P.SP1_DATE=to_date(:ym_date,'YYYY/MM/DD') AND GROUP_TYPE =:group_id
                                                   ORDER BY sp1_osw_grp,sp1_date,cp_type,sp1_seq_no,sp1_kind_id1,sp1_kind_id2";

            return db.GetDataTable(sql, parms);
        }

        /// <summary>
        /// Delete SP2S Current Data
        /// </summary>
        /// <returns>Execute Rows</returns>
        public int DeleteSP2S() {

            string sql = @"delete cfo.SP2S";

            return db.ExecuteSQL(sql, null);
        }

        public DataTable GetSP2SColumns() {

            string sql = @"
                                    SELECT 
                                        SP2S_DATE, 
                                        SP2S_TYPE, 
                                        SP2S_KIND_ID1, 
                                        SP2S_KIND_ID2, 
                                        SP2S_VALUE_DATE, 
                                        SP2S_W_TIME, 
                                        SP2S_W_USER_ID, 
                                        SP2S_OSW_GRP, 
                                        SP2S_SPAN_CODE, 
                                        SP2S_ADJ_CODE, 
                                        SP2S_USER_CM 
	                                    FROM CFO.SP2S";

            return db.GetDataTable(sql, null);
        }
    }
}
