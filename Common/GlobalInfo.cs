using BusinessObjects;
using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public class GlobalInfo {
        public static string USER_ID;

        public static string USER_NAME;

        public static string USER_DPT_ID;

        public static string USER_DPT_NAME;

        public static DateTime OCF_DATE;

        public static DateTime OCF_NEXT_DATE;

        public static DateTime OCF_PREV_DATE;

        public static string DEFAULT_REPORT_DIRECTORY_PATH;

        public static string DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH;

        public static string DEFAULT_BATCH_ErrSP_DIRECTORY_PATH;

        public static string SYS = "CI";
        public static string ResultText = "處理結果";  //Information!
        public static string WarningText = "警告訊息";    //Exclamation!
        public static string ErrorText = "錯誤訊息";//StopSign!
        public static string QuestionText = "請選擇";       //Question!
        public static string MsgOK = "處理完成";
        public static string MsgNoData = "無此筆資料!";
        public static string MsgFieldError = "欄位資料輸入錯誤!";
        public static string MsgNoAuth = "無此權限，執行此交易";
        public static string MsgNotAllowExec = "時點不允許執行此交易,視窗即將關閉.";

        /// <summary>
        /// 存放像是FO或OO
        /// </summary>
        private static string _SYSTEM_ALIAS;

        public static string SYSTEM_ALIAS {
            get {
                switch (SystemStatus.SystemType) {
                    case SystemType.CI:
                        _SYSTEM_ALIAS = "CI";
                        break;
                    default:
                        break;
                }

                return _SYSTEM_ALIAS;
            }
        }

        /// <summary>
        /// 用來放標題文字的備註，像是(夜盤)這樣的文字，如果是日盤目前就是空白
        /// </summary>
        public static string REPORT_TITLE_MEMO;
    }
}
