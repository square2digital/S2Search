using Domain.Interfaces;
using Newtonsoft.Json.Linq;
using S2Search.Backend.Domain.Search.Elastic.Models.Response.Generic;

namespace S2Search.Backend.Services.Services.Search.Elastic.Helpers
{
    public static class GenericResponseHelper
    {
        private static Dictionary<string, string> _textTypeDictionary = new Dictionary<string, string>();

        /// <summary>
        /// Will convert the data documents saved in elastic and convert to the GenericResponse type
        /// using the Index Schema to populate the relevant generic property lists by data type
        /// </summary>
        /// <param name="response">Json Documents</param>
        /// <param name="indexSchema">Json Index Schema</param>
        /// <returns></returns>
        public static List<GenericResponse> BuildGenericResponse(string response, string indexSchema)
        {
            List<GenericResponse> returnObj = new List<GenericResponse>();

            // we need a flag for a text property to understand if we need to add to keywords or Text Additional Properties
            _textTypeDictionary = IndexHelper.GetTextTypeDictionary(indexSchema);

            JObject json = JObject.Parse(response);
            var parentHits = json["hits"];

            if (parentHits.HasValues)
            {
                var total = parentHits["total"]["value"];
                JArray hits = (JArray)parentHits["hits"];

                foreach (JToken? hit in hits)
                {
                    GenericResponse obj = new GenericResponse();

                    PopulateGenericProductProperties(obj, hit);

                    foreach (var x in hit["_source"].Where(x => x != null))
                    {
                        var key = ((JProperty)x).Name;
                        var jvalue = ((JProperty)x).Value;

                        if(HelperBase.DoesKeyMatchDefaultProperties(key))
                        {
                            continue;
                        }

                        PopulateAdditionalProperties(obj, key, jvalue.ToString());
                    }

                    returnObj.Add(obj);
                }
            }

            return returnObj;
        }

        private static void PopulateGenericProductProperties(GenericResponse obj, JToken? hit)
        {
            // ****************************
            // Generic Product Properties
            // ****************************

            if (hit["_source"]["id"] != null)
            {
                obj.Id = hit["_source"]["id"].ToString();
            }

            if (hit["_source"]["title"] != null)
            {
                obj.Title = hit["_source"]["title"].ToString();
            }

            if (hit["_source"]["subtitle"] != null)
            {
                obj.Subtitle = hit["_source"]["subtitle"].ToString();
            }

            if (hit["_source"]["price"] != null)
            {
                var priceString = hit["_source"]["price"].ToString();
                obj.Price = Convert.ToDecimal(priceString);
            }

            if (hit["_source"]["city"] != null)
            {
                obj.City = hit["_source"]["city"].ToString();
            }

            if (hit["_source"]["imageURL"] != null)
            {
                obj.ImageUrl = hit["_source"]["imageURL"].ToString();
            }

            if (hit["_source"]["linkUrl"] != null)
            {
                obj.LinkUrl = hit["_source"]["linkUrl"].ToString();
            }
        }

        private static void PopulateAdditionalProperties(GenericResponse obj, string key, string value)
        {
            bool boolValue;
            int intValue;
            long longValue;
            decimal decimalValue;
            DateTime dateValue;

            // Place checks higher in if-else statement to give higher priority to type.

            if (bool.TryParse(value, out boolValue))
            {
                obj.AdditionalProperties_Boolean.Add(key, boolValue);
            }
            else if (int.TryParse(value, out intValue))
            {
                obj.AdditionalProperties_Integer.Add(key, intValue);
            }
            else if (long.TryParse(value, out longValue))
            {
                obj.AdditionalProperties_Long.Add(key, longValue);
            }
            else if (decimal.TryParse(value, out decimalValue))
            {
                obj.AdditionalProperties_Decimal.Add(key, decimalValue);
            }
            else if (DateTime.TryParse(value, out dateValue))
            {
                obj.AdditionalProperties_Date.Add(key, dateValue);
            }
            else
            {
                if (_textTypeDictionary.ContainsKey(key.ToLower()))
                {
                    if (_textTypeDictionary[key.ToLower()] == "keyword")
                    {
                        obj.AdditionalProperties_Keyword.Add(key, value);
                    }
                    else
                    {
                        obj.AdditionalProperties_Text.Add(key, value);
                    }
                }                
            }
        }
    }
}
