namespace Services.Interfaces.Providers
{
    public interface IPercentageChangeProvider
    {
        double Get(double current, double previous);
    }
}
