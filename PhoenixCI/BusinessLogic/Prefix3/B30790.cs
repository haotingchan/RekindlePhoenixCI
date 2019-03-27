using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 20190327,john,盤後交易時段分時交易量分布
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 盤後交易時段分時交易量分布
   /// </summary>
   public class B30790
   {
      private string lsFile;
      private string startDateText;
      private string endDateText;
      private string txStartDateText;
      private string txEndDateText;
      D30790 dao30790;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      public B30790(string FilePath, string StartDate, string EndDate, string TxStartDate, string TxEndDate)
      {
         lsFile = FilePath;
         startDateText = StartDate;
         endDateText = EndDate;
         txStartDateText = TxStartDate;
         txEndDateText = TxEndDate;
         dao30790 = new D30790();
      }
      /// <summary>
      /// wf_30790()
      /// </summary>
      /// <returns></returns>
      public string Wf30790()
      {
         try {
            DateTime startDate = startDateText.AsDateTime();
            DateTime endDate = endDateText.AsDateTime();
            //讀取資料
            DataTable dt = dao30790.Get30790Data(startDate, endDate);
            if (dt.Rows.Count <= 0) {
               return $"{startDateText},{endDateText},30790－盤後交易時段分時交易量分布,無任何資料!";
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[0];

            worksheet.Range["A1"].Select();
            worksheet.Cells["C2"].Value = $"{startDateText}-{endDateText}";
            int rowsCount = 0;
            int rowIndex = 4 - 1;
            for (int j = 0; j < dt.Rows.Count; j++) {
               DataRow row = dt.Rows[j];
               int beginTime = row["BEGIN_TIME"].AsInt();
               string timeFormat = "00:00";
               if (j == 0) {
                  worksheet.Rows[rowIndex][1 - 1].Value = beginTime.ToString(timeFormat);
                  worksheet.Rows[rowIndex][2 - 1].Value = row["BEGIN_TYPE"].AsString();
                  worksheet.Rows[rowIndex][3 - 1].Value = row["END_TIME"].AsInt().ToString(timeFormat);
                  worksheet.Rows[rowIndex][4 - 1].Value = row["END_TYPE"].AsString();
               }
               else if (j > 0) {
                  if (beginTime != dt.Rows[j - 1]["BEGIN_TIME"].AsInt()) {

                     if (beginTime == 9999) {
                        rowIndex = 61 - 1;
                     }
                     else {
                        rowIndex = rowIndex + 1;

                        worksheet.Rows[rowIndex][1 - 1].Value = beginTime.ToString(timeFormat);
                        worksheet.Rows[rowIndex][2 - 1].Value = row["BEGIN_TYPE"].AsString();
                        worksheet.Rows[rowIndex][3 - 1].Value = row["END_TIME"].AsInt().ToString(timeFormat);
                        worksheet.Rows[rowIndex][4 - 1].Value = row["END_TYPE"].AsString();

                        rowsCount = rowIndex;
                     }
                  }
               }//else if (j > 0)

               //TX振幅
               if (row["KIND_ID"].AsString() == "TXF") {
                  workbook.Worksheets[1 - 1].Rows[rowIndex][5 - 1].SetValue(row["AVG_TX_HIGH_LOW"]);
               }
               //量
               int seqNO = row["SEQ_NO"].AsInt();
               workbook.Worksheets[1 - 1].Rows[rowIndex][seqNO - 1].SetValue(row["AVG_QNTY"]);
               workbook.Worksheets[2 - 1].Rows[rowIndex][seqNO - 1].SetValue(row["M_QNTY"]);
               if (beginTime != 9999) {
                  workbook.Worksheets[3 - 1].Rows[rowIndex][seqNO - 1].SetValue(row["M_RATE"]);
               }

            }//for (int j = 0; j < dt.Rows.Count; j++)

            //刪除空白列
            if (rowsCount < 59) {
               for (int k = 0; k < 3; k++) {
                  workbook.Worksheets[k].Rows.Remove(rowsCount + 1, 59 - 1 - rowsCount);
                  workbook.Worksheets[k].ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
               }
            }
            workbook.SaveDocument(lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("Wf30790:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }
      /// <summary>
      /// wf_30790_4()
      /// </summary>
      /// <returns></returns>
      public string Wf30790four()
      {
         try {
            DateTime startDate = txStartDateText.AsDateTime();
            DateTime endDate = txEndDateText.AsDateTime();
            //讀取資料
            DataTable dt = dao30790.Get30790_4Data(startDate, endDate);
            if (dt.Rows.Count <= 0) {
               return $"{startDateText},{endDateText},30790_4－TX每日日盤及夜盤之振幅及收盤價,無任何資料!";
            }
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(lsFile);
            Worksheet worksheet = workbook.Worksheets[3];
            worksheet.Cells["B2"].Value = $"{txStartDateText}-{txEndDateText}";
            //從第四行(0~3)開始寫入
            worksheet.Import(dt, false, 3, 0);
            worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
            //存檔
            workbook.SaveDocument(lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("Wf30790_4:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }
   }
}
