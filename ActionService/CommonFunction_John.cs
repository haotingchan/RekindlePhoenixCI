using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ActionService
{
   public class CommonFunction_John
   {
      /// <summary>
      /// alarm音效
      /// </summary>
      public static void f_alarm_sound()
      {
         SoundPlayer lui_NumDevs;
         string ls_SoundFile;
         string gs_excel_path = GlobalInfo.DEFAULT_EXCEL_TEMPLATE_DIRECTORY_PATH;
         ls_SoundFile = Path.Combine(gs_excel_path, "alarm_bb.wav");// 音效檔檔名
         if (File.Exists(ls_SoundFile)) { // 檢查是否有音效裝置
            lui_NumDevs = new SoundPlayer(ls_SoundFile);
            lui_NumDevs.LoadAsync();// 非同步播放一次
         }
      }

   }
}
