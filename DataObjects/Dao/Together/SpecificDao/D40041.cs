using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D40041 : DataGate {

      public DataTable ListData(string changeFlag, DateTime tradeDate, string oswGrp) {
         object[] parms = {
                ":as_change_flag", changeFlag,
                ":as_trade_date",tradeDate,
                ":as_osw_grp",oswGrp
            };

         string sql = @"
SELECT S.*, ROWNUM from (
SELECT case :as_change_flag when case WHEN MG41_KIND_ID = 'CPF' OR MG41_KIND_ID = 'GBF' then 'N' else 'Y' end then 'Y' 
                            else 'N' end as RUN_FLAG,     
       MG41_KIND_ID AS MG1_KIND_ID,
       case when MG41_PROD_SUBTYPE='S' then APDK_STOCK_ID else ' ' end as  APDK_STOCK_ID,
       MG41_LAST_MG4_DATE AS MG1_SDATE,
       MG41_START_DATE AS DATA_SDATE,
       --MG41_START_DATE AS DATA_SDATE_ORG,
       :as_trade_date as DATA_EDATE,
       MG41_START_DAY_CNT AS DATA_CNT,
       MG41_PROD_SUBTYPE AS MG1_PROD_SUBTYPE,
       MG41_PROD_TYPE AS MG1_PROD_TYPE,
       APDK_NAME as APDK_NAME,
       --APDK_STOCK_ID as APDK_STOCK_ID,
       nvl(PID_NAME,' ') as PID_NAME,
       NVL(MGT2_SEQ_NO,99) as MG1_SEQ_NO
  FROM ci.MG41,
       --當日計算結果
       ci.MG1,
       --基本資料
       ci.APDK,ci.MGT2,
       --上市/上櫃中文名稱
       (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM')
WHERE MG41_DATE = :as_trade_date
  AND MG41_KIND_ID = APDK_KIND_ID
  AND APDK_UNDERLYING_MARKET = COD_ID(+)    
  AND APDK_KIND_ID = MGT2_KIND_ID(+)  
  AND MG1_DATE = MG41_DATE
  AND MG1_KIND_ID = MG41_KIND_ID
  AND MG1_TYPE in ( '-','A' )  
  AND MG1_OSW_GRP LIKE :as_osw_grp
  AND MG1_CHANGE_FLAG LIKE :as_change_flag
  ORDER BY MG1_SEQ_NO, MG1_KIND_ID
) S";
    
         return db.GetDataTable(sql, parms); ;
      }

      public string DiffOcfDays(DateTime date) {
         object[] parms = {
                ":ld_bef30_date", date
            };
         try {
            string sql = @"select fut.DATE_DIFF_OCF_DAYS(:ld_bef30_date,-29) as bef30_date from dual";

            DataTable result = db.GetDataTable(sql, parms);

            if (result != null) {
               return result.Rows[0].ToString();
            }
         } catch (Exception ex) {
            throw ex;
         }
         return "";
      }

      public DataTable GetExportData(string kindId, DateTime sDate,DateTime eDate) {

         object[] parms = {
                ":as_kind_id", kindId,
                ":adt_sdate",sDate,
                ":adt_edate",eDate
            };

         string sql = @"SELECT ROWNUM ,MG6_DATE,   
         case when MG6_PROD_TYPE = 'F' and MG6_PROD_SUBTYPE = 'S'  then MG6_SETTLE_PRICE else MG6_PRICE end as MG6_PRICE,   
         --MG6_PRICE,
         MG6_RISK,   
         MG6_CUR_CM,   
         MG6_CP_CM,   
         MG6_CHANGE_RANGE,   
         MG6_ADJ_CM,   
         MG6_ADJ_RANGE  
    FROM CI.MG6  
   WHERE ( TRIM(MG6_KIND_ID) = :as_kind_id ) AND  
         ( MG6_DATE >= :adt_sdate ) AND  
         ( MG6_DATE <= :adt_edate )";

         return db.GetDataTable(sql, parms); ;
      }

      public string GetReCount(string kindId, DateTime sDate, DateTime eDate) {

         object[] parms = {
                ":as_kind_id", kindId,
                ":adt_sdate",sDate,
                ":adt_edate",eDate
            };

         string sql = @"select count(*) from (
SELECT ROWNUM ,MG6_DATE,   
         case when MG6_PROD_TYPE = 'F' and MG6_PROD_SUBTYPE = 'S'  then MG6_SETTLE_PRICE else MG6_PRICE end as MG6_PRICE,   
         --MG6_PRICE,
         MG6_RISK,   
         MG6_CUR_CM,   
         MG6_CP_CM,   
         MG6_CHANGE_RANGE,   
         MG6_ADJ_CM,   
         MG6_ADJ_RANGE  
    FROM CI.MG6  
   WHERE ( TRIM(MG6_KIND_ID) = :as_kind_id ) AND  
         ( MG6_DATE >= :adt_sdate ) AND  
         ( MG6_DATE <= :adt_edate )
)";

         return db.ExecuteScalar(sql, CommandType.Text, parms);
      }
   }
}
