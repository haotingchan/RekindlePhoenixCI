using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Drawing;
/// <summary>
/// 20190408,john,調整保證金資料記錄
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 調整保證金資料記錄
   /// </summary>
   public class B40060
   {
      private readonly string _lsFile;
      private readonly DataTable _Data;
      private readonly int _emCount;

      public B40060(string FilePath, DataTable dataTable, int emCount)
      {
         _lsFile = FilePath;
         _emCount = emCount;
         _Data = dataTable;
      }

      /// <summary>
      /// 跨商品折抵率 契約價值耗用比率 共用方法 sp3_type='SS'or 'SD'
      /// </summary>
      /// <param name="worksheet"></param>
      /// <param name="dt"></param>
      private void RefactoringMethod(Worksheet worksheet, DataTable dt)
      {
         int ColorDecimal = 11711154;//color十進位代碼 預設為淺灰
         int RowIndex = 3;//Excel的Row位置
         int tolCount = _emCount == 0 ? 9999 : _emCount;

         string kindID1 = "";
         string kindID2 = "";
         int bgColor = 0;
         for (int k = 0; k < dt.Rows.Count; k++) {
            DataRow row = dt.Rows[k];
            if (kindID1 != row["SP3_KIND_ID1"].AsString() || kindID2 != row["SP3_KIND_ID2"].AsString()) {
               kindID1 = row["SP3_KIND_ID1"].AsString();
               kindID2 = row["SP3_KIND_ID2"].AsString();

               int grpCount = row["CP_CNT"].AsInt();

               bgColor = bgColor != ColorDecimal ? ColorDecimal : 16777215;//灰底or白底
               worksheet.Range[$"{RowIndex + 1 }:{RowIndex + grpCount}"].Fill.BackgroundColor = Color.FromArgb(bgColor);

               if (grpCount > 0 && grpCount > tolCount) {
                  k = k + (grpCount - tolCount);
               }
            }//if (kindID1 != row["SP3_KIND_ID1"].AsString() || kindID2 != row["SP3_KIND_ID2"].AsString())

            RowIndex = RowIndex + 1;

            worksheet.Cells[$"A{RowIndex}"].SetValue(row["CNT"]);
            worksheet.Cells[$"B{RowIndex}"].SetValue(row["SP3_DATE"]);
            worksheet.Cells[$"C{RowIndex}"].SetValue(row["SP3_KIND_ID1"]);
            worksheet.Cells[$"D{RowIndex}"].SetValue(row["SP3_KIND_ID2"]);
            worksheet.Cells[$"E{RowIndex}"].SetValue(row["SP3_RATE"]);

            if (row["CNT"].AsInt() != 0) {
               worksheet.Cells[$"F{RowIndex}"].Formula = string.Format("=IF(C{0}=C{1},(E{0}-E{1})/E{1},\"\")", RowIndex, RowIndex - 1);
            }

         }//for (int k = 0; k < dt.Rows.Count; k++)
      }

      /// <summary>
      /// wf_40061()
      /// </summary>
      /// <param name="ColorDecimal">color十進位代碼 預設為淺灰</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <returns></returns>
      public bool Wf40061(int ColorDecimal = 11711154, int RowIndex = 3)
      {
         try {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets["波動度偵測全距"];
            worksheet.Cells["E1"].Value = "作業日期：" + DateTime.Now.ToLongDateString().ToString();
            DataTable dt = _Data.Filter("sp3_type='SV' and trim(sp3_kind_id1) not in ('RHF','RTF')");
            int tolCount = _emCount == 0 ? 9999 : _emCount;

            string kindID1 = "";
            string kindID2 = "";
            int bgColor = 0;
            for (int k = 0; k < dt.Rows.Count; k++) {
               DataRow row = dt.Rows[k];
               if (kindID1 != row["SP3_KIND_ID1"].AsString()|| kindID2!= row["SP3_KIND_ID2"].AsString()) {
                  kindID1 = row["SP3_KIND_ID1"].AsString();
                  kindID2 = row["SP3_KIND_ID2"].AsString();

                  int grpCount = row["CP_CNT"].AsInt();

                  bgColor = bgColor != ColorDecimal ? ColorDecimal : 16777215;//灰底or白底
                  worksheet.Range[$"{RowIndex + 1 }:{RowIndex+ grpCount}"].Fill.BackgroundColor = Color.FromArgb(bgColor);

                  if (grpCount > 0 && grpCount > tolCount) {
                     k = k + (grpCount - tolCount);
                  }
               }//if (kindID1 != row["SP3_KIND_ID1"].AsString() || kindID2 != row["SP3_KIND_ID2"].AsString())

               RowIndex = RowIndex + 1;

               worksheet.Cells[$"A{RowIndex}"].SetValue(row["CNT"]);
               worksheet.Cells[$"B{RowIndex}"].SetValue(row["SP3_DATE"]);
               worksheet.Cells[$"C{RowIndex}"].SetValue(row["SP3_KIND_ID1"]);
               worksheet.Cells[$"D{RowIndex}"].SetValue(row["SP3_RATE"].AsDecimal()/100);

               if (row["CNT"].AsInt() != 0) {
                  worksheet.Cells[$"E{RowIndex}"].Formula = string.Format("=IF(C{0}=C{1},(D{0}-D{1})/D{1},\"\")", RowIndex, RowIndex - 1);
               }
               
            }//for (int k = 0; k < dt.Rows.Count; k++)

            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40061:" + ex.Message);
#else
            throw ex;
#endif
         }
         return true;
      }

      /// <summary>
      /// wf_40062()
      /// </summary>
      /// <returns></returns>
      public bool Wf40062()
      {
         try {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets["跨商品折抵率"];
            worksheet.Cells["F1"].Value = "作業日期：" + DateTime.Now.ToLongDateString().ToString();
            DataTable dt = _Data.Filter("sp3_type='SS'");
            RefactoringMethod(worksheet, dt);

            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40062:" + ex.Message);
#else
            throw ex;
#endif
         }
         return true;
      }

      /// <summary>
      /// wf_40063()
      /// </summary>
      /// <returns></returns>
      public bool Wf40063()
      {
         try {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets["契約價值耗用比率"];
            worksheet.Cells["F1"].Value = "作業日期：" + DateTime.Now.ToLongDateString().ToString();
            DataTable dt = _Data.Filter("sp3_type='SD'");
            RefactoringMethod(worksheet, dt);

            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40063:" + ex.Message);
#else
            throw ex;
#endif
         }
         return true;
      }

   }
}
