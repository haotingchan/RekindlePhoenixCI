using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Linq;
using System.Text;
/// <summary>
/// john,20190710,保證金狀況表 (Group1)
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 保證金狀況表 (Group1)
   /// </summary>
   public class B40011 : B4001xTemplate
   {

      public B40011(string daoID, string FilePath, string emDate)
      {
         this._TxnID = "40011";
         this._lsFile = FilePath;
         this._emDateText = emDate;
         this.dao = new D4001x().ConcreteDao(daoID);
         this._workbook = new Workbook();
      }

      /// <summary>
      /// FMIF APDK_MARKET_CLOSE 值
      /// </summary>
      /// <returns>1 or 5 or 7</returns>
      protected override string GetOswGrp()
      {
         return "1";
      }

      /// <summary>
      /// sheet rpt_option
      /// </summary>
      /// <returns></returns>
      public override string WfOptionSheet(int sheetIndex = 1)
      {
         try {
            //切換Sheet
            _workbook.LoadDocument(_lsFile);
            Worksheet worksheet = _workbook.Worksheets[sheetIndex];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Cells["K3"].Value = "資料日期：" + emdate.ToLongDateString().ToString();
            const int SheetTwo = 2;//第二張sheet

            //確認有無資料
            DataTable dt = dao.ListFutOptData(emdate, _TxnID, SheetTwo);
            if (dt.Rows.Count <= 0) {
               return $"{_emDateText},{_TxnID}_{SheetTwo}－保證金狀況表,無任何資料!";
            }

            StringBuilder ItemOne = new StringBuilder();
            StringBuilder ItemTwo = new StringBuilder();
            StringBuilder ItemThree = new StringBuilder();
            StringBuilder ItemFour = new StringBuilder();
            StringBuilder ItemFive = new StringBuilder();

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

               //三、	作業事項
               if (!string.IsNullOrEmpty(dr["MG1_PROD_TYPE"].AsString()) && kindIdOut != dr["MGT2_KIND_ID_OUT"].AsString()) {

                  kindIdOut = dr["MGT2_KIND_ID_OUT"].AsString();
                  //MG1_MODEL_TYPE大寫
                  //2.參考現貨資料，以EWMA計算之保證金變動幅度已達 10%得調整標準。
                  bool optEWMAchangFlag = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(2)
                     .Where(r => r.Field<object>("MGT2_KIND_ID_OUT").AsString().Contains(kindIdOut)
                     && (r.Field<object>("MG1_MODEL_TYPE").AsString() == "E")
                     && r.Field<object>("MG1_CHANGE_FLAG").AsString() == "Y").Any();
                  ItemTwo.Append(optEWMAchangFlag ? "■" : "□");
                  ItemTwo.Append(kindIdOut + "　");
                  //3.參考現貨資料，以SMA或MAX計算之保證金變動幅度已達 10%得調整標準，且進位後金額改變，建議事項如「保證金調整審核會議紀錄」。
                  bool optChangFlag = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(2)
                     .Where(r => r.Field<object>("MGT2_KIND_ID_OUT").AsString().Contains(kindIdOut)
                     && (r.Field<object>("MG1_MODEL_TYPE").AsString() == "M" || r.Field<object>("MG1_MODEL_TYPE").AsString() == "S")
                     && r.Field<object>("MG1_CHANGE_FLAG").AsString() == "Y").Any();
                  ItemThree.Append(optChangFlag ? "■" : "□");
                  ItemThree.Append(kindIdOut + "　");

                  //MG1_MODEL_TYPE小寫
                  //4.參考期貨資料，以EWMA計算之保證金變動幅度已達 10%得調整標準。
                  bool futEWMAchangFlag = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(2)
                     .Where(r => r.Field<object>("MGT2_KIND_ID_OUT").AsString().Contains(kindIdOut)
                     && (r.Field<object>("MG1_MODEL_TYPE").AsString() == "e")
                     && r.Field<object>("MG1_CHANGE_FLAG").AsString() == "Y").Any();
                  ItemFour.Append(futEWMAchangFlag ? "■" : "□");
                  ItemFour.Append(kindIdOut + "　");
                  //5.參考期貨資料，以SMA或MAX計算之保證金變動幅度已達 10%得調整標準，且進位後金額改變，建議事項如「保證金調整審核會議紀錄」。
                  bool futChangFlag = dt.AsEnumerable().AsParallel().WithDegreeOfParallelism(2)
                     .Where(r => r.Field<object>("MGT2_KIND_ID_OUT").AsString().Contains(kindIdOut)
                     && (r.Field<object>("MG1_MODEL_TYPE").AsString() == "m" || r.Field<object>("MG1_MODEL_TYPE").AsString() == "s")
                     && r.Field<object>("MG1_CHANGE_FLAG").AsString() == "Y").Any();
                  ItemFive.Append(futChangFlag ? "■" : "□");
                  ItemFive.Append(kindIdOut + "　");

                  //1.以SMA及MAX計算之保證金變動幅度均未達 10% 得調整標準，或雖達得調整標準但進位後金額不變，風險保證金（A值）及風險保證金最低值（B值）維持現行收取標準.
                  ItemOne.Append(!optChangFlag && !futChangFlag ? "■" : "□");
                  ItemOne.Append(kindIdOut + "　");

               }//作業事項

            }//foreach (DataRow dr in dt.Rows) 

            //三、	作業事項

            int itemRowIndex = dao.GetRptLV(_TxnID, SheetTwo);
            if (itemRowIndex > 0) {
               int dist = OptWorkItemCellDist();
               worksheet.Cells[$"B{itemRowIndex}"].Value = ItemOne.ToString();
               worksheet.Cells[$"B{itemRowIndex + dist}"].Value = ItemTwo.ToString();
               worksheet.Cells[$"B{itemRowIndex + dist * 2}"].Value = ItemThree.ToString();
               worksheet.Cells[$"B{itemRowIndex + dist * 3}"].Value = ItemFour.ToString();
               worksheet.Cells[$"B{itemRowIndex + dist * 4}"].Value = ItemFive.ToString();
            }

            //save
            worksheet.ScrollTo(0, 0);
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

   }
}
