namespace Anasys.TaxCollectData.Dto.Transfer;

public record SignedRequest(string Signature, string? SignatureKeyId)
{
    protected SignedRequest()
        : this(string.Empty, string.Empty)
    {

    }
}