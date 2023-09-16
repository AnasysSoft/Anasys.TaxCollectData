using System.Text;

namespace Anasys.TaxCollectData.Dto.Transfer;

public record AsyncRequestDto<T> : SignedRequest
{
    protected override Type EqualityContract => typeof(AsyncRequestDto<T>);

    public List<PacketDto<T>> Packets { get; }

    public AsyncRequestDto(List<PacketDto<T>> packets) => Packets = packets ?? throw new ArgumentNullException(nameof(packets));

    public AsyncRequestDto(string signature, string? signatureKeyId, List<PacketDto<T>> packets)
        : base(signature, signatureKeyId)
    {
        Packets = packets ?? throw new ArgumentNullException(nameof(packets));
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(nameof(AsyncRequestDto<T>));
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
        builder.Append("Packets = ");
        builder.Append(Packets);
        return true;
    }
}