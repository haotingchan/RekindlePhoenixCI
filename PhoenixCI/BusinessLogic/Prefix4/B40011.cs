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
/// john,20190410,保證金狀況表 (Group1)
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 保證金狀況表 (Group1)
   /// </summary>
   public class B40011 : B4001xTemplate
   {

      public B40011(string daoID, string FilePath, string emDate)
      {
         this._TxnID = "40011";
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
         return "1";
      }

      /// <summary>
      /// 寫入 現行收取保證金金額 資料表
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dtR1"></param>
      protected override void WriteFutR1Data(Worksheet worksheet, DataTable dtR1)
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
      protected override void WriteFutR2Data(Worksheet worksheet, DataTable dtR2)
      {
         //要Take的筆數
         int takeRow = dtR2.Rows.Count - 1;
         //dtR2最後一筆是 excel第49行 十年期公債期貨契約結算保證金
         worksheet.Import(dtR2.AsEnumerable().Take(takeRow).CopyToDataTable(), false, 35, 2);
         //第46行E~H欄格式設定
         for (int k = 4; k <= 7; k++) {
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
      /// wf_40011_3
      /// </summary>
      /// <param name="AsProdType">O or F</param>
      /// <param name="SheetName">工作表名稱</param>
      /// <returns></returns>
      public string WfStat(string AsProdType, string SheetName)
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["A1"].Value = "資料日期：\r\n" + emdate.ToLongDateString().ToString();
            worksheet.Cells["A1"].Alignment.WrapText = true;
            //確認有無資料
            DataTable dt = new D40011().List40011Stat(_emDateText.Replace("/", "")).Sort("seq_no,kind_id ");
            if (dt.Rows.Count <= 0) {
               return $"{_emDateText},40011_stat－保證金狀況表,無任何資料!";
            }
            dt = dt.Filter($"prod_type ='{AsProdType}' and prod_subtype <> 'S' and osw_grp = '1'");
            //共38欄位 取前33欄位
            for (int k = 0; k < 5; k++) {
               dt.Columns.Remove(dt.Columns[33].ColumnName);//刪除後面5欄
            }
            //寫入資料
            worksheet.Import(dt, false, 2, 0);

            //save
            worksheet.ScrollTo(0, 0);
            //workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"WfStat:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            //save
            workbook.SaveDocument(_lsFile);
         }
         return MessageDisplay.MSG_OK;
      }
   }
}
