﻿using BaseGround;
using BusinessObjects.Enums;
using BaseGround.Report;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DataObjects.Dao.Together;
using BaseGround.Shared;
using DevExpress.XtraEditors.Controls;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using DevExpress.XtraLayout.Utils;
using System.IO;
using DevExpress.XtraReports.UI;
using static BaseGround.Report.ReportHelper;
using System.Collections.Generic;
using DevExpress.XtraPrinting;
using System;
using System.Linq;
using DevExpress.Spreadsheet;
using System.Drawing;

namespace PhoenixCI.FormUI.Prefix5
{
   public partial class W50032 : FormParent
   {
      public D500xx _D500Xx { get; set; }
      private DataTable _Data { get; set; }
      private defReport _defReport;
      private ABRK daoABRK;
      private APDK daoAPDK;
      private D50032 dao50032;

      public W50032(string programID, string programName) : base(programID, programName)
      {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;
         daoABRK = new ABRK();
         daoAPDK = new APDK();
         _D500Xx = new D500xx();
         dao50032 = new D50032();
      }

      private bool StartRetrieve(string sbrkno = "", string ebrkno = "")
      {
         /*******************
         條件值檢核
         *******************/
         _D500Xx.IsCheck = "N";
         /*造市者代號 */
         _D500Xx.Sbrkno = dwSbrkno.EditValue.AsString();
         if (string.IsNullOrEmpty(_D500Xx.Sbrkno)) {
            _D500Xx.Sbrkno = sbrkno;
         }
         _D500Xx.Ebrkno = dwEbrkno.EditValue.AsString();
         if (string.IsNullOrEmpty(_D500Xx.Ebrkno)) {
            _D500Xx.Ebrkno = ebrkno;
         }
         if ((string.Compare(dwSbrkno.SelectedText, dwEbrkno.SelectedText) > 0) && !string.IsNullOrEmpty(_D500Xx.Ebrkno)) {
            PbFunc.messageBox(GlobalInfo.ErrorText, "造市者代號起始不可大於迄止", MessageBoxIcon.Stop);

            dwEbrkno.Focus();
            _D500Xx.IsCheck = "Y";
            return false;
         }

         /* 商品群組 */
         _D500Xx.ProdCategory = dwProdCt.EditValue.AsString();
         if (string.IsNullOrEmpty(_D500Xx.ProdCategory) || dwProdCt.Enabled == false) {
            _D500Xx.ProdCategory = "";
         }
         _D500Xx.ProdCategory = _D500Xx.ProdCategory.AsString() + "%";

         /* 商品 */
         _D500Xx.ProdKindId = "";
         _D500Xx.ProdKindIdSto = dwProdKdSto.EditValue.AsString();
         if (string.IsNullOrEmpty(_D500Xx.ProdKindIdSto) || dwProdKdSto.Enabled == false) {
            _D500Xx.ProdKindIdSto = "";
         }
         _D500Xx.ProdKindIdSto = _D500Xx.ProdKindIdSto.AsString() + "%";

         //DateTime dtDate;
         /* 月報表 */
         if (emStartYM.Visible == true) {
            if (!emStartYM.IsDate(emStartYM.Text + "/01", CheckDate.Start)) {
               _D500Xx.IsCheck = "Y";
               return false;
            }
            _D500Xx.Sdate = emStartYM.Text.Replace("/", "").SubStr(0, 6);

         }
         if (emEndYM.Visible == true) {
            if (!emEndYM.IsDate(emEndYM.Text + "/01", CheckDate.End)) {
               _D500Xx.IsCheck = "Y";
               return false;
            }
            _D500Xx.Edate = emEndYM.Text.Replace("/", "").SubStr(0, 6);

         }
         /*******************
         資料類別
         *******************/
         _D500Xx.DataType = "Q";

         /*******************
         條件值檢核OK
         *******************/
         _D500Xx.IsCheck = "Y";


         /*******************
         //Local Window 
         條件值檢核
         if		is_chk <> 'E'	then
               is_chk = 'Y'
         end	if

         資料類別:
         報價:
         is_data_type = 'Q' 
         詢價:
         is_data_type = 'R' 
         *******************/
         return true;
      }

      private bool EndRetrieve(DataTable dt)
      {
         if (dt.Rows.Count <= 0) {
            MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
            return false;
         }
         return true;
      }

      /// <summary>
      /// 匯出轉檔前的狀態顯示
      /// </summary>
      /// <param name="RptID">程式代號</param>
      /// <param name="RptName">報表名稱</param>
      /// <param name="ls_param_key">契約</param>
      /// <param name="li_ole_col">欄位位置</param>
      private void StartExport(string RptID, string RptName)
      {
         /*************************************
        ls_rpt_name = 報表名稱
        ls_rpt_id = 報表代號
        li_ole_col = 欄位位置
        ls_param_key = 契約
        *************************************/
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         stMsgTxt.Text = RptID + "－" + RptName + " 轉檔中...";
         this.Cursor = Cursors.WaitCursor;
         this.Refresh();
         Thread.Sleep(5);
      }

      /// <summary>
      /// 轉檔結束後
      /// </summary>
      private void EndExport()
      {
         stMsgTxt.Text = "轉檔完成!";
         this.Refresh();
         Thread.Sleep(5);
         //is_time = is_time + "～" + DateTime.Now;
         //PbFunc.messageBox(GlobalInfo.gs_t_result + " " + is_time, "轉檔完成!", MessageBoxIcon.Information);
         stMsgTxt.Visible = false;
         this.Cursor = Cursors.Arrow;
      }

      private string ConditionText()
      {
         string lsText;
         /*******************
         顯示條件
         *******************/
         lsText = "一般交易時段";
         if (!string.IsNullOrEmpty(_D500Xx.Sbrkno) || !string.IsNullOrEmpty(_D500Xx.Ebrkno)) {
            lsText = lsText + ",造市者:";

            if (_D500Xx.Sbrkno == _D500Xx.Ebrkno) {
               lsText = lsText + _D500Xx.Sbrkno + " ";
            }
            else {

               lsText = lsText + _D500Xx.Sbrkno + "～" + _D500Xx.Ebrkno + " ";
            }
         }

         if (!string.IsNullOrEmpty(_D500Xx.ProdCategory)) {
            lsText = lsText + ",商品群組:" + _D500Xx.ProdCategory;
         }
         if (!string.IsNullOrEmpty(_D500Xx.ProdKindIdSto)) {
            lsText = lsText + ",2碼商品(個股):" + _D500Xx.ProdKindIdSto;
         }
         if (!string.IsNullOrEmpty(_D500Xx.ProdKindId)) {
            lsText = lsText + ",造市商品:" + _D500Xx.ProdKindId;
         }
         if (PbFunc.Left(lsText, 1) == ",") {
            lsText = PbFunc.Mid(lsText, 1, 50);
         }
         if (!string.IsNullOrEmpty(lsText)) {
            lsText = "報表條件：" + lsText;
         }
         return lsText;
      }

      private string DateText()
      {
         /*******************
         顯示條件
         *******************/
         string lsText = "";
         if (!string.IsNullOrEmpty(_D500Xx.Sdate)) {
            lsText = lsText + _D500Xx.Sdate;
         }
         if (!string.IsNullOrEmpty(_D500Xx.Edate)) {
            lsText = lsText + '～' + _D500Xx.Edate;
         }
         return lsText;
      }

      private void WfRunError()
      {
         /*******************
         Messagebox
         *******************/
         //SetPointer(Arrow!);
         this.Cursor = Cursors.Arrow;
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "轉檔有錯誤!";

         File.Delete(_D500Xx.Filename);
      }

      protected bool GetData()
      {
         _ToolBtnPrintAll.Enabled = true;

         if (!StartRetrieve("       ", "ZZZZZZZ")) return false;
         /* 報表內容 */
         _Data = dao50032.List50032(_D500Xx);

         if (_Data.Rows.Count <= 0) {
            documentViewer1.DocumentSource = null;
            MessageDisplay.Info(MessageDisplay.MSG_NO_DATA);
            return false;
         }
         return true;
      }

      protected override ResultStatus Open()
      {
         base.Open();
         //Input Condition
         emEndYM.DateTimeValue = GlobalInfo.OCF_DATE.AddMonths(-1);
         emStartYM.DateTimeValue = emEndYM.DateTimeValue.AddMonths(-1);
         /* 造市者代號 */
         //起始選項
         dwSbrkno.SetDataTable(daoABRK.ListAll2(), "ABRK_NO", "CP_DISPLAY", TextEditStyles.Standard, null);
         //目的選項
         dwEbrkno.SetDataTable(daoABRK.ListAll2(), "ABRK_NO", "CP_DISPLAY", TextEditStyles.Standard, null);
         /* 商品群組 */
         dwProdCt.SetDataTable(daoAPDK.ListParamKey2(), "APDK_PARAM_KEY", "APDK_PARAM_KEY", TextEditStyles.Standard, null);
         /* 2碼商品 */
         dwProdKdSto.SetDataTable(daoAPDK.ListKind2(), "APDK_KIND_ID_STO", "APDK_KIND_ID_STO", TextEditStyles.Standard, null);
         //預設資料表
         _D500Xx.TableName = "AMM0";

         return ResultStatus.Success;
      }

      protected override ResultStatus AfterOpen()
      {
         return ResultStatus.Success;
      }

      protected override ResultStatus ActivatedForm()
      {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;
         _ToolBtnRetrieve.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Retrieve()
      {
         ShowMsg("開始讀取");
         if (!GetData()) return ResultStatus.Fail;

         ShowMsg("資料搜尋...");
         //判斷連續x個月不符造市規定
         DataTable ids = _Data.Clone();
         object brkNo = new object(), prodID = new object(); 
         string lsYMD = "";
         int liCnt = 0;
         bool found = false;
         int liMth = SleCMth.Text.AsInt();
         _Data.Rows.Add(_Data.NewRow());
         int lastRowIndex = _Data.Rows.Count - 1;
         _Data.Rows[lastRowIndex]["AMM0_BRK_NO"] = "";
         _Data.Rows[lastRowIndex]["AMM0_PROD_ID"] = "";
         _Data.Rows[lastRowIndex]["CP_INVALID"] = 1;
         for (int k = 0; k < _Data.Rows.Count; k++) {
            DataRow dr = _Data.Rows[k];
            //成績有通過
            if (dr["CP_INVALID"].AsInt() == 0 && k != _Data.Rows.Count - 1)
               continue;
            //判斷是否同brk_no+prod_id+ym,已判斷跳過
            try {
               //found = ids.Select($@"AMM0_BRK_NO='{dr["AMM0_BRK_NO"]}' and AMM0_PROD_ID='{dr["AMM0_PROD_ID"]}' and AMM0_YMD='{dr["AMM0_YMD"]}'").Any();
               /*found = ids.AsEnumerable().Where(r => r.Field<string>("AMM0_BRK_NO") == dr["AMM0_BRK_NO"].AsString()
                                                      && r.Field<string>("AMM0_PROD_ID") == dr["AMM0_PROD_ID"].AsString()
                                                      && r.Field<string>("AMM0_YMD") == dr["AMM0_YMD"].AsString()).Any();*/
               found = ids.AsEnumerable().Where(r => r.Field<object>("AMM0_BRK_NO") == dr["AMM0_BRK_NO"]
                                                                     && r.Field<object>("AMM0_PROD_ID") == dr["AMM0_PROD_ID"]
                                                                     && r.Field<object>("AMM0_YMD").AsString() == dr["AMM0_YMD"].AsString()).Any();
            }
            catch (Exception ex) {
#if DEBUG
               MessageDisplay.Error(ex.Source + ":" + k, "判斷是否同brk_no+prod_id+ym,已判斷跳過");
#endif
            }
            if (found)
               continue;

            //判斷是否同brk_no+prod_id+ym,不同acc_no是否有"成績有通過"
            try {
               //found = _Data.Select($@"AMM0_BRK_NO='{dr["AMM0_BRK_NO"]}' and AMM0_PROD_ID='{dr["AMM0_PROD_ID"]}' and AMM0_YMD='{dr["AMM0_YMD"]}' and AMM0_ACC_NO <>'{dr["AMM0_ACC_NO"]}' and CP_INVALID=0").Any();
               found = _Data.AsEnumerable().Where(r => r.Field<object>("AMM0_BRK_NO") == dr["AMM0_BRK_NO"]
                                                   && r.Field<object>("AMM0_PROD_ID") == dr["AMM0_PROD_ID"]
                                                   && r.Field<object>("AMM0_YMD") == dr["AMM0_YMD"]
                                                   && r.Field<object>("AMM0_ACC_NO") != dr["AMM0_ACC_NO"] 
                                                   && r.Field<int>("CP_INVALID") == 0).Any();
            }
            catch (Exception ex) {
#if DEBUG
               MessageDisplay.Error(ex.Source + ":" + k, "判斷不同acc_no是否有成績有通過");
#endif
            }
            if (found)
               continue;

            if (brkNo != dr["AMM0_BRK_NO"] || prodID != dr["AMM0_PROD_ID"]) {
               if (liCnt >= liMth) {
                  ids.Rows.Add(ids.NewRow());
                  int iDsLastRowIndex = ids.Rows.Count - 1;
                  ids.Rows[iDsLastRowIndex]["AMM0_BRK_NO"] = brkNo;
                  ids.Rows[iDsLastRowIndex]["AMM0_PROD_ID"] = prodID;
                  ids.Rows[iDsLastRowIndex]["AMM0_OM_QNTY"] = liCnt;
               }
               brkNo = dr["AMM0_BRK_NO"];
               prodID = dr["AMM0_PROD_ID"];
               lsYMD = dr["AMM0_YMD"].AsString();
               liCnt = 1;
            }//if (brkNo != dr["AMM0_BRK_NO"].AsString() || prodID != dr["AMM0_PROD_ID"].AsString())
            else {
               if (k == _Data.Rows.Count - 1)
                  continue;

               string lsDate = dr["AMM0_YMD"].AsDateTime("yyyyMM").AddMonths(-1).ToString("yyyyMM");
               //連續月份
               if (lsDate == lsYMD) {
                  liCnt = liCnt + 1;
               }
               lsYMD = dr["AMM0_YMD"].AsString();
            }
         }//for (int k = 0; k < _Data.Rows.Count; k++)
         _Data.Rows.RemoveAt(_Data.Rows.Count - 1);

         DataTable dt = _Data;
         object brkNo2 = new object(), prodID2 = new object();
         for (int k = 0; k < dt.Rows.Count; k++) {
            DataRow dr = dt.Rows[k];
            if (brkNo2 != dr["AMM0_BRK_NO"] || prodID2 != dr["AMM0_PROD_ID"]) {
               brkNo2 = dr["AMM0_BRK_NO"];
               prodID2 = dr["AMM0_PROD_ID"];
               //found = ids.Select($@"AMM0_BRK_NO='{dr["AMM0_BRK_NO"]}' and AMM0_PROD_ID='{dr["AMM0_PROD_ID"]}'").Any();
               //found = ids.AsEnumerable().Where(r => r.Field<string>("AMM0_BRK_NO") == brkNo2 && r.Field<string>("AMM0_PROD_ID") == prodID2).Any();
               found = ids.AsEnumerable().Where(r => r.Field<object>("AMM0_BRK_NO") == brkNo2 && r.Field<object>("AMM0_PROD_ID") == prodID2).Any();
            }
            if (!found) {
               _Data.Rows.RemoveAt(k);
               k = k - 1;
            }
         }//for (int k = 0; k < dt.Rows.Count; k++)
         _Data.AddSeriNumToDataTable();

         ShowMsg("讀取中...");

         List<ReportProp> caption = new List<ReportProp>{
            new ReportProp{DataColumn="CP_ROW",Caption= "筆數" ,CellWidth=40,DetailRowFontSize=8,HeaderFontSize=11},
            new ReportProp{DataColumn="AMM0_BRK_NO",Caption= "期貨商        代號",CellWidth=70,DetailRowFontSize=10,HeaderFontSize=11,DataRowMerge=true},
            new ReportProp{DataColumn="BRK_ABBR_NAME",Caption= "期貨商名稱" ,CellWidth=150,DetailRowFontSize=9,HeaderFontSize=11,DataRowMerge=true},
            new ReportProp{DataColumn="AMM0_ACC_NO",Caption= "帳號",CellWidth=60,DetailRowFontSize=10,HeaderFontSize=11,DataRowMerge=true },
            new ReportProp{DataColumn="AMM0_PROD_ID",Caption= "商品名稱",CellWidth=80,DetailRowFontSize=10,HeaderFontSize=11,DataRowMerge=true},
            new ReportProp{DataColumn="AMM0_YMD",Caption= "日期" ,CellWidth=65,DetailRowFontSize=9,HeaderFontSize=11},
            new ReportProp{DataColumn="AMM0_OM_QNTY",Caption= "委託          成交量",CellWidth=65,textAlignment=TextAlignment.MiddleRight,TextFormatString="{0:#,##0}",DetailRowFontSize=10,HeaderFontSize=11},
            new ReportProp{DataColumn="AMM0_QM_QNTY",Caption= "報價          成交量",CellWidth=65,textAlignment=TextAlignment.MiddleRight,TextFormatString="{0:#,##0}",DetailRowFontSize=10,HeaderFontSize=11},
            new ReportProp{DataColumn="QNTY",Caption= "造市量" ,CellWidth=65,textAlignment=TextAlignment.MiddleRight,TextFormatString="{0:#,##0}",DetailRowFontSize=10,HeaderFontSize=11,Expression=new ReportExprssion{ Condition="[QNTY] < [MMF_QNTY_LOW]",BackColor=Color.FromArgb(255,128,255),ForeColor=Color.Black } },
            new ReportProp{DataColumn="CP_M_QNTY",Caption= "造市者   總成交量" ,CellWidth=75,textAlignment=TextAlignment.MiddleRight,TextFormatString="{0:#,##0}",DetailRowFontSize=10,HeaderFontSize=11},
            new ReportProp{DataColumn="CP_RATE_M",Caption= "總成交量   市佔率(%)",CellWidth=60,textAlignment=TextAlignment.MiddleRight,TextFormatString="{0:##0.0#}",DetailRowFontSize=10,HeaderFontSize=8},
            new ReportProp{DataColumn="AMM0_VALID_CNT",Caption= "有效報價     筆數",CellWidth=75,textAlignment=TextAlignment.MiddleRight,TextFormatString="{0:#,##0}",DetailRowFontSize=10,HeaderFontSize=11},
            new ReportProp{DataColumn="VALID_RATE",Caption= "有效報/詢價   比例(%)",CellWidth=70,textAlignment=TextAlignment.MiddleRight,TextFormatString="{0:##0.0#}",DetailRowFontSize=10,HeaderFontSize=8,Expression=new ReportExprssion{ Condition="[VALID_RATE] < [MMF_RESP_RATIO]",BackColor=Color.FromArgb(255,128,255),ForeColor=Color.Black }},
            new ReportProp{DataColumn="AMM0_MARKET_R_CNT",Caption= "全市場   詢價筆數",CellWidth=75,textAlignment=TextAlignment.MiddleRight,TextFormatString="{0:#,##0}",DetailRowFontSize=10,HeaderFontSize=11},
            new ReportProp{DataColumn="AMM0_MARKET_M_QNTY",Caption= "全市場   總成交量",CellWidth=75,textAlignment=TextAlignment.MiddleRight,TextFormatString="{0:#,##0}",DetailRowFontSize=10,HeaderFontSize=11},
            new ReportProp{DataColumn="AMM0_KEEP_FLAG",Caption= "符合報價每日平均維持時間",CellWidth=55,textAlignment=TextAlignment.MiddleCenter,DetailRowFontSize=10,HeaderFontSize=8}
            };
         _defReport = new defReport(_Data, caption);
         documentViewer1.DocumentSource = _defReport;
         _defReport.CreateDocument(true);

         ShowMsg("匯出檔案...");
         string destinationFilePath = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH, "50032.xlsx");
         try {
            Workbook workbook = new Workbook();
            workbook.CreateNewDocument();
            workbook.SaveDocument(destinationFilePath, DocumentFormat.Xlsx);
            workbook.Worksheets[0].Import(ids, true, 0, 0);
            workbook.SaveDocument(destinationFilePath);
         }
         catch (Exception ex) {
            if (File.Exists(destinationFilePath))
               File.Delete(destinationFilePath);
            throw ex;
         }
         finally {
            EndExport();
         }

         return ResultStatus.Success;
      }

      protected void ShowMsg(string msg)
      {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = msg;
         this.Refresh();
         Thread.Sleep(5);
      }

      protected override ResultStatus Export()
      {
         stMsgTxt.Visible = true;
         stMsgTxt.Text = "開始轉檔...";
         return ResultStatus.Success;
      }

      protected override ResultStatus Print(ReportHelper reportHelper)
      {
         CommonReportLandscapeA4 reportLandscapeA4 = new CommonReportLandscapeA4();
         XtraReport xtraReport = reportHelper.CreateCompositeReport(_defReport, reportLandscapeA4);
         string dateCondition = DateText() == "" ? "" : "," + DateText();
         reportHelper.LeftMemo =
            (ConditionText() == "" ?
            $"報表條件：連續{SleCMth.Text}個月不符造市規定" :
            ConditionText() + $"連續{SleCMth.Text}個月不符造市規定")
            + dateCondition;
         reportHelper.Create(xtraReport);

         //reportHelper.Preview();
         base.Print(reportHelper);
         return ResultStatus.Success;
      }

      protected override ResultStatus BeforeClose()
      {
         _Data = null;
         documentViewer1.DocumentSource = null;
         return ResultStatus.Success;
      }
   }
}