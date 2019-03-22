using BusinessObjects;
using BusinessObjects.Enums;
using Oracle.ManagedDataAccess.Client;
using Sybase.Data.AseClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace OnePiece {
   public class Db {
      private DbProviderFactory factory = null;

      private string gDatabaseName = "";

      public string gConnectionString = "";

      public DbConnection dbConnection { get; set; }

      public DbTransaction dbTransaction { get; set; }

      public bool IsTransaction { get; set; } = false;

      public Db(string connectionString , string providerName , string database) {
         gDatabaseName = database;

         gConnectionString = connectionString;

         // 設定DbProvider
         factory = DbProviderFactoriesEx.GetFactory(providerName);
      }

      public void StartTransaction(DbConnection connection , DbTransaction tran) {
         dbConnection = connection;
         dbTransaction = tran;
         IsTransaction = true;
      }

      public void EndTransaction() {
         dbConnection.Close();
         dbConnection = null;
         dbTransaction = null;
         IsTransaction = false;
      }

      public DataTable GetDataTable(string sql , params object[] parms) {
         DataTable dt = new DataTable();

         var connection = CreateConnection();

         using (var command = CreateCommand(sql , connection , CommandType.Text , parms)) {
            // dt.Load會把來源的Table的Schema都寫到DataTable裡面
            // 像是某個欄位AllowDbNull為False
            //using (var reader = command.ExecuteReader())
            //{
            //    dt.Load(reader);
            //}

            // Fill的方法不會把來源Table的Schema寫到DataTable，除非用FillSchema
            using (var adapter = CreateAdapter(command)) {
               adapter.Fill(dt);
            }
         }

         // 如果是外部Transaction的話這裡會有值，就先不要關閉連線
         if (!IsTransaction) {
            connection.Close();
         }

         dt.TableName = "Default";

         return dt;
      }

      public DataTable GetDataTableForTransaction(string sql , params object[] parms) {
         DataTable dt = new DataTable();

         using (var connection = CreateConnection()) {
            DbTransaction tran = connection.BeginTransaction();

            try {
               using (DbCommand command = CreateCommand(sql , connection , CommandType.Text , parms)) {
                  command.Transaction = tran;

                  // dt.Load會把來源的Table的Schema都寫到DataTable裡面
                  // 像是某個欄位AllowDbNull為False
                  //using (var reader = command.ExecuteReader())
                  //{
                  //    dt.Load(reader);
                  //}

                  // Fill的方法不會把來源Table的Schema寫到DataTable，除非用FillSchema
                  using (var adapter = CreateAdapter(command)) {
                     adapter.Fill(dt);
                  }
               }

               tran.Commit();
            } catch (Exception ex) {
               tran.Rollback();
               throw ex;
            }

            dt.TableName = "Default";

            return dt;
         }
      }

      public DbDataAdapter GetDataAdapter(string sql , params object[] parms) {
         var connection = CreateConnection();
         var command = CreateCommand(sql , connection , CommandType.Text , parms);
         var adapter = CreateAdapter(command);
         return adapter;
      }

      public DbCommandBuilder GetCommandBuilder(DbDataAdapter adapter) {
         var builder = CreateCommandBuilder(adapter);
         return builder;
      }

      public int ExecuteSQL(string sql , params object[] parms) {
         try {
            var connection = CreateConnection();

            using (var command = CreateCommand(sql , connection , CommandType.Text , parms)) {
               int myResult = command.ExecuteNonQuery();

               // 如果是外部Transaction的話這裡會有值，就先不要關閉連線
               if (!IsTransaction) {
                  connection.Close();
               }

               return myResult;
            }
         } catch (Exception ex) {
            throw ExceptionHelper.TranformException(ex);
         }
      }

      public int ExecuteSQLForTransaction(string sql , params object[] parms) {
         int myResult = 0;

         using (var connection = CreateConnection()) {
            DbTransaction tran = connection.BeginTransaction();

            try {
               using (var command = CreateCommand(sql , connection , CommandType.Text , parms)) {
                  command.Transaction = tran;
                  myResult = command.ExecuteNonQuery();
               }

               tran.Commit();
            } catch {
               tran.Rollback();
            }

            return myResult;
         }
      }

      /// <summary>
      /// call StoredProcedure 但是參數可以有一個int16的output參數
      /// </summary>
      /// <param name="sql"></param>
      /// <param name="parameters"></param>
      /// <param name="hasReturnParameter"></param>
      /// <returns></returns>
      public ResultData ExecuteStoredProcedure(string sql , List<DbParameterEx> parameters , bool hasReturnParameter) {
         ResultData resultData = new ResultData();
         resultData.Status = ResultStatus.Fail;

         try {
            var connection = CreateConnection();

            using (var command = CreateCommand(sql , connection , CommandType.StoredProcedure)) {
               if (parameters != null) {
                  foreach (DbParameterEx everyPara in parameters) {
                     DbParameter dbParam = command.CreateParameter();

                     dbParam = TransformToDbParameter(dbParam , everyPara);

                     command.Parameters.Add(dbParam);
                  }
               }

               DbParameter dbParamReturn = null;

               if (hasReturnParameter) {
                  if (command is OracleCommand) {
                     dbParamReturn = command.CreateParameter();
                     dbParamReturn.ParameterName = "RETURNPARAMETER";
                     dbParamReturn.Direction = ParameterDirection.Output;
                     dbParamReturn.DbType = DbType.Int16;
                     command.Parameters.Add(dbParamReturn);
                  } else if (command is AseCommand) {
                     dbParamReturn = command.CreateParameter();
                     dbParamReturn.Direction = ParameterDirection.ReturnValue;
                     dbParamReturn.DbType = DbType.Int32;
                     command.Parameters.Add(dbParamReturn);
                  }
               }

               using (var adapter = CreateAdapter(command)) {
                  DataTable dt = new DataTable();
                  adapter.Fill(dt);
                  resultData.ReturnData = dt;

                  if (dbParamReturn != null) {
                     resultData.ReturnObject = dbParamReturn.Value;
                  }
               }

               int returnValueInt = 0;
               int.TryParse((resultData.ReturnObject is null) ? "0" : resultData.ReturnObject.ToString() , out returnValueInt);

               if (returnValueInt == 0) {
                  resultData.Status = ResultStatus.Success;
               } else {
                  resultData.Status = ResultStatus.Fail;
               }

               // 如果是外部Transaction的話這裡會有值，就先不要關閉連線
               if (!IsTransaction) {
                  connection.Close();
               }

               return resultData;
            }
         } catch (Exception ex) {
            string errorStr = "";

            if (ex is AseException) {
               AseException aseEx = ((AseException)ex);

               foreach (AseError error in aseEx.Errors) {
                  errorStr += Environment.NewLine + error.ProcName + Environment.NewLine +
                              error.MessageNumber + Environment.NewLine +
                              "LineNum:" + error.LineNum;
               }
            }

            Exception exNew = new Exception(ExceptionHelper.TranformException(ex).Message + errorStr);

            throw exNew;
         }
      }

      public IEnumerable<T> Read<T>(string sql , Func<IDataReader , T> make , params object[] parms) {
         using (var connection = CreateConnection()) {
            using (var command = CreateCommand(sql , connection , CommandType.Text , parms)) {
               using (var reader = command.ExecuteReader()) {
                  while (reader.Read()) {
                     yield return make(reader);
                  }
               }
            }
         }
      }

      public DbConnection CreateConnection() {
         DbConnection connection;

         if (dbConnection == null) {
            connection = factory.CreateConnection();

            connection.ConnectionString = gConnectionString;

            connection.Open();

            if (gDatabaseName != "") {
               if (!(factory is Oracle.ManagedDataAccess.Client.OracleClientFactory)) {
                  connection.ChangeDatabase(gDatabaseName);
               }
            }
         } else {
            connection = dbConnection;
         }

         return connection;
      }

      private DbCommand CreateCommand(string sql , DbConnection conn , CommandType cmdType , params object[] parms) {
         var command = factory.CreateCommand();
         command.Connection = conn;

         if (parms != null && parms.Length > 0) {
            if ((factory is OracleClientFactory)) {
               ((OracleCommand)command).BindByName = true;

               sql = sql.Replace("@" , ":");

               for (int i = 0 ; i < parms.Length ; i += 2) {
                  parms[i] = parms[i].ToString().Replace("@" , "");
               }
            }
         }

         command.CommandText = sql;
         command.CommandType = cmdType;
         command.AddParameters(factory , parms);

         if (dbTransaction != null) {
            command.Transaction = dbTransaction;
         }

         return command;
      }

      private DbCommand CreateCommand(string sql , DbConnection conn , CommandType cmdType) {
         var command = factory.CreateCommand();
         command.Connection = conn;
         command.CommandText = sql;
         command.CommandType = cmdType;

         if (dbTransaction != null) {
            command.Transaction = dbTransaction;
         }

         return command;
      }

      private DbDataAdapter CreateAdapter(DbCommand command) {
         var adapter = factory.CreateDataAdapter();
         adapter.SelectCommand = command;
         return adapter;
      }

      private DbCommandBuilder CreateCommandBuilder(DbDataAdapter adapter) {
         var builder = factory.CreateCommandBuilder();
         builder.DataAdapter = adapter;
         return builder;
      }

      private DbParameter TransformToDbParameter(DbParameter para , DbParameterEx paraEx) {
         para.ParameterName = paraEx.Name;
         para.Value = paraEx.Value;
         para.Direction = paraEx.Direction;

         if (paraEx.DbType == DbTypeEx.RefCursor) {
            ((OracleParameter)para).OracleDbType = OracleDbType.RefCursor;
         } else if (paraEx.DbType == DbTypeEx.None) {
            para.DbType = DbExtentions.TransformDbTypeByValue(factory , para.Value);
         } else {
            switch (paraEx.DbType) {
               case DbTypeEx.String:
                  if (factory is OracleClientFactory) {
                     para.DbType = DbType.StringFixedLength;
                  } else {
                     para.DbType = DbType.AnsiString;
                  }
                  break;

               case DbTypeEx.Int:
                  para.DbType = DbType.Int32;
                  break;

               case DbTypeEx.SmallInt:
                  para.DbType = DbType.Int16;
                  break;

               case DbTypeEx.Date:
                  para.DbType = DbType.Date;
                  break;

               default:
                  break;
            }
         }

         return para;
      }

      /// <summary>
      /// 回傳第一筆第一個欄位
      /// </summary>
      /// <param name="sql"></param>
      /// <param name="commandType"></param>
      /// <param name="parms"></param>
      /// <returns></returns>
      public string ExecuteScalar(string sql , CommandType commandType = CommandType.Text , params object[] parms) {
         try {
            var connection = CreateConnection();
            var command = CreateCommand(sql , connection , commandType , parms);
            var objReturn = command.ExecuteScalar();

            return (objReturn == null ? "" : objReturn.ToString());
         } catch (Exception ex) {
            throw ExceptionHelper.TranformException(ex);
         }
      }

      /// <summary>
      /// call StoredProcedure 但是參數裡面很單純,沒有output
      /// </summary>
      /// <param name="sql"></param>
      /// <param name="parms"></param>
      /// <returns></returns>
      public DataTable ExecuteStoredProcedure_Override(string sql , params object[] parms) {
         DataTable dt = new DataTable();
         try {
            var conn = CreateConnection();
            DbCommand command = CreateCommand(sql , conn , CommandType.StoredProcedure , parms);
            DbDataAdapter adapter = CreateAdapter(command);
            adapter.Fill(dt);
         } catch (Exception ex) {

         }
         return dt;
      }

      /// <summary>
      /// call StoredProcedure 但是參數可有output
      /// </summary>
      /// <param name="sql"></param>
      /// <param name="parameters"></param>
      /// <param name="hasReturnParameter"></param>
      /// <returns></returns>
      public DataTable ExecuteStoredProcedureEx(string sql , List<DbParameterEx> parameters , bool hasReturnParameter) {
         ResultData resultData = new ResultData();
         resultData.Status = ResultStatus.Fail;

         try {
            var connection = CreateConnection();
            OracleCommand command = new OracleCommand(sql , (OracleConnection)connection);//ken
            command.CommandType = CommandType.StoredProcedure;//ken

            if (parameters != null) {
               foreach (DbParameterEx everyPara in parameters) {
                  DbParameter dbParam = command.CreateParameter();

                  dbParam = TransformToDbParameter(dbParam , everyPara);

                  command.Parameters.Add(dbParam);
               }
            }

            DbParameter dbParamReturn = null;

            if (hasReturnParameter) {
               if (command is OracleCommand) {
                  dbParamReturn = command.CreateParameter();
                  dbParamReturn.ParameterName = "RETURNPARAMETER";
                  dbParamReturn.Direction = ParameterDirection.Output;
                  ((OracleParameter)dbParamReturn).OracleDbType = OracleDbType.RefCursor;//ken
                  command.Parameters.Add(dbParamReturn);
               }
            }

            command.ExecuteNonQuery();//ken
            OracleDataAdapter da = new OracleDataAdapter(command);//ken
            DataTable dtResult = new DataTable();
            da.Fill(dtResult);

            //ken,如果有多個output cursor,則需要用以下寫法
           //DataTable dtResult = (DataTable)command.Parameters[2];

            return dtResult;
         } catch (Exception ex) {
            string errorStr = "";

            if (ex is AseException) {
               AseException aseEx = ((AseException)ex);

               foreach (AseError error in aseEx.Errors) {
                  errorStr += Environment.NewLine + error.ProcName + Environment.NewLine +
                              error.MessageNumber + Environment.NewLine +
                              "LineNum:" + error.LineNum;
               }
            }

            Exception exNew = new Exception(ExceptionHelper.TranformException(ex).Message + errorStr);

            throw exNew;
         }
      }

        /// <summary>
        /// call StoredProcedure 但是參數可有output( not RefCursor )
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="hasReturnParameter"></param>
        /// <returns></returns>
        public string ExecuteStoredProcedureReturnString(string sql, List<DbParameterEx> parameters, bool hasReturnParameter,OracleDbType returnType) {
            string result = "";

            try {
                var connection = CreateConnection();
                OracleCommand command = new OracleCommand(sql, (OracleConnection)connection);//ken
                command.CommandType = CommandType.StoredProcedure;//ken

                if (parameters != null) {
                    foreach (DbParameterEx everyPara in parameters) {
                        DbParameter dbParam = command.CreateParameter();

                        dbParam = TransformToDbParameter(dbParam, everyPara);

                        command.Parameters.Add(dbParam);
                    }
                }

                DbParameter dbParamReturn = null;

                if (hasReturnParameter) {
                    if (command is OracleCommand) {
                        dbParamReturn = command.CreateParameter();
                        dbParamReturn.ParameterName = "RETURNPARAMETER";
                        dbParamReturn.Direction = ParameterDirection.Output;
                        ((OracleParameter)dbParamReturn).OracleDbType = returnType;//David
                        command.Parameters.Add(dbParamReturn);
                    }
                }

                command.ExecuteNonQuery();
                result= command.Parameters["RETURNPARAMETER"].Value.ToString();

                return result;
            }
            catch (Exception ex) {
                string errorStr = "";

                if (ex is AseException) {
                    AseException aseEx = ((AseException)ex);

                    foreach (AseError error in aseEx.Errors) {
                        errorStr += Environment.NewLine + error.ProcName + Environment.NewLine +
                                    error.MessageNumber + Environment.NewLine +
                                    "LineNum:" + error.LineNum;
                    }
                }

                Exception exNew = new Exception(ExceptionHelper.TranformException(ex).Message + errorStr);

                throw exNew;
            }
        }

        /// <summary>
        /// use oraclecommand update DB by sql string and update column
        /// </summary>
        /// <param name="inputDT"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public ResultData UpdateOracleDB(DataTable inputDT, string sql) {
            var connection = CreateConnection();
            OracleConnection oracleConn = (OracleConnection)connection;

            OracleCommand command = new OracleCommand(sql, oracleConn);
            OracleTransaction tran = oracleConn.BeginTransaction(IsolationLevel.ReadCommitted);
            ResultData resultData = new ResultData();
            resultData.Status = ResultStatus.Fail;
            command.Transaction = tran;

            try {
                OracleDataAdapter DataAdapter = new OracleDataAdapter();
                DataAdapter.SelectCommand = command;

                OracleCommandBuilder commandBuilder = new OracleCommandBuilder(DataAdapter);
                DataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                DataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                DataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();

                int rows = DataAdapter.Update(inputDT);

                if (rows >= 1) {
                    resultData.Status = ResultStatus.Success;
                    return resultData;
                }
                else {
                    return resultData;
                }
            }
            catch (Exception ex) {
                //#if DEBUG
                //            MessageBox.Show(ex.Message);
                //#endif
                tran.Rollback();
                throw ex;
            }
        }

    }

   public static class DbExtentions {
      public static void AddParameters(this DbCommand command , DbProviderFactory factory , object[] parms) {
         if (parms != null && parms.Length > 0) {
            for (int i = 0 ; i < parms.Length ; i += 2) {
               string name = parms[i].ToString();

               // if null, set to DbNull
               object value = parms[i + 1] ?? DBNull.Value;

               var dbParameter = command.CreateParameter();
               dbParameter.ParameterName = name;
               dbParameter.Value = value;
               dbParameter.DbType = TransformDbTypeByValue(factory , parms[i + 1]);

               command.Parameters.Add(dbParameter);
            }
         }
      }

      public static DbType TransformDbTypeByValue(DbProviderFactory factory , object val) {
         if (val is Byte) {
            return DbType.Byte;
         } else if (val is Int16) {
            return DbType.Int16;
         } else if (val is Int32) {
            return DbType.Int32;
         } else if (val is Int64) {
            return DbType.Int64;
         } else if (val is DateTime) {
            if (factory is OracleClientFactory) {
               return DbType.Date;
            } else {
               return DbType.DateTime;
            }
         } else if (val is Decimal) {
            return DbType.Decimal;
         } else if (val is Boolean) {
            return DbType.Boolean;
         } else {
            if (factory is OracleClientFactory) {
               return DbType.StringFixedLength;
            } else {
               return DbType.AnsiString;
            }
         }
      }


   }


}