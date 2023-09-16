using System.Text;

namespace Anasys.TaxCollectData.Dto.Transfer;

public record AsyncResponseModel(long Timestamp,
        HashSet<PacketResponse> Result,
        List<ErrorModel> Errors)
    : SignedRequest
{
    protected override Type EqualityContract => typeof(AsyncResponseModel);

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(nameof(AsyncResponseModel));
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
        builder.Append("Timestamp = ");
        builder.Append(Timestamp);
        builder.Append(", Result = ");
        builder.Append(Result);
        builder.Append(", Errors = ");
        builder.Append(Errors);
        return true;
    }
}