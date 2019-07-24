using System;
using System.Data;
using System.Windows.Forms;
using BaseGround;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors;
using BusinessObjects;
using BaseGround.Report;
using BaseGround.Widget;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.Utils;
using DataObjects.Dao.Together.TableDao;
using System.Collections.Generic;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Linq;
using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraGrid;
using DataObjects.Dao.Together;

namespace PhoenixCI.FormUI.Prefix4 {
   public partial class W40080 : FormParent {

      protected string nullYmd { get; set; }
      private D40080 dao40080;
      private SP2 daoSP2;

      public W40080(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao40080 = new D40080();
         daoSP2 = new SP2();
      }

      protected override ResultStatus Open() {
         base.Open();
         nullYmd = null;

         //日期
         txtTradeDate.DateTimeValue = DateTime.Now;
         txtDate1.Text = "1900/01/01";
         txtDate2.Text = "1900/01/01";

         //觀察/調整 RadioGroup
         RadioGroupItem item1 = new RadioGroupItem();
         item1.Description = "";
         item1.Value = " ";
         RadioGroupItem item2 = new RadioGroupItem();
         item2.Description = "";
         item2.Value = "Y";

         RepositoryItemRadioGroup repositoryItemRadioGroup = new RepositoryItemRadioGroup();
         repositoryItemRadioGroup.Items.Add(item1);
         repositoryItemRadioGroup.Items.Add(item2);
         repositoryItemRadioGroup.Columns = 2;
         //repositoryItemRadioGroup.BestFitWidth = 40;
         repositoryItemRadioGroup.GlyphAlignment = HorzAlignment.Center;

         SP2_ADJ_CODE.ColumnEdit = repositoryItemRadioGroup;
         SP2_ADJ_CODE.ColumnEdit.Appearance.TextOptions.HAlignment = HorzAlignment.Center;

         //設定依條件選擇狀態的下拉選單
         //List<LookupItem> adjustType = new List<LookupItem>(){
         //                               new LookupItem() { ValueMember = "none", DisplayMember = "全取消"},
         //                               new LookupItem() { ValueMember = "indes", DisplayMember = "全選指數類" },
         //                               new LookupItem() { ValueMember = "all", DisplayMember = "全選"},
         //                               new LookupItem() { ValueMember = "StcEtc", DisplayMember = "全選STC,ETC" },
         //                               new LookupItem() { ValueMember = "1", DisplayMember = "全選Group1"},
         //                               new LookupItem() { ValueMember = "2", DisplayMember = "全選Group2" }};

         DataTable dtAdjustType = new CODW().ListLookUpEdit("40080" , "40080_DDL_ADJUST");
         Extension.SetDataTable(ddlAdjust , dtAdjustType , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , "");
         ddlAdjust.ItemIndex = 0; // none

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {

         DataTable dt = dao40080.GetData(txtTradeDate.DateTimeValue);
         DataTable dtSp2 = dao40080.GetSP2Data(txtTradeDate.DateTimeValue);

         #region 設定生效日期
         int ll_found = 0; ;
         //Group1
         if (dt.Select("sp1_osw_grp='1' and sp2_value_date is not null").Length != 0) { //有找到
            ll_found = dt.Rows.IndexOf(dt.Select("sp1_osw_grp='1' and sp2_value_date is not null")[0]); //ll_found = 找到的列數(從0開始)
         } else {
            ll_found = -1;
         }

         if (ll_found >= 0) { //有找到
            string sp2ValueDate = dt.Rows[ll_found]["sp2_value_date"].AsDateTime().ToString("yyyy/MM/dd");
            txtDate1.Text = sp2ValueDate;
         } else { //沒找到
            string tmpDate = PbFunc.f_get_ocf_next_n_day(txtTradeDate.DateTimeValue , 1).ToString("yyyy/MM/dd");
            txtDate1.Text = tmpDate;
         }

         //Group2
         if (dt.Select("sp1_osw_grp='5' and sp2_value_date is not null").Length != 0) {
            ll_found = dt.Rows.IndexOf(dt.Select("sp1_osw_grp='5' and sp2_value_date is not null")[0]);
         } else {
            ll_found = -1;
         }

         if (ll_found >= 0) {
            string sp2ValueDate = dt.Rows[ll_found]["sp2_value_date"].AsDateTime().ToString("yyyy/MM/dd");
            txtDate2.Text = sp2ValueDate;
         } else {
            string tmpDate = PbFunc.f_get_ocf_next_n_day(txtTradeDate.DateTimeValue , 2).ToString("yyyy/MM/dd");
            txtDate2.Text = tmpDate;
         }
         #endregion

         gcMain.Visible = true;
         gcMain.DataSource = dt;
         GridHelper.SetCommonGrid(gvMain);
         gvMain.OptionsBehavior.AllowFixedGroups = DefaultBoolean.True;

         gvMain.Columns["OSW_GRP"].Group();
         gvMain.Columns["SP1_CHANGE_RANGE"].DisplayFormat.FormatType = FormatType.Numeric;
         gvMain.Columns["SP1_CHANGE_RANGE"].DisplayFormat.FormatString = "P";

         gvMain.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
         gvMain.ColumnPanelRowHeight = 20;
         gvMain.AppearancePrint.HeaderPanel.Font = new Font("Microsoft YaHei" , gvMain.AppearancePrint.HeaderPanel.Font.Size);
         gvMain.AppearancePrint.Row.Font = new Font("Microsoft YaHei" , 12);
         gvMain.OptionsPrint.AllowMultilineHeaders = true;
         gvMain.AppearancePrint.GroupRow.Font = new Font("Microsoft YaHei" , 12);

         gvMain.BestFitColumns();
         gvMain.ExpandAllGroups();
         gcMain.Focus();

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall poke) {
         gvMain.UpdateCurrentRow();
         gvMain.CloseEditor();
         try {

            DataTable dtCurrent = dao40080.GetData(txtTradeDate.DateTimeValue); //原始資料
            DataTable dt = (DataTable)gcMain.DataSource; //現在更改後的
            DataTable dtChange = dt.Clone();

            int w = -1;
            foreach (DataRow row1 in dtCurrent.Rows) {

               w++;
               var array1 = row1.ItemArray;
               var array2 = dt.Rows[w].ItemArray;

               if (!array1.SequenceEqual(array2)) {
                  dtChange.ImportRow(dt.Rows[w]);
               }
               continue;
            }

            if (dtChange == null) {
               MessageDisplay.Warning("沒有變更資料,不需要存檔!" , GlobalInfo.WarningText);
               return ResultStatus.FailButNext;
            }

            DateTime ldt_w_time = DateTime.Now;
            DataTable dtSp2 = dao40080.GetSP2Data(txtTradeDate.DateTimeValue);

            foreach (DataRow dr in dt.Rows) {
               string cpType = dr["op_type"].AsString();
               if (cpType == "I") {
                  dr["op_type"] = "I";
               } else {
                  if (dr["sp2_adj_code"].AsString() == dr["sp2_adj_code_org"].AsString() && dr["sp2_span_code"].AsString() == dr["sp2_span_code_org"].AsString()
                     && dr["sp2_value_date"].AsString() == dr["sp2_value_date_org"].AsString()) {
                     dr["op_type"] = " ";
                  } else {
                     dr["op_type"] = "U";
                  }
               }

               if (string.IsNullOrEmpty(dr["op_type"].AsString())) continue;

               string ls_type = dr["sp1_type"].AsString();
               string ls_kind_id1 = dr["sp1_kind_id1"].AsString();
               string ls_kind_id2 = dr["sp1_kind_id2"].AsString();

               int ll_found = 0;
               if (dtSp2.Select("sp2_type ='" + ls_type + "' and sp2_kind_id1='" + ls_kind_id1 + "' and sp2_kind_id2='" + ls_kind_id2 + "'").Length <= 0) {
                  //新增
                  DataRow insertRow = dtSp2.NewRow();
                  insertRow["sp2_date"] = txtTradeDate.DateTimeValue;
                  insertRow["sp2_type"] = ls_type;
                  insertRow["sp2_kind_id1"] = ls_kind_id1;
                  insertRow["sp2_kind_id2"] = ls_kind_id2;
                  insertRow["sp2_value_date"] = dr["sp2_value_date"];
                  insertRow["sp2_adj_code"] = dr["sp2_adj_code"];
                  insertRow["sp2_span_code"] = dr["sp2_span_code"];
                  insertRow["sp2_osw_grp"] = dr["sp1_osw_grp"];
                  insertRow["sp2_w_time"] = ldt_w_time;
                  insertRow["sp2_w_user_id"] = GlobalInfo.USER_ID;
                  dtSp2.Rows.Add(insertRow);
               } else {
                  ll_found = dtSp2.Rows.IndexOf(dtSp2.Select("sp2_type ='" + ls_type + "' and sp2_kind_id1='" + ls_kind_id1 + "' and sp2_kind_id2='" + ls_kind_id2 + "'")[0]);
                  dtSp2.Rows[ll_found]["sp2_value_date"] = dr["sp2_value_date"];
                  dtSp2.Rows[ll_found]["sp2_adj_code"] = dr["sp2_adj_code"];
                  dtSp2.Rows[ll_found]["sp2_span_code"] = dr["sp2_span_code"];
                  dtSp2.Rows[ll_found]["sp2_osw_grp"] = dr["sp1_osw_grp"];
                  dtSp2.Rows[ll_found]["sp2_w_time"] = ldt_w_time;
                  dtSp2.Rows[ll_found]["sp2_w_user_id"] = GlobalInfo.USER_ID;
               }
               dr["sp2_adj_code_org"] = dr["sp2_adj_code"];

            }//foreach (DataRow dr in dt.Rows)

            //dw_2.update()
            ResultData myResultData = daoSP2.UpdateData(dtSp2);
            if (myResultData.Status == ResultStatus.Fail) {
               MessageDisplay.Error("更新資料庫錯誤! " , GlobalInfo.ErrorText);
               return ResultStatus.Fail;
            }

            //dw_1.update(將dw_1的op_type全改為" ")
            foreach (DataRow dr in dt.Rows) {
               dr["op_type"] = " ";
            }

            AfterSaveForPrint(gcMain , null , null , dtChange);
            MessageDisplay.Info("報表儲存完成!" , GlobalInfo.ResultText);

         } catch (Exception ex) {
            WriteLog(ex);
            MessageDisplay.Error("儲存錯誤" , GlobalInfo.ErrorText);
         }
         return ResultStatus.Success;
      }

      /// <summary>
      /// 將新增、刪除、變更的紀錄分別都列印或匯出出來(橫式A4)
      /// </summary>
      /// <param name="gridControl"></param>
      /// <param name="ChangedForAdded"></param>
      /// <param name="ChangedForDeleted"></param>
      /// <param name="ChangedForModified"></param>
      protected void AfterSaveForPrint(GridControl gridControl , DataTable ChangedForAdded ,
          DataTable ChangedForDeleted , DataTable ChangedForModified , bool IsHandlePersonVisible = true , bool IsManagerVisible = true) {
         GridControl gridControlPrint = GridHelper.CloneGrid(gridControl);

         string _ReportTitle = _ProgramID + "─" + _ProgramName + GlobalInfo.REPORT_TITLE_MEMO;
         ReportHelper reportHelper = new ReportHelper(gridControl , _ProgramID , _ReportTitle);
         CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4(); //橫向A4
         reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;

         reportLandscape.IsHandlePersonVisible = IsHandlePersonVisible;
         reportLandscape.IsManagerVisible = IsManagerVisible;
         reportHelper.Create(reportLandscape);

         if (ChangedForAdded != null)
            if (ChangedForAdded.Rows.Count != 0) {
               gridControlPrint.DataSource = ChangedForAdded;
               reportHelper.PrintableComponent = gridControlPrint;
               reportHelper.ReportTitle = _ReportTitle + "─" + "新增";

               reportHelper.Export(FileType.PDF , reportHelper.FilePath);
            }

         if (ChangedForDeleted != null)
            if (ChangedForDeleted.Rows.Count != 0) {
               DataTable dtTemp = ChangedForDeleted.Clone();

               int rowIndex = 0;
               foreach (DataRow dr in ChangedForDeleted.Rows) {
                  DataRow drNewDelete = dtTemp.NewRow();
                  for (int colIndex = 0 ; colIndex < ChangedForDeleted.Columns.Count ; colIndex++) {
                     drNewDelete[colIndex] = dr[colIndex , DataRowVersion.Original];
                  }
                  dtTemp.Rows.Add(drNewDelete);
                  rowIndex++;
               }

               gridControlPrint.DataSource = dtTemp.AsDataView();
               reportHelper.PrintableComponent = gridControlPrint;
               reportHelper.ReportTitle = _ReportTitle + "─" + "刪除";

               reportHelper.Export(FileType.PDF , reportHelper.FilePath);
            }

         if (ChangedForModified != null)
            if (ChangedForModified.Rows.Count != 0) {
               gridControlPrint.DataSource = ChangedForModified;
               reportHelper.PrintableComponent = gridControlPrint;
               reportHelper.ReportTitle = _ReportTitle + "─" + "變更";

               reportHelper.Export(FileType.PDF , reportHelper.FilePath);
            }
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnSave.Enabled = true;
         _ToolBtnRetrieve.Enabled = true;
         _ToolBtnPrintAll.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            ReportHelper _ReportHelper = new ReportHelper(gcMain , _ProgramID , this.Text);
            CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4();//設定為橫向列印
            reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            _ReportHelper.Create(reportLandscape);

            _ReportHelper.Print();//如果有夜盤會特別標註
            _ReportHelper.Export(FileType.PDF , _ReportHelper.FilePath);

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      //對價平上下檔數(SP1_TYPE)欄位做值轉換
      private void gvMain_CustomColumnDisplayText(object sender , CustomColumnDisplayTextEventArgs e) {
         if (e.Column.FieldName == "SP1_TYPE") {
            switch (Convert.ToString(e.Value)) {
               case "F":
                  e.DisplayText = "Future";
                  break;
               case "O":
                  e.DisplayText = "Option";
                  break;
               case "SV":
                  e.DisplayText = "Span-VSR";
                  break;
               case "SD":
                  e.DisplayText = "Span-Delta Per Spread Ratio";
                  break;
               case "SS":
                  e.DisplayText = "Spsn-Spread Credit";
                  break;
            }

         }
         //時間格式呈現微調
         if (e.Column.FieldName == "AMMD_W_TIME") {
            e.DisplayText = String.Format("{0:yyyy/MM/dd HH:mm:ss.fff}" , e.Value);
         }
      }

      /// <summary>
      /// 根據user所選列來確定search Date
      /// </summary>
      /// <param name="userSelect"></param>
      /// <returns></returns>
      private DateTime GetDateByUserSelect(string userSelect) {

         DateTime txtDate = new DateTime();
         Control[] formControls = this.Controls.Find("txtDate" + userSelect , true);
         TextDateEdit txt = (TextDateEdit)formControls[0];
         txtDate = txt.DateTimeValue;

         return txtDate;
      }

      /// <summary>
      /// 調整下拉選單改變時
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void ddlAdjust_EditValueChanged(object sender , EventArgs e) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();
         DataTable dtAdjust = (DataTable)gcMain.DataSource;
         if (dtAdjust == null) return;
         if (dtAdjust.Rows.Count == 0) return;

         switch (ddlAdjust.EditValue.AsString()) {
            case "none":
               foreach (DataRow dr in dtAdjust.Rows) {
                  dr["SP2_ADJ_CODE"] = " ";
                  dr["SP2_VALUE_DATE"] = DBNull.Value;
               }
               break;
            case "indes":
               for (int f = 0 ; f < dtAdjust.Rows.Count ; f++) {
                  DataRow dr = dtAdjust.Rows[f];
                  if (dr["APDK_PROD_SUBTYPE"].AsString() == "I") {
                     dr["SP2_ADJ_CODE"] = "Y";
                     if (dr["SP1_OSW_GRP"].AsString() == "1") {
                        dr["SP2_VALUE_DATE"] = txtDate1.Text;
                     } else {
                        dr["SP2_VALUE_DATE"] = txtDate2.Text;
                     }
                  } else {
                     dr["SP2_ADJ_CODE"] = " ";
                     dr["SP2_VALUE_DATE"] = DBNull.Value;
                  }
               }
               break;
            case "all":
               for (int f = 0 ; f < dtAdjust.Rows.Count ; f++) {
                  DataRow dr = dtAdjust.Rows[f];
                  dr["SP2_ADJ_CODE"] = "Y";
                  if (dr["SP1_OSW_GRP"].AsString() == "1") {
                     dr["SP2_VALUE_DATE"] = txtDate1.Text;
                  } else {
                     dr["SP2_VALUE_DATE"] = txtDate2.Text;
                  }
               }
               break;
            case "StcEtc":
               for (int f = 0 ; f < dtAdjust.Rows.Count ; f++) {
                  DataRow dr = dtAdjust.Rows[f];
                  if (dr["SP1_KIND_ID1"].AsString() == "STC" || dr["SP1_KIND_ID1"].AsString() == "ETC") {
                     dr["SP2_ADJ_CODE"] = "Y";
                     if (dr["SP1_OSW_GRP"].AsString() == "1") {
                        dr["SP2_VALUE_DATE"] = txtDate1.Text;
                     } else {
                        dr["SP2_VALUE_DATE"] = txtDate2.Text;
                     }
                  } else {
                     dr["SP2_ADJ_CODE"] = " ";
                     dr["SP2_VALUE_DATE"] = DBNull.Value;
                  }
               }
               break;
            case "1":
               for (int f = 0 ; f < dtAdjust.Rows.Count ; f++) {
                  DataRow dr = dtAdjust.Rows[f];
                  if (dr["SP1_OSW_GRP"].AsString() == "1") {
                     dr["SP2_ADJ_CODE"] = "Y";
                     dr["SP2_VALUE_DATE"] = txtDate1.Text;
                  } else {
                     dr["SP2_ADJ_CODE"] = " ";
                     dr["SP2_VALUE_DATE"] = DBNull.Value;
                  }
               }
               break;
            case "2":
               for (int f = 0 ; f < dtAdjust.Rows.Count ; f++) {
                  DataRow dr = dtAdjust.Rows[f];
                  if (dr["SP1_OSW_GRP"].AsString() == "5") {
                     dr["SP2_ADJ_CODE"] = "Y";
                     dr["SP2_VALUE_DATE"] = txtDate2.Text;
                  } else {
                     dr["SP2_ADJ_CODE"] = " ";
                     dr["SP2_VALUE_DATE"] = DBNull.Value;
                  }
               }
               break;
         }//switch (ddlAdjust.EditValue.AsString())

         gcMain.DataSource = dtAdjust;
      }

      private void gvMain_CellValueChanging(object sender , CellValueChangedEventArgs e) {
         GridView gv = sender as GridView;

         DataTable dt = (DataTable)gcMain.DataSource;
         if (e.Column.Name == "SP2_ADJ_CODE") {
            if (e.Value.AsString() != "Y") {
               gv.SetRowCellValue(e.RowHandle , "SP2_VALUE_DATE" , DBNull.Value);
            } else {
               switch (gv.GetRowCellValue(e.RowHandle , "SP1_OSW_GRP").AsString()) {
                  case "1":
                     gv.SetRowCellValue(e.RowHandle , "SP2_VALUE_DATE" , txtDate1.Text);
                     break;
                  case "5":
                     gv.SetRowCellValue(e.RowHandle , "SP2_VALUE_DATE" , txtDate2.Text);
                     break;
               }
            }
         }
      }

      private string wf_set_valid_date(int ai_row) {
         string osw_grp = gvMain.GetRowCellValue(ai_row , "OSW_GRP").AsString();
         if (gvMain.GetRowCellValue(ai_row , "SP2_ADJ_CODE").AsString() == "Y") {
            if (osw_grp == "1") {
               if (txtDate1.Text == "1901/01/01") {
                  MessageDisplay.Error("請先輸入" + labG1.Text , GlobalInfo.ErrorText);
                  return "N";
               }
               gvMain.SetRowCellValue(ai_row , "ISSUE_BEGIN_YMD" , txtDate1.DateTimeValue.ToString("yyyyMMdd"));
            }
            if (osw_grp == "5") {
               if (txtDate2.Text == "1901/01/01") {
                  MessageDisplay.Error("請先輸入" + labG2.Text , GlobalInfo.ErrorText);
                  return "N";
               }
               gvMain.SetRowCellValue(ai_row , "ISSUE_BEGIN_YMD" , txtDate2.DateTimeValue.ToString("yyyyMMdd"));
            }
         } else {
            if (gvMain.GetRowCellValue(ai_row , "ISSUE_BEGIN_YMD").AsString() != null) {
               gvMain.SetRowCellValue(ai_row , "ISSUE_BEGIN_YMD" , nullYmd);
            }
         }
         return "";
      }

      /// <summary>
      /// 決定哪些欄位無法編輯的事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         string adjCode = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["SP2_ADJ_CODE"]) == null ? "0" :
              gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["SP2_ADJ_CODE"]).AsString();

         if (gv.FocusedColumn.FieldName == "SP2_ADJ_CODE") {
            e.Cancel = false;
         } else {
            e.Cancel = true;
         }

         if (gv.FocusedColumn.FieldName == "SP2_VALUE_DATE") {
            if (adjCode != "Y") {
               e.Cancel = true;
            } else {
               e.Cancel = false;
            }
         }
      }
   }
}