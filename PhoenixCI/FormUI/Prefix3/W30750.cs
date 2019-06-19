using System;
using System.Data;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using DevExpress.Spreadsheet;
using DataObjects.Dao.Together.SpecificDao;
using System.Globalization;
using BaseGround.Shared;
using System.IO;

namespace PhoenixCI.FormUI.Prefix3 {
   public partial class W30750 : FormParent {
      private D30750 dao30750;

      public W30750(string programID, string programName) : base(programID, programName) {
         dao30750 = new D30750();

         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtSDate.DateTimeValue = GlobalInfo.OCF_DATE.ToString("yyyy/01").AsDateTime();
         txtEDate.DateTimeValue = GlobalInfo.OCF_DATE;

         ExportShow.Hide();
      }

      protected override ResultStatus Export() {
         ExportShow.Text = "轉檔中...";
         ExportShow.Show();

         Workbook workbook = new Workbook();

         string destinationFilePath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);
         string sYmd = txtSDate.DateTimeValue.ToString("yyyyMM");
         string eYmd = txtEDate.DateTimeValue.ToString("yyyyMM");
         int inputYear = txtEDate.DateTimeValue.Year - txtSDate.DateTimeValue.Year;
         int oleRow = 2, rowStart = oleRow + 2, rowEnd = rowStart;
         int rowTol = 0;
         int colTot = dao30750.GetColTot();

         try {
            workbook.LoadDocument(destinationFilePath);
            Worksheet worksheet = workbook.Worksheets[0];

            //insert Row (按年分)
            if (inputYear > 0) {
               for (int i = 1; i <= inputYear; i++) {
                  rowEnd += 12;
                  //新增12筆空白列
                  for (int j = 1; j <= 12; j++) {
                     worksheet.Rows[rowEnd - 1].Insert();
                  }
                  //將上面的12列 copy 過去
                  Range ra = worksheet.Range[(rowStart).ToString() + ":" + (rowStart + 11).ToString()];
                  worksheet.Rows[rowEnd - 1].CopyFrom(ra);
               }
            }
            rowTol = rowStart + ((inputYear + 1) * 12);

            DataTable dtAI2 = dao30750.GetAI2Data(sYmd, eYmd);
            DataTable dtDayCount = dao30750.GetDayCount(sYmd, eYmd);

            if (dtAI2.Rows.Count <= 0) {
               ExportShow.Hide();
               MessageDisplay.Info(sYmd + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!");
               File.Delete(destinationFilePath);
               return ResultStatus.Fail;
            }

            //補上未選月
            if (txtSDate.DateTimeValue.Month != 1) {
               for (int i = 1; i <= txtSDate.DateTimeValue.Month - 1; i++) {
                  DataRow addRow = dtDayCount.NewRow();
                  addRow["ai2_ymd"] = txtSDate.DateTimeValue.Year.ToString() + i.ToString("D2");
                  addRow["cp_day_count"] = 0;
                  dtDayCount.Rows.Add(addRow);
               }
               dtDayCount = dtDayCount.Sort("AI2_YMD");
            }

            string lsYear = "";
            foreach (DataRow r in dtDayCount.Rows) {
               DateTime aiYm = r["ai2_ymd"].AsDateTime("yyyyMM");
               TaiwanCalendar tai = new TaiwanCalendar();
               oleRow += 1;
               //清除未選月公式
               if (r["ai2_ymd"].AsDateTime("yyyyMM") < txtSDate.DateTimeValue) {
                  worksheet.Rows[oleRow].ClearContents();
               }
               if (lsYear != aiYm.Year.ToString()) {
                  if (aiYm.Month != 1) {
                     for (int i = 1; i < aiYm.Month; i++) {
                        worksheet.Cells[oleRow, 0].Value = tai.GetYear(aiYm).ToString();
                        worksheet.Cells[oleRow, 1].Value = i.ToString();
                        worksheet.Cells[oleRow, 2].Value = "";
                        oleRow++;
                     }
                  }
                  lsYear = aiYm.Year.ToString();
               }

               worksheet.Cells[oleRow, 0].Value = tai.GetYear(aiYm).ToString();
               worksheet.Cells[oleRow, 1].Value = aiYm.Month.ToString();
               worksheet.Cells[oleRow, 2].Value = r["cp_day_count"].ToString() != "0" ? r["cp_day_count"].ToString() : "";
               //DateTime lastAiYm = dtDayCount.Rows[dtDayCount.Rows.IndexOf(r) - 1]["ai2_ymd"].AsDateTime("yyyyMM");
               //worksheet.Cells[oleRow, 0].Value = tai.GetYear(lastAiYm).ToString();
               //worksheet.Cells[oleRow, 1].Value = lastAiYm.Month.ToString();
               //worksheet.Cells[oleRow, 2].Value = "";

               //日均量總計
               if (colTot > 0) {
                  int cpMQnty = dtAI2.Compute("SUM(ai2_m_qnty)", "ai2_ymd=" + r["ai2_ymd"].ToString()).AsInt();
                  int cpDayCount = r["cp_day_count"].AsInt();

                  if (cpDayCount > 0) {
                     worksheet.Cells[oleRow, colTot].Value = Math.Round((double)cpMQnty / cpDayCount, 0, MidpointRounding.AwayFromZero);
                  }
               }
               if (dtAI2.Select("ai2_ymd=" + r["ai2_ymd"].ToString()).Length != 0) {
                  DataTable dtAI2ByYmd = dtAI2.Filter("ai2_ymd=" + r["ai2_ymd"].ToString());
                  foreach (DataRow row in dtAI2ByYmd.Rows) {
                     //月總量
                     int col = row["rpt_seq_no"].AsInt();
                     if (col > 0) {
                        worksheet.Cells[oleRow, col - 1].Value = row["ai2_m_qnty"].AsInt();
                     }
                     //日均量
                     int col2 = row["rpt_seq_no_2"].AsInt();
                     if (col2 > 0) {
                        int ai2DayCount = row["ai2_day_count"].AsInt();
                        if (ai2DayCount > 0) {
                           worksheet.Cells[oleRow, col2 - 1].Value = Math.Round((double)row["ai2_m_qnty"].AsInt() / ai2DayCount, 0, MidpointRounding.AwayFromZero);
                        }
                     }
                  }
               }// if
            }
            //清除空白列
            if (rowTol != oleRow + 2) {
               Range rowRange = worksheet.Range[(oleRow + 2).ToString() + ":" + (rowTol - 1).ToString()];
               rowRange.ClearContents();
            }

            //總計
            //insert Row (按年分)
            if (inputYear >= 2) {
               rowStart = rowTol;
               for (int i = 1; i <= inputYear - 1; i++) {
                  worksheet.Rows[rowStart].Insert();
                  //將上面的總計列 copy 過去
                  Range ra = worksheet.Rows[rowStart - 1];
                  worksheet.Rows[rowStart].CopyFrom(ra);
                  rowStart += 1;
               }
            }

            DataTable dtAI2Sum = dao30750.GetAI2Sum(sYmd, eYmd);
            string ymd = "";
            oleRow = rowTol - 2;
            foreach (DataRow sumR in dtAI2Sum.Rows) {
               if (ymd != sumR["ai2_ymd"].ToString()) {
                  DateTime aiYm = sumR["ai2_ymd"].AsDateTime("yyyy");
                  TaiwanCalendar tai = new TaiwanCalendar();
                  oleRow += 1;
                  worksheet.Cells[oleRow, 0].Value = tai.GetYear(aiYm).ToString() + "年";
                  worksheet.Cells[oleRow, 2].Value = dtAI2Sum.Compute("MAX(ai2_day_count)", "ai2_ymd=" + sumR["ai2_ymd"].ToString()).AsInt();
                  ymd = sumR["ai2_ymd"].ToString();
               }
               //日均量總計
               if (colTot > 0) {
                  int cpMQnty = dtAI2Sum.Compute("SUM(ai2_m_qnty)", "ai2_ymd=" + sumR["ai2_ymd"].ToString()).AsInt();
                  int cpDayCount = dtAI2Sum.Compute("MAX(ai2_day_count)", "ai2_ymd=" + sumR["ai2_ymd"].ToString()).AsInt();

                  if (cpDayCount > 0) {
                     worksheet.Cells[oleRow, colTot].Value = Math.Round((double)cpMQnty / cpDayCount, 0, MidpointRounding.AwayFromZero);
                  }
               }
               //月總量
               int col = sumR["rpt_seq_no"].AsInt();
               if (col > 0) {
                  worksheet.Cells[oleRow, col - 1].Value = sumR["ai2_m_qnty"].AsInt();
               }
               //日均量
               int col2 = sumR["rpt_seq_no_2"].AsInt();
               if (col2 > 0) {
                  int ai2DayCount = sumR["ai2_day_count"].AsInt();
                  if (ai2DayCount > 0) {
                     worksheet.Cells[oleRow, col2 - 1].Value = Math.Round((double)sumR["ai2_m_qnty"].AsInt() / ai2DayCount, 0, MidpointRounding.AwayFromZero);
                  }
               }
            }
            //刪除空白列
            if (rowTol > oleRow) {
               Range ra = worksheet.Range[(oleRow + 2).ToString() + ":" + (rowTol + 2).ToString()];
               ra.Delete(DeleteMode.EntireRow);
            }

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
   }
}