using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;

/// <summary>
/// ken,2019/4/8
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 黃金期貨契約價量資料
   /// </summary>
   public partial class W30395 : FormParent {

      private D30395 dao30395;

      public enum SheetType {
         [Description("「黃金」期貨契約價量資料")]
         GDF = 0,
         [Description("「新台幣計價黃金」期貨契約價量資料")]
         TGF,
         [Description("「黃金」期貨契約價量資料(買賣方比重)")]
         GDF_Detail,
         [Description("「新台幣計價黃金」期貨契約價量資料(買賣方比重)")]
         TGF_Detail
      }

      public enum GridName { First, Second }

      protected class LookupItem {
         public string ValueMember { get; set; }
         public string DisplayMember { get; set; }
      }

      public W30395(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao30395 = new D30395();

      }

      protected override ResultStatus Open() {
         base.Open();

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();

         txtStartMonth.DateTimeValue = GlobalInfo.OCF_DATE;

#if DEBUG
         txtStartMonth.EditValue = DateTime.ParseExact("2018/10/11" , "yyyy/MM/dd" , null).ToString("yyyy/MM");
         this.Text += "(開啟測試模式)";
#endif


         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

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


      protected override ResultStatus Export() {
         try {
            //1.開始轉出資料
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "訊息：資料轉出中........";
            this.Refresh();


            //1.1 copy template xls to target path
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , _ProgramID);
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            //2.匯出資料
            //共同的參數一起設定,init param = { D30395 dao, string startMonth }
            Object[] args = { dao30395 , txtStartMonth.Text };

            int pos = 0;
            IReportData GDF = CreateReport(GetType() , "GDF" , args);
            pos += GDF.Export(workbook);

            //IReportData GDF_Detail = CreateReport(GetType(), "GDF_Detail", args);
            //GDF_Detail.Export(workbook);

            IReportData TGF = CreateReport(GetType() , "TGF" , args);
            pos += TGF.Export(workbook);

            //IReportData TGF_Detail = CreateReport(GetType(), "TGF_Detail", args);
            //TGF_Detail.Export(workbook);

            if (pos <= 0) {
               File.Delete(excelDestinationPath);
               MessageDisplay.Info("查無資料!");
               return ResultStatus.Fail;
            }

            //存檔
            workbook.SaveDocument(excelDestinationPath);

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



      public interface IReportData {

         /// <summary>
         /// 匯出資料
         /// </summary>
         /// <param name="workbook"></param>
         int Export(Workbook workbook);
      }

      /// <summary>
      /// 轉換介面 (注意,內部的class執行時其fullName是用+號連結起來)
      /// </summary>
      /// <param name="type"></param>
      /// <param name="name"></param>
      /// <param name="args">該物件初始化時候必要的參數</param>
      /// <returns></returns>
      public IReportData CreateReport(Type type , string name , Object[] args = null) {
         //讀取dll/主要exe的寫法
         string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = type.FullName + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
         return (IReportData)Assembly.Load(AssemblyName).CreateInstance(className , true , BindingFlags.CreateInstance , null , args , null , null);

      }

      public class Report30395 : IReportData {
         protected D30395 Dao { get; }
         protected SheetType SheetIndex { get; set; }

         protected string StartMonth { get; set; }
         protected string KindId { get; set; }
         protected int RowBegin { get; set; }

         public Report30395(D30395 dao , string startMonth) {
            Dao = dao;
            StartMonth = startMonth;
         }

         /// <summary>
         /// 匯出資料
         /// </summary>
         /// <param name="workbook"></param>
         public virtual int Export(Workbook workbook) {
            int pos = 0;

            Worksheet ws = workbook.Worksheets[(int)SheetIndex];
            pos += ExportSummary(workbook , (int)SheetIndex , 1);

            int tempSheetIndex = (int)SheetIndex + 2;
            pos += ExportDetail(workbook , tempSheetIndex , 3);
            return pos;
         }

         protected virtual int ExportSummary(Workbook workbook , int sheetIndex , int rowBegin) {
            //1.1 前月倒數2天交易日
            DateTime ldt_sdate = PbFunc.f_get_last_day("AI3" , KindId , StartMonth , 2);

            //1.2 抓當月最後交易日
            DateTime ldt_edate = PbFunc.f_get_end_day("AI3" , KindId , StartMonth);

            Worksheet ws = workbook.Worksheets[sheetIndex];
            int rowIndex = rowBegin;
            int emptyRowCount = rowIndex + 1 + 32;

            //1.3 get ai3 data
            DataTable dtTemp = Dao.d_ai3(KindId , ldt_sdate , ldt_edate);
            if (dtTemp.Rows.Count <= 0) {
               //刪除空白列
               if (rowIndex < emptyRowCount) {
                  string selectBegin = (rowIndex + 2).ToString();
                  string selectEnd = (emptyRowCount).ToString();
                  string cellRange = string.Format("A{0}:G{1}" , selectBegin , selectEnd);
                  ws.DeleteCells(ws.Range[cellRange] , DeleteMode.EntireRow);
               }
               return 0;
            }

            //1.4 export to sheet
            DateTime ldt_ymd = DateTime.MinValue;
            foreach (DataRow dr in dtTemp.Rows) {
               DateTime ai3_date = dr["ai3_date"].AsDateTime();
               if (ldt_ymd != ai3_date) {
                  ldt_ymd = ai3_date;
                  rowIndex++;
                  ws.Cells[rowIndex , 0].Value = ldt_ymd.ToString("MM/dd");
               }

               ws.Cells[rowIndex , 1].Value = dr["ai3_close_price"].AsDecimal();
               ws.Cells[rowIndex , 3].Value = dr["ai3_m_qnty"].AsDecimal();
               ws.Cells[rowIndex , 4].Value = dr["ai3_oi"].AsDecimal();
               ws.Cells[rowIndex , 5].Value = dr["ai3_index"].AsDecimal();

            }//foreach (DataRow dr in dtTemp.Rows) {

            //1.5 刪除空白列(注意,沒刪除好會影響到後面sheet的圖表)
            if (rowIndex < emptyRowCount) {
               string selectBegin = (rowIndex + 2).ToString();
               string selectEnd = (emptyRowCount).ToString();
               string cellRange = string.Format("A{0}:G{1}" , selectBegin , selectEnd);
               ws.DeleteCells(ws.Range[cellRange] , DeleteMode.EntireRow);
               //ken,用DeleteCells還是不行,測試結果似乎xlsx的圖表公式一直固定,不會更新
            }
            return 1;
         }

         protected virtual int ExportDetail(Workbook workbook , int sheetIndex , int rowBegin) {

            Worksheet ws = workbook.Worksheets[sheetIndex];
            int rowIndex = rowBegin;
            int emptyRowCount = rowIndex + 12;

            //2.1 get ai2 data
            DataTable dtDetail = Dao.d_ai2(KindId , StartMonth.SubStr(0 , 4) + "01" , StartMonth.Replace("/" , ""));
            if (dtDetail.Rows.Count <= 0) {
               if (rowIndex < emptyRowCount) {
                  ws.Rows.Remove(rowIndex + 1 , emptyRowCount - rowIndex);
               }
               return 0;
            }


            //2.2 總列數         
            ws.Cells[rowIndex + 12 + 1 , 0].Value = (StartMonth.SubStr(0 , 4).AsInt() - 1911).AsString() + "小計";
            string ls_ymd = "";//日期

            foreach (DataRow dr in dtDetail.Rows) {
               string am2_ymd = dr["am2_ymd"].AsString();
               int am2_idfg_type = dr["am2_idfg_type"].AsInt();
               string am2_bs_code = dr["am2_bs_code"].AsString();
               Decimal am2_m_qnty = dr["am2_m_qnty"].AsDecimal();

               if (ls_ymd != am2_ymd) {
                  ls_ymd = am2_ymd;
                  rowIndex++;
                  //li_month_cnt++;
                  string chineseMonth = (ls_ymd.SubStr(0 , 4).AsInt() - 1911).AsString() + "/" + ls_ymd.SubStr(4 , 2);
                  ws.Cells[rowIndex , 0].Value = chineseMonth;
               }

               //ken,get li_ole_col , but 順序亂跳
               int li_ole_col = (am2_bs_code == "B" ? -1 : 0);
               switch (am2_idfg_type) {
                  case 1:
                     li_ole_col += 2;
                     break;
                  case 2:
                     li_ole_col += 4;
                     break;
                  case 3:
                     li_ole_col += 6;
                     break;
                  //沒有case 4:
                  case 5:
                     li_ole_col += 8;
                     break;
                  case 6:
                     li_ole_col += 10;
                     break;
                  case 7:
                     li_ole_col += 14;
                     break;
                  case 8:
                     li_ole_col += 12;
                     break;
               }
               ws.Cells[rowIndex , li_ole_col].Value = am2_m_qnty;

            }//foreach (DataRow dr in dtDetail.Rows) {


            //2.5 刪除空白列
            if (rowIndex < emptyRowCount) {
               ws.Rows.Remove(rowIndex + 1 , emptyRowCount - rowIndex);
            }
            return 1;
         }
      }

      /// <summary>
      /// 黃金期貨
      /// </summary>
      public class GDF : Report30395 {

         public GDF(D30395 dao , string startMonth)
                     : base(dao , startMonth) {
            //初始化設定,先讓父類別把共用參數設定好,再設定自己特定的參數

            KindId = "GDF";
            SheetIndex = SheetType.GDF;
            //RowBegin = 2;

         }

         public override int Export(Workbook workbook) {
            return base.Export(workbook);
         }
      }

      /// <summary>
      /// 新台幣計價黃金
      /// </summary>
      public class TGF : Report30395 {

         public TGF(D30395 dao , string startMonth)
                     : base(dao , startMonth) {
            //初始化設定,先讓父類別把共用參數設定好,再設定自己特定的參數

            KindId = "TGF";
            SheetIndex = SheetType.TGF;
            //RowBegin = 2;

         }

         public override int Export(Workbook workbook) {
            return base.Export(workbook);
         }
      }

   }
}