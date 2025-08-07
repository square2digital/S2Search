namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

public interface IQueryKeyNameValidationManager
{
    bool IsValid(string name, out string errorMessage);
}
