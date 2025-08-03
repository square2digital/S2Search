using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace S2Search.Common.Database.Sql.Dapper.Helpers
{
    public static class DynamicToComplexObjectHelper
    {
        public  static T Convert<T>(dynamic result)
        {
            JObject resultJson = JToken.FromObject(result);

            var complexObject = JsonConvert.DeserializeObject<T>(resultJson.ToString());

            return complexObject;
        }
    }
}
