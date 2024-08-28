namespace Cubico.Identity.Users.Admins.GetAll;

public record GetAllAdminsResponse(IEnumerable<UserAdminDto> UserAdminsDto);

public class GetAllAdminsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/user/get-all-admins", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllAdminsQuery());

            var response = result.Adapt<GetAllAdminsResponse>();

            return Results.Ok(response);
        })
        .WithName("GetAllAdmins")
        .Produces<GetAllAdminsResponse>(StatusCodes.Status200OK)
        .WithDescription("Get All Admins");
    }
}
