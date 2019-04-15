using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixCI.Shared {
   public class ReturnMessageClass {
      public string ReturnMessage { get; set; }
      public bool ShowMessage { get; set; }
      public ResultStatus Status { get; set; }

      public ReturnMessageClass() { }

      public ReturnMessageClass(string returnmessage, bool showmessage = true) {
         ReturnMessage = returnmessage;
         ShowMessage = showmessage;
      }
   }
}
