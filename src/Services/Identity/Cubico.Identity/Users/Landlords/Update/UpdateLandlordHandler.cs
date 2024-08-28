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
        var user = await GetUserById(command.Id);

        user.Name = command.Name;
        user.Address = command.Address;
        user.PhoneNumber = command.PhoneNumber;

        var result = await userManager.UpdateAsync(user);
        result.ValidateOperation();

        return new UpdateLandlordResult(true);
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