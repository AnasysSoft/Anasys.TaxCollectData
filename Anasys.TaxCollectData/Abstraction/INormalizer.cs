namespace Anasys.TaxCollectData.Abstraction;

public interface INormalizer
{
    string? Normalize<T>(T data, Dictionary<string, string>? headers);

    string? NormalizeJson(string data, Dictionary<string, string>? headers);
}