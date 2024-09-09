namespace RealEstate.Application.Commands.Provinces.DeleteProvince;

public class DeleteProvinceHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteProvinceCommand, DeleteProvinceResult>
{
    public async Task<DeleteProvinceResult> Handle(DeleteProvinceCommand command, CancellationToken cancellationToken)
    {
        var province = await dbContext.Provinces
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id.Value == command.Id);

        if (province is null)
            throw new RealEstateNotFoundException(string.Format(ErrorMessages.NotFound, "Province"));

        dbContext.Provinces.Remove(province);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteProvinceResult(true);
    }
}
