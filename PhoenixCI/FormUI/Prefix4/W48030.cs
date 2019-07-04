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
using System.Windows.Forms;

/// <summary>
/// ken,2019/3/7
/// TODO:左邊grid 有兩個欄位要顯示%
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 風險價格係數狀況彙總表
   /// </summary>
   public partial class W48030 : FormParent {
      private D48030 dao48030;
      protected bool flagTest = true;
      protected DataTable dtKind;

      public W48030(string programID , string programName) : base(programID , programName) {
         try {
            InitializeComponent();

            this.Text = _ProgramID + "─" + _ProgramName;
            GridHelper.SetCommonGrid(gvKind);
            GridHelper.SetCommonGrid(gvDate);
            gcDate.Enabled = false;

            txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;

            dao48030 = new D48030();

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
            string tempDate = dao48030.GetMaxDate(txtEndDate.Text);
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

         try {
            //2.開始轉出資料
            panFilter.Enabled = panSecond.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Refresh();

            //2.1 copy template xlsx to target path and open
            Workbook workbook = new Workbook();
            string originalFilePath = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH , _ProgramID + "." + FileType.XLSX.ToString().ToLower());
            string excelDestinationPath = "";

            DataTable dtDate = (DataTable)gcDate.DataSource;

            #region 指標:SMA,EWMA,MAX
            foreach (CheckedListBoxItem item in chkModel.Items) {
               if (item.CheckState == CheckState.Unchecked) continue;

               int sheetIndex = 0;
               int flag = 0;
               string modelType = "";
               switch (item.Value) {
                  case "chkSma":
                     excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                _ProgramID + "_SMA_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss.") + FileType.XLSX.ToString().ToLower());
                     modelType = "S";
                     break;
                  case "chkEwma":
                     excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                _ProgramID + "_EWMA_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss.") + FileType.XLSX.ToString().ToLower());
                     modelType = "E";
                     break;
                  case "chkMax":
                     excelDestinationPath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH ,
                _ProgramID + "_MAX_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss.") + FileType.XLSX.ToString().ToLower());
                     modelType = "M";
                     break;
               }

               //copy template and change filename
               File.Copy(originalFilePath , excelDestinationPath , true);
               workbook.LoadDocument(excelDestinationPath);

               #region 時段
               //每個時間區間為一個sheet,總共5個
               foreach (DataRow drDate in dtDate.Rows) {
                  string monDiff = drDate["MON_DIFF"].AsString();//期間
                  string startDate = drDate["SDATE"].AsDateTime().ToString("yyyyMMdd");//資料起日
                  string endDate = drDate["EDATE"].AsDateTime().ToString("yyyyMMdd");//資料迄日
                                                                                     //DateTime startDate = drDate["SDATE"].AsDateTime();//資料起日
                                                                                     //DateTime endDate = drDate["EDATE"].AsDateTime();//資料迄日
                  int dayCount = drDate["DAY_CNT"].AsInt();//天數

                  //2.2跳到指定sheet,寫檔頭
                  Worksheet worksheet = workbook.Worksheets[sheetIndex++];
                  worksheet.Cells[1 , 2].Value = cbxSubType.Text;
                  worksheet.Cells[2 , 11].Value = string.Format("列印日期：{0}" , DateTime.Now.ToString("yyyy/MM/dd"));
                  worksheet.Cells[2 , 1].Value = string.Format("{0}({1}～{2})，計{3}天" ,
                                                                  monDiff ,
                                                                  drDate["SDATE"].AsDateTime().ToString("yyyy/MM/dd") ,
                                                                  drDate["EDATE"].AsDateTime().ToString("yyyy/MM/dd") ,
                                                                  dayCount.ToString());

                  #region //2.3分別讀取每個商品的詳細資訊
                  int rowIndex = 7;
                  int emptyRowCount = 60;//template 空白行的數量
                  int kindCount = 0;

                  //逐一看勾選的商品有哪些
                  foreach (DataRow drKind in dtTemp.Rows) {
                     if (drKind["CPR_SELECT"].AsString() != "Y")
                        continue;

                     kindCount++;
                     string kindId = drKind["cpr_kind_id"].AsString();//契約ID
                     Decimal riskRate = drKind["cpr_price_risk_rate"].AsDecimal();//現行最小風險價格係數
                     Decimal interval = drKind["risk_interval"].AsDecimal();//最小風險價格係數級距

                     //2.3.1讀取子table data
                     DataTable dtSingleKind = dao48030.ListKindByKindId(startDate , endDate , riskRate , interval , kindId , modelType);
                     if (dtSingleKind.Rows.Count <= 0) {
                        labMsg.Text += string.Format("{0},{1}~{2}無任何資料!\r\n" , kindId , startDate , endDate);
                        this.Refresh();
                        continue;
                     }

                     //2.3.2寫入明細
                     //ken,原則上一個商品只會找到一筆明細(已經group by)
                     DataRow drDetail = dtSingleKind.Rows[0];
                     int tempCount = drDetail["cnt"].AsInt();
                     Decimal level_1 = drDetail["level_1"].AsDecimal();
                     Decimal level_23 = drDetail["level_23"].AsDecimal();
                     Decimal level_4 = drDetail["level_4"].AsDecimal();

                     worksheet.Cells[rowIndex , 0].Value = drDetail["mg1_kind_id"].AsString();
                     worksheet.Cells[rowIndex , 1].Value = drDetail["avg_risk"].AsDecimal();
                     worksheet.Cells[rowIndex , 2].Value = drDetail["max_risk"].AsDecimal();
                     worksheet.Cells[rowIndex , 3].Value = drDetail["min_risk"].AsDecimal();
                     worksheet.Cells[rowIndex , 4].Value = riskRate;

                     worksheet.Cells[rowIndex , 6].Value = level_1;
                     worksheet.Cells[rowIndex , 7].Value = Math.Round(level_1 / tempCount , 4 , MidpointRounding.AwayFromZero);
                     worksheet.Cells[rowIndex , 8].Value = level_23;
                     worksheet.Cells[rowIndex , 9].Value = Math.Round(level_23 / tempCount , 4 , MidpointRounding.AwayFromZero);
                     worksheet.Cells[rowIndex , 10].Value = level_4;
                     worksheet.Cells[rowIndex , 11].Value = Math.Round(level_4 / tempCount , 4 , MidpointRounding.AwayFromZero);

                     worksheet.Cells[rowIndex , 12].Value = interval;

                     rowIndex++;
                     flag++;
                  }//foreach (DataRow drKind in dtTemp.Rows) 商品
                  #endregion

                  //2.4刪除空白列 (結束商品foreach才刪除空白列，跑下一個sheet)
                  if (kindCount < emptyRowCount) {
                     worksheet.Rows.Remove(rowIndex , emptyRowCount - kindCount);
                  }

               }//foreach (DataRow drDate in dtDate.Rows) 時段
               #endregion

               //2.9存檔
               if (flag > 0) {
                  workbook.SaveDocument(excelDestinationPath);
               } else {
                  File.Delete(excelDestinationPath);
               }

            }//foreach (CheckedListBoxItem item in chkModel.Items) 指標
            #endregion

            //if (FlagAdmin)
            //   System.Diagnostics.Process.Start(excelDestinationPath);

            return ResultStatus.Success;

         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = panSecond.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
         }
         return ResultStatus.Fail;
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
            DataTable dtDate = dao48030.ListDateArea(txtEndDate.DateTimeValue , nextEndDate);
            gcDate.DataSource = dtDate;

            dtKind = dao48030.ListKind(txtEndDate.DateTimeValue);
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
