namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;

public interface IConnectionStringProvider
{
    string Get(string connectionName);
}