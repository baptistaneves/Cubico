namespace RealEstate.Application.Commands.Categories.DeleteCategory;

public class DeleteCategoryHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteCategoryCommand, DeleteCategoryResult>
{
    public async Task<DeleteCategoryResult> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id.Value == command.Id);

        if (category is null)
            throw new NotFoundException(string.Format(ErrorMessages.NotFound, "Category"));

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteCategoryResult(true);
    }
}