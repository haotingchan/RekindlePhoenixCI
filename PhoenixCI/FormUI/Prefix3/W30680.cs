using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.TableDao;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/3/5
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30680 臺指選擇權VIX指數日內1分鐘及每日收盤資料查詢
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30680 : FormParent {

      private VIXS daoVIXS;
      private VIX daoVIX;
      private VOLS daoVOLS;
      private VOLD daoVOLD;

      private enum Type {
         N,
         O,
         V
      }

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

      public W30680(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         daoVIXS = new VIXS();
         daoVIX = new VIX();
         daoVOLS = new VOLS();
         daoVOLD = new VOLD();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartYMD.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01");
         txtEndYMD.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
         labMsg.Visible = false;

         //Winni test
         //20180410-0420
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {

         base.Export();

         #region 日期檢核
         //if (!txtStartYMD.IsDate(txtStartYMD.Text , CheckDate.Start)
         //         || !txtEndYMD.IsDate(txtEndYMD.Text , CheckDate.End)) {
         //   return ResultStatus.Fail; ;
         //}

         //if (string.Compare(txtStartYMD.Text , txtEndYMD.Text) > 0) {
         //   MessageDisplay.Error(GlobalInfo.ErrorText , CheckDate.Datedif);
         //   return ResultStatus.Fail; ;
         //}
         #endregion

         if (chkGroup.CheckedItemsCount == 0) {
            MessageDisplay.Error("請勾選要匯出的報表!");
            return ResultStatus.Fail; ;
         }

         labMsg.Visible = true;

         string DateYmd = StartDate + "-" + EndDate;

         /*************************************
            chkGroup.Items[0] = chkNewVixs
            chkGroup.Items[1] = chkNewVix
            chkGroup.Items[2] = chkNewVols
            chkGroup.Items[3] = chkNewVold
            chkGroup.Items[4] = chkOldVixs
            chkGroup.Items[5] = chkOldVix
         *************************************/

         string tmp = DateYmd + "_w" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";  //串檔名
         int outputCount = 0;
         foreach (CheckedListBoxItem item in chkGroup.Items) {
            string filePath = "";
            string title = "";
            string tableName = item.Value.AsString();
            string type = "";

            if (item.CheckState == CheckState.Unchecked) {
               continue;
            }

            #region Item選項
            switch (item.Tag) {
               case 0:
                  filePath = "新VIX統計檔_" + tmp;
                  title = "VIX統計表";
                  type = "N";
                  break;
               case 1:
                  filePath = "新VIX明細檔_" + tmp;
                  title = "VIX明細表";
                  type = "N";
                  break;
               case 2:
                  filePath = "新VIX平均檔_" + tmp;
                  title = "新VIX平均值";
                  type = "V";
                  break;
               case 3:
                  filePath = "新VIX平均明細檔_" + tmp;
                  title = "新VIX平均值";
                  type = "V";
                  break;
               case 4:
                  filePath = "舊VIX統計檔_" + tmp;
                  title = "VIX統計表";
                  type = "O";
                  break;
               case 5:
                  filePath = "舊VIX明細檔_" + tmp;
                  title = "VIX明細表";
                  type = "O";
                  break;
            }
            #endregion

            outputCount += wfExport(type , filePath , title , tableName);
         }

         if (outputCount == 0) return ResultStatus.Fail;

         labMsg.Visible = false;
         return ResultStatus.Success;
      }

      /// <summary>
      /// 依checkbox的勾選決定export的內容
      /// </summary>
      /// <param name="type">N=New,O=Old,V=???</param>
      /// <param name="filePath">檔名(.csv)</param>
      /// <param name="fileTile">功能title</param>
      /// <param name="tableName"></param>
      private int wfExport(string type , string filePath , string fileTile , string tableName) {
         string fileName; //報表名稱
         try {
            if (type == "N") {
               fileName = "新";
            } else if (type == "O") {
               fileName = "舊";
            } else { //V
               fileName = "";
            }
            fileName += fileTile;
            labMsg.Text = _ProgramID + "－" + fileName + " 轉檔中...";

            //讀取資料
            DataTable dtData = new DataTable();
            switch (tableName) {
               case "chkNewVixs":
               case "chkOldVixs":
                  dtData = daoVIXS.GetDataByDate(type , StartDate , EndDate , "Y");
                  break;
               case "chkNewVix":
               case "chkOldVix":
                  dtData = daoVIX.GetDataByDate(type , StartDate , EndDate , "Y");
                  break;
               case "chkNewVols":
                  dtData = daoVOLS.GetDataByDate(StartDate , EndDate , type , "Y");
                  break;
               case "chkNewVold":
                  dtData = daoVOLD.GetDataByDate(StartDate , EndDate , type , "Y");
                  break;
            }

            if (dtData.Rows.Count <= 0) {
               labMsg.Text = String.Format("{0},{1}-{2},無任何資料!" , StartDate.SubStr(0 , 6) , _ProgramID , fileName);
               MessageDisplay.Info(String.Format("{0},{1}-{2},無任何資料!" , StartDate.SubStr(0 , 6) , _ProgramID , fileName));
               return 0;
            }

            //存CSV (ps:輸出csv 都用ascii)
            filePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , filePath);
            ExportOptions csvref = new ExportOptions();
            csvref.HasHeader = true;
            csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
            Common.Helper.ExportHelper.ToCsv(dtData , filePath , csvref);
            return 1;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return 0;
      }

      /// <summary>
      /// set checkbox list focus background color
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void chkGroup_DrawItem(object sender , ListBoxDrawItemEventArgs e) {
         e.AllowDrawSkinBackground = false;
      }

   }
}