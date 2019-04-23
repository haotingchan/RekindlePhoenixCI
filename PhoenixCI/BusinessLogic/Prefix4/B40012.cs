using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
/// <summary>
/// john,20190415,保證金狀況表 (Group2)
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 保證金狀況表 (Group2)
   /// </summary>
   public class B40012: B4001xTemplate
   {

      public B40012(string daoID, string FilePath, string emDate)
      {
         this._TxnID = "40012";
         this._lsFile = FilePath;
         this._emDateText = emDate;
         this.dao = new D4001x().ConcreteDao(daoID);
      }

      /// <summary>
      /// FMIF APDK_MARKET_CLOSE 值
      /// </summary>
      /// <returns>1 or 5 or 7</returns>
      protected override string GetOswGrp()
      {
         return "5";
      }

      /// <summary>
      /// 寫入 現行收取保證金金額 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR1"></param>
      protected override void WriteFutR1Data(Worksheet worksheet, DataTable dtR1)
      {
         worksheet.Import(dtR1, false, 2, 2);
      }

      /// <summary>
      /// 寫入 本日結算保證金計算 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR2"></param>
      protected override void WriteFutR2Data(Worksheet worksheet, DataTable dtR2)
      {
         worksheet.Import(dtR2, false, 32, 2);
      }


      /// <summary>
      /// 寫入 現行金額 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR1"></param>
      protected override void WriteOptR1Data(Worksheet worksheet, DataTable dtR1)
      {
         worksheet.Import(dtR1, false, 8, 2);
      }

      /// <summary>
      /// 寫入 本日結算保證金之適用風險保證金 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR2"></param>
      protected override void WriteOptR2Data(Worksheet worksheet, DataTable dtR2)
      {
         worksheet.Import(dtR2, false, 48, 3);
      }

      /// <summary>
      /// rpt_option工作表 兩筆作業項目儲存格間距
      /// </summary>
      /// <returns></returns>
      protected override int OptWorkItemCellDist()
      {
         return 3;
      }

   }
}
