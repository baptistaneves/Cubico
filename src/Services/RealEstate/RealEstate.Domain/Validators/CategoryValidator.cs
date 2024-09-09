namespace RealEstate.Domain.Validators;

internal class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name must have at least 3 characters.");

        RuleFor(m => m.Id.Value)
            .NotEqual(Guid.Empty).WithMessage("ID is not valid");
    }

    public static void ValidateCategory(Category category)
    {
        var validator = new CategoryValidator();

        var validationResult = validator.Validate(category);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }
}
