namespace Cubico.Identity.Users.Landlords.Update;

public record UpdateLandlordResponse(bool IsSuccess);

public record UpdateLandlordRequest(Guid Id, string Name, string PhoneNumber, string Address);

public class UpdateLandlordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/user/update-landlord", async ([FromBody] UpdateLandlordRequest updateLandlordRequest, ISender sender) =>
        {
            var command = updateLandlordRequest.Adapt<UpdateLandlordCommand>();

            var result = await sender.Send(command);

            var response =  result.Adapt<UpdateLandlordResponse>();

            return Results.Ok(response);

        })
         .WithName("UpdateLandlord")
         .Produces<UpdateLandlordResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithDescription("Update Lanllord");
    }
}