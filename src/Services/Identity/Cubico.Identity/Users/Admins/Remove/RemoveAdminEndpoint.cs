namespace Cubico.Identity.Users.Admins.Remove;

public record RemoveAdminResponse(bool IsSuccess);

public class RemoveAdminEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/user/remove-admin/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new RemoveAdminCommand(id));

            var response = result.Adapt<RemoveAdminResponse>();

            return Results.Ok(response);
        })
         .WithName("RemoveAdmin")
         .Produces<RemoveAdminResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithDescription("Remove Admin");
    }
}
