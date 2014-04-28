using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace SaasOvation.Common.Port.Adapter.Persistence {
    public class Database {
        public static SqlConnection GetConnection() {
            return
                GetConnection(ConfigurationManager.ConnectionStrings["CartisanDapperConnectionString"].ConnectionString);
        }

        public static SqlConnection GetConnection(string connectionString) {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// 检查表中指定字段是否存在指定的值
        /// </summary>
        /// <typeparam name="TCompare">值类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="compareFieldName">字段名</param>
        /// <param name="compareValue">比较值</param>
        /// <returns></returns>
        public static bool Duplicate<TCompare>(string tableName, string compareFieldName, TCompare compareValue) {
            const string duplicateSql = "select count(1) as DuplicateCount from {0} where {1} = @compareValue";
            using (var connection = GetConnection()) {
                var result = connection.Query(string.Format(duplicateSql, tableName, compareFieldName), new {
                    compareValue
                }).Single();

                return result.DuplicateCount != 0;
            }
        }

        /// <summary>
        /// 检查表中指定范围外是否存在指定的值
        /// </summary>
        /// <typeparam name="TIdentity">主键类型</typeparam>
        /// <typeparam name="TCompare">值的类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="idName">主键字段名</param>
        /// <param name="compareFieldName">比较字段名</param>
        /// <param name="idValue">主键</param>
        /// <param name="compareValue">比较值</param>
        /// <returns></returns>
        public static bool DuplicateOutScope<TIdentity, TCompare>(string tableName, string idName, string compareFieldName, TIdentity idValue, TCompare compareValue) {
            const string duplicateSql = "select count(1) as DuplicateCount from {0} where {2} = @compareValue and {1} <> @idValue";
            using (var connection = GetConnection()) {
                var result = connection.Query(string.Format(duplicateSql, tableName, idName, compareFieldName), new {
                    idValue,
                    compareValue
                }).Single();

                return result.DuplicateCount != 0;
            }
        }

        /// <summary>
        /// 检查表中指定范围外是否存在指定的值
        /// </summary>
        /// <typeparam name="TIdentity">主键类型</typeparam>
        /// <typeparam name="TCompare">值的类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="idName">主键字段名</param>
        /// <param name="compareFieldName">比较字段名</param>
        /// <param name="idValue">主键</param>
        /// <param name="compareValue">比较值</param>
        /// <returns></returns>
        public static bool DuplicateInScope<TIdentity, TCompare>(string tableName, string idName, string compareFieldName, TIdentity idValue, TCompare compareValue) {
            const string duplicateSql = "select count(1) as DuplicateCount from {0} where {2} = @compareValue and {1} = @idValue";
            using (var connection = GetConnection()) {
                var result = connection.Query(string.Format(duplicateSql, tableName, idName, compareFieldName), new {
                    idValue,
                    compareValue
                }).Single();

                return result.DuplicateCount != 0;
            }
        }
    }
}