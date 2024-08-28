namespace Cubico.Identity.Users.Admins.Remove;

public record RemoveAdminResult(bool IsSuccess);

public record RemoveAdminCommand(Guid Id) : ICommand<RemoveAdminResult>;

public class RemoveAdminHandler(UserManager<ApplicationUser> userManager) : ICommandHandler<RemoveAdminCommand, RemoveAdminResult>
{
    public async Task<RemoveAdminResult> Handle(RemoveAdminCommand command, CancellationToken cancellationToken)
    {
        var user = await GetUserById(command.Id);

        var result = await userManager.DeleteAsync(user);
        result.ValidateOperation();

        return new RemoveAdminResult(true);
    }

    private async Task<ApplicationUser> GetUserById(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user is null)
            throw new NotFoundException(AdminErrorsMessage.UserNotFound);

        return user;
    }

}