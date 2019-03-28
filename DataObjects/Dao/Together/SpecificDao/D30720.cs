using System.Data;
/// <summary>
/// john,20190328,D30720
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D30720 : DataGate
   {
      /// <summary>
      /// return AM2_YMD/AI2_PARAM_KEY/AI2_M_QNTY/AI2_OI/AM2_B_QNTY7/AM2_S_QNTY7/AM2_B_QNTY1/
      /// AM2_S_QNTY1/AM2_B_QNTY2/AM2_S_QNTY2/AM2_B_QNTY3/AM2_B_QNTY4/AM2_S_QNTY4/AM2_B_QNTY5/AM2_S_QNTY5
      /// BO/BQ/SO/SQ/AM2_B_QNTY6/AM2_S_QNTY6/AM2_SUM_TYPE/RPT_SEQ_NO
      /// </summary>
      /// <param name="as_sum_type"></param>
      /// <param name="as_ymd"></param>
      /// <param name="as_sym"></param>
      /// <param name="as_eym"></param>
      /// <param name="as_market_code"></param>
      /// <returns></returns>
      public DataTable GetData(string as_sum_type, string as_ymd,string as_sym, string as_eym, string as_market_code)
      {
         object[] parms = {
            ":as_sum_type",as_sum_type,
            ":as_ymd",as_ymd,
            ":as_sym",as_sym,
            ":as_eym",as_eym,
            ":as_market_code",as_market_code
            };

         string sql =
             @"
               SELECT AM2_YMD,AI2_PARAM_KEY,
                        AI2_M_QNTY,   
                        AI2_OI,   
                        nvl(AM2_B_QNTY7,0) as AM2_B_QNTY7,
                        nvl(AM2_S_QNTY7,0) as AM2_S_QNTY7,
                        nvl(AM2_B_QNTY1,0) as AM2_B_QNTY1,
                        nvl(AM2_S_QNTY1,0) as AM2_S_QNTY1,
                        nvl(AM2_B_QNTY2,0) as AM2_B_QNTY2,
                        nvl(AM2_S_QNTY2,0) as AM2_S_QNTY2,
                        nvl(AM2_B_QNTY3,0) as AM2_B_QNTY3,
                        nvl(AM2_S_QNTY3,0) as AM2_S_QNTY3,
                        nvl(AM2_B_QNTY4,0) as AM2_B_QNTY4,
                        nvl(AM2_S_QNTY4,0) as AM2_S_QNTY4,
                        nvl(AM2_B_QNTY5,0) as AM2_B_QNTY5,
                        nvl(AM2_S_QNTY5,0) as AM2_S_QNTY5,
                        BO,BQ,SO,SQ,
                        nvl(AM2_B_QNTY6,0) as AM2_B_QNTY6,
                        nvl(AM2_S_QNTY6,0) as AM2_S_QNTY6,
                        AM2_SUM_TYPE,
                        RPT_SEQ_NO
                   FROM  
                 --總交易量及OI
                (SELECT AI2_PARAM_KEY,
                        SUM(case :as_market_code 
                                                 when '0%' then AI2_M_QNTY - AI2_AH_M_QNTY 
                                                 when '1%' then AI2_AH_M_QNTY 
                                                 else AI2_M_QNTY end) AS AI2_M_QNTY,
                        SUM(case when TRIM(AI2_YMD) = :as_eym then AI2_OI else 0 end) AS AI2_OI
                   FROM ci.AI2  
                  WHERE AI2_SUM_TYPE = 'M'  AND  
                        AI2_YMD >= :as_sym  AND
                        AI2_YMD <= :as_eym  AND
                        AI2_PROD_TYPE IN ('F','O') AND
                        AI2_SUM_SUBTYPE = '3' 
                  GROUP BY AI2_PARAM_KEY) I,
                 --各類別成交量
                (SELECT AM2_PARAM_KEY,   
                        SUM(case when AM2_IDFG_TYPE = '7' and AM2_BS_CODE = 'B' then AM2_M_QNTY else 0 end) AS AM2_B_QNTY7, 
                        SUM(case when AM2_IDFG_TYPE = '7' and AM2_BS_CODE = 'S' then AM2_M_QNTY else 0 end) AS AM2_S_QNTY7,
                        SUM(case when AM2_IDFG_TYPE = '1' and AM2_BS_CODE = 'B' then AM2_M_QNTY else 0 end) AS AM2_B_QNTY1,
                        SUM(case when AM2_IDFG_TYPE = '1' and AM2_BS_CODE = 'S' then AM2_M_QNTY else 0 end) AS AM2_S_QNTY1,
                        SUM(case when AM2_IDFG_TYPE = '2' and AM2_BS_CODE = 'B' then AM2_M_QNTY else 0 end) AS AM2_B_QNTY2,
                        SUM(case when AM2_IDFG_TYPE = '2' and AM2_BS_CODE = 'S' then AM2_M_QNTY else 0 end) AS AM2_S_QNTY2,
                        SUM(case when AM2_IDFG_TYPE = '3' and AM2_BS_CODE = 'B' then AM2_M_QNTY else 0 end) AS AM2_B_QNTY3,
                        SUM(case when AM2_IDFG_TYPE = '3' and AM2_BS_CODE = 'S' then AM2_M_QNTY else 0 end) AS AM2_S_QNTY3,
                        SUM(case when AM2_IDFG_TYPE = '4' and AM2_BS_CODE = 'B' then AM2_M_QNTY else 0 end) AS AM2_B_QNTY4,
                        SUM(case when AM2_IDFG_TYPE = '4' and AM2_BS_CODE = 'S' then AM2_M_QNTY else 0 end) AS AM2_S_QNTY4,
                        SUM(case when AM2_IDFG_TYPE = '5' and AM2_BS_CODE = 'B' then AM2_M_QNTY else 0 end) AS AM2_B_QNTY5,
                        SUM(case when AM2_IDFG_TYPE = '5' and AM2_BS_CODE = 'S' then AM2_M_QNTY else 0 end) AS AM2_S_QNTY5,
                        SUM(case when AM2_IDFG_TYPE = '6' and AM2_BS_CODE = 'B' then AM2_M_QNTY else 0 end) AS AM2_B_QNTY6,  
                        SUM(case when AM2_IDFG_TYPE = '6' and AM2_BS_CODE = 'S' then AM2_M_QNTY else 0 end) AS AM2_S_QNTY6, 
                        AM2_YMD,   
                        AM2_SUM_TYPE  
                   FROM 
                       (select AM2_PARAM_KEY,AM2_YMD,AM2_SUM_TYPE,AM2_BS_CODE,AM2_IDFG_TYPE,
                               SUM(case :as_market_code 
                                                        when '0%' then AM2_M_QNTY - AM2_AH_M_QNTY 
                                                        when '1%' then AM2_AH_M_QNTY 
                                                        else AM2_M_QNTY end) AS AM2_M_QNTY
                          from ci.AM2  
                         where AM2_SUM_TYPE = :as_sum_type 
                           and AM2_YMD = :as_ymd
                           and AM2_SUM_SUBTYPE = '3'
                           and AM2_IDFG_TYPE in ('1','2','3','4','5','6','7')
                        group by AM2_PARAM_KEY,AM2_YMD,AM2_SUM_TYPE,AM2_BS_CODE,AM2_IDFG_TYPE)           
                 GROUP BY AM2_PARAM_KEY,
                        AM2_YMD,   
                        AM2_SUM_TYPE) M,
                 --造市者
                (SELECT APDK_PARAM_KEY as PARAM_KEY,  
                        SUM(RAMM1_BO_QNTY) as BO,   
                        SUM(RAMM1_BQ_QNTY) as BQ,   
                        SUM(RAMM1_SO_QNTY) as SO,   
                        SUM(RAMM1_SQ_QNTY) as SQ  
                   FROM ci.RAMM1,ci.APDK  
                  WHERE RAMM1_YMD >= trim(:as_sym)||'01'
                    and RAMM1_YMD <= trim(:as_eym)||'31'
                    and RAMM1_SOURCE = 'O'    
                    and RAMM1_SUM_TYPE = 'D'
                    and RAMM1_KIND_ID = APDK_KIND_ID
                    and RAMM1_BRK_TYPE = '9'
                    and RAMM1_MARKET_CODE LIKE :as_market_code
                  GROUP BY APDK_PARAM_KEY) K,
                 --報表位置 
                (select RPT_VALUE,RPT_SEQ_NO from ci.RPT where RPT_TXD_ID = '30720') R
               where TRIM(AI2_PARAM_KEY)= TRIM(RPT_VALUE(+))
                 and AI2_PARAM_KEY = AM2_PARAM_KEY(+)
                 and AI2_PARAM_KEY = PARAM_KEY(+)   
                    ";
         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
   }
}
