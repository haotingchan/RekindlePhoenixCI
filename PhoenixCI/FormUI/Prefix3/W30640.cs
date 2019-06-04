using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/01/24
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30640 交易人各商品未平倉結構比重
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30640 : FormParent {

      int ii_ole_row;
      private D30640 dao30640;

      public W30640(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao30640 = new D30640();
         //txtStartMonth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/01"); //起始月份皆設為當年1月
         //txtEndMonth.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM");
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            //1. 設定初始年月
            txtStartMonth.DateTimeValue = DateTime.ParseExact(GlobalInfo.OCF_DATE.ToString("yyyy/01/dd") , "yyyy/MM/dd" , null);//起始月份皆設為當年1月
            txtStartMonth.EnterMoveNextControl = true;
            txtStartMonth.Focus();

            txtEndMonth.DateTimeValue = GlobalInfo.OCF_DATE;
            txtEndMonth.EnterMoveNextControl = true;
            txtEndMonth.Focus();

#if DEBUG
            txtStartMonth.DateTimeValue = DateTime.ParseExact("2014/01" , "yyyy/MM" , null);
            txtEndMonth.DateTimeValue = DateTime.ParseExact("2014/04" , "yyyy/MM" , null);
            this.Text += "(開啟測試模式)";
#endif
            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {

         try {

            //0. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //1.複製檔案 & 開啟檔案 
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);
            Worksheet worksheet = workbook.Worksheets[0];

            //2.填資料
            ii_ole_row = 3;
            bool result = wf_Export(workbook , worksheet , txtStartMonth.Text.Replace("/" , "") , txtEndMonth.Text.Replace("/" , ""));

            if (!result) {
               workbook = null;
               File.Delete(excelDestinationPath);
               return ResultStatus.Fail;
            }

            //3.存檔
            workbook.SaveDocument(excelDestinationPath);
            labMsg.Visible = false;

            return ResultStatus.Success;

         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
            this.Cursor = Cursors.Arrow;
         }

         return ResultStatus.Fail;
      }

      private bool wf_Export(Workbook workbook , Worksheet worksheet , string as_symd , string as_eymd) {

         try {
            int li_ole_row_tol = ii_ole_row + 60;
            int li_ole_col, li_header = 0;

            DataTable dt = dao30640.GetData(as_symd , as_eymd);
            if (dt.Rows.Count == 0) {
               MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtStartMonth.Text + "-" + txtEndMonth.Text , this.Text));
               return false;
            }

            li_header = 10;
            //PB有寫這段可是報表未顯示，先備註
            //worksheet.Cells[1 , 5].Value = txtStartMonth.Text.Replace("/" , "") + " - " + txtEndMonth.Text.Replace("/" , "") + worksheet.Cells[1 , 5].Value;
            worksheet.Cells[li_header - 4 , 0].Value = "查詢條件：" + txtStartMonth.Text.Replace("/" , "") + " - " + txtEndMonth.Text.Replace("/" , "");

            //依資料條件判斷去填excel計算欄位
            for (int i = 0 ; i < dt.Rows.Count ; i++) {
               ii_ole_row = dt.Rows[i]["RPT_SEQ_NO"].AsInt();
               li_ole_col = 0;
               switch (dt.Rows[i]["AM2_IDFG_TYPE"].AsInt()) {
                  case 1:
                     li_ole_col = 5;
                     break;
                  case 2:
                     li_ole_col = 7;
                     break;
                  case 3:
                     li_ole_col = 9;
                     break;
                  case 4:
                     li_ole_col = 11;
                     break;
                  case 5:
                     li_ole_col = 13;
                     break;
                  case 6:
                     li_ole_col = 17;
                     break;
                  case 7:
                     li_ole_col = 3;
                     break;
                  case 8:
                     li_ole_col = 15;
                     break;
               }
               if (ii_ole_row > 0 && li_ole_col > 0) {
                  worksheet.Cells[ii_ole_row - 1 , li_ole_col - 1].Value = dt.Rows[i]["AM2_OI"].AsDecimal();
               }
            }
            return true;
         } catch (Exception ex) { //失敗寫LOG
            PbFunc.f_write_logf(_ProgramID , "error" , ex.Message);
            return false;
         }


      }
   }
}
