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
using Common;
using BusinessObjects.Enums;
using BusinessObjects;
using DevExpress.Spreadsheet;
using ActionService;
using BaseGround.Report;
using DevExpress.XtraPrinting;
using DataObjects.Dao.Together.SpecificDao;

namespace PhoenixCI.FormUI.Prefix3 {
   //Winni, 2018/12/26
   public partial class W30201 : FormParent {
      private D30201 dao30201;
      public W30201(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         dao30201 = new D30201();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtFromDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtToDate.DateTimeValue = GlobalInfo.OCF_DATE;
      }

      public override ResultStatus BeforeOpen() {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open() {
         base.Open();

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {
         base.Retrieve();

         return ResultStatus.Success;
      }

      protected override ResultStatus CheckShield() {
         base.CheckShield();

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {
         base.Save(pokeBall);

         return ResultStatus.Success;
      }

      protected override ResultStatus Run(PokeBall args) {
         base.Run(args);

         return ResultStatus.Success;
      }

      protected override ResultStatus Import() {
         base.Import();

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {
         base.Export();

         string excelDestinationPath = CopyExcelTemplateFile(_ProgramID , FileType.XLS);

         if (Convert.ToInt32(txtToDate.FormatValue) < Convert.ToInt32(txtFromDate.FormatValue)) {
            MessageDisplay.Info("月份起始年月不可小於迄止年月!");
         } else if (Convert.ToInt32(txtFromDate.FormatValue.Substring(0 , 4)) < Convert.ToInt32(txtToDate.FormatValue.Substring(0 , 4)) &&
            Convert.ToInt32(txtFromDate.FormatValue.Substring(4 , 2)) < Convert.ToInt32(txtToDate.FormatValue.Substring(4 , 2))) {
            MessageDisplay.Info("最大查詢範圍為12個月!");
         } else {
            ManipulateExcel(excelDestinationPath);
         }

         return ResultStatus.Success;
      }

      private void ManipulateExcel(string excelDestinationPath) {

         //string ls_rpt_name, ls_rpt_id;
         int li_ole_col, ii_ole_row, li_month_cnt, li_start, i;
         string ls_kind_id;
         /*************************************
         ls_rpt_name = 報表名稱
         ls_rpt_id = 報表代號
         *************************************/
         //ls_rpt_name = "各月份平均交易量及OI";
         //ls_rpt_id = "30201";
         //st_msg_txt.text = ls_rpt_id + "－" + ls_rpt_name + " 轉檔中...";

         #region Excel

         //讀取資料
         DataTable dtContent = dao30201.ListData(txtFromDate.FormatValue , txtToDate.FormatValue);
         //ids_1.setfilter("rpt_seq_no > 0")
         //ids_1.filter()
         if (dtContent.Rows.Count == 0) {
            MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtFromDate.Text , this.Text));
            return;
         }

         //填資料
         Workbook workbook = new Workbook();
         workbook.LoadDocument(excelDestinationPath);
         Worksheet worksheet = workbook.Worksheets[0];

         ls_kind_id = "";
         li_ole_col = 0;
         ii_ole_row = 0;
         li_month_cnt = 0; //近X月的值
         li_start = 0;

         for (i = 0 ; i < dtContent.Rows.Count ; i++) {
            if (ls_kind_id != dtContent.Rows[i]["ai2_kind_id"].ToString()) {
               ls_kind_id = dtContent.Rows[i]["ai2_kind_id"].ToString();
               if (li_start == 1) {
                  li_month_cnt = li_ole_col;
               }
               li_start += 1;
            }
            ii_ole_row = dtContent.Rows[i]["rpt_seq_no"].AsInt() - 1; //原PB ii_ole_row = 6
            li_ole_col = dtContent.Rows[i]["month_seq_no"].AsInt(); //原PB li_ole_col = 1

            //首筆填入月份表頭
            if (li_start == 1) {
               if (li_ole_col == 1) {
                  worksheet.Cells[3 , (li_ole_col * 2)].Value =
                  "(" + String.Format(dtContent.Rows[i]["dt_eymd"].ToString().Trim() , "@@@@/@@") + ")";
               } else {
                  worksheet.Cells[3 , (li_ole_col * 2)].Value =
                  "(" + String.Format(dtContent.Rows[i]["dt_symd"].ToString().Trim() , "@@@@/@@") + ")" + "~" +
                  String.Format(dtContent.Rows[i]["dt_eymd"].ToString().Trim() , "@@@@/@@") + ")";
               }
            }

            //填值
            worksheet.Cells[ii_ole_row , (li_ole_col * 2)].Value = dtContent.Rows[i]["avg_qnty"].AsDecimal();
            worksheet.Cells[ii_ole_row , (li_ole_col * 2) + 1].Value = dtContent.Rows[i]["avg_oi"].AsDecimal();

         }
         worksheet.Cells[0 , 0].Value = "前" + NumberToChinese(li_month_cnt) + "個月日均交易量與OI統計";


         //刪除空白欄
         if (li_month_cnt < 12) {
            worksheet.Columns.Remove((li_month_cnt * 2) + 2 , (12 - li_month_cnt) * 2);
         }

         workbook.SaveDocument(excelDestinationPath);

         #endregion

      }

      /// <summary>
      /// 將選取資料之總月份換成中文
      /// </summary>
      /// <param name="ai_num"></param>
      /// <returns></returns>
      private string NumberToChinese(int ai_num) {
         string ls_num, ls_ch, ls_rtn = "";
         int li_len;

         ls_num = ai_num.ToString();
         li_len = ls_num.Length;

         for (int i = 0 ; i < li_len ; i++) {
            ls_ch = ls_num.Substring(i , 1);
            switch (ls_ch) {
               case "0":
                  ls_ch = "";
                  break;
               case "1":
                  ls_ch = "一";
                  break;
               case "2":
                  ls_ch = "二";
                  break;
               case "3":
                  ls_ch = "三";
                  break;
               case "4":
                  ls_ch = "四";
                  break;
               case "5":
                  ls_ch = "五";
                  break;
               case "6":
                  ls_ch = "六";
                  break;
               case "7":
                  ls_ch = "七";
                  break;
               case "8":
                  ls_ch = "八";
                  break;
               case "9":
                  ls_ch = "九";
                  break;
            }
            switch (i) {
               case 0:
                  switch (li_len) {
                     case 2:
                        if (ls_num.Substring(0 , 1) == "1") {
                           ls_ch = "十";
                        } else {
                           ls_ch = ls_ch + "十";
                        }
                        break;
                     case 3:
                        ls_ch = ls_ch + "百";
                        break;
                  }
                  break;
               case 1:
                  switch (li_len) {
                     case 3:
                        ls_ch = ls_ch + "十";
                        break;
                  }
                  break;
            }
            ls_rtn = ls_rtn + ls_ch;
         }

         return ls_rtn;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         base.InsertRow();

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow();

         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose() {
         return base.BeforeClose();
      }
   }
}