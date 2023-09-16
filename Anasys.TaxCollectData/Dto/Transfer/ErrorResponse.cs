using System.Text;

namespace Anasys.TaxCollectData.Dto.Transfer;

public record ErrorResponse(long Timestamp, List<ErrorModel> Errors) : SignedRequest
{
    protected override Type EqualityContract => typeof(ErrorResponse);

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(nameof(ErrorResponse));
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
        builder.Append(", Errors = ");
        builder.Append(Errors);
        return true;
    }
}