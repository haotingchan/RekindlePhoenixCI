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
/// Winni, 2019/04/02
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 43032 上市、上櫃受益憑證原始資料查詢
   /// </summary>
   public partial class W43032 : FormParent {

      private D43032 dao43032;
      int flag;

      public W43032(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = DateTime.Now;
         txtSid.Text = "";

         dao43032 = new D43032();

#if DEBUG
         txtDate.DateTimeValue = DateTime.ParseExact("2018/10/11" , "yyyy/MM/dd" , null);
         this.Text += "(開啟測試模式),ocfDate=2018/10/11";
#endif
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            //設定初始年月yyyy/MM/dd
            txtDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
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

         #region 證券代號檢核
         if (gbItem.EditValue.AsString() == "rbSid") {
            if (string.IsNullOrEmpty(txtSid.Text)) {
               MessageDisplay.Error("請輸入單一證券代號!");
               return ResultStatus.Fail;
            }
         }
         #endregion

         try {
            string startDate = txtDate.Text.Replace("/" , "");
            DataTable dtTmp = dao43032.ExecuteStoredProcedure(startDate);

            //1. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "轉檔中...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //2. 條件篩選
            DataTable dt = new DataTable();
            if (gbItem.EditValue.AsString() == "rbFut") {
               dt = dtTmp.Filter("prod_type = 'F'");
            } else if (gbItem.EditValue.AsString() == "rbOpt") {
               dt = dtTmp.Filter("prod_type = 'O'");
            } else if (gbItem.EditValue.AsString() == "rbAll") {
               dt = dtTmp.Filter("prod_type in ('F','O')");
            } else if (gbItem.EditValue.AsString() == "rbSid") {
               dt = dtTmp.Filter("prod_type = '' and kind_id = ''");
            }

            //3. 模型代碼
            if (chkModel.CheckedItemsCount == 0) {
               MessageDisplay.Error("請勾選要匯出的報表!");
               return ResultStatus.Fail; ;
            }

            string modelType, modelName;

            flag = 0;
            foreach (CheckedListBoxItem item in chkModel.Items) {
               if (item.CheckState == CheckState.Unchecked) {
                  continue;
               }

               switch (item.Value) {
                  case "chkSma":
                     modelType = "S";
                     modelName = item.Description;
                     if (gbItem.EditValue.AsString() == "rbAdj") {
                        dt = dtTmp.Filter("flag_sma = 'Y'");
                     }
                     flag += wf_43032(modelType , modelName , dt);
                     break;
                  case "chkEwma":
                     modelType = "E";
                     modelName = item.Description;
                     if (gbItem.EditValue.AsString() == "rbAdj") {
                        dt = dtTmp.Filter("flag_ewma = 'Y'");
                     }
                     flag += wf_43032(modelType , modelName , dt);
                     break;
                  case "chkMaxVol":
                     modelType = "M";
                     modelName = item.Description;
                     if (gbItem.EditValue.AsString() == "rbAdj") {
                        dt = dtTmp.Filter("flag_maxvol = 'Y'");
                     }
                     flag += wf_43032(modelType , modelName , dt);
                     break;
               }
            }

            if (flag <= 0)
               return ResultStatus.Fail;

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

      private int wf_43032(string modelType , string modelName , DataTable dtTmp) {
         string rptName = "上市證券原始資料查詢";
         ShowMsg(string.Format("{0}-{1}轉檔中..." , _ProgramID , rptName));
         //string startDate = DateTime.Now.ToString("yyyyMMdd");

         try {

            if (dtTmp.Rows.Count <= 0) {
               MessageDisplay.Info(string.Format("{0},{1},{2}-{3},無任何資料!" ,
                                          txtDate.Text , modelName , _ProgramID , rptName));
               return 0;
            }
            string stockId, etfFileName;

            //逐每一商品/股票轉出資料
            foreach (DataRow dr in dtTmp.Rows) {
               string sDate = dr["ymd_fm"].AsString();
               string eDate = dr["ymd_to"].AsString();
               string prodType = dr["prod_type"].AsString();
               string kindId = dr["kind_id"].AsString();

               if (gbItem.EditValue.AsString() == "rbSid") {
                  stockId = txtSid.Text.Trim();
                  etfFileName = stockId + "_" + modelName + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
               } else {
                  stockId = dr["stock_id"].AsString(); //?
                  etfFileName = kindId + "_" + modelName + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
               }

               DataTable dtRes = dao43032.ExecuteStoredProcedure2(modelType , sDate , eDate , prodType , kindId , stockId);
               if (dtRes.Rows.Count <= 0) continue;

               //存CSV (ps:輸出csv 都用ascii)
               etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
               ExportOptions csvref = new ExportOptions();
               csvref.HasHeader = false;
               csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
               Common.Helper.ExportHelper.ToCsv(dtRes , etfFileName , csvref);

            }//foreach (DataRow dr in dtTmp.Rows)

            return 1;
         } catch (Exception ex) {
            WriteLog(ex);
         }
         return 0;
      }
   }
}