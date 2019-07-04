using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/02/27
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30670 匯率及國外指數商品周報
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30670 : FormParent {

      private D30670 dao30670;
      private AI5 daoAI5;
      string rptFileName, rptFuncName, kindName, excelDestinationPath;
      Workbook file = new Workbook();

      #region 一般交易查詢條件縮寫
      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string StartDate {
         get {
            return txtStartYMD.DateTimeValue.ToString("yyyyMMdd");
         }
      }

      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string EndDate {
         get {
            return txtEndYMD.DateTimeValue.ToString("yyyyMMdd");
         }
      }
      #endregion

      public W30670(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         dao30670 = new D30670();
         daoAI5 = new AI5();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartYMD.Text = GlobalInfo.OCF_DATE.AddDays(-6).ToString("yyyy/MM/dd");
         txtEndYMD.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         //txtStartYMD.Text = "2017/05/15";
         //txtEndYMD.Text = "2017/05/23";

         //Winni test
         //20170515-0523
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {

         labMsg.Visible = true;

         #region 日期檢核
         //if (!txtStartYMD.IsDate(txtStartYMD.Text , CheckDate.Start)
         //         || !txtEndYMD.IsDate(txtEndYMD.Text , CheckDate.End)) {
         //       labMsg.Visible = false;
         //       return ResultStatus.Fail; ;
         //}

         if (string.Compare(txtStartYMD.Text , txtEndYMD.Text) > 0) {
            MessageDisplay.Error(CheckDate.Datedif , GlobalInfo.ErrorText);
            labMsg.Visible = false;
            return ResultStatus.Fail; ;
         }
         #endregion

         if (chkGroup.CheckedItemsCount == 0) {
            MessageDisplay.Error("請勾選要匯出的報表!" , GlobalInfo.ErrorText);
            return ResultStatus.Fail; ;
         }

         /*************************************
            chkGroup.Items[0] = chkRhf
            chkGroup.Items[1] = chkXef
            chkGroup.Items[2] = chkTjf
            chkGroup.Items[3] = chkUdf
            chkGroup.Items[4] = chkXbf
            chkGroup.Items[5] = chkBrf
         *************************************/

         foreach (CheckedListBoxItem item in chkGroup.Items) {

            if (item.CheckState == CheckState.Unchecked) {
               continue;
            }

            #region Item選項
            switch (item.Tag) {
               case 0:
                  rptFuncName = "人民幣匯率期貨及選擇權周報";
                  rptFileName = "30670_RHF";
                  kindName = "RHF%";
                  break;
               case 1:
                  rptFuncName = "歐元兌美元及美元兌日圓期貨周報";
                  rptFileName = "30670_XEF";
                  kindName = "XEF%";
                  break;
               case 2:
                  rptFuncName = "TJF及I5F周報";
                  rptFileName = "30670_TJF";
                  kindName = "TJF%";
                  break;
               case 3:
                  rptFuncName = "UDF及SPF周報";
                  rptFileName = "30670_UDF";
                  kindName = "UDF%";
                  break;
               case 4:
                  rptFuncName = "XBF及XAF周報";
                  rptFileName = "30670_XBF";
                  kindName = "XBF%";
                  break;
               case 5:
                  rptFuncName = "BRF周報";
                  rptFileName = "30670_BRF";
                  kindName = "BRF%";
                  break;
            }
            #endregion

            WriteFile(rptFuncName , rptFileName , kindName);
         }

         return ResultStatus.Success;
      }

      /// <summary>
      /// 開啟檔案
      /// </summary>
      /// <param name="rptFileName">30670_XXX</param>
      /// <returns></returns>
      private Workbook OpenFile(string rptFileName) {
         excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , rptFileName);
         file.LoadDocument(excelDestinationPath);
         return file;
      }

      /// <summary>
      /// 分別在Template填資料
      /// </summary>
      /// <param name="rptFuncName">功能文字敘述</param>
      /// <param name="rptFileName">30670_XXX</param>
      /// <param name="kindName">XXX%</param>
      private void WriteFile(string rptFuncName , string rptFileName , string kindName) {
         try {
            //1.複製檔案 & 開啟檔案(rhf)
            file = OpenFile(rptFileName);

            //2.填資料
            bool resultOne = false, resultTwo = false;
            resultOne = wf30670_1(kindName , file);
            resultTwo = wf30670_2(kindName , file);

            CheckFile(resultOne , resultTwo , file); //若兩個皆false則Fail

            //3.存檔
            file.SaveDocument(excelDestinationPath);
            labMsg.Visible = false;

         } catch (Exception ex) {
            WriteLog(ex);
            return;
         }
      }

      /// <summary>
      /// wf30670_1
      /// </summary>
      /// <param name="rptFileName">30670_XXX</param>
      /// <param name="kindName">XXX%</param>
      /// <param name="workbook"></param>
      /// <returns></returns>
      private bool wf30670_1(string kindName , Workbook workbook) {
         try {
            //1.切換第1張Sheet
            Worksheet worksheet = workbook.Worksheets[0];

            //1.1
            labMsg.Text = rptFuncName + "轉檔中...";

            //2.讀取資料：到期月份
            wf_30670_seq(kindName , worksheet);

            //3.讀取資料：日期
            wf_30670_ymd(kindName , worksheet);

            //4.讀取資料：本周名目本金
            wf_30670_amt(kindName , worksheet);

            return true;
         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }
      }

      /// <summary>
      /// wf30670_2
      /// </summary>
      /// <param name="kindName">XXX%</param>
      /// <param name="workbook"></param>
      /// <returns></returns>
      private bool wf30670_2(string kindName , Workbook workbook) {
         string sheetName = "走勢圖", maxDate;
         DateTime ldtDate;
         int cellCol, cellRow, preRow;

         try {
            //取得AI5最大日期
            ldtDate = txtEndYMD.DateTimeValue;
            //string maxDate = daoAI5.GetNameByNo(ldtDate).SubStr(0 , 10).Trim().Replace("/" , ""); //有補0問題
            maxDate = ldtDate.ToString("yyyy/MM/dd").Replace("/" , "");

            //讀取資料：走勢圖
            DataTable dtTrend = dao30670.d30670_AI3(maxDate , kindName);
            if (dtTrend.Rows.Count <= 0) {
               labMsg.Text = string.Format("{0}-{1}" , StartDate , EndDate) + "," + rptFileName + '－' + rptFuncName + "無任何資料!";
               MessageDisplay.Info(string.Format("{0}-{1},{2}-{3}無任何資料!" , txtStartYMD.Text , txtEndYMD.Text , rptFileName , rptFuncName) , GlobalInfo.ResultText);
               return false;
            }

            //切換第2張Sheet
            Worksheet worksheet = workbook.Worksheets[1];

            //資料寫入
            cellRow = 0;
            for (int w = 0 ; w < dtTrend.Rows.Count ; w++) {
               ldtDate = dtTrend.Rows[w]["AI3_DATE"].AsDateTime();
               if (w == 0) {
                  cellRow++;
               } else if (w > 0) {
                  preRow = w - 1;

                  if (ldtDate != dtTrend.Rows[preRow]["AI3_DATE"].AsDateTime()) {
                     cellRow++;
                  }
               }

               worksheet.Cells[cellRow , 0].Value = dtTrend.Rows[w]["AI3_DATE"].AsDateTime();

               cellCol = dtTrend.Rows[w]["RPT_PRICE_COL"].AsInt() - 1;
               worksheet.Cells[cellRow , cellCol].Value = dtTrend.Rows[w]["AI5_SETTLE_PRICE"].AsDecimal();

               cellCol = dtTrend.Rows[w]["RPT_INDEX_COL"].AsInt() - 1;
               worksheet.Cells[cellRow , cellCol].Value = dtTrend.Rows[w]["AI3_INDEX"].AsDecimal();
            }

            if (cellRow < 30) {
               MessageDisplay.Info(string.Format("{0}-{1},{2}-{3}筆數不足30筆" , txtStartYMD.Text , txtEndYMD.Text , rptFileName , rptFuncName) , GlobalInfo.ResultText);
               for (int w = cellRow ; w <= 30 ; w++) {
                  cellRow++;
                  worksheet.Cells[cellRow , 0].Value = "";
               }
            }
            return true;

         } catch (Exception ex) {
            WriteLog(ex);
            return false;
         }

      }

      /// <summary>
      /// 若都沒資料則回傳Fail
      /// </summary>
      /// <param name="resultOne"></param>
      /// <param name="resultTwo"></param>
      /// <param name="workbook"></param>
      /// <returns></returns>
      private ResultStatus CheckFile(bool resultOne , bool resultTwo , Workbook workbook) {
         if (!resultOne && !resultTwo) {
            try {
               workbook = null;
               File.Delete(excelDestinationPath);
            } catch (Exception ex) {
               WriteLog(ex);
            }
            return ResultStatus.Fail;
         }
         return ResultStatus.Success;
      }

      /// <summary>
      /// wf_30670_seq (讀取資料：到期月份)
      /// </summary>
      /// <param name="rptFileName">30670_XXX</param>
      /// <param name="kindName">XXX%</param>
      /// <param name="worksheet"></param>
      private void wf_30670_seq(string kindName , Worksheet worksheet) {
         int cellCol, cellRow;
         DataTable dtMon = dao30670.d30670_AI2_SEQ(StartDate , EndDate , kindName);
         if (dtMon.Rows.Count <= 0) {
            labMsg.Text = string.Format("{0}-{1}" , StartDate , EndDate) + "," + rptFileName + '－' + rptFuncName + "(到期月份),無任何資料!";
            MessageDisplay.Info(string.Format("{0}-{1},{2}-{3}(到期月份),無任何資料!" , txtStartYMD.Text , txtEndYMD.Text , rptFileName , rptFuncName) , GlobalInfo.ResultText);
            return;
         }

         //資料寫入
         for (int w = 0 ; w < dtMon.Rows.Count ; w++) {
            cellRow = dtMon.Rows[w]["RPT_ROW"].AsInt() + dtMon.Rows[w]["MTH_SEQ_NO"].AsInt() - 2;

            cellCol = dtMon.Rows[w]["RPT_M_COL"].AsInt() - 1;
            worksheet.Cells[cellRow , cellCol].Value = dtMon.Rows[w]["SUM_QNTY"].AsDecimal();

            cellCol = dtMon.Rows[w]["RPT_OI_COL"].AsInt() - 1;
            worksheet.Cells[cellRow , cellCol].Value = dtMon.Rows[w]["SUM_OI"].AsDecimal();
         }
      }

      /// <summary>
      /// wf_30670_ymd (讀取資料：日期)
      /// </summary>
      /// <param name="kindName">XXX%</param>
      /// <param name="worksheet"></param>
      private void wf_30670_ymd(string kindName , Worksheet worksheet) {
         int cellCol, cellRow;
         DataTable dtDate = dao30670.d30670_AI2_YMD(StartDate , EndDate , kindName);
         if (dtDate.Rows.Count <= 0) {
            labMsg.Text = string.Format("{0}-{1}" , StartDate , EndDate) + "," + rptFileName + '－' + rptFuncName + "(日期),無任何資料!";
            MessageDisplay.Info(string.Format("{0}-{1},{2}-{3}(日期),無任何資料!" , txtStartYMD.Text , txtEndYMD.Text , rptFileName , rptFuncName) , GlobalInfo.ResultText);
            return;
         }

         //資料寫入
         for (int w = 0 ; w < dtDate.Rows.Count ; w++) {
            cellRow = dtDate.Rows[w]["RPT_ROW"].AsInt() + dtDate.Rows[w]["DAY_SEQ_NO"].AsInt() - 2;
            string tmp = dtDate.Rows[w]["AI2_YMD"].AsString();
            worksheet.Cells[cellRow , 0].Value = tmp.AsDateTime("yyyyMMdd");
            //worksheet.Cells[cellRow , 0].Value = tmp.SubStr(0 , 4) + "/" + tmp.SubStr(4 , 2) + "/" + tmp.SubStr(6 , 2);
            //worksheet.Cells[cellRow , 0].Value = string.Format("yyyy/MM/dd",dtDate.Rows[w]["AI2_YMD"]);

            cellCol = dtDate.Rows[w]["RPT_M_COL"].AsInt() - 1;
            worksheet.Cells[cellRow , cellCol].Value = dtDate.Rows[w]["SUM_QNTY"].AsDecimal();

            cellCol = dtDate.Rows[w]["RPT_OI_COL"].AsInt() - 1;
            worksheet.Cells[cellRow , cellCol].Value = dtDate.Rows[w]["SUM_OI"].AsDecimal();
         }
      }

      /// <summary>
      /// wf_30670_amt (讀取資料：本周名目本金)
      /// </summary>
      /// <param name="kindName">XXX%</param>
      /// <param name="worksheet"></param>
      private void wf_30670_amt(string kindName , Worksheet worksheet) {
         int cellCol, cellRow;
         DataTable dtMoney = dao30670.d30670_AA2_AMT(StartDate , EndDate , kindName);
         if (dtMoney.Rows.Count <= 0) {

            labMsg.Text = string.Format("{0}-{1}" , StartDate , EndDate) + "," + rptFileName + '－' + rptFuncName + "(amt),無任何資料!";
            MessageDisplay.Info(string.Format("{0}-{1},{2}-{3}(amt),無任何資料!" , txtStartYMD.Text , txtEndYMD.Text , rptFileName , rptFuncName) , GlobalInfo.ResultText);
            return;
         }

         //資料寫入
         for (int w = 0 ; w < dtMoney.Rows.Count ; w++) {
            cellRow = dtMoney.Rows[w]["RPT_ROW"].AsInt() - 1;

            cellCol = dtMoney.Rows[w]["RPT_COL"].AsInt() - 1;
            worksheet.Cells[cellRow , cellCol].Value = dtMoney.Rows[w]["SUM_AMT_ORG"].AsDecimal();

            if (kindName == "RHF%") {
               cellRow += 2;
               worksheet.Cells[cellRow , cellCol].Value = dtMoney.Rows[w]["SUM_AMT_USD"].AsDecimal();
            }
         }
      }

   }
}