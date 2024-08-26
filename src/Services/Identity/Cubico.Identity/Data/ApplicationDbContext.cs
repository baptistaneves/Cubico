namespace Cubico.Identity.Data;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, Guid, 
    IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
