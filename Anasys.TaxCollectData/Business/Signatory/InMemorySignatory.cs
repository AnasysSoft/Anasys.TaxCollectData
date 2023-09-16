using System.Security.Cryptography;
using System.Text;
using Anasys.TaxCollectData.Abstraction;
using Anasys.TaxCollectData.Dto.Config;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Anasys.TaxCollectData.Business.Signatory;

internal class InMemorySignatory : ISignatory
{
    private readonly SignatoryConfig _signatoryConfig;

    public InMemorySignatory(SignatoryConfig signatoryConfig) => 
        _signatoryConfig = signatoryConfig ?? throw new ArgumentNullException(nameof(signatoryConfig));

    public string Sign(string stringToBeSigned)
    {
        var rsaParameters = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)new PemReader(new StringReader($"-----BEGIN PRIVATE KEY-----\n{_signatoryConfig.PrivateKey}\n-----END PRIVATE KEY-----")).ReadObject());
        var cryptoServiceProvider = new RSACryptoServiceProvider();
        cryptoServiceProvider.ImportParameters(rsaParameters);
        return Convert.ToBase64String(cryptoServiceProvider.SignData(Encoding.UTF8.GetBytes(stringToBeSigned), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
    }

    public string? GetKeyId() => _signatoryConfig.KeyId;
}