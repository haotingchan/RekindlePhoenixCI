using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
/// <summary>
/// john,20190611
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

      public void MGR2Save(DataTable MGR2_SMA)
      {
         //儲存至DB
         dao40010.UpdateMGR2_SMA(MGR2_SMA);
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
            Worksheet worksheet = workbook.Worksheets["Rawdata"];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");

            //1.RowData sheet第一行
            string lsDate = emdate.ToString("yyyyMMdd");
            //確認有無資料
            DataTable RowData = dao40010.ListRowDataSheet(lsDate, KindID);
            if (RowData.Rows.Count <= 0) {
               File.Delete(FilePath);
               return null;
            }

            worksheet.Cells["A1"].SetValue(RowData.Rows[0]["MG1_PROD_SUBTYPE"]);
            worksheet.Cells["C1"].SetValue(RowData.Rows[0]["KIND_ID"]);
            worksheet.Cells["D1"].SetValue(RowData.Rows[0]["MG1_PARAM_KEY"]);
            worksheet.Cells["E1"].SetValue(RowData.Rows[0]["MG1_M_KIND_ID"]);
            worksheet.Cells["F1"].SetValue(RowData.Rows[0]["MG1_OSW_GRP"]);
            worksheet.Cells["P3"].SetValue(RowData.Rows[0]["MG1_CUR_CM"]);
            worksheet.Cells["N1"].SetValue(RowData.Rows[0]["MG1_MIN_RISK"]);
            worksheet.Cells["O3"].SetValue(RowData.Rows[0]["MG1_XXX"]);

            //2.寫入40010template計算EWMA
            //共15欄位 取前7欄位
            for (int k = 7; k < RowData.Columns.Count; k++) {
               RowData.Columns.Remove(RowData.Columns[7].ColumnName);//刪除後面8欄
            }
            worksheet.Import(RowData, false, 2, 0);
            worksheet.ScrollTo(0, 0);
            //save
            workbook.SaveDocument(FilePath);

            //3.RowData寫回DB
            //確認有無資料 沒有則新增一行
            DataTable MGR2_SMA = dao40010.ListMGR2_SMA(lsDate, KindID);
            
            if (MGR2_SMA.Rows.Count <= 0) {
               row = MGR2_SMA.NewRow();
               MGR2_SMA.Rows.Add(row);
            }
            else {
               row = MGR2_SMA.Rows[0];
            }
            string f6 = "F6";
            string f4 = "F4";
            string f2 = "F2";

            //模型(S:SMA/M:MAXV/E:EWMA)
            row["MGR2_MODEL_TYPE"] = "E";
            //日期
            row["MGR2_YMD"] = worksheet.Cells["B3"].Value.DateTimeValue.ToString("yyyyMMdd");
            //股票代號
            row["MGR2_M_KIND_ID"] = worksheet.Cells["E1"].Value;
            //B開參/最低價 (A-B)
            row["MGR2_PRICE1"] = worksheet.Cells["E3"].Value.NumericValue.ToString(f2).AsDecimal();
            //A收盤價/最高價/開參 (A-B)
            row["MGR2_PRICE2"] = worksheet.Cells["D3"].Value.NumericValue.ToString(f2).AsDecimal();
            //每日報酬率
            row["MGR2_RETURN_RATE"] = worksheet.Cells["G3"].Value.NumericValue.ToString(f2).AsDecimal();
            //30日風險價格係數
            row["MGR2_30_RATE"] = worksheet.Cells["H3"].Value.NumericValue.ToString(f2).AsDecimal();
            //60日風險價格係數
            row["MGR2_60_RATE"] = worksheet.Cells["I3"].Value.NumericValue.ToString(f2).AsDecimal();
            //90日風險價格係數
            row["MGR2_90_RATE"] = worksheet.Cells["J3"].Value.NumericValue.ToString(f2).AsDecimal();
            //180日風險價格係數
            row["MGR2_180_RATE"] = worksheet.Cells["K3"].Value.NumericValue.ToString(f2).AsDecimal();
            //2500日風險價格係數
            row["MGR2_2500_RATE"] = worksheet.Cells["L3"].Value.NumericValue.ToString(f2).AsDecimal();
            //風險價格係數最大值
            row["MGR2_MIN_RATE"] = worksheet.Cells["N1"].Value.NumericValue.ToString(f2).AsDecimal();
            //每日風險價格係數
            row["MGR2_DAY_RATE"] = worksheet.Cells["N3"].Value.NumericValue.ToString(f2).AsDecimal();
            /*資料計算狀態
              N  正常                
              X  不計算*/
            row["MGR2_STATUS_CODE"] = "N";
            //股票市場別
            row["MGR2_PROD_TYPE"] = worksheet.Cells["B1"].Value;
            //子類別
            row["MGR2_PROD_SUBTYPE"] = worksheet.Cells["A1"].Value;
            //契約對照瑪
            row["MGR2_PARAM_KEY"] = worksheet.Cells["D1"].Value;
            //計算的風險價格係數
            row["MGR2_CP_RATE"] = worksheet.Cells["M3"].Value.NumericValue.ToString(f2).AsDecimal();
            //計算的風險價格係數(涵蓋1日)
            row["MGR2_1DAY_CP_RATE"] = worksheet.Cells["M1"].Value.NumericValue.ToString(f2).AsDecimal();
            //轉檔時間
            row["MGR2_W_TIME"] = DateTime.Now;
            //收盤群組
            row["MGR2_OSW_GRP"] = worksheet.Cells["F1"].Value;
            
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
