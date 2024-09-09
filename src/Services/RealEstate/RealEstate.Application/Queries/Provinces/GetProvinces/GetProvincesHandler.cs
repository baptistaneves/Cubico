namespace RealEstate.Application.Queries.Provinces.GetProvinces;

public class GetProvincesHandler(IApplicationDbContext dbContext) : IQueryHandler<GetProvincesQuery, GetProvincesResult>
{
    public async Task<GetProvincesResult> Handle(GetProvincesQuery query, CancellationToken cancellationToken)
    {
        var provinces = await dbContext.Provinces
                                    .AsNoTracking()
                                    .Select(province => new ProvinceDto
                                    {
                                        Id = province.Id.Value,
                                        Name = province.Name,
                                        LastModified = province.LastModified,
                                        LastModifiedBy = province.LastModifiedBy,
                                        CreatedAt = province.CreatedAt,
                                        CreatedBy = province.CreatedBy
                                    }).ToListAsync(cancellationToken);

        return new GetProvincesResult(provinces);
    }
}