namespace Cubico.Identity.Models;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public ApplicationRole Role { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
}