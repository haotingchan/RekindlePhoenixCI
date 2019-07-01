using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/3/21修改
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// /// <summary>
   /// 30201 各月份平均交易量及OI
   /// </summary>
   public partial class W30201 : FormParent {

      int monthCnt; //monthCnt為近X月的值

      public W30201(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartMon.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEndMon.DateTimeValue = GlobalInfo.OCF_DATE;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {

         try {
            #region 輸入&日期檢核 (exportbefore)
            if (!txtStartMon.IsDate(txtStartMon.Text , CheckDate.Start)
                  || !txtEndMon.IsDate(txtEndMon.Text , CheckDate.End)) {
               return ResultStatus.Fail;
            }

            if (string.Compare(txtStartMon.Text , txtEndMon.Text) > 0) {
               MessageDisplay.Error("月份起始年月不可小於迄止年月!" , GlobalInfo.ErrorText);//若多隻功能皆有相同訊息可再寫入CheckDate Enum中
               return ResultStatus.Fail;
            }

            if (txtStartMon.Text.SubStr(0 , 4).AsInt() < txtEndMon.Text.SubStr(0 , 4).AsInt() &&
               txtStartMon.Text.SubStr(5 , 2).AsInt() < txtEndMon.Text.SubStr(5 , 2).AsInt()) {
               MessageDisplay.Error("最大查詢範圍為12個月!" , GlobalInfo.ErrorText); //若多隻功能皆有相同訊息可再寫入CheckDate Enum中
               return ResultStatus.Fail;
            }
            #endregion

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //2. copy template xls to target path
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);

            //3. open xlsx
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);
            Worksheet ws = workbook.Worksheets[0];

            //4. write data (w_30201)
            labMsg.Text = this.Text + "轉檔中...";
            string startMon = txtStartMon.Text.Replace("/" , "");
            string endMon = txtEndMon.Text.Replace("/" , "");
            string kindId = "";
            int startNum = 0;

            DataTable dt = new D30201().ListData(startMon , endMon);
            DataTable dtFilter = dt.Filter("rpt_seq_no > 0");
            if (dtFilter.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}-{1},{2},無任何資料" , txtStartMon , txtEndMon , this.Text));
               return ResultStatus.Fail;
            }//if (dtFilter.Rows.Count <= 0)

            int colNum = 0, rowNum = 0;
            foreach (DataRow dr in dtFilter.Rows) {
               string ai2KindId = dr["ai2_kind_id"].AsString();
               DateTime dSymd = DateTime.ParseExact(dr["dt_symd"].AsString() , "yyyyMM" , CultureInfo.InvariantCulture);
               string dtSymd = dSymd.ToString("yyyy\\/MM");
               DateTime dEymd = DateTime.ParseExact(dr["dt_eymd"].AsString() , "yyyyMM" , CultureInfo.InvariantCulture);
               string dtEymd = dEymd.ToString("yyyy\\/MM");
               decimal avgQnty = dr["avg_qnty"].AsDecimal();
               decimal avgOi = dr["avg_oi"].AsDecimal();

               if (kindId != ai2KindId) {
                  kindId = ai2KindId;
                  if (startNum == 1) {
                     monthCnt = colNum;
                  }//if (startNum == 1)
                  startNum++;
               }//if (kindId != ai2KindId)

               rowNum = dr["rpt_seq_no"].AsInt() - 1;
               colNum = dr["month_seq_no"].AsInt();

               //首筆填入月份表頭
               if (startNum == 1) {
                  if (colNum == 1) {
                     ws.Cells[3 , colNum * 2].Value = string.Format("({0})" , dtEymd);
                  } else {
                     ws.Cells[3 , colNum * 2].Value = string.Format("({0}~{1})" , dtSymd , dtEymd);
                  }
               }//if (startNum == 1)

               //填值
               ws.Cells[rowNum , colNum * 2].Value = avgQnty;
               ws.Cells[rowNum , colNum * 2 + 1].Value = avgOi;

            }//foreach (DataRow dr in dtFilter.Rows) 
            string chineseNum = PbFunc.f_number_to_ch((long)(monthCnt - 1)); //因index從0開始，減1才正確
            ws.Cells[0 , 0].Value = string.Format("前{0}個月日均交易量與OI統計" , chineseNum);

            //5. delete blank
            if (monthCnt < 12) {
               ws.Columns.Remove((monthCnt * 2) + 2 , (12 - monthCnt) * 2);
            }

            //6. save
            workbook.SaveDocument(excelDestinationPath);
            labMsg.Visible = false;

            if (FlagAdmin)
               System.Diagnostics.Process.Start(excelDestinationPath);

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
   }
}