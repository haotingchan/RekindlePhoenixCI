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
            BO_OCF boOCF = null;

            DataTable dt = GetData();

            if (dt.Rows.Count != 0) {
                boOCF = new BO_OCF();
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

        public bool UpdateCI()
        {   
            #region sql

            string sql1 =
                @"
                SELECT 
                            CASE WHEN F.OCF_DATE > O.OCF_DATE THEN O.OCF_DATE ELSE F.OCF_DATE END AS OCF_DATE,
                            CASE WHEN F.OCF_DATE > O.OCF_DATE THEN O.OCF_PREV_DATE ELSE F.OCF_PREV_DATE END AS OCF_PREV_DATE,
                            CASE WHEN F.OCF_DATE > O.OCF_DATE THEN O.OCF_NEXT_DATE ELSE F.OCF_NEXT_DATE END AS OCF_NEXT_DATE,
                            F.OCF_OPEN_TIME,
                            F.OCF_CLOSE_TIME 
                FROM  FUT.OCF F,OPT.OCF O
                ";

            string sql =
                @"
                  UPDATE CI.OCF SET 
                                 OCF_DATE = @OCF_DATE,   
                                 OCF_PREV_DATE = @OCF_PREV_DATE,   
                                 OCF_NEXT_DATE = @OCF_NEXT_DATE,   
                                 OCF_OPEN_TIME = @ OCF_OPEN_TIME,   
                                 OCF_CLOSE_TIME = @OCF_CLOSE_TIME
                ";

            #endregion sql
            DataTable dtResult = db.GetDataTable(sql1, null);

            if (dtResult.Rows.Count > 0) 
            {
                object[] parms =
                {
                "@OCF_DATE", dtResult.Rows[0]["OCF_DATE"],
                "@OCF_PREV_DATE", dtResult.Rows[0]["OCF_PREV_DATE"],
                "@OCF_NEXT_DATE", dtResult.Rows[0]["OCF_NEXT_DATE"],
                "@OCF_OPEN_TIME", dtResult.Rows[0]["OCF_OPEN_TIME"],
                "@OCF_CLOSE_TIME", dtResult.Rows[0]["OCF_CLOSE_TIME"]
                };


                int executeResult = db.ExecuteSQL(sql, parms);

                if (executeResult > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 更新OCF_DATE
        /// </summary>
        /// <param name="OCF_DATE"></param>
        /// <returns></returns>
        public bool UpdateDate(DateTime OCF_DATE)
        {
            object[] parms =
            {
                "@OCF_DATE", OCF_DATE
            };

            #region sql

            string sql = String.Format(
                @"
                  UPDATE {0}.OCF SET OCF_DATE = @OCF_DATE
                ",_dbType);

            #endregion sql

            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool UpdatePrevDate(DateTime OCF_PREV_DATE)
        {
            object[] parms =
            {
                "@OCF_PREV_DATE", OCF_PREV_DATE
            };

            #region sql

            string sql = String.Format(
                @"
                  UPDATE {0}.OCF SET OCF_PREV_DATE = @OCF_PREV_DATE
                ", _dbType);

            #endregion sql

            int executeResult = db.ExecuteSQL(sql, parms);

            if (executeResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
