namespace Cubico.Identity.Users.Tenants.GetById;

public record GetTenantByIdResult(UserTenantDto UserTenantDto);

public record GetTenantByIdQuery(Guid Id) : IQuery<GetTenantByIdResult>;

public class GetTenantByIdHandler(UserManager<ApplicationUser> userManager) : IQueryHandler<GetTenantByIdQuery, GetTenantByIdResult>
{
    public async Task<GetTenantByIdResult> Handle(GetTenantByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await userManager
                            .Users
                            .AsNoTracking()
                            .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                            .Where(x => x.Id == query.Id)
                            .Select(user => new UserTenantDto
                            {
                                Id = user.Id,
                                Name = user.Name,
                                Email = user.Email,
                                Role = user.UserRoles.Select(x => x.Role.Name).FirstOrDefault()

                            }).FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new NotFoundException(TenantErrorMessages.UserNotFound);

        return new GetTenantByIdResult(user);
    }
}
