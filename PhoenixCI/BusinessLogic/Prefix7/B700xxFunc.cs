using BaseGround.Shared;
using Common;
using Common.Helper;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixCI.BusinessLogic.Prefix7
{
   public class B700xxFunc
   {
      /// <summary>
      /// 作業:轉70010 週檔 (公司網站\統計資料\週)
      /// |
      /// 呼叫來源: 70010 (由業務單位手動產生) 10012,10022 (由OP操作批次時自動產生)
      /// </summary>
      /// <param name="is_save_file">檔名</param>
      /// <param name="is_symd">起始日期</param>
      /// <param name="is_eymd">終止日期</param>
      /// <param name="is_sum_type">統計別:D日,M月,Y年</param>
      /// <param name="is_prod_type">商品別:F期貨,O選擇權</param>
      /// <param name="is_market_code"></param>
      /// <param name="selectEng">選取英文版本</param>
      /// <returns>RETURN:false代表error/true代表成功</returns>
      public bool f_70010_week_by_market_code
         (string is_save_file, string is_symd, string is_eymd, string is_sum_type, string is_prod_type, string is_market_code, bool selectEng = false)
      {
         /********************************
         作業:轉70010 週檔 (公司網站\統計資料\週)
         呼叫來源: 70010 (由業務單位手動產生)
                   10012,10022 (由OP操作批次時自動產生)
         參數:(1)檔名
              (2)起始日期
	           (3)終止日期
	           (4)統計別:D日,M月,Y年
	           (5)商品別:F期貨,O選擇權
         RETURN:E代表error
                Y代表成功
         ********************************/
         string openData = "";

         //避免重複寫入
         if (File.Exists(is_save_file)) {
            File.Delete(is_save_file);
         }
         File.Create(is_save_file).Close();
         if (!selectEng) {
            openData = PbFunc.f_chg_filename(is_save_file, "_OpenData");
            if (!File.Exists(openData)) {
               File.Delete(openData);
            }
            File.Create(openData).Close();
            writeFile(openData, "期貨商代號,期貨商名稱,日期,商品,交易量");
         }

         D70010 dao70010 = new D70010();
         try {
            ///******************
            //讀取資料
            //******************/
            DataTable ids_1 = dao70010.ListRowdata(is_symd, is_eymd, is_sum_type, is_prod_type, is_market_code);
            if (ids_1.Rows.Count <= 0) {
               throw new System.Exception($@"轉70010-交易量資料轉檔作業(週)({is_symd }-{ is_eymd })(期貨/選擇權:{is_prod_type })筆數為０!");
            }
            /* 期貨商 */
            DataTable lds_brk = dao70010.List70010brkByMarketCode(is_symd, is_eymd, is_sum_type, is_prod_type, is_market_code);
            /* 日期 */
            DataTable lds_ymd_d = dao70010.ListYmdByMarketCode(is_symd, is_eymd, is_sum_type, is_prod_type, is_market_code);
            DataTable lds_ymd = dao70010.ListYmdEnd(is_symd, is_eymd, is_sum_type, is_prod_type);
            lds_ymd.Clear();//PB還沒有在這retrieve
            /* 商品 */
            DataTable lds_pk;
            if (is_prod_type == "F") {
               if (is_market_code == "1") {

                  lds_pk = dao70010.ListParamKey("70011n");
               }
               else {
                  lds_pk = dao70010.ListParamKey("70011");
               }
            }//if (is_prod_type == "F")
            else {
               if (is_market_code == "1") {
                  lds_pk = dao70010.ListParamKey("70012n");
               }
               else {
                  lds_pk = dao70010.ListParamKey("70012");
               }
            }//if (is_prod_type == "F")else
            DataTable newDs_pk = lds_pk.Filter($@"rpt_value_3 Is Null or TRIM(rpt_value_3) = '' or rpt_value_3 >= '{ is_symd }'");
            string ls_brk_no, ls_brk_name, ls_brk_no4, ls_brk_type, ls_ymd, ls_param_key, ls_str;
            DateTime ld_ymd, ld_ymd_n;
            //日期:週
            try {
               DataRow toInsert = lds_ymd_d.NewRow();
               toInsert["am0_ymd"] = "20051231";
               lds_ymd_d.Rows.InsertAt(toInsert, 0);
               int dtymdCount = lds_ymd_d.Rows.Count - 1;
               for (int k = 1; k <= dtymdCount; k++) {
                  ls_ymd = lds_ymd_d.Rows[k]["am0_ymd"].AsString();
                  ld_ymd = DateTime.ParseExact(ls_ymd, "yyyyMMdd", CultureInfo.CurrentCulture);
                  ld_ymd_n = DateTime.ParseExact(lds_ymd_d.Rows[k - 1]["am0_ymd"].AsString(), "yyyyMMdd", CultureInfo.CurrentCulture);
                  /* 符合下列條件才寫Excel
                  1.最後一筆
                  2.換週 (判斷星期x是否變小)
                  3.與下一日期相差7天以上 (for日期99999999) 
                  */
                  if (k == 1 || ld_ymd.DayOfWeek < ld_ymd_n.DayOfWeek || Math.Abs(PbFunc.DaysAfter(ld_ymd, ld_ymd_n)) > 6) {
                     toInsert = lds_ymd.NewRow();
                     toInsert["am0_ymd"] = ls_ymd;
                     lds_ymd.Rows.Add(toInsert);
                  }
                  lds_ymd.Rows[lds_ymd.Rows.Count - 1]["ymd_end"] = ls_ymd;
               }//for (int k = 2; k <= dtymdCount; k++)
               toInsert = lds_ymd.NewRow();
               toInsert["am0_ymd"] = "99999999";
               toInsert["ymd_end"] = "99999999";
               lds_ymd.Rows.Add(toInsert);
               DataView dsDv = lds_ymd.AsDataView();
               dsDv.Sort = "am0_ymd";
               lds_ymd = dsDv.ToTable();
            }
            catch (Exception ex) {
               MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-日期:週");
               return false;
            }
            /******************
            表頭
            ******************/
            ls_str = "";
            string ls_output1, ls_output2, ls_OpenData_str;
            string writeOpenData = "";
            decimal ld_sum = 0;
            try {
               ls_output1 = !selectEng ? "期貨商代號" + "," + "名稱" : " Sequential No.";
               ls_output2 = !selectEng ? "Date" + "," : "Date";
               foreach (DataRow ymdRow in lds_ymd.Rows) {
                  ls_ymd = ymdRow["am0_ymd"].AsString();
                  ls_output2 = ls_output2 + "," + ls_ymd + " - " + ymdRow["ymd_end"].AsString() + ",".PadRight(newDs_pk.Rows.Count, ',');
                  if (ls_ymd == "99999999") {
                     ls_str = !selectEng ? "總計" : "Year-To-Date Volume of";
                  }
                  foreach (DataRow pkRow in newDs_pk.Rows) {
                     ls_param_key = pkRow["am0_param_key"].AsString();
                     /*******************
                     換商品代號
                     *******************/
                     ls_param_key = pkRow["rpt_value_2"].AsString();
                     ls_output1 = ls_output1 + "," + ls_str + ls_param_key;
                  }//foreach(DataRow pkRow in newDs_pk.Rows)
                  if (ls_ymd == "99999999") {
                     ls_output1 = ls_output1 + "," + (!selectEng ? ls_str : "Year-To-Date Market Volume");
                  }
                  else {
                     ls_output1 = ls_output1 + "," + (!selectEng ? "小計" : "Subtotal");
                  }
               }//foreach (DataRow ymdRow in lds_ymd.Rows)
               ls_output1 = ls_output1 + "," + (!selectEng ? "市佔率" : "YTD Market shares(%)");
               writeFile(is_save_file, ls_output1);
               writeFile(is_save_file, ls_output2);
            }
            catch (Exception ex) {
               MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-表頭");
               return false;
            }
            /******************
            內容
            ******************/
            DataRow newRow = ids_1.NewRow();
            newRow["am0_ymd"] = "20060101";
            ids_1.Rows.Add(newRow);
            /*******************
            OpenData
            *******************/
            //期貨商代號&名稱
            try {
               foreach (DataRow brkRow in lds_brk.Rows) {
                  ls_brk_no4 = brkRow["am0_brk_no4"].AsString();
                  ls_brk_type = brkRow["am0_brk_type"].AsString();
                  if (ls_brk_type.Trim() == "9") {
                     ls_brk_no = ls_brk_no4.Trim() + "999";
                  }
                  else {
                     ls_brk_no = ls_brk_no4.Trim() + "000";
                  }
                  ls_brk_name = new ABRK().GetNameByNo(ls_brk_no);// f_get_abrk_name(ls_brk_no,'0')	
                  ls_output1 = !selectEng ? (ls_brk_no + "," + ls_brk_name) : ls_brk_no;//轉英文只秀編號
                  //日期
                  foreach (DataRow ymdRow in lds_ymd.Rows) {
                     ls_ymd = ymdRow["am0_ymd"].AsString();
                     DataTable newDs_1 = ids_1.Filter($@"am0_brk_no4='{ ls_brk_no4 }' and am0_brk_type='{ ls_brk_type }' and am0_ymd>='{ ymdRow["am0_ymd"].AsString() }' and am0_ymd<='{ ymdRow["ymd_end"].AsString() }'");
                     for (int k = 0; k <= newDs_1.Rows.Count - 1; k++) {
                        int ll_found = newDs_pk.Rows.IndexOf(newDs_pk.Select($@"am0_param_key ='{ newDs_1.Rows[k]["am0_param_key"] }'").FirstOrDefault());
                        if (ll_found > -1) {
                           decimal ll_qnty = newDs_pk.Rows[ll_found]["qnty"].AsDecimal() + newDs_1.Rows[k]["qnty"].AsDecimal();
                           newDs_pk.Rows[ll_found]["qnty"] = ll_qnty;
                        }
                     }
                     ls_OpenData_str = ls_brk_no + "," + ls_brk_name + "," + ymdRow["am0_ymd"].AsString() + " - " + ymdRow["ymd_end"].AsString();
                     if (newDs_pk.Rows.Count > 1)
                        ld_sum = newDs_pk.Compute("sum(qnty)", "").AsDecimal();//ld_sum = newDs_pk.getitemdecimal(1,"cp_sum_qnty")

                     //商品
                     foreach (DataRow pkRow in newDs_pk.Rows) {
                        ls_output1 = ls_output1 + "," + pkRow["qnty"];
                        if (ls_ymd.SubStr(0, 8) != "99999999" && !selectEng) {
                           writeOpenData = ls_OpenData_str + "," + pkRow["am0_param_key"].AsString() + "," + pkRow["qnty"].AsString();
                           writeFile(openData, writeOpenData);
                        }
                        pkRow["qnty"] = 0;
                     }//foreach(DataRow pkRow in newDs_pk.Rows)
                     ls_output1 = ls_output1 + "," + ld_sum.AsString();
                  }//foreach (DataRow ymdRow in lds_ymd.Rows)
                  ls_output1 = ls_output1 + $",{brkRow["cp_rate"].AsDecimal().ToString("n")}";
                  writeFile(is_save_file, ls_output1);
               }
            }
            catch (Exception ex) {
               MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-內容");
               return false;
            }
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err);
            return false;
         }
         return true;
      }



      /// <summary>
      /// 重複寫入文字並換行
      /// </summary>
      /// <param name="openData">檔案路徑</param>
      /// <param name="textToAdd">文字內容</param>
      private void writeFile(string openData, string textToAdd)
      {
         using (FileStream fs = new FileStream(openData, FileMode.Append)) {
            using (StreamWriter writer = new StreamWriter(fs, Encoding.GetEncoding(950))) {
               writer.WriteLine(textToAdd);
            }
         }
      }
      /// <summary>
      /// 作業:轉70010 週檔 (公司網站\統計資料\週)英文版
      /// |
      /// 呼叫來源: 70010 (由業務單位手動產生) 10012,10022 (由OP操作批次時自動產生)
      /// </summary>
      /// <param name="is_save_file">檔名</param>
      /// <param name="is_symd">起始日期</param>
      /// <param name="is_eymd">終止日期</param>
      /// <param name="is_sum_type">統計別:D日,M月,Y年</param>
      /// <param name="is_prod_type">商品別:F期貨,O選擇權</param>
      /// <param name="is_market_code"></param>
      /// <param name="selectEng">選取英文版本</param>
      /// <returns>RETURN:false代表error/true代表成功</returns>
      public bool f_70010_week_eng_by_market_code(string is_save_file, string is_symd, string is_eymd, string is_sum_type, string is_prod_type, string is_market_code)
      {
         /********************************
         作業:轉70010 週檔 (公司網站\統計資料\週)
         呼叫來源: 70010 (由業務單位手動產生)
                   10012,10022 (由OP操作批次時自動產生)
         參數:(1)檔名
              (2)起始日期
	           (3)終止日期
	           (4)統計別:D日,M月,Y年
	           (5)商品別:F期貨,O選擇權
         RETURN:E代表error
                Y代表成功
         ********************************/
         if (f_70010_week_by_market_code(is_save_file, is_symd, is_eymd, is_sum_type, is_prod_type, is_market_code, true))
            return true;
         else
            return false;
      }
      /// <summary>
      /// 作業:轉70010 週檔 (公司網站\統計資料\週)
      /// |
      /// 呼叫來源: 11010
      /// </summary>
      /// <param name="is_save_file"></param>
      /// <param name="is_symd"></param>
      /// <param name="is_eymd"></param>
      /// <param name="is_sum_type"></param>
      /// <param name="is_prod_type"></param>
      /// <returns></returns>
      public bool f_70010_week_his(string is_save_file, string is_symd, string is_eymd, string is_sum_type, string is_prod_type)
      {
         return true;
      }
      /// <summary>
      /// 作業:轉70010 週檔 (公司網站\統計資料\週)
      /// |
      /// 呼叫來源:70010 (由業務單位手動產生)10012,10022 (由OP操作批次時自動產生)
      /// </summary>
      /// <param name="is_save_file">檔名</param>
      /// <param name="is_symd">起始日期</param>
      /// <param name="is_eymd">終止日期</param>
      /// <param name="is_sum_type">統計別:D日,M月,Y年</param>
      /// <param name="is_kind_id2">商品別:F期貨,O選擇權</param>
      /// <param name="is_param_key">商品</param>
      /// <param name="is_prod_type"></param>
      /// <returns></returns>
      public bool f_70010_week_w(string is_save_file, string is_symd, string is_eymd, string is_sum_type, string is_kind_id2, string is_param_key, string is_prod_type)
      {
         /********************************
         作業:轉70010 週檔 (公司網站\統計資料\週)
         呼叫來源: 70010 (由業務單位手動產生)
                   10012,10022 (由OP操作批次時自動產生)
         參數:(1)檔名
              (2)起始日期
	           (3)終止日期
	           (4)統計別:D日,M月,Y年
	           (5)商品別:F期貨,O選擇權
         RETURN:E代表error
                Y代表成功
         ********************************/
         //避免重複寫入
         if (File.Exists(is_save_file)) {
            File.Delete(is_save_file);
         }
         File.Create(is_save_file).Close();

         D70050 dao70050 = new D70050();
         D70010 dao70010 = new D70010();
         try {
            ///******************
            //讀取資料
            //******************/
            DataTable ids_1 = dao70050.ListAll(is_symd, is_eymd, is_sum_type, is_prod_type, is_kind_id2, is_param_key);
            if (ids_1.Rows.Count <= 0) {
               throw new System.Exception($@"轉{is_param_key}-交易量資料轉檔作業(週)({is_symd}-{is_eymd})(期貨/選擇權:{ is_prod_type })筆數為０!");
            }
            /* 期貨商 */
            DataTable lds_brk;
            lds_brk = dao70050.List70050brk(is_symd, is_eymd, is_sum_type, is_prod_type, is_kind_id2, is_param_key);

            /* 日期 */
            DataTable lds_ymd_d = dao70010.ListYMD(is_symd, is_eymd, is_sum_type, is_prod_type);
            DataTable lds_ymd = dao70010.ListYmdEnd(is_symd, is_eymd, is_sum_type, is_prod_type);
            lds_ymd.Clear();//PB還沒有在這retrieve
            /* 商品 */
            DataTable lds_pk;
            if (is_param_key == "TXO") {
               lds_pk = dao70010.ListParamKey("70040");
            }//if (is_param_key == "TXO")
            else {
               lds_pk = dao70010.ListParamKey("70050");
            }
            DataTable newDs_pk = lds_pk.Filter($@"am0_param_key like '{ is_kind_id2 }'");
            string ls_brk_no, ls_brk_no4, ls_brk_type, ls_ymd, ls_str;
            DateTime ld_ymd, ld_ymd_n;
            //日期:週
            try {
               DataRow toInsert = lds_ymd_d.NewRow();
               toInsert["am0_ymd"] = "20051231";
               lds_ymd_d.Rows.InsertAt(toInsert, 0);
               int dtymdCount = lds_ymd_d.Rows.Count - 1;
               for (int k = 1; k <= dtymdCount; k++) {
                  ls_ymd = lds_ymd_d.Rows[k]["am0_ymd"].AsString();
                  ld_ymd = DateTime.ParseExact(ls_ymd, "yyyyMMdd", CultureInfo.CurrentCulture);
                  ld_ymd_n = DateTime.ParseExact(lds_ymd_d.Rows[k - 1]["am0_ymd"].AsString(), "yyyyMMdd", CultureInfo.CurrentCulture);
                  /* 符合下列條件才寫Excel
                  1.最後一筆
                  2.換週 (判斷星期x是否變小)
                  3.與下一日期相差7天以上 (for日期99999999) 
                  */
                  if (k == 1 || ld_ymd.DayOfWeek < ld_ymd_n.DayOfWeek || Math.Abs(PbFunc.DaysAfter(ld_ymd, ld_ymd_n)) > 6) {
                     toInsert = lds_ymd.NewRow();
                     toInsert["am0_ymd"] = ls_ymd;
                     lds_ymd.Rows.Add(toInsert);
                  }
                  lds_ymd.Rows[lds_ymd.Rows.Count - 1]["ymd_end"] = ls_ymd;
               }//for (int k = 2; k <= dtymdCount; k++)
               toInsert = lds_ymd.NewRow();
               toInsert["am0_ymd"] = "99999999";
               toInsert["ymd_end"] = "99999999";
               lds_ymd.Rows.Add(toInsert);
               DataView dsDv = lds_ymd.AsDataView();
               dsDv.Sort = "am0_ymd";
               lds_ymd = dsDv.ToTable();
            }
            catch (Exception ex) {
               MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-日期:週");
               return false;
            }
            /******************
            表頭
            ******************/
            ls_str = "";
            string ls_output1, ls_output2;
            decimal ld_sum = 0;
            DataTable newDs_ymd = lds_ymd;
            try {
               ls_output1 = "期貨商代號" + "," + "名稱";
               ls_output2 = "Date" + ",";
               foreach (DataRow ymdRow in lds_ymd.Rows) {
                  ls_ymd = ymdRow["am0_ymd"].AsString();
                  ls_output2 = ls_output2 + "," + ls_ymd + " - " + ymdRow["ymd_end"].AsString();
                  if (is_kind_id2 == "%") {
                     ls_output2 = ls_output2 + ",".PadRight(newDs_pk.Rows.Count, ',');
                  }

                  if (ls_ymd == "99999999") {
                     ls_str = "總計";
                  }
                  foreach (DataRow pkRow in newDs_pk.Rows) {
                     is_param_key = pkRow["am0_param_key"].AsString();
                     /*******************
                     換商品代號
                     *******************/
                     is_param_key = pkRow["rpt_value_2"].AsString();
                     ls_output1 = ls_output1 + "," + ls_str + is_param_key;
                  }//foreach(DataRow pkRow in newDs_pk.Rows)
                  if (is_kind_id2 == "%") {
                     if (ls_ymd == "99999999") {
                        ls_output1 = ls_output1 + "," + ls_str;
                     }
                     else {
                        ls_output1 = ls_output1 + "," + "小計";
                     }
                  }
               }//foreach (DataRow ymdRow in lds_ymd.Rows)
               ls_output1 = ls_output1 + "," + "市佔率";
               writeFile(is_save_file, ls_output1);
               writeFile(is_save_file, ls_output2);
            }
            catch (Exception ex) {
               MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-表頭");
               return false;
            }
            /******************
                  內容
            ******************/
            DataRow newRow = ids_1.NewRow();
            newRow["am0_ymd"] = "20060101";
            ids_1.Rows.Add(newRow);
            //期貨商代號&名稱
            try {
               foreach (DataRow brkRow in lds_brk.Rows) {
                  ls_brk_no4 = brkRow["am0_brk_no4"].AsString();
                  ls_brk_type = brkRow["am0_brk_type"].AsString();
                  if (ls_brk_type.Trim() == "9") {
                     ls_brk_no = ls_brk_no4.Trim() + "999";
                  }
                  else {
                     ls_brk_no = ls_brk_no4.Trim() + "000";
                  }
                  string ls_brk_name = new ABRK().GetNameByNo(ls_brk_no);// f_get_abrk_name(ls_brk_no,'0')	
                  ls_output1 = ls_brk_no + "," + ls_brk_name;

                  //日期
                  foreach (DataRow ymdRow in lds_ymd.Rows) {
                     //將週日期區間數量相加
                     DataTable newDs_1 = ids_1.Filter($@"am0_brk_no4='{ ls_brk_no4 }' and am0_brk_type='{ ls_brk_type }' and am0_ymd>='{ ymdRow["am0_ymd"].AsString() }' and am0_ymd<='{ ymdRow["ymd_end"].AsString() }'");
                     for (int k = 0; k <= newDs_1.Rows.Count - 1; k++) {
                        int ll_found = newDs_pk.Rows.IndexOf(newDs_pk.Select($@"am0_param_key ='{ newDs_1.Rows[k]["am0_param_key"] }'").FirstOrDefault());
                        if (ll_found > -1) {
                           decimal ll_qnty = newDs_pk.Rows[ll_found]["qnty"].AsDecimal() + newDs_1.Rows[k]["qnty"].AsDecimal();
                           newDs_pk.Rows[ll_found]["qnty"] = ll_qnty;
                        }
                     }
                     if (newDs_pk.Rows.Count > 1)
                        ld_sum = newDs_pk.Compute("sum(qnty)", "").AsDecimal();//ld_sum = newDs_pk.getitemdecimal(1,"cp_sum_qnty")

                     //商品
                     foreach (DataRow pkRow in newDs_pk.Rows) {
                        ls_output1 = ls_output1 + "," + pkRow["qnty"];
                        pkRow["qnty"] = 0;
                     }//foreach(DataRow pkRow in newDs_pk.Rows)

                     if (is_kind_id2 == "%") {
                        ls_output1 = ls_output1 + "," + ld_sum.AsString();
                     }

                  }//foreach (DataRow ymdRow in lds_ymd.Rows)
                  ls_output1 = ls_output1 + $",{brkRow["cp_rate"].AsDecimal().ToString("n")}";
                  writeFile(is_save_file, ls_output1);
               }//foreach (DataRow brkRow in lds_brk.Rows)
            }
            catch (Exception ex) {
               MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-內容");
               return false;
            }
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-f_70010_week_w");
            return false;
         }
      }

      /// <summary>
      /// 作業:轉70010 日,月,年檔 (公司網站\統計資料\日,月,年)
      /// |
      /// 呼叫來源: 70010 (由業務單位手動產生)10012,10022 (由OP操作批次時自動產生)
      /// </summary>
      /// <param name="is_save_file">檔名</param>
      /// <param name="is_symd">起始日期</param>
      /// <param name="is_eymd">終止日期</param>
      /// <param name="is_sum_type">統計別:D日,M月,Y年</param>
      /// <param name="is_prod_type">商品別:F期貨,O選擇權</param>
      /// <param name="is_market_code"></param>
      /// <param name="selectEng">選取英文版本</param>
      /// <returns>RETURN:false代表error/true代表成功</returns>
      public bool f_70010_ymd_by_market_code(string is_save_file, string is_symd, string is_eymd, string is_sum_type, string is_prod_type, string is_market_code, bool selectEng = false)
      {
         /********************************
         作業:轉70010 日,月,年檔 (公司網站\統計資料\日,月,年)
         呼叫來源: 70010 (由業務單位手動產生)
                   10012,10022 (由OP操作批次時自動產生)
         參數:(1)檔名
              (2)起始日期
	           (3)終止日期
	           (4)統計別:D日,M月,Y年
	           (5)商品別:F期貨,O選擇權
         RETURN:E代表error
                Y代表成功
         ********************************/
         string openData = "";

         //避免重複寫入
         if (File.Exists(is_save_file)) {
            File.Delete(is_save_file);
         }
         File.Create(is_save_file).Close();
         if (!selectEng) {
            openData = PbFunc.f_chg_filename(is_save_file, "_OpenData");
            if (!File.Exists(openData)) {
               File.Delete(openData);
            }
            File.Create(openData).Close();
            writeFile(openData, "期貨商代號,期貨商名稱,日期,商品,交易量");
         }

         D70010 dao70010 = new D70010();
         try {
            ///******************
            //讀取資料
            //******************/
            DataTable ids_1 = dao70010.ListRowdata(is_symd, is_eymd, is_sum_type, is_prod_type, is_market_code);
            if (ids_1.Rows.Count <= 0) {
               throw new System.Exception($@"轉70010-交易量資料轉檔作業({is_sum_type})({is_symd}-{is_eymd})(期貨/選擇權:{ is_prod_type })筆數為０!");
            }
            /* 期貨商 */
            DataTable lds_brk;
            //年度依當年度排序
            if (is_sum_type != "Y") {
               lds_brk = dao70010.List70010brkByMarketCode(is_symd, is_eymd, is_sum_type, is_prod_type, is_market_code);
            }
            else {
               lds_brk = dao70010.List70010brkYearByMarketCode(is_symd, is_eymd, is_sum_type, is_prod_type, is_market_code);
            }

            /* 日期 */
            DataTable lds_ymd = dao70010.ListYmdByMarketCode(is_symd, is_eymd, is_sum_type, is_prod_type, is_market_code);
            //年度不需要總計
            if (is_sum_type != "Y") {
               DataRow newymdRow = lds_ymd.NewRow();
               newymdRow["am0_ymd"] = "99999999";
               lds_ymd.Rows.Add(newymdRow);
            }
            /* 商品 */
            DataTable lds_pk;
            if (is_prod_type == "F") {
               if (is_market_code == "1") {

                  lds_pk = dao70010.ListParamKey("70011n");
               }
               else {
                  lds_pk = dao70010.ListParamKey("70011");
               }
            }//if (is_prod_type == "F")
            else {
               if (is_market_code == "1") {
                  lds_pk = dao70010.ListParamKey("70012n");
               }
               else {
                  lds_pk = dao70010.ListParamKey("70012");
               }
            }//if (is_prod_type == "F")else
            var Ds_pk_query = from dt in lds_pk.AsEnumerable() select dt;
            switch (is_sum_type) {
               case "D":
                  //newDs_pk = lds_pk.Filter($@"rpt_value_3 Is Null or Trim( rpt_value_3) = '' or rpt_value_3 >= '{ is_symd }'");
                  Ds_pk_query = Ds_pk_query.Where(dt => string.IsNullOrEmpty(dt.Field<string>("rpt_value_3").AsString())|| dt.Field<string>("rpt_value_3")== is_symd);
                  break;
               case "M":
                  //newDs_pk = lds_pk.Filter($@"rpt_value_3 Is Null or Trim( rpt_value_3) = '' or Substring(rpt_value_3,0,5) >= '{ is_symd }'");
                  Ds_pk_query = Ds_pk_query.Where(dt => string.IsNullOrEmpty(dt.Field<string>("rpt_value_3").AsString()) || dt.Field<string>("rpt_value_3").SubStr(0,5) == is_symd);
                  break;
               case "Y":
                  //newDs_pk = lds_pk.Filter($@"rpt_value_3 Is Null or Trim( rpt_value_3) = '' or Substring(rpt_value_3,0,3) >= '{ is_symd } '");
                  Ds_pk_query = Ds_pk_query.Where(dt => string.IsNullOrEmpty(dt.Field<string>("rpt_value_3").AsString()) || dt.Field<string>("rpt_value_3").SubStr(0, 3) == is_symd);
                  break;
               default:
                  break;
            }
            DataTable newDs_pk = Ds_pk_query.CopyToDataTable();
            int li_area;
            string ls_brk_no, ls_brk_no4, ls_brk_type, ls_ymd, ls_param_key, ls_str, ls_brk_name;
            /***************************
            因Excel column數限制,
            若"期貨"選擇日期迄超過15,則分成2區
            ***************************/
            if (PbFunc.Right(is_eymd, 2).AsInt() > 15 && is_sum_type == "D" && is_prod_type == "F") {
               li_area = 2;
            }
            else {
               li_area = 1;
            }
            /******************
            表頭
            ******************/
            ls_str = "";
            string ls_output1, ls_output2, ls_OpenData_str;
            string writeOpenData = "";
            decimal ld_sum, ld_val;
            DataTable newDs_ymd = lds_ymd;
            DataTable newDs_1 = ids_1;
            for (int li_area_cnt = 1; li_area_cnt <= li_area; li_area_cnt++) {
               try {
                  if (li_area == 2) {
                     if (li_area_cnt == 1) {
                        newDs_ymd = lds_ymd.Filter("Substring(am0_ymd,6,2) <= '15' and  am0_ymd <> '99999999' ");
                        newDs_1 = ids_1.Filter("Substring(am0_ymd,6,2) <= '15' and  am0_ymd <> '99999999' ");
                     }
                     else {
                        newDs_ymd = lds_ymd.Filter("Substring(am0_ymd,6,2) > '15' or am0_ymd = '99999999' ");
                        newDs_1 = ids_1.Filter("Substring(am0_ymd,6,2) > '15' or am0_ymd = '99999999' ");
                        for (int k = 1; k <= 8; k++) {
                           writeFile(is_save_file, "");
                        }
                     }
                  }

                  ls_output1 = !selectEng ? "期貨商代號" + "," + "名稱" : " Sequential No.";
                  ls_output2 = !selectEng ? "Date" + "," : "Date";
                  li_area_cnt = 1;
                  foreach (DataRow ymdRow in lds_ymd.Rows) {
                     ls_ymd = ymdRow["am0_ymd"].AsString();
                     if (li_area == 2 && PbFunc.Right(is_eymd, 2).AsInt() > 15 && li_area_cnt == 1) {
                        li_area_cnt = 2;
                     }
                     ls_output2 = ls_output2 + "," + ls_ymd + ",".PadRight(newDs_pk.Rows.Count, ',');
                     if (ls_ymd == "99999999") {
                        ls_str = !selectEng ? "總計" : "Year-To-Date Volume of";
                     }
                     foreach (DataRow pkRow in newDs_pk.Rows) {
                        ls_param_key = pkRow["am0_param_key"].AsString();
                        /*******************
                        換商品代號
                        *******************/
                        ls_param_key = pkRow["rpt_value_2"].AsString();
                        ls_output1 = ls_output1 + "," + ls_str + ls_param_key;
                     }//foreach(DataRow pkRow in newDs_pk.Rows)
                     if (ls_ymd == "99999999") {
                        ls_output1 = ls_output1 + "," + (!selectEng ? ls_str : "Year-To-Date Market Volume");
                     }
                     else {
                        ls_output1 = ls_output1 + "," + (!selectEng ? "小計" : "Subtotal");
                     }
                  }//foreach (DataRow ymdRow in lds_ymd.Rows)
                  if (li_area_cnt == li_area && is_sum_type != "Y") {
                     ls_output1 = ls_output1 + "," + (!selectEng ? "市佔率" : "YTD Market shares(%)");
                  }
                  writeFile(is_save_file, ls_output1);
                  writeFile(is_save_file, ls_output2);
               }
               catch (Exception ex) {
                  MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-表頭");
                  return false;
               }
               //內容
               /*******************
               OpenData
               *******************/
               //期貨商代號&名稱
               try {
                  foreach (DataRow brkRow in lds_brk.Rows) {
                     ls_brk_no4 = brkRow["am0_brk_no4"].AsString();
                     ls_brk_type = brkRow["am0_brk_type"].AsString();
                     if (ls_brk_type.Trim() == "9") {
                        ls_brk_no = ls_brk_no4.Trim() + "999";
                     }
                     else {
                        ls_brk_no = ls_brk_no4.Trim() + "000";
                     }
                     ls_brk_name = new ABRK().GetNameByNo(ls_brk_no);// f_get_abrk_name(ls_brk_no,'0')	
                     ls_output1 = !selectEng ? (ls_brk_no + "," + ls_brk_name) : ls_brk_no;//轉英文只秀編號

                     //日期
                     foreach (DataRow ymdRow in lds_ymd.Rows) {
                        ls_ymd = ymdRow["am0_ymd"].AsString();
                        ls_OpenData_str = ls_brk_no + "," + ls_brk_name + "," + ls_ymd;
                        ld_sum = 0;
                        //商品
                        foreach (DataRow pkRow in newDs_pk.Rows) {
                           ls_param_key = pkRow["am0_param_key"].AsString();
                           int ll_found = newDs_1.Rows.IndexOf(newDs_1.Select($@"am0_brk_no4='{ ls_brk_no4 }' and am0_brk_type='{ls_brk_type}' and am0_ymd='{ ls_ymd }' and am0_param_key='{ ls_param_key}'").FirstOrDefault());
                           /* 沒有填0 */
                           if (ll_found > -1) {
                              ld_val = newDs_1.Rows[ll_found]["qnty"].AsDecimal();
                              //ld_sum = ids_1.getitemdecimal(ll_found,"cp_sum_qnty")
                              //ld_sum = (from dt in ids_1.AsEnumerable() where dt.Field<string>("am0_brk_no4") == ls_brk_no4 && dt.Field<string>("am0_brk_type") == ls_brk_type && dt.Field<string>("am0_ymd") == ls_ymd select dt).ToList().Sum(x=>x.Field<decimal>("qnty"));
                              ld_sum = ids_1.Compute("sum(qnty)", $@"am0_brk_no4='{ls_brk_no4}' and am0_brk_type='{ls_brk_type}' and am0_ymd='{ls_ymd }'").AsDecimal();
                           }
                           else {
                              ld_val = 0;
                           }
                           ls_output1 = ls_output1 + "," + ld_val.AsString();
                           if (ls_ymd != "99999999" && !selectEng) {
                              writeOpenData = ls_OpenData_str + "," + ls_param_key + "," + ld_val.AsString();
                              writeFile(openData, writeOpenData);
                           }
                        }//foreach(DataRow pkRow in newDs_pk.Rows)
                        ls_output1 = ls_output1 + "," + ld_sum.AsString();
                     }//foreach (DataRow ymdRow in lds_ymd.Rows)
                     if (li_area_cnt == li_area && is_sum_type != "Y") {
                        ls_output1 = ls_output1 + $",{brkRow["cp_rate"].AsDecimal().ToString("n")}";
                     }
                     writeFile(is_save_file, ls_output1);
                  }//foreach (DataRow brkRow in lds_brk.Rows)
               }
               catch (Exception ex) {
                  MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-內容");
                  return false;
               }
            }//for (int li_area_cnt = 1; li_area_cnt <= li_area; li_area_cnt++)
             /*******************
             W_OpenData
             *******************/
            if (is_prod_type == "O" && is_sum_type == "D") {
               ids_1 = dao70010.ListRowdataOpendata(is_eymd, is_eymd, is_market_code);
               ExportOptions csvref = new ExportOptions();
               csvref.HasHeader = true;
               csvref.Encoding = System.Text.Encoding.GetEncoding(950);//ASCII
               ExportHelper.ToCsv(ids_1, PbFunc.f_chg_filename(is_save_file, "_W_opendata"), csvref);
            }
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err);
            return false;
         }
         return true;
      }
      /// <summary>
      /// 作業:轉70010 日,月,年檔 (公司網站\統計資料\日,月,年)英文版
      /// |
      /// 呼叫來源: 70010 (由業務單位手動產生)10012,10022 (由OP操作批次時自動產生)
      /// </summary>
      /// <param name="is_save_file">檔名</param>
      /// <param name="is_symd">起始日期</param>
      /// <param name="is_eymd">終止日期</param>
      /// <param name="is_sum_type">統計別:D日,M月,Y年</param>
      /// <param name="is_prod_type">商品別:F期貨,O選擇權</param>
      /// <param name="is_market_code"></param>
      /// <param name="selectEng">選取英文版本</param>
      /// <returns>RETURN:false代表error/true代表成功</returns>
      public bool f_70010_ymd_eng_by_market_code(string is_save_file, string is_symd, string is_eymd, string is_sum_type, string is_prod_type, string is_market_code)
      {
         if (f_70010_ymd_by_market_code(is_save_file, is_symd, is_eymd, is_sum_type, is_prod_type, is_market_code, true))
            return true;
         else
            return false;
      }
      /// <summary>
      /// 作業:轉"歷史"70010日,月,年檔 (公司網站\統計資料\日,月,年)
      /// |
      /// 呼叫來源: 11010
      /// </summary>
      /// <param name="is_save_file"></param>
      /// <param name="is_symd"></param>
      /// <param name="is_eymd"></param>
      /// <param name="is_sum_type"></param>
      /// <param name="is_prod_type"></param>
      /// <returns></returns>
      public bool f_70010_ymd_his(string is_save_file, string is_symd, string is_eymd, string is_sum_type, string is_prod_type)
      {
         return true;
      }
      /// <summary>
      /// 作業:轉70010 日,月,年檔 (公司網站\統計資料\日,月,年)
      /// |
      /// 呼叫來源: 11010
      /// </summary>
      /// <param name="is_save_file"></param>
      /// <param name="is_symd"></param>
      /// <param name="is_eymd"></param>
      /// <param name="is_sum_type"></param>
      /// <param name="is_prod_type"></param>
      /// <returns></returns>
      public bool f_70010_ymd_org(string is_save_file, string is_symd, string is_eymd, string is_sum_type, string is_prod_type)
      {
         return true;
      }
      /// <summary>
      /// 作業: 轉70010 日, 月, 年檔 (公司網站\統計資料\日 , 月 , 年)
      /// |
      /// 呼叫來源: 70010(由業務單位手動產生)10012,10022(由OP操作批次時自動產生)
      /// </summary>
      /// <param name="is_save_file">檔名</param>
      /// <param name="is_symd">起始日期</param>
      /// <param name="is_eymd">終止日期</param>
      /// <param name="is_sum_type">統計別: D日,M月,Y年</param>
      /// <param name="is_kind_id2">商品別: F期貨,O選擇權</param>
      /// <param name="is_param_key"></param>
      /// <param name="is_prod_type"></param>
      public bool f_70010_ymd_w(string is_save_file, string is_symd, string is_eymd,
                                 string is_sum_type, string is_kind_id2,
                                 string is_param_key, string is_prod_type)
      {

         //作業: 轉70010 日, 月, 年檔 (公司網站\統計資料\日 , 月 , 年)
         //呼叫來源: 70010(由業務單位手動產生)
         //          10012,10022(由OP操作批次時自動產生)
         //參數: (1)檔名
         //      (2)起始日期
         //      (3)終止日期
         //      (4)統計別: D日,M月,Y年
         //       (5)商品別: F期貨,O選擇權
         //   RETURN:E代表error
         //          Y代表成功
         //避免重複寫入
         if (File.Exists(is_save_file)) {
            File.Delete(is_save_file);
         }
         File.Create(is_save_file).Close();

         D70050 dao70050 = new D70050();
         D70010 dao70010 = new D70010();
         try {
            ///******************
            //讀取資料
            //******************/
            DataTable ids_1 = dao70050.ListAll(is_symd, is_eymd, is_sum_type, is_prod_type, is_kind_id2, is_param_key);
            if (ids_1.Rows.Count <= 0) {
               throw new System.Exception($@"轉{is_param_key}-交易量資料轉檔作業({is_sum_type})({is_symd}-{is_eymd})(期貨/選擇權:{ is_prod_type })筆數為０!");
            }
            /* 期貨商 */
            DataTable lds_brk;
            lds_brk = dao70050.List70050brk(is_symd, is_eymd, is_sum_type, is_prod_type, is_kind_id2, is_param_key);

            /* 日期 */
            DataTable lds_ymd = dao70010.ListYMD(is_symd, is_eymd, is_sum_type, is_prod_type);
            DataRow newymdRow = lds_ymd.NewRow();
            newymdRow["am0_ymd"] = "99999999";
            lds_ymd.Rows.Add(newymdRow);
            /* 商品 */
            DataTable lds_pk;
            if (is_param_key == "TXO") {
               lds_pk = dao70010.ListParamKey("70040");
            }//if (is_param_key == "TXO")
            else {
               lds_pk = dao70010.ListParamKey("70050");
            }
            DataTable newDs_pk = lds_pk.Filter($@"am0_param_key like '{ is_kind_id2 }'");
            string ls_brk_no, ls_brk_no4, ls_brk_type, ls_ymd, ls_str;
            /******************
            表頭
            ******************/
            ls_str = "";
            string ls_output1, ls_output2;
            decimal ld_sum;
            DataTable newDs_ymd = lds_ymd;
            DataTable newDs_1 = ids_1;
            try {
               ls_output1 = "期貨商代號" + "," + "名稱";
               ls_output2 = "Date" + ",";
               foreach (DataRow ymdRow in lds_ymd.Rows) {
                  ls_ymd = ymdRow["am0_ymd"].AsString();
                  ls_output2 = ls_output2 + "," + ls_ymd;
                  if (is_kind_id2 == "%") {
                     ls_output2 = ls_output2 + ",".PadRight(newDs_pk.Rows.Count, ',');
                  }

                  if (ls_ymd == "99999999") {
                     ls_str = "總計";
                  }
                  foreach (DataRow pkRow in newDs_pk.Rows) {
                     is_param_key = pkRow["am0_param_key"].AsString();
                     /*******************
                     換商品代號
                     *******************/
                     is_param_key = pkRow["rpt_value_2"].AsString();
                     ls_output1 = ls_output1 + "," + ls_str + is_param_key;
                  }//foreach(DataRow pkRow in newDs_pk.Rows)
                  if (is_kind_id2 == "%") {
                     if (ls_ymd == "99999999") {
                        ls_output1 = ls_output1 + "," + ls_str;
                     }
                     else {
                        ls_output1 = ls_output1 + "," + "小計";
                     }
                  }
               }//foreach (DataRow ymdRow in lds_ymd.Rows)
               ls_output1 = ls_output1 + "," + "市佔率";
               writeFile(is_save_file, ls_output1);
               writeFile(is_save_file, ls_output2);
            }
            catch (Exception ex) {
               MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-表頭");
               return false;
            }
            //內容
            //期貨商代號&名稱
            try {
               foreach (DataRow brkRow in lds_brk.Rows) {
                  ls_brk_no4 = brkRow["am0_brk_no4"].AsString();
                  ls_brk_type = brkRow["am0_brk_type"].AsString();
                  if (ls_brk_type.Trim() == "9") {
                     ls_brk_no = ls_brk_no4.Trim() + "999";
                  }
                  else {
                     ls_brk_no = ls_brk_no4.Trim() + "000";
                  }
                  string ls_brk_name = new ABRK().GetNameByNo(ls_brk_no);// f_get_abrk_name(ls_brk_no,'0')	
                  ls_output1 = ls_brk_no + "," + ls_brk_name;

                  //日期
                  foreach (DataRow ymdRow in lds_ymd.Rows) {
                     ls_ymd = ymdRow["am0_ymd"].AsString();
                     ld_sum = 0;
                     //商品
                     foreach (DataRow pkRow in newDs_pk.Rows) {
                        is_param_key = pkRow["am0_param_key"].AsString();
                        int ll_found = newDs_1.Rows.IndexOf(newDs_1.Select($@"am0_brk_no4='{ ls_brk_no4 }' and am0_brk_type='{ls_brk_type}' and am0_ymd='{ ls_ymd }' and am0_param_key='{ is_param_key}'").FirstOrDefault());
                        /* 沒有填0 */
                        if (ll_found > -1) {
                           ls_output1 = ls_output1 + "," + newDs_1.Rows[ll_found]["qnty"].AsDecimal();
                           //ld_sum = ids_1.getitemdecimal(ll_found,"cp_sum_qnty")
                           //ld_sum = (from dt in ids_1.AsEnumerable() where dt.Field<string>("am0_brk_no4") == ls_brk_no4 && dt.Field<string>("am0_brk_type") == ls_brk_type && dt.Field<string>("am0_ymd") == ls_ymd select dt).ToList().Sum(x=>x.Field<decimal>("qnty"));
                           ld_sum = ids_1.Compute("sum(qnty)", $@"am0_brk_no4='{ls_brk_no4}' and am0_brk_type='{ls_brk_type}' and am0_ymd='{ls_ymd }'").AsDecimal();
                        }
                        else {
                           ls_output1 = ls_output1 + ",0";
                        }
                     }//foreach(DataRow pkRow in newDs_pk.Rows)
                     if (is_kind_id2 == "%") {
                        ls_output1 = ls_output1 + "," + ld_sum.AsString();
                     }
                  }//foreach (DataRow ymdRow in lds_ymd.Rows)
                  ls_output1 = ls_output1 + $",{brkRow["cp_rate"].AsDecimal().ToString("n")}";
                  writeFile(is_save_file, ls_output1);
               }//foreach (DataRow brkRow in lds_brk.Rows)
            }
            catch (Exception ex) {
               MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-內容");
               return false;
            }
            return true;
         }
         catch (Exception ex) {
            MessageDisplay.Error(ex.Message, GlobalInfo.gs_t_err + "-f_70010_ymd_w");
            return false;
         }
      }
   }
}
