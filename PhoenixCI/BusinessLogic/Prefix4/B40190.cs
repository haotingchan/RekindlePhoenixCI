using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
/// <summary>
/// 20190408,john,每月保證金狀況表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 每月保證金狀況表
   /// </summary>
   public class B40190
   {
      /// <summary>
      /// 檔案輸出路徑
      /// </summary>
      private readonly string _lsFile;
      /// <summary>
      /// 交易日期
      /// </summary>
      private readonly string _emDateText;
      /// <summary>
      /// DataLayer
      /// </summary>
      private readonly D40190 dao40190;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="FilePath">Excel_Template</param>
      public B40190(string FilePath, string emDate)
      {
         _lsFile = FilePath;
         _emDateText = emDate;
         dao40190 = new D40190();
      }

      /// <summary>
      /// 備註2 期貨和選擇權的備註內容
      /// </summary>
      /// <param name="worksheet">目前寫入的sheet</param>
      /// <param name="dt">stf過濾後的data</param>
      /// <param name="queryType">F/O</param>
      /// <returns></returns>
      private static string TypeLevel(Worksheet worksheet, DataTable dt, string queryType)
      {
         Dictionary<string, string> typeDic = new Dictionary<string, string>()
        {
            {"O", "選擇權"}, {"F", "期貨"}
        };
         string[] mgrLV = new string[] { "2", "3", "Z" };
         StringBuilder sb = null;
         foreach (string level in mgrLV) {
            DataTable TypeLevel = dt.Filter($"mgr4_prod_type = '{queryType}' and mgrt1_level = '{level}'");
            if (TypeLevel.Rows.Count <= 0) {
               continue;
            }

            if (sb == null) {
               sb = new StringBuilder("備註2：目前除");
            }
            else {
               sb.Append("、");
            }

            sb = AppendContent(sb, TypeLevel);
         }//foreach (string level in mgrLV)

         //結尾
         sb.Append($"外，所有其餘股票{typeDic[queryType]}保證金皆適用級距1。");
         return sb.ToString();
      }
      /// <summary>
      /// 寫入備註內容
      /// </summary>
      /// <param name="sb"></param>
      /// <param name="TypeLevel">level為 2/3/Z 的條件</param>
      /// <returns>StringBuilder</returns>
      private static StringBuilder AppendContent(StringBuilder sb, DataTable TypeLevel)
      {
         string kindId = "";
         if (TypeLevel.Rows.Count > 0) {

            DataRow newRow = TypeLevel.NewRow();
            newRow["MGR4_KIND_ID"] = "  ";
            TypeLevel.Rows.Add(newRow);//新增空row

            for (int k = 0; k < TypeLevel.Rows.Count - 1; k++) {
               DataRow row = TypeLevel.Rows[k];
               //head
               if (kindId != row["MGR4_KIND_ID"].AsString().SubStr(0, 2)) {
                  kindId = row["MGR4_KIND_ID"].AsString().SubStr(0, 2);
                  if (k != 0) {
                     sb.Append("、");
                  }
                  sb.Append($"{row["APDK_NAME"].AsString()}(");
               }
               else {
                  sb.Append(",");
               }
               //body
               sb.Append(row["MGR4_KIND_ID"].AsString());
               //foot
               if (row["MGR4_KIND_ID"].AsString().SubStr(0, 2) != TypeLevel.Rows[k + 1]["MGR4_KIND_ID"].AsString().SubStr(0, 2)) {

                  if (TypeLevel.Rows[0]["MGRT1_LEVEL"].AsString() != "Z") {
                     sb.Append(")");
                  }
                  else {
                     sb.Append($")結算保證金適用比例{row["MGR4_CM"].AsPercent(0)}");
                  }

               }
            }//for (int k = 0; k < TypeFLevel.Rows.Count-1; k++)
            TypeLevel.AsEnumerable().LastOrDefault().Delete();//刪除空row
            string mgr1Level = TypeLevel.Rows[0]["MGRT1_LEVEL"].AsString();
            if (mgr1Level != "Z") {
               sb.Append($"保證金適用級距為級距{mgr1Level}");
            }
         }//if (TypeFLevel.Rows.Count > 0)
         return sb;
      }

      /// <summary>
      /// wf_40191() 保證金適用比例級距
      /// </summary>
      /// <returns></returns>
      public string Wf40191()
      {
         Workbook workbook = new Workbook();
         try {
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[0];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");
            worksheet.Rows[0][0].Value = emdate;

            //讀取資料(保證金適用比例級距)
            DataTable dtMgrt1F = dao40190.Get42010Mgrt1FData();
            if (dtMgrt1F.Rows.Count <= 0) {
               return $"{_emDateText},40190_1－期貨保證金,讀取「保證金適用比例級距」無任何資料!";
            }
            worksheet.Import(dtMgrt1F, false, 106, 1);

            DataTable dtMgrt1O = dao40190.Get42010Mgrt1OData();
            if (dtMgrt1O.Rows.Count <= 0) {
               return $"{_emDateText},40190_1－期貨保證金,讀取「保證金適用比例級距」無任何資料!";
            }
            worksheet.Import(dtMgrt1O, false, 208, 1);

            //讀取資料 (期貨)
            DataTable dtFUT = dao40190.Get49191FUT(emdate);
            if (dtFUT.Rows.Count <= 0) {
               return $"{_emDateText},40190_1－期貨保證金,無任何資料!";
            }

            worksheet.Import(dtFUT.AsEnumerable().Take(1).CopyToDataTable(), false, 5, 2);
            ////跳過第7行
            worksheet.Import(dtFUT.AsEnumerable().Skip(1).CopyToDataTable(), false, 7, 2);

            //ETF類股票期貨
            DataTable dtETF = dao40190.Get49191ETF(emdate);
            worksheet.Import(dtETF, false, 52, 1);
            //刪除空白列
            int ETFcount = dtETF.Rows.Count;
            worksheet.Rows.Hide(52 + ETFcount, 102 - 1);

            DataTable stfData = dao40190.GetStfData(emdate);
            //註2
            //備註2：目前除銘異期貨(IVF)、三陽期貨(FPF)保證金適用級距為級距2、XX期貨(XXX)結算保證金適用比例XX%外，所有其餘股票期貨保證金皆適用級距1。
            DataTable TypeFdata = stfData.Filter("mgr4_prod_type = 'F' and mgrt1_level <> '1'");
            worksheet.Cells["B112"].Value = TypeFdata.Rows.Count <= 0 ? "備註2：目前所有股票期貨保證金皆適用級距1。" : TypeLevel(worksheet, stfData, "F");
            //註2
            //備註2：目前除XX選擇權(XXX)、XX選擇權(XXX)保證金適用級距為級距2、XX選擇權(XXX)結算保證金適用比例XX%外，所有其餘目前所有股票選擇權保證金皆適用級距1。						
            DataTable TypeOdata = stfData.Filter(" mgr4_prod_type = 'O'  and mgrt1_level = 'Z'");
            worksheet.Cells["B216"].Value = TypeOdata.Rows.Count <= 0 ? "備註2：目前所有股票選擇權保證金皆適用級距1。" : TypeLevel(worksheet, stfData, "O");

         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40191:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            //save
            workbook.SaveDocument(_lsFile);
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_40192() 選擇權保證金
      /// </summary>
      /// <returns></returns>
      public string Wf40192()
      {
         Workbook workbook = new Workbook();
         try {
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[0];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");

            //讀取資料
            DataTable dtFUT = dao40190.Get49192FUT(emdate);
            if (dtFUT.Rows.Count <= 0) {
               return $"{_emDateText},40192－選擇權保證金,無任何資料!";
            }
            worksheet.Import(dtFUT, false, 114, 2);

            //ETF類股票選擇權
            DataTable dtETF = dao40190.Get49192ETF(emdate);
            worksheet.Import(dtETF, false, 162, 1);

            //刪除空白列
            int ETFcount = dtETF.Rows.Count;
            if (40 > ETFcount) {
               worksheet.Rows.Hide(162 + ETFcount, 202 - 1);
            }
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40192:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// wf_40193() 公告日期
      /// </summary>
      /// <returns></returns>
      public string Wf40193()
      {
         Workbook workbook = new Workbook();
         try {
            workbook.LoadDocument(_lsFile);
            Worksheet worksheet = workbook.Worksheets[0];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");

            //讀取資料
            DataTable dt = dao40190.Get40193Data(new DateTime(emdate.Year, emdate.Month, 01), emdate);
            if (dt.Rows.Count <= 0) {
               return $"{new DateTime(emdate.Year, emdate.Month, 01)},40193－調整狀況,無任何資料!";
            }
            worksheet.Import(dt, false, dao40190.Get40193SeqNo - 1, 1);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40193:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            workbook.SaveDocument(_lsFile);
         }
         return MessageDisplay.MSG_OK;
      }


   }
}
