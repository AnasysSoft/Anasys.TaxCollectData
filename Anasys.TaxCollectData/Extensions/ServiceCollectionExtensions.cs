using Anasys.TaxCollectData.Abstraction;
using Anasys.TaxCollectData.Api;
using Anasys.TaxCollectData.Business;
using Anasys.TaxCollectData.Business.Normalize;
using Anasys.TaxCollectData.Business.Signatory;
using Anasys.TaxCollectData.Dto;
using Anasys.TaxCollectData.Dto.Config;
using Anasys.TaxCollectData.Dto.Transfer;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Anasys.TaxCollectData.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTaxApi<TSignatory>(
        this IServiceCollection serviceDescriptors,
        string baseUrl,
        string apiVersion,
        string clientId,
        EncryptionConfig? encryptionConfig = null)
        where TSignatory : class, ISignatory
    {
        serviceDescriptors.AddServiceCollection(baseUrl, apiVersion, clientId, encryptionConfig);
        serviceDescriptors.AddSingleton<ISignatory, TSignatory>();
        return serviceDescriptors;
    }

    public static IServiceCollection AddTaxApi(
        this IServiceCollection serviceDescriptors,
        string baseUrl,
        string apiVersion,
        string clientId,
        SignatoryConfig signatoryConfig,
        EncryptionConfig? encryptionConfig = null)
    {
        serviceDescriptors.AddServiceCollection(baseUrl, apiVersion, clientId, encryptionConfig);
        serviceDescriptors.AddSingleton<ISignatory, InMemorySignatory>();
        serviceDescriptors.AddSingleton(signatoryConfig);
        return serviceDescriptors;
    }

    private static IServiceCollection AddServiceCollection(
        this IServiceCollection serviceDescriptors,
        string baseUrl,
        string apiVersion,
        string clientId,
        EncryptionConfig? encryptionConfig)
    {
        serviceDescriptors.AddAutoMapper((Action<IMapperConfigurationExpression>)(c => c.CreateMap(typeof(PacketDto<>), typeof(PacketDtoWithoutSignatureKeyId<>))));
        serviceDescriptors.AddSingleton<IEncryptor, DefaultEncryptor>();
        serviceDescriptors.AddSingleton<IVerhoeffProvider, VerhoffProvider>();
        serviceDescriptors.AddSingleton<ITaxApis, DefaultTaxApiClient>(p =>
        {
            var service = p.GetService<ITransferApi>();
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            
            var clientId1 = clientId;
            return new DefaultTaxApiClient(service, clientId1, p.GetService<EncryptionConfig>() ?? throw new ArgumentNullException(nameof(encryptionConfig)));
        });
        serviceDescriptors.AddSingleton(encryptionConfig ?? new EncryptionConfig());
        serviceDescriptors.AddSingleton<IPacketCodec, PacketCodec>();
        serviceDescriptors.AddSingleton<ITaxIdGenerator, TaxIdGenerator>();
        serviceDescriptors.AddApiVersionBasedConfigs(baseUrl, apiVersion);
        return serviceDescriptors;
    }

    private static IServiceCollection AddApiVersionBasedConfigs(
        this IServiceCollection serviceDescriptors,
        string baseUrl,
        string apiVersion)
    {
        baseUrl = baseUrl.EndsWith("/") ? baseUrl : $"{baseUrl}/";
        if (string.IsNullOrWhiteSpace(apiVersion))
        {
            serviceDescriptors.AddHttpClientByUri(new Uri(baseUrl));
            serviceDescriptors.AddSingleton<INormalizer, ObjectNormalizer>();
            serviceDescriptors.AddSingleton<ITransferApi, ObjectTransferApi>();
        }
        else if (apiVersion.Equals("v1", StringComparison.InvariantCultureIgnoreCase))
        {
            serviceDescriptors.AddHttpClientByUri(new Uri(new Uri(baseUrl), $"{apiVersion}/"));
            serviceDescriptors.AddSingleton<INormalizer, SimpleNormalizer>();
            serviceDescriptors.AddSingleton<ITransferApi, SimpleTransferApi>();
        }

        return serviceDescriptors;
    }

    private static IServiceCollection AddHttpClientByUri(
        this IServiceCollection serviceDescriptors,
        Uri uri)
    {
        serviceDescriptors.AddHttpClient<IHttpRequestSender, RestSharpHttpRequestSender>(client => client.BaseAddress = uri).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        });
        return serviceDescriptors;
    }
}