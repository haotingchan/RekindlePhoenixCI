using BaseGround.Shared;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
/// <summary>
/// john,20190422,收盤前保證金試算表
/// </summary>
namespace PhoenixCI.BusinessLogic.Prefix4
{
   /// <summary>
   /// 收盤前保證金試算表
   /// </summary>
   public class B40042
   {

      private readonly D40042 dao40042;
      /// <summary>
      /// 輸出Excel檔案路徑
      /// </summary>
      private readonly string _lsFile;
      /// <summary>
      /// 輸入的日期 yyyy/MM/dd
      /// </summary>
      private readonly string _emDateText;

      private readonly string _gsUserID;

      private readonly B40011 b40011;

      private readonly B40012 b40012;

      private readonly B40013 b40013;

      public B40042()
      {
         dao40042 = new D40042();
      }

      public B40042(string FilePath, string emDate, string gsUserID)
      {
         this._lsFile = FilePath;
         this._emDateText = emDate;
         this._gsUserID = gsUserID;
         dao40042 = new D40042();
         b40011 = new B40011("40042_40011", FilePath, emDate);
         b40012 = new B40012("40042_40012", FilePath, emDate);
         b40013 = new B40013("40042_40013", FilePath, emDate);
      }

      public DataTable GetDataList()
      {
         DataTable dt = dao40042.List40042();
         //OSW_GRP有空白字串 會造成有某些值抓不到
         foreach (DataRow dr in dt.Rows) {
            dr["OSW_GRP"] = dr["OSW_GRP"].AsString();
         }
         dt.AcceptChanges();
         return dt;
      }

      public string Status(string DateTxt)
      {
         string status = "";
         if (dao40042.Tfxm1DateCount(DateTxt) > 0) {
            status = "(標的現貨收盤價已轉入)";
         }
         else {
            status = "";
         }
         return status;
      }

      public string Wf40011Fut()
      {
         return b40011.WfFutureSheet();
      }

      public string Wf40011Opt()
      {
         return b40011.WfOptionSheet();
      }

      public string Wf40012Fut()
      {
         return b40012.WfFutureSheet(2);
      }

      public string Wf40012Opt()
      {
         return b40012.WfOptionSheet(3);
      }

      public string Wf40013Fut()
      {
         return b40013.WfFutureSheet(4);
      }

      public string Wf40042()
      {
         //切換Sheet
         Workbook workbook = new Workbook();
         workbook.LoadDocument(_lsFile);
         Worksheet worksheet = workbook.Worksheets["40042轉出報表"];
         worksheet.Cells["K2"].Value = _emDateText;
         worksheet.Cells["M2"].Value = DateTime.Now.ToString("HH:mm");
         worksheet.Cells["K42"].Value = _emDateText;
         worksheet.Cells["M42"].Value = DateTime.Now.ToString("HH:mm");

         try {
            //ETF（股票類）
            DataTable dtETF = dao40042.List40042Mg1ETF(_emDateText);
            if (dtETF.Rows.Count <= 0) {
               return MessageDisplay.MSG_OK;
            }
            worksheet.Import(dtETF, false, 43, 0);//li_start_row = 43

            //其它（非股票類）
            DataTable dtOther = dao40042.List40042Mg1Other(_emDateText);
            if (dtOther.Rows.Count <= 0) {
               return MessageDisplay.MSG_OK;
            }
            worksheet.Import(dtOther, false, 3, 0);//ii_ole_row = 3
         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception($"Wf40042:" + ex.Message);
#else
            throw ex;
#endif
         }
         finally {
            //save
            worksheet.ScrollTo(0, 0);
            workbook.SaveDocument(_lsFile);
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// ue_export_before()
      /// </summary>
      /// <param name="dt"></param>
      /// <param name="GlobalSavePath"></param>
      /// <returns></returns>
      public string ExportBeforeCheck(DataTable dt, string GlobalSavePath = "")
      {
         try {
            //標的證券價格
            DataTable dtTfxm1u = dao40042.List40042Tfxm1u().Clone();
            foreach (DataRow row in dt.Rows) {
               if (row["F_CHOOSE"].AsString() == "N" && row["O_CHOOSE"].AsString() == "N") {
                  continue;
               }

               //檢核值=0
               if (row["F_CHOOSE"].AsString() == "N" && row["AI5_SETTLE_PRICE"].AsDecimal() == 0) {
                  return row["F_KIND_ID"].AsString() + " - 近月份期貨價格不可為0";
               }
               if (row["PDK_SUBTYPE"].AsString() == "S" && row["TFXM1_SFD_FPR"].AsDecimal() == 0) {
                  return row["PDK_STOCK_ID"].AsString() + " - 標的證券收盤價格不可為0";
               }

               //現貨價格
               if (row["PID"].AsString() != "F") {
                  dtTfxm1u.Rows.Add(dtTfxm1u.NewRow());
                  int rowsCount = dtTfxm1u.Rows.Count - 1;
                  for (int k = 0; k < 10; k++) {
                     dtTfxm1u.Rows[rowsCount][k] = row[k];
                  }
                  if (row["pdk_subtype"].AsString() != "S") {
                     dtTfxm1u.Rows[rowsCount]["TFXM1_SID"] = row["O_KIND_ID"].AsString();
                  }
               }

               //期貨價格
               if (row["F_CHOOSE"].AsString() == "Y") {
                  dtTfxm1u.Rows.Add(dtTfxm1u.NewRow());
                  int rowsCount = dtTfxm1u.Rows.Count - 1;
                  dtTfxm1u.Rows[rowsCount]["TFXM1_DATE"] = row["DT_DATE"];
                  dtTfxm1u.Rows[rowsCount]["TFXM1_SFD_FPR"] = row["AI5_SETTLE_PRICE"];
                  dtTfxm1u.Rows[rowsCount]["TFXM1_SPF_IPR"] = row["TFXMSPF_IPR"];
                  dtTfxm1u.Rows[rowsCount]["TFXM1_SID"] = row["F_KIND_ID"].AsString();
                  dtTfxm1u.Rows[rowsCount]["TFXM1_PID"] = "F";
                  dtTfxm1u.Rows[rowsCount]["TFXM1_PARAM_KEY"] = row["PARAM_KEY"];
               }

            }//foreach (DataRow row in dt.Rows)

            //異動Table
            dao40042.DeleteTFXM1U();//delete ci.TFXM1U;

            if (!string.IsNullOrEmpty(GlobalSavePath)) {
               string txtFpath = Path.Combine(GlobalSavePath, "TFXM1U.TXT");
               Workbook workbook = new Workbook();
               workbook.Worksheets[0].Import(dtTfxm1u, true, 0, 0);
               workbook.SaveDocument(txtFpath, DocumentFormat.Text);
            }

            try {
               dao40042.UpdateTFXM1U(dtTfxm1u);
            }
            catch {
               throw new Exception("標的證券價格錯誤(ci.TFXM1U)");
            }

            //執行SP
            ExecuteSP();

         }
         catch (Exception ex) {
#if DEBUG
            throw new Exception("ExportBeforeCheck:" + ex.Message);
#else
            throw ex;
#endif
         }
         return MessageDisplay.MSG_OK;
      }

      /// <summary>
      /// 執行SP
      /// </summary>
      /// <returns></returns>
      private string ExecuteSP()
      {
         dao40042.DeleteMG1U();
         dao40042.DeleteMG6U();
         dao40042.DeleteMGR1U();
         dao40042.DeleteMGR2U();

         /*******************
         轉統計資料TDT
         fut.sp_F_gen_H_TFXM1U_F
         opt.sp_O_gen_H_MGR1U
         opt.sp_O_gen_H_MGR2U_I
         opt.sp_O_gen_H_MGR2U_STF
         fut.sp_F_gen_H_MG1U_S
         fut.sp_F_gen_H_MG1U_I
         *******************/
         const string TxnID = "40042";
         string logtext = "執行SP";

         try {
            //My_SP1
            logtext = "執行fut.sp_H_gen_H_TFXM1U";
            dao40042.SP_F_GEN_H_TFXM1U_F();
            WriteLogf(TxnID, "E", logtext);
            //My_SP2
            logtext = "執行opt.sp_O_gen_H_MGR1U";
            dao40042.SP_O_GEN_H_MGR1U();
            WriteLogf(TxnID, "E", logtext);
            //My_SP3
            logtext = "執行opt.sp_O_gen_H_MGR2U_I";
            dao40042.SP_O_GEN_H_MGR2U_I();
            WriteLogf(TxnID, "E", logtext);
            //My_SP4
            logtext = "執行opt.sp_O_gen_H_MGR2U_STF";
            dao40042.SP_O_GEN_H_MGR2U_STF();
            WriteLogf(TxnID, "E", logtext);
            //My_SP5
            logtext = "執行fut.sp_F_gen_H_MG1U_S";
            dao40042.SP_F_GEN_H_MG1U_S();
            WriteLogf(TxnID, "E", logtext);
            //My_SP6
            logtext = "執行fut.sp_F_gen_H_MG1U_I";
            dao40042.SP_F_GEN_H_MG1U_I();
            WriteLogf(TxnID, "E", logtext);
            //My_SP7
            logtext = "執行opt.sp_O_gen_H_MG1U_S";
            dao40042.SP_O_GEN_H_MG1U_S();
            WriteLogf(TxnID, "E", logtext);
            //My_SP8
            logtext = "執行opt.sp_O_gen_H_MG1U_I";
            dao40042.SP_O_GEN_H_MG1U_I();
            WriteLogf(TxnID, "E", logtext);
         }
         catch (Exception ex) {
            throw new Exception($"{logtext}:" + ex.Message);
         }
         return "";
      }

      /// <summary>
      /// 此func 已移至 Form Parent WriteLog()
      /// </summary>
      /// <param name="gs_txn_id"></param>
      /// <param name="as_type"></param>
      /// <param name="as_text"></param>
      /// <returns>正常回傳0,失敗回傳-1</returns>
      private int WriteLogf(string gs_txn_id, string as_type, string as_text)
      {
         try {
            as_text = as_text.SubStr(0, 100);//取前100字元

            LOGF logf = new LOGF();
            logf.Insert(_gsUserID, gs_txn_id, as_text, as_type);
            return 0;
         }
         catch (Exception ex) {
            //寫db log失敗,只好寫入本地端的file
            //這段再找時間補
            return -1;
         }
      }

   }
}
