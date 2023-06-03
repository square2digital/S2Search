using Newtonsoft.Json.Linq;

namespace Services.Helpers
{
    public static class IndexHelper
    {
        public static Dictionary<string, string> GetTextTypeDictionary(string indexSchema)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            JObject json = JObject.Parse(indexSchema);
            var index = json.Properties().Select(p => p.Name).First();
            var schema = json[index];
            var mappings = schema["mappings"];

            var properties = mappings["properties"];

            foreach (var x in properties)
            {
                var key = ((JProperty)x).Name;
                var value = ((JProperty)x).Value;

                if (HelperBase.DoesKeyMatchDefaultProperties(key))
                {
                    continue;
                }

                var type = value["type"].ToString();

                if (type == "text")
                {
                    if (value["fields"] != null)
                    {
                        if(value["fields"]["raw"] != null)
                        {
                            dict.Add(key.ToLower(), "keyword");
                        }                        
                    }
                    else
                    {
                        dict.Add(key.ToLower(), "text");
                    }
                }
            }

            return dict;
        }
    }
}