﻿using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.Dao.Together.SpecificDao {
    public class D20230: DataGate {

        public DataTable ListAll() {

            string sql =
                    @"
 SELECT 
 	PLST1_LEVEL,
 	PLST1_C1_QNTY_MIN,
 	PLST1_C1_QNTY_MAX,
 	PLST1_C2_QNTY_MIN,
 	PLST1_C2_QNTY_MAX,
 	PLST1_STKOUT_MIN,
 	PLST1_STKOUT_MAX,
 	PLST1_NATURE,
 	PLST1_LEGAL,
 	PLST1_999,
 	' ' as OP_TYPE
 FROM CI.PLST1
";
            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public ResultData UpdatePLST1(DataTable inputData) {

            string tableName = "CI.PLST1";
            string keysColumnList = @"PLST1_LEVEL";
            string insertColumnList = @"PLST1_LEVEL,
 	                                     PLST1_C1_QNTY_MIN,
 	                                     PLST1_C1_QNTY_MAX,
 	                                     PLST1_C2_QNTY_MIN,
 	                                     PLST1_C2_QNTY_MAX,
 	                                     PLST1_STKOUT_MIN,
 	                                     PLST1_STKOUT_MAX,
 	                                     PLST1_NATURE,
 	                                     PLST1_LEGAL,
 	                                     PLST1_999";
            string updateColumnList = insertColumnList;
            try {
                //update to DB
                return SaveForChanged(inputData, tableName, insertColumnList, updateColumnList, keysColumnList);
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}
