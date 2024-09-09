namespace RealEstate.Application.Queries.Provinces.GetProvinces;

public record GetProvincesResult(IEnumerable<ProvinceDto> ProvincesDto);

public record GetProvincesQuery() : IQuery<GetProvincesResult>;