namespace Anasys.TaxCollectData.Dto.Content;

public record InquiryResultModel(string ReferenceNumber,
    string Uid,
    string Status,
    object Data,
    string PacketType,
    string FiscalId);