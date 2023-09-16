using System.Text;
using System.Text.Json;
using Anasys.TaxCollectData.Abstraction;
using Anasys.TaxCollectData.Constants;

namespace Anasys.TaxCollectData.Business.Normalize;

internal class SimpleNormalizer : INormalizer
{
    public string Normalize<T>(T data, Dictionary<string, string>? headers) => 
        NormalizeJson(Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(data, JsonSerializerConfig.JsonSerializerOptions)), headers);

    public string NormalizeJson(string data, Dictionary<string, string>? headers)
    {
        if (string.IsNullOrWhiteSpace(data))
            return data;
        
        var stringBuilder = new StringBuilder(data);
        var strArray = new[]
        {
            TransferConstants.AuthorizationHeader,
            TransferConstants.RequestTraceIdHeader,
            TransferConstants.TimestampHeader
        };
        
        if(!(headers is null))
            foreach (var key in strArray)
            {
                if (headers.TryGetValue(key, out var str))
                    stringBuilder.Append(str);
            }

        return stringBuilder.ToString();
    }
}