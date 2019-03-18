using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// john,20190312,D40200
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D40200 : DataGate
   {
      /// <summary>
      /// return AI3_DATE/AI3_CLOSE_PRICE/AI3_INDEX/AI3_AMOUNT/AI3_M_QNTY/AI3_OI
      /// </summary>
      /// <param name="as_sdate"></param>
      /// <param name="as_edate"></param>
      /// <returns></returns>
      public DataTable GetData(DateTime as_sdate, DateTime as_edate)
      {
         object[] parms = {
            ":as_sdate",as_sdate,
            ":as_edate",as_edate
            };

         string sql =
             @"
               select DATA_KIND_ID as AI5_KIND_ID,DATA_DATE as AI5_DATE,
                       AI5_SETTLE_PRICE,AI5_OPEN_REF,
                       --case when nvl(AI5_PROD_SUBTYPE,' ') in ('C','E')  then AMIFU_CLOSE_PRICE else SCP_CLOSE_PRICE end as SCP_CLOSE_PRICE
                       case when SCP_CLOSE_PRICE is null then AMIFU_CLOSE_PRICE else SCP_CLOSE_PRICE end as SCP_CLOSE_PRICE 
                  from 
                     --日期及商品組合
                     (select DATA_DATE,DATA_KIND_ID 
                        from ci.APDK,
                            (select AI5_DATE as DATA_DATE from ci.AI5
                              where AI5_DATE >= :as_sdate
                                and AI5_DATE <= :as_edate
                                and AI5_PROD_TYPE = 'F'
                                and AI5_PROD_SUBTYPE <> 'S'
                              group by AI5_DATE) DT,
                            (select AI5_KIND_ID as DATA_KIND_ID from ci.AI5
                              where AI5_DATE >= :as_sdate
                                and AI5_DATE <= :as_edate
                                and AI5_PROD_TYPE = 'F'
                                and AI5_PROD_SUBTYPE <> 'S'
                              group by AI5_KIND_ID) S
                       where DATA_KIND_ID = APDK_KIND_ID
                           and APDK_EXPIRY_TYPE = 'S'
                           and APDK_KIND_ID <> 'MXF') D,
                       --資料
                       ci.AI5,ci.HPDK,
                     (SELECT AMIFU_DATE,AMIFU_KIND_ID,AMIFU_CLOSE_PRICE 
                        FROM CI.AMIFU T
                        WHERE T.AMIFU_DATE >= :as_sdate
                          AND T.AMIFU_DATE <= :as_edate
                          AND T.AMIFU_KIND_ID in ('RHF','RTF','GDF','TGF')
                          AND T.AMIFU_SETTLE_DATE = '000000')
                  where DATA_DATE = AI5_DATE(+)
                    and DATA_KIND_ID = AI5_KIND_ID(+)
                    and DATA_DATE = PDK_DATE(+)
                    and DATA_KIND_ID = PDK_KIND_ID(+)
                    and DATA_DATE = AMIFU_DATE(+)
                    and DATA_KIND_ID = AMIFU_KIND_ID(+)
               ORDER BY ai5_kind_id, ai5_date 
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
