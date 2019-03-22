using System.Data;
using System.Windows.Forms;
using BaseGround;
using DataObjects.Dao.Together;
using DevExpress.XtraEditors.Repository;
using BaseGround.Report;
using BusinessObjects.Enums;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraGrid.Views.Grid;
using BusinessObjects;
using System.ComponentModel;
using Common;
using System.Drawing;
using System;
using BaseGround.Shared;
using System.Collections.Generic;
using System.Data.OracleClient;
using DataObjects.Dao.Together.TableDao;

namespace PhoenixCI.FormUI.Prefix5
{
   /// <summary>
   /// 20190123,john,51030 造市者限制設定
   /// </summary>
   public partial class W51030 : FormParent
   {
      #region 全域變數
      /// <summary>
      /// 交易時段
      /// </summary>
      private readonly string MARKET_CODE = "MMF_MARKET_CODE";
      /// <summary>
      /// 期貨/選擇權
      /// </summary>
      private readonly string PROD_TYPE = "MMF_PROD_TYPE";
      /// <summary>
      /// 商品類別
      /// </summary>
      private readonly string PARAM_KEY = "MMF_PARAM_KEY";
      /// <summary>
      /// 週六豁免造市
      /// </summary>
      //private readonly string CP_FLAG = "MMF_SAT_CP_FLAG";
      /// <summary>
      /// 報價規定判斷方式MMF_CP_KIND
      /// </summary>
      private readonly string CP_KIND = "MMF_CP_KIND";
      private APDK daoAPDK;
      private COD daoCOD;
      private D51030 dao51030;
      private Dictionary<string, string> dic;
      /// <summary>
      /// 交易時段
      /// </summary>
      private RepositoryItemLookUpEdit MARKET_CODE_LookUpEdit;
      /// <summary>
      /// 期貨/選擇權
      /// </summary>
      private RepositoryItemLookUpEdit PROD_TYPE_LookUpEdit;
      /// <summary>
      /// 商品類別
      /// </summary>
      private RepositoryItemLookUpEdit PARAM_KEY_LookUpEdit;
      /// <summary>
      /// 週六豁免造市
      /// </summary>
      //private RepositoryItemLookUpEdit CP_FLAG_LookUpEdit;
      /// <summary>
      /// 報價規定判斷方式
      /// </summary>
      private RepositoryItemLookUpEdit CP_KIND_LookUpEdit;
      #endregion

      public W51030(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();

         GridHelper.SetCommonGrid(gvMain);
         PrintableComponent = gcMain;

         dao51030 = new D51030();
         //交易時段
         dic = new Dictionary<string, string>() { { "0", "一般" }, { "1", "夜盤" } };
         DataTable mk_code = setcolItem(dic);
         MARKET_CODE_LookUpEdit = new RepositoryItemLookUpEdit();
         MARKET_CODE_LookUpEdit.SetColumnLookUp(mk_code, "ID", "Desc");
         MMF_MARKET_CODE.ColumnEdit = MARKET_CODE_LookUpEdit;
         //期貨/選擇權
         dic = new Dictionary<string, string>() { { "F", "F" }, { "O", "O" } };
         DataTable mmfType = setcolItem(dic);
         PROD_TYPE_LookUpEdit = new RepositoryItemLookUpEdit();
         PROD_TYPE_LookUpEdit.SetColumnLookUp(mmfType, "ID", "Desc");
         MMF_PROD_TYPE.ColumnEdit = PROD_TYPE_LookUpEdit;
         //商品類別
         daoAPDK = new APDK();
         PARAM_KEY_LookUpEdit = new RepositoryItemLookUpEdit();
         PARAM_KEY_LookUpEdit.SetColumnLookUp(daoAPDK.ListParamKey(), "APDK_PARAM_KEY", "APDK_PARAM_KEY");
         MMF_PARAM_KEY.ColumnEdit = PARAM_KEY_LookUpEdit;
         //-週六豁免造市-此功能移除 
         /*dic = new Dictionary<string, string>() { { "", "" }, { "N", "豁免" } };
         DataTable CP_FLAG = setcolItem(dic);
         CP_FLAG_LookUpEdit = new RepositoryItemLookUpEdit();
         CP_FLAG_LookUpEdit.SetColumnLookUp(CP_FLAG, "ID", "Desc");
         MMF_SAT_CP_FLAG.ColumnEdit = CP_FLAG_LookUpEdit;*/
         //報價規定判斷方式
         daoCOD = new COD();
         dic = new Dictionary<string, string>();
         foreach (DataRow dr in daoCOD.ListByCol("MMF", CP_KIND).Rows) {
            string codid = dr["COD_ID"].AsString();
            if (string.IsNullOrEmpty(codid)) {
               continue;
            }
            dic.Add(codid, string.Format("({0}){1}", codid, dr["COD_DESC"].AsString()));
         }
         DataTable mmfKIND = setcolItem(dic);
         CP_KIND_LookUpEdit = new RepositoryItemLookUpEdit();
         CP_KIND_LookUpEdit.SetColumnLookUp(mmfKIND, "ID", "Desc");
         MMF_CP_KIND.ColumnEdit = CP_KIND_LookUpEdit;
      }
      /// <summary>
      /// 自訂下拉式選項
      /// </summary>
      /// <param name="dic">陣列</param>
      /// <returns></returns>
      private DataTable setcolItem(Dictionary<string, string> dic)
      {
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
      protected override ResultStatus Retrieve()
      {
         base.Retrieve(gcMain);
         DataTable returnTable = new DataTable();
         returnTable = dao51030.ListD50130();
         /*******************
         沒有新增資料時,則自動新增內容
         *******************/
         if (returnTable.Rows.Count == 0) {
            InsertRow();
         }
         returnTable.Columns.Add("Is_NewRow", typeof(string));
         gcMain.DataSource = returnTable;

         gcMain.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus InsertRow()
      {
         int focusIndex = gvMain.GetFocusedDataSourceRowIndex();
         gvMain.CloseEditor();//必須先做close edit, like dt.AcceptChanges();

         //新增一行並做初始值設定
         DataTable dt = (DataTable)gcMain.DataSource;
         DataRow drNew = dt.NewRow();

         drNew["Is_NewRow"] = 1;
         drNew["MMF_W_USER_ID"] = GlobalInfo.USER_ID;
         drNew["MMF_W_TIME"] = DateTime.Now;

         dt.Rows.InsertAt(drNew, focusIndex);
         gcMain.DataSource = dt;//重新設定給grid,雖然會更新但是速度太快,畫面不會閃爍
         gvMain.FocusedRowHandle = focusIndex;//原本的focusRowHandle會記住之前的位置,其實只是往上一行
         gvMain.FocusedColumn = gvMain.Columns[0];
         return ResultStatus.Success;
      }

      private void gvMain_ShowingEditor(object sender, CancelEventArgs e)
      {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
             gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false;
            gv.SetRowCellValue(gv.FocusedRowHandle, gv.Columns["Is_NewRow"], 1);
         }
         else if (gv.FocusedColumn.FieldName == MARKET_CODE ||
            gv.FocusedColumn.FieldName == PROD_TYPE ||
             gv.FocusedColumn.FieldName == PARAM_KEY) {
            e.Cancel = true;
         }
         else {
            e.Cancel = false;
         }
      }

      private void gvMain_RowCellStyle(object sender, RowCellStyleEventArgs e)
      {
         GridView gv = sender as GridView;
         string isNewRow = gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]) == null ? "0" :
              gv.GetRowCellValue(e.RowHandle, gv.Columns["Is_NewRow"]).ToString();

         if (e.Column.FieldName == MARKET_CODE ||
             e.Column.FieldName == PROD_TYPE ||
             e.Column.FieldName == PARAM_KEY) {
            e.Appearance.BackColor = isNewRow == "1" ? Color.White : Color.Silver;
         }
         if (e.Column.FieldName == CP_KIND) {
            int value = gv.GetRowCellValue(e.RowHandle, MMF_CP_KIND).AsInt();
            if (value != 3)
               e.Appearance.ForeColor = Color.FromArgb(0, 0, 255);
         }
      }

      private bool SaveBefore(DataTable dt)
      {
         string lsType, lsVal1, lsVal2, lsVal3, lsVal4, lsVal5;
         try {
            //只檢查變動的部分
            foreach (DataRow dr in dt.GetChanges().Rows) {
               if (dr["op_type"].AsString() == " ") {
                  continue;
               }
               //key值不能為null
               if (string.IsNullOrEmpty(dr[MARKET_CODE].AsString())) {
                  PbFunc.messageBox(GlobalInfo.ErrorText, "「交易時段」必須要選取值！", MessageBoxIcon.Stop);
                  //set Focused
                  setFocused(dt, dr, MARKET_CODE);
                  return false;
               }
               if (string.IsNullOrEmpty(dr[PARAM_KEY].AsString())) {
                  PbFunc.messageBox(GlobalInfo.ErrorText, "「商品類別」必須要選取值！", MessageBoxIcon.Stop);
                  //set Focused
                  setFocused(dt, dr, PARAM_KEY);
                  return false;
               }
               //必須回應詢價比
               if (string.IsNullOrEmpty(dr["mmf_resp_ratio"].AsString())) {
                  PbFunc.messageBox(GlobalInfo.ErrorText, "「必須回應詢價比(%)」必須要輸入值！", MessageBoxIcon.Stop);
                  //set Focused
                  setFocused(dt, dr, "MMF_RESP_RATIO");
                  return false;
               }
               //最低造市量
               if (string.IsNullOrEmpty(dr["mmf_qnty_low"].AsString())) {
                  PbFunc.messageBox(GlobalInfo.ErrorText, "「最低造市量」必須要輸入值！", MessageBoxIcon.Stop);
                  //set Focused
                  setFocused(dt, dr, "MMF_QNTY_LOW");
                  return false;
               }
               //報價有效量比率
               if (string.IsNullOrEmpty(dr["mmf_quote_valid_rate"].AsString())) {
                  PbFunc.messageBox(GlobalInfo.ErrorText, "「報價有效量比率」必須要輸入值！", MessageBoxIcon.Stop);
                  //set Focused
                  setFocused(dt, dr, "MMF_QUOTE_VALID_RATE");
                  return false;
               }
               //報價每日平均維持分鐘
               if (string.IsNullOrEmpty(dr["mmf_avg_time"].AsString())) {
                  PbFunc.messageBox(GlobalInfo.ErrorText, "「報價每日平均維持分鐘」必須要輸入值！", MessageBoxIcon.Stop);
                  //set Focused
                  setFocused(dt, dr, "MMF_AVG_TIME");
                  return false;
               }
               //寫LOGV
               lsType = "I";
               lsVal1 = dr["mmf_param_key"].AsString();
               lsVal2 = dr["mmf_resp_ratio"].AsString();
               lsVal3 = dr["mmf_qnty_low"].AsString();
               lsVal4 = dr["mmf_quote_valid_rate"].AsString();
               lsVal5 = dr["mmf_avg_time"].AsString();
               new LOGV().Insert(_ProgramID, GlobalInfo.USER_ID, lsType, lsVal1, lsVal2, lsVal3, lsVal4, lsVal5);
            }
         }
         catch (Exception ex) {
            WriteLog(ex);
            return false;
         }
         return true;
      }

      private void setFocused(DataTable dt, DataRow dr, string colName)
      {
         gvMain.FocusedRowHandle = dt.Rows.IndexOf(dr);
         gvMain.FocusedColumn = gvMain.Columns[colName];
         gvMain.ShowEditor();
      }

      protected override ResultStatus Save(PokeBall poke)
      {
         base.Save(gcMain);

         DataTable dt = (DataTable)gcMain.DataSource;
         DataTable dtChange = dt.GetChanges();
         DataTable dtDeleteChange = dt.GetChanges(DataRowState.Deleted);
         int getDeleteCount = dtDeleteChange != null ? dtDeleteChange.Rows.Count : 0;
         ////存檔前檢查
         if (getDeleteCount == 0 && dtChange != null)//無法經由資料列存取已刪除的資料列資訊。
         {
            if (!SaveBefore(dt)) {
               return ResultStatus.Fail;
            }
         }
         if (dtChange != null) {
            try {
               // 寫入DB
               ResultData myResultData = dao51030.UpdateMMF(dt);
            }
            catch (Exception ex) {
               WriteLog(ex);
            }
            return ResultStatus.Success;
         }
         else {
            MessageBox.Show("沒有變更資料,不需要存檔!", "注意", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
         }
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper ReportHelper)
      {
         try {
            ReportHelper reportHelper = new ReportHelper(gcMain, _ProgramID, _ProgramID + _ProgramName);
            reportHelper.Print();

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

         _ToolBtnInsert.Enabled = true;
         _ToolBtnSave.Enabled = true;
         _ToolBtnDel.Enabled = true;
         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;

         return ResultStatus.Success;
      }

      public override ResultStatus BeforeOpen()
      {
         base.BeforeOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus Open()
      {
         base.Open();

         //直接讀取資料
         Retrieve();
         //Header上色
         //CustomDrawColumnHeader(gcMain,gvMain);

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         base.AfterOpen();

         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         return base.BeforeClose();
      }

      protected override ResultStatus COMPLETE()
      {
         MessageDisplay.Info(MessageDisplay.MSG_OK);
         Retrieve();
         return ResultStatus.Success;
      }
   }
}