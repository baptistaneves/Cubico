namespace Cubico.Identity.Users.Roles.Remove;

public record RemoveRoleResult(bool IsSuccess);

public record RemoveRoleCommand(Guid Id) : ICommand<RemoveRoleResult>;

public class RemoveRoleCommandValidation : AbstractValidator<RemoveRoleCommand>
{
    public RemoveRoleCommandValidation()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty).WithMessage(RoleErrorsMessage.IdIsNotValid);
    }
}

public class RemoveRoleHandler(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager) : ICommandHandler<RemoveRoleCommand, RemoveRoleResult>
{
    public async Task<RemoveRoleResult> Handle(RemoveRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await GetRoleById(command.Id);

        EnsureRoleDoesNotHasUsers(role.Name);

        var result = await roleManager.DeleteAsync(role);
        result.ValidateOperation();

        return new RemoveRoleResult(true);
    }

    private async Task<ApplicationRole> GetRoleById(Guid id)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());

        if (role is null)
            throw new NotFoundException(RoleErrorsMessage.RoleNotFound);

        return role;
    }

    private void EnsureRoleDoesNotHasUsers(string name)
    {
        if (userManager.GetUsersInRoleAsync(name).Result.Any())
            throw new BadRequestException(RoleErrorsMessage.RoleCanNotBeRemoved);
    }
}