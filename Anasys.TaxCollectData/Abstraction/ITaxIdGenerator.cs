namespace Anasys.TaxCollectData.Abstraction;

public interface ITaxIdGenerator
{
    string GenerateTaxId(string memoryId, long serial, DateTime createDate);
}