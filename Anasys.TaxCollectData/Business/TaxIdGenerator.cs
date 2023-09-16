using System.Text;
using Anasys.TaxCollectData.Abstraction;

namespace Anasys.TaxCollectData.Business;

public class TaxIdGenerator : ITaxIdGenerator
{
    private readonly IVerhoeffProvider _verhoffProvider;

    public TaxIdGenerator(IVerhoeffProvider verhoffProvider)
    {
        _verhoffProvider = verhoffProvider;
    }

    public string GenerateTaxId(string memoryId, long serial, DateTime createDate)
    {
        var num = (int)(new DateTimeOffset(createDate).ToUnixTimeSeconds() / 86400L);
        var str1 = Convert.ToString(num, 16);
        var str2 = Convert.ToString(serial, 16);

        var verhoeff = _verhoffProvider.GenerateVerhoeff($"{ToDecimal(memoryId)}{num.ToString().PadLeft(6, '0')}{serial.ToString().PadLeft(12, '0')}");

        return $"{memoryId}{str1.PadLeft(5, '0')}{str2.PadLeft(10, '0')}{verhoeff}".ToUpperInvariant();
    }

    private static string ToDecimal(string memoryId)
    {
        var stringBuilder = new StringBuilder();
        foreach (var c in memoryId)
            if (char.IsDigit(c))
                stringBuilder.Append(c);
            else
                stringBuilder.Append((int)c);
        return stringBuilder.ToString();
    }
}