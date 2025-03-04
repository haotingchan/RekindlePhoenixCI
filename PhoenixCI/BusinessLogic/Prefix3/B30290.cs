﻿using Common;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Data;
using DevExpress.Spreadsheet;
using BaseGround.Shared;
using System.IO;
using DevExpress.XtraEditors;
using DataObjects.Dao.Together;
using BusinessObjects;
using System.Linq;
/// <summary>
/// john,20190424,轉出交易系統部位限制檔及公告表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix3
{
   /// <summary>
   /// 轉出交易系統部位限制檔及公告表
   /// </summary>
   public class B30290
   {
      private D30290 dao30290;
      private readonly string _txnID;

      public B30290(string ProgramID)
      {
         dao30290 = new D30290();
         _txnID = ProgramID;
      }

      /// <summary>
      /// 生效日期清單
      /// </summary>
      /// <param name="emDateText"></param>
      /// <returns></returns>
      public DataTable GetEffectiveYMD(string emDateText)
      {
         return dao30290.ListEffectiveYmd(emDateText.Replace("/", ""));
      }

      /// <summary>
      /// 產生新資料
      /// </summary>
      /// <param name="isCalYMD"></param>
      /// <param name="isYMD"></param>
      /// <returns></returns>
      public DataTable ListInsertGridData(string isCalYMD, string isYMD)
      {
         return dao30290.ListInsertData(isCalYMD.Replace("/", ""), isYMD);
      }

      /// <summary>
      /// 已存檔資料
      /// </summary>
      /// <param name="isYMD"></param>
      /// <returns></returns>
      public DataTable List30290GridData(string isYMD)
      {
         return dao30290.List30290Data(isYMD);
      }

      /// <summary>
      /// 相同生效日期資料總數
      /// </summary>
      /// <param name="YMD"></param>
      /// <returns></returns>
      public int DataCount(string YMD)
      {
         return dao30290.PLP13ListCount(YMD);
      }

      /// <summary>
      /// 刪除PLP13資料
      /// </summary>
      /// <param name="isYMD"></param>
      /// <returns></returns>
      public bool DeleteData(string isYMD)
      {
         return dao30290.DeletePLP13(isYMD);
      }

      /// <summary>
      /// 新增PLP13資料
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData)
      {
         return dao30290.UpdatePLP13(inputData);
      }

      /// <summary>
      /// //取得前一季月份
      /// </summary>
      /// <returns></returns>
      public string LastQuarter(DateTime ocfYMD)
      {
         DateTime dateTime = ocfYMD;
         //取得前一季月份
         int month = dateTime.Month;
         int num = 0;
         if (month % 3 > 0) {
            num = (month / 3) + 1;
         }
         else if ((month % 3) == 0) {
            num = month / 3;
         }

         DateTime ymd = new DateTime();

         if (num == 1) {
            num = 4;
            month = num * 3;
            ymd = new DateTime(dateTime.Year - 1, month, 1);
         }
         else {
            month = (num - 1) * 3;
            ymd = new DateTime(dateTime.Year, month, 1);
         }

         //取得前一季月底最後交易日
         DateTime maxDate = new AOCF().GetMaxDate(ymd.ToString("yyyyMM01"), ymd.ToString("yyyyMM31"));
         return maxDate.ToString("yyyy/MM/dd");
      }

      /// <summary>
      /// WfExport
      /// </summary>
      /// <returns></returns>
      public string WfExport(string lsFile, string SelDate, string emDateTxt,string GlobalSavePath)
      {
         string msg;
         try {
            DataTable dtF = dao30290.List30290TxtF(SelDate);
            if (dtF.Rows.Count<=0) {
               return $"{emDateTxt},期貨交易系統用TXT檔無任何資料,請先執行「儲存」!";
            }
            string txtFpath = Path.Combine(GlobalSavePath,
             "P13_fut_"+ SelDate + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH.mm.ss") + ".txt");

            Common.Helper.ExportHelper.ToText(dtF, txtFpath);

            DataTable dtO = dao30290.List30290TxtO(SelDate);
            if (dtO.Rows.Count <= 0) {
               return $"{emDateTxt},選擇權交易系統用TXT檔無任何資料,請先執行「儲存」!";
            }
            string txtOpath = Path.Combine(GlobalSavePath,
             "P13_opt_" + SelDate + "_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("HH.mm.ss") + ".txt");

            Common.Helper.ExportHelper.ToText(dtO, txtOpath);

            msg = Wf30290gbf(lsFile, SelDate, emDateTxt);
            if (msg != MessageDisplay.MSG_OK) return msg;
            msg = Wf30290NStock(lsFile, SelDate, emDateTxt);
            if (msg != MessageDisplay.MSG_OK) return msg;
            msg = Wf30290Stock(lsFile, SelDate, emDateTxt);
            if (msg != MessageDisplay.MSG_OK) return msg;

            return MessageDisplay.MSG_OK;
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("WfExport:" + ex.Message);
#else
            throw ex;
#endif
         }
      }

      /// <summary>
      /// wf_30290_gbf()
      /// </summary>
      /// <param name="lsFile">產檔路徑</param>
      /// <param name="SelDate">生效日期</param>
      /// <param name="emDateTxt">查詢日期</param>
      /// <returns></returns>
      public string Wf30290gbf(string lsFile, string SelDate, string emDateTxt)
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(lsFile);

            //讀取資料
            DataTable dt = dao30290.ListGBF(SelDate);
            if (dt.Rows.Count <= 0) {
               return $"{emDateTxt},30290－股價指數暨黃金類公告表,無任何資料!";
            }

            foreach (DataRow row in dt.Rows) {
               int rowIndex = row["C_SEQ_NO"].AsInt();

               string legalMth = row["PL2B_NATURE_LEGAL_MTH"].AsString();
               string legalTot = row["PL2B_NATURE_LEGAL_TOT"].AsString();
               string mth999 = row["PL2B_999_MTH"].AsString();
               string nearbyMth = row["PL2B_999_NEARBY_MTH"].AsString();
               string tot999 = row["PL2B_999_TOT"].AsString();
               //中文版
               workbook.Worksheets[0].Cells[$"C{rowIndex}"].Value = string.Format("單一月份{0:N0}，各月份合計{1:N0}", legalMth.AsInt(), legalTot.AsInt());//自然人
               workbook.Worksheets[0].Cells[$"E{rowIndex}"].Value = string.Format("單一月份{0:N0}(最近到期月份{1:N0})，各月份合計{2:N0}", mth999.AsInt(), nearbyMth.AsInt(), tot999.AsInt());//期貨自營商/造市者
               //英文版
               int EngRowIndex = row["E_SEQ_NO"].AsInt();
               workbook.Worksheets[1].Cells[$"B{EngRowIndex}"].Value = string.Format("{0:N0} contracts for any single month, and {1:N0} contracts for all months combined ", legalMth.AsInt(), legalTot.AsInt());//Individual
               workbook.Worksheets[1].Cells[$"D{EngRowIndex}"].Value = string.Format("{0:N0} contracts for any single month({1:N0} contracts for nearest month), and {2:N0} contracts for all months combined ", mth999.AsInt(), nearbyMth.AsInt(), tot999.AsInt());//Proprietary Trader
            }
         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(lsFile);
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30290_n_stock_ch() and wf_30290_n_stock_eng()
      /// </summary>
      /// <param name="lsFile">產檔路徑</param>
      /// <param name="SelDate">生效日期</param>
      /// <param name="emDateTxt">查詢日期</param>
      /// <returns></returns>
      public string Wf30290NStock(string lsFile, string SelDate, string emDateTxt)
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(lsFile);

            //讀取資料
            DataTable dt = dao30290.ListNStock(SelDate);
            if (dt.Rows.Count <= 0) {
               return $"{emDateTxt},30290－股價指數暨黃金類公告表,無任何資料!";
            }
            else {
               string date = dt.AsEnumerable().LastOrDefault()["PLP13_ISSUE_YMD"].AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
               workbook.Worksheets[0].Cells["B1"].Value = workbook.Worksheets[0].Cells["B1"].Value + date;//最新更新(生效)日期：
               workbook.Worksheets[1].Cells["A1"].Value = workbook.Worksheets[1].Cells["A1"].Value + date;//Effective on
            }

            foreach (DataRow row in dt.Rows) {
               int ChRowIndex = row["C_SEQ_NO"].AsInt();

               var pl2Nature = row["PL2_NATURE"];
               var pl2Legal = row["PL2_LEGAL"];
               var pl2999 = row["PL2_999"];
               //中文版
               workbook.Worksheets[0].Cells[$"C{ChRowIndex}"].SetValue(pl2Nature);//自然人
               workbook.Worksheets[0].Cells[$"D{ChRowIndex}"].SetValue(pl2Legal);//法人
               workbook.Worksheets[0].Cells[$"E{ChRowIndex}"].SetValue(pl2999);//期貨自營商/造市者
               //英文版
               int EngRowIndex = row["E_SEQ_NO"].AsInt();
               workbook.Worksheets[1].Cells[$"B{EngRowIndex}"].SetValue(pl2Nature);//Individual
               workbook.Worksheets[1].Cells[$"C{EngRowIndex}"].SetValue(pl2Legal);//Institution
               workbook.Worksheets[1].Cells[$"D{EngRowIndex}"].SetValue(pl2999);//Proprietary Trader
            }
         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(lsFile);
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30290_stock_ch() and wf_30290_stock_eng()
      /// </summary>
      /// <param name="lsFile">產檔路徑</param>
      /// <param name="SelDate">生效日期</param>
      /// <param name="emDateTxt">查詢日期</param>
      /// <returns></returns>
      public string Wf30290Stock(string lsFile, string SelDate, string emDateTxt)
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(lsFile);

            //讀取資料
            DataTable dt = dao30290.ListStock(SelDate);
            if (dt.Rows.Count <= 0) {
               return $"{emDateTxt},30290－個股類部位限制公告表,無任何資料!";
            }
            //取得最大日期
            string date = dt.AsEnumerable().Select(dr => dr.Field<string>("PLP13_ISSUE_YMD")).Max().AsDateTime("yyyyMMdd").ToString("yyyy/MM/dd");
            //個股類公告表(中文版)
            ChStock(workbook, dt, date);
            //個股類公告表(英文版)
            EngStock(workbook, dt, date);
         }
         catch (Exception ex) {
            throw ex;
         }
         finally {
            workbook.SaveDocument(lsFile);//save
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_30290_stock_ch()
      /// </summary>
      /// <param name="workbook">Excel</param>
      /// <param name="dt">ListStock</param>
      /// <param name="date">最大日期</param>
      private static void ChStock(Workbook workbook, DataTable dt, string date)
      {
         //個股類公告表(中文版)
         Worksheet worksheet = workbook.Worksheets[2];
         //最新更新(生效)日期：
         worksheet.Cells["B1"].Value = worksheet.Cells["B1"].Value + date;

         int rowIndex = 4;
         foreach (DataRow row in dt.Rows) {
            rowIndex = rowIndex + 1;
            //提高報表中的降低
            var pls2Level = row["PLS2_LEVEL"];
            var pls2Nature = row["PLS2_NATURE"];
            var pls2Legal = row["PLS2_LEGAL"];
            var pls2999 = row["PLS2_999"];

            //商品代號(前2碼)
            worksheet.Cells[$"A{rowIndex}"].SetValue(row["PLS2_KIND_ID2"].AsString());
            //級數
            worksheet.Cells[$"B{rowIndex}"].SetValue(pls2Level);
            string str = row["APDK_NAME"].AsString();
            str = str.IndexOf("小型") >= 0 ? str.SubStr(str.IndexOf("小型") + 2, 99) : str;
            str = str.IndexOf("期貨") >= 0 ? str.SubStr(0, str.IndexOf("期貨")) : str;
            str = str.IndexOf("選擇權") >= 0 ? str.SubStr(0, str.IndexOf("選擇權")) : str;
            //標的證券簡稱
            worksheet.Cells[$"C{rowIndex}"].SetValue(str);
            //證券代號
            worksheet.Cells[$"D{rowIndex}"].SetValue(row["PLS2_SID"]);

            //大型
            if (row["PLS2_KIND_ID2"].AsString() == row["APDK_KIND_GRP2"].AsString()) {
               if (row["PLS2_FUT"].AsString() == "F") {
                  //股票期貨
                  worksheet.Cells[$"E{rowIndex}"].SetValue("○");
               }
               if (row["PLS2_OPT"].AsString() == "O") {
                  //股票選擇權
                  worksheet.Cells[$"F{rowIndex}"].SetValue("○");
               }
               //自然人
               worksheet.Cells[$"G{rowIndex}"].SetValue(pls2Nature);
               //法人/期貨自營商
               worksheet.Cells[$"H{rowIndex}"].SetValue(pls2Legal);
               //造市者
               worksheet.Cells[$"I{rowIndex}"].SetValue(pls2999);
            }
            else {//小型
               if (row["PLS2_FUT"].AsString() == "F") {
                  //股票期貨
                  worksheet.Cells[$"E{rowIndex}"].SetValue("◎");
                  str = string.Format("與{0}期貨合併計算(依20口小型{0}期貨等於1口{0}期貨合併計算)", str);
               }
               if (row["PLS2_OPT"].AsString() == "O") {
                  //股票選擇權
                  worksheet.Cells[$"F{rowIndex}"].SetValue("◎");
               }
               //自然人
               worksheet.Cells[$"G{rowIndex}"].SetValue(str);
               worksheet.Cells[$"G{rowIndex}"].Font.Size = 10;
               worksheet.Cells[$"G{rowIndex}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.General;
            }
         }
         //刪除空白列
         if (1004 > rowIndex) {
            worksheet.Rows.Remove(rowIndex, 1004 - rowIndex);
            worksheet.ScrollTo(0, 0);
         }
      }

      /// <summary>
      /// wf_30290_stock_eng()
      /// </summary>
      /// <param name="workbook">Excel</param>
      /// <param name="dt">ListStock</param>
      /// <param name="date">最大日期</param>
      private static void EngStock(Workbook workbook, DataTable dt, string date)
      {
         //個股類公告表(英文版)
         Worksheet worksheet = workbook.Worksheets[3];
         //Stock Futures and Options (Effective on 
         worksheet.Cells["B1"].Value = worksheet.Cells["B1"].Value + date + ")";

         int rowIndex = 2;
         foreach (DataRow row in dt.Rows) {
            rowIndex = rowIndex + 1;
            //提高報表中的降低
            var pls2Level = row["PLS2_LEVEL"];
            var pls2Nature = row["PLS2_NATURE"];
            var pls2Legal = row["PLS2_LEGAL"];
            var pls2999 = row["PLS2_999"];

            //Ticker Symbol
            worksheet.Cells[$"A{rowIndex}"].SetValue(row["PLS2_KIND_ID2"].AsString());
            //Tier
            worksheet.Cells[$"B{rowIndex}"].SetValue(pls2Level);
            //Stock Code 
            worksheet.Cells[$"C{rowIndex}"].SetValue(row["PLS2_SID"]);

            //大型
            if (row["PLS2_KIND_ID2"].AsString() == row["APDK_KIND_GRP2"].AsString()) {
               if (row["PLS2_FUT"].AsString() == "F") {
                  //Stock Futures
                  worksheet.Cells[$"D{rowIndex}"].SetValue("○");
               }
               if (row["PLS2_OPT"].AsString() == "O") {
                  //Stock Options
                  worksheet.Cells[$"E{rowIndex}"].SetValue("○");
               }
               //Individual
               worksheet.Cells[$"F{rowIndex}"].SetValue(pls2Nature);
               //Institution / Proprietary
               worksheet.Cells[$"G{rowIndex}"].SetValue(pls2Legal);
               //Market Maker
               worksheet.Cells[$"H{rowIndex}"].SetValue(pls2999);
            }
            else {//小型
               if (row["PLS2_FUT"].AsString() == "F") {
                  //Stock Futures
                  worksheet.Cells[$"D{rowIndex}"].SetValue("◎");
                  //Individual
                  worksheet.Cells[$"F{rowIndex}"].Value = $"Combined with the calculation of {row["APDK_KIND_GRP2"].AsString()}F  position limit (on a pro rata basis of 20:1 contract size)";
               }
               if (row["PLS2_OPT"].AsString() == "O") {
                  //Stock Options
                  worksheet.Cells[$"E{rowIndex}"].SetValue("◎");
               }
               //Individual
               worksheet.Cells[$"F{rowIndex}"].Font.Size = 8;
               worksheet.Cells[$"F{rowIndex}"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.General;
            }
         }
         //刪除空白列
         if (1002 > rowIndex) {
            worksheet.Rows.Remove(rowIndex, 1002 - rowIndex);
            worksheet.ScrollTo(0, 0);
         }
      }


   }
}