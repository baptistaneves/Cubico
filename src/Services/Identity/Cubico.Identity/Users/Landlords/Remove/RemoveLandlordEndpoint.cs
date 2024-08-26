namespace Cubico.Identity.Users.Landlords.Remove;

public record RemoveLandlordResponse(bool IsSuccess);

public record RemoveLandlordRequest(Guid Id);

public class RemoveLandlordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/user/remove-landlord/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new RemoveLandlordCommand(id));

            var response = result.Adapt<RemoveLandlordResponse>();

            return Results.Ok(response);
        })
         .WithName("CreateLandlord")
         .Produces<RemoveLandlordResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithDescription("Create Lanllord");
    }
}