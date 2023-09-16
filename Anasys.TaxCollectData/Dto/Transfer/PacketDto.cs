namespace Anasys.TaxCollectData.Dto.Transfer;

public record PacketDto<T>(
    string Uid,
    string PacketType,
    string FiscalId,
    T? Data,
    bool Retry,
    string? EncryptionKeyId,
    string? SymmetricKey,
    string? Iv,
    string? DataSignature,
    string? SignatureKeyId);