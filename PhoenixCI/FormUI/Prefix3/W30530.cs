using System;
using System.Data;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using DevExpress.Spreadsheet;
using BaseGround.Shared;
using DataObjects.Dao.Together.SpecificDao;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace PhoenixCI.FormUI.Prefix3 {
   public partial class W30530 : FormParent {
      private D30530 dao30530;
      int idfgCount = 6;// idfg_Type

      public W30530(string programID, string programName) : base(programID, programName) {
         dao30530 = new D30530();
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

         ExportShow.Hide();
      }

      protected override ResultStatus Export() {
         ExportShow.Text = "轉檔中...";
         ExportShow.Show();

         Workbook workbook = new Workbook();
         DataTable dtYearData = new DataTable();
         DataTable dtMontData = new DataTable();

         string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
         string inputMonth = txtDate.DateTimeValue.ToString("yyyyMM");
         string inputYear = txtDate.DateTimeValue.Year.ToString();

         try {
            workbook.LoadDocument(destinationFilePath);
            Worksheet worksheet = workbook.Worksheets[0];
            string startYear = worksheet.Cells[2, 1].Value.ToString();
            int rowTol = worksheet.Cells[2, 0].Value.AsInt();
            List<int> ListBIndex = new List<int>();
            int rowStart = 4;

            for (int i = 1; i <= idfgCount; i++) {
               rowStart = 4;
               int bIndex = GetBIndex(i, ListBIndex);
               int ymd = 0;

               ListBIndex.Add(bIndex);

               if (i == 6) {
                  i = 7;// idfgtype 跳過6
               }
               dtYearData = dao30530.ListYearData(startYear, inputYear, inputYear + "01", inputMonth, i.ToString(), bIndex.ToString(), (bIndex + 1).ToString());
               dtMontData = dao30530.ListMonthData(inputYear + "01", inputMonth, i.ToString(), bIndex.ToString(), (bIndex + 1).ToString());

               if (dtYearData.Rows.Count <= 0) {
                  ExportShow.Hide();
                  MessageDisplay.Info(inputMonth + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!");
                  return ResultStatus.Fail;
               }

               // 年分
               for (int j = startYear.AsInt(); j <= inputYear.AsInt(); j++) {
                  DataTable dtYear = dtYearData.Filter("AM2_YMD = " + j.ToString());
                  if (ymd != j) {
                     //有資料時才增加列數
                     if (dtYear.Rows.Count > 0) {
                        rowStart++;
                     }

                     ymd = j;
                  }
                  foreach (DataRow dr in dtYear.Rows) {
                     worksheet.Cells[rowStart, 0].Value = ymd;
                     worksheet.Cells[rowStart, dr["BS_Index"].AsInt()].Value = dr["am2_m_qnty"].AsDecimal();
                  }
               }

               //每月
               for (int j = (inputYear.ToString() + "01").AsInt(); j <= inputMonth.AsInt(); j++) {
                  DataTable dtMon = dtMontData.Filter("AM2_YMD = " + j.ToString());
                  if (ymd != j) {

                     //有資料時才增加列數
                     if (dtMon.Rows.Count > 0) {
                        rowStart++;
                     }

                     ymd = j;
                  }
                  foreach (DataRow dr in dtMon.Rows) {
                     worksheet.Cells[rowStart, 0].Value = dr["am2_ymd"].AsDateTime("yyyyMM").ToString("MMM", CultureInfo.CreateSpecificCulture("en-US")) + ".";
                     worksheet.Cells[rowStart, dr["BS_Index"].AsInt()].Value = dr["am2_m_qnty"].AsDecimal();
                  }
               }
            }

            if (rowTol < 38) {
               Range ra = worksheet.Range[(rowStart + 2).ToString() + ":39"];
               ra.Delete(DeleteMode.EntireRow);
            }

            worksheet.ScrollToRow(0);
            workbook.SaveDocument(destinationFilePath);
         } catch (Exception ex) {
            ExportShow.Text = "轉檔失敗";
            throw ex;
         }
         ExportShow.Text = "轉檔成功!";
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();
         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      /// <summary>
      /// get 買進excel row index 賣出為 買進+1
      /// </summary>
      /// <param name="idfgType"></param>
      /// <param name="ListBIndex"></param>
      /// <returns></returns>
      private int GetBIndex(int idfgType, List<int> ListBIndex) {
         int re = idfgType;

         while (re % 2 == 0) {
            re++;
         }

         if (ListBIndex.Where(b => b == re).Count() != 0) {
            re++;
            re = GetBIndex(re, ListBIndex);
         }

         return re;
      }
   }
}