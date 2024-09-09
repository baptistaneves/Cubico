namespace RealEstate.Domain.ValueObjects;

public class CategoryId
{
    public Guid Value { get; }

    public CategoryId(Guid value) => Value = value;

    public static CategoryId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty)
        {
            throw new ValidationException("Categpry Id cannot be empty");
        }

        return new CategoryId(value);
    }
}
