using OnePiece;
using System.Data;

/// <summary>
/// Lukas, 2018/12/26
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {

    /// <summary>
    /// for w_55060
    /// </summary>
    public class D55070:DataGate {

        /// <summary>
        /// (3).Deductable item：
        ///Market maker rebate
        /// </summary>
        /// <param name="ls_sym"></param>
        /// <param name="ls_eym"></param>
        /// <returns></returns>
        public DataTable getAMT(string ls_sym,string ls_eym) {

            object[] parms = {
                ":ls_sym",ls_sym,
                ":ls_eym",ls_eym,
            };

            string sql =
                @"
                         select 
			                        max(trd_ym) as trd_ym,
			                        max(cm_ym) as cm_ym,
			                        sum(trd_amt) as trd_amt 
                           from
                               (select MIN(FEETRD_YM) || '-' || MAX(FEETRD_YM) as trd_ym,
                                      CAST('' as char(13)) as cm_ym,
                                       sum(FEETRD_AR - FEETRD_REC_AMT) as trd_amt
                                  from ci.FEETRD
                                 where FEETRD_YM >= :ls_sym
                                   and FEETRD_YM <= :ls_eym
                                   and FEETRD_PARAM_KEY IN ('BRF')
                                 union all
                                select '',
                                       MIN(FEETDCC_YM) || '-' || MAX(FEETDCC_YM) as cm_ym,
                                       sum(FEETDCC_DISC_AMT) as cm_amt
                                  from ci.FEETDCC
                                 where FEETDCC_YM >= :ls_sym
                                   and FEETDCC_YM <= :ls_eym
                                   and FEETDCC_KIND_ID IN ('BRF')
                               group by FEETDCC_YM)
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable getCurrAMT(string ls_sym, string ls_eym)
        {

            object[] parms = {
                ":ls_sym",ls_sym,
                ":ls_eym",ls_eym,
            };

            string sql =
                @"
                         select (RWDOMNI_ALL_CURR_CLR_AMT + RWDOMNI_ALL_CURR_TRD_AMT) as CURR_AMT
                           from ci.TD_RWDOMNI_ALL,
                                (SELECT IDFG_ACC_CODE
                                   FROM ci.IDFG ,
                                       (SELECT COD_ID as tbl_id,
                                               acc_grp_code as COD_ID 
                                          FROM CI.COD ,
                                              (SELECT TRIM(COD_COL_ID) as tbl_cod_id,
                                                      TRIM(COD_ID) as acc_grp_code,
                                                      TRIM(COD_DESC) as acc_grp_name
                                                 FROM ci.COD
                                                WHERE COD_TXN_ID = '20420')
                                         WHERE COD_TXN_ID = '20420d' 
                                           AND trim(COD_DESC) = tbl_cod_id  
                                           AND COD_COL_ID = '55070')
                                 WHERE IDFG_TABLE_ID = tbl_id
                                   AND IDFG_TYPE = COD_ID)
                          where RWDOMNI_ALL_MONTH >= :ls_sym
                            and RWDOMNI_ALL_MONTH <= :ls_eym
                            and RWDOMNI_ALL_KIND_ID = 'BRF'
                            and RWDOMNI_ALL_CODE = IDFG_ACC_CODE 
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable getInfoFee(string ls_sym, string ls_eym)
        {

            object[] parms = {
                ":ls_sym",ls_sym,
                ":ls_eym",ls_eym,
            };

            string sql =
                @"
                         select nvl(sum(FE_PVCS_INFO_FEE),0) as INFO_FEE
                           from ci.FE_PVCS
                          where FE_PVCS_YM >= :ls_sym
                            and FE_PVCS_YM <= :ls_eym
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }


        public DataTable getQnty(string ls_sym, string ls_eym)
        {

            object[] parms = {
                ":ls_sym",ls_sym,
                ":ls_eym",ls_eym,
            };

            string sql =
                @"
                         SELECT AI2_M_QNTY AS QNTY,
 			                         ROUND(AI2_M_QNTY/DAY_COUNT,0) AS QNTY_AVG,
			                         TOL_AI2_M_QNTY_AVG AS TOL_QNTY_AVG
                         FROM 
                         (
                              SELECT 
                                     SUM(CASE AI2_PARAM_KEY WHEN 'BRF' THEN AI2_M_QNTY ELSE 0 END )AS AI2_M_QNTY,
                                     MAX(CASE AI2_PARAM_KEY WHEN 'BRF' THEN AI2_DAY_COUNT ELSE 0 END) AS DAY_COUNT,
                                     ROUND(SUM(AI2_M_QNTY)/MAX(AI2_DAY_COUNT),0) AS TOL_AI2_M_QNTY_AVG
                                FROM ci.AI2  
                               WHERE AI2_SUM_TYPE = 'M'    
                                     AND AI2_YMD >= :ls_sym  
                                     AND AI2_YMD <= :ls_eym  
                                     AND AI2_SUM_SUBTYPE = '3' 
                                     AND AI2_PROD_TYPE IN ('F','O')  
)
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable getOI(string ls_sym, string ls_eym)
        {

            object[] parms = {
                ":ls_sym",ls_sym,
                ":ls_eym",ls_eym,
            };

            string sql =
                @"
                        SELECT NVL(sum(APDKP_SETTLE_OI),0) as BRF_OI 
                        FROM CI.APDKP 
                        WHERE APDKP_KIND_ID IN ('BRF')
 	                        AND TO_CHAR(APDKP_DELIVERY_DATE,'yyyymmdd') >=  :ls_sym || '01'
  	                        AND TO_CHAR(APDKP_DELIVERY_DATE,'yyyymmdd') <=  :ls_eym || '31'
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
