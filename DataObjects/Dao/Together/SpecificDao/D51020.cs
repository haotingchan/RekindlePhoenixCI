using OnePiece;
using System.Data;

/// <summary>
/// ken 2019/1/2
/// </summary>
namespace DataObjects.Dao.Together.SpecificDao
{
    /// <summary>
    /// 報價商品累計時間檔維護
    /// </summary>
    public class D51020
    {
        private Db db;

        public D51020()
        {
            db = GlobalDaoSetting.DB;
        }

        /// <summary>
        /// d_51020
        /// </summary>
        /// <returns></returns>
        public DataTable ListAll()
        {
            string sql = @"
SELECT ' ' as OP_TYPE,   
MMFT_ID,   
MMFT_KIND_ID,   
MMFT_MTH_CNT,   
MMFT_BEGIN_MTH,  

MMFT_END_S,   
MMFT_END_E,   
MMFT_IN_CNT,   
MMFT_OUT_CNT,   
MMFT_AVG_CNT,

MMFT_PROD_TYPE,
MMFT_REF_KIND_ID ,
MMFT_AVG_MTH_CNT ,
MMFT_MARKET_CODE ,
MMFT_CP_KIND 

FROM CI.MMFT 
order by mmft_market_code , mmft_prod_type , mmft_kind_id , mmft_id
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        /// <summary>
        /// ken,對應舊系統的d_51020_2,但似乎沒用到
        /// </summary>
        /// <returns></returns>
        public DataTable ListAll2() {


            string sql = @"
SELECT ' ' as OP_TYPE,   
CI.MMFT.MMFT_ID,   
CI.MMFT.MMFT_KIND_ID,   
CI.MMFT.MMFT_MTH_CNT,   
CI.MMFT.MMFT_BEGIN_MTH,  

CI.MMFT.MMFT_END_S,   
CI.MMFT.MMFT_END_E,   
CI.MMFT.MMFT_IN_CNT,   
CI.MMFT.MMFT_OUT_CNT,   
CI.MMFT.MMFT_AVG_CNT,

CI.MMFT.MMFT_PROD_TYPE,
CI.MMFT.MMFT_REF_KIND_ID ,
CI.MMFT.MMFT_AVG_MTH_CNT 

FROM CI.MMFT
order by mmft_prod_type , mmft_kind_id , mmft_id
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }
    }
}