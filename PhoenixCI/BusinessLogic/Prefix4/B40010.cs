using BusinessObjects.Enums;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.IO;
using System.Threading;
/// <summary>
/// john,20190619
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 保證金狀況表 (Group 1/2/3)
   /// </summary>
   public class B40010
   {
      /// <summary>
      /// 輸入的日期 yyyy/MM/dd
      /// </summary>
      protected string _emDateText;
      /// <summary>
      /// Data
      /// </summary>
      protected D40010 dao40010;

      public B40010()
      {
      }

      public B40010(string emDate)
      {
         this._emDateText = emDate;
         this.dao40010 = new D40010();
      }

      public DataTable MGR2DataClone()
      {
         DataTable RowData = dao40010.ListMGR2_SMA("", "").Clone();
         return RowData;
      }

      public DataTable ProductList(string oswGrp)
      {
         DataTable dt = dao40010.ListProd(_emDateText.Replace("/", ""), oswGrp);
         return dt;
      }

      /// <summary>
      /// 儲存至MGR2_SMA並執行opt.sp_O_gen_H_MG1_3M
      /// </summary>
      /// <param name="MGR2_SMA"></param>
      /// <param name="oswGrp"></param>
      public void MGR2SaveToDB(DataTable MGR2_SMA, string oswGrp)
      {
         DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
         //儲存至DB
         ResultStatus resultStatus = dao40010.UpdateMGR2_SMA(MGR2_SMA).Status;
         //呼叫opt.sp_O_gen_H_MG1_3M
         if (resultStatus == ResultStatus.Success) {
#if DEBUG
            //dao40010.SP_O_gen_H_MG1_3M(emdate, oswGrp);
#else
            dao40010.SP_O_gen_H_MG1_3M(emdate, oswGrp);
#endif
         }

      }

      /// <summary>
      /// 新增EWMA計算按鈕，產出資料，並將excel計算資料回寫資料庫
      /// </summary>
      /// <returns></returns>
      public DataRow ComputeEWMA(string FilePath, string KindID)
      {
         Workbook workbook = new Workbook();
         DataRow row;
         try {
            //切換Sheet
            workbook.LoadDocument(FilePath);
            Thread.Sleep(0);
            Worksheet worksheet = workbook.Worksheets["Rawdata"];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");

            //1.RowData sheet第一行
            string lsDate = emdate.ToString("yyyyMMdd");
            //確認有無資料
            DataTable RowData = dao40010.ListRowDataSheet(lsDate, KindID);
            if (RowData.Rows.Count <= 0) {
               return null;
            }

            worksheet.Cells["A1"].SetValue(RowData.Rows[0]["MG1_PROD_SUBTYPE"]);
            worksheet.Cells["B1"].SetValue(RowData.Rows[0]["MG1_PROD_TYPE"]);
            worksheet.Cells["C1"].SetValue(RowData.Rows[0]["KIND_ID"]);
            worksheet.Cells["D1"].SetValue(RowData.Rows[0]["MG1_CURRENCY_TYPE"]);
            worksheet.Cells["E1"].SetValue(RowData.Rows[0]["MG1_PARAM_KEY"].AsString());//E1那欄文字加trim
            worksheet.Cells["F1"].SetValue(RowData.Rows[0]["MG1_M_KIND_ID"]);
            worksheet.Cells["G1"].SetValue(RowData.Rows[0]["MG1_OSW_GRP"]);
            worksheet.Cells["N1"].SetValue(RowData.Rows[0]["MG1_MIN_RISK"]);

            //契約規格
            worksheet.Cells["O3"].SetValue(RowData.Rows[0]["MG1_XXX"]);
            //現行結算保證金
            worksheet.Cells["P3"].SetValue(RowData.Rows[0]["MG1_CUR_CM"]);

            //2.寫入40010template計算EWMA
            ////寫入O欄跟P欄
            //DataTable tempData = RowData.Copy();
            //foreach (DataColumn col in RowData.Columns) {
            //   if (col.ColumnName == "MG1_XXX" || col.ColumnName == "MG1_CUR_CM")
            //      continue;

            //   tempData.Columns.Remove(col.ColumnName);
            //}
            //tempData.Columns["MG1_XXX"].SetOrdinal(0);
            //tempData.Columns["MG1_CUR_CM"].SetOrdinal(1);
            //worksheet.Import(tempData, false, 2, 14);

            //共16欄位 取前7欄位
            int k = RowData.Columns.Count;
            while (k > 7) {
               RowData.Columns.Remove(RowData.Columns[7].ColumnName);
               k--;//刪除後面的欄位直到剩下前面7欄
            }

            worksheet.Import(RowData, false, 2, 0);
            Thread.Sleep(0);
            worksheet.ScrollTo(0, 0);
            //save
            workbook.SaveDocument(FilePath);

            //3.return datarow
            DataTable MGR2_SMA = MGR2DataClone();

            row = MGR2_SMA.NewRow();
            MGR2_SMA.Rows.Add(row);

            //模型(S:SMA/M:MAXV/E:EWMA)
            row["MGR2_MODEL_TYPE"] = "E";
            //日期
            row["MGR2_YMD"] = worksheet.Cells["B3"].Value.DateTimeValue.ToString("yyyyMMdd");
            //股票代號
            row["MGR2_M_KIND_ID"] = worksheet.Cells["F1"].Value;
            //B開參/最低價 (A-B)
            row["MGR2_PRICE1"] = worksheet.Cells["E3"].Value.NumericValue;
            //A收盤價/最高價/開參 (A-B)
            row["MGR2_PRICE2"] = worksheet.Cells["D3"].Value.NumericValue;
            //每日報酬率
            row["MGR2_RETURN_RATE"] = worksheet.Cells["G3"].Value.NumericValue;
            //30日風險價格係數
            row["MGR2_30_RATE"] = worksheet.Cells["H3"].Value.NumericValue;
            //60日風險價格係數
            row["MGR2_60_RATE"] = worksheet.Cells["I3"].Value.NumericValue;
            //90日風險價格係數
            row["MGR2_90_RATE"] = worksheet.Cells["J3"].Value.NumericValue;
            //180日風險價格係數
            row["MGR2_180_RATE"] = worksheet.Cells["K3"].Value.NumericValue;
            //2500日風險價格係數
            row["MGR2_2500_RATE"] = worksheet.Cells["L3"].Value.NumericValue;
            //風險價格係數最大值
            row["MGR2_MIN_RATE"] = worksheet.Cells["N1"].Value.NumericValue;
            //每日風險價格係數
            row["MGR2_DAY_RATE"] = worksheet.Cells["N3"].Value.NumericValue;
            /*資料計算狀態
              N  正常                
              X  不計算*/
            row["MGR2_STATUS_CODE"] = "N";
            //股票市場別
            row["MGR2_PROD_TYPE"] = worksheet.Cells["B1"].Value;
            //子類別
            row["MGR2_PROD_SUBTYPE"] = worksheet.Cells["A1"].Value;
            //契約對照瑪
            row["MGR2_PARAM_KEY"] = worksheet.Cells["E1"].Value;
            //計算的風險價格係數
            row["MGR2_CP_RATE"] = worksheet.Cells["M3"].Value.NumericValue;
            //計算的風險價格係數(涵蓋1日)
            row["MGR2_1DAY_CP_RATE"] = worksheet.Cells["M1"].Value.NumericValue;
            //轉檔時間
            row["MGR2_W_TIME"] = DateTime.Now;
            //收盤群組
            row["MGR2_OSW_GRP"] = worksheet.Cells["G1"].Value;

         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"ComputeEWMA:" + ex.Message);
#else
            throw ex;
#endif
         }
         return row;
      }


   }

}
