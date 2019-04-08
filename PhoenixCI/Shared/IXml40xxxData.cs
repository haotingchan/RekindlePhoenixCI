using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PhoenixCI.Shared {
   public interface IXml40xxxData {

      /// <summary>
      /// 匯出函稿 / 公告
      /// </summary>
      /// <param name="workbook"></param>
      void Export(DataTable source, string filePath);
   }
}
