namespace Cubico.Identity.Services;

public interface IJwtService
{
    string WriteToken(SecurityToken token);
    SecurityToken CreateSecurityToken(ClaimsIdentity claimsIdentity);
}