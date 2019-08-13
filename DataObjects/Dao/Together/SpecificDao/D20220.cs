using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2018/2/11
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D20220 : DataGate {

      public DataTable ListAll() {

         string sql =
     @"
 SELECT PLT1_PROD_TYPE, 
 		PLT1_PROD_SUBTYPE, 
 		PLT1_QNTY_MIN, 
 		PLT1_QNTY_MAX, 
 		PLT1_MULTIPLE, 
 		PLT1_MIN_NATURE, 
 		PLT1_MIN_LEGAL, 
 		' ' as OP_TYPE
 FROM CI.PLT1
 ORDER BY PLT1_PROD_TYPE, PLT1_PROD_SUBTYPE, PLT1_QNTY_MIN
";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }

      public ResultData updatePLT1(DataTable inputData) {
         string sql = @"
 SELECT PLT1_PROD_TYPE, 
 		  PLT1_PROD_SUBTYPE, 
 		  PLT1_QNTY_MIN, 
 		  PLT1_QNTY_MAX, 
 		  PLT1_MULTIPLE, 
 		  PLT1_MIN_NATURE, 
 		  PLT1_MIN_LEGAL
 FROM CI.PLT1";

         return db.UpdateOracleDB(inputData , sql);
      }

      public int GetDuplicate(string prodtype , string prodsubtype , string qntymin , string qntymax) {
         object[] parms = {
                ":prodtype", prodtype,
                ":prodsubtype",prodsubtype,
                ":qntymin",qntymin,
                ":qntymax",qntymax
         };

         string sql = @"
                        select  
                           plt1_prod_type, 
                           plt1_prod_subtype, 
                           plt1_qnty_min, 
                           plt1_qnty_max
                        from ci.plt1
                        where plt1_prod_type = :prodtype
                        and plt1_prod_subtype = :prodsubtype
                        and plt1_qnty_min = :qntymin
                        and plt1_qnty_max = :qntymax
                       ";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult.Rows.Count;
      }

   }
}
