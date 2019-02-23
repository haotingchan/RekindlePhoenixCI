using OnePiece;
using System.Data;

/// <summary>
/// ken,2019/2/19
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    /// <summary>
    /// 上市、上櫃受益憑證原始資料查詢
    /// </summary>
    public class D43032 {

        private Db db;

        public D43032() {

            db = GlobalDaoSetting.DB;

        }

        /// <summary>
        /// get future detail data , return 19 fields
        /// </summary>
        /// <param name="as_sid"></param>
        /// <param name="as_symd"></param>
        /// <param name="as_eymd"></param>
        /// <param name="as_kind_id"></param>
        /// <param name="as_prod_type">F / O  (期貨/選擇權)</param>
        /// <returns></returns>
        protected DataTable ListDetail(string as_sid,
                                        string as_symd,
                                        string as_eymd,
                                        string as_kind_id,
                                        string as_prod_type = "F") {

            object[] parms = {
                ":as_sid", as_sid,
                ":as_symd", as_symd,
                ":as_eymd", as_eymd,
                ":as_kind_id", as_kind_id,
                ":as_prod_type", as_prod_type
            };


            string sql = @"
SELECT NVL(MG6_DATE,MGR2_DATE) AS DATA_DATE,  
         MG6_SETTLE_PRICE, 
         NVL(MG6_PRICE,MGR2_CLOSE_PRICE) AS DATA_PRICE,   
         NVL(MG6_OPEN_REF,MGR2_OPEN_REF) AS DATA_OPEN_REF , 
         ABS(NVL(MG6_UP_DOWN, MGR2_CLOSE_PRICE - MGR2_OPEN_REF)) AS DATA_UP_DOWN,    

         NVL(MG6_RETURN_RATE,MGR2_RETURN_RATE) AS DATA_RETURN_RATE, 
         NVL(MG6_30,MGR2_30_RATE) AS DATA_30, 
         NVL(MG6_60,MGR2_60_RATE) AS DATA_60, 
         NVL(MG6_90,MGR2_90_RATE) AS DATA_90,   
         NVL(MG6_180,MGR2_180_RATE) AS DATA_180,   

         NVL(MG6_CP_RISK,MGR2_CP_RATE) AS DATA_CP_RISK,   
         NVL(MG6_RISK,MGR2_DAY_RATE) AS DATA_RISK,  
         MG6_KIND_ID,
         MG6_CUR_CM,    
         MG6_CP_CM,    

         MG6_CHANGE_RANGE,   
         MG6_ADJ_CM,   
         MG6_ADJ_RANGE,
         MGR2_DATA_FLAG
    FROM ci.MG6,
        (SELECT MGR2_SID,
                TO_DATE(MGR2_YMD,'YYYYMMDD') as MGR2_DATE,
                case when NVL(MGR2_STATUS_CODE,' ') <>'N' then '*' else ' ' end as MGR2_DATA_FLAG,  
                MGR2_CLOSE_PRICE,
                MGR2_OPEN_REF,  
                case when MGR2_CLOSE_PRICE = 0 then 0 else MGR2_CLOSE_PRICE - MGR2_OPEN_REF end as MGR2_UP_DOWN,    
                MGR2_RETURN_RATE,   
                MGR2_30_RATE,    
                MGR2_60_RATE,   
                MGR2_90_RATE,    
                MGR2_180_RATE,   
                MGR2_CP_RATE, 
                MGR2_DAY_RATE 
           FROM ci.MGR2 
          WHERE MGR2_SID = :as_sid
            and MGR2_YMD >= :as_symd
            and MGR2_YMD <= :as_eymd) R
   WHERE MGR2_SID = MG6_STOCK_ID(+) 
     AND MGR2_DATE = MG6_DATE(+)      
     AND MG6_PROD_TYPE(+) = :as_prod_type
     AND MG6_KIND_ID(+) = :as_kind_id
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            DataView dvTemp = dtResult.AsDataView();
            dvTemp.Sort = " data_date desc ";

            return dvTemp.ToTable();
        }

        /// <summary>
        /// get future data , return 9 fields
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <param name="as_abs">-1 or 0 </param>
        /// <returns></returns>
        public DataTable d_43032_future(string as_ymd, int as_abs = -1) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_abs", as_abs
            };


            string sql = @"
SELECT MG6_DATE as DATA_DATE,
         MG6_KIND_ID as KIND_ID,  
         APDK_NAME as K_NAME,  
         APDK_STOCK_ID as STOCK_ID, 
         PID_NAME as P_NAME,
         MG6_CP_RISK as CP_RISK,  
         MG6_RISK as DAY_RISK,   
         MG6_MIN_RISK as MIN_RISK,
         MG6_CP_CM as CP_CM
    FROM ci.MG6,
         ci.APDK,
         --上市/上櫃中文名稱
         (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM')   
   WHERE MG6_DATE = TO_DATE(:as_ymd,'YYYYMMDD') 
     AND MG6_PARAM_KEY = 'ETF'
     AND ABS(MG6_CUR_CM - MG6_ADJ_CM) > :as_abs 
     AND MG6_KIND_ID = APDK_KIND_ID
     AND APDK_UNDERLYING_MARKET = COD_ID
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            return dtResult;
        }

        /// <summary>
        /// get future detail data , return 19 fields
        /// </summary>
        /// <param name="as_sid"></param>
        /// <param name="as_symd"></param>
        /// <param name="as_eymd"></param>
        /// <param name="as_kind_id"></param>
        /// <returns></returns>
        public DataTable d_43032_future_detail(string as_sid,
                                                string as_symd,
                                                string as_eymd,
                                                string as_kind_id) {

            return ListDetail(as_sid, as_symd, as_eymd, as_kind_id, "F");

        }

        /// <summary>
        /// get option data , return 9 fields
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <param name="as_abs">-1 or 0 </param>
        /// <returns></returns>
        public DataTable d_43032_option(string as_ymd, int as_abs = -1) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_abs", as_abs
            };


            string sql = @"
SELECT MG6_DATE as DATA_DATE,
         MG6_KIND_ID as KIND_ID,  
         APDK_NAME as K_NAME,  
         APDK_STOCK_ID as STOCK_ID, 
         PID_NAME as P_NAME,
         MG6_CP_RISK as CP_RISK,  
         MG6_RISK as DAY_RISK,   
         MG6_MIN_RISK as MIN_RISK,
         MG6_CP_CM as CP_CM,
         MG1_CP_CM_B as CP_CM_B
    FROM ci.MG6,
         ci.APDK,
         --B值
        (SELECT MG1_KIND_ID,MG1_CP_CM  as MG1_CP_CM_B
           FROM ci.MG1
          WHERE MG1_DATE = TO_DATE(:as_ymd,'YYYYMMDD') 
            AND MG1_PARAM_KEY = 'ETC'
            AND MG1_TYPE = 'B'),
         --上市/上櫃中文名稱
         (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM')   
   WHERE MG6_DATE = TO_DATE(:as_ymd,'YYYYMMDD') 
     AND MG6_PARAM_KEY = 'ETC'
     AND ABS(MG6_CUR_CM - MG6_ADJ_CM) > :as_asb
     AND MG6_KIND_ID = APDK_KIND_ID
     AND APDK_UNDERLYING_MARKET = COD_ID  
     AND MG6_KIND_ID = MG1_KIND_ID
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            return dtResult;
        }

        /// <summary>
        /// get option detail data , return 19 fields
        /// </summary>
        /// <param name="as_sid"></param>
        /// <param name="as_symd"></param>
        /// <param name="as_eymd"></param>
        /// <param name="as_kind_id"></param>
        /// <returns></returns>
        public DataTable d_43032_option_detail(string as_sid,
                                                string as_symd,
                                                string as_eymd,
                                                string as_kind_id) {

            return ListDetail(as_sid, as_symd, as_eymd, as_kind_id, "O");

        }

        /// <summary>
        /// get stock data , return 7 fields
        /// </summary>
        /// <param name="as_ymd">yyyyMMdd</param>
        /// <param name="as_sid">證券ID</param>
        /// <returns></returns>
        public DataTable d_43032_stock(string as_ymd, string as_sid) {

            object[] parms = {
                ":as_ymd", as_ymd,
                ":as_sid", as_sid
            };


            string sql = @"
select to_date(mgr5e_ymd,'YYYYMMDD') as data_date,
         tfxms_sname as s_name,
         mgr5e_sid as stock_id, 
         pid_name as p_name,
         mgr5e_cp_risk as cp_risk,  
         mgr5e_risk as day_risk,   
         mgr5e_min_risk as min_risk
    from ci.mgr5e,
         --股票名稱   
         ci.tfxms,
         --上市/上櫃中文名稱
         (select trim(cod_id) as cod_id,trim(cod_desc) as pid_name from ci.cod where cod_txn_id = 'TFXM')   
   where mgr5e_ymd = :as_ymd
     and mgr5e_sid = :as_sid
     and mgr5e_sid = tfxms_sid 
     and mgr5e_pid = cod_id 
     and mgr5e_ab_type = 'A'
";

            DataTable dtResult = db.GetDataTable(sql, parms);
            return dtResult;
        }

        /// <summary>
        /// get stock detail data , return 12 fields
        /// </summary>
        /// <param name="as_sid">證券ID</param>
        /// <param name="as_symd">yyyyMMdd</param>
        /// <param name="as_eymd">yyyyMMdd</param>
        /// <returns></returns>
        public DataTable d_43032_stock_detail(string as_sid, string as_symd, string as_eymd) {

            object[] parms = {
                ":as_sid", as_sid,
                ":as_symd", as_symd,
                ":as_eymd", as_eymd
            };
            
            string sql = @"
select to_date(mgr2_ymd,'YYYYMMDD') as mgr2_date,   
         mgr2_close_price,
         mgr2_open_ref,  
         (case when mgr2_close_price = 0 then 0 else mgr2_close_price - mgr2_open_ref end) as mgr2_up_down,    
         abs(mgr2_return_rate) as mgr2_return_rate,   

         mgr2_30_rate,    
         mgr2_60_rate,   
         mgr2_90_rate,    
         mgr2_180_rate,   
         mgr2_cp_rate, 

         mgr2_day_rate,
         (case when nvl(mgr2_status_code,' ') <>'N' then '*' else ' ' end) as mgr2_data_flag         
    from ci.mgr2 
   where mgr2_sid = :as_sid
     and mgr2_ymd >= :as_symd
     and mgr2_ymd <= :as_eymd
order by mgr2_ymd desc
";

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
