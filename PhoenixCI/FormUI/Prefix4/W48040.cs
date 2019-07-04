using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// ken,2019/3/5
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 個別契約風險價格係數狀況表
   /// </summary>
   public partial class W48040 : FormParent {
      private D48040 dao48040;
      protected bool flagTest = true;
      protected DataTable dtKind;

      public W48040(string programID , string programName) : base(programID , programName) {
         try {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;
            GridHelper.SetCommonGrid(gvKind);
            GridHelper.SetCommonGrid(gvDate);

            txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;

            dao48040 = new D48040();

            DataTable dtSubType = new COD().ListByCol("48010" , "PDK_SUBTYPE" , "全選" , "%");
            cbxSubType.SetDataTable(dtSubType , "COD_ID" , "COD_DESC" , TextEditStyles.DisableTextEditor);

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }

      /// <summary>
      /// 視窗啟動時,設定一些UI元件初始值
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Open() {
         try {
            base.Open();

            //1.找HPDK的MAX日期,如果有值則直接帶入txtEndDate
            string tempDate = dao48040.GetMaxDate(txtEndDate.Text);
            if (!string.IsNullOrEmpty(tempDate)) {
               txtEndDate.Text = tempDate;
            }

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return ResultStatus.Fail;
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

         _ToolBtnRetrieve.Enabled = false;//畫面查詢條件選定之後,按下此按鈕,讀取資料 to Grid
         _ToolBtnRun.Enabled = false;//執行,跑job專用按鈕

         _ToolBtnImport.Enabled = false;//匯入
         _ToolBtnExport.Enabled = true;//匯出,格式可以為 pdf/xls/txt/csv, 看功能
         _ToolBtnPrintAll.Enabled = false;//列印

         //ken,初始畫面直接撈大量資料顯示,如果放在load or open事件會卡很久才出現畫面,放到這裡才對
         //2.直接觸發兩個grid data update
         cbxSubType.ItemIndex = 1;//setup gvKind filter
         btnFirstFilter_Click(btnFirstFilter , null);//update 2 grid

         return ResultStatus.Success;
      }

      /// <summary>
      /// 按下[匯出]按鈕時
      /// </summary>
      /// <returns></returns>
      protected override ResultStatus Export() {
         //1.check
         if (gvKind.DataRowCount <= 0) {
            MessageDisplay.Normal("選擇的日期必須有契約資訊,請重新選擇日期");
            return ResultStatus.Fail;
         }

         //1.1檢查最少必須勾選一筆商品
         gvDate.CloseEditor();//ken,一定要先呼叫這兩個函數,最後點選的那筆才會被記錄起來
         gvDate.UpdateCurrentRow();//ken,一定要先呼叫這兩個函數,最後點選的那筆才會被記錄起來
         gvKind.CloseEditor();//ken,一定要先呼叫這兩個函數,最後點選的那筆才會被記錄起來
         gvKind.UpdateCurrentRow();//ken,一定要先呼叫這兩個函數,最後點選的那筆才會被記錄起來
         DataTable dtTemp = (DataTable)gcKind.DataSource;
         bool haveKind = false;
         foreach (DataRow drKind in dtTemp.Rows) {
            if (drKind["CPR_SELECT"].AsString() == "Y") {
               haveKind = true;
               break;
            }
         }

         if (!haveKind) {
            MessageDisplay.Normal("必須勾選一筆契約");
            return ResultStatus.Fail;
         }

         if (chkModel.CheckedItemsCount < 1) {
            MessageDisplay.Error("請至少勾選一種指標種類" , GlobalInfo.ErrorText);
            return ResultStatus.Fail;
         }

         //1.2檢查統計資料是否已經轉入完畢
         string FinishedJob = PbFunc.f_get_jsw(_ProgramID , "E" , txtEndDate.Text);
         if (FinishedJob != "Y") {
            DialogResult chooseResult = MessageDisplay.Choose(string.Format("{0} 統計資料未轉入完畢,是否要繼續?" , txtEndDate.Text) , MessageBoxDefaultButton.Button2 , GlobalInfo.QuestionText);
            if (chooseResult != DialogResult.Yes) {
               return ResultStatus.Fail;
            }
         }

         string kindId = "";
         Workbook workbook = new Workbook();

         try {
            //2.開始轉出資料
            panFilter.Enabled = panSecond.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Refresh();

            DataTable dtDate = (DataTable)gcDate.DataSource;
            string startDate = dtDate.Rows[dtDate.Rows.Count - 1]["SDATE"].AsDateTime().ToString("yyyyMMdd");
            string endDate = dtDate.Rows[dtDate.Rows.Count - 1]["EDATE"].AsDateTime().ToString("yyyyMMdd");

            #region 指標:SMA,EWMA,MAX
            foreach (CheckedListBoxItem item in chkModel.Items) {
               if (item.CheckState == CheckState.Unchecked) continue;

               string modelType = "";
               string modelName = "";
               switch (item.Value) {
                  case "chkSma":
                     modelType = "S";
                     modelName = "SMA";
                     break;
                  case "chkEwma":
                     modelType = "E";
                     modelName = "EWMA";
                     break;
                  case "chkMax":
                     modelType = "M";
                     modelName = "MAX";
                     break;
               }

               #region 商品
               //每一個商品都會產生一個excel檔案
               foreach (DataRow drKind in dtTemp.Rows) {
                  if (drKind["CPR_SELECT"].AsString() != "Y")
                     continue;

                  //2.0 get some column data
                  string effDate = drKind["cpr_effective_date"].AsDateTime().ToString("yyyy/MM/dd");
                  Decimal lastRiskRate = drKind["last_risk_rate"].AsDecimal(0);
                  Decimal riskRateOrg = drKind["cpr_price_risk_rate_org"].AsDecimal(0);

                  kindId = drKind["CPR_KIND_ID"].AsString();

                  //2.1 copy template xlsx to target path and open
                  string excelDestinationPath = CopyExcelTemplateFile2(_ProgramID , modelName , kindId);
                  workbook.LoadDocument(excelDestinationPath);
                  Worksheet worksheet = workbook.Worksheets["RawData"];

                  //2.3寫入檔頭[M1:M6]
                  worksheet.Cells[0 , 12].Value = kindId;//商品
                  worksheet.Cells[1 , 12].Value = drKind["cpr_effective_date"].AsDateTime();//日期,最近一次調整日期
                  if (drKind["cpr_price_risk_rate"] != DBNull.Value)
                     worksheet.Cells[2 , 12].Value = drKind["cpr_price_risk_rate"].AsDecimal();//百分比,現行最小風險價格係數
                  if (drKind["last_risk_rate"] != DBNull.Value)
                     worksheet.Cells[3 , 12].Value = drKind["last_risk_rate"].AsDecimal();//百分比,最近一次修改前之最小風險價格係數
                  if (drKind["risk_interval"] != DBNull.Value)
                     worksheet.Cells[4 , 12].Value = drKind["risk_interval"].AsDecimal();//百分比,最小風險價格係數級距
                  worksheet.Cells[5 , 12].Value = DateTime.Today;//日期,作業日期


                  //2.4讀取子table data (mg1_ymd/mg1_risk/mg1_min_risk)
                  DataTable dtSingleKind = dao48040.ListKindByKindId(kindId , startDate , endDate , modelType);
                  if (dtSingleKind.Rows.Count <= 0) {
                     File.Delete(excelDestinationPath);
                     //workbook.SaveDocument(excelDestinationPath);//存檔
                     labMsg.Text += string.Format("{0},{1}~{2}無任何資料!\r\n" , kindId , startDate , endDate);
                     this.Refresh();
                     continue;
                  }

                  #region //2.5寫入五段日期基本資訊[F2:J6]
                  int rowIndex = 2;
                  int pos = 1;
                  foreach (DataRow drDate in dtDate.Rows) {

                     worksheet.Cells[pos , 5].Value = drDate["SDATE"].AsString();//資料起日
                     worksheet.Cells[pos , 6].Value = drDate["EDATE"].AsString();//資料迄日
                     worksheet.Cells[pos , 7].Value = drDate["DAY_CNT"].AsInt();//天數

                     //ken,使用DataView的Find之前,要指定Sort欄位(可多個欄位)
                     int filterIndex = dtSingleKind.Rows.IndexOf(dtSingleKind.Select($"mg1_ymd >= '{drDate["SDATE"].AsString()}'").FirstOrDefault());
                     if (filterIndex >= 0) {
                        worksheet.Cells[pos , 8].Value = rowIndex + filterIndex;//起日位址(FirstRowIndex)
                     } else {
                        worksheet.Cells[pos , 8].Value = rowIndex;//起日位址(FirstRowIndex)
                     }

                     worksheet.Cells[pos , 9].Value = rowIndex + dtSingleKind.Rows.Count - 1;//迄日位址(LastRowIndex)

                     pos++;
                  }//foreach(DataRow drDate in dtDate.Rows) {
                  #endregion

                  //2.6寫入整個子table (日期/實際風險價格係數/最小風險價格係數)
                  worksheet.Import(dtSingleKind , false , 1 , 0);//dataTable, isAddHeader, RowFirstIndex, ColFirstIndex
                  worksheet.Range["A1"].Select();
                  worksheet.ScrollToRow(0);

                  //2.7刪多的圖表(共五個,起始rowIndex=24,每個高31)
                  //ken,從最後一個圖表開始刪除比較正確,才不會跑位,最後圖表rowIndex=148
                  worksheet = workbook.Worksheets["Graph"];
                  int graphRowIndex = 148;
                  int graphHeight = 31;
                  for (int k = dtDate.Rows.Count - 1 ; k >= 0 ; k--) {
                     if (dtDate.Rows[k]["AI2_SELECT"].AsString() == "N") {
                        worksheet.Rows.Remove(graphRowIndex , graphHeight - 1);
                     }
                     graphRowIndex -= graphHeight;
                  }//for(int k = dtDate.Rows.Count - 1;k >= 0;k--) {


                  //2.8寫入[註3]資訊
                  riskRateOrg = Math.Round(riskRateOrg * 100 , 1 , MidpointRounding.AwayFromZero);
                  if (lastRiskRate == 0) {
                     worksheet.Cells[18 , 0].Value = string.Format("註3：上市日起至今最小風險價格係數均為{0}%" , riskRateOrg.ToString());
                  } else {
                     lastRiskRate = Math.Round(lastRiskRate * 100 , 1 , MidpointRounding.AwayFromZero);
                     worksheet.Cells[18 , 0].Value = string.Format("註3：最小風險價格係數自{0}起由{1}%調整為{2}" ,
                                                                 effDate ,
                                                                 lastRiskRate.ToString() ,
                                                                 riskRateOrg.ToString());
                  }

                  //2.9存檔
                  workbook.SaveDocument(excelDestinationPath);

               }//foreach (DataRow drKind in dtTemp.Rows) 商品
               #endregion

            }
            #endregion




            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex , kindId);
         } finally {
            panFilter.Enabled = panSecond.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
         }
         return ResultStatus.Fail;
      }

      /// <summary>
      /// Copy template to target,跟FormParent.CopyExcelTemplateFile只差在檔名需要異動(CopyExcelTemplateFile參數寫蠻死)
      /// </summary>
      /// <param name="fileName"></param>
      /// <param name="kindId"></param>
      /// <returns></returns>
      protected string CopyExcelTemplateFile2(string fileName , string modelName , string kindId = "") {
         string originalFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH , fileName + ".xlsx");
         string tempFileName = fileName;

         //1.檢查範本檔是否存在
         if (!File.Exists(originalFilePath)) {
            throw new Exception("無此檔案「" + originalFilePath + "」!");
         }

         if (kindId != "") {
            tempFileName += string.Format("({0})_{1}_" , kindId , modelName);
         }

         string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                                     tempFileName + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".xlsx");

         File.Copy(originalFilePath , destinationFilePath , true);

         return destinationFilePath;
      }

      #region 按鈕事件
      /// <summary>
      /// 當按下Enter,觸發查詢按鈕
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void txtEndDate_KeyDown(object sender , System.Windows.Forms.KeyEventArgs e) {
         try {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter) {
               btnFirstFilter_Click(btnFirstFilter , null);
            }
         } catch (Exception ex) {
            WriteLog(ex);
         }
      }

      /// <summary>
      /// 查詢,update 2 grid
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnFirstFilter_Click(object sender , EventArgs e) {
         try {
            panFilter.Enabled = panSecond.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "訊息：資料查詢中........";
            this.Refresh();

            //查詢,update 2 grid
            DateTime nextEndDate = txtEndDate.DateTimeValue.AddDays(1);
            DataTable dtDate = dao48040.ListDateArea(txtEndDate.DateTimeValue , nextEndDate);
            gcDate.DataSource = dtDate;

            dtKind = dao48040.ListKind(txtEndDate.DateTimeValue);
            gcKind.DataSource = dtKind;

            //再觸發過濾條件
            cbxSubType_EditValueChanged(cbxSubType , null);

            labMsg.Text = "各契約之最小風險價格係數資料及資料期間已更新!";
            this.Refresh();
         } catch (Exception ex) {
            WriteLog(ex);
            labMsg.Visible = false;
         } finally {
            panFilter.Enabled = panSecond.Enabled = true;
         }
      }

      /// <summary>
      /// filter gvKind
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void cbxSubType_EditValueChanged(object sender , EventArgs e) {
         try {
            if (dtKind == null) return;//初始化

            //filter gvKind
            DevExpress.XtraEditors.LookUpEdit cbx = sender as DevExpress.XtraEditors.LookUpEdit;
            string subType = cbx.EditValue.AsString("");
            if (subType == "%") {
               gcKind.DataSource = dtKind;
            } else {
               gcKind.DataSource = dtKind.Filter(string.Format("cpr_prod_subtype='{0}'" , subType));
            }

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }

      /// <summary>
      /// choose all checkboxs in gvKind
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnChooseAll_Click(object sender , EventArgs e) {
         try {
            if (dtKind == null) return;
            gvKind.CloseEditor();//ken,一定要先呼叫這兩個函數,最後點選的那筆才會被記錄起來
            gvKind.UpdateCurrentRow();//ken,一定要先呼叫這兩個函數,最後點選的那筆才會被記錄起來
            DataTable dtTemp = (DataTable)gcKind.DataSource;
            foreach (DataRow drKind in dtTemp.Rows) {
               drKind["CPR_SELECT"] = "Y";
            }
            gcKind.DataSource = dtTemp;


         } catch (Exception ex) {
            WriteLog(ex);
         }
      }

      /// <summary>
      /// clear all checkboxs in gvKind
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnClearAll_Click(object sender , EventArgs e) {
         try {
            if (dtKind == null) return;
            gvKind.CloseEditor();//ken,一定要先呼叫這兩個函數,最後點選的那筆才會被記錄起來
            gvKind.UpdateCurrentRow();//ken,一定要先呼叫這兩個函數,最後點選的那筆才會被記錄起來
            DataTable dtTemp = (DataTable)gcKind.DataSource;
            foreach (DataRow drKind in dtTemp.Rows) {
               drKind["CPR_SELECT"] = "N";
            }
            gcKind.DataSource = dtTemp;

         } catch (Exception ex) {
            WriteLog(ex);
         }
      }

      #endregion
   }
}
