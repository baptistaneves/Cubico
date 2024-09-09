namespace RealEstate.Application.Commands.Categories.UpdateCategory;

public class UpdateCategoryHandler(IApplicationDbContext dbContext) 
    : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResult>
{
    public async Task<UpdateCategoryResult> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id.Value == command.Id);

        if (category is null)
        {
            throw new NotFoundException(string.Format(ErrorMessages.NotFound, "Category"));
        }

        if (await CategoryExists(command.Id, command.Name, cancellationToken))
        {
            throw new BadRequestException(string.Format(ErrorMessages.NameOrDescriptionExists, "Category"));
        }

        category.Update(command.Name, command.LastModifiedBy);

        dbContext.Categories.Update(category);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateCategoryResult(true);
    }

    private async Task<bool> CategoryExists(Guid id, string name, CancellationToken cancellationToken)
    {
        return await dbContext.Categories
                .AsNoTracking()
                .Where(x => x.Name == name && x.Id.Value != id)
                .AnyAsync(cancellationToken);
    }
}
