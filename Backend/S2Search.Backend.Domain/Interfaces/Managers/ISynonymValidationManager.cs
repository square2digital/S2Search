using S2Search.Backend.Domain.Customer.SearchResources.Synonyms;

namespace S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

public interface ISynonymValidationManager
{
    bool IsValid(SynonymRequest synonymRequest, out string errorMessage);
}
