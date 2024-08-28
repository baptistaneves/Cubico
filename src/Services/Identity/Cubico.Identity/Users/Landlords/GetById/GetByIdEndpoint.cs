namespace Cubico.Identity.Users.Landlords.GetById;

public record GetLandlordByIdResponse(UserLandlordDto UserDto);

public class GetByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/user/get-landlord-by-id/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetLandlordByIdQuery(id));

            var response = result.Adapt<GetLandlordByIdResponse>();

            return Results.Ok(response);

        })
         .WithName("GetLandlordById")
         .Produces<GetLandlordByIdResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithDescription("Get Lanllord By Id");
    }
}