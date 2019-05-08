using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// John, 2019/5/6
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
   public class D20231 : DataGate
   {
      /// <summary>
      /// d_20231
      /// </summary>
      /// <param name="as_ymd"></param>
      /// <returns></returns>
      public DataTable List20231(string as_ymd)
      {

         object[] parms = {
                ":as_ymd",as_ymd
            };

         string sql =
            @"
            SELECT   PLS4_SID,   
                     PLS4_KIND_ID2,   
                     PLS4_YMD,   
                     PLS4_FUT,   
                     PLS4_OPT,   
                     PLS4_PDK_YMD,   
                     PLS4_W_TIME,   
                     PLS4_W_USER_ID,   
                     ' ' as OP_TYPE,   
                     PLS4_STATUS_CODE,   
                     PLS4_PID  ,
                     APDK_KIND_GRP2 as APDK_KIND_GRP2
                FROM CI.PLS4 ,
                   (select APDK_KIND_ID2,APDK_KIND_GRP2 from ci.APDK group by APDK_KIND_ID2,APDK_KIND_GRP2) A
               WHERE PLS4_YMD = :as_ymd    
                and PLS4_KIND_ID2 = APDK_KIND_ID2(+)
               ORDER BY apdk_kind_grp2, pls4_kind_id2
            ";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
      /// <summary>
      /// d_20231_apdk
      /// </summary>
      /// <returns></returns>
      public DataTable ListApdkData()
      {
         string sql =
            @"
            SELECT APDK_PROD_TYPE,
                   APDK_KIND_ID,
                   APDK_NAME,
                   APDK_STOCK_ID,
                   APDK_BEGIN_DATE,
                   APDK_XXX,
                   APDK_UNDERLYING_MARKET,
                   APDK_KIND_GRP2,
                   APDK_REMARK,
                   --填入值
                   APDK_PROD_SUBTYPE,
                   APDK_KIND_ID_STO,
                   APDK_KIND_ID_OUT,
                   APDK_PARAM_KEY,
                   APDK_QUOTE_CODE,
                   APDK_KIND_ID2,
                   APDK_EXPIRY_TYPE,
                   APDK_NAME_OUT,
                   APDK_KIND_LEVEL,
                   APDK_MARKET_CODE,
	            APDK_MARKET_CLOSE,
	            APDK_CURRENCY_TYPE,
                   ' ' as OP_TYPE
            FROM ci.APDK 
            WHERE APDK_PROD_TYPE IN ('F','O')
              AND APDK_KIND_ID = ' '
            ORDER BY APDK_PROD_TYPE,APDK_KIND_ID
            ";

         DataTable dtResult = db.GetDataTable(sql, null);

         return dtResult;
      }
      /// <summary>
      /// d_20231_hpdk
      /// </summary>
      /// <param name="as_pdk_ymd"></param>
      /// <returns></returns>
      public DataTable ListHpdkData(string as_pdk_ymd)
      {

         object[] parms = {
                ":as_pdk_ymd",as_pdk_ymd
            };

         string sql =
            @"
            SELECT PDK_STOCK_ID,
                   PDK_KIND_ID2 as PLS4_KIND_ID2,
                   '        ' as PLS4_YMD,
                   max(PDK_FUT) as PLS4_FUT,
                   max(PDK_OPT) as PLS4_OPT,     
                   to_char(PDK_DATE,'yyyymmdd') as PLS4_PDK_YMD,
                   sysdate as   PLS4_W_TIME,
                   '          ' as PLS4_W_USER_ID,
                   'I' as OP_TYPE,
                   case when PDK_MINI_TYPE <> 'M' then 'N' else PDK_MINI_TYPE end as PDK_STATUS_CODE,
                   NVL(PDK_UNDERLYING_MARKET,'1') as PDK_UNDERLYING_MARKET,APDK_KIND_GRP2 as APDK_KIND_GRP2
            from
            (select PDK_DATE,PDK_UNDERLYING_MARKET,PDK_STOCK_ID,substr(PDK_KIND_ID,1,2) as PDK_KIND_ID2,'F' as PDK_FUT,' ' as PDK_OPT,nvl(trim(APDK_REMARK),' ') as PDK_MINI_TYPE,APDK_KIND_GRP2 
              from ci.HPDK,ci.APDK
            where PDK_DATE =to_date(:as_pdk_ymd,'yyyymmdd')
              and PDK_PROD_TYPE = 'F'
              and PDK_SUBTYPE = 'S'
              and PDK_STATUS_CODE in ('N','P','1','2')
              and PDK_KIND_ID = APDK_KIND_ID
              --and APDK_REMARK <> 'M'
              --and substr(APDK_KIND_ID,1,2) = substr(APDK_KIND_GRP,1,2)
            union all
            select PDK_DATE,PDK_UNDERLYING_MARKET,PDK_STOCK_ID,substr(PDK_KIND_ID,1,2),' ','O',' ',APDK_KIND_GRP2
             from ci.HPDK,ci.APDK
            where PDK_DATE =to_date(:as_pdk_ymd,'yyyymmdd')
              and PDK_PROD_TYPE = 'O'
              and PDK_SUBTYPE = 'S'
              and PDK_STATUS_CODE in ('N','P','1','2')
              and PDK_KIND_ID = APDK_KIND_ID
              --and APDK_REMARK <> 'M'
              --and substr(APDK_KIND_ID,1,2) = substr(APDK_KIND_GRP,1,2)
             )
            group by PDK_DATE,PDK_UNDERLYING_MARKET,PDK_STOCK_ID,PDK_KIND_ID2,PDK_MINI_TYPE,APDK_KIND_GRP2
            order by PLS4_KIND_ID2,APDK_KIND_GRP2
            ";

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }
      /// <summary>
      /// W_20231存檔
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdatePLS4(DataTable inputData)
      {
         string sql = @"SELECT   PLS4_SID,   
                                 PLS4_KIND_ID2,   
                                 PLS4_YMD,   
                                 PLS4_FUT,   
                                 PLS4_OPT,   
                                 PLS4_PDK_YMD,   
                                 PLS4_W_TIME,   
                                 PLS4_W_USER_ID,   
                                 PLS4_STATUS_CODE,   
                                 PLS4_PID
                            FROM CI.PLS4";

         return db.UpdateOracleDB(inputData, sql);
      }
      /// <summary>
      /// W_20231_adpk存檔
      /// </summary>
      /// <param name="inputData"></param>
      /// <returns></returns>
      public ResultData UpdateAPDK(DataTable inputData)
      {
         string sql = @"SELECT APDK_PROD_TYPE,
                               APDK_KIND_ID,
                               APDK_NAME,
                               APDK_STOCK_ID,
                               APDK_BEGIN_DATE,
                               APDK_XXX,
                               APDK_UNDERLYING_MARKET,
                               APDK_KIND_GRP2,
                               APDK_REMARK,
                               APDK_PROD_SUBTYPE,
                               APDK_KIND_ID_STO,
                               APDK_KIND_ID_OUT,
                               APDK_PARAM_KEY,
                               APDK_QUOTE_CODE,
                               APDK_KIND_ID2,
                               APDK_EXPIRY_TYPE,
                               APDK_NAME_OUT,
                               APDK_KIND_LEVEL,
                               APDK_MARKET_CODE,
                            APDK_MARKET_CLOSE,
                            APDK_CURRENCY_TYPE
                        FROM ci.APDK ";

         return db.UpdateOracleDB(inputData, sql);
      }
      /// <summary>
      /// delete ci.PLS4 where PLS4_YMD = :ls_cp_ymd;
      /// </summary>
      /// <param name="ls_cp_ymd"></param>
      /// <returns></returns>
      public bool DeletePLS4(string ls_cp_ymd)
      {
         object[] parms = {
               ":ls_cp_ymd",ls_cp_ymd
            };
         string sql = @"delete ci.PLS4 where PLS4_YMD = :ls_cp_ymd";
         int executeResult = db.ExecuteSQL(sql, parms);

         if (executeResult > 0) {
            return true;
         }
         else {
            throw new Exception("PLP13刪除失敗");
         }
      }

   }
}
