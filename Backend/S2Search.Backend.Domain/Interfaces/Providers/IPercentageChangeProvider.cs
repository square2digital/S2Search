namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Providers;

public interface IPercentageChangeProvider
{
    double Get(double current, double previous);
}
