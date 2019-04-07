using BaseGround;
using BaseGround.Report;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;
using System.Reflection;

/// <summary>
/// Winni, 2019/02/18 交易人帳號狀態查詢
/// </summary>
namespace PhoenixCI.FormUI.PrefixP {
   public partial class WP0040 : FormParent {

      public WP0040(string programID , string programName) : base(programID , programName) {
         try {
            InitializeComponent();
            GridHelper.SetCommonGrid(gvMain);
            this.Text = _ProgramID + "─" + _ProgramName;
         } catch (Exception ex) {
            MessageDisplay.Error(ex.ToString());
         }
      }

      protected override ResultStatus Open() {
         base.Open();
         return ResultStatus.Success;
      }

      /// <summary>
      /// 設定此功能哪些按鈕可以按
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = false;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = false;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = false;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = true;//列印

         return ResultStatus.Success;
      }

      /// <summary>
      /// 按下[讀取/預覽]按鈕時,去資料庫撈資料
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Retrieve() {
         base.Retrieve();
         try {
            DP00xx daoP00xx = new DP00xx();
            DataTable dt = new DataTable();
            DataTable dtTXFP = new DataTable();
            dtTXFP = new TXFP().ListDataByKey("POS");

            IGridDataP00xx gridData = daoP00xx.CreateGridData(daoP00xx.GetType(), GetType(), MethodBase.GetCurrentMethod().Name);
            QP00xx qP00xx = new QP00xx(txtFcmNo.Text, txtAccNo.Text, dtTXFP);
            dt = gridData.GetData(qP00xx);
           // dt = new DP0040().SP_QUERY_USER_STATUS(txtFcmNo.Text , txtAccNo.Text , "POS" , dtTXFP);

            //將datatable的Title換掉
            dt.Columns[0].ColumnName = "FCM_NAME";
            dt.Columns[1].ColumnName = "FCM_NO";
            dt.Columns[2].ColumnName = "SEQ_ACC_NO";
            dt.Columns[3].ColumnName = "SYS_TYPE";
            dt.Columns[4].ColumnName = "W_STATUS";
            dt.Columns[5].ColumnName = "APPLY_DATE";
            dt.Columns[6].ColumnName = "LOCK_CNT";

            //dddw_pos_sys_id
            for (int i = 0 ; i < dt.Rows.Count ; i++) {
               if (dt.Rows[i][3].AsString() == "W") {
                  dt.Rows[i][3] = "網際網路";
               } else if (dt.Rows[i][3].AsString() == "V") {
                  dt.Rows[i][3] = "電話語音";
               }
            }

            gcMain.DataSource = null;
            gvMain.GroupSummary.Clear();
            gvMain.Columns.Clear();//清除grid
            gcMain.DataSource = dt;

            gcMain.Visible = true;
            gcMain.Focus();

            //David 將第一筆以外的三個欄位都設為空值(仿PB產出結果)
            for (int i = 1 ; i <= gvMain.RowCount ; i++) {
               gvMain.SetRowCellValue(i , "FCM_NAME" , "");
               gvMain.SetRowCellValue(i , "FCM_NO" , "");
               gvMain.SetRowCellValue(i , "SEQ_ACC_NO" , "");
            }

            //可測資料
            //S653010 0014735 (W V)
            //F002000 0875493
            //F002000 1003688
            //F002000 1121612
            //F002000 9101809
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }

         return ResultStatus.Success;
      }

      /// <summary>
      /// 列印功能
      /// </summary>
      /// <param name="reportHelper"></param>
      /// <returns></returns>
      protected override ResultStatus Print(ReportHelper reportHelper) {

         //gcMain.Print();
         CommonReportLandscapeA4 rep = new CommonReportLandscapeA4();
         //如沒有查詢則不印出gvMain
         if (gvMain.RowCount != 0) {
            rep.printableComponentContainerMain.PrintableComponent = gcMain;
         }

         reportHelper.Create(rep);
         base.Print(reportHelper);

         return ResultStatus.Success;
      }

   }
}