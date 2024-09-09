namespace RealEstate.Application.Dtos;

public class MunicipalityDto : EntityDto
{
    public string Name { get; set; }
    public string ProvinceName { get; set; }
    public Guid ProvinceId { get; set; }
}
