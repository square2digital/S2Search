namespace Services.Interfaces.Managers
{
    public interface ICronDescriptionManager
    {
        string Get(string cronExpression);
    }
}
