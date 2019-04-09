using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/4/9
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D30053:DataGate {

        /// <summary>
        /// 期貨商品行情表
        /// </summary>
        /// <param name="as_date"></param>
        /// <returns></returns>
        public DataTable d_30053_f(DateTime as_date) {

            object[] parms = {
                ":as_date", as_date
            };

            string sql =
@"
select AMIF_KIND_ID,AMIF_SETTLE_DATE_OUT,
        OPEN_PRICE, 
        HIGH_PRICE, 
        LOW_PRICE,  
        CLOSE_PRICE,
        AMIF_SETTLE_PRICE, 
        AMIF_UP_DOWN_VAL,  
        AMIF_M_QNTY_TAL,   
        AMIF_OPEN_INTEREST,
        AMIF_KIND_ID2,RPT_KIND_ID,RPT_SEQ_NO,
        ROW_NUMBER( ) OVER (PARTITION BY nvl(AMIF_KIND_ID2,RPT_KIND_ID) ORDER BY AMIF_SETTLE_DATE NULLS LAST) as SEQ_NO ,RPT_DEL_ROW
   from
       (select RPT_VALUE as RPT_KIND_ID,RPT_SEQ_NO,RPT_LEVEL_1 AS RPT_DEL_ROW 
          from ci.RPT 
         where RPT_TXN_ID = '30053' and RPT_TXD_ID = '30053f') R,
       (SELECT AMIF_KIND_ID,
               case when AMIF_SETTLE_DATE = '000000' then '      ' else AMIF_SETTLE_DATE end as AMIF_SETTLE_DATE,
               decode( AMIF_M_QNTY_TAL,0 ,0,AMIF_OPEN_PRICE ) as OPEN_PRICE,
               decode( AMIF_M_QNTY_TAL,0 ,0,AMIF_HIGH_PRICE ) as HIGH_PRICE,
               decode( AMIF_M_QNTY_TAL,0 ,0,AMIF_LOW_PRICE )  as LOW_PRICE,
               decode( AMIF_M_QNTY_TAL,0 ,0,AMIF_CLOSE_PRICE ) as CLOSE_PRICE,
               AMIF_SETTLE_PRICE,
               AMIF_UP_DOWN_VAL,
               AMIF_M_QNTY_TAL,
               AMIF_OPEN_INTEREST,
               case when AMIF_SETTLE_DATE = '000000' then 'TXF00  ' else AMIF_KIND_ID2 end as AMIF_KIND_ID2,
               AMIF_SETTLE_DATE_OUT
          from ci.AMIF
         WHERE AMIF_DATE= :as_date
           and AMIF_PROD_TYPE = 'F'
           and AMIF_PROD_SUBTYPE <> 'S'
           and (AMIF_MTH_SEQ_NO <= 2 )
       UNION ALL 
       --現貨資料
       SELECT NVL(AMIF_KIND_ID,AMIFU_KIND_ID) AS AMIF_KIND_ID,
              '      ' as AMIF_SETTLE_DATE,
              decode( NVL(AMIF_M_QNTY_TAL,AMIFU_M_QNTY_TAL),0 ,0,NVL(AMIF_OPEN_PRICE,AMIFU_OPEN_PRICE)) as OPEN_PRICE,
              decode( NVL(AMIF_M_QNTY_TAL,AMIFU_M_QNTY_TAL),0 ,0,NVL(AMIF_HIGH_PRICE,AMIFU_HIGH_PRICE)) as HIGH_PRICE,
              decode( NVL(AMIF_M_QNTY_TAL,AMIFU_M_QNTY_TAL),0 ,0,NVL(AMIF_LOW_PRICE,AMIFU_LOW_PRICE))  as LOW_PRICE,
              decode( NVL(AMIF_M_QNTY_TAL,AMIFU_M_QNTY_TAL),0 ,0,NVL(AMIF_CLOSE_PRICE,AMIFU_LOW_PRICE)) as CLOSE_PRICE,
              0 AS AMIF_SETTLE_PRICE,
              NVL(AMIF_UP_DOWN_VAL,AMIFU_UP_DOWN_VAL) AS AMIF_UP_DOWN_VAL,
              NVL(AMIF_M_QNTY_TAL,AMIFU_M_QNTY_TAL) AS AMIF_M_QNTY_TAL,
              0 AS AMIF_OPEN_INTEREST,
              'TXF00  ' ,
              '       '
          from 
              (SELECT AMIF_KIND_ID,AMIF_OPEN_PRICE,AMIF_HIGH_PRICE,AMIF_LOW_PRICE,AMIF_CLOSE_PRICE,AMIF_M_QNTY_TAL,AMIF_UP_DOWN_VAL
                 FROM ci.AMIF 
                WHERE AMIF_DATE=  :as_date
                  and AMIF_PROD_ID = 'TXF00') A
               FULL OUTER JOIN 
              (SELECT U.AMIFU_KIND_ID,U.AMIFU_OPEN_PRICE,U.AMIFU_HIGH_PRICE,U.AMIFU_LOW_PRICE,U.AMIFU_CLOSE_PRICE,U.AMIFU_M_QNTY_TAL,R.AMIFU_UP_DOWN_VAL
                 FROM ci.AMIFU U,ci.AMIFU R
                WHERE U.AMIFU_DATE=  :as_date
                  and U.AMIFU_KIND_ID = 'TXF' 
                  and U.AMIFU_SETTLE_DATE = '000000'
                  and U.AMIFU_DATA_SOURCE = 'U'
                  and U.AMIFU_DATE= R.AMIFU_DATE
                  and U.AMIFU_KIND_ID = R.AMIFU_KIND_ID
                  and U.AMIFU_SETTLE_DATE = R.AMIFU_SETTLE_DATE
                  and R.AMIFU_DATA_SOURCE = 'R') B
            ON (AMIF_KIND_ID = AMIFU_KIND_ID)
       ) A
  where TRIM(R.RPT_KIND_ID) = TRIM(A.AMIF_KIND_ID2(+))
  order by rpt_seq_no, seq_no
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 取得期貨商品行情表預設列數
        /// </summary>
        /// <returns></returns>
        public int get30053fRow() {

            string sql =
@"
SELECT RPT_SEQ_NO as ii_ole_row
 FROM CI.RPT
WHERE RPT_TXN_ID = '30053'
  AND RPT_TXD_ID = '30053f'
  AND RPT_VALUE = 'I5F'  
";
            DataTable dtResult = db.GetDataTable(sql, null);

            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                return dtResult.Rows[0]["II_OLE_ROW"].AsInt();
            }
        }

        /// <summary>
        /// 選擇權商品行情表(2表)
        /// </summary>
        /// <param name="as_date"></param>
        /// <returns></returns>
        public DataTable d_30053_o(DateTime as_date) {

            object[] parms = {
                ":as_date", as_date
            };

            string sql =
@"
select AMIF_KIND_ID,
             AMIF_SETTLE_DATE_OUT,
             AMIF_STRIKE_PRICE,
             PC_CODE,
             OPEN_PRICE,
             HIGH_PRICE,
             LOW_PRICE,
             CLOSE_PRICE,
             AMIF_SETTLE_PRICE,
             AMIF_UP_DOWN_VAL,
             AMIF_M_QNTY_TAL,
             AMIF_OPEN_INTEREST,
             AMIF_MTH_SEQ_NO,RPT_SEQ_NO,
             ROW_NUMBER( ) OVER (PARTITION BY AMIF_KIND_ID ORDER BY AMIF_MTH_SEQ_NO,AMIF_PC_CODE,AMIF_M_QNTY_TAL desc,AMIF_STRIKE_PRICE NULLS LAST) as SEQ_NO,
          RPT_DEL_ROW
      from
        (SELECT AMIF_SETTLE_DATE,
                  AMIF_STRIKE_PRICE,
                  decode(AMIF_PC_CODE,'C','買　','  賣' ) as PC_CODE,
                  decode( AMIF_M_QNTY_TAL,0 ,0,AMIF_OPEN_PRICE ) as OPEN_PRICE,
                  decode( AMIF_M_QNTY_TAL,0 ,0,AMIF_HIGH_PRICE ) as HIGH_PRICE,
                  decode( AMIF_M_QNTY_TAL,0 ,0,AMIF_LOW_PRICE ) as LOW_PRICE,
                  decode( AMIF_M_QNTY_TAL,0 ,0,AMIF_CLOSE_PRICE ) as CLOSE_PRICE,
                  AMIF_SETTLE_PRICE ,
                  AMIF_UP_DOWN_VAL ,
                  AMIF_M_QNTY_TAL ,
                  AMIF_OPEN_INTEREST ,
                  AMIF_PROD_ID,  
                  AMIF_KIND_ID,
                  AMIF_PC_CODE,
                  AMIF_PARAM_KEY,
                  AMIF_KIND_ID2,
                  AMIF_SETTLE_DATE_OUT,
                  AMIF_EXPIRY_TYPE,
                  AMIF_MTH_SEQ_NO,
                  ROW_NUMBER( ) OVER (PARTITION BY AMIF_KIND_ID,AMIF_PC_CODE,AMIF_SETTLE_DATE ORDER BY AMIF_M_QNTY_TAL desc,AMIF_STRIKE_PRICE NULLS LAST) as SEQ_NO
              FROM ci.AMIF A 
              WHERE A.AMIF_DATE = :as_date
                 and A.AMIF_PROD_TYPE = 'O'
                 and A.AMIF_PROD_SUBTYPE <> 'S'    
                 and A.AMIF_MTH_SEQ_NO <= (case AMIF_KIND_ID when 'TXO' then 2 else 1 end) ) A,
             (select RPT_VALUE,RPT_LEVEL_1 as RPT_DEL_ROW,RPT_SEQ_NO from ci.RPT where RPT_TXN_ID = '30053' and RPT_TXD_ID = '30053o') R
     where AMIF_KIND_ID2(+) = RPT_VALUE
        and SEQ_NO(+) <= (case AMIF_PARAM_KEY(+) when 'TXO' then 4 else 1 end)
     order by rpt_seq_no, seq_no
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 確認商品STC有無資料
        /// </summary>
        /// <returns></returns>
        public int checkSTCdata() {

            string sql =
@"
select count(*) as li_cnt from ci.APDK
 where trim(APDK_PARAM_KEY) = 'STC'
";
            DataTable dtResult = db.GetDataTable(sql, null);

            if (dtResult.Rows.Count == 0) {
                return 0;
            }
            else {
                return dtResult.Rows[0]["LI_CNT"].AsInt();
            }
        }

        /// <summary>
        /// 股票選擇權(3表) STC有資料
        /// </summary>
        /// <param name="as_date">yyyy/MM/dd</param>
        /// <param name="as_prod_type"></param>
        /// <returns></returns>
        public DataTable d_30053_c(string as_date,string as_prod_type) {

            object[] parms = {
                ":as_date", as_date,
                ":as_prod_type", as_prod_type
            };

            string sql =
@"
select * from
(
   SELECT  case AMIF_PROD_TYPE WHEN 'O' then substr(trim(APDK_NAME),1,length(trim(APDK_NAME))-3) 
                               else trim(APDK_NAME)  end as c1 ,
           AMIF_SETTLE_DATE  as c2,
           AMIF_STRIKE_PRICE  as c3 ,
           decode(AMIF_PC_CODE,'C','買　','　賣' ) as c4,
           decode(AMIF_M_QNTY_TAL,0 ,0,AMIF_OPEN_PRICE ) as c5 ,
                   decode(AMIF_M_QNTY_TAL,0 ,0,AMIF_HIGH_PRICE ) as c6 ,
                   decode(AMIF_M_QNTY_TAL,0 ,0,AMIF_LOW_PRICE ) as c7 ,
           decode(AMIF_M_QNTY_TAL,0 ,0,AMIF_CLOSE_PRICE ) as c8,
           AMIF_SETTLE_PRICE as c9,
           AMIF_UP_DOWN_VAL as c10,
           AMIF_M_QNTY_TAL as c11,
           AMIF_OPEN_INTEREST as c12 ,
           AMIF_PROD_ID ,  
           AMIF_KIND_ID,
           AMIF_PC_CODE
       FROM ci.AMIF A ,
            ci.APDK D
        WHERE A.AMIF_DATE=to_date(:as_date,'yyyy/mm/dd') 
          and A.AMIF_PROD_TYPE = :as_prod_type
          and A.AMIF_PROD_SUBTYPE='S'    
          and trim(D.APDK_KIND_ID)=trim(A.AMIF_KIND_ID) 
        order by   AMIF_M_QNTY_TAL desc , AMIF_OPEN_INTEREST desc , AMIF_PROD_ID 
)
where rownum<= 30 
order by c11 Desc, c12 Desc
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 股票選擇權(3表) STC無資料
        /// </summary>
        /// <param name="as_date">yyyy/MM/dd</param>
        /// <param name="as_prod_type"></param>
        /// <returns></returns>
        public DataTable d_30053_c_sto(string as_date, string as_prod_type) {

            object[] parms = {
                ":as_date", as_date,
                ":as_prod_type", as_prod_type
            };

            string sql =
@"
select * from
(
   SELECT substr(trim(APDK_NAME),1,length(trim(APDK_NAME))-3) as c1 ,
           AMIF_SETTLE_DATE  as c2,
           AMIF_STRIKE_PRICE  as c3 ,
           decode(AMIF_PC_CODE,'C','買　','　賣' ) as c4,
           decode(AMIF_M_QNTY_TAL,0 ,0,AMIF_OPEN_PRICE ) as c5 ,
				   decode(AMIF_M_QNTY_TAL,0 ,0,AMIF_HIGH_PRICE ) as c6 ,
				   decode(AMIF_M_QNTY_TAL,0 ,0,AMIF_LOW_PRICE ) as c7 ,
           decode(AMIF_M_QNTY_TAL,0 ,0,AMIF_CLOSE_PRICE ) as c8,
           AMIF_SETTLE_PRICE as c9,
           AMIF_UP_DOWN_VAL as c10,
           AMIF_M_QNTY_TAL as c11,
           AMIF_OPEN_INTEREST as c12 ,
           AMIF_PROD_ID ,  
           AMIF_KIND_ID,
           AMIF_PC_CODE
       FROM ci.AMIF A ,
            ci.APDK D
        WHERE A.AMIF_DATE=to_date(:as_date,'yyyy/mm/dd') 
          and A.AMIF_PROD_TYPE = :as_prod_type
          and A.AMIF_PROD_SUBTYPE='S'    
          and trim(D.APDK_KIND_ID)=trim(A.AMIF_KIND_ID) 
        order by   c11 desc , c12 desc , AMIF_PROD_ID 
)
where rownum<= 30 
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 股票期貨(For工商時報)
        /// </summary>
        /// <param name="as_date">yyyy/MM/dd</param>
        /// <param name="as_prod_type"></param>
        /// <returns></returns>
        public DataTable d_30053_c_stf_near40(string as_date, string as_prod_type, string rowNum) {

            object[] parms = {
                ":as_date", as_date,
                ":as_prod_type", as_prod_type
            };

            string sql =
string.Format(@"
SELECT R.*
FROM
    (SELECT  
        case A.AMIF_PROD_TYPE WHEN 'O' then substr(trim(APDK_NAME),1,length(trim(APDK_NAME))-3) 
                               else trim(APDK_NAME)  end as c1 ,
           A.AMIF_SETTLE_DATE  as c2,
           A.AMIF_KIND_ID  as c3 ,
           '' as c4,
           decode(A.AMIF_M_QNTY_TAL,0 ,0,A.AMIF_OPEN_PRICE ) as c5 ,
                   decode(A.AMIF_M_QNTY_TAL,0 ,0,AMIF_HIGH_PRICE ) as c6 ,
                   decode(A.AMIF_M_QNTY_TAL,0 ,0,AMIF_LOW_PRICE ) as c7 ,
           decode(A.AMIF_M_QNTY_TAL,0 ,0,A.AMIF_CLOSE_PRICE ) as c8,
           A.AMIF_SETTLE_PRICE as c9,
           A.AMIF_UP_DOWN_VAL as c10,
           A.AMIF_M_QNTY_TAL as c11,
           A.AMIF_OPEN_INTEREST as c12
       FROM ci.AMIF A ,
            ci.APDK D     
        WHERE A.AMIF_DATE=to_date(:as_date,'yyyy/mm/dd') 
          and A.AMIF_PROD_TYPE = :as_prod_type
          and A.AMIF_PROD_SUBTYPE='S'    
          and trim(D.APDK_KIND_ID)=trim(A.AMIF_KIND_ID) 
          order by A.AMIF_M_QNTY_TAL desc, AMIF_OPEN_INTEREST desc , AMIF_PROD_ID) R
WHERE ROWNUM <= {0}
", rowNum);
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 股票期貨Top10檔(For經濟日報)
        /// </summary>
        /// <param name="as_date">yyyy/MM/dd</param>
        /// <returns></returns>
        public DataTable d_30053_c_stf_top40(string as_date) {

            object[] parms = {
                ":as_date", as_date
            };

            string sql =
@"
select * from 
   (
   SELECT  case AMIF_PROD_TYPE WHEN 'O' then substr(trim(APDK_NAME),1,length(trim(APDK_NAME))-3) 
                               else trim(APDK_NAME)  end as c1 ,
           AMIF_SETTLE_DATE  as c2,
           AMIF_KIND_ID  as c3 ,          
           decode(AMIF_M_QNTY_TAL,0 ,0,AMIF_CLOSE_PRICE ) as c4,         
           AMIF_UP_DOWN_VAL as c5,
           AMIF_M_QNTY_TAL as c6,
           AMIF_OPEN_INTEREST as c7 
        FROM ci.AMIF A ,
            ci.APDK D
        WHERE A.AMIF_DATE= to_date(:as_date,'yyyy/mm/dd') 
          and A.AMIF_PROD_TYPE = 'F'
          and A.AMIF_PROD_SUBTYPE='S'    
          and trim(D.APDK_KIND_ID)=trim(A.AMIF_KIND_ID)         
        order by   AMIF_M_QNTY_TAL desc , AMIF_OPEN_INTEREST desc , AMIF_PROD_ID 
    )
    where   rownum <= 40     
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// ETF期貨前10大行情表
        /// </summary>
        /// <param name="as_date">yyyy/MM/dd</param>
        /// <returns></returns>
        public DataTable d_30053_c_etf_top10(string as_date) {

            object[] parms = {
                ":as_date", as_date
            };

            string sql =
@"
select * from 
   (
   SELECT  case AMIF_PROD_TYPE WHEN 'O' then substr(trim(APDK_NAME),1,length(trim(APDK_NAME))-3) 
                               else trim(APDK_NAME)  end as c1 ,
           AMIF_SETTLE_DATE  as c2,
           AMIF_KIND_ID  as c3 ,          
           decode(AMIF_M_QNTY_TAL,0 ,0,AMIF_CLOSE_PRICE ) as c4,         
           AMIF_UP_DOWN_VAL as c5,
           AMIF_M_QNTY_TAL as c6,
           AMIF_OPEN_INTEREST as c7 
        FROM ci.AMIF A ,
            ci.APDK D
        WHERE A.AMIF_DATE= to_date(:as_date,'yyyy/mm/dd') 
          and A.AMIF_PROD_TYPE = 'F'
          and A.AMIF_PROD_SUBTYPE='S'    
          and A.AMIF_PARAM_KEY = 'ETF'
          and trim(D.APDK_KIND_ID)=trim(A.AMIF_KIND_ID)         
        order by   AMIF_M_QNTY_TAL desc , AMIF_OPEN_INTEREST desc , AMIF_PROD_ID 
    )
    where   rownum<= 10     
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 股票期貨Top10檔_聯晚
        /// </summary>
        /// <param name="as_date">yyyy/MM/dd</param>
        /// <returns></returns>
        public DataTable d_30053_c_stc_top10(string as_date) {

            object[] parms = {
                ":as_date", as_date
            };

            string sql =
@"
select * from
(
   SELECT substr(trim(APDK_NAME),1,length(trim(APDK_NAME))-3) as c1 ,
           AMIF_SETTLE_DATE  as c2,
           AMIF_STRIKE_PRICE  as c3 ,
           decode(AMIF_PC_CODE,'C','買　','　賣' ) as c4,
    
           AMIF_SETTLE_PRICE as c5,
           AMIF_UP_DOWN_VAL as c6,
           AMIF_M_QNTY_TAL as c7,
           AMIF_OPEN_INTEREST as c8    
       FROM ci.AMIF A ,
            ci.APDK D
        WHERE A.AMIF_DATE=to_date(:as_date,'yyyy/mm/dd') 
          and A.AMIF_PROD_TYPE = 'O'
          and A.AMIF_PROD_SUBTYPE='S'             
          and A.AMIF_PARAM_KEY = 'STC' 
          and trim(D.APDK_KIND_ID)=trim(A.AMIF_KIND_ID) 
        order by   AMIF_M_QNTY_TAL desc ,AMIF_OPEN_INTEREST desc , AMIF_PROD_ID 
)
where rownum<= 10  
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// ETF選擇權前20大行情表
        /// </summary>
        /// <param name="as_date">yyyy/MM/dd</param>
        /// <returns></returns>
        public DataTable d_30053_c_etc_top20(string as_date) {

            object[] parms = {
                ":as_date", as_date
            };

            string sql =
@"
select * from
(
   SELECT substr(trim(APDK_NAME),1,length(trim(APDK_NAME))-3) as c1 ,
           AMIF_SETTLE_DATE  as c2,
           AMIF_STRIKE_PRICE  as c3 ,
           decode(AMIF_PC_CODE,'C','買　','　賣' ) as c4,
    
           AMIF_SETTLE_PRICE as c5,
           AMIF_UP_DOWN_VAL as c6,
           AMIF_M_QNTY_TAL as c7,
           AMIF_OPEN_INTEREST as c8    
       FROM ci.AMIF A ,
            ci.APDK D
        WHERE A.AMIF_DATE=to_date(:as_date,'yyyy/mm/dd') 
          and A.AMIF_PROD_TYPE = 'O'
          and A.AMIF_PROD_SUBTYPE='S'    
          and A.AMIF_PARAM_KEY = 'ETC'
          and A.AMIF_MTH_SEQ_NO = 1
          and trim(D.APDK_KIND_ID)=trim(A.AMIF_KIND_ID) 
        order by   AMIF_M_QNTY_TAL desc ,AMIF_OPEN_INTEREST desc , AMIF_PROD_ID 
)
where rownum<= 20
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        /// <summary>
        /// 美元兌人民幣選擇權(RHO)20大行情表
        /// </summary>
        /// <param name="as_date">yyyy/MM/dd</param>
        /// <param name="as_param_key"></param>
        /// <returns></returns>
        public DataTable d_30053_c_top20(string as_date, string as_param_key) {

            object[] parms = {
                ":as_date", as_date,
                ":as_param_key", as_param_key
            };

            string sql =
@"
select * from
(
   SELECT substr(trim(APDK_NAME),1,length(trim(APDK_NAME))-3) as c1 ,
           AMIF_SETTLE_DATE  as c2,
           AMIF_STRIKE_PRICE  as c3 ,
           decode(AMIF_PC_CODE,'C','買　','　賣' ) as c4,
    
           AMIF_SETTLE_PRICE as c5,
           AMIF_UP_DOWN_VAL as c6,
           AMIF_M_QNTY_TAL as c7,
           AMIF_OPEN_INTEREST as c8    
       FROM ci.AMIF A ,
            ci.APDK D
        WHERE A.AMIF_DATE=to_date(:as_date,'yyyy/mm/dd') 
          and A.AMIF_PROD_TYPE = 'O'
          --and A.AMIF_PROD_SUBTYPE='S'    
          and A.AMIF_PARAM_KEY =  :as_param_key
          and A.AMIF_MTH_SEQ_NO = 1
          and trim(D.APDK_KIND_ID)=trim(A.AMIF_KIND_ID) 
        order by   AMIF_M_QNTY_TAL desc ,AMIF_OPEN_INTEREST desc , AMIF_PROD_ID 
)
where rownum<= 20
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
