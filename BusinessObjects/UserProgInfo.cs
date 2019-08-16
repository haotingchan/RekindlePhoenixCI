using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class UserProgInfo
    {
        public string UserID { get; set; }

        public string TxnID { get; set; }

         public string TxnName { get; set; }    //20190815 中文模糊查詢
   }
}
