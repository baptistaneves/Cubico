namespace RealEstate.Application.Commands.Municipalities.DeleteMunicipality;

public class DeleteMunicipalityHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteMunicipalityCommand, DeleteMunicipalityResult>
{
    public async Task<DeleteMunicipalityResult> Handle(DeleteMunicipalityCommand command, CancellationToken cancellationToken)
    {
        var municipality = await dbContext.Municipalities
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id.Value == command.Id);

        if (municipality is null)
            throw new RealEstateNotFoundException(string.Format(ErrorMessages.NotFound, "Province"));

        dbContext.Municipalities.Remove(municipality);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteMunicipalityResult(true);
    }
}
