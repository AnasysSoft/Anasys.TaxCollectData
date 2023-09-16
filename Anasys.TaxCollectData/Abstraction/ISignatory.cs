namespace Anasys.TaxCollectData.Abstraction;

public interface ISignatory
{
    string Sign(string data);

    string? GetKeyId();
}