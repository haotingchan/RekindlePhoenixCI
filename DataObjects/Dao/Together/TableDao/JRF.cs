using OnePiece;
using System;
using System.Data;

namespace DataObjects.Dao.Together
{
   public class JRF
   {
      private Db db;

      public JRF()
      {
         db = GlobalDaoSetting.DB;
      }

      public DataTable ListData(string JRF_TXN_ID,string JRF_TXF_TID)
      {
         object[] parms =
         {
                "@JRF_TXN_ID", JRF_TXN_ID,
                "@JRF_TXF_TID", JRF_TXF_TID
            };

         #region sql

         string sql =
             @"
                    SELECT  *
                    FROM    ci.JRF
                    WHERE   JRF_TXN_ID = @JRF_TXN_ID
                    AND JRF_TXF_TID = @JRF_TXF_TID
                ";

         #endregion sql

         DataTable dtResult = db.GetDataTable(sql, parms);

         return dtResult;
      }


   }
}