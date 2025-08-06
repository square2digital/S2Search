using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;
using System.Text.RegularExpressions;

namespace S2Search.Backend.Services.Services.Admin.Customer.Managers
{
    public class QueryKeyNameValidationManager : IQueryKeyNameValidationManager
    {
        private const string expression = "^(?=.{1,40}$)[a-zA-Z0-9-_]+$";
        //1-40 chars
        //[a-z] - _

        public bool IsValid(string name, out string errorMessage)
        {
            errorMessage = null;
            bool IsValid = Regex.IsMatch(name, expression);

            if (!IsValid)
            {
                errorMessage = "Must be between 1 and 40 alphanumeric characters, can contain alphanumeric characters, hyphens, underscores";
            }

            return IsValid;
        }
    }
}
