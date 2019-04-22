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
   public class B40042
   {

      private readonly D40042 dao40042;
      /// <summary>
      /// 輸出Excel檔案路徑
      /// </summary>
      private readonly string _lsFile;
      /// <summary>
      /// 輸入的日期 yyyy/MM/dd
      /// </summary>
      private readonly string _emDateText;

      private readonly B40011 b40011;

      private readonly B40012 b40012;

      private readonly B40013 b40013;

      public B40042(string FilePath, string emDate)
      {
         this._lsFile = FilePath;
         this._emDateText = emDate;
         dao40042 = new D40042();
         b40011 = new B40011("40042_40011", FilePath, emDate);
         b40012 = new B40012("40042_40012", FilePath, emDate);
         b40013 = new B40013("40042_40013", FilePath, emDate);
      }

      public string Wf40011Fut()
      {
         return b40011.WfFutureSheet();
      }

      public string Wf40011Opt()
      {
         return b40011.WfOptionSheet();
      }

      public string Wf40012Fut()
      {
         return b40012.WfFutureSheet();
      }

      public string Wf40012Opt()
      {
         return b40012.WfOptionSheet();
      }

      public string Wf40013Fut()
      {
         return b40013.WfFutureSheet();
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
