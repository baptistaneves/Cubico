namespace Cubico.Identity.Users.Login;
public record LoginRequest(string Email, string Password);

public record LoginResponse(Guid Id, string Token, string Email, string Name);

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (LoginRequest loginRequest, ISender sender) =>
        {
            var command = loginRequest.Adapt<LoginCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<LoginResponse>();

            return Results.Ok(response);
        })
         .WithName("Login")
         .Produces<LoginResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithDescription("Login");
    }
}