using Anasys.TaxCollectData.Enums;

namespace Anasys.TaxCollectData.Dto.Content;

public record FiscalFullInformationModel(string NameTrade,
    FiscalStatus FiscalStatus,
    decimal SaleThreshold,
    string EconomicCode);