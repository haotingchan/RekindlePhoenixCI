﻿using BaseGround;
using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Winni, 2019/02/21
/// </summary>
namespace PhoenixCI.FormUI.Prefix3 {
   /// <summary>
   /// 30686 加權指數日內值查詢
   /// 有寫到的功能：Export
   /// </summary>
   public partial class W30686 : FormParent {

      private D30686 dao30686;
      private ResultStatus exportStatus = ResultStatus.Fail;

      #region 一般查詢縮寫
      /// <summary>
      /// 起日(yyyyMMdd)
      /// </summary>
      public string StartDate {
         get {
            return txtStartDate.Text.Replace("/" , "");
         }
      }

      /// <summary>
      /// 訖日(yyyyMMdd)
      /// </summary>
      public string EndDate {
         get {
            return txtEndDate.Text.Replace("/" , "");
         }
      }
      #endregion

      public W30686(string programID , string programName) : base(programID , programName) {
         InitializeComponent();
         this.Text = _ProgramID + "─" + _ProgramName;

         dao30686 = new D30686();
         txtStartDate.DateTimeValue = GlobalInfo.OCF_DATE;
         txtEndDate.DateTimeValue = GlobalInfo.OCF_DATE;
         //winni test
         //20171218-20171219
      }

      protected override ResultStatus ActivatedForm() {
         base.ActivatedForm();

         _ToolBtnExport.Enabled = true;

         return ResultStatus.Success;
      }

      protected override ResultStatus Export() {

         try {

            //0. ready
            panFilter.Enabled = false;
            labMsg.Visible = true;
            labMsg.Text = "開始轉檔...";
            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            Thread.Sleep(5);

            //1.撈資料
            DataTable dtContent = dao30686.GetData(StartDate , EndDate , 'Y');
            if (dtContent.Rows.Count <= 0) {
               labMsg.Visible = false;
               MessageDisplay.Info(string.Format("{0},{1},無任何資料!" , txtStartDate.Text + "-" + txtEndDate.Text , this.Text));
               return ResultStatus.Fail;
            }

            //1.1處理資料型態
            DataTable dt = dtContent.Clone(); //轉型別用的datatable
            dt.Columns["VPR_DATA_TIME"].DataType = typeof(string); //將原DataType(datetime)轉為string
            foreach (DataRow row in dtContent.Rows) {
               dt.ImportRow(row);
            }

            for (int i = 0 ; i < dt.Rows.Count ; i++) {
               dt.Rows[i]["VPR_DATA_TIME"] = Convert.ToDateTime(dtContent.Rows[i]["VPR_DATA_TIME"]).ToString("yyyy/MM/dd HH:mm:ss");
            }

            //2.存Csv
            string etfFileName = "30686_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + ".csv";
            etfFileName = Path.Combine(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH , etfFileName);
            ExportOptions csvref = new ExportOptions();
            csvref.HasHeader = true;
            csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
            Common.Helper.ExportHelper.ToCsv(dt , etfFileName , csvref);

            labMsg.Text = "轉檔成功!";
            exportStatus = ResultStatus.Success;
            return ResultStatus.Success;

         } catch (Exception ex) {
            WriteLog(ex);
            labMsg.Text = "轉檔失敗";
         } finally {
            panFilter.Enabled = true;
            labMsg.Text = "";
            labMsg.Visible = false;
            this.Cursor = Cursors.Arrow;
         }
         return ResultStatus.Fail;

      }

      protected override ResultStatus ExportAfter(string startTime) {
         if (exportStatus == ResultStatus.Success) {
            MessageDisplay.Info("轉檔完成!");
            return ResultStatus.Success;
         } else {
            MessageDisplay.Warning("轉檔失敗");
            return ResultStatus.Fail;
         }
      }

   }
}