using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using System;
using System.Data;
using System.Linq;
/// <summary>
/// 20190318,john,國內股票選擇權交易概況表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 國內股票選擇權交易概況表
   /// </summary>
   public class B30560
   {
      private string lsFile;
      private string emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30560(string FilePath, string datetime)
      {
         lsFile = FilePath;
         emMonthText = datetime;
      }

      private static int IDFGtype(DataRow row)
      {
         int columnIndex = 0;
         string codeCol = "AM2_PC_CODE";
         string code = "C";
         if (row["AM2_IDFG_TYPE"].AsString() == "A") {
            /* 期貨自營 */
            columnIndex = (row[codeCol].AsString() == code ? 2 : 3) - 1;
         }
         else {
            /* 期貨經紀 */
            columnIndex = (row[codeCol].AsString() == code ? 4 : 5) - 1;
         }

         return columnIndex;
      }

      private static void OImethod(int RowIndex, Worksheet worksheet, DataTable dtAI2, string lsYMD, string pcCode)
      {
         int foundIndex = dtAI2.Rows.IndexOf(dtAI2.Select($@"ai2_ymd ='{lsYMD}' and ai2_pc_code='{pcCode}'").FirstOrDefault());
         if (foundIndex > -1) {
            //iole_1.application.activecell(ii_ole_row,6).value = ids_tmp.getitemdecimal(ll_found,"cp_m_qnty")
            worksheet.Rows[RowIndex][6 - 1].Value = dtAI2.Compute("sum(AI2_M_QNTY)", $@"AI2_SUM_TYPE='{dtAI2.Rows[foundIndex]["AI2_SUM_TYPE"]}' and AI2_YMD='{dtAI2.Rows[foundIndex]["AI2_YMD"]}'").AsDecimal();
            int colIndex = 0;
            switch (pcCode) {
               case "C":
                  colIndex = 7 - 1;
                  break;
               case "P":
                  colIndex = 8 - 1;
                  break;
            }
            worksheet.Rows[RowIndex][colIndex].Value = dtAI2.Rows[foundIndex]["AI2_OI"].AsDecimal();
         }
      }

      /// <summary>
      /// wf_30561
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public string Wf30561(int RowIndex = 6, string RptName = "國內股票選擇權及黃金選擇權交易概況表")
      {
         string flowStepDesc = "開始轉出資料";
         try {

            //切換Sheet
            flowStepDesc = "切換Sheet";
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Range["A1"].Select();

            //總列數,隱藏於A5
            int rowTotal = RowIndex + worksheet.Cells["A5"].Value.AsInt();

            //起始年份,隱藏於B5
            string firstYear = worksheet.Cells["B5"].Value.AsString();

            /******************
               讀取資料
               分三段：
               1.年
               2.當年1月至當月合計
               3.當年1月至當月明細
            ******************/
            flowStepDesc = "讀取資料";
            DataTable dt = new D30560().List30561(firstYear, PbFunc.Left(emMonthText, 4), $"{PbFunc.Left(emMonthText, 4)}01", emMonthText.Replace("/", ""));
            if (dt.Rows.Count <= 0) {
               return $"{firstYear}~{PbFunc.Left(emMonthText, 4)},{PbFunc.Left(emMonthText, 4)}01～{emMonthText.Replace("/", "")},30561－{RptName},無任何資料!";
            }
            /* 成交量 & OI */
            flowStepDesc = "讀取成交量資料";
            DataTable dtAI2 = new D30560().List30561AI2(firstYear, PbFunc.Left(emMonthText, 4), $"{PbFunc.Left(emMonthText, 4)}01", emMonthText.Replace("/", ""));
            if (dtAI2.Rows.Count <= 0) {
               return $"{firstYear}~{PbFunc.Left(emMonthText, 4)},{PbFunc.Left(emMonthText, 4)}01～{emMonthText.Replace("/", "")},30561－{RptName},無任何資料!";
            }

            //寫入內容
            flowStepDesc = "寫入內容";
            string lsYMD = "";
            int rowCurr = 0;
            string endYMD = dt.AsEnumerable().LastOrDefault()["AM2_YMD"].AsString();
            foreach (DataRow row in dt.Rows) {
               if (lsYMD != row["AM2_YMD"].AsString()) {
                  lsYMD = row["AM2_YMD"].AsString();
                  RowIndex = RowIndex + 1;

                  if (lsYMD.Length == 4) {
                     worksheet.Rows[RowIndex][1 - 1].Value = PbFunc.Left(lsYMD, 4).AsInt();
                     rowCurr = RowIndex;
                  }
                  else {
                     rowCurr = lsYMD != endYMD ? RowIndex : rowTotal;
                     worksheet.Rows[rowCurr][1 - 1].Value = PbFunc.f_get_month_eng_name(PbFunc.Right(lsYMD, 2).AsInt(), "1");
                  }//if (lsYMD.Length==4)
                  /* 成交量 & OI */

                  //ai2_pc_code=C 
                  OImethod(rowCurr, worksheet, dtAI2, lsYMD, "C");
                  //ai2_pc_code=P
                  OImethod(rowCurr, worksheet, dtAI2, lsYMD, "P");

               }//if (lsYMD != row["AM2_YMD"].AsString())

               //判斷欄位
               int columnIndex = IDFGtype(row);
               worksheet.Rows[rowCurr][columnIndex].Value = row["AM2_M_QNTY"].AsDecimal();

            }//foreach (DataRow row in dt.Rows)

            //刪除空白列
            flowStepDesc = "刪除空白列";
            if (rowTotal > RowIndex) {
               worksheet.Range[$"{RowIndex + 1}:{rowTotal + 1 - 1}"].Delete(DeleteMode.EntireRow);
               worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            }
            workbook.SaveDocument(lsFile);
            return MessageDisplay.MSG_OK;
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"wf30561-{flowStepDesc}:" + ex.Message);
#else
            throw ex;
#endif
         }
      }

   }
}
