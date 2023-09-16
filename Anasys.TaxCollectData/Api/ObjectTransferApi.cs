using Anasys.TaxCollectData.Abstraction;
using Anasys.TaxCollectData.Dto;
using Anasys.TaxCollectData.Dto.Transfer;
using AutoMapper;

namespace Anasys.TaxCollectData.Api;

public class ObjectTransferApi : TransferApi
{
    private readonly IMapper _mapper;

    public ObjectTransferApi(
        IMapper mapper,
        INormalizer normalizer,
        ISignatory signatory,
        IHttpRequestSender httpRequestSender,
        IEncryptor encryptor)
        : base(normalizer, signatory, httpRequestSender, encryptor)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof (mapper));
    }

    protected override AsyncRequestDto<T> GetAsyncSendData<T>(List<PacketDto<T>> packets, Dictionary<string, string> headers) =>
        new(Signatory.Sign(Normalizer.Normalize<object>(!string.IsNullOrWhiteSpace(Signatory.GetKeyId())
                ? packets.ToList<object>()
                : _mapper.Map<List<PacketDtoWithoutSignatureKeyId<T>>>(packets), headers)!),
            Signatory.GetKeyId(),
            packets);

    protected override SyncRequestDto<T> GetSyncSendData<T>(PacketDto<T> packet, Dictionary<string, string> headers) =>
        new(Signatory.Sign(Normalizer.Normalize<object>(!string.IsNullOrWhiteSpace(Signatory.GetKeyId())
                ? packet
                : _mapper.Map<PacketDtoWithoutSignatureKeyId<T>>(packet), headers)!),
            Signatory.GetKeyId(),
            packet);
}