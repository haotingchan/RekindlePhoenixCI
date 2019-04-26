using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/4/24
/// </summary>
namespace PhoenixCI.FormUI.PrefixS {
   /// <summary>
   /// WS0010 SPAN參數調整前後全市場保證金變化比較表
   /// </summary>
   public partial class WS0010 : FormParent {

      protected enum SheetNo {
         cm = 0,
         fcm = 1
      }

      private DS0010 daoS0010;

      public WS0010(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

         daoS0010 = new DS0010();

#if DEBUG
         txtDate.DateTimeValue = DateTime.ParseExact("2018/08/03" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),Date=2018/08/03";
#endif
      }

      protected override ResultStatus ActivatedForm() {

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = false;//列印

         return ResultStatus.Success;
      }

      protected void ShowMsg(string msg) {
         labMsg.Text = msg;
         labMsg.Visible = true;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export() {
         try {
            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //2. copy template xls to target path
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            //3. write data
            bool result1 = false, result2 = false;
            string tmpDate = txtDate.DateTimeValue.ToString("yyyy/MM/dd");
            result1 = wf_export(workbook , SheetNo.cm , "依結算會員" , 206 , 6 , tmpDate);
            result2 = wf_export(workbook , SheetNo.fcm , "依期貨商" , 206 , 6 , tmpDate);

            if (!result1 && !result2) {
               try {
                  workbook = null;
                  System.IO.File.Delete(excelDestinationPath);
               } catch (Exception) {
                  //
               }
               return ResultStatus.Fail;
            }

            //4.存檔並關閉
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

      /// <summary>
      /// 根據報表類型,轉出excel (其實三張報表都輸出到同一份excel)
      /// </summary>
      /// <param name="sheetNo"></param>
      /// <param name="rowTotalCount">最後一列空白行列</param>
      /// /// <param name="ls_rpt_name">sheet標題中文</param>
      /// <param name="startRowIndex">起始插入資料的row index</param>
      /// <param name="searchDate">根據reportType傳入[起始年月]或[起始年月日]</param>
      /// <returns></returns>
      protected bool wf_export(Workbook workbook , SheetNo sheetNo , string ls_rpt_name , int rowTotalCount ,
                                 int startRowIndex , string searchDate) {
         try {
            IFormatProvider culture = new System.Globalization.CultureInfo("zh-TW" , true);
            DateTime xxxDate = DateTime.ParseExact(searchDate , "yyyy/MM/dd" , culture);

            //1.讀取資料
            DataTable dtContent = new DataTable();
            if ((int)sheetNo == 0) {
               dtContent = daoS0010.d_s0010_cm(xxxDate); //結算會員
            } else {
               dtContent = daoS0010.d_s0010_fcm(xxxDate); //期貨商
            }

            //1.1 check row count
            if (dtContent.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , searchDate , _ProgramID + "－" + ls_rpt_name));
               return false;
            }

            //2.切換Sheet
            Worksheet sheet1 = workbook.Worksheets[(int)sheetNo];

            //2.1 設定資料日期
            sheet1.Cells[0 , 4].Value += searchDate;

            //3.匯出資料(單純輸出dataTable,直接用函數sheet.Import(DataTable,isNeedTitle,rowIndex,colIndex)
            sheet1.Import(dtContent , false , startRowIndex , 1);

            #region (結算會員)sheet1 處理內容的資料(old)
            //if ((int)sheetNo == 0) {
            //   ////4.處理sheet1(結算會員)下方處理內容的資料
            //   DataTable dtTemp = new DataTable();
            //dtTemp = daoS0010.d_s0010_sp2_old(xxxDate);
            //string ls_str, ls_str2, ls_str3;

            ////4.1.PSR (40070-MG2)
            //DataView dv = dtTemp.AsDataView();
            //dv.RowFilter = "SP2_TYPE = 'PSR'";
            //DataTable dt = dv.ToTable();
            //ls_str = "";
            //for (int i = 0 ; i < dt.Rows.Count ; i++) {
            //   if (!String.IsNullOrEmpty(ls_str)) {
            //      ls_str = ls_str + "、";
            //   }
            //   ls_str += dt.Rows[i]["SP2_KIND_ID1"].AsString().Trim() + ":" + string.Format("{0:#.0#%}" , dt.Rows[i]["SP1_CHANGE_RANGE"].AsDecimal());
            //}
            //sheet1.Cells[218 , 1].Value = ls_str;

            ////4.2.VSR (SV)
            //DataView dv2 = dtTemp.AsDataView();
            //dv2.RowFilter = "SP2_TYPE = 'SV'";
            //DataTable dt2 = dv.ToTable();
            //ls_str2 = "";
            //for (int i = 0 ; i < dt.Rows.Count ; i++) {
            //   if (!String.IsNullOrEmpty(ls_str2)) {
            //      ls_str2 = ls_str2 + "、";
            //   }
            //   ls_str2 += dt.Rows[i]["SP2_KIND_ID1"].AsString().Trim() + ":" + string.Format("{0:#.0#%}" , dt.Rows[i]["SP1_CHANGE_RANGE"].AsDecimal());
            //}
            //sheet1.Cells[220 , 1].Value = ls_str2;

            ////4.3.契約價值耗用比率 (SD)
            //DataView dv3 = dtTemp.AsDataView();
            //dv2.RowFilter = "SP2_TYPE = 'SD'";
            //DataTable dt3 = dv.ToTable();
            //ls_str3 = "";
            //for (int i = 0 ; i < dt.Rows.Count ; i++) {
            //   if (!String.IsNullOrEmpty(ls_str3)) {
            //      ls_str3 = ls_str3 + "、";
            //   }
            //   ls_str3 += dt.Rows[i]["SP2_KIND_ID1"].AsString().Trim() + "/" + dt.Rows[i]["SP2_KIND_ID2"].AsString().Trim() +
            //                                    ":" + string.Format("{0:#.0#%}" , dt.Rows[i]["SP1_CHANGE_RANGE"].AsDecimal());
            //}
            //sheet1.Cells[222 , 1].Value = ls_str3;
            //}
            #endregion

            //用ken改寫的SQL去兜可跑這段
            //4.如果是結算會員sheet(0)要另外填上調整內容：PSR/VSR/契約價值耗用比率
            if ((int)sheetNo == 0) {
               DataTable dtTemp = daoS0010.d_s0010_sp2(searchDate);
               if (dtTemp.Rows.Count > 0) {
                  sheet1.Cells[218 , 1].Value = dtTemp.Rows[0]["TEST"].AsString();
                  sheet1.Cells[220 , 1].Value = dtTemp.Rows[1]["TEST"].AsString();
                  sheet1.Cells[222 , 1].Value = dtTemp.Rows[2]["TEST"].AsString();
               }
            }

            //5.刪除空白列
            Range ra = sheet1.Range[string.Format("{0}:{1}" , dtContent.Rows.Count + 7 , rowTotalCount)];
            ra.Delete(DeleteMode.EntireRow);

            //5.1 把指標移到[A1]
            sheet1.Range["A1"].Select();
            sheet1.ScrollToRow(0);

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }
      }

   }
}