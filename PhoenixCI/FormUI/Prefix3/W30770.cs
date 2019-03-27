using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;

/// <summary>
/// ken,2019/3/26
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 延長交易時間商品13:45後交易量比重
   /// </summary>
   public partial class W30770 : FormParent {

      private D30770 dao30770;

      public enum SheetType {
         [Description("期貨日明細")]
         FutureDetail = 0,
         [Description("期貨月統計")]
         FutureSum,
         [Description("選擇權日明細")]
         OptionDetail,
         [Description("選擇權月統計")]
         OptionSum
      }

      public enum GridName { First, Second }

      protected class LookupItem {
         public string ValueMember { get; set; }
         public string DisplayMember { get; set; }
      }


      #region 抓取畫面元件Value(主要是縮寫)
      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string StartMonth {
         get {
            return txtStartMonth.DateTimeValue.ToString("yyyyMM  ");//ken,無言了,要多塞兩個空白char(8),不然where會無資料
         }
      }

      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string EndMonth {
         get {
            return txtEndMonth.DateTimeValue.ToString("yyyyMM  ");//ken,無言了,要多塞兩個空白char(8),不然where會無資料
         }
      }

      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string StartDate {
         get {
            return txtStartDate.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string EndDate {
         get {
            return txtEndDate.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// 5% or % or 7%
      /// </summary>
      public string OswGrp {
         get {
            return ddlOswGrp.EditValue.ToString();
         }
      }

      /// <summary>
      /// OswGrpText
      /// </summary>
      public string OswGrpText {
         get {
            return ddlOswGrp.Text;
         }
      }
      #endregion

      public W30770(string programID, string programName) : base(programID, programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao30770 = new D30770();

      }

      protected override ResultStatus Open() {
         base.Open();

         //設定 營業時間 下拉選單
         List<LookupItem> lstType = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "5%", DisplayMember = "13:45 - 16:15"},
                                        new LookupItem() { ValueMember = "%", DisplayMember = "13:45 - 18:15" },
                                        new LookupItem() { ValueMember = "7%", DisplayMember = "16:15 - 18:15" }};
         Extension.SetDataTable(ddlOswGrp, lstType, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, "");

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();

         txtEndDate.EditValue = PbFunc.f_ocf_date(0);
         txtStartDate.EditValue = txtEndDate.DateTimeValue.ToString("yyyy/MM/01");
         txtStartMonth.EditValue = txtEndDate.DateTimeValue.ToString("yyyy/MM");
         txtEndMonth.EditValue = txtEndDate.DateTimeValue.ToString("yyyy/MM");

#if DEBUG
         txtStartMonth.DateTimeValue = DateTime.ParseExact("2018/10/01", "yyyy/MM/dd", null);
         txtEndMonth.DateTimeValue = DateTime.ParseExact("2018/10/01", "yyyy/MM/dd", null);
         txtStartDate.DateTimeValue = DateTime.ParseExact("2018/10/01", "yyyy/MM/dd", null);
         txtEndDate.DateTimeValue = DateTime.ParseExact("2018/10/11", "yyyy/MM/dd", null);
         this.Text += "(開啟測試模式)";
#endif

         ddlOswGrp.ItemIndex = 1;// % = 13:45 - 18:15

         if (FlagAdmin)
            ddlOswGrp.Visible = true;

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
            string excelDestinationPath = CopyExcelTemplateFile(_ProgramID, FileType.XLSX);
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);

            #region //ken,第一次改寫(廢除)
            //if (chkGroup.Items[1].CheckState == System.Windows.Forms.CheckState.Checked) {
            //   if (rdoGroup.Text == "期貨" || rdoGroup.Text == "全部")
            //      wf_30770_date(workbook, "F%", SheetType.FutureDetail, StartDate, EndDate, OswGrp);//sheetName = 期貨日明細

            //   if (rdoGroup.Text == "選擇權" || rdoGroup.Text == "全部")
            //      wf_30770_date(workbook, "O%", SheetType.FutureDetail, StartDate, EndDate, OswGrp);//sheetName = 選擇權日明細
            //}//if (chkGroup.Items[1].CheckState == System.Windows.Forms.CheckState.Checked) {

            //if (chkGroup.Items[0].CheckState == System.Windows.Forms.CheckState.Checked) {
            //   if (rdoGroup.Text == "期貨" || rdoGroup.Text == "全部")
            //      wf_30770_mth(workbook, "F%", SheetType.FutureSum, StartDate, EndDate, OswGrp);//sheetName = 期貨月統計

            //   if (rdoGroup.Text == "選擇權" || rdoGroup.Text == "全部")
            //      wf_30770_mth(workbook, "O%", SheetType.OptionSum, StartDate, EndDate, OswGrp);//sheetName = 選擇權月統計
            //}//if (chkGroup.Items[0].CheckState == System.Windows.Forms.CheckState.Checked) {
            #endregion

            //2.匯出資料(看勾選情況,目前最多跑2x2=4次)
            Object[] args = { dao30770, _ProgramID, StartMonth, EndMonth, StartDate, EndDate, OswGrp, OswGrpText };

            foreach (CheckedListBoxItem chk in chkGroup.Items) {
               if (chk.CheckState == CheckState.Checked) {
                  if (rdoGroup.Text == "%") {
                     //全部,兩個都要跑
                     IReportData Future = CreateReport(GetType(), "Future" + chk.Value.ToString(), args);
                     Future.Export(workbook);

                     IReportData Option = CreateReport(GetType(), "Option" + chk.Value.ToString(), args);
                     Option.Export(workbook);

                  } else {
                     IReportData ReportData = CreateReport(GetType(), rdoGroup.Text + chk.Value.ToString(), args);
                     ReportData.Export(workbook);
                  }//if (rdoGroup.Text == "%") {
               }//if(chk.CheckState == CheckState.Checked){
            }//foreach(CheckedListBoxItem chk in chkGroup.Items){


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

      /// <summary>
      /// ken,第一次改寫(廢除) 延長交易時間商品13:45後交易量比重－日明細
      /// </summary>
      /// <param name="ws"></param>
      /// <param name="prodType"></param>
      /// <param name="sheetType"></param>
      /// <param name="startDate"></param>
      /// <param name="endDate"></param>
      /// <param name="oswGrp"></param>
      private void wf_30770_date(Workbook workbook, string prodType, SheetType sheetType,
                                 string startDate, string endDate, string oswGrp) {

         Worksheet ws = workbook.Worksheets[(int)sheetType];

         DataTable dtTemp = dao30770.d_30770(prodType, "D", startDate, endDate, oswGrp);
         if (dtTemp.Rows.Count <= 0) {
            MessageDisplay.Info(string.Format("{0}～{1},{2}－{3}無任何資料!", startDate, endDate, _ProgramID, "延長交易時間商品13:45後交易量比重－日明細"));
            return;
         }

         //表頭
         int cp_max_seq_no = dtTemp.Rows[0]["cp_max_seq_no"].AsInt();

         int colStart1 = 0;
         int colStart2 = cp_max_seq_no + 1;
         int colStart3 = (cp_max_seq_no * 2) + 2;
         int rowIndex = 0;

         ws.Cells[rowIndex, colStart1 + 1].Value = oswGrp + "交易量";
         ws.Cells[rowIndex + 2, colStart2].Value = "小計";
         ws.Cells[rowIndex, colStart2 + 1].Value = "一般交易時段交易量";
         ws.Cells[rowIndex + 2, colStart3].Value = "小計";
         ws.Cells[rowIndex, colStart3 + 1].Value = "延長交易時段交易量比重";
         ws.Cells[rowIndex + 2, colStart3 + cp_max_seq_no + 1].Value = "小計";

         DataView dv = dtTemp.AsDataView();
         dv.Sort = "seq_no";

         rowIndex = 1;
         for (int k = 1;k <= cp_max_seq_no;k++) {
            int ll_found = dv.Find(k);
            string am11_kind_id = dtTemp.Rows[ll_found]["am11_kind_id"].AsString();
            string apdk_name = dtTemp.Rows[ll_found]["apdk_name"].AsString();

            ws.Cells[rowIndex + 0, k + colStart1].Value = am11_kind_id;
            ws.Cells[rowIndex + 1, k + colStart1].Value = apdk_name;
            ws.Cells[rowIndex + 0, k + colStart2].Value = am11_kind_id;
            ws.Cells[rowIndex + 1, k + colStart2].Value = apdk_name;
            ws.Cells[rowIndex + 0, k + colStart3].Value = am11_kind_id;
            ws.Cells[rowIndex + 1, k + colStart3].Value = apdk_name;

         }// for (int k = 0;k < cp_max_seq_no;k++) {


         rowIndex = 2;
         string ymd = "";
         string kindId = "";

         foreach (DataRow dr in dtTemp.Rows) {
            string am11_ymd = dr["am11_ymd"].AsString();
            DateTime.TryParseExact(am11_ymd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out DateTime am11);
            string am11_kind_id = dr["am11_kind_id"].AsString();

            if (ymd != am11_ymd) {
               ymd = am11_ymd;
               rowIndex++;
               ws.Cells[rowIndex, 0].Value = am11.ToString("yyyy/MM/dd");
            }//if (ymd != am11_ymd) {

            if (kindId != am11_kind_id) {
               kindId = am11_kind_id;

               Decimal cp_grp_m_qnty = dr["cp_grp_m_qnty"].AsDecimal();
               Decimal cp_grp_tot_qnty = dr["cp_grp_tot_qnty"].AsDecimal();
               ws.Cells[rowIndex, colStart2].Value = cp_grp_m_qnty;
               ws.Cells[rowIndex, colStart3].Value = cp_grp_tot_qnty;

               if (cp_grp_tot_qnty > 0) {
                  ws.Cells[rowIndex, colStart3 + cp_max_seq_no + 1].Value = Math.Round(cp_grp_m_qnty / cp_grp_tot_qnty, 4) * 100;
               } else {
                  ws.Cells[rowIndex, colStart3 + cp_max_seq_no + 1].Value = 0;
               }
            }//if (kindId != am11_kind_id) {

            int li_col = dr["seq_no"].AsInt();
            Decimal m_qnty = dr["m_qnty"].AsDecimal();
            Decimal tot_qnty = dr["tot_qnty"].AsDecimal();

            ws.Cells[rowIndex, li_col + colStart1].Value = m_qnty;
            ws.Cells[rowIndex, li_col + colStart2].Value = tot_qnty;
            if (tot_qnty > 0) {
               ws.Cells[rowIndex, li_col + colStart3].Value = Math.Round(m_qnty / tot_qnty, 4) * 100;
            } else {
               ws.Cells[rowIndex, li_col + colStart3].Value = 0;
            }

         }//foreach (DataRow dr in dtTemp.Rows) {

      }




      public interface IReportData {
         //DataTable GetData();

         /// <summary>
         /// 匯出資料
         /// </summary>
         /// <param name="workbook"></param>
         void Export(Workbook workbook);
      }

      /// <summary>
      /// 轉換介面 (注意,內部的class執行時其fullName是用+號連結起來)
      /// </summary>
      /// <param name="type"></param>
      /// <param name="name"></param>
      /// <param name="args">該物件初始化時候必要的參數</param>
      /// <returns></returns>
      public IReportData CreateReport(Type type, string name, Object[] args = null) {
         //讀取dll/主要exe的寫法
         string AssemblyName = type.Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = type.FullName + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
         return (IReportData)Assembly.Load(AssemblyName).CreateInstance(className, true, BindingFlags.CreateInstance, null, args, null, null);

         //另一種讀取其他exe的寫法
         //string AssemblyName = Assembly.GetExecutingAssembly().Location;//最後compile出來的exe
         //string className = type.FullName + "+" + name;//完整的class路徑(注意,內部的class執行時其fullName是用+號連結起來)
         //return (IReportData)Assembly.LoadFile(AssemblyName).CreateInstance(className, true, BindingFlags.CreateInstance, null, args, null, null);
      }

      public class Report30770 : IReportData {
         protected D30770 Dao { get; }
         protected string ReportName { get; set; }
         protected string ReportId { get; set; }

         protected string ProdType { get; set; }
         protected string SumType { get; set; }
         protected SheetType SheetIndex { get; set; }

         protected string StartDate { get; set; }
         protected string EndDate { get; set; }
         protected string OswGrp { get; set; }
         protected string OswGrpText { get; set; }

         public Report30770(D30770 dao, string reportId, string startMonth, string endMonth, string startDate, string endDate, string oswGrp, string oswGrpText) {
            Dao = dao;
            ReportId = reportId;

            OswGrp = oswGrp;
            OswGrpText = oswGrpText;
         }

         /// <summary>
         /// 匯出資料
         /// </summary>
         /// <param name="workbook"></param>
         public virtual void Export(Workbook workbook) {

            DataTable dtTemp = Dao.d_30770(ProdType, SumType, StartDate, EndDate, OswGrp);
            if (dtTemp.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0}～{1},{2}－{3}無任何資料!", StartDate, EndDate, ReportId, ReportName));
               return;
            }

            Worksheet ws = workbook.Worksheets[(int)SheetIndex];

            int rowIndex = 0;
            rowIndex = ExportGrid(ws, dtTemp, GridName.First, rowIndex, OswGrpText);

            //如果為月報,則多產一個grid
            if (SumType == "M")
               ExportGrid(ws, dtTemp, GridName.Second, ++rowIndex, OswGrpText);

         }


         protected virtual int ExportGrid(Worksheet ws, DataTable dtTemp, GridName gridName, int rowBeginIndex, string oswGrpText) {
            int rowIndex = rowBeginIndex;
            int cp_max_seq_no = dtTemp.Rows[0]["cp_max_seq_no"].AsInt();
            int colStart1 = 0;
            int colStart2 = cp_max_seq_no + 1;
            int colStart3 = (cp_max_seq_no * 2) + 2;

            //2.4月總量/日均量--第一排表頭
            if (SumType == "D") {
               //
            } else {
               if (gridName == GridName.First) {
                  ws.Cells[rowIndex, 0].Value = "月總量";
               } else {
                  ws.Cells[rowIndex, 0].Value = "日均量";
                  ws.Cells[rowIndex + 1, 0].Value = "代碼";
                  ws.Cells[rowIndex + 2, 0].Value = "名稱";
               }
            }
            ws.Cells[rowIndex, colStart1 + 1].Value = oswGrpText + "交易量";
            ws.Cells[rowIndex, colStart2 + 1].Value = "一般交易時段交易量";
            ws.Cells[rowIndex, colStart3 + 1].Value = "延長交易時段交易量比重";

            ws.Cells[rowIndex + 2, colStart2].Value = "小計";
            ws.Cells[rowIndex + 2, colStart3].Value = "小計";
            ws.Cells[rowIndex + 2, colStart3 + cp_max_seq_no + 1].Value = "小計";

            //2.5月總量/日均量--第二排表頭
            rowIndex++;

            //ken,又一個特殊處理,當sql的order by am11_ymd , am11_kind_id , seq_no,要使用find必須單純sort,所以要儘量把seq_no弄成唯一
            //DataTable dtHeader = dtTemp.Clone();
            //for (int y = 0;y < cp_max_seq_no;y++) {
            //   dtHeader.Rows.Add(dtTemp.Rows[y]);
            //}
            //DataView dv = dtHeader.AsDataView();
            //dv.Sort = "seq_no";

            for (int k = 1;k <= cp_max_seq_no;k++) {
               //int ll_found = dv.Find(k);
               string am11_kind_id = dtTemp.Rows[k-1]["am11_kind_id"].AsString();
               string apdk_name = dtTemp.Rows[k-1]["apdk_name"].AsString();

               ws.Cells[rowIndex + 0, k + colStart1].Value = am11_kind_id;
               ws.Cells[rowIndex + 1, k + colStart1].Value = apdk_name;
               ws.Cells[rowIndex + 0, k + colStart2].Value = am11_kind_id;
               ws.Cells[rowIndex + 1, k + colStart2].Value = apdk_name;
               ws.Cells[rowIndex + 0, k + colStart3].Value = am11_kind_id;
               ws.Cells[rowIndex + 1, k + colStart3].Value = apdk_name;

            }// for (int k = 0;k < cp_max_seq_no;k++) {

            //2.6月總量/日均量--資料
            rowIndex++;
            string ymd = "";
            string kindId = "";

            foreach (DataRow dr in dtTemp.Rows) {
               string am11_ymd = dr["am11_ymd"].AsString();
               DateTime.TryParseExact(am11_ymd, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out DateTime am11);
               string am11_kind_id = dr["am11_kind_id"].AsString();
               int day_cnt = dr["day_cnt"].AsInt();
               int dec = (gridName == GridName.First ? 4 : 6);//小數點4位或是6位

               if (ymd != am11_ymd) {
                  //每換一個日期
                  ymd = am11_ymd;
                  rowIndex++;
                  ws.Cells[rowIndex, 0].Value = (SumType == "D" ? am11.ToString("yyyy/MM/dd") : am11_ymd);
               }//if (ymd != am11_ymd) {

               if (kindId != am11_kind_id) {
                  //每換一個契約
                  kindId = am11_kind_id;

                  Decimal cp_grp_m_qnty = dr["cp_grp_m_qnty"].AsDecimal();
                  Decimal cp_grp_tot_qnty = dr["cp_grp_tot_qnty"].AsDecimal();
                  Decimal groupCount = (gridName == GridName.First ? cp_grp_m_qnty : Math.Round(cp_grp_m_qnty / day_cnt, 4));
                  Decimal groupTotalCount = (gridName == GridName.First ? cp_grp_tot_qnty : Math.Round(cp_grp_tot_qnty / day_cnt, 4));

                  ws.Cells[rowIndex, colStart2].Value = groupCount;
                  ws.Cells[rowIndex, colStart3].Value = groupTotalCount;

                  if (groupTotalCount > 0) {
                     ws.Cells[rowIndex, colStart3 + cp_max_seq_no + 1].Value = Math.Round(groupCount / groupTotalCount, dec) * 100;
                  } else {
                     ws.Cells[rowIndex, colStart3 + cp_max_seq_no + 1].Value = 0;
                  }

               }//if (kindId != am11_kind_id) {

               int li_col = dr["seq_no"].AsInt();
               Decimal m_qnty = dr["m_qnty"].AsDecimal();
               Decimal tot_qnty = dr["tot_qnty"].AsDecimal();
               Decimal count = (gridName == GridName.First ? m_qnty : Math.Round(m_qnty / day_cnt, 4));
               Decimal totalCount = (gridName == GridName.First ? tot_qnty : Math.Round(tot_qnty / day_cnt, 4));

               ws.Cells[rowIndex, li_col + colStart1].Value = count;
               ws.Cells[rowIndex, li_col + colStart2].Value = totalCount;
               if (totalCount > 0) {
                  ws.Cells[rowIndex, li_col + colStart3].Value = Math.Round(count / totalCount, dec) * 100;
               } else {
                  ws.Cells[rowIndex, li_col + colStart3].Value = 0;
               }

            }//foreach (DataRow dr in dtTemp.Rows) {

            return rowIndex;
         }//protected virtual void ExportGrid(Worksheet ws, DataTable dtTemp, GridName gridName,int rowIndex,string oswGrp) {

      }



      public class FutureDay : Report30770 {

         public FutureDay(D30770 dao, string reportId, string startMonth, string endMonth, string startDate, string endDate, string oswGrp, string oswGrpText)
                     : base(dao, reportId, startMonth, endMonth, startDate, endDate, oswGrp, oswGrpText) {
            //初始化設定,先讓父類別把共用參數設定好,再設定自己特定的參數
            ReportName = "延長交易時間商品13:45後交易量比重－日明細";

            ProdType = "F%";
            SumType = "D";
            SheetIndex = SheetType.FutureDetail;

            StartDate = startDate;
            EndDate = endDate;
         }

         public override void Export(Workbook workbook) {
            base.Export(workbook);
         }
      }

      public class FutureMonth : Report30770 {

         public FutureMonth(D30770 dao, string reportId, string startMonth, string endMonth, string startDate, string endDate, string oswGrp, string oswGrpText)
                     : base(dao, reportId, startMonth, endMonth, startDate, endDate, oswGrp, oswGrpText) {
            //初始化設定,先讓父類別把共用參數設定好,再設定自己特定的參數
            ReportName = "延長交易時間商品13:45後交易量比重－月統計";

            ProdType = "F%";
            SumType = "M";
            SheetIndex = SheetType.FutureSum;

            StartDate = startMonth;
            EndDate = endMonth;
         }

         public override void Export(Workbook workbook) {
            base.Export(workbook);
         }
      }

      public class OptionDay : Report30770 {

         public OptionDay(D30770 dao, string reportId, string startMonth, string endMonth, string startDate, string endDate, string oswGrp, string oswGrpText)
                     : base(dao, reportId, startMonth, endMonth, startDate, endDate, oswGrp, oswGrpText) {
            //初始化設定,先讓父類別把共用參數設定好,再設定自己特定的參數
            ReportName = "延長交易時間商品13:45後交易量比重－日明細";

            ProdType = "O%";
            SumType = "D";
            SheetIndex = SheetType.OptionDetail;

            StartDate = startDate;
            EndDate = endDate;
         }

         public override void Export(Workbook workbook) {
            base.Export(workbook);
         }
      }

      public class OptionMonth : Report30770 {

         public OptionMonth(D30770 dao, string reportId, string startMonth, string endMonth, string startDate, string endDate, string oswGrp, string oswGrpText)
                     : base(dao, reportId, startMonth, endMonth, startDate, endDate, oswGrp, oswGrpText) {
            //初始化設定,先讓父類別把共用參數設定好,再設定自己特定的參數
            ReportName = "延長交易時間商品13:45後交易量比重－月統計";

            ProdType = "O%";
            SumType = "M";
            SheetIndex = SheetType.OptionSum;

            StartDate = startMonth;
            EndDate = endMonth;
         }

         public override void Export(Workbook workbook) {
            base.Export(workbook);
         }
      }

   }
}