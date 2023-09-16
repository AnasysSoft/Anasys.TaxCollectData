using Anasys.TaxCollectData.Abstraction;
using Anasys.TaxCollectData.Constants;
using Anasys.TaxCollectData.Dto.Transfer;

namespace Anasys.TaxCollectData.Api;

public class SimpleTransferApi : TransferApi
{
    public SimpleTransferApi(
        INormalizer normalizer,
        ISignatory signatory,
        IHttpRequestSender httpRequestSender,
        IEncryptor encryptor)
        : base(normalizer, signatory, httpRequestSender, encryptor)
    {
    }

    protected override AsyncRequestDto<T> GetAsyncSendData<T>(
        List<PacketDto<T>> packets,
        Dictionary<string, string> headers)
    {
        var request = new AsyncRequestDto<T>(packets);
        FillHeaders(headers, Serialize(request));
        return request;
    }

    private void FillHeaders(Dictionary<string, string> headers, string serializedRequest)
    {
        var str = Signatory.Sign(Normalizer.NormalizeJson(serializedRequest, headers)!);
        headers.Add(TransferConstants.SignatureHeader, str);
        var signatoryKeyId = Signatory.GetKeyId();
        if (string.IsNullOrWhiteSpace(signatoryKeyId))
            return;
        headers.Add(TransferConstants.SignatureKeyIdHeader, signatoryKeyId);
    }

    protected override SyncRequestDto<T> GetSyncSendData<T>(
        PacketDto<T> packet,
        Dictionary<string, string> headers)
    {
        var request = new SyncRequestDto<T>(packet);
        FillHeaders(headers, Serialize(request));
        return request;
    }
}