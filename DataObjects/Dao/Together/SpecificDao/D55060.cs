using OnePiece;
using System.Data;

/// <summary>
/// Lukas, 2018/12/26
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {

    /// <summary>
    /// for w_55060
    /// </summary>
    public class D55060 {

        private Db db;

        public D55060() {
            db = GlobalDaoSetting.DB;
        }


        public DataTable d_55060_1(string as_symd, string as_eymd) {

            object[] parms = {
                "@as_symd",as_symd,
                "@as_eymd",as_eymd
            };

            string sql =
                @"
SELECT to_char(to_date(AI2_YMD,'YYYYMMDD'),'YYYY/MM/DD') AS DATA_DATE,
       sum(case when AI2_PARAM_KEY = 'SPF' then AI2_M_QNTY else 0 end) as UDF_QNTY,
       sum(case when AI2_PARAM_KEY = 'UDF' then AI2_M_QNTY else 0 end) as SPF_QNTY
  FROM CI.AI2
 WHERE AI2_YMD >= :as_symd
   AND AI2_YMD <= :as_eymd
   AND AI2_SUM_TYPE = 'D'
   AND AI2_SUM_SUBTYPE = '3'
   AND AI2_PARAM_KEY IN ('UDF','SPF')
 GROUP BY AI2_YMD
 Order By data_date
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }

        public DataTable d_55060_2(string as_sdate, string as_edate) {

            object[] parms = {
                "@as_sdate",as_sdate,
                "@as_edate",as_edate
            };


            //這裡有改動：  and APDKP_DELIVERY_DATE >= :as_sdate
            //              and APDKP_DELIVERY_DATE <= :as_edate
            string sql =
                @"
SELECT to_char(APDKP_DELIVERY_DATE,'YYYY/MM/DD') AS DATA_DATE,
       sum(case when APDKP_KIND_ID = 'UDF' then APDKP_SETTLE_OI else 0 end) as UDF_OI,
       sum(case when APDKP_KIND_ID= 'SPF' then APDKP_SETTLE_OI else 0 end) as SPF_OI
  FROM CI.APDKP 
where APDKP_KIND_ID IN ('SPF','UDF')
  and APDKP_DELIVERY_DATE >= to_date(:as_sdate,'YYYY/MM/DD AM HH:MI:SS')
  and APDKP_DELIVERY_DATE <= to_date(:as_edate,'YYYY/MM/DD AM HH:MI:SS')
 GROUP BY APDKP_DELIVERY_DATE 
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }

        public DataTable d_55060_3(string as_sym, string as_eym) {

            object[] parms = {
                "@as_sym",as_sym,
                "@as_eym",as_eym
            };

            string sql =
                @"
select data_ym,data_fcm,kind_id,
       sum(trd_ar_amt) as trd_ar_amt,
       sum(trd_rec_amt) as trd_rec_amt,
       sum(cm_ar_amt) as cm_ar_amt,
       sum(cm_rec_amt) as cm_rec_amt
  from
      (select FEETRD_YM as data_ym,
              FEETRD_FCM_NO as data_fcm,
              FEETRD_KIND_ID as kind_id,
              sum(FEETRD_AR) as trd_ar_amt,
              sum(FEETRD_REC_AMT) as trd_rec_amt,
              0 as cm_ar_amt,
              0 as cm_rec_amt
         from ci.FEETRD
        where FEETRD_YM >= :as_sym
          and FEETRD_YM <= :as_eym
          and FEETRD_PARAM_KEY IN ('UDF','SPF')
        group by FEETRD_YM,FEETRD_FCM_NO,FEETRD_KIND_ID
        union all
       select FEETDCC_YM as data_ym,
              FEETDCC_FCM_NO as data_fcm,
              FEETDCC_KIND_ID as kind_id,
              0,0,
              sum(FEETDCC_ORG_AR) as ar_amt,
              sum(FEETDCC_ORG_AR - FEETDCC_DISC_AMT) as rec_amt
         from ci.FEETDCC
        where FEETDCC_YM >= :as_sym
          and FEETDCC_YM <= :as_eym
          and FEETDCC_KIND_ID IN ('UDF','SPF')
      group by FEETDCC_YM,FEETDCC_FCM_NO,FEETDCC_KIND_ID)
 GROUP BY data_ym,data_fcm,kind_id
 Order By kind_id,
          data_ym,
          data_fcm
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }

        public DataTable d_55060_3_all(string as_sym, string as_eym) {

            object[] parms = {
                "@as_sym",as_sym,
                "@as_eym",as_eym
            };

            string sql =
                @"
select FEETRD_YM as feetrd_feetrd_ym,
       FEETRD_FCM_NO as feetrd_feetrd_fcm_no,
       FEETRD_KIND_ID,
       FEETRD_DISC_QNTY as feetrd_feetrd_disc_qnty,
       1 - FEETRD_REC_RATE AS DISC_RATE,
       FEETRD_AR + NVL(FEETDCC_ORG_AR,0) as AR,
       FEETRD_MK_DISC_AMT + FEETRD_OTH_DISC_AMT + NVL(FEETDCC_DISC_AMT,0) AS DISC_AMT,
       FEETRD_REC_AMT + (NVL(FEETDCC_ORG_AR,0) - NVL(FEETDCC_DISC_AMT,0)) as REC_AMT,
       FEETRD_M_QNTY as feetrd_feetrd_m_qnty,
       FEETRD_FCM_KIND as feetrd_feetrd_fcm_kind,
       FEETRD_PARAM_KEY as feetrd_feetrd_param_key,
       FEETRD_ACC_NO as feetrd_feetrd_acc_no,
       FEETRD_SESSION as feetrd_feetrd_session
  from ci.FEETRD,ci.FEETDCC 
 Where FEETRD_YM >= :as_sym
   and FEETRD_YM <= :as_eym
   and FEETRD_PARAM_KEY IN ('UDF','SPF') 
   and FEETRD_YM = FEETDCC_YM(+)
   and FEETRD_FCM_NO = FEETDCC_FCM_NO(+)
   and FEETRD_KIND_ID = FEETDCC_KIND_ID(+)
   and FEETRD_ACC_NO = FEETDCC_ACC_NO(+)
   and '0'||FEETRD_SESSION = FEETDCC_SESSION(+)
 Order By feetrd_kind_id,
          feetrd_feetrd_ym,
          feetrd_feetrd_fcm_no
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }

        public DataTable d_55060_3_cm(string as_sym, string as_eym) {

            object[] parms = {
                "@as_sym",as_sym,
                "@as_eym",as_eym
            };

            string sql =
                @"
select FEETDCC_YM,
       FEETDCC_FCM_NO,
       FEETDCC_KIND_ID,
       FEETDCC_DISC_QNTY,
       FEETDCC_DISC_RATE AS DISC_RATE,
       FEETDCC_ORG_AR,
       FEETDCC_DISC_AMT,
       FEETDCC_ORG_AR - FEETDCC_DISC_AMT AS REC_AMT,
       FEETDCC_ACC_NO,
       FEETDCC_SESSION
 from ci.FEETDCC
where FEETDCC_YM >= :as_sym
   and FEETDCC_YM <= :as_eym
   and FEETDCC_KIND_ID IN ('UDF','SPF')
order by feetdcc_kind_id,
         feetdcc_ym,
         feetdcc_fcm_no 
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }

        public DataTable d_55060_3_trd(string as_sym, string as_eym) {

            object[] parms = {
                "@as_sym",as_sym,
                "@as_eym",as_eym
            };

            string sql =
                @"
select FEETRD_YM,
       FEETRD_FCM_NO,
       FEETRD_KIND_ID,
       FEETRD_DISC_QNTY,
       1 - FEETRD_REC_RATE AS DISC_RATE,
       FEETRD_AR,
       FEETRD_MK_DISC_AMT + FEETRD_OTH_DISC_AMT AS DISC_AMT,
       FEETRD_REC_AMT,
       FEETRD_M_QNTY,
       FEETRD_FCM_KIND,
       FEETRD_PARAM_KEY,
       FEETRD_ACC_NO,
       FEETRD_SESSION
 from ci.FEETRD
where FEETRD_YM >= :as_sym
   and FEETRD_YM <= :as_eym
   and FEETRD_PARAM_KEY IN ('UDF','SPF')
order by feetrd_kind_id,
         feetrd_ym,
         feetrd_fcm_no
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;

        }

        public DataTable d_55060_after_export(string ls_ym) {

            object[] parms = {
                "@ls_ym",ls_ym
            };

            string sql =
                @"
select nvl(sum(FEETDCC_DISC_QNTY),0) as ld_disc_qnty
 from ci.FEETDCC
where FEETDCC_YM = :ls_ym
   and FEETDCC_KIND_ID IN ('UDF','SPF') 
                    ";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }
    }
}
