namespace Cubico.Identity.Extensions;

public static class IdentityResultExtension
{
    public static void ValidateOperation(this IdentityResult result)
    {
        if (!result.Succeeded)
        {
            var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"{errorMessages}");
        }
    }
}