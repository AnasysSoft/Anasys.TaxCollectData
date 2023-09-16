using System.Runtime.CompilerServices;
using System.Text;

namespace Anasys.TaxCollectData.Dto.Content;

public record GetTokenDto(string Username)
{
    protected virtual bool PrintMembers(StringBuilder builder)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        builder.Append("Username = ");
        builder.Append(Username);
        return true;
    }
}