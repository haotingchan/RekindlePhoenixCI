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
/// Winni, 2019/04/08
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30392 國外指數類期貨契約價量資料
   /// </summary>
   public partial class W30392 : FormParent {

      private D40160 dao40160;
      string startYmd, startYmd2;

      public W30392(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = GlobalInfo.OCF_DATE;

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

            //1. 設定初始年月yyyy/MM/dd
            txtDate.Text = GlobalInfo.OCF_DATE.ToString("yyyy/MM/01");
            txtDate.EnterMoveNextControl = true;
            txtDate.Focus();

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

         try {

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "轉檔中...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //   //2. 資料日期區間
            //   if (gbItem.EditValue.AsString() == "rbSdateToEdate") {
            //      startYmd = DateTime.ParseExact(txtStartDate.Text , "yyyy/MM/dd" , null).ToString("yyyyMMdd");
            //      endYmd = DateTime.ParseExact(txtEndDate.Text , "yyyy/MM/dd" , null).ToString("yyyyMMdd");
            //   } else if (gbItem.EditValue.AsString() == "rbEndDate") {
            //      endYmd = DateTime.ParseExact(txtEndDate.Text , "yyyy/MM/dd" , null).ToString("yyyyMMdd");
            //      days = txtDay.Text.AsDecimal();

            //      //2.1 預估工作天20天,1個月31天
            //      aocfYmd = txtDate.DateTimeValue.AddDays((double)(Math.Ceiling(days / 20) * 31 * -1)).ToString("yyyyMMdd");

            //      //2.2 取得資料起始日
            //      startYmd2 = dao40160.GetStartDate(endYmd , aocfYmd , days);

            //   }

            //   //3. 商品
            //   DataTable dtKindId = dao40160.GetDwList(); //第一行空白+SORT_SEQ_NO/RPT_KEY/RPT_NAME/CP_DISPLAY

            //   //4. 模型代碼
            //   if (chkModel.CheckedItemsCount == 0) {
            //      MessageDisplay.Error("請勾選要匯出的報表!");
            //      return ResultStatus.Fail; ;
            //   }

            //   string modelType, modelName, kindId;

            //   foreach (CheckedListBoxItem item in chkModel.Items) {
            //      if (item.CheckState == CheckState.Unchecked) {
            //         continue;
            //      }

            //      //startYmd = DateTime.ParseExact(txtStartDate.Text , "yyyy/MM/dd" , null).ToString("yyyyMMdd");
            //      endYmd = DateTime.ParseExact(txtEndDate.Text , "yyyy/MM/dd" , null).ToString("yyyyMMdd");

            //      switch (item.Value) {
            //         case "chkSma":
            //            modelType = "S";
            //            modelName = item.Description;
            //            kindId = dwKindId.EditValue.AsString(); //rpt_key
            //            startYmd = gbItem.EditValue.AsString() == "rbSdateToEdate" ? startYmd : startYmd2;

            //            //一個商品產生一個檔
            //            if (kindId == "%") {
            //               foreach (DataRow dr in dtKindId.Rows) {
            //                  string rptKey = dr["rpt_key"].AsString();
            //                  kindId = rptKey;

            //                  if (kindId != "%") {
            //                     wf_40160(modelType , startYmd , endYmd , kindId , modelName);
            //                  }
            //               }
            //            } else {
            //               wf_40160(modelType , startYmd , endYmd , kindId , modelName);
            //            }
            //            break;
            //         case "chkEwma":
            //            modelType = "E";
            //            modelName = item.Description;
            //            kindId = dwKindId.EditValue.AsString(); //rpt_key
            //            startYmd = gbItem.EditValue.AsString() == "rbSdateToEdate" ? startYmd : startYmd2;

            //            //一個商品產生一個檔
            //            if (kindId == "%") {
            //               foreach (DataRow dr in dtKindId.Rows) {
            //                  string rptKey = dr["rpt_key"].AsString();
            //                  kindId = rptKey;

            //                  if (kindId != "%") {
            //                     wf_40160(modelType , startYmd , endYmd , kindId , modelName);
            //                  }
            //               }
            //            } else {
            //               wf_40160(modelType , startYmd , endYmd , kindId , modelName);
            //            }
            //            break;
            //         case "chkMaxVol":
            //            modelType = "M";
            //            modelName = item.Description;
            //            kindId = dwKindId.EditValue.AsString(); //rpt_key
            //            startYmd = gbItem.EditValue.AsString() == "rbSdateToEdate" ? startYmd : startYmd2;

            //            //一個商品產生一個檔
            //            if (kindId == "%") {
            //               foreach (DataRow dr in dtKindId.Rows) {
            //                  string rptKey = dr["rpt_key"].AsString();
            //                  kindId = rptKey;

            //                  if (kindId != "%") {
            //                     wf_40160(modelType , startYmd , endYmd , kindId , modelName);
            //                  }
            //               }
            //            } else {
            //               wf_40160(modelType , startYmd , endYmd , kindId , modelName);
            //            }
            //            break;
            //      }
            //   }

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

   }
}