using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Lukas, 2019/3/13
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {

   /// <summary>
   /// 30203 股價指數類暨黃金類部位限制數確認
   /// </summary>
   public partial class W30203 : FormParent {

      private D30203 dao30203;
      private RepositoryItemLookUpEdit statusLookUpEdit;
      //private Dictionary<string, string> dic;
      private ReportHelper _ReportHelper;

      public W30203(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         GridHelper.SetCommonGrid(gvMain);
         GridHelper.SetCommonGrid(gvGBF);
         //PrintableComponent = gcMain;
         gvMain.OptionsView.ShowColumnHeaders = false;
         gvGBF.OptionsView.ShowColumnHeaders = false;
      }

      protected override ResultStatus Open() {
         try {
            base.Open();
            txtDate.DateTimeValue = GlobalInfo.OCF_DATE;
            txtEffDate.EditValue = "1901/01/01";
            txtEffDateLower.EditValue = "1901/01/01";
#if DEBUG
            txtDate.EditValue = "2018/12/28";
#endif
            //「本次擬調整狀態」欄位的下拉選單
            //dic = new Dictionary<string, string>() { { " ", "不變" }, { "+", "調高" }, { "-", "降低" } };
            //DataTable dtStatus = setColItem(dic);
            DataTable dtStatus = new CODW().ListLookUpEdit("30203" , "PL1B_ADJ");
            foreach (DataRow dr in dtStatus.Rows) {
               if (dr["CODW_ID"].AsString() == "S") {
                  dr["CODW_ID"] = " ";
               }
            }
            statusLookUpEdit = new RepositoryItemLookUpEdit();
            statusLookUpEdit.SetColumnLookUp(dtStatus , "CODW_ID" , "CODW_DESC");
            PL1B_ADJ.ColumnEdit = statusLookUpEdit;
            PL1_NATURE_ADJ.ColumnEdit = statusLookUpEdit;
            PL1_LEGAL_ADJ.ColumnEdit = statusLookUpEdit;

            #region BandedColumnCaption換行
            //gvGBF
            PL1B_PREV_NATURE_LEGAL_MTH.Caption = "單一" + Environment.NewLine + "月份";
            PL1B_PREV_NATURE_LEGAL_TOT.Caption = "各月份" + Environment.NewLine + "合計";
            PL1B_PREV_999_MTH.Caption = "單一" + Environment.NewLine + "月份";
            PL1B_PREV_999_NEARBY_MTH.Caption = "最近到期" + Environment.NewLine + "月份";
            PL1B_PREV_999_TOT.Caption = "各月份" + Environment.NewLine + "合計";

            PL1B_NATURE_LEGAL_MTH.Caption = "單一" + Environment.NewLine + "月份";
            PL1B_NATURE_LEGAL_TOT.Caption = "各月份" + Environment.NewLine + "年月";
            PL1B_999_MTH.Caption = "單一" + Environment.NewLine + "月份";
            PL1B_999_NEARBY_MTH.Caption = "單一" + Environment.NewLine + "月份";
            PL1B_999_TOT.Caption = "最近到期" + Environment.NewLine + "合計";

            PL1B_ADJ.Caption = "本次" + Environment.NewLine + "擬調整" + Environment.NewLine + "狀態";

            //gvMain
            bandCur.Caption = "現行" + Environment.NewLine + "部位限制數";
            bandCP.Caption = "按交易規則檢視後" + Environment.NewLine + "之部位限制數";
            bandCompute1.Caption = "近1~6月" + Environment.NewLine + "日均交易量與未沖銷量" + Environment.NewLine + "數值取大者";
            bandPL1_NATURE_LEGAL.Caption = "針對須調降之商品" + Environment.NewLine + "再增加檢視標準後";
            bandPL1B_ADJ.Caption = Environment.NewLine + "本次擬調整狀態";
            #endregion

            gcMain.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            gcMain.LookAndFeel.UseDefaultLookAndFeel = false;
            gvMain.OptionsSelection.EnableAppearanceFocusedRow = false;
            gvMain.OptionsSelection.EnableAppearanceFocusedCell = false;
            gcMain.Visible = false;
            gcGBF.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            gcGBF.LookAndFeel.UseDefaultLookAndFeel = false;
            gvGBF.OptionsSelection.EnableAppearanceFocusedRow = false;
            gvGBF.OptionsSelection.EnableAppearanceFocusedCell = false;
         } catch (Exception ex) {
            throw ex;
         }
         return ResultStatus.Success;
      }

      /// <summary>
      /// 自訂下拉式選項
      /// </summary>
      /// <param name="dic">陣列</param>
      /// <returns></returns>
      private DataTable setColItem(Dictionary<string , string> dic) {
         DataTable dt = new DataTable();
         dt.Columns.Add("ID");
         dt.Columns.Add("Desc");
         foreach (var str in dic) {
            DataRow rows = dt.NewRow();
            rows["ID"] = str.Key;
            rows["Desc"] = str.Value;
            dt.Rows.Add(rows);
         }
         return dt;
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnInsert.Enabled = true;//當按下此按鈕時,Grid新增一筆空的(還未存檔都是暫時的)
         _ToolBtnSave.Enabled = true;//儲存(把新增/刪除/修改)多筆的結果一次更新到資料庫
         _ToolBtnDel.Enabled = true;//先選定刪除grid上面的其中一筆,然後按下此刪除按鈕(還未存檔都是暫時的)

         _ToolBtnRetrieve.Enabled = true;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = false;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = true;//列印

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {

         try {
            dao30203 = new D30203();
            gcMain.Visible = false;
            string lsYmd;
            int found;
            lsYmd = txtDate.Text.Replace("/" , "");

            DataTable dt30203 = dao30203.d_30203(lsYmd);
            if (dt30203.Rows.Count == 0) {
               MessageDisplay.Info("PL1無任何資料!");
            } else {
               dt30203.Columns.Add("Is_NewRow" , typeof(string));
               gcMain.DataSource = dt30203;
               gcMain.Visible = true;
               gcMain.Focus();
            }

            DataTable dt30203gbf = dao30203.d_30203_gbf();
            if (dt30203gbf.Rows.Count == 0) {
               MessageDisplay.Info("PL1B無任何資料!");
               return ResultStatus.Fail;
            }
            gcGBF.DataSource = dt30203gbf;

            //公告日期
            DataTable dtPostDate = dao30203.PostDate(lsYmd);
            if (dtPostDate.Rows.Count == 0) {
               MessageDisplay.Info("公告日期無任何資料!");
               return ResultStatus.Fail;
            }
            if (dtPostDate.Rows[0]["RAISE_YMD"].AsDateTime("yyyyMMdd") != default(DateTime)) {
               txtEffDate.DateTimeValue = dtPostDate.Rows[0]["RAISE_YMD"].AsDateTime("yyyyMMdd");
               txtEffDateLower.DateTimeValue = dtPostDate.Rows[0]["LOWER_YMD"].AsDateTime("yyyyMMdd");
               lblEff.Text = "（已確認）";
            } else {
               lblEff.Text = "";
            }

            DataTable dt30203PL2 = dao30203.d_30203_pl2(lsYmd);
            if (dt30203PL2.Rows.Count == 0) {
               MessageDisplay.Info("PL2無任何資料!");
               return ResultStatus.Fail;
            }

            if (dtPostDate.Rows[0]["LI_COUNT"].AsInt() <= 0) return ResultStatus.Fail;
            DialogResult result = MessageDisplay.Choose("已確認資料，按「是」讀取已存檔資料，按「否」為重新產製資料");
            if (result == DialogResult.No) return ResultStatus.Fail;

            foreach (DataRow dr in dt30203PL2.Rows) {
               //此時gridview的資料還沒被動過，原本要在gridview中查找(datawindow.find)的資料直接在datasource查找即可
               DataRow[] find = dt30203.Select("PL1_KIND_ID='" + dr["PL2_KIND_ID"].ToString() + "'");
               if (find.Length > 0) {
                  found = dt30203.Rows.IndexOf(find[0]);
               } else {
                  found = -1;
               }
               if (found == -1) {
                  InsertRow();
                  found = gvMain.RowCount;
               }

               if (dr["PL2_EFFECTIVE_YMD"].AsString() == dtPostDate.Rows[0]["LOWER_YMD"].AsString()) {
                  gvMain.SetRowCellValue(found , "PL1_NATURE_ADJ" , "-");
               }
               gvMain.SetRowCellValue(found , "PL1_YMD" , dr["PL2_YMD"].AsString());
               gvMain.SetRowCellValue(found , "PL1_KIND_ID" , dr["PL2_KIND_ID"].ToString());
               gvMain.SetRowCellValue(found , "PL1_NATURE" , dr["PL2_NATURE"]);
               gvMain.SetRowCellValue(found , "PL1_LEGAL" , dr["PL2_LEGAL"]);
               gvMain.SetRowCellValue(found , "PL1_999" , dr["PL2_999"]);
               gvMain.SetRowCellValue(found , "PL1_NATURE_ADJ" , dr["PL2_NATURE_ADJ"].ToString());
               gvMain.SetRowCellValue(found , "PL1_LEGAL_ADJ" , dr["PL2_LEGAL_ADJ"].ToString());
               gvMain.SetRowCellValue(found , "PL1_999_ADJ" , dr["PL2_999_ADJ"].ToString());
               gvMain.SetRowCellValue(found , "PL1_CUR_NATURE" , dr["PL2_PREV_NATURE"]);
               gvMain.SetRowCellValue(found , "PL1_CUR_LEGAL" , dr["PL2_PREV_LEGAL"]);
               gvMain.SetRowCellValue(found , "PL1_CUR_999" , dr["PL2_PREV_999"]);
            }

            DataTable dt30203PL2B = dao30203.d_30203_pl2b(lsYmd);
            if (dt30203PL2B.Rows.Count == 0) {
               MessageDisplay.Info("PL2B無任何資料!");
               return ResultStatus.Fail;
            }
            foreach (DataRow dr in dt30203PL2B.Rows) {
               //此時gridview的資料還沒被動過，原本要在gridview中查找(datawindow.find)的資料直接在datasource查找即可
               DataRow[] find = dt30203gbf.Select("PL1B_KIND_ID='" + dr["PL2B_KIND_ID"].ToString() + "'");
               if (find.Length > 0) {
                  found = dt30203gbf.Rows.IndexOf(find[0]);
                  gvGBF.SetRowCellValue(found , "PL1B_PROD_TYPE" , dr["PL2B_PROD_TYPE"].AsString());
                  gvGBF.SetRowCellValue(found , "PL1B_PROD_SUBTYPE" , dr["PL2B_PROD_SUBTYPE"].AsString());
                  gvGBF.SetRowCellValue(found , "PL1B_KIND_ID" , dr["PL2B_KIND_ID"].ToString());
                  gvGBF.SetRowCellValue(found , "PL1B_NATURE_LEGAL_MTH" , dr["PL2B_NATURE_LEGAL_MTH"].AsInt());
                  gvGBF.SetRowCellValue(found , "PL1B_NATURE_LEGAL_TOT" , dr["PL2B_NATURE_LEGAL_TOT"].AsInt());
                  gvGBF.SetRowCellValue(found , "PL1B_999_MTH" , dr["PL2B_999_MTH"].AsInt());
                  gvGBF.SetRowCellValue(found , "PL1B_999_NEARBY_MTH" , dr["PL2B_999_NEARBY_MTH"].AsInt());
                  gvGBF.SetRowCellValue(found , "PL1B_999_TOT" , dr["PL2B_999_TOT"].AsInt());
                  gvGBF.SetRowCellValue(found , "PL1B_PREV_NATURE_LEGAL_MTH" , dr["PL2B_PREV_NATURE_LEGAL_MTH"].AsInt());
                  gvGBF.SetRowCellValue(found , "PL1B_PREV_NATURE_LEGAL_TOT" , dr["PL2B_PREV_NATURE_LEGAL_TOT"].AsInt());
                  gvGBF.SetRowCellValue(found , "PL1B_PREV_999_MTH" , dr["PL2B_PREV_999_MTH"].AsInt());
                  gvGBF.SetRowCellValue(found , "PL1B_PREV_999_NEARBY_MTH" , dr["PL2B_PREV_999_NEARBY_MTH"].AsInt());
                  gvGBF.SetRowCellValue(found , "PL1B_PREV_999_TOT" , dr["PL2B_PREV_999_TOT"].AsInt());
                  gvGBF.SetRowCellValue(found , "PL1B_ADJ" , dr["PL2B_ADJ"].ToString());
               }
            }

         } catch (Exception ex) {
            WriteLog(ex);
         }

         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow() {
         try {
            base.InsertRow(gvMain);
            gvMain.Focus();
            gvMain.FocusedColumn = gvMain.Columns[0];
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         try {
            base.DeleteRow(gvMain);
         } catch (Exception ex) {
            MessageDisplay.Error("刪除資料列錯誤");
            WriteLog(ex);
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {

         //PB不管資料有無異動都會存檔

         #region ue_save_before
         if (gvMain.RowCount == 0) {
            MessageDisplay.Error("下方視窗無資料無法進行存檔，請先執行「讀取／預覽」!");
            return ResultStatus.Fail;
         }

         //0. 先結束編輯
         gvGBF.CloseEditor();
         gvMain.CloseEditor();

         string showMsg = "";
         //1. 寫LOG到ci.PLLOG
         try {
            showMsg = "異動紀錄(PLLOG)更新資料庫錯誤! ";
            DataTable dtPLLOG = dao30203.d_30203_pllog();
            int i;
            string ls_prod_type, ls_prod_subtype, ls_kind_id;
            for (i = 0 ; i < gvMain.RowCount ; i++) {
               if (gvMain.GetRowCellValue(i , "Is_NewRow").AsString() == "1") {
                  ls_kind_id = gvMain.GetRowCellValue(i , "PL1_KIND_ID").AsString();
                  DataTable dtProdType = dao30203.ProdType(ls_kind_id);
                  if (dtProdType.Rows.Count == 0) {
                     MessageDisplay.Error("商品 " + ls_kind_id + " 無商品基本資料，無法新增!");
                     return ResultStatus.Fail;
                  }
                  ls_prod_type = dtProdType.Rows[0]["PROD_TYPE"].AsString();
                  ls_prod_subtype = dtProdType.Rows[0]["PROD_SUBTYPE"].AsString();

                  gvMain.SetRowCellValue(i , "PL1_YMD" , txtDate.Text.Replace("/" , "").AsString());
                  gvMain.SetRowCellValue(i , "PL1_PROD_TYPE" , ls_prod_type);
                  gvMain.SetRowCellValue(i , "PL1_PROD_SUBTYPE" , ls_prod_subtype);
                  gvMain.SetRowCellValue(i , "PL1_999" , gvMain.GetRowCellValue(i , "PL1_LEGAL").AsDecimal() * 3);
                  gvMain.SetRowCellValue(i , "PL1_CUR_NATURE" , 0);
                  gvMain.SetRowCellValue(i , "PL1_CUR_LEGAL" , 0);
                  gvMain.SetRowCellValue(i , "PL1_CUR_999" , gvMain.GetRowCellValue(i , "PL1_CUR_LEGAL").AsDecimal() * 3);
                  gvMain.SetRowCellValue(i , "PL1_PREV_AVG_QNTY" , 0);
                  gvMain.SetRowCellValue(i , "PL1_PREV_AVG_OI" , 0);
                  gvMain.SetRowCellValue(i , "PL1_AVG_QNTY" , 0);
                  gvMain.SetRowCellValue(i , "PL1_AVG_OI" , 0);
                  gvMain.SetRowCellValue(i , "PL1_CHANGE_RANGE" , 0);
                  gvMain.SetRowCellValue(i , "PL1_CP_999" , gvMain.GetRowCellValue(i , "PL1_CP_LEGAL").AsDecimal() * 3);
                  gvMain.SetRowCellValue(i , "PL1_999_ADJ" , "+");
                  gvMain.SetRowCellValue(i , "PL1_UPD_TIME" , DateTime.Now);
                  gvMain.SetRowCellValue(i , "PL1_UPD_USER_ID" , GlobalInfo.USER_ID);
               }
               if (gvMain.GetRowCellValue(i , "PL1_NATURE").AsDecimal() == gvMain.GetRowCellValue(i , "PL1_NATURE_ORG").AsDecimal() &&
                   gvMain.GetRowCellValue(i , "PL1_LEGAL").AsDecimal() == gvMain.GetRowCellValue(i , "PL1_LEGAL_ORG").AsDecimal()) continue;
               if (gvMain.GetRowCellValue(i , "PL1_NATURE").AsDecimal() != gvMain.GetRowCellValue(i , "PL1_NATURE_ORG").AsDecimal()) {
                  dtPLLOG.Rows.Add();
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_YMD"] = gvMain.GetRowCellValue(i , "PL1_YMD").AsString();
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_KIND_ID"] = gvMain.GetRowCellValue(i , "PL1_KIND_ID").AsString();
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_DATA_TYPE"] = "N";
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_ORG_VALUE"] = gvMain.GetRowCellValue(i , "PL1_NATURE_ORG").AsDecimal().AsString();
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_UPD_VALUE"] = gvMain.GetRowCellValue(i , "PL1_NATURE").AsDecimal().AsString();
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_W_TIME"] = DateTime.Now;
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_W_USER_ID"] = GlobalInfo.USER_ID;
               }
               if (gvMain.GetRowCellValue(i , "PL1_LEGAL").AsDecimal() != gvMain.GetRowCellValue(i , "PL1_LEGAL_ORG").AsDecimal()) {
                  dtPLLOG.Rows.Add();
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_YMD"] = gvMain.GetRowCellValue(i , "PL1_YMD").AsString();
                  //dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_YMD"] = txtDate.Text.Replace("/", "").AsString();
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_KIND_ID"] = gvMain.GetRowCellValue(i , "PL1_KIND_ID").AsString();
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_DATA_TYPE"] = "E";
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_ORG_VALUE"] = gvMain.GetRowCellValue(i , "PL1_LEGAL_ORG").AsDecimal().AsString();
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_UPD_VALUE"] = gvMain.GetRowCellValue(i , "PL1_LEGAL").AsDecimal().AsString();
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_W_TIME"] = DateTime.Now;
                  dtPLLOG.Rows[dtPLLOG.Rows.Count - 1]["PLLOG_W_USER_ID"] = GlobalInfo.USER_ID;

                  gvMain.SetRowCellValue(i , "PL1_999" , gvMain.GetRowCellValue(i , "PL1_LEGAL").AsDecimal() * 3);
               }
            }
            // 寫入DB
            int count = dtPLLOG.Rows.Count;
            ResultData myResultData = dao30203.updatePLLOG(dtPLLOG);
            #endregion

            string ls_ymd, ls_eff_ymd, ls_eff_ymd_lower;
            bool delResult = false;
            ls_ymd = txtDate.Text.Replace("/" , "");
            ls_eff_ymd = txtEffDate.Text.Replace("/" , "");
            ls_eff_ymd_lower = txtEffDateLower.Text.Replace("/" , "");
            //2. 刪除資料 PL2,PL2B
            showMsg = "PL2刪除失敗";
            delResult = dao30203.DeletePL2ByDate(ls_ymd);
            if (!delResult) {
               MessageDisplay.Error(showMsg);
               return ResultStatus.Fail;
            }
            showMsg = "PL2B刪除失敗";
            delResult = dao30203.DeletePL2BByDate(ls_ymd);
            if (!delResult) {
               MessageDisplay.Error(showMsg);
               return ResultStatus.Fail;
            }

            //3. 新增 PL2
            showMsg = "確認資料(PL2)更新資料庫錯誤! ";
            DataTable dtInsertPL2 = dao30203.d_30203_pl2(ls_ymd);
            for (i = 0 ; i < gvMain.RowCount ; i++) {
               dtInsertPL2.Rows.Add();
               if (gvMain.GetRowCellValue(i , "PL1_NATURE_ADJ").AsString() == "-") {
                  dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_EFFECTIVE_YMD"] = ls_eff_ymd_lower;
               } else {
                  dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_EFFECTIVE_YMD"] = ls_eff_ymd;
               }
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_YMD"] = gvMain.GetRowCellValue(i , "PL1_YMD").AsString();
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_KIND_ID"] = gvMain.GetRowCellValue(i , "PL1_KIND_ID").ToString();
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_NATURE"] = gvMain.GetRowCellValue(i , "PL1_NATURE").AsInt();
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_LEGAL"] = gvMain.GetRowCellValue(i , "PL1_LEGAL").AsInt();
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_999"] = gvMain.GetRowCellValue(i , "PL1_999").AsInt();
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_NATURE_ADJ"] = gvMain.GetRowCellValue(i , "PL1_NATURE_ADJ").ToString();
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_LEGAL_ADJ"] = gvMain.GetRowCellValue(i , "PL1_LEGAL_ADJ").ToString();
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_999_ADJ"] = gvMain.GetRowCellValue(i , "PL1_999_ADJ").ToString();
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_PREV_NATURE"] = gvMain.GetRowCellValue(i , "PL1_CUR_NATURE").AsInt();
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_PREV_LEGAL"] = gvMain.GetRowCellValue(i , "PL1_CUR_LEGAL").AsInt();
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_PREV_999"] = gvMain.GetRowCellValue(i , "PL1_CUR_999").AsInt();
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_W_TIME"] = DateTime.Now;
               dtInsertPL2.Rows[dtInsertPL2.Rows.Count - 1]["PL2_W_USER_ID"] = GlobalInfo.USER_ID;
            }
            // 寫入DB
            myResultData = dao30203.updatePL2(dtInsertPL2);

            //4. 新增 PL2B
            showMsg = "確認資料(PL2B)更新資料庫錯誤! ";
            DataTable dtInsertPL2B = dao30203.d_30203_pl2b(ls_ymd);
            for (i = 0 ; i < gvGBF.RowCount ; i++) {
               dtInsertPL2B.Rows.Add();
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_EFFECTIVE_YMD"] = ls_eff_ymd;
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_YMD"] = ls_ymd;
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_PROD_TYPE"] = gvGBF.GetRowCellValue(i , "PL1B_PROD_TYPE");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_PROD_SUBTYPE"] = gvGBF.GetRowCellValue(i , "PL1B_PROD_SUBTYPE");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_KIND_ID"] = gvGBF.GetRowCellValue(i , "PL1B_KIND_ID");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_NATURE_LEGAL_MTH"] = gvGBF.GetRowCellValue(i , "PL1B_NATURE_LEGAL_MTH");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_NATURE_LEGAL_TOT"] = gvGBF.GetRowCellValue(i , "PL1B_NATURE_LEGAL_TOT");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_999_MTH"] = gvGBF.GetRowCellValue(i , "PL1B_999_MTH");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_999_NEARBY_MTH"] = gvGBF.GetRowCellValue(i , "PL1B_999_NEARBY_MTH");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_999_TOT"] = gvGBF.GetRowCellValue(i , "PL1B_999_TOT");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_PREV_NATURE_LEGAL_MTH"] = gvGBF.GetRowCellValue(i , "PL1B_PREV_NATURE_LEGAL_MTH");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_PREV_NATURE_LEGAL_TOT"] = gvGBF.GetRowCellValue(i , "PL1B_PREV_NATURE_LEGAL_TOT");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_PREV_999_MTH"] = gvGBF.GetRowCellValue(i , "PL1B_PREV_999_MTH");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_PREV_999_NEARBY_MTH"] = gvGBF.GetRowCellValue(i , "PL1B_PREV_999_NEARBY_MTH");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_PREV_999_TOT"] = gvGBF.GetRowCellValue(i , "PL1B_PREV_999_TOT");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_ADJ"] = gvGBF.GetRowCellValue(i , "PL1B_ADJ");
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_W_TIME"] = DateTime.Now;
               dtInsertPL2B.Rows[dtInsertPL2B.Rows.Count - 1]["PL2B_W_USER_ID"] = GlobalInfo.USER_ID;
            }
            // 寫入DB
            myResultData = dao30203.updatePL2B(dtInsertPL2B);

            //5. 更新 PL1 (gvMain資料，gvGBF不需寫入DB)
            showMsg = "確認資料(PL1)更新資料庫錯誤! ";
            DataTable dtPL1 = (DataTable)gcMain.DataSource;
            myResultData = dao30203.updatePL1(dtPL1);

         } catch (Exception ex) {
            MessageDisplay.Error(showMsg);
            WriteLog(ex);
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            _ReportHelper = reportHelper;
            CommonReportLandscapeA4 report = new CommonReportLandscapeA4();
            report.printableComponentContainerMain.PrintableComponent = gcMain;
            _ReportHelper.Create(report);

            base.Print(_ReportHelper);
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Success;
      }

      #region GridView Events

      /// <summary>
      /// 新增資料列時，本次擬調整狀態預設為"調高"。
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_InitNewRow(object sender , InitNewRowEventArgs e) {
         try {
            GridView gv = sender as GridView;
            gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"] , 1);

            //直接設定值給dataTable(have UI)
            gv.SetRowCellValue(e.RowHandle , gv.Columns["PL1_NATURE_ADJ"] , "+");
            gv.SetRowCellValue(e.RowHandle , gv.Columns["PL1_LEGAL_ADJ"] , "+");

            //直接設定值給dataTable(no UI)
         } catch (Exception ex) {
            WriteLog(ex , "" , false);
         }
      }

      /// <summary>
      /// 決定哪些欄位無法編輯的事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         try {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
                 gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["Is_NewRow"]).ToString();

            if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
               e.Cancel = false;
               if (gv.FocusedColumn.Name == "PL1_CUR_NATURE" ||
                   gv.FocusedColumn.Name == "PL1_CUR_LEGAL" ||
                   gv.FocusedColumn.Name == "COMPUTE_1") {
                  e.Cancel = true;
               }
            }
            //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
            else if (gv.FocusedColumn.Name == "PL1_NATURE" ||
                    gv.FocusedColumn.Name == "PL1_LEGAL" ||
                    gv.FocusedColumn.Name == "PL1_NATURE_ADJ" ||
                    gv.FocusedColumn.Name == "PL1_LEGAL_ADJ") {
               e.Cancel = false;
            } else {
               e.Cancel = true;
            }
         } catch (Exception ex) {
            WriteLog(ex , "" , false);
         }
      }

      /// <summary>
      /// 動態改變資料列底色的事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         try {
            //要用RowHandle不要用FocusedRowHandle
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
                               gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]).ToString();

            int pl1Nature = gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_NATURE"]).AsInt();
            int pl1Legal = gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_LEGAL"]).AsInt();
            string pl1NatureAdj = gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_NATURE_ADJ"]).ToString();
            string pl1LegalAdj = gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_LEGAL_ADJ"]).ToString();

            //描述每個欄位,在is_newRow時候要顯示的顏色
            //當該欄位不可編輯時,設定為Mint Color.FromArgb(192,220,192)
            switch (e.Column.FieldName) {
               case ("PL1_KIND_ID"):
                  e.Appearance.BackColor = Color.White;
                  break;
               case ("PL1_CUR_NATURE"):
                  e.Appearance.BackColor = Color.FromArgb(224 , 224 , 224);
                  break;
               case ("PL1_CUR_LEGAL"):
                  e.Appearance.BackColor = Color.FromArgb(224 , 224 , 224);
                  break;
               case ("PL1_CP_NATURE"):
                  e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(224 , 224 , 224);
                  break;
               case ("PL1_CP_LEGAL"):
                  e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(224 , 224 , 224);
                  break;
               case ("COMPUTE_1"):
                  e.Appearance.BackColor = Color.FromArgb(224 , 224 , 224);
                  break;
               case ("PL1_NATURE"):
                  if (pl1Nature != gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_NATURE_ORG"]).AsInt() ||
                          gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_NATURE_ORG"]).AsString() == null) {
                     e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(255 , 168 , 255);
                  } else {
                     e.Appearance.BackColor = Color.White;
                  }
                  break;
               case ("PL1_LEGAL"):
                  if (pl1Legal != gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_LEGAL_ORG"]).AsInt() ||
                          gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_LEGAL_ORG"]).AsString() == null) {
                     e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(255 , 168 , 255);
                  } else {
                     e.Appearance.BackColor = Color.White;
                  }
                  break;
               case ("PL1_NATURE_ADJ"):
                  if (pl1NatureAdj != gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_NATURE_ADJ_ORG"]).ToString()) {
                     e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(255 , 168 , 255);
                  } else {
                     e.Appearance.BackColor = Color.White;
                  }
                  break;
               case ("PL1_LEGAL_ADJ"):
                  if (pl1LegalAdj != gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_LEGAL_ADJ_ORG"]).ToString()) {
                     e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(255 , 168 , 255);
                  } else {
                     e.Appearance.BackColor = Color.White;
                  }
                  break;
            }//switch (e.Column.FieldName) {
         } catch (Exception ex) {
            WriteLog(ex , "" , false);
         }
      }

      private void gvMain_CellValueChanged(object sender , CellValueChangedEventArgs e) {
         try {
            GridView gv = sender as GridView;
            string Is_NewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]) == null ? "0" :
                               gv.GetRowCellValue(e.RowHandle , gv.Columns["Is_NewRow"]).ToString();

            if (e.Column.Name == "PL1_NATURE_ADJ" ||
                e.Column.Name == "PL1_LEGAL_ADJ") {
               if (gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_NATURE_ADJ"]).ToString() == " " &&
                   gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_LEGAL_ADJ"]).ToString() == " ") {
                  gv.SetRowCellValue(e.RowHandle , gv.Columns["COMPUTE_1"] , "不適用");
               } else {
                  string volume = "";
                  if (gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_MAX_TYPE"]).AsString() == "OI") {
                     volume = "未平倉量" + "(" + gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_MAX_QNTY"]).AsInt().ToString("#,###") + ")";
                  } else {
                     volume = "交易量" + "(" + gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_MAX_QNTY"]).AsInt().ToString("#,###") + ")";
                  }
                  if (Is_NewRow != "1") {
                     gv.SetRowCellValue(e.RowHandle , gv.Columns["COMPUTE_1"] , "近" +
                        gv.GetRowCellValue(e.RowHandle , gv.Columns["PL1_MAX_MONTH_CNT"]).AsString() + "個月" + volume);
                  } else {
                     gv.SetRowCellValue(e.RowHandle , gv.Columns["COMPUTE_1"] , "");
                  }
               }
            }
         } catch (Exception ex) {
            WriteLog(ex , "" , false);
         }
      }

      private void gvGBF_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         try {
            //要用RowHandle不要用FocusedRowHandle
            GridView gv = sender as GridView;

            switch (e.Column.FieldName) {
               case ("PL1B_PREV_NATURE_LEGAL_MTH"):
               case ("PL1B_PREV_NATURE_LEGAL_TOT"):
               case ("PL1B_PREV_999_MTH"):
               case ("PL1B_PREV_999_NEARBY_MTH"):
               case ("PL1B_PREV_999_TOT"):
                  e.Appearance.BackColor = Color.FromArgb(224 , 224 , 224);
                  break;
               default:
                  e.Appearance.BackColor = Color.White;
                  break;
            }//switch (e.Column.FieldName) {
         } catch (Exception ex) {
            WriteLog(ex , "" , false);
         }
      }

      private void gvGBF_ShowingEditor(object sender , CancelEventArgs e) {
         try {
            GridView gv = sender as GridView;

            //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
            if (gv.FocusedColumn.Name == "PL1B_KIND_ID" ||
                gv.FocusedColumn.Name == "PL1B_PREV_NATURE_LEGAL_MTH" ||
                    gv.FocusedColumn.Name == "PL1B_PREV_NATURE_LEGAL_TOT" ||
                    gv.FocusedColumn.Name == "PL1B_PREV_999_MTH" ||
                    gv.FocusedColumn.Name == "PL1B_PREV_999_NEARBY_MTH" ||
                    gv.FocusedColumn.Name == "PL1B_PREV_999_TOT") {
               e.Cancel = true;
            } else {
               e.Cancel = false;
            }
         } catch (Exception ex) {
            WriteLog(ex , "" , false);
         }
      }
      #endregion

   }
}