using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/24
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D40071: DataGate {

        public DataTable d_40071(string as_ymd, string adj_type) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":adj_type", adj_type
            };

            string sql =
@"
SELECT A.*, CASE WHEN A.mgd2_prod_subtype <>'S' THEN 0 ELSE 1 END as cpSort
FROM
    (SELECT *
    FROM ci.MGD2
    WHERE MGD2_YMD = :as_ymd
    AND MGD2_ADJ_TYPE = :adj_type) A
ORDER BY mgd2_ymd, mgd2_prod_type, cpSort, mgd2_prod_subtype, mgd2_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// for W40072 gvMain 取得table的schema，因為程式開啟會預設插入一筆空資料列
        /// </summary>
        /// <returns></returns>
        public DataTable d_40071() {

            string sql =
@"
SELECT A.MGD2_STOCK_ID AS STOCK_ID,
       A.MGD2_ADJ_RATE AS RATE,
       A.MGD2_PUB_YMD AS PUB_YMD,
       A.MGD2_IMPL_BEGIN_YMD AS IMPL_BEGIN_YMD,
       A.MGD2_IMPL_END_YMD AS IMPL_END_YMD,       
       A.MGD2_ISSUE_BEGIN_YMD AS ISSUE_BEGIN_YMD,       
       A.MGD2_ISSUE_END_YMD AS ISSUE_END_YMD,
       A.MGD2_YMD AS YMD,
       CASE WHEN A.mgd2_prod_subtype <>'S' THEN 0 ELSE 1 END as cpSort
FROM
    (SELECT *
    FROM ci.MGD2
    WHERE MGD2_YMD = ''
    AND MGD2_ADJ_TYPE = '') A
ORDER BY mgd2_ymd, mgd2_prod_type, cpSort, mgd2_prod_subtype, mgd2_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// for W40073 gvMain 取得table的schema，因為程式開啟會預設插入一筆空資料列
        /// </summary>
        /// <returns></returns>
        public DataTable d_40073() {

            string sql =
@"
SELECT A.MGD2_STOCK_ID AS STOCK_ID,
       A.MGD2_LEVEL AS M_LEVEL,
       A.MGD2_CM AS CM_A,
       A.MGD2_CM AS CM_B,
       A.MGD2_MM AS MM_A,       
       A.MGD2_MM AS MM_B,     
       A.MGD2_IM AS IM_A,
       A.MGD2_IM AS IM_B,
       A.MGD2_ADJ_RSN AS ADJ_RSN,
       CASE WHEN A.mgd2_prod_subtype <>'S' THEN 0 ELSE 1 END as cpSort
FROM
    (SELECT *
    FROM ci.MGD2
    WHERE MGD2_YMD = ''
    AND MGD2_ADJ_TYPE = '') A
ORDER BY mgd2_ymd, mgd2_prod_type, cpSort, mgd2_prod_subtype, mgd2_kind_id
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// 預設空參數是為了只取table schema
        /// </summary>
        /// <param name="as_trade_ymd"></param>
        /// <param name="as_prod_subtype"></param>
        /// <param name="as_param_key"></param>
        /// <param name="as_abroad"></param>
        /// <param name="as_kind_id"></param>
        /// <param name="as_sid"></param>
        /// <param name="as_adj_rate"></param>
        /// <returns></returns>
        public DataTable d_40071_detail(string as_trade_ymd="", string as_prod_subtype="", string as_param_key="", string as_abroad="", string as_kind_id="", string as_sid="", decimal as_adj_rate=0) {

            List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("as_trade_ymd",as_trade_ymd),
            new DbParameterEx("as_prod_subtype",as_prod_subtype),
            new DbParameterEx("as_param_key",as_param_key),
            new DbParameterEx("as_abroad",as_abroad),
            new DbParameterEx("as_kind_id",as_kind_id),
            new DbParameterEx("as_sid",as_sid),
            new DbParameterEx("as_adj_rate",as_adj_rate)
            //new DbParameterEx("RETURNPARAMETER",0)

         };

            string sql = "CI.SP_H_TXN_40071_DETL";

            return db.ExecuteStoredProcedureEx(sql, parms, true);
        }

        public DataTable d_40071_log() {

            string sql =
@"
SELECT * FROM ci.MGD2L
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public DataTable d_40071_input() {

            string sql =
@"
SELECT COD_SEQ_NO AS PROD_SEQ_NO,
       '' AS KIND_ID,
       RPT_VALUE AS PROD_SUBTYPE,
       TRIM(COD_DESC) AS PROD_SUBTYPE_NAME,
       RPT_VALUE_2 AS PARAM_KEY,
       RPT_VALUE_3 AS ABROAD,
       '' AS RATE
FROM ci.COD,ci.RPT 
WHERE COD_TXN_ID = '40071'
    AND COD_COL_ID = 'SUBTYPE'
  AND RPT_TXN_ID = '40071'
  AND RPT_TXD_ID = 'SUBTYPE'
  AND RPT_SEQ_NO = COD_SEQ_NO
ORDER BY COD_SEQ_NO
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// for RowInsert
        /// </summary>
        /// <param name="prod_seq_no"></param>
        /// <returns></returns>
        public DataTable d_40071_input(string prod_seq_no) {

            object[] parms = {
                ":prod_seq_no", prod_seq_no
            };

            string sql =
@"
SELECT COD_SEQ_NO AS PROD_SEQ_NO,
       '' AS KIND_ID,
       RPT_VALUE AS PROD_SUBTYPE,
       TRIM(COD_DESC) AS PROD_SUBTYPE_NAME,
       RPT_VALUE_2 AS PARAM_KEY,
       RPT_VALUE_3 AS ABROAD,
       '' AS RATE
FROM ci.COD,ci.RPT 
WHERE COD_TXN_ID = '40071'
    AND COD_COL_ID = 'SUBTYPE'
  AND RPT_TXN_ID = '40071'
  AND RPT_TXD_ID = 'SUBTYPE'
  AND RPT_SEQ_NO = COD_SEQ_NO
  AND COD_SEQ_NO = :prod_seq_no
ORDER BY COD_SEQ_NO
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_40071_prod_type_ddl() {

            string sql =
@"
SELECT COD_SEQ_NO AS PROD_SEQ_NO,
       TRIM(COD_DESC) AS SUBTYPE_NAME,
       RPT_VALUE AS CND_PROD_SUBTYPE,
       RPT_VALUE_2 AS CND_PARAM_KEY,
       RPT_VALUE_3 AS CND_ABROAD
FROM ci.COD,ci.RPT 
WHERE COD_TXN_ID = '40071'
    AND COD_COL_ID = 'SUBTYPE'
  AND RPT_TXN_ID = '40071'
  AND RPT_TXD_ID = 'SUBTYPE'
  AND RPT_SEQ_NO = COD_SEQ_NO
ORDER BY COD_SEQ_NO  
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// 商品下拉選單1
        /// </summary>
        /// <param name="as_ymd"></param>
        /// <param name="as_param_key"></param>
        /// <returns></returns>
        public DataTable dddw_pdk_kind_id_40071(string as_ymd, string as_param_key) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_param_key", as_param_key
            };

            string sql =
@"
SELECT PDK_KIND_ID AS KIND_ID,
       PDK_NAME,
       PDK_PROD_TYPE AS PROD_TYPE,
       PDK_PARAM_KEY AS PARAM_KEY,
       MGT2_SEQ_NO AS SEQ_NO
  FROM ci.HPDK,ci.MGT2
 WHERE PDK_DATE = TO_DATE(:as_ymd,'YYYYMMDD')
   AND PDK_SUBTYPE = 'S'
   AND PDK_PARAM_KEY LIKE :as_param_key
   AND PDK_STATUS_CODE = 'N'
   AND PDK_PARAM_KEY = MGT2_KIND_ID
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 商品下拉選單2
        /// </summary>
        /// <param name="as_prod_subtype"></param>
        /// <param name="as_abroad"></param>
        /// <returns></returns>
        public DataTable dddw_mgt2_kind(string as_prod_subtype, string as_abroad) {

            object[] parms = {
                ":as_prod_subtype", as_prod_subtype,
                ":as_abroad", as_abroad
            };

            string sql =
@"
SELECT MGT2_KIND_ID AS KIND_ID,
       MGT2_ABBR_NAME,
       MGT2_PROD_TYPE AS PROD_TYPE,
       APDK_PARAM_KEY AS PARAM_KEY,
       MGT2_SEQ_NO AS SEQ_NO
    FROM CI.MGT2,CI.APDK        
   WHERE NVL(MGT2_DATA_TYPE,' ') = ' '
     AND MGT2_PROD_SUBTYPE LIKE :as_prod_subtype
     AND MGT2_ABROAD = :as_abroad
     AND MGT2_KIND_ID = APDK_KIND_ID
ORDER BY MGT2_SEQ_NO
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 確認商品是否在同一交易日不同情境下設定過
        /// </summary>
        /// <param name="ls_kind_id"></param>
        /// <param name="ls_ymd"></param>
        /// <param name="is_adj_type"></param>
        /// <returns></returns>
        public DataTable IsSetOnSameDay(string ls_kind_id, string ls_ymd, string is_adj_type) {

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
        /// </summary>
        /// <param name="ls_kind_id"></param>
        /// <param name="ls_ymd"></param>
        /// <param name="ls_issue_begin_ymd"></param>
        /// <param name="ls_issue_end_ymd"></param>
        /// <returns></returns>
        public DataTable IsSetInSameSession(string ls_kind_id, string ls_ymd, string ls_issue_begin_ymd, string ls_issue_end_ymd) {

            object[] parms = {
                ":ls_kind_id", ls_kind_id,
                ":ls_ymd", ls_ymd,
                ":ls_issue_begin_ymd", ls_issue_begin_ymd,
                ":ls_issue_end_ymd", ls_issue_end_ymd
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
                            (MGD2_ISSUE_BEGIN_YMD  >= :ls_issue_begin_ymd and MGD2_ISSUE_BEGIN_YMD < :ls_issue_end_ymd) or
                            (MGD2_ISSUE_END_YMD > :ls_issue_begin_ymd and MGD2_ISSUE_END_YMD <=:ls_issue_end_ymd)or
                            (MGD2_ISSUE_BEGIN_YMD < :ls_issue_begin_ymd and MGD2_ISSUE_END_YMD > :ls_issue_end_ymd)                
                        )
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 確認商品是否在同一生效日區間設定過 for W40073
        /// </summary>
        /// <param name="ls_kind_id"></param>
        /// <param name="ls_ymd"></param>
        /// <param name="ls_issue_begin_ymd"></param>
        /// <param name="ls_issue_end_ymd"></param>
        /// <returns></returns>
        public DataTable IsSetInSameSession(string ls_kind_id, string ls_ymd, string ls_issue_begin_ymd) {

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

        public int DeleteMGD2(string ls_ymd, string is_adj_type) {

            object[] parms = {
                ":ls_ymd", ls_ymd,
                ":is_adj_type", is_adj_type
            };

            string sql = @"
		delete ci.MGD2
		where MGD2_YMD = :ls_ymd
		and MGD2_ADJ_TYPE = :is_adj_type
";

            return db.ExecuteSQL(sql, parms);
        }
    }
}
