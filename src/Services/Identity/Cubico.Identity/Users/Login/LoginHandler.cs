namespace Cubico.Identity.Users.Login;

public record LoginResult(Guid Id, string Token, string Email, string Name);

public record LoginCommand(string Email, string Password) : ICommand<LoginResult>;

public class LoginCommandValidation : AbstractValidator<LoginCommand>
{
    public LoginCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage(LoginErrorMessages.EmailIsRequired);
        RuleFor(x => x.Password).NotEmpty().WithMessage(LoginErrorMessages.PasswordIsRequired);
    }
}

public class LoginHandler
    (SignInManager<ApplicationUser> signInManager, 
     UserManager<ApplicationUser> userManager,
     RoleManager<ApplicationRole> roleManager,
     IJwtService jwtService) 
    : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if(user is null)
        {
            throw new BadRequestException(LoginErrorMessages.EmailOrPasswordIncorrect);
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, command.Password, true);

        if(signInResult.IsLockedOut)
        {
            throw new BadRequestException(LoginErrorMessages.IsLockedOut);
        }

        if(!signInResult.Succeeded) 
        {
            throw new BadRequestException(LoginErrorMessages.EmailOrPasswordIncorrect);
        }

        return new LoginResult(user.Id, await GetJwtString(user), user.Email, user.Name);
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