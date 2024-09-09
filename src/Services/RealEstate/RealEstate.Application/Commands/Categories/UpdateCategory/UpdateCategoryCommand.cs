namespace RealEstate.Application.Commands.Categories.UpdateCategory;
public record UpdateCategoryResult(bool IsSuccess);

public record UpdateCategoryCommand(Guid Id, string Name, string LastModifiedBy) : ICommand<UpdateCategoryResult>;
