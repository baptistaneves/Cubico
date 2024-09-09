namespace RealEstate.Application.Commands.Categories.DeleteCategory;
public record DeleteCategoryResult(bool IsSuccess);

public record DeleteCategoryCommand(Guid Id) : ICommand<DeleteCategoryResult>;
