using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PhoenixCI.Shared {
   public interface IXml40xxxData {


      void Export(DataTable dt, string filePath);

   }
}
