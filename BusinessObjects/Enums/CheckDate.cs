using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Enums
{
   /// <summary>
   /// 驗證起迄日所顯示的文字
   /// </summary>
   public class CheckDate
   {
      public const string Start = "起始日期輸入錯誤!";
      public const string End  = "迄止日期輸入錯誤!";
      public const string Datedif= "起始日期不可大於迄止日期!";
   }
}
