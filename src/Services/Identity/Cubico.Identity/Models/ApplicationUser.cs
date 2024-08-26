namespace Cubico.Identity.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public IEnumerable<ApplicationUserRole> UserRoles { get; set; } = default!;

}