using System;
using System.Data;
using System.Reflection;

/// <summary>
/// ken,2019/3/18
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    /// <summary>
    /// 最小風險價格係數現況查詢
    /// </summary>
    public class D48010 : DataGate {


        /// <summary>
        /// 契約代號 return mgt2_seq_no/mgt2_kind_id/mgt2_prod_subtype
        /// </summary>
        /// <returns></returns>
        public DataTable ListKind() {

            string sql = @"
select mgt2_seq_no,
    mgt2_kind_id,
    mgt2_prod_subtype
from ci.mgt2    
where (nvl(mgt2_data_type,' ') = ' ' 
or mgt2_kind_id in ('ETF','ETC'))
order by mgt2_seq_no";


            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        // 專案的namespace
        //private static readonly string AssemblyName = "DataObjects";//其實就是最後compile出來的dll名稱

        /// <summary>
        /// 針對不同的grid data source,合併相同的輸入與輸出
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IGridData CreateGridData(Type type, string name) {

            //string className = string.Format("{0}.Dao.Together.SpecificDao.{1}",AssemblyName, name);//完整的class路徑

            string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
            string className = type.FullName + name;//完整的class路徑

            // 這裡就是Reflection，直接依照className實體化具體類別
            return (IGridData)Assembly.Load(AssemblyName).CreateInstance(className);
        }

    }

    public class Q48010 {
        public DateTime ad_date { get; set; }
        public string as_prod_subtype { get; set; }
        public string as_kind_id { get; set; }
        /// <summary>
        /// 只為了排序,value=DATE/KIND
        /// </summary>
        public string as_sort_type { get; set; }

        public Q48010(DateTime ad_date, string as_prod_subtype, string as_kind_id, string as_sort_type = "DATE") {
            this.ad_date = ad_date;
            this.as_prod_subtype = as_prod_subtype;
            this.as_kind_id = as_kind_id;
            this.as_sort_type = as_sort_type;
        }

        /// <summary>
        /// convert all Properties to object[]
        /// </summary>
        /// <returns></returns>
        public object[] ToParam() {
            object[] aryParam = new object[GetType().GetProperties().Length * 2];
            int pos = 0;

            foreach (var prop in GetType().GetProperties()) {
                aryParam[pos++] = ":" + prop.Name;//":"其實可不用
                aryParam[pos++] = prop.GetValue(this);
            }

            return aryParam;
        }
    }

    public interface IGridData {
        //第一次改寫(廢除)
        DataTable ListAll(DateTime ad_date, string as_prod_subtype, string as_kind_id, string as_sort_type = "DATE");

        DataTable ListAll(Q48010 query);
    }

    /// <summary>
    /// 重點資料
    /// </summary>
    public class D48010KeyInfo : DataGate, IGridData {

        /// <summary>
        /// 第一次改寫(廢除) 重點資料, return 8 fields
        /// </summary>
        /// <param name="ad_date"></param>
        /// <param name="as_prod_subtype"></param>
        /// <param name="as_kind_id"></param>
        /// <param name="as_sort_type">只為了排序,value=DATE/KIND</param>
        /// <returns></returns>
        public DataTable ListAll(DateTime ad_date, string as_prod_subtype, string as_kind_id, string as_sort_type = "DATE") {

            object[] parms = {
                ":ad_date", ad_date,
                ":as_prod_subtype", as_prod_subtype,
                ":as_kind_id", as_kind_id
            };

            string sort = (as_sort_type == "DATE" ? "cpr_effective_date" : "cpr_prod_subtype");

            string sql = string.Format(@"
select cpr_prod_subtype,
    cod.cod_desc,
    cpr_kind_id,
    to_char(cpr_effective_date,'yyyy/mm/dd') as cpr_effective_date,
    trim(to_char(round(nvl(cpr_price_risk_rate,0)*100,2),990.99)||'%') as cpr_price_risk_rate,

    to_char(cpr_approval_date,'yyyy/mm/dd') as cpr_approval_date,
    --cpr_approval_number,
    --cpr_remark,
    --to_char(cpr_w_time,'yyyy/mm/dd hh24:mi:ss') as cpr_w_time,
    --trim(cpr_w_user_id) as cpr_w_user_id,
    
    nvl(mgt2_seq_no,999) as seq_no,
    mgt2_prod_type as prod_type
  from ci.hcpr,
       ci.mgt2,
        (select cpr_kind_id as max_kind_id,max(cpr_effective_date) as max_effective_date
         from ci.hcpr
         where cpr_effective_date <= :ad_date
         group by cpr_kind_id) test,
       (select trim(cod_id) as cod_id,trim(cod_desc) as cod_desc
        from ci.cod where cod_txn_id = '49020') cod
 where cpr_kind_id = mgt2_kind_id(+)
   and cpr_kind_id = max_kind_id
   and cpr_effective_date = max_effective_date
   and nvl(cpr_price_risk_rate,-999) <> -999
   and cpr_prod_subtype=COD.COD_ID
   and (nvl(mgt2_data_type,' ') = ' '  or mgt2_kind_id in ('ETF','ETC'))
   and cpr_prod_subtype like :as_prod_subtype
   and cpr_kind_id like :as_kind_id
order by {0} , cpr_prod_subtype , prod_type , cpr_kind_id , cpr_effective_date
", sort);


            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 重點資料, return 8 fields
        /// </summary>
        /// <param name="query">ref Q48010</param>
        /// <returns></returns>
        public DataTable ListAll(Q48010 query) {

            object[] parms = query.ToParam();

            string sort = (query.as_sort_type == "DATE" ? "cpr_effective_date" : "cpr_prod_subtype");

            string sql = string.Format(@"
select cpr_prod_subtype,
    cod.cod_desc,
    cpr_kind_id,
    to_char(cpr_effective_date,'yyyy/mm/dd') as cpr_effective_date,
    trim(to_char(round(nvl(cpr_price_risk_rate,0)*100,2),990.99)||'%') as cpr_price_risk_rate,

    to_char(cpr_approval_date,'yyyy/mm/dd') as cpr_approval_date,
    --cpr_approval_number,
    --cpr_remark,
    --to_char(cpr_w_time,'yyyy/mm/dd hh24:mi:ss') as cpr_w_time,
    --trim(cpr_w_user_id) as cpr_w_user_id,
    
    nvl(mgt2_seq_no,999) as seq_no,
    mgt2_prod_type as prod_type
  from ci.hcpr,
       ci.mgt2,
        (select cpr_kind_id as max_kind_id,max(cpr_effective_date) as max_effective_date
         from ci.hcpr
         where cpr_effective_date <= :ad_date
         group by cpr_kind_id) test,
       (select trim(cod_id) as cod_id,trim(cod_desc) as cod_desc
        from ci.cod where cod_txn_id = '49020') cod
 where cpr_kind_id = mgt2_kind_id(+)
   and cpr_kind_id = max_kind_id
   and cpr_effective_date = max_effective_date
   and nvl(cpr_price_risk_rate,-999) <> -999
   and cpr_prod_subtype=COD.COD_ID
   and (nvl(mgt2_data_type,' ') = ' '  or mgt2_kind_id in ('ETF','ETC'))
   and cpr_prod_subtype like :as_prod_subtype
   and cpr_kind_id like :as_kind_id
order by {0} , cpr_prod_subtype , prod_type , cpr_kind_id , cpr_effective_date
", sort);


            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }

    /// <summary>
    /// 明細資料
    /// </summary>
    public class D48010Detail : DataGate, IGridData {

        /// <summary>
        /// 第一次改寫(廢除) 明細資料, return 12 fields
        /// </summary>
        /// <param name="ad_date"></param>
        /// <param name="as_prod_subtype"></param>
        /// <param name="as_kind_id"></param>
        /// <param name="as_sort_type">只為了排序,value=DATE/KIND</param>
        /// <returns></returns>
        public DataTable ListAll(DateTime ad_date, string as_prod_subtype, string as_kind_id, string as_sort_type = "DATE") {

            object[] parms = {
                ":ad_date", ad_date,
                ":as_prod_subtype", as_prod_subtype,
                ":as_kind_id", as_kind_id
            };

            string sort = (as_sort_type == "DATE" ? "cpr_effective_date" : "cpr_prod_subtype");

            string sql = string.Format(@"
select cpr_prod_subtype,
    cod.cod_desc,
    cpr_kind_id,
    to_char(cpr_effective_date,'yyyy/mm/dd') as cpr_effective_date,
    trim(to_char(round(nvl(cpr_price_risk_rate,0)*100,2),990.99)||'%') as cpr_price_risk_rate,

    to_char(cpr_approval_date,'yyyy/mm/dd') as cpr_approval_date,
    cpr_approval_number,
    cpr_remark,
    to_char(cpr_w_time,'yyyy/mm/dd hh24:mi:ss') as cpr_w_time,
    trim(cpr_w_user_id) as cpr_w_user_id,
    
    nvl(mgt2_seq_no,999) as seq_no,
    mgt2_prod_type as prod_type
  from ci.hcpr,
       ci.mgt2,
        (select cpr_kind_id as max_kind_id,max(cpr_effective_date) as max_effective_date
         from ci.hcpr
         where cpr_effective_date <= :ad_date
         group by cpr_kind_id) test,
       (select trim(cod_id) as cod_id,trim(cod_desc) as cod_desc
        from ci.cod where cod_txn_id = '49020') cod
 where cpr_kind_id = mgt2_kind_id(+)
   and cpr_kind_id = max_kind_id
   and cpr_effective_date = max_effective_date
   and nvl(cpr_price_risk_rate,-999) <> -999
   and cpr_prod_subtype=COD.COD_ID
   and (nvl(mgt2_data_type,' ') = ' '  or mgt2_kind_id in ('ETF','ETC'))
   and cpr_prod_subtype like :as_prod_subtype
   and cpr_kind_id like :as_kind_id
order by {0} , cpr_prod_subtype , prod_type , cpr_kind_id , cpr_effective_date
", sort);


            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 明細資料, return 12 fields
        /// </summary>
        /// <param name="query">ref Q48010</param>
        /// <returns></returns>
        public DataTable ListAll(Q48010 query) {

            object[] parms = query.ToParam();

            string sort = (query.as_sort_type == "DATE" ? "cpr_effective_date" : "cpr_prod_subtype");

            string sql = string.Format(@"
select cpr_prod_subtype,
    cod.cod_desc,
    cpr_kind_id,
    to_char(cpr_effective_date,'yyyy/mm/dd') as cpr_effective_date,
    trim(to_char(round(nvl(cpr_price_risk_rate,0)*100,2),990.99)||'%') as cpr_price_risk_rate,

    to_char(cpr_approval_date,'yyyy/mm/dd') as cpr_approval_date,
    cpr_approval_number,
    cpr_remark,
    to_char(cpr_w_time,'yyyy/mm/dd hh24:mi:ss') as cpr_w_time,
    trim(cpr_w_user_id) as cpr_w_user_id,
    
    nvl(mgt2_seq_no,999) as seq_no,
    mgt2_prod_type as prod_type
  from ci.hcpr,
       ci.mgt2,
        (select cpr_kind_id as max_kind_id,max(cpr_effective_date) as max_effective_date
         from ci.hcpr
         where cpr_effective_date <= :ad_date
         group by cpr_kind_id) test,
       (select trim(cod_id) as cod_id,trim(cod_desc) as cod_desc
        from ci.cod where cod_txn_id = '49020') cod
 where cpr_kind_id = mgt2_kind_id(+)
   and cpr_kind_id = max_kind_id
   and cpr_effective_date = max_effective_date
   and nvl(cpr_price_risk_rate,-999) <> -999
   and cpr_prod_subtype=COD.COD_ID
   and (nvl(mgt2_data_type,' ') = ' '  or mgt2_kind_id in ('ETF','ETC'))
   and cpr_prod_subtype like :as_prod_subtype
   and cpr_kind_id like :as_kind_id
order by {0} , cpr_prod_subtype , prod_type , cpr_kind_id , cpr_effective_date
", sort);


            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
