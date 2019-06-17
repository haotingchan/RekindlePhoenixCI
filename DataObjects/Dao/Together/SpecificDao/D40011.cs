using BusinessObjects;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// John, 2019/4/11
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D40011 : D4001x
   {
      //繼承D4001x
      protected override string FutOptDataSql()
      {
         string sql = @"
                     SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CUR_CM_RATE,MG1_CUR_MM_RATE,MG1_CUR_IM_RATE,
                                   MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,
                                   decode(:as_txn_id||'_'||:as_sheet,'40011_2',MG1_CP_CM,MG1_CM) AS MG1_CM,MG1_CUR_CM as MG1_CUR_CM2,MG1_CHANGE_RANGE,
                                   MG1_CHANGE_FLAG,
                                   MG1_PROD_TYPE,MG1_KIND_ID,MG1_AB_TYPE,R1,decode(MG1_MODEL_TYPE,'S',RPT_LEVEL_3,'M',RPT_LEVEL_4,'E',RPT_LEVEL_CNT,'s',RPT_VALUE_2,'m',RPT_VALUE_3,'e',RPT_VALUE_4) AS R2,SHEET,MGT2_KIND_ID_OUT,MG1_MODEL_TYPE
                       FROM ci.MG1_3M,ci.MGT2,
                             (SELECT RPT_VALUE AS R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,
                             RPT_LEVEL_3,RPT_LEVEL_4,RPT_LEVEL_CNT,TO_NUMBER(RPT_VALUE_2) AS RPT_VALUE_2,TO_NUMBER(RPT_VALUE_3) AS RPT_VALUE_3,TO_NUMBER(RPT_VALUE_4) AS RPT_VALUE_4
                                FROM ci.RPT
                                WHERE trim(RPT_TXN_ID) = :as_txn_id AND trim(RPT_TXD_ID) = :as_txn_id||'_'||:as_sheet) R
                       WHERE MG1_YMD = to_char(:as_date,'YYYYMMDD')
                          AND MG1_KIND_ID = R_KIND_ID
                          AND MG1_KIND_ID = MGT2_KIND_ID
                     union all
                     select MG8_CM , MG8_MM ,MG8_IM , 
                                case when MG9_PRICE is null then 0 else  TRUNC((MG8_CM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                case when MG9_PRICE is null then 0 else  TRUNC((MG8_MM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                                case when MG9_PRICE is null then 0 else  TRUNC((MG8_IM / (MG9_PRICE * MGT8_XXX)) * 10000) / 10000 end,
                            MG9_PRICE,MGT8_XXX,NULL,NULL,NULL,NULL,NULL,NULL,NULL,
                            NULL,MG8D_F_ID,NULL,R1,RPT_LEVEL_3 AS R2,SHEET,MG8D_F_ID,NULL
                       from ci.MG8D,ci.MG8,ci.MG9,ci.MGT8,
                            (SELECT RPT_VALUE AS R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3
                                FROM ci.RPT
                              WHERE trim(RPT_TXN_ID) = :as_txn_id AND trim(RPT_TXD_ID) = :as_txn_id||'_'||:as_sheet) R
                      where MG8D_YMD = to_char(:as_date,'YYYYMMDD')
                        AND MG8D_F_ID = 'SGX02'
                        AND MG8D_EFFECT_YMD = MG8_EFFECT_YMD
                        AND MG8D_F_ID = MG8_F_ID
                        AND MG8D_YMD = MG9_YMD
                        AND MG8D_F_ID = MG9_F_ID   
                        AND MG8D_F_ID = MGT8_F_ID
                        and MG8D_F_ID = R_KIND_ID
                      ORDER BY R1,R2,MG1_KIND_ID,MG1_AB_TYPE";
         return sql;
      }
   }
}
