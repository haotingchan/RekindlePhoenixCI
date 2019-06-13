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
         this._workbook = new Workbook();
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
      /// rpt_option工作表 兩筆作業項目儲存格間距
      /// </summary>
      /// <returns></returns>
      protected override int OptWorkItemCellDist()
      {
         return 3;
      }

   }
}
