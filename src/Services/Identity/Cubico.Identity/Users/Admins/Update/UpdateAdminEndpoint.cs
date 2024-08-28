namespace Cubico.Identity.Users.Admins.Update;

public record UpdateAdminResponse(bool IsSuccess);

public record UpdateAdminRequest(Guid Id, string Name, string Email, string Role);

public class UpdateAdminEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user/update-admin", async (UpdateAdminRequest updateAdminRequest, ISender sender) =>
        {
            var command = updateAdminRequest.Adapt<UpdateAdminCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<UpdateAdminResponse>();

            return Results.Ok(response);
        })
        .WithName("UpdateAdmin")
        .Produces<UpdateAdminResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Update Admin");
    }
}