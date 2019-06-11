using Common;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

/// <summary>
/// John, 2019/4/11
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{

   public class D4001x : DataGate, ID4001x
   {

      protected virtual string FutDataCountSql()
      {
         return "";
      }

      protected virtual string OptDataCountSql()
      {
         return "";
      }

      protected virtual string FutDataSql(int sheet)
      {
         return "";
      }

      protected virtual string OptDataSql(int sheet)
      {
         return "";
      }

      protected virtual string WorkItemSql(int Num)
      {
         return "";
      }

      public ID4001x ConcreteDao(string programID)
      {

         //string className = string.Format("{0}.Dao.Together.SpecificDao.{1}",AssemblyName, name);//完整的class路徑

         string AssemblyName = GetType().Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = GetType().FullName.Replace("D4001x", "D" + programID);//完整的class路徑

         // 這裡就是Reflection，直接依照className實體化具體類別
         return (ID4001x)Assembly.Load(AssemblyName).CreateInstance(className);
      }


      /// <summary>
      /// R1或R2的資料切換
      /// </summary>
      /// <param name="as_date"></param>
      /// <param name="R">1 or 2</param>
      /// <returns>R1 or R2,MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM</returns>
      public DataTable ListFutData(DateTime as_date, int sheet)
      {
         object[] parms = {
                ":as_date",as_date,
            };
         string sql = FutDataSql(sheet);
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }

      /// <summary>
      /// R1或R2的資料切換
      /// </summary>
      /// <param name="as_date"></param>
      /// <param name="R">1 or 2</param>
      public DataTable ListOptData(DateTime as_date, int sheet)
      {
         object[] parms = {
                ":as_date",as_date,
            };
         string sql = OptDataSql(sheet);
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }

      /// <summary>
      /// 確認sheet1有無資料
      /// </summary>
      /// <param name="as_date"></param>
      /// <returns></returns>
      public int FutR1DataCount(DateTime as_date)
      {
         object[] parms = {
                ":as_date",as_date,
            };
         string sql = FutDataCountSql();
         DataTable dtResult = db.GetDataTable(sql, parms);
         if (dtResult.Rows.Count <= 0)
            return 0;
         return dtResult.Rows[0][0].AsInt();
      }

      /// <summary>
      /// 確認sheet2有無資料
      /// </summary>
      /// <param name="as_date"></param>
      /// <returns></returns>
      public int OptR1DataCount(DateTime as_date)
      {
         object[] parms = {
                ":as_date",as_date,
            };
         string sql = OptDataCountSql();
         DataTable dtResult = db.GetDataTable(sql, parms);
         if (dtResult.Rows.Count <= 0)
            return 0;
         return dtResult.Rows[0][0].AsInt();
      }

      /// <summary>
      /// 四、	作業事項 已達或未達10%
      /// </summary>
      /// <param name="as_date">datetime</param>
      /// <param name="Num">sheet 1 or 2</param>
      /// <returns></returns>
      public DataTable WorkItem(DateTime as_date, int Num)
      {
         object[] parms = {
                ":as_date",as_date,
            };
         string sql = WorkItemSql(Num);
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }

      /// <summary>
      /// sheet 作業事項
      /// </summary>
      /// <param name="TxnID">作業名稱</param>
      /// <param name="Num">1 or 2</param>
      /// <returns></returns>
      public int GetRptLV(string TxnID, int Num)
      {
         object[] parms = {
                ":as_txn",TxnID,
                ":as_txn_sheet",$"{TxnID}_{Num}"
            };
         string sql = @"select rpt_level_2
                           from ci.rpt
                           where trim(rpt_txn_id) = :as_txn
                             and trim(rpt_txd_id) = :as_txn_sheet
                             and rpt_value = 'E'";
         DataTable dtResult = db.GetDataTable(sql, parms);
         if (dtResult.Rows.Count <= 0)
            return 0;
         return dtResult.Rows[0][0].AsInt();
      }

      /// <summary>
      /// 判斷FMIF資料已轉入
      /// </summary>
      /// <param name="ld_date">輸入日期</param>
      /// <returns></returns>
      public int CheckFMIF(DateTime ld_date, string os_osw_grp)
      {
         object[] parms = {
                ":ld_date",ld_date,
                ":os_osw_grp",os_osw_grp
            };
         string sql = $@"select count(*) as CNT
                          from ci.AI5,ci.APDK
                         where AI5_DATE = :ld_date
                            and AI5_KIND_ID = APDK_KIND_ID
                            and trim(APDK_MARKET_CLOSE) = :os_osw_grp";
         DataTable dtResult = db.GetDataTable(sql, parms);
         if (dtResult.Rows.Count <= 0)
            return 0;
         return dtResult.Rows[0][0].AsInt();
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

      public DataTable ListRowDataSheet(DateTime as_date)
      {
         object[] parms = {
                ":ad_sdate",as_date.AddDays(-2500),
                ":ad_edate",as_date
            };

         string sql = $@"select TO_DATE(MGP1_YMD,'YYYYMMDD') as AI5_DATE, --IDXF_DATE
                               '000000' as AI5_SETTLE_DATE,
                               CASE WHEN MGP1_PROD_TYPE = 'F' THEN MGP1_SETTLE_PRICE ELSE MGP1_CLOSE_PRICE END  as AI5_SETTLE_PRICE , --IDXF_IDX
                               RPT_SEQ_NO , --RPT_SEQ_NO
                               MGP1_OPEN_REF as AI5_OPEN_REF,
                               PDK_XXX, --契約規格
                               MGR4_CM --現行保證金
                          from ci.MGP1_SMA,ci.RPT ,
                              (select MGP1_M_KIND_ID as DATA_SID from ci.MGP1_SMA where MGP1_PROD_TYPE = 'F' and MGP1_YMD = TO_CHAR(:ad_edate,'YYYYMMDD')),
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
                          and MGP1_PROD_TYPE = 'F'
                          AND RPT_TXD_ID = '40010_1'   --'40010_2'
                          AND MGP1_M_KIND_ID = RPT_VALUE
                          and MGP1_M_KIND_ID = DATA_SID
                          and PDK_KIND_ID = DATA_SID
                        order by MGP1_M_KIND_ID,MGP1_YMD DESC";

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

      public void UpdateMG1_3M(DataTable inputData)
      {
         try {
            string sql = @"
                        SELECT MG1_MODEL_TYPE,MG1_YMD,MG1_PROD_TYPE,MG1_KIND_ID,MG1_AB_TYPE,MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CM,MG1_CUR_CM,MG1_CHANGE_RANGE,
                              MG1_CUR_MM,MG1_CUR_IM,MG1_CP_CM,MG1_MM,MG1_IM,MG1_CURRENCY_TYPE,MG1_M_MULTI,MG1_I_MULTI,MG1_PARAM_KEY,MG1_PROD_SUBTYPE,MG1_W_TIME,MG1_OSW_GRP
                          FROM CI.MG1_3M";

            db.UpdateOracleDB(inputData, sql);
         }
         catch (Exception ex) {
            throw ex;
         }
      }


   }

   public interface ID4001x
   {

      /// <summary>
      /// 確認sheet1有無資料
      /// </summary>
      /// <param name="as_date"></param>
      int FutR1DataCount(DateTime as_date);

      /// <summary>
      /// 確認sheet2有無資料
      /// </summary>
      /// <param name="as_date"></param>
      int OptR1DataCount(DateTime as_date);

      /// <summary>
      /// R1或R2的資料切換
      /// </summary>
      /// <param name="as_date"></param>
      /// <param name="R">R1 or R2</param>
      /// <returns>R1 or R2,MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM</returns>
      DataTable ListFutData(DateTime as_date, int sheet);

      DataTable ListOptData(DateTime as_date, int sheet);

      /// <summary>
      /// 四、	作業事項 已達或未達10%
      /// </summary>
      /// <param name="as_date">datetime</param>
      /// <param name="Num">sheet 1 or 2</param>
      DataTable WorkItem(DateTime as_date, int Num);

      /// <summary>
      /// sheet 作業事項
      /// </summary>
      /// <param name="TxnID">作業名稱</param>
      /// <param name="Num">1 or 2</param>
      int GetRptLV(string TxnID, int Num);

      /// <summary>
      /// 判斷FMIF資料已轉入
      /// </summary>
      /// <param name="ld_date">輸入日期</param>
      int CheckFMIF(DateTime ld_date, string os_osw_grp);

      DataTable List40010CPR(DateTime ad_date, string as_txd_id);

      DataTable ListMG1_3M(string MG1_YMD, string MG1_PROD_TYPE, string MG1_KIND_ID, string MG1_AB_TYPE);

      DataTable ListRowDataSheet(DateTime as_date);

      void UpdateMG1_3M(DataTable inputData);
   }

}
