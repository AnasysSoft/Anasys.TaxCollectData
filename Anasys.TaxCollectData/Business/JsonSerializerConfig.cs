using System.Text.Json;
using System.Text.Json.Serialization;

namespace Anasys.TaxCollectData.Business;

internal static class JsonSerializerConfig
{
    public static readonly JsonSerializerOptions JsonSerializerOptions;

    static JsonSerializerConfig()
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        serializerOptions.Converters.Add(new JsonStringEnumConverter());
        JsonSerializerOptions = serializerOptions;
    }
}