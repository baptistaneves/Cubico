namespace Cubico.Identity.Users.Landlords.Remove;

public record RemoveLandlordResult(bool IsSuccess);

public record RemoveLandlordCommand(Guid id) : ICommand<RemoveLandlordResult>;

public class RemoveLandlordCommandValidation : AbstractValidator<RemoveLandlordCommand>
{
    public RemoveLandlordCommandValidation()
    {
        RuleFor(x => x.id).NotEqual(Guid.Empty).WithMessage(LandlordErrorMessages.IdIsNotValid);
    }
}

public class RemoveLandlordHandler(UserManager<ApplicationUser> userManager) : ICommandHandler<RemoveLandlordCommand, RemoveLandlordResult>
{
    public async Task<RemoveLandlordResult> Handle(RemoveLandlordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.id.ToString());
        if (user is null)
            throw new NotFoundException(LandlordErrorMessages.UserNotFound);

        var removeUserResult = await userManager.DeleteAsync(user);
        if (!removeUserResult.Succeeded)
        {
            foreach (var error in removeUserResult.Errors)
            {
                throw new BadRequestException(error.Description);
            }
        }

        return new RemoveLandlordResult(true);
    }
}