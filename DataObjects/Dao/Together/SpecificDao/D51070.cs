using OnePiece;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
   //Winni, 2019/01/02
   public class D51070 {
      private Db db;

      public D51070() {
         db = GlobalDaoSetting.DB;
      }
   
      /// <summary>
      /// 取得grid資料
      /// </summary>
      /// <param name="dbName"></param>
      /// <returns></returns>
      public DataTable ListData(string dbName) {

         //object[] parms = { };
         string sql = "";
         switch (dbName) {
            // 一般 - 期貨
            case "fut":
               sql = @"
SELECT FUT.SLT.SLT_KIND_ID,   
         FUT.SLT.SLT_MAX,   
         FUT.SLT.SLT_MIN,   
         FUT.SLT.SLT_SPREAD,   
         FUT.SLT.SLT_SPREAD_LONG,   
         FUT.SLT.SLT_SPREAD_MULTI,   
         FUT.SLT.SLT_SPREAD_MAX,   
         ' ' as OP_TYPE, 
         FUT.SLT.SLT_VALID_QNTY,   
         FUT.SLT.SLT_PRICE_FLUC  
    FROM FUT.SLT   
ORDER BY slt_kind_id , slt_min , slt_max , slt_spread_long ASC
";
               break;

            // 夜盤 - 期貨
            case "futAh":
               sql = @"
SELECT FUTAH.SLT.SLT_KIND_ID,   
         FUTAH.SLT.SLT_MAX,   
         FUTAH.SLT.SLT_MIN,   
         FUTAH.SLT.SLT_SPREAD,   
         FUTAH.SLT.SLT_SPREAD_LONG,   
         FUTAH.SLT.SLT_SPREAD_MULTI,   
         FUTAH.SLT.SLT_SPREAD_MAX,   
         ' ' as OP_TYPE,   
         FUTAH.SLT.SLT_VALID_QNTY,   
         FUTAH.SLT.SLT_PRICE_FLUC  
    FROM FUTAH.SLT   
ORDER BY slt_kind_id , slt_min , slt_max , slt_spread_long ASC
";
               break;
            // 一般 - 選擇權
            case "opt":
               sql = @"
SELECT SLT_KIND_ID,   
         SLT_MAX,   
         SLT_MIN,   
         SLT_SPREAD,   
         SLT_SPREAD_LONG,   
         SLT_SPREAD_MULTI  , 
         SLT_SPREAD_MAX ,
         ' ' as OP_TYPE,
        SLT_PRICE_FLUC  as SLT_PRICE_FLUC ,
        SLT_VALID_QNTY as SLT_VALID_QNTY  
    FROM opt.SLT   
ORDER BY slt_kind_id , slt_min , slt_max , slt_spread_long ASC
";
               break;
            // 夜盤 - 選擇權
            case "optAh":
               sql = @"
SELECT SLT_KIND_ID,   
         SLT_MAX,   
         SLT_MIN,   
         SLT_SPREAD,   
         SLT_SPREAD_LONG,   
         SLT_SPREAD_MULTI  , 
         SLT_SPREAD_MAX ,
         ' ' as OP_TYPE,
        SLT_PRICE_FLUC  as SLT_PRICE_FLUC ,
        SLT_VALID_QNTY as SLT_VALID_QNTY  
    FROM optAH.SLT   
ORDER BY slt_kind_id , slt_min , slt_max , slt_spread_long ASC
";
               break;
         }

         DataTable dtResult = db.GetDataTable(sql , null);
         return dtResult;
      }

      //更新資料庫
      public DataTable UpdateData(string dbName, decimal slt_spread, decimal slt_spread_multi, decimal slt_spread_max, decimal slt_valid_qnty, string slt_kind_id ) {
         object[] parms = {
               "@dbName",dbName,
               "@slt_spread",slt_spread,
               "@slt_spread_multi",slt_spread_multi,
               "@slt_spread_max",slt_spread_max,
               "@slt_valid_qnty",slt_valid_qnty,
               "@slt_kind_id",slt_kind_id
            };
         string sql = "";
         switch (dbName) {
            // 一般 - 期貨
            case "fut":
               sql = @"
UPDATE FUT.SLT 
SET slt_spread = @slt_spread, slt_spread_multi = @slt_spread_multi, slt_spread_max = @slt_spread_max, slt_valid_qnty = @slt_valid_qnty
WHERE slt_kind_id = @slt_kind_id
";
               break;

            // 夜盤 - 期貨
            case "futAh":
               sql = @"
SELECT FUTAH.SLT.SLT_KIND_ID,   
         FUTAH.SLT.SLT_MAX,   
         FUTAH.SLT.SLT_MIN,   
         FUTAH.SLT.SLT_SPREAD,   
         FUTAH.SLT.SLT_SPREAD_LONG,   
         FUTAH.SLT.SLT_SPREAD_MULTI,   
         FUTAH.SLT.SLT_SPREAD_MAX,   
         ' ' as OP_TYPE,   
         FUTAH.SLT.SLT_VALID_QNTY,   
         FUTAH.SLT.SLT_PRICE_FLUC  
    FROM FUTAH.SLT   
ORDER BY slt_kind_id , slt_min , slt_max , slt_spread_long ASC
";
               break;
            // 一般 - 選擇權
            case "opt":
               sql = @"
SELECT SLT_KIND_ID,   
         SLT_MAX,   
         SLT_MIN,   
         SLT_SPREAD,   
         SLT_SPREAD_LONG,   
         SLT_SPREAD_MULTI  , 
         SLT_SPREAD_MAX ,
         ' ' as OP_TYPE,
        SLT_PRICE_FLUC  as SLT_PRICE_FLUC ,
        SLT_VALID_QNTY as SLT_VALID_QNTY  
    FROM opt.SLT   
ORDER BY slt_kind_id , slt_min , slt_max , slt_spread_long ASC
";
               break;
            // 夜盤 - 選擇權
            case "optAh":
               sql = @"
SELECT SLT_KIND_ID,   
         SLT_MAX,   
         SLT_MIN,   
         SLT_SPREAD,   
         SLT_SPREAD_LONG,   
         SLT_SPREAD_MULTI  , 
         SLT_SPREAD_MAX ,
         ' ' as OP_TYPE,
        SLT_PRICE_FLUC  as SLT_PRICE_FLUC ,
        SLT_VALID_QNTY as SLT_VALID_QNTY  
    FROM optAH.SLT   
ORDER BY slt_kind_id , slt_min , slt_max , slt_spread_long ASC
";
               break;
         }
         DataTable dtResult = db.GetDataTable(sql , parms);
         return dtResult;
      }
   }
}
