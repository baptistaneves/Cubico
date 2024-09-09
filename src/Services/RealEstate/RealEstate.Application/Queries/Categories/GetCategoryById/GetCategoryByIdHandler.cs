namespace RealEstate.Application.Queries.Categories.GetCategoryById;

public class GetCategoryByIdHandler(IApplicationDbContext dbContext) 
    : IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdResult>
{
    public async Task<GetCategoryByIdResult> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
                                    .AsNoTracking()
                                    .Where(x => x.Id.Value == query.Id)
                                    .Select(category => new CategoryDto
                                    {
                                        Id = category.Id.Value,
                                        Name = category.Name,
                                        LastModified = category.LastModified,
                                        LastModifiedBy = category.LastModifiedBy,
                                        CreatedAt = category.CreatedAt,
                                        CreatedBy = category.CreatedBy
                                    }).FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(string.Format(ErrorMessages.NotFound, "Category"));
        }

        return new GetCategoryByIdResult(category);
    }
}