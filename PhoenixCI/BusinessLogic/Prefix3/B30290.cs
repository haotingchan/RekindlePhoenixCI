using Common;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;
using DevExpress.Spreadsheet;
using BaseGround.Shared;
using System.IO;
using DevExpress.XtraEditors;
using DataObjects.Dao.Together;
using BusinessObjects;
/// <summary>
/// john,20190424,轉出交易系統部位限制檔及公告表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 轉出交易系統部位限制檔及公告表
   /// </summary>
   public class B30290
   {
      private D30290 dao30290;
      private readonly string _txnID;

      public B30290(string ProgramID)
      {
         dao30290 = new D30290();
         _txnID = ProgramID;
      }

      public DataTable GetEffectiveYMD(string emDateText)
      {
         return dao30290.ListEffectiveYmd(emDateText.Replace("/", ""));
      }

      public DataTable ListInsertGridData(string isCalYMD,string isYMD)
      {
         return dao30290.ListInsertData(isCalYMD.Replace("/", ""), isYMD);
      }

      public DataTable List30290GridData(string isYMD)
      {
         return dao30290.List30290Data(isYMD);
      }

      public int DataCount(string YMD)
      {
         return dao30290.PLP13ListCount(YMD);
      }

      public bool DeleteData(string isYMD)
      {
         return dao30290.DeletePLP13(isYMD);
      }

      public ResultData UpdateData(DataTable inputData)
      {
         return dao30290.UpdatePLP13(inputData);
      }

      /// <summary>
      /// //取得前一季月份
      /// </summary>
      /// <returns></returns>
      public string LastQuarter(DateTime ocfYMD)
      {
         DateTime dateTime = ocfYMD;
         //取得前一季月份
         int month = dateTime.Month;
         int num = 0;
         if (month % 3 > 0) {
            num = (month / 3) + 1;
         }
         else if ((month % 3) == 0) {
            num = month / 3;
         }

         DateTime ymd = new DateTime();

         if (num == 1) {
            num = 4;
            month = num * 3;
            ymd = new DateTime(dateTime.Year-1, month, 1);
         }
         else {
            month = (num-1) * 3;
            ymd = new DateTime(dateTime.Year, month, 1);
         }

         //取得前一季月底最後交易日
         DateTime maxDate = new AOCF().GetMaxDate(ymd.ToString("yyyyMM01"), ymd.ToString("yyyyMM31"));
         return maxDate.ToString("yyyy/MM/dd");
      }

      /// <summary>
      /// WfExport
      /// </summary>
      /// <returns></returns>
      public string WfExport(string lsFile, string emDate)
      {
         
         try {
            
            return MessageDisplay.MSG_OK;
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("WfExport:" + ex.Message);
#else
            throw ex;
#endif
         }
      }

   }
}