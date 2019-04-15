﻿using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
/// <summary>
/// john,20190415,保證金狀況表 (Group3)
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 保證金狀況表 (Group3)
   /// </summary>
   public class B40013: B4001xTemplate
   {

      public B40013(string ProgramID, string FilePath, string emDate)
      {
         this._TxnID = ProgramID;
         this._lsFile = FilePath;
         this._emDateText = emDate;
         this.dao = new D4001x().ConcreteDao(ProgramID);
      }

      /// <summary>
      /// FMIF APDK_MARKET_CLOSE 值
      /// </summary>
      /// <returns>1 or 5 or 7</returns>
      public override string GetOswGrp()
      {
         return "7";
      }

      /// <summary>
      /// 寫入 現行收取保證金金額 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR1"></param>
      public override void WriteFutR1Data(Worksheet worksheet, DataTable dtR1)
      {
         worksheet.Import(dtR1, false, 2, 2);
      }

      /// <summary>
      /// 寫入 本日結算保證金計算 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR2"></param>
      public override void WriteFutR2Data(Worksheet worksheet, DataTable dtR2)
      {
         worksheet.Import(dtR2, false, 23, 2);
      }

   }
}
