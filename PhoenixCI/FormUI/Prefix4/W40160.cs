using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/04/01
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 40160 期貨原始資料下載
   /// </summary>
   public partial class W40160 : FormParent {

      private D40160 dao40160;

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

      /// <summary>
      /// yyyyMMdd
      /// </summary>
      public string FinalDate {
         get {
            return txtDate.DateTimeValue.ToString("yyyyMMdd");
         }
      }
      #endregion

      public W40160(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtDay.Text = "2500";

         dao40160 = new D40160();

#if DEBUG
         //Winni test
         //txtSDate.DateTimeValue = DateTime.ParseExact("2018/06/15", "yyyy/MM/dd", null);
         //this.Text += "(開啟測試模式),ocfDate=2018/06/15";
#endif
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            //1. 設定初始值
            txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE.AddDays(-GlobalInfo.OCF_DATE.Day + 1); //取得當月第1天
            txtStartDate.EnterMoveNextControl = true;
            txtStartDate.Focus();

            txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
            txtEndDate.EnterMoveNextControl = true;

            txtDate.DateTimeValue = txtEndDate.DateTimeValue;
            txtDay.EditValue = 2500;

            //2. 設定dropdownlist(商品)
            DataTable dtKindId = dao40160.GetDwList(); //第一行空白+SORT_SEQ_NO/RPT_KEY/RPT_NAME/CP_DISPLAY
            dwKindId.SetDataTable(dtKindId , "RPT_KEY" , "CP_DISPLAY" , TextEditStyles.DisableTextEditor , "");
            dwKindId.ItemIndex = 0;

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
            return ResultStatus.Fail;
         }
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected void ExportAfter() {
         labMsg.Text = "轉檔完成!";
         this.Cursor = Cursors.Arrow;
         this.Refresh();
         Thread.Sleep(5);
         labMsg.Visible = false;
      }

      protected void ShowMsg(string msg) {
         labMsg.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export() {
         try {

            #region 日期檢核
            if (!txtStartDate.IsDate(txtStartDate.Text , CheckDate.Start)
                     || !txtEndDate.IsDate(txtEndDate.Text , CheckDate.End)) {
               labMsg.Visible = false;
               return ResultStatus.Fail; ;
            }

            if (string.Compare(txtStartDate.Text , txtEndDate.Text) > 0) {
               MessageDisplay.Error(CheckDate.Datedif , GlobalInfo.ErrorText);
               labMsg.Visible = false;
               return ResultStatus.Fail; ;
            }
            #endregion

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "轉檔中...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //2. 資料日期區間
            string startYmd = "", endYmd = "", aocfYmd = "";
            decimal days;
            if (gbItem.EditValue.AsString() == "rbSdateToEdate") {
               startYmd = StartDate;
               endYmd = EndDate;
            } else if (gbItem.EditValue.AsString() == "rbEndDate") {
               endYmd = FinalDate;
               days = txtDay.EditValue.AsDecimal();

               //2.1 預估工作天20天,1個月31天
               aocfYmd = txtDate.DateTimeValue.AddDays((double)(Math.Ceiling(days / 20) * 31 * -1)).ToString("yyyyMMdd");

               //2.2 取得資料起始日
               startYmd = dao40160.GetStartDate(endYmd , aocfYmd , days);

            }

            //3. 商品
            DataTable dtKindId = dao40160.GetDwList(); //第一行空白+SORT_SEQ_NO/RPT_KEY/RPT_NAME/CP_DISPLAY

            //4. 模型代碼
            if (chkModel.CheckedItemsCount < 1) {
               MessageDisplay.Error("請勾選要匯出的報表!");
               return ResultStatus.Fail;
            }

            string modelType, modelName, kindId;

            int res = 0;
            foreach (CheckedListBoxItem item in chkModel.Items) {
               if (item.CheckState == CheckState.Unchecked) continue;

               switch (item.Value) {
                  case "chkSma":
                     modelType = "S";
                     modelName = item.Description;
                     kindId = dwKindId.EditValue.AsString();

                     //一個商品產生一個檔
                     if (kindId == "%") {
                        foreach (DataRow dr in dtKindId.Rows) {
                           if (dr["RPT_KEY"].AsString() == "%") continue; //跳過全部
                           res += wf_40160(modelType , startYmd , endYmd , dr["RPT_KEY"].AsString() , modelName);
                        }
                     } else {
                        res += wf_40160(modelType , startYmd , endYmd , kindId , modelName);
                     }
                     break;
                  case "chkEwma":
                     modelType = "E";
                     modelName = item.Description;
                     kindId = dwKindId.EditValue.AsString();

                     //一個商品產生一個檔
                     if (kindId == "%") {
                        foreach (DataRow dr in dtKindId.Rows) {
                           if (dr["RPT_KEY"].AsString() == "%") continue;
                           res += wf_40160(modelType , startYmd , endYmd , dr["RPT_KEY"].AsString() , modelName);
                        }
                     } else {
                        res += wf_40160(modelType , startYmd , endYmd , kindId , modelName);
                     }
                     break;
                  case "chkMaxVol":
                     modelType = "M";
                     modelName = item.Description;
                     kindId = dwKindId.EditValue.AsString();

                     //一個商品產生一個檔
                     if (kindId == "%") {
                        foreach (DataRow dr in dtKindId.Rows) {
                           if (dr["RPT_KEY"].AsString() == "%") continue;
                           res += wf_40160(modelType , startYmd , endYmd , dr["RPT_KEY"].AsString() , modelName);
                        }
                     } else {
                        res += wf_40160(modelType , startYmd , endYmd , kindId , modelName);
                     }
                     break;
               }//switch (item.Value)
               if (res <= 0) {
                  MessageDisplay.Info("查無資料!");
                  return ResultStatus.Fail;
               }
            }//foreach (CheckedListBoxItem item in chkModel.Items)

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
            this.Cursor = Cursors.Arrow;
         }
         return ResultStatus.Fail;
      }

      private int wf_40160(string modelType , string startDate , string endDate , string kindId , string modelName) {
         try {
            //執行CI.SP_H_TXN_40160_DETL
            DataTable dt = dao40160.ExecuteStoredProcedure(modelType , startDate , endDate , kindId);
            if (dt.Rows.Count <= 1) {
               return 0;
            }

            //存CSV (ps:輸出csv 都用ascii)
            string etfFileName = kindId + "_" + modelName + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
            etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
            ExportOptions csvref = new ExportOptions();
            csvref.HasHeader = false;
            csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
            Common.Helper.ExportHelper.ToCsv(dt , etfFileName , csvref);

            WriteLog("執行ci.SP_H_TXN_40160_DETL" , "Info" , "X" , false);
            return 1;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return 0;
      }
   }
}