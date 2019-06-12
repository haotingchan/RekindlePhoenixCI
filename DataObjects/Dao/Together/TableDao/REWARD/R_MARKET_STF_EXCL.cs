using BusinessObjects;
using System.Data;

namespace DataObjects.Dao.Together.TableDao.REWARD
{
    public class R_MARKET_STF_EXCL : DataGate
    {
        public DataTable ListData()
        {
            string sql = @"
                            SELECT 
                                MC_MONTH,
                                GOODS_ID,
                                ' ' AS IS_NEWROW 
                            FROM REWARD.R_MARKET_STF_EXCL
                          ";

            DataTable dtResult = db.GetDataTable(sql, null);
            return dtResult;
        }

        public ResultData UpdateData(DataTable inputData)
        {
            string sql = @"
                            SELECT 
                                MC_MONTH,
                                GOODS_ID
                            FROM REWARD.R_MARKET_STF_EXCL
                          ";

            return db.UpdateOracleDB(inputData, sql);
        }
    }
}
