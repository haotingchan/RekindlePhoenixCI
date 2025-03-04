﻿using BusinessObjects;
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

         //if (dt.Rows.Count > 0) {
         //   dt = dt.AsEnumerable().OrderBy(d => d.Field<int>("SEQ_NO"))
         //                     .ThenBy(d => d.Field<string>("PROD_TYPE"))
         //                     .ThenBy(d => d.Field<string>("KIND_GRP2"))
         //                     //.ThenBy(d => {
         //                     //   if (d.Field<string>("KIND_ID").Substring(0, 2) == d.Field<string>("KIND_GRP2"))
         //                     //      return 0;
         //                     //   else
         //                     //      return 1;
         //                     //})
         //                     .ThenBy(d => d.Field<string>("KIND_ID"))
         //                     .ThenBy(d => d.Field<string>("AB_TYPE"))
         //                     .CopyToDataTable();
         //}

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
               case when SP1_TYPE = 'SV' then 'VSR'
                    when SP1_TYPE = 'SD' then 'Delta耗用比率'
                    when SP1_TYPE = 'SS' then '跨商品折抵率' end as SP1_TYPE_RE,
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
                                 case when SP1_TYPE ='SV' then (SP1_RATE *100) ||'%' else '1:'|| TRIM(TO_CHAR(SP1_RATE,'0.00')) end as SP1_RATE,  
                                 case when SP1_TYPE ='SV' then (SP1_CUR_RATE *100) ||'%' else '1:'|| TRIM(TO_CHAR(SP1_CUR_RATE, '0.00')) end as SP1_CUR_RATE ,   
                                 SP1_CHANGE_RANGE *100||'%' as SP1_CHANGE_RANGE,
                                 SP1_SEQ_NO
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
                         ORDER BY  SP1_DATE DESC, SP1_SEQ_NO ASC, SP1_KIND_ID1 ASC, SP1_KIND_ID2 ASC", notIn, asIn);

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

        /// <summary>
        /// get Margin Change value - Max(M)
        /// </summary>
        /// <param name="as_ymd">日期(eg. 20190212)</param>
        /// <param name="kid">代碼(eg. TXF)</param>
        /// <returns></returns>
        public double getMax(String as_ymd, String kid)
        {
            object[] parms = {
            ":as_ymd",as_ymd,
            ":kid",kid
            };
            string sql = @"select MG1_CHANGE_RANGE from ci.MG1_3M where MG1_YMD = :as_ymd and MG1_KIND_ID = :kid and MG1_MODEL_TYPE = 'M' ";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count > 0) return (double)dtResult.Rows[0][0];
            return 0;
        }

        /// <summary>
        /// get Margin Change value - Max(M)
        /// </summary>
        /// <param name="as_ymd">日期(eg. 20190212)</param>
        /// <param name="kid">代碼(eg. TXO)</param>
        /// <param name="ab">選擇權的AB值(eg. A|B)</param>
        /// <returns></returns>
        public double getOptMax(String as_ymd, String kid, String ab)
        {
            object[] parms = {
            ":as_ymd",as_ymd,
            ":kid",kid,
            ":ab",ab
            };
            string sql = @"select MG1_CHANGE_RANGE from ci.MG1_3M where MG1_YMD = :as_ymd and MG1_KIND_ID = :kid and MG1_MODEL_TYPE ='M' and MG1_AB_TYPE = :ab ";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count > 0) return (double)dtResult.Rows[0][0];
            return 0;
        }

        /// <summary>
        /// get Margin Change value - Max(m)
        /// </summary>
        /// <param name="as_ymd">日期(eg. 20190212)</param>
        /// <param name="kid">代碼(eg. TXO)</param>
        /// <param name="ab">選擇權的AB值(eg. A|B)</param>
        /// <returns></returns>
        public double getOptfMax(String as_ymd, String kid, String ab)
        {
            object[] parms = {
            ":as_ymd",as_ymd,
            ":kid",kid,
            ":ab",ab
            };
            string sql = @"select MG1_CHANGE_RANGE from ci.MG1_3M where MG1_YMD = :as_ymd and MG1_KIND_ID = :kid and MG1_MODEL_TYPE ='m' and MG1_AB_TYPE = :ab ";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count > 0) return (double)dtResult.Rows[0][0];
            return 0;
        }

        /// <summary>
        /// get Margin Change value - SMA(S)
        /// </summary>
        /// <param name="as_ymd">日期(eg. 20190212)</param>
        /// <param name="kid">代碼(eg. TXF)</param>
        /// <returns></returns>
        public double getSMA(String as_ymd, String kid)
        {
            object[] parms = {
            ":as_ymd",as_ymd,
            ":kid",kid
            };
            string sql = @"select MG1_CHANGE_RANGE from ci.MG1_3M where MG1_YMD = :as_ymd and MG1_KIND_ID = :kid and MG1_MODEL_TYPE = 'S' ";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count > 0) return (double)dtResult.Rows[0][0];
            return 0;
        }

        /// <summary>
        /// get Margin Change value - SMA(S)
        /// </summary>
        /// <param name="as_ymd">日期(eg. 20190212)</param>
        /// <param name="kid">代碼(eg. TXO)</param>
        /// <param name="ab">選擇權的AB值(eg. A|B)</param>
        /// <returns></returns>
        public double getOptSMA(String as_ymd, String kid, String ab)
        {
            object[] parms = {
            ":as_ymd",as_ymd,
            ":kid",kid,
            ":ab",ab
            };
            string sql = @"select MG1_CHANGE_RANGE from ci.MG1_3M where MG1_YMD = :as_ymd and MG1_KIND_ID = :kid and MG1_MODEL_TYPE ='S' and MG1_AB_TYPE = :ab ";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count > 0) return (double)dtResult.Rows[0][0];
            return 0;
        }

        /// <summary>
        /// get Margin Change value - SMA(s)
        /// </summary>
        /// <param name="as_ymd">日期(eg. 20190212)</param>
        /// <param name="kid">代碼(eg. TXO)</param>
        /// <param name="ab">選擇權的AB值(eg. A|B)</param>
        /// <returns></returns>
        public double getOptfSMA(String as_ymd, String kid, String ab)
        {
            object[] parms = {
            ":as_ymd",as_ymd,
            ":kid",kid,
            ":ab",ab
            };
            string sql = @"select MG1_CHANGE_RANGE from ci.MG1_3M where MG1_YMD = :as_ymd and MG1_KIND_ID = :kid and MG1_MODEL_TYPE ='s' and MG1_AB_TYPE = :ab ";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count > 0) return (double)dtResult.Rows[0][0];
            return 0;
        }


        /// <summary>
        /// get Margin Change value - EWMA(E)
        /// </summary>
        /// <param name="as_ymd">日期(eg. 20190212)</param>
        /// <param name="kid">代碼(eg. TXF)</param>
        /// <returns></returns>
        public double getEWMA(String as_ymd, String kid)
        {
            object[] parms = {
            ":as_ymd",as_ymd,
            ":kid",kid
            };
            string sql = @"select MG1_CHANGE_RANGE from ci.MG1_3M where MG1_YMD = :as_ymd and MG1_KIND_ID = :kid and MG1_MODEL_TYPE = 'E' ";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count >0) return (double)dtResult.Rows[0][0];
            return 0;
        }

        /// <summary>
        /// get Margin Change value - EWMA(E)
        /// </summary>
        /// <param name="as_ymd">日期(eg. 20190212)</param>
        /// <param name="kid">代碼(eg. TXF)</param>
        /// <param name="ab">選擇權的AB值(eg. A|B)</param>
        /// <returns></returns>
        public double getOptEWMA(String as_ymd, String kid, String ab )
        {
            object[] parms = {
            ":as_ymd",as_ymd,
            ":kid",kid,
            ":ab",ab
            };
            string sql = @"select MG1_CHANGE_RANGE from ci.MG1_3M where MG1_YMD = :as_ymd and MG1_KIND_ID = :kid and MG1_MODEL_TYPE ='E' and MG1_AB_TYPE = :ab ";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count > 0) return (double)dtResult.Rows[0][0];
            return 0;
        }

        /// <summary>
        /// get Margin Change value - EWMA(e)
        /// </summary>
        /// <param name="as_ymd">日期(eg. 20190212)</param>
        /// <param name="kid">代碼(eg. TXF)</param>
        /// <param name="ab">選擇權的AB值(eg. A|B)</param>
        /// <returns></returns>
        public double getOptfEWMA(String as_ymd, String kid, String ab)
        {
            object[] parms = {
            ":as_ymd",as_ymd,
            ":kid",kid,
            ":ab",ab
            };
            string sql = @"select MG1_CHANGE_RANGE from ci.MG1_3M where MG1_YMD = :as_ymd and MG1_KIND_ID = :kid and MG1_MODEL_TYPE ='e' and MG1_AB_TYPE = :ab ";
            DataTable dtResult = db.GetDataTable(sql, parms);
            if (dtResult.Rows.Count > 0) return (double)dtResult.Rows[0][0];
            return 0;
        }

    }

}
