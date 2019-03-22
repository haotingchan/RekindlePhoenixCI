using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190320,D43030
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D43030 : DataGate
   {
      /// <summary>
      /// return 
      /// seq_no/MGR5E_SID/TFXMSE_SNAME/PID_NAME/MGR5E_CP_RISK/MGR5E_RISK/
      /// MGR5E_FUT_xxx/MGR5E_FUT_CM/MGR5E_FUT_MM/MGR5E_FUT_IM/MGR5E_CM_RATE/MGR5E_MM_RATE/MGR5E_IM_RATE/KIND_ID/APDK_NAME/MGR5E_FUT_KIND_ID/
      /// MGR5E_CP_STATUS_CODE/KIND_SEQ_NO
      /// </summary>
      /// <param name="as_sdate"></param>
      /// <param name="as_edate"></param>
      /// <returns></returns>
      public DataTable GetListF(string as_ymd)
      {
         object[] parms = {
            ":as_ymd",as_ymd
            };

         string sql =
             @"
               SELECT rownum as seq_no,
                     MGR5E_SID,   
                     CI.TFXMSE.TFXMSE_SNAME,   
                     PID_NAME, --MGR5E_PID,   
                     MGR5E_CP_RISK,   
                     MGR5E_RISK,   
                     case when nvl(MGR5E_FUT_KIND_ID,' ') = ' ' then MGR5E_FUT_xxx else MG1_XXX end as MGR5E_FUT_xxx,   
                     case when nvl(MGR5E_FUT_KIND_ID,' ') = ' ' then MGR5E_FUT_CM else MG1_CM end as MGR5E_FUT_CM,   
                     case when nvl(MGR5E_FUT_KIND_ID,' ') = ' ' then MGR5E_FUT_MM else MG1_MM end as MGR5E_FUT_MM,   
                     case when nvl(MGR5E_FUT_KIND_ID,' ') = ' ' then MGR5E_FUT_IM else MG1_IM end as MGR5E_FUT_IM,   
                     case when nvl(MGR5E_FUT_KIND_ID,' ') = ' ' then MGR5E_CM_rate else MG1_CM_RATE end as MGR5E_CM_RATE,   
                     case when nvl(MGR5E_FUT_KIND_ID,' ') = ' ' then MGR5E_MM_rate else MG1_MM_RATE end as MGR5E_MM_RATE,   
                     case when nvl(MGR5E_FUT_KIND_ID,' ') = ' ' then MGR5E_IM_rate else MG1_IM_RATE end as MGR5E_IM_RATE,   
                     case when nvl(MGR5E_FUT_KIND_ID,' ') = ' ' then ' '  else MG1_KIND_ID end as KIND_ID,
                     case when nvl(MGR5E_FUT_KIND_ID,' ') = ' ' then ' '  else APDK_NAME end as APDK_NAME,
                     case when nvl(MGR5E_FUT_KIND_ID,' ') = ' ' then ' '  else 'Y' end as MGR5E_FUT_KIND_ID,   
                     MGR5E_CP_STATUS_CODE,
                     M.seq_no as KIND_SEQ_NO
                FROM ci.MGR5E,   
                     ci.TFXMSE ,
                    (select APDK_STOCK_ID,MG1_KIND_ID,APDK_NAME,MG1_XXX,MG1_CM,MG1_MM,MG1_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                            ROW_NUMBER( ) OVER (PARTITION BY APDK_STOCK_ID ORDER BY (case when SUBSTR(APDK_KIND_ID,3,1) = 'F' then ' ' else SUBSTR(APDK_KIND_ID,3,1) end) NULLS LAST) as seq_no
                       from ci.MG1,ci.APDK
                      where MG1_DATE = to_date(:as_ymd,'yyyymmdd')
                        and MG1_PARAM_KEY = 'ETF'
                        and MG1_KIND_ID = APDK_KIND_ID) M,
                     --上市/上櫃中文名稱
                     (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM')
               WHERE MGR5E_YMD = :as_ymd  
                 and MGR5E_SID = TFXMSE_SID
                 and MGR5E_SID  = M.APDK_STOCK_ID(+) 
                 and MGR5E_AB_TYPE  = 'A'
                 and MGR5E_PID = COD_ID
               ORDER BY MGR5E_SID,KIND_SEQ_NO
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// return 
      /// seq_no/MGR5E_SID/TFXMSE_SNAME/PID_NAME/MGR5E_CP_RISK/MGR5E_RISK/
      /// MGR5E_FUT_xxx/MGR5E_OPT_CM_A/MGR5E_OPT_MM_A/MGR5E_OPT_IM_A/MGR5E_OPT_CM_B/MGR5E_OPT_MM_B/MGR5E_OPT_IM_B/KIND_ID/APDK_NAME/MGR5E_OPT_KIND_ID/
      /// MGR5E_CP_STATUS_CODE/KIND_SEQ_NO
      /// </summary>
      /// <param name="as_sdate"></param>
      /// <param name="as_edate"></param>
      /// <returns></returns>
      public DataTable GetListO(string as_ymd)
      {
         object[] parms = {
            ":as_ymd",as_ymd
            };

         string sql =
             @"
               SELECT rownum as seq_no,
                     A.MGR5E_SID,   
                     CI.TFXMSE.TFXMSE_SNAME,  
                     PID_NAME, --A.MGR5E_PID, 
                     A.MGR5E_CP_RISK,   
                     A.MGR5E_RISK,   
                     case when nvl(A.MGR5E_OPT_KIND_ID,' ') = ' ' then A.MGR5E_OPT_XXX else AA.MG1_XXX end as MGR5E_OPT_XXX,   
                     case when nvl(A.MGR5E_OPT_KIND_ID,' ') = ' ' then A.MGR5E_OPT_CM else AA.MG1_CM end as MGR5E_OPT_CM_A,   
                     case when nvl(A.MGR5E_OPT_KIND_ID,' ') = ' ' then A.MGR5E_OPT_MM else AA.MG1_MM end as MGR5E_OPT_MM_A,   
                     case when nvl(A.MGR5E_OPT_KIND_ID,' ') = ' ' then A.MGR5E_OPT_IM else AA.MG1_IM end as MGR5E_OPT_IM_A,   
                     case when nvl(B.MGR5E_OPT_KIND_ID,' ') = ' ' then B.MGR5E_OPT_CM else BB.MG1_CM end as MGR5E_OPT_CM_B,   
                     case when nvl(B.MGR5E_OPT_KIND_ID,' ') = ' ' then B.MGR5E_OPT_MM else BB.MG1_MM end as MGR5E_OPT_MM_B,   
                     case when nvl(B.MGR5E_OPT_KIND_ID,' ') = ' ' then B.MGR5E_OPT_IM else BB.MG1_IM end as MGR5E_OPT_IM_B,   
                     case when nvl(A.MGR5E_OPT_KIND_ID,' ') = ' ' then ' '  else AA.MG1_KIND_ID end as KIND_ID,
                     case when nvl(A.MGR5E_OPT_KIND_ID,' ') = ' ' then ' '  else AA.APDK_NAME end as APDK_NAME,
                     case when nvl(A.MGR5E_OPT_KIND_ID,' ') = ' ' then ' '  else 'Y' end as MGR5E_OPT_KIND_ID,   
                     A.MGR5E_CP_STATUS_CODE,
                     AA.seq_no as KIND_SEQ_NO
                FROM ci.MGR5E A,ci.MGR5E B,   
                     ci.TFXMSE ,
                    (select APDK_STOCK_ID,MG1_KIND_ID,APDK_NAME,MG1_XXX,MG1_CM,MG1_MM,MG1_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                            ROW_NUMBER( ) OVER (PARTITION BY APDK_STOCK_ID ORDER BY (case when SUBSTR(APDK_KIND_ID,3,1) = 'O' then ' ' else SUBSTR(APDK_KIND_ID,3,1) end) NULLS LAST) as seq_no
                       from ci.MG1,ci.APDK
                      where MG1_DATE = to_date(:as_ymd,'yyyymmdd')
                        and MG1_PARAM_KEY = 'ETC'
                        and MG1_KIND_ID = APDK_KIND_ID
                        and MG1_type = 'A') AA,
                    (select MG1_KIND_ID,MG1_XXX,MG1_CM,MG1_MM,MG1_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE
                       from ci.MG1
                      where MG1_DATE = to_date(:as_ymd,'yyyymmdd')
                        and MG1_PARAM_KEY = 'ETC'
                        and MG1_type = 'B') BB,
                     --上市/上櫃中文名稱
                     (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM')
               WHERE A.MGR5E_YMD = :as_ymd  
                 and A.MGR5E_SID = TFXMSE_SID
                 and A.MGR5E_SID  = AA.APDK_STOCK_ID(+) 
                 and A.MGR5E_AB_TYPE  = 'A'
                 and A.MGR5E_SID = B.MGR5E_SID
                 and B.MGR5E_YMD = :as_ymd
                 and B.MGR5E_AB_TYPE  = 'B'
                 and AA.MG1_KIND_ID = BB.MG1_KIND_ID(+)
                 and A.MGR5E_PID = COD_ID
               ORDER BY A.MGR5E_SID,KIND_SEQ_NO
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

   }
}
