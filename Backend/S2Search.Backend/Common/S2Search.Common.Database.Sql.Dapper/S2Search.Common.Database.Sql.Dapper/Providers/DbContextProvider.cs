using Dapper;
using S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Dynamic;
using S2Search.Backend.Domain.Models.Dapper;
using S2Search.Backend.Common.S2Search.Common.Database.Sql.Dapper.S2Search.Common.Database.Sql.Dapper.Models;
using S2Search.Backend.Services.Services.Admin.Dapper.Helpers;
using S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Interfaces.Providers;

namespace S2Search.Backend.Common.S2Search.Common.Database.Sql.Dapper.S2Search.Common.Database.Sql.Dapper.Providers
{
    public class DbContextProvider : IDbContextProvider
    {
        private readonly IDbConnectionFactory _dbConnection;

        public DbContextProvider(IDbConnectionFactory dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public int Execute(string connectionString, string procedureName, object parameters) => 
            DapperExecute(connectionString, procedureName, ParameterHelper.ParseParameters(procedureName, parameters));

        public async Task<int> ExecuteAsync(string connectionString, string procedureName, object parameters) => 
            await DapperExecuteAsync(connectionString, procedureName, ParameterHelper.ParseParameters(procedureName, parameters));

        public T ExecuteScalar<T>(string connectionString, string procedureName, object parameters) => 
            DapperExecuteScalar<T>(connectionString, procedureName, ParameterHelper.ParseParameters(procedureName, parameters));

        public async Task<T> ExecuteScalarAsync<T>(string connectionString, string procedureName, object parameters) => 
            await DapperExecuteScalarAsync<T>(connectionString, procedureName, ParameterHelper.ParseParameters(procedureName, parameters));

        public IEnumerable<T> Query<T>(string connectionString, string procedureName, object parameters) => 
            DapperQuery<T>(connectionString, procedureName, ParameterHelper.ParseParameters(procedureName, parameters));

        public async Task<IEnumerable<T>> QueryAsync<T>(string connectionString, string procedureName, object parameters) => 
            await DapperQueryAsync<T>(connectionString, procedureName, ParameterHelper.ParseParameters(procedureName, parameters));

        public T QueryFirstOrDefault<T>(string connectionString, string procedureName, object parameters) => 
            DapperQueryFirstOrDefault<T>(connectionString, procedureName, ParameterHelper.ParseParameters(procedureName, parameters));

        public async Task<T> QueryFirstOrDefaultAsync<T>(string connectionString, string procedureName, object parameters) => 
            await DapperQueryFirstOrDefaultAsync<T>(connectionString, procedureName, ParameterHelper.ParseParameters(procedureName, parameters));

        public T QuerySingleOrDefault<T>(string connectionString, string procedureName, object parameters) => 
            DapperQuerySingleOrDefault<T>(connectionString, procedureName, ParameterHelper.ParseParameters(procedureName, parameters));

        public async Task<T> QuerySingleOrDefaultAsync<T>(string connectionString, string procedureName, object parameters) => 
            await DapperQuerySingleOrDefaultAsync<T>(connectionString, procedureName, ParameterHelper.ParseParameters(procedureName, parameters));

        public T QueryMultiple<T>(string connectionString, string procedureName, object parameters) => 
            DapperQueryMultiple<T>(connectionString, procedureName, ParameterHelper.ParseParameters(procedureName, parameters));

        public async Task<T> QueryMultipleAsync<T>(string connectionString, string procedureName, object parameters) => 
            await DapperQueryMultipleAsync<T>(connectionString, procedureName, ParameterHelper.ParseParameters(procedureName, parameters));

        private int DapperExecute(string connectionString, string procedureName, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        private async Task<int> DapperExecuteAsync(string connectionString, string procedureName, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        private T DapperExecuteScalar<T>(string connectionString, string procedureName, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return connection.ExecuteScalar<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        private async Task<T> DapperExecuteScalarAsync<T>(string connectionString, string procedureName, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return await connection.ExecuteScalarAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        private IEnumerable<T> DapperQuery<T>(string connectionString, string procedureName, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return connection.Query<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        private async Task<IEnumerable<T>> DapperQueryAsync<T>(string connectionString, string procedureName, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return await connection.QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        private T DapperQueryFirstOrDefault<T>(string connectionString, string procedureName, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return connection.QueryFirstOrDefault<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        private async Task<T> DapperQueryFirstOrDefaultAsync<T>(string connectionString, string procedureName, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return await connection.QueryFirstOrDefaultAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        private T DapperQuerySingleOrDefault<T>(string connectionString, string procedureName, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return connection.QuerySingleOrDefault<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        private async Task<T> DapperQuerySingleOrDefaultAsync<T>(string connectionString, string procedureName, object parameters)
        {
            var connection = _dbConnection.Create(connectionString);
            return await connection.QuerySingleOrDefaultAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }

        private T DapperQueryMultiple<T>(string connectionString, string procedureName, object parameters)
        {
            var data = new ExpandoObject();
            var customObjectMap = CustomObjectMapHelper.Create<T>();

            var connection = _dbConnection.Create(connectionString);
            using (var result = connection.QueryMultiple(procedureName, parameters, commandType: CommandType.StoredProcedure))
            {
                MapCustomObjects(customObjectMap, data, result);
            }

            var returnObject = DynamicToComplexObjectHelper.Convert<T>(data);

            return returnObject;
        }

        private async Task<T> DapperQueryMultipleAsync<T>(string connectionString, string procedureName, object parameters)
        {
            var data = new ExpandoObject();
            var customObjectMap = CustomObjectMapHelper.Create<T>();

            var connection = _dbConnection.Create(connectionString);
            using (var result = await connection.QueryMultipleAsync(procedureName, parameters, commandType: CommandType.StoredProcedure))
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
