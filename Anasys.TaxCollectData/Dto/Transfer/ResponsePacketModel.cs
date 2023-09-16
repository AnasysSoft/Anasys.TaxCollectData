namespace Anasys.TaxCollectData.Dto.Transfer;

public record ResponsePacketModel<T>(string Uid,
    string PacketType,
    T? Data,
    string? EncryptionKeyId,
    string? SymmetricKey,
    string? Iv);