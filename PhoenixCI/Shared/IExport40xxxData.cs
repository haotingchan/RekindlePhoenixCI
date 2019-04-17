using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixCI.Shared {
   public interface IExport40xxxData {

      /// <summary>
      /// 匯出函稿 / 公告
      /// </summary>
      /// <param name="workbook"></param>
      ReturnMessageClass Export();

      /// <summary>
      /// 取得資料
      /// </summary>
      /// <returns></returns>
      ReturnMessageClass GetData();

      /// <summary>
      /// 錯誤處理
      /// </summary>
      /// <param name="ex"></param>
      /// <param name="msg"></param>
      void ErrorHandle(Exception ex, ReturnMessageClass msg);

      /// <summary>
      /// 寫錯誤log
      /// </summary>
      /// <param name="msg"></param>
      /// <param name="logType"></param>
      /// <param name="operationType"></param>
      void WriteLog(string msg, string logType = "Info", string operationType = "");
   }
}
