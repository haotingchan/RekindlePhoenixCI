using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
/// <summary>
/// john,20190412,保證金狀況表 (Group 1/2/3)
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 保證金狀況表 (Group 1/2/3)
   /// </summary>
   public class B4001xTemplate : I4001x
   {
      /// <summary>
      /// 輸出Excel檔案路徑
      /// </summary>
      protected string _lsFile;
      /// <summary>
      /// 輸入的日期 yyyy/MM/dd
      /// </summary>
      protected string _emDateText;
      /// <summary>
      /// Data
      /// </summary>
      protected ID4001x dao;
      /// <summary>
      /// 程式代號(_ProgramID)
      /// </summary>
      protected string _TxnID;

      public B4001xTemplate()
      {
      }

      public I4001x ConcreteClass(string programID, object[] args = null)
      {

         //string className = string.Format("{0}.Dao.Together.SpecificDao.{1}",AssemblyName, name);//完整的class路徑

         string AssemblyName = GetType().Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = GetType().FullName.Replace("B4001xTemplate", "B" + programID);//完整的class路徑

         // 這裡就是Reflection，直接依照className實體化具體類別
         return (I4001x)Assembly.Load(AssemblyName).CreateInstance(className, true, BindingFlags.CreateInstance, null, args, null, null);
      }

      public enum SheetName : int
      {
         /// <summary>
         /// rpt_future
         /// </summary>
         Rpt_Future = 0,
         /// <summary>
         /// rpt_option
         /// </summary>
         Rpt_Option = 1
      }
      #region 共用方法
      /// <summary>
      /// 作業項目共用方法
      /// </summary>
      /// <param name="emdate"></param>
      /// <param name="Sheet"></param>
      /// <returns></returns>
      private Dictionary<string, string> WorkItem(DateTime emdate, int Sheet)
      {
         string itemOne = string.Empty;
         string itemTwo = string.Empty;
         dao.WorkItem(emdate, Sheet).AsEnumerable().ToList().ForEach(dr => {
            itemOne += dr.Field<string>("N10P").AsString() + "　";
            itemTwo += dr.Field<string>("R10P").AsString() + "　";
         });
         Dictionary<string, string> dic = new Dictionary<string, string>()
        {
            {"ItemOne", itemOne}, {"ItemTwo", itemTwo}
        };
         return dic;
      }

      /// <summary>
      /// 判斷FMIF資料已轉入
      /// </summary>
      /// <returns></returns>
      public string CheckFMIF()
      {
         //判斷FMIF資料已轉入
         DateTime inputDT = _emDateText.AsDateTime();
         int cnt = dao.CheckFMIF(inputDT, GetOswGrp());
         if (cnt == 0) {
            return _emDateText + "期貨結算價資料未轉入完畢,是否要繼續?";
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// 130批次作業做完
      /// </summary>
      /// <returns></returns>
      public string Check130Wf()
      {
         DateTime inputDT = _emDateText.AsDateTime();
         string strRtn = PbFunc.f_chk_130_wf(_TxnID, inputDT, GetOswGrp());
         if (!string.IsNullOrEmpty(strRtn)) {
            return $"{_emDateText}-{strRtn}，是否要繼續?";
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// FMIF APDK_MARKET_CLOSE 值
      /// </summary>
      /// <returns>1 or 5 or 7</returns>
      protected virtual string GetOswGrp()
      {
         return "1";
      }
      #endregion


      #region 期貨


      /// <summary>
      /// 寫入 現行收取保證金金額 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR1"></param>
      protected virtual void WriteFutR1Data(Worksheet worksheet, DataTable dtR1)
      {
         //worksheet.Import(dtR1, false, , );
      }

      /// <summary>
      /// 寫入 本日結算保證金計算 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR2"></param>
      protected virtual void WriteFutR2Data(Worksheet worksheet, DataTable dtR2)
      {
         //worksheet.Import(dtR2, false, , );
      }

      /// <summary>
      /// rpt_future工作表 兩筆作業項目儲存格間距
      /// </summary>
      /// <returns></returns>
      protected virtual int FutWorkItemCellDist()
      {
         return 3;
      }

      /// <summary>
      /// sheet=1 現行收取保證金金額
      /// </summary>
      /// <returns>MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE</returns>
      private DataTable GetFutR1Data(DateTime as_date)
      {
         DataTable dtResult = dao.ListFutData(as_date, 1);
         //import 商品資料
         dtResult.Columns.Remove(dtResult.Columns["R1"]);
         for (int k = 6; k < 12; k++) {
            dtResult.Columns.Remove(dtResult.Columns[6].ColumnName);//刪除後面6欄
         }
         return dtResult;
      }

      /// <summary>
      /// sheet=1 本日結算保證金計算
      /// </summary>
      /// <returns>MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM</returns>
      private DataTable GetFutR2Data(DateTime as_date)
      {
         DataTable dtResult = dao.ListFutData(as_date, 2);
         //import 商品資料
         dtResult.Columns.Remove(dtResult.Columns["R2"]);
         for (int k = 0; k < 6; k++) {
            dtResult.Columns.Remove(dtResult.Columns[0].ColumnName);//刪除前面6欄
         }
         return dtResult;
      }

      #endregion

      #region 選擇權

      /// <summary>
      /// 寫入 現行金額 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR1"></param>
      protected virtual void WriteOptR1Data(Worksheet worksheet, DataTable dtR1)
      {
         //worksheet.Import(dtR1, false, 8, 2);
      }

      /// <summary>
      /// 寫入 本日結算保證金之適用風險保證金 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR2"></param>
      protected virtual void WriteOptR2Data(Worksheet worksheet, DataTable dtR2)
      {
         //worksheet.Import(dtR2, false, 48, 3);
      }

      /// <summary>
      /// rpt_option工作表 兩筆作業項目儲存格間距
      /// </summary>
      /// <returns></returns>
      protected virtual int OptWorkItemCellDist()
      {
         return 2;
      }

      /// <summary>
      /// sheet=2 現行收取保證金金額
      /// </summary>
      /// <returns>MG1_CUR_CM,'',MG1_CUR_MM,'',MG1_CUR_IM</returns>
      private DataTable GetOptR1Data(DateTime as_date)
      {
         DataTable dtResult = dao.ListOptData(as_date, 1);
         //import 商品資料
         dtResult.Columns.Remove(dtResult.Columns["R1"]);
         dtResult.Columns.Remove(dtResult.Columns["MG1_TYPE"]);
         for (int k = 5; k < 10; k++) {
            dtResult.Columns.Remove(dtResult.Columns[5].ColumnName);//刪除後面5欄
         }

         return dtResult;
      }

      /// <summary>
      /// sheet=2 本日結算保證金計算
      /// </summary>
      /// <returns>MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_CP_CM</returns>
      private DataTable GetOptR2Data(DateTime as_date)
      {
         DataTable dtResult = dao.ListOptData(as_date, 2);
         //import 商品資料
         dtResult.Columns.Remove(dtResult.Columns["R2"]);
         dtResult.Columns.Remove(dtResult.Columns["MG1_TYPE"]);
         for (int k = 0; k < 5; k++) {
            dtResult.Columns.Remove(dtResult.Columns[0].ColumnName);//刪除前面6欄
         }

         return dtResult;
      }

      #endregion

      /// <summary>
      /// sheet rpt_future
      /// </summary>
      /// <returns></returns>
      public string WfFutureSheet(int sheetIndex = 0)
      {
         try {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[sheetIndex];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["G1"].Value = "資料日期：" + emdate.ToLongDateString().ToString();
            const int SheetOne = 1;//第一張sheet

            //確認有無資料
            int SheetFutDataCount = dao.FutR1DataCount(emdate);
            if (SheetFutDataCount <= 0) {
               return $"{_emDateText},{_TxnID}_1－保證金狀況表,無任何資料!";
            }

            //一、現行收取保證金金額：CDEFGH
            DataTable dtR1 = GetFutR1Data(emdate);
            WriteFutR1Data(worksheet, dtR1);

            //二、	本日結算保證金計算：CDEFGH
            DataTable dtR2 = GetFutR2Data(emdate);
            WriteFutR2Data(worksheet, dtR2);

            //四、	作業事項
            int itemRowIndex = dao.GetRptLV(_TxnID, SheetOne);
            if (itemRowIndex > 0) {
               Dictionary<string, string> dic = WorkItem(emdate, SheetOne);
               int dist = FutWorkItemCellDist();
               worksheet.Cells[$"B{itemRowIndex}"].Value = dic["ItemOne"];
               worksheet.Cells[$"B{itemRowIndex + dist}"].Value = dic["ItemTwo"];
            }

            //save
            worksheet.ScrollTo(0, 0);
            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"WfFutureSheet:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// sheet rpt_option
      /// </summary>
      /// <returns></returns>
      public string WfOptionSheet(int sheetIndex = 1)
      {
         try {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[sheetIndex];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["G5"].Value = "資料日期：" + emdate.ToLongDateString().ToString();
            const int SheetTwo = 2;//第二張sheet

            //確認有無資料
            int SheetOptDataCount = dao.FutR1DataCount(emdate);
            if (SheetOptDataCount <= 0) {
               return $"{_emDateText},{_TxnID}_2－保證金狀況表,無任何資料!";
            }

            //一、現行收取保證金金額：CDEFGH
            DataTable dtR1 = GetOptR1Data(emdate);
            WriteOptR1Data(worksheet, dtR1);

            //二、	本日結算保證金計算：CDEFGH
            DataTable dtR2 = GetOptR2Data(emdate);
            WriteOptR2Data(worksheet, dtR2);

            //四、	作業事項

            int itemRowIndex = dao.GetRptLV(_TxnID, SheetTwo);
            if (itemRowIndex > 0) {
               Dictionary<string, string> dic = WorkItem(emdate, SheetTwo);
               int dist = OptWorkItemCellDist();
               worksheet.Cells[$"B{itemRowIndex}"].Value = dic["ItemOne"];
               worksheet.Cells[$"B{itemRowIndex + dist}"].Value = dic["ItemTwo"];
            }

            //save
            worksheet.ScrollTo(0, 0);
            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"WfOptionSheet:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }

   }

   public interface I4001x
   {

      /// <summary>
      /// 判斷FMIF資料已轉入
      /// </summary>
      string CheckFMIF();

      /// <summary>
      /// 130批次作業做完
      /// </summary>
      string Check130Wf();

      /// <summary>
      /// sheet rpt_future
      /// </summary>
      /// <param name="sheetIndex">工作表</param>
      /// <returns></returns>
      string WfFutureSheet(int sheetIndex = 0);

      /// <summary>
      /// sheet rpt_option
      /// </summary>
      /// <param name="sheetIndex">工作表</param>
      /// <returns></returns>
      string WfOptionSheet(int sheetIndex = 1);

   }



}
