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
   /// 40170 選擇權原始資料下載
   /// </summary>
   public partial class W40170 : FormParent {

      private D40170 dao40170;
      string startYmd, startYmd2;

      public W40170(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtDay.Text = "2500";

         dao40170 = new D40170();

#if DEBUG
         //Winni test
         //txtSDate.DateTimeValue = DateTime.ParseExact("2018/06/15", "yyyy/MM/dd", null);
         //this.Text += "(開啟測試模式),ocfDate=2018/06/15";
#endif
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            //1. 設定初始年月yyyy/MM/dd
            txtStartDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01");
            txtStartDate.EnterMoveNextControl = true;
            txtStartDate.Focus();

            txtEndDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/dd");
            txtEndDate.EnterMoveNextControl = true;

            txtDate.Text = txtEndDate.Text;

            //2. 設定dropdownlist(商品)
            DataTable dtKindId = dao40170.GetDwList(); //第一行空白+SORT_SEQ_NO/RPT_KEY/RPT_NAME/CP_DISPLAY
            dwKindId.SetDataTable(dtKindId , "RPT_KEY");

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
         base.Export();

         #region 日期檢核
         if (gbItem.EditValue.AsString() == "rbSdateToEdate") {
            if (!txtStartDate.IsDate(txtStartDate.Text , "日期輸入錯誤!") ||
               !txtEndDate.IsDate(txtEndDate.Text , "日期輸入錯誤!")) {
               txtEndDate.Focus();
               MessageDisplay.Error("日期輸入錯誤!");
               return ResultStatus.Fail;
            }
         } else {
            if (!txtDate.IsDate(txtDate.Text , "日期輸入錯誤!")) {
               txtEndDate.Focus();
               MessageDisplay.Error("日期輸入錯誤!");
               return ResultStatus.Fail;
            }
         }
         #endregion

         try {
            string endYmd, aocfYmd;
            decimal days;

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "轉檔中...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //2. 資料日期區間
            if (gbItem.EditValue.AsString() == "rbSdateToEdate") {
               startYmd = DateTime.ParseExact(txtStartDate.Text , "yyyy/MM/dd" , null).ToString("yyyyMMdd");
               endYmd = DateTime.ParseExact(txtEndDate.Text , "yyyy/MM/dd" , null).ToString("yyyyMMdd");
            } else if (gbItem.EditValue.AsString() == "rbEndDate") {
               endYmd = DateTime.ParseExact(txtEndDate.Text , "yyyy/MM/dd" , null).ToString("yyyyMMdd");
               days = txtDay.Text.AsDecimal();

               //2.1 預估工作天20天,1個月31天
               aocfYmd = txtDate.DateTimeValue.AddDays((double)(Math.Ceiling(days / 20) * 31 * -1)).ToString("yyyyMMdd");

               //2.2 取得資料起始日
               startYmd2 = dao40170.GetStartDate(endYmd , aocfYmd , days);

            }

            //3. 商品
            DataTable dtKindId = dao40170.GetDwList(); //第一行空白+SORT_SEQ_NO/RPT_KEY/RPT_NAME/CP_DISPLAY

            //4. 模型代碼
            if (chkModel.CheckedItemsCount == 0) {
               MessageDisplay.Error("請勾選要匯出的報表!");
               return ResultStatus.Fail; ;
            }

            string modelType, modelName, kindId;

            foreach (CheckedListBoxItem item in chkModel.Items) {
               if (item.CheckState == CheckState.Unchecked) {
                  continue;
               }

               //startYmd = DateTime.ParseExact(txtStartDate.Text , "yyyy/MM/dd" , null).ToString("yyyyMMdd");
               endYmd = DateTime.ParseExact(txtEndDate.Text , "yyyy/MM/dd" , null).ToString("yyyyMMdd");

               switch (item.Value) {
                  case "chkSma":
                     modelType = "S";
                     modelName = item.Description;
                     kindId = dwKindId.EditValue.AsString(); //rpt_key
                     startYmd = gbItem.EditValue.AsString() == "rbSdateToEdate" ? startYmd : startYmd2;

                     //一個商品產生一個檔
                     if (kindId == "%") {
                        foreach (DataRow dr in dtKindId.Rows) {
                           string rptKey = dr["rpt_key"].AsString();
                           kindId = rptKey;

                           if (kindId == "") continue;

                           if (kindId != "%") {
                              wf_40170(modelType , startYmd , endYmd , kindId , modelName);
                           }
                        }
                     } else {
                        wf_40170(modelType , startYmd , endYmd , kindId , modelName);
                     }
                     break;
                  case "chkEwma":
                     modelType = "E";
                     modelName = item.Description;
                     kindId = dwKindId.EditValue.AsString(); //rpt_key
                     startYmd = gbItem.EditValue.AsString() == "rbSdateToEdate" ? startYmd : startYmd2;

                     //一個商品產生一個檔
                     if (kindId == "%") {
                        foreach (DataRow dr in dtKindId.Rows) {
                           string rptKey = dr["rpt_key"].AsString();
                           kindId = rptKey;

                           if (kindId == "") continue;

                           if (kindId != "%") {
                              wf_40170(modelType , startYmd , endYmd , kindId , modelName);
                           }
                        }
                     } else {
                        wf_40170(modelType , startYmd , endYmd , kindId , modelName);
                     }
                     break;
                  case "chkMaxVol":
                     modelType = "M";
                     modelName = item.Description;
                     kindId = dwKindId.EditValue.AsString(); //rpt_key
                     startYmd = gbItem.EditValue.AsString() == "rbSdateToEdate" ? startYmd : startYmd2;

                     //一個商品產生一個檔
                     if (kindId == "%") {
                        foreach (DataRow dr in dtKindId.Rows) {
                           string rptKey = dr["rpt_key"].AsString();
                           kindId = rptKey;

                           if (kindId == "") continue;

                           if (kindId != "%") {
                              wf_40170(modelType , startYmd , endYmd , kindId , modelName);
                           }
                        }
                     } else {
                        wf_40170(modelType , startYmd , endYmd , kindId , modelName);
                     }
                     break;
               }
            }

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

      private void wf_40170(string modelType , string startDate , string endDate , string kindId , string modelName) {
         try {
            //執行CI.SP_H_TXN_40170_DETL
            DataTable dt = dao40170.ExecuteStoredProcedure(modelType , startDate , endDate , kindId);
            if (dt.Rows.Count <= 0) {
               return;
            }

            //存CSV (ps:輸出csv 都用ascii)
            string etfFileName = kindId + "_" + modelName + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
            etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
            ExportOptions csvref = new ExportOptions();
            csvref.HasHeader = false;
            csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
            Common.Helper.ExportHelper.ToCsv(dt , etfFileName , csvref);

            WriteLog("執行ci.SP_H_TXN_40160_DETL" , "Info" , "X" , false);

         } catch (Exception ex) {
            WriteLog(ex);
         }
         return;
      }
   }
}