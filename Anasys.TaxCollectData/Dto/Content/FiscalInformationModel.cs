using Anasys.TaxCollectData.Enums;

namespace Anasys.TaxCollectData.Dto.Content;

public record FiscalInformationModel
{
    public FiscalInformationModel(string nameTrade, FiscalStatus fiscalStatus, string economicCode)
    {
        this.NameTrade = nameTrade;
        this.FiscalStatus = fiscalStatus;
        this.EconomicCode = economicCode;
    }

    public string NameTrade { get; }

    public FiscalStatus FiscalStatus { get; }

    public string EconomicCode { get; }

    protected FiscalInformationModel(FiscalInformationModel original)
    {
        this.NameTrade = original.NameTrade;
        this.FiscalStatus = original.FiscalStatus;
        this.EconomicCode = original.EconomicCode;
    }
}