namespace RealEstate.Application.Commands.Provinces.CreateProvince;

public class CreateProvinceHandler(IApplicationDbContext context) : ICommandHandler<CreateProvinceCommand, CreateProvinceResult>
{
    public async Task<CreateProvinceResult> Handle(CreateProvinceCommand command, CancellationToken cancellationToken)
    {
        if (await ProvinceExists(command.Name, cancellationToken))
        {
            throw new BadRequestException(ProvinceErrorMessages.ProviceExists);
        }

        var newProvince = Province.Create(ProvinceId.Of(Guid.NewGuid()), command.Name, command.CreatedBy);

        context.Provinces.Add(newProvince);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateProvinceResult(true);
    }

    private async Task<bool> ProvinceExists(string name, CancellationToken cancellationToken)
    {
        return await context.Provinces
                .AsNoTracking()
                .Where(x => x.Name == name)
                .AnyAsync(cancellationToken);
    }
}