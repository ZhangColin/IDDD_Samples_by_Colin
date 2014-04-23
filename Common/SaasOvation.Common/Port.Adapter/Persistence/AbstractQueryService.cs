using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices.ComTypes;

namespace SaasOvation.Common.Port.Adapter.Persistence {
    public abstract class AbstractQueryService {
        private readonly string _connectionString;
        private readonly DbProviderFactory _providerFactory;

        protected AbstractQueryService(string connectionString, string providerName) {
            this._connectionString = connectionString;
            this._providerFactory = DbProviderFactories.GetFactory(providerName);
        }

        protected T QueryObject<T>(string query, JoinOn joinOn, params object[] arguments) {
            using (DbConnection conn = CreateOpenConnection()) {
                using (DbCommand selectStatement = CreateCommand(conn, query, arguments)) {
                    using (IDataReader dataReader = selectStatement.ExecuteReader()) {
                        if (dataReader.Read()) {
                            return new ResultSetObjectMapper<T>(dataReader, joinOn).MapResultToType();
                        }
                        else {
                            return default(T);
                        }
                    }
                }
            }
        }

        protected IList<T> QueryObjects<T>(string query, JoinOn joinOn, params object[] arguments) {
            using(DbConnection conn = CreateOpenConnection()) {
                using(DbCommand selectStatement = CreateCommand(conn, query, arguments)) {
                    using(IDataReader dataReader = selectStatement.ExecuteReader()) {
                        List<T> objects = new List<T>();
                        while(dataReader.Read()) {
                            objects.Add(new ResultSetObjectMapper<T>(dataReader, joinOn).MapResultToType());
                        }

                        return objects;
                    }
                }
            }
        }

        protected string QueryString(string query, params object[] arguments) {
            using(DbConnection conn = CreateOpenConnection()) {
                using(DbCommand selectStatement = CreateCommand(conn, query, arguments)) {
                    using(IDataReader dataReader = selectStatement.ExecuteReader()) {
                        if(dataReader.Read()) {
                            return dataReader.GetString(0);
                        }
                        else {
                            return null;
                        }
                    }
                }
            }
        }

        private DbCommand CreateCommand(DbConnection conn, string query, object[] arguments) {
            DbCommand command = conn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = query;

            for(int i = 0; i < arguments.Length; i++) {
                object argument = arguments[i];
                DbParameter parameter = command.CreateParameter();
                parameter.Value = argument;

                command.Parameters.Add(parameter);
            }

            return command;
        }

        private DbConnection CreateOpenConnection() {
            DbConnection conn = this._providerFactory.CreateConnection();
            conn.ConnectionString = this._connectionString;
            conn.Open();
            return conn;
        }
    }
}