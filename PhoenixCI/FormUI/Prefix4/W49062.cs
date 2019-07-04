using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/04/08
/// </summary>
namespace PhoenixCI.FormUI.Prefix4 {
   /// <summary>
   /// 49062 每日國外保證金查詢
   /// </summary>
   public partial class W49062 : FormParent {

      private D49062 dao49062;

      public W49062(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         txtDate.DateTimeValue = DateTime.Now;
         txtStartDate.DateTimeValue = DateTime.Now;
         txtEndDate.DateTimeValue = DateTime.Now;
         txtStartDate2.DateTimeValue = DateTime.Now;
         txtEndDate2.DateTimeValue = DateTime.Now;

         dao49062 = new D49062();

#if DEBUG
         //Winni test
         //txtDate.DateTimeValue = DateTime.ParseExact("2018/10/11" , "yyyy/MM/dd" , null);
         //this.Text += "(開啟測試模式),ocfDate=2018/10/11";
#endif
      }

      protected override ResultStatus Open() {
         base.Open();
         try {

            //1. 設定初始年月
            //txtDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            txtDate.DateTimeValue = DateTime.ParseExact(DateTime.Now.ToString("yyyy/MM/dd") , "yyyy/MM/dd" , null);
            txtDate.EnterMoveNextControl = true;
            txtDate.Focus();

            txtStartDate.DateTimeValue = DateTime.ParseExact(DateTime.Now.ToString("yyyy/MM/dd") , "yyyy/MM/dd" , null);
            txtStartDate.EnterMoveNextControl = true;
            txtStartDate.Focus();

            txtEndDate.DateTimeValue = DateTime.ParseExact(DateTime.Now.ToString("yyyy/MM/dd") , "yyyy/MM/dd" , null);
            txtEndDate.EnterMoveNextControl = true;
            txtEndDate.Focus();

            txtStartDate2.DateTimeValue = DateTime.ParseExact(DateTime.Now.ToString("yyyy/MM/dd") , "yyyy/MM/dd" , null);
            txtStartDate2.EnterMoveNextControl = true;
            txtStartDate2.Focus();

            txtEndDate2.DateTimeValue = DateTime.ParseExact(DateTime.Now.ToString("yyyy/MM/dd") , "yyyy/MM/dd" , null);
            txtEndDate2.EnterMoveNextControl = true;
            txtEndDate2.Focus();

            //2. 設定dropdownlist
            //交易所 + 商品
            DataTable dtFId = dao49062.GetMgt8FIdList(); //mgt8_f_id/mgt8_f_name/mgt8_f_exchange/cp_display 4 fields
            dwFId.SetDataTable(dtFId , "MGT8_F_ID" , "CP_DISPLAY" , TextEditStyles.DisableTextEditor);
            dwFId.ItemIndex = 0;

            //契約種類
            DataTable dtKindId = dao49062.GetMgt8KindFIdList(); //kind_type/kind_name/cod_seq_no 3 fields 
            dwKindId.SetDataTable(dtKindId , "KIND_TYPE" , "KIND_NAME" , TextEditStyles.DisableTextEditor);
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
         labMsg.Visible = true;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export() {

         #region 日期檢核
         if (string.Compare(txtStartDate.Text , txtEndDate.Text) > 0) {
            MessageDisplay.Error(CheckDate.Datedif , GlobalInfo.ErrorText);
            labMsg.Visible = false;
            return ResultStatus.Fail;
         }

         if (string.Compare(txtStartDate2.Text , txtEndDate2.Text) > 0) {
            MessageDisplay.Error(CheckDate.Datedif , GlobalInfo.ErrorText);
            labMsg.Visible = false;
            return ResultStatus.Fail;
         }
         #endregion

         try {
            string rptName = "每日國外保證金查詢";
            string rptId;

            //1. ready
            //panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "轉檔中...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //2. 判斷為歷史資料還是生效日
            if (gbItem.EditValue.AsString() == "rbHistory") {
               rptId = "49062_Daily";
            } else {
               rptId = "49062";
            }
            ShowMsg(rptId + "-" + rptName + " 轉檔中...");

            //3.1 copy template xls to target path
            string excelDestinationPath = PbFunc.wf_copy_file(_ProgramID , rptId);

            //3.2 open xls
            Workbook workbook = new Workbook();
            workbook.LoadDocument(excelDestinationPath);
            Worksheet ws = workbook.Worksheets[0];

            //4. write data (wf_49062)
            string startDate = "", endDate = "";
            int seqNo = 0;

            string fId = dwFId.EditValue.AsString();
            if (string.IsNullOrEmpty(fId) || fId == "%")
               fId = "%";
            else
               fId += "%";
            string kindId = dwKindId.EditValue.AsString();
            if (string.IsNullOrEmpty(kindId) || kindId == "%")
               kindId = "%";
            else
               kindId += "%";

            if (gbItem.EditValue.AsString() == "rbNewDate") {
               startDate = "19010101";
               endDate = txtDate.DateTimeValue.ToString("yyyyMMdd");
               seqNo = 2;
            } else if (gbItem.EditValue.AsString() == "rbOldDate") {
               startDate = txtStartDate.DateTimeValue.ToString("yyyyMMdd");
               endDate = txtEndDate.DateTimeValue.ToString("yyyyMMdd");
               seqNo = 9999;
            } else if (gbItem.EditValue.AsString() == "rbHistory") {
               startDate = txtStartDate2.DateTimeValue.ToString("yyyyMMdd");
               endDate = txtEndDate2.DateTimeValue.ToString("yyyyMMdd");
               seqNo = 9999;
            }

            //SP return dt
            DataTable dt = new DataTable();
            if (gbItem.EditValue.AsString() == "rbHistory") {
               dt = dao49062.ExecuteStoredProcedure2(startDate , endDate , fId , kindId , seqNo);

               if (dt.Rows.Count <= 0) {
                  MessageDisplay.Info(string.Format("{0},{1},{2}-{3},無任何資料!" , startDate , endDate , rptId , rptName) , GlobalInfo.ResultText);
                  File.Delete(excelDestinationPath);
                  return ResultStatus.FailButNext;
               }

               var result = dt.AsEnumerable().OrderBy(x => {
                  switch (x.Field<string>("mgt8_foreign")) {
                     case "Y": return 1;
                  }
                  return 2;
               }).
                           ThenBy(x => {
                              switch (x.Field<string>("mgt8_amt_type")) {
                                 case "A": return 1;
                              }
                              return 2;
                           }).
                           ThenBy(x => x.Field<string>("f_name")).
                           ThenByDescending(x => x.Field<string>("detl_ymd"));

               dt = result.CopyToDataTable();

            } else {
               dt = dao49062.ExecuteStoredProcedure(startDate , endDate , fId , kindId , seqNo);

               if (dt.Rows.Count <= 0) {
                  MessageDisplay.Info(string.Format("{0},{1},{2}-{3},無任何資料!" , startDate , endDate , rptId , rptName) , GlobalInfo.ResultText);
                  File.Delete(excelDestinationPath);
                  return ResultStatus.FailButNext;
               }

               var result = dt.AsEnumerable().OrderBy(x => {
                  switch (x.Field<string>("mgt8_foreign")) {
                     case "Y": return 1;
                  }
                  return 2;
               }).
                           ThenBy(x => {
                              switch (x.Field<string>("mgt8_amt_type")) {
                                 case "A": return 1;
                              }
                              return 2;
                           }).
                           ThenBy(x => x.Field<string>("f_name")).
                           ThenByDescending(x => x.Field<string>("eff_ymd"));

               dt = result.CopyToDataTable();
            }

            int tmpRow = 0, rowNum = 0;
            int li_b1 = 3, li_b2 = 1003, li_b3 = 2003;
            int li_b1_end = 1003, li_b2_end = 2003, li_b3_end = 3003;
            for (int w = 0 ; w < dt.Rows.Count ; w++) {
               DataRow dr = dt.Rows[w];
               string foreign = dr["mgt8_foreign"].AsString();
               string amtType = dr["mgt8_amt_type"].AsString();
               if (foreign == "Y") {
                  if (amtType == "A") {
                     li_b1++;
                     tmpRow = li_b1;
                     //20190702 取消只顯示1000筆的限制
                     //if (li_b1 == li_b1_end + 1) {
                     //   MessageDisplay.Warning("國外「金額類型」保證金資料超過1000筆，只顯示1000筆資料！",GlobalInfo.WarningText);
                     //}
                     //if (li_b1 > li_b1_end) {
                     //   continue;
                     //}
                  } else if (amtType == "P") {
                     li_b2++;
                     tmpRow = li_b2;
                     //20190702 取消只顯示1000筆的限制
                     //if (li_b2 == li_b2_end + 1) {
                     //   MessageDisplay.Warning("國外「比例類型」保證金資料超過1000筆，只顯示1000筆資料！",GlobalInfo.WarningText);
                     //}
                     //if (li_b2 > li_b2_end) {
                     //   continue;
                     //}
                  }
               } else {
                  li_b3++;
                  tmpRow = li_b3;
               }

               //選最近及次新生效日資料時，次新的欄位顯示空白
               int tmpSeqNo = dr["seq_no"].AsInt();
               string fName = dr["f_name"].AsString();
               //decimal cPrice = dr["c_price"].AsDecimal();
               rowNum = tmpRow - 1;
               if (gbItem.EditValue.AsString() == "rbNewDate" && tmpSeqNo == 2) {
                  ws.Cells[rowNum , 0].Value = "";
               } else {
                  ws.Cells[rowNum , 0].Value = fName;
               }

               string ymd = "";
               if (gbItem.EditValue.AsString() == "rbHistory") {
                  try {
                     ymd = DateTime.ParseExact(dr["detl_ymd"].AsString() , "yyyyMMdd" , null).ToString("yyyy/MM/dd");
                  } catch (Exception ex) {
                     WriteLog(ex);
                     MessageDisplay.Error("日期轉換失敗" , GlobalInfo.ErrorText);
                     return ResultStatus.FailButNext;
                  }
               } else {
                  try {
                     ymd = DateTime.ParseExact(dr["eff_ymd"].AsString() , "yyyyMMdd" , null).ToString("yyyy/MM/dd");
                  } catch (Exception ex) {
                     WriteLog(ex);
                     MessageDisplay.Error("日期轉換失敗" , GlobalInfo.ErrorText);
                     return ResultStatus.FailButNext;
                  }
               }

               ws.Cells[rowNum , 1].Value = DateTime.ParseExact(ymd , "yyyy/MM/dd" , null);
               ws.Cells[rowNum , 2].Value = dr["currency_name"].AsString();

               if (dr["mg8_cm"] != DBNull.Value)
                  ws.Cells[rowNum , 3].Value = dr["mg8_cm"].AsDecimal();
               if (dr["cm_rate"] != DBNull.Value)
                  ws.Cells[rowNum , 4].Value = dr["cm_rate"].AsDecimal();
               if (dr["mg8_mm"] != DBNull.Value)
                  ws.Cells[rowNum , 5].Value = dr["mg8_mm"].AsDecimal();
               if (dr["mm_rate"] != DBNull.Value)
                  ws.Cells[rowNum , 6].Value = dr["mm_rate"].AsDecimal();
               if (dr["mg8_im"] != DBNull.Value)
                  ws.Cells[rowNum , 7].Value = dr["mg8_im"].AsDecimal();
               if (dr["im_rate"] != DBNull.Value)
                  ws.Cells[rowNum , 8].Value = dr["im_rate"].AsDecimal();
               ws.Cells[rowNum , 9].Value = dr["mgt8_xxx"].AsInt();
               ws.Cells[rowNum , 10].Value = dr["rate1"].AsDecimal();
               ws.Cells[rowNum , 11].Value = dr["rate2"].AsDecimal();
               ws.Cells[rowNum , 12].Value = dr["m_structure"].AsString();

               if (dr["c_price"] != DBNull.Value)
                  ws.Cells[rowNum , 14].Value = dr["c_price"].AsDecimal();

               //選最近及次新生效日資料時，若只有1筆則重覆顯示至第2筆 (by慈昕)
               if (gbItem.EditValue.AsString() == "rbNewDate" && tmpSeqNo == 1) {
                  string nextFName = "";
                  if (w < dt.Rows.Count - 1) {
                     nextFName = dt.Rows[w + 1]["f_name"].AsString();
                  }
                  if (fName != nextFName) {
                     dr["seq_no"] = 2;
                     w--;
                  }
               }


            }//for (int w = 0 ; w < dt.Rows.Count ; w++ )

            //刪除空白列
            if (li_b3 + 1 <= li_b3_end) {
               Range ra1 = ws.Range[li_b3 + 1 + ":" + li_b3_end];
               ra1.Delete(DeleteMode.EntireRow);
               ws.Range["A1"].Select();
            }
            if (li_b2 + 1 <= li_b2_end - 1) {
               Range ra2 = ws.Range[li_b2 + 1 + ":" + li_b2_end];
               ra2.Delete(DeleteMode.EntireRow);
               ws.Range["A1"].Select();
            }
            if (li_b1 + 1 <= li_b1_end - 1) {
               Range ra3 = ws.Range[li_b1 + 1 + ":" + li_b1_end];
               ra3.Delete(DeleteMode.EntireRow);
               ws.Range["A1"].Select();
            }

            //5.save
            workbook.SaveDocument(excelDestinationPath);

            labMsg.Visible = false;

            return ResultStatus.Success;
         } catch (Exception ex) {
            WriteLog(ex);
         } finally {
            //panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
            this.Cursor = Cursors.Arrow;
         }
         return ResultStatus.Fail;
      }
   }
}