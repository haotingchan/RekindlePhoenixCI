using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190312,D49080
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D49080 : DataGate
   {
      /// <summary>
      /// return OP_TYPE/TFXMSE_SID/TFXMSE_SNAME/TFXMSE_PID/TFXMSE_RAISE_LIMIT/TFXMSE_FALL_LIMIT/TFXMSE_FUT_XXX/TFXMSE_OPT_XXX/TFXMSE_W_TIME/TFXMSE_W_USER_ID/TFXMSE_SP_W_TIME
      /// </summary>
      /// <param name="as_kind_id"></param>
      /// <param name="as_sdate"></param>
      /// <param name="as_edate"></param>
      /// <returns></returns>
      public DataTable GetData(string as_kind_id, DateTime as_sdate, DateTime as_edate)
      {
         string sql =
             @"
               SELECT ' '  as OP_TYPE,   
                     TFXMSE.TFXMSE_SID,   
                     TFXMSE.TFXMSE_SNAME,   
                     TFXMSE.TFXMSE_PID,   
                     TFXMSE.TFXMSE_RAISE_LIMIT,   
                     TFXMSE.TFXMSE_FALL_LIMIT,   
                     TFXMSE.TFXMSE_FUT_XXX,   
                     TFXMSE.TFXMSE_OPT_XXX,   
                     TFXMSE.TFXMSE_W_TIME,   
                     TFXMSE.TFXMSE_W_USER_ID,   
                     TFXMSE.TFXMSE_SP_W_TIME  
                FROM CI.TFXMSE
                    ";
         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }
   }
}
