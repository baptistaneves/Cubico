namespace Cubico.Identity.Users.Admins.GetById;

public record GetAdminByIdResult(UserAdminDto UserAdminDto);

public record GetAdminByIdQuery(Guid Id) : IQuery<GetAdminByIdResult>;

public class GetAdminByIdHandler(UserManager<ApplicationUser> userManager) : IQueryHandler<GetAdminByIdQuery, GetAdminByIdResult>
{
    public async Task<GetAdminByIdResult> Handle(GetAdminByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await userManager
                            .Users
                            .AsNoTracking()
                            .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                            .Where(x => x.Id == query.Id)
                            .Select(user => new UserAdminDto
                            {
                                Id = user.Id,
                                Name = user.Name,
                                Email = user.Email,
                                Role = user.UserRoles.Select(x => x.Role.Name).FirstOrDefault()

                            }).FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new NotFoundException(AdminErrorsMessage.UserNotFound);

        return new GetAdminByIdResult(user);
    }
}