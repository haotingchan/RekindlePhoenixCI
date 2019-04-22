using BusinessObjects;
using BusinessObjects.Enums;
using Common.Config;
using OnePiece;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataObjects.Dao {
   public class DataGate {
      protected Db db;

      public DataGate() {
         db = GlobalDaoSetting.DB;
      }

      public DataGate(ConnectionInfo connectionInfo) {
         db = new Db(connectionInfo.ConnectionString, connectionInfo.ProviderName, connectionInfo.Database);
      }

      public ResultData SaveForChanged(DataTable inputDT, string tableName, string insertColumnList, string updateColumnList, string updateOrDeleteKeysColumnList, PokeBall pokeBall) {
         ResultData myResultData = new ResultData();

         pokeBall.TrackedDataTables.Add(inputDT);

         int myReturnValue = 0;

         #region 參數轉換

         List<string> myInsertColumnList = new List<string>();

         if (!string.IsNullOrEmpty(insertColumnList)) {
            foreach (string everyStr in insertColumnList.Split(',')) {
               myInsertColumnList.Add(everyStr.Trim());
            }
         }

         List<string> myUpdateColumnList = new List<string>();

         foreach (string everyStr in updateColumnList.Split(',')) {
            myUpdateColumnList.Add(everyStr.Trim());
         }

         List<string> myUpdateOrDeleteKeysColumnList = new List<string>();

         foreach (string everyStr in updateOrDeleteKeysColumnList.Split(',')) {
            myUpdateOrDeleteKeysColumnList.Add(everyStr.Trim());
         }

         #endregion

         DbConnection conn = db.CreateConnection();

         try {
            DataTable myChangeDT = inputDT.GetChanges();

            myResultData.ChangedDataTable = myChangeDT;

            if (myChangeDT != null) {
               if (myChangeDT.Rows.Count == 0) {
                  throw new Exception("傳入的DataTable內無任何資料");
               }

               foreach (DataRow everyRow in myChangeDT.Rows) {
                  switch (everyRow.RowState) {
                     case DataRowState.Added:
                        #region Insert

                        #region params

                        List<object> myParamsNameAndValue = new List<object>();

                        foreach (string paramName in myInsertColumnList) {
                           myParamsNameAndValue.Add(paramName);
                           myParamsNameAndValue.Add(everyRow[paramName]);
                        }

                        object[] parms = myParamsNameAndValue.ToArray();

                        #endregion

                        #region sql

                        string sql = "INSERT INTO " + tableName + " (";

                        for (int i = 0; i < parms.Length; i = i + 2) {
                           sql += parms[i] + ",";
                        }

                        sql = sql.TrimEnd(',');

                        sql += ") VALUES (";

                        for (int i = 0; i < parms.Length; i = i + 2) {
                           sql += "@" + parms[i] + ",";
                        }

                        sql = sql.TrimEnd(',') + ")";

                        #endregion

                        myReturnValue = db.ExecuteSQL(sql, parms);

                        if (myReturnValue <= 0) {
                           throw new Exception("INSERT失敗，" + sql);
                        }

                        #endregion
                        break;

                     case DataRowState.Deleted:
                        #region Delete

                        string myDeletedKeyAndValueStr = "";

                        #region params

                        myParamsNameAndValue = new List<object>();

                        foreach (string paramName in myUpdateOrDeleteKeysColumnList) {
                           myParamsNameAndValue.Add(paramName);
                           myParamsNameAndValue.Add(everyRow[paramName, DataRowVersion.Original]);

                           myDeletedKeyAndValueStr += paramName + ":" + everyRow[paramName, DataRowVersion.Original].ToString().Trim() + Environment.NewLine;
                        }

                        parms = myParamsNameAndValue.ToArray();

                        #endregion

                        #region sql

                        sql = "DELETE FROM " + tableName + " WHERE ";

                        foreach (string everyKey in myUpdateOrDeleteKeysColumnList) {
                           sql += " " + everyKey + "=" + "@" + everyKey + " AND";
                        }

                        sql = sql.TrimEnd("AND".ToArray());

                        #endregion

                        myReturnValue = db.ExecuteSQL(sql, parms);

                        if (myReturnValue <= 0) {
                           throw new Exception("DELETE失敗，找不到此筆紀錄" + Environment.NewLine + myDeletedKeyAndValueStr);
                        }

                        #endregion
                        break;

                     case DataRowState.Detached:
                        break;

                     case DataRowState.Modified:
                        #region Update

                        string myModiedKeyAndValueStr = "";

                        #region params

                        myParamsNameAndValue = new List<object>();

                        foreach (string paramName in myUpdateColumnList) {
                           if (!myParamsNameAndValue.Contains(paramName)) {
                              myParamsNameAndValue.Add(paramName);
                              myParamsNameAndValue.Add(everyRow[paramName]);
                           }
                        }

                        foreach (string paramName in myUpdateOrDeleteKeysColumnList) {
                           // KEY值前面加上KEY_來判斷並給予DataTable裡面原本還沒修改的值
                           myParamsNameAndValue.Add("KEY_" + paramName);
                           myParamsNameAndValue.Add(everyRow[paramName, DataRowVersion.Original]);

                           myModiedKeyAndValueStr += paramName + ":" + everyRow[paramName].ToString().Trim() + Environment.NewLine;
                        }

                        parms = myParamsNameAndValue.ToArray();

                        #endregion

                        #region sql

                        sql = "UPDATE " + tableName + " SET ";

                        foreach (string everyKey in myUpdateColumnList) {
                           sql += everyKey + "=" + "@" + everyKey + ",";
                        }

                        sql = sql.TrimEnd(',') + " WHERE ";

                        foreach (string everyKey in myUpdateOrDeleteKeysColumnList) {
                           sql += " " + everyKey + "=" + "@" + "KEY_" + everyKey + " AND";
                        }

                        sql = sql.TrimEnd("AND".ToArray());

                        #endregion

                        myReturnValue = db.ExecuteSQL(sql, parms);

                        if (myReturnValue <= 0) {
                           throw new Exception("UPDATE失敗，找不到此筆紀錄" + Environment.NewLine + myModiedKeyAndValueStr);
                        }

                        #endregion
                        break;

                     case DataRowState.Unchanged:
                        break;
                     default:
                        break;
                  }
               }

               DataView dvChanged = new DataView(myChangeDT);
               myResultData.ChangedDataView = dvChanged;

               DataView dvAdded = new DataView(myChangeDT);
               dvAdded.RowStateFilter = DataViewRowState.Added;
               myResultData.ChangedDataViewForAdded = dvAdded;

               //ken,架構已調整
               //DataView dvDeleted = new DataView(myChangeDT);
               //dvDeleted.RowStateFilter = DataViewRowState.Deleted;
               //myResultData.ChangedDataViewForDeleted = dvDeleted;

               DataView dvModified = new DataView(myChangeDT);
               dvModified.RowStateFilter = DataViewRowState.ModifiedCurrent;
               myResultData.ChangedDataViewForModified = dvModified;
            } else {
               throw new Exception("無資料需要儲存");
            }

            myResultData.Status = ResultStatus.Success;
         } catch (Exception ex) {
            myReturnValue = -1;
            myResultData.Status = ResultStatus.Fail;

            throw ex;
         } finally {

         }

         myResultData.ReturnObject = myReturnValue;

         return myResultData;
      }

      public ResultData SaveForChanged(DataTable inputDT, string tableName, string insertColumnList, string updateColumnList, string updateOrDeleteKeysColumnList) {
         ResultData myResultData = new ResultData();

         //pokeBall.TrackedDataTables.Add(inputDT);

         int myReturnValue = 0;

         #region 參數轉換

         List<string> myInsertColumnList = new List<string>();

         if (!string.IsNullOrEmpty(insertColumnList)) {
            foreach (string everyStr in insertColumnList.Split(',')) {
               myInsertColumnList.Add(everyStr.Trim());
            }
         }

         List<string> myUpdateColumnList = new List<string>();

         foreach (string everyStr in updateColumnList.Split(',')) {
            myUpdateColumnList.Add(everyStr.Trim());
         }

         List<string> myUpdateOrDeleteKeysColumnList = new List<string>();

         foreach (string everyStr in updateOrDeleteKeysColumnList.Split(',')) {
            myUpdateOrDeleteKeysColumnList.Add(everyStr.Trim());
         }

         #endregion

         DbConnection conn = db.CreateConnection();

         try {
            DataTable myChangeDT = inputDT.GetChanges();

            myResultData.ChangedDataTable = myChangeDT;

            if (myChangeDT != null) {
               if (myChangeDT.Rows.Count == 0) {
                  throw new Exception("傳入的DataTable內無任何資料");
               }

               foreach (DataRow everyRow in myChangeDT.Rows) {
                  switch (everyRow.RowState) {
                     case DataRowState.Added:
                        #region Insert

                        #region params

                        List<object> myParamsNameAndValue = new List<object>();

                        foreach (string paramName in myInsertColumnList) {
                           myParamsNameAndValue.Add(paramName);
                           myParamsNameAndValue.Add(everyRow[paramName]);
                        }

                        object[] parms = myParamsNameAndValue.ToArray();

                        #endregion

                        #region sql

                        string sql = "INSERT INTO " + tableName + " (";

                        for (int i = 0; i < parms.Length; i = i + 2) {
                           sql += parms[i] + ",";
                        }

                        sql = sql.TrimEnd(',');

                        sql += ") VALUES (";

                        for (int i = 0; i < parms.Length; i = i + 2) {
                           sql += "@" + parms[i] + ",";
                        }

                        sql = sql.TrimEnd(',') + ")";

                        #endregion

                        myReturnValue = db.ExecuteSQL(sql, parms);

                        if (myReturnValue <= 0) {
                           throw new Exception("INSERT失敗，" + sql);
                        }

                        #endregion
                        break;

                     case DataRowState.Deleted:
                        #region Delete

                        string myDeletedKeyAndValueStr = "";

                        #region params

                        myParamsNameAndValue = new List<object>();

                        foreach (string paramName in myUpdateOrDeleteKeysColumnList) {
                           myParamsNameAndValue.Add(paramName);
                           myParamsNameAndValue.Add(everyRow[paramName, DataRowVersion.Original]);

                           myDeletedKeyAndValueStr += paramName + ":" + everyRow[paramName, DataRowVersion.Original].ToString().Trim() + Environment.NewLine;
                        }

                        parms = myParamsNameAndValue.ToArray();

                        #endregion

                        #region sql

                        sql = "DELETE FROM " + tableName + " WHERE ";

                        foreach (string everyKey in myUpdateOrDeleteKeysColumnList) {
                           sql += " " + everyKey + "=" + "@" + everyKey + " AND";
                        }

                        sql = sql.TrimEnd("AND".ToArray());

                        #endregion

                        myReturnValue = db.ExecuteSQL(sql, parms);

                        if (myReturnValue <= 0) {
                           throw new Exception("DELETE失敗，找不到此筆紀錄" + Environment.NewLine + myDeletedKeyAndValueStr);
                        }

                        #endregion
                        break;

                     case DataRowState.Detached:
                        break;

                     case DataRowState.Modified:
                        #region Update

                        string myModiedKeyAndValueStr = "";

                        #region params

                        myParamsNameAndValue = new List<object>();

                        foreach (string paramName in myUpdateColumnList) {
                           if (!myParamsNameAndValue.Contains(paramName)) {
                              myParamsNameAndValue.Add(paramName);
                              myParamsNameAndValue.Add(everyRow[paramName]);
                           }
                        }

                        foreach (string paramName in myUpdateOrDeleteKeysColumnList) {
                           // KEY值前面加上KEY_來判斷並給予DataTable裡面原本還沒修改的值
                           myParamsNameAndValue.Add("KEY_" + paramName);
                           myParamsNameAndValue.Add(everyRow[paramName, DataRowVersion.Original]);

                           myModiedKeyAndValueStr += paramName + ":" + everyRow[paramName].ToString().Trim() + Environment.NewLine;
                        }

                        parms = myParamsNameAndValue.ToArray();

                        #endregion

                        #region sql

                        sql = "UPDATE " + tableName + " SET ";

                        foreach (string everyKey in myUpdateColumnList) {
                           sql += everyKey + "=" + "@" + everyKey + ",";
                        }

                        sql = sql.TrimEnd(',') + " WHERE ";

                        foreach (string everyKey in myUpdateOrDeleteKeysColumnList) {
                           sql += " " + everyKey + "=" + "@" + "KEY_" + everyKey + " AND";
                        }

                        sql = sql.TrimEnd("AND".ToArray());

                        #endregion

                        myReturnValue = db.ExecuteSQL(sql, parms);

                        if (myReturnValue <= 0) {
                           throw new Exception("UPDATE失敗，找不到此筆紀錄" + Environment.NewLine + myModiedKeyAndValueStr);
                        }

                        #endregion
                        break;

                     case DataRowState.Unchanged:
                        break;
                     default:
                        break;
                  }
               }

               DataView dvChanged = new DataView(myChangeDT);
               myResultData.ChangedDataView = dvChanged;

               DataView dvAdded = new DataView(myChangeDT);
               dvAdded.RowStateFilter = DataViewRowState.Added;
               myResultData.ChangedDataViewForAdded = dvAdded;

               //ken,架構已調整
               //DataView dvDeleted = new DataView(myChangeDT);
               //dvDeleted.RowStateFilter = DataViewRowState.Deleted;
               //myResultData.ChangedDataViewForDeleted = dvDeleted;

               DataView dvModified = new DataView(myChangeDT);
               dvModified.RowStateFilter = DataViewRowState.ModifiedCurrent;
               myResultData.ChangedDataViewForModified = dvModified;
            } else {
               throw new Exception("無資料需要儲存");
            }

            myResultData.Status = ResultStatus.Success;
         } catch (Exception ex) {
            myReturnValue = -1;
            myResultData.Status = ResultStatus.Fail;

            throw ex;
         } finally {

         }

         myResultData.ReturnObject = myReturnValue;

         return myResultData;
      }

      public ResultData SaveForAll(DataTable inputDT, string tableName, string insertColumnList, string updateColumnList, string updateOrDeleteKeysColumnList) {
         ResultData myResultData = new ResultData();

         //pokeBall.TrackedDataTables.Add(inputDT);

         int myReturnValue = 0;

         #region 參數轉換

         List<string> myInsertColumnList = new List<string>();

         if (!string.IsNullOrEmpty(insertColumnList)) {
            foreach (string everyStr in insertColumnList.Split(',')) {
               myInsertColumnList.Add(everyStr.Trim());
            }
         }

         List<string> myUpdateColumnList = new List<string>();

         foreach (string everyStr in updateColumnList.Split(',')) {
            myUpdateColumnList.Add(everyStr.Trim());
         }

         List<string> myUpdateOrDeleteKeysColumnList = new List<string>();

         foreach (string everyStr in updateOrDeleteKeysColumnList.Split(',')) {
            myUpdateOrDeleteKeysColumnList.Add(everyStr.Trim());
         }

         #endregion

         DbConnection conn = db.CreateConnection();

         try {
            if (inputDT != null) {
               if (inputDT.Rows.Count == 0) {
                  throw new Exception("傳入的DataTable內無任何資料" + Environment.NewLine + "函式:SaveForAll");
               }

               foreach (DataRow everyRow in inputDT.Rows) {
                  #region 先SELECT看有沒有此筆

                  // 組SQL
                  string sql = "";
                  sql = "SELECT * FROM " + tableName + " WHERE ";

                  foreach (string everyKey in myUpdateOrDeleteKeysColumnList) {
                     sql += " " + everyKey + "=" + "@" + everyKey + " AND";
                  }

                  sql = sql.TrimEnd("AND".ToArray());

                  // 參數
                  List<object> paramWhere = new List<object>();

                  foreach (string paramName in myUpdateOrDeleteKeysColumnList) {
                     if (!paramWhere.Contains(paramName)) {
                        paramWhere.Add(paramName);
                        paramWhere.Add(everyRow[paramName]);
                     }
                  }

                  DataTable dtResult = db.GetDataTable(sql, paramWhere.ToArray());

                  #endregion

                  if (dtResult.Rows.Count != 0) {
                     #region 已存在，下UPDATE

                     string myModiedKeyAndValueStr = "";

                     #region params

                     List<object> myParamsNameAndValue = new List<object>();

                     foreach (string paramName in myUpdateColumnList) {
                        if (!myParamsNameAndValue.Contains(paramName)) {
                           myParamsNameAndValue.Add(paramName);
                           myParamsNameAndValue.Add(everyRow[paramName]);
                        }
                     }

                     foreach (string paramName in myUpdateOrDeleteKeysColumnList) {
                        if (!myParamsNameAndValue.Contains(paramName)) {
                           myParamsNameAndValue.Add(paramName);
                           myParamsNameAndValue.Add(everyRow[paramName]);
                        }

                        myModiedKeyAndValueStr += paramName + ":" + everyRow[paramName].ToString().Trim() + Environment.NewLine;
                     }

                     object[] parms = myParamsNameAndValue.ToArray();

                     #endregion

                     #region sql

                     sql = "UPDATE " + tableName + " SET ";

                     foreach (string everyKey in myUpdateColumnList) {
                        sql += everyKey + "=" + "@" + everyKey + ",";
                     }

                     sql = sql.TrimEnd(',') + " WHERE ";

                     foreach (string everyKey in myUpdateOrDeleteKeysColumnList) {
                        sql += " " + everyKey + "=" + "@" + everyKey + " AND";
                     }

                     sql = sql.TrimEnd("AND".ToArray());

                     #endregion

                     myReturnValue = db.ExecuteSQL(sql, parms);

                     #endregion
                  } else {
                     #region 不存在，下INSERT

                     if (myInsertColumnList.Count == 0) {
                        throw new Exception("沒有設定insertColumnList參數無法新增");
                     }

                     #region params

                     List<object> myParamsNameAndValue = new List<object>();

                     foreach (string paramName in myInsertColumnList) {
                        myParamsNameAndValue.Add(paramName);
                        myParamsNameAndValue.Add(everyRow[paramName]);
                     }

                     object[] parms = myParamsNameAndValue.ToArray();

                     #endregion

                     #region sql

                     sql = "INSERT INTO " + tableName + " (";

                     for (int i = 0; i < parms.Length; i = i + 2) {
                        sql += parms[i] + ",";
                     }

                     sql = sql.TrimEnd(',');

                     sql += ") VALUES (";

                     for (int i = 0; i < parms.Length; i = i + 2) {
                        sql += "@" + parms[i] + ",";
                     }

                     sql = sql.TrimEnd(',') + ")";

                     #endregion

                     myReturnValue = db.ExecuteSQL(sql, parms);

                     #endregion
                  }
               }
            } else {
               throw new Exception("無資料需要儲存");
            }

            myResultData.Status = ResultStatus.Success;
         } catch (Exception ex) {
            myReturnValue = -1;
            myResultData.Status = ResultStatus.Fail;
            throw ex;
         } finally {

         }

         myResultData.ReturnObject = myReturnValue;

         return myResultData;
      }

      public ResultData MultiActionTransaction(Func<PokeBall, ResultStatus> inputFunc) {
         ResultData result = new ResultData();

         ResultStatus resultStatus = ResultStatus.Fail;

         using (var connection = db.CreateConnection()) {
            DbTransaction tran = connection.BeginTransaction();

            db.StartTransaction(connection, tran);

            try {
               PokeBall pokeBall = new PokeBall();
               resultStatus = inputFunc(pokeBall);

               if (resultStatus == ResultStatus.Fail) {
                  throw new Exception("儲存錯誤!");
               }

               tran.Commit();

               foreach (DataTable dt in pokeBall.TrackedDataTables) {
                  dt.AcceptChanges();
               }

               //resultStatus = ResultStatus.Success;
            } catch (Exception ex) {
               result.returnString = ex.Message;
               tran.Rollback();
               resultStatus = ResultStatus.Fail;
               throw;
            } finally {
               db.EndTransaction();
            }
         }

         result.Status = resultStatus;

         return result;
      }

      public ResultData ExecuteStoredProcedure(string sql, List<DbParameterEx> dbParmsEx, bool hasReturnParameter) {
         return db.ExecuteStoredProcedure(sql, dbParmsEx, hasReturnParameter);
      }

      /// <summary>
      /// change DB
      /// </summary>
      /// <param name="dtTXFP">get list of TXFP</param>
      /// <param name="iniKey">TXFP_TXN_ID</param>
      /// <returns></returns>
      public Db ChangeDB(DataTable dtTXFP) {
         DataTable dt = dtTXFP;

         DBInfo info = SettingDragons.Instance.GetDBInfo(dt.Rows[0]["ls_str1"].ToString().Trim());

         try {
            string ls_str2 = DeCode(dt.Rows[0]["ls_str2"].ToString());
            //string ls_srv = dt.Rows[0]["ls_srv"].ToString().Split('/')[0];

            if (string.IsNullOrEmpty(dt.Rows[0]["ls_db"].ToString().Trim())) { dt.Rows[0]["ls_db"] = ""; }
            if (string.IsNullOrEmpty(dt.Rows[0]["ls_dbparm"].ToString().Trim())) { dt.Rows[0]["ls_dbparm"] = ""; }

            string connectionString = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})" +
                "(PORT=1521)))(CONNECT_DATA=(SID=CI)(SERVER=DEDICATED))); User Id={1};Password={2};",
                info.InitialCatalog, dt.Rows[0]["ls_str1"].ToString(), ls_str2);

            return new Db(connectionString, "Oracle.ManagedDataAccess.Client", dt.Rows[0]["ls_db"].ToString().Trim());
         } catch (Exception ex) {
            throw ex;
         }
      }

      /// <summary>
      /// string decode
      /// </summary>
      /// <param name="data"></param>
      /// <returns></returns>
      public string DeCode(string data) {
         string is_out = "";
         long il_y = 0, il_len;
         byte[] il_bit;
         try {
            il_len = data.Length;

            for (int i = 0; i < il_len; i += 2) {

               il_bit = Encoding.ASCII.GetBytes(data.Substring(i, 1));
               //取前4位值
               il_y = (il_bit[0] - 64) * 16;
               //取后4位值
               il_bit = Encoding.ASCII.GetBytes(data.Substring(i + 1, 1));
               il_y = il_y + il_bit[0] - 64;
               is_out = is_out + Convert.ToChar(il_y);
            }
            return is_out;
         } catch (Exception ex) {
            throw ex;
         }
      }
   }
}
