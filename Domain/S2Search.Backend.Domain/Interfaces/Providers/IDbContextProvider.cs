namespace S2Search.Backend.Domain.Interfaces.Providers;

public interface IDbContextProvider
{
    int Execute(string connectionString, string procedureName, Dictionary<string, object> parameters);
    Task<int> ExecuteAsync(string connectionString, string procedureName, Dictionary<string, object> parameters);
    T ExecuteScalar<T>(string connectionString, string procedureName, Dictionary<string, object> parameters);
    Task<T> ExecuteScalarAsync<T>(string connectionString, string procedureName, Dictionary<string, object> parameters);
    IEnumerable<T> Query<T>(string connectionString, string procedureName, Dictionary<string, object> parameterss);
    T QueryFirstOrDefault<T>(string connectionString, string procedureName, Dictionary<string, object> parameters);
    T QuerySingleOrDefault<T>(string connectionString, string procedureName, Dictionary<string, object> parameters);
    Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string procedureName, Dictionary<string, object> parameters);
    Task<T> QueryFirstOrDefaultAsync<T>(string connectionString, string procedureName, Dictionary<string, object> parameters);
    Task<T> QuerySingleOrDefaultAsync<T>(string connectionString, string procedureName, Dictionary<string, object> parameters);
    T QueryMultiple<T>(string connectionString, string procedureName, Dictionary<string, object> parameters);
    Task<T> QueryMultipleAsync<T>(string connectionString, string procedureName, Dictionary<string, object> parameters);
}
