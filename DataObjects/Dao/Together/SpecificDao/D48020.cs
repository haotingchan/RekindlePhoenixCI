﻿using System;
using System.Data;
using System.Reflection;

/// <summary>
/// ken,2019/3/20
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    /// <summary>
    /// 最小風險價格係數歷次調整資料查詢
    /// </summary>
    public class D48020 : DataGate {


        /// <summary>
        /// 契約代號 return mgt2_seq_no/mgt2_kind_id/mgt2_prod_subtype (同48010)
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

        /// <summary>
        /// 針對不同的grid data source,合併相同的輸入與輸出
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public I48020GridData CreateGridData(Type type, string name) {
            string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
            string className = type.FullName + name;//完整的class路徑

            // 這裡就是Reflection，直接依照className實體化具體類別
            return (I48020GridData)Assembly.Load(AssemblyName).CreateInstance(className);
        }

    }

    public class Q48020 {
        public DateTime as_sdate { get; set; }
        public DateTime as_edate { get; set; }
        public string as_prod_subtype { get; set; }
        public string as_kind_id { get; set; }
        /// <summary>
        /// 只為了排序,value=DATE/KIND
        /// </summary>
        public string as_sort_type { get; set; }

        public Q48020(DateTime as_sdate, DateTime as_edate, string as_prod_subtype, string as_kind_id, string as_sort_type = "DATE") {
            this.as_sdate = as_sdate;
            this.as_edate = as_edate;
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

    public interface I48020GridData {

        DataTable ListAll(Q48020 query);
    }

    /// <summary>
    /// 重點資料
    /// </summary>
    public class D48020KeyInfo : DataGate, I48020GridData {

        /// <summary>
        /// 重點資料, return 9 fields
        /// </summary>
        /// <param name="query">ref Q48020</param>
        /// <returns></returns>
        public DataTable ListAll(Q48020 query) {

            object[] parms = query.ToParam();

            string sort = (query.as_sort_type == "DATE" ? "cpr_effective_date" : "cpr_prod_subtype");

            string sql = string.Format(@"
select cpr_data_num,
    cpr_prod_subtype,
    cod.cod_desc,
    cpr_kind_id,
    to_char(cpr_effective_date,'yyyy/mm/dd') as cpr_effective_date,

    (case when nvl(cpr_price_risk_rate,9999)=9999 then '' else trim(to_char(round(cpr_price_risk_rate*100,2),990.99)||'%') end) as cpr_price_risk_rate,

    to_char(cpr_approval_date,'yyyy/mm/dd') as cpr_approval_date,
    --cpr_approval_number,
    --cpr_remark,
    --to_char(cpr_w_time,'yyyy/mm/dd hh24:mi:ss') as cpr_w_time,

    --cpr_w_user_id,
    nvl(mgt2_seq_no,999) as mgt2_seq_no,
    nvl(mgt2_prod_type,' ') as prod_type
from ci.hcpr,
    ci.mgt2,
    (select trim(cod_id) as cod_id,trim(cod_desc) as cod_desc
     from ci.cod where cod_txn_id = '49020') cod

where cpr_kind_id = mgt2_kind_id(+)
and cpr_prod_subtype=cod.cod_id
and cpr_prod_subtype like :as_prod_subtype
and cpr_kind_id like :as_kind_id
and cpr_effective_date >= :as_sdate
and cpr_effective_date <= :as_edate
order by {0} , cpr_prod_subtype , prod_type , cpr_kind_id , cpr_effective_date , cpr_data_num
" , sort);


            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }

    /// <summary>
    /// 明細資料
    /// </summary>
    public class D48020Detail : DataGate, I48020GridData {


        /// <summary>
        /// 明細資料, return 13 fields
        /// </summary>
        /// <param name="query">ref Q48020</param>
        /// <returns></returns>
        public DataTable ListAll(Q48020 query) {

            object[] parms = query.ToParam();

            string sort = (query.as_sort_type == "DATE" ? "cpr_effective_date" : "cpr_prod_subtype");

            string sql = string.Format(@"
select cpr_data_num,
    cpr_prod_subtype,
    cod.cod_desc,
    cpr_kind_id,
    (case when nvl(cpr_price_risk_rate,0)=0 then '' else trim(to_char(round(nvl(cpr_price_risk_rate,0)*100,2),990.99)||'%') end) as cpr_price_risk_rate,

    to_char(cpr_effective_date,'yyyy/mm/dd') as cpr_effective_date,
    to_char(cpr_approval_date,'yyyy/mm/dd') as cpr_approval_date,
    cpr_approval_number,
    cpr_remark,
    to_char(cpr_w_time,'yyyy/mm/dd hh24:mi:ss') as cpr_w_time,

    cpr_w_user_id,
    nvl(mgt2_seq_no,999) as mgt2_seq_no,
    nvl(mgt2_prod_type,' ') as prod_type
from ci.hcpr,
    ci.mgt2,
    (select trim(cod_id) as cod_id,trim(cod_desc) as cod_desc
     from ci.cod where cod_txn_id = '49020') cod

where cpr_kind_id = mgt2_kind_id(+)
and cpr_prod_subtype=cod.cod_id
and cpr_prod_subtype like :as_prod_subtype
and cpr_kind_id like :as_kind_id
and cpr_effective_date >= :as_sdate
and cpr_effective_date <= :as_edate
order by {0} , cpr_prod_subtype , prod_type , cpr_kind_id , cpr_effective_date , cpr_data_num
" , sort);


            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
