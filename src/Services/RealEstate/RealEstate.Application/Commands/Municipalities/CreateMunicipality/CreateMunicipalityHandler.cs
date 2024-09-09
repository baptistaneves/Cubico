namespace RealEstate.Application.Commands.Municipalities.CreateMunicipality;

public class CreateMunicipalityHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateMuniciplaityCommand, CreateMunicipalityResult>
{
    public async Task<CreateMunicipalityResult> Handle(CreateMuniciplaityCommand command, CancellationToken cancellationToken)
    {
        if (await MunicipalityExists(command.Name, cancellationToken))
        {
            throw new BadRequestException(string.Format(ErrorMessages.NameOrDescriptionExists, "Municipality"));
        }

        var newMunicipality = Municipality.Create(MunicipalityId.Of(Guid.NewGuid()), command.Name, command.ProvinceId, command.CreatedBy);

        dbContext.Municipalities.Add(newMunicipality);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateMunicipalityResult(true);
    }

    private async Task<bool> MunicipalityExists(string name, CancellationToken cancellationToken)
    {
        return await dbContext.Municipalities
                .AsNoTracking()
                .Where(x => x.Name == name)
                .AnyAsync(cancellationToken);
    }
}