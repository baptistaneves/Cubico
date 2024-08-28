namespace Cubico.Identity.Users.Roles.GetById;

public record GetRoleByIdResponse(RoleDto Role);

public class GetRoleByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/role/get-role-by-id/{id}", async (Guid id, ISender sender) =>
        {
            var query = new GetRoleByIdQuery(id);

            var result = await sender.Send(query);

            var response = result.Adapt<GetRoleByIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetRoleById")
        .Produces<GetRoleByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithDescription("Get Role By Id");
    }
}
