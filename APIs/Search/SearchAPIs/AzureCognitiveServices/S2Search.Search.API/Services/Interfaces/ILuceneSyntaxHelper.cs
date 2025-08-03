namespace Services.Interfaces
{
    public interface ILuceneSyntaxHelper
    {
        string GenerateLuceneSearchString(string searchTerm, string callingHost);
        bool ContainsSpecialCharacters(string searchTerm);
        string EscapeLuceneSpecialCharacters(string searchTerm);
    }
}