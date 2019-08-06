using System;
using System.Data;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using DevExpress.Spreadsheet;
using BaseGround.Shared;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;

namespace PhoenixCI.FormUI.Prefix3 {
   public partial class W35020 : FormParent {
      private D35020 dao35020;

      public W35020(string programID , string programName) : base(programID , programName) {
         dao35020 = new D35020();
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         txtDate.DateTimeValue = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01").AsDateTime();

         //報表類別 下拉選單
         //DataTable exportTypeSource = daoCod.ListByCol2("35020", "ddlb_rpt");
         //exportType.SetDataTable(exportTypeSource, "COD_ID", "COD_DESC", DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor, null);
         //exportType.EditValue = "0";
         exportType.SelectedIndex = 0;

         ExportShow.Hide();
      }

      protected override ResultStatus Export() {
         ExportShow.Text = "轉檔中...";
         ExportShow.Show();

         Workbook workbook = new Workbook();
         DataTable dtAdd = new DataTable();
         DataTable dtSub = new DataTable();

         string destinationFilePath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);//Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, ls_filename);
         DateTime date = txtDate.DateTimeValue;
         string yearQ = "";
         int sheetIndex = exportType.EditValue.AsInt(), rowStart = 6, colStart = 0, rowEnd = 0;
         int sheetType = exportType.EditValue.AsInt();

         if (txt.EditValue == null) {
            MessageDisplay.Info("請輸入季度資訊 ! ");
            ExportShow.Hide();
            return ResultStatus.FailButNext;
         }

         try {

            workbook.LoadDocument(destinationFilePath);
            Worksheet worksheet = workbook.Worksheets[sheetType];

            yearQ = txt.EditValue.ToString();

            dtAdd = dao35020.GenAddReport(yearQ , date , date);
            if (dtAdd.Rows.Count <= 0) {
               ExportShow.Text = date.ToShortDateString() + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!";
               return ResultStatus.Fail;
            }
            dtSub = dao35020.GenSubReport(yearQ , date , date);
            if (dtSub.Rows.Count <= 0) {
               ExportShow.Text = date.ToShortDateString() + "," + _ProgramID + '－' + _ProgramName + ",無任何資料!";
               return ResultStatus.Fail;
            }

            worksheet.Cells[2 , 0].Value = worksheet.Cells[2 , 0].Value + txt.EditValue.ToString();

            switch (sheetType) {
               case 0: {
                     //新增部分報表
                     worksheet.Import(dtAdd , false , rowStart , colStart);
                     worksheet.Import(dtSub , false , rowStart , colStart + 2);
                     rowEnd = Math.Max(dtAdd.Rows.Count , dtSub.Rows.Count);
                     break;
                  }
               case 1: {
                     //減少部分報表
                     worksheet.Import(dtAdd , false , rowStart , colStart);
                     rowEnd = dtAdd.Rows.Count;
                     break;
                  }
               case 2: {
                     //減少部分報表
                     worksheet.Import(dtSub , false , rowStart , colStart);
                     rowEnd = dtSub.Rows.Count;
                     break;
                  }
            }
            //刪除空白列
            Range ra = worksheet.Range[(rowEnd + rowStart + 1).ToString() + ":250"];
            ra.Delete(DeleteMode.EntireRow);

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