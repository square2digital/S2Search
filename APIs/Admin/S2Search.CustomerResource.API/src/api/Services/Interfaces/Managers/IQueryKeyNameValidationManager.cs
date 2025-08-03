namespace Services.Interfaces.Managers
{
    public interface IQueryKeyNameValidationManager
    {
        bool IsValid(string name, out string errorMessage);
    }
}
