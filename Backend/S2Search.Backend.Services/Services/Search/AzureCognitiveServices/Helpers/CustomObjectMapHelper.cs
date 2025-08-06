using S2Search.Backend.Domain.Models.Dapper;
using System.Collections;
using System.Reflection;

namespace S2Search.Backend.Services.Services.Search.AzureCognitiveServices.Helpers
{
    public static class CustomObjectMapHelper
    {
        public static IEnumerable<CustomObjectMap> Create<T>()
        {
            var properties = typeof(T).GetProperties();
            var customObjectMapList = new List<CustomObjectMap>();

            foreach(PropertyInfo info in properties)
            {
                var type = info.PropertyType;
                var isList = typeof(IEnumerable).IsAssignableFrom(type);

                if (isList)
                {
                    var underlyingType = type.GetGenericArguments().FirstOrDefault();

                    if(underlyingType != null)
                    {
                        type = underlyingType;
                    }
                }

                string propertyName = info.Name;

                customObjectMapList.Add(new CustomObjectMap(type, isList, propertyName));
            }

            return customObjectMapList;
        }
    }
}
