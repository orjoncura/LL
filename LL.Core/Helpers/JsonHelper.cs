using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace LL.Core.Helpers;

public static class JsonHelper
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
        // Regex to match JSON enclosed in curly braces or triple backticks
        var match = Regex.Match(text, @"(?<json>{(?:[^{}]|(?<Nested>{)|(?<-Nested>}))*(?(Nested)(?!))})", RegexOptions.Multiline);

        if (match.Success) 
            return DeserializeObject<T>(match.Value);
            
        return default;
    }
}