namespace RealEstate.Application.Queries.Municipalities.GetMunicipalities;
public record GetMunicipalitiesResult(IEnumerable<MunicipalityDto> MunicipalityDtos);

public record GetMunicipalitiesQuery : IQuery<GetMunicipalitiesResult>;
