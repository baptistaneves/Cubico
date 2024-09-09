namespace RealEstate.Application.Queries.Categories.GetCategoryById;
public record GetCategoryByIdResult(CategoryDto CategoryDto);

public record GetCategoryByIdQuery(Guid Id) : IQuery<GetCategoryByIdResult>;
