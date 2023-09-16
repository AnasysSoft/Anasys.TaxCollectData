namespace Anasys.TaxCollectData.Dto;

public record HttpResponse<T>(T? Body, int Status)
{
    public HttpResponse(int status) : this(default, status)
    {
    }
}