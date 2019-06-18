using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Winni, 2019/04/16
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   public class D49080 : DataGate {
      /// <summary>
      /// get CI.TFXMSE data retuen 11 field (d_49080)
      /// </summary>
      /// <returns></returns>
      public DataTable GetDataList() {
         string sql = @"
SELECT 
    TFXMSE_PID,
    TFXMSE_SID,   
    TFXMSE_SNAME,        
    TFXMSE_RAISE_LIMIT,   
    TFXMSE_FALL_LIMIT,  

    TFXMSE_FUT_XXX,   
    TFXMSE_OPT_XXX,
    TFXMSE_W_TIME,
    --to_char(TFXMSE_W_TIME,'yyyy/mm/dd hh24:mi:ss') as TFXMSE_W_TIME,   
    TFXMSE_W_USER_ID,   
    TFXMSE_SP_W_TIME,

    ' ' AS IS_NEWROW
FROM CI.TFXMSE   
ORDER BY TFXMSE_PID ,TFXMSE_SID
";
         DataTable dtResult = db.GetDataTable(sql , null);

         return dtResult;
      }
   }
}
