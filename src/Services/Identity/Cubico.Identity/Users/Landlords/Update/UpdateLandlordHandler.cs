namespace Cubico.Identity.Users.Landlords.Update;

public record UpdateLandlordResult(bool IsSuccess);

public record UpdateLandlordCommand(Guid Id, string Name, string PhoneNumber, string Address)
    : ICommand<UpdateLandlordResult>;

public class UpdateLandlordCommandValidation : AbstractValidator<UpdateLandlordCommand>
{
    public UpdateLandlordCommandValidation()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty).WithMessage(LandlordErrorMessages.IdIsNotValid);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(LandlordErrorMessages.NameIsRequired)
            .MinimumLength(10).WithMessage(LandlordErrorMessages.NameMinimumLength);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(LandlordErrorMessages.PhoneIsRequired);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage(LandlordErrorMessages.AddressIsRequired)
            .MinimumLength(10).WithMessage(LandlordErrorMessages.AddressMinimumLength);
    }
}

public class UpdateLandlordHandler(UserManager<ApplicationUser> userManager)
    : ICommandHandler<UpdateLandlordCommand, UpdateLandlordResult>
{
    public async Task<UpdateLandlordResult> Handle(UpdateLandlordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.Id.ToString());
        if (user is null)
            throw new NotFoundException(LandlordErrorMessages.UserNotFound);

        user.Name = command.Name;
        user.Address = command.Address;
        user.PhoneNumber = command.PhoneNumber;

        var updateUserResult = await userManager.UpdateAsync(user);
        if (!updateUserResult.Succeeded)
        {
            foreach(var error in updateUserResult.Errors)
            {
                throw new BadHttpRequestException(error.Description);
            }
        }

        return new UpdateLandlordResult(true);
    }
}