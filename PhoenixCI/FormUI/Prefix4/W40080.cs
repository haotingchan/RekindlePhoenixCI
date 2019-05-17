﻿using System;
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

namespace PhoenixCI.FormUI.Prefix4 {
   public partial class W40080 : FormParent {
      private D40080 dao40080;
      private int countGroup = 3;//設定群組數

      public W40080(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao40080 = new D40080();

         adjustmentRadioGroup.SelectedIndex = 0;
      }

      protected override ResultStatus Open() {
         base.Open();

         //日期
         txtTradeDate.DateTimeValue = DateTime.Now;
         txtDate1.DateTimeValue = DateTime.Now;
         txtDate2.DateTimeValue = DateTime.Now;

         //觀察/調整 RadioGroup
         RadioGroupItem item1 = new RadioGroupItem();
         item1.Description = "　　";
         item1.Value = " ";
         RadioGroupItem item2 = new RadioGroupItem();
         item2.Description = "　　";
         item2.Value = "Y";

         RepositoryItemRadioGroup repositoryItemRadioGroup = new RepositoryItemRadioGroup();
         repositoryItemRadioGroup.Items.Add(item1);
         repositoryItemRadioGroup.Items.Add(item2);
         repositoryItemRadioGroup.Columns = 2;
         SP2_ADJ_CODE.ColumnEdit = repositoryItemRadioGroup;
         SP2_ADJ_CODE.ColumnEdit.Appearance.TextOptions.HAlignment = HorzAlignment.Center;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve() {
         base.Retrieve(gcMain);
         DataTable dt = dao40080.GetData(txtTradeDate.DateTimeValue);
         DataTable dtSp2 = dao40080.GetSP2Data(txtTradeDate.DateTimeValue);

         int ll_found = 0; ;

         //Group1
         if (dt.Select("sp1_osw_grp='1' and sp2_value_date is not null").Length != 0) {
            ll_found = dt.Rows.IndexOf(dt.Select("sp1_osw_grp='1' and sp2_value_date is not null")[0]) + 1;
         }

         if (ll_found > 0) {
            string sp2ValueDate = dt.Rows[ll_found - 1]["sp2_value_date"].AsDateTime().ToString("yyyy/MM/dd");
            txtDate1.Text = sp2ValueDate;
         } else {
            string tmpDate = PbFunc.f_get_ocf_next_n_day(txtTradeDate.DateTimeValue , 1).ToString("yyyy/MM/dd");
            txtDate1.Text = tmpDate;
         }

         //Group2
         if (dt.Select("sp1_osw_grp='5' and sp2_value_date is not null").Length != 0) {
            ll_found = dt.Rows.IndexOf(dt.Select("sp1_osw_grp='5' and sp2_value_date is not null")[0]) + 1;
         }

         if (ll_found > 0) {
            string sp2ValueDate = dt.Rows[ll_found - 1]["sp2_value_date"].AsDateTime().ToString("yyyy/MM/dd");
            txtDate2.Text = sp2ValueDate;
         } else {
            string tmpDate = PbFunc.f_get_ocf_next_n_day(txtTradeDate.DateTimeValue , 1).ToString("yyyy/MM/dd");
            txtDate2.Text = tmpDate;
         }

         gvMain.Columns["SP1_OSW_GRP"].Group();
         gvMain.Columns["SP1_CHANGE_RANGE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         gvMain.Columns["SP1_CHANGE_RANGE"].DisplayFormat.FormatString = "P";

         gcMain.DataSource = dt;
         GridHelper.SetCommonGrid(gvMain);
         gvMain.ExpandAllGroups();
         gvMain.BestFitColumns();

         return ResultStatus.Success;
      }

      protected override ResultStatus Save(PokeBall poke) {
         gvMain.CloseEditor();
         gvMain.UpdateCurrentRow();
         ResultStatus resultStatus = ResultStatus.Fail;
         try {
            //先照翻

            int ll_found = 0;
            string ls_kind_id1, ls_kind_id2, ls_type;
            DateTime ldt_w_time = DateTime.Now;

            DataTable dt = dao40080.GetData(txtTradeDate.DateTimeValue);
            foreach (DataRow dr in dt.Rows) {


            }//foreach (DataRow dr in dt.Rows)




            //            for      i = 1 to dw_1.rowcount()

            //      dw_1.setitem(i , "op_type" , dw_1.getitemstring(i , "cp_op_type"))

            //      if    dw_1.getitemstring(i , "op_type") = ' '   then

            //            continue

            //      end   if


            //      ls_type = dw_1.getitemstring(i , "sp1_type")

            //      ls_kind_id1 = dw_1.getitemstring(i , "sp1_kind_id1")

            //      ls_kind_id2 = dw_1.getitemstring(i , "sp1_kind_id2")

            //      ll_found = dw_2.find("sp2_type ='" + ls_type + "' and sp2_kind_id1='" + ls_kind_id1 + "' and sp2_kind_id2='" + ls_kind_id2 + "'" , 1 , dw_2.rowcount())
            //      //新增		
            //            if    ll_found = 0 then
            //            //if		dw_1.getitemstring(i,"adj_code") = 'Y' or dw_1.getitemstring(i,"span_code")	= 'Y' then
            //            dw_2.insertrow(0)

            //                  ll_found = dw_2.rowcount()

            //                  dw_2.setitem(ll_found , "sp2_date" , date(em_date.text))

            //                  dw_2.setitem(ll_found , "sp2_type" , ls_type)

            //                  dw_2.setitem(ll_found , "sp2_kind_id1" , ls_kind_id1)

            //                  dw_2.setitem(ll_found , "sp2_kind_id2" , ls_kind_id2)
            //            //end	if
            //            end   if
            //      //刪除
            //      dw_2.setitem(ll_found , "sp2_value_date" , dw_1.getitemdatetime(i , "sp2_value_date"))

            //      dw_2.setitem(ll_found , "sp2_adj_code" , dw_1.getitemstring(i , "adj_code"))

            //      dw_2.setitem(ll_found , "sp2_span_code" , dw_1.getitemstring(i , "span_code"))

            //      dw_2.setitem(ll_found , "sp2_osw_grp" , dw_1.getitemstring(i , "sp1_osw_grp"))

            //      dw_2.setitem(ll_found , "sp2_w_time" , ldt_w_time)

            //      dw_2.setitem(ll_found , "sp2_w_user_id" , gs_user_id)

            //      dw_1.setitem(i , "adj_code_org" , dw_1.getitemstring(i , "adj_code"))
            //next





            //DataTable dt = (DataTable)gcMain.DataSource;
            //DataTable dtChange = dt.GetChanges();

            //if (dtChange == null) {
            //   MessageBox.Show("沒有變更資料,不需要存檔!" , "注意" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            //   return ResultStatus.FailButNext;
            //}

         } catch (Exception ex) {
            throw ex;
         }
         return resultStatus;
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
      private void gvMain_CustomColumnDisplayText(object sender , DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e) {
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
                  e.DisplayText = "Span- Delta Per Spread Ratio";
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

      private void adjustmentRadioGroup_SelectedIndexChanged(object sender , EventArgs e) {
         RadioGroup radios = sender as RadioGroup;

         #region Set SPAN CODE 
         switch (radios.Properties.Items[radios.SelectedIndex].Value.ToString()) {
            case "Clear": {
                  for (int i = 0 ; i < gvMain.DataRowCount ; i++) {
                     gvMain.SetRowCellValue(i , "MG2_SPAN_CODE" , " ");
                  }
                  break;
               }
            case "AllSelect": {
                  for (int i = 0 ; i < gvMain.DataRowCount ; i++) {
                     gvMain.SetRowCellValue(i , "MG2_SPAN_CODE" , "Y");
                  }
                  break;
               }
            case "1": {
                  for (int i = 0 ; i < gvMain.DataRowCount ; i++) {
                     if (gvMain.GetRowCellValue(i , "GROUP_TYPE").AsString() == "1") {
                        gvMain.SetRowCellValue(i , "MG2_SPAN_CODE" , "Y");
                     } else {
                        gvMain.SetRowCellValue(i , "MG2_SPAN_CODE" , " ");
                     }
                  }
                  break;
               }
            case "2": {
                  for (int i = 0 ; i < gvMain.DataRowCount ; i++) {
                     if (gvMain.GetRowCellValue(i , "GROUP_TYPE").AsString() == "2") {
                        gvMain.SetRowCellValue(i , "MG2_SPAN_CODE" , "Y");
                     } else {
                        gvMain.SetRowCellValue(i , "MG2_SPAN_CODE" , " ");
                     }
                  }
                  break;
               }
            case "3": {
                  for (int i = 0 ; i < gvMain.DataRowCount ; i++) {
                     if (gvMain.GetRowCellValue(i , "GROUP_TYPE").AsString() == "3") {
                        gvMain.SetRowCellValue(i , "MG2_SPAN_CODE" , "Y");
                     } else {
                        gvMain.SetRowCellValue(i , "MG2_SPAN_CODE" , " ");
                     }
                  }
                  break;
               }
            case "ETF": {
                  for (int i = 0 ; i < gvMain.DataRowCount ; i++) {
                     if (gvMain.GetRowCellValue(i , "MG1_PROD_SUBTYPE").AsString() == "S") {
                        gvMain.SetRowCellValue(i , "MG2_SPAN_CODE" , "Y");
                     } else {
                        gvMain.SetRowCellValue(i , "MG2_SPAN_CODE" , " ");
                     }
                  }
                  break;
               }
            case "Index": {
                  for (int i = 0 ; i < gvMain.DataRowCount ; i++) {
                     if (gvMain.GetRowCellValue(i , "MG1_PROD_SUBTYPE").AsString() == "I") {
                        gvMain.SetRowCellValue(i , "MG2_SPAN_CODE" , "Y");
                     } else {
                        gvMain.SetRowCellValue(i , "MG2_SPAN_CODE" , " ");
                     }
                  }
                  break;
               }
         }
         #endregion
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

      private void adjustmentRadioGroup_SelectedIndexChanged_1(object sender , EventArgs e) {

      }
   }
}