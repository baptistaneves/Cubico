namespace Cubico.Identity.Users.Landlords.GetById;

public record GetLandlordByIdResult(UserDto UserDto);

public record GetLandlordByIdQuery(Guid Id) : IQuery<GetLandlordByIdResult>;

public class GetByIdHandler(UserManager<ApplicationUser> userManager) : IQueryHandler<GetLandlordByIdQuery, GetLandlordByIdResult>
{
    public async Task<GetLandlordByIdResult> Handle(GetLandlordByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await userManager
                            .Users
                            .AsNoTracking()
                            .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                            .Where(x => x.Id == query.Id)
                            .Select(user => new UserDto
                            {
                                Id = user.Id,
                                Name = user.Name,
                                Email = user.Email,
                                PhoneNumber = user.PhoneNumber,
                                Address = user.Address,
                                Role = user.UserRoles.Select(x => x.Role.Name).FirstOrDefault()

                            }).FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new NotFoundException(LandlordErrorMessages.UserNotFound);

        return new GetLandlordByIdResult(user);
    }
}