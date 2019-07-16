using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.TableDao
{
    public class BLP_OPTVOL : DataGate
    {
        public DataTable ListData(DateTime sdate,DateTime edate,string ovIndex)
        {

            object[] parms = {
                ":as_sdate",sdate,
                ":as_edate",edate,
                ":as_index",ovIndex
            };

            string sql =
        @"
                      SELECT   BLP_OV_DATE,   
                                     BLP_OV_TIME,   
                                     BLP_OV_INDEX,   
                                     BLP_OV_DURATION,   
                                     BLP_OV_EXPIRY_DATE,   
                                     BLP_OV_DELTA,   
                                     BLP_OV_STRIKE,   
                                     BLP_OV_VOL
                        FROM CI.BLP_OPTVOL
                    WHERE BLP_OV_DATE >= :as_sdate
                    AND BLP_OV_DATE <= :as_edate
                    AND BLP_OV_INDEX LIKE :as_index
            ";
            DataTable dtResult = db.GetDataTable(sql, parms);
            return dtResult;
        }
    }
}
