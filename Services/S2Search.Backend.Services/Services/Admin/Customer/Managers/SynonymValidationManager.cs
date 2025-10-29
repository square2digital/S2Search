using S2Search.Backend.Domain.Customer.SearchResources.Synonyms;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;
using System.Text.RegularExpressions;

namespace S2Search.Backend.Services.Services.Admin.Customer.Managers
{
    public class SynonymValidationManager : ISynonymValidationManager
    {
        /// <summary>
        /// Allows a single word (alphanumeric) upto a maximum of 30 characters
        /// </summary>
        private const string keyWordExpression = @"^(?=.{1,30}$)\w*$";

        /// <summary>
        /// Allows words (alphanumeric) with single spaces upto a maximum of 30 characters
        /// </summary>
        private const string synonymExpression = @"^(?=.{1,30}$)\w+( \w+)*$";

        public bool IsValid(SynonymRequest synonymRequest, out string errorMessage)
        {
            //check the words can be converted to a solrFormat
            errorMessage = "";
            bool keyWordValid = Regex.IsMatch(synonymRequest.KeyWord, keyWordExpression);

            if (!keyWordValid)
            {
                errorMessage += $"{nameof(synonymRequest.KeyWord)} must be a single word between 1 and 30 alphanumeric characters";
            }

            bool synonymsInvalid = synonymRequest.Synonyms.Any(x => !Regex.IsMatch(x.Trim(), synonymExpression));

            if (synonymsInvalid)
            {
                errorMessage += "A synonym must be between 1 and 30 alphanumeric characters, with single spaces";
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return false;
            }

            return true;
        }
    }
}
