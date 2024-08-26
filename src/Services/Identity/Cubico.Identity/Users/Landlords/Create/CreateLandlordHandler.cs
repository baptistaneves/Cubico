namespace Cubico.Identity.Users.Landlords.Create;

public record CreateLandlordResult(Guid Id, string Token, string Email, string Name);

public record CreateLandlordCommand(string Email, string Name, string PhoneNumber, string Address, string Password)
    : ICommand<CreateLandlordResult>;

public class LandlordCommandValidation : AbstractValidator<CreateLandlordCommand>
{
    public LandlordCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(LandlordErrorMessages.NameIsRequired)
            .MinimumLength(5).WithMessage(LandlordErrorMessages.NameMinimumLength);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(LandlordErrorMessages.PhoneIsRequired);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage(LandlordErrorMessages.AddressIsRequired)
            .MinimumLength(10).WithMessage(LandlordErrorMessages.AddressMinimumLength);
    }
}

public class CreateLandlordHandler
    (UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IJwtService jwtService)
    : ICommandHandler<CreateLandlordCommand, CreateLandlordResult>
{
    public async Task<CreateLandlordResult> Handle(CreateLandlordCommand command, CancellationToken cancellationToken)
    {
        var newUser = new ApplicationUser
        {
            UserName = command.Email,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            Name = command.Name,
            Address = command.Address
        };

        var createUserResult = await userManager.CreateAsync(newUser, command.Password);
        ValidateOperation(createUserResult);

        var addUserToRoleResult = await userManager.AddToRoleAsync(newUser, "Landlord");
        ValidateOperation(addUserToRoleResult);

        return new CreateLandlordResult(newUser.Id, await GetJwtString(newUser), newUser.Email, newUser.Name);
    }

    private void ValidateOperation(IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
            {
                throw new BadRequestException(error.Description);
            }
        }
    }

    private async Task<string> GetJwtString(ApplicationUser user)
    {
        var claimsIdentity = new ClaimsIdentity(await GetClaims(user));

        var token = jwtService.CreateSecurityToken(claimsIdentity);

        return jwtService.WriteToken(token);
    }

    private async Task<List<Claim>> GetClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("IdentityId", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var roleNames = await userManager.GetRolesAsync(user);
        foreach (var roleName in roleNames)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));

            var role = await roleManager.FindByNameAsync(roleName);
            if (role is not null)
            {
                var roleClaims = await roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }
        }

        return claims;
    }

}