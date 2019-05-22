using BaseGround;
using BaseGround.Report;
using BaseGround.Shared;
using BusinessObjects;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;

/// <summary>
/// Winni, 2019/3/19
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {

   /// <summary>
   /// 49060 國外保證金資料輸入
   /// </summary>
   public partial class W49060 : FormParent {

      DataTable retDt; //存retrieve後的datatable   
      RepositoryItemLookUpEdit lupTradekindId;
      private D49060 dao49060;

      #region 一般交易查詢條件縮寫
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
      #endregion

      public W49060(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         dao49060 = new D49060();
         retDt = new DataTable();
         lupTradekindId = new RepositoryItemLookUpEdit();
      }

      protected override ResultStatus Open() {
         base.Open();
         try {
            txtStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/01/01");
            txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;

#if DEBUG
            //winni test
            txtStartDate.DateTimeValue = DateTime.ParseExact("2019/03/01" , "yyyy/MM/dd" , null);
            txtEndDate.DateTimeValue = DateTime.ParseExact("2019/03/28" , "yyyy/MM/dd" , null);
            this.Text += "(開啟測試模式),Date=2019/03/01~2019/03/28";
#endif

            //交易所+商品清單
            DataTable dtTradekindId = new MGT8().ListDataByMGT8();
            Extension.SetColumnLookUp(lupTradekindId , dtTradekindId , "MGT8_F_ID" , "CP_DISPLAY" , TextEditStyles.DisableTextEditor , "");
            gcMain.RepositoryItems.Add(lupTradekindId);
            MG8_F_ID.ColumnEdit = lupTradekindId;

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
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
            DataTable dt = new MG8().ListData(StartDate , EndDate);
            if (dt.Rows.Count <= 0) {
               MessageDisplay.Info("無任何資料");
            } else {
   
               retDt = dt.Clone();
               foreach (DataRow r in dt.Rows) {
                  retDt.ImportRow(r);
               }

               //設定grid裡的 date format
               RepositoryItemTextEdit effectYmd = new RepositoryItemTextEdit();
               effectYmd.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
               effectYmd.Mask.EditMask = "[0-9]{4}/(((0[13578]|(10|12))/(0[1-9]|[1-2][0-9]|3[0-1]))|(02/(0[1-9]|[1-2][0-9]))|((0[469]|11)/(0[1-9]|[1-2][0-9]|30)))";
               effectYmd.Mask.UseMaskAsDisplayFormat = true;

               gcMain.RepositoryItems.Add(effectYmd);
               gvMain.Columns["MG8_EFFECT_YMD"].ColumnEdit = effectYmd;

               RepositoryItemTextEdit issueYmd = new RepositoryItemTextEdit();
               issueYmd.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
               issueYmd.Mask.EditMask = "[0-9]{4}/(((0[13578]|(10|12))/(0[1-9]|[1-2][0-9]|3[0-1]))|(02/(0[1-9]|[1-2][0-9]))|((0[469]|11)/(0[1-9]|[1-2][0-9]|30)))";
               issueYmd.Mask.UseMaskAsDisplayFormat = true;

               gcMain.RepositoryItems.Add(issueYmd);
               gvMain.Columns["MG8_ISSUE_YMD"].ColumnEdit = issueYmd;

            }
          
            //設定gvMain
            gcMain.Visible = true;
            //gvMain.Columns.Clear();
            gcMain.DataSource = dt;

            //gvMain.BestFitColumns();
            GridHelper.SetCommonGrid(gvMain);
            gcMain.Focus();

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus Save(PokeBall pokeBall) {
         try {
            DataTable dt = (DataTable)gcMain.DataSource; //mg8
            gvMain.CloseEditor();
            gvMain.UpdateCurrentRow();

            DataTable dtChange = dt.GetChanges();
            DataTable dtForAdd = dt.GetChanges(DataRowState.Added);
            DataTable dtForModified = dt.GetChanges(DataRowState.Modified);
            DataTable dtForDeleted = dt.GetChanges(DataRowState.Deleted);

            #region save
            foreach (DataRow dr in dt.Rows) {
               switch (dr.RowState) {
                  case DataRowState.Added:
                  case DataRowState.Modified:
                     dr["MG8_W_TIME"] = DateTime.Now;
                     dr["MG8_W_USER_ID"] = GlobalInfo.USER_ID;
                     dr["MG8_EFFECT_YMD"] = dr["MG8_EFFECT_YMD"].AsString().Replace("/","");
                     dr["MG8_ISSUE_YMD"] = dr["MG8_ISSUE_YMD"].AsString().Replace("/" , "");
                     break;
                  case DataRowState.Unchanged:
                     if (dr["MG8_W_TIME"] == null) {
                        dr["MG8_W_TIME"] = " ";
                     }
                     if (dr["MG8_W_USER_ID"] == null) {
                        dr["MG8_W_USER_ID"] = " ";
                     }
                     break;
               }
            }

            dtChange = dt.GetChanges();
            ResultData res = new MG8().UpdateData(dtChange);
            if (res.Status == ResultStatus.Fail) {
               MessageDisplay.Error("儲存失敗");
               return ResultStatus.Fail;
            } else {
               //save成功才寫異動LOG: 紀錄異動前後的值
               foreach (DataRow dr in dt.Rows) {
                  if (dr.RowState == DataRowState.Modified) {
                     string effectYmd = dr["MG8_EFFECT_YMD"].AsString().Replace("/" , "");
                     string fId = dr["MG8_F_ID"].AsString();

                     DataView dv = retDt.AsDataView();
                     dv.Sort = "MG8_EFFECT_YMD,MG8_F_ID";
                     object[] filter = new object[2];
                     filter[0] = effectYmd;
                     filter[1] = fId;

                     int found = dv.Find(filter);
                     if (found < 0) continue;
                     for (int w = 2 ; w <= 5 ; w++) {
                        if (dr[w].AsString() != retDt.Rows[found][w].AsString()) { //沒有轉string比對會變成true(?)
                           string befChange = dr[w].AsString();
                           string aftChange = retDt.Rows[found][w].AsString();
                           WriteLog(string.Format("變更後:{0},原始:{1}" , aftChange , befChange) , "Info" , "U");
                        }
                     }
                  }
               }//foreach (DataRow dr in dt.Rows) {
            }
            #endregion

            //呼叫SP
            foreach (DataRow dr in dt.Rows) {
               int pos = 0;
               if (dr.RowState != DataRowState.Added) continue;

               string effectYmd = dr["MG8_EFFECT_YMD"].AsString();
               string fId = dr["MG8_F_ID"].AsString();

               #region 寫txt檔(for insert)
               DataTable dtTxt = dao49060.GetTxtDataById(effectYmd , fId);
               DataRow drTxt = dtTxt.Rows[pos];
               string fExchange = drTxt["MGT8_F_EXCHANGE"].AsString();
               string issueYmd = dr["MG8_ISSUE_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
               decimal mg8Im_1 = drTxt["MG8_IM"].AsDecimal();
               //decimal mg8Im_2 = dtTxt.Rows[pos + 1]["mg8_im"].AsDecimal();
               string fName = drTxt["MGT8_F_NAME"].AsString();
               string currencyName = drTxt["COD_CURRENCY_NAME"].AsString();
               string amtType = drTxt["MGT8_AMT_TYPE"].AsString();
               decimal imRate = drTxt["IM_RATE"].AsDecimal();

               string txt = string.Format("{0}於{1}公告" , fExchange , issueYmd);
               string flag = "";
               if (dtTxt.Rows.Count == 2) {
                  if (mg8Im_1 < dtTxt.Rows[1]["MG8_IM"].AsDecimal()) {
                     flag = "調降";
                  } else {
                     flag = "調升";
                  }
               }

               txt += string.Format("{0}{1}保證金，原始保證金" , flag , fName);

               if (!string.IsNullOrEmpty(flag)) {
                  txt += string.Format("由{0}" , currencyName);
                  if (amtType == "A") {
                     txt += string.Format("{0:N0}{1}至{2:N0}" , dtTxt.Rows[1]["MG8_IM"].AsDecimal() , flag , mg8Im_1);
                  } else {
                     txt += string.Format("{0:0.00%}{1}至{2:0.00%}" , dtTxt.Rows[1]["MG8_IM"].AsDecimal() , flag , mg8Im_1);
                  }
                  txt += string.Format("，調幅{0:0.00%}，" , imRate);
               } else {
                  txt += currencyName;
                  if (amtType == "A") {
                     txt += string.Format("{0:N0}" , mg8Im_1);
                  } else {
                     txt += string.Format("{0:0.00%}" , mg8Im_1);
                  }
                  txt += "，";
               }

               txt += string.Format("自{0}起生效。{1}" , issueYmd , Environment.NewLine);

               string fileName = _ProgramID + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".txt";
               string filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , fileName);

               bool IsSuccess = ToText(txt , filePath , System.Text.Encoding.GetEncoding(950));
               if (!IsSuccess) {
                  MessageDisplay.Error("文字檔「" + filePath + "」Open檔案錯誤!");
                  return ResultStatus.Fail;
               }
               #endregion

               //轉ci.MG8D (執行ci.sp_H_stt_MG8D)
               string resInsert = dao49060.ExecuteStoredProcedure(effectYmd , fId , "I");
               if (resInsert != "0") {
                  MessageDisplay.Error("執行SP(ci.sp_H_stt_MG8D)錯誤!");
                  WriteLog("執行SP(ci.sp_H_stt_MG8D)-(I)錯誤!" , "Error" , "Z" , false);
               }
               WriteLog("執行ci.sp_H_stt_MG8D(I)" , "Info" , "X" , false);
            }//foreach (DataRow dr in dt.Rows)

            //刪除資料
            if (dtForDeleted != null) {
               foreach (DataRow dr in dtForDeleted.Rows) {
                  //轉ci.MG8D
                  string effectYmd = dr["MG8_EFFECT_YMD" , DataRowVersion.Original].AsString();
                  string fId = dr["MG8_F_ID" , DataRowVersion.Original].AsString();

                  string resDelete = dao49060.ExecuteStoredProcedure(effectYmd , fId , "D");
                  if (resDelete != "0") {
                     MessageDisplay.Error("執行SP(ci.sp_H_stt_MG8D)錯誤!");
                     WriteLog("執行SP(ci.sp_H_stt_MG8D)-(D)錯誤!" , "Error" , "Z" , false);
                  }
                  WriteLog("執行ci.sp_H_stt_MG8D(D)" , "Info" , "X" , false);
               }
            }//if (dtForDeleted != null)

            AfterSaveForPrint(gcMain , dtForAdd , dtForDeleted , dtForModified);

         } catch (Exception ex) {
            WriteLog(ex);
         }
         _IsPreventFlowPrint = true; //不要自動列印
         return ResultStatus.Success;
      }

      /// <summary>
      /// 將新增、刪除、變更的紀錄分別都另存成PDF(橫式A4)
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

      protected override ResultStatus Print(ReportHelper reportHelper) {
         try {
            ReportHelper _ReportHelper = new ReportHelper(gcMain , _ProgramID , this.Text);
            CommonReportLandscapeA4 reportLandscape = new CommonReportLandscapeA4(); //橫向A4

            reportLandscape.printableComponentContainerMain.PrintableComponent = gcMain;
            reportLandscape.IsHandlePersonVisible = false;
            reportLandscape.IsManagerVisible = false;
            _ReportHelper.Create(reportLandscape);

            _ReportHelper.Print();

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
      }

      protected override ResultStatus InsertRow() {
         DataTable dt = (DataTable)gcMain.DataSource;
         gvMain.AddNewRow();

         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MG8_EFFECT_YMD"] , DateTime.Now.ToString("yyyy/MM/dd"));
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MG8_ISSUE_YMD"] , DateTime.Now.ToString("yyyy/MM/dd"));
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MG8_CM"] , 0);
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MG8_MM"] , 0);
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["MG8_IM"] , 0);
         gvMain.SetRowCellValue(GridControl.NewItemRowHandle , gvMain.Columns["IS_NEWROW"] , 1);

         gvMain.Focus();
         gvMain.FocusedColumn = gvMain.Columns[0];

         return ResultStatus.Success;
      }

      protected override ResultStatus DeleteRow() {
         base.DeleteRow(gvMain);
         return ResultStatus.Success;
      }

      /// <summary>
      /// write string to txt
      /// </summary>
      /// <param name="source"></param>
      /// <param name="filePath"></param>
      /// <param name="encoding">System.Text.Encoding.GetEncoding(950)</param>
      /// <returns></returns>
      protected bool ToText(string source , string filePath , System.Text.Encoding encoding) {
         try {
            FileStream fs = new FileStream(filePath , FileMode.Create);
            StreamWriter str = new StreamWriter(fs , encoding);
            str.Write(source);

            str.Flush();
            str.Close();
            return true;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return false;
      }

      #region GridControl事件

      /// <summary>
      /// 決定哪些欄位無法編輯的事件
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void gvMain_ShowingEditor(object sender , CancelEventArgs e) {
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"]) == null ? "0" :
              gv.GetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"]).ToString();

         if (gv.IsNewItemRow(gv.FocusedRowHandle) || Is_NewRow == "1") {
            e.Cancel = false;
            gv.SetRowCellValue(gv.FocusedRowHandle , gv.Columns["IS_NEWROW"] , 1);
         }
         //編輯狀態時,設定可以編輯的欄位( e.Cancel = false 等於可以編輯)
         else if (gv.FocusedColumn.FieldName == "MG8_EFFECT_YMD" || gv.FocusedColumn.FieldName == "MG8_F_ID") {
            e.Cancel = true;
         } else {
            e.Cancel = false;
         }
      }

      private void gvMain_RowCellStyle(object sender , RowCellStyleEventArgs e) {
         //要用RowHandle不要用FocusedRowHandle
         GridView gv = sender as GridView;
         string Is_NewRow = gv.GetRowCellValue(e.RowHandle , gv.Columns["IS_NEWROW"]) == null ? "0" :
                   gv.GetRowCellValue(e.RowHandle , gv.Columns["IS_NEWROW"]).ToString();

         //描述每個欄位,在is_newRow時候要顯示的顏色
         //當該欄位不可編輯時,設定為灰色 Color.FromArgb(192,192,192)
         //當該欄位不可編輯時,AllowFocus為false(PB的wf_set_order方法)
         switch (e.Column.FieldName) {
            case ("MG8_EFFECT_YMD"):
            case ("MG8_F_ID"):
               e.Column.OptionsColumn.AllowFocus = Is_NewRow == "1" ? true : false;
               e.Appearance.BackColor = Is_NewRow == "1" ? Color.White : Color.FromArgb(192 , 192 , 192);
               break;
            default:
               e.Appearance.BackColor = Color.White;
               break;
         }//switch (e.Column.FieldName) 
      }

      //TODO: 目前會無限loop 之後來優化
      //private void gvMain_CellValueChanged(object sender , CellValueChangedEventArgs e) {
      //   GridView gv = sender as GridView;
      //   //bool point = false;
      //   if (gv == null) return;
      //   int index = e.RowHandle;
      //   //if (point) return;
      //   if (e.Column.Name == "MG8_CM" || e.Column.Name == "MG8_MM" || e.Column.Name == "MG8_IM") {
      //      //此三個欄位若有變動數值即顯示#.####格式
      //      if (gv.GetRowCellValue(index , gv.Columns["MG8_CM"]).AsString() != retDt.Rows[index]["MG8_CM"].AsString()) {
      //         decimal tmpValue = string.Format("{0:0.0000}" , e.Value).AsDecimal();
      //         gv.SetRowCellValue(index , gv.Columns["MG8_CM"] , tmpValue);
      //         //point = true;
      //      }
      //      //if (gv.GetRowCellValue(e.RowHandle , gv.Columns["MG8_MM"]).AsString() != retDt.Rows[e.RowHandle]["MG8_MM"].AsString()) {
      //      //   decimal tmpValue = string.Format("{0:0.0000}" , e.Value).AsDecimal();
      //      //   gv.SetRowCellValue(e.RowHandle , gv.Columns["MG8_MM"] , tmpValue);
      //      //   //point = true;
      //      //}
      //      //if (gv.GetRowCellValue(e.RowHandle , gv.Columns["MG8_IM"]).AsString() != retDt.Rows[e.RowHandle]["MG8_IM"].AsString()) {
      //      //   decimal tmpValue = string.Format("{0:0.0000}" , e.Value).AsDecimal();
      //      //   gv.SetRowCellValue(e.RowHandle , gv.Columns["MG8_IM"] , tmpValue);
      //      //   //point = true;
      //      //}

      //   }
      //}
      #endregion

   }
}