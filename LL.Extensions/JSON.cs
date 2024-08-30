
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace LL.Extensions
{
    public class JSON
    {
        public static string SerializeObject(object obj)
        {
           return JsonConvert.SerializeObject(obj);
        }

        public static T? DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static T? Extract<T>(string text) where T : class
        {
            // Regular expression to match JSON objects
            var matches = Regex.Matches(text, @"\{.*?\}");

            if (matches.Count > 0)
            {
                var json = string.Join("", matches.Select(m => m.Value));
                return DeserializeObject<T>(json);
            }

            return null;
        }
    }
}
