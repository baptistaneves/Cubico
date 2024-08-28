namespace Cubico.Identity.Users.Tenants.Create;

public record CreateTenantResult(Guid Id, string Token, string Email, string Name);

public record CreateTenantCommand(string Name, string Email, string Password) : ICommand<CreateTenantResult>;

public class CreateTenantCommandValidation : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(TenantErrorMessages.NameIsRequired);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(TenantErrorMessages.EmailIsRequired)
            .EmailAddress().WithMessage(TenantErrorMessages.EmailIsNotValid);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(TenantErrorMessages.PasswordIsRequired);
    }
}

public class CreateTenantHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IJwtService jwtService)
    : ICommandHandler<CreateTenantCommand, CreateTenantResult>
{
    public async Task<CreateTenantResult> Handle(CreateTenantCommand command, CancellationToken cancellationToken)
    {
        var newUser = new ApplicationUser
        {
            UserName = command.Email,
            Email = command.Email,
            Name = command.Name,
        };

        var createUserResult = await userManager.CreateAsync(newUser, command.Password);
        ValidateOperation(createUserResult);

        var addUserToRoleResult = await userManager.AddToRoleAsync(newUser, "Tenant");
        ValidateOperation(addUserToRoleResult);

        return new CreateTenantResult(newUser.Id, await GetJwtString(newUser), newUser.Email, newUser.Name);
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