using Domain.Customer.SearchResources.Synonyms;

namespace Services.Customer.Interfaces.Managers
{
    public interface ISynonymValidationManager
    {
        bool IsValid(SynonymRequest synonymRequest, out string errorMessage);
    }
}
