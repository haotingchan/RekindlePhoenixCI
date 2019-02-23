using DataObjects;
using System;
using System.Data;
using System.Windows.Forms;


namespace ActionService
{
   public static class sqlca
   {
      public static int SqlCode { get; set; }
      public static string SQLErrText { get; set; }
      public static string ServerName { get; set; }
   }

   public class CommonFunction
   {

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

      /*******************************
      使用者
      *******************************/
      static string gs_user_id, gs_dpt_id, gs_user_name, gs_dpt_name;

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

      /*******************************
      Path 
      *******************************/
      //AP路徑(application path)
      static string gs_ap_path;
      static string gs_bmp_path;
      static string gs_bmp_folder;
      static string gs_work_path;//本磁碟機
      static string gs_Excel_path;//Excel路徑
      static string gs_SaveReport_path;//報表儲存路徑
      static string gs_bcp_path;//載入Data程式位置
      static string gs_batch_path;//Run Batch的路徑

      /*******************************
      Enviroment 
      *******************************/
      static string gs_screen_type;
      static DateTime gdt_ocf_date;
      #endregion


      public static string f_20110_sp(DateTime ldt_date, string as_txn_id)
      {
         int li_return;
         string ls_prod_type = "M";
         /*******************
         轉統計資料TDT
         *******************/

         //[ken] DECLARE My_SP PROCEDURE FOR ci.sp_U_gen_H_TDT(:ldt_date,:ls_prod_type) USING sqlca;
         //[ken] execute My_SP;

         if (sqlca.SqlCode < 0) {
            MessageBox.Show("執行SP(sp_U_gen_H_TDT(" + ls_prod_type + "))錯誤! " + "sqlca.sqlCode= " +
                            sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return "E";
         }
         else {
            //[ken] Fetch My_SP INTO :li_return;
            //[ken] Close My_SP;
         }
         f_write_logf(gs_txn_id, "E", "執行sp_U_gen_H_TDT(" + ls_prod_type + ")");


         if (as_txn_id == "20110") {
            ls_prod_type = "J";
            //[ken] DECLARE My_SP9 PROCEDURE FOR ci.sp_U_gen_H_TDT(:ldt_date,:ls_prod_type) USING sqlca;
            //[ken] execute My_SP9;

            if (sqlca.SqlCode < 0) {
               MessageBox.Show("執行SP(sp_U_gen_H_TDT(" + ls_prod_type + "))錯誤! " + "sqlca.sqlCode= " +
               sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
               return "E";
            }
            else {
               //[ken] Fetch My_SP9 INTO :li_return;
               //[ken] Close My_SP9;

            }
            f_write_logf(gs_txn_id, "E", "執行sp_U_gen_H_TDT(" + ls_prod_type + ")");


            //JTX 日統計AI2;
            //[ken] DECLARE My_SP6 PROCEDURE FOR ci.sp_U_stt_H_AI2_Day(:ldt_date,:ls_prod_type) USING sqlca;
            //[ken] execute My_SP6;

            if (sqlca.SqlCode < 0) {
               MessageBox.Show("執行SP(sp_U_stt_H_AI2_Day)錯誤! " + "sqlca.sqlCode= " +
               sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
               return "E";
            }
            else {
               //[ken] Fetch My_SP6 INTO :li_return;
               //[ken] Close My_SP6;

            }
            f_write_logf(gs_txn_id, "E", "執行sp_U_stt_H_AI2_Day");
            ;
            //JTX 月統計AI2;
            //[ken] DECLARE My_SP7 PROCEDURE FOR ci.sp_U_stt_H_AI2_Month(:ldt_date,:ls_prod_type) USING sqlca;
            //[ken] execute My_SP7;

            if (sqlca.SqlCode < 0) {
               MessageBox.Show("執行SP(sp_U_stt_H_AI2_Month)錯誤! " + "sqlca.sqlCode= " +
               sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
               return "E";
            }
            else {
               //[ken] Fetch My_SP7 INTO :li_return;
               //[ken] Close My_SP7;

            }
            f_write_logf(gs_txn_id, "E", "執行sp_U_stt_H_AI2_Month");
         }

         /*******************
         轉統計資料AI3
         *******************/
         //[ken] DECLARE My_SP2 PROCEDURE FOR ci.sp_H_stt_AI3(:ldt_date) USING sqlca;
         //[ken] execute My_SP2;

         if (sqlca.SqlCode < 0) {
            MessageBox.Show("執行SP(sp_H_stt_AI3)錯誤! " + "sqlca.sqlCode= " +
            sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return "E";
         }
         else {
            //[ken] Fetch My_SP2 INTO :li_return;
            //[ken] Close My_SP2;

         }
         f_write_logf(gs_txn_id, "E", "執行sp_H_stt_AI3");

         /*******************
         更新AI6 (震幅波動度)
         *******************/

         //[ken] DECLARE My_SP3 PROCEDURE FOR ci.sp_H_gen_AI6(:ldt_date) USING sqlca;
         //[ken] execute My_SP3;

         if (sqlca.SqlCode < 0) {
            MessageBox.Show("執行SP(sp_H_gen_AI6)錯誤! " + "sqlca.sqlCode= " +
            sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return "E";
         }
         else {
            //[ken] Fetch My_SP3 INTO :li_return;
            //[ken] Close My_SP3;

         }
         f_write_logf(gs_txn_id, "E", "執行sp_H_gen_AI6");


         /*******************
         更新AA3
         *******************/

         //[ken] DECLARE My_SP4 PROCEDURE FOR ci.sp_H_upd_AA3(to_char(:ldt_date,"yyyymmdd")) USING sqlca;
         //[ken] execute My_SP4;

         if (sqlca.SqlCode < 0) {
            MessageBox.Show("執行SP(sp_H_upd_AA3)錯誤! " + "sqlca.sqlCode= " +
            sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return "E";
         }
         else {
            //[ken] Fetch My_SP4 INTO :li_return;
            //[ken] Close My_SP4;

         }
         f_write_logf(gs_txn_id, "E", "執行sp_H_upd_AA3");


         /*******************
         更新AI8
         *******************/

         //[ken] DECLARE My_SP5 PROCEDURE FOR ci.sp_H_gen_H_AI8(to_char(:ldt_date,"yyyymmdd")) USING sqlca;
         //[ken] execute My_SP5;

         if (sqlca.SqlCode < 0) {
            MessageBox.Show("執行SP(sp_H_gen_H_AI8)錯誤! " + "sqlca.sqlCode= " +
            sqlca.SqlCode.ToString() + " : " + sqlca.SQLErrText, gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return "E";
         }
         else {
            //[ken] Fetch My_SP5 INTO :li_return;
            //[ken] Close My_SP5;

         }
         f_write_logf(gs_txn_id, "E", "執行sp_H_gen_H_AI8");


         return "";
      }

      private static void f_write_logf(string gs_txn_id, string v1, string v2)
      {
         throw new NotImplementedException();
      }

      public static DataTable f_week(string is_symd, string is_eymd)
      {
         /********************************
         轉出週日期區間
         參數:(1)起始日期
              (2)終止日期
         呼叫來源: 30417
         ********************************/

         DataTable lds_ymd, lds_ymd_d;
         lds_ymd_d = new DataTable();
            //////lds_ymd_d.dataobject = "d_ai2_ymd";
            //////lds_ymd_d.settransobject(sqlca);
            //////lds_ymd_d.retrieve(is_symd, is_eymd);
            lds_ymd = new DataTable();
         //////lds_ymd.dataobject = "d_ai2_week";

         //////long i, j, K, ll_qnty, ll_found;
         //////string ls_brk_no, ls_brk_name, ls_brk_no4, ls_brk_type, ls_ymd, ls_param_key, ls_str;
         //////DateTime ld_ymd, ld_ymd_n;
         ////////日期:週;
         //////lds_ymd_d.insertrow(1);
         //////lds_ymd_d.setitem(1, "ai2_ymd", "20051231");
         //////for(int i = 2;i<= lds_ymd_d.Rows.Count;i++) { 
         //////    ls_ymd = trim(lds_ymd_d.getitemstring(i, "ai2_ymd"));
         //////    ld_ymd = DateTime(left(ls_ymd, 4) + "/" + mid(ls_ymd, 5, 2) + "/" + right(ls_ymd, 2));
         //////    ld_ymd_n = DateTime(left(lds_ymd_d.getitemstring(i - 1, "ai2_ymd"), 4) + "/" + mid(lds_ymd_d.getitemstring(i - 1, "ai2_ymd"), 5, 2) + "/" + right(lds_ymd_d.getitemstring(i - 1, "ai2_ymd"), 2));
         //////    /* 符合下列條件;
         //////       1.最後一筆;
         //////       2.換週 (判斷星期x是否變小);
         //////        3.與下一日期相差7天以上 (for日期99999999) ;
         //////    */
         //////    ;
         //////    if (i == 2 
         //////        || DayNumber(ld_ymd) < DayNumber(ld_ymd_n) 
         //////        || abs(DaysAfter(ld_ymd, ld_ymd_n)) > 6 ){
         //////        lds_ymd.insertrow(0);
         //////        lds_ymd.setitem(lds_ymd.Rows.Count, "ai2_ymd", ls_ymd);
         //////    }
         //////    lds_ymd.setitem(lds_ymd.Rows.Count, "ymd_end", ls_ymd);
         //////}//for;

         return lds_ymd;

      }


      public static string f_chk_130_wf(string as_txn_id, DateTime adt_date, string os_osw_grp)
      {
         /***************************************
         有加f_chk_130的作業代號：
         42010, 42011, 42020, 42030 
         40011, 40021,40012
         40013
         43010, 43020, 43030
         40010, 40020
         ***************************************/

         //////int li_rtn;
         //////string ls_osw_grp = "";
         //////int li_cnt_start, li_cnt_end, i;

         //////switch (as_txn_id) {
         //////    case "40010":
         //////    case "40020":
         //////    case "40011":
         //////    case "40021":
         //////    case "40012":
         //////    case "40013":

         //////        if (as_txn_id == "40010" || as_txn_id == "40020" || as_txn_id == "40011") {
         //////            ls_osw_grp = "1";
         //////        } else if (as_txn_id == "40021" || as_txn_id == "40012") {
         //////            ls_osw_grp = "5";
         //////        } else if (as_txn_id == "40013") {
         //////            ls_osw_grp = "7";
         //////        }
         //////        string sql = @"select count(*)  into: li_rtn
         //////                from
         //////                (select JLOG_WORKFLOW
         //////                from CI.JLOG
         //////                where JLOG_DATE >= :adt_date
         //////                and JLOG_WORKFLOW in ('wf_FB_AI0130C', 'wf_OB_AI0130C')
         //////                and JLOG_ID = ('AI0130C')
         //////                and JLOG_OSW_GRP = :ls_osw_grp
         //////                group by JLOG_WORKFLOW)";

         //////        object[] parms = { ":ls_osw_grp", ls_osw_grp };
         //////        OnePiece.Db db = GlobalDaoSetting.DB;
         //////        li_rtn = db.ExecuteSQL(sql, parms);


         //////        if (li_rtn < 2) {
         //////            return "機房「130C」批次作業尚未完成";
         //////        } else {
         //////            return "";
         //////        }
         //////        break;


         //////    case "43010":
         //////    case "43020":

         //////        switch (mid(os_osw_grp, 1, 1)) {
         //////            case "%":
         //////                li_cnt_start = 1;
         //////                li_cnt_end = 2;
         //////                break;
         //////            case "1":
         //////                li_cnt_start = 1;
         //////                li_cnt_end = 1;
         //////                break;
         //////            case "5":
         //////                li_cnt_start = 2;
         //////                li_cnt_end = 2;
         //////                break;
         //////        }//switch


         //////        for (int i = li_cnt_start;i <= li_cnt_end;i++) {

         //////            ls_osw_grp = (i == 1 ? "1" : "5");

         //////            string sql = @"select count(*)  into :li_rtn 
         //////                          from (select JLOG_WORKFLOW 
         //////                                from CI.JLOG
         //////                                where JLOG_DATE >= :adt_date
         //////                                and JLOG_WORKFLOW in ('wf_FB_AI0130C','wf_OB_AI0130C')
         //////                                and JLOG_ID = ('AI0130C')
         //////                                and JLOG_OSW_GRP = :ls_osw_grp
         //////                                group by JLOG_WORKFLOW)";

         //////            object[] parms = { ":ls_osw_grp", ls_osw_grp };
         //////            OnePiece.Db db = GlobalDaoSetting.DB;
         //////            li_rtn = db.ExecuteSQL(sql, parms);

         //////            if (li_rtn < 2) {
         //////                return "機房「130C」批次（群組:" + i.ToString() + "）作業尚未完成";
         //////            }
         //////        }//for
         //////        return "";
         //////        break;
         //////    case "40030":

         //////        switch (mid(os_osw_grp, 1, 1)) {
         //////            case "%":
         //////                li_cnt_start = 1;
         //////                li_cnt_end = 3;
         //////                break;
         //////            case "1":
         //////                li_cnt_start = 1;
         //////                li_cnt_end = 1;
         //////                break;
         //////            case "5":
         //////                li_cnt_start = 2;
         //////                li_cnt_end = 2;
         //////                break;
         //////            case "7":
         //////                li_cnt_start = 3;
         //////                li_cnt_end = 3;
         //////                break;
         //////        }
         //////        for (int i = li_cnt_start;i <= li_cnt_end;i++) {
         //////            switch (i) {
         //////                case 1:
         //////                    ls_osw_grp = "1";
         //////                    break;
         //////                case 2:
         //////                    ls_osw_grp = "5";
         //////                    break;
         //////                case 3:
         //////                    ls_osw_grp = "7";
         //////                    break;
         //////            }

         //////            string sql = @"select count(*)  into :li_rtn 
         //////                          from (select JLOG_WORKFLOW 
         //////                                from CI.JLOG
         //////                                where JLOG_DATE >= :adt_date
         //////                                and JLOG_WORKFLOW in ('wf_FB_AI0130C','wf_OB_AI0130C')
         //////                                and JLOG_ID = ('AI0130C')
         //////                                and JLOG_OSW_GRP = :ls_osw_grp
         //////                                group by JLOG_WORKFLOW)";

         //////            object[] parms = { ":ls_osw_grp", ls_osw_grp };
         //////            OnePiece.Db db = GlobalDaoSetting.DB;
         //////            li_rtn = db.ExecuteSQL(sql, parms);

         //////            if ((li_rtn < 2 && ls_osw_grp <> "7") || (li_rtn < 1 && ls_osw_grp == "7")) {
         //////                return "機房「130C」批次（群組:" + i.ToString() + "）作業尚未完成";
         //////            }
         //////        }//for    ;

         return "";
      }
      public static string f_chk_ai2(string as_txn_id, string ls_ymd, string is_osw_grp, string ls_choose)
      {
         /***************************************
         有加f_chk_ai2的作業代號：
         40030,40040
         ***************************************/

         //////int li_cnt;
         //////string ls_grp;

         ////////Group1;
         //////if (is_osw_grp == "1" || is_osw_grp == "%") {
         //////    ls_grp = "1";
         //////    //select count(distinct AI2_PROD_TYPE);
         //////    //into: li_cnt;
         //////    //from ci.AI2,ci.APDK;
         //////    //where AI2_YMD = :ls_ymd;
         //////    //and AI2_SUM_TYPE = "D";
         //////    //and AI2_PROD_TYPE in ("F", "O");
         //////    //and AI2_KIND_ID = APDK_KIND_ID;
         //////    //and APDK_MARKET_CLOSE like :ls_grp||'%'


         //////    if (li_cnt < 2) {
         //////        if (ls_choose == "N") {
         //////            MessageBox.Show("每日行情統計檔(AI2)－Group1 轉檔未完成，請稍候再執行! ", gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////            //洽OP：盤後資訊管理系統「(F)10012作業第5項&(O)10022作業第5項」是否已完成?;
         //////            return "E";
         //////        } else {
         //////            if (MessageBox.Show(gs_t_question, "每日行情統計檔(AI2)－Group1 轉檔未完成，是否要繼續? ", Question!, YesNo!, 2) = 2) {
         //////                return "E";
         //////            }
         //////        }
         //////    }
         //////}//if (is_osw_grp == "1" || is_osw_grp == "%") {

         ////////Group5;
         //////if (is_osw_grp == "5" || is_osw_grp == "%") {

         //////    ls_grp = "5";
         //////    //select count(distinct AI2_PROD_TYPE);
         //////    //into: li_cnt;
         //////    //from ci.AI2,ci.APDK;
         //////    //where AI2_YMD = :ls_ymd;
         //////    //and AI2_SUM_TYPE = "D";
         //////    //and AI2_PROD_TYPE in ("F", "O");
         //////    //and AI2_KIND_ID = APDK_KIND_ID;
         //////    //and APDK_MARKET_CLOSE like :ls_grp||"%";
         //////    ;
         //////    ;
         //////    if (li_cnt < 1) {
         //////        if (ls_choose == "N") {
         //////            MessageBox.Show("每日行情統計檔(AI2)－Group2 轉檔未完成，請稍候再執行! ", gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////            //洽OP：盤後資訊管理系統「(F)10013作業第5項」是否已完成?;
         //////            return "E";
         //////        } else {
         //////            if (MessageBox.Show(gs_t_question, "每日行情統計檔(AI2)－Group2 轉檔未完成，是否要繼續? ", Question!, YesNo!, 2) = 2) {
         //////                return "E";
         //////            }
         //////        }
         //////    }
         //////}//if (is_osw_grp == "5" || is_osw_grp == "%") {
         return "";
      }


      public static string f_chk_mg5(string as_txn_id, string ls_ymd, string is_osw_grp, string ls_choose)
      {
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


      public static int f_chk_new_etf()
      {
         DateTime ld_date;
         int li_cnt=0;
         ld_date = DateTime.Today;

         //select count(*) into: li_cnt
         //from ci.TFXMSE
         //where TFXMSE_W_USER_ID is null


         //紀錄log
         //////f_write_logf("49080", "C", "檢核今日有新增ETF標的警示");
         //////;
         //////if (li_cnt > 0) {
         //////    MessageBox.Show(gs_t_warning, "今日有新增ETF標的，請至「49080」確認基本資料設定＆執行「存檔」功能！", Exclamation!);
         //////}
         ;
         return li_cnt;
      }


      //////public static DateTime f_get_month_relativedate(DateTime ad_date, int ai_value)
      //////{
      //////   DateTime ld_date;
      //////   ad_date = relativedate(ad_date, 1);
      //////   //select add_months(:ad_date, :ai_value) into: ld_date FROM DUAL



      //////   //add_month相減後無當天日期,會往前找最近的日期,所以往後加1天;
      //////   if (month(ad_date) - month(ld_date) + ((year(ad_date) - year(ld_date)) * 12) >= abs(ai_value) and day(ld_date) < day(ad_date)    ){
      //////      ld_date = relativedate(ld_date, 1);
      //////   }
      //////   //add_month會有日期差異,ex:2012/2/29 - 3個月 = 2011/11/30,再減去日期差異-->2011/11/29;
      //////   if (day(ld_date) - day(ad_date) > 0) {
      //////      ld_date = relativedate(ld_date, day(ad_date) - day(ld_date));
      //////   }
      //////   ;
      //////   return ld_date;
      //////}


      public static int f_cncharnum(string astring)
      {
         /********************************
         函名: f_cncharnum
         用途: 返回字串中中文字
         輸入: aString - string, 給定的字符串
         返回值: li_num - int, 中文個數
         注意: 1. 不符合編碼的無效！
               2. 若字串含有非中文字,如圖形符號或ASCII碼,則保持不變.
         例如: li_ret = f_cncharnum("測試ferryman") li_ret = 2
         ********************************/


         string ls_ch;
         string ls_SecondSecTable;
         int li_num = 0; //返回值
         int i, j;

         //////for (int i = 0;i <= Len(aString);i++) {
         //////    ls_ch = Mid(aString, i, 1);
         //////    if (Asc(ls_ch) >= 128) { //是中文字
         //////        li_num++;
         //////        i = i + 1;
         //////    }
         //////}//for;

         return li_num;
      }


      public static void f_make_path_arg(string ls_path)
      {
         /**********************************************************************
         Name        : f_make_path
         Call By    : 無
         功能        : 設定目錄

                       以當營業日為主目錄存放報表檔
                    建立目錄 = gs_work_path + gs_sys +"\"+ ls_today   + "\"
                                  "c:\CI\20050101\"
                       .bat檔案名稱 = gs_work_path + trim(sqlca.servername)+"_"+"mk.bat"    
                                      "c:\CI_mk.bat"    
         **********************************************************************/

         //////boolean lb_exist;
         //////string ls_bat_f;
         //////int li_filenum;

         ////////ls_today = f_ocf_date(1)  ;
         ////////ls_path  = gs_work_path + gs_sys +"\"+ ls_today + "\";
         //////ls_bat_f = gs_work_path + trim(sqlca.servername) + "_" + "mk.bat";

         //////int li_pos;
         //////int li_last_pos = 1;
         //////string ls_folder, ls_folder_last, a;

         //////if (!System.IO.File.Exists(ls_path)) {

         //////    //檔案清空
         //////    do until li_FileNum > 0  ;
         //////    li_FileNum = FileOpen(ls_bat_f, lineMode!, Write!, LockWrite!, replace!);
         //////    loop;

         //////    //建立Path;
         //////    li_last_pos = 3;
         //////    ls_folder_last = mid(ls_path, 1, li_last_pos - 1);
         //////    filewrite(li_FileNum, ls_folder_last);
         //////    filewrite(li_FileNum, "cd " + ls_folder_last + @"\");

         //////    filewrite(li_FileNum, @"cd \");


         //////    ChangeDirectory(ls_folder_last);
         //////    a = GetCurrentDirectory();
         //////    do {

         //////        li_pos = pos(ls_path, @"\", li_last_pos + 1);
         //////        if (li_pos > 0) {

         //////            ls_folder = mid(ls_path, li_last_pos + 1, li_pos - li_last_pos - 1);
         //////            ls_folder_last = ls_folder_last + @"\" + ls_folder;
         //////            if (!System.IO.File.Exists(ls_folder_last)) {

         //////                filewrite(li_FileNum, "md " + ls_folder);
         //////                CreateDirectory(ls_folder);
         //////                ChangeDirectory(ls_folder);
         //////            }
         //////            filewrite(li_FileNum, "cd " + ls_folder);
         //////            ChangeDirectory(ls_folder);
         //////            li_last_pos = li_pos;
         //////        } while (li_pos > 0) ;

         //////        //[ken] Close(li_FileNum);

         //////        li_pos = run(ls_bat_f, Minimized!);
         //////        sleep(5);
         //////        //MessageBox.Show("產生資料夾error "+ls_bat_f);
         //////        if (!System.IO.File.Exists(ls_path)) {

         //////            MessageBox.Show(gs_t_warning + " (f_make_path)", "無法建立新資料夾「" + ls_path + "」,請手動執行「" + ls_bat_f + "」(檔案Double Click)", Exclamation!);
         //////        }

         //////    }


      }


      public static string f_bat_span()
      {
         return "";
      }

      public static string f_bat_span(string as_txn_id, string as_module, string as_user_id)
      {
         ////////SPAN_PATH_BAT 
         //////string ls_oper_bat, ls_output;
         ////////    select SPAN_PATH_BAT, SPAN_PATH_VALUE
         ////////    into: ls_oper_bat,:ls_output
         ////////from cfo.SPAN_PATH
         ////////where SPAN_PATH_MODULE = :as_module;


         //////if (isnull(ls_oper_bat) || trim(ls_oper_bat) == "") {
         //////   MessageBox.Show(gs_t_err + "(作業代號：" + as_txn_id + ")", "無設定執行外部SPAN程式路徑（SPAN_PATH），~r~n請聯絡 SPAN 負責人！", gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////   return "N";
         //////};
         //////if (!System.IO.File.Exists(ls_oper_bat)) {
         //////   MessageBox.Show(gs_t_err + "(作業代號：" + as_txn_id + ")", "執行檔「" + trim(ls_oper_bat) + "」不存在，~r~n請聯絡 SPAN 負責人！", gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////   return "N";
         //////}

         ///////*******************
         //////刪*.err,*.flag
         //////*******************/

         //////string ls_err, ls_flag;
         //////ls_err = gs_batch_path + as_txn_id + ".log";
         //////ls_flag = "";
         //////filedelete(ls_err);
         //////filedelete(ls_flag);

         ///////****************************************** 
         //////*.Bat
         //////以下指令是確保dos中之上一指令執行完畢
         //////繼續下一指令行(dos 為單一視窗),echo XXX 
         //////******************************************/
         //////;
         //////int k;
         //////k = run(ls_oper_bat + " " + as_user_id + " > " + ls_err, Minimized!);
         //////if (k <> 1) {
         //////   beep(10);
         //////   MessageBox.Show(gs_t_result + "(作業代號：" + as_txn_id + ")", "執行「" + trim(ls_oper_bat) + "」失敗，~r~n請聯絡 SPAN 負責人！", gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);
         //////   return "E";
         //////}

         //////MessageBox.Show(gs_t_result + "(作業代號：" + as_txn_id + ")", "已執行「" + trim(ls_oper_bat) + "」，請到「" + ls_output + "」查輸出結果", gs_t_err, MessageBoxButtons.OK, MessageBoxIcon.Stop);

         return "";
      }

      public static string f_getstring(string a_string, string a_char, string a_path)
      {
         int i, li_pos;
         string ls_str="";
         //////i = 1;
         //////switch (a_path) {
         //////   case "F":   //找到是屬於 option || future || comm ||  || other
         //////      i = pos(a_string, a_char, i);
         //////      ls_str = mid(a_string, 1, i - 1);
         //////      break;
         //////   case "B":
         //////      while (i != 0) {
         //////         i = pos(a_string, a_char, i);
         //////         if (i != 0) {
         //////            li_pos = i;
         //////            i++;
         //////         }
         //////      }// while (i != 0) {

         //////      ls_str = mid(a_string, li_pos + 1, (len(a_string) - li_pos));
         //////      break;
         //////}//switch (a_path) { 
         return ls_str;

      }
   }
}

