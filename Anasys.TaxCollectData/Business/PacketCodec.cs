using System.Security.Cryptography;
using Anasys.TaxCollectData.Abstraction;

namespace Anasys.TaxCollectData.Business;

internal class PacketCodec : IPacketCodec
{
    public byte[] GenerateAesSecretKey()
    {
        var aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();
        return aes.Key;
    }

    public byte[] GenerateIv()
    {
        var iv = new byte[16];
        RandomNumberGenerator.Create().GetNonZeroBytes(iv);
        return iv;
    }

    public byte[] Xor(byte[] b1, byte[] b2) => b1.Length >= b2.Length ? XorBlocks(b2, b1) : XorBlocks(b1, b2);

    private byte[] XorBlocks(byte[] smallerArray, byte[] biggerArray)
    {
        var numArray = new byte[biggerArray.Length];
        var num = (int)Math.Ceiling(biggerArray.Length / (double)smallerArray.Length);
        for (var index1 = 0; index1 < num; ++index1)
        {
            for (var index2 = 0; index2 < smallerArray.Length && index1 * smallerArray.Length + index2 < biggerArray.Length; ++index2)
                numArray[index1 * smallerArray.Length + index2] = (byte)((uint)smallerArray[index2] ^ biggerArray[index1 * smallerArray.Length + index2]);
        }

        return numArray;
    }
}