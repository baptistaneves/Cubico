namespace Cubico.Identity.Users.Tenants.Remove;

public record RemoveTenantResult(bool IsSuccess);

public record RemoveTenantCommand(Guid Id) : ICommand<RemoveTenantResult>;

public class RemoveTenantCommandValidation : AbstractValidator<RemoveTenantCommand>
{
    public RemoveTenantCommandValidation()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty).WithMessage(TenantErrorMessages.IdIsNotValid);
    }
}

public class RemoveTenantHandler(UserManager<ApplicationUser> userManager) : ICommandHandler<RemoveTenantCommand, RemoveTenantResult>
{
    public async Task<RemoveTenantResult> Handle(RemoveTenantCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.Id.ToString());
        if (user is null)
            throw new NotFoundException(TenantErrorMessages.UserNotFound);

        var removeUserResult = await userManager.DeleteAsync(user);
        if (!removeUserResult.Succeeded)
        {
            foreach (var error in removeUserResult.Errors)
            {
                throw new BadRequestException(error.Description);
            }
        }

        return new RemoveTenantResult(true);
    }
}