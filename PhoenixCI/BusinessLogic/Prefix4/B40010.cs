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
      protected ID40010 dao;
      /// <summary>
      /// 程式代號(_ProgramID)
      /// </summary>
      protected string _TxnID;

      public B40010()
      {
      }

      public B40010(string daoID, string emDate)
      {
         this._TxnID = "40010";
         this._emDateText = emDate;
         this.dao = new D40010().ConcreteDao(daoID);
      }

      public I4001x ConcreteClass(string programID, object[] args = null)
      {

         //string className = string.Format("{0}.Dao.Together.SpecificDao.{1}",AssemblyName, name);//完整的class路徑

         string AssemblyName = GetType().Namespace.Split('.')[0];//最後compile出來的dll名稱
         string className = GetType().FullName.Replace("B4001xTemplate", "B" + programID);//完整的class路徑

         // 這裡就是Reflection，直接依照className實體化具體類別
         return (I4001x)Assembly.Load(AssemblyName).CreateInstance(className, true, BindingFlags.CreateInstance, null, args, null, null);
      }

      /// <summary>
      /// 新增EWMA計算按鈕，產出資料，並將excel計算資料回寫資料庫
      /// </summary>
      /// <returns></returns>
      public string ComputeEWMA(string FilePath,string TxdID,string ProdType)
      {
         Workbook workbook = new Workbook();
         try {
            //切換Sheet
            workbook.LoadDocument(FilePath);
            Worksheet worksheet = workbook.Worksheets[1];
            DateTime emdate = _emDateText.AsDateTime("yyyy/MM/dd");

            //1.RowData sheet第一行
            DataTable dtCPR = dao.List40010CPR(emdate, TxdID);
            //確認有無資料
            if (dtCPR.Rows.Count <= 0) {
               return $"{_emDateText},40010_1,讀取「風險價格係數」無任何資料!";
            }
            foreach (DataRow dr in dtCPR.Rows) {
               int rowIndex = dr["RPT_VALUE_2"].AsInt();
               if (rowIndex <= 0)
                  continue;
               worksheet.Rows[0][rowIndex - 1].SetValue(dr["CPR_PRICE_RISK_RATE"]);
            }

            //2.寫入40010template計算EWMA
            DataTable RowData = dao.ListRowDataSheet(emdate, TxdID, ProdType);
            if (RowData.Rows.Count <= 0) {
               return MessageDisplay.MSG_NO_DATA;
            }
            worksheet.Cells["M3"].SetValue(RowData.Rows[0]["PDK_XXX"]);
            worksheet.Cells["N3"].SetValue(RowData.Rows[0]["MGR4_CM"]);
            RowData.Columns.Remove(RowData.Columns["AI5_SETTLE_DATE"]);
            RowData.Columns.Remove(RowData.Columns["RPT_SEQ_NO"]);
            RowData.Columns.Remove(RowData.Columns["PDK_XXX"]);
            RowData.Columns.Remove(RowData.Columns["MGR4_CM"]);
            worksheet.Import(RowData, false, 2, 0);
            //3.讀取第一個sheet寫入DB
            Worksheet ws = workbook.Worksheets[0];
            //確認有無資料 沒有則新增一行
            string lsDate = emdate.ToString("yyyyMMdd");
            DataTable MG1_3M = dao.ListMG1_3M(lsDate, "F", "", "-");
            if (MG1_3M.Rows.Count <= 0) {
               DataRow dr = MG1_3M.NewRow();
               
               //模型(S:SMA/M:MAXV/E:EWMA)
               dr["MG1_MODEL_TYPE"] = "E";
               //日期
               dr["MG1_YMD"] = lsDate;
               //期貨or選擇權
               dr["MG1_PROD_TYPE"] = ProdType;
               //商品別
               dr["MG1_KIND_ID"] = ws.Cells["B4"].Value;
               //type
               dr["MG1_AB_TYPE"] = "-";
               //近月份期貨
               dr["MG1_PRICE"] = ws.Cells["C4"].Value.AsDecimal();
               //指數每點價值
               dr["MG1_XXX"] = ws.Cells["D4"].Value.AsDecimal();
               //適用風險價格係數C
               dr["MG1_RISK"] = ws.Cells["E4"].Value.AsDecimal();
               //實際風險價格係數
               dr["MG1_CP_RISK"] = ws.Cells["F4"].Value.AsDecimal();
               //最小風險價格係數
               dr["MG1_MIN_RISK"] = ws.Cells["G4"].Value.AsDecimal();
               //本日結算保證金D=A×B×C
               dr["MG1_CM"] = ws.Cells["H4"].Value.AsDecimal();
               //現行收取結算保證金
               dr["MG1_CUR_CM"] = ws.Cells["I4"].Value.AsDecimal();
               //變動幅度
               dr["MG1_CHANGE_RANGE"] = ws.Cells["J4"].Value.AsDecimal();

               //以下欄位不得為空
               //現行維持保證金
               dr["MG1_CUR_MM"] = 0;
               //現行原始保證金
               dr["MG1_CUR_IM"] = 0;
               //計算結算保證金
               dr["MG1_CP_CM"] = 0;
               //本日維持保證金
               dr["MG1_MM"] = 0;
               //本日原始保證金
               dr["MG1_IM"] = 0;
               //幣別
               dr["MG1_CURRENCY_TYPE"] = "1";
               //維持乘數
               dr["MG1_M_MULTI"] = 0;
               //原始乘數
               dr["MG1_I_MULTI"] = 0;
               //契約對照碼
               dr["MG1_PARAM_KEY"] = "ETC";
               //子類別
               dr["MG1_PROD_SUBTYPE"] = "I";
               //轉檔時間
               dr["MG1_W_TIME"] = DateTime.Now;
               //收盤群組
               dr["MG1_OSW_GRP"] = "0";

               MG1_3M.Rows.Add(dr);
            }
            else {
               DataRow dr = MG1_3M.Rows[0];
               //日期
               dr["MG1_YMD"] = lsDate;
               //期貨or選擇權
               dr["MG1_PROD_TYPE"] = ProdType;
               //商品別
               dr["MG1_KIND_ID"] = ws.Cells["B4"].Value;
               //type
               dr["MG1_AB_TYPE"] = "-";
               //近月份期貨
               dr["MG1_PRICE"] = ws.Cells["C4"].Value.AsDecimal();
               //指數每點價值
               dr["MG1_XXX"] = ws.Cells["D4"].Value.AsDecimal();
               //適用風險價格係數C
               dr["MG1_RISK"] = ws.Cells["E4"].Value.AsDecimal();
               //實際風險價格係數
               dr["MG1_CP_RISK"] = ws.Cells["F4"].Value.AsDecimal();
               //最小風險價格係數
               dr["MG1_MIN_RISK"] = ws.Cells["G4"].Value.AsDecimal();
               //本日結算保證金D=A×B×C
               dr["MG1_CM"] = ws.Cells["H4"].Value.AsDecimal();
               //現行收取結算保證金
               dr["MG1_CUR_CM"] = ws.Cells["I4"].Value.AsDecimal();
               //變動幅度
               dr["MG1_CHANGE_RANGE"] = ws.Cells["J4"].Value.AsDecimal();

               //轉檔時間
               dr["MG1_W_TIME"] = DateTime.Now;
            }
            //儲存至DB
            dao.UpdateMG1_3M(MG1_3M);

            worksheet.ScrollTo(0, 0);
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"ComputeEWMA:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            //save
            workbook.SaveDocument(FilePath);
         }
         return MessageDisplay.MSG_OK;
      }
   }

}
