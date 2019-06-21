using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D40xxx : DataGate {

      public DataTable GetData(string AS_ISSUE_YMD, string AS_ADJ_TYPE, string AS_ADJ_SUBTYPE) {

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("AS_ISSUE_YMD",AS_ISSUE_YMD),
            new DbParameterEx("AS_ADJ_TYPE",AS_ADJ_TYPE),
            new DbParameterEx("AS_ADJ_SUBTYPE",AS_ADJ_SUBTYPE),
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_TXN_40090_DETL";

         DataTable dt = db.ExecuteStoredProcedureEx(sql, parms, true);

         DataView dv = dt.AsDataView();
         dv.Sort = "SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC";
         dt = dv.ToTable();

         //if (dt.Rows.Count > 0) {
         //   dt = dt.AsEnumerable().OrderBy(d => d.Field<int>("SEQ_NO"))
         //         .ThenBy(d => d.Field<string>("PROD_TYPE"))
         //         .ThenBy(d => d.Field<string>("KIND_GRP2"))
         //         //.ThenBy(d => {
         //         //   if (d.Field<string>("KIND_ID").Substring(0, 2) == d.Field<string>("KIND_GRP2"))
         //         //      return 0;
         //         //   else
         //         //      return 1;
         //         //})
         //         .ThenBy(d => d.Field<string>("KIND_ID"))
         //         .ThenBy(d => d.Field<string>("AB_TYPE"))
         //         .CopyToDataTable();
         //}

         return dt;
      }

      /// <summary>
      /// 取得SPAN資料
      /// </summary>
      /// <param name="asDate"></param>
      /// <returns></returns>
      public DataTable GetSpanData(DateTime asDate) {
         object[] parms = {
                ":AD_DATE",asDate
            };

         string sql = @"SELECT   
                         CASE WHEN SP1_TYPE = 'SD' THEN 'DELTA耗用比率'
                           WHEN SP1_TYPE = 'SV' THEN 'VSR'   
                           WHEN SP1_TYPE = 'SS' THEN '跨商品折抵率' END AS SP1_TYPE ,
                           SPT1_COM_ID  ,  
                           SP1_CUR_RATE ,
                           SP1_RATE1 ,
                           SP1_CHANGE_RANGE
                  FROM(
                        SELECT
                         SP1_TYPE ,
                         SPT1_COM_ID ,  
                         CASE WHEN SP1_TYPE <> 'SV' THEN CONCAT( '1:', TO_CHAR(SP1_CUR_RATE , '0.00')) ELSE  TO_CHAR(ROUND(SP1_CUR_RATE *100 , 1),'99.9')||'%' END AS SP1_CUR_RATE , 
                         CASE WHEN SP1_TYPE <> 'SV' THEN CONCAT( '1:', TO_CHAR(SP1_RATE , '0.00')) ELSE TO_CHAR(ROUND(SP1_RATE *100 , 1),'99.9')||'%' END AS SP1_RATE1 ,                            TO_CHAR(ROUND(SP1_CHANGE_RANGE *100 , 1),'99.9')||'%' AS SP1_CHANGE_RANGE,
                        SP1_SEQ_NO
                         FROM CI.SPT1,   
                              CI.SP1 ,CI.SP2
                        WHERE ( SP1_KIND_ID1 = SPT1_KIND_ID1 ) AND  
                              ( SP1_KIND_ID2 = SPT1_KIND_ID2) AND 
                              ( SP2_VALUE_DATE = :AD_DATE  )  AND
                              ( SP1_DATE = SP2_DATE )  AND
                              ( SP1_KIND_ID1 = SP2_KIND_ID1) AND
                              ( SP1_KIND_ID2 = SP2_KIND_ID2)
                        ORDER BY SP1_SEQ_NO , SP1_TYPE , SP1_KIND_ID1 , SP1_KIND_ID2
                     )";

         return db.GetDataTable(sql, parms);
      }

      public string GetProdSubType(string prodsubtype) {
         object[] parms = {
                ":prodsubtype",prodsubtype
            };

         string sql = @"SELECT
                           trim(COD_DESC) as COD_DESC
                            FROM CI.COD 
                           WHERE COD_TXN_ID = '49020'
                           and trim(cod_id)=:prodsubtype
                           and COD_COL_ID = 'PDK_SUBTYPE'
                           order by cod_seq_no";

         return db.ExecuteScalar(sql, CommandType.Text, parms);
      }

      public string GetMarketTime(string oswgrp) {

         object[] parms = {
                ":ls_osw_grp",oswgrp
            };


         string sql = @"SELECT to_char(OCFG_CLOSE_TIME,'hh : mi') as ls_osw_grp_time
                               from ci.OCFG
                               WHERE OCFG_MARKET_CODE = '0'
                               AND trim(OCFG_OSW_GRP) = :ls_osw_grp";

         return db.ExecuteScalar(sql, CommandType.Text, parms);
      }
   }
}
