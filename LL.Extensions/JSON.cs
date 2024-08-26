
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

        public static T? Extract<T>(string text)
        {
            T? output;

            // Regular expression to match JSON objects
            string pattern = @"\{.*?\}";

            Match match = Regex.Match(text, pattern);

            if (match.Success)
                output = DeserializeObject<T>(match.Value);
            

            return output;
        }
    }
}
