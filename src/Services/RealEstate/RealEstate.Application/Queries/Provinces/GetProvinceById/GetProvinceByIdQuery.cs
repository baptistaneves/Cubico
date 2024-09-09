namespace RealEstate.Application.Queries.Provinces.GetProvinceById;

public record GetProvinceByIdResult(ProvinceDto ProvinceDto);

public record GetProvinceByIdQuery(Guid Id) : IQuery<GetProvinceByIdResult>;
