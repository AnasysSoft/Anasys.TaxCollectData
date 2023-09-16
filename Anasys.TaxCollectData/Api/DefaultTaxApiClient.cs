using Anasys.TaxCollectData.Abstraction;
using Microsoft.VisualStudio.Threading;
using Anasys.TaxCollectData.Constants;
using Anasys.TaxCollectData.Dto;
using Anasys.TaxCollectData.Dto.Config;
using Anasys.TaxCollectData.Dto.Content;
using Anasys.TaxCollectData.Dto.Transfer;
using Anasys.TaxCollectData.Exceptions;

namespace Anasys.TaxCollectData.Api;

internal class DefaultTaxApiClient : ITaxApis
{
    private readonly ITransferApi _transferApi;
    private readonly string _clientId;
    private readonly EncryptionConfig _encryptionConfig;
    private TokenModel? _token;

    public DefaultTaxApiClient(
        ITransferApi transferApi,
        string clientId,
        EncryptionConfig encryptionConfig)
    {
        _transferApi = transferApi;
        _clientId = clientId;
        _encryptionConfig = encryptionConfig;
    }

    public TokenModel? RequestToken() =>
        _token = GetPacketResponse<GetTokenDto, TokenModel>(PacketDtoBuilder(new GetTokenDto(_clientId), PacketTypeConstants.PacketTypeGetToken),
            new Dictionary<string, string>(),
            false,
            false);

    public async Task<TokenModel?> RequestTokenAsync() =>
        _token = await GetPacketResponseAsync<GetTokenDto, TokenModel>(PacketDtoBuilder(new GetTokenDto(_clientId), PacketTypeConstants.PacketTypeGetToken),
                new Dictionary<string, string>(),
                false,
                false)
            .ConfigureAwait(false);

    public HttpResponse<AsyncResponseModel?>? SendInvoices(List<InvoiceDto> invoices)
    {
        using var owner = new JoinableTaskContext();
        var joinableTaskFactory = new JoinableTaskFactory(owner);
        var packets = invoices.Select(invoiceDto => PacketDtoBuilder(invoiceDto, PacketTypeConstants.InvoiceV01)).ToList();
        var func = (async () => await _transferApi.SendPacketsAsync(packets, GetHeaders(), true, true).ConfigureAwait(false));
        return joinableTaskFactory.Run(func);
    }

    public async Task<HttpResponse<AsyncResponseModel?>?> SendInvoicesAsync(List<InvoiceDto> invoices)
    {
        var list = invoices.Select(invoiceDto => PacketDtoBuilder(invoiceDto, PacketTypeConstants.InvoiceV01)).ToList();
        return await _transferApi.SendPacketsAsync(list, GetHeaders(), true, true).ConfigureAwait(false);
    }

    public ServerInformationModel? GetServerInformation()
    {
        var packetResponse = GetPacketResponse<object, ServerInformationModel>(PacketDtoBuilder((object?)null, PacketTypeConstants.PacketTypeGetServerInformation),
            new Dictionary<string, string>(),
            false,
            false);
        SetEncryptionConfig(packetResponse);
        return packetResponse;
    }

    public async Task<ServerInformationModel?> GetServerInformationAsync()
    {
        var responseData = await GetPacketResponseAsync<object, ServerInformationModel>(PacketDtoBuilder((object?)null, PacketTypeConstants.PacketTypeGetServerInformation),
                new Dictionary<string, string>(),
                false,
                false)
            .ConfigureAwait(false);
        SetEncryptionConfig(responseData);
        return responseData;
    }

    public List<InquiryResultModel>? InquiryByUidAndFiscalId(List<UidAndFiscalId> uidAndFiscalIds) =>
        GetPacketResponse<List<UidAndFiscalId>, List<InquiryResultModel>>(PacketDtoBuilder(uidAndFiscalIds, PacketTypeConstants.PacketTypeInquiryByUid),
            GetHeaders(),
            false,
            false);

    public async Task<List<InquiryResultModel>?> InquiryByUidAndFiscalIdAsync(List<UidAndFiscalId> uidAndFiscalIds) =>
        await GetPacketResponseAsync<List<UidAndFiscalId>, List<InquiryResultModel>>(PacketDtoBuilder(uidAndFiscalIds, PacketTypeConstants.PacketTypeInquiryByUid),
                GetHeaders(),
                false,
                false)
            .ConfigureAwait(false);

    public List<InquiryResultModel>? InquiryByTime(string persianTime) =>
        GetPacketResponse<InquiryByTimeDto, List<InquiryResultModel>>(PacketDtoBuilder(new InquiryByTimeDto(persianTime), PacketTypeConstants.PacketTypeInquiryByTime),
            GetHeaders(),
            false,
            false);

    public async Task<List<InquiryResultModel>?> InquiryByTimeAsync(string persianTime) =>
        await GetPacketResponseAsync<InquiryByTimeDto, List<InquiryResultModel>>(PacketDtoBuilder(new InquiryByTimeDto(persianTime), PacketTypeConstants.PacketTypeInquiryByTime),
                GetHeaders(),
                false,
                false)
            .ConfigureAwait(false);

    public List<InquiryResultModel>? InquiryByTimeRange(string startDatePersian, string toDatePersian) =>
        GetPacketResponse<InquiryByTimeRangeDto, List<InquiryResultModel>>(PacketDtoBuilder(new InquiryByTimeRangeDto(startDatePersian, toDatePersian), PacketTypeConstants.PacketTypeInquiryByTimeRange),
            GetHeaders(),
            false,
            false);

    public async Task<List<InquiryResultModel>?> InquiryByTimeRangeAsync(string startDatePersian, string toDatePersian) =>
        await GetPacketResponseAsync<InquiryByTimeRangeDto, List<InquiryResultModel>>(PacketDtoBuilder(new InquiryByTimeRangeDto(startDatePersian, toDatePersian), PacketTypeConstants.PacketTypeInquiryByTimeRange),
                GetHeaders(),
                false,
                false)
            .ConfigureAwait(false);

    public List<InquiryResultModel>? InquiryByReferenceId(List<string> referenceIds) =>
        GetPacketResponse<InquiryByReferenceNumberDto, List<InquiryResultModel>>(PacketDtoBuilder(new InquiryByReferenceNumberDto(referenceIds), PacketTypeConstants.PacketTypeInquiryByReferenceNumber), 
            GetHeaders(), 
            false, 
            false);

    public async Task<List<InquiryResultModel>?> InquiryByReferenceIdAsync(List<string> referenceIds) =>
        await GetPacketResponseAsync<InquiryByReferenceNumberDto, List<InquiryResultModel>>(PacketDtoBuilder(new InquiryByReferenceNumberDto(referenceIds), PacketTypeConstants.PacketTypeInquiryByReferenceNumber), 
            GetHeaders(), 
            false, 
            false)
            .ConfigureAwait(false);

    public FiscalInformationModel? GetFiscalInformation(string fiscalId) =>
        GetPacketResponse<string, FiscalInformationModel>(PacketDtoBuilder(fiscalId, PacketTypeConstants.PacketTypeGetFiscalInformation), 
            GetHeaders(), 
            false, 
            false);

    public async Task<FiscalInformationModel?> GetFiscalInformationAsync(string fiscalId) =>
        await GetPacketResponseAsync<string, FiscalInformationModel>(PacketDtoBuilder(fiscalId, PacketTypeConstants.PacketTypeGetFiscalInformation), 
                GetHeaders(), 
                false, 
                false)
            .ConfigureAwait(false);

    public SearchResultModel<ServiceStuffModel>? GetServiceStuffList(SearchDto searchDto) =>
        GetPacketResponse<SearchDto, SearchResultModel<ServiceStuffModel>>(PacketDtoBuilder(searchDto, PacketTypeConstants.PacketTypeGetServiceStuffList), 
            GetHeaders(), 
            false, 
            false, 
            true);

    public async Task<SearchResultModel<ServiceStuffModel>?> GetServiceStuffListAsync(SearchDto searchDto) =>
        await GetPacketResponseAsync<SearchDto, SearchResultModel<ServiceStuffModel>>(PacketDtoBuilder(searchDto, PacketTypeConstants.PacketTypeGetServiceStuffList), 
            GetHeaders(), 
            false, 
            false, 
            true)
            .ConfigureAwait(false);

    public EconomicCodeModel? GetEconomicCodeInformation(string economicCode) =>
        GetPacketResponse<EconomicCodeDto, EconomicCodeModel>(PacketDtoBuilder(new EconomicCodeDto(economicCode), PacketTypeConstants.PacketTypeGetEconomicCodeInformation), 
            GetHeaders(), 
            false, 
            false);

    public async Task<EconomicCodeModel?> GetEconomicCodeInformationAsync(string economicCode) =>
        await GetPacketResponseAsync<EconomicCodeDto, EconomicCodeModel>(PacketDtoBuilder(new EconomicCodeDto(economicCode), PacketTypeConstants.PacketTypeGetEconomicCodeInformation), 
            GetHeaders(), 
            false, 
            false)
            .ConfigureAwait(false);

    private void SetEncryptionConfig(ServerInformationModel? responseData)
    {
        if (responseData?.PublicKeys == null || !responseData.PublicKeys.Any())
            return;
        var keyDto = responseData.PublicKeys.First();
        _encryptionConfig.TaxOrgPublicKey = keyDto.Key;
        _encryptionConfig.EncryptionKeyId = keyDto.Id;
    }

    private Dictionary<string, string> GetHeaders()
    {
        var headers = new Dictionary<string, string>();
        if (_token != null)
            headers.Add(TransferConstants.AuthorizationHeader, $"Bearer {_token.Token}");
        return headers;
    }

    private PacketDto<T> PacketDtoBuilder<T>(T? data, string packetType)
    {
        var uid = (data is IPacketableDto p) ? p.Uid : Guid.NewGuid().ToString();
        return new(uid, packetType, _clientId, data, false, null, null, null, null, null);
    }

    private async Task<TResponse?> GetPacketResponseAsync<TRequest, TResponse>(
        PacketDto<TRequest> packet,
        Dictionary<string, string> headers,
        bool encrypt,
        bool sign,
        bool returnNullIfResponseIsNull = false)
        where TResponse : class
    {
        var httpResponse = await _transferApi.SendPacketAsync<TRequest, TResponse>(packet, headers, encrypt, sign).ConfigureAwait(false);
        if (returnNullIfResponseIsNull && httpResponse is null)
            return default;
        if (httpResponse?.Body?.Errors != null && httpResponse.Body.Errors.Any())
            throw new TaxApiException(httpResponse.Body.Errors[0].Detail);
        TResponse? packetResponseAsync;
        if (httpResponse is null)
        {
            packetResponseAsync = default;
        }
        else
        {
            var body = httpResponse.Body;
            packetResponseAsync = body?.Result.Data;
        }

        return packetResponseAsync;
    }

    private TResponse? GetPacketResponse<TRequest, TResponse>(
        PacketDto<TRequest> packet,
        Dictionary<string, string> headers,
        bool encrypt,
        bool sign,
        bool returnNullIfResponseIsNull = false)
        where TResponse : class
    {
        using var owner = new JoinableTaskContext();
        return new JoinableTaskFactory(owner).Run(async () => await GetPacketResponseAsync<TRequest, TResponse>(packet, headers, encrypt, sign, returnNullIfResponseIsNull).ConfigureAwait(true));
    }
}