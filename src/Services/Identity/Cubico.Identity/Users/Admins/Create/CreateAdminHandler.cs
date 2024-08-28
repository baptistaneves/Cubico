namespace Cubico.Identity.Users.Admins.Create;

public record CreateAdminResult(Guid Id, string Token, string Email, string Name);

public record CreateAdminCommand(string Email, string Name, string Role, string Password)
    : ICommand<CreateAdminResult>;

public class CreateAdminCommandValidation : AbstractValidator<CreateAdminCommand>
{
    public CreateAdminCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(AdminErrorsMessage.NameIsRequired);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(AdminErrorsMessage.EmailIsRequired)
            .EmailAddress().WithMessage(AdminErrorsMessage.EmailIsNotValid);

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage(AdminErrorsMessage.RoleIsRequired);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(AdminErrorsMessage.PasswordIsRequired);
    }
}

public class CreateAdminHandler
    (UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IJwtService jwtService) : ICommandHandler<CreateAdminCommand, CreateAdminResult>
{
    public async Task<CreateAdminResult> Handle(CreateAdminCommand command, CancellationToken cancellationToken)
    {
        var newUser = new ApplicationUser
        {
            UserName = command.Email,
            Email = command.Email,
            Name = command.Name
        };

        var createUserResult = await userManager.CreateAsync(newUser, command.Password);
        createUserResult.ValidateOperation();

        var addUserToRoleResult = await userManager.AddToRoleAsync(newUser, command.Role);
        addUserToRoleResult.ValidateOperation();

        return new CreateAdminResult(newUser.Id, await GetJwtString(newUser), newUser.Email, newUser.Name);
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