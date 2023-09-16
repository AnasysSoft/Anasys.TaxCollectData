using System.Globalization;
using System.Text;
using Anasys.TaxCollectData.Abstraction;
using Anasys.TaxCollectData.Constants;
using Anasys.TaxCollectData.Dto;
using Newtonsoft.Json.Linq;

namespace Anasys.TaxCollectData.Business.Normalize;

internal class ObjectNormalizer : INormalizer
{
    public string? Normalize<T>(T data, Dictionary<string, string>? headers)
    {
        if (data is null && headers is null)
            return null;
        var map = GetToken(data) ?? new JObject();
        return Normalize(headers, map);
    }

    public string? NormalizeJson(string data, Dictionary<string, string>? headers)
    {
        if (string.IsNullOrWhiteSpace(data))
            return null;
        var map = JObject.Parse(data);
        return Normalize(headers, map);
    }

    private static string Normalize(Dictionary<string, string>? headers, JObject map)
    {
        if (headers != null)
            SetHeaders(headers, map);
        var result = new JObject();
        FlatMap(null, map, result);
        var stringBuilder = new StringBuilder();
        var list = result.Properties().Select(p => p.Name).ToList();
        list.Sort();
        foreach (var propertyName in list)
        {
            var jToken = result[propertyName];
            string str1;
            if (jToken != null && !string.IsNullOrEmpty(jToken.ToString()))
            {
                var str2 = jToken.ToString();
                str1 = string.IsNullOrEmpty(str2) ? "#" : str2.Replace("#", "##");
            }
            else
                str1 = "#";

            stringBuilder.Append(str1).Append('#');
        }

        return stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString();
    }

    private static void SetHeaders(Dictionary<string, string> headers, JObject map)
    {
        foreach (var (key, str) in headers)
        {
            if (key.Equals(TransferConstants.AuthorizationHeader))
            {
                if (!string.IsNullOrWhiteSpace(str) && str.Length > 7)
                {
                    map.Add(key, (JToken)str[7..]);
                    continue;
                }
            }

            map.Add(key, (JToken)str);
        }
    }

    private static JObject? GetToken(object? data) =>
        data switch
        {
            null => null,
            IEnumerable<object> packets => JObject.FromObject(new PacketsCollectionWrapper(packets)),
            _ => JObject.FromObject(data)
        };

    private static string GetKey(string? rootKey, string myKey) => 
        string.IsNullOrWhiteSpace(rootKey) ? myKey : $"{rootKey}.{myKey}";

    private static void FlatMap(string? rootKey, object? input, JObject result)
    {
        switch (input)
        {
            case JArray source:
                using (IEnumerator<(JToken, int)> enumerator = source.Select((x, i) => (x, i)).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        FlatMap(GetKey(rootKey, $"E{current.Item2}"), current.Item1, result);
                    }

                    break;
                }
            case JObject jObject:
                using (var enumerator = jObject.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        FlatMap(GetKey(rootKey, current.Key), current.Value, result);
                    }

                    break;
                }
            case JValue o:
                result.Add(rootKey!, o.Type == JTokenType.Boolean ? JToken.FromObject(o.ToString(CultureInfo.InvariantCulture).ToLowerInvariant()) : JToken.FromObject(o));
                break;
            default:
                result.Add(rootKey!, null);
                break;
        }
    }
}