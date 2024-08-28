namespace Cubico.Identity.Users.Tenants.GetAll;

public record GetAllTenantsResponse(IEnumerable<UserTenantDto> UserTenantsDto);

public class GetAllTenantsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/user/get-all-tenants", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllTenantsQuery());

            var response = result.Adapt<GetAllTenantsResponse>();

            return Results.Ok(response);
        })
        .WithName("GetAllTenants")
        .Produces<GetAllTenantsResponse>(StatusCodes.Status200OK)
        .WithDescription("Get All Tenants");
    }
}
