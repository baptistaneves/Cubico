namespace RealEstate.Application.Queries.Municipalities.GetMunicipalities;

public class GetMunicipalitiesHandler(IApplicationDbContext dbContext) : IQueryHandler<GetMunicipalitiesQuery, GetMunicipalitiesResult>
{
    public async Task<GetMunicipalitiesResult> Handle(GetMunicipalitiesQuery request, CancellationToken cancellationToken)
    {
        var municipalities = await dbContext.Municipalities
                                    .AsNoTracking()
                                    .Include(x => x.Province)
                                    .Select(municipality => new MunicipalityDto
                                    {
                                        Id = municipality.Id.Value,
                                        Name = municipality.Name,
                                        ProvinceId = municipality.ProvinceId,
                                        ProvinceName = municipality.Province.Name,
                                        LastModified = municipality.LastModified,
                                        LastModifiedBy = municipality.LastModifiedBy,
                                        CreatedAt = municipality.CreatedAt,
                                        CreatedBy = municipality.CreatedBy
                                    }).ToListAsync(cancellationToken);


        return new GetMunicipalitiesResult(municipalities);
    }
}
