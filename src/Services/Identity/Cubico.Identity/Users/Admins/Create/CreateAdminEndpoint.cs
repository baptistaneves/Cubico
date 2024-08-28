namespace Cubico.Identity.Users.Admins.Create;

public record CreateAdminResponse(Guid Id, string Token, string Email, string Name);

public record CreateAdminRequest(string Email, string Name, string Role, string Password);

public class CreateAdminEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user/create-admin", async (CreateAdminRequest createAdminRequest, ISender sender) =>
        {
            var command = createAdminRequest.Adapt<CreateAdminCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<CreateAdminResponse>();

            return Results.Ok(response);
        })
        .WithName("CreateAdmin")
        .Produces<CreateAdminResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Create Admin");
    }
}