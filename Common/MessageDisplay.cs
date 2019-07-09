using System.Windows.Forms;
using static Common.MessageBoxTimeOut;

namespace Common {
   public class MessageDisplay {
      public const string MSG_NO_PERMISSION = "時點不允許執行此交易";
      public const string MSG_NO_DATA_FOR_MODIFY = "無資料可以異動";
      public const string MSG_NO_DATA = "無任何資料!";
      public const string MSG_NO_DATA_MULTI = "{0},{1} ─ {2},讀取無任何資料!";//{條件},{報表代號或function名稱},{報表名稱}
        public const string MSG_OK = "處理完成";
      public const string MSG_PRINT = "列印完成";
      public const string MSG_IMPORT = "轉檔完成";
      public const string MSG_IMPORT_FAIL = "轉檔失敗";

      public static DialogResult Normal(string content, string caption = "訊息") {
         return MessageBox.Show(content, caption);
      }

      public static DialogResult Error(string content, string caption = "注意") {
         return MessageBox.Show(content, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      public static DialogResult Warning(string content, string caption = "注意") {

         return MessageBox.Show(content, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }

      public static DialogResult Info(string content, string caption = "注意") {
#if DEBUG
         TimerThread tt = new TimerThread(2000);
         try {
            return MessageBox.Show(content, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
         } finally {
            tt.Terminate();
         }
#else
            return MessageBox.Show(content, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
      }

      public static DialogResult Choose(string content,MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button2,string caption = "注意") {
         return MessageBox.Show(content, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Information, defaultButton);
      }

      /// <summary>
      /// 紀錄處理完成的狀態，如果是MessageDisplay.MSG_OK以外的訊息則為空值
      /// </summary>
      protected string _msg;
      /// <summary>
      /// show文字訊息，紀錄處理完成的狀態，如果是MessageDisplay.MSG_OK以外的訊息則為空值
      /// </summary>
      public string OutputShowMessage {
         set {
            if (value != MSG_OK)
               Info(value);
            else
               _msg = value;
         }
         get {
            return _msg;
         }
      }

   }
}