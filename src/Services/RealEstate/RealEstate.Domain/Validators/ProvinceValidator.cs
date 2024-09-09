namespace RealEstate.Domain.Validators;

public class ProvinceValidator : AbstractValidator<Province>
{
    public ProvinceValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name must have at least 3 characters.");
    }

    public static void ValidateProvince(Province province)
    {
        var validator = new ProvinceValidator();

        var validationResult = validator.Validate(province);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }
}
