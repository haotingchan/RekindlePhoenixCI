using System;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using System.Threading;
using BaseGround.Shared;
using Common;
using PhoenixCI.BusinessLogic.Prefix3;
using System.IO;
using System.Data;
using DataObjects.Dao.Together.TableDao;
using Common.Helper;

namespace PhoenixCI.FormUI.Prefix3
{
   /// <summary>
   /// 證期局七組月報
   /// </summary>
   public partial class W30688 : FormParent
   {
      public W30688(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
      }

      protected override ResultStatus Open()
      {
         base.Open();
        //讀取交易日
         txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtStartDate.DateTimeValue = new DateTime(GlobalInfo.OCF_DATE.Year, GlobalInfo.OCF_DATE.Month, 1);
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();
         _ToolBtnExport.Enabled = true;
         return ResultStatus.Success;
      }

      /// <summary>
      /// 轉檔前檢查日期格式
      /// </summary>
      /// <returns></returns>
      private bool StartExport()
      {
         /*******************
         Messagebox
         *******************/
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }

      /// <summary>
      /// 轉檔後清除文字訊息
      /// </summary>
      private void EndExport()
      {
         stMsgTxt.Text = "";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         stMsgTxt.Visible = false;
      }

      /// <summary>
      /// show出訊息在label
      /// </summary>
      /// <param name="msg"></param>
      private void ShowMsg(string msg)
      {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export()
      {
         if (!StartExport()) {
            return ResultStatus.Fail;
         }

            string lsFile ="";

         try {
                DataTable dt = new BLP_OPTVOL().ListData(txtStartDate.DateTimeValue, txtEndDate.DateTimeValue, rdoGroup.EditValue.AsString());
                dt.Columns[0].ColumnName = "日期";
                dt.Columns[1].ColumnName = "時間";
                dt.Columns[2].ColumnName = "商品";
                dt.Columns[3].ColumnName = "存續期間";
                dt.Columns[4].ColumnName = "到期日";
                dt.Columns[5].ColumnName = "Delta";
                dt.Columns[6].ColumnName = "期貨履約價格";
                dt.Columns[7].ColumnName = "波動率(%)";

                if (dt.Rows.Count == 0)
                {
                    MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
                    return ResultStatus.Fail;
                }
                string etfFileName = _ProgramID+"_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
                etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, etfFileName);
                ExportOptions csvref = new ExportOptions();
                csvref.HasHeader = true;
                csvref.Encoding = System.Text.Encoding.GetEncoding(950);
                ExportHelper.ToCsv(dt, etfFileName, csvref);

            }
            catch (Exception ex) {
            if (File.Exists(lsFile))
               File.Delete(lsFile);
            WriteLog(ex);
            return ResultStatus.Fail;
         }
         finally {
            EndExport();
         }
         return ResultStatus.Success;
      }

   }
}