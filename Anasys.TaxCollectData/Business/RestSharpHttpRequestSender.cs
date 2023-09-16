using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using Anasys.TaxCollectData.Abstraction;
using Anasys.TaxCollectData.Dto;

namespace Anasys.TaxCollectData.Business;

internal class RestSharpHttpRequestSender : IHttpRequestSender
{
    private readonly HttpClient _httpClient;

    public RestSharpHttpRequestSender(HttpClient httpClient)
    {
        _httpClient = httpClient;
        ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((sender, certificate, chain, sslPolicyErrors) => true);
    }

    public async Task<HttpResponse<T?>> SendPostRequestAsync<T>(
        string url,
        string requestBody,
        Dictionary<string, string> headers)
    {
        using var request = GetHttpRequestMessage<T>(url, headers, requestBody);
        using var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        try
        {
            return new HttpResponse<T?>(await response.Content.ReadFromJsonAsync<T>(JsonSerializerConfig.JsonSerializerOptions).ConfigureAwait(false), (int) response.StatusCode);
        }
        catch (Exception ex)
        {
            return new HttpResponse<T?>(ex is AuthenticationException ? 496 : 408);
        }
    }

    private static HttpRequestMessage GetHttpRequestMessage<T>(
        string url,
        Dictionary<string, string> headers,
        string content)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        foreach (var header in headers)
            httpRequestMessage.Headers.Add(header.Key, header.Value);
        httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
        return httpRequestMessage;
    }
}