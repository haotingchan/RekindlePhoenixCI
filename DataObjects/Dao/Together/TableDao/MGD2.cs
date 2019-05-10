using BusinessObjects;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/24
/// </summary>
namespace DataObjects.Dao.Together.TableDao {
    public class MGD2 : DataGate {

        /// <summary>
        /// 確認商品是否在同一交易日不同情境下設定過
        /// </summary>
        /// <param name="ls_kind_id"></param>
        /// <param name="ls_ymd"></param>
        /// <param name="is_adj_type"></param>
        /// <returns></returns>
        public DataTable IsProdSetOnSameDay(string ls_kind_id, string ls_ymd, string is_adj_type) {

            object[] parms = {
                ":ls_kind_id", ls_kind_id,
                ":ls_ymd", ls_ymd,
                ":is_adj_type", is_adj_type
            };

            string sql =
@"
select COUNT(*) as li_count,
                        MAX(
                            case MGD2_ADJ_TYPE 
                                when '0' then '一般' 
                                when '1' then '長假' 
                                when '2' then '處置股票'  
                                when '3' then '股票'
                                when '4' then '上下市商品'
                            end 
                        )AS ls_adj_type_name
                from ci.MGD2
                where MGD2_KIND_ID = :ls_kind_id
                and MGD2_YMD = :ls_ymd
                and MGD2_ADJ_CODE = 'Y'
                and MGD2_ADJ_TYPE <> :is_adj_type
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 確認商品是否在同一生效日區間設定過
        /// 生效起日若與生效迄日相同，不重疊
        /// 10/11的至10/31一般交易時段結束止，10/30的從10/31一般交易時段結束後始>>應不重疊
        /// </summary>
        /// <param name="ls_kind_id"></param>
        /// <param name="ls_ymd"></param>
        /// <param name="ls_issue_begin_ymd"></param>
        /// <returns></returns>
        public DataTable IsProdSetInSameInterval(string ls_kind_id, string ls_ymd, string ls_issue_begin_ymd) {

            object[] parms = {
                ":ls_kind_id", ls_kind_id,
                ":ls_ymd", ls_ymd,
                ":ls_issue_begin_ymd", ls_issue_begin_ymd
            };

            string sql =
@"
select COUNT(*) as li_count,
                        MAX(
                            case MGD2_ADJ_TYPE 
                                when '0' then '一般' 
                                when '1' then '長假' 
                                when '2' then '處置股票'  
                                when '3' then '股票' 
                                when '4' then '上下市商品'
                            end 
                        )AS ls_adj_type_name,
                        MAX(MGD2_YMD) as ls_trade_ymd
                from ci.MGD2
                where MGD2_KIND_ID = :ls_kind_id
                and MGD2_ADJ_CODE = 'Y'    
                and MGD2_YMD <> :ls_ymd
                and(
                    (MGD2_ISSUE_BEGIN_YMD  = :ls_issue_begin_ymd) or
                    (MGD2_ISSUE_BEGIN_YMD < :ls_issue_begin_ymd and MGD2_ISSUE_END_YMD > :ls_issue_begin_ymd)                
                )
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 刪除已存在資料
        /// </summary>
        /// <param name="ls_ymd"></param>
        /// <param name="is_adj_type"></param>
        /// <param name="ls_kind_id"></param>
        /// <returns></returns>
        public int DeleteMGD2(string ls_ymd, string is_adj_type, string ls_kind_id) {

            object[] parms = {
                ":ls_ymd", ls_ymd,
                ":is_adj_type", is_adj_type,
                ":ls_kind_id", ls_kind_id
            };

            string sql = @"
delete ci.MGD2
    where MGD2_YMD = :ls_ymd
      and MGD2_ADJ_TYPE = :is_adj_type   
      and MGD2_KIND_ID = :ls_kind_id
";

            return db.ExecuteSQL(sql, parms);
        }

        public int DeleteMGD2(string ls_ymd, string is_adj_type, string ls_stock_id, string ls_kind_id) {

            object[] parms = {
                ":ls_ymd", ls_ymd,
                ":is_adj_type", is_adj_type,
                ":ls_stock_id", ls_stock_id,
                ":ls_kind_id", ls_kind_id
            };

            string sql = @"
				delete ci.MGD2
				where MGD2_YMD = :ls_ymd
				and MGD2_ADJ_TYPE = :is_adj_type 
				and MGD2_STOCK_ID = :ls_stock_id
				and MGD2_KIND_ID = :ls_kind_id
";

            return db.ExecuteSQL(sql, parms);
        }

        /// <summary>
        /// 判斷是否重新塞入新資料
        /// </summary>
        /// <param name="ls_ymd"></param>
        /// <param name="is_adj_type"></param>
        /// <param name="ls_kind_id"></param>
        /// <returns></returns>
        public int IsInsertNeeded(string ls_ymd, string is_adj_type, string ls_kind_id) {

            int ld_value = 0;
            object[] parms = {
                ":ls_ymd", ls_ymd,
                ":is_adj_type", is_adj_type,
                ":ls_kind_id", ls_kind_id
            };

            string sql =
@"
select count(*) as li_count
		from ci.MGD2
		where MGD2_YMD = :ls_ymd
		and MGD2_ADJ_TYPE = :is_adj_type   
		and MGD2_KIND_ID = :ls_kind_id";

            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count == 0) {
                return ld_value;
            }
            else {
                ld_value = dtResult.Rows[0]["LI_COUNT"].AsInt();
                return ld_value;
            }

        }

        public ResultData UpdateMGD2(DataTable inputData) {
            string sql = @"
SELECT 
    MGD2_YMD,            
    MGD2_PROD_TYPE,      
    MGD2_KIND_ID,     
    MGD2_STOCK_ID,       
    MGD2_ADJ_TYPE,

    MGD2_ADJ_RATE,       
    MGD2_ADJ_CODE,       
    MGD2_ISSUE_BEGIN_YMD,
    MGD2_ISSUE_END_YMD,  
    MGD2_IMPL_BEGIN_YMD,

    MGD2_IMPL_END_YMD,   
    MGD2_PUB_YMD,        
    MGD2_PROD_SUBTYPE,   
    MGD2_PARAM_KEY,      
    MGD2_AB_TYPE,

    MGD2_AMT_TYPE,       
    MGD2_CUR_CM,         
    MGD2_CUR_MM,         
    MGD2_CUR_IM,         
    MGD2_CUR_LEVEL,

    MGD2_CM,             
    MGD2_MM,             
    MGD2_IM,            
    MGD2_LEVEL,          
    MGD2_CURRENCY_TYPE,

    MGD2_SEQ_NO,         
    MGD2_OSW_GRP,        
    MGD2_W_TIME,         
    MGD2_W_USER_ID,      
    MGD2_ADJ_RSN        
  
FROM CI.MGD2";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
