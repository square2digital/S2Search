using S2Search.Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S2Search.Backend.Services
{
    public class SearchFilterFormatter : ISearchFilterFormatter
    {
        public string Format(List<string> unformattedFilters)
        {
            var newFilters = new List<string>();

            if (unformattedFilters != null && unformattedFilters.All(x => x != null))
            {
                foreach (var filter in unformattedFilters)
                {
                    if (filter.IndexOf('|') != -1)
                    {
                        var newFilter = filter.Split('|').ToList();
                        newFilters.AddRange(newFilter.Where(x => !string.IsNullOrEmpty(x)));
                    }
                    else
                    {
                        newFilters.Add(filter);
                    }
                }

                unformattedFilters = newFilters;
            }

            var filterList = GroupFilters(unformattedFilters);

            var filterStringNew = string.Join("", filterList);

            return filterStringNew;
        }

        private List<string> GroupFilters(List<string> filters)
        {
            List<string> filterList = new List<string>();
            Dictionary<string, int> occurances = new Dictionary<string, int>();

            foreach (var filter in filters)
            {
                if (string.IsNullOrEmpty(filter)) continue;

                var categoryName = filter.Substring(0, filter.IndexOf(' '));

                if (occurances.Count == 0 || !occurances.ContainsKey(categoryName))
                {
                    occurances.Add(categoryName, 1);
                    continue;
                }

                if (occurances.ContainsKey(categoryName))
                {
                    int count = occurances[categoryName];
                    occurances[categoryName] = (count + 1);
                }
            }

            foreach (var kvp in occurances)
            {
                StringBuilder sb = new StringBuilder();

                int i = 1;
                foreach (var filter in filters)
                {
                    var categoryName = filter.Substring(0, filter.IndexOf(' '));
                    if (categoryName != kvp.Key) continue;

                    string keyword = string.Empty;

                    if (kvp.Value % 2 == 0)
                    {
                        keyword = " or ";
                    }
                    else
                    {
                        keyword = " and ";
                    }

                    if (i == 1 && i != kvp.Value)
                    {
                        sb.Append("(" + filter);
                        i++;
                        continue;
                    }
                    if (i == 1 && i == kvp.Value)
                    {
                        sb.Append(filter);
                        i++;
                        continue;
                    }
                    if (i > 1 && i != kvp.Value)
                    {
                        sb.Append(" or " + filter);
                        i++;
                        continue;
                    }
                    if (i > 1 && i == kvp.Value)
                    {
                        sb.Append(" or " + filter + ")");
                        i++;
                        continue;
                    }
                    if (i == kvp.Value)
                    {
                        sb.Append(filter + ")");
                        i++;
                        continue;
                    }
                }

                filterList.Add(sb.ToString());
            }

            List<string> filterListCopy = new List<string>();

            int index = 1;
            foreach (string filter in filterList)
            {
                if (index != filterList.Count)
                {
                    filterListCopy.Add(filter + " and ");
                }
                else
                {
                    filterListCopy.Add(filter);
                }

                index++;
            }

            return filterListCopy;
        }
    }
}
