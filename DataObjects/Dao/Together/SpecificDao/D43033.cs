using BusinessObjects;
using OnePiece;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D43033 : DataGate {
      /// <summary>
      /// SP_H_TXN_43033_DETL
      /// </summary>
      /// <param name="AS_DATE_FM"></param>
      /// <param name="AS_DATE_TO"></param>
      /// <returns></returns>
      public DataTable ExecuteStoredProcedure(DateTime AS_DATE_FM,DateTime AS_DATE_TO) {
         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("AS_DATE_FM",AS_DATE_FM),
            new DbParameterEx("AS_DATE_TO",AS_DATE_TO)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_TXN_43033_DETL";
         DataTable spData= db.ExecuteStoredProcedureEx(sql, parms, true);
         //PB 有排序
         DataView dsDv = spData.AsDataView();
         dsDv.Sort = "KIND_ID,DATA_DATE";
         spData = dsDv.ToTable();

         return spData;
      }
      /// <summary>
      /// 測試20190227~20190315 SP_H_TXN_43033_DETL 資料
      /// </summary>
      /// <returns></returns>
      public DataTable TestSpData()
      {
         string sql =
            @"SELECT DATA_DATE,DATA_KIND_ID as KIND_ID,APDK_NAME,APDK_STOCK_ID,PID_NAME,
                   MGP1_CLOSE_PRICE as T_PRICE,
                   MGP1_OPEN_REF as T_OPEN_REF,
                   AI5_SETTLE_PRICE as F_SETTLE_PRICE,
                   AI5_OPEN_REF as F_OPEN_REF
              FROM
                  (SELECT MGR0_YMD as DATA_YMD, TO_DATE(MGR0_YMD, 'YYYYMMDD') AS DATA_DATE,
                          DATA_KIND_ID, DATA_M_KIND_ID
                     FROM ci.MGR0,
                         (SELECT MG1_KIND_ID as DATA_KIND_ID, MG1_M_KIND_ID as DATA_M_KIND_ID
                            FROM ci.MG1_3M
                           WHERE MG1_YMD >= '20190227'
                             AND MG1_YMD <= '20190315'
                             AND MG1_PARAM_KEY = 'ETF'
                             AND MG1_MODEL_TYPE = 'S'
                           GROUP BY MG1_KIND_ID, MG1_M_KIND_ID) S
                     WHERE MGR0_YMD >= '20190227'
                      AND MGR0_YMD <= '20190315') D,
                   ci.MGP1_SMA,ci.AI5,ci.APDK,
                  --上市 / 上櫃中文名稱
                 (SELECT TRIM(COD_ID) as COD_ID, TRIM(COD_DESC) as PID_NAME
                    FROM CI.COD
                   WHERE COD_TXN_ID = 'TFXM' AND COD_COL_ID = 'TFXM_PID')
            WHERE DATA_YMD = MGP1_YMD(+)
              AND DATA_M_KIND_ID = MGP1_M_KIND_ID(+)
              AND DATA_DATE = AI5_DATE(+)
              AND DATA_KIND_ID = AI5_KIND_ID(+)
              AND DATA_KIND_ID = APDK_KIND_ID
              AND APDK_UNDERLYING_MARKET = COD_ID
            ORDER BY KIND_ID,DATA_DATE";
         DataTable dtResult = db.GetDataTable(sql, null);
         return dtResult;

      }
   }
}
