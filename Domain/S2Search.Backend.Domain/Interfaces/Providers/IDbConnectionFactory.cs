using System.Data;

namespace S2Search.Common.Database.Sql.Dapper.Interfaces.Providers;

public interface IDbConnectionFactory
{
    IDbConnection Create(string connectionName);
}
