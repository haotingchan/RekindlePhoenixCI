using ActionService;
using ActionService.DbDirect;
using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace PhoenixCI.FormUI.Prefix6
{
   public partial class W60210 : FormParent
   {

      private D60210 dao60210;
      private  int flag;
        public W60210(string programID, string programName) : base(programID, programName)
        {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;
            txtDate.DateTimeValue = GlobalInfo.OCF_DATE;
            dao60210 = new D60210();
            ExportShow.Hide();
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
            ExportShow.Text = "轉檔中...";
            ExportShow.Show();
            string excelDestinationPath="";
            try
            {
                base.Export();

                excelDestinationPath = PbFunc.wf_copy_file(_ProgramID, _ProgramID);// CopyExcelTemplateFile(_ProgramID, FileType.XLS);
                flag = 0;

                ManipulateExcel(excelDestinationPath);

                if (flag == 0)
                {
                    File.Delete(excelDestinationPath);
                    ExportShow.Text = "轉檔失敗";
                    return ResultStatus.Fail;
                }
            }
            catch (Exception ex)
            {
                File.Delete(excelDestinationPath);
                ExportShow.Text = "轉檔失敗";
                WriteLog(ex);
                return ResultStatus.Fail;
            }
            ExportShow.Text = "轉檔成功!";
            return ResultStatus.Success;
      }

      private void ManipulateExcel(string excelDestinationPath)
      {
         DateTime dateIn = txtDate.DateTimeValue;

         Workbook workbook = new Workbook();
         workbook.LoadDocument(excelDestinationPath);

         Worksheet worksheet = workbook.Worksheets[0];

         worksheet.Cells[0, 0].Value = txtDate.Text + worksheet.Cells[0, 0].Value;


         #region 602111

         DataTable dt602111 = dao60210.ListDataFor602111(dateIn);
         if (dt602111.Rows.Count == 0) {
            MessageDisplay.Info(string.Format("{0},{1}─{2},無任何資料!", txtDate.Text, _ProgramID, "日報表．一、證券及期貨市場指數價格日報(選擇權市場)"));
         }
         else {
            foreach (DataRow row in dt602111.Rows) {
               int rowIndex = row["RPT_SEQ_NO"].AsInt();

               if (rowIndex == 0) continue;

               worksheet.Cells[rowIndex - 1, 2].Value = row["AMIF_CLOSE_PRICE"].AsDecimal();
               worksheet.Cells[rowIndex - 1, 3].Value = row["AMIF_UP_DOWN_VAL"].AsDecimal();

               decimal amif_sum_amt = row["AMIF_SUM_AMT"].AsDecimal();

               if (amif_sum_amt > 0) {
                  worksheet.Cells[rowIndex - 1, 5].Value = amif_sum_amt;
               }
            }
                flag++;
         }

         #endregion 602111

         #region 602112

         DataTable dt602112 = dao60210.ListDataFor602112(dateIn);
         if (dt602112.Rows.Count == 0) {
            MessageDisplay.Info(string.Format("{0},{1}─{2},無任何資料!", txtDate.Text, _ProgramID, "日報表．一、證券及期貨市場指數價格日報(期貨市場)"));
         }
         else {
            // GroupBy用
            var queryForGroupBy = dt602112.AsEnumerable().GroupBy(t => new { t1 = t.Field<string>("AMIF_KIND_ID") })
                              .Select(g => new {
                                 KindID = g.Key.t1.Trim(),
                                 Qty = g.Sum(s => s.Field<decimal>("AMIF_M_QNTY_TAL"))
                              });

            string kindId = "";

            foreach (DataRow row in dt602112.Rows) {
               int rowIndex = row["RPT_SEQ_NO"].AsInt();

               if (rowIndex == 0) continue;

               string AMIF_KIND_ID = row["AMIF_KIND_ID"].AsString();

               if (kindId == AMIF_KIND_ID) continue;

               kindId = AMIF_KIND_ID;

               int AMIF_M_QNTY_TAL = row["AMIF_M_QNTY_TAL"].AsInt();

               if (AMIF_M_QNTY_TAL != 0) {
                  worksheet.Cells[rowIndex - 1, 2].Value = row["AMIF_CLOSE_PRICE"].AsDecimal();
                  worksheet.Cells[rowIndex - 1, 3].Value = row["AMIF_UP_DOWN_VAL"].AsDecimal();
               }
               else {
                  worksheet.Cells[rowIndex - 1, 2].Value = 0;
                  worksheet.Cells[rowIndex - 1, 3].Value = "-";
               }

               var groupKindId = queryForGroupBy.Where(m => m.KindID == AMIF_KIND_ID).FirstOrDefault();

               if (groupKindId != null) {
                  worksheet.Cells[rowIndex - 1, 5].Value = groupKindId.Qty;
               }
            }
             flag++;
         }

         #endregion 602112

         #region 602113

         DateTime lastDate = dao60210.GetStwLastDate(dateIn);

         DataTable dt602113 = dao60210.ListDataFor602113(dateIn, lastDate);
         if (dt602113.Rows.Count == 0) {
            MessageDisplay.Info(string.Format("{0},{1}─{2},無任何資料!", txtDate.Text, _ProgramID, "日報表.一、證券及期貨市場指數價格日報(新加坡摩臺)"));
         }
         else {
            // GroupBy用
            var groupBy602113 = dt602113.AsEnumerable().GroupBy(t => new { })
                              .Select(g => new {
                                 Qty = g.Sum(s => s.Field<decimal>("M_QNTY"))
                              });

            string kindIDPrev = "";

            foreach (DataRow row in dt602113.Rows) {
               int rowIndex = row["RPT_SEQ_NO"].AsInt();

               if (rowIndex == 0) continue;

               string KIND_ID = row["KIND_ID"].AsString();
               string SETTLE_DATE = row["SETTLE_DATE"].AsString();
               decimal CLOSE_PRICE = row["CLOSE_PRICE"].AsDecimal();
               decimal STW_LAST_SETTLE = row["STW_LAST_SETTLE"].AsDecimal();

               // 現貨
               if (SETTLE_DATE == "000000") {
                  worksheet.Cells[rowIndex - 1, 7].Value = CLOSE_PRICE;
                  continue;
               }
               // 期貨
               if (kindIDPrev == KIND_ID) continue;

               kindIDPrev = KIND_ID;

               decimal M_QNTY = row["M_QNTY"].AsDecimal();

               if (M_QNTY != 0) {
                  worksheet.Cells[rowIndex - 1, 2].Value = CLOSE_PRICE;
                  worksheet.Cells[rowIndex - 1, 3].Value = CLOSE_PRICE - STW_LAST_SETTLE;
               }

               var groupSum = groupBy602113.FirstOrDefault();

               if (groupSum != null) {
                  worksheet.Cells[rowIndex - 1, 5].Value = groupSum.Qty;
               }
            }
             flag++;
         }

         #endregion 602113

         #region 602114

         DataTable dtRPT = new RPT().ListData("%602114%");
         if (dtRPT.Rows.Count == 0) {
            MessageDisplay.Info(string.Format("{0},{1}─{2},無任何資料!", txtDate.Text, _ProgramID, "RPT"));
         }
         else {
            int RPT_SEQ_NO = dtRPT.Rows[0]["RPT_SEQ_NO"].AsInt();
            int RPT_VALUE_2 = dtRPT.Rows[0]["RPT_VALUE_2"].AsInt();
            string RPT_VALUE = dtRPT.Rows[0]["RPT_VALUE"].AsString().Trim();

            worksheet.Cells[RPT_SEQ_NO - 1, RPT_VALUE_2 - 1].Value = RPT_VALUE + GlobalInfo.USER_DPT_NAME + "/" + GlobalInfo.USER_NAME;
         }

         #endregion 602114

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