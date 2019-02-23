using System.ComponentModel;
/// ken,2018/12/27

/// <summary>
/// 幣別縮寫與對應資料庫數字,如果要顯示中文則用反射找Description
/// </summary>
public enum CurrencyType {
    /// <summary>
    /// 元
    /// </summary>
    [Description("元")]
    TWD = 1,

    /// <summary>
    /// 美元
    /// </summary>
    [Description("美元")]
    USD = 2,

    /// <summary>
    /// 日元
    /// </summary>
    [Description("日元")]
    JPY = 4,

    /// <summary>
    /// 人民幣
    /// </summary>
    [Description("人民幣")]
    CNY = 8
}

