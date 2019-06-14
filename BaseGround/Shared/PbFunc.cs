using BusinessObjects;
using Common;
using DataObjects.Dao.Together;
using DataObjects.Dao.Together.TableDao;
using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Media;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// ken,2018/12/19 翻寫
/// </summary>
namespace BaseGround.Shared {
   public static class PbFunc {

      #region 全域變數
      const string gs_sys = "CI";
      const string gs_t_result = "處理結果";     //Information!
      const string gs_t_warning = "警告訊息";        //Exclamation!
      const string gs_t_err = "錯誤訊息";    //gs_t_err,MessageBoxButtons.OK,MessageBoxIcon.Stop
      const string gs_t_question = "請選擇";            //Question!
      const string gs_m_ok = "處理完成";
      const string gs_m_no_data = "無此筆資料!";
      const string gs_m_field_err = "欄位資料輸入錯誤!";
      const string gs_m_no_auth = "無此權限，執行此交易";
      const string gs_m_not_allow_exec = "時點不允許執行此交易,視窗即將關閉.";

      const int gi_w_height = 2650;
      const int gi_w_width = 4630;
      const int gi_sub_dw_height = 0;
      const int gi_sub_dw_width = 0;

      static string gs_user_id = GlobalInfo.USER_ID,
                    gs_dpt_id = GlobalInfo.USER_DPT_ID,
                    gs_user_name = GlobalInfo.USER_NAME,
                    gs_dpt_name = GlobalInfo.USER_DPT_NAME;//使用者

      /*******************************
      執行作業
      *******************************/
      //現正執行之作業代號
      static string gs_txn_id;
      static string gs_txn_name;
      const int gi_txn_id_pos = 2;
      const int gi_txn_id_len = 5;
      const int gi_txn_name_pos = 9;
      const int gi_txn_name_len = 40;


      static string gs_ap_path;//AP路徑(application path)
      static string gs_bmp_path;
      static string gs_bmp_folder;
      static string gs_work_path;//本磁碟機
      static string gs_excel_path { get { return GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH; } }//Excel路徑
      static string gs_savereport_path { get { return GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH; } }//報表儲存路徑
      static string gs_bcp_path;//載入Data程式位置
      static string gs_batch_path;//Run Batch的路徑

      static string gs_screen_type;//Enviroment 
      static DateTime gdt_ocf_date;//Enviroment 
      #endregion

      /// <summary>
      /// 指定日期加減N天
      /// </summary>
      /// <param name="d"></param>
      /// <param name="n"></param>
      /// <returns></returns>
      public static DateTime relativedate(DateTime d, int n) {
         return d.AddDays(n);
      }

      /// <summary>
      /// alarm音效
      /// </summary>
      public static void f_alarm_sound() {
         SoundPlayer lui_NumDevs;
         string ls_SoundFile;
         //string gs_excel_path = GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH;
         ls_SoundFile = Path.Combine(gs_excel_path, "alarm_bb.wav");// 音效檔檔名
         if (File.Exists(ls_SoundFile)) { // 檢查是否有音效裝置
            lui_NumDevs = new SoundPlayer(ls_SoundFile);
            lui_NumDevs.LoadAsync();// 非同步播放一次
         }
      }

      /// <summary>
      /// 計算該月份YYYYMM實際交易日(除了六日,還扣掉颱風天,加上補上班)
      /// </summary>
      /// <param name="as_ym"></param>
      /// <returns></returns>
      public static int f_calc_mth_trade_days(string as_ym) {
         DateTime ldt_fm_date = DateTime.ParseExact(as_ym + "01", "yyyyMMdd", null, DateTimeStyles.AllowWhiteSpaces);
         DateTime ldt_to_date = ldt_fm_date;
         int tradeDayCount = 0;

         //先排除六日
         while (ldt_to_date.ToString("yyyyMM") == as_ym) {
            if (ldt_to_date.DayOfWeek != DayOfWeek.Saturday && ldt_to_date.DayOfWeek != DayOfWeek.Sunday) {
               tradeDayCount++;
            }
            ldt_to_date = ldt_to_date.AddDays(1);
         }
         //for (int k = 0; k <= 30; k++) {
         //    if (ldt_fm_date >= ldt_to_date) {
         //        break;
         //    }
         //    if (ldt_fm_date.DayOfWeek != DayOfWeek.Saturday && ldt_fm_date.DayOfWeek != DayOfWeek.Sunday) {
         //        tradeDayCount++;
         //    }
         //    ldt_to_date = ldt_to_date.AddDays(1);
         //}//for(int k = 0;k <= 30;k++) {

         //該月份額外減少的交易日 = 輸出颱風天數-假日補上班天數
         DTS dts = new DTS();
         int substractDay = dts.GetSubstractDay(ldt_fm_date, ldt_to_date);

         return tradeDayCount - substractDay;
      }

      /// <summary>
      /// 改檔名(不更動副檔名),例如Daily_OPT20181225.csv變成Daily_OPT20181225_OpenData.csv
      /// </summary>
      /// <param name="filename"></param>
      /// <param name="appendString"></param>
      /// <returns></returns>
      public static string f_chg_filename(string filename, string appendString) {
         int pos = filename.LastIndexOf('.');//抓最後的小數點才對
         string res = filename.Substring(0, pos) + appendString + filename.Substring(pos);
         return res;
      }


      public static string f_20110_sp(DateTime ldt_date, string as_txn_id) {
         //////int li_return;
         //////string ls_prod_type = "M";
         ///////*******************
         //////轉統計資料TDT
         //////*******************/

         ////////[ken] DECLARE My_SP PROCEDURE FOR ci.sp_U_gen_H_TDT(:ldt_date,:ls_prod_type) USING sqlca;
         ////////[ken] execute My_SP;

         //////if (sqlca.SqlCode < 0) {
         //////    MessageBox.Show("執行SP(sp_U_gen_H_TDT(" + ls_prod_type + "))錯誤! " + "sqlca.sqlCode= " +
         //////                    sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////    return "E";
         //////} else {
         //////    //[ken] Fetch My_SP INTO :li_return;
         //////    //[ken] Close My_SP;
         //////}
         //////f_write_logf(gs_txn_id, "E", "執行sp_U_gen_H_TDT(" + ls_prod_type + ")");


         //////if (as_txn_id == "20110") {
         //////    ls_prod_type = "J";
         //////    //[ken] DECLARE My_SP9 PROCEDURE FOR ci.sp_U_gen_H_TDT(:ldt_date,:ls_prod_type) USING sqlca;
         //////    //[ken] execute My_SP9;

         //////    if (sqlca.SqlCode < 0) {
         //////        MessageBox.Show("執行SP(sp_U_gen_H_TDT(" + ls_prod_type + "))錯誤! " + "sqlca.sqlCode= " +
         //////        sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////        return "E";
         //////    } else {
         //////        //[ken] Fetch My_SP9 INTO :li_return;
         //////        //[ken] Close My_SP9;

         //////    }
         //////    f_write_logf(gs_txn_id, "E", "執行sp_U_gen_H_TDT(" + ls_prod_type + ")");


         //////    //JTX 日統計AI2;
         //////    //[ken] DECLARE My_SP6 PROCEDURE FOR ci.sp_U_stt_H_AI2_Day(:ldt_date,:ls_prod_type) USING sqlca;
         //////    //[ken] execute My_SP6;

         //////    if (sqlca.SqlCode < 0) {
         //////        MessageBox.Show("執行SP(sp_U_stt_H_AI2_Day)錯誤! " + "sqlca.sqlCode= " +
         //////        sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////        return "E";
         //////    } else {
         //////        //[ken] Fetch My_SP6 INTO :li_return;
         //////        //[ken] Close My_SP6;

         //////    }
         //////    f_write_logf(gs_txn_id, "E", "執行sp_U_stt_H_AI2_Day");
         //////    ;
         //////    //JTX 月統計AI2;
         //////    //[ken] DECLARE My_SP7 PROCEDURE FOR ci.sp_U_stt_H_AI2_Month(:ldt_date,:ls_prod_type) USING sqlca;
         //////    //[ken] execute My_SP7;

         //////    if (sqlca.SqlCode < 0) {
         //////        MessageBox.Show("執行SP(sp_U_stt_H_AI2_Month)錯誤! " + "sqlca.sqlCode= " +
         //////        sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////        return "E";
         //////    } else {
         //////        //[ken] Fetch My_SP7 INTO :li_return;
         //////        //[ken] Close My_SP7;

         //////    }
         //////    f_write_logf(gs_txn_id, "E", "執行sp_U_stt_H_AI2_Month");
         //////}

         ///////*******************
         //////轉統計資料AI3
         //////*******************/
         ////////[ken] DECLARE My_SP2 PROCEDURE FOR ci.sp_H_stt_AI3(:ldt_date) USING sqlca;
         ////////[ken] execute My_SP2;

         //////if (sqlca.SqlCode < 0) {
         //////    MessageBox.Show("執行SP(sp_H_stt_AI3)錯誤! " + "sqlca.sqlCode= " +
         //////    sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////    return "E";
         //////} else {
         //////    //[ken] Fetch My_SP2 INTO :li_return;
         //////    //[ken] Close My_SP2;

         //////}
         //////f_write_logf(gs_txn_id, "E", "執行sp_H_stt_AI3");

         ///////*******************
         //////更新AI6 (震幅波動度)
         //////*******************/

         ////////[ken] DECLARE My_SP3 PROCEDURE FOR ci.sp_H_gen_AI6(:ldt_date) USING sqlca;
         ////////[ken] execute My_SP3;

         //////if (sqlca.SqlCode < 0) {
         //////    MessageBox.Show("執行SP(sp_H_gen_AI6)錯誤! " + "sqlca.sqlCode= " +
         //////    sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////    return "E";
         //////} else {
         //////    //[ken] Fetch My_SP3 INTO :li_return;
         //////    //[ken] Close My_SP3;

         //////}
         //////f_write_logf(gs_txn_id, "E", "執行sp_H_gen_AI6");


         ///////*******************
         //////更新AA3
         //////*******************/

         ////////[ken] DECLARE My_SP4 PROCEDURE FOR ci.sp_H_upd_AA3(to_char(:ldt_date,"yyyymmdd")) USING sqlca;
         ////////[ken] execute My_SP4;

         //////if (sqlca.SqlCode < 0) {
         //////    MessageBox.Show("執行SP(sp_H_upd_AA3)錯誤! " + "sqlca.sqlCode= " +
         //////    sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////    return "E";
         //////} else {
         //////    //[ken] Fetch My_SP4 INTO :li_return;
         //////    //[ken] Close My_SP4;

         //////}
         //////f_write_logf(gs_txn_id, "E", "執行sp_H_upd_AA3");


         ///////*******************
         //////更新AI8
         //////*******************/

         ////////[ken] DECLARE My_SP5 PROCEDURE FOR ci.sp_H_gen_H_AI8(to_char(:ldt_date,"yyyymmdd")) USING sqlca;
         ////////[ken] execute My_SP5;

         //////if (sqlca.SqlCode < 0) {
         //////    MessageBox.Show("執行SP(sp_H_gen_H_AI8)錯誤! " + "sqlca.sqlCode= " +
         //////    sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////    return "E";
         //////} else {
         //////    //[ken] Fetch My_SP5 INTO :li_return;
         //////    //[ken] Close My_SP5;

         //////}
         //////f_write_logf(gs_txn_id, "E", "執行sp_H_gen_H_AI8");


         return "";
      }

      /// <summary>
      /// 執行程式前,檢查之前必要的批次有沒有正常執行
      /// </summary>
      /// <param name="as_txn_id"></param>
      /// <param name="adt_date"></param>
      /// <param name="os_osw_grp"></param>
      /// <returns></returns>
      public static string f_chk_130_wf(string as_txn_id, DateTime adt_date, string os_osw_grp) {
         /***************************************
         有加f_chk_130的作業代號：
         42010, 42011, 42020, 42030 
         40011, 40021,40012
         40013
         43010, 43020, 43030
         40010, 40020
         ***************************************/

         //ken,說有13個功能引用,下面switch只有9個功能阿(40010, 40011, 40012, 40013, 40020, 40021, 43010, 43020, 40030)
         //john,2019/04/19因應PB更新修改(40010,40011,40012,40013,40021,40022)

         string res = "";
         JLOG jLOG = new JLOG();

         int li_rtn = 0;
         string ls_osw_grp = "";
         int li_cnt_start = 1;
         int li_cnt_end = 1;

         switch (as_txn_id) {
            case "40010":
            case "40021":
            case "40011":
               ls_osw_grp = "1";
               li_rtn = jLOG.GetJobCount(adt_date, ls_osw_grp);

               if (li_rtn < 2) {
                  res = "機房「130C」批次作業尚未完成";
                  break;
               }
               break;
            case "40012":
            case "40022":
               ls_osw_grp = "5";
               li_rtn = jLOG.GetJobCount(adt_date, ls_osw_grp);

               if (li_rtn < 2) {
                  res = "機房「130C」批次作業尚未完成";
                  break;
               }
               break;
            case "40013":
               ls_osw_grp = "7";
               li_rtn = jLOG.ListJobWorkflow(adt_date, ls_osw_grp).Select("JLOG_WORKFLOW='wf_FB_AI0130C'").Length;//Count

               if (li_rtn < 1) {
                  res = "機房「130C」批次作業尚未完成";
                  break;
               }
               break;
            case "43010":
            case "43020":
               #region 1 or 2 time, os_osw_grp=1/5, 正常1和5需要跑兩次
               switch (os_osw_grp.Substring(0, 1)) {
                  case "%":
                     //ken,跑兩次,第一次ls_osw_grp=1,第二次ls_osw_grp=5
                     li_cnt_start = 1;
                     li_cnt_end = 2;
                     break;
                  case "1":
                     li_cnt_start = 1;
                     li_cnt_end = 1;
                     break;
                  case "5":
                     li_cnt_start = 2;
                     li_cnt_end = 2;
                     break;
               }//switch

               for (int k = li_cnt_start; k <= li_cnt_end; k++) {
                  ls_osw_grp = (k == 1 ? "1" : "5");

                  li_rtn = jLOG.GetJobCount(adt_date, ls_osw_grp);

                  if (li_rtn < 2) {
                     res = "機房「130C」批次（群組:" + k.ToString() + "）作業尚未完成";
                     break;
                  }
               }//for
               #endregion
               break;
            case "40030":
               #region 1 or 3 time, os_osw_grp=1/5/7, 正常1和5需要跑兩次,7需要跑一次
               switch (os_osw_grp.Substring(0, 1)) {
                  case "%":
                     li_cnt_start = 1;
                     li_cnt_end = 3;
                     break;
                  case "1":
                     li_cnt_start = 1;
                     li_cnt_end = 1;
                     break;
                  case "5":
                     li_cnt_start = 2;
                     li_cnt_end = 2;
                     break;
                  case "7":
                     li_cnt_start = 3;
                     li_cnt_end = 3;
                     break;
               }

               for (int k = li_cnt_start; k <= li_cnt_end; k++) {
                  switch (k) {
                     case 1:
                        ls_osw_grp = "1";
                        break;
                     case 2:
                        ls_osw_grp = "5";
                        break;
                     case 3:
                        ls_osw_grp = "7";
                        break;
                  }

                  li_rtn = jLOG.GetJobCount(adt_date, ls_osw_grp);

                  if ((li_rtn < 2 && ls_osw_grp != "7") || (li_rtn < 1 && ls_osw_grp == "7")) {
                     res = "機房「130C」批次（群組:" + k.ToString() + "）作業尚未完成";
                     break;
                  }
               }//for
               #endregion
               break;
         }//switch (as_txn_id) {

         return res;
      }

      /// <summary>
      /// 查看Ai2 作業 40040 / 40030
      /// </summary>
      /// <param name="ls_ymd"></param>
      /// <param name="is_osw_grp">欲查詢之group 1 / 5 / %</param>
      /// <param name="ls_choose"></param>
      /// <param name="group_name">群組名稱 EX: group1 / group2</param>
      /// <param name="cnt_limt">li_cnt 正常值 group1>2 / group2>1</param>
      /// <returns></returns>
      public static string f_chk_ai2(string ls_ymd, string is_osw_grp, string ls_choose, string group_name, int cnt_limt) {
         int li_cnt = 0;

         li_cnt = new AI2().GetJobStatus(ls_ymd, is_osw_grp);

         if (li_cnt < cnt_limt) {
            if (ls_choose == "N") {
               MessageDisplay.Error($"每日行情統計檔(AI2)－{group_name}轉檔未完成，請稍候再執行!");
               return "E";
            } else if (MessageDisplay.Choose($"每日行情統計檔(AI2)－{group_name}轉檔未完成，是否要繼續?") == DialogResult.No) {
               return "E";
            }
         }

         return "";
      }

      public static string f_chk_mg5(string as_txn_id, string ls_ymd, string is_osw_grp, string ls_choose) {
         /***************************************
         有加f_chk_mg5的作業代號：
         40030,40010
         ***************************************/

         int li_cnt;
         string ls_grp;

         ls_grp = is_osw_grp;
         if (ls_grp != "%") {
            ls_grp = ls_grp + "%";
         }
      ;
         //判斷是否有沒輸入49060;
         //select sum(case when MG5_KIND_ID is null then 1 else 0 end) into: li_cnt
         //from ci.MGT2,ci.APDK,
         //    (select MG5_KIND_ID from ci.MG5 
         //    where MG5_DATE = TO_DATE(:ls_ymd, "YYYYMMDD") group by MG5_KIND_ID)
         //where MGT2_KIND_ID = APDK_MARKET_CLOSE LIKE :ls_grp
         //and MGT2_F_FLAG = "Y"
         //and MGT2_KIND_ID = MG5_KIND_ID(+)



         //////if (li_cnt > 0) {
         //////    if (ls_choose == "N") {
         //////        MessageBox.Show("49060資料未輸入，請先輸入完成再執行! ", gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////        //洽OP：盤後資訊管理系統「(F)10012作業第5項&(O)10022作業第5項」是否已完成?;
         //////        return "E";
         //////    } else {
         //////        if (MessageBox.Show(gs_t_question, "49060資料未輸入，是否要繼續? ", Question!, YesNo!, 2) = 2) {
         //////            return "E";
         //////        }
         //////    }
         //////}

         return "";
      }

      /// <summary>
      /// 檢查今日盤後轉檔作業是否都完成,但這邊都回傳true
      /// </summary>
      /// <param name="txnId"></param>
      /// <returns></returns>
      public static bool f_chk_run_timing(string txnId) {
         return true;
      }

      /// <summary>
      /// 改成擴充方法,直接引用(這邊以後可以移除)
      /// </summary>
      /// <param name="currencyType"></param>
      /// <returns></returns>
      public static string f_conv_currency_type(CurrencyType currencyType) {
         return currencyType.GetDesc();
      }

      /// <summary>
      /// 將日期格式轉換成字串,之後要改為擴充方法
      /// </summary>
      /// <param name="value"></param>
      /// <param name="dateType"></param>
      /// <returns></returns>
      public static string f_conv_date(DateTime value, int dateType) {
         //1  = yyyymmdd
         //2  = yy年mm月dd日(Ex.1998/7/1 -->87年07月01日)
         //3  = yy年mm月dd日(月, 日若是個位數則不補0, Ex.1998/7/1 -->87年7月1日)
         //4  = 中華民國yy年mm月dd日(Ex.1998/7/1 -->87年07月01日)
         //5  = 中華民國yy年mm月dd日(月, 日若是個位數則不補0, Ex.1998/7/1 -->87年7月1日)
         //6  = yy/mm/dd日(Ex.1998/7/1 -->87/07/01)

         string ls_rtn = "";
         DateTime tmp = value;
         TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

         switch (dateType) {
            case 1://1  = yyyymmdd
               ls_rtn = tmp.ToString("yyyyMMdd");
               break;

            case 2://2  = yy年mm月dd日(Ex.1998/7/1 -->87年07月01日)
               ls_rtn = string.Format("{0}年{1}月{2}日",
                                       taiwanCalendar.GetYear(tmp),
                                       tmp.Month.ToString().PadLeft(2, '0'),
                                       tmp.Day.ToString().PadLeft(2, '0'));
               break;
            case 3://3  = yy年mm月dd日(月, 日若是個位數則不補0, Ex.1998/7/1 -->87年7月1日)
               ls_rtn = string.Format("{0}年{1}月{2}日",
                                       taiwanCalendar.GetYear(tmp),
                                       tmp.Month,
                                       tmp.Day);
               break;
            case 4://4  = 中華民國yy年mm月dd日(Ex.1998/7/1 -->87年07月01日)
               ls_rtn = string.Format("中華民國{0}年{1}月{2}日",
                                       taiwanCalendar.GetYear(tmp),
                                       tmp.Month.ToString().PadLeft(2, '0'),
                                       tmp.Day.ToString().PadLeft(2, '0'));
               break;
            case 5://5  = 中華民國yy年mm月dd日(月, 日若是個位數則不補0, Ex.1998/7/1 -->87年7月1日)
               ls_rtn = string.Format("中華民國{0}年{1}月{2}日",
                                       taiwanCalendar.GetYear(tmp),
                                       tmp.Month,
                                       tmp.Day);
               break;
            case 6://6  = yy/mm/dd日(Ex.1998/7/1 -->87/07/01)
            case 7://7  = yy/mm/dd日(Ex.1998/7/1 -->87/07/01)
               ls_rtn = string.Format("{0}/{1}/{2}",
                                       taiwanCalendar.GetYear(tmp),
                                       tmp.Month.ToString().PadLeft(2, '0'),
                                       tmp.Day.ToString().PadLeft(2, '0'));
               break;
         }//switch

         return ls_rtn;
      }//function

      /// <summary>
      /// 讀取ci.RPTX設定,將範本檔copy到Report Path,成功的話回傳 完整的路徑/單純的新檔名
      /// </summary>
      /// <param name="txnId"></param>
      /// <param name="excelFileName">該功能裡面,各分類所對應不同的報表template檔名</param>
      /// <param name="newExcelFileName">如果為空,則txnId_yyyy.mm.dd-hh.mm.ss.xlsx</param>
      /// <returns>只有回傳copy過來的檔名不包含路徑,內容還是範本</returns>
      public static string wf_copy_file(string txnId, string excelFileName, string newExcelFileName = "") {
         string ls_excel_path = gs_excel_path;
         string ls_excel_ext = ".xlsx";//tempalte 副檔名,複製過去的副檔名也是這個
         string ls_sub_path = ".";

         //1.讀取ci.RPTX設定檔 (正常只會撈出一筆資料,column = ls_sub_path/ls_excel_ext/ls_rename)
         DataTable dtRpt = new RPTX().ListByTxn(txnId, excelFileName);
         //if (dtRpt.Rows.Count <= 0) {
         //   throw new Exception(string.Format("RPTX無{0}設定!", txnId));
         //}
         if (dtRpt != null) {
            if (dtRpt.Rows.Count > 0) {
               ls_sub_path = dtRpt.Rows[0]["ls_sub_path"].AsString();
               ls_excel_ext = dtRpt.Rows[0]["ls_excel_ext"].AsString();
            }
         }
         //string ls_rename = dtRpt.Rows[0]["ls_rename"].AsString();//ken,根本沒用到,抓爽的

         if (ls_sub_path != ".") {
            ls_excel_path += ls_sub_path;
         }

         string excelFilePath = Path.Combine(ls_excel_path, excelFileName + ls_excel_ext);

         //2.檢查範本檔是否存在
         if (!File.Exists(excelFilePath)) {
            throw new Exception("無此檔案「" + excelFilePath + "」!");
         }

         string tmpDate1 = DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss");
         string targetFileName = excelFileName + "_" + tmpDate1 + ls_excel_ext;
         string targetFilePath = "";
         if (newExcelFileName == "") {
            targetFilePath = Path.Combine(gs_savereport_path, targetFileName);
         } else {
            targetFilePath = Path.Combine(gs_savereport_path, newExcelFileName);
         }

         //3.copy template excel to target report path
         try {
            File.Copy(excelFilePath, targetFilePath);
         } catch {
            throw new Exception(string.Format("複製「{0}」到「{1}」檔案錯誤!", excelFilePath, targetFilePath));
         }

         //4.return
         if (newExcelFileName == "") {
            return targetFilePath;//回傳完整的路徑
         } else {
            return newExcelFileName;//只有回傳單純的新檔名
         }
      }

      /// <summary>
      /// 將範本檔copy到Report Path,成功的話回傳檔名
      /// </summary>
      /// <param name="excelFileName"></param>
      /// <param name="as_type"></param>
      /// <param name="as_addstr"></param>
      /// <returns>只有回傳copy過來的檔名不包含路徑,內容還是範本</returns>
      public static string f_copy_file(string excelFileName, string as_type, string as_addstr) {
         /*********************************************************** 
         as_type : T : [as_filename]_yyyy.mm.dd-hh24:mi:ss
                   D : [as_filename]_yyyy.mm.dd
                   d : yyyy.mm.dd 
                   空白 : 空白
         as_addstr : 檔案名後加特定文字

         eg. f_copy_file("30055_TJF","D","") -->30050_TJF_2015.12.21.xls  
         eg. f_copy_file("30055_TJF","d","_TJF") -->2015.12.21_TJF.xls  
         eg. f_copy_file("30055_TJF"," ","2015.12.21_TJF") -->2015.12.21_TJF.xls   (10023)
         ***********************************************************/
         string excelFilePath = Path.Combine(gs_excel_path, excelFileName + ".xls");

         //1.檢查範本檔是否存在
         if (!File.Exists(excelFilePath)) {
            throw new Exception("無此檔案「" + excelFilePath + "」!");
         }

         string tmpDate1 = DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss");
         string tmpDate2 = DateTime.Now.ToString("yyyy.MM.dd");

         string ls_filename = "";
         switch (as_type) {
            case "T":
               ls_filename = excelFileName + "_" + tmpDate1;
               break;
            case "D":
               ls_filename = excelFileName + "_" + tmpDate2;
               break;
            case "d":
               ls_filename = tmpDate2;
               break;
            case " ":
               ls_filename = "";
               break;
         }
         string targetFileName = ls_filename + as_addstr + ".xls";
         string targetFilePath = Path.Combine(gs_savereport_path, targetFileName);
         string targetFilePathBak = targetFilePath.Replace(".xls", "_bak_" + tmpDate1 + ".xls");

         //2.假如檔案存在,改原檔名加上_bak_yyyy.MM.dd-HH.mm.ss
         if (File.Exists(targetFilePath)) {
            File.Move(targetFilePath, targetFilePathBak);
         }

         //3.copy template excel to target report path
         try {
            File.Copy(excelFilePath, targetFilePath);

            //ken,如果要開檔案寫檔,要把該檔案先lock
            //using (FileStream lockFile = new FileStream(targetFilePath,FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.Delete)){}

         } catch (Exception ex) {
            throw new Exception(string.Format("複製「{0}」到「{1}」檔案錯誤!", excelFilePath, targetFilePath));
         }

         return targetFileName;//只有回傳copy過來的檔名不包含路徑,內容還是範本
      }

      /// <summary>
      /// 傳入T,回傳T+N 得到起始時間之後相隔多久的營業日
      /// </summary>
      /// <param name="startDate">T</param>
      /// <param name="interval">N , 單位:天</param>
      /// <returns></returns>
      public static DateTime f_cp_relativedate(DateTime startDate, int interval) {
         DateTime tmp = startDate;
         int workdayCount = 0;

         //ken,本來要用DTS資料表來排除颱風和補上班,但是PB沒這樣寫,而且table設計怪,撈起來很麻煩,就先不做
         DTS dts = new DTS();
         DataTable dt = dts.ListWorkDay(startDate);

         for (int k = 0; k < 31; k++) {
            tmp = startDate.AddDays(k);

            if (tmp.DayOfWeek == DayOfWeek.Saturday || tmp.DayOfWeek == DayOfWeek.Sunday) {
               //jump
            } else {
               workdayCount++;
            }

            if (workdayCount == interval) {
               break;
            }
         }

         return tmp;
      }

      /// <summary>
      /// 檢查路徑是否存在,否的話就create,這個c#有函數,之後拿掉
      /// </summary>
      /// <param name="ls_path"></param>
      public static void f_create_path(string ls_path) {
         bool exists = Directory.Exists(ls_path);

         if (!exists)
            Directory.CreateDirectory(ls_path);
      }

      /// <summary>
      /// 抓當月最後交易日(沒有考慮是否為未來日期)
      /// </summary>
      /// <param name="as_table"></param>
      /// <param name="as_kind_id"></param>
      /// <param name="as_ym">yyyy/MM or yyyyMM</param>
      /// <returns></returns>
      public static DateTime f_get_end_day(string as_table, string as_kind_id, string as_ym) {

         DateTime startDate = DateTime.ParseExact(as_ym.Replace("/", "") + "01", "yyyyMMdd", null);
         DateTime endDate = startDate.AddMonths(1);
         string tmp = "";

         switch (as_table) {
            case "AI2":
               AI2 ai2 = new AI2();
               tmp = ai2.GetMaxDate(as_kind_id + "%", endDate);
               break;
            case "AI3":
               AI3 ai3 = new AI3();
               tmp = ai3.GetMaxDate(as_kind_id + "%", endDate);
               break;
            case "DATE":
               //一般日曆日
               tmp = "";
               break;
         }

         if (!DateTime.TryParseExact(tmp, "yyyyMMdd", null, DateTimeStyles.AllowWhiteSpaces, out DateTime lastTradeDate)) {
            lastTradeDate = endDate.AddDays(-1);
         }

         return lastTradeDate;
      }

      /// <summary>
      /// 上個月的倒數n天交易日
      /// </summary>
      /// <param name="as_table"></param>
      /// <param name="as_kind_id"></param>
      /// <param name="as_ym">yyyy/MM or yyyyMM</param>
      /// <param name="ai_day_cnt"></param>
      /// <returns></returns>
      public static DateTime f_get_last_day(string as_table, string as_kind_id, string as_ym, int ai_day_cnt, string ymformat = "yyyyMM") {

         as_ym = as_ym.Replace("/", "");
         DateTime startDate = DateTime.ParseExact(as_ym + "01", "yyyyMMdd", null);
         string tmp = "";

         switch (as_table) {
            case "AI2":
               AI2 ai2 = new AI2();
               tmp = ai2.GetLastMonthLastTradeDate(as_kind_id, startDate, "D", ai_day_cnt);
               break;
            case "AI3":
               AI3 ai3 = new AI3();
               tmp = ai3.GetLastMonthLastTradeDate(as_kind_id, startDate, ai_day_cnt);
               break;
         }

         if (!DateTime.TryParseExact(tmp, "yyyyMMdd", null, DateTimeStyles.AllowWhiteSpaces, out DateTime lastTradeDate))
            return as_ym.AsDateTime(ymformat);

         return lastTradeDate;
      }

      /// <summary>
      /// 月份完整英文 或 英文月份縮寫
      /// </summary>
      /// <param name="ai_mm"></param>
      /// <param name="as_type">1=英文月份縮寫,other=月份完整英文</param>
      /// <returns></returns>
      public static string f_get_month_eng_name(int ai_mm, string as_type = "0") {
         //c#有函數可以直接輸出月份的完整英文
         DateTime tmp = DateTime.ParseExact(DateTime.Now.Year + ai_mm.ToString().PadLeft(2, '0'), "yyyyMM", null, DateTimeStyles.AllowWhiteSpaces);
         CultureInfo ci = new CultureInfo("en-US");
         var monthEngName = "";

         if (as_type == "1") {
            //ken,要英文月份縮寫應該是用MMM才對,怎麼是擷取前面三個字呢?
            //monthEngName = tmp.ToString("MMM", ci) + ".";
            monthEngName = tmp.ToString("MMMM", ci).Substring(0, 3) + ".";
         } else {
            monthEngName = tmp.ToString("MMMM", ci);
         }
         return monthEngName;
      }

      /// <summary>
      /// 直接呼叫fut.DATE_DIFF_OCF_DAYS
      /// </summary>
      /// <param name="ldt_date"></param>
      /// <param name="day_cnt"></param>
      /// <returns></returns>
      public static DateTime f_get_ocf_next_n_day(DateTime ldt_date, int day_cnt) {
         MOCF mocf = new MOCF();
         DateTime ldt_next_date;
         string ls_symd, ls_eymd;
         if (day_cnt > 0) {
            ls_symd = f_conv_date(ldt_date, 1);
            ls_eymd = relativedate(ldt_date, 30).ToString("yyyyMMdd");
            ldt_next_date = mocf.GetSpecOcfDay(ls_symd, ls_eymd, day_cnt);
         } else {
            ls_symd = relativedate(ldt_date, -30).ToString("yyyyMMdd");
            ls_eymd = f_conv_date(ldt_date, 1);
            day_cnt = day_cnt * -1;
            ldt_next_date = mocf.GetSpecOcfDay2(ls_symd, ls_eymd, day_cnt);
         }

         return ldt_next_date;
      }

      /// <summary>
      /// 檢查統計資料是否轉入完畢,Y=轉完,N=還沒轉完
      /// </summary>
      /// <param name="as_txn_id"></param>
      /// <param name="as_job_type"></param>
      /// <param name="as_value">yyyy/mm/dd</param>
      /// <returns></returns>
      public static string f_get_jsw(string as_txn_id, string as_job_type, string as_value) {
         DateTime.TryParseExact(as_value, "yyyy/MM/dd", null, DateTimeStyles.AllowWhiteSpaces, out DateTime adt_value);

         //判斷最大日期 >= 查詢日期
         JSW jsw = new JSW();
         string tmpDate = jsw.GetMaxDate(as_txn_id, as_job_type);
         DateTime.TryParseExact(tmpDate, "yyyy/MM/dd", null, DateTimeStyles.AllowWhiteSpaces, out DateTime maxDate);

         if (maxDate != DateTime.MinValue) {
            if (maxDate < adt_value)
               return "N";
            else if (maxDate > adt_value)
               return "Y";
         }

         string tmpCount = jsw.GetCount(as_txn_id, as_job_type, "N");
         int.TryParse(tmpCount, out int result);
         if (result > 0)
            return "N";

         return "Y";
      }

      /// <summary>
      /// 檢查批次作業是否都完成,回傳空字串=完成,否則回傳錯誤訊息
      /// </summary>
      /// <param name="as_txn_id"></param>
      /// <param name="as_job_type"></param>
      /// <param name="ai_seq_no"></param>
      /// <param name="adt_value"></param>
      /// <param name="as_market_code"></param>
      /// <param name="as_osw_grp"></param>
      /// <returns></returns>
      public static string f_get_jsw_by_seq(string as_txn_id, string as_job_type,
                                              int ai_seq_no, DateTime adt_value,
                                              string as_market_code, string as_osw_grp) {
         //判斷最大日期 >= 查詢日期
         JSW jsw = new JSW();
         OCFG ocfg = new OCFG();
         string ls_rtn = "";
         int li_fut_seq_no = 0, li_opt_seq_no = 0;

         if (ai_seq_no == 0) {
            if (as_market_code == "0") {
               switch (as_osw_grp) {
                  case "1":
                     li_fut_seq_no = 12;
                     li_opt_seq_no = 22;
                     break;
                  case "5":
                     li_fut_seq_no = 13;
                     li_opt_seq_no = 23;
                     break;
                  case "0":
                  case "7":
                     li_fut_seq_no = 17;
                     li_opt_seq_no = 23;
                     break;
               }
            } else {
               li_fut_seq_no = 19;
               li_opt_seq_no = 29;
            }//if (as_market_code == "0") {

            //get 2 field, 
            //ldt_fut_date=max(JSW_SEQ_NO) of li_fut_seq_no 
            //ldt_opt_date=max(JSW_SEQ_NO) of li_opt_seq_no
            DataTable dt = jsw.ListMaxBySeq(as_txn_id, as_job_type, adt_value, li_fut_seq_no, li_opt_seq_no);
            DateTime futDate = dt.Rows[0][0].AsDateTime();
            DateTime optDate = dt.Rows[0][1].AsDateTime();


            if (futDate == null || futDate < adt_value) {
               string tmpCloseTime = ocfg.GetCloseTime(as_osw_grp, "0");
               return string.Format("期貨作業還未完成({0})批次作業", tmpCloseTime);
            }

            if (optDate == null || optDate < adt_value) {
               string tmpCloseTime = ocfg.GetCloseTime(as_osw_grp, "0");
               return string.Format("選擇權作業還未完成({0})批次作業", tmpCloseTime);
            }

            return "";
         }//if (ai_seq_no == 0) {


         string tmpDate = jsw.GetMaxDate(as_txn_id, as_job_type, ai_seq_no);//yyyy/MM/dd
         DateTime.TryParseExact(tmpDate, "yyyy/MM/dd", null, DateTimeStyles.AllowWhiteSpaces, out DateTime maxDate);
         if (maxDate < adt_value) {
            return string.Format("作業時機未到 (目前日期{0})", tmpDate);
         }

         return ls_rtn;//正常就直接回傳空字串
      }

      /// <summary>
      /// 檢查批次作業是否都完成,回傳空字串=完成,否則回傳錯誤訊息
      /// </summary>
      /// <param name="as_txn_id"></param>
      /// <param name="as_job_type"></param>
      /// <param name="ai_seq_no"></param>
      /// <param name="adt_value"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public static string f_get_jsw_seq(string as_txn_id, string as_job_type,
                                              int ai_seq_no, DateTime adt_value,
                                              string as_market_code) {
         //判斷最大日期 >= 查詢日期
         JSW jsw = new JSW();
         OCFG ocfg = new OCFG();
         string ls_rtn = "";
         int li_fut_seq_no = 0, li_opt_seq_no = 0;
         DataTable dt;
         DateTime ldt_date, ldt_fut_grp5_date, ldt_fut_grp7_date;
         if (ai_seq_no == 0) {
            if (as_market_code == "0") {
               dt = jsw.ListMaxSeq(as_txn_id, as_job_type, adt_value);
               ldt_date = dt.Rows[0][0].AsDateTime();
               ldt_fut_grp5_date = dt.Rows[0][1].AsDateTime();
               ldt_fut_grp7_date = dt.Rows[0][2].AsDateTime();
            } else {
               li_fut_seq_no = 19;
               li_opt_seq_no = 29;
               dt = jsw.ListMaxBySeq(as_txn_id, as_job_type, adt_value, li_fut_seq_no, li_opt_seq_no);
               ldt_date = dt.Rows[0][0].AsDateTime();
               ldt_fut_grp7_date = dt.Rows[0][1].AsDateTime();
               ldt_fut_grp5_date = adt_value;
            }//if (as_market_code == "0") {


            if (ldt_fut_grp5_date == null || ldt_fut_grp5_date < adt_value) {
               ls_rtn = "▲期貨作業還未完成全部收盤批次作業";
            } else {
               if (ldt_fut_grp7_date == null || ldt_fut_grp7_date < adt_value) {
                  ls_rtn = "▲期貨作業還未完成全部收盤批次作業";
               }
            }

            if (ldt_date == null || ldt_date < adt_value) {
               if (ls_rtn != "") {
                  ls_rtn = ls_rtn + "，\r\n";
               }
               ls_rtn = ls_rtn + "▲選擇權作業還未完成全部收盤批次作業";
            }

            return ls_rtn;
         }//if (ai_seq_no == 0) {
         else {
            string tmpDate = jsw.GetMaxDate(as_txn_id, as_job_type, ai_seq_no);//yyyy/MM/dd
            DateTime.TryParseExact(tmpDate, "yyyy/MM/dd", null, DateTimeStyles.AllowWhiteSpaces, out DateTime maxDate);
            if (maxDate < adt_value) {
               return string.Format("(目前日期{0})", tmpDate);
            }
         }

         return ls_rtn;//正常就直接回傳空字串
      }

      /// <summary>
      /// 比對現在時間,如果小於晚上6:15(時段7),則回傳"5", 否則回傳"%"
      /// </summary>
      /// <returns></returns>
      public static string f_get_txn_osw_grp() {

         DataTable dtTemp = new OCFG().ListCloseTime("7");
         DateTime grp7 = dtTemp.Rows[0]["ocfg_close_time"].AsDateTime();
         if (DateTime.Now <= grp7) {
            return "5";
         } else {
            return "%";
         }
      }

      /// <summary>
      /// 先把操作(file)的子menu都enable=false,視需要再enable=true
      /// Call By	: 每個window的Activate
      /// </summary>
      public static void f_menu_file_disable_all() {
         //TODO:ken,這邊是要把功能menuBar的file子menu都disable,然後會連動到imgBtnBar

         //m_frame.m_file.m_insert.enabled = false;
         //m_frame.m_file.m_save.enabled = false;
         //m_frame.m_file.m_delete.enabled = false;

         //m_frame.m_file.m_retrieve.enabled = false;
         //m_frame.m_file.m_run.enabled = false;

         //m_frame.m_file.m_import.enabled = false;
         //m_frame.m_file.m_export.enabled = false;

         //m_frame.m_file.m_print.enabled = false;
      }

      /// <summary>
      /// 數字轉中文(非正統中文字)
      /// </summary>
      /// <param name="value"></param>
      /// <param name="unitEnable"></param>
      /// <returns></returns>
      public static string f_number_to_ch(long value, bool unitEnable = true) {
         string[] number = { "一", "二", "三", "四", "五", "六", "七", "八", "九" };
         string[] unit = { "", "十", "百", "千", "萬", "十", "百", "千", "億", "十", "百", "千", "兆", "十", "百", "千" };
         //string[] number = { "零", "壹", "貳", "叁", "肆", "伍", "陸", "柒", "捌", "玖" };
         //string[] unit = { "", "拾", "佰", "仟", "萬", "拾", "佰", "仟", "億", "拾", "佰", "仟", "兆", "拾", "佰", "仟" };

         string temp = value.ToString();
         string chinese = "";
         if (value > 9999999999999999) {
            throw new Exception("超過千兆");
         }
         if (temp.Substring(0, 1).Equals("-")) {
            chinese = "負的";
            temp = temp.Substring(1, temp.Length - 1);
         }
         if (temp.Substring(0, 1).Equals("+")) {
            chinese = "正的";
            temp = temp.Substring(1, temp.Length - 1);
         }

         for (int k = 0; k < temp.Length; k++) {
            chinese = chinese + number[Convert.ToInt32(temp.Substring(k, 1))];
            if (unitEnable.Equals(true))
               chinese = chinese + unit[(temp.Length - (k + 1)) % 16];
         }

         return chinese;
      }

      /// <summary>
      /// 將日期格式轉換成字串(還會去資料庫取前一日或後一日),之後要改為擴充方法
      /// </summary>
      /// <param name="dateType"></param>
      /// <returns></returns>
      public static string f_ocf_date(int dateType, string dbType = "ci") {
         //0 = OCF_DATE		--->  yyyy/mm/dd
         //1 = OCF_DATE		--->  yyyymmdd
         //2 = OCF_NEXT_DATE	--->  yyyymmdd
         //3 = OCF_DATE		--->  中華民國yy年mm月dd日
         //4 = TODAY			--->  中華民國yy年mm月dd日
         //5 = OCF_PREV_DATE	--->  yyyymmdd
         //6 = OCF_NEXT_DATE	--->  中華民國yy年mm月dd日
         //7 = OCF_NEXT_DATE	--->  string(date)  mode

         string ls_rtn = "";

         OCF ocf = new OCF(dbType);
         BO_OCF boOCF = ocf.GetOCF();
         if (boOCF == null) {
            throw new Exception("交易日期檔(OCF)讀取錯誤!");
         }
         DateTime ldt_date = boOCF.OCF_DATE;
         DateTime ldt_next_date = boOCF.OCF_NEXT_DATE;
         DateTime ldt_prev_date = boOCF.OCF_PREV_DATE;

         TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
         DateTime tmp;

         switch (dateType) {
            //OCF_DATE yyyy/mm/dd
            case 0:
               ls_rtn = ldt_date.ToString("yyyy/MM/dd");
               break;
            //OCF_DATE yyyymmdd
            case 1:
               ls_rtn = ldt_date.ToString("yyyyMMdd");
               break;
            //OCF_NEXT-DATE yyyymmdd
            case 2:
               ls_rtn = ldt_next_date.ToString("yyyyMMdd");
               break;
            //OCF_DATE 轉民國年yyyymmdd
            case 3:
               tmp = ldt_date;
               ls_rtn = string.Format("中華民國 {0} 年 {1} 月 {2} 日",
                                       taiwanCalendar.GetYear(tmp),
                                       tmp.Month.ToString().PadLeft(2, '0'),
                                       tmp.Day.ToString().PadLeft(2, '0'));
               break;
            //TODAY 轉民國年yyyymmdd
            case 4:
               tmp = DateTime.Today;
               ls_rtn = string.Format("中華民國 {0} 年 {1} 月 {2} 日",
                                       taiwanCalendar.GetYear(tmp),
                                       tmp.Month.ToString().PadLeft(2, '0'),
                                       tmp.Day.ToString().PadLeft(2, '0'));
               break;
            //OCF_PREV_DATE yyyymmdd
            case 5:
               ls_rtn = ldt_prev_date.ToString("yyyyMMdd");
               break;
            //OCF_NEXT-DATE 轉民國年yyyymmdd
            case 6:
               tmp = ldt_next_date;
               ls_rtn = string.Format("中華民國 {0} 年 {1} 月 {2} 日",
                                       taiwanCalendar.GetYear(tmp),
                                       tmp.Month.ToString().PadLeft(2, '0'),
                                       tmp.Day.ToString().PadLeft(2, '0'));
               break;
            //OCF_NEXT-DATE 單純轉字串
            case 7:
               ls_rtn = ldt_next_date.ToString();
               break;
         }//switch

         return ls_rtn;
      }//function

      /// <summary>
      /// 轉出週日期區間
      /// 以AI2_YMD為主,以每一周為一筆,輸出欄位yyyyIW/startDate/endDate
      /// </summary>
      /// <param name="is_symd">起始日期</param>
      /// <param name="is_eymd">終止日期</param>
      /// <returns>yyyyIW/startDate/endDate</returns>
      public static DataTable f_week(string is_symd, string is_eymd) {
         DateTime startDate = DateTime.ParseExact(is_symd, "yyyyMMdd", null, DateTimeStyles.AllowWhiteSpaces);
         DateTime endDate = DateTime.ParseExact(is_eymd, "yyyyMMdd", null, DateTimeStyles.AllowWhiteSpaces);

         AI2 ai2 = new AI2();
         //ken,整個語法改寫AI2.ListWeek()
         return ai2.ListWeek(startDate, endDate);

      }

      /// <summary>
      /// 此func 已移至 Form Parent WriteLog()
      /// </summary>
      /// <param name="gs_txn_id"></param>
      /// <param name="as_type"></param>
      /// <param name="as_text"></param>
      /// <returns>正常回傳0,失敗回傳-1</returns>
      public static void f_write_logf(string gs_txn_id, string as_type, string as_text) {
         //try {
         //    as_text = as_text.SubStr(0, 100);//取前100字元

         //    LOGF logf = new LOGF();
         //    logf.Insert(gs_user_id, gs_txn_id, as_text, as_type);
         //    return 0;
         //}
         //catch (Exception ex) {
         //    //寫db log失敗,只好寫入本地端的file
         //    //這段再找時間補
         //    return -1;
         //}
      }
      /// <summary>
      /// 彈出選擇存檔的系統視窗
      /// </summary>
      /// <param name="as_filename">檔名</param>
      /// <returns></returns>
      public static string wf_GetFileSaveName(string as_filename) {

         if (!Directory.Exists(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH)) {
            Directory.CreateDirectory(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH);
         }

         SaveFileDialog dialog = new SaveFileDialog();
         dialog.Title = "請點選儲存檔案之目錄";
         dialog.InitialDirectory = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH;
         dialog.FileName = as_filename;
         dialog.Filter = "Excel(*.*)|*.xlsx;*.xls";
         if (dialog.ShowDialog() == DialogResult.OK) {
            return dialog.FileName;
         }
         return "";
      }

      public static Stream wf_getfileopenname(string as_filename, string fileExtension) {

         if (!Directory.Exists(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH)) {
            Directory.CreateDirectory(GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH);
         }

         OpenFileDialog dialog = new OpenFileDialog();
         dialog.Title = "請點選儲存檔案之目錄";
         dialog.InitialDirectory = GlobalInfo.DEFAULT_REPORT_DIRECTORY_PATH;
         dialog.FileName = as_filename;
         dialog.Filter = fileExtension;
         if (dialog.ShowDialog() == DialogResult.OK) {
            return dialog.OpenFile();
         }
         return null;
      }

      #region pb內建函數
      /// <summary>
      /// 功能得到字符串左部指定個數的字符。
      /// 語法Left(string, n )
      ///參數string：string類型，指定要提取子串的字符串n：long類型，指定子串長度返回值String。
      ///函數執行成功時返回string字符串左邊n個字符，發生錯誤時返回空字符串（""）。如果任何參數的值為NULL，Left()函數返回NULL。
      ///如果n的值大於string字符串的長度，那麼Left()函數返回整個string字符串，但並不增加其它字符。
      /// </summary>
      /// <param name="param">參數string</param>
      /// <param name="length">指定要提取子串的字符串n</param>
      /// <returns></returns>
      public static string Left(string param, int length) {
         string result = param.Substring(0, length);
         return result;
      }
      /// <summary>
      /// 功能從字符串右端取指定個數字符。
      ///語法Right(string, n )
      ///參數string：string類型，指定要提取子串的字符串n：long類型，指定子串長度返回值String。
      ///函數執行成功時返回string字符串右邊n個字符，發生錯誤時返回空字符串（""）。如果任何參數的值為NULL，Right()函數返回NULL。
      ///如果n的值大於string字符串的長度，那麼Right()函數返回整個string字符串，但並不增加其它字符。
      /// </summary>
      /// <param name="param">參數string</param>
      /// <param name="length">指定要提取子串的字符串n</param>
      /// <returns></returns>
      public static string Right(string param, int length) {
         string result = param.Substring(param.Length - length, length);
         return result;
      }
      /// <summary>
      /// 功能取字符串的子串。
      ///語法Mid(string, start {, length } ) 
      ///參數string：string類型，指定要從中提取子串的字符串start：long類型，指定子串第一個字符在string字符串中的位置，第一個位置為1length：long類型，可選項，指定子串的長度返回值String。
      ///函數執行成功時返回string字符串中從start位置開始、長度為length的子串。
      ///如果start參數的值大於string中字符個數，那麼Mid()函數返回空字符串。
      ///如果省略了length參數或length參數的值大於從start開始、string字符串中餘下字符的長度，那麼Mid()函數返回所有餘下的字符。
      ///如果任何參數的值為NULL，Mid()函數返回NULL。
      /// </summary>
      /// <param name="param">指定要從中提取子串的字符串start</param>
      /// <param name="startIndex">指定子串第一個字符在string字符串中的位置</param>
      /// <param name="length">第一個位置為0</param>
      /// <returns></returns>
      public static string Mid(string param, int startIndex, int length) {
         if (length - startIndex == 0) {
            return "";
         }
         string result = param.Substring(startIndex, length - startIndex);
         return result;
      }
      /// <summary>
      /// 功能取字符串的子串。
      ///語法Mid(string, start ) 
      ///參數string：string類型，指定要從中提取子串的字符串start：long類型，指定子串第一個字符在string字符串中的位置，第一個位置為1length：long類型，可選項，指定子串的長度返回值String。
      ///函數執行成功時返回string字符串中從start位置開始、長度為length的子串。如果start參數的值大於string中字符個數，那麼Mid()函數返回空字符串。如果省略了length參數或length參數的值大於從start開始、string字符串中餘下字符的長度，那麼Mid()函數返回所有餘下的字符。
      ///如果任何參數的值為NULL，Mid()函數返回NULL。
      /// </summary>
      /// <param name="param">指定要從中提取子串的字符串start</param>
      /// <param name="startIndex">指定子串第一個字符在string字符串中的位置</param>
      /// <returns></returns>
      public static string Mid(string param, int startIndex) {
         string result = param.Substring(startIndex);
         return result;
      }
      /// <summary>
      /// 功能在一個字符串中查找所包含的另一個字符串的起始位置。
      ///語法Pos(string1, string2 {, start } ) 
      ///參數string1：string類型，指定要從中查找子串string2的字符串string2：string類型，指定要在string1中查找的字符串start：long類型，可選項，指定從string1的第幾個字符開始查找。缺省值為1返回值Long。函數執行成功時返回在start位置後string2在string1中第一次出現的起始位置。如果在string1中按指定要求未找到string2、或start的值超過了string1的長度，那麼Pos()函數返回0。如果任何參數的值為NULL，Pos()函數返回NULL。
      ///用法Pos()函數在字符串查找時區分大小寫，因此，"aa"不匹配"AA"。
      /// </summary>
      /// <param name="str">string類型，指定要從中查找子串param的字符串</param>
      /// <param name="param">指定要在str中查找的字符串</param>
      /// <returns>int</returns>
      public static int Pos(string str, string param) {
         int result = str.IndexOf(param);
         return result;
      }
      /// <summary>
      /// 功能在一個字符串中查找所包含的另一個字符串的起始位置。
      ///語法Pos(string1, string2 {, start } ) 
      ///參數string1：string類型，指定要從中查找子串string2的字符串string2：string類型，指定要在string1中查找的字符串start：long類型，可選項，指定從string1的第幾個字符開始查找。缺省值為1返回值Long。函數執行成功時返回在start位置後string2在string1中第一次出現的起始位置。如果在string1中按指定要求未找到string2、或start的值超過了string1的長度，那麼Pos()函數返回0。如果任何參數的值為NULL，Pos()函數返回NULL。
      ///用法Pos()函數在字符串查找時區分大小寫，因此，"aa"不匹配"AA"。
      /// </summary>
      /// <param name="str">string類型，指定要從中查找子串param的字符串</param>
      /// <param name="param">指定要在str中查找的字符串</param>
      /// <param name="start">指定從str的第幾個字符開始查找。</param>
      /// <returns>int</returns>
      public static int Pos(string str, string param, int start) {
         int result = str.IndexOf(param, start);
         return result;
      }
      /// <summary>
      /// 功能得到字符串的長度。
      ///語法Len(string)
      ///參數string：string類型變量返回值Long。函數執行成功時返回字符串的長度，發生錯誤時返回-1。如果任何參數的值為NULL，則Len()函數返回NULL。
      /// </summary>
      /// <param name="str">string類型</param>
      /// <returns></returns>
      public static int Len(string str) {
         int result = str.Length;
         return result;
      }
      /// <summary>
      /// 功能顯示一個消息對話框。
      /// </summary>
      /// <param name="caption">標題</param>
      /// <param name="text">訊息</param>
      /// <param name="icon">圖示</param>
      public static void messageBox(string caption, string text, MessageBoxIcon icon) {
         MessageBox.Show(text, caption, MessageBoxButtons.OK, icon);
      }
      /// <summary>
      /// 功能得到兩個日期間的天數。
      /// 語法DaysAfter ( date1, date2 ) 
      ///參數date1：​​date類型，指定起始日期date2：date類型，指定終止日期返回值Long。
      ///函數執行成功時得到兩個日期之間的天數。如果date2的日期在date1的前面，那麼DaysAfter()函數返回負值。
      ///如果任何參數的值為NULL，則DaysAfter()函數返回NULL。
      /// </summary>
      /// <param name="date1"></param>
      /// <param name="date2"></param>
      /// <returns>int</returns>
      public static double DaysAfter(DateTime date1, DateTime date2) {
         if (date1 == null || date2 == null)
            return 0;
         TimeSpan ts = date2 - date1;
         return ts.TotalDays;
      }
      /// <summary>
      /// 功能建立一個由指定字符串填充的指定長度的字符串。
      /// 函數執行成功時返回n個字符的字符串，該字符串以參數chars中的字符串重複填充而成。如果參數chars中的字符個數多於n個，那麼使用chars字符串的前n個字符填充函數返回的字符串；
      /// 如果參數chars中的字符個數少於n個，那麼使用chars字符串反复填充，直到返回的字符串長度達到n為止。如果任何參數的值為NULL，Fill()函數返回NULL。
      /// </summary>
      /// <param name="chars">指定用於重複填充的字符串n</param>
      /// <param name="n">指定由該函數返回的字符串的長度返回值String</param>
      /// <returns></returns>
      public static string Fill(string chars, int n) {

         return "";
      }

      /// <summary>
      /// 解密方法
      /// </summary>
      /// <param name="as_data"></param>
      /// <returns></returns>
      public static string f_decode(string as_data) {
         string is_out = "";
         long il_y = 0, il_len;
         byte[] il_bit;

         il_len = Len(as_data);

         for (int i = 0; i < il_len; i += 2) {

            il_bit = Encoding.ASCII.GetBytes(as_data.SubStr(i, 1));
            //取前4位值
            il_y = (il_bit[0] - 64) * 16;
            //取后4位值
            il_bit = Encoding.ASCII.GetBytes(as_data.SubStr(i + 1, 1));
            il_y = il_y + il_bit[0] - 64;
            is_out = is_out + Convert.ToChar(il_y);
         }
         return is_out;
         
      }

      /// <summary>
      /// 呼叫並執行bat
      /// </summary>
      /// <param name="as_txn_id"></param>
      /// <param name="as_module"></param>
      /// <param name="as_user_id"></param>
      /// <returns></returns>
      public static string f_bat_span(string as_txn_id, string as_module, string as_user_id) {
         try {
            SPAN_PATH dao = new SPAN_PATH();
            DataTable dtSpanPath = dao.GetPathByModule(as_module);
            string ls_oper_bat = dtSpanPath.Rows[0]["SPAN_PATH_BAT"].ToString();

            if (string.IsNullOrEmpty(ls_oper_bat)) {
               MessageDisplay.Error("(作業代號：" + as_txn_id + ")無設定執行外部SPAN程式路徑（SPAN_PATH），請聯絡 SPAN 負責人！");
               return "N";
            }
            if (!File.Exists(ls_oper_bat)) {
               MessageDisplay.Error("(作業代號：" + as_txn_id + ")執行檔「" + (ls_oper_bat.Trim()) + "」不存在，請聯絡 SPAN 負責人！");
               return "N";
            }

            //刪*.err,*.flag (delete log)
            string ls_err, ls_flag;
            ls_err = gs_batch_path + as_txn_id + ".log";
            ls_flag = "";
            File.Delete(ls_err);
            //File.Delete(ls_flag);

            //*.Bat 以下指令是確保dos中之上一指令執行完畢繼續下一指令行(dos 為單一視窗),echo XXX
            var processInfo = new ProcessStartInfo(ls_oper_bat);

            processInfo.CreateNoWindow = true;

            processInfo.UseShellExecute = false;

            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);

            process.Start();

            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(error)) {
               MessageDisplay.Error(error);
               MessageDisplay.Error("(作業代號：" + as_txn_id + ")執行「" + (ls_oper_bat.Trim()) + "」失敗，請聯絡 SPAN 負責人！");
               return "N";
            }

            process.Close();

            MessageDisplay.Info("(作業代號：" + as_txn_id + ")已執行「" + (ls_oper_bat.Trim()) + "」，請到「" + (ls_oper_bat.Trim()) + "」查輸出結果！");
            return "Y";
         } catch (Exception ex) {
            throw ex;
         }
      }
      #endregion


   }
}



