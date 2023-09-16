using System.Runtime.CompilerServices;
using System.Text;
using Anasys.TaxCollectData.Enums;

namespace Anasys.TaxCollectData.Dto.Content;

public record FilterDto(string Field, OperatorType Operator)
{
    public string Value { get; } = "";

    private bool Or { get; }

    public FilterDto(string field, OperatorType operatorType, string value)
        : this(field, operatorType)
    {
        Value = value;
    }

    public FilterDto(string field, OperatorType operatorType, string value, bool or)
        : this(field, operatorType, value)
    {
        Or = or;
    }

    protected virtual bool PrintMembers(StringBuilder builder)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        builder.Append("Field = ");
        builder.Append(Field);
        builder.Append(", Value = ");
        builder.Append(Value);
        builder.Append(", Operator = ");
        builder.Append(Operator);
        return true;
    }
}