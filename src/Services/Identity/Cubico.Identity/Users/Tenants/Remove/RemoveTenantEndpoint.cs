namespace Cubico.Identity.Users.Tenants.Remove;

public record RemoveTenantResponse(bool IsSuccess);

public class RemoveTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/user/remove-tenant/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new RemoveTenantCommand(id));

            var response = result.Adapt<RemoveTenantResponse>();

            return Results.Ok(response);
        })
         .WithName("RemoveTenant")
         .Produces<RemoveTenantResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithDescription("Remove Tenant");
    }
}