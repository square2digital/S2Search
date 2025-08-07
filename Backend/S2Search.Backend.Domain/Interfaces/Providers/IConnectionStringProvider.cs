namespace S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;

public interface IConnectionStringProvider
{
    string Get(string connectionName);
}
