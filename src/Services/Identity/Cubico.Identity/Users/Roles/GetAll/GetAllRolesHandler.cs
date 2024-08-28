namespace Cubico.Identity.Users.Roles.GetAll;

public record GetAllRolesResult(IEnumerable<RoleDto> Roles);

public record GetAllRolesQuery : IQuery<GetAllRolesResult>;

public class GetAllRolesHandler(RoleManager<ApplicationRole> roleManager) 
    : IQueryHandler<GetAllRolesQuery, GetAllRolesResult>
{
    public async Task<GetAllRolesResult> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles
            .AsNoTracking()
            .Where(x => x.Name != "Tenant" && x.Name != "Landlord")
            .ToListAsync();

        var roleDtos = new List<RoleDto>();

        foreach (var role in roles)
        {
            var claims = await roleManager.GetClaimsAsync(role);

            var roleDto = new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Claims = claims.Select(claim => new ClaimDto(claim.Type, claim.Value)).ToList()
            };

            roleDtos.Add(roleDto);
        }

        return new GetAllRolesResult(roleDtos);
    }
}