namespace Cubico.Identity.Users.Landlords.GetAll;

public record GetAllLandlordsResult(IEnumerable<UserDto> UserDtos);

public record GetAllLandlordsQuery : IQuery<GetAllLandlordsResult>;

public class GetAllHandler(UserManager<ApplicationUser> userManager) : IQueryHandler<GetAllLandlordsQuery, GetAllLandlordsResult>
{
    public async Task<GetAllLandlordsResult> Handle(GetAllLandlordsQuery request, CancellationToken cancellationToken)
    {
        var users = await userManager
                            .Users
                            .AsNoTracking()
                            .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                            .Select(user => new UserDto
                            {
                                Id = user.Id,
                                Name = user.Name,
                                Email = user.Email,
                                PhoneNumber = user.PhoneNumber,
                                Address = user.Address,
                                Role = user.UserRoles.Select(x => x.Role.Name).FirstOrDefault()

                            }).ToListAsync(cancellationToken);

        return new GetAllLandlordsResult(users);
    }
}
