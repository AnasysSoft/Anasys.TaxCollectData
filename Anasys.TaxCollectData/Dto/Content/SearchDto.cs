using System.Runtime.CompilerServices;
using System.Text;

namespace Anasys.TaxCollectData.Dto.Content;

public record SearchDto(List<FilterDto> Filters,
    int Page,
    int Size,
    List<OrderByDto> OrderBy,
    bool SkipCount)
{
    public SearchDto(int page, int size)
        : this(new List<FilterDto>(), page, size, new List<OrderByDto>(), false)
    {
    }

    public void AddFilter(FilterDto filterDto) => Filters.Add(filterDto);

    protected virtual bool PrintMembers(StringBuilder builder)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        builder.Append("Filters = ");
        builder.Append(Filters);
        builder.Append(", Page = ");
        builder.Append(Page);
        builder.Append(", Size = ");
        builder.Append(Size);
        builder.Append(", SkipCount = ");
        builder.Append(SkipCount);
        builder.Append(", OrderBy = ");
        builder.Append(OrderBy);
        return true;
    }
}