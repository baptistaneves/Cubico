namespace Cubico.Identity.Users.Tenants.Update;

public record UpdateTenantResult(bool IsSuccess);

public record UpdateTenantCommand(Guid Id, string Name, string Email) : ICommand<UpdateTenantResult>;

public class UpdateTenantCommandValidation : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidation()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty).WithMessage(TenantErrorMessages.IdIsNotValid);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(TenantErrorMessages.NameIsRequired);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(TenantErrorMessages.EmailIsRequired)
            .EmailAddress().WithMessage(TenantErrorMessages.EmailIsNotValid);
    }
}

public class UpdateTenantHandler(UserManager<ApplicationUser> userManager) : ICommandHandler<UpdateTenantCommand, UpdateTenantResult>
{
    public async Task<UpdateTenantResult> Handle(UpdateTenantCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.Id.ToString());
        if (user is null)
            throw new NotFoundException(TenantErrorMessages.UserNotFound);

        user.Name = command.Name;
        user.Email = command.Email;

        var updateUserResult = await userManager.UpdateAsync(user);
        if (!updateUserResult.Succeeded)
        {
            foreach (var error in updateUserResult.Errors)
            {
                throw new BadHttpRequestException(error.Description);
            }
        }

        return new UpdateTenantResult(true);
    }
}