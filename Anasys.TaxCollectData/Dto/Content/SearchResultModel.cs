namespace Anasys.TaxCollectData.Dto.Content;

public record SearchResultModel<T>(List<T> Result, PaginationModel Pagination);