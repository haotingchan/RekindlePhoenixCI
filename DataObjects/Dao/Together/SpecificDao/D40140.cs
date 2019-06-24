using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Winni,2019/3/13
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
   /// <summary>
   /// 黃金類契約保證金調整補充說明
   /// </summary>
   public class D40140 {

      private Db db;

      public D40140() {

         db = GlobalDaoSetting.DB;

      }

      /// <summary>
      /// get CI.MG1/CI.MGT6/CI.MGT2/CI.MG2 data (d40140_1_mg1)
      /// return MG1_CUR_CM/MG1_CM/MG1_CHANGE_RANGE/MG1_IM/MG1_KIND_ID/MGT2_ABBR_NAME 6 fields 
      /// </summary>
      /// <param name="isDate">yyyy/MM/dd</param>
      /// <returns></returns>
      public DataTable GetGoldData(DateTime isDate) {

         object[] parms = {
                ":isDate", isDate
            };


         string sql = @"
SELECT 
   MGD2_CUR_CM,   
   MGD2_CM,   
   MGD2_ADJ_RATE,   
   MGD2_IM,   
   MGD2_KIND_ID,   
   MGT2_ABBR_NAME  
FROM ci.MGD2,ci.MGT2,ci.MGT6
WHERE MGD2_YMD = TO_CHAR(:isDate,'YYYYMMDD')
AND MGD2_PROD_TYPE = MGT2_PROD_TYPE
AND MGD2_KIND_ID = MGT2_KIND_ID
AND MGD2_KIND_ID = MGT6_KIND_ID
AND MGT6_GRP_ID = 'GOLD'
AND MGD2_ADJ_CODE = 'Y' 
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// get CI.MG1/CI.RPT/CI.MG8D/CI.MG8/CI.MG9/CI.MGT8/CI.HEXRT data return 12 fields (d40140_2) 
      /// </summary>
      /// <param name="isDate">yyyy/MM/dd</param>
      /// <returns></returns>
      public DataTable ListMoneyData(DateTime isDate) {

         object[] parms = {
                ":isDate", isDate
            };


         string sql = @"
SELECT 
	'1' AS DATA_TYPE,
	'TAIFEX'AS COM,
	MG1_KIND_ID,
	MG1_CM,
	MG1_MM,
	MG1_IM,
	MG1_PRICE,
	0 AS EXCHANGE_RATE,
	SEQ_NO AS RPT_SEQ_NO,
	MG1_IM AS OUT_IM,
	'' AS F_NAME,
	MG1_XXX
FROM CI.MG1,
    (SELECT RPT_VALUE, MIN(RPT_SEQ_NO) AS SEQ_NO 
	 FROM CI.RPT
	 WHERE RPT_TXD_ID = '40140_2'
	 GROUP BY RPT_VALUE) R
WHERE MG1_DATE = :isDate
AND MG1_KIND_ID IN ('GDF','TGF')
AND MG1_KIND_ID = RPT_VALUE
UNION ALL
SELECT 
	'2',
	MGT8_F_ID,
	MGT8_PDK_KIND_ID,
	MG8_CM,
	MG8_MM,
	MG8_IM,
	MG9_PRICE,
	NVL(CASE WHEN HEXRT_COUNT_CURRENCY <> '1' THEN HEXRT_MARKET_EXCHANGE_RATE ELSE HEXRT_EXCHANGE_RATE END,0),
	SEQ_NO,
	MG8_IM AS OUT_IM,       
	MGT8_F_EXCHANGE,
	MGT8_XXX
FROM CI.MG8D,CI.MG8,CI.MG9,CI.MGT8,
    (SELECT RPT_VALUE, MIN(RPT_SEQ_NO) AS SEQ_NO 
	 FROM CI.RPT
	 WHERE RPT_TXD_ID = '40140_2'
	 GROUP BY RPT_VALUE) R,
	(SELECT HEXRT_CURRENCY_TYPE,
		HEXRT_COUNT_CURRENCY,
		HEXRT_EXCHANGE_RATE,
		HEXRT_MARKET_EXCHANGE_RATE
     FROM CI.HEXRT
     WHERE HEXRT_DATE = :isDate) E
WHERE MG8D_YMD = TO_CHAR(:isDate,'YYYYMMDD')
--保證金
AND MG8D_EFFECT_YMD = MG8_EFFECT_YMD
AND MG8D_F_ID = MG8_F_ID
--基本資料
AND MG8D_F_ID = MGT8_F_ID
AND MGT8_PDK_KIND_ID = 'GDF' 
--報表位置
AND MG8D_F_ID = RPT_VALUE(+)
--價格
AND MG8D_YMD = MG9_YMD
AND MG8D_F_ID = MG9_F_ID
--匯率
AND MGT8_CURRENCY_TYPE = HEXRT_CURRENCY_TYPE(+)   
AND CASE WHEN MGT8_F_ID = 'TOC01' THEN '2' ELSE '1' END = HEXRT_COUNT_CURRENCY(+)
ORDER BY RPT_SEQ_NO
";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

   }
}
