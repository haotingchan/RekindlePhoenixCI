using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// john,20190128
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   /// <summary>
   /// 
   /// </summary>
   public class D70030
   {

      private Db db;

      public D70030()
      {

         db = GlobalDaoSetting.DB;

      }
      /// <summary>
      /// return RAM1_YM/RAM1_BRK_NO/RAM1_PROD_TYPE/RAM1_B_AMT/RAM1_S_AMT/RAM1_B_QNTY/RAM1_S_QNTY
      /// </summary>
      /// <param name="as_ym">ex: 201808</param>
      /// <returns></returns>
      public DataTable ListAll(string as_ym)
      {

         object[] parms = {
                ":as_ym",as_ym
            };
         string sql = @"SELECT RAM1_YM,   
                        RAM1_BRK_NO,   
                        RAM1_PROD_TYPE,   
                        RAM1_B_AMT,   
                        RAM1_S_AMT,   
                        RAM1_B_QNTY,   
                        RAM1_S_QNTY
                   FROM ci.RAM1  
                  WHERE RAM1_YM = :as_ym  
                     ";
         DataTable dtResult = db.GetDataTable(sql, parms);
         return dtResult;
      }
      /// <summary>
      /// 執行SP
      /// </summary>
      /// <param name="Is_ym"></param>
      /// <param name="RETURNPARAMETER">一開始傳null，成功回傳0</param>
      /// <returns></returns>
      public DataTable sp_H_stt_RAM1(string Is_ym)
      {
         string returnparameter=string.Empty;
         List<DbParameterEx> parms = new List<DbParameterEx>();
         parms.Add(new DbParameterEx(":ls_ym", Is_ym));
         parms.Add(new DbParameterEx("RETURNPARAMETER", returnparameter));
         string sp = "sp_H_stt_RAM1";

         DataTable reResult = db.ExecuteStoredProcedureEx(sp, parms,true);

         return reResult;
      }
   }
}
