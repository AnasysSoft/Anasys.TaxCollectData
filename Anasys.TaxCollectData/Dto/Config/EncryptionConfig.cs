namespace Anasys.TaxCollectData.Dto.Config;

public record EncryptionConfig(string TaxOrgPublicKey, string EncryptionKeyId)
{
    public string TaxOrgPublicKey { get; internal set; } = TaxOrgPublicKey;

    public string EncryptionKeyId { get; internal set; } = EncryptionKeyId;

    public EncryptionConfig()
        : this(string.Empty, string.Empty)
    {
    }
}