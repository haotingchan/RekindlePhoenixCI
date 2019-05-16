﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BaseGround;
using Common;
using DevExpress.XtraEditors.Repository;
using DataObjects.Dao.Together.SpecificDao;
using BusinessObjects.Enums;
using DevExpress.XtraGrid.Views.Grid;
using BaseGround.Shared;
using BaseGround.Report;
using BusinessObjects;
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Controls;
using System.IO;
using PhoenixCI.BusinessLogic.Prefix2;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

/// <summary>
/// John, 2019/5/13
/// </summary>
namespace PhoenixCI.FormUI.Prefix2
{
   /// <summary>
   /// 20231_apdk 部位限制個股類標的轉入 新增apdk table資料
   /// </summary>
   public partial class W20231_2 : FormParent
   {
      #region 全域變數
      private D20231 dao20231;
      #endregion

      public W20231_2(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = "20231 ─ " + _ProgramName + " 新增個股契約基本資料";

         GridHelper.SetCommonGrid(gvMain);
         PrintableComponent = gcMain;

         dao20231 = new D20231();

         //系統
         ApdkProdType();
         //商品子類別
         ApdkProdSubtype();
         //商品對照類別
         ApdkParamKey();
         //上市/上櫃
         UnderlyingMarket();
         //一般/小型
         ApdkRemark();
         //盤別
         ApdkMarketClose();
         //幣別
         ApdkCurrencyType();
         // 設定List LookupItem變數項目
         SetOtherLookupItem();
      }

      /// <summary>
      /// 設定List LookupItem變數項目
      /// </summary>
      private void SetOtherLookupItem()
      {
         //STF	STF/ETF	ETF/
         List<LookupItem> StfList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "STF", DisplayMember = "STF" },
                                        new LookupItem() { ValueMember = "ETF", DisplayMember = "ETF" }};
         ApdkParamKeyLookUpEditF.SetColumnLookUp(StfList, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);

         //STC	STC/ETC	ETC/
         List<LookupItem> StcList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "STC", DisplayMember = "STC" },
                                        new LookupItem() { ValueMember = "ETC", DisplayMember = "ETC" }};
         ApdkParamKeyLookUpEditC.SetColumnLookUp(StcList, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);

      }

      /// <summary>
      /// 欄位
      /// </summary>
      private void ApdkCurrencyType()
      {
         ApdkCurrencyTypeLookUpEdit.SetColumnLookUp(new COD().ListByCurrency(), "CURRENCY_TYPE", "CURRENCY_NAME", TextEditStyles.DisableTextEditor, null);
         APDK_CURRENCY_TYPE.ColumnEdit = ApdkCurrencyTypeLookUpEdit;
      }

      /// <summary>
      /// 盤別欄位
      /// </summary>
      private void ApdkMarketClose()
      {
         ApdkMarketCloseLookUpEdit.SetColumnLookUp(new OCFG().ListAll(), "OSW_GRP", "OSW_GRP_NAME", TextEditStyles.DisableTextEditor, null);
         APDK_MARKET_CLOSE.ColumnEdit = ApdkMarketCloseLookUpEdit;
      }

      /// <summary>
      /// 一般/小型 欄位
      /// </summary>
      private void ApdkRemark()
      {
         List<LookupItem> remarkList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = " ", DisplayMember = "一般"},
                                        new LookupItem() { ValueMember = "M", DisplayMember = "小型"},
                                        new LookupItem() { ValueMember = " ", DisplayMember = " " }};
         ApdkRemarkLookUpEdit.SetColumnLookUp(remarkList, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         APDK_REMARK.ColumnEdit = ApdkRemarkLookUpEdit;
      }

      /// <summary>
      /// 上市/上櫃欄位
      /// </summary>
      private void UnderlyingMarket()
      {
         List<LookupItem> UnderlyingMarketList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = "1", DisplayMember = "上市"},
                                        new LookupItem() { ValueMember = "2", DisplayMember = "上櫃"},
                                        new LookupItem() { ValueMember = " ", DisplayMember = " " }};
         ApdkUnderlyingMarketLookUpEdit.SetColumnLookUp(UnderlyingMarketList, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         APDK_UNDERLYING_MARKET.ColumnEdit = ApdkUnderlyingMarketLookUpEdit;
      }

      /// <summary>
      /// 商品對照類別欄位
      /// </summary>
      private void ApdkParamKey()
      {
         List<LookupItem> pkList = new List<LookupItem>(){
                                        new LookupItem() { ValueMember = " ", DisplayMember = " " }};
         ApdkParamKeyLookUpEditC.SetColumnLookUp(pkList, "ValueMember", "DisplayMember", TextEditStyles.Standard, null);
         APDK_PARAM_KEY.ColumnEdit = ApdkParamKeyLookUpEditC;
      }

      /// <summary>
      /// 商品子類別欄位
      /// </summary>
      private void ApdkProdSubtype()
      {
         DataTable apdkListCol = new COD().ListByCol("APDK", "APDK_PROD_SUBTYPE");
         apdkListCol.Rows.RemoveAt(0);
         ApdkProdSubtypeLookUpEdit.SetColumnLookUp(apdkListCol, "COD_ID", "COD_DESC", TextEditStyles.DisableTextEditor, null);
         APDK_PROD_SUBTYPE.ColumnEdit = ApdkProdSubtypeLookUpEdit;
      }

      /// <summary>
      /// 系統欄位
      /// </summary>
      private void ApdkProdType()
      {
         List<LookupItem> typelist = new List<LookupItem>(){
            new LookupItem() { ValueMember = "F", DisplayMember = "期貨" },
            new LookupItem() { ValueMember = "O", DisplayMember = "選擇權"}};
         ApdkProdTypeLookUpEdit.SetColumnLookUp(typelist, "ValueMember", "DisplayMember", TextEditStyles.DisableTextEditor, null);
         APDK_PROD_TYPE.ColumnEdit = ApdkProdTypeLookUpEdit;
      }

      /// <summary>
      /// Enabled GridView按鈕
      /// </summary>
      /// <param name="flag"></param>
      private void EnabledGridViewBtn(bool flag)
      {
         gcMain.Visible = flag;
         _ToolBtnSave.Enabled = flag;
         _ToolBtnDel.Enabled = flag;
         _ToolBtnPrintAll.Enabled = flag;
      }

      /// <summary>
      /// 沒有新增資料時,則自動新增內容
      /// </summary>
      private void StartShowGridView()
      {
         DataTable data = StartGridViewData();
         //沒有新增資料時,則自動新增內容
         if (data.Rows.Count == 0) {
            data.Rows.Add(data.NewRow());
         }
         gcMain.DataSource = data;
         gcMain.Focus();
      }

      /// <summary>
      /// 初始化轉換資料
      /// </summary>
      /// <returns></returns>
      private DataTable StartGridViewData()
      {
         DataTable returnTable = dao20231.ListApdkData();
         DataTable dtCloned = dao20231.ListApdkData().Clone();
         //編輯 時間格式轉為字串
         dtCloned.Columns["APDK_BEGIN_DATE"].DataType = typeof(string);
         for (int k = 0; k < returnTable.Rows.Count; k++) {
            dtCloned.Rows.Add(dtCloned.NewRow());
            foreach (DataColumn col in returnTable.Columns) {
               if (col.ColumnName == "APDK_BEGIN_DATE") {
                  if (returnTable.Rows[k]["APDK_BEGIN_DATE"] != DBNull.Value)
                     dtCloned.Rows[k][col.ColumnName] = returnTable.Rows[k][col.ColumnName].AsDateTime("yyyy/MM/dd").ToString("yyyy/MM/dd");
                  continue;
               }
               dtCloned.Rows[k][col.ColumnName] = returnTable.Rows[k][col.ColumnName];
            }
         }

         return dtCloned;
      }

      protected override ResultStatus Retrieve()
      {
         base.Retrieve(gcMain);
         DataTable returnTable = dao20231.ListApdkData();
         //沒有資料時,則隱藏GridView
         if (returnTable.Rows.Count <= 0) {
            EnabledGridViewBtn(false);
            MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
            return ResultStatus.Success;
         }
         else {
            EnabledGridViewBtn(true);
         }

         gcMain.DataSource = StartGridViewData();
         gcMain.Focus();
         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow()
      {
         if (gcMain.Visible == false) {
            StartShowGridView();
            EnabledGridViewBtn(true);
            return ResultStatus.Success;
         }
         int focusIndex = gvMain.GetFocusedDataSourceRowIndex();
         gvMain.CloseEditor();//必須先做close edit, like dt.AcceptChanges();

         //新增一行並做初始值設定
         DataTable dt = (DataTable)gcMain.DataSource;
         if (dt == null) {
            return ResultStatus.FailButNext;
         }

         dt.Rows.InsertAt(dt.NewRow(), focusIndex);
         gcMain.DataSource = dt;//重新設定給grid,雖然會更新但是速度太快,畫面不會閃爍
         gvMain.FocusedRowHandle = focusIndex;//原本的focusRowHandle會記住之前的位置,其實只是往上一行
         gvMain.FocusedColumn = gvMain.Columns[0];
         return ResultStatus.Success;
      }

      private void SetFocused(DataTable dt, DataRow dr, string colName)
      {
         gvMain.FocusedRowHandle = dt.Rows.IndexOf(dr);
         gvMain.FocusedColumn = gvMain.Columns[colName];
         gvMain.ShowEditor();
      }

      protected override ResultStatus Save(PokeBall poke)
      {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();

         DataTable dt = (DataTable)gcMain.DataSource;
         DataTable dtChange = dt.GetChanges();
         DataTable dtDeleteChange = dt.GetChanges(DataRowState.Deleted);
         DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
         DataTable dtForModified = dt.GetChanges(DataRowState.Modified);

         int getDeleteCount = dtDeleteChange != null ? dtDeleteChange.Rows.Count : 0;
         //存檔前檢查
         if (getDeleteCount == 0 && dtChange != null)//無法經由資料列存取已刪除的資料列資訊。
         {
            // 寫入DB
            //轉換原資料表型態
            DataTable insertData = dt;
            DataTable data = dao20231.ListApdkData().Clone();//dw_1.reset()
            for (int k = 0; k < insertData.Rows.Count; k++) {
               DataRow insertDr = insertData.Rows[k];

               data.Rows.Add(data.NewRow());
               foreach (DataColumn col in insertData.Columns) {
                  DataRow dr = data.Rows[k];
                  //APDK_BEGIN_DATE String 轉 Date
                  if (col.ColumnName == "APDK_BEGIN_DATE") {
                     DateTime dateTime;
                     if (!DateTime.TryParse(insertDr["APDK_BEGIN_DATE"].ToString(), out dateTime)) {
                        MessageDisplay.Error(insertDr["APDK_BEGIN_DATE"].ToString() + "日期格式錯誤!");
                        SetFocused(insertData, insertDr, "APDK_BEGIN_DATE");
                        return ResultStatus.FailButNext;
                     }
                     dr[col.ColumnName] = insertDr[col.ColumnName].AsDateTime("yyyy/MM/dd");
                     continue;
                  }
                  //檢查該欄位是否填寫資料
                  if (insertDr[col.ColumnName] == DBNull.Value && gvMain.Columns[col.ColumnName] != null) {
                     MessageDisplay.Warning($"[{gvMain.Columns[col.ColumnName].Caption}]資料未填寫完成，請確認!!");
                     SetFocused(insertData, insertDr, col.ColumnName);
                     return ResultStatus.FailButNext;
                  }
                  dr[col.ColumnName] = insertDr[col.ColumnName];

               }//foreach (DataColumn col in insertData.Columns)

            }//for (int k = 0; k < insertData.Rows.Count; k++

            foreach (DataRow dr in data.Rows) {
               string kindidsub2 = dr["APDK_KIND_ID"].AsString().SubStr(0, 2);
               dr["APDK_KIND_ID_STO"] = kindidsub2;
               dr["APDK_QUOTE_CODE"] = "Y";
               dr["APDK_KIND_ID2"] = kindidsub2;
               dr["APDK_KIND_ID_OUT"] = dr["APDK_KIND_ID"].AsString();
               dr["APDK_EXPIRY_TYPE"] = "S";
               dr["APDK_NAME_OUT"] = dr["APDK_NAME"].AsString();
               dr["APDK_MARKET_CODE"] = "0";
               dr["APDK_KIND_LEVEL"] = dr["APDK_REMARK"].AsString() == "M" ? 6 : 1;
            }

            if (dtChange != null) {
               try {
                  ResultData myResultData = dao20231.UpdateAPDK(data);
               }
               catch (Exception ex) {
                  WriteLog(ex);
               }

               ReportHelper ReportHelper = new ReportHelper(gcMain, "20231", "[20231] 部位限制個股類標的轉入－新增個股契約基本資料");
               Print(ReportHelper);
            }
            else {
               MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

         }
         MessageDisplay.Info(MessageDisplay.MSG_OK);
         return ResultStatus.Success;
      }

      protected override ResultStatus COMPLETE()
      {
         //不做任何事
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper ReportHelper)
      {
         try {
            ReportHelper = new ReportHelper(gcMain, "20231", "[20231] 部位限制個股類標的轉入－新增個股契約基本資料");
            CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            ReportHelper.Create(reportLandscape);
            ReportHelper.Print();
            ReportHelper.Export(FileType.PDF, ReportHelper.FilePath);

         }
         catch (Exception ex) {
            throw ex;
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow()
      {
         base.DeleteRow(gvMain);
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnInsert.Enabled = true;
         EnabledGridViewBtn(true);

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         StartShowGridView();
         return ResultStatus.Success;
      }

      private void gvMain_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
      {
         GridView gv = sender as GridView;
         switch (e.Column.FieldName) {
            case "APDK_BEGIN_DATE":
               DateTime dateTime;
               if (DateTime.TryParse(e.Value.ToString(), out dateTime)) {
                  return;
               }
               else {
                  MessageDisplay.Warning(e.Value.ToString() + "日期格式錯誤!");
                  gv.FocusedRowHandle = e.RowHandle;
                  gv.FocusedColumn = gv.Columns[e.Column.FieldName];
                  gv.ShowEditor();
               }
               return;
            case "APDK_PROD_TYPE":
            case "APDK_PROD_SUBTYPE":
            case "APDK_KIND_ID":
               if (gv.GetRowCellValue(e.RowHandle, gv.Columns["APDK_PROD_SUBTYPE"]).AsString() != "S") {
                  gv.SetRowCellValue(e.RowHandle, gv.Columns["APDK_PARAM_KEY"], gv.GetRowCellValue(e.RowHandle, gv.Columns["APDK_KIND_ID"]));
               }
               else {
                  gv.SetRowCellValue(e.RowHandle, gv.Columns["APDK_PARAM_KEY"], DBNull.Value);
               }
               return;
            default:
               return;
         }
      }

      private void gvMain_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
      {
         GridView gv = sender as GridView;
         switch (e.Column.FieldName) {
            case "APDK_PARAM_KEY":
               if (gv.GetRowCellValue(e.RowHandle, gv.Columns["APDK_PROD_SUBTYPE"]).AsString() == "S"
                  && gv.GetRowCellValue(e.RowHandle, gv.Columns["APDK_PROD_TYPE"]).AsString() == "F") {
                  e.RepositoryItem = ApdkParamKeyLookUpEditF;
               }

               if (gv.GetRowCellValue(e.RowHandle, gv.Columns["APDK_PROD_SUBTYPE"]).AsString() == "S"
                  && gv.GetRowCellValue(e.RowHandle, gv.Columns["APDK_PROD_TYPE"]).AsString() != "F") {
                  e.RepositoryItem = ApdkParamKeyLookUpEditC;
               }

               if (gv.GetRowCellValue(e.RowHandle, gv.Columns["APDK_PROD_SUBTYPE"]).AsString() != "S") {
                  string kindid = gv.GetRowCellValue(e.RowHandle, gv.Columns["APDK_KIND_ID"]).AsString();
                  gv.SetRowCellValue(e.RowHandle, gv.Columns["APDK_PARAM_KEY"], kindid);
                  e.RepositoryItem = pkTextEdit;
               }
               break;
         }
      }

      private void ApdkProdSubtypeLookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         gvMain.FocusedColumn = gvMain.Columns["APDK_PARAM_KEY"];
         gvMain.ShowEditor();
      }

      private void ApdkProdTypeLookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         gvMain.FocusedColumn = gvMain.Columns["APDK_PARAM_KEY"];
         gvMain.ShowEditor();
      }

   }
}