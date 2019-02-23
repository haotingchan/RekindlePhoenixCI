using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BaseGround;
using DataObjects.Dao.Together;
using BusinessObjects.Enums;
using BaseGround.Report;
using PhoenixCI.Shared;
using DataObjects.Dao.Together.SpecificDao;
using Common;
using BaseGround.Shared;
using System.Threading;
using DevExpress.Spreadsheet;
using System.IO;
using PhoenixCI.BusinessLogic.Prefix7;
/// <summary>
/// john,20190212,造市者交易量轉檔作業
/// </summary>
namespace PhoenixCI.FormUI.Prefix7
{
   /// <summary>
   /// 造市者交易量轉檔作業
   /// </summary>
   public partial class W70020 : FormParent
   {
      private B70020 b70020;
      private D70020 dao70020;
      private FormParentExport parentExport;
      string is_save_file;
      public W70020(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         parentExport = new FormParentExport();
         dao70020 = new D70020();
      }

      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         em_sdate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/") + "01";
         em_edate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();
         em_sdate.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      private bool ExportBefore()
      {
         /* 條件值檢核*/
         string ls_rtn;
         DialogResult li_rtn;
         if (rgTime.EditValue.Equals("rb_market1")) {
            ls_rtn = "1";
         }
         else {
            ls_rtn = "0";
         }
         //檢查批次作業是否完成
         ls_rtn = PbFunc.f_get_jsw_seq(_ProgramID, "E", 0, em_edate.Text.AsDateTime(), "0");//f_get_jsw_seq(is_txn_id,'E',0,datetime(date(em_edate.text)),'0')
         if (ls_rtn!="") {
            li_rtn = MessageBox.Show(em_edate.Text + " 統計資料未轉入完畢,是否要繼續?\r\n" + ls_rtn, GlobalInfo.gs_t_question, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(li_rtn== DialogResult.No) {
               st_msg_txt.Visible = false;
               this.Cursor = Cursors.Arrow;
               return false;
            }
         }
         //檢查日期是否符合DateTime型別
         if (!em_sdate.IsDate(em_sdate.Text, CheckDate.Start)
                  || !em_edate.IsDate(em_edate.Text, CheckDate.End)) {
            return false;
         }
         if (string.Compare(em_sdate.Text, em_edate.Text) > 0) {
            MessageDisplay.Error(GlobalInfo.gs_t_err, CheckDate.Datedif);
            return false;
         }
         //TextBox轉DateTime
         DateTime ld_s = Convert.ToDateTime(em_sdate.Text);
         DateTime ld_e = Convert.ToDateTime(em_edate.Text);
         //資料來源選取
         string ls_type;
         if (rgData.EditValue.Equals("rb_mmk")) {
            ls_type = "_M";
         }
         else {
            ls_type = "";
         }

         /*點選儲存檔案之目錄*/
         switch (rgTime.EditValue) {
            case "rb_market0":
               ls_rtn = "_ 一般";
               break;
            case "rb_market1":
               ls_rtn = "_ 盤後";
               break;
            default:
               ls_rtn = "_ 全部";
               break;
         }
         //選取儲存路徑
         is_save_file = ReportExportFunc.wf_GetFileSaveName($@"MarketMaker{ls_type}_{ls_rtn}-{ld_s.ToString("yyyyMMdd")}-{ld_e.ToString("yyyyMMdd")}.xls");
         if (string.IsNullOrEmpty(is_save_file)) {
            return false;
         }
         /*******************
         Messagebox
         *******************/
         st_msg_txt.Visible = true;
         st_msg_txt.Text = "開始轉檔...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
         return true;
      }
      
      protected void ExportAfter()
      {
         st_msg_txt.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         st_msg_txt.Visible = false;
      }
      ///// <summary>
      ///// 判斷資料量選擇要存檔的格式(xls|txt)
      ///// </summary>
      ///// <param name="dataTable">要輸出的資料</param>
      //private void saveExcel(DataTable dataTable)
      //{
      //   try {
      //      if (dataTable.Rows.Count <= 0) {
      //         MessageDisplay.Error("轉出筆數為０!", GlobalInfo.gs_t_err);
      //      }

      //      Workbook wb = new Workbook();
      //      wb.Worksheets[0].Import(dataTable, true, 0, 0);
      //      wb.Worksheets[0].Name=Path.GetFileNameWithoutExtension(is_save_file).SubStr(0,31);//sheetName不能超過31字
      //      //存檔
      //      if (dataTable.Rows.Count > 0) {
      //         if (dataTable.Rows.Count <= 65536) {
      //            wb.SaveDocument(is_save_file, DocumentFormat.Xls);
      //         }
      //         else {
      //            wb.SaveDocument(is_save_file, DocumentFormat.Text);
      //         }
      //      }
      //   }
      //   catch (Exception ex) {
      //      MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-ExportAfter");
      //      return;
      //   }
      //}

      protected override ResultStatus Export()
      {
         if (!ExportBefore()) {
            return ResultStatus.Fail;
         }
         try {
            string ls_market_code;

            //交易時段
            switch (rgTime.EditValue) {
               case "rb_market0"://一般
                  ls_market_code = "0%";
                  break;
               case "rb_market1"://盤後
                  ls_market_code = "1%";
                  break;
               default://全部
                  ls_market_code = "%";
                  break;
            }

            string startDate = em_sdate.Text.Replace("/", "").SubStr(0, 8);
            string endDate = em_edate.Text.Replace("/", "").SubStr(0, 8);
            //資料來源
            b70020 = new B70020(is_save_file);
            switch (rgData.EditValue) {
               case "rb_0"://自營商成交量(身份碼8,2)
                  b70020.exportAM8(startDate, endDate, ls_market_code);
                  break;
               case "rb_mtf"://成交資料
                  b70020.exportListO(startDate, endDate, ls_market_code);
                  break;
               case "rb_mmk"://造市者資料
                  b70020.exportListM(startDate, endDate, ls_market_code);
                  break;
               default:
                  break;
            }
            writelog();

            ExportAfter();
         }
         catch (Exception ex) {
            PbFunc.messageBox(GlobalInfo.gs_t_err, ex.Message, MessageBoxIcon.Stop);
            return ResultStatus.Fail;
         }
         
         return ResultStatus.Success;
      }
      private void writelog()
      {
         int li_rtn = PbFunc.f_write_logf(_ProgramID, "E", "轉出檔案:" + is_save_file);
         if (li_rtn < 0) {
            return;
         }
      }

      ///// <summary>
      ///// ListM輸出excel
      ///// </summary>
      ///// <param name="startDate">起始日期</param>
      ///// <param name="endDate">終止日期</param>
      ///// <param name="ls_market_code">交易時段</param>
      //public void exportListM(string startDate, string endDate, string ls_market_code)
      //{
      //   DataTable dt = dao70020.ListM(startDate, endDate, "M", ls_market_code);
      //   dt.Columns["RAMM1_BRK_TYPE"].ColumnName = "自營商(9)/一般法人(0)";
      //   dt.Columns["KIND_ID"].ColumnName = "商品";
      //   dt.Columns["BO"].ColumnName = "買一般委託";
      //   dt.Columns["BQ"].ColumnName = "買報價委託";
      //   dt.Columns["SO"].ColumnName = "賣一般委託";
      //   dt.Columns["SQ"].ColumnName = "賣報價委託";
      //   dt.Columns["MARKET_CODE"].ColumnName = "交易時段";
      //   dt.Columns["IBQ"].ColumnName = "買－報價範圍外成交";
      //   dt.Columns["ISQ"].ColumnName = "賣－報價範圍外成交";
      //   saveExcel(dt);
      //}
      ///// <summary>
      ///// ListO輸出excel
      ///// </summary>
      ///// <param name="startDate">起始日期</param>
      ///// <param name="endDate">終止日期</param>
      ///// <param name="ls_market_code">交易時段</param>
      //public void exportListO(string startDate, string endDate, string ls_market_code)
      //{
      //   DataTable dt = dao70020.ListO(startDate, endDate, "O", ls_market_code);
      //   dt.Columns["RAMM1_BRK_TYPE"].ColumnName = "自營商(9)/一般法人(0)";
      //   dt.Columns["KIND_ID"].ColumnName = "商品";
      //   dt.Columns["BO"].ColumnName = "買一般委託";
      //   dt.Columns["BQ"].ColumnName = "買報價委託";
      //   dt.Columns["SO"].ColumnName = "賣一般委託";
      //   dt.Columns["SQ"].ColumnName = "賣報價委託";
      //   dt.Columns["MARKET_CODE"].ColumnName = "交易時段";
      //   saveExcel(dt);
      //}
      ///// <summary>
      ///// AM8輸出excel
      ///// </summary>
      ///// <param name="startDate">起始日期</param>
      ///// <param name="endDate">終止日期</param>
      ///// <param name="ls_market_code">交易時段</param>
      //public void exportAM8(string startDate, string endDate, string ls_market_code)
      //{
      //   DataTable dtAM8 = dao70020.ListAM8(startDate, endDate, ls_market_code);
      //   dtAM8.Columns["AM8_YMD"].ColumnName = "日期";
      //   dtAM8.Columns["AM8_PROD_TYPE"].ColumnName = "商品別";
      //   dtAM8.Columns["AM8_FCM_NO"].ColumnName = "期貨商代號";
      //   dtAM8.Columns["AM8_PARAM_KEY"].ColumnName = "商品";
      //   dtAM8.Columns["qnty_8"].ColumnName = "造市者帳號(8)成交量";
      //   dtAM8.Columns["qnty_2"].ColumnName = "自營商帳號(2)成交量";
      //   dtAM8.Columns["MARKET_CODE"].ColumnName = "交易時段";
      //   saveExcel(dtAM8);
      //}

      protected override ResultStatus Export(ReportHelper reportHelper)
      {
         base.Export(reportHelper);

         return ResultStatus.Success;
      }

      protected override ResultStatus CheckShield()
      {
         return ResultStatus.Success;
      }

      protected override ResultStatus COMPLETE()
      {
         return ResultStatus.Success;
      }
   }
}