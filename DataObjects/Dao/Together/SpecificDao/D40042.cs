using BusinessObjects;
using Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
/// <summary>
/// John, 2019/4/22
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D40042: DataGate
   {
      /// <summary>
      /// d_40042
      /// </summary>
      /// <param name="as_ymd"></param>
      /// <returns></returns>
      public DataTable List40042()
      {
         string sql = "CI.SP_H_TXN_40042_COND";
         DataTable dt = db.ExecuteStoredProcedureEx(sql, null, true);
         return dt;
      }

      /// <summary>
      /// ETF（股票類）
      /// d_40042_mg1 where mg1_prod_subtype = 'S'
      /// </summary>
      /// <returns></returns>
      public DataTable List40042Mg1ETF(string as_date)
      {
         object[] parms ={
                ":as_date",as_date.AsDateTime("yyyy/MM/dd")
            };

         string sql = @"SELECT MG1_KIND_ID,   
                                    decode(MG1_TYPE,'-',APDK_NAME,TRIM(APDK_NAME)||'('||MG1_TYPE||'值)') as APDK_NAME,   
                                    APDK_STOCK_ID,
                                    PID_NAME,
                                    MG1_PRICE,   
                                    MG1_XXX,   
                                    MG1_RISK,   
                                    MG1_CP_RISK, 
                                    MG1_MIN_RISK, 
                                    MG1_CP_CM,  
                                    MG1_CUR_CM,
                                    MG1_CHANGE_RANGE,
                                    MG1_CHANGE_FLAG
                           FROM
                           (SELECT MG1_DATE,   
                                    MG1_PROD_TYPE,   
                                    MG1_TYPE,   
                                    MG1_CUR_CM,   
                                    MG1_CUR_MM,   
                                    MG1_CUR_IM,   
                                    MG1_CP_CM,   
                                    MG1_CM,   
                                    MG1_MM,   
                                    MG1_IM,   
                                    MG1_RISK,   
                                    MG1_CP_RISK,   
                                    MG1_CHANGE_RANGE,   
                                    MG1_PRICE,   
                                    MG1_CURRENCY_TYPE,   
                                    MG1_M_MULTI,   
                                    MG1_I_MULTI,   
                                    MG1_XXX,   
                                    MG1_SEQ_NO,   
                                    MG1_MIN_RISK,   
                                    MG1_CHANGE_FLAG,   
                                    MG1_CM_RATE,   
                                    MG1_MM_RATE,   
                                    MG1_IM_RATE,   
                                    MG1_PARAM_KEY,   
                                    MG1_PROD_SUBTYPE,   
                                    MG1_KIND_ID,   
                                    APDK_NAME,   
                                    APDK_UNDERLYING_MARKET,   
                                    APDK_STOCK_ID  ,
                                    PID_NAME,
                                    RPT_ROW
                               FROM CI.MG1U,   
                                    CI.APDK  ,
                                    --上市/上櫃中文名稱
                                    (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM') X,
                                    (SELECT TRIM(RPT_VALUE) AS RPT_PROD_TYPE,TRIM(RPT_VALUE_2) AS RPT_PROD_SUBTYPE,TRIM(RPT_VALUE_3) AS RPT_PARAM_KEY,TRIM(RPT_VALUE_4) ||'%'AS RPT_OSW_GRP,RPT_LEVEL_1 AS RPT_ROW 
                                        FROM ci.RPT WHERE RPT_TXN_ID = '40042' and RPT_TXD_ID = '40042') R
                              WHERE MG1_DATE = :as_date
                                AND MG1_KIND_ID = APDK_KIND_ID 
                                AND MG1_PROD_TYPE = APDK_PROD_TYPE      
                                AND APDK_UNDERLYING_MARKET = COD_ID(+)
                                AND APDK_PROD_TYPE = RPT_PROD_TYPE(+)
                                AND APDK_PROD_SUBTYPE = RPT_PROD_SUBTYPE(+)
                                AND TRIM(APDK_PARAM_KEY) LIKE RPT_PARAM_KEY(+)
                                AND APDK_MARKET_CLOSE LIKE RPT_OSW_GRP(+)
                           ) main
                           WHERE MG1_PROD_SUBTYPE = 'S'
                           ORDER BY main.RPT_ROW,main.MG1_SEQ_NO,main.MG1_KIND_ID,main.MG1_TYPE,main.MG1_DATE";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// 其它（非股票類）
      /// d_40042_mg1 where mg1_prod_subtype <> 'S' and mg1_type <> 'B'
      /// </summary>
      /// <returns></returns>
      public DataTable List40042Mg1Other(string as_date)
      {
         object[] parms ={
                ":as_date",as_date.AsDateTime("yyyy/MM/dd")
            };

         string sql = @"SELECT decode(MG1_TYPE,'-',MG1_KIND_ID,TRIM(MG1_KIND_ID)||'('||MG1_TYPE||'值)') as MG1_KIND_ID,   
                                 APDK_NAME,   
                                 APDK_STOCK_ID,
                                 PID_NAME,
                                 MG1_PRICE,   
                                 MG1_XXX,   
                                 MG1_RISK,   
                                 MG1_CP_RISK, 
                                 MG1_MIN_RISK, 
                                 MG1_CP_CM,  
                                 MG1_CUR_CM,
                                 MG1_CHANGE_RANGE,
                                 MG1_CHANGE_FLAG
                        FROM
                        (SELECT MG1_DATE,   
                                 MG1_PROD_TYPE,   
                                 MG1_TYPE,   
                                 MG1_CUR_CM,   
                                 MG1_CUR_MM,   
                                 MG1_CUR_IM,   
                                 MG1_CP_CM,   
                                 MG1_CM,   
                                 MG1_MM,   
                                 MG1_IM,   
                                 MG1_RISK,   
                                 MG1_CP_RISK,   
                                 MG1_CHANGE_RANGE,   
                                 MG1_PRICE,   
                                 MG1_CURRENCY_TYPE,   
                                 MG1_M_MULTI,   
                                 MG1_I_MULTI,   
                                 MG1_XXX,   
                                 MG1_SEQ_NO,   
                                 MG1_MIN_RISK,   
                                 MG1_CHANGE_FLAG,   
                                 MG1_CM_RATE,   
                                 MG1_MM_RATE,   
                                 MG1_IM_RATE,   
                                 MG1_PARAM_KEY,   
                                 MG1_PROD_SUBTYPE,   
                                 MG1_KIND_ID,   
                                 APDK_NAME,   
                                 APDK_UNDERLYING_MARKET,   
                                 APDK_STOCK_ID  ,
                                 PID_NAME,
                                 RPT_ROW
                            FROM CI.MG1U,   
                                 CI.APDK  ,
                                 --上市/上櫃中文名稱
                                 (SELECT TRIM(COD_ID) as COD_ID,TRIM(COD_DESC) as PID_NAME FROM CI.COD where COD_TXN_ID = 'TFXM') X,
                                 (SELECT TRIM(RPT_VALUE) AS RPT_PROD_TYPE,TRIM(RPT_VALUE_2) AS RPT_PROD_SUBTYPE,TRIM(RPT_VALUE_3) AS RPT_PARAM_KEY,TRIM(RPT_VALUE_4) ||'%'AS RPT_OSW_GRP,RPT_LEVEL_1 AS RPT_ROW 
                                     FROM ci.RPT WHERE RPT_TXN_ID = '40042' and RPT_TXD_ID = '40042') R
                           WHERE MG1_DATE = :as_date
                             AND MG1_KIND_ID = APDK_KIND_ID 
                             AND MG1_PROD_TYPE = APDK_PROD_TYPE      
                             AND APDK_UNDERLYING_MARKET = COD_ID(+)
                             AND APDK_PROD_TYPE = RPT_PROD_TYPE(+)
                             AND APDK_PROD_SUBTYPE = RPT_PROD_SUBTYPE(+)
                             AND TRIM(APDK_PARAM_KEY) LIKE RPT_PARAM_KEY(+)
                             AND APDK_MARKET_CLOSE LIKE RPT_OSW_GRP(+)
                             ) main
                        WHERE MG1_PROD_SUBTYPE <> 'S' and MG1_TYPE <> 'B'
                        ORDER BY main.RPT_ROW,main.MG1_SEQ_NO,main.MG1_KIND_ID,main.MG1_TYPE,main.MG1_DATE";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      /// <summary>
      /// d_40042_tfxm1u 
      /// </summary>
      /// <returns></returns>
      public DataTable List40042Tfxm1u()
      {
         string sql = @"SELECT 'N' as PDK_CHOOSE ,
                           '       ' as PDK_KIND_ID,   
                           '                                          ' as PDK_NAME  ,
                           0.00 as AI5_SETTLE_PRICE,
                           TFXM1_SFD_FPR,
                           TFXM1_SPF_IPR,
                           TFXM1_SID,
                           TFXM1_DATE, 
                           TFXM1_PARAM_KEY,
                           TFXM1_PID
                      FROM ci.TFXM1U   ";
         DataTable dtResult = db.GetDataTable(sql, null);
         return dtResult;
      }

      /// <summary>
      /// delete ci.TFXM1U
      /// </summary>
      /// <returns></returns>
      public void DeleteTFXM1U()
      {
         string sql = @"TRUNCATE TABLE ci.TFXM1U";
         int executeResult = db.ExecuteSQL(sql, null);
      }

      /// <summary>
      /// delete ci.MG1U
      /// </summary>
      /// <returns></returns>
      public void DeleteMG1U()
      {
         string sql = @"TRUNCATE TABLE ci.MG1U";
         int executeResult = db.ExecuteSQL(sql, null);
      }

      /// <summary>
      /// delete ci.MG6U
      /// </summary>
      /// <returns></returns>
      public void DeleteMG6U()
      {
         string sql = @"TRUNCATE TABLE ci.MG6U";
         int executeResult = db.ExecuteSQL(sql, null);
      }

      /// <summary>
      /// delete ci.MGR1U
      /// </summary>
      /// <returns></returns>
      public void DeleteMGR1U()
      {
         string sql = @"TRUNCATE TABLE ci.MGR1U";
         int executeResult = db.ExecuteSQL(sql, null);
      }

      /// <summary>
      /// delete ci.MGR2U
      /// </summary>
      /// <returns></returns>
      public void DeleteMGR2U()
      {
         string sql = @"TRUNCATE TABLE ci.MGR2U";
         int executeResult = db.ExecuteSQL(sql, null);
      }

      /// <summary>
      /// DECLARE My_SP1 PROCEDURE FOR fut.sp_F_gen_H_TFXM1U_F() USING SQLCA;
      /// </summary>
      /// <returns>0</returns>
      public string SP_F_GEN_H_TFXM1U_F()
      {
         string sql = "FUT.SP_F_GEN_H_TFXM1U_F";

         return db.ExecuteStoredProcedureReturnString(sql, null, true, OracleDbType.Int32);
      }

      /// <summary>
      /// DECLARE My_SP2 PROCEDURE FOR opt.sp_O_gen_H_MGR1U() USING SQLCA;
      /// </summary>
      /// <returns>0</returns>
      public string SP_O_GEN_H_MGR1U()
      {
         string sql = "OPT.SP_O_GEN_H_MGR1U";

         return db.ExecuteStoredProcedureReturnString(sql, null, true, OracleDbType.Int32);
      }

      /// <summary>
      /// DECLARE My_SP3 PROCEDURE FOR opt.sp_O_gen_H_MGR2U_I() USING SQLCA;
      /// </summary>
      /// <returns>0</returns>
      public string SP_O_GEN_H_MGR2U_I()
      {
         string sql = "OPT.SP_O_GEN_H_MGR2U_I";

         return db.ExecuteStoredProcedureReturnString(sql, null, true, OracleDbType.Int32);
      }

      /// <summary>
      /// DECLARE My_SP4 PROCEDURE FOR opt.sp_O_gen_H_MGR2U_STF() USING SQLCA;
      /// </summary>
      /// <returns>0</returns>
      public string SP_O_GEN_H_MGR2U_STF()
      {
         string sql = "OPT.SP_O_GEN_H_MGR2U_STF";

         return db.ExecuteStoredProcedureReturnString(sql, null, true, OracleDbType.Int32);
      }

      /// <summary>
      /// DECLARE My_SP5 PROCEDURE FOR fut.sp_F_gen_H_MG1U_S() USING SQLCA;
      /// </summary>
      /// <returns>0</returns>
      public string SP_F_GEN_H_MG1U_S()
      {
         string sql = "FUT.SP_F_GEN_H_MG1U_S";

         return db.ExecuteStoredProcedureReturnString(sql, null, true, OracleDbType.Int32);
      }

      /// <summary>
      /// DECLARE My_SP6 PROCEDURE FOR fut.sp_F_gen_H_MG1U_I() USING SQLCA;
      /// </summary>
      /// <returns>0</returns>
      public string SP_F_GEN_H_MG1U_I()
      {
         string sql = "FUT.SP_F_GEN_H_MG1U_I";

         return db.ExecuteStoredProcedureReturnString(sql, null, true, OracleDbType.Int32);
      }

      /// <summary>
      /// DECLARE My_SP7 PROCEDURE FOR opt.sp_O_gen_H_MG1U_S() USING SQLCA;
      /// </summary>
      /// <returns>0</returns>
      public string SP_O_GEN_H_MG1U_S()
      {
         string sql = "OPT.SP_O_GEN_H_MG1U_S";

         return db.ExecuteStoredProcedureReturnString(sql, null, true, OracleDbType.Int32);
      }

      /// <summary>
      /// DECLARE My_SP8 PROCEDURE FOR opt.sp_O_gen_H_MG1U_I() USING SQLCA;
      /// </summary>
      /// <returns>0</returns>
      public string SP_O_GEN_H_MG1U_I()
      {
         string sql = "OPT.SP_O_GEN_H_MG1U_I";

         return db.ExecuteStoredProcedureReturnString(sql, null, true, OracleDbType.Int32);
      }

      /// <summary>
      /// lds_tfxm1u.update()
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateTFXM1U(DataTable inputData)
      {
         string sql = @"SELECT 
                           TFXM1_SFD_FPR,
                           TFXM1_SPF_IPR,
                           TFXM1_SID,
                           TFXM1_DATE, 
                           TFXM1_PARAM_KEY,
                           TFXM1_PID
                      FROM ci.TFXM1U";

         return db.UpdateOracleDB(inputData, sql);
      }

      public int Tfxm1DateCount(string lsDate)
      {
         object[] parms = {
                ":ls_date",lsDate,
            };
         string sql = @"select count(*) from ci.TFXM1 where TFXM1_DATE=to_date(:ls_date,'yyyy/mm/dd')";
         DataTable dtResult = db.GetDataTable(sql, parms);
         
         return dtResult.Rows[0][0].AsInt();
      }
   }

   public class D40042_40011 : D4001x
   {
      protected override string FutOptDataSql()
      {
         string sql = @"
                     SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                            MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM,
                            MG1_CHANGE_FLAG,
                            MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                       from ci.MG1U,ci.MGT2,
                            (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                               FROM ci.RPT
                              WHERE RPT_TXN_ID = :as_txn_id AND RPT_TXD_ID = :as_txn_id||'_'||:as_sheet) R
                      where MG1_DATE = :as_date
                        and MG1_KIND_ID = R_KIND_ID 
                        and MG1_KIND_ID = MGT2_KIND_ID";
         return sql;
      }
   }

   public class D40042_40012 : D4001x
   {
      protected override string FutOptDataSql()
      {
         string sql = @"
                     SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                            MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM,
                            MG1_CHANGE_FLAG,
                            MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                       from ci.MG1U,ci.MGT2,
                            (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                               FROM ci.RPT
                              WHERE RPT_TXN_ID = :as_txn_id AND RPT_TXD_ID = :as_txn_id||'_'||:as_sheet) R
                      where MG1_DATE = :as_date
                        and MG1_KIND_ID = R_KIND_ID 
                        and MG1_KIND_ID = MGT2_KIND_ID";
         return sql;
      }
   }

   public class D40042_40013 : D4001x
   {
      protected override string FutOptDataSql()
      {
         string sql = @"
                     SELECT MG1_CUR_CM,MG1_CUR_MM,MG1_CUR_IM,MG1_CM_RATE,MG1_MM_RATE,MG1_IM_RATE,
                            MG1_PRICE,MG1_XXX,MG1_RISK,MG1_CP_RISK,MG1_MIN_RISK,MG1_CP_CM,
                            MG1_CHANGE_FLAG,
                            MG1_PROD_TYPE,MG1_KIND_ID,MG1_TYPE,R1,R2,SHEET,MGT2_KIND_ID_OUT
                       from ci.MG1U,ci.MGT2,
                            (SELECT RPT_VALUE as R_KIND_ID,RPT_LEVEL_1 AS SHEET, RPT_LEVEL_2 AS R1,RPT_LEVEL_3 AS R2
                               FROM ci.RPT
                              WHERE RPT_TXN_ID = :as_txn_id AND RPT_TXD_ID = :as_txn_id||'_'||:as_sheet) R
                      where MG1_DATE = :as_date
                        and MG1_KIND_ID = R_KIND_ID 
                        and MG1_KIND_ID = MGT2_KIND_ID";
         return sql;
      }
   }

}
