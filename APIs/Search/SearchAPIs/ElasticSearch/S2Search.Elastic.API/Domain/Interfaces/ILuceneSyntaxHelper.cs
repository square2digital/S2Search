namespace Domain.Interfaces
{
    public interface ILuceneSyntaxHelper
    {
        string GenerateLuceneSearchString(string searchTerm, string index);
        bool ContainsSpecialCharacters(string searchTerm);
        string EscapeLuceneSpecialCharacters(string searchTerm);
    }
}