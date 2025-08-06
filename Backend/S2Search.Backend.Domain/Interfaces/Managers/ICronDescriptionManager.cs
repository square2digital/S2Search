namespace S2Search.Backend.Services.Admin.Customer.Interfaces.Managers
{
    public interface ICronDescriptionManager
    {
        string Get(string cronExpression);
    }
}
