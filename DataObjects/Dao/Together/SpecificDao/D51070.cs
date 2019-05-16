using BusinessObjects;
using OnePiece;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// winni , 2019/5/15
   /// </summary>
   public class D51070 : DataGate {
      /// <summary>
      /// get fut.slt or futah.slt or opt.slt or optah.slt data return 10 field
      /// </summary>
      /// <returns></returns>
      public DataTable ListData(string ls_dw_name) {

         string dbName = "";
         switch (ls_dw_name) {
            case "_fut":
               dbName = "fut.slt";
               break;
            case "_fut_AH":
               dbName = "futah.slt";
               break;
            case "_opt":
               dbName = "opt.slt";
               break;
            case "_opt_AH":
               dbName = "optah.slt";
               break;
         }

         string sql = string.Format(@"
select 
    slt_kind_id,   
    slt_max,   
    slt_min,   
    slt_spread,   
    slt_spread_long,   
    slt_spread_multi,   
    slt_spread_max,   
    ' ' as op_type,   
    slt_valid_qnty,   
    slt_price_fluc  
from {0}   
order by slt_kind_id , slt_min , slt_max , slt_spread_long 
" , dbName);
         DataTable result = db.GetDataTable(sql , null);

         return result;
      }

      /// <summary>
      /// save data
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData , string ls_dw_name) {

         string dbName = "";
         switch (ls_dw_name) {
            case "_fut":
               dbName = "fut.slt";
               break;
            case "_fut_AH":
               dbName = "futah.slt";
               break;
            case "_opt":
               dbName = "opt.slt";
               break;
            case "_opt_AH":
               dbName = "optah.slt";
               break;
         }

         string sql = string.Format(@"
select 
    slt_kind_id,   
    slt_max,   
    slt_min,   
    slt_spread,   
    slt_spread_long,   
    slt_spread_multi,   
    slt_spread_max,   
    slt_valid_qnty,   
    slt_price_fluc  
from {0}   
", dbName);

         return db.UpdateOracleDB(inputData , sql);
      }
   }
}
