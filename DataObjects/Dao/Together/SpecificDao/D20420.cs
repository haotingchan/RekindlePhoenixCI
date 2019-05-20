using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Lukas, 2019/2/20
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao {
    public class D20420 : DataGate {

        public DataTable ListAllByTxnId(string as_txn_id) {
            object[] parms = {
                ":as_txn_id", as_txn_id
            };

            string sql =
@"
SELECT IDFG_TYPE,   
         IDFG_ACC_CODE ,   
         IDFG_W_TIME,
         IDFG_W_USER_ID,
         IDFG_TABLE_ID ,
         ' ' as OP_TYPE 
    FROM ci.IDFG ,
        (SELECT COD_ID as tbl_id,
                trim(acc_grp_code) as acc_grp_code
           FROM CI.COD ,
               (SELECT TRIM(COD_COL_ID) as tbl_cod_id,
                       TRIM(COD_ID) as acc_grp_code,
                       TRIM(COD_DESC) as acc_grp_name
                  FROM ci.COD
                 WHERE COD_TXN_ID = '20420')
          WHERE COD_TXN_ID = '20420d' 
            AND trim(COD_DESC) = tbl_cod_id  
            AND COD_COL_ID = :as_txn_id 
        )
  WHERE IDFG_TABLE_ID = tbl_id
    AND IDFG_TYPE = acc_grp_code(+)
ORDER BY idfg_type, idfg_acc_code
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public DataTable d_idfg_txn_id() {

            string sql =
@"
SELECT trim(COD_COL_ID) as COD_ID,   
        COD_DESC  
FROM CI.COD  
WHERE COD_TXN_ID = '20420d'
UNION 
    SELECT ' ',' '
    FROM DUAL
ORDER BY COD_DESC
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public DataTable dddw_idfg_txn_id() {

            string sql =
@"
SELECT trim(COD_COL_ID) as COD_ID,   
        TRIM(COD_COL_ID) ||'－'||TRIM(NVL(TXN_NAME,' ')) as COD_DESC  
FROM ci.COD ,ci.TXN 
WHERE COD_TXN_ID = '20420d'    
  AND COD_COL_ID = TXN_ID(+)
ORDER BY COD_DESC
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public DataTable dddw_idfg_type() {

            string sql =
@"
SELECT A.COD_ID,
       TRIM(A.cod_id) || ' : '|| trim(A.cod_desc ) as CP_DISPLAY
FROM
    (SELECT trim(acc_grp_code) as COD_ID,   
            trim(acc_grp_name) as COD_DESC  
    FROM CI.COD ,
        (SELECT TRIM(COD_COL_ID) as tbl_cod_id,
                TRIM(COD_ID) as acc_grp_code,
                TRIM(COD_DESC) as acc_grp_name
            FROM ci.COD
            WHERE COD_TXN_ID = '20420'
              AND COD_COL_ID <> 'TABLE_ID')
    WHERE COD_TXN_ID = '20420d' 
      AND trim(COD_DESC) = tbl_cod_id) A
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public DataTable dddw_idfg_type(string data) {

            object[] parms = {
                ":data", data
            };

            string sql =
@"
SELECT A.COD_ID,
       TRIM(A.cod_id) || ' : '|| trim(A.cod_desc ) as CP_DISPLAY
FROM
    (SELECT trim(acc_grp_code) as COD_ID,   
            trim(acc_grp_name) as COD_DESC  
    FROM CI.COD ,
        (SELECT TRIM(COD_COL_ID) as tbl_cod_id,
                TRIM(COD_ID) as acc_grp_code,
                TRIM(COD_DESC) as acc_grp_name
            FROM ci.COD
            WHERE COD_TXN_ID = '20420'
              AND COD_COL_ID <> 'TABLE_ID')
    WHERE COD_TXN_ID = '20420d' 
      AND trim(COD_DESC) = tbl_cod_id
      AND COD_COL_ID = :data ) A
";
            DataTable dtResult = db.GetDataTable(sql, parms);

            return dtResult;
        }

        public ResultData UpdateIDFG(DataTable inputData) {
            string sql =
@"
SELECT   IDFG_TYPE,   
         IDFG_ACC_CODE,   
         IDFG_W_TIME,
         IDFG_W_USER_ID,
         IDFG_TABLE_ID
    FROM ci.IDFG
";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
