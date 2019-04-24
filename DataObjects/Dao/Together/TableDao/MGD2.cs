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
    public class MGD2: DataGate {

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
    }
}
