using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Anasys.TaxCollectData.Abstraction;
using Anasys.TaxCollectData.Dto.Config;
using Anasys.TaxCollectData.Dto.Transfer;
using Anasys.TaxCollectData.Exceptions;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Anasys.TaxCollectData.Business;

internal class DefaultEncryptor : IEncryptor
{
    private readonly IPacketCodec _packetCodec;
    private readonly EncryptionConfig _encryptionConfig;

    public DefaultEncryptor(IPacketCodec packetCodec, EncryptionConfig encryptionConfig)
    {
        _packetCodec = packetCodec;
        _encryptionConfig = encryptionConfig;
    }

    public List<PacketDto<string>> Encrypt<T>(List<PacketDto<T>> packets)
    {
        var aesKey = _packetCodec.GenerateAesSecretKey();
        var iv = _packetCodec.GenerateIv();
        var rsaEncryptSymmetricKey = EncryptData(BitConverter.ToString(aesKey).Replace("-", ""), _encryptionConfig.TaxOrgPublicKey);
        try
        {
            return packets.Select(packetDto => GetEncryptedPacket(packetDto, aesKey, iv, rsaEncryptSymmetricKey)).ToList();
        }
        catch (Exception ex)
        {
            throw new TaxApiException("unable to encrypt invoice json using AES symmetric key", ex);
        }
    }

    public PacketDto<string> Encrypt<T>(PacketDto<T> packet)
    {
        var aesSecretKey = _packetCodec.GenerateAesSecretKey();
        var iv = _packetCodec.GenerateIv();
        var rsaEncryptSymmetricKey = EncryptData(BitConverter.ToString(aesSecretKey).Replace("-", ""), _encryptionConfig.TaxOrgPublicKey);
        try
        {
            return GetEncryptedPacket(packet, aesSecretKey, iv, rsaEncryptSymmetricKey);
        }
        catch (Exception ex)
        {
            throw new TaxApiException("unable to encrypt invoice json using AES symmetric key", ex);
        }
    }

    private PacketDto<string> GetEncryptedPacket<T>(
        PacketDto<T> packetDto,
        byte[] aesKey,
        byte[] iv,
        string rsaEncryptSymmetricKey) =>
        new (packetDto.Uid,
            packetDto.PacketType,
            packetDto.FiscalId,
            GetAesEncrypt(packetDto, aesKey, iv),
            packetDto.Retry,
            _encryptionConfig.EncryptionKeyId,
            rsaEncryptSymmetricKey,
            BitConverter.ToString(iv).Replace("-", ""),
            packetDto.DataSignature,
            packetDto.SignatureKeyId);

    private string GetAesEncrypt<T>(PacketDto<T> packetDto, byte[] aesKey, byte[] iv)=>
        AesEncrypt(_packetCodec.Xor(JsonSerializer.SerializeToUtf8Bytes<T>(packetDto.Data!, JsonSerializerConfig.JsonSerializerOptions), aesKey), aesKey, iv);

    private string AesEncrypt(byte[] payload, byte[] key, byte[] iv)
    {
        var gcmBlockCipher = new GcmBlockCipher(new AesEngine());
        var associatedText = Array.Empty<byte>();
        gcmBlockCipher.Init(true, new AeadParameters(new KeyParameter(key), 128, iv, associatedText));
        var numArray = new byte[gcmBlockCipher.GetOutputSize(payload.Length)];
        var outOff = gcmBlockCipher.ProcessBytes(payload, 0, payload.Length, numArray, 0);
        gcmBlockCipher.DoFinal(numArray, outOff);
        return Convert.ToBase64String(numArray);
    }

    private string EncryptData(string stringToBeEncrypted, string publicKey)
    {
        try
        {
            var key = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            var parameters = new RSAParameters()
            {
                Modulus = key.Modulus.ToByteArrayUnsigned(),
                Exponent = key.Exponent.ToByteArrayUnsigned()
            };
            var rsaCng = RSA.Create();
            rsaCng.ImportParameters(parameters);
            return Convert.ToBase64String(rsaCng.Encrypt(Encoding.UTF8.GetBytes(stringToBeEncrypted), RSAEncryptionPadding.OaepSHA256));
        }
        catch
        {
            return "error";
        }
    }
}