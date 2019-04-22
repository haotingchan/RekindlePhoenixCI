namespace Common.Config {
   public class Database {
      public Database() {
      }

      /// <summary>
      /// 期貨
      /// </summary>
      public DBInfo Futures { get; set; }

      /// <summary>
      /// 選擇權
      /// </summary>
      public DBInfo Options { get; set; }

      /// <summary>
      /// 期貨夜盤
      /// </summary>
      public DBInfo Futures_AH { get; set; }

      /// <summary>
      /// 選擇權夜盤
      /// </summary>
      public DBInfo Options_AH { get; set; }

      /// <summary>
      /// 現貨
      /// </summary>
      public DBInfo Tfxms { get; set; }

      /// <summary>
      /// 交易資訊統計系統(帳號ci)
      /// </summary>
      public DBInfo Ci { get; set; }

      /// <summary>
      /// 交易資訊統計系統(帳號user_ap)
      /// </summary>
      public DBInfo CiUserAp { get; set; }

      /// <summary>
      /// 交易資訊統計系統(帳號fut)
      /// </summary>
      public DBInfo CiFut { get; set; }

      /// <summary>
      /// 交易資訊統計系統(帳號opt)
      /// </summary>
      public DBInfo CiOpt { get; set; }

      /// <summary>
      /// 交易資訊統計系統(帳號futAH)
      /// </summary>
      public DBInfo CiFutAH { get; set; }

      /// <summary>
      /// 交易資訊統計系統(帳號optAH)
      /// </summary>
      public DBInfo CiOptAH { get; set; }

      /// <summary>
      /// 交易資訊統計系統(帳號monit)
      /// </summary>
      public DBInfo CiMonit { get; set; }

      /// <summary>
      /// Pos_Onwer (之後會調整)
      /// </summary>
      public DBInfo POSOnwer { get; set; }
   }
}