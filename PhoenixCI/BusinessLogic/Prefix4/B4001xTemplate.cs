using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
/// <summary>
/// john,20190412,保證金狀況表 (Group 1/2/3)
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 保證金狀況表 (Group 1/2/3)
   /// </summary>
   public class B4001xTemplate : I4001x
   {
      /// <summary>
      /// 輸出Excel檔案路徑
      /// </summary>
      protected string _lsFile;
      /// <summary>
      /// 輸入的日期 yyyy/MM/dd
      /// </summary>
      protected string _emDateText;
      /// <summary>
      /// Data
      /// </summary>
      protected ID4001x dao;
      /// <summary>
      /// 程式代號(_ProgramID)
      /// </summary>
      protected string _TxnID;

      protected Workbook _workbook;

      public B4001xTemplate()
      {
         _workbook = new Workbook();
      }

      public I4001x ConcreteClass(string programID, object[] args = null)
      {

         //string className = string.Format("{0}.Dao.Together.SpecificDao.{1}",AssemblyName, name);//完整的class路徑

         string AssemblyName = GetType().Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = GetType().FullName.Replace("B4001xTemplate", "B" + programID);//完整的class路徑

         // 這裡就是Reflection，直接依照className實體化具體類別
         return (I4001x)Assembly.Load(AssemblyName).CreateInstance(className, true, BindingFlags.CreateInstance, null, args, null, null);
      }

      public enum SheetName : int
      {
         /// <summary>
         /// rpt_future
         /// </summary>
         Rpt_Future = 0,
         /// <summary>
         /// rpt_option
         /// </summary>
         Rpt_Option = 1
      }
      #region 共用方法

      /// <summary>
      /// 判斷FMIF資料已轉入
      /// </summary>
      /// <returns></returns>
      public string CheckFMIF()
      {
         //判斷FMIF資料已轉入
         DateTime inputDT = _emDateText.AsDateTime();
         int cnt = dao.CheckFMIF(inputDT, GetOswGrp());
         if (cnt == 0) {
            return _emDateText + "期貨結算價資料未轉入完畢,是否要繼續?";
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// 130批次作業做完
      /// </summary>
      /// <returns></returns>
      public string Check130Wf()
      {
         DateTime inputDT = _emDateText.AsDateTime();
         string strRtn = PbFunc.f_chk_130_wf(_TxnID, inputDT, GetOswGrp());
         if (!string.IsNullOrEmpty(strRtn)) {
            return $"{_emDateText}-{strRtn}，是否要繼續?";
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// FMIF APDK_MARKET_CLOSE 值
      /// </summary>
      /// <returns>1 or 5 or 7</returns>
      protected virtual string GetOswGrp()
      {
         return "1";
      }
      #endregion


      #region 期貨

      /// <summary>
      /// rpt_future工作表 兩筆作業項目儲存格間距
      /// </summary>
      /// <returns></returns>
      protected virtual int FutWorkItemCellDist()
      {
         return 3;
      }

      #endregion

      #region 選擇權

      /// <summary>
      /// rpt_option工作表 兩筆作業項目儲存格間距
      /// </summary>
      /// <returns></returns>
      protected virtual int OptWorkItemCellDist()
      {
         return 2;
      }

      #endregion

      /// <summary>
      /// sheet rpt_future
      /// </summary>
      /// <returns></returns>
      public virtual string WfFutureSheet(int sheetIndex = 0)
      {
         try {
            //切換Sheet
            _workbook.LoadDocument(_lsFile);
            Worksheet worksheet = _workbook.Worksheets[sheetIndex];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["I1"].Value = "資料日期：" + emdate.ToLongDateString().ToString();
            const int SheetOne = 1;//第一張sheet

            //確認有無資料
            DataTable dt = dao.ListFutOptData(emdate, _TxnID, SheetOne);
            if (dt.Rows.Count <= 0) {
               return $"{_emDateText},{_TxnID}_{SheetOne}－保證金狀況表,無任何資料!";
            }

            StringBuilder ItemOne = new StringBuilder();
            StringBuilder ItemTwo = new StringBuilder();
            StringBuilder ItemThree = new StringBuilder();
            string kindIdOut = "";

            foreach (DataRow dr in dt.Rows) {
               //一、現行收取保證金金額：CDEFGH
               int R1rowIndex = dr["R1"] != DBNull.Value ? dr["R1"].AsInt() : 0;

               if (R1rowIndex > 0) {
                  for (int j = 0; j < 6; j++) {
                     worksheet.Rows[R1rowIndex - 1][j + 2].SetValue(dr[j]);
                  }
               }//if (R1rowIndex > 0) 

               //二、	本日結算保證金計算：CDEFGH
               int R2rowIndex = dr["R2"] != DBNull.Value ? dr["R2"].AsInt() : 0;

               if (R2rowIndex > 0 && dr["MG1_KIND_ID"].AsString() != "SGX02") {
                  for (int j = 7 - 1; j < 14; j++) {
                     worksheet.Rows[R2rowIndex - 1][j - 4].SetValue(dr[j]);
                  }
               }//if (R2rowIndex > 0 && dr["MG1_KIND_ID"].AsString() != "SGX02")


               //四、	作業事項
               if (!string.IsNullOrEmpty(dr["MG1_PROD_TYPE"].AsString())
                  && kindIdOut != dr["MGT2_KIND_ID_OUT"].AsString()) {
                  kindIdOut = dr["MGT2_KIND_ID_OUT"].AsString();
                  bool ySMA = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(2)
                              .Where(r => r.Field<object>("MGT2_KIND_ID_OUT").AsString() == kindIdOut
                              && r.Field<object>("MG1_MODEL_TYPE").AsString() == "S"
                              && r.Field<object>("MG1_CHANGE_FLAG").AsString() == "Y").Any();
                  bool yMAX = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(2)
                              .Where(r => r.Field<object>("MGT2_KIND_ID_OUT").AsString() == kindIdOut
                              && r.Field<object>("MG1_MODEL_TYPE").AsString() == "M"
                              && r.Field<object>("MG1_CHANGE_FLAG").AsString() == "Y").Any();
                  bool yEWMA = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(2)
                              .Where(r => r.Field<object>("MGT2_KIND_ID_OUT").AsString() == kindIdOut
                              && r.Field<object>("MG1_MODEL_TYPE").AsString() == "E"
                              && r.Field<object>("MG1_CHANGE_FLAG").AsString() == "Y").Any();
                  //1.以SMA及MAX計算之保證金變動幅度均未達 10% 得調整標準
                  ItemOne.Append(!ySMA && !yMAX ? "■" : "□");
                  ItemOne.Append(kindIdOut + "　");
                  //2.以EWMA計算保證金變動幅度已達 10%得調整標準，且進位後金額改變
                  ItemTwo.Append(yEWMA ? "■" : "□");
                  ItemTwo.Append(kindIdOut + "　");
                  //3.以SMA或MAX計算之保證金變動幅度已達 10%得調整標準
                  ItemThree.Append(ySMA || yMAX ? "■" : "□");
                  ItemThree.Append(kindIdOut + "　");
               }//if (!string.IsNullOrEmpty(dr["MG1_PROD_TYPE"].AsString()))

            }//foreach (DataRow dr in dt.Rows)

            //四、	作業事項
            int itemRowIndex = dao.GetRptLV(_TxnID, SheetOne);
            if (itemRowIndex > 0) {
               int dist = FutWorkItemCellDist();
               worksheet.Cells[$"B{itemRowIndex}"].Value = ItemOne.ToString();
               worksheet.Cells[$"B{itemRowIndex + dist}"].Value = ItemTwo.ToString();
               worksheet.Cells[$"B{itemRowIndex + dist*2}"].Value = ItemThree.ToString();
            }

            worksheet.ScrollTo(0, 0);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"WfFutureSheet:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            //save
            _workbook.SaveDocument(_lsFile);
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// sheet rpt_option
      /// </summary>
      /// <returns></returns>
      public virtual string WfOptionSheet(int sheetIndex = 1)
      {
         try {
            //切換Sheet
            _workbook.LoadDocument(_lsFile);
            Worksheet worksheet = _workbook.Worksheets[sheetIndex];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["J3"].Value = "資料日期：" + emdate.ToLongDateString().ToString();
            const int SheetTwo = 2;//第二張sheet

            //確認有無資料
            DataTable dt = dao.ListFutOptData(emdate, _TxnID, SheetTwo);
            if (dt.Rows.Count <= 0) {
               return $"{_emDateText},{_TxnID}_{SheetTwo}－保證金狀況表,無任何資料!";
            }

            StringBuilder ItemOne = new StringBuilder();
            StringBuilder ItemTwo = new StringBuilder();
            StringBuilder ItemThree = new StringBuilder();
            string kindIdOut = "";
            foreach (DataRow dr in dt.Rows) {
               //一、現行收取保證金金額：CDEFGH
               int R1rowIndex = dr["R1"] != DBNull.Value ? dr["R1"].AsInt() : 0;

               if (R1rowIndex > 0) {
                  if (dr["MG1_AB_TYPE"].AsString() == "B")
                     R1rowIndex = R1rowIndex + 1;

                  for (int j = 0; j < 3; j++) {
                     worksheet.Rows[R1rowIndex - 1][(j + 1) * 2].SetValue(dr[j]);
                  }
               }//if (R1rowIndex > 0) 

               //二、	本日結算保證金計算：CDEFGH 
               int R2rowIndex = dr["R2"] != DBNull.Value ? dr["R2"].AsInt() : 0;

               if (R2rowIndex > 0) {
                  if (dr["MG1_AB_TYPE"].AsString() == "B")
                     R2rowIndex = R2rowIndex + 1;

                  for (int j = 7 - 1; j < 14; j++) {
                     if (dr["MG1_AB_TYPE"].AsString() == "B" && dt.Columns[j].ColumnName == "MG1_MIN_RISK")
                        continue;
                     worksheet.Rows[R2rowIndex - 1][j - 3].SetValue(dr[j]);
                  }
               }//if (R2rowIndex > 0)

               //四、	作業事項
               if (!string.IsNullOrEmpty(dr["MG1_PROD_TYPE"].AsString())
                  && kindIdOut != dr["MGT2_KIND_ID_OUT"].AsString()) {
                  kindIdOut = dr["MGT2_KIND_ID_OUT"].AsString();
                  bool ySMA = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(2)
                              .Where(r => r.Field<object>("MGT2_KIND_ID_OUT").AsString() == kindIdOut
                              && r.Field<object>("MG1_MODEL_TYPE").AsString() == "S"
                              && r.Field<object>("MG1_CHANGE_FLAG").AsString() == "Y").Any();
                  bool yMAX = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(2)
                              .Where(r => r.Field<object>("MGT2_KIND_ID_OUT").AsString() == kindIdOut
                              && r.Field<object>("MG1_MODEL_TYPE").AsString() == "M"
                              && r.Field<object>("MG1_CHANGE_FLAG").AsString() == "Y").Any();
                  bool yEWMA = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(2)
                              .Where(r => r.Field<object>("MGT2_KIND_ID_OUT").AsString() == kindIdOut
                              && r.Field<object>("MG1_MODEL_TYPE").AsString() == "E"
                              && r.Field<object>("MG1_CHANGE_FLAG").AsString() == "Y").Any();
                  //1.以SMA及MAX計算之保證金變動幅度均未達 10% 得調整標準
                  ItemOne.Append(!ySMA && !yMAX ? "■" : "□");
                  ItemOne.Append(kindIdOut + "　");
                  //2.以EWMA計算保證金變動幅度已達 10%得調整標準，且進位後金額改變
                  ItemTwo.Append(yEWMA ? "■" : "□");
                  ItemTwo.Append(kindIdOut + "　");
                  //3.以SMA或MAX計算之保證金變動幅度已達 10%得調整標準
                  ItemThree.Append(ySMA || yMAX ? "■" : "□");
                  ItemThree.Append(kindIdOut + "　");
               }//四、	作業事項

            }//foreach (DataRow dr in dt.Rows) 

            //四、	作業事項

            int itemRowIndex = dao.GetRptLV(_TxnID, SheetTwo);
            if (itemRowIndex > 0) {
               int dist = OptWorkItemCellDist();
               worksheet.Cells[$"B{itemRowIndex}"].Value = ItemOne.ToString();
               worksheet.Cells[$"B{itemRowIndex + dist}"].Value = ItemTwo.ToString();
               worksheet.Cells[$"B{itemRowIndex + dist * 2}"].Value = ItemThree.ToString();
            }

            //save
            worksheet.ScrollTo(0, 0);
            //workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"WfOptionSheet:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            _workbook.SaveDocument(_lsFile);
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_40011_3
      /// </summary>
      /// <param name="AsProdType">O or F</param>
      /// <param name="SheetName">工作表名稱</param>
      /// <returns></returns>
      public string WfStat(string AsProdType, string SheetName)
      {
         try {
            //切換Sheet
            _workbook.LoadDocument(_lsFile);
            Worksheet worksheet = _workbook.Worksheets[SheetName];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["A1"].Value = "資料日期：" + emdate.ToLongDateString().ToString();
            //確認有無資料
            DataTable dt = dao.List40011Stat(_emDateText.Replace("/", "")).Sort("seq_no,kind_id ");
            dt = dt.Filter($"prod_type ='{AsProdType}' and prod_subtype <> 'S' and osw_grp like '{GetOswGrp()}%'");
            if (dt.Rows.Count <= 0) {
               return $"{_emDateText},40011_stat－保證金狀況表,無任何資料!";
            }

            //共42欄位 取前37欄位
            for (int k = 0; k < 5; k++) {
               dt.Columns.Remove(dt.Columns[37].ColumnName);//刪除後面5欄
            }
            //寫入資料
            worksheet.Import(dt, false, 2, 0);

            //save
            worksheet.ScrollTo(0, 0);
            //workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"WfStat:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            //save
            _workbook.SaveDocument(_lsFile);
         }
         return MessageDisplay.MSG_OK;
      }


   }

   public interface I4001x
   {

      /// <summary>
      /// 判斷FMIF資料已轉入
      /// </summary>
      string CheckFMIF();

      /// <summary>
      /// 130批次作業做完
      /// </summary>
      string Check130Wf();

      /// <summary>
      /// sheet rpt_future
      /// </summary>
      /// <param name="sheetIndex">工作表</param>
      /// <returns></returns>
      string WfFutureSheet(int sheetIndex = 0);

      /// <summary>
      /// sheet rpt_option
      /// </summary>
      /// <param name="sheetIndex">工作表</param>
      /// <returns></returns>
      string WfOptionSheet(int sheetIndex = 1);

      /// <summary>
      /// wf_40011_3
      /// </summary>
      /// <param name="AsProdType">O or F</param>
      /// <param name="SheetName">工作表名稱</param>
      string WfStat(string AsProdType, string SheetName);

   }



}
