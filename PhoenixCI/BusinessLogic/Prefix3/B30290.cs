using Common;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;
using DevExpress.Spreadsheet;
using BaseGround.Shared;
using System.IO;
using DevExpress.XtraEditors;
/// <summary>
/// john,20190329,每月報局交易量報表(國內期貨暨選擇權)
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 每月報局交易量報表(國內期貨暨選擇權)
   /// </summary>
   public class B30290
   {
      private readonly string _lsFile;
      private readonly string _emDateText;
      private readonly string _lsMarketCode;
      private readonly DateTime _emEndDateText;
      private D30290 dao30290;

      public B30290()
      {
         dao30290 = new D30290();
      }

      public B30290(string lsFile, string emDate,string MarketCode, DateTime emEndDate)
      {
         _lsFile = lsFile;
         _emDateText = emDate;
         _lsMarketCode = MarketCode;
         _emEndDateText = emEndDate;
         dao30290 = new D30290();
      }

      public DataTable GetEffectiveYMD(string emDateText)
      {
         return dao30290.ListEffectiveYmd(emDateText.Replace("/",""));
      }

      /// <summary>
      /// //取得前一季月份
      /// </summary>
      /// <returns></returns>
      public string LastQuarter(string emDateText)
      {

         return "";
      }

      /// <summary>
      /// wf_30780_1()
      /// </summary>
      /// <returns></returns>
      public string WF30780one()
      {
         try {

            return MessageDisplay.MSG_OK;
         }
         catch (Exception ex) {
            File.Delete(_lsFile);
#if DEBUG
            throw new Exception("WF30780one:" + ex.Message);
#else
            throw ex;
#endif
         }
      }

   }
}