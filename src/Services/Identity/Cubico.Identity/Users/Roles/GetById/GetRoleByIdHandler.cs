namespace Cubico.Identity.Users.Roles.GetById;

public record GetRoleByIdResult(RoleDto Role);

public record GetRoleByIdQuery(Guid Id) : IQuery<GetRoleByIdResult>;

public class GetRoleByIdHandler(RoleManager<ApplicationRole> roleManager) : IQueryHandler<GetRoleByIdQuery, GetRoleByIdResult>
{
    public async Task<GetRoleByIdResult> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(query.Id.ToString());
        if (role == null)
        {
            throw new NotFoundException(RoleErrorsMessage.RoleNotFound);
        }

        var claims = await roleManager.GetClaimsAsync(role);

        var roleDto = new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Claims = claims.Select(claim => new ClaimDto(claim.Type, claim.Value)).ToList()
        };

        return new GetRoleByIdResult(roleDto);
    }
}