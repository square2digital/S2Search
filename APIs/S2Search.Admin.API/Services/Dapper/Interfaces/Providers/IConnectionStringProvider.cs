namespace Services.Dapper.Interfaces.Providers
{
    public interface IConnectionStringProvider
    {
        string Get(string connectionName);
    }
}
