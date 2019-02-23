using BusinessObjects;
using DataObjects.Dao.Together;
using System;
using System.Globalization;

/// <summary>
/// ken,2018/12/19 翻寫
/// </summary>
namespace PhoenixCI.Shared {
    public class SharedFunction {

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
        /// 寫Log至LOGF
        /// </summary>
        /// <param name="gs_txn_id"></param>
        /// <param name="as_type"></param>
        /// <param name="as_text"></param>
        public static void f_write_logf(string gs_txn_id, string as_type, string as_text) {
            try {
                as_text = as_text.Substring(0, 100);//取前100字元

                LOGF logf = new LOGF();
                logf.Insert(gs_user_id, gs_txn_id, as_text, as_type);

            }
            catch (Exception ex) {
                //寫db log失敗,只好寫入本地端的file
                //這段再找時間補
            }
        }



        /// <summary>
        /// 將日期格式轉換成字串
        /// </summary>
        /// <param name="ai_code"></param>
        /// <returns></returns>
        public static string f_ocf_date(int ai_code) {
            //0  = OCF_DATE			--->  yyyy/mm/dd
            //1  = OCF_DATE			--->  yyyymmdd
            //2  = OCF_NEXT_DATE	--->  yyyymmdd
            //3  = OCF_DATE			--->  中華民國yy年mm月dd日
            //4  = TODAY			--->  中華民國yy年mm月dd日
            //5  = OCF_PREV_DATE	--->  yyyymmdd
            //6  = OCF_NEXT_DATE	--->  中華民國yy年mm月dd日
            //7  = OCF_NEXT_DATE	--->  string(date)  mode

            string ls_rtn = "";


            OCF ocf = new OCF();
            BO_OCF boOCF = ocf.GetOCF();
            if (boOCF == null) {
                throw new Exception("交易日期檔(OCF)讀取錯誤!");
            }
            DateTime ldt_date = boOCF.OCF_DATE;
            DateTime ldt_next_date = boOCF.OCF_NEXT_DATE;
            DateTime ldt_prev_date = boOCF.OCF_PREV_DATE;

            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
            DateTime tmp;

            switch (ai_code) {
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
                                            tmp.Month,
                                            tmp.Day);
                    break;
                //TODAY 轉民國年yyyymmdd
                case 4:
                    tmp = DateTime.Today;
                    ls_rtn = string.Format("中華民國 {0} 年 {1} 月 {2} 日",
                                            taiwanCalendar.GetYear(tmp),
                                            tmp.Month,
                                            tmp.Day);
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
                                            tmp.Month,
                                            tmp.Day);
                    break;
                //OCF_NEXT-DATE 單純轉字串
                case 7:
                    ls_rtn = ldt_next_date.ToString();
                    break;
            }//switch

            return ls_rtn;
        }//function


    }
}



