namespace RealEstate.Domain.Validators;

internal class MunicipalityValidator : AbstractValidator<Municipality>
{
    public MunicipalityValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name must have at least 3 characters.");

        RuleFor(m => m.ProvinceId)
            .NotEqual(Guid.Empty).WithMessage("Province ID is not valid");

        RuleFor(m => m.Id.Value)
            .NotEqual(Guid.Empty).WithMessage("ID is not valid");
    }

    public static void ValidateMunicipality(Municipality municipality)
    {
        var validator = new MunicipalityValidator();

        var validationResult = validator.Validate(municipality);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }
}