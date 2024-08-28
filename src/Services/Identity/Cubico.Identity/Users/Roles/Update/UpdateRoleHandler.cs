namespace Cubico.Identity.Users.Roles.Update;

public record UpdateRoleResult(bool IsSuccess);

public record UpdateRoleCommand(Guid Id, string Name, List<ClaimDto> Claims) : ICommand<UpdateRoleResult>;

public class UpdateRoleCommandValidation : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty).WithMessage(RoleErrorsMessage.IdIsNotValid);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(RoleErrorsMessage.RoleNameIsRequired)
            .MinimumLength(3).WithMessage(RoleErrorsMessage.RoleNameMinimuLength);
    }
}

public class UpdateRoleHandler(RoleManager<ApplicationRole> roleManager) : ICommandHandler<UpdateRoleCommand, UpdateRoleResult>
{
    public async Task<UpdateRoleResult> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await GetRoleById(command.Id);

        await EnsureRoleDoesNotExist(command.Id, command.Name);

        var currentClaims = await roleManager.GetClaimsAsync(role);
        await RemoveClaimFromRole(role, currentClaims, command.Claims);
        await AddClaimToRole(role, currentClaims, command.Claims);

        role.Name = command.Name;
        var result = await roleManager.UpdateAsync(role);
        result.ValidateOperation();

        return new UpdateRoleResult(true);
    }

    private async Task EnsureRoleDoesNotExist(Guid id, string roleName)
    {
        if (await roleManager.Roles.Where(x => x.Name == roleName && x.Id != id).AnyAsync())
            throw new BadRequestException(RoleErrorsMessage.RoleNameAlreadyExists);
    }

    private async Task<ApplicationRole> GetRoleById(Guid id)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());

        if (role is null)
            throw new NotFoundException(RoleErrorsMessage.RoleNotFound);

        return role;
    }

    private async Task RemoveClaimFromRole(ApplicationRole role, IList<Claim> currentClaims, List<ClaimDto> newClaims)
    {
        if (!newClaims.Any() || !currentClaims.Any()) return;

        var tasks = newClaims
            .Where(claim => currentClaims.Any(x => x.Type == claim.Type && x.Value == claim.Value))
            .Select(async claim =>
            {
                var claimInstance = new Claim(claim.Type, claim.Value);
                var result = await roleManager.RemoveClaimAsync(role, claimInstance);
                result.ValidateOperation();
            });

        await Task.WhenAll(tasks);
    }

    private async Task AddClaimToRole(ApplicationRole role, IList<Claim> currentClaims, List<ClaimDto> newClaims)
    {
        if (!newClaims.Any() || !currentClaims.Any()) return;

        var tasks = newClaims
            .Where(claim => !currentClaims.Any(x => x.Type == claim.Type && x.Value == claim.Value))
            .Select(async claim =>
            {
                var claimInstance = new Claim(claim.Type, claim.Value);
                var result = await roleManager.AddClaimAsync(role, claimInstance);
                result.ValidateOperation();
            });

        await Task.WhenAll(tasks);
    }
}