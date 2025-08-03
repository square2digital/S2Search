using S2Search.Common.Database.Sql.Dapper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace S2Search.Common.Database.Sql.Dapper.Interfaces.Providers
{
    public interface IDbContextProvider
    {
        int Execute(string connectionString, string procedureName, object parameters);
        Task<int> ExecuteAsync(string connectionString, string procedureName, object parameters);
        T ExecuteScalar<T>(string connectionString, string procedureName, object parameters);
        Task<T> ExecuteScalarAsync<T>(string connectionString, string procedureName, object parameters);
        IEnumerable<T> Query<T>(string connectionString, string procedureName, object parameters);
        T QueryFirstOrDefault<T>(string connectionString, string procedureName, object parameters);
        T QuerySingleOrDefault<T>(string connectionString, string procedureName, object parameters);
        Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string procedureName, object parameters);
        Task<T> QueryFirstOrDefaultAsync<T>(string connectionString, string procedureName, object parameters);
        Task<T> QuerySingleOrDefaultAsync<T>(string connectionString, string procedureName, object parameters);
        T QueryMultiple<T>(string connectionString, string procedureName, object parameters);
        Task<T> QueryMultipleAsync<T>(string connectionString, string procedureName, object parameters);
    }
}
