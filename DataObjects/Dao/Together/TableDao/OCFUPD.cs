using BusinessObjects;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataObjects.Dao.Together {
    public class OCFUPD {
        private Db db;
        private string _dbType;

        public OCFUPD() {
            db = GlobalDaoSetting.DB;
            _dbType = "ci";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType">DB name</param>
        public OCFUPD(string dbType) {
            db = GlobalDaoSetting.DB;

            //撈取其他的資料庫相同table
            //ken,簡易防止sql injection(基本上這兩個值不應該從UI傳進來)
            string tmp = dbType.Length > 20 ? dbType.Substring(0, 20) : dbType;
            _dbType = tmp.Replace("'", "").Replace("--", "").Replace(";", "");
        }

        public bool Insert(DateTime OCFUPD_PREV_DATE,DateTime OCFUPD_ORIG_DATE,DateTime OCFUPD_OCF_DATE,string OCFUPD_W_USER_ID) {
            object[] parms = {
                "@OCFUPD_PREV_DATE",OCFUPD_PREV_DATE,
                "@OCFUPD_ORIG_DATE",OCFUPD_ORIG_DATE,
                "@OCFUPD_OCF_DATE",OCFUPD_OCF_DATE,
                "@OCFUPD_W_USER_ID",OCFUPD_W_USER_ID
            };

            #region sql

            string sql = string.Format(@"
                insert into {0}.OCFUPD
				values(@OCFUPD_PREV_DATE,
                            @OCFUPD_ORIG_DATE,
                            @OCFUPD_OCF_DATE,
                            'T',
                            @OCFUPD_W_USER_ID,
                            sysdate)", _dbType);



            #endregion

            int executeResult = db.ExecuteSQL(sql);

            if (executeResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public bool Delete()
        {
            #region sql

            string sql = string.Format(
                @"
                        delete  {0}.OCFUPD 
                ", _dbType);

            #endregion sql

            int executeResult = db.ExecuteSQL(sql);

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
