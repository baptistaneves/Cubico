namespace Cubico.Identity.Users.Roles.Create;

public record CreateRoleResponse(bool IsSuccess);

public record CreateRoleRequest(string Name, List<ClaimDto> Claims);

public class CreateRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/role/create-role", async (CreateRoleRequest createRoleRequest, ISender sender) =>
        {
            var command = createRoleRequest.Adapt<CreateRoleCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<CreateRoleResponse>();

            return Results.Ok(response);
        })
        .WithName("CreateRole")
        .Produces<CreateRoleResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Create Role");
    }
}
