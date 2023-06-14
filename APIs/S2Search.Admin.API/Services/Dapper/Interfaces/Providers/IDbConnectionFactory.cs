using System.Data;

namespace Services.Dapper.Interfaces.Providers
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create(string connectionName);
    }
}
