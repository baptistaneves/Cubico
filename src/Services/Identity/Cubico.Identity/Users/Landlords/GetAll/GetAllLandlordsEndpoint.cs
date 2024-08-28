namespace Cubico.Identity.Users.Landlords.GetAll;

public record GetAllLandLordResponse(IEnumerable<UserLandlordDto> UserDtos);

public class GetAllLandlordsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/user/get-all-landlords", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllLandlordsQuery());

            var response = result.Adapt<GetAllLandLordResponse>();

            return Results.Ok(response);
        })
         .WithName("GetAllLandlords")
         .Produces<GetAllLandLordResponse>(StatusCodes.Status200OK)
         .WithDescription("Get All Lanllords");
    }
}
