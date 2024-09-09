namespace RealEstate.Application.Queries.Provinces.GetProvinceById;

public class GetProvinceByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetProvinceByIdQuery, GetProvinceByIdResult>
{
    public async Task<GetProvinceByIdResult> Handle(GetProvinceByIdQuery query, CancellationToken cancellationToken)
    {
        var province = await dbContext.Provinces
                                    .AsNoTracking()
                                    .Where(x => x.Id.Value == query.Id)
                                    .Select(province => new ProvinceDto
                                    {
                                        Id = province.Id.Value,
                                        Name = province.Name,
                                        LastModified = province.LastModified,
                                        LastModifiedBy = province.LastModifiedBy,
                                        CreatedAt = province.CreatedAt,
                                        CreatedBy = province.CreatedBy
                                    }).FirstOrDefaultAsync(cancellationToken);

        if (province is null)
        {
            throw new RealEstateNotFoundException(string.Format(ErrorMessages.NotFound, "Province"));
        }

        return new GetProvinceByIdResult(province);
    }
}
