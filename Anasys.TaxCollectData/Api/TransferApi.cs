using System.Text;
using System.Text.Json;
using Anasys.TaxCollectData.Abstraction;
using Anasys.TaxCollectData.Business;
using Anasys.TaxCollectData.Constants;
using Anasys.TaxCollectData.Dto;
using Anasys.TaxCollectData.Dto.Transfer;
using Anasys.TaxCollectData.Enums;

namespace Anasys.TaxCollectData.Api;

public abstract class TransferApi : ITransferApi
{
    private readonly IHttpRequestSender _httpRequestSender;
    private readonly IEncryptor _encryptor;

    internal TransferApi(
        INormalizer normalizer,
        ISignatory signatory,
        IHttpRequestSender httpRequestSender,
        IEncryptor encryptor)
    {
        Normalizer = normalizer ?? throw new ArgumentNullException(nameof(normalizer));
        Signatory = signatory ?? throw new ArgumentNullException(nameof(signatory));
        _httpRequestSender = httpRequestSender ?? throw new ArgumentNullException(nameof(httpRequestSender));
        _encryptor = encryptor ?? throw new ArgumentNullException(nameof(encryptor));
    }

    protected INormalizer Normalizer { get; }

    protected ISignatory Signatory { get; }

    public async Task<HttpResponse<AsyncResponseModel?>?> SendPacketsAsync<TRequest>(
        List<PacketDto<TRequest>> packets,
        Dictionary<string, string>? headers = null,
        bool encrypt = true,
        bool sign = true,
        PriorityLevel priorityLevel = PriorityLevel.NORMAL)
    {
        if (packets == null)
            throw new ArgumentNullException(nameof(packets));
        
        if (!packets.Any())
            return null;
        
        headers ??= new Dictionary<string, string>();
        FillEssentialHeaders(headers);
        var sendData = GetSendData(packets, headers, encrypt, sign);
        return await _httpRequestSender.SendPostRequestAsync<AsyncResponseModel>(GetUrl(priorityLevel), sendData, headers).ConfigureAwait(false);
    }

    public async Task<HttpResponse<SyncResponseModel<TResponse>?>?> SendPacketAsync<TRequest, TResponse>(
        PacketDto<TRequest> packet,
        Dictionary<string, string>? headers = null,
        bool encrypt = true,
        bool sign = true)
    {
        if (packet is null)
            throw new ArgumentNullException(nameof(packet));
        
        headers ??= new Dictionary<string, string>();
        FillEssentialHeaders(headers);
        var sendData = GetSendData<TRequest, TResponse>(packet, headers, encrypt, sign);
        return await _httpRequestSender.SendPostRequestAsync<SyncResponseModel<TResponse>>(GetUrl(packet.PacketType), sendData, headers).ConfigureAwait(false);
    }

    private string GetSendData<TRequest>(
        List<PacketDto<TRequest>> packets,
        Dictionary<string, string> headers,
        bool encrypt,
        bool sign)
    {
        var packets1 = sign ? packets.Select(GetSignedPacket).ToList() : packets;
        return !encrypt ? Serialize(GetAsyncSendData(packets1, headers)) : Serialize(GetAsyncSendData(_encryptor.Encrypt(packets1), headers));
    }

    protected static string Serialize<T>(T request)
    {
        return Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(request, JsonSerializerConfig.JsonSerializerOptions));
    }

    private string GetSendData<TRequest, TResponse>(
        PacketDto<TRequest> packet,
        Dictionary<string, string> headers,
        bool encrypt,
        bool sign)
    {
        var packet1 = sign ? GetSignedPacket(packet) : packet;
        return !encrypt ? Serialize(GetSyncSendData(packet1, headers)) : Serialize(GetSyncSendData(_encryptor.Encrypt(packet), headers));
    }

    protected abstract SyncRequestDto<T> GetSyncSendData<T>(
        PacketDto<T> packet,
        Dictionary<string, string> headers);

    protected abstract AsyncRequestDto<T> GetAsyncSendData<T>(
        List<PacketDto<T>> packets,
        Dictionary<string, string> headers);

    private static string GetUrl(PriorityLevel priorityLevel) =>
        string.Concat("async/", priorityLevel == PriorityLevel.HIGH ? "fast-enqueue" : "normal-enqueue");

    private static string GetUrl(string packetType) =>
        string.Concat("sync/", packetType.ToUpperInvariant());

    private static void FillEssentialHeaders(IDictionary<string, string> essentialHeaders)
    {
        if (!essentialHeaders.ContainsKey(TransferConstants.RequestTraceIdHeader))
            essentialHeaders.Add(TransferConstants.RequestTraceIdHeader, Guid.NewGuid().ToString());
        if (essentialHeaders.ContainsKey(TransferConstants.TimestampHeader))
            return;
        var timeMilliseconds = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
        essentialHeaders.Add(TransferConstants.TimestampHeader, timeMilliseconds.ToString());
    }

    private PacketDto<T> GetSignedPacket<T>(PacketDto<T> packet)
    {
        var dataSignature = Signatory.Sign(Normalizer.Normalize<T>(packet.Data!, new Dictionary<string, string>())!);
        return packet with { DataSignature = dataSignature, SignatureKeyId = Signatory.GetKeyId() };
    }

    private PacketDto<string> GetSerializedPacket<T>(PacketDto<T> packet)
    {
        return new PacketDto<string>(packet.Uid, packet.PacketType, packet.FiscalId, Serialize<T>(packet.Data!), packet.Retry, packet.EncryptionKeyId, packet.SymmetricKey, packet.Iv, packet.DataSignature, packet.SignatureKeyId);
    }
}