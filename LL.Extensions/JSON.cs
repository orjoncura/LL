
using Newtonsoft.Json;

namespace LL.Extensions
{
    public class JSON
    {
        public static string SerializeObject(object obj)
        {
           return JsonConvert.SerializeObject(obj);
        }

        public static T? SerializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
