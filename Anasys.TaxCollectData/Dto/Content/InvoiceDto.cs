using System.Text.Json.Serialization;
using Anasys.TaxCollectData.Abstraction;

namespace Anasys.TaxCollectData.Dto.Content;

public record InvoiceDto(InvoiceHeaderDto Header,
    List<InvoiceBodyDto> Body,
    List<PaymentDto> Payments,
    List<InvoiceExtension> Extension) : IPacketableDto
{
    public InvoiceDto(int clientEntityId,
        InvoiceHeaderDto header,
        List<InvoiceBodyDto> body,
        List<PaymentDto> payments,
        List<InvoiceExtension> extension)
        : this(header, body, payments, extension)
    {
        ClientEntityId = clientEntityId;
    }

    [JsonIgnore]
    public string Uid { get; } = Guid.NewGuid().ToString();
    
    [JsonIgnore]
    public int ClientEntityId { get; }
}