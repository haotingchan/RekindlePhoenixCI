using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,201900403,D40050
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D40050 : DataGate
   {
      /// <summary>
      /// return CNT/
      /// MG4_DATE/MG4_KIND_ID/MG4_TYPE/MG4_CM/MG4_MM/MG4_IM/MG4_PROD_TYPE/RPT_SEQ_NO/MGT2_SEQ_NO/
      /// MG4_CM_B/MG4_MM_B/MG4_IM_B/APDK_CURRENCY_TYPE/MG4_PROD_SUBTYPE/MG4_PARAM_KEY/APDK_NAME/APDK_STOCK_ID/PID_NAME/
      /// CP_CNT
      /// </summary>
      /// <param name="as_date"></param>
      /// <param name="as_year_date"></param>
      /// <param name="as_osw_grp"></param>
      /// <returns></returns>
      public DataTable GetData(DateTime as_date, DateTime as_year_date, string as_osw_grp)
      {
         object[] parms = {
            ":as_date",as_date,
            ":as_year_date",as_year_date,
            ":as_osw_grp",as_osw_grp
            };

         string sql =
             @"SELECT rank() OVER (partition by sort.MG4_KIND_ID order by sort.MG4_DATE )-1 as CNT,sort.*,count(MG4_KIND_ID) Over( partition by sort.MG4_KIND_ID ) as CP_CNT
               FROM
                (SELECT MG4_DATE,   
                        nvl(MGT2_KIND_ID_OUT,MG4_KIND_ID) AS MG4_KIND_ID,   
                        MG4_TYPE,   
                        MG4_CM,MG4_MM,MG4_IM,   
                        MG4_PROD_TYPE ,
                        RPT_SEQ_NO    ,
                        MGT2_SEQ_NO   ,
                        MG4_CM_B,MG4_MM_B,MG4_IM_B, 
                        APDK_CURRENCY_TYPE,
                        MG4_PROD_SUBTYPE,MG4_PARAM_KEY,
                        APDK_NAME,APDK_STOCK_ID, PID_NAME
                   FROM ci.MG4  ,
                        (select TRIM(rpt_value) as rpt_value,rpt_seq_no from ci.rpt where rpt_txd_id IN ('40051','40052')) rpt,
                        ci.MGT2,ci.APDK,
                        --上市/上櫃中文名稱
                        (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM')
                  WHERE MG4_DATE <= :as_date  AND  
                        MG4_DATE  >= :as_year_date   and
                        trim(MG4_KIND_ID) = RPT_VALUE(+) and
                        MG4_PROD_TYPE = MGT2_PROD_TYPE(+) and
                        MG4_KIND_ID = MGT2_KIND_ID(+) and
                        MG4_KIND_ID = APDK_KIND_ID 
                    and APDK_UNDERLYING_MARKET = COD_ID(+)
                    and APDK_MARKET_CLOSE like :as_osw_grp) sort
               ORDER BY MGT2_SEQ_NO,decode(RPT_SEQ_NO,null,99,RPT_SEQ_NO),SUBSTR(MG4_KIND_ID,1,2), decode(SUBSTR(MG4_KIND_ID,3,1),MG4_PROD_TYPE,' ' ,SUBSTR(MG4_KIND_ID,3,1)),MG4_DATE
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
