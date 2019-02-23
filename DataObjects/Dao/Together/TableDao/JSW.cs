using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together
{
   public class JSW
   {
      private Db db;

      public JSW()
      {
         db = GlobalDaoSetting.DB;
      }

      public DataTable GetData(string JSW_ID, string JSW_TYPE, string JSW_SW_CODE)
      {
         object[] parms =
         {
                "@JSW_ID", JSW_ID,
                "@JSW_TYPE", JSW_TYPE,
                "@JSW_SW_CODE", JSW_SW_CODE
            };

         #region sql

         string sql =
             @"
                    SELECT  *
                    FROM    JSW
                    WHERE   JSW_ID = @JSW_ID
                    AND     JSW_TYPE = @JSW_TYPE
                    AND     JSW_SW_CODE = @JSW_SW_CODE
                ";

         #endregion sql

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      public DataTable GetData(string JSW_ID, string JSW_SW_CODE)
      {
         object[] parms =
         {
                "@JSW_ID", JSW_ID,
                "@JSW_SW_CODE", JSW_SW_CODE
            };

         #region sql

         string sql =
             @"
                    SELECT  *
                    FROM    JSW
                    WHERE   JSW_ID = @JSW_ID
                    AND     JSW_SW_CODE = @JSW_SW_CODE
                ";

         #endregion sql

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }

      public bool HasJswPermission(string JSW_ID, string JSW_TYPE)
      {
         DataTable myDT = GetData(JSW_ID, JSW_TYPE, "Y");
         if (myDT.Rows.Count != 0) {
            return true;
         }
         else {
            return false;
         }
      }

      public bool HasJswPermission(string JSW_ID)
      {
         DataTable myDT = GetData(JSW_ID, "Y");
         if (myDT.Rows.Count != 0) {
            return true;
         }
         else {
            return false;
         }
      }

      public DataTable ListAllowJswPermission(string JSW_ID)
      {
         DataTable myDT = GetData(JSW_ID, "Y");
         if (myDT.Rows.Count != 0) {
            return myDT;
         }
         else {
            return null;
         }
      }

      public bool DeleteJswByTxnId(string JSW_ID)
      {
         object[] parms =
         {
                "@JSW_ID", JSW_ID
            };

         string sql = @"DELETE FROM JSW WHERE JSW_ID = @JSW_ID";

         int executeResult = db.ExecuteSQL(sql, parms);

         if (executeResult > 0) {
            return true;
         }
         else {
            return false;
         }
      }

      /// <summary>
      /// GetMaxDate yyyy/mm/dd
      /// </summary>
      /// <param name="JSW_TXN_ID"></param>
      /// <param name="JSW_JOB_TYPE"></param>
      /// <returns></returns>
      public string GetMaxDate(string JSW_TXN_ID, string JSW_JOB_TYPE)
      {
         object[] parms =
         {
                ":as_txn_id", JSW_TXN_ID,
                ":as_job_type", JSW_JOB_TYPE
            };

         string sql =
             @"
select to_char(max(JSW_DATE),'yyyy/mm/dd') as ldt_date
from ci.JSW
where JSW_TXN_ID = :as_txn_id
and JSW_JOB_TYPE = :as_job_type
";
         return db.ExecuteScalar(sql, CommandType.Text, parms);

      }


      /// <summary>
      /// GetMaxDate return to_char(max(JSW_DATE),'yyyy/mm/dd') as ldt_opt_date
      /// </summary>
      /// <param name="JSW_TXN_ID"></param>
      /// <param name="JSW_JOB_TYPE"></param>
      /// <param name="SEQ_NO"></param>
      /// <returns></returns>
      public string GetMaxDate(string JSW_TXN_ID, string JSW_JOB_TYPE, int SEQ_NO)
      {
         object[] parms =
         {
                ":as_txn_id", JSW_TXN_ID,
                ":as_job_type", JSW_JOB_TYPE,
                ":as_seq_no", SEQ_NO
            };

         string sql =
             @"
select to_char(max(JSW_DATE),'yyyy/mm/dd') as ldt_opt_date
from ci.JSW
where JSW_TXN_ID = :as_txn_id
and JSW_JOB_TYPE = :as_job_type
and JSW_SEQ_NO = :as_seq_no
";
         return db.ExecuteScalar(sql, CommandType.Text, parms);

      }

      /// <summary>
      /// Get Count from JSW
      /// </summary>
      /// <param name="JSW_TXN_ID"></param>
      /// <param name="JSW_JOB_TYPE"></param>
      /// <param name="JSW_SW_CODE"></param>
      /// <returns></returns>
      public string GetCount(string JSW_TXN_ID, string JSW_JOB_TYPE, string JSW_SW_CODE = "N")
      {
         object[] parms =
         {
                ":as_txn_id", JSW_TXN_ID,
                ":as_job_type", JSW_JOB_TYPE,
                ":as_sw_code", JSW_SW_CODE
            };

         string sql =
             @"
select count(0) as li_rtn
from ci.JSW
where JSW_TXN_ID = :as_txn_id
and JSW_JOB_TYPE = :as_job_type
and JSW_SW_CODE = :as_sw_code
";
         return db.ExecuteScalar(sql, CommandType.Text, parms);

      }


      /// <summary>
      /// get 2 field, 
      /// ldt_fut_date=max(JSW_SEQ_NO) of li_fut_seq_no 
      /// ldt_opt_date=max(JSW_SEQ_NO) of li_opt_seq_no
      /// </summary>
      /// <param name="JSW_TXN_ID"></param>
      /// <param name="JSW_JOB_TYPE"></param>
      /// <param name="JSW_DATE"></param>
      /// <param name="optSeqNo"></param>
      /// <param name="futSeqNo"></param>
      /// <returns></returns>
      public DataTable ListMaxBySeq(string JSW_TXN_ID, string JSW_JOB_TYPE, DateTime JSW_DATE, int futSeqNo, int optSeqNo)
      {
         //20190212,john,func名稱ListMaxSeq更改為ListMaxBySeq,MaxSeq和MaxBySeq傳的參數還是有差異
         object[] parms =
            {
                ":as_txn_id", JSW_TXN_ID,
                ":as_job_type", JSW_JOB_TYPE,
                ":adt_value", JSW_DATE,
                ":li_fut_seq_no", futSeqNo,
                ":li_opt_seq_no", optSeqNo
            };

         string sql =
             @"
select MAX(case when JSW_SEQ_NO = :li_fut_seq_no then JSW_DATE else to_date('19000101','YYYYMMDD') end) as ldt_fut_date,
       MAX(case when JSW_SEQ_NO = :li_opt_seq_no then JSW_DATE else to_date('19000101','YYYYMMDD') end) as ldt_opt_date       
from ci.JSW
where JSW_TXN_ID = :as_txn_id
and JSW_JOB_TYPE = :as_job_type
and JSW_DATE >= :adt_value
";
         return db.GetDataTable(sql, parms);

      }

      /// <summary>
      /// get 3 field, 
      /// 23,13,17
      /// </summary>
      /// <param name="JSW_TXN_ID"></param>
      /// <param name="JSW_JOB_TYPE"></param>
      /// <param name="JSW_DATE"></param>
      /// <returns></returns>
      public DataTable ListMaxSeq(string JSW_TXN_ID, string JSW_JOB_TYPE, DateTime JSW_DATE)
      {
         //20190212,john,add
         object[] parms =
            {
                ":as_txn_id", JSW_TXN_ID,
                ":as_job_type", JSW_JOB_TYPE,
                ":adt_value", JSW_DATE
            };

         string sql =
             @"
select MAX(case when JSW_SEQ_NO = 23 then JSW_DATE else to_date('19000101','YYYYMMDD') end),
       MAX(case when JSW_SEQ_NO = 13 then JSW_DATE else to_date('19000101','YYYYMMDD') end),       
       MAX(case when JSW_SEQ_NO = 17 then JSW_DATE else to_date('19000101','YYYYMMDD') end)       
from ci.JSW
where JSW_TXN_ID = :as_txn_id
and JSW_JOB_TYPE = :as_job_type
and JSW_DATE >= :adt_value
";
         return db.GetDataTable(sql, parms);

      }

   }
}