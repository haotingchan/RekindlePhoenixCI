﻿using OnePiece;
using System.Data;

/// <summary>
/// Austin 2018/12/23
/// </summary>
namespace DataObjects.Dao.Together {
   /// <summary>
   /// 就是lookup檔,每個功能有自己的參照資訊,之後會亂
   /// </summary>
   public class CODW {
      private Db db;

      public CODW() {
         db = GlobalDaoSetting.DB;
      }

      /// <summary>
      /// CI.CODW
      /// </summary>
      /// <param name="CODW_TXN_ID"></param>
      /// <returns>CODW_id/CODW_desc/cp_display</returns>
      public DataTable ListByTxn(string CODW_TXN_ID) {
         object[] parms =
         {
                ":CODW_TXN_ID", CODW_TXN_ID
            };

         string sql = @" SELECT trim(CODW_ID) as CODW_id, 
                               trim(CODW_DESC) as CODW_desc,
                               trim(CODW_id) || ' : ' || trim(CODW_desc) as cp_display
                           FROM CI.CODW  
                           WHERE CODW_TXN_ID = :CODW_TXN_ID";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// CI.CODW 
      /// </summary>
      /// <param name="CODW_TXN_ID"></param>
      /// <param name="CODW_COL_ID"></param>
      /// <param name="FirstRowText">自行定義新增的第一個item的Text</param>
      /// <param name="FirstRowValue">自行定義新增的第一個item的Value</param>
      /// <returns>第一行空白+CODW_ID/CODW_DESC/CODW_SEQ_NO</returns>
      public DataTable ListByCol(string CODW_TXN_ID , string CODW_COL_ID , string FirstRowText = " " , string FirstRowValue = " ") {
         object[] parms =
         {
                ":CODW_TXN_ID", CODW_TXN_ID,
                ":CODW_COL_ID", CODW_COL_ID
            };

         //ken,簡易防止sql injection(基本上這兩個值不應該從UI傳進來)
         string tmp = FirstRowText.Length > 20 ? FirstRowText.Substring(0 , 20) : FirstRowText;
         string firstRowText = tmp.Replace("'" , "").Replace("--" , "").Replace(";" , "");
         string tmp2 = FirstRowValue.Length > 20 ? FirstRowValue.Substring(0 , 20) : FirstRowValue;
         string firstRowValue = tmp2.Replace("'" , "").Replace("--" , "").Replace(";" , "");

         string sql = string.Format(@"SELECT TRIM(CODW_ID) AS CODW_ID,
                                     TRIM(CODW_DESC) AS CODW_DESC,
                                     CODW_SEQ_NO
                                    FROM CI.CODW
                                    WHERE CODW_TXN_ID = :CODW_TXN_ID
                                    AND CODW_COL_ID = :CODW_COL_ID
                                    UNION ALL
                                    SELECT '{0}','{1}',0 FROM DUAL
                                    order by CODW_seq_no" , FirstRowValue , FirstRowText);

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// CI.CODW 
      /// </summary>
      /// <param name="CODW_TXN_ID"></param>
      /// <param name="CODW_COL_ID"></param>
      /// <returns>CODW_ID/CODW_DESC/CODW_SEQ_NO/cp_display</returns>
      public DataTable ListByCol2(string CODW_TXN_ID , string CODW_COL_ID) {

         object[] parms =
{
                ":CODW_TXN_ID", CODW_TXN_ID,
                ":CODW_COL_ID", CODW_COL_ID
            };

         string sql = @"select a.CODW_ID as CODW_ID,
                        TRIM(a.CODW_DESC) as CODW_DESC,
                        a.CODW_SEQ_NO,
                        '('||CODW_ID||')'||CODW_DESC as cp_display
                        from (
                            SELECT trim(CODW_ID) as CODW_ID,   
                                CODW_DESC,
                                CODW_SEQ_NO
                            FROM CI.CODW  
                            WHERE TRIM(CODW_TXN_ID) = :CODW_TXN_ID
                            AND TRIM(CODW_COL_ID) = :CODW_COL_ID
                        ) a   
                        order by CODW_seq_no";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// CI.CODW return CODW_id / cp_display
      /// </summary>
      /// <param name="CODW_TXN_ID"></param>
      /// <param name="CODW_COL_ID"></param>
      /// <param name="FirstRowText"></param>
      /// <param name="FirstRowValue"></param>
      /// <returns>CODW_id / cp_display</returns>
      public DataTable ListByCol3(string CODW_TXN_ID , string CODW_COL_ID , string FirstRowText = " " , string FirstRowValue = " ") {
         object[] parms =
         {
            ":CODW_TXN_ID", CODW_TXN_ID,
            ":CODW_COL_ID", CODW_COL_ID
         };

         //ken,簡易防止sql injection(基本上這兩個值不應該從UI傳進來)
         string tmp = FirstRowText.Length > 20 ? FirstRowText.Substring(0 , 20) : FirstRowText;
         string firstRowText = tmp.Replace("'" , "").Replace("--" , "").Replace(";" , "");
         string tmp2 = FirstRowValue.Length > 20 ? FirstRowValue.Substring(0 , 20) : FirstRowValue;
         string firstRowValue = tmp2.Replace("'" , "").Replace("--" , "").Replace(";" , "");

         string sql = string.Format(@"SELECT CODW_id,
                                     CODW_id||' ('||CODW_desc||')' as cp_display
                                       FROM (
                                     SELECT TRIM(CODW_id) AS CODW_id,
                                       TRIM(CODW_desc) AS CODW_desc,
                                       CODW_seq_no
                                       FROM ci.CODW
                                       WHERE CODW_txn_id = :CODW_txn_id
                                       AND CODW_col_id = :CODW_col_id
                                     UNION ALL
                                     SELECT '{0}','{1}',0 from dual
                                     ORDER BY CODW_seq_no
                                 )" , FirstRowValue , FirstRowText);

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
      /// <summary>
      /// 幣別選單
      /// </summary>
      /// <returns></returns>
      public DataTable ListByCurrency(string CODW_TXN_ID = "EXRT" , string CODW_COL_ID = "EXRT_CURRENCY_TYPE") {
         object[] parms =
         {
            ":CODW_TXN_ID", CODW_TXN_ID,
            ":CODW_COL_ID", CODW_COL_ID
         };
         string sql = @"SELECT TRIM(CODW_ID)as CURRENCY_TYPE,   
                              TRIM(CODW_DESC) as CURRENCY_NAME,   
                              CODW_SEQ_NO
                         FROM CI.CODW
                        WHERE CODW_TXN_ID = :CODW_TXN_ID
                          AND CODW_COL_ID = :CODW_COL_ID
                        ORDER BY CODW_SEQ_NO";

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }

      /// <summary>
      /// for all lookupedit from ci.codw 
      /// </summary>
      /// <param name="CODW_TXN_ID">功能代號</param>
      /// <param name="CODW_COL_ID"></param>
      /// <param name="programID"></param>
      /// <returns>codw_id/codw_desc/codw_seq_no</returns>
      public DataTable ListLookUpEdit(string CODW_TXN_ID , string CODW_COL_ID , string programID = "") {

         object[] parms ={
                ":CODW_TXN_ID", CODW_TXN_ID,
                ":CODW_COL_ID", CODW_COL_ID
            };

         string addSql = "";

         switch (programID) {
            case "48010":
            case "48020":
               addSql = " union select  '%' as codw_id, '全選' as codw_desc , 0 as codw_seq_no from dual " +
                  " union select ' ' as codw_id, '選單一契約' as codw_desc, -1 as codw_seq_no from dual ";
               break;
            case "48030":
            case "48040":
               addSql = " union select '%' as codw_id, '全選' as codw_desc , 0 as codw_seq_no from dual ";
               break;
            case "49062":
               addSql = " union select '%' as codw_id, '全部' as codw_desc , 0 as codw_seq_no from dual ";
               break;
         }

         string sql = string.Format(@"
select 
   codw_id,
   codw_desc,
   codw_seq_no
from ci.codw
where codw_txn_id = :CODW_TXN_ID
and codw_col_id = :CODW_COL_ID 
{0}
order by codw_seq_no
" , addSql);

         DataTable dtResult = db.GetDataTable(sql , parms);

         return dtResult;
      }
   }
}