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

        public int Execute(string connectionString, string sqlQuery, object parameters) => 
            DapperExecute(connectionString, sqlQuery, parameters);

        public async Task<int> ExecuteAsync(string connectionString, string sqlQuery, object parameters) => 
            await DapperExecuteAsync(connectionString, sqlQuery, parameters);

        public T ExecuteScalar<T>(string connectionString, string sqlQuery, object parameters) => 
            DapperExecuteScalar<T>(connectionString, sqlQuery, parameters);

        public async Task<T> ExecuteScalarAsync<T>(string connectionString, string sqlQuery, object parameters) => 
            await DapperExecuteScalarAsync<T>(connectionString, sqlQuery, parameters);

        public IEnumerable<T> Query<T>(string connectionString, string sqlQuery, object parameters) => 
            DapperQuery<T>(connectionString, sqlQuery, parameters);

        public async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string sqlQuery, object parameters) => 
            await DapperQueryAsync<T>(connectionString, sqlQuery, parameters);

        public T QueryFirstOrDefault<T>(string connectionString, string sqlQuery, object parameters) => 
            DapperQueryFirstOrDefault<T>(connectionString, sqlQuery, parameters);

        public async Task<T> QueryFirstOrDefaultAsync<T>(string connectionString, string sqlQuery, object parameters) => 
            await DapperQueryFirstOrDefaultAsync<T>(connectionString, sqlQuery, parameters);

        public T QuerySingleOrDefault<T>(string connectionString, string sqlQuery, object parameters) => 
            DapperQuerySingleOrDefault<T>(connectionString, sqlQuery, parameters);

        public async Task<T> QuerySingleOrDefaultAsync<T>(string connectionString, string sqlQuery, object parameters) => 
            await DapperQuerySingleOrDefaultAsync<T>(connectionString, sqlQuery, parameters);

        public T QueryMultiple<T>(string connectionString, string sqlQuery, object parameters) => 
            DapperQueryMultiple<T>(connectionString, sqlQuery, parameters);

        public async Task<T> QueryMultipleAsync<T>(string connectionString, string sqlQuery, object parameters) => 
            await DapperQueryMultipleAsync<T>(connectionString, sqlQuery, parameters);

        private int DapperExecute(string connectionString, string sqlQuery, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return connection.Execute(sqlQuery, parameters, commandType: CommandType.Text);
        }

        private async Task<int> DapperExecuteAsync(string connectionString, string sqlQuery, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return await connection.ExecuteAsync(sqlQuery, parameters, commandType: CommandType.Text);
        }

        private T DapperExecuteScalar<T>(string connectionString, string sqlQuery, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return connection.ExecuteScalar<T>(sqlQuery, parameters, commandType: CommandType.Text);
        }

        private async Task<T> DapperExecuteScalarAsync<T>(string connectionString, string sqlQuery, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return await connection.ExecuteScalarAsync<T>(sqlQuery, parameters, commandType: CommandType.Text);
        }

        private IEnumerable<T> DapperQuery<T>(string connectionString, string sqlQuery, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return connection.Query<T>(sqlQuery, parameters, commandType: CommandType.Text);
        }

        private async Task<IEnumerable<T>> DapperQueryAsync<T>(string connectionString, string sqlQuery, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return await connection.QueryAsync<T>(sqlQuery, parameters, commandType: CommandType.Text);
        }

        private T DapperQueryFirstOrDefault<T>(string connectionString, string sqlQuery, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return connection.QueryFirstOrDefault<T>(sqlQuery, parameters, commandType: CommandType.Text);
        }

        private async Task<T> DapperQueryFirstOrDefaultAsync<T>(string connectionString, string sqlQuery, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return await connection.QueryFirstOrDefaultAsync<T>(sqlQuery, parameters, commandType: CommandType.Text);
        }

        private T DapperQuerySingleOrDefault<T>(string connectionString, string sqlQuery, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return connection.QuerySingleOrDefault<T>(sqlQuery, parameters, commandType: CommandType.Text);
        }

        private async Task<T> DapperQuerySingleOrDefaultAsync<T>(string connectionString, string sqlQuery, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return await connection.QuerySingleOrDefaultAsync<T>(sqlQuery, parameters, commandType: CommandType.Text);
        }

        private T DapperQueryMultiple<T>(string connectionString, string sqlQuery, object parameters)
        {
            var data = new ExpandoObject();
            var customObjectMap = CustomObjectMapHelper.Create<T>();

            var connection = _dbConnection.Create(connectionString);
            using (var result = connection.QueryMultiple(sqlQuery, parameters, commandType: CommandType.Text))
            {
                MapCustomObjects(customObjectMap, data, result);
            }

            var returnObject = DynamicToComplexObjectHelper.Convert<T>(data);

            return returnObject;
        }

        private async Task<T> DapperQueryMultipleAsync<T>(string connectionString, string sqlQuery, object parameters)
        {
            var data = new ExpandoObject();
            var customObjectMap = CustomObjectMapHelper.Create<T>();

            var connection = _dbConnection.Create(connectionString);
            using (var result = await connection.QueryMultipleAsync(sqlQuery, parameters, commandType: CommandType.Text))
            {
                await MapCustomObjectsAsync(customObjectMap, data, result);
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
