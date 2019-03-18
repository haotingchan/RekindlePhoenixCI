using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190313,D30508
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30508 : DataGate
   {
      /// <summary>
      /// return BST1_YMD/PDK_NAME/BST1_B_TOT_SEC/BST1_S_TOT_SEC
      /// </summary>
      /// <param name="as_sdate"></param>
      /// <param name="as_edate"></param>
      /// <returns></returns>
      public DataTable GetData(DateTime as_symd, DateTime as_eymd)
      {
         object[] parms = {
            ":as_symd",as_symd.ToString("yyyyMMdd"),
            ":as_eymd",as_eymd.ToString("yyyyMMdd")
            };

         string sql =
             @"
               select BST1_YMD AS BST1_YMD,
                     BST1_KIND_ID,APDK_NAME as PDK_NAME,
                     BST1_B_QNTY_WEIGHT,BST1_B_TOT_SEC,
                     BST1_S_QNTY_WEIGHT,BST1_S_TOT_SEC
                  from ci.BST1,ci.APDK
               where BST1_YMD >=:as_symd 
                  and BST1_YMD <=:as_eymd
                  and BST1_KIND_ID = APDK_KIND_ID
               order by BST1_KIND_ID,BST1_YMD
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
