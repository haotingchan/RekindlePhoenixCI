using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataObjects.Dao.Together {
    public class OCF {
        private Db db;
        private string _dbType;

        public OCF() {
            db = GlobalDaoSetting.DB;
            _dbType = "ci";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType">DB name</param>
        public OCF(string dbType) {
            db = GlobalDaoSetting.DB;

            //撈取其他的資料庫相同table
            //ken,簡易防止sql injection(基本上這兩個值不應該從UI傳進來)
            string tmp = dbType.Length > 20 ? dbType.Substring(0, 20) : dbType;
            _dbType = tmp.Replace("'", "").Replace("--", "").Replace(";", "");
        }

        public DataTable GetData() {
            #region sql

            string sql = string.Format(@"
SELECT OCF_DATE,OCF_PREV_DATE,OCF_NEXT_DATE,OCF_CURR_OPEN_SW,OCF_OPEN_TIME,OCF_CLOSE_TIME 
FROM {0}.OCF", _dbType);

            #endregion

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public BO_OCF GetOCF() {
            BO_OCF boOCF = new BO_OCF();

            DataTable dt = GetData();

            if (dt.Rows.Count != 0) {
                boOCF.OCF_DATE = Convert.ToDateTime(dt.Rows[0]["OCF_DATE"]);
                boOCF.OCF_NEXT_DATE = Convert.ToDateTime(dt.Rows[0]["OCF_NEXT_DATE"]);
                boOCF.OCF_PREV_DATE = Convert.ToDateTime(dt.Rows[0]["OCF_PREV_DATE"]);
            }

            return boOCF;
        }

        public DataTable GetDataList() {
            #region sql

            string sql = string.Format(@"
                                                              SELECT 'ci' as OCF_SYS,
                                                                     OCF_DATE,   
                                                                     OCF_PREV_DATE,   
                                                                     OCF_NEXT_DATE,   
                                                                     OCF_CURR_OPEN_SW,   
                                                                     OCF_OPEN_TIME,   
                                                                     OCF_CLOSE_TIME
                                                                FROM ci.OCF   
                                                                union all
                                                              SELECT 'fut' ,
                                                                     OCF_DATE,   
                                                                     OCF_PREV_DATE,   
                                                                     OCF_NEXT_DATE,   
                                                                     OCF_CURR_OPEN_SW,   
                                                                     OCF_OPEN_TIME,   
                                                                     OCF_CLOSE_TIME
                                                                FROM FUT.OCF   
                                                               union all  
                                                              SELECT 'opt',
                                                                     OCF_DATE,   
                                                                     OCF_PREV_DATE,   
                                                                     OCF_NEXT_DATE,   
                                                                     OCF_CURR_OPEN_SW,   
                                                                     OCF_OPEN_TIME,   
                                                                     OCF_CLOSE_TIME
                                                                FROM opt.OCF   
                                                                
            ");

            #endregion

            DataTable dtResult = db.GetDataTable(sql, null);

            return dtResult;
        }

        public List<BO_OCF> GetOCFList()
        {
            List<BO_OCF> list = new List<BO_OCF>();
            BO_OCF boOCF ;

            DataTable dt = GetDataList();

            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Constraints)
                {
                    boOCF = new BO_OCF();
                    boOCF.OCF_DATE = Convert.ToDateTime(row["OCF_DATE"]);
                    boOCF.OCF_NEXT_DATE = Convert.ToDateTime(row["OCF_NEXT_DATE"]);
                    boOCF.OCF_PREV_DATE = Convert.ToDateTime(row["OCF_PREV_DATE"]);
                    list.Add(boOCF);
                }
            }

            return list;
        }
    }
}
