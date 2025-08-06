using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Domain.Models.Facets;
using Newtonsoft.Json;
using S2Search.Backend.Domain.Models.Facets;

namespace S2Search.Backend.Services.Services.Search.Elastic.Helpers
{
    public static class LuceneSyntaxHelper
    {
        private static readonly Dictionary<int, string> _nonExactTermContainer = new Dictionary<int, string>();
        private static readonly List<int> _termIndexsCompletedContainer = new List<int>();
        private static readonly IEnumerable<string> _facetGroupNamesToMatch = new List<string> { "make", "model", "transmission", "Fuel Type", "year", "colour", "Body Style" };
        private static readonly IEnumerable<char> _luceneSpecialCharacters = new char[] { '+', '-', '&', '|', '!', '(', ')', '{', '}', '[', ']', '^', '"', '~', '*', '?', ':', '\\' };
        private static Dictionary<int, string> _searchTermContainer = new Dictionary<int, string>();
        private static string _fullSearchTerm = string.Empty;

        private static IList<FacetGroup> _defaultFacets = null;

        private static IList<FacetGroup> FacetsFromLocalFile()
        {
            if (_defaultFacets == null)
            {
                var json = File.ReadAllText(@"DefaultFacets.json");
                _defaultFacets = JsonConvert.DeserializeObject<IList<FacetGroup>>(json);
            }

            return _defaultFacets;            
        }

        public static string GenerateLuceneSearchString(string searchTerm)
        {
            FacetsFromLocalFile();
            ClearObjects();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return string.Empty;
            }

            searchTerm = searchTerm.Trim();
            _fullSearchTerm = searchTerm;

            StringBuilder SearchTermBuilder = new StringBuilder();

            string NewSearchTerm = Regex.Replace(searchTerm, @"\s+", " ");

            //var genericSynonyms = _synonymsService.GetGenericSynonyms().GetAwaiter().GetResult();
            //var synonyms = _synonymsHelper.GetSynonymsDictionary(genericSynonyms);

            //if (synonyms != null && synonyms.Any())
            //{
            //    if (DoesSearchTermExactMatchInSynonym(NewSearchTerm, synonyms))
            //    {
            //        BuildSearchTerm(NewSearchTerm, SearchTermBuilder);
            //        return SearchTermBuilder.ToString();
            //    }
            //}

            // initially exact match the term before splitting - eg "4 series"
            if (DoesSearchTermExactMatchInFacet(NewSearchTerm))
            {
                BuildSearchTerm(NewSearchTerm, SearchTermBuilder);
                return SearchTermBuilder.ToString();
            }

            List<string> TermSplit = NewSearchTerm.Split(' ').ToList();

            // add the terms to the dictionary
            int i = 0;
            foreach (var term in TermSplit)
            {
                _searchTermContainer.TryAdd(i, term);
                i++;
            }

            // ***************************************************
            // Single Search String         
            // ***************************************************
            if (_searchTermContainer.Count == 1)
            {
                string comparisonStr = _searchTermContainer.First().Value;

                if (DoesSearchTermExactMatchInFacet(comparisonStr))
                {
                    SearchTermBuilder.Append(comparisonStr);
                }
                else
                {
                    SearchTermBuilder.Append(comparisonStr).Append("*");
                }

                return SearchTermBuilder.ToString();
            }

            // ***************************************************
            // Multi Strings
            // ***************************************************
            Dictionary<int, string> searchTermContainer_copy = new Dictionary<int, string>(_searchTermContainer);
            _searchTermContainer.Clear();

            foreach (KeyValuePair<int, string> term in searchTermContainer_copy)
            {
                // if (DoesSearchTermExactMatchInFacet(term.Value) || DoesSearchTermExactMatchInSynonym(term.Value, synonyms))
                if (DoesSearchTermExactMatchInFacet(term.Value))
                {
                    _searchTermContainer.TryAdd(term.Key, term.Value);
                }
                else
                {
                    _nonExactTermContainer.TryAdd(term.Key, term.Value);
                }
            }

            Dictionary<int, string> nonExactTermContainer_copy = new Dictionary<int, string>(_nonExactTermContainer);
            foreach (KeyValuePair<int, string> term in nonExactTermContainer_copy)
            {
                if (_termIndexsCompletedContainer.Contains(term.Key)) continue;

                if (_nonExactTermContainer.ContainsKey(term.Key + 1))
                {
                    string FirstValue = _nonExactTermContainer[term.Key];
                    string SecondValue = _nonExactTermContainer[term.Key + 1];

                    string TermConcatinated = $"{FirstValue} {SecondValue}";
                    if (DoesSearchTermExactMatchInFacet(TermConcatinated))
                    {
                        UpdateDictionaries(term, TermConcatinated);
                        continue;
                    }

                    string seatsRegExPattern = @"(?i)\d+ seat?[s]{0,1}";
                    if (TestForRegExInSearchTerm(term, seatsRegExPattern, TermConcatinated, "seat"))
                    {
                        continue;
                    }

                    string doorsRegExPattern = @"(?i)\d+ door?[s]{0,1}";
                    if (TestForRegExInSearchTerm(term, doorsRegExPattern, TermConcatinated, "door"))
                    {
                        continue;
                    }

                    _searchTermContainer.TryAdd(term.Key, term.Value);
                }
                else
                {
                    _searchTermContainer.TryAdd(term.Key, term.Value);
                }
            }

            _searchTermContainer = _searchTermContainer.OrderBy(x => x.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            ConcatResults(SearchTermBuilder);
            return CleanLuceneString(SearchTermBuilder.ToString());

        }

        public static bool ContainsSpecialCharacters(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return false;

            foreach (char c in searchTerm)
            {
                if (_luceneSpecialCharacters.Contains(c))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Escaping Special Characters
        /// Lucene supports escaping special characters that are part of the query syntax.The current list special characters are
        /// + - && || ! ( ) { } [ ] ^ " ~ * ? : \
        /// To escape these characters use the \ before the character.For example to search for (1+1):2 use the query:
        /// \(1\+1\)\:2
        /// </summary>
        public static string EscapeLuceneSpecialCharacters(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return searchTerm;

            searchTerm = searchTerm.Trim();

            StringBuilder sb = new StringBuilder();

            foreach (char c in searchTerm)
            {
                if (_luceneSpecialCharacters.Contains(c))
                {
                    sb.Append(@"\");
                    sb.Append(@"\");
                    sb.Append($"{c}");
                }
                else
                {
                    sb.Append($"{c}");
                }
            }

            return sb.ToString().Replace(@"\\", @"\");
        }

        private static void BuildSearchTerm(string NewSearchTerm, StringBuilder SearchTermBuilder)
        {
            if (NewSearchTerm.Contains(" ") || NewSearchTerm.Contains("-"))
            {
                SearchTermBuilder.Append("\"").Append(NewSearchTerm).Append("\"");
            }
            else
            {
                SearchTermBuilder.Append(NewSearchTerm);
            }
        }

        private static bool TestForRegExInSearchTerm(KeyValuePair<int, string> term, string regExPattern, string termConcatinated, string regRexTerm)
        {
            if (Regex.IsMatch(termConcatinated, regExPattern))
            {
                string RegEx = RegExTest(termConcatinated, regExPattern);
                if (!string.IsNullOrEmpty(RegEx))
                {
                    if (!RegEx.Contains($"{regRexTerm}s", StringComparison.OrdinalIgnoreCase))
                    {
                        RegEx = RegEx.Replace($"{regRexTerm}", $"{regRexTerm}s");
                    }

                    UpdateDictionaries(term, RegEx);

                    return true;
                }
            }

            return false;
        }

        private static void ConcatResults(StringBuilder SearchTermBuilder)
        {
            // ***************************************************
            // Concat the results
            // ***************************************************
            int i = 0;
            Dictionary<int, string> searchTermContainer_copy = new Dictionary<int, string>(_searchTermContainer);
            foreach (KeyValuePair<int, string> term in searchTermContainer_copy)
            {
                if (string.IsNullOrEmpty(term.Value)) continue;

                string testTerm = term.Value.Replace("\"", "");

                // if (DoesSearchTermExactMatchInFacet(testTerm) || DoesSearchTermExactMatchInSynonym(testTerm, synonyms))
                if (DoesSearchTermExactMatchInFacet(testTerm))
                {
                    if (testTerm.Contains(" ") || testTerm.Contains("-"))
                    {
                        SearchTermBuilder.Append("\"").Append(testTerm).Append("\"");
                    }
                    else
                    {
                        SearchTermBuilder.Append(term.Value);
                    }
                }
                else
                {
                    if (term.Value.Contains("doors", StringComparison.OrdinalIgnoreCase) || term.Value.Contains("seats", StringComparison.OrdinalIgnoreCase))
                    {
                        SearchTermBuilder.Append(term.Value);
                    }
                    else
                    {
                        SearchTermBuilder.Append(term.Value).Append("*");
                    }
                }

                if (i != _searchTermContainer.Count - 1)
                {
                    SearchTermBuilder.Append(" AND ");
                }

                i++;
            }
        }

        private static void UpdateDictionaries(KeyValuePair<int, string> term, string SearchTerm)
        {
            _searchTermContainer.Remove(term.Key);
            _searchTermContainer.Remove(term.Key + 1);

            _termIndexsCompletedContainer.Add(term.Key);
            _termIndexsCompletedContainer.Add(term.Key + 1);

            _searchTermContainer.TryAdd(term.Key, $"\"{SearchTerm}\"");
        }

        private static string RegExTest(string searchTerm, string regExPattern)
        {
            string RegExMatch = string.Empty;

            if (Regex.IsMatch(searchTerm, regExPattern))
            {
                RegExMatch = Regex.Match(searchTerm, regExPattern).ToString();
            }

            return RegExMatch;
        }

        private static bool DoesSearchTermExactMatchInFacet(string searchTerm)
        {
            searchTerm = searchTerm?.Trim();

            //if (HandleEdgeCases(searchTerm))
            //{
            //    return false;
            //}

            foreach (FacetGroup facetGroup in _defaultFacets)
            {
                if (_facetGroupNamesToMatch.Any(x => x.ToUpper(CultureInfo.InvariantCulture) == facetGroup.FacetKey.ToUpper(CultureInfo.InvariantCulture)))
                {
                    foreach (FacetItem facetItem in facetGroup.FacetItems)
                    {
                        if (string.IsNullOrEmpty(facetItem.Value)) continue;

                        if (ContainsSpecialCharacters(searchTerm))
                        {
                            if (searchTerm.Equals(EscapeLuceneSpecialCharacters(facetItem.Value), StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (facetItem.Value.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static bool DoesSearchTermExactMatchInSynonym(string searchTerm, IDictionary<string, List<string>> synonyms)
        {
            //if (HandleEdgeCases(searchTerm))
            //{
            //    return false;
            //}

            foreach (var Synonyms_KVP in synonyms)
            {
                foreach (string Synonym in Synonyms_KVP.Value)
                {
                    if (string.IsNullOrEmpty(Synonym)) continue;

                    if (Synonym.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //private static bool HandleEdgeCases(string searchTerm)
        //{
        //    foreach (var edgeCase in _facetEdgeCases)
        //    {
        //        if (searchTerm == edgeCase.SearchTerm && _fullSearchTerm.Contains(edgeCase.FullSearchTerm, StringComparison.OrdinalIgnoreCase))
        //        {
        //            return edgeCase.Result;
        //        }
        //    }

        //    return false;
        //}

        /// <summary>
        /// used to clean the result of a lucence string - they can become corrupted with high throughput of search API calls
        /// although this likley wont be an issue in a scaled production environment
        /// ensure the last part of the string is not " AND" which will throw a bad request in Azure Search
        /// </summary>
        /// <param name="luceneString"></param>
        /// <returns></returns>
        private static string CleanLuceneString(string luceneString)
        {
            luceneString = luceneString.Trim();

            var last4chars = luceneString.Substring(Math.Max(0, luceneString.Length - 4));

            if (last4chars.Equals(" AND"))
            {
                luceneString = luceneString.Substring(0, luceneString.Length - 4);
            }

            return luceneString;
        }

        private static void ClearObjects()
        {
            _searchTermContainer.Clear();
            _nonExactTermContainer.Clear();
            _termIndexsCompletedContainer.Clear();
        }
    }
}