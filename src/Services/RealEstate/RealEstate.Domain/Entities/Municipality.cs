namespace RealEstate.Domain.Entities;

public class Municipality : Aggregate<MunicipalityId>
{
    public string Name { get; private set; }
    public Guid ProvinceId { get; private set; }

    public Province Province { get; private set; }

    public static Municipality Create(MunicipalityId id, string name, Guid provinceId, string createdBy)
    {
        var municipality = new Municipality
        {
            Id = id,
            Name = name,
            ProvinceId = provinceId,
            CreatedBy = createdBy
        };

        MunicipalityValidator.ValidateMunicipality(municipality);

        return municipality;
    }

    public void Update(string name, Guid provinceId, string lastModifiedBy)
    {
        Name = name;
        ProvinceId = provinceId;
        LastModifiedBy = lastModifiedBy;

        MunicipalityValidator.ValidateMunicipality(this);
    }
}