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
/// 20190312,john,國內股價指數選擇權交易概況明細表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 國內股價指數選擇權交易概況明細表
   /// </summary>
   public class B30550
   {
      private string lsFile;
      private string emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="datetime">em_month.Text</param>
      public B30550(string FilePath,string datetime)
      {
         lsFile = FilePath;
         emMonthText = datetime;
      }

      private static int IDFGtype(DataRow row)
      {
         int columnIndex = 0;
         string codeCol = "AM2_PC_CODE";
         string code = "C";
         switch (row["AM2_IDFG_TYPE"].AsString()) {
            case "1":
               columnIndex = (row[codeCol].AsString() == code ? 2 : 3) - 1;
               break;                                           
            case "2":                                           
               columnIndex = (row[codeCol].AsString() == code ? 4 : 5) - 1;
               break;                                           
            case "3":                                           
               columnIndex = (row[codeCol].AsString() == code ? 6 : 7) - 1;
               break;                                           
            case "4":                                           
               columnIndex = (row[codeCol].AsString() == code ? 8 : 9) - 1;
               break;                                           
            case "5":                                           
               columnIndex = (row[codeCol].AsString() == code ? 10 : 11) - 1;
               break;                                           
            case "7":                                           
               columnIndex = (row[codeCol].AsString() == code ? 12 : 13) - 1;
               break;
         }

         return columnIndex;
      }

      /// <summary>
      /// wf_30550
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="RowTotal">Excel的Column預留數</param>
      /// <returns></returns>
      public bool Wf30550(int RowIndex = 4, string SheetName = "30550", string RptName = "國內股價指數選擇權交易概況明細表")
      {
         /*************************************
         ls_rpt_name = 報表名稱 
         ls_rpt_id = 報表代號
         RowIndex = Excel的Row位置
         columnIndex = Excel的Column位置
         RowTotal = Excel的Column預留數
         ii_ole_y_row_tol = Excel年部份的Column預留數
         li_month_cnt = Excel的月份個數
         lsYMD = 日期
         ls_end_ymd = 最後一筆日期
         *************************************/
         try {

            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];
            worksheet.Range["A1"].Select();

            //總列數,隱藏於A3
            int rowTotal = RowIndex + worksheet.Cells["A3"].Value.AsInt();

            //起始年份,隱藏於B3
            string firstYear = worksheet.Cells["B3"].Value.AsString();

            /******************
               讀取資料
               分三段：
               1.年
               2.當年1月至當月合計
               3.當年1月至當月明細
            ******************/
            DataTable dt = new D30550().GetData(firstYear, PbFunc.Left(emMonthText, 4), $"{PbFunc.Left(emMonthText, 4)}01", emMonthText.Replace("/", ""));
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info($"{firstYear}~{PbFunc.Left(emMonthText, 4)},{PbFunc.Left(emMonthText, 4)}01～{emMonthText.Replace("/", "")},{SheetName}－{RptName},無任何資料!");
               return true;
            }
            string lsYMD = "";
            string endYMD = dt.AsEnumerable().LastOrDefault()["AM2_YMD"].AsString();
            foreach (DataRow row in dt.Rows) {
               if (lsYMD != row["AM2_YMD"].AsString()) {
                  lsYMD = row["AM2_YMD"].AsString();
                  RowIndex = RowIndex + 1;

                  if (lsYMD.Length==4) {
                     worksheet.Rows[RowIndex][1 - 1].Value = PbFunc.Left(lsYMD, 4).AsInt();
                  }
                  else {
                     string getMonthEngName= PbFunc.f_get_month_eng_name(PbFunc.Right(lsYMD, 2).AsInt(), "1");
                     if (lsYMD != endYMD) {
                        worksheet.Rows[RowIndex][1 - 1].Value = getMonthEngName;
                     }
                     else {
                        worksheet.Rows[rowTotal][1 - 1].Value = getMonthEngName;
                     }
                  }//if (lsYMD.Length==4)

               }//if (lsYMD != row["AM2_YMD"].AsString())

               //判斷欄位
               int columnIndex = IDFGtype(row);
               if (row["AM2_YMD"].AsString() != endYMD) {
                  worksheet.Rows[RowIndex][columnIndex].Value = row["AM2_M_QNTY"].AsDecimal();
               }
               else {
                  worksheet.Rows[rowTotal][columnIndex].Value = row["AM2_M_QNTY"].AsDecimal();
               }
            }
            //刪除空白列
            if (rowTotal > RowIndex) {
               worksheet.Range[$"{RowIndex+1}:{rowTotal+1-1}"].Delete(DeleteMode.EntireRow);
               worksheet.ScrollTo(0,0);//直接滾動到最上面，不然看起來很像少行數
            }
            workbook.SaveDocument(lsFile);
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, "wf30550");
            return false;
         }
      }

      
   }
}
