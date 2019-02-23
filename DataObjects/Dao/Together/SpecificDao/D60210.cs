using OnePiece;
using System;
using System.Data;
using System.Globalization;

namespace DataObjects.Dao.Together.SpecificDao
{
    public class D60210
    {
        private Db db;

        public D60210()
        {
            db = GlobalDaoSetting.DB;
        }

        public DataTable ListDataFor602111(DateTime queryDate)
        {
            object[] parms =
            {
                "@RPT_TXD_ID", "602111",
                "@AMIF_DATE", queryDate,
                "AMIF_SETTLE_DATE", "000000",
                "AMIF_DATA_SOURCE", "U",
                "AMIF_PROD_TYPE", "F"
            };

            #region sql

            string sql =
                @"   
                    SELECT   AMIF_DATE,       
                             AMIF_KIND_ID,   
                             AMIF_SETTLE_DATE,  
                             AMIF_CLOSE_PRICE,   
                             AMIF_UP_DOWN_VAL, 
                             AMIF_SUM_AMT,
                             (select NVL(min(RPT.RPT_SEQ_NO),0)
                                from ci.RPT
                               where RPT.RPT_TXD_ID = @RPT_TXD_ID  
                                 and RPT.RPT_VALUE = AMIF_PROD_ID) as RPT_SEQ_NO,
                             ' ' as OP_TYPE    
                    FROM    ci.AMIF 
                    WHERE   AMIF_DATE = @AMIF_DATE 
                    AND     AMIF_SETTLE_DATE = @AMIF_SETTLE_DATE 
                    AND     AMIF_DATA_SOURCE = @AMIF_DATA_SOURCE 
                    AND     AMIF_PROD_TYPE IN (@AMIF_PROD_TYPE) 
                    ORDER BY RPT_SEQ_NO, AMIF_SETTLE_DATE
                ";

            #endregion

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable ListDataFor602112(DateTime queryDate)
        {
            object[] parms =
            {
                "@RPT_TXD_ID", "602112",
                "@AMIF_DATE", queryDate,
                "AMIF_SETTLE_DATE", "000000",
                "AMIF_DATA_SOURCE", "T",
                "AMIF_PROD_TYPE", "F"
            };

            #region sql

            string sql =
                @"   
                    SELECT   AMIF_DATE,       
                             AMIF_KIND_ID,   
                             AMIF_SETTLE_DATE,  
                             AMIF_CLOSE_PRICE,   
                             AMIF_UP_DOWN_VAL, 
                             AMIF_M_QNTY_TAL,
                             (select NVL(min(RPT.RPT_SEQ_NO),0)
                                from ci.RPT
                               where RPT.RPT_TXD_ID = @RPT_TXD_ID  
                                 and RPT.RPT_VALUE = AMIF_KIND_ID) as RPT_SEQ_NO,
                             ' ' as OP_TYPE    
                    FROM    ci.AMIF 
                    WHERE   AMIF_DATE = @AMIF_DATE 
                    AND     AMIF_SETTLE_DATE <> @AMIF_SETTLE_DATE 
                    AND     AMIF_DATA_SOURCE = @AMIF_DATA_SOURCE 
                    AND     AMIF_PROD_TYPE IN (@AMIF_PROD_TYPE) 
                    ORDER BY RPT_SEQ_NO, AMIF_KIND_ID, AMIF_SETTLE_DATE
                ";

            #endregion

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable ListDataFor602113(DateTime queryDate, DateTime lastDate)
        {
            object[] parms =
            {
                "@RPT_TXD_ID", "602113",
                "@as_date", queryDate.ToString("yyyyMMdd"),
                "@as_date_last", lastDate.ToString("yyyyMMdd")
            };

            #region sql

            string sql =
                @"   
                     SELECT S.STW_YMD,
                           'STW' AS KIND_ID,

                           ( CASE Nvl(Trim(To_char(STW_SETTLE_M)), ' ')
                               WHEN ' ' THEN '000000'
                               ELSE Trim(To_char(STW_SETTLE_Y)) || Trim(To_char(STW_SETTLE_M))
                             END ) AS SETTLE_DATE,

                           ( CASE Nvl(Trim(To_char(STW_SETTLE_M)), ' ')
                               WHEN ' ' THEN To_number(S.STW_CLSE_1)
                               ELSE To_number(S.STW_CLSE_1) / 10
                             END ) AS CLOSE_PRICE,

                           To_number(S.STW_VOLUMN) AS M_QNTY,

                           (SELECT Nvl(Min(RPT.RPT_SEQ_NO), 0)
                            FROM   ci.RPT
                            WHERE  RPT.RPT_TXD_ID = @RPT_TXD_ID) AS RPT_SEQ_NO,

                           (SELECT To_number(Nvl(Trim(W.STW_SETTLE), '0')) / 10
                            FROM   ci.STW W
                            WHERE  W.STW_YMD = @as_date_last
                                   AND W.STW_COM = S.STW_COM
                                   AND W.STW_SETTLE_Y = S.STW_SETTLE_Y
                                   AND W.STW_SETTLE_M = S.STW_SETTLE_M
                                   AND W.STW_RECTYP = S.STW_RECTYP) AS STW_LAST_SETTLE
                    FROM   ci.STW S
                    WHERE  S.STW_YMD = @as_date 
                    ORDER BY RPT_SEQ_NO, KIND_ID, SETTLE_DATE
                ";

            #endregion

            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DateTime GetStwLastDate(DateTime queryDate)
        {
            DateTime result = new DateTime();

            object[] parms =
            {
                "@ls_ymd", queryDate.ToString("yyyyMMdd")
            };

            #region sql

            string sql =
                @"   
                    SELECT  nvl(max(STW_YMD),'19000101')
                    FROM    ci.STW
                    WHERE   STW_YMD < @ls_ymd
                ";

            #endregion

            DataTable dtResult = db.GetDataTable(sql, parms);

            if(dtResult.Rows.Count != 0)
            {
                result = DateTime.ParseExact(dtResult.Rows[0][0].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture); ;
            }

            return result;
        }

    }
}