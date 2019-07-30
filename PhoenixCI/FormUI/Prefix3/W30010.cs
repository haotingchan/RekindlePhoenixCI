using BaseGround;
using BaseGround.Shared;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Lukas, 2019/4/2
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30010 市場動態報導
   /// </summary>
   public partial class W30010 : FormParent {
      private OCFG daoOCFG;
      private RPT daoRPT;
      private D30010 dao30010;
      private int flag;

      public W30010(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         dao30010 = new D30010();
      }

      protected override ResultStatus Open() {
         base.Open();
         //日期
         txtSDate.DateTimeValue = GlobalInfo.OCF_DATE;

         //盤別下拉選單
         //List<LookupItem> ddlb_grp = new List<LookupItem>(){
         //                               new LookupItem() { ValueMember = "1", DisplayMember = "16:15收盤"},
         //                               new LookupItem() { ValueMember = "2", DisplayMember = "全部收盤" }};
         DataTable ddlb_grp = new CODW().ListLookUpEdit("GRP" , "GRP_NO");
         Extension.SetDataTable(ddlType , ddlb_grp , "CODW_ID" , "CODW_DESC" , TextEditStyles.DisableTextEditor , "");
         ddlType.ItemIndex = 1;

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen() {
         try {
            //決定盤別下拉選單
            daoOCFG = new OCFG();
            if (daoOCFG.f_get_txn_osw_grp(_ProgramID) == "5") {
               ddlType.EditValue = "1";
            } else {
               ddlType.EditValue = "2";
            }

         } catch (Exception ex) {
            throw ex;
         }
         return ResultStatus.Success;
      }

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

         return ResultStatus.Success;
      }

      protected void ShowMsg(string msg) {
         labMsg.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export() {
         try {
            //ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            string rptId, file, rptName = "",
                     cpYmd = txtSDate.DateTimeValue.ToString("yyyyMMdd");


            #region ue_export_before
            //判斷盤別
            int rtnInt, seq;
            string rtnStr, grp;
            if (ddlType.Text == "16:15收盤") {
               grp = "1";
               DialogResult result = MessageDisplay.Choose("盤別為「16:15收盤」，請問是否繼續轉出報表？" , MessageBoxDefaultButton.Button2 , GlobalInfo.QuestionText);
               if (result == DialogResult.No) {
                  ShowMsg("已取消轉檔...");
                  return ResultStatus.Fail;
               }
            } else {
               grp = "2";
            }

            //判斷統計資料轉檔已完成
            for (int f = 1 ; f <= 2 ; f++) {
               if (grp == "1") {
                  if (f == 1) {
                     seq = 13;
                  } else {
                     seq = 23;
                  }
               } else {
                  seq = 17;
                  f = 2;
               }
               //check JSW
               rtnStr = PbFunc.f_get_jsw_seq(_ProgramID , "E" , seq , txtSDate.DateTimeValue , "0");
               if (rtnStr != "") {
                  DialogResult result = MessageDisplay.Choose(" 統計資料未轉入完畢,是否要繼續?" , MessageBoxDefaultButton.Button2 , GlobalInfo.QuestionText);
                  if (result == DialogResult.No) {
                     return ResultStatus.Fail;
                  }
               }
            }

            //判斷20110作業已完成
            rtnInt = dao30010.check20110(txtSDate.Text);
            if (rtnInt == 0) {
               DialogResult result = MessageDisplay.Choose("無 " + txtSDate.Text + " 現貨資料 (資料來自20110作業)，" +
                  Environment.NewLine + "請問是否繼續轉出報表？" , MessageBoxDefaultButton.Button2 , GlobalInfo.QuestionText);
               if (result == DialogResult.No) {
                  ShowMsg("已取消轉檔...");
                  return ResultStatus.Fail;
               }
            }
            #endregion

            rptId = "30010_";

            //複製檔案
            file = wfCopy30010(rptId + grp , grp);
            if (file == "") return ResultStatus.Fail;

            //開啟檔案
            Workbook workbook = new Workbook();
            workbook.LoadDocument(file);
            flag = 0;

            //切換Sheet
            Worksheet ws30011 = workbook.Worksheets["30011"];

            /* Sheet:30011 */
            wf_30012(rptId , rptName , ws30011);
            wf_30011(rptId , rptName , ws30011);

            /* Sheet:30013 */
            //切換Sheet
            Worksheet ws30013 = workbook.Worksheets["30013"];

            DataTable dtRowCount = dao30010.getRowIndexandCount();
            if (dtRowCount.Rows.Count == 0) {
               MessageDisplay.Error("無法取得30013總列數");
               return ResultStatus.Fail;
            }
            int totalRowcount = dtRowCount.Rows[0]["LI_TOT_ROWCOUNT"].AsInt();

            //上市股票
            int rowIndex = dtRowCount.Rows[0]["II_OLE_ROW"].AsInt() - 1;
            if (rowIndex > 0) {
               rowIndex = wf_30013("STF" , "F" , totalRowcount , "1" , rowIndex , ws30013);
            }
            //上櫃股票
            rowIndex = rowIndex + 2;
            if (rowIndex > 0) {
               rowIndex = wf_30013("STF" , "F" , totalRowcount , "2" , rowIndex , ws30013);
            }
            //ETF股票
            rowIndex = rowIndex + 2;
            if (rowIndex > 0) {
               rowIndex = wf_30013("ETF" , "F" , totalRowcount , "%" , rowIndex , ws30013);
            }
            //上市選擇權
            rowIndex = rowIndex + 2;
            if (rowIndex > 0) {
               rowIndex = wf_30013("STC" , "O" , totalRowcount , "1" , rowIndex , ws30013);
            }
            //上櫃選擇權
            rowIndex = rowIndex + 2;
            if (rowIndex > 0) {
               rowIndex = wf_30013("STC" , "O" , totalRowcount , "2" , rowIndex , ws30013);
            }
            //ETF選擇權
            rowIndex = rowIndex + 2;
            if (rowIndex > 0) {
               rowIndex = wf_30013("ETC" , "O" , totalRowcount , "%" , rowIndex , ws30013);
            }

            /* Sheet:30014 */
            Worksheet ws30014 = workbook.Worksheets["30014"];

            wf_30014(ws30014);

            wf_30015(ws30014);

            //Eurex
            wf_30016(ws30014);

            if (flag <= 0) {
               File.Delete(file);
               return ResultStatus.Fail;
            }

            //存檔
            ws30014.ScrollToRow(0);
            ws30013.ScrollToRow(0);
            ws30011.ScrollToRow(0);
            workbook.SaveDocument(file);
            ShowMsg("轉檔完成");
            return ResultStatus.Success;

         } catch (Exception ex) {
            MessageDisplay.Error("輸出錯誤");
            WriteLog(ex);
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
            this.Cursor = Cursors.Arrow;
         }
         return ResultStatus.Fail;
      }

      /// <summary>
      /// 期貨市場動態報導－選擇權
      /// </summary>
      /// <param name="rptId"></param>
      /// <param name="rptName"></param>
      /// <param name="ws30011"></param>
      private void wf_30012(string rptId , string rptName , Worksheet ws30011) {
         try {
            string kindID = "", settleDate = "", kindID2 = "";
            int? rowIndex = null;
            int txwCnt = 0, txwRow = 0;
            Range delRange;
            rptName = "期貨市場動態報導－選擇權";
            rptId = "30012";
            ShowMsg(rptId + "－" + rptName + " 轉檔中...");

            //讀取資料
            DataTable dt30012 = dao30010.d_30012(txtSDate.DateTimeValue);
            if (dt30012.Rows.Count == 0) {
               MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
               labMsg.Visible = false;
               return;
            }
            flag++;

            //填資料
            foreach (DataRow dr in dt30012.Rows) {
               if (kindID != dr["AMIF_PARAM_KEY"].AsString()) {
                  kindID = dr["AMIF_PARAM_KEY"].AsString();
                  settleDate = dr["AMIF_SETTLE_DATE"].AsString();
                  if (dr["RPT_SEQ_NO"] != DBNull.Value) rowIndex = dr["RPT_SEQ_NO"].AsInt() - 1;
                  rowIndex = rowIndex;
                  if (kindID == "TXW") {
                     kindID2 = dr["AMIF_KIND_ID"].AsString();
                     if (rowIndex != null) txwRow = rowIndex.AsInt();
                     txwCnt = txwCnt + 1;
                  }
               } else {
                  if (kindID2 != dr["AMIF_KIND_ID"].AsString()) {
                     kindID2 = dr["AMIF_KIND_ID"].AsString();
                     if (kindID == "TXW") {
                        txwCnt = txwCnt + 1;
                        if (settleDate == dr["AMIF_SETTLE_DATE"].AsString()) {
                           rowIndex = rowIndex + 1;
                        }
                     }
                  }
               }
               if (rowIndex == null) continue;
               int row = rowIndex.AsInt();
               if (settleDate != dr["AMIF_SETTLE_DATE"].AsString()) {
                  settleDate = dr["AMIF_SETTLE_DATE"].AsString();
                  row = row + 1;
                  rowIndex = rowIndex + 1;
               }
               if (dr["AMIF_EXPIRY_TYPE"].AsString() == "W") {
                  ws30011.Cells[row , 1].Value = settleDate.SubStr(settleDate.Length - 2 , 2).AsInt() + "W" + dr["AMIF_KIND_ID"].AsString().SubStr(2 , 1);
               } else {
                  ws30011.Cells[row , 1].Value = settleDate.SubStr(settleDate.Length - 2 , 2).AsInt();
               }
               if (dr["AMIF_PC_CODE"].AsString() == "C") {
                  if (dr["M_QNTY"] != DBNull.Value) ws30011.Cells[row , 2].Value = dr["M_QNTY"].AsDecimal();
                  if (dr["OPEN_INTEREST"] != DBNull.Value) ws30011.Cells[row , 4].Value = dr["OPEN_INTEREST"].AsDecimal();
               } else {
                  if (dr["M_QNTY"] != DBNull.Value) ws30011.Cells[row , 6].Value = dr["M_QNTY"].AsDecimal();
                  if (dr["OPEN_INTEREST"] != DBNull.Value) ws30011.Cells[row , 8].Value = dr["OPEN_INTEREST"].AsDecimal();
               }
            }//foreach (DataRow dr in dt30012.Rows)
            if (txwCnt == 0) {
               txwRow = dao30010.getTxwRow() - 1;
               //delRange = ws30011.Range[(li_txw_row).ToString() + ":" + (li_txw_row + 1).ToString()];
               ws30011.Rows.Hide(txwRow , txwRow + 1);
               //delRange.Delete(DeleteMode.EntireRow);
            } else if (txwCnt < 2) {
               //delRange = ws30011.Rows[(li_txw_row + 1).ToString()];
               ws30011.Rows.Hide(txwRow + 1 , txwRow + 1);
               //delRange.Delete(DeleteMode.EntireRow);
            }
         } catch (Exception ex) {
            throw ex;
         }
      }

      /// <summary>
      /// 期貨市場動態報導－期貨
      /// </summary>
      /// <param name="rptId"></param>
      /// <param name="rptName"></param>
      /// <param name="ws30011"></param>
      private void wf_30011(string rptId , string rptName , Worksheet ws30011) {
         try {
            rptName = "期貨市場動態報導－期貨";
            rptId = "30011";
            ShowMsg(rptId + "－" + rptName + " 轉檔中...");

            string kindID = "", settleDate = "", index = "", indexStr = "", kindID2 = "";
            int rowIndex = 0, mxwCnt = 0, mxwRow = 0, rowCnt = 0;
            decimal value, mQnty, value2;
            Range delRange;
            indexStr = "000000";

            ws30011.Cells[0 , 10].Value = "民國" + (txtSDate.DateTimeValue.Year - 1911)
                  + "年" + txtSDate.DateTimeValue.Month + "月" + txtSDate.DateTimeValue.Day + "日";

            //讀取資料
            DataTable dt30011 = dao30010.d_30011(txtSDate.DateTimeValue);
            if (dt30011.Rows.Count == 0) {
               MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
               labMsg.Visible = false;
               return;
            }

            //RPT
            daoRPT = new RPT();
            DataTable dtRPT = daoRPT.ListAllByTXD_ID(rptId);
            if (dtRPT.Rows.Count == 0) {
               MessageDisplay.Info(rptId + '－' + "RPT無任何資料!");
               labMsg.Visible = false;
               return;
            }
            flag++;

            //填資料
            mxwCnt = 0;
            foreach (DataRow dr in dt30011.Rows) {
               mQnty = dr["AMIF_M_QNTY_TAL"].AsDecimal();
               if (kindID2 != dr["AMIF_KIND_ID2"].AsString()) {
                  kindID2 = dr["AMIF_KIND_ID2"].AsString();
                  rowIndex = dr["RPT_SEQ_NO"].AsInt() - 1 - 1;
                  rowCnt = 0;
                  index = dr["RPT_VALUE_3"].AsString();
               }
               kindID = dr["AMIF_KIND_ID"].AsString();
               rowIndex = rowIndex + 1;
               if (kindID2 == "MXW") {
                  mxwRow = rowIndex;
                  mxwCnt = mxwCnt + 1;
               }
               settleDate = dr["AMIF_SETTLE_DATE"].AsString();
               if (settleDate == indexStr) {
                  settleDate = "指數";
               } else {
                  if (kindID == "STW" && rowCnt >= 2) {
                     rowCnt = rowCnt + 1;
                     continue;
                  }
                  /* 第一筆不是指數,則跳一列 */
                  if (rowCnt == 0 && index == indexStr) {
                     rowIndex = rowIndex + 1;
                     rowCnt = rowCnt + 1;
                  }
                  if (dr["AMIF_EXPIRY_TYPE"].AsString() == "W") {
                     ws30011.Cells[rowIndex , 1].Value = settleDate.SubStr(settleDate.Length - 2 , 2).AsInt() + "W" + kindID.SubStr(2 , 1);
                  } else {
                     ws30011.Cells[rowIndex , 1].Value = settleDate.SubStr(settleDate.Length - 2 , 2).AsInt();
                  }
               }
               value = dr["AMIF_OPEN_PRICE"].AsDecimal();
               if ((value != 0 && mQnty > 0) || (settleDate == "指數" && value != 0)) {
                  ws30011.Cells[rowIndex , 2].Value = value;
               }
               value = dr["AMIF_HIGH_PRICE"].AsDecimal();
               if (value != 0) {
                  ws30011.Cells[rowIndex , 3].Value = value;
               }
               value = dr["AMIF_LOW_PRICE"].AsDecimal();
               if (value != 0 || (settleDate == "指數" && value != 0)) {
                  ws30011.Cells[rowIndex , 4].Value = value;
               }
               value = dr["AMIF_CLOSE_PRICE"].AsDecimal();
               if ((value != 0 && mQnty > 0) || (settleDate == "指數" && value != 0)) {
                  ws30011.Cells[rowIndex , 5].Value = value;
               }
               value2 = dr["AMIF_UP_DOWN_VAL"].AsDecimal();
               if ((value != 0 && mQnty > 0) || (settleDate == "指數" && value != 0)) {
                  ws30011.Cells[rowIndex , 6].Value = value2;
                  if ((dr["AMIF_CLOSE_PRICE"].AsDecimal() - dr["AMIF_UP_DOWN_VAL"].AsDecimal()) == 0) {
                     MessageDisplay.Error(kindID + " 收盤價-漲跌幅=0 ,計算漲跌點%造成除數為0");
                     return;
                  }
                  value2 = Math.Round((dr["AMIF_UP_DOWN_VAL"].AsDecimal() / (dr["AMIF_CLOSE_PRICE"].AsDecimal() - dr["AMIF_UP_DOWN_VAL"].AsDecimal())) * 100 ,
                                          3 , MidpointRounding.AwayFromZero);
                  ws30011.Cells[rowIndex , 7].Value = value2;
               }
               ws30011.Cells[rowIndex , 8].SetValue(dr["AMIF_M_QNTY_TAL"]);
               if (settleDate != "指數") {
                  if (kindID.SubStr(0 , 3) != "STW") {
                     ws30011.Cells[rowIndex , 9].SetValue(dr["AMIF_SETTLE_PRICE"]);
                  }
                  ws30011.Cells[rowIndex , 10].SetValue(dr["AMIF_OPEN_INTEREST"]);
               }
               rowCnt = rowCnt + 1;
            }//foreach (DataRow dr in dt30011.Rows)
            if (mxwCnt == 0) {
               mxwRow = dao30010.getMxwRow() - 1;
               //delRange = ws30011.Range[(li_mxw_row).ToString() + ":" + (li_mxw_row + 1).ToString()];
               //delRange.Delete(DeleteMode.EntireRow);
               ws30011.Rows.Hide(mxwRow , mxwRow + 1);
            } else if (mxwCnt < 2) {
               //delRange = ws30011.Rows[(li_mxw_row + 1).ToString()];
               //delRange.Delete(DeleteMode.EntireRow);
               ws30011.Rows.Hide(mxwRow + 1 , mxwRow + 1);
            }
         } catch (Exception ex) {
            throw ex;
         }
      }

      /// <summary>
      /// 期貨市場動態報導－股票選擇權
      /// </summary>
      /// <param name="aiPdkParamKey"></param>
      /// <param name="prodType"></param>
      /// <param name="aiTotalRowcount"></param>
      /// <param name="asPdkUnderlyingMarket"></param>
      /// <param name="rowIndex"></param>
      /// <param name="ws30013"></param>
      private int wf_30013(string aiPdkParamKey , string prodType , int aiTotalRowcount , string asPdkUnderlyingMarket ,
                            int rowIndex , Worksheet ws30013) {
         try {
            string rptName, rptId;
            string pdkName;
            int colAdd, rowStart, totalRowcount;
            Range delRange;

            rptName = "期貨市場動態報導－股票選擇權";
            rptId = "30013";
            ShowMsg(rptId + "－" + rptName + " 轉檔中...");

            //讀取資料
            DataTable dt30013 = dao30010.d_30013(txtSDate.DateTimeValue , aiPdkParamKey , prodType , asPdkUnderlyingMarket);
            colAdd = 0;
            rowStart = rowIndex;
            totalRowcount = rowStart + Math.Ceiling(aiTotalRowcount.AsDecimal() / 2).AsInt();
            if (dt30013.Rows.Count == 0) {
               //全部刪除
               rowIndex = rowIndex - 2;
               delRange = ws30013.Range[(rowIndex + 2).ToString() + ":" + (totalRowcount + 2).ToString()];
               delRange.Delete(DeleteMode.EntireRow);
               return rowIndex;
            }

            /* 列數在B1 */
            for (int f = 0 ; f < dt30013.Rows.Count ; f++) {
               DataRow dr = dt30013.Rows[f];
               if (Math.IEEERemainder(f , 2) == 0) {
                  colAdd = 0;
               } else {
                  colAdd = 4;
                  rowIndex = rowIndex - 1;
               }
               rowIndex = rowIndex + 1;
               /* 標的名稱:不要"選擇權" */
               pdkName = dr["PDK_NAME"].AsString();
               ws30013.Cells[rowIndex , 1 + colAdd].Value = dr["PDK_STOCK_ID"].AsString() + "(" + dr["AMIF_KIND_ID"].AsString() + ")";
               ws30013.Cells[rowIndex , 2 + colAdd].Value = pdkName;

               if (dr["AMIF_DATA_SOURCE"].AsString() == "P") {
                  ws30013.Cells[rowIndex , 3 + colAdd].Value = "停止交易";
               } else {
                  ws30013.Cells[rowIndex , 3 + colAdd].SetValue(dr["M_QNTY"]);
               }
               ws30013.Cells[rowIndex , 4 + colAdd].SetValue(dr["OPEN_INTEREST"]);
            }
            rowStart = rowIndex;

            //刪除空白列
            //rowStart = rowStart + 5;
            if (totalRowcount > rowStart) {
               delRange = ws30013.Range[(rowStart + 1 + 1).ToString() + ":" + (totalRowcount + 1).ToString()];
               delRange.Delete(DeleteMode.EntireRow);
            }
            rowIndex = rowStart + 1;
            return rowIndex;
         } catch (Exception ex) {
            throw ex;
         }
      }

      /// <summary>
      /// 期貨市場動態報導－交易量表
      /// </summary>
      /// <param name="ws30014"></param>
      private void wf_30014(Worksheet ws30014) {
         string rptName, rptId, date;
         int rowIndex;
         decimal value;
         rptName = "期貨市場動態報導－交易量表";
         rptId = "30014";
         ShowMsg(rptId + "－" + rptName + " 轉檔中...");

         //讀取資料
         DataTable dt30014 = dao30010.d_30014(txtSDate.DateTimeValue);
         if (dt30014.Rows.Count == 0) {
            MessageDisplay.Info(txtSDate.Text + "," + rptId + '－' + rptName + ",無任何資料!");
            labMsg.Visible = false;
            return;
         }
         flag++;

         //填資料
         foreach (DataRow dr in dt30014.Rows) {
            rowIndex = dr["RPT_SEQ_NO"].AsInt() - 1;
            if (rowIndex < 0) continue;
            ws30014.Cells[rowIndex , 3].SetValue(dr["AI1_M_QNTY"]);
            ws30014.Cells[rowIndex , 4].SetValue(dr["M_INCREASE"]);
            ws30014.Cells[rowIndex , 5].SetValue(dr["AI1_OI"]);
            ws30014.Cells[rowIndex , 6].SetValue(dr["OI_INCREASE"]);
            ws30014.Cells[rowIndex , 7].SetValue(dr["AI1_AVG_MONTH"]);
            ws30014.Cells[rowIndex , 8].SetValue(dr["AI1_AVG_YEAR"]);
            ws30014.Cells[rowIndex , 9].SetValue(dr["AI1_HIGH_QNTY"]);
            date = dr["AI1_HIGH_DATE"].AsDateTime().ToString("yyyy.MM.dd");
            date = (date.SubStr(0 , 4).AsInt() - 1911) + date.SubStr(4 , 6);
            ws30014.Cells[rowIndex , 10].Value = date;
         }
      }

      /// <summary>
      /// 期貨市場動態報導－開戶數
      /// </summary>
      /// <param name="ws30014"></param>
      private void wf_30015(Worksheet ws30014) {
         string rptName, rptId, date;
         int rowIndex;
         decimal value;
         DateTime ldtDate, ldDateFm;
         rptName = "期貨市場動態報導－開戶數";
         rptId = "30015";
         ShowMsg(rptId + "－" + rptName + " 轉檔中...");

         ldtDate = txtSDate.DateTimeValue;
         ldDateFm = PbFunc.relativedate(txtSDate.DateTimeValue , -365);
         ldtDate = dao30010.checkPreviousData(ldtDate , ldDateFm);

         //讀取資料
         DataTable dt30015 = dao30010.d_30015(ldtDate);
         if (dt30015.Rows.Count == 0) {
            MessageDisplay.Info(ldtDate.ToString("yyyy/MM/dd") + "(前營業日)開戶資料未轉入(功能28610)，或請轉入後重新執行此功能!");
            labMsg.Visible = false;
            return;
         }
         flag++;

         //填資料
         /* 只會有1筆 */
         DataRow dr = dt30015.Rows[0];
         rowIndex = dao30010.get30015Row() - 1;
         ws30014.Cells[rowIndex , 2].SetValue(dr["AB3_COUNT"]);
         ws30014.Cells[rowIndex , 4].SetValue(dr["AB3_INCREASE"]);
         ws30014.Cells[rowIndex , 6].SetValue(dr["AB3_COUNT1"]);
         ws30014.Cells[rowIndex , 8].SetValue(dr["AB3_COUNT2"]);
         ws30014.Cells[rowIndex , 9].Value = dr["AB3_DATE"].AsDateTime().ToString("MM月dd日");
         ws30014.Cells[rowIndex , 10].SetValue(dr["AB3_TRADE_COUNT"]);

         //成交值
         rowIndex = rowIndex + 4;
         ldtDate = txtSDate.DateTimeValue;
         decimal amt1;
         date = txtSDate.DateTimeValue.ToString("yyyyMMdd");
         amt1 = dao30010.get30015Amt_1(date);
         amt1 = Math.Round(amt1 / 100000000 / 2 , 2 , MidpointRounding.AwayFromZero);
         ws30014.Cells[rowIndex , 0].Value = amt1;

         decimal amt2;
         amt2 = dao30010.get30015Amt_2(ldtDate); //PB這邊看起來若傳回的值為0，就跟前面的值會一樣
         if (amt2 == 0) {
            amt2 = amt1;
         }
         ws30014.Cells[rowIndex , 3].Value = amt2;
      }

      /// <summary>
      /// Eurex/TAIFEX 合作商品概況
      /// </summary>
      /// <param name="ws30014"></param>
      private void wf_30016(Worksheet ws30014) {
         string rptName, rptId, date;
         int rowIndex;
         decimal value;
         rptName = "Eurex/TAIFEX 合作商品概況";
         rptId = "30016";
         ShowMsg(rptId + "－" + rptName + " 轉檔中...");

         //讀取前一交易日
         date = txtSDate.DateTimeValue.ToString("yyyyMMdd");
         date = dao30010.checkPreviousDay(date).ToString("yyyy/MM/dd");
         if (dao30010.checkPreviousDay(date) == DateTime.MinValue) {
            date = txtSDate.Text;
         }

         //讀取資料
         DataTable dt30016 = dao30010.d_30016(txtSDate.DateTimeValue);
         if (dt30016.Rows.Count == 0) {
            labMsg.Visible = false;
            return;
         }
         flag++;

         //填資料
         int add = 55 - 1;
         ws30014.Cells[add + 1 , 9].Value = "民國" + (date.SubStr(0 , 4).AsInt() - 1911) + "年" + date.SubStr(5 , 2) + "月" + date.SubStr(8 , 2) + "日";
         foreach (DataRow dr in dt30016.Rows) {
            rowIndex = dr["RPT_SEQ_NO"].AsInt() + add;
            if (rowIndex == 0) continue;
            ws30014.Cells[rowIndex , 3].SetValue(dr["AE3_M_QNTY"]);
            ws30014.Cells[rowIndex , 4].SetValue(dr["M_INCREASE"]);
            ws30014.Cells[rowIndex , 5].SetValue(dr["AE3_OI"]);
            ws30014.Cells[rowIndex , 6].SetValue(dr["OI_INCREASE"]);
            ws30014.Cells[rowIndex , 7].SetValue(dr["AE3_AVG_MONTH"]);
            ws30014.Cells[rowIndex , 8].SetValue(dr["AE3_AVG_YEAR"]);
            ws30014.Cells[rowIndex , 9].SetValue(dr["AE3_HIGH_QNTY"]);
            date = dr["AE3_HIGH_DATE"].AsDateTime().ToString("yyyy.MM.dd");
            date = (date.SubStr(0 , 4).AsInt() - 1911) + date.SubStr(4 , 6);
            ws30014.Cells[rowIndex , 10].Value = date;
         }
      }

      /// <summary>
      /// 這支功能PB覆寫公用的wf_copyfile
      /// </summary>
      /// <param name="fileName"></param>
      /// <param name="grp"></param>
      /// <returns></returns>
      private string wfCopy30010(string fileName , string grp) {

         string template = Path.Combine(GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH , fileName + ".xlsx");
         if (!File.Exists(template)) {
            MessageDisplay.Error("無此檔案「" + template + "」!");
            return "";
         }
         string lsFilename;
         lsFilename = "動態報導" + (txtSDate.DateTimeValue.Year - 1911) + "." +
                      txtSDate.DateTimeValue.Month + "." + txtSDate.DateTimeValue.Day;
         if (grp == "1") {
            lsFilename = lsFilename + "(16時15分收盤)" + ".xlsx";
         } else {
            lsFilename = lsFilename + "(全部收盤)" + ".xlsx";
         }
         bool lbChk;
         string file = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , lsFilename);
         lbChk = File.Exists(file);
         if (lbChk) {
            File.Move(file , file + "_bak_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".xlsx");
         }
         try {
            File.Copy(template , file , false);
         } catch {
            MessageDisplay.Error("複製「" + template + "」到「" + file + "」檔案錯誤!");
            return "";
         }
         lsFilename = file;
         return lsFilename;
      }
   }
}