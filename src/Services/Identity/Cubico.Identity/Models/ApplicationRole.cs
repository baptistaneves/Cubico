namespace Cubico.Identity.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public IEnumerable<ApplicationUserRole> UserRoles { get; set; } = default!;
}
