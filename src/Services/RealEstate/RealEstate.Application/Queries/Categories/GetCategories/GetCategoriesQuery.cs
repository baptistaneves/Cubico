namespace RealEstate.Application.Queries.Categories.GetCategories;
public record GetCategoriesResult(IEnumerable<CategoryDto> CategoryDtos);

public record GetCategoriesQuery : IQuery<GetCategoriesResult>;
