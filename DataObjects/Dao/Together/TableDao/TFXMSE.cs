using BusinessObjects;
using OnePiece;
using System;
using System.Data;
using System.Data.Common;

namespace DataObjects.Dao.Together {
    public class TFXMSE:DataGate {

      /// <summary>
      /// save CI.TFXMSE
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateData(DataTable inputData) {

         string sql = @"
SELECT 
    TFXMSE_PID,
    TFXMSE_SID,   
    TFXMSE_SNAME,        
    TFXMSE_RAISE_LIMIT,   
    TFXMSE_FALL_LIMIT,  

    TFXMSE_FUT_XXX,   
    TFXMSE_OPT_XXX,   
    to_date(TFXMSE_W_TIME,'yyyy/mm/dd hh24:mi:ss') as TFXMSE_W_TIME,   
    TFXMSE_W_USER_ID,   
    TFXMSE_SP_W_TIME
FROM CI.TFXMSE  
";
         return db.UpdateOracleDB(inputData , sql);
      }
   }
}