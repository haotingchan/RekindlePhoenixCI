using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Charts;
using System;
using System.Data;
using System.Linq;
/// <summary>
/// 20190305,john,年度期間法人機構期貨交易量統計表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 年度期間法人機構期貨交易量統計表
   /// </summary>
   public class B30370
   {
      private readonly string _lsFile;
      private string _emMonthText;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      /// <param name="DatetimeVal">em_month.Text</param>
      public B30370(string FilePath,string DatetimeVal)
      {
         _lsFile = FilePath;
         _emMonthText = DatetimeVal;
      }

      private static int IDFGtype(DataRow row)
      {
         int columnIndex = 0;
         switch (row["AM2_IDFG_TYPE"].AsString()) {
            case "1":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 2 : 3) - 1;
               break;
            case "2":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 4 : 5) - 1;
               break;
            case "3":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 6 : 7) - 1;
               break;
            case "5":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 8 : 9) - 1;
               break;
            case "6":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 10 : 11) - 1;
               break;
            case "7":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 12 : 13) - 1;
               break;
            case "8":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 14 : 15) - 1;
               break;
            case "4":
               columnIndex = (row["AM2_BS_CODE"].AsString() == "B" ? 16 : 17) - 1;
               break;
         }//switch (row["AM2_IDFG_TYPE"].AsString())

         return columnIndex;
      }

      /// <summary>
      /// wf_30371()
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      public string Wf30371(int RowIndex = 3,  string SheetName = "30371", string RptName = "年度期間法人機構期貨交易量統計表")
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];

            //總列數,隱藏於A1
            int monthTotal = 12;//12個月份
            int rowYtolIndex = RowIndex + worksheet.Cells["A1"].Value.AsInt();

            //起始年份,隱藏於B1
            string firstYear = worksheet.Cells["B1"].Value.AsString();

            /******************
               讀取資料
               分三段：
               1.年
               2.當年1月至當月明細
               3.當年1月至當月合計
            ******************/
            DataTable dt = new D30370().Get30371Data(firstYear, PbFunc.Left(_emMonthText, 4), $"{PbFunc.Left(_emMonthText, 4)}01", _emMonthText.Replace("/", ""));
            if (dt.Rows.Count <= 0) {
               return $"{firstYear}~{PbFunc.Left(_emMonthText, 4)},{PbFunc.Left(_emMonthText, 4)}01～{_emMonthText.Replace("/", "")},{SheetName}－{RptName},無任何資料!";
            }

            string endYMD = dt.AsEnumerable().LastOrDefault()["AM2_YMD"].AsString();
            /* 不等於起始年,則從第2個年開始 */
            if(dt.Rows[0]["AM2_YMD"].AsString()!= firstYear) {
               //刪除兩列
               worksheet.Rows.Hide(RowIndex + 1, RowIndex + 2);
               RowIndex = RowIndex + 3;
               worksheet.Range["A1"].Select();
               rowYtolIndex = rowYtolIndex - 2;
            }
            int monthCnt = 0;
            string lsYMD = "";
            foreach (DataRow row in dt.Rows) {
               if (lsYMD != row["AM2_YMD"].AsString()) {
                  if (lsYMD.Length == 4) {
                     /* 年與年中間跳2 row */
                     RowIndex = RowIndex + 1;
                     /* 年部份結束,則把"年"空白列刪除 */
                     if (row["AM2_YMD"].AsString().Length != 4) {
                        try {
                           worksheet.Rows.Hide(RowIndex + 1, rowYtolIndex);
                        }
                        catch (Exception ex) {
                           return ex.Message + Environment.NewLine + "30370template[B1]每新增一個年份[A1]要加3";
                        }
                        RowIndex = rowYtolIndex;
                        worksheet.Range["A1"].Select();
                     }
                  }
                  
                  RowIndex = RowIndex + 1;
                  lsYMD = row["AM2_YMD"].AsString();
                  if (lsYMD.Length == 4) {
                     if (lsYMD != endYMD) {
                        worksheet.Rows[RowIndex][1 - 1].Value = $"{PbFunc.Left(lsYMD, 4).AsInt() - 1911}小計";
                     }
                     else {
                        worksheet.Rows[RowIndex + monthTotal- monthCnt][1 - 1].Value = $"{PbFunc.Left(lsYMD, 4).AsInt() - 1911}小計";//這在倒數第二行
                     }
                  }
                  else {
                     monthCnt++;//li_month_cnt = li_month_cnt + 1
                     worksheet.Rows[RowIndex][1 - 1].Value = $"{PbFunc.Left(lsYMD, 4).AsInt() - 1911}/{PbFunc.Right(lsYMD, 2)}";
                  }

               }//if (lsYMD != row["AM2_YMD"].AsString()) 

               //判斷欄位
               int columnIndex = IDFGtype(row);
               if (row["AM2_YMD"].AsString() != endYMD) {
                  worksheet.Rows[RowIndex][columnIndex].Value = row["AM2_M_QNTY"].AsDecimal();
               }
               else {
                  worksheet.Rows[RowIndex + monthTotal - monthCnt][columnIndex].Value = row["AM2_M_QNTY"].AsDecimal();//這在倒數第二行
               }

            }//foreach (DataRow row in dt.Rows)

            //刪除剩餘空白列
            try {
               if (monthTotal > monthCnt) {
                  //worksheet.Rows.Remove(RowIndex + 1, RowTotal - addRowCount);
                  worksheet.Rows.Hide(RowIndex, RowIndex + monthTotal - monthCnt - 1);
               }
            }
            catch (Exception ex) {
               throw ex;
            }
            
         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }

         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30375()
      /// </summary>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <param name="SheetName">工作表</param>
      /// <param name="RptName">作業名稱</param>
      /// <returns></returns>
      public string Wf30375(int RowIndex = 0, string SheetName = "Data_30375", string RptName = "年度期間法人機構期貨交易量統計表(維持率)")
      {
         Workbook workbook = new Workbook();
         try {
            //輸入日期
            DateTime queryDate = _emMonthText.AsDateTime("yyyy/MM");

            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[SheetName];

            //讀取資料
            DataTable dt = new D30370().Get30375Data(queryDate.AddYears(-1).ToString("yyyyMM"), queryDate.ToString("yyyyMM"));
            //if (dt.Rows.Count<=0) {
            //   return MessageDisplay.MSG_NO_DATA;
            //}

            //總列數(C1 = 100)
            int rowTotal = worksheet.Cells["C1"].Value.AsInt();

            int addRowCount = 0;//總計寫入的行數

            foreach (DataRow row in dt.Rows) {//for	i = 1 to ids_1.rowcount()	- 1
               RowIndex = RowIndex + 1;
               worksheet.Rows[RowIndex][1 - 1].Value = row["KPR_DATE"].AsDateTime().ToShortDateString();
               worksheet.Rows[RowIndex][2 - 1].Value = row["KPR_RATE"].AsDecimal();
               addRowCount++;
            }

            if (rowTotal > addRowCount && dt.Rows.Count > 0) {
               worksheet.Rows[RowIndex][1 - 1].Value = dt.AsEnumerable().LastOrDefault()["KPR_DATE"].AsDateTime().ToShortDateString();
               worksheet.Rows[RowIndex][2 - 1].Value = dt.AsEnumerable().LastOrDefault()["KPR_RATE"].AsDecimal();
            }
            //刪除空白列
            if (rowTotal > addRowCount) {
               if (dt.Rows.Count > 0) {
                  worksheet.Rows.Remove(RowIndex + 1, rowTotal - addRowCount);
                  //worksheet.Rows.Hide(RowIndex + 1, RowIndex + (rowTotal - addRowCount));
                  //重新選取圖表範圍
                  workbook.ChartSheets["30375"].Chart.Series[0].Values = new ChartData {
                     RangeValue = worksheet.Range[$@"B2:B{RowIndex + 1}"]
                  };
               }
               else {
                  worksheet.Rows.Remove(RowIndex + 2, rowTotal - addRowCount);
                  //重新選取圖表範圍
                  workbook.ChartSheets["30375"].Chart.Series[0].Values = new ChartData {
                     RangeValue = worksheet.Range["B2:B2"]
                  };
               }

            }

         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }

         return MessageDisplay.MSG_OK;
      }
   }
}
