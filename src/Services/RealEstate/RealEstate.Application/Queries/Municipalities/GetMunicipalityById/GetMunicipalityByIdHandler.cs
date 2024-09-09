namespace RealEstate.Application.Queries.Municipalities.GetMunicipalityById;

public class GetMunicipalityByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetMunicipalityByIdQuery, GetMunicipalityByIdResult>
{
    public async Task<GetMunicipalityByIdResult> Handle(GetMunicipalityByIdQuery query, CancellationToken cancellationToken)
    {
        var municipality = await dbContext.Municipalities
                                    .AsNoTracking()
                                    .Include(x => x.Province)
                                    .Where(x => x.Id.Value == query.Id)
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
                                    }).FirstOrDefaultAsync(cancellationToken);

        if (municipality is null)
        {
            throw new RealEstateNotFoundException(string.Format(ErrorMessages.NotFound, "Municipality"));
        }

        return new GetMunicipalityByIdResult(municipality);
    }
}
