using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;

/// <summary>
/// Winni, 2019/3/13
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// SPAN參數一覽表
   /// </summary>
   public partial class W40150 : FormParent {

      protected enum SheetNo {
         DPSR = 0,
         VSR = 1
      }

      public W40150(string programID , string programName) : base(programID , programName) {
         InitializeComponent();

         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartDate.DateTimeValue = DateTime.Now;

#if DEBUG
         //ken test
         txtStartDate.DateTimeValue = DateTime.ParseExact("2018/06/13" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),Date=2018/06/13";
#endif

      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      /// <summary>
      /// Export return 1 txt & 1 excel
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Export() {
         try {

            //2. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "訊息：" + this.Text + "轉檔中...";
            this.Refresh();

            //2.1 copy template xls to target path
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);

            //2.2 open xls
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            //2.3 寫入資料
            bool res1 = false, res2 = false;
            res1 = wf_40150(workbook , SheetNo.DPSR);
            res2 = wf_40152(workbook , SheetNo.VSR);

                //2.3 關閉、儲存檔案
                if (!res1 && !res2) {
                    workbook = null;
                    File.Delete(excelDestinationPath);
                    labMsg.Visible = false;
                    return ResultStatus.Fail;
                }
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
         }
         return ResultStatus.Fail;
      }

      /// <summary>
      /// wf_40150
      /// </summary>
      /// <returns></returns>
      protected bool wf_40150(Workbook workbook , SheetNo sheetNo) {
         try {
            //1. 切換Sheet
            Worksheet worksheet = workbook.Worksheets[(int)sheetNo];
            worksheet.Range["A1"].Select();

            //2. 填資料
            DataTable dt = new D40150().GetDataList(txtStartDate.DateTimeValue);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0},讀取「SPAN參數一覽表」無任何資料!" , txtStartDate.Text));
                    return false;
            }//if (dt.Rows.Count <= 0 )

            DataTable dtSp2 = new D40150().ListSp2ByDate(txtStartDate.DateTimeValue);

            int row = 1;
            foreach (DataRow dr in dt.Rows) {
               row++;
               worksheet.Cells[row - 1 , 0].Value = dr["sp1_seq_no"].AsInt();
               worksheet.Cells[row - 1 , 1].Value = dr["spt1_com_id"].AsString();
               worksheet.Cells[row - 1 , 4].Value = dr["spt1_kind_id1_out"].AsString();
               worksheet.Cells[row - 1 , 7].Value = dr["spt1_kind_id2_out"].AsString();

               string sp1KindId1 = dr["sp1_kind_id1"].AsString();
               string sp1KindId2 = dr["sp1_kind_id2"].AsString();
               string txt = "";

               //SS
               txt = "sp2_type='SS' and sp2_kind_id1='" + sp1KindId1 + "' and sp2_kind_id2='" + sp1KindId2 + "' ";
               DataRow[] drtmp = dtSp2.Select(txt);
               if (drtmp.Length != 0) { //不是空陣列
                  worksheet.Cells[row - 1 , 2].Value = dr["SS_sp1_rate"].AsDecimal();
                  worksheet.Cells[row - 1 , 2].Font.Bold = true;
               } else {
                  worksheet.Cells[row - 1 , 2].Value = dr["SS_sp1_cur_rate"].AsDecimal();
               }//if (drtmp.Length != 0)

               //SD
               txt = "sp2_type='SD' and sp2_kind_id1='" + sp1KindId1 + "' and sp2_kind_id2='" + sp1KindId2 + "' ";
               DataRow[] drtmp2 = dtSp2.Select(txt);
               if (drtmp2.Length != 0) { //不是空陣列
                  worksheet.Cells[row - 1 , 6].Value = dr["SD_sp1_rate"].AsDecimal();
                  worksheet.Cells[row - 1 , 6].Font.Bold = true;
               } else {
                  worksheet.Cells[row - 1 , 6].Value = dr["SD_sp1_cur_rate"].AsDecimal();
               }//if (drtmp2.Length != 0)

            }//foreach (DataRow dr in dt.Rows)

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }
      }

      /// <summary>
      /// wf_40152
      /// </summary>
      /// <returns></returns>
      protected bool wf_40152(Workbook workbook , SheetNo sheetNo) {
         try {

            labMsg.Text = "訊息：40152-" + _ProgramName + "轉檔中...";

            //1. 切換Sheet
            Worksheet worksheet = workbook.Worksheets[(int)sheetNo];
            worksheet.Range["A1"].Select();

            //2. 填資料
            DataTable dt2 = new D40150().ListByDate(txtStartDate.DateTimeValue);
            if (dt2.Rows.Count <= 0) {
               MessageDisplay.Info(String.Format("{0},讀取「SPAN參數一覽表」無任何資料!" , txtStartDate.Text));
                    return false;
                }//if (dt.Rows.Count <= 0 )

            DataTable dtSp2 = new D40150().ListSp2ByDate(txtStartDate.DateTimeValue);

            //int row = 1;
            foreach (DataRow dr in dt2.Rows) {
               int row = dr["rpt_seq_no"].AsInt();

               string sp1KindId1 = dr["sp1_kind_id1"].AsString();
               string txt = "";

               //SS
               txt = "sp2_type='SV' and sp2_kind_id1='" + sp1KindId1 + "'";
               DataRow[] drtmpSp2 = dtSp2.Select(txt);

               if (drtmpSp2.Length != 0) { //不是空陣列
                  worksheet.Cells[row - 1 , 1].Value = dr["SD_sp1_rate"].AsDecimal();
                  worksheet.Cells[row - 1 , 1].Font.Bold = true;
               } else {
                  worksheet.Cells[row - 1 , 1].Value = dr["SD_sp1_cur_rate"].AsDecimal();
               }//if (found > 0)

            }//foreach (DataRow dr in dt.Rows)

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }
      }
   }
}