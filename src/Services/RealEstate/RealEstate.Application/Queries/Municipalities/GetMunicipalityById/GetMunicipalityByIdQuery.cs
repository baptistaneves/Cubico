namespace RealEstate.Application.Queries.Municipalities.GetMunicipalityById;
public record GetMunicipalityByIdResult(MunicipalityDto MunicipalityDto);

public record GetMunicipalityByIdQuery(Guid Id) : IQuery<GetMunicipalityByIdResult>;
