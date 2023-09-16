using System.Text;

namespace Anasys.TaxCollectData.Dto.Transfer;

public record SyncRequestDto<T> : SignedRequest
{
    protected override Type EqualityContract => typeof(SyncRequestDto<T>);

    public SyncRequestDto(PacketDto<T> packet) => Packet = packet ?? throw new ArgumentNullException(nameof(packet));

    public SyncRequestDto(string signature, string? signatureKeyId, PacketDto<T> packet)
        : base(signature, signatureKeyId)
    {
        Packet = packet ?? throw new ArgumentNullException(nameof(packet));
    }

    public PacketDto<T> Packet { get; }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(nameof(SyncRequestDto<T>));
        builder.Append(" { ");
        if (PrintMembers(builder))
            builder.Append(' ');
        builder.Append('}');
        return builder.ToString();
    }

    protected override bool PrintMembers(StringBuilder builder)
    {
        if (base.PrintMembers(builder))
            builder.Append(", ");
        builder.Append("Packet = ");
        builder.Append(Packet);
        return true;
    }
}