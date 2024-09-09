namespace RealEstate.Application.Queries.Categories.GetCategories;

public class GetCategoriesHandler(IApplicationDbContext dbContext) 
    : IQueryHandler<GetCategoriesQuery, GetCategoriesResult>
{
    public async Task<GetCategoriesResult> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories
                                    .AsNoTracking()
                                    .Select(category => new CategoryDto
                                    {
                                        Id = category.Id.Value,
                                        Name = category.Name,
                                        LastModified = category.LastModified,
                                        LastModifiedBy = category.LastModifiedBy,
                                        CreatedAt = category.CreatedAt,
                                        CreatedBy = category.CreatedBy
                                    }).ToListAsync(cancellationToken);


        return new GetCategoriesResult(categories);
    }
}