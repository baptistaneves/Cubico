namespace RealEstate.Application.Commands.Provinces.UpdateProvince;

public class UpdateProvinceHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateProvinceCommand, UpdateProvinceResult>
{
    public async Task<UpdateProvinceResult> Handle(UpdateProvinceCommand command, CancellationToken cancellationToken)
    {
        var province = await dbContext.Provinces.FirstOrDefaultAsync(x => x.Id.Value == command.Id);

        if (province is null)
        {
            throw new RealEstateNotFoundException(string.Format(ErrorMessages.NotFound, "Province"));
        }

        if (await ProvinceExists(command.Id, command.Name, cancellationToken))
        {
            throw new BadRequestException(string.Format(ErrorMessages.NameOrDescriptionExists, "Province"));
        }

        province.Update(command.Name, command.LastModifiedBy);
        dbContext.Provinces.Update(province);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProvinceResult(true);
    }

    private async Task<bool> ProvinceExists(Guid id, string name, CancellationToken cancellationToken)
    {
        return await dbContext.Provinces
                .AsNoTracking()
                .Where(x => x.Name == name && x.Id.Value != id)
                .AnyAsync(cancellationToken);
    }
}
