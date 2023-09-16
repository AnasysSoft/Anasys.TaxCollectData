using Anasys.TaxCollectData.Abstraction;
using Anasys.TaxCollectData.Dto.Config;
using Anasys.TaxCollectData.Exceptions;
using Anasys.TaxCollectData.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Anasys.TaxCollectData.Business;

public class TaxApiService
{
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();
    private IServiceProvider? _serviceProvider;
    private bool _isInitialized;
    private ITaxApis? _taxApis;
    private ITransferApi? _transferApi;
    private ITaxIdGenerator? _taxIdGenerator;

    public static TaxApiService Instance { get; } = new();

    public ITaxApis TaxApis => _taxApis ??= _serviceProvider!.GetService<ITaxApis>() ?? throw new NotInitializedException("_taxApis");

    public ITransferApi TransferApi => _transferApi ??= _serviceProvider!.GetService<ITransferApi>() ?? throw new NotInitializedException("_transferApi");

    public ITaxIdGenerator TaxIdGenerator => _taxIdGenerator ??= _serviceProvider!.GetService<ITaxIdGenerator>() ?? throw new NotInitializedException("_taxIdGenerator");

    public void Init(
        string clientId,
        SignatoryConfig signatoryConfig,
        string baseUrl = "https://wantolan.ir/requestsmanager/api/self-tsp/",
        string apiVersion = "",
        EncryptionConfig? encryptionConfig = null)
    {
        if (_isInitialized)
            return;
        _serviceCollection.AddTaxApi(baseUrl, apiVersion, clientId, signatoryConfig, encryptionConfig);
        _serviceProvider = _serviceCollection.BuildServiceProvider();
        _isInitialized = true;
    }

    public void Init<TSignatory>(
        string clientId,
        string baseUrl = "https://wantolan.ir/requestsmanager/api/self-tsp/",
        string apiVersion = "",
        EncryptionConfig? encryptionConfig = null)
        where TSignatory : class, ISignatory
    {
        if (_isInitialized)
            return;
        _serviceCollection.AddTaxApi<TSignatory>(baseUrl, apiVersion, clientId, encryptionConfig);
        _serviceProvider = _serviceCollection.BuildServiceProvider();
        _isInitialized = true;
    }
}