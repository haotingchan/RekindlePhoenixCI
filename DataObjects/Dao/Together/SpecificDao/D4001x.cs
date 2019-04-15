using Common;
using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;

/// <summary>
/// John, 2019/4/11
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{

   public class D4001x : DataGate, ID4001x
   {

      public virtual string FutDataCountSql()
      {
         return "";
      }

      public virtual string OptDataCountSql()
      {
         return "";
      }

      public virtual string FutDataSql(SheetType R)
      {
         return "";
      }

      public virtual string OptDataSql(SheetType R)
      {
         return "";
      }

      public virtual string WorkItemSql(int Num)
      {
         return "";
      }

      public ID4001x ConcreteDao(string programID)
      {

         //string className = string.Format("{0}.Dao.Together.SpecificDao.{1}",AssemblyName, name);//完整的class路徑

         string AssemblyName = GetType().Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = GetType().FullName.Replace("D4001x", "D"+programID);//完整的class路徑

         // 這裡就是Reflection，直接依照className實體化具體類別
         return (ID4001x)Assembly.Load(AssemblyName).CreateInstance(className);
      }


      /// <summary>
      /// R1或R2的資料切換
      /// </summary>
      /// <param name="as_date"></param>
      /// <param name="R">R1 or R2</param>
      /// <returns>R1 or R2,MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM</returns>
      private DataTable GetFutData(DateTime as_date, SheetType R)
      {
         object[] parms = {
                ":as_date",as_date,
            };
         string sql = FutDataSql(R);
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }

      private DataTable GetOptData(DateTime as_date, SheetType R)
      {
         object[] parms = {
                ":as_date",as_date,
            };
         string sql = OptDataSql(R);
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
         return dtResult.Rows[0][0].AsInt();
      }

      /// <summary>
      /// sheet=1 現行收取保證金金額
      /// </summary>
      /// <returns>MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE</returns>
      public DataTable GetFutR1Data(DateTime as_date)
      {
         DataTable dtResult = GetFutData(as_date, SheetType.R1);
         //import 商品資料
         dtResult.Columns.Remove(dtResult.Columns["R1"]);
         for (int k = 6; k < 12; k++)
         {
            dtResult.Columns.Remove(dtResult.Columns[6].ColumnName);//刪除後面6欄
         }
         return dtResult;
      }

      /// <summary>
      /// sheet=1 本日結算保證金計算
      /// </summary>
      /// <returns>MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM</returns>
      public DataTable GetFutR2Data(DateTime as_date)
      {
         DataTable dtResult = GetFutData(as_date, SheetType.R2);
         //import 商品資料
         dtResult.Columns.Remove(dtResult.Columns["R2"]);
         for (int k = 0; k < 6; k++)
         {
            dtResult.Columns.Remove(dtResult.Columns[0].ColumnName);//刪除前面6欄
         }
         return dtResult;
      }

      /// <summary>
      /// sheet=2 現行收取保證金金額
      /// </summary>
      /// <returns>MG1_CUR_CM,'',MG1_CUR_MM,'',MG1_CUR_IM</returns>
      public DataTable GetOptR1Data(DateTime as_date)
      {
         DataTable dtResult = GetOptData(as_date, SheetType.R1);
         //import 商品資料
         dtResult.Columns.Remove(dtResult.Columns["R1"]);
         dtResult.Columns.Remove(dtResult.Columns["MG1_TYPE"]);
         for (int k = 5; k < 10; k++)
         {
            dtResult.Columns.Remove(dtResult.Columns[5].ColumnName);//刪除後面5欄
         }

         return dtResult;
      }

      /// <summary>
      /// sheet=2 本日結算保證金計算
      /// </summary>
      /// <returns>MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_CP_CM</returns>
      public DataTable GetOptR2Data(DateTime as_date)
      {
         DataTable dtResult = GetOptData(as_date, SheetType.R2);
         //import 商品資料
         dtResult.Columns.Remove(dtResult.Columns["R2"]);
         dtResult.Columns.Remove(dtResult.Columns["MG1_TYPE"]);
         for (int k = 0; k < 5; k++)
         {
            dtResult.Columns.Remove(dtResult.Columns[0].ColumnName);//刪除前面6欄
         }

         return dtResult;
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
         return dtResult.Rows[0][0].AsInt();
      }

   }

   /// <summary>
   /// 選取R1 或 R2
   /// </summary>
   public enum SheetType
   {
      [Description("R1")]
      R1 = 1,
      [Description("R2")]
      R2 = 2
   }

   public interface ID4001x
   {
      string FutDataSql(SheetType R);

      string OptDataSql(SheetType R);

      string FutDataCountSql();

      string OptDataCountSql();

      string WorkItemSql(int Num);

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
      /// sheet=1 現行收取保證金金額
      /// </summary>
      DataTable GetFutR1Data(DateTime as_date);

      /// <summary>
      /// sheet=1 本日結算保證金計算
      /// </summary>
      DataTable GetFutR2Data(DateTime as_date);

      /// <summary>
      /// sheet=2 現行收取保證金金額
      /// </summary>
      DataTable GetOptR1Data(DateTime as_date);

      /// <summary>
      /// sheet=2 本日結算保證金計算
      /// </summary>
      DataTable GetOptR2Data(DateTime as_date);

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

   }

}
