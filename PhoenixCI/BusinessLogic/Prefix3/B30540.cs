using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Linq;
/// <summary>
/// 20190318,john,國內股價指數選擇權交易概況表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 國內股價指數選擇權交易概況表
   /// </summary>
   public class B30540
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
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30540(string FilePath, string datetime)
      {
         _lsFile = FilePath;
         _emMonthText = datetime;
      }

      /// <summary>
      /// 判斷 期貨自營/期貨經紀 買權(Calls)或賣權(Puts)的欄位
      /// </summary>
      /// <param name="row">datarow AM2_PC_CODE</param>
      /// <returns></returns>
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

      /// <summary>
      /// 判斷要填入未沖銷契約數買權(Calls)或賣權(Puts)的欄位
      /// </summary>
      /// <param name="RowIndex"></param>
      /// <param name="worksheet"></param>
      /// <param name="dtAI2"></param>
      /// <param name="lsYMD"></param>
      /// <param name="pcCode">C(Calls)/P(Puts)</param>
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
      /// wf_30541
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public string Wf30541(int RowIndex = 6, string RptName = "國內股價指數選擇權交易概況表")
      {
         Workbook workbook = new Workbook();
         try {

            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Range["A1"].Select();

            //總列數,隱藏於A5
            int rowTol = RowIndex + worksheet.Cells["A5"].Value.AsInt();

            //起始年份,隱藏於B5
            string firstYear = worksheet.Cells["B5"].Value.AsString();

            /******************
               讀取資料
               分三段：
               1.年
               2.當年1月至當月合計
               3.當年1月至當月明細
            ******************/
            DataTable dt = new D30540().List30541(firstYear, PbFunc.Left(_emMonthText, 4), $"{PbFunc.Left(_emMonthText, 4)}01", _emMonthText.Replace("/", ""));
            if (dt.Rows.Count <= 0) {
               return $"{firstYear}~{PbFunc.Left(_emMonthText, 4)},{PbFunc.Left(_emMonthText, 4)}01～{_emMonthText.Replace("/", "")},30541－{RptName},無任何資料!";
            }
            /* 成交量 & OI */
            DataTable dtAI2 = new D30540().List30541AI2(firstYear, PbFunc.Left(_emMonthText, 4), $"{PbFunc.Left(_emMonthText, 4)}01", _emMonthText.Replace("/", ""));
            if (dtAI2.Rows.Count <= 0) {
               return $"{firstYear}~{PbFunc.Left(_emMonthText, 4)},{PbFunc.Left(_emMonthText, 4)}01～{_emMonthText.Replace("/", "")},30541－{RptName},無任何資料!";
            }

            //寫入內容
            string lsYMD = "";
            int rowEndIndex = 0;
            string endYMD = dt.AsEnumerable().LastOrDefault()["AM2_YMD"].AsString();
            foreach (DataRow row in dt.Rows) {
               if (lsYMD != row["AM2_YMD"].AsString()) {
                  lsYMD = row["AM2_YMD"].AsString();
                  RowIndex = RowIndex + 1;

                  /* 最後一列時 */
                  if (lsYMD == endYMD) {
                     rowEndIndex = RowIndex;
                     RowIndex = rowTol;
                  }
                  /* 年度資料 */
                  if (lsYMD.Length == 4) {
                     worksheet.Rows[RowIndex][1 - 1].Value = PbFunc.Left(lsYMD, 4).AsInt();
                  }
                  else {
                     worksheet.Rows[RowIndex][1 - 1].Value = PbFunc.f_get_month_eng_name(PbFunc.Right(lsYMD, 2).AsInt(), "1");
                  }//if (lsYMD.Length==4)
                  /* 成交量 & OI */

                  //ai2_pc_code=C 
                  OImethod(RowIndex, worksheet, dtAI2, lsYMD, "C");
                  //ai2_pc_code=P
                  OImethod(RowIndex, worksheet, dtAI2, lsYMD, "P");
               }//if (lsYMD != row["AM2_YMD"].AsString())

               //判斷欄位
               int columnIndex = IDFGtype(row);
               if (row["AM2_YMD"].AsString()!= endYMD) {
                  worksheet.Rows[RowIndex][columnIndex].Value = row["AM2_M_QNTY"].AsDecimal();
               }
               else {
                  worksheet.Rows[rowTol][columnIndex].Value = row["AM2_M_QNTY"].AsDecimal();
               }
               
            }//foreach (DataRow row in dt.Rows)

            //刪除空白列
            if (rowTol > rowEndIndex) {
               worksheet.Range[$"{rowEndIndex + 1}:{rowTol + 1 - 1}"].Delete(DeleteMode.EntireRow);
               worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            }
            
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"wf30541:" + ex.Message);
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
