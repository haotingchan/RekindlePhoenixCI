using BusinessObjects;
using Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

/// <summary>
/// John, 2019/6/11
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{

   public class D40010 : DataGate
   {

      protected string ListRowDataSql()
      {
         string sql = $@"select ROW_NUMBER() OVER (PARTITION BY MG1_KIND_ID ORDER BY MGP1_YMD )AS ROW_NUM,--A 資料序號
                              TO_DATE(MGP1_YMD,'YYYYMMDD') as DATA_DATE, --B 日期
                              MGP1_SETTLE_PRICE,  --C 期貨結算價
                              CASE WHEN MGP1_PROD_TYPE = 'F'  and MGP1_PARAM_KEY <>'ETF' THEN MGP1_SETTLE_PRICE ELSE MGP1_CLOSE_PRICE END  as PRICE, --D 期貨結算價/現貨收盤價
                              MGP1_OPEN_REF as OPEN_REF, --E 開盤參考價
                              MGP1_V_SMA, --F 當日漲跌點數
                              MGP1_RTN_SMA, --G |報酬率|
                              MG1_PROD_SUBTYPE,  --A1 SUBTYPE
                              MG1_PROD_TYPE,  --B1 TYPE
                              MG1_KIND_ID as KIND_ID, --C1 KIND_ID
                              MG1_CURRENCY_TYPE, --D1 幣別
                              MG1_PARAM_KEY,  --E1 PARAM_KEY
                              MG1_M_KIND_ID as MG1_M_KIND_ID, --F1 M_KIND_ID
                              MG1_OSW_GRP, --G1 OSW_GRP
                              MG1_CUR_CM,--P3 現行結算保證金
                              MG1_MIN_RISK, --N1 最小風險價格係數
                              MG1_XXX, --O3 契約規格
                              MGP1_PROD_TYPE --H1
                         from ci.MGP1_SMA,
                             (select MG1_M_KIND_ID,MG1_KIND_ID,MG1_PROD_SUBTYPE,MG1_PARAM_KEY,MG1_OSW_GRP,MG1_MIN_RISK,MG1_CUR_CM,MG1_XXX,MG1_CURRENCY_TYPE,MG1_PROD_TYPE
                               from ci.MG1_3M
                              where trim(MG1_KIND_ID) = :as_kind_id and MG1_AB_TYPE in('A','-')  and MG1_YMD = :as_eymd and MG1_MODEL_TYPE = 'S')
                       where MGP1_YMD <= :as_eymd
                         and trim(MG1_KIND_ID) = :as_kind_id --BY 單一商品用條件
                         and MGP1_M_KIND_ID = MG1_M_KIND_ID
                       order by MG1_KIND_ID ,MGP1_YMD DESC";
         return sql;
      }

      public DataTable ListMGR2_SMA(string as_ymd, string as_kind_id)
      {
         object[] parms = {
                ":as_ymd",as_ymd,
               ":as_kind_id",as_kind_id
            };

         string sql = @"select MGR2_MODEL_TYPE, MGR2_YMD, MGR2_M_KIND_ID, MGR2_PRICE1, MGR2_PRICE2, MGR2_RETURN_RATE, MGR2_30_RATE, MGR2_60_RATE, MGR2_90_RATE, MGR2_180_RATE, MGR2_2500_RATE, MGR2_MIN_RATE, MGR2_DAY_RATE, MGR2_DAY_CNT, MGR2_STATUS_CODE, MGR2_PROD_TYPE, MGR2_PROD_SUBTYPE, MGR2_PARAM_KEY, MGR2_CP_RATE, MGR2_1DAY_CP_RATE, MGR2_W_TIME, MGR2_OSW_GRP
                        from ci.MGR2_SMA
                        where MGR2_MODEL_TYPE='E'
                        and MGR2_YMD=:as_ymd
                        and trim(MGR2_M_KIND_ID)=:as_kind_id";

         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }

      public DataTable ListMGR1_SMA(string as_type, string as_ymd, string as_kind_id)
      {
         object[] parms = {
            ":as_type",as_type,
                ":as_ymd",as_ymd,
               ":as_kind_id",as_kind_id
            };

         string sql = @"select MGR1_MODEL_TYPE, MGR1_YMD, MGR1_M_KIND_ID, MGR1_PRICE1, MGR1_PRICE2, MGR1_RETURN_RATE, MGR1_30_AVG_RRATE, MGR1_60_AVG_RRATE, MGR1_90_AVG_RRATE, MGR1_180_AVG_RRATE, MGR1_30_CNT, MGR1_60_CNT, MGR1_90_CNT, MGR1_180_CNT, MGR1_30_STD, MGR1_60_STD, MGR1_90_STD, MGR1_180_STD, MGR1_STATUS_CODE, MGR1_DATA_CNT, MGR1_W_TIME, MGR1_120_AVG_RRATE, MGR1_120_CNT, MGR1_120_STD, MGR1_150_AVG_RRATE, MGR1_150_CNT, MGR1_150_STD, MGR1_2500_AVG_RRATE, MGR1_2500_CNT, MGR1_2500_STD, MGR1_PROD_TYPE, MGR1_OSW_GRP 
	                       from CI.MGR1_SMA
                        where MGR1_MODEL_TYPE=:as_type
                        and MGR1_YMD=:as_ymd
                        and trim(MGR1_M_KIND_ID)=:as_kind_id";

         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }

      public DataTable ListRowDataSheet(string as_eymd, string as_kind_id)
      {
         object[] parms = {
                ":as_eymd",as_eymd,
                ":as_kind_id",as_kind_id
            };

         string sql = ListRowDataSql();

         return db.GetDataTable(sql, parms);
      }

      public DataTable ListProd(string as_eymd, string as_osw_grp)
      {
         object[] parms = {
                ":as_eymd",as_eymd,
                ":as_osw_grp",as_osw_grp
            };

         string sql = @"select MG1_KIND_ID
                           from ci.MG1_3M 
                           where MG1_OSW_GRP like :as_osw_grp 
                           and MG1_MODEL_TYPE='S' and MG1_AB_TYPE in('A','-')  
                           and MG1_YMD = :as_eymd";

         return db.GetDataTable(sql, parms);
      }

      public ResultData UpdateMGR2_SMA(DataTable inputData)
      {
         try {
            string tableName = "CI.MGR2_SMA";
            string keysColumnList = "MGR2_MODEL_TYPE, MGR2_YMD, MGR2_M_KIND_ID";
            string insertColumnList = @"MGR2_MODEL_TYPE, MGR2_YMD, MGR2_M_KIND_ID, MGR2_PRICE1, MGR2_PRICE2, MGR2_RETURN_RATE, 
MGR2_30_RATE, MGR2_60_RATE, MGR2_90_RATE, MGR2_180_RATE, MGR2_2500_RATE, MGR2_MIN_RATE, MGR2_DAY_RATE, 
MGR2_DAY_CNT, MGR2_STATUS_CODE, MGR2_PROD_TYPE, MGR2_PROD_SUBTYPE, MGR2_PARAM_KEY, MGR2_CP_RATE, MGR2_1DAY_CP_RATE, MGR2_W_TIME, MGR2_OSW_GRP";
            string updateColumnList = insertColumnList;
            
            return SaveForAll(inputData, tableName, insertColumnList, updateColumnList, keysColumnList);
         }
         catch (Exception ex) {
            throw ex;
         }
      }

      public ResultData UpdateMGR1_SMA(DataTable inputData)
      {
         try {
            string tableName = "CI.MGR1_SMA";
            string keysColumnList = "MGR1_MODEL_TYPE, MGR1_YMD, MGR1_M_KIND_ID";
            string insertColumnList = @"MGR1_MODEL_TYPE, MGR1_YMD, MGR1_M_KIND_ID, MGR1_PRICE1, MGR1_PRICE2, MGR1_RETURN_RATE, MGR1_30_AVG_RRATE, MGR1_60_AVG_RRATE, MGR1_90_AVG_RRATE, 
MGR1_180_AVG_RRATE, MGR1_30_CNT, MGR1_60_CNT, MGR1_90_CNT, MGR1_180_CNT, MGR1_30_STD, MGR1_60_STD, MGR1_90_STD, MGR1_180_STD, MGR1_STATUS_CODE, MGR1_DATA_CNT, MGR1_W_TIME,
MGR1_120_AVG_RRATE, MGR1_120_CNT, MGR1_120_STD, MGR1_150_AVG_RRATE, MGR1_150_CNT, MGR1_150_STD, MGR1_2500_AVG_RRATE, MGR1_2500_CNT, MGR1_2500_STD, MGR1_PROD_TYPE, MGR1_OSW_GRP";
            string updateColumnList = insertColumnList;

            return SaveForAll(inputData, tableName, insertColumnList, updateColumnList, keysColumnList);
         }
         catch (Exception ex) {
            throw ex;
         }
      }

      /// <summary>
      /// 呼叫opt.sp_O_gen_H_MG1_3M
      /// </summary>
      /// <param name="ts_ymd">日期：date格式</param>
      /// <param name="ts_osw_grp">1 / 5 / 7</param>
      /// <returns></returns>
      public string SP_O_gen_H_MG1_3M(DateTime ts_ymd, string ts_osw_grp)
      {

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("ts_ymd",ts_ymd),
            new DbParameterEx("ts_osw_grp",ts_osw_grp)
         };

         string sql = "OPT.SP_O_GEN_H_MG1_3M";
         return db.ExecuteStoredProcedureReturnString(sql, parms, true, OracleDbType.Int32);
      }

   }

}
