namespace Services.Customer.Interfaces.Managers
{
    public interface ICronDescriptionManager
    {
        string Get(string cronExpression);
    }
}
