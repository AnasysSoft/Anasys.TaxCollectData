using Anasys.TaxCollectData.Dto;
using Anasys.TaxCollectData.Dto.Transfer;
using Anasys.TaxCollectData.Enums;

namespace Anasys.TaxCollectData.Abstraction;

public interface ITransferApi
{
    Task<HttpResponse<AsyncResponseModel?>?> SendPacketsAsync<TRequest>(
        List<PacketDto<TRequest>> packets,
        Dictionary<string, string> headers,
        bool encrypt,
        bool sign,
        PriorityLevel priorityLevel = PriorityLevel.NORMAL);

    Task<HttpResponse<SyncResponseModel<TResponse>?>?> SendPacketAsync<TRequest, TResponse>(
        PacketDto<TRequest> packet,
        Dictionary<string, string> headers,
        bool encrypt,
        bool sign);
}