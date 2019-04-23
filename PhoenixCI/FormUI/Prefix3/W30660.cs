using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.TableDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Threading;

/// <summary>
/// Winni, 2019/02/18
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30650 專、兼營期貨商當沖交易量統計
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30660 : FormParent {

      private D30660 dao30660;
      private AE6 daoAE6;
      private int li_ole_row;

      protected enum SheetNo {
         eurex = 0,
         detail = 1
      }

      #region 一般條件查詢縮寫
      /// <summary>
      /// 本週起：yyyyMMdd
      /// </summary>
      public string AftStart {
         get {
            return txtAftSymd.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// 本周訖：yyyyMMdd
      /// </summary>
      public string AftEnd {
         get {
            return txtAftEymd.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// 上週起：yyyyMMdd
      /// </summary>
      public string PrevStart {
         get {
            return txtPrevSymd.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// 上週訖：yyyyMMdd
      /// </summary>
      public string PrevEnd {
         get {
            return txtPrevEymd.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// 全期起：yyyyMMdd
      /// </summary>
      public string AllStart {
         get {
            return txtAllSymd.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// 全期訖：yyyyMMdd
      /// </summary>
      public string AllEnd {
         get {
            return txtAllEymd.DateTimeValue.ToString("yyyyMMdd");
         }
      }
      #endregion

      public W30660(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         dao30660 = new D30660();
         this.Text = _ProgramID + "─" + _ProgramName;

         //取得資料庫內最大日期
         DataTable tmpDt = new AE6().MaxDate();
         string tmpDate = tmpDt.Rows[0][0].AsString(); //20171110
         if (tmpDate != "") {
            //本週
            txtAftEymd.Text = tmpDate.SubStr(0 , 4) + "/" + tmpDate.SubStr(4 , 2) + "/" + tmpDate.SubStr(6 , 2);
            txtAftSymd.Text = (DateTime.ParseExact(txtAftEymd.Text , "yyyy/MM/dd" , null).AddDays(-6)).AsString().SubStr(0 , 10);
            //上週
            txtPrevEymd.Text = (DateTime.ParseExact(txtAftSymd.Text , "yyyy/MM/dd" , null).AddDays(-1)).AsString().SubStr(0 , 10);
            txtPrevSymd.Text = (DateTime.ParseExact(txtPrevEymd.Text , "yyyy/MM/dd" , null).AddDays(-6)).AsString().SubStr(0 , 10);
            //全期
            txtAllEymd.Text = txtAftEymd.Text;
            txtAllSymd.Text = "2017/11/01";
            //txtAllSymd.Text = "2014/5/15"; //照PB寫死
         }

      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {

         #region 檢查日期起訖
         if (txtAftSymd.DateTimeValue > txtAftEymd.DateTimeValue) {
            MessageDisplay.Error(string.Format("{0}起日期({1})不可大於迄日期({2})" , labDate1.Text , txtAftSymd.Text , txtAftEymd.Text));
            return ResultStatus.Fail;
         }

         if (txtPrevSymd.DateTimeValue > txtPrevEymd.DateTimeValue) {
            MessageDisplay.Error(string.Format("{0}起日期({1})不可大於迄日期({2})" , labDate2.Text , txtPrevSymd.Text , txtPrevEymd.Text));
            return ResultStatus.Fail;
         }

         if (txtAllSymd.DateTimeValue > txtAllEymd.DateTimeValue) {
            MessageDisplay.Error(string.Format("{0}起日期({1})不可大於迄日期({2})" , labDateAll.Text , txtAllSymd.Text , txtAllEymd.Text));
            return ResultStatus.Fail;
         }

         #endregion

         try {
            labMsg.Visible = true;

            //1.複製檔案 & 開啟檔案
            string excelDestinationPath = CopyExcelTemplateFile(_ProgramID , FileType.XLS);
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);
            //Worksheet worksheet = workbook.Worksheets[0];

            //2.填資料
            bool result1 = false, result2 = false;
            result1 = wfExport(workbook , SheetNo.eurex);
            if (chkDetail.CheckState.AsString() == "Checked") {
               result2 = wfExport(workbook , SheetNo.detail);
            }

            if (!result1 && !result2) {
               try {
                  workbook = null;
                  System.IO.File.Delete(excelDestinationPath);
               } catch (Exception) {
                  //
               }
               return ResultStatus.Fail;
            }

            //存檔並關閉
            workbook.SaveDocument(excelDestinationPath);
            labMsg.Visible = false;
            return ResultStatus.Success;

         } catch (Exception ex) {
            PbFunc.f_write_logf(_ProgramID , "Error" , ex.Message);
            MessageDisplay.Error(ex.Message);
         }
         return ResultStatus.Fail;
      }

      protected void ShowMsg(string msg) {
         labMsg.Text = msg;
         labMsg.Visible = true;
         this.Refresh();
         Thread.Sleep(5);
      }

      private bool wfExport(Workbook workbook , SheetNo sheetNo) {
         try {
            string rptName = "Eurex FTX vs TX 振幅、波動度及交易量統計(總表)";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , _ProgramID , rptName));

            //1.讀取資料
            DataTable dtContent = new DataTable();
            if ((int)sheetNo == 0) {
               dtContent = dao30660.GetData(PrevStart , PrevEnd , AftStart , AftEnd , AllStart , AllEnd); ; // Eurex vs TX
            } else {
               dtContent = dao30660.GetDetailData(AllStart , AllEnd); //每日明細
            }

            if (dtContent.Rows.Count == 0) {
               MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , AllStart + "-" + AllEnd , this.Text));
               return false;
            }

            //2.切換Sheet填資料
            Worksheet worksheet = workbook.Worksheets[(int)sheetNo];
            if ((int)sheetNo == 0) {

               li_ole_row = 3;
               for (int i = 0 ; i < dtContent.Rows.Count ; i++) {
                  li_ole_row += 1;
                  for (int j = 0 ; j < 14 ; j++) {
                     worksheet.Cells[li_ole_row - 1 , j].Value = dtContent.Rows[i][j].AsDecimal();               
                  }
               }
            } else {
               li_ole_row = 3;
               for (int i = 0 ; i < dtContent.Rows.Count ; i++) {
                  li_ole_row += 1;
                  for (int j = 0 ; j < 19 ; j++) {
                     worksheet.Cells[li_ole_row - 1 , j].Value = dtContent.Rows[i][j].AsDecimal();
                  }
               }
            }

            return true;

         } catch (Exception ex) {
            PbFunc.f_write_logf(_ProgramID , "error" , ex.Message);
            return false;
         }
      }

   }
}