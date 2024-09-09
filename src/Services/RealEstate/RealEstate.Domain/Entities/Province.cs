namespace RealEstate.Domain.Entities;

public class Province : Aggregate<ProvinceId>
{
    public string Name { get; private set; } = default!;

    public static Province Create(ProvinceId id, string name, string createdBy)
    {
        var province = new Province
        {
            Id = id,
            Name = name,
            CreatedBy = createdBy
        };

        ProvinceValidator.ValidateProvince(province);

        return province;
    }

    public void Update(string name, string modifiedBy)
    {
        Name = name;
        LastModifiedBy = modifiedBy;

        ProvinceValidator.ValidateProvince(this);
    }

}
