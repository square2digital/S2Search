using Domain.SearchResources.Synonyms;

namespace Services.Interfaces.Managers
{
    public interface ISynonymValidationManager
    {
        bool IsValid(SynonymRequest synonymRequest, out string errorMessage);
    }
}
