using System.Text;

namespace Anasys.TaxCollectData.Dto.Transfer;

public record SyncResponseModel<T>(long Timestamp,
        ResponsePacketModel<T> Result,
        List<ErrorModel> Errors)
    : SignedRequest
{
    protected override Type EqualityContract => typeof(SyncResponseModel<T>);

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(nameof(SyncResponseModel<T>));
        builder.Append(" { ");
        if (this.PrintMembers(builder))
            builder.Append(' ');
        builder.Append('}');
        return builder.ToString();
    }

    protected override bool PrintMembers(StringBuilder builder)
    {
        if (base.PrintMembers(builder))
            builder.Append(", ");
        builder.Append("Timestamp = ");
        builder.Append(Timestamp);
        builder.Append(", Result = ");
        builder.Append(Result);
        builder.Append(", Errors = ");
        builder.Append(Errors);
        return true;
    }
}