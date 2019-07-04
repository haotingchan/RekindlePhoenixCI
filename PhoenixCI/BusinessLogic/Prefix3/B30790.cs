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
      private readonly string _lsFile;
      private string _startDateText;
      private string _endDateText;
      private string _txStartDateText;
      private string _txEndDateText;
      D30790 dao30790;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      public B30790(string FilePath, string StartDate, string EndDate, string TxStartDate, string TxEndDate)
      {
         _lsFile = FilePath;
         _startDateText = StartDate;
         _endDateText = EndDate;
         _txStartDateText = TxStartDate;
         _txEndDateText = TxEndDate;
         dao30790 = new D30790();
      }
      /// <summary>
      /// wf_30790()
      /// </summary>
      /// <returns></returns>
      public string Wf30790()
      {
         Workbook workbook = new Workbook();
         try {
            DateTime startDate = _startDateText.AsDateTime("yyyy/MM/dd");
            DateTime endDate = _endDateText.AsDateTime("yyyy/MM/dd");

            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[0];

            //讀取資料
            DataTable dt = dao30790.Get30790Data(startDate, endDate);
            if (dt.Rows.Count <= 0) {
               return $"{_startDateText},{_endDateText},30790－盤後交易時段分時交易量分布,無任何資料!";
            }

            worksheet.Range["A1"].Select();
            worksheet.Cells["C2"].Value = $"{_startDateText} - {_endDateText}";
            int rowsCount = 0;
            int rowIndex = 4 - 1;
            for (int j = 0; j < dt.Rows.Count; j++) {
               DataRow row = dt.Rows[j];
               int beginTime = row["BEGIN_TIME"].AsInt();
               string timeFormat = "00:00";
               if (j == 0) {
                  //起時
                  worksheet.Rows[rowIndex][1 - 1].Value = beginTime.ToString(timeFormat);
                  worksheet.Rows[rowIndex][2 - 1].Value = row["BEGIN_TYPE"].AsString();
                  //迄時
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
                        //起時
                        worksheet.Rows[rowIndex][1 - 1].Value = beginTime.ToString(timeFormat);
                        worksheet.Rows[rowIndex][2 - 1].Value = row["BEGIN_TYPE"].AsString();
                        //迄時
                        worksheet.Rows[rowIndex][3 - 1].Value = row["END_TIME"].AsInt().ToString(timeFormat);
                        worksheet.Rows[rowIndex][4 - 1].Value = row["END_TYPE"].AsString();

                        rowsCount = rowIndex;
                     }
                  }
               }//else if (j > 0)

               //TX振幅
               if (row["KIND_ID"].AsString() == "TXF") {
                  workbook.Worksheets[1 - 1].Cells[$"E{rowIndex}"].SetValue(row["AVG_TX_HIGH_LOW"]);
               }
               //量
               int seqNO = row["SEQ_NO"].AsInt();
               //日均量
               workbook.Worksheets["日均量"].Rows[rowIndex][seqNO - 1].SetValue(row["AVG_QNTY"]);
               //總量
               workbook.Worksheets["總量"].Rows[rowIndex][seqNO - 1].SetValue(row["M_QNTY"]);
               if (beginTime != 9999) {
                  //比重
                  workbook.Worksheets["比重"].Rows[rowIndex][seqNO - 1].SetValue(row["M_RATE"]);
               }

            }//for (int j = 0; j < dt.Rows.Count; j++)

            //刪除空白列
            if (rowsCount < 59) {
               for (int k = 0; k < 3; k++) {
                  workbook.Worksheets[k].Rows.Remove(rowsCount + 1, 59 - 1 - rowsCount);
                  workbook.Worksheets[k].ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
               }
            }

         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("Wf30790:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }

         return MessageDisplay.MSG_OK;
      }
      /// <summary>
      /// wf_30790_4()
      /// </summary>
      /// <returns></returns>
      public string Wf30790four()
      {
         Workbook workbook = new Workbook();
         try {
            DateTime startDate = _txStartDateText.AsDateTime("yyyy/MM/dd");
            DateTime endDate = _txEndDateText.AsDateTime("yyyy/MM/dd");
            //切換Sheet
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets["TX振幅"];

            //讀取資料
            DataTable dt = dao30790.Get30790_4Data(startDate, endDate);
            if (dt.Rows.Count <= 0) {
               return $"{_startDateText},{_endDateText},30790_4－TX每日日盤及夜盤之振幅及收盤價,無任何資料!";
            }
            worksheet.Cells["B2"].Value = $"{_txStartDateText} - {_txEndDateText}";
            //從第四行(0~3)開始寫入
            worksheet.Import(dt, false, 3, 0);
            worksheet.ScrollTo(0, 0);//直接滾動到最上面，不然看起來很像少行數
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("Wf30790_4:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            //存檔
            workbook.SaveDocument(_lsFile);
         }

         return MessageDisplay.MSG_OK;
      }
   }
}
