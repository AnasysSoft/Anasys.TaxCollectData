namespace Anasys.TaxCollectData.Dto.Transfer;

public record PacketResponse(string Uid,
    string ReferenceNumber,
    string ErrorCode,
    string ErrorDetail);