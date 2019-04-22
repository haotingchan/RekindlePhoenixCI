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
/// john,20190422,收盤前保證金試算表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 收盤前保證金試算表
   /// </summary>
   public class B40042: B4001xTemplate
   {

      private readonly D40042 dao40042;

      public B40042(string ProgramID, string FilePath, string emDate)
      {
         this._TxnID = ProgramID;
         this._lsFile = FilePath;
         this._emDateText = emDate;
         this.dao = new D4001x().ConcreteDao(ProgramID);
         dao40042 = new D40042();
      }

      /// <summary>
      /// FMIF APDK_MARKET_CLOSE 值
      /// </summary>
      /// <returns>1 or 5 or 7</returns>
      public override string GetOswGrp()
      {
         return "1";
      }

      /// <summary>
      /// 寫入 現行收取保證金金額 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR1"></param>
      public override void WriteFutR1Data(Worksheet worksheet, DataTable dtR1)
      {
         worksheet.Import(dtR1.AsEnumerable().Take(1).CopyToDataTable(), false, 2, 2);
         ////跳過第4行
         worksheet.Import(dtR1.AsEnumerable().Skip(1).CopyToDataTable(), false, 4, 2);
      }

      /// <summary>
      /// 寫入 本日結算保證金計算 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR2"></param>
      public override void WriteFutR2Data(Worksheet worksheet, DataTable dtR2)
      {
         //要Take的筆數
         int takeRow = dtR2.Rows.Count - 1;
         //dtR2最後一筆是 excel第49行 十年期公債期貨契約結算保證金
         worksheet.Import(dtR2.AsEnumerable().Take(takeRow).CopyToDataTable(), false, 35, 2);
         //第46行E~H欄格式設定
         for (int k = 4; k <= 7; k++)
         {
            worksheet.Rows[45][k].SetValueFromText("-");
         }
         //excel第49行 十年期公債期貨契約結算保證金
         worksheet.Import(dtR2.AsEnumerable().Skip(takeRow).CopyToDataTable(), false, 48, 2);
      }


      /// <summary>
      /// 寫入 現行金額 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR1"></param>
      public override void WriteOptR1Data(Worksheet worksheet, DataTable dtR1)
      {
         worksheet.Import(dtR1, false, 8, 2);
      }

      /// <summary>
      /// 寫入 本日結算保證金之適用風險保證金 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR2"></param>
      public override void WriteOptR2Data(Worksheet worksheet, DataTable dtR2)
      {
         worksheet.Import(dtR2, false, 48, 3);
      }

      public string Wf40042()
      {
         DataTable dt = dao40042.List40042Mg1();
         return MessageDisplay.MSG_OK;
      }

      public string Wf43032()
      {
         DataTable dt = dao40042.List40042Head();
         return MessageDisplay.MSG_OK;
      }

   }
}
