using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao
{
   public class D51030:DataGate
   {
      public DataTable ListD50130()
      {
         string sql = @"
                     SELECT 
                         MMF_PARAM_KEY 
                        ,MMF_RESP_RATIO 
                        ,MMF_QNTY_LOW 
                        ,MMF_QUOTE_VALID_RATE 
                        ,' ' AS OP_TYPE 
                        ,MMF_AVG_TIME 
                        ,MMF_W_USER_ID 
                        ,MMF_W_TIME 
                        ,MMF_RFC_MIN_CNT 
                        ,MMF_MARKET_CODE 
                        ,MMF_END_TIME 
                        ,MMF_PROD_TYPE 
                        ,MMF_CP_KIND 
                        ,MMF_RESP_TIME 
                        ,MMF_QUOTE_DURATION
                     FROM CI.MMF
                     ORDER BY
                        mmf_market_code,mmf_prod_type,SUBSTR(mmf_param_key,3,1),mmf_param_key
";
         DataTable dtResult = db.GetDataTable(sql, null);
         return dtResult;
      }//public DbDataAdapter ListD51030
   }
}
