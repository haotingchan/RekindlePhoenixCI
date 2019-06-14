using Common;
using System;
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

      public ID40010 ConcreteDao(string programID)
      {

         //string className = string.Format("{0}.Dao.Together.SpecificDao.{1}",AssemblyName, name);//完整的class路徑

         string AssemblyName = GetType().Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = GetType().FullName.Replace("D40010", "D" + programID);//完整的class路徑

         // 這裡就是Reflection，直接依照className實體化具體類別
         return (ID40010)Assembly.Load(AssemblyName).CreateInstance(className);
      }

      protected virtual string ListRowDataSql()
      {
         string sql = $@"select TO_DATE(MGP1_YMD,'YYYYMMDD') as AI5_DATE, --IDXF_DATE
                               '000000' as AI5_SETTLE_DATE,
                               CASE WHEN MGP1_PROD_TYPE = :MGP1_PROD_TYPE THEN MGP1_SETTLE_PRICE ELSE MGP1_CLOSE_PRICE END  as AI5_SETTLE_PRICE , --IDXF_IDX
                               RPT_SEQ_NO , --RPT_SEQ_NO
                               MGP1_OPEN_REF as AI5_OPEN_REF,
                               PDK_XXX, --契約規格
                               MGR4_CM --現行保證金
                          from ci.MGP1_SMA,ci.RPT ,
                              (select MGP1_M_KIND_ID as DATA_SID from ci.MGP1_SMA where MGP1_PROD_TYPE = :MGP1_PROD_TYPE and MGP1_YMD = TO_CHAR(:ad_edate,'YYYYMMDD')),
                              (select MGR4_YMD,
                        MGR4_KIND_ID,
                        MGR4_CM,
                        PDK_XXX,
                        PDK_KIND_ID
                        from ci.MGR4,ci.HPDK
                        where MGR4_YMD = TO_CHAR(PDK_DATE,'YYYYMMDD')
                        and MGR4_KIND_ID = PDK_KIND_ID
                        and MGR4_YMD = TO_CHAR(:ad_edate,'YYYYMMDD')
                        )            
                        where MGP1_YMD >= TO_CHAR(:ad_sdate,'YYYYMMDD')
                          and MGP1_YMD <= TO_CHAR(:ad_edate,'YYYYMMDD')
                          and MGP1_PROD_TYPE = :MGP1_PROD_TYPE
                          AND RPT_TXD_ID = :as_txd_id   --'40010_2'
                          AND MGP1_M_KIND_ID = RPT_VALUE
                          and MGP1_M_KIND_ID = DATA_SID
                          and PDK_KIND_ID = DATA_SID
                        order by MGP1_M_KIND_ID,MGP1_YMD DESC";
         return sql;
      }

      public DataTable ListMG1_3M(string MG1_YMD, string MG1_PROD_TYPE, string MG1_KIND_ID, string MG1_AB_TYPE)
      {
         object[] parms = {
                ":MG1_YMD",MG1_YMD,
                ":MG1_PROD_TYPE",MG1_PROD_TYPE,
            ":MG1_KIND_ID",MG1_KIND_ID,
            ":MG1_AB_TYPE",MG1_AB_TYPE
            };

         string sql = @"select MG1_MODEL_TYPE,MG1_YMD,MG1_PROD_TYPE,MG1_KIND_ID,MG1_AB_TYPE,MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CM,MG1_CUR_CM,MG1_CHANGE_RANGE,
                              MG1_CUR_MM,MG1_CUR_IM,MG1_CP_CM,MG1_MM,MG1_IM,MG1_CURRENCY_TYPE,MG1_M_MULTI,MG1_I_MULTI,MG1_PARAM_KEY,MG1_PROD_SUBTYPE,MG1_W_TIME,MG1_OSW_GRP
                           from ci.MG1_3M
                           where MG1_MODEL_TYPE='E'
                           and MG1_YMD=:MG1_YMD
                           and MG1_PROD_TYPE=:MG1_PROD_TYPE
                           and MG1_KIND_ID=:MG1_KIND_ID
                           and MG1_AB_TYPE=:MG1_AB_TYPE";

         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }

      public DataTable ListRowDataSheet(DateTime as_date, string as_txd_id,string MGP1_PROD_TYPE)
      {
         object[] parms = {
                ":ad_sdate",as_date.AddDays(-2500),
                ":ad_edate",as_date,
                ":as_txd_id",as_txd_id,
                ":MGP1_PROD_TYPE",MGP1_PROD_TYPE
            };

         string sql = ListRowDataSql();

         return db.GetDataTable(sql, parms);
      }

      public DataTable List40010CPR(DateTime ad_date,string as_txd_id)
      {
         object[] parms = {
                ":ad_date",ad_date,
                ":as_txd_id",as_txd_id
            };

         string sql = @"SELECT CPR_KIND_ID,
                               max(case when ROW_NUM = 1 then CPR_EFFECTIVE_DATE else null end) as CPR_EFFECTIVE_DATE,RPT_VALUE_2,
                               max(case when ROW_NUM = 1 then CPR_PRICE_RISK_RATE else null end) as CPR_PRICE_RISK_RATE,
                               max(case when ROW_NUM = 2 then CPR_PRICE_RISK_RATE else null end) as LAST_RISK_RATE
                          from ci.hcpr,ci.rpt,
                              (select CPR_KIND_ID AS MAX_KIND_ID,CPR_EFFECTIVE_DATE as MAX_EFFECTIVE_DATE,
                                      ROW_NUMBER( ) OVER (PARTITION BY CPR_KIND_ID ORDER BY CPR_EFFECTIVE_DATE DESC NULLS LAST) as ROW_NUM
                                 from ci.HCPR
                                where CPR_EFFECTIVE_DATE <= :ad_date)
                         where ROW_NUM <= 2
                           and CPR_KIND_ID = MAX_KIND_ID
                           and CPR_EFFECTIVE_DATE = MAX_EFFECTIVE_DATE
                           and RPT_TXD_ID = :as_txd_id
                           and trim(CPR_KIND_ID) = trim(RPT_VALUE)
                          group by CPR_KIND_ID,RPT_VALUE_2";

         return db.GetDataTable(sql, parms);
      }

      public void UpdateMGR2_SMA(DataTable inputData)
      {
         try {
            string sql = @"
                        SELECT MGR2_MODEL_TYPE, MGR2_YMD, MGR2_M_KIND_ID, MGR2_PRICE1, MGR2_PRICE2, MGR2_RETURN_RATE, MGR2_30_RATE, MGR2_60_RATE, MGR2_90_RATE, MGR2_180_RATE, MGR2_2500_RATE, MGR2_MIN_RATE, MGR2_DAY_RATE, MGR2_DAY_CNT, MGR2_STATUS_CODE, MGR2_PROD_TYPE, MGR2_PROD_SUBTYPE, MGR2_PARAM_KEY, MGR2_CP_RATE, MGR2_1DAY_CP_RATE, MGR2_W_TIME, MGR2_OSW_GRP 
	                       FROM CI.MGR2_SMA";

            db.UpdateOracleDB(inputData, sql);
         }
         catch (Exception ex) {
            throw ex;
         }
      }

   }

   public interface ID40010
   {
      DataTable ListMG1_3M(string MG1_YMD, string MG1_PROD_TYPE, string MG1_KIND_ID, string MG1_AB_TYPE);

      DataTable ListRowDataSheet(DateTime as_date, string as_txd_id, string MGP1_PROD_TYPE);

      DataTable List40010CPR(DateTime ad_date, string as_txd_id);

      void UpdateMGR2_SMA(DataTable inputData);
   }

}
