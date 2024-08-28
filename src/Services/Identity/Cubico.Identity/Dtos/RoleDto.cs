namespace Cubico.Identity.Dtos;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<ClaimDto> Claims { get; set; }
}