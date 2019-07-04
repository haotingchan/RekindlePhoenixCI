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
using System.Windows.Forms;

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
         daoAE6 = new AE6();
         this.Text = _ProgramID + "─" + _ProgramName;

         //取得資料庫內最大日期
         DataTable tmpDt = daoAE6.MaxDate();
         string tmpDate = tmpDt.Rows[0][0].AsString(); //20171110
         if (tmpDate != "") {
            //本週
            txtAftEymd.DateTimeValue = DateTime.ParseExact(tmpDate.SubStr(0 , 4) + "/" + tmpDate.SubStr(4 , 2) + "/" + tmpDate.SubStr(6 , 2) , "yyyy/MM/dd" , null);
            txtAftSymd.DateTimeValue = txtAftEymd.DateTimeValue.AddDays(-6);
            //上週
            txtPrevEymd.DateTimeValue = txtAftSymd.DateTimeValue.AddDays(-1);
            txtPrevSymd.DateTimeValue = txtPrevEymd.DateTimeValue.AddDays(-6);
            //全期
            txtAllEymd.DateTimeValue = txtAftEymd.DateTimeValue;
            txtAllSymd.DateTimeValue = DateTime.ParseExact("2014/05/15" , "yyyy/MM/dd" , null);//照PB寫死
         }

      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {

         try {
            #region 日期檢核
            if (string.Compare(txtPrevSymd.Text , txtPrevEymd.Text) > 0) {
               MessageDisplay.Error("上週起始日期不可小於迄止日期!" , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            if (string.Compare(txtAftSymd.Text , txtAftEymd.Text) > 0) {
               MessageDisplay.Error("本週起始日期不可小於迄止日期!" , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            if (string.Compare(txtAllSymd.Text , txtAllEymd.Text) > 0) {
               MessageDisplay.Error("全期起始日期不可小於迄止日期!" , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }
            #endregion

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
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected void ShowMsg(string msg) {
         labMsg.Text = msg;
         labMsg.Visible = true;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected bool wfExport(Workbook workbook , SheetNo sheetNo) {
         try {
            string rptName = "Eurex FTX vs TX 振幅、波動度及交易量統計(總表)";
            ShowMsg(string.Format("{0}－{1} 轉檔中..." , _ProgramID , rptName));

            //1.讀取資料
            DataTable dt = new DataTable();
            if ((int)sheetNo == 0) {
               dt = dao30660.GetData(PrevStart , PrevEnd , AftStart , AftEnd , AllStart , AllEnd);  // Eurex vs TX
            } else {
               dt = dao30660.GetDetailData(AllStart , AllEnd); //每日明細
            }

            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , AllStart + "-" + AllEnd , this.Text) , GlobalInfo.ResultText);
               return false;
            }

            //2.切換Sheet填資料
            Worksheet worksheet = workbook.Worksheets[(int)sheetNo];
            if ((int)sheetNo == 0) {

               li_ole_row = 3;
               for (int i = 0 ; i < dt.Rows.Count ; i++) {
                  li_ole_row += 1;
                  for (int j = 0 ; j < 14 ; j++) {
                     if (j == 0) {
                        worksheet.Cells[li_ole_row - 1 , j].Value = dt.Rows[i][j].AsString();
                     } else {
                        if (dt.Rows[i][j] != DBNull.Value) {
                           worksheet.Cells[li_ole_row - 1 , j].Value = dt.Rows[i][j].AsDecimal();
                        }
                     }
                  }
               }
            } else {
               li_ole_row = 3;
               for (int i = 0 ; i < dt.Rows.Count ; i++) {
                  li_ole_row += 1;
                  for (int j = 0 ; j < 19 ; j++) {
                     if (j == 0) {
                        worksheet.Cells[li_ole_row - 1 , j].Value = dt.Rows[i][j].AsDateTime();
                     } else {
                        if (dt.Rows[i][j] != DBNull.Value) {
                           worksheet.Cells[li_ole_row - 1 , j].Value = dt.Rows[i][j].AsDecimal();
                        }
                     }
                  }
               }
            }

            return true;

         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
            this.Cursor = Cursors.Arrow;
         }
      }

   }
}