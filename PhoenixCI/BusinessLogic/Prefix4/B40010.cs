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


      /// <summary>
      /// 沒資料新增一筆row
      /// </summary>
      /// <param name="dt"></param>
      /// <returns></returns>
      private static DataTable DataTableModify(DataTable dt)
      {
         if (dt.Rows.Count <= 0) {
            DataRow row = dt.NewRow();
            dt.Rows.Add(row);
         }

         return dt;
      }

      /// <summary>
      /// 儲存資料到MGR2_SMA
      /// </summary>
      /// <param name="MGR2_SMA"></param>
      /// <param name="worksheet"></param>
      private void SaveMGR2smaTypeE(DataTable MGR2_SMA, Worksheet worksheet)
      {
         if (MGR2_SMA == null || MGR2_SMA.Rows.Count <= 0) {
            return;
         }

         DataRow row = MGR2_SMA.Rows[0];

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
         row["MGR2_PROD_TYPE"] = worksheet.Cells["H1"].Value;
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

         //存檔
         dao40010.UpdateMGR2_SMA(MGR2_SMA);
      }

      /// <summary>
      /// 儲存資料到MGR1_SMA MODEL_TYPE = 'E'
      /// </summary>
      /// <param name="MGR1_SMA"></param>
      /// <param name="worksheet"></param>
      private void SaveMGR1smaTypeE(DataTable MGR1_SMA, Worksheet worksheet)
      {
         if (MGR1_SMA == null || MGR1_SMA.Rows.Count <= 0) {
            return;
         }

         DataRow row = MGR1_SMA.Rows[0];

         //模型(S:SMA/M:MAXV/E:EWMA)
         row["MGR1_MODEL_TYPE"] = "E";
         //日期
         row["MGR1_YMD"] = worksheet.Cells["B3"].Value.DateTimeValue.ToString("yyyyMMdd");
         //商品代號
         row["MGR1_M_KIND_ID"] = worksheet.Cells["C1"].Value;
         //B開參/最低價 (A-B)
         row["MGR1_PRICE1"] = worksheet.Cells["E3"].Value.NumericValue;
         //A收盤價/最高價/開參 (A-B)
         row["MGR1_PRICE2"] = worksheet.Cells["D3"].Value.NumericValue;
         //每日報酬率
         row["MGR1_RETURN_RATE"] = worksheet.Cells["G3"].Value.NumericValue;
         //30日平均報酬率 
         row["MGR1_30_AVG_RRATE"] = worksheet.Cells["AK3"].Value.NumericValue;
         //60日平均報酬率
         row["MGR1_60_AVG_RRATE"] = worksheet.Cells["AL3"].Value.NumericValue;
         //90日平均報酬率
         row["MGR1_90_AVG_RRATE"] = worksheet.Cells["AM3"].Value.NumericValue;
         //180日平均報酬率
         row["MGR1_180_AVG_RRATE"] = worksheet.Cells["AN3"].Value.NumericValue;
         //30日報酬率標準差
         row["MGR1_30_STD"] = worksheet.Cells["AU3"].Value.NumericValue;
         //60日報酬率標準差 
         row["MGR1_60_STD"] = worksheet.Cells["AV3"].Value.NumericValue;
         //90日報酬率標準差
         row["MGR1_90_STD"] = worksheet.Cells["AW3"].Value.NumericValue;
         //180日報酬率標準差
         row["MGR1_180_STD"] = worksheet.Cells["AX3"].Value.NumericValue;
         /*資料計算狀態
           N  正常                
           X  不計算*/
         row["MGR1_STATUS_CODE"] = "N";
         //資料筆數
         row["MGR1_DATA_CNT"] = worksheet.Cells["A3"].Value.NumericValue;
         //轉檔時間
         row["MGR1_W_TIME"] = DateTime.Now;
         //2500日平均報酬率
         row["MGR1_2500_AVG_RRATE"] = worksheet.Cells["AO3"].Value.NumericValue;
         //2500日報酬率標準差
         row["MGR1_2500_STD"] = worksheet.Cells["AY3"].Value.NumericValue;
         //市場別 
         row["MGR1_PROD_TYPE"] = worksheet.Cells["H1"].Value;
         //收盤群組
         row["MGR1_OSW_GRP"] = worksheet.Cells["G1"].Value;

         //存檔
         dao40010.UpdateMGR1_SMA(MGR1_SMA);
      }

      /// <summary>
      /// 儲存資料到MGR1_SMA MODEL_TYPE = 3
      /// </summary>
      /// <param name="MGR1_SMA"></param>
      /// <param name="worksheet"></param>
      private void SaveMGR1smaType3(DataTable MGR1_SMA, Worksheet worksheet)
      {
         if (MGR1_SMA == null || MGR1_SMA.Rows.Count <= 0) {
            return;
         }

         DataRow row = MGR1_SMA.Rows[0];

         //模型(S:SMA/M:MAXV/E:EWMA)
         row["MGR1_MODEL_TYPE"] = "3";
         //日期
         row["MGR1_YMD"] = worksheet.Cells["B3"].Value.DateTimeValue.ToString("yyyyMMdd");
         //商品代號
         row["MGR1_M_KIND_ID"] = worksheet.Cells["C1"].Value;
         //B開參/最低價 (A-B)
         row["MGR1_PRICE1"] = worksheet.Cells["E3"].Value.NumericValue;
         //A收盤價/最高價/開參 (A-B)
         row["MGR1_PRICE2"] = worksheet.Cells["D3"].Value.NumericValue;
         //每日報酬率
         row["MGR1_RETURN_RATE"] = worksheet.Cells["G3"].Value.NumericValue;
         //30日平均報酬率 
         row["MGR1_30_AVG_RRATE"] = worksheet.Cells["U3"].Value.NumericValue;
         //60日平均報酬率
         row["MGR1_60_AVG_RRATE"] = worksheet.Cells["V3"].Value.NumericValue;
         //90日平均報酬率
         row["MGR1_90_AVG_RRATE"] = worksheet.Cells["W3"].Value.NumericValue;
         //180日平均報酬率
         row["MGR1_180_AVG_RRATE"] = worksheet.Cells["X3"].Value.NumericValue;
         //30日報酬率標準差
         row["MGR1_30_STD"] = worksheet.Cells["Z3"].Value.NumericValue;
         //60日報酬率標準差 
         row["MGR1_60_STD"] = worksheet.Cells["AA3"].Value.NumericValue;
         //90日報酬率標準差
         row["MGR1_90_STD"] = worksheet.Cells["AB3"].Value.NumericValue;
         //180日報酬率標準差
         row["MGR1_180_STD"] = worksheet.Cells["AC3"].Value.NumericValue;
         /*資料計算狀態
           N  正常                
           X  不計算*/
         row["MGR1_STATUS_CODE"] = "N";
         //資料筆數
         row["MGR1_DATA_CNT"] = worksheet.Cells["A3"].Value.NumericValue;
         //轉檔時間
         row["MGR1_W_TIME"] = DateTime.Now;
         //2500日平均報酬率
         row["MGR1_2500_AVG_RRATE"] = worksheet.Cells["Y3"].Value.NumericValue;
         //2500日報酬率標準差
         row["MGR1_2500_STD"] = worksheet.Cells["AD3"].Value.NumericValue;
         //市場別 
         row["MGR1_PROD_TYPE"] = worksheet.Cells["H1"].Value;
         //收盤群組
         row["MGR1_OSW_GRP"] = worksheet.Cells["G1"].Value;

         //存檔
         dao40010.UpdateMGR1_SMA(MGR1_SMA);
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
      /// 儲存至MGR2_SMA
      /// </summary>
      /// <param name="MGR2_SMA"></param>
      public ResultStatus MGR2SaveToDB(DataTable MGR2_SMA)
      {
         //儲存至DB
         return dao40010.UpdateMGR2_SMA(MGR2_SMA).Status;

      }

      /// <summary>
      /// 儲存至MGR1_SMA
      /// </summary>
      /// <param name="MGR1_SMA"></param>
      public ResultStatus MGR1SaveToDB(DataTable MGR1_SMA)
      {
         //儲存至DB
         return dao40010.UpdateMGR1_SMA(MGR1_SMA).Status;
      }

      /// <summary>
      /// 執行opt.sp_O_gen_H_MG1_3M
      /// </summary>
      /// <param name="oswGrp">交易時段</param>
      public void ExecuteSP(string oswGrp)
      {
         DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
#if DEBUG
         //dao40010.SP_O_gen_H_MG1_3M(emdate, oswGrp);
#else
            dao40010.SP_O_gen_H_MG1_3M(emdate, oswGrp);
#endif
      }

      /// <summary>
      /// 新增EWMA計算按鈕，產出資料，並將excel計算資料回寫資料庫
      /// </summary>
      /// <returns></returns>
      public string ComputeEWMA(string FilePath, string KindID)
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(FilePath);
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
            worksheet.Cells["H1"].SetValue(RowData.Rows[0]["MGP1_PROD_TYPE"]);

            //契約規格
            worksheet.Cells["O3"].SetValue(RowData.Rows[0]["MG1_XXX"]);
            //現行結算保證金
            worksheet.Cells["P3"].SetValue(RowData.Rows[0]["MG1_CUR_CM"]);

            //共18欄位 取前7欄位
            int k = RowData.Columns.Count;
            while (k > 7) {
               RowData.Columns.Remove(RowData.Columns[7].ColumnName);
               k--;//刪除後面的欄位直到剩下前面7欄
            }

            worksheet.Import(RowData, false, 2, 0);
            worksheet.ScrollTo(0, 0);
            //save
            workbook.SaveDocument(FilePath);

            //3.確認有無相同資料存在
            DataTable MGR2_SMA = dao40010.ListMGR2_SMA(lsDate, KindID);
            SaveMGR2smaTypeE(DataTableModify(MGR2_SMA), worksheet);
            //MODEL_TYPE = 'E'
            DataTable MGR1_SMAe = dao40010.ListMGR1_SMA("E", lsDate, KindID);
            SaveMGR1smaTypeE(DataTableModify(MGR1_SMAe), worksheet);
            //MODEL_TYPE = 3
            DataTable MGR1_SMA3 = dao40010.ListMGR1_SMA("3", lsDate, KindID);
            SaveMGR1smaType3(DataTableModify(MGR1_SMA3), worksheet);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"ComputeEWMA:" + ex.Message);
#else
            throw ex;
#endif
         }

         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// 新增EWMA計算按鈕，產出資料，並將excel計算資料回寫資料庫，7/11以前開發的function
      /// </summary>
      /// <returns></returns>
      public DataRow ComputeEWMA_V01(string FilePath, string KindID)
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
