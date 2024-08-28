namespace Cubico.Identity.Users.Tenants.Create;

public record CreateTenantResponse(Guid Id, string Token, string Email, string Name);

public record CreateTenantRequest(string Name, string Email, string Password);

public class CreateTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user/create-tenant", async (CreateTenantRequest createTenantRequest, ISender sender) =>
        {
            var command = createTenantRequest.Adapt<CreateTenantCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<CreateTenantResponse>();

            return Results.Ok(response);
        })
         .WithName("CreateTenant")
         .Produces<CreateTenantResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithDescription("Create Tenant");
    }
}