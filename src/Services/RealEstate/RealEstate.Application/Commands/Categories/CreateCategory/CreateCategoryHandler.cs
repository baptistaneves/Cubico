namespace RealEstate.Application.Commands.Categories.CreateCategory;

public class CreateCategoryHandler(IApplicationDbContext dbContext) 
    : ICommandHandler<CreateCategoryCommand, CreateCategoryResult>
{
    public async Task<CreateCategoryResult> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        if (await CategoryExists(command.Name, cancellationToken))
        {
            throw new BadRequestException(string.Format(ErrorMessages.NameOrDescriptionExists, "Category"));
        }

        var newCategory = Category.Create(CategoryId.Of(Guid.NewGuid()), command.Name, command.CreatedBy);

        dbContext.Categories.Add(newCategory);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateCategoryResult(true);
    }

    private async Task<bool> CategoryExists(string name, CancellationToken cancellationToken)
    {
        return await dbContext.Categories
                .AsNoTracking()
                .Where(x => x.Name == name)
                .AnyAsync(cancellationToken);
    }
}
