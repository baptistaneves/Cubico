namespace RealEstate.Application.Commands.Municipalities.UpdateMunicipality;

public class UpdateMunicipalityHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateMunicipalityCommand, UpdateMunicipalityResult>
{
    public async Task<UpdateMunicipalityResult> Handle(UpdateMunicipalityCommand command, CancellationToken cancellationToken)
    {
        var municipality = await dbContext.Municipalities.FirstOrDefaultAsync(x => x.Id.Value == command.Id);

        if (municipality is null)
        {
            throw new RealEstateNotFoundException(string.Format(ErrorMessages.NotFound, "Municipality"));
        }

        if (await MunicipalityExists(command.Id, command.Name, cancellationToken))
        {
            throw new BadRequestException(string.Format(ErrorMessages.NameOrDescriptionExists, "Municipality"));
        }

        municipality.Update(command.Name, command.ProvinceId, command.LastModifiedBy);
        dbContext.Municipalities.Update(municipality);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateMunicipalityResult(true);
    }

    private async Task<bool> MunicipalityExists(Guid id, string name, CancellationToken cancellationToken)
    {
        return await dbContext.Municipalities
                .AsNoTracking()
                .Where(x => x.Name == name && x.Id.Value != id)
                .AnyAsync(cancellationToken);
    }
}
