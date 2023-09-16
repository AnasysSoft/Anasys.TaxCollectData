namespace Anasys.TaxCollectData.Dto;

public record PacketDtoWithoutSignatureKeyId<T>(
    string Uid,
    string PacketType,
    string FiscalId,
    T? Data,
    bool Retry,
    string? EncryptionKeyId,
    string? SymmetricKey,
    string? Iv,
    string? DataSignature);