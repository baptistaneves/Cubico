namespace RealEstate.Application.Dtos;

public abstract class EntityDto
{
    public Guid Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}
