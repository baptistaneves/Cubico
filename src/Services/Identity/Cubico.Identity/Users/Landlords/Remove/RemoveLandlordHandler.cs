namespace Cubico.Identity.Users.Landlords.Remove;

public record RemoveLandlordResult(bool IsSuccess);

public record RemoveLandlordCommand(Guid Id) : ICommand<RemoveLandlordResult>;

public class RemoveLandlordCommandValidation : AbstractValidator<RemoveLandlordCommand>
{
    public RemoveLandlordCommandValidation()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty).WithMessage(LandlordErrorMessages.IdIsNotValid);
    }
}

public class RemoveLandlordHandler(UserManager<ApplicationUser> userManager) : ICommandHandler<RemoveLandlordCommand, RemoveLandlordResult>
{
    public async Task<RemoveLandlordResult> Handle(RemoveLandlordCommand command, CancellationToken cancellationToken)
    {
        var user = await GetUserById(command.Id);

        var result = await userManager.DeleteAsync(user);
        result.ValidateOperation();

        return new RemoveLandlordResult(true);
    }

    private async Task<ApplicationUser> GetUserById(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            throw new NotFoundException(LandlordErrorMessages.UserNotFound);
        }

        return user;
    }
}