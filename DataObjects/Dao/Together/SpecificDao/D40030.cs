using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao {
   public class D40030 : DataGate {

      public DataTable GetData(string AS_ISSUE_YMD, string AS_OSW_GRP, string AS_ADJ_TYPE, string AS_ADJ_SUBTYPE) {

         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("AS_YMD",AS_ISSUE_YMD),
            new DbParameterEx("AS_OSW_GRP",AS_OSW_GRP),
            new DbParameterEx("AS_ADJ_TYPE",AS_ADJ_TYPE),
            new DbParameterEx("AS_ADJ_SUBTYPE",AS_ADJ_SUBTYPE)
            //new DbParameterEx("RETURNPARAMETER",0)
         };

         string sql = "CI.SP_H_TXN_40030_MG_DETL";

         DataTable dt = db.ExecuteStoredProcedureEx(sql, parms, true);

         DataView dv = dt.AsDataView();
         dv.Sort = "SEQ_NO ASC, PROD_TYPE ASC, KIND_GRP2 ASC, KIND_ID ASC, AB_TYPE ASC";
         dt = dv.ToTable();

         return dt;
      }

      public DataTable GetAborad(DateTime AS_ISSUE_YMD, string AS_OSW_GRP) {
         List<DbParameterEx> parms = new List<DbParameterEx>() {
            new DbParameterEx("AS_DATE",AS_ISSUE_YMD),
            new DbParameterEx("AS_OSW_GRP",AS_OSW_GRP)
         };

         string sql = "CI.SP_H_TXN_40030_ABROAD";

         DataTable dt = db.ExecuteStoredProcedureEx(sql, parms, true);

         DataView dv = dt.AsDataView();
         dv.Sort = "KIND_GRP ASC, DATA_TYPE ASC, SEQ_NO ASC";
         dt = dv.ToTable();

         return dt;
      }


      public DataTable GetSpan(DateTime ymd, string oswGrp, string notIn, string asIn) {
         object[] parms = {
                ":ad_date",ymd,
                ":as_osw_grp",oswGrp
            };
         notIn = notIn == "" ? "" : $"and SP1_KIND_ID1 Not In('{notIn}')";
         asIn = asIn == "" ? "" : $"or SP1_KIND_ID1 In('{asIn}')";

         string sql = string.Format(
                   @"SELECT SP1_DATE,   
               SP1_TYPE,   
               SP1_KIND_ID1,   
               SP1_KIND_ID2,   
               SPT1_KIND_ID1_OUT ,  
               SPT1_KIND_ID2_OUT ,   
               SPT1_ABBR_NAME ,   
               SPT1_COM_ID ,    
               SP1_CUR_RATE,   
               SP1_RATE,   
               SP1_CHANGE_RANGE,
               SP1_SEQ_NO,
               SP1_CHANGE_COND,
               NVL(SP2_ADJ_CODE,' ') AS SP2_ADJ_CODE
          FROM ci.SPT1,   
               ci.SP1,ci.SP2
         WHERE SP1_KIND_ID1 = SPT1_KIND_ID1 
           AND SP1_KIND_ID2 = SPT1_KIND_ID2
           AND SP1_DATE = :ad_date  
           AND SP1_CHANGE_FLAG = 'Y' 
           AND SP1_DATE = SP2_DATE(+)
           AND SP1_TYPE = SP2_TYPE(+)
           AND( (SP1_OSW_GRP LIKE :as_osw_grp {0}){1})
           AND SP1_KIND_ID1 = SP2_KIND_ID1(+)
           AND SP1_KIND_ID2 = SP2_KIND_ID2(+)
           order by  sp1_date desc, sp1_seq_no asc, sp1_kind_id1 asc, sp1_kind_id2 asc", notIn, asIn);

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      public DataTable GetSpanTableData(DateTime ymd, string oswGrp, string notIn, string asIn) {
         object[] parms = {
                ":ad_date",ymd,
                ":as_osw_grp",oswGrp
            };

         notIn = notIn == "" ? "" : $"and SP1_KIND_ID1 Not In('{notIn}')";
         asIn = asIn == "" ? "" : $"or SP1_KIND_ID1 In('{asIn}')";

         string sql = string.Format(
                   @"SELECT  case when SP1_TYPE = 'SV' then 'VSR'
                                  when SP1_TYPE = 'SD' then 'Delta耗用比率'
                                  when SP1_TYPE = 'SS' then '跨商品折抵率' end as SP1_TYPE,
                              SPT1_COM_ID ,    
                                 case when SP1_TYPE ='SV' then (SP1_RATE *100) ||'%' else '1:'|| TO_CHAR(SP1_RATE,'0.00') end as SP1_RATE,  
                                 case when SP1_TYPE ='SV' then (SP1_CUR_RATE *100) ||'%' else '1:'|| TO_CHAR(SP1_CUR_RATE, '0.00') end as SP1_CUR_RATE ,                                 
                                 SP1_CHANGE_RANGE *100||'%' as SP1_CHANGE_RANGE
                            FROM ci.SPT1,   
                              ci.SP1,ci.SP2
                           WHERE SP1_KIND_ID1 = SPT1_KIND_ID1 
                             AND SP1_KIND_ID2 = SPT1_KIND_ID2
                             AND SP1_DATE = :ad_date  
                             AND SP1_CHANGE_FLAG = 'Y' 
                             AND SP1_DATE = SP2_DATE(+)
                             AND SP1_TYPE = SP2_TYPE(+)
                             AND( (SP1_OSW_GRP LIKE :as_osw_grp {0}){1})
                             AND SP1_KIND_ID1 = SP2_KIND_ID1(+)
                             AND SP1_KIND_ID2 = SP2_KIND_ID2(+)
                         order by  sp1_date desc, sp1_seq_no asc, sp1_kind_id1 asc, sp1_kind_id2 asc", notIn, asIn);

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

   }

}
