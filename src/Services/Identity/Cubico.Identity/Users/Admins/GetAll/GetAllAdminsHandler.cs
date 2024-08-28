namespace Cubico.Identity.Users.Admins.GetAll;

public record GetAllAdminsResult(IEnumerable<UserAdminDto> UserAdminsDto);

public record GetAllAdminsQuery : IQuery<GetAllAdminsResult>;

public class GetAllAdminsHandler(UserManager<ApplicationUser> userManager) : IQueryHandler<GetAllAdminsQuery, GetAllAdminsResult>
{
    public async Task<GetAllAdminsResult> Handle(GetAllAdminsQuery request, CancellationToken cancellationToken)
    {
        var users = await userManager
                            .Users
                            .AsNoTracking()
                            .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                            .Where(x => 
                            x.UserRoles.Select(x => x.Role.Name).FirstOrDefault() != "Tenant"
                            && 
                            x.UserRoles.Select(x => x.Role.Name).FirstOrDefault() != "Landlord")
                            .Select(user => new UserAdminDto
                            {
                                Id = user.Id,
                                Name = user.Name,
                                Email = user.Email,
                                Role = user.UserRoles.Select(x => x.Role.Name).FirstOrDefault()

                            }).ToListAsync(cancellationToken);

        return new GetAllAdminsResult(users);
    }
}