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
using BaseGround.Report;
using BusinessObjects;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.Utils.Drawing;
using DataObjects.Dao.Together.TableDao;

namespace PhoenixCI.FormUI.Prefix5 {
   // winni, 2019/01/07 造市商品單邊回應詢價價格限制設定  
   public partial class W51050 : FormParent {

      private ReportHelper _ReportHelper;
      private COD daoCOD;
      private D51050 dao51050;
      private RepositoryItemLookUpEdit _RepLookUpEdit;

      public W51050(string programID , string programName) : base(programID , programName) {

         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         dao51050 = new D51050();

         daoCOD = new COD();
         _RepLookUpEdit = new RepositoryItemLookUpEdit();
         _RepLookUpEdit.DataSource = daoCOD.ListByCol2("MMFT" , "MMFT_MARKET_CODE"); //一般、夜盤
         _RepLookUpEdit.ValueMember = "COD_ID";
         _RepLookUpEdit.DisplayMember = "COD_DESC";
         _RepLookUpEdit.ShowHeader = false;
         _RepLookUpEdit.ShowFooter = false;
         _RepLookUpEdit.NullText = "";
         _RepLookUpEdit.SearchMode = SearchMode.AutoFilter;
         _RepLookUpEdit.TextEditStyle = TextEditStyles.Standard;
         //讓下拉選單只剩單一欄位
         LookUpColumnInfoCollection singleCol2 = _RepLookUpEdit.Columns;
         singleCol2.Add(new LookUpColumnInfo("CP_DISPLAY"));
         gcMain.RepositoryItems.Add(_RepLookUpEdit);
         MMFO_MARKET_CODE.ColumnEdit = _RepLookUpEdit;
         _RepLookUpEdit.BestFitMode = BestFitMode.BestFitResizePopup;

      }

      protected override ResultStatus Retrieve() {
         base.Retrieve(gcMain);
         DataTable returnTable = new DataTable();

         returnTable = dao51050.ListAll();
         if (returnTable.Rows.Count == 0) {
            MessageBox.Show("無任何資料" , "訊息" , MessageBoxButtons.OK , MessageBoxIcon.Information);
         }
         returnTable.Columns.Add("Is_NewRow" , typeof(string));//新增Is_NewRow的欄位
         gcMain.DataSource = returnTable;
         gcMain.Visible = true;
         gcMain.Focus();

         return ResultStatus.Success;
      }

      /// <summary>
      /// 新增一行 
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus InsertRow() {
         base.InsertRow(gvMain);
         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      //設定只有新增列可以編輯，原有資料不能編輯
      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
                 gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false; //新增行可編輯
            gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"] , 1);
            object a = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]);
         } else if (gv.FocusedColumn.Name == "MMFO_MIN_PRICE") {
            e.Cancel = false; //委託價格限制可編輯
         } else {
            e.Cancel = true; //既有資料不可編輯
         }
      }

      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
              gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]).ToString();

         if (e.Column.FieldName != "MMFO_MIN_PRICE") {
            e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.Silver;
         }
      }

      // 新增一行
      private void gvMain_InitNewRow(object sender , InitNewRowEventArgs e) {
         GridView gv = sender as GridView;
         gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"] , 1);
      }

      protected override ResultStatus Save(PokeBall poke) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         DataTable dt = (DataTable)gcMain.DataSource;

         DataTable dtChange = dt.GetChanges();

         if (dtChange.Rows.Count == 0) {
            MessageBox.Show("沒有變更資料,不需要存檔!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
         } else {
            ResultStatus status = base.Save_Override(dt , "MMFO");
            if (status == ResultStatus.Fail) {
               return ResultStatus.Fail;
            }
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         _ReportHelper = reportHelper;
         CommonReportPortraitA4 report = new CommonReportPortraitA4();
         report.printableComponentContainerMain.PrintableComponent = gcMain;
         _ReportHelper.Create(report);

         base.Print(_ReportHelper);
         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = true;
         _ToolBtnSave.Enabled = true;
         _ToolBtnDel.Enabled = true;
         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;

         return ResultStatus.Success;
      }

      public override ResultStatus BeforeOpen() {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open() {
         base.Open();

         //直接讀取資料
         Retrieve();

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         base.AfterOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose() {
         return base.BeforeClose();
      }

      protected override ResultStatus COMPLETE() {
         MessageDisplay.Info(MessageDisplay.MSG_OK);
         Retrieve();
         return ResultStatus.Success;
      }

   }
}