namespace Cubico.Identity.Users.Admins.Update;

public record UpdateAdminResult(bool IsSuccess);

public record UpdateAdminCommand(Guid Id, string Name, string Email, string Role) : ICommand<UpdateAdminResult>;

public class UpdateAdminCommandValidation : AbstractValidator<UpdateAdminCommand>
{
    public UpdateAdminCommandValidation()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty).WithMessage(AdminErrorsMessage.IdIsNotValid);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(AdminErrorsMessage.NameIsRequired);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(AdminErrorsMessage.EmailIsRequired)
            .EmailAddress().WithMessage(AdminErrorsMessage.EmailIsNotValid);
    }
}

public class UpdateAdminHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) : ICommandHandler<UpdateAdminCommand, UpdateAdminResult>
{
    public async Task<UpdateAdminResult> Handle(UpdateAdminCommand command, CancellationToken cancellationToken)
    {
        var user = await GetUserById(command.Id);

        user.Name = command.Name;
        user.Email = command.Email;

        var result = await userManager.UpdateAsync(user);
        result.ValidateOperation();

        if (!String.IsNullOrWhiteSpace(command.Role))
        {
            var currentRole = userManager.GetRolesAsync(user).Result.FirstOrDefault();

            await RemoveUserFromRole(user, currentRole);
            await AddUserToRole(user, command.Role);
        }
        
        return new UpdateAdminResult(true);
    }

    private async Task<ApplicationUser> GetUserById(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user is null)
            throw new NotFoundException(AdminErrorsMessage.UserNotFound);

        return user;
    }

    private async Task RemoveUserFromRole(ApplicationUser user, string role)
    {
        var result = await userManager.RemoveFromRoleAsync(user, role);
        result.ValidateOperation();
    }

    private async Task AddUserToRole(ApplicationUser user, string role)
    {
        var result = await userManager.AddToRoleAsync(user, role);
        result.ValidateOperation();
    }
}