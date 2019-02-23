using ActionService;
using BaseGround;
using BaseGround.Report;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;

namespace PhoenixCI.FormUI.Prefix6
{
   public partial class W60110 : FormParent
   {
      private D60110 dao60110;

      public W60110(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         txtMonth.DateTimeValue = GlobalInfo.OCF_DATE;

         dao60110 = new D60110();
      }

      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve()
      {
         base.Retrieve();

         return ResultStatus.Success;
      }

      protected override ResultStatus CheckShield()
      {
         base.CheckShield();

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall)
      {
         base.Save(pokeBall);

         return ResultStatus.Success;
      }

      protected override ResultStatus Run(PokeBall args)
      {
         base.Run(args);

         return ResultStatus.Success;
      }

      protected override ResultStatus Import()
      {
         base.Import();

         return ResultStatus.Success;
      }

      protected override ResultStatus Export()
      {
         base.Export();

         List<DbParameterEx> listParams = new List<DbParameterEx>();
         DbParameterEx dbParaEx = new DbParameterEx("as_ym", txtMonth.FormatValue);
         listParams.Add(dbParaEx);

         ResultData resultData = serviceCommon.ExecuteStoredProcedure("ci.sp_H_stt_W_AM", listParams, true);

         string excelDestinationPath = CopyExcelTemplateFile(_ProgramID, FileType.XLS);

         ManipulateExcel(excelDestinationPath);

         return ResultStatus.Success;
      }

      private void ManipulateExcel(string excelDestinationPath)
      {
         DateTime dateIn = txtMonth.DateTimeValue;

         Workbook workbook = new Workbook();
         workbook.LoadDocument(excelDestinationPath);

         Worksheet worksheet = workbook.Worksheets[0];

         #region 表頭

         worksheet.Cells[0, 0].Value = "臺灣期貨交易所" + dateIn.Year + "年" + dateIn.ToTwoNumMonth() + "月份成交量統計概況";
         worksheet.Cells[1, 0].Value = "TAIFEX " + dateIn.ToLongEnglishMonth() + " " + dateIn.Year + " Trading Stastics"; ;
         worksheet.Cells[3, 2].Value = dateIn.Year + " " + dateIn.ToShortEnglishMonthWithDot(); ;
         worksheet.Cells[3, 3].Value = dateIn.AddYears(-1).Year + " " + dateIn.ToShortEnglishMonthWithDot(); ;
         worksheet.Cells[3, 5].Value = dateIn.Year + " " + dateIn.AddMonths(-1).ToShortEnglishMonthWithDot(); ;
         worksheet.Cells[3, 7].Value = "Jan.-" + dateIn.ToShortEnglishMonthWithDot(); ;
         worksheet.Cells[3, 8].Value = "Jan.-" + dateIn.ToShortEnglishMonthWithDot(); ;
         worksheet.Cells[4, 7].Value = dateIn.Year; ;
         worksheet.Cells[4, 8].Value = dateIn.AddYears(-1).Year; ;
         worksheet.Cells[3, 10].Value += dateIn.Year.ToString(); ;
         worksheet.Cells[4, 10].Value = "(Jan.-" + dateIn.ToShortEnglishMonthWithDot() + ")"; ;

         #endregion 表頭

         #region 明細

         DataTable dtContent = dao60110.ListW_AM_And_APDK_PARAM(txtMonth.FormatValue);

         string kindID = "";
         int rowIndex = -1 + 5;

         foreach (DataRow row in dtContent.Rows) {
            string W_AM_PARAM_KEY = row["W_AM_PARAM_KEY"].AsString();

            if (kindID != W_AM_PARAM_KEY) {
               kindID = W_AM_PARAM_KEY;
               rowIndex += 1;
            }

            worksheet.Rows[rowIndex][0].Value = W_AM_PARAM_KEY;
            worksheet.Rows[rowIndex][1].Value = row["PARAM_NAME"].AsString();
            worksheet.Rows[rowIndex][2].Value = row["W_AM_QNTY_M"].AsDecimal();
            worksheet.Rows[rowIndex][3].Value = row["W_AM_QNTY_Y_L"].AsDecimal();
            worksheet.Rows[rowIndex][5].Value = row["W_AM_QNTY_M_L"].AsDecimal();
            worksheet.Rows[rowIndex][7].Value = row["W_AM_QNTY_ACCU_Y"].AsDecimal();
            worksheet.Rows[rowIndex][8].Value = row["W_AM_QNTY_ACCU_Y_L"].AsDecimal();
            worksheet.Rows[rowIndex][10].Value = row["W_AM_QNTY_AVG_Y"].AsDecimal();
            worksheet.Rows[rowIndex][11].Value = row["W_AM_OI"].AsDecimal();
         }

         rowIndex++;

         if (rowIndex < 105) {
            worksheet.Rows.Remove(rowIndex, (104 - rowIndex) + 1);
         }

         #endregion 明細

         workbook.SaveDocument(excelDestinationPath);
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow()
      {
         base.InsertRow();

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow()
      {
         base.DeleteRow();

         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         return base.BeforeClose();
      }
   }
}