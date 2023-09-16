using Anasys.TaxCollectData.Dto.Transfer;

namespace Anasys.TaxCollectData.Abstraction;

public interface IEncryptor
{
    List<PacketDto<string>> Encrypt<T>(List<PacketDto<T>> packets);

    PacketDto<string> Encrypt<T>(PacketDto<T> packet);
}