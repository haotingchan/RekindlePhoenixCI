using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class DS0011
    {
        private Db db;

        public DS0011() {
            db = GlobalDaoSetting.DB;
        }

        public DataTable GetMG1Data(string ym_date, string group_Id) {
            object[] parms = {
                "@ym_date", ym_date,
                "@group_id",group_Id
            };

            string sql = @"
                                SELECT * FROM (
                                            SELECT (case when MG1_osw_grp = 1 then 1 when MG1_osw_grp = 5 then 2 else 3 end)as GROUP_TYPE,
                                            MG1.* 
                                            FROM (
                                                        SELECT MG1_DATE,   
                                                        MG1_PROD_TYPE,
                                                        MG1_KIND_ID,   
                                                        round(MG1_CHANGE_RANGE * 100, 4)|| '%' as MG1_CHANGE_RANGE,   
                                                        ' ' as MG2_SPAN_CODE  , 
                                                        null as MG2_VALUE_DATE, 
                                                        MG1_CHANGE_COND,
                                                        MG1_CM,
                                                        MG1_CUR_CM,
                                                        MG1_SEQ_NO,
                                                        MG1_PROD_SUBTYPE,
                                                        MG1_OSW_GRP,
                                                        CAST(null AS decimal(19,4)) as USER_CM
                                                        FROM CI.MG1
                                                        WHERE MG1_CHANGE_FLAG = 'Y'
                                                        and (MG1_TYPE = '-' OR MG1_TYPE = 'A') 
                                                        ) MG1
                                                    ) MG
                                                    WHERE MG.MG1_DATE = to_date(:ym_date,'YYYY/MM/DD') and GROUP_TYPE = :group_id
                                                    ORDER BY MG1_OSW_GRP,MG1_SEQ_NO ,MG1_PROD_TYPE , MG1_KIND_ID";

            return db.GetDataTable(sql, parms);
        }

        /// <summary>
        /// Delete SP2S Current Data
        /// </summary>
        /// <returns>Execute Rows</returns>
        public int DeleteMG2S() {

            string sql = @"delete cfo.MG2S";

            return db.ExecuteSQL(sql, null);
        }

        public DataTable GetMG2SColumns() {

            string sql = @"
                                    SELECT 
                                        MG2S_DATE, 
                                        MG2S_KIND_ID, 
                                        MG2S_VALUE_DATE, 
                                        MG2S_W_TIME, 
                                        MG2S_W_USER_ID, 
                                        MG2S_OSW_GRP, 
                                        MG2S_SPAN_CODE, 
                                        MG2S_ADJ_CODE, 
                                        MG2S_USER_CM 
                                        FROM CFO.MG2S";

            return db.GetDataTable(sql, null);
        }
    }
}
