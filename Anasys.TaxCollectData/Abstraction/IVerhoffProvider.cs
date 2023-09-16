namespace Anasys.TaxCollectData.Abstraction;

public interface IVerhoeffProvider
{
    bool ValidateVerhoeff(string num);

    string GenerateVerhoeff(string num);
}