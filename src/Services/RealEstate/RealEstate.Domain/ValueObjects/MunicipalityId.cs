namespace RealEstate.Domain.ValueObjects;

public record MunicipalityId
{
    public Guid Value { get;}
    public MunicipalityId(Guid value) => Value = value;

    public static MunicipalityId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty)
        {
            throw new ValidationException("Municipality Id cannot be empty");
        }

        return new MunicipalityId(value);
    }
}