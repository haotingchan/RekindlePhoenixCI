using BusinessObjects;
using Common;
using System;
using System.Collections.Generic;
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

      protected virtual string FutOptDataSql()
      {
         string sql = @"
                     SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CUR_CM_RATE,MG1_CUR_MM_RATE,MG1_CUR_IM_RATE,
                                   MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,
                                   decode(:as_txn_id||'_'||:as_sheet,'40011_2',MG1_CP_CM,MG1_CM) AS MG1_CM,MG1_CUR_CM as MG1_CUR_CM2,MG1_CHANGE_RANGE,
                                   MG1_CHANGE_FLAG,
                                   MG1_PROD_TYPE,MG1_KIND_ID,MG1_AB_TYPE,R1,decode(MG1_MODEL_TYPE,'S',RPT_LEVEL_3,'M',RPT_LEVEL_4,'E',RPT_LEVEL_CNT) AS R2,SHEET,MGT2_KIND_ID_OUT,MG1_MODEL_TYPE
                       FROM ci.MG1_3M,ci.MGT2,
                             (SELECT RPT_VALUE AS R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3,
                                   RPT_LEVEL_4,RPT_LEVEL_CNT
                                FROM ci.RPT
                                WHERE trim(RPT_TXN_ID) = :as_txn_id AND trim(RPT_TXD_ID) = :as_txn_id||'_'||:as_sheet) R
                       WHERE MG1_YMD = to_char(:as_date,'YYYYMMDD')
                          AND MG1_KIND_ID = R_KIND_ID
                          AND MG1_KIND_ID = MGT2_KIND_ID
                      ORDER BY R1,R2,MG1_KIND_ID,MG1_AB_TYPE";
         return sql;
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
      /// 一二大項資料
      /// </summary>
      /// <param name="as_date">yyyy/MM/dd</param>
      /// <param name="as_txn_id">作業代號</param>
      /// <param name="sheet">第幾個sheet</param>
      /// <returns></returns>
      public DataTable ListFutOptData(DateTime as_date,string as_txn_id, int sheet)
      {
         object[] parms = {
                ":as_date",as_date,
                ":as_txn_id",as_txn_id,
                ":as_sheet",sheet
            };
         string sql = FutOptDataSql();
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }

      /// <summary>
      /// d_40011_stat
      /// </summary>
      /// <param name="AS_YMD"></param>
      /// <returns></returns>
      public DataTable List40011Stat(string as_ymd)
      {

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("as_ymd",as_ymd)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_TXN_40011_STAT";
         DataTable dt = db.ExecuteStoredProcedureEx(sql, parms, true);
         return dt;
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


   }

   public interface ID4001x
   {
      /// <summary>
      /// 一二大項資料
      /// </summary>
      /// <param name="as_date">yyyy/MM/dd</param>
      /// <param name="as_txn_id">作業代號</param>
      /// <param name="sheet">第幾個sheet</param>
      DataTable ListFutOptData(DateTime as_date, string as_txn_id, int sheet);

      /// <summary>
      /// d_40011_stat
      /// </summary>
      /// <param name="AS_YMD"></param>
      /// <returns></returns>
      DataTable List40011Stat(string as_ymd);

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
   }

}
