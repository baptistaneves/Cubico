namespace Cubico.Identity.Users.Admins.GetById;

public record GetAdminByIdResponse(UserAdminDto UserAdminDto);

public class GetAdminByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/user/get-admin-by-id/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetAdminByIdQuery(id));

            var response = result.Adapt<GetAdminByIdResponse>();

            return Results.Ok(response);
        })
         .WithName("GetAdminById")
         .Produces<GetAdminByIdResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithDescription("Get Admin By Id");
    }
}