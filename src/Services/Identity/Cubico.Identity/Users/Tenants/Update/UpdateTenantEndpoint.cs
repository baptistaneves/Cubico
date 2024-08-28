namespace Cubico.Identity.Users.Tenants.Update;

public record UpdateTenantResponse(bool IsSuccess);

public record UpdateTenantRequest(Guid Id, string Name, string Email);

public class UpdateTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/user/update-tenant", async (UpdateTenantRequest updateTenantRequest, ISender sender) =>
        {
            var command = updateTenantRequest.Adapt<UpdateTenantCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<UpdateTenantResponse>();

            return Results.Ok(response);
        })
         .WithName("UpdateTenant")
         .Produces<UpdateTenantResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithDescription("Update Tenant");
    }
}