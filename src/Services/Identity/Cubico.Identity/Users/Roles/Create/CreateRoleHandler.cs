namespace Cubico.Identity.Users.Roles.Create;

public record CreateRoleResult(bool IsSuccess);

public record CreateRoleCommand(string Name, List<ClaimDto> Claims) : ICommand<CreateRoleResult>;

public class CreateRoleCommandValidation : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(RoleErrorsMessage.RoleNameIsRequired)
            .MinimumLength(3).WithMessage(RoleErrorsMessage.RoleNameMinimuLength);
    }
}

public class CreateRoleHandler(RoleManager<ApplicationRole> roleManager) : ICommandHandler<CreateRoleCommand, CreateRoleResult>
{
    public async Task<CreateRoleResult> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        await EnsureRoleDoesNotExist(command.Name);

        var newRole = new ApplicationRole { Name = command.Name };

        var result = await roleManager.CreateAsync(newRole);
        result.ValidateOperation();

        await AddClaimsToRole(newRole, command.Claims);

        return new CreateRoleResult(true);
    }

    private async Task EnsureRoleDoesNotExist(string roleName)
    {
        if(await roleManager.Roles.Where(x => x.Name == roleName).AnyAsync()) 
            throw new BadRequestException(RoleErrorsMessage.RoleNameAlreadyExists);
    }

    private async Task AddClaimsToRole(ApplicationRole role, List<ClaimDto> claims)
    {
        if (!claims.Any()) return;

        var tasks = claims.Select(async claim =>
        {
            var result = await roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));
            result.ValidateOperation();
        });

        await Task.WhenAll(tasks);
    }
}