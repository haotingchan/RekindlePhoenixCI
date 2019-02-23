using OnePiece;
using System;
using System.Data;

/// <summary>
/// lukas,2019/1/14
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    /// <summary>
    /// 路透人民幣商品成交量資料維護
    /// </summary>
    public class D20130 {

        private Db db;

        public D20130() {

            db = GlobalDaoSetting.DB;

        }

        /// <summary>
        /// AM12
        /// </summary>
        /// <param name="as_fm_ymd">起始日期yyyyMMdd</param>
        /// <param name="as_to_ymd">結束日期yyyyMMdd</param>
        /// <returns></returns>
        public DataTable ListAll(string as_fm_ymd, string as_to_ymd) {

            object[] parms = {
                ":as_fm_ymd", as_fm_ymd,
                ":as_to_ymd", as_to_ymd
            };

            string sql = @"
SELECT AM12_YMD,   
	trim(AM12_F_ID) as AM12_F_ID,
	AM12_VOL,   
	' '  as OP_TYPE ,
	AM12_W_TIME,
	AM12_W_USER_ID,
	AM12_DATA_TYPE,
	AM12_KIND_ID,
	sysdate as AM12_UPD_TIME,
	(case when am12_data_type = 'T' then to_char(am12_w_time,'yyyy/mm/dd HH24:mi:ss')||' 當日檔資料'
	     when am12_data_type='H' then to_char(am12_w_time,'yyyy/mm/dd HH24:mi:ss')||' 歷史檔資料'
	     else to_char(am12_w_time,'yyyy/mm/dd HH24:mi:ss')||' 使用者'||TRIM(am12_w_user_id)||'變更資料' end) as AM12_STATUS
FROM ci.AM12
WHERE AM12_YMD >= :as_fm_ymd
AND AM12_YMD <= :as_to_ymd
order by am12_ymd , am12_f_id";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public bool InsertAM12L(string am12_ymd, string am12_f_id, string am12_kind_id) {

            object[] parms =
            {
                ":am12_ymd",am12_ymd,
                ":am12_f_id",am12_f_id,
                ":am12_kind_id",am12_kind_id
            };

            string sql = @"
                                    INSERT INTO ci.AM12L
                                    SELECT AM12_YMD, AM12_F_ID, AM12_KIND_ID, AM12_VOL, AM12_W_TIME, AM12_W_USER_ID, AM12_DATA_TYPE, sysdate as AM12_UPD_TIME
                                    FROM ci.AM12
                                    WHERE am12_ymd = :am12_ymd
                                      and am12_f_id = :am12_f_id 
                                      and am12_kind_id = :am12_kind_id
                                ";

            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0) {
                return true;
            }
            else {
                throw new Exception("歷史檔AM12L更新失敗");
            }
        }

    }
}
