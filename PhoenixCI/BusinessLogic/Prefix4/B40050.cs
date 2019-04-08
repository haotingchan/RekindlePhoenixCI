using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Data;
using System.Drawing;
/// <summary>
/// 20190403,john,指數類期貨及現貨資料下載
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 指數類期貨及現貨資料下載
   /// </summary>
   public class B40050
   {
      private readonly string _lsFile;
      private readonly DataTable _Data;
      private readonly int _emCount;

      public B40050(string FilePath, DataTable dataTable, int emCount)
      {
         _lsFile = FilePath;
         _emCount = emCount;
         _Data = dataTable;
      }

      private void Wf40053Fut(Worksheet worksheet, DataTable dt)
      {
         worksheet.Cells["L1"].Value = "作業日期：" + DateTime.Now.ToLongDateString().ToString();

         int ColorDecimal = 11711154;//color十進位代碼 預設為淺灰
         int RowIndex = 3;
         int tolCount = _emCount == 0 ? 9999 : _emCount;
         string kindID = "";
         int bgColor = 0;
         for (int k = 0; k < dt.Rows.Count; k++) {
            DataRow row = dt.Rows[k];
            if (kindID != row["MG4_KIND_ID"].AsString()) {
               kindID = row["MG4_KIND_ID"].AsString();

               int grpCount = row["CP_CNT"].AsInt();

               bgColor = bgColor != ColorDecimal ? ColorDecimal : 16777215;//灰底or白底
               worksheet.Range[$"{RowIndex + 1 }:{RowIndex + grpCount}"].Fill.BackgroundColor = Color.FromArgb(bgColor);

               if (grpCount > 0 && grpCount > tolCount) {
                  k = k + (grpCount - tolCount);
               }
            }//if (kindID != row["MG4_KIND_ID"].AsString())

            RowIndex = RowIndex + 1;

            worksheet.Cells[$"A{RowIndex}"].SetValue(row["CNT"]);
            worksheet.Cells[$"B{RowIndex}"].SetValue(row["MG4_DATE"]);
            worksheet.Cells[$"C{RowIndex}"].SetValue(row["MG4_KIND_ID"]);
            worksheet.Cells[$"D{RowIndex}"].SetValue(row["APDK_NAME"]);
            worksheet.Cells[$"E{RowIndex}"].SetValue(row["APDK_STOCK_ID"]);
            worksheet.Cells[$"F{RowIndex}"].SetValue(row["PID_NAME"]);
            worksheet.Cells[$"G{RowIndex}"].SetValue(row["MG4_CM"]);
            worksheet.Cells[$"H{RowIndex}"].SetValue(row["MG4_MM"]);
            worksheet.Cells[$"I{RowIndex}"].SetValue(row["MG4_IM"]);

            if (row["CNT"].AsInt() != 0) {
               worksheet.Cells[$"J{RowIndex}"].Formula = string.Format("=IF(C{0}=C{1},(G{0}-G{1})/G{1},\"\")", RowIndex, RowIndex - 1);
               worksheet.Cells[$"K{RowIndex}"].Formula = string.Format("=IF(C{0}=C{1},(H{0}-H{1})/H{1},\"\")", RowIndex, RowIndex - 1);
               worksheet.Cells[$"L{RowIndex}"].Formula = string.Format("=IF(C{0}=C{1},(I{0}-I{1})/I{1},\"\")", RowIndex, RowIndex - 1);
            }

         }//for (int k = 0; k < dt.Rows.Count; k++)
      }

      private void Wf40054Opt(Worksheet worksheet, DataTable dt)
      {
         worksheet.Cells["Q1"].Value = "作業日期：" + DateTime.Now.ToLongDateString().ToString();

         int ColorDecimal = 11711154;//color十進位代碼 預設為淺灰
         int RowIndex = 2;//Excel的Row位置
         int tolCount = _emCount == 0 ? 9999 : _emCount;

         string kindID = "";
         int bgColor = 0;
         for (int k = 0; k < dt.Rows.Count; k++) {
            DataRow row = dt.Rows[k];
            if (kindID != row["MG4_KIND_ID"].AsString()) {
               kindID = row["MG4_KIND_ID"].AsString();

               int grpCount = row["CP_CNT"].AsInt();

               bgColor = bgColor != ColorDecimal ? ColorDecimal : 16777215;//灰底or白底
               worksheet.Range[$"{RowIndex + 1 }:{RowIndex + grpCount}"].Fill.BackgroundColor = Color.FromArgb(bgColor);

               if (grpCount > 0 && grpCount > tolCount) {
                  k = k + (grpCount - tolCount);
               }
            }//if (kindID != row["MG4_KIND_ID"].AsString())

            RowIndex = RowIndex + 1;

            worksheet.Cells[$"A{RowIndex}"].SetValue(row["CNT"]);
            worksheet.Cells[$"B{RowIndex}"].SetValue(row["MG4_DATE"]);
            worksheet.Cells[$"C{RowIndex}"].SetValue(row["MG4_KIND_ID"]);
            worksheet.Cells[$"D{RowIndex}"].SetValue(row["APDK_NAME"]);
            worksheet.Cells[$"E{RowIndex}"].SetValue(row["APDK_STOCK_ID"]);
            worksheet.Cells[$"F{RowIndex}"].SetValue(row["PID_NAME"]);
            worksheet.Cells[$"G{RowIndex}"].SetValue("A值");
            worksheet.Cells[$"H{RowIndex}"].SetValue(row["MG4_CM"]);
            worksheet.Cells[$"I{RowIndex}"].SetValue(row["MG4_MM"]);
            worksheet.Cells[$"J{RowIndex}"].SetValue(row["MG4_IM"]);
            worksheet.Cells[$"K{RowIndex}"].SetValue("B值");
            worksheet.Cells[$"L{RowIndex}"].SetValue(row["MG4_CM_B"]);
            worksheet.Cells[$"M{RowIndex}"].SetValue(row["MG4_MM_B"]);
            worksheet.Cells[$"N{RowIndex}"].SetValue(row["MG4_IM_B"]);
            if (row["CNT"].AsInt() != 0) {
               worksheet.Cells[$"O{RowIndex}"].Formula = string.Format("=(H{0}-H{1})/H{1}", RowIndex, RowIndex - 1);
               worksheet.Cells[$"P{RowIndex}"].Formula = string.Format("=(I{0}-I{1})/I{1}", RowIndex, RowIndex - 1);
               worksheet.Cells[$"Q{RowIndex}"].Formula = string.Format("=(J{0}-J{1})/J{1}", RowIndex, RowIndex - 1);
            }

         }//for (int k = 0; k < dt.Rows.Count; k++)
      }


      /// <summary>
      /// wf_40051()
      /// </summary>
      /// <param name="ColorDecimal">color十進位代碼 預設為淺灰</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <returns></returns>
      public bool Wf40051(int ColorDecimal = 11711154, int RowIndex = 3)
      {
         try {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets["Fut"];
            worksheet.Cells["I1"].Value = "作業日期：" + DateTime.Now.ToLongDateString().ToString();
            DataTable dt = _Data.Filter("mg4_prod_type = 'F' and  mg4_prod_subtype <> 'S' ");
            int tolCount = _emCount == 0 ? 9999 : _emCount;

            string kindID = "";
            int bgColor = 0;
            for (int k = 0; k < dt.Rows.Count; k++) {
               DataRow row = dt.Rows[k];
               if (kindID != row["MG4_KIND_ID"].AsString()) {
                  kindID = row["MG4_KIND_ID"].AsString();
                  
                  int grpCount = row["CP_CNT"].AsInt();

                  bgColor = bgColor != ColorDecimal ? ColorDecimal : 16777215;//灰底or白底
                  worksheet.Range[$"{RowIndex + 1 }:{RowIndex+ grpCount}"].Fill.BackgroundColor = Color.FromArgb(bgColor);

                  if (grpCount > 0 && grpCount > tolCount) {
                     k = k + (grpCount - tolCount);
                  }
               }//if (kindID != row["MG4_KIND_ID"].AsString())

               RowIndex = RowIndex + 1;

               worksheet.Cells[$"A{RowIndex}"].SetValue(row["CNT"]);
               worksheet.Cells[$"B{RowIndex}"].SetValue(row["MG4_DATE"]);
               worksheet.Cells[$"C{RowIndex}"].SetValue(row["MG4_KIND_ID"]);
               worksheet.Cells[$"D{RowIndex}"].SetValue(row["MG4_CM"]);
               worksheet.Cells[$"E{RowIndex}"].SetValue(row["MG4_MM"]);
               worksheet.Cells[$"F{RowIndex}"].SetValue(row["MG4_IM"]);

               if (row["CNT"].AsInt() != 0) {
                  worksheet.Cells[$"G{RowIndex}"].Formula = string.Format("=IF(C{0}=C{1},(D{0}-D{1})/D{1},\"\")", RowIndex, RowIndex - 1);
                  worksheet.Cells[$"H{RowIndex}"].Formula = string.Format("=IF(C{0}=C{1},(E{0}-E{1})/E{1},\"\")", RowIndex, RowIndex - 1);
                  worksheet.Cells[$"I{RowIndex}"].Formula = string.Format("=IF(C{0}=C{1},(F{0}-F{1})/F{1},\"\")", RowIndex, RowIndex - 1);
               }
               
            }//for (int k = 0; k < dt.Rows.Count; k++)

            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40051:" + ex.Message);
#else
            throw ex;
#endif
         }
         return true;
      }

      /// <summary>
      /// wf_40052()
      /// </summary>
      /// <param name="ColorDecimal">color十進位代碼 預設為淺灰</param>
      /// <param name="RowIndex">Excel的Row位置</param>
      /// <returns></returns>
      public bool Wf40052(int ColorDecimal = 11711154, int RowIndex = 2)
      {
         try {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets["Opt"];
            worksheet.Cells["N1"].Value = "作業日期：" + DateTime.Now.ToLongDateString().ToString();
            DataTable dt = _Data.Filter("mg4_prod_type = 'O' and  mg4_prod_subtype <> 'S' ");
            int tolCount = _emCount == 0 ? 9999 : _emCount;

            string kindID = "";
            int bgColor = 0;
            for (int k = 0; k < dt.Rows.Count; k++) {
               DataRow row = dt.Rows[k];
               if (kindID != row["MG4_KIND_ID"].AsString()) {
                  kindID = row["MG4_KIND_ID"].AsString();

                  int grpCount = row["CP_CNT"].AsInt();

                  bgColor = bgColor != ColorDecimal ? ColorDecimal : 16777215;//灰底or白底
                  worksheet.Range[$"{RowIndex + 1 }:{RowIndex + grpCount}"].Fill.BackgroundColor = Color.FromArgb(bgColor);

                  if (grpCount > 0 && grpCount > tolCount) {
                     k = k + (grpCount - tolCount);
                  }
               }//if (kindID != row["MG4_KIND_ID"].AsString())

               RowIndex = RowIndex + 1;

               worksheet.Cells[$"A{RowIndex}"].SetValue(row["CNT"]);
               worksheet.Cells[$"B{RowIndex}"].SetValue(row["MG4_DATE"]);
               worksheet.Cells[$"C{RowIndex}"].SetValue(row["MG4_KIND_ID"]);
               worksheet.Cells[$"D{RowIndex}"].SetValue("A值");
               worksheet.Cells[$"E{RowIndex}"].SetValue(row["MG4_CM"]);
               worksheet.Cells[$"F{RowIndex}"].SetValue(row["MG4_MM"]);
               worksheet.Cells[$"G{RowIndex}"].SetValue(row["MG4_IM"]);
               worksheet.Cells[$"H{RowIndex}"].SetValue("B值");
               worksheet.Cells[$"I{RowIndex}"].SetValue(row["MG4_CM_B"]);
               worksheet.Cells[$"J{RowIndex}"].SetValue(row["MG4_MM_B"]);
               worksheet.Cells[$"K{RowIndex}"].SetValue(row["MG4_IM_B"]);
               if (row["CNT"].AsInt()!=0) {
                  worksheet.Cells[$"L{RowIndex}"].Formula = string.Format("=(E{0}-E{1})/E{1}", RowIndex, RowIndex - 1);
                  worksheet.Cells[$"M{RowIndex}"].Formula = string.Format("=(F{0}-F{1})/F{1}", RowIndex, RowIndex - 1);
                  worksheet.Cells[$"N{RowIndex}"].Formula = string.Format("=(G{0}-G{1})/G{1}", RowIndex, RowIndex - 1);
               }
               
            }//for (int k = 0; k < dt.Rows.Count; k++)

            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40052:" + ex.Message);
#else
            throw ex;
#endif
         }
         return true;
      }

      /// <summary>
      /// wf_40053()
      /// </summary>
      /// <returns></returns>
      public bool Wf40053()
      {
         try {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet =null;
            DataTable dt=null;
            for (int sheetCount = 0; sheetCount < 2; sheetCount++) {
               if (sheetCount==0) {
                  worksheet = workbook.Worksheets["Fut(ETF)"];
                  dt = _Data.Filter("mg4_prod_type = 'F' and ( mg4_prod_subtype = 'S' and  mg4_param_key ='ETF')");
               }
               else {
                  worksheet = workbook.Worksheets["Fut(STF)"];
                  dt = _Data.Filter("mg4_prod_type = 'F' and ( mg4_prod_subtype = 'S' and  mg4_param_key <>'ETF')");
               }
               Wf40053Fut(worksheet, dt);
            }
            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40053:" + ex.Message);
#else
            throw ex;
#endif
         }
         return true;
      }

      /// <summary>
      /// wf_40054()
      /// </summary>
      /// <returns></returns>
      public bool Wf40054()
      {
         try {
            //切換Sheet
            Workbook workbook = new Workbook();
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = null;
            DataTable dt = null;
            for (int sheetCount = 0; sheetCount < 2; sheetCount++) {
               if (sheetCount == 0) {
                  worksheet = workbook.Worksheets["Opt(ETC)"];
                  dt = _Data.Filter("mg4_prod_type = 'O' and ( mg4_prod_subtype = 'S' and  mg4_param_key ='ETC')");
               }
               else {
                  worksheet = workbook.Worksheets[5];
                  dt = _Data.Filter("mg4_prod_type = 'O' and ( mg4_prod_subtype = 'S' and  mg4_param_key <>'ETC')");
               }
               Wf40054Opt(worksheet, dt);
            }
            workbook.SaveDocument(_lsFile);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40054:" + ex.Message);
#else
            throw ex;
#endif
         }
         return true;
      }

   }
}
