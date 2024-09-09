namespace RealEstate.Application.Commands.Categories.CreateCategory;
public record CreateCategoryResult(bool IsSuccess);

public record CreateCategoryCommand(string Name, string CreatedBy) : ICommand<CreateCategoryResult>;
