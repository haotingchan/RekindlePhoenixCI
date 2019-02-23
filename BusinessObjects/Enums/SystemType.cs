using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Enums
{
    public enum SystemType
    {
        /// <summary>
        /// 交易資訊統計系統
        /// </summary>
        CI,
        /// <summary>
        /// 日盤期貨
        /// </summary>
        FutureDay,
        /// <summary>
        /// 日盤選擇權
        /// </summary>
        OptionDay,
        /// <summary>
        /// 夜盤期貨
        /// </summary>
        FutureNight,
        /// <summary>
        /// 夜盤期貨
        /// </summary>
        OptionNight
    }
}
