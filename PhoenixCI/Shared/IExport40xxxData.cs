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

   }
}
