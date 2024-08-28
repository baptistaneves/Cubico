namespace Cubico.Identity.Users.Tenants.GetAll;

public record GetAllTenantsResult(IEnumerable<UserTenantDto> UserTenantsDto);

public record GetAllTenantsQuery : IQuery<GetAllTenantsResult>;

public class GetAllTenantsHandler(UserManager<ApplicationUser> userManager) : IQueryHandler<GetAllTenantsQuery, GetAllTenantsResult>
{
    public async Task<GetAllTenantsResult> Handle(GetAllTenantsQuery request, CancellationToken cancellationToken)
    {
        var users = await userManager
                            .Users
                            .AsNoTracking()
                            .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                            .Where(x => x.UserRoles.Select(x => x.Role.Name).FirstOrDefault() == "Tenant")
                            .Select(user => new UserTenantDto
                            {
                                Id = user.Id,
                                Name = user.Name,
                                Email = user.Email,
                                Role = user.UserRoles.Select(x => x.Role.Name).FirstOrDefault()

                            }).ToListAsync(cancellationToken);

        return new GetAllTenantsResult(users);
    }
}