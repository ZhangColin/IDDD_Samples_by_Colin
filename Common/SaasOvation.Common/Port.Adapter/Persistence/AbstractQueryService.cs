using System.Data.Common;

namespace SaasOvation.Common.Port.Adapter.Persistence {
    public abstract class AbstractQueryService {
        private readonly string _connectionString;

        protected AbstractQueryService(string connectionString) {
            this._connectionString = connectionString;
        }

        protected DbConnection GetConnection() {
            return Database.GetConnection(_connectionString);
        }
    }
}