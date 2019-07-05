using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
/// <summary>
/// 20190318,john,國內期貨交易概況表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 國內期貨交易概況表
   /// </summary>
   public class B30520
   {
      /// <summary>
      /// 檔案輸出路徑
      /// </summary>
      private readonly string _lsFile;
      /// <summary>
      /// 交易日期 月份
      /// </summary>
      private readonly string _emMonthText;
      /// <summary>
      /// DataLayer
      /// </summary>
      private readonly D30520 dao30520;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30520(string FilePath, string datetime)
      {
         _lsFile = FilePath;
         _emMonthText = datetime;
         dao30520 = new D30520();
      }

      /// <summary>
      /// 判斷要填入買或賣的欄位
      /// </summary>
      /// <param name="row">datarow AM2_BS_CODE</param>
      /// <returns></returns>
      private static int IDFGtype(DataRow row)
      {
         int columnIndex = 0;
         string codeCol = "AM2_BS_CODE";
         string code = "B";
         if (row["AM2_IDFG_TYPE"].AsString() == "A")
         {
            /* 期貨自營 */
            columnIndex = (row[codeCol].AsString() == code ? 2 : 3) - 1;
         }
         else
         {
            /* 期貨經紀 */
            columnIndex = (row[codeCol].AsString() == code ? 4 : 5) - 1;
         }

         return columnIndex;
      }

      /// <summary>
      /// wf_30521
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public string Wf30521(int RowIndex = 4, string RptName = "國內期貨交易概況表")
      {
         Workbook workbook = new Workbook();
         try
         {
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[0];//30520

            //起始年份,隱藏於B3
            string firstYear = worksheet.Cells["B3"].Value.AsString();

            /******************
               讀取資料
               分三段：
               1.年
               2.當年1月至當月合計
               3.當年1月至當月明細
            ******************/
            DataTable dt = dao30520.List30521(firstYear, PbFunc.Left(_emMonthText, 4), $"{PbFunc.Left(_emMonthText, 4)}01", _emMonthText.Replace("/", ""));
            if (dt.Rows.Count <= 0)
            {
               return $"{firstYear}~{PbFunc.Left(_emMonthText, 4)},{PbFunc.Left(_emMonthText, 4)}01～{_emMonthText.Replace("/", "")},30521－{RptName},無任何資料!";
            }
            /* 成交量 & OI */
            DataTable dtAI2 = dao30520.List30521AI2(firstYear, PbFunc.Left(_emMonthText, 4), $"{PbFunc.Left(_emMonthText, 4)}01", _emMonthText.Replace("/", ""));
            if (dtAI2.Rows.Count <= 0)
            {
               return $"{firstYear}~{PbFunc.Left(_emMonthText, 4)},{PbFunc.Left(_emMonthText, 4)}01～{_emMonthText.Replace("/", "")},30521－{RptName},無任何資料!";
            }

            //寫入內容
            string lsYMD = "";
            int addCount = 0;//計算新增row的數量

            foreach (DataRow row in dt.Rows)
            {
               if (lsYMD != row["AM2_YMD"].AsString())
               {
                  lsYMD = row["AM2_YMD"].AsString();
                  RowIndex = RowIndex + 1;
                  addCount++;

                  /* 年度資料 */
                  if (lsYMD.Length == 4)
                  {
                     worksheet.Cells[$"A{RowIndex + 1}"].Value = PbFunc.Left(lsYMD, 4).AsInt();
                  }
                  else
                  {
                     worksheet.Cells[$"A{RowIndex + 1}"].Value = PbFunc.f_get_month_eng_name(PbFunc.Right(lsYMD, 2).AsInt(), "1");
                  }//if (lsYMD.Length==4)

                  /* 成交量 & OI */
                  int foundIndex = dtAI2.Rows.IndexOf(dtAI2.Select($@"ai2_ymd ='{lsYMD}'").FirstOrDefault());
                  if (foundIndex > -1)
                  {
                     worksheet.Cells[$"F{RowIndex + 1}"].SetValue(dtAI2.Rows[foundIndex]["AI2_M_QNTY"]);//成交契約總數 Total Trading Volume
                     worksheet.Cells[$"G{RowIndex + 1}"].SetValue(dtAI2.Rows[foundIndex]["AI2_OI"]);//未沖銷契約數  Open Interest
                  }

               }//if (lsYMD != row["AM2_YMD"].AsString())

               //判斷欄位
               int columnIndex = IDFGtype(row);
               worksheet.Rows[RowIndex][columnIndex].SetValue(row["AM2_M_QNTY"]);//買進 Long or 賣出 Short

            }//foreach (DataRow row in dt.Rows)

            //刪除空白列
            //總列數,隱藏於A3
            int rowTotal = worksheet.Cells["A3"].Value.AsInt();
            if (rowTotal > addCount)
            {
               worksheet.Rows.Remove(RowIndex + 1, rowTotal - addCount);
               //表格最後一行設回原本的框線
               int lineIndex = RowIndex + 1;
               worksheet.Range[$"A{lineIndex}:G{lineIndex}"].Borders.BottomBorder.LineStyle = BorderLineStyle.Thin;
               worksheet.Range[$"A{lineIndex}:G{lineIndex}"].Borders.BottomBorder.Color = Color.Black;
               worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            }
            
         }
         catch (Exception ex)
         {
#if DEBUG
            throw new Exception($"wf30520:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }

         return MessageDisplay.MSG_OK;
      }


   }
}
