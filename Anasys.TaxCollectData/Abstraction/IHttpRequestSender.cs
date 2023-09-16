using Anasys.TaxCollectData.Dto;

namespace Anasys.TaxCollectData.Abstraction;

public interface IHttpRequestSender
{
    Task<HttpResponse<T?>> SendPostRequestAsync<T>(
        string url,
        string requestBody,
        Dictionary<string, string> headers);
}