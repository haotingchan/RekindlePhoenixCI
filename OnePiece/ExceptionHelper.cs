using Sybase.Data.AseClient;
using System;

namespace OnePiece
{
    public class ExceptionHelper
    {
        public static Exception TranformException(Exception ex)
        {
            Exception result = new Exception("錯誤" + Environment.NewLine + ex.Message);

            if (ex is AseException)
            {
                // 例："Attempt to insert duplicate key row in object 'TXN' with unique index 'PK_TXN'\n"
                if (ex.Message.IndexOf("Attempt to insert duplicate key row") != -1)
                {
                    result = new Exception("資料已經存在資料庫無法重複新增" + Environment.NewLine + ex.Message);
                }
                else if(ex.Message.IndexOf("EXECUTE permission denied on object") != -1)
                {
                    result = new Exception("無執行SP的權限" + Environment.NewLine + ex.Message);
                }
            }

            return result;
        }
    }
}