using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using System.Data;
using System.Dynamic;
using S2Search.Backend.Domain.Models.Dapper;
using S2Search.Backend.Domain.Interfaces.Providers;
using S2Search.Backend.Services.Services.Admin.Dapper.Helpers;
using Dapper;
using static Dapper.SqlMapper;

namespace S2Search.Backend.Services.Services
{
    public class DbContextProvider : IDbContextProvider
    {
        private readonly IDbConnectionFactory _dbConnection;

        public DbContextProvider(IDbConnectionFactory dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public int Execute(string connectionString, string functionName, Dictionary<string, object> parameters) =>
            DapperExecute(connectionString, functionName, parameters);

        public async Task<int> ExecuteAsync(string connectionString, string functionName, Dictionary<string, object> parameters) =>
            await DapperExecuteAsync(connectionString, functionName, parameters);

        public T ExecuteScalar<T>(string connectionString, string functionName, Dictionary<string, object> parameters) =>
            DapperExecuteScalar<T>(connectionString, functionName, parameters);

        public async Task<T> ExecuteScalarAsync<T>(string connectionString, string functionName, Dictionary<string, object> parameters) =>
            await DapperExecuteScalarAsync<T>(connectionString, functionName, parameters);

        public IEnumerable<T> Query<T>(string connectionString, string functionName, Dictionary<string, object> parameters) =>
            DapperQuery<T>(connectionString, functionName, parameters);

        public async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string functionName, Dictionary<string, object> parameters) =>
            await DapperQueryAsync<T>(connectionString, functionName, parameters);

        public T QueryFirstOrDefault<T>(string connectionString, string functionName, Dictionary<string, object> parameters) =>
            DapperQueryFirstOrDefault<T>(connectionString, functionName, parameters);

        public async Task<T> QueryFirstOrDefaultAsync<T>(string connectionString, string functionName, Dictionary<string, object> parameters) =>
            await DapperQueryFirstOrDefaultAsync<T>(connectionString, functionName, parameters);

        public T QuerySingleOrDefault<T>(string connectionString, string functionName, Dictionary<string, object> parameters) =>
            DapperQuerySingleOrDefault<T>(connectionString, functionName, parameters);

        public async Task<T> QuerySingleOrDefaultAsync<T>(string connectionString, string functionName, Dictionary<string, object> parameters) =>
            await DapperQuerySingleOrDefaultAsync<T>(connectionString, functionName, parameters);

        public T QueryMultiple<T>(string connectionString, string functionName, Dictionary<string, object> parameters) =>
            DapperQueryMultiple<T>(connectionString, functionName, parameters);

        public async Task<T> QueryMultipleAsync<T>(string connectionString, string functionName, Dictionary<string, object> parameters) =>
            await DapperQueryMultipleAsync<T>(connectionString, functionName, parameters);

        private string BuildFunctionCall(string functionName, Dictionary<string, object> parameters)
        {
            if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentException("Function name must be provided.", nameof(functionName));

            // Ensure functionName contains only safe identifier characters (alphanumeric, underscore, dot)
            if (!System.Text.RegularExpressions.Regex.IsMatch(functionName, @"^[A-Za-z0-9_\.]+$"))
                throw new ArgumentException("Function name contains invalid characters.", nameof(functionName));

            if (parameters == null || parameters.Count == 0)
                return $"SELECT * FROM {functionName}()";

            var paramPlaceholders = parameters.Keys.Select(k =>
            {
                if (string.IsNullOrWhiteSpace(k))
                    throw new ArgumentException("Parameter names must be non-empty.", nameof(parameters));

                // Normalize parameter name: trim and remove leading '@' if present
                var raw = k.Trim();
                var name = raw.StartsWith("@") ? raw.Substring(1) : raw;

                var placeholder = $"@{name}";
                var lower = name.ToLowerInvariant();

                // Apply casts based on parameter name hints
                // Note: it's uncommon for a name to indicate multiple conflicting casts.
                if (lower.Contains("json"))
                    placeholder += "::jsonb";
                if (lower.Contains("date"))
                    placeholder += "::date";

                return placeholder;
            });

            var paramList = string.Join(", ", paramPlaceholders);
            var query = $"SELECT * FROM {functionName}({paramList})";

            return query;
        }

        private int DapperExecute(string connectionString, string functionName, Dictionary<string, object> parameters)
        {
            using (var connection = _dbConnection.Create(connectionString))
            {
                var sql = BuildFunctionCall(functionName, parameters);
                return connection.Execute(sql, parameters, commandType: CommandType.Text);
            }
        }

        private async Task<int> DapperExecuteAsync(string connectionString, string functionName, Dictionary<string, object> parameters)
        {
            using (var connection = _dbConnection.Create(connectionString))
            {
                var sql = BuildFunctionCall(functionName, parameters);
                return await connection.ExecuteAsync(sql, parameters, commandType: CommandType.Text);
            }
        }

        private T DapperExecuteScalar<T>(string connectionString, string functionName, Dictionary<string, object> parameters)
        {
            using (var connection = _dbConnection.Create(connectionString))
            {
                var sql = BuildFunctionCall(functionName, parameters);
                return connection.ExecuteScalar<T>(sql, parameters, commandType: CommandType.Text);
            }
        }

        private async Task<T> DapperExecuteScalarAsync<T>(string connectionString, string functionName, Dictionary<string, object> parameters)
        {
            using (var connection = _dbConnection.Create(connectionString))
            {
                var sql = BuildFunctionCall(functionName, parameters);
                return await connection.ExecuteScalarAsync<T>(sql, parameters, commandType: CommandType.Text);
            }
        }

        private IEnumerable<T> DapperQuery<T>(string connectionString, string functionName, Dictionary<string, object> parameters)
        {
            using (var connection = _dbConnection.Create(connectionString))
            {
                var sql = BuildFunctionCall(functionName, parameters);
                return connection.Query<T>(sql, parameters, commandType: CommandType.Text);
            }
        }

        private async Task<IEnumerable<T>> DapperQueryAsync<T>(string connectionString, string functionName, Dictionary<string, object> parameters)
        {
            using (var connection = _dbConnection.Create(connectionString))
            {
                var sql = BuildFunctionCall(functionName, parameters);
                return await connection.QueryAsync<T>(sql, parameters, commandType: CommandType.Text);
            }
        }

        private T DapperQueryFirstOrDefault<T>(string connectionString, string functionName, Dictionary<string, object> parameters)
        {
            using (var connection = _dbConnection.Create(connectionString))
            {
                var sql = BuildFunctionCall(functionName, parameters);
                return connection.QueryFirstOrDefault<T>(sql, parameters, commandType: CommandType.Text);
            }
        }

        private async Task<T> DapperQueryFirstOrDefaultAsync<T>(string connectionString, string functionName, Dictionary<string, object> parameters)
        {
            using (var connection = _dbConnection.Create(connectionString))
            {
                var sql = BuildFunctionCall(functionName, parameters);
                return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandType: CommandType.Text);
            }
        }

        private T DapperQuerySingleOrDefault<T>(string connectionString, string functionName, Dictionary<string, object> parameters)
        {
            using (var connection = _dbConnection.Create(connectionString))
            {
                var sql = BuildFunctionCall(functionName, parameters);
                return connection.QuerySingleOrDefault<T>(sql, parameters, commandType: CommandType.Text);
            }
        }

        private async Task<T> DapperQuerySingleOrDefaultAsync<T>(string connectionString, string functionName, Dictionary<string, object> parameters)
        {
            using (var connection = _dbConnection.Create(connectionString))
            {
                var sql = BuildFunctionCall(functionName, parameters);
                return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters, commandType: CommandType.Text);
            }
        }

        private T DapperQueryMultiple<T>(string connectionString, string functionName, Dictionary<string, object> parameters)
        {
            var data = new ExpandoObject();
            var customObjectMap = CustomObjectMapHelper.Create<T>();

            using (var connection = _dbConnection.Create(connectionString))
            {
                var sql = BuildFunctionCall(functionName, parameters);
                using (var result = connection.QueryMultiple(sql, parameters, commandType: CommandType.Text))
                {
                    MapCustomObjects(customObjectMap, data, result);
                }
            }

            var returnObject = DynamicToComplexObjectHelper.Convert<T>(data);

            return returnObject;
        }

        private async Task<T> DapperQueryMultipleAsync<T>(string connectionString, string functionName, Dictionary<string, object> parameters)
        {
            var data = new ExpandoObject();
            var customObjectMap = CustomObjectMapHelper.Create<T>();

            using (var connection = _dbConnection.Create(connectionString))
            {
                var sql = BuildFunctionCall(functionName, parameters);
                using (var result = await connection.QueryMultipleAsync(sql, parameters, commandType: CommandType.Text))
                {
                    await MapCustomObjectsAsync(customObjectMap, data, result);
                }
            }

            var returnObject = DynamicToComplexObjectHelper.Convert<T>(data);

            return returnObject;
        }

        private void MapCustomObjects(IEnumerable<CustomObjectMap> customObjects, ExpandoObject data, GridReader result)
        {
            foreach (var item in customObjects)
            {
                if (!item.IsList)
                {
                    var singleItem = result.ReadSingleOrDefault(item.Type);
                    ((IDictionary<string, object>)data).Add(item.PropertyName, singleItem);
                }
                else
                {
                    var listItem = result.Read(item.Type);
                    ((IDictionary<string, object>)data).Add(item.PropertyName, listItem);
                }
            }
        }

        private async Task MapCustomObjectsAsync(IEnumerable<CustomObjectMap> customObjects, ExpandoObject data, GridReader result)
        {
            foreach (var item in customObjects)
            {
                if (!item.IsList)
                {
                    var singleItem = await result.ReadSingleOrDefaultAsync(item.Type);
                    ((IDictionary<string, object>)data).Add(item.PropertyName, singleItem);
                }
                else
                {
                    var listItem = await result.ReadAsync(item.Type);
                    ((IDictionary<string, object>)data).Add(item.PropertyName, listItem);
                }
            }
        }
    }
}
