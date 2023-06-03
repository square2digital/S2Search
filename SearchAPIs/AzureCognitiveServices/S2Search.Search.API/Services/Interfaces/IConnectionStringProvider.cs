namespace Services.Interfaces
{
    public interface IConnectionStringProvider
    {
        string Get(string connectionName);
    }
}
